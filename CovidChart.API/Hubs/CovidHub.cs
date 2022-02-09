using CovidChart.API.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Hubs
{
    public class CovidHub : Hub
    {
        private readonly CovidService _covidService;
        public CovidHub(CovidService covidService)
        {
            _covidService = covidService;
        }

        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceviceCovidList",_covidService.GetCovidChartList());
        }
    }
}
