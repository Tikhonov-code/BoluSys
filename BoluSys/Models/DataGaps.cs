using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoluSys.Models
{
    public class Data_Gaps
    {
        public int bolus_id { get; set; }
        public string animal_id { get; set; }
        public DateTime? dt_from { get; set; }
        public DateTime? dt_to { get; set; }     
        public string interval { get; set; }
    }
}