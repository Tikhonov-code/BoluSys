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

namespace BoluSys.Admin
{
    public partial class IntakesData : System.Web.UI.Page
    {
        public static string user_id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(user_id)) return;

            string SP = Request.QueryString["SP"];
            switch (SP)
            {
                case "CheckIntakes":
                    CheckIntakes(user_id, Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["bid"]);
                    break; 
                case "GetBolusList":
                    GetBolusList();
                    break;          
            default:
                    break;
            }
        }

        public void GetBolusList()
        {
            string res_json = string.Empty;
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
                                      animal_id = bl.animal_id.Value
                                  }
                             ).Distinct().OrderBy(x => x.animal_id).ToArray();
                    //----------------------------------------------------
                    res_json = JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine(ex.Message);
                res_json = JsonConvert.SerializeObject(ex.Message);                
            }
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write(res_json);
            Response.End();
        }

        public void CheckIntakes(string user_id, string v1, string v2, string bid)
        {
            Response.Clear();
            Response.ContentType = "application/json;charset=UTF-8";
            Response.Write("result initial "+ user_id+" "+v1+" "+v2+" "+bid);
            Response.End();
        }
    }
}