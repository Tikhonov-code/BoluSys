//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BoluSys.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cows_log
    {
        public int id { get; set; }
        public int animal_id { get; set; }
        public string Event { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Event_Date { get; set; }
    }
}
