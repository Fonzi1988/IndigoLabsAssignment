using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using IndigoLabsAssignment.Models;
using IndigoLabsAssignment.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace IndigoLabsAssignment.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/region")]
    public class CoronaCasesController : ControllerBase
    {
        /// <summary>
        /// Slovene regions
        /// </summary>
        private static readonly string[] AvailableRegions = new[]
        {
            "LJ", "CE", "KR", "NM", "KK", "KP", "MB", "MS", "NG", "PO", "SG", "ZA"
        };

        private readonly ILogger<CoronaCasesController> _logger;
        public CoronaCasesController(ILogger<CoronaCasesController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="region">Slovene region to get data for. Possible values (LJ, CE, KR, NM, KK, KP, MB, MS, NG, PO, SG, ZA)</param>
        /// <param name="from">From what date to get data for in format yyyy-MM--dd</param>
        /// <param name="to">To what date to get data for in format yyyy-MM--dd</param>
        /// <returns>List of daily corona cases (active, vaccinated 1st, vaccinted 2nd, deceased) for Slovene regions </returns>
        [HttpGet("cases")]
        public async Task<IEnumerable<DayData>> GetCases(string region, string from, string to)
        {
            //download data from web
            string data = await  GetDataFromWeb("https://raw.githubusercontent.com/sledilnik/data/master/csv/region-cases.csv");

            //check if from and to parameters are valid dates
            DateTime? fromDate = Helpers.Helpers.ValidateDate(from);
            DateTime? toDate = Helpers.Helpers.ValidateDate(to);

            //check if parameter region matches Slovene regions
            if(region != null)
            {
                if (!AvailableRegions.Contains(region.ToUpper())) region = null;
                else region = region.ToLower();
            }

            var result = Helpers.Helpers.ParseFromStringCsv(data, region, fromDate, toDate);
            return result;
        }

        /// <summary>
        /// Get last weeks active corona cases in Slovenia
        /// </summary>
        /// <returns>Returns the total of active corona cases in the last week for all Slovene regions</returns>
        [HttpGet("lastweek")]
        public async Task<IEnumerable<WeekData>> GetLastWeek()
        {
            //download data from web
            string data = await GetDataFromWeb("https://raw.githubusercontent.com/sledilnik/data/master/csv/region-cases.csv");

            // get lastweeks data -- could probably speed it up with from parameter
            var result = Helpers.Helpers.GetLastWeekActiveCases(Helpers.Helpers.ParseFromStringCsv(data));

            return result;
        }

        /// <summary>
        /// Get data from the internet
        /// </summary>
        /// <param name="url">Url to get data from</param>
        /// <returns>String of data from the referenced url</returns>
        private async Task<string> GetDataFromWeb(string url)
        {

            string data = "";

            using (WebClient client = new WebClient())
            {
                data = await client.DownloadStringTaskAsync(url);
            }

            return data;
        }

    }
}
