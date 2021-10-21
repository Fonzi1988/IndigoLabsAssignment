using IndigoLabsAssignment.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IndigoLabsAssignment.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// Parses data from a string (from a .csv file) to a list of DayData
        /// </summary>
        /// <param name="data">String data from .csv</param>
        /// <param name="reg">Name of region to get data for. Returns all regions if null.</param>
        /// <param name="from">From what date to get data for. Returns from first day of data if null.</param>
        /// <param name="to">To what date to get data for. Returns to last day of data if null.</param>
        /// <returns>A list of DayData</returns>
        public static List<DayData> ParseFromStringCsv(string data, string reg = null, DateTime? from = null, DateTime? to = null)
        {
            List<DayData> result = new List<DayData>();
            using (StringReader sr = new StringReader(data))
            {
                // get header data
                string[] headerLine = sr.ReadLine().Split(",");
                string line;

                //reads all lines
                while ((line = sr.ReadLine()) != null)
                {
                    var lineData = line.Split(",");
                    var day = DateTime.Parse(lineData[0]);

                    //skip data if from date is bigger than date of current line
                    if (from != null && from > day)
                    {
                        continue;
                    }

                    //skip all data if to date is smaller than date of current line
                    if (to != null && to < day)
                    {
                        // use continue if data isn't sorted by date
                        break;
                    }

                    var tempDayData = new DayData(day);
                    Region region = null;
                    string previousColumnHeaderRegion = "";
                    for (int i = 1; i < lineData.Length; i++)
                    {
                        var columnHeader = headerLine[i].Split(".");

                        //skip data if curent column region name does not match input parameter reg
                        if (reg != null && reg != columnHeader[1]) continue;

                        //Add region data to cuurent date and start new region
                        if (columnHeader[1] != previousColumnHeaderRegion)
                        {
                            if (region != null) tempDayData.Regions.Add(region);
                            region = new Region();
                            region.RegionName = columnHeader[1];
                        }

                        //change empty string to 0 so that parse doesnt fail
                        if (lineData[i] == "") lineData[i] = "0";

                        //fill region data based on header value
                        switch (columnHeader[2])
                        {
                            case "cases":
                                if (columnHeader[3] == "active") region.Active = Int32.Parse(lineData[i]);
                                break;
                            case "deceased":
                                region.Deceased = Int32.Parse(lineData[i]);
                                break;
                            case "vaccinated":
                                if (columnHeader[3] == "1st") region.VaccinatedFirst = Int32.Parse(lineData[i]);
                                if (columnHeader[3] == "2nd") region.VaccinatedSecond = Int32.Parse(lineData[i]);
                                break;
                        }

                        previousColumnHeaderRegion = columnHeader[1];
                    }

                    //add last region data
                    tempDayData.Regions.Add(region);

                    //add day data to list
                    result.Add(tempDayData);
                }
            }
            return result;
        }

        /// <summary>
        /// Get last week active COVID-19 cases
        /// </summary>
        /// <param name="allDays">List of DayData with COVID-19 cases</param>
        /// <returns>A list of WeekData for all Slovene regions  </returns>
        public static List<WeekData> GetLastWeekActiveCases(List<DayData> allDays)
        {
            //get last 7 days
            var lastWeek = allDays.GetRange(allDays.Count - 7, 7);
            List<WeekData> weekData = new List<WeekData>();

            foreach (var daydata in lastWeek)
            {
                foreach (var region in daydata.Regions)
                {
                    // add regions to week data and sum active cases
                    WeekData wd;
                    var tempRegion = weekData.Where(wd => wd.Region == region.RegionName).ToList();

                    //add region
                    if (tempRegion.Count() > 0)
                    {
                        wd = tempRegion[0];
                        wd.AddCases(region.Active);
                    }
                    //update region
                    else
                    {
                        wd = new WeekData(region.RegionName);
                        wd.AddCases(region.Active);
                        weekData.Add(wd);
                    }
                }
            }

            weekData.Sort();
            weekData.Reverse();
            return weekData;
        }
        /// <summary>
        /// Checks if string is a valid date.
        /// </summary>
        /// <param name="date">String to check if it a valid date</param>
        /// <returns>Returns  DateTime if input param is valid - othervise returns null.</returns>
        public static DateTime? ValidateDate(string date)
        {
            if (date == null || date == "") return null;
            else if (DateTime.TryParse(date, out var _date)) return _date;
            else return null;
        }
    }
}
