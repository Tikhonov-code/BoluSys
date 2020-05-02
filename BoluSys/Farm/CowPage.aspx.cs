using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class CowPage : System.Web.UI.Page
    {
        public string TotalIntakes { get; set; }
        public string CowInfo { get; set; }
        public int Bolus_id { get; set; }
        public string Animal_id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string SP = Request.QueryString["SP"];
            int bid;
            switch (SP)
            {
                case "GetFarmName":
                    string user_id = User.Identity.GetUserId();
                    GetFarmName(user_id);
                    break;
                case "GetDataForChart":
                    bid = Convert.ToInt16(Request.QueryString["Bolus_id"]);
                    GetDataForChart(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], bid);
                    break;
                case "GetIntakesData":
                    bid = Convert.ToInt16(Request.QueryString["Bolus_id"]);
                    GetIntakesData(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], bid);
                    break;
                case "GetTotalIntakes":
                    bid = Convert.ToInt16(Request.QueryString["Bolus_id"]);
                    GetTotalIntakes(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], bid);
                    break;
                case "GetCowInfoSt":
                    bid = Convert.ToInt16(Request.QueryString["Bolus_id"]);
                    GetCowInfoSt(bid);
                    break;
                default:
                    break;
            }

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
                    string dob = (bidini[0].Date_of_Birth == null) ? "N/A" : bidini[0].Date_of_Birth.Value.ToShortDateString();
                    string cdd = (bidini[0].Calving_Due_Date == null) ? "N/A" : bidini[0].Calving_Due_Date.Value.ToShortDateString();
                    string acd = (bidini[0].Actual_Calving_Date == null) ? "N/A" : bidini[0].Actual_Calving_Date.Value.ToShortDateString();

                    result = "<table><tr><td>Lactation # </td><td>" + bidini[0].Age_Lactation +
                         "</td><td>&nbsp;&nbsp;</td><td>Date of Birth :</td><td>" + dob + "</td></tr>" +
                              "<tr><td>Current Stage of Lactation : </td><td>" + bidini[0].Current_Stage_Of_Lactation +
                                 "</td><td>&nbsp;&nbsp;</td><td>Calving Due Date :</td><td>" + cdd + "</td></tr>" +
                              "<tr><td>Health Concerns Illness History : </td><td>" + bidini[0].Health_Concerns_Illness_History +
                              "</td><td>&nbsp;&nbsp;</td><td>Actual Calving Date : </td><td>" + acd + "</td></tr>" +
                              "<tr><td>Overall Health : </td><td>" + bidini[0].Overall_Health + "</td><td></td></tr>" +
                              "<tr><td>Comments : </td><td>" + bidini[0].Comments + "</td><td></td></tr>" +
                              "</table>"
                              ;

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

        [WebMethod]
        public static string GetTotalIntakes(string DateFrom, string DateTo, int bid)
        {
            DateTime dt1 = DateTime.Parse(DateFrom);
            DateTime dt2 = DateTime.Parse(DateTo);

            string result = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = context.WaterIntakes_sig(dt1, dt2, bid, 2).ToList();
                    result = res.Select(x => x.intakes).Sum().ToString();

                }
            }
            catch (Exception)
            {
                result = "0";
            }
            //Response.Clear();
            ////Response.ContentType = "application/json;charset=UTF-8";
            //Response.Write(result);
            //Response.End();
            return result;
        }

        public void GetIntakesData(string dtfrom, string dtto, int bid)
        {
            DateTime dt1 = DateTime.Parse(dtfrom);
            DateTime dt2 = DateTime.Parse(dtto);

            List<Intakes> intakesList = new List<Intakes>();
            string res_json;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = context.WaterIntakes_sig(dt1, dt2, bid, 2).Select(x => new
                    {
                        arg = x.bolus_full_date,
                        val = x.intakes
                    }).ToList();

                    //Check if intakes info exits
                    if (res.Count == 0)
                    {
                        res_json = null;
                    }
                    else
                    {
                        // Intakes data exits!
                        var arr_date = context.MeasDatas.Where(x => x.bolus_id == bid &&
                                (x.bolus_full_date >= dt1 && x.bolus_full_date <= dt2)).Select(
                            y => new
                            {
                                t = y.bolus_full_date.Value
                            }).OrderBy(y => y.t).ToList();

                        DateTime[] arr_dt = new DateTime[arr_date.Count + res.Count];
                        arr_dt[0] = dt1;
                        Intakes p = new Intakes();
                        p.val = 0.0;
                        foreach (var item in arr_date)
                        {
                            p.arg = item.t;
                            res.Add(new { p.arg, p.val });

                        }
                        var resOrdered = res.OrderBy(t => t.arg);

                        foreach (var item in resOrdered)
                        {
                            intakesList.Add(new Intakes { arg = item.arg, val = item.val });
                        }

                        List<IntakesStr> ilist = new List<IntakesStr>();
                        foreach (var item in intakesList)
                        {
                            ilist.Add(new IntakesStr
                            {
                                arg = item.arg.Value.ToShortDateString() + " " + item.arg.Value.ToShortTimeString(),
                                val = item.val
                            });
                        }

                        //res_json = JsonConvert.SerializeObject(intakesList);
                        res_json = JsonConvert.SerializeObject(ilist);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                res_json = null;
            }
            //return res_json;
            TotalIntakes = intakesList.Sum(x => x.val).Value.ToString();
            Page.DataBind();
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }


        private void GetFarmName(string user_id)
        {
            string res_json;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var farmName = context.Farms.Where(x => x.AspNetUser_Id == user_id).SingleOrDefault().Name;
                    var animalList = context.SP_GET_AnimalIdList(user_id).ToList();
                    //var animalList = {
                    //    name: "fffff",
                    //    al: [ "12", "14", "15"]
                    //};
                    string farmAnimalList = "{\"name\": \"" + farmName + "\",\"al\": ";

                    farmAnimalList += JsonConvert.SerializeObject(animalList) + "}";

                    res_json = farmAnimalList;// JsonConvert.SerializeObject(farmAnimalList);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                res_json = null;
            }
            //return res_json;
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        public void GetDataForChart(string DateFrom, string DateTo, int Bolus_id)
        {
            //GetCowInfo(Bolus_id, Bolus_id_Ini);
            //1. get data for chart Parse_StringToDateTime
            DateTime dt_from = Parse_StringToDateTime(DateFrom);
            DateTime dt_to = Parse_StringToDateTime(DateTo);
            List<BolusIDChart> res = new List<BolusIDChart>();
            //---------------------
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    //Temperature--------------------------------------------------------
                    var result2 = context.ChartsXY_temp(dt_from, dt_to, Bolus_id, 0.5);
                    DateTime dtvar = new DateTime();
                    foreach (var item in result2)
                    {
                        dtvar = DateTime.Parse(item.t);
                        var it = new BolusIDChart
                        {
                            // t = item.t,
                            t = dtvar.ToShortDateString() + " " + dtvar.ToShortTimeString(),
                            Temperature = item.Temperature
                        };
                        res.Add(it);
                        it = null;
                    }

                    var res_json = JsonConvert.SerializeObject(res);
                    Response.Clear();
                    Response.ContentType = "application/json;charset=UTF-8";
                    Response.Write(res_json);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                //return null;
            }
        }

        public DateTime Parse_StringToDateTime(string dt)
        {
            DateTime Evnt_Date = new DateTime();

            //dt = "Sun Nov 24 2019 00:00:00 GMT + 0200(Eastern European Standard Time)";

            bool dtct = DateTime.TryParse(dt, out Evnt_Date);
            if (!dtct)
            {
                string date = dt.Substring(4, 11);
                string s = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                Evnt_Date = (DateTime.TryParse(s, out Evnt_Date)) ? Evnt_Date : DateTime.Now;
            }
            return Evnt_Date;
        }
    }
}