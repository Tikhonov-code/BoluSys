﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DB_A4A060_csEntities : DbContext
    {
        public DB_A4A060_csEntities()
            : base("name=DB_A4A060_csEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bolu> Bolus { get; set; }
        public virtual DbSet<MeasData> MeasDatas { get; set; }
        public virtual DbSet<FarmCow> FarmCows { get; set; }
    
        public virtual ObjectResult<SP_GET_FARM_STAT_Result> SP_GET_FARM_STAT(Nullable<System.DateTime> date, Nullable<double> temp_limit, Nullable<int> measurements)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            var temp_limitParameter = temp_limit.HasValue ?
                new ObjectParameter("temp_limit", temp_limit) :
                new ObjectParameter("temp_limit", typeof(double));
    
            var measurementsParameter = measurements.HasValue ?
                new ObjectParameter("measurements", measurements) :
                new ObjectParameter("measurements", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GET_FARM_STAT_Result>("SP_GET_FARM_STAT", dateParameter, temp_limitParameter, measurementsParameter);
        }
    
        public virtual ObjectResult<ChartsXY_Result> ChartsXY(Nullable<System.DateTime> dt1, Nullable<System.DateTime> dt2, Nullable<int> bolus_id, Nullable<double> tmin, Nullable<double> tmax)
        {
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var dt2Parameter = dt2.HasValue ?
                new ObjectParameter("dt2", dt2) :
                new ObjectParameter("dt2", typeof(System.DateTime));
    
            var bolus_idParameter = bolus_id.HasValue ?
                new ObjectParameter("bolus_id", bolus_id) :
                new ObjectParameter("bolus_id", typeof(int));
    
            var tminParameter = tmin.HasValue ?
                new ObjectParameter("tmin", tmin) :
                new ObjectParameter("tmin", typeof(double));
    
            var tmaxParameter = tmax.HasValue ?
                new ObjectParameter("tmax", tmax) :
                new ObjectParameter("tmax", typeof(double));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ChartsXY_Result>("ChartsXY", dt1Parameter, dt2Parameter, bolus_idParameter, tminParameter, tmaxParameter);
        }
    }
}
