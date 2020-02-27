using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class BolusChart_new : System.Web.UI.Page
    {
        public string CowInfo { get; set; }
        public int Bolus_id { get; set; }
        public int Bolus_id_Ini { get; set; }
        public string Animal_id_Ini { get; set; }
        public string Animal_id { get; set; }
        public string DateSearch { get; set; }
        public static string user_id { get; set; }
        public string TotalIntakes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            string SP = Request.QueryString["SP"];
            DateSearch = Request.QueryString["DateSearch"];

            //Gate-1  case DateSearch = (DateSearch == "today") ? DateTime.Now.ToShortDateString() : DateSearch;
            if ((DateSearch == "today" || string.IsNullOrEmpty(DateSearch)) && string.IsNullOrEmpty(SP))
            {
                Gate_1();
                return;
            }

            switch (SP)
            {
                case "GetDataForChart":
                    string chart_type = Request.QueryString["chart_type"];
                    GetDataForChart(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Convert.ToInt32(Request.QueryString["Bolus_id"]), chart_type);
                    break;
                case "ShowChart":
                    //Gate 2 case request with full parameters set 
                    //BolusChart.aspx?Animal_id=2753&Bolus_id=22&DateSearch=2019-11-25&SP=ShowChart
                    Gate_2();
                    break;
                case "GetBolusIDList":
                    GetBolusIDList();
                    break;
                case "GetCowInfoSt":
                    GetCowInfoSt(Convert.ToUInt16(Request.QueryString["bolus_id"]));
                    break;
                case "GetCowsLogs":
                    GetCowsLogs(Convert.ToUInt16(Request.QueryString["Animal_id"]));
                    break;
                case "GetIntakesData":
                    GetIntakesData(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Request.QueryString["Bolus_id"]);
                    break;
                case "GetGapsData":
                    GetGapsData(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Request.QueryString["Bolus_id"]);
                    break;
                default:
                    break;
            }
        }

        private void Gate_1()
        {
            //1. DateFrom & DateTo settings
            DateSearch = DateTime.Now.ToShortDateString();
            //2. Set initial Bolus_id and Animal_id
            SetBolusAnimalIdsInitial(user_id);
            Bolus_id = Bolus_id_Ini;
            Animal_id = Animal_id_Ini;
            //3. DataBinding
            Page.DataBind();
        }

        private void Gate_2()
        {
            //1. DateFrom & DateTo settings
            DateSearch = Request.QueryString["DateSearch"];

            //2. Set Bolus_id and Animal_id from input get request
            //BolusChart.aspx?Animal_id=2753&Bolus_id=22&DateSearch=2019-11-25&SP=ShowChart

            Bolus_id = Convert.ToInt16(Request.QueryString["Bolus_id"]);
            Animal_id = Request.QueryString["Animal_id"];

            //3. DataBinding
            Page.DataBind();
        }
        private void SetBolusAnimalIdsInitial(string user_id)
        {
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bidaid = (from b in context.Bolus
                                  join a in context.FarmCows on b.bolus_id equals a.Bolus_ID
                                  where a.AspNetUser_ID == user_id
                                  select new
                                  {
                                      bid = b.bolus_id,
                                      aid = b.animal_id
                                  }).OrderBy(x => x.aid).FirstOrDefault();

                    Bolus_id_Ini = bidaid.bid;
                    Animal_id_Ini = bidaid.aid.ToString();
                }
            }
            catch (Exception)
            {
                Bolus_id_Ini = 0;
                Animal_id_Ini = "";
            }
        }

        private string GetAnimalIdInitial(int bid)
        {
            string result = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bidini = context.Bolus.Where(x => x.bolus_id == bid).OrderBy(x => x.animal_id).Take(1).ToList();
                    result = bidini[0].animal_id.ToString();

                }
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        [WebMethod]
        public void GetCowsLogs(int aid)
        {
            //------------------------------------------------------------------------
            string res_json;
            ArrayList ds = new ArrayList();

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {

                var result = context.Cows_log.Where(x => x.animal_id == aid).Select(x => new
                {
                    raw = x.Event_Date.Value.ToString() + "  : " + x.Event + ". " + x.Description
                }).ToList();
                //---------------------------------------
                foreach (var item in result)
                {
                    ds.Add(item.raw);
                }
                //---------------------------------------
                res_json = JsonConvert.SerializeObject(ds);
            }
            //return res_json;
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
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
        public void GetDataForChart(string DateFrom, string DateTo, int Bolus_id, string chart_type)
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
                    switch (chart_type)
                    {
                        case "full":
                            //Full-------------------------------------------------------------
                            var result1 = context.MeasDatas.Where(x => x.bolus_id == Bolus_id &&
                            (x.bolus_full_date >= dt_from && x.bolus_full_date <= dt_to)).Select(y => new
                                    {
                                        t = y.bolus_full_date.Value,
                                        Temperature = y.temperature
                                    }).OrderBy(y => y.t).ToList();
                            foreach (var item in result1)
                            {
                                var it = new BolusIDChart
                                {
                                    t = item.t.ToShortDateString() + " " + item.t.ToShortTimeString(),
                                    Temperature = item.Temperature
                                };
                                res.Add(it);
                                it = null;
                            }
                            //Full-------------------------------------------------------------
                            break;
                        case "temp":
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
                            //Temperature--------------------------------------------------------
                            break;
                        default:
                            break;
                    }
                    //----------------------------------------------------
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
                                  where fc.AspNetUser_ID == user_id && bl.status == true
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
        //[WebMethod]
        //public void GetIntakesData(string dtfrom, string dtto, string bid)
        //{
        //    DateTime dt1 = DateTime.Parse(dtfrom);
        //    DateTime dt2 = DateTime.Parse(dtto);
        //    int bid_int = Convert.ToInt32(bid);
        //    string res_json;
        //    try
        //    {
        //        using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
        //        {
        //            var res = context.WaterIntakes_sig(dt1, dt2, bid_int, 1).Select(x => new
        //            {
        //                arg = x.bolus_full_date,
        //                val = x.intakes
        //            }).ToList();
        //            TotalIntakes = res.Sum(x => x.val).ToString();

        //            //Check if intakes info exits
        //            if (res.Count == 0)
        //            {
        //                res_json = null;
        //            }
        //            else
        //            {
        //                // Intakes data exits!
        //                var arr_date = context.MeasDatas.Where(x => x.bolus_id == bid_int &&
        //                        (x.bolus_full_date >= dt1 && x.bolus_full_date <= dt1)).Select(
        //                    y => new
        //                    {
        //                        t = y.bolus_full_date.Value
        //                    }).OrderBy(y => y.t).ToList();

        //                DateTime[] arr_dt = new DateTime[arr_date.Count + res.Count];
        //                arr_dt[0] = dt1;
        //                Intakes p = new Intakes();
        //                p.val = 0.0;
        //                foreach (var item in arr_date)
        //                {
        //                    p.arg = item.t;
        //                    res.Add(new { p.arg, p.val });
        //                }

        //                var resOrdered = res.OrderBy(x => x.arg);

        //                res_json = JsonConvert.SerializeObject(resOrdered);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        res_json = null;
        //    }
        //    //return res_json;
        //    Response.Clear();
        //    Response.ContentType = "application/json;charset=UTF-8";
        //    Response.Write(res_json);
        //    Response.End();
        //}


        [WebMethod]
        public void GetIntakesData(string dtfrom, string dtto, string bid)
        {
            DateTime dt1 = DateTime.Parse(dtfrom);
            DateTime dt2 = DateTime.Parse(dtto);
            int bid_int = Convert.ToInt32(bid);
            List<Intakes> intakesList = new List<Intakes>();
            string res_json;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = context.WaterIntakes_sig(dt1, dt2, bid_int, 2).Select(x => new
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
                        var arr_date = context.MeasDatas.Where(x => x.bolus_id == bid_int &&
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

                        //Intakes_Data IData = new Intakes_Data();
                        //IData.li = intakesList;
                        //var tiw = IData.Sum();

                        res_json = JsonConvert.SerializeObject(intakesList);
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


        [WebMethod]
        public static string GetTotalIntakes(string DateFrom, string DateTo, string bid)
        {
            DateTime dt1 = DateTime.Parse(DateFrom);
            DateTime dt2 = DateTime.Parse(DateTo);
            int bid_int = Convert.ToInt32(bid);
            string result = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = context.WaterIntakes_sig(dt1, dt2, bid_int, 2).ToList();
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
        [WebMethod]
        public static string GetAverageTemperature(string DateFrom, string DateTo, string bid)
        {
            DateTime dt1 = DateTime.Parse(DateFrom);
            DateTime dt2 = DateTime.Parse(DateTo);
            int bid_int = Convert.ToInt32(bid);
            string result = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = context.MeasDatas.Where(x => x.bolus_id == bid_int
                                && x.bolus_full_date >= dt1
                                && x.bolus_full_date <= dt2).OrderBy(x => x.temperature).ToList();
                    int indx50 = res.Count / 2;
                    double tup = res[indx50].temperature;
                    double tdw = res[indx50 + 1].temperature;

                    result = ((tup + tdw) / 2).ToString("##.##");
                }
            }
            catch (Exception)
            {
                result = "0";
            }
            return result;
        }

        private void GetGapsData(string dt0, string dt1, string bi_d )
        {
            int bolus_id = Convert.ToInt16(bi_d);
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0);
            DateTime dt_to = DateTime.Parse(dt1);

            List<Data_Gaps> result = new List<Data_Gaps>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var res = (from m in context.MeasDatas
                           join b in context.Bolus on m.bolus_id equals b.bolus_id
                           join fc in context.FarmCows on m.bolus_id equals fc.Bolus_ID
                           where m.bolus_full_date >= dt_from && m.bolus_full_date <= dt_to
                                && m.bolus_id == bolus_id
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

    }
}