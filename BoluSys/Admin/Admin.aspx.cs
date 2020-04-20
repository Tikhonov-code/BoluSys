﻿using BoluSys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Admin
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SP = Request.QueryString["SP"];
            switch (SP)
            {
                case "GetAlertsData":
                    //GetAlertsData();
                    break;
                case "GetAlertEmailList":
                    GetAlertEmailList(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["userid"]);
                    break;
                case "GetGapsData":
                    GetGapsData(Request.QueryString["dt0"], Request.QueryString["dt1"]);
                    break;
                case "GetCowsLogs":
                    GetCowsLogs();
                    break;
                case "InsertCowsLogs":
                    InsertCowsLogs();
                    break;
                case "RemoveCowsLogs":
                    RemoveCowsLogs();
                    break;
                case "GetAnimalList":
                    GetAnimalList();
                    break;
                case "UpdateCowsLogs":
                    UpdateCowsLogs();
                    break;
                case "GetWiReportData":
                    GetWiReportData(Request.QueryString["dt0"]);
                    break;
                case "TTNRawConvertData":
                    string tz = Request.QueryString["TimeZ"];
                    string rv = Request.QueryString["RawValue"];
                    TTNRawConvertData(tz, rv);
                    break;
                case "GetBolusesSet":
                    GetBolusesSet();
                    break;
                case "UpdateBolusStatus":
                    UpdateBolusStatus();
                    break;
                case "GetDataGapsPercent":
                    GetDataGapsPercent(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["userid"]);
                    break;
                case "GetFarmNameList":
                    GetFarmNameList();
                    break; 
                case "GetTempIntakesData":
                    GetTempIntakesData(Request.QueryString["dt0"], Request.QueryString["dt1"],Convert.ToInt16( Request.QueryString["bid"]));
                    break;
                case "GetBolusList":
                    GetBolusList();
                    break;
                case "GetSmsLogs":
                    GetSmsLogs(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["userid"]);
                    break;
                default:
                    break;
            }
        }

        private void GetSmsLogs(string dt0, string dt1, string userid)
        {
            DateTime? dtfrom = DateTime.Parse(dt0);
            DateTime? dtto = DateTime.Parse(dt1);
            object result = new List<SmsLog>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                result = context.SP_Admin_SMSserviceList(dtfrom, dtto, userid).ToList();

            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetBolusList()
        {
            string res_json;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var bid_list = context.Bolus.Where(s=> s.status == true).Select(x => new {
                        bolus_id = x.bolus_id,
                        animal_id = x.animal_id
                    }).OrderBy(b=>b.animal_id).ToList();
                    res_json = JsonConvert.SerializeObject(bid_list);
                }
            }
            catch (Exception ex)
            {
                res_json = ex.Message;
                res_json = "";
            }

            //var res_json = JsonConvert.SerializeObject(bolus_id_list);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetTempIntakesData(string dt0, string dt1, int bid)
        {
            object farmlist;
            DateTime? dtfrom = DateTime.Parse(dt0);
            DateTime? dtto = DateTime.Parse(dt1);

            try
            {

                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    farmlist = context.SP_Admin_TempIntakesChart(dtfrom, dtto,bid,2).ToList();
                }
            }
            catch (Exception ex)
            {
                farmlist = ex.Message;
            }

            var res_json = JsonConvert.SerializeObject(farmlist);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmNameList()
        {
            object farmlist;
            try
            {

                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    farmlist = context.Farms.ToList();
                }
            }
            catch (Exception)
            {
                farmlist = "No data";
            }

            var res_json = JsonConvert.SerializeObject(farmlist);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void UpdateBolusStatus()
        {
            //using (var reader = new StreamReader(Request.InputStream))
            //{
            //    var content = reader.ReadToEnd();
            //}

            int bid = Convert.ToInt16(Request.QueryString["bolus_id"]);
            bool status = Convert.ToBoolean(Request.QueryString["status"]);
            try
            {
                Bolu xel = new Bolu();
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    xel = context.Bolus.Where(x => x.bolus_id == bid).FirstOrDefault();
                    xel.status = status;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(JsonConvert.SerializeObject("OK"));
            Response.End();
        }

        private void GetBolusesSet()
        {
            string res_json = "";
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var r = (from b in context.Bolus
                             join fc in context.FarmCows on b.bolus_id equals fc.Bolus_ID
                             join f in context.Farms on fc.AspNetUser_ID equals f.AspNetUser_Id
                             select new
                             {
                                 Name = f.Name,
                                 bolus_id = b.bolus_id,
                                 animal_id = b.animal_id,
                                 status = b.status
                             }).ToList();

                    res_json = JsonConvert.SerializeObject(r);
                }
            }
            catch (Exception ex)
            {
                res_json = ex.Message;
            }

            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetWiReportData(string dt)
        {
            DateTime dt0 = DateTime.Parse(dt);
            string res_json = "";
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                context.Database.CommandTimeout = 60;
                var r = context.SP_Admin_WaterIntakesReport(dt0, 7);
                res_json = JsonConvert.SerializeObject(r);
            }
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        [WebMethod]
        public void GetAlertEmailList(string dt0, string dt1,string userid)
        {
            DateTime? dtfrom = DateTime.Parse(dt0);
            DateTime? dtto = DateTime.Parse(dt1);
            string res_json = string.Empty;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    // var res = context.Z_AlertLogs.OrderByDescending(x => x.date_emailsent).ToList();
                    var res = context.SP_Admin_Z_AlertLogs(dtfrom, dtto, userid).ToList();
                    res_json = JsonConvert.SerializeObject(res);
                }
            }
            catch (Exception ex)
            {
                res_json = ex.Message;
            }

            //return res_json;
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetGapsData(string dt0, string dt1)
        {
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0);
            DateTime dt_to = DateTime.Parse(dt1);

            List<Data_Gaps> result = new List<Data_Gaps>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var res = (from m in context.MeasDatas
                           join b in context.Bolus on m.bolus_id equals b.bolus_id
                           join fc in context.FarmCows on m.bolus_id equals fc.Bolus_ID
                           where m.bolus_full_date >= dt_from && m.bolus_full_date <= dt_to && b.status == true
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

        private void GetDataGapsPercent(string dt0, string dt1, string userid)
        {
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0).Date;
            DateTime dt_to = DateTime.Parse(dt1).Date;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(connstr);
            SqlCommand cmd = new SqlCommand("SP_Admin_GapsByFarmHerd", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@dt0", dt_from.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@dt1", dt_to.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@user", userid));
            SqlDataReader rdr = null;
            try
            {
                con.Open();
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            finally
            {
                con.Close();
                rdr.Close();
            }


            var res_json = JsonConvert.SerializeObject(dt);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();

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
        public void GetAnimalList()
        {
            //------------------------------------------------------------------------
            object result;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //var userid = User.Identity.GetUserId();

                result = (from f in context.FarmCows
                          join b in context.Bolus on f.Bolus_ID equals b.bolus_id
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
            int aid = Convert.ToInt32(Request.Form["animal_id"]);
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
        public void GetCowsLogs()
        {
            //------------------------------------------------------------------------
            List<Cows_log> result;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                //var userid = User.Identity.GetUserId();

                result = (from cl in context.Cows_log
                          join b in context.Bolus on cl.animal_id equals b.animal_id
                          join f in context.FarmCows on b.bolus_id equals f.Bolus_ID
                          select cl).ToList<Cows_log>();
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
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

        //TTN Raw Converter------------------------------------------------
        public void TTNRawConvertData(string dtz, string rawvalue)
        {
            DateTime[] time_arr;
            double[] temp_arr;
            //3. convert temperature
            temp_arr = GetTemperatureArr(rawvalue);
            //4. convert time
            time_arr = GetTimePointArr(dtz);
            //5. print results
            string result = string.Empty;
            for (int i = 0; i < time_arr.Length; i++)
            {
                result += "time = " + time_arr[i] + "  temperature =" + temp_arr[i] + "\r\n";
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();

            //return result;
        }
        private double[] GetTemperatureArr(string raw)
        {
            byte[] payload = Convert.FromBase64String(raw);
            int len_payload = payload.Length;
            double[] tp = new double[16];
            for (int i = 0; i < tp.Length; i++)
            {
                if (len_payload < 2 * i + 1) break;
                tp[i] = ((payload[i * 2] << 8) | payload[2 * i + 1]) / 100.0;
            }
            return tp;
        }
        private DateTime[] GetTimePointArr(string time)
        {
            DateTime[] tarr = new DateTime[16];
            // Convert to Toronto Time
            tarr[0] = ConvertZuluToEDT(DateTime.Parse(time));
            for (int i = 1; i < tarr.Length; i++)
            {
                tarr[i] = tarr[0].AddMinutes(-15 * i);
            }
            return tarr;
        }
        private DateTime ConvertZuluToEDT(DateTime dtZ)
        {
            var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var targetTime = TimeZoneInfo.ConvertTime(dtZ, est);
            return targetTime;
        }

    }
}