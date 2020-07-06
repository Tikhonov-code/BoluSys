using BoluSys.Models;
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
using System.Web.Script.Services;
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
                    //dt0=' + dt0 + '&dt1=' + dt1 + "&userid=" + userid + "&lactat=" + lactat + "&bid=" + bid)

                    GetDataGapsPercent(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["userid"], Request.QueryString["lactat"], Request.QueryString["bid"]);
                    break;
                case "GetDataGapsMap":
                    //+ "&lactat=" + lactat + "&bid=" + bid
                    int bid = Convert.ToInt16(Request.QueryString["bid"]);
                    string lactat = Request.QueryString["lactat"];
                    GetDataGapsMap(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["userid"], lactat, bid);
                    break;
                case "GetBolusesSet_GapsMap":
                    GetBolusesSet_GapsMap(Request.QueryString["userid"]);
                    break;
                case "GetFarmNameList":
                    GetFarmNameList();
                    break;
                case "GetTempIntakesData":
                    GetTempIntakesData(Request.QueryString["dt0"], Request.QueryString["dt1"], Convert.ToInt16(Request.QueryString["bid"]));
                    break;
                case "GetBolusList":
                    GetBolusList();
                    break;
                case "GetSmsLogs":
                    GetSmsLogs(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["userid"]);
                    break;
                case "GetFarmInfo":
                    GetFarmInfo();
                    break;
                case "SaveFarmInfo":
                    SaveFarmInfo(Request.QueryString["data"]);
                    break;
                case "GetFarmPhoneList":
                    GetFarmPhoneList(Request.QueryString["action"], Request.QueryString["userid"]);
                    break;
                case "GetFarmEmailList":
                    GetFarmEmailList(Request.QueryString["action"], Request.QueryString["userid"]);
                    break;
                default:
                    break;
            }
        }
        //------------------------------------------------------------------------------
        private void GetFarmPhoneList(string action, string userid)
        {
            switch (action)
            {
                case "load":
                    GetFarmPhoneList_load(userid);
                    break;
                case "insert":
                    GetFarmPhoneList_insert(userid);
                    break;
                case "remove":
                    GetFarmPhoneList_remove();
                    break;
                case "update":
                    GetFarmPhoneList_update();
                    break;
                default:
                    break;
            }

        }

        private void GetFarmPhoneList_update()
        {
            int id = Convert.ToInt16(Request.Params["rec_id"]);
            string phone_new = Request.Params["Phone"];

            FarmPhone fp_updated = new FarmPhone();

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                fp_updated = context.FarmPhones.Where(x => x.id == id).SingleOrDefault();
                fp_updated.Phone = (phone_new != null) ? phone_new : fp_updated.Phone;
                fp_updated.Status = (Request.Params["Status"] != null) ? Convert.ToBoolean(Request.Params["Status"]) : fp_updated.Status; ;

                context.SaveChanges();
            }
            string res_json = JsonConvert.SerializeObject(fp_updated);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmPhoneList_remove()
        {
            int id = Convert.ToInt16(Request.Params["rec_id"]);
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var item = context.FarmPhones.Where(x => x.id == id).SingleOrDefault();
                context.FarmPhones.Remove(item);
                context.SaveChanges();
            }
            string res_json = JsonConvert.SerializeObject("OK");
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmPhoneList_insert(string userid)
        {
            FarmPhone fp = new FarmPhone();
            fp.AspNetUser_ID = userid;
            fp.Phone = Request.Params["Phone"];
            fp.Status = Convert.ToBoolean(Request.Params["Status"]);

            FarmPhone fp_insert = new FarmPhone();

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                context.FarmPhones.Add(fp);
                context.SaveChanges();
                //----------------------------
                fp_insert = context.FarmPhones.Where(x => x.AspNetUser_ID == userid
                                                      && x.Phone == fp.Phone
                                                      && x.Status == fp.Status
                ).SingleOrDefault();
            }
            string res_json = JsonConvert.SerializeObject(fp_insert);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmPhoneList_load(string userid)
        {
            List<FarmPhone> fp = new List<FarmPhone>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                fp = context.FarmPhones.Where(x => x.AspNetUser_ID == userid).ToList();
            }
            string res_json = JsonConvert.SerializeObject(fp);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
        //------------------------------------------------------------------------------

        private void GetFarmEmailList(string action, string userid)
        {
            switch (action)
            {
                case "load":
                    GetFarmEmailList_load(userid);
                    break;
                case "insert":
                    GetFarmEmailList_insert(userid);
                    break;
                case "remove":
                    GetFarmEmailList_remove();
                    break;
                case "update":
                    GetFarmEmailList_update();
                    break;
                default:
                    break;
            }
        }
        private void GetFarmEmailList_update()
        {
            int id = Convert.ToInt16(Request.Params["rec_id"]);
            string email_new = Request.Params["Email"];

            FarmEmail fp_updated = new FarmEmail();

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                fp_updated = context.FarmEmails.Where(x => x.id == id).SingleOrDefault();
                fp_updated.Email = (email_new != null) ? email_new : fp_updated.Email;
                fp_updated.Status = (Request.Params["Status"] != null) ? Convert.ToBoolean(Request.Params["Status"]) : fp_updated.Status; ;

                context.SaveChanges();
            }
            string res_json = JsonConvert.SerializeObject(fp_updated);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmEmailList_remove()
        {
            int id = Convert.ToInt16(Request.Params["rec_id"]);
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var item = context.FarmEmails.Where(x => x.id == id).SingleOrDefault();
                context.FarmEmails.Remove(item);
                context.SaveChanges();
            }
            string res_json = JsonConvert.SerializeObject("OK");
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmEmailList_insert(string userid)
        {
            FarmEmail fp = new FarmEmail();
            fp.AspNetUser_ID = userid;
            fp.Email = Request.Params["Email"];
            fp.Status = Convert.ToBoolean(Request.Params["Status"]);

            FarmEmail fp_insert = new FarmEmail();

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                context.FarmEmails.Add(fp);
                context.SaveChanges();
                //----------------------------
                fp_insert = context.FarmEmails.Where(x => x.AspNetUser_ID == userid
                                                      && x.Email == fp.Email
                                                      && x.Status == fp.Status
                ).SingleOrDefault();
            }
            string res_json = JsonConvert.SerializeObject(fp_insert);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private void GetFarmEmailList_load(string userid)
        {
            List<FarmEmail> fp = new List<FarmEmail>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                fp = context.FarmEmails.Where(x => x.AspNetUser_ID == userid).ToList();
            }
            string res_json = JsonConvert.SerializeObject(fp);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
        //---------------------------------------------------------------------------------------

        [WebMethod]
        private void SaveFarmInfo(string data)
        {
            string res_json = string.Empty;
            List<FarmRules> farminfo = ConvertDataToFarmRules(data);

            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    string farmname = farminfo[0].Name;
                    var user_id = context.Farms.Where(x => x.Name == farmname).SingleOrDefault().AspNetUser_Id.ToString();
                    var frec = context.Farm_Alert_Rules.Where(x => x.AspNetUser_Id == user_id).ToList();
                    foreach (var item in frec)
                    {
                        var smsemailbits = farminfo.Where(x => x.Alert_Name == item.Alert_Name).SingleOrDefault();
                        if (smsemailbits != null)
                        {
                            item.sms = smsemailbits.sms_s;
                            item.email = smsemailbits.email_s;
                        }
                        ;
                    }

                    context.SaveChanges();
                    res_json = JsonConvert.SerializeObject("OK");

                }
            }
            catch (Exception ex)
            {
                res_json = JsonConvert.SerializeObject(ex.Message);
            }

            res_json = JsonConvert.SerializeObject("OK");
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        private List<FarmRules> ConvertDataToFarmRules(string xd)
        {
            List<FarmRules> result = new List<FarmRules>();
            string[] xdd = xd.Split(',');
            string xdd_Name = GetValueOfField(xdd[0]);
            string xdd_Owner = GetValueOfField(xdd[1]);
            string xdd_geoPosition = GetValueOfField(xdd[2]);
            string xdd_Phone = GetValueOfField(xdd[3]);
            string xdd_Email = GetValueOfField(xdd[4]);


            Boolean Q405_SMS = GetValueOfFieldBool(xdd[5]);
            Boolean Q405_email = GetValueOfFieldBool(xdd[6]);
            Boolean Q41_SMS = GetValueOfFieldBool(xdd[7]);
            Boolean Q41_email = GetValueOfFieldBool(xdd[8]);
            Boolean WI20_SMS = GetValueOfFieldBool(xdd[9]);
            Boolean WI20_email = GetValueOfFieldBool(xdd[10]);

            FarmRules r_Q405 = new FarmRules();
            r_Q405.Alert_Name = "Q40.5";
            r_Q405.Name = xdd_Name;
            r_Q405.Owner = xdd_Owner;
            //-----------------------------------
            //string xy = "-80.4156195 43.5243177";
            //System.Data.Entity.Spatial.DbGeography geox = System.Data.Entity.Spatial.DbGeography.FromText(xy);
            //r_Q405.SetGeoPosition(geox);
            //-----------------------------------
            r_Q405.Phone = xdd_Phone;
            r_Q405.Email = xdd_Email;
            //---------------------------------------
            FarmRules r_Q41 = new FarmRules();
            r_Q41.Alert_Name = "Q41";
            r_Q41.Name = xdd_Name;
            r_Q41.Owner = xdd_Owner;
            r_Q41.Phone = xdd_Phone;
            r_Q41.Email = xdd_Email;
            //---------------------------------------
            FarmRules r_WI20 = new FarmRules();
            r_WI20.Alert_Name = "WI20";
            r_WI20.Name = xdd_Name;
            r_WI20.Owner = xdd_Owner;
            r_WI20.Phone = xdd_Phone;
            r_WI20.Email = xdd_Email;


            //----------------Q405 settings--------------------------
            r_Q405.sms_s = Q405_SMS;
            r_Q405.email_s = Q405_email;
            //----------------Q41 settings---------------------------
            r_Q41.sms_s = Q41_SMS;
            r_Q41.email_s = Q41_email;
            //----------------WI20 settings--------------------------
            r_WI20.sms_s = WI20_SMS;
            r_WI20.email_s = WI20_email;
            //--------------------------------------------------------

            result.Add(r_Q405);
            result.Add(r_Q41);
            result.Add(r_WI20);
            return result;
        }

        private string GetValueOfField(string str)
        {
            string[] str0 = str.Split(':');
            string result = str0[1].Replace("\"", "").Replace("}", "").Trim();
            return result;
        }
        private bool GetValueOfFieldBool(string str)
        {
            bool b_result = false;
            string[] str0 = str.Split(':');
            string result = str0[1].Replace("\"", "").Replace("}", "").Trim();
            if (result == "0" || result == "false")
            {
                b_result = false;
            }
            if (result == "1" || result == "true")
            {
                b_result = true;
            }

            return b_result;
        }

        private void GetFarmInfo()
        {
            string result = string.Empty;
            List<FarmRules> qfr = new List<FarmRules>();

            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var q = (from f in context.Farms
                         join a in context.Farm_Alert_Rules on f.AspNetUser_Id equals a.AspNetUser_Id
                         join u in context.AspNetUsers on f.AspNetUser_Id equals u.Id
                         select new
                         {
                             userid = f.AspNetUser_Id,
                             Name = f.Name,
                             Owner = f.Owner,
                             geoPosition = f.GeoPosition,
                             Phone = u.PhoneNumber,
                             Email = u.Email,
                             Alert_Name = a.Alert_Name,
                             sms_s = a.sms,
                             email_s = a.email
                         }).ToList();

                foreach (var item in q)
                {
                    FarmRules fr = new FarmRules();
                    fr.userid = item.userid;
                    fr.Name = item.Name;
                    fr.Owner = item.Owner;
                    fr.geoPosition = item.geoPosition;
                    fr.Phone = item.Phone;
                    fr.Email = item.Email;
                    fr.Alert_Name = item.Alert_Name;
                    fr.sms_s = item.sms_s;
                    fr.email_s = item.email_s;
                    qfr.Add(fr);
                }
            }
            result = ConvertToFAlist(qfr);
            //var res_json = JsonConvert.SerializeObject(q);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            //Response.Write(res_json);
            Response.Write(result);
            Response.End();
        }

        private string ConvertToFAlist(List<FarmRules> qfr)
        {
            string result = "[";
            var farms = qfr.Select(x => x.Name).Distinct().ToList();

            foreach (var item in farms)
            {
                var xf = qfr.Where(t => t.Name == item).Distinct().ToList();
                //1.-------------------------------------
                result += "{"
                                + "\"userid\": \"" + xf[0].userid + "\","
                                + "\"Name\": \"" + xf[0].Name + "\","
                                + "\"Owner\": \"" + xf[0].Owner + "\","
                                + "\"GeoPosition\": \"" + xf[0].geoPosition + "\","
                                + "\"Phone\": \"" + xf[0].Phone + "\","
                                + "\"Email\": \"" + xf[0].Email + "\",";
                //2. Get rules list-----------------------------------------
                var rules = qfr.Where(t => t.Name == item).Select(x => new
                {
                    Alert_Name = x.Alert_Name,
                    sms_s = x.sms_s,
                    email_s = x.email_s
                }).ToList();
                //3. Add rules----------------------------------------------

                //PropertyInfo[] props = fieldsType.GetProperties(BindingFlags.Public
                //    | BindingFlags.Instance);
                foreach (var itemR in rules)
                {
                    string an = itemR.Alert_Name.Replace(".", "");
                    result += "\"" + an + "_SMS\":" + Convert.ToInt16(itemR.sms_s).ToString() + ","
                            + "\"" + an + "_email\":" + Convert.ToInt16(itemR.email_s).ToString() + ",";
                }
                result = result.Substring(0, result.Length - 1);
                result += "},";
                ;
            }
            //----------------------------------------------
            result = result.Substring(0, result.Length - 1);
            result += "]";
            return result;
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
                    var bid_list = context.Bolus.Where(s => s.status == true).Select(x => new
                    {
                        bolus_id = x.bolus_id,
                        animal_id = x.animal_id
                    }).OrderBy(b => b.animal_id).ToList();
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
                    farmlist = context.SP_Admin_TempIntakesChart(dtfrom, dtto, bid, 2).ToList();
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
                var x = ex.Message;
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
        private void GetBolusesSet_GapsMap(string user_id)
        {
            string res_json = "";
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var r = (from b in context.Bolus
                             join fc in context.FarmCows on b.bolus_id equals fc.Bolus_ID
                             join f in context.Farms on fc.AspNetUser_ID equals f.AspNetUser_Id
                             where f.AspNetUser_Id == user_id
                             select new
                             {
                                 Name = f.Name,
                                 bolus_id = b.bolus_id,
                                 animal_id = b.animal_id,
                                 status = b.status
                             }).OrderBy(x => x.animal_id).ToList();
                    //----------add item bolus_id = Any-----------------------
                    //{\"Name\":\"Markhill Farm\",\"bolus_id\":97,\"animal_id\":568,\"status\":true}
                    //--------------------------------------------------------
                    res_json = JsonConvert.SerializeObject(r);
                    res_json = "[{\"Name\":\"Any Farm\",\"bolus_id\":0,\"animal_id\":\"Any\",\"status\":true}," + res_json.Substring(1, res_json.Length - 1);
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
        public void GetAlertEmailList(string dt0, string dt1, string userid)
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

        private void GetDataGapsPercent(string dt0, string dt1, string userid, string lactat, string bid)
        {
            int bolus_id = Convert.ToInt16(bid);
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0).Date;
            DateTime dt_to = DateTime.Parse(dt1).Date;

            ///DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(connstr);
            SqlCommand cmd = new SqlCommand("SP_Admin_GapsByFarmHerdlacbid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@dt0", dt_from.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@dt1", dt_to.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@user", userid));
            cmd.Parameters.Add(new SqlParameter("@lactat", lactat));
            cmd.Parameters.Add(new SqlParameter("@bid", bolus_id));
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
                dt = null;
            }
            finally
            {
                con.Close();
                if (rdr != null) rdr.Close();
            }
            string todaydate = DateTime.Now.Date.ToShortDateString();
            string res_json = "[{\"bolus_id\":0,\"animal_id\":0}]";//,\""+todaydate+"\":0}]";
            if (dt != null)
            {
                res_json = JsonConvert.SerializeObject(dt);
            }

            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();

        }

        private void GetDataGapsMap(string dt0, string dt1, string userid, string lactat, int bid)
        {
            //Wed Jan 15 2020 10:23:00 GMT 0200 (Eastern European Standard Time)
            DateTime dt_from = DateTime.Parse(dt0.Substring(0, 10)).Date;
            DateTime dt_to = DateTime.Parse(dt1.Substring(0, 10)).Date;
            int days;
            if (dt_from.Date >= dt_to.Date)
            {
                days = 1;
            }
            else
            {
                days = Convert.ToInt16((dt_to - dt_from).TotalDays);
            }

            int time_interval = 1;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(connstr);
            SqlCommand cmd = new SqlCommand("SP_Admin_GapsDHMap", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@dt", dt_to.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@day", days));
            cmd.Parameters.Add(new SqlParameter("@time_interval", time_interval));
            cmd.Parameters.Add(new SqlParameter("@user_id", userid));

            string lacstr = "";

            if (lactat != "")
            {
                lacstr = "\'\'" + lactat.Replace("_", "\'\',\'\'");
                lacstr = lacstr.Substring(0, lacstr.Length - 3) + "\'\'";
            }
            else
            {
                lacstr = "NoChoice";
            }
            if (lacstr.IndexOf(',') == -1)
            {
                lacstr = lacstr.Replace("''", "");
            }
            cmd.Parameters.Add(new SqlParameter("@lactat", lacstr));

            cmd.Parameters.Add(new SqlParameter("@bid", bid));
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
                dt = null;
            }
            finally
            {
                con.Close();
                if (rdr != null) rdr.Close();
            }

            if (dt == null)
            {
                Response.Clear();
                Response.ContentType = "application/json;charset=UTF-8";
                Response.Write(JsonConvert.SerializeObject(""));
                Response.End();
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