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

        //public string bid_ext { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bid_ext.Value = Request.QueryString["bid_ext"];
            }
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
                    string bidstr = Request.QueryString["Bolus_id"];
                    bid = (string.IsNullOrEmpty(bidstr)) ? -1 : Convert.ToInt16(bidstr);
                    GetCowInfoSt(bid);
                    break;
                case "GetCowsLogs":
                    GetCowsLogs(Convert.ToInt16(Request.QueryString["Bolus_id"]));
                    break;
                case "InsertCowsLogs":
                    InsertCowsLogs();
                    break;
                case "RemoveCowsLogs":
                    RemoveCowsLogs();
                    break;
                case "UpdateCowsLogs":
                    UpdateCowsLogs();
                    break;
                case "GetAnimalList":
                    GetAnimalList(User.Identity.GetUserId(), Convert.ToInt16(Request.QueryString["Bolus_id"]));
                    break;
                case "GetGapsData":
                    GetGapsData(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Request.QueryString["Bolus_id"]);
                    break;
                case "GetGapsDataValue":
                    GetGapsDataValue(Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Request.QueryString["Bolus_id"]);
                    break;
                case "CowDataSaveUpdate":
                    int bolus_id = Convert.ToInt16(Request.QueryString["bolus_id"]);
                    string Date_of_Birth = Request.QueryString["Date_of_Birth"];
                    string Age_Lactation = Request.QueryString["Age_Lactation"];
                    string Current_Stage_Of_Lactation = Request.QueryString["Current_Stage_Of_Lactation"];
                    string Calving_Due_Date = Request.QueryString["Calving_Due_Date"];
                    string Actual_Calving_Date = Request.QueryString["Actual_Calving_Date"];

                    CowDataSaveUpdate(bolus_id, Date_of_Birth, Age_Lactation, Current_Stage_Of_Lactation, Calving_Due_Date, Actual_Calving_Date);
                    break;
                case "FermerFeedback":
                    bolus_id = Convert.ToInt16(Request.QueryString["bolus_id"]);
                    FermerFeedback(bolus_id);
                    break;
                default:
                    break;
            }

        }

        private void FermerFeedback(int bolus_id)
        {
            List<SP_FermerFeedbackByBolusID_Result> result = new List<SP_FermerFeedbackByBolusID_Result>();
            //---------------------
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    //Temperature--------------------------------------------------------
                    result = context.SP_FermerFeedbackByBolusID(bolus_id).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        [WebMethod]
        public static string CowDataSaveUpdate(int bolus_id, string Date_of_Birth, string Age_Lactation, string Current_Stage_Of_Lactation, string Calving_Due_Date, string Actual_Calving_Date)
        {
            string result = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    DateTime xd = new DateTime();
                    Bolu b_item = new Bolu();
                    //DateTime fields preparation---------------------------------------------
                    if (DateTime.TryParse(Date_of_Birth, out xd))
                    { b_item.Date_of_Birth = xd; }
                    else
                    { b_item.Date_of_Birth = DateTime.Now; }

                    if (DateTime.TryParse(Calving_Due_Date, out xd))
                    { b_item.Calving_Due_Date = xd; }
                    else
                    { b_item.Calving_Due_Date = DateTime.Now; }

                    if (DateTime.TryParse(Actual_Calving_Date, out xd))
                    { b_item.Actual_Calving_Date = xd; }
                    else
                    { b_item.Actual_Calving_Date = DateTime.Now; }
                    //-----------------------------------------------------------------------
                    Int16 al = 0;
                    if (Int16.TryParse(Age_Lactation, out al))
                    { b_item.Age_Lactation = al; }
                    else
                    { b_item.Age_Lactation = 0; }

                    //b_item.Age_Lactation                = Convert.ToInt16(Age_Lactation);
                    b_item.Current_Stage_Of_Lactation = Current_Stage_Of_Lactation;

                    var b_old = context.Bolus.Where(x => x.bolus_id == bolus_id).SingleOrDefault();

                    b_old.Date_of_Birth = b_item.Date_of_Birth;
                    b_old.Age_Lactation = b_item.Age_Lactation;
                    b_old.Current_Stage_Of_Lactation = b_item.Current_Stage_Of_Lactation;
                    b_old.Calving_Due_Date = b_item.Calving_Due_Date;
                    b_old.Actual_Calving_Date = b_item.Actual_Calving_Date;

                    context.SaveChanges();
                    b_item = null;
                    result = "Updated Successfully!";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public static string GetCowInfoSt(int bolus_id)
        {
            string result = string.Empty;
            SP_Farm_CowInfoStByBolus_ID_Result cow_infonull = new SP_Farm_CowInfoStByBolus_ID_Result();
            if (bolus_id == -1) return JsonConvert.SerializeObject(cow_infonull);

            SP_Farm_CowInfoStByBolus_ID_Result cow_info = new SP_Farm_CowInfoStByBolus_ID_Result();
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    cow_info = context.SP_Farm_CowInfoStByBolus_ID(bolus_id).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                cow_info = null;
            }
            if (cow_info == null)
            {
                cow_infonull.Bolus_ID = bolus_id;
                result = JsonConvert.SerializeObject(cow_infonull);
            }
            else
            {
                result = JsonConvert.SerializeObject(cow_info);
            }

            return result;
        }
        //public static string GetCowInfoSt(int bolus_id)
        //{
        //    string result = string.Empty;
        //    Bolu cow_info = new Bolu();
        //    try
        //    {
        //        using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
        //        {
        //            var q = context.Bolus.Where(x => x.bolus_id == bolus_id).Select(x => new
        //            {
        //                Animal_ID = x.animal_id,
        //                Bolus_ID = x.bolus_id,
        //                BirthDate = x.Date_of_Birth,
        //                Current_Lactation = x.Age_Lactation,
        //                Lactation_Stage = x.Current_Stage_Of_Lactation,
        //                Lactation_Day = 0,
        //                x.Calving_Due_Date,
        //                x.Actual_Calving_Date
        //            }).SingleOrDefault();

        //            result = JsonConvert.SerializeObject(q);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var m = ex.Message;
        //        cow_info = null;
        //    }
        //    return result;
        //}

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

        // Administrator Cows Logs
        [WebMethod]
        public int UpdateCowsLogs()
        {
            int id_Update = Convert.ToInt32(Request.QueryString["id"]);
            string Evnt = Request.Form["Event"];
            string Descr = Request.Form["Description"];
            string Event_Date = Request.Form["Event_Date"];

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var cl = context.Cows_log.SingleOrDefault(x => x.id == id_Update);

                if (!string.IsNullOrEmpty(Evnt)) cl.Event = Evnt;
                if (!string.IsNullOrEmpty(Descr)) cl.Description = Descr;
                if (!string.IsNullOrEmpty(Event_Date)) cl.Event_Date = Parse_StringToDateTime(Event_Date);

                context.SaveChanges();

            }
            return id_Update;
        }
        [WebMethod]
        public void GetAnimalList(string user_id, int bid)
        {
            //------------------------------------------------------------------------
            object result;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //var userid = User.Identity.GetUserId();

                result = (from f in context.FarmCows
                          join b in context.Bolus on f.Bolus_ID equals b.bolus_id
                          where f.AspNetUser_ID == user_id && f.Bolus_ID == bid
                          select new
                          {
                              id = f.id,
                              animal_id = b.animal_id
                          }
                          ).ToList();
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
        [WebMethod]
        public int RemoveCowsLogs()
        {
            int idDel = Convert.ToInt32(Request.QueryString["id"]);
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var xraw = context.Cows_log.SingleOrDefault(x => x.id == idDel);
                if (xraw.id != 0)
                {
                    context.Cows_log.Remove(xraw);
                    context.SaveChanges();
                }
            }
            return idDel;
        }

        [WebMethod]
        public int InsertCowsLogs()
        {
            int id_new = 0;
            int aid = Convert.ToInt32(Request.QueryString["animal_id"]);
            string Evnt = Request.Form["Event"];
            string Descr = Request.Form["Description"];

            DateTime Evnt_Date = Parse_StringToDateTime(Request.Form["Event_Date"]);

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                Cows_log cl = new Cows_log();
                cl.animal_id = aid;
                cl.Event = Evnt;
                cl.Description = Descr;
                cl.Event_Date = Evnt_Date;
                context.Cows_log.Add(cl);
                context.SaveChanges();
                id_new = cl.id;
            }
            return id_new;
        }

        [WebMethod]
        public void GetCowsLogs(int bid)
        {
            //------------------------------------------------------------------------
            List<Cows_log> result;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //var userid = User.Identity.GetUserId();

                result = (from cl in context.Cows_log
                          join b in context.Bolus on cl.animal_id equals b.animal_id
                          join f in context.FarmCows on b.bolus_id equals f.Bolus_ID
                          where b.bolus_id == bid
                          select cl).ToList<Cows_log>();
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetGapsData(string dt0, string dt1, string bi_d)
        {
            int bolus_id = Convert.ToInt16(bi_d);
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0);
            DateTime dt_to = DateTime.Parse(dt1);

            //Day Begin---------------------------------------
            Data_Gaps dg_begin = new Data_Gaps();
            dg_begin.bolus_id = bolus_id;
            dg_begin.dt_from = dt_from;
            //------------------------------------------------
            //Day End---------------------------------------
            Data_Gaps dg_end = new Data_Gaps();
            dg_end.bolus_id = bolus_id;
            dg_end.dt_to = dt_to;
            //------------------------------------------------

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
                if (res.Count > 0)
                {


                    double diffinterval = 0;
                    //Day Begin--------------------------------------------------------------------
                    dg_begin.animal_id = res[0].animal_id;
                    dg_begin.dt_to = res[0].bolus_full_date;
                    diffinterval = (dg_begin.dt_to.Value - dg_begin.dt_from.Value).TotalMinutes;

                    dg_begin.interval = String.Format("{0:0.00}", diffinterval);
                    if (diffinterval > 15.5)
                    {
                        result.Add(dg_begin);
                    }
                    //--------------------------------------------------------------------


                    var bid = res.Select(x => new { bid = x.bolus_id }).Distinct().OrderBy(x => x.bid).ToArray();
                    int num_bid = bid.Length;

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
                    //Day End---------------------------------------------------------------------
                    dg_end.animal_id = res[0].animal_id;
                    dg_end.dt_from = res[res.Count - 1].bolus_full_date;
                    //dg_end.dt_to = dt_to;
                    diffinterval = (dg_end.dt_to.Value - dg_end.dt_from.Value).TotalMinutes;

                    dg_end.interval = String.Format("{0:0.00}", diffinterval);
                    if (diffinterval > 15.5)
                    {
                        result.Add(dg_end);
                    }
                    //Day End---------------------------------------------------------------------
                }
                else
                {
                    //result = "";
                }
            }
            //---------------------------------------
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
        private void GetGapsDataValue(string dtfrom, string dtto, string bid)
        {
            DateTime dt1 = DateTime.Parse(dtfrom);
            DateTime dt2 = DateTime.Parse(dtto);
            int bid_int = Convert.ToInt32(bid);
            //DateTime dt1 = DateTime.Parse("2020-03-15");
            //DateTime dt2 = DateTime.Parse("2020-03-15 11:59 PM");
            //int bid_int = Convert.ToInt32(bid);
            string res_json;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = context.SP_GET_BolusDataGaps(dt1, dt2, bid_int).ToList();
                    double gaps = Convert.ToDouble(res[0].Gaps);
                    gaps = (gaps <= 0) ? 0 : gaps;
                    res_json = JsonConvert.SerializeObject(res[0].Points + gaps.ToString());
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



        //public DateTime Parse_StringToDateTime(string dt)
        //{
        //    DateTime Evnt_Date = new DateTime();

        //    //dt = "Sun Nov 24 2019 00:00:00 GMT + 0200(Eastern European Standard Time)";

        //    bool dtct = DateTime.TryParse(dt, out Evnt_Date);
        //    if (!dtct)
        //    {
        //        string date = dt.Substring(4, 11);
        //        string s = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        //        Evnt_Date = (DateTime.TryParse(s, out Evnt_Date)) ? Evnt_Date : DateTime.Now;
        //    }
        //    return Evnt_Date;
        //}
    }
}