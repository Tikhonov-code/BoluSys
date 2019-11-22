using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoluSys.Models
{
    using System;
    using System.Collections.Generic;

    public partial class BolusList
    {
        public int bolus_id { get; set; }
        public int? animal_id { get; set; }
    }
}