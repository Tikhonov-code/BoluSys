using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoluSys.Models
{
    public class BolusIDChart
    {
        public string t { get; set; }
        public double Temperature { get; set; }
    }
    public class BolusAnimalID
    {
        public string bolus_id { get; set; }
        public string animal_id { get; set; }
    }
}