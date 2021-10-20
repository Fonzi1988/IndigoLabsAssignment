using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndigoLabsAssignment.Models
{
    public class Region
 
    {
        public string RegionName { get; set; }
        public int Active { get; set; }
        public int VaccinatedFirst { get; set; }
        public int VaccinatedSecond { get; set; }
        public int Deceased { get; set; }

    }
}
