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
    
    public partial class SP_Admin_Z_AlertLogs_Result
    {
        public int bolus_id { get; set; }
        public Nullable<int> animal_id { get; set; }
        public string Name { get; set; }
        public string @event { get; set; }
        public string message { get; set; }
        public Nullable<System.DateTime> date_emailsent { get; set; }
        public string email { get; set; }
    }
}
