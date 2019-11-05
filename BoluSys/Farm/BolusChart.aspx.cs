using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class BolusChart : System.Web.UI.Page
    {
        public string CowInfo { get; set; }
        public int Bolus_id { get; set; }
        public int Bolus_id_Ini { get; set; }
        public string Animal_id_Ini { get; set; }
        public string Animal_id { get; set; }
        public string DateSearch { get; set; }
        public static string user_id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            string SP = Request.QueryString["SP"];
            Animal_id = Request.QueryString["Animal_id"];
            DateSearch = Request.QueryString["DateSearch"];
            DateSearch = (DateSearch == "today") ? DateTime.Now.ToShortDateString() : DateSearch;

            Bolus_id_Ini = GetBolusIdInitial(user_id);
            //CowInfo = GetCowInfo(Bolus_id, Bolus_id_Ini);

            switch (SP)
            {
                case "GetBolusIDList":
                    GetBolusIDList();
                    break;
                case "GetCowInfoSt":
                    GetCowInfoSt(Convert.ToUInt16(Request.QueryString["bolus_id"]));
                    break;
                default:
                    break;
            }
            //-------------------------------------------------------------------------------
            string DateFrom, DateTo;
            //if (!string.IsNullOrEmpty(DateSearch))
            //{
            //    DateTime ddtt = DateTime.Parse(DateSearch);
            //    DateFrom = (new DateTime(ddtt.Year, ddtt.Month, ddtt.Day, 0, 0, 0)).ToString();
            //    DateTo = (new DateTime(ddtt.Year, ddtt.Month, ddtt.Day, 23, 59, 59)).ToString();
            //}
            //else
            {
                DateFrom = Request.QueryString["DateFrom"];
                DateTo = Request.QueryString["DateTo"];
            }


            Bolus_id = Convert.ToUInt16(Request.QueryString["Bolus_id"]);
            Animal_id = Request.QueryString["Animal_id"];
            CowInfo = GetCowInfo(Bolus_id, Bolus_id_Ini);

            Page.DataBind();

            if (DateFrom != null && DateTo != null && Bolus_id != 0)
            {
                GetDataForChart(DateFrom, DateTo, Bolus_id);
            }
        }
        [WebMethod]
        private string GetCowInfo(int bolus_id, int bolus_id_Ini)
        {
            string result = string.Empty;
            int bid = (bolus_id != 0) ? bolus_id : bolus_id_Ini;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bidini = context.Bolus.Where(x => x.bolus_id == bid).DefaultIfEmpty().ToList();
                    result = "<table><tr><td>Age Lactation # </td><td>" + bidini[0].Age_Lactation + "</td></tr>" +
                             "<tr><td>Current Stage of Lactation : </td><td>" + bidini[0].Current_Stage_Of_Lactation + "</td></tr>" +
                             "<tr><td>Health Concerns Illness History : </td><td>" + bidini[0].Health_Concerns_Illness_History + "</td></tr>" +
                             "<tr><td>Overall Health : </td><td>" + bidini[0].Overall_Health + "</td></tr>" +
                             "<tr><td>Comments : </td><td>" + bidini[0].Comments + "</td></tr>" +
                             "</table>"
                             ;
                    Animal_id_Ini = bidini[0].animal_id.ToString();
                }
            }
            catch (Exception)
            {
                result = "None";
            }
            //var res_json = JsonConvert.SerializeObject(result);
            return result;
        }
        [WebMethod]
        public static string GetCowInfoSt(int bolus_id)
        {
            string result = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bidini = context.Bolus.Where(x => x.bolus_id == bolus_id).DefaultIfEmpty().ToList();
                    result = "<table><tr><td>Age Lactation # </td><td>" + bidini[0].Age_Lactation + "</td></tr>" +
                             "<tr><td>Current Stage of Lactation : </td><td>" + bidini[0].Current_Stage_Of_Lactation + "</td></tr>" +
                             "<tr><td>Health Concerns Illness History : </td><td>" + bidini[0].Health_Concerns_Illness_History + "</td></tr>" +
                             "<tr><td>Overall Health : </td><td>" + bidini[0].Overall_Health + "</td></tr>" +
                             "<tr><td>Comments : </td><td>" + bidini[0].Comments + "</td></tr>" +
                             "</table>";

                }
            }
            catch (Exception)
            {
                result = "None";
            }
            //Response.Clear();
            ////Response.ContentType = "application/json;charset=UTF-8";
            //Response.Write(result);
            //Response.End();
            return result;
        }

        private int GetBolusIdInitial(string userid)
        {
            int result = Convert.ToUInt16(Request.QueryString["Bolus_id"]);
            if (result != 0) return result;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bidini = context.FarmCows.Where(x => x.AspNetUser_ID == userid).OrderBy(x => x.Bolus_ID).Take(1).ToList();
                    result = bidini[0].Bolus_ID;

                }
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }

        [WebMethod]
        public void GetDataForChart(string DateFrom, string DateTo, int Bolus_id)
        {
            //GetCowInfo(Bolus_id, Bolus_id_Ini);
            //1. get data for chart
            DateTime dt_from = DateTime.Parse(DateFrom);
            DateTime dt_to = DateTime.Parse(DateTo);
            //---------------------
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var result = context.MeasDatas.Where(x => x.bolus_id == Bolus_id &&
                            (x.bolus_full_date >= dt_from && x.bolus_full_date <= dt_to)).Select(
                        y => new
                        {
                            t = y.bolus_full_date.Value,
                            Temperature = y.temperature
                        }).OrderBy(y => y.t).ToList();
                    //----------------------------------------------------
                    List<BolusIDChart> res = new List<BolusIDChart>();
                    foreach (var item in result)
                    {
                        var it = new BolusIDChart();
                        it.t = item.t.ToShortDateString() + " " + item.t.ToShortTimeString();
                        it.Temperature = item.Temperature;
                        res.Add(it);
                        it = null;
                    }
                    var res_json = JsonConvert.SerializeObject(res);
                    Response.Clear();
                    Response.ContentType = "application/json;charset=UTF-8";
                    Response.Write(res_json);
                    Response.End();
                    // return res_json;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                //return null;
            }
        }

        [WebMethod]
        public static Array GetBolusIDList()
        {
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bolusIdList = context.FarmCows.Where(x => x.AspNetUser_ID == user_id).Select(bl => bl.Bolus_ID).ToArray();
                    //----------------------------------------------------
                    var result = (from bl in context.Bolus
                              join fc in context.FarmCows on bl.bolus_id equals fc.Bolus_ID
                              where fc.AspNetUser_ID == user_id
                              select new
                              {
                                  bolus_id = bl.bolus_id,
                                  animal_id = bl.animal_id
                              }
                             ).Distinct().OrderBy(x => x.animal_id).ToArray();
                    //----------------------------------------------------
                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        //[WebMethod]
        //public void ShowChart(string DateSearch, string Animal_id, string Bolus_id)
        //{
        //    //1. Set dateboxes and selecBox
        //    //2. create chart
        //    ;
        //    string DateFrom = "9/22/2019";
        //    string DateTo = "9/22/2019 23:00:00";
        //    GetDataForChart(DateFrom, DateTo, Convert.ToInt16(Bolus_id));
        //}
    }
}