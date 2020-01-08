using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class Dashboard : System.Web.UI.Page
    {
        public static string user_id { get; set; }
        public static string user_email { get; set; }
        public string HerdList_Sad { get; set; }
        public string HerdList_Sick { get; set; }
        public string HerdList_Healthy { get; set; }
        public int num_HerdList_Sad { get; set; }
        public int num_HerdList_Sick { get; set; }
        public int num_HerdList_Healthy { get; set; }

        public int TotalCowsNumberInfo { get; set; }
        public int CowsUnderMonitoring { get; set; }
        public int CowsToCheck { get; set; }
        public int CowsAtRisk { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            user_email = User.Identity.GetUserName();
            GetTotalCountsForDashboard(user_id);
            GetCowsHerd();
            string SP = Request.QueryString["SP"];
            switch (SP)
            {
                case "GetAlerts":
                    GetAlerts();
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

                DateTime dt_to = DateTime.Now;
                DateTime dt_from = dt_to.AddHours(-12);

                CowsUnderMonitoring = context.MeasDatas.Where(x => x.bolus_full_date >= dt_from && x.bolus_full_date <= dt_to && bolusIdList.Contains(x.bolus_id)).Select(
                    x => new
                    {
                        bl = x.bolus_id
                    }).Distinct().Count();
                ;
            }
            CowsToCheck = 2;
            CowsAtRisk = 3;
        }

        [WebMethod]
        public string GetCowsHerd()
        {
            string result = string.Empty;
            user_id = User.Identity.GetUserId();

            //1. Herd ------------------------------------------------------------------------
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var cowslist = (from b in context.Bolus
                                join a in context.FarmCows on b.bolus_id equals a.Bolus_ID
                                where a.AspNetUser_ID == user_id
                                select new
                                {
                                    bid = b.bolus_id,
                                    aid = b.animal_id
                                }).OrderBy(x => x.aid).ToList();
                //2. Current Toronto Time
                DateTime dtToronto = GetTorontoLocalDateTime();

                //3. Sick cows---------------------------------------Q41
                var SickCowsList = context.FCN_CowsIsSick(dtToronto, 24, 41, user_id).OrderBy(x => x.aid).ToList();
                num_HerdList_Sick = SickCowsList.Count();
                HerdList_Sick = string.Empty;
                foreach (var item in SickCowsList)
                {
                    HerdList_Sick += "<div style='background-color: crimson; width: 80px; height: 80px; display: inline-block;' >" +
                     "<img src='imgs/cowsick.jpg' class='img-circle img-thumbnail'" +
                     " style='width: 80px; height: 80px;'" +
                     " data-toggle='tooltip' data-placement='top' title='" + item.aid + "' /><h4>" + item.aid + "</h4></div>";
                }
                //2. Sad cows---------------------------------------

                var SadCowsList = context.FCN_CowsIsSad(dtToronto, 3, 40.5, user_id).OrderBy(x => x.aid).ToList();
                // remove sick bolus from sad list
                foreach (var item in SickCowsList)
                {
                    SadCowsList.RemoveAll(x => x.bid == item.bid);
                }
                num_HerdList_Sad = SadCowsList.Count();
                HerdList_Sad = string.Empty;
                foreach (var item in SadCowsList)
                {
                    HerdList_Sad += "<div style='background-color: goldenrod; width: 80px; height: 80px; display: inline-block;' >" +
                     "<img src='imgs/cowSad.jpg' class='img-circle img-thumbnail'" +
                     " style='width: 80px; height: 80px;'" +
                     " data-toggle='tooltip' data-placement='top' title='" + item.aid + "' /><h4>" + item.aid + "</h4></div>";
                }

                //4. Healthy cows
                foreach (var item in SickCowsList)
                {
                    cowslist.RemoveAll(x => x.bid == item.bid);
                }
                foreach (var item in SadCowsList)
                {
                    cowslist.RemoveAll(x => x.bid == item.bid);
                }

                HerdList_Healthy = string.Empty;
                num_HerdList_Healthy = cowslist.Count();
                foreach (var item in cowslist)
                {
                    HerdList_Healthy += "<div id='image' style='background-color: darkseagreen; width: 80px; height: 80px; display: inline-block;' >" +
                    "<img src='imgs/cowHealthy.jpg' onclick='Myalert();' class='img-circle img-thumbnail'" +
                    " style='width: 80px; height: 80px;'" +
                    " data-toggle='tooltip' data-placement='top' title='" + item.aid + "'" +
                    "  /><h4>" + item.aid + "</h4></div>";
                }
                //---------------------------------------
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
        [WebMethod]
        public void GetAlerts()
        {

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var result = context.Z_AlertLogs.OrderByDescending(x => x.date_emailsent)
                    .Where(x=>x.email == user_email)
                    .Select(x=> new {
                                line = x.@event+" "+x.message
                                    }).Take(10).ToList();

                var res_json = JsonConvert.SerializeObject(result);
                //return res_json;
                Response.Clear();
                Response.ContentType = "application/json;charset=UTF-8";
                Response.Write(res_json);
                Response.End();
            }
        }
    }
}