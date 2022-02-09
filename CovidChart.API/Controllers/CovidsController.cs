using CovidChart.API.Models;
using CovidChart.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CovidChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;
        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);
            IQueryable<Covid> covidList = _covidService.GetList();
            return Ok(covidList);
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random rnd = new Random();

            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid
                    {
                        City = item,
                        Count = rnd.Next(100, 1000),
                        CovidDate = DateTime.Now.AddDays(x),
                    };

                    _covidService.SaveCovid(newCovid).Wait();
                    Thread.Sleep(1000);
                }
            });

            return Ok("Covid19 Verileri Veritabanına Kaydedildi.");
        }
    }
}
