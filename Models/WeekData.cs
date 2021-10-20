using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndigoLabsAssignment.Models
{
    public class WeekData : IComparable<WeekData>
    {
        public string Region { get; private set; }

        public int Cases { get; private set; }
        public WeekData(string region)
        {
            Region = region;
            Cases = 0;
        }

        public void AddCases(int cases)
        {
            // ?? just sum ??
            Cases += cases;
        }

        public int CompareTo(WeekData other)
        {
            return this.Cases.CompareTo(other.Cases);
        }
    }
}
