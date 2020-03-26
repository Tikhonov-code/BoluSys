using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoluSys.Models
{
    public class Intakes_Data
    {
        public List<Intakes> li { get; set; }
        public double? TotalIntakes { get; set; }
        public double? Sum()
        {
            TotalIntakes =li.Sum(x => x.val);
            return TotalIntakes;
        }
    }
    public class Intakes
    {
        public DateTime? arg { get; set; }
        //public string arg { get; set; }
        public double? val { get; set; }

    }
    public class IntakesStr
    {
        public string arg { get; set; }
        public double? val { get; set; }

    }
}