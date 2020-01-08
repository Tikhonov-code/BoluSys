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
                default:
                    break;
            }
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
                HealthyCowsNumber = context.FCN_Farm_CowsUnderMonitoring(dt,user_id,24).Count();
            }
            Page.DataBind();
            return "";
        }
        public DateTime GetTorontoLocalDateTime()
        {
            string html = string.Empty;
            string url = "https://www.timeanddate.com/worldclock/fullscreen.html?n=250";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "C# console client";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            // Read local Toronto time and calculate hour difference
            int indx_TimeTor = html.IndexOf("i_time");
            int len_TimeTor = html.IndexOf('<', indx_TimeTor) - indx_TimeTor - 7;
            int TimeToronto = Convert.ToInt16(html.Substring(indx_TimeTor + 7, len_TimeTor - 3).Split(':')[0]);

            DateTime tdToroto = DateTime.Now.AddHours(TimeToronto - DateTime.Now.Hour);
            return tdToroto;
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
                q405 = context.FCN_Farm_TodayEventsList(dt, user_id, User.Identity.Name, "Q40.5").OrderByDescending(x=>x.date_emailsent).ToList();
            }
            var res_json = JsonConvert.SerializeObject(q405);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
    }
}