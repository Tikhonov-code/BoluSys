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
    
    public partial class Bolu
    {
        public int id { get; set; }
        public int bolus_id { get; set; }
        public Nullable<int> animal_id { get; set; }
        public string AnimalInfo { get; set; }
        public Nullable<int> Age_Lactation { get; set; }
        public string Current_Stage_Of_Lactation { get; set; }
        public string Health_Concerns_Illness_History { get; set; }
        public string Overall_Health { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> Date_of_Birth { get; set; }
        public Nullable<System.DateTime> Calving_Due_Date { get; set; }
        public Nullable<System.DateTime> Actual_Calving_Date { get; set; }
        public bool status { get; set; }
        public Nullable<int> current_lactation { get; set; }
    }
}
