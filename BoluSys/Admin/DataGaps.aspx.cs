using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Admin
{
    public partial class DataGaps : System.Web.UI.Page
    {
        public static string user_id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(user_id)) return;

            string SP = Request.QueryString["SP"];
            switch (SP)
            {
                case "GetGapsData":
                    GetGapsData(user_id, Request.QueryString["dt0"], Request.QueryString["dt1"]);
                    break;
                default:
                    break;
            }
        }

        private void GetGapsData(string user_id, string dt0, string dt1)
        {
            user_id = User.Identity.GetUserId();
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0); 
            DateTime dt_to = DateTime.Parse(dt1); 

            List<Data_Gaps> result = new List<Data_Gaps>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var res = (from m in context.MeasDatas
                           join b in context.Bolus on m.bolus_id equals b.bolus_id
                           join fc in context.FarmCows on m.bolus_id equals fc.Bolus_ID
                           where fc.AspNetUser_ID == user_id
                           && m.bolus_full_date >= dt_from && m.bolus_full_date <= dt_to
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
                    var m = res.Where(x => x.bolus_id == item.bid).OrderBy(x=>x.bolus_full_date).ToList();
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
    }
}