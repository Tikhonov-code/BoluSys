using BoluSys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            ;
        }

        [WebMethod]
        public static string SendParameters(string DateSearch)//, int age)
        {
            //string DateSearch = "9/8/2019";
            DateTime dt = (!string.IsNullOrEmpty(DateSearch)) ? DateTime.Parse(DateSearch) : DateTime.Now;
            DateTime dtStart = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var b = context.MeasDatas.Where(a => a.bolus_full_date >= dtStart && a.bolus_full_date <= dtEnd).Select(x => new
                {
                    x.bolus_id,
                    x.animal_id
                }).Distinct().ToList();
                //-----------------------------------------------------
                //var newInnerText = "<button type='button' class='btn btn - light' onclick='ChartsShowAllRequest();'>All</button>";
                //for (var i = 0; i < b.Count; i++)
                //{
                //    newInnerText = newInnerText +
                //        "<button id='" + b[i].bolus_id + "' type='button' class='btn btn - light' onclick='ShowChartByDateBolus_ID(" + b[i].bolus_id + "," + b[i].animal_id + ");'>"
                //        + b[i].animal_id
                //        + "</button>";
                //}
                var res_json = JsonConvert.SerializeObject(b);
                return res_json;
            }
            //return string.Format("Name: {0}{2}Age: {1}", name, age, Environment.NewLine);
        }
    }
}