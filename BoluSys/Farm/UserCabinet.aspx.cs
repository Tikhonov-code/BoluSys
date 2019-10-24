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
    public partial class UserCabinet : System.Web.UI.Page
    {
        public string ReportAnimalsIdList { get; set; }
        public string AnimalsIdListIni { get; set; }
        public string AlertEmail { get; set; }
        public int AlertTemperatureInd { get; set; }
        public int AlertWaterIntakesInd { get; set; }
        public int AlertOutOfRangeInd { get; set; }
        public int ReportTempChartInd { get; set; }
        public int ReportStatInd { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string user_id = User.Identity.GetUserId();


            //string username = Context.User.Identity.GetUserName();
            if (!IsPostBack)
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var set_par = context.UserCabinets.Where(x => x.AspNetUsersID == user_id).SingleOrDefault();
                    AlertEmail = set_par.Email;
                    AlertTemperatureInd = Convert.ToInt16(set_par.AlertTemperatureInd);
                    AlertWaterIntakesInd = Convert.ToInt16(set_par.AlertWaterIntakesInd);
                    AlertOutOfRangeInd = Convert.ToInt16(set_par.AlertOutOfRangeInd);
                    ReportAnimalsIdList = set_par.ReportAnimalsIdList;
                    ReportTempChartInd = Convert.ToInt16(set_par.ReportTempChartInd);
                    ReportStatInd = Convert.ToInt16(set_par.ReportStatInd);
                }
                AnimalsIdListIni = Get_AnimalsIdListIni();
            }
            string par = Request.QueryString["PARAM"];
            //if (par == "Get_AnimalsIdList")
            //{
            //    Get_AnimalsIdList();
            //}
            switch (par)
            {
                case "Get_AnimalsIdList":
                    Get_AnimalsIdList();
                    break;
                default:
                    break;
            }
        }

        private string Get_AnimalsIdListIni()
        {
            string user_id = User.Identity.GetUserId();
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    // Animals ID list by userid
                    var idlistini = context.UserCabinets.Where(t => t.AspNetUsersID == user_id).Select(x => new { aidlist = x.ReportAnimalsIdList }).DefaultIfEmpty().ToList();
                    string[] aidl = idlistini[0].aidlist.Split(',');
                    int?[] arr = new int?[aidl.Length];
                    for (int i = 0; i < aidl.Length; i++)
                    {
                        arr[i] = Int32.Parse(aidl[i]);
                    }
                    var aidlist = context.Bolus.Where(x => arr.Contains(x.animal_id)).Select(x => new { ID = x.id }).ToList();
                    string result = "[";
                    foreach (var item in aidlist)
                    {
                        result += item.ID + ",";
                    }
                    result = result.Substring(0, result.Length - 1);
                    result += "]";
                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        [WebMethod]
        public void Get_AnimalsIdList()
        {
            string user_id = User.Identity.GetUserId();
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    // Animals ID list by userid
                    var bl = context.FarmCows.Where(t => t.AspNetUser_ID == user_id).Select(x => new { x.Bolus_ID }).ToList();

                    int[] arr = new int[bl.Count];
                    int i = 0;
                    foreach (var item in bl)
                    {
                        arr[i++] = item.Bolus_ID;
                    }

                    var aidlist = context.Bolus.Where(x => arr.Contains(x.bolus_id)).Select(x => new
                    {
                        ID = x.id,
                        Animal_ID = x.animal_id
                    }).ToList();
                    //---------------------------------------------------------------------
                    var res_json = JsonConvert.SerializeObject(aidlist);
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
        public static string UserCabinetSaveSettings(
            string mail,
            int AlertTemp,
            int AlertWaterIntakesInd,
            int AlertOutOfRangeInd,
            int ReportStatInd,
            int ReportTempChartInd,
            string ReportAidList)
        {
            // User ID

            var user_id = HttpContext.Current.User.Identity.GetUserId();
            BoluSys.Models.UserCabinet uc = new BoluSys.Models.UserCabinet();
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    //-----------------------------------------------------
                    var rec = context.UserCabinets.Where(x => x.AspNetUsersID == user_id).Take(1).ToList();
                    rec[0].Email = mail;
                    rec[0].AlertTemperatureInd = Convert.ToBoolean(AlertTemp);
                    rec[0].AlertWaterIntakesInd = Convert.ToBoolean(AlertWaterIntakesInd);
                    rec[0].AlertOutOfRangeInd = Convert.ToBoolean(AlertOutOfRangeInd);
                    rec[0].ReportAnimalsIdList = ReportAidList;
                    rec[0].ReportTempChartInd = Convert.ToBoolean(ReportTempChartInd);
                    rec[0].ReportStatInd = Convert.ToBoolean(ReportStatInd);
                    context.SaveChanges();
                    //var res_json = JsonConvert.SerializeObject(b);
                    //return res_json;
                    return "Done";
                }
            }
            catch (Exception ex)
            {
                return "Failed ex="+ex.InnerException;
            }
           
        }
    }
}