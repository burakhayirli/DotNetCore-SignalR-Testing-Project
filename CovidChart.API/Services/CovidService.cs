using CovidChart.API.Hubs;
using CovidChart.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceviceCovidList", GetCovidChartList());
        }

        public List<CovidChartModel> GetCovidChartList()
        {
            List<CovidChartModel> covidChartModels = new List<CovidChartModel>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"SELECT CovidDate,[1],[2],[3],[4],[5]
                        FROM (SELECT[City],[Count], CAST([CovidDate] as DATE) as CovidDate  FROM Covids) as covidT
                        PIVOT (
                        SUM ([Count]) For City IN([1],[2],[3],[4],[5])
                        )
                        AS PVT
                        ORDER BY CovidDate ASC";

                command.CommandType = CommandType.Text;
                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChartModel c = new CovidChartModel();
                        c.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (DBNull.Value.Equals(reader[x]))
                            {
                                c.Counts.Add(0);
                            }
                            else
                            {
                                c.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidChartModels.Add(c);
                    }
                }

                _context.Database.CloseConnection();

                return covidChartModels;
            }
        }

    }
}
