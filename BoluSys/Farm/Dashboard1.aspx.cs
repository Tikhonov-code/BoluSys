using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class Dashboard1 : System.Web.UI.Page
    {
        public int TotalCowsNumberInfo { get; set; }
        public int HealthyCowsNumber { get; set; }
        public int CowsAtRiskNumber { get; set; }
        public int CowsToCheckNumber { get; set; }

        public static string user_id { get; set; }
        public static string user_email { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            user_email = User.Identity.GetUserName();
            GetTotalCountsForDashboard(user_id);
            GetCowsInfo(user_id);
            string SP = Request.QueryString["SP"];
            switch (SP)
            {

                case "GetRiskData":
                    GetRiskData();
                    break;
                case "GetCheckData":
                    GetCheckData();
                    break;
                case "GetDataIntegrity":
                    GetDataIntegrity(user_id);
                    break;
                default:
                    break;
            }
        }

        private void GetDataIntegrity(string user_id)
        {
            DateTime dt = GetTorontoLocalDateTime().Date.AddDays(-1);
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            List<DataGapsFarm_Result> result ;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                result = context.DataGapsFarm(dt, 7, user_id).ToList();
            }

            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetTotalCountsForDashboard(string user_id)
        {
            TotalCowsNumberInfo = 0;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var bolusIdList = context.FarmCows.Where(x => x.AspNetUser_ID == user_id).Select(bl => bl.Bolus_ID).ToArray();
                TotalCowsNumberInfo = bolusIdList.Count();
            }
        }

        public string GetCowsInfo(string user_id)
        {
            string result = string.Empty;
            user_id = User.Identity.GetUserId();
            DateTime dt = GetTorontoLocalDateTime();
            //Begin of day
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //1. Cows At Risk Number ---------Q41------------------------------
                var q41 = context.FCN_Farm_TodayEventsList(dt, user_id, User.Identity.Name, "Q41").ToList();
                CowsAtRiskNumber = q41.Count();

                //3. Cows To Check Number---------------------------------------Q40.5
                var q405 = context.FCN_Farm_TodayEventsList(dt, user_id, User.Identity.Name, "Q40.5").ToList();
                CowsToCheckNumber = q405.Count();

                //2. Сows Under Monitoring Last 24 hours---------------------------------------
                HealthyCowsNumber = context.FCN_Farm_CowsUnderMonitoring(dt, user_id, 24).Count();
            }
            Page.DataBind();
            return "";
        }

        private void GetGapsData()
        {
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_to = DateTime.Now.Date.AddDays(-1);
            DateTime dt_from = dt_to.AddDays(-8);
            string user_id = User.Identity.GetUserId();

            List<Data_Gaps> result = new List<Data_Gaps>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var res = (from m in context.MeasDatas
                           join b in context.Bolus on m.bolus_id equals b.bolus_id
                           join fc in context.FarmCows on m.bolus_id equals fc.Bolus_ID
                           where m.bolus_full_date >= dt_from && m.bolus_full_date <= dt_to &&b.status == true
                           select new
                           {
                               bolus_id = m.bolus_id,
                               animal_id = m.animal_id,
                               bolus_full_date = m.bolus_full_date,
                           }).ToList();
                //--------------------------------------------------------------------
                var bid = res.Select(x => new { bid = x.bolus_id }).Distinct().OrderBy(x => x.bid).ToArray();
                int num_bid = bid.Length;
                double diffinterval = 0;
                foreach (var item in bid)
                {
                    var m = res.Where(x => x.bolus_id == item.bid).OrderBy(x => x.bolus_full_date).ToList();
                    int num_m = m.Count;
                    for (int i = 1; i < num_m; i++)
                    {
                        Data_Gaps dg = new Data_Gaps();
                        dg.bolus_id = m[i].bolus_id;
                        dg.animal_id = m[i].animal_id;
                        dg.dt_to = m[i].bolus_full_date.Value;
                        dg.dt_from = m[i - 1].bolus_full_date.Value;
                        diffinterval = (m[i].bolus_full_date.Value - m[i - 1].bolus_full_date.Value).TotalMinutes;

                        if (diffinterval > 15.5)
                        {
                            dg.interval = String.Format("{0:0.00}", diffinterval);
                            result.Add(dg);
                        }
                        dg = null;
                    }
                }
                //--------------------------------------------------------------------
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        public DateTime GetTorontoLocalDateTime()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            //----------------------------------------------------------------------------
            //return tdTor;
            return easternTime;
        }

        public void GetRiskData()
        {
            string result = string.Empty;
            user_id = User.Identity.GetUserId();
            DateTime dt = GetTorontoLocalDateTime();
            //Begin of day
            dt = dt.AddDays(-7);
            object q41;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //1. Cows At Risk Number ------------------------------------------------------------------------
                q41 = context.FCN_Farm_TodayEventsList(dt, user_id, User.Identity.Name, "Q41").OrderByDescending(x => x.date_emailsent).ToList();
            }
            var res_json = JsonConvert.SerializeObject(q41);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        public void GetCheckData()
        {
            string result = string.Empty;
            user_id = User.Identity.GetUserId();
            DateTime dt = GetTorontoLocalDateTime();
            //Begin of day
            dt = dt.AddDays(-7);
            object q405;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //1. Cows At Risk Number ------------------------------------------------------------------------
                q405 = context.FCN_Farm_TodayEventsList(dt, user_id, User.Identity.Name, "Q40.5").OrderByDescending(x => x.date_emailsent).ToList();
            }
            var res_json = JsonConvert.SerializeObject(q405);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
    }
}