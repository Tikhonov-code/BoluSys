using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class json_data : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SP = Request.QueryString["SP"];
            string PARAMS = Request.QueryString["PARAMS"];
            switch (SP)
            {
                case "GetDataFarmStat":
                    GetDataFarmStat(SP, PARAMS);
                    break;
                case "GetDS_bolusListJson":
                    GetDS_bolusListJson();
                    break;
                default:
                    break;
            }

        }
        [WebMethod]
        public void GetDataFarmStat(string SP, string PARAMS)
        {
            //------------------------------------------------------------------------
            DateTime? dt = DateTime.Parse(PARAMS);
            List<SP_GET_FARM_STAT_Result> result;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var userid = User.Identity.GetUserId();
                ;
                var bolusIDarr = context.FarmCows.Where(x => x.AspNetUser_ID == userid).Select(b => b.Bolus_ID).ToArray();
                // result = context.SP_GET_FARM_STAT(dt, 40.0, 32).ToList();
                result = context.SP_GET_FARM_STAT(dt, 40.0, 32).Where(x => bolusIDarr.Contains(x.BOLUS_ID)).ToList();
            }
            var res_json = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }
        [WebMethod]
        public void GetDS_bolusListJson()
        {
            object result ;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var user_id = User.Identity.GetUserId();
                    var bolusIdList = context.FarmCows.Where(x => x.AspNetUser_ID == user_id).Select(bl => bl.Bolus_ID).ToArray();
                    //----------------------------------------------------
                    result = (from bl in context.Bolus
                                   join fc in context.FarmCows on bl.bolus_id equals fc.Bolus_ID
                                   where fc.AspNetUser_ID == user_id
                                   select new
                                   {
                                       bolus_id = bl.bolus_id,
                                       animal_id = bl.animal_id
                                   }
                             ).Distinct().OrderBy(x => x.animal_id).ToList();//ToArray();
                    //----------------------------------------------------
                }
                var res_json = JsonConvert.SerializeObject(result);
                Response.Clear();
                Response.ContentType = "application/json;charset=UTF-8";
                Response.Write(res_json);
                Response.End();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Response.Clear();
                Response.ContentType = "application/json;charset=UTF-8";
                Response.Write(ex.Message);
                Response.End();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DateTime dd = DateTime.Parse("9/6/2019 13:00:00");
            GetDataFarmStat("001", "9/6/2019 13:00:00");

        }
    }
}