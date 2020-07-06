using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoluSys.Models
{
    public class FarmRules
    {
        public string userid { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }

        public System.Data.Entity.Spatial.DbGeography geoPosition;

        private System.Data.Entity.Spatial.DbGeography GetGeoPosition()
        {
            return geoPosition;
        }

        public void SetGeoPosition(System.Data.Entity.Spatial.DbGeography value)
        {
            geoPosition = value;
        }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Alert_Name { get; set; }
        public bool sms_s { get; set; }
        public bool email_s { get; set; }
    }
}