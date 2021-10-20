using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndigoLabsAssignment.Models
{
    public class DayData
    {
        public DateTime Date { get; set; }
        public List<Region> Regions { get; set; }


        public DayData(DateTime day)
        {
            Date = day;
            Regions = new List<Region>();

        }
    }
}
