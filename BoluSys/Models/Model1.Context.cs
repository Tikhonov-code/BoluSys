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
    
        public virtual DbSet<MeasData> MeasDatas { get; set; }
        public virtual DbSet<FarmCow> FarmCows { get; set; }
        public virtual DbSet<UserCabinet> UserCabinets { get; set; }
        public virtual DbSet<Bolu> Bolus { get; set; }
        public virtual DbSet<Cows_log> Cows_log { get; set; }
        public virtual DbSet<Z_AlertLogs> Z_AlertLogs { get; set; }
        public virtual DbSet<Farm> Farms { get; set; }
        public virtual DbSet<SmsLog> SmsLogs { get; set; }
        public virtual DbSet<Farm_Alert_Rules> Farm_Alert_Rules { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
    
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
    
        public virtual int ChartsXY_All(Nullable<System.DateTime> dt1, Nullable<System.DateTime> dt2, string userid)
        {
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var dt2Parameter = dt2.HasValue ?
                new ObjectParameter("dt2", dt2) :
                new ObjectParameter("dt2", typeof(System.DateTime));
    
            var useridParameter = userid != null ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ChartsXY_All", dt1Parameter, dt2Parameter, useridParameter);
        }
    
        public virtual ObjectResult<ChartsXY_sig_Result> ChartsXY_sig(Nullable<System.DateTime> dt1, Nullable<System.DateTime> dt2, Nullable<int> bolus_id, Nullable<double> tmin, Nullable<double> tmax)
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ChartsXY_sig_Result>("ChartsXY_sig", dt1Parameter, dt2Parameter, bolus_idParameter, tminParameter, tmaxParameter);
        }
    
        public virtual ObjectResult<WaterIntakes_sig_Result> WaterIntakes_sig(Nullable<System.DateTime> dt1, Nullable<System.DateTime> dt2, Nullable<int> bolus_id, Nullable<double> wi_calbr)
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
    
            var wi_calbrParameter = wi_calbr.HasValue ?
                new ObjectParameter("wi_calbr", wi_calbr) :
                new ObjectParameter("wi_calbr", typeof(double));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<WaterIntakes_sig_Result>("WaterIntakes_sig", dt1Parameter, dt2Parameter, bolus_idParameter, wi_calbrParameter);
        }
    
        public virtual ObjectResult<ChartsXY_temp_Result> ChartsXY_temp(Nullable<System.DateTime> dt1, Nullable<System.DateTime> dt2, Nullable<int> bolus_id, Nullable<double> interval)
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
    
            var intervalParameter = interval.HasValue ?
                new ObjectParameter("Interval", interval) :
                new ObjectParameter("Interval", typeof(double));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ChartsXY_temp_Result>("ChartsXY_temp", dt1Parameter, dt2Parameter, bolus_idParameter, intervalParameter);
        }
    
        [DbFunction("DB_A4A060_csEntities", "FCN_CowsIsSick")]
        public virtual IQueryable<FCN_CowsIsSick_Result> FCN_CowsIsSick(Nullable<System.DateTime> dt, Nullable<int> hoursinterval, Nullable<double> temp_limit, string user_id)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var hoursintervalParameter = hoursinterval.HasValue ?
                new ObjectParameter("hoursinterval", hoursinterval) :
                new ObjectParameter("hoursinterval", typeof(int));
    
            var temp_limitParameter = temp_limit.HasValue ?
                new ObjectParameter("temp_limit", temp_limit) :
                new ObjectParameter("temp_limit", typeof(double));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FCN_CowsIsSick_Result>("[DB_A4A060_csEntities].[FCN_CowsIsSick](@dt, @hoursinterval, @temp_limit, @user_id)", dtParameter, hoursintervalParameter, temp_limitParameter, user_idParameter);
        }
    
        [DbFunction("DB_A4A060_csEntities", "FCN_CowsIsSad")]
        public virtual IQueryable<FCN_CowsIsSad_Result> FCN_CowsIsSad(Nullable<System.DateTime> dt, Nullable<int> hoursinterval, Nullable<double> temp_limit, string user_id)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var hoursintervalParameter = hoursinterval.HasValue ?
                new ObjectParameter("hoursinterval", hoursinterval) :
                new ObjectParameter("hoursinterval", typeof(int));
    
            var temp_limitParameter = temp_limit.HasValue ?
                new ObjectParameter("temp_limit", temp_limit) :
                new ObjectParameter("temp_limit", typeof(double));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FCN_CowsIsSad_Result>("[DB_A4A060_csEntities].[FCN_CowsIsSad](@dt, @hoursinterval, @temp_limit, @user_id)", dtParameter, hoursintervalParameter, temp_limitParameter, user_idParameter);
        }
    
        [DbFunction("DB_A4A060_csEntities", "FCN_Farm_TodayEventsList")]
        public virtual IQueryable<FCN_Farm_TodayEventsList_Result> FCN_Farm_TodayEventsList(Nullable<System.DateTime> dt, string user_id, string email, string @event)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var eventParameter = @event != null ?
                new ObjectParameter("event", @event) :
                new ObjectParameter("event", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FCN_Farm_TodayEventsList_Result>("[DB_A4A060_csEntities].[FCN_Farm_TodayEventsList](@dt, @user_id, @Email, @event)", dtParameter, user_idParameter, emailParameter, eventParameter);
        }
    
        [DbFunction("DB_A4A060_csEntities", "FCN_Farm_CowsUnderMonitoring")]
        public virtual IQueryable<Nullable<int>> FCN_Farm_CowsUnderMonitoring(Nullable<System.DateTime> dt, string user_id, Nullable<int> interval)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            var intervalParameter = interval.HasValue ?
                new ObjectParameter("interval", interval) :
                new ObjectParameter("interval", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Nullable<int>>("[DB_A4A060_csEntities].[FCN_Farm_CowsUnderMonitoring](@dt, @user_id, @interval)", dtParameter, user_idParameter, intervalParameter);
        }
    
        public virtual int SP_GET_DataGaps(Nullable<System.DateTime> dt, Nullable<int> interval, string user_id)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var intervalParameter = interval.HasValue ?
                new ObjectParameter("interval", interval) :
                new ObjectParameter("interval", typeof(int));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_DataGaps", dtParameter, intervalParameter, user_idParameter);
        }
    
        public virtual ObjectResult<DataGapsFarm_Result> DataGapsFarm(Nullable<System.DateTime> dt, Nullable<int> interval, string user_id)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var intervalParameter = interval.HasValue ?
                new ObjectParameter("interval", interval) :
                new ObjectParameter("interval", typeof(int));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DataGapsFarm_Result>("DataGapsFarm", dtParameter, intervalParameter, user_idParameter);
        }
    
        public virtual ObjectResult<SP_Admin_WaterIntakesReport_Result> SP_Admin_WaterIntakesReport(Nullable<System.DateTime> dt, Nullable<int> interval)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var intervalParameter = interval.HasValue ?
                new ObjectParameter("interval", interval) :
                new ObjectParameter("interval", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Admin_WaterIntakesReport_Result>("SP_Admin_WaterIntakesReport", dtParameter, intervalParameter);
        }
    
        public virtual ObjectResult<SP_Admin_Z_AlertLogs_Result> SP_Admin_Z_AlertLogs(Nullable<System.DateTime> dt0, Nullable<System.DateTime> dt1, string user_id)
        {
            var dt0Parameter = dt0.HasValue ?
                new ObjectParameter("dt0", dt0) :
                new ObjectParameter("dt0", typeof(System.DateTime));
    
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Admin_Z_AlertLogs_Result>("SP_Admin_Z_AlertLogs", dt0Parameter, dt1Parameter, user_idParameter);
        }
    
        public virtual ObjectResult<Nullable<double>> SP_GET_WaterIntakesSum(Nullable<System.DateTime> dt1, Nullable<System.DateTime> dt2, Nullable<int> bolus_id, Nullable<double> wi_calbr)
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
    
            var wi_calbrParameter = wi_calbr.HasValue ?
                new ObjectParameter("wi_calbr", wi_calbr) :
                new ObjectParameter("wi_calbr", typeof(double));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<double>>("SP_GET_WaterIntakesSum", dt1Parameter, dt2Parameter, bolus_idParameter, wi_calbrParameter);
        }
    
        public virtual ObjectResult<SP_GET_BolusDataGaps_Result> SP_GET_BolusDataGaps(Nullable<System.DateTime> dt0, Nullable<System.DateTime> dt1, Nullable<int> bid)
        {
            var dt0Parameter = dt0.HasValue ?
                new ObjectParameter("dt0", dt0) :
                new ObjectParameter("dt0", typeof(System.DateTime));
    
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var bidParameter = bid.HasValue ?
                new ObjectParameter("bid", bid) :
                new ObjectParameter("bid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GET_BolusDataGaps_Result>("SP_GET_BolusDataGaps", dt0Parameter, dt1Parameter, bidParameter);
        }
    
        public virtual int SP_GET_DataGapsFarm(Nullable<System.DateTime> dt, Nullable<int> interval, string user_id)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var intervalParameter = interval.HasValue ?
                new ObjectParameter("interval", interval) :
                new ObjectParameter("interval", typeof(int));
    
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_DataGapsFarm", dtParameter, intervalParameter, user_idParameter);
        }
    
        public virtual int SP_Admin_GapsByFarmHerd(Nullable<System.DateTime> dt0, Nullable<System.DateTime> dt1, string user)
        {
            var dt0Parameter = dt0.HasValue ?
                new ObjectParameter("dt0", dt0) :
                new ObjectParameter("dt0", typeof(System.DateTime));
    
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Admin_GapsByFarmHerd", dt0Parameter, dt1Parameter, userParameter);
        }
    
        public virtual int SP_GET_DataTempWIntakes(Nullable<System.DateTime> dt0, Nullable<System.DateTime> dt1, Nullable<int> bolus_id)
        {
            var dt0Parameter = dt0.HasValue ?
                new ObjectParameter("dt0", dt0) :
                new ObjectParameter("dt0", typeof(System.DateTime));
    
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var bolus_idParameter = bolus_id.HasValue ?
                new ObjectParameter("bolus_id", bolus_id) :
                new ObjectParameter("bolus_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GET_DataTempWIntakes", dt0Parameter, dt1Parameter, bolus_idParameter);
        }
    
        public virtual ObjectResult<SP_GET_AnimalIdList_Result> SP_GET_AnimalIdList(string user_id)
        {
            var user_idParameter = user_id != null ?
                new ObjectParameter("user_id", user_id) :
                new ObjectParameter("user_id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GET_AnimalIdList_Result>("SP_GET_AnimalIdList", user_idParameter);
        }
    
        public virtual ObjectResult<SP_Admin_TempIntakesChart_Result> SP_Admin_TempIntakesChart(Nullable<System.DateTime> dtfrom, Nullable<System.DateTime> dtto, Nullable<int> bid, Nullable<double> wi_calbrx)
        {
            var dtfromParameter = dtfrom.HasValue ?
                new ObjectParameter("dtfrom", dtfrom) :
                new ObjectParameter("dtfrom", typeof(System.DateTime));
    
            var dttoParameter = dtto.HasValue ?
                new ObjectParameter("dtto", dtto) :
                new ObjectParameter("dtto", typeof(System.DateTime));
    
            var bidParameter = bid.HasValue ?
                new ObjectParameter("bid", bid) :
                new ObjectParameter("bid", typeof(int));
    
            var wi_calbrxParameter = wi_calbrx.HasValue ?
                new ObjectParameter("wi_calbrx", wi_calbrx) :
                new ObjectParameter("wi_calbrx", typeof(double));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Admin_TempIntakesChart_Result>("SP_Admin_TempIntakesChart", dtfromParameter, dttoParameter, bidParameter, wi_calbrxParameter);
        }
    
        public virtual ObjectResult<SP_Admin_SMSserviceList_Result> SP_Admin_SMSserviceList(Nullable<System.DateTime> dt0, Nullable<System.DateTime> dt1, string farm_userID)
        {
            var dt0Parameter = dt0.HasValue ?
                new ObjectParameter("dt0", dt0) :
                new ObjectParameter("dt0", typeof(System.DateTime));
    
            var dt1Parameter = dt1.HasValue ?
                new ObjectParameter("dt1", dt1) :
                new ObjectParameter("dt1", typeof(System.DateTime));
    
            var farm_userIDParameter = farm_userID != null ?
                new ObjectParameter("farm_userID", farm_userID) :
                new ObjectParameter("farm_userID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Admin_SMSserviceList_Result>("SP_Admin_SMSserviceList", dt0Parameter, dt1Parameter, farm_userIDParameter);
        }
    }
}
