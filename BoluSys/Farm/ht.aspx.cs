using BoluSys.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class ht : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SP = Request.QueryString["SP"];
            switch (SP)
            {
                case "GetCowsLogs":
                    int aid = Convert.ToInt32(Request.QueryString["Animal_id"]);
                    GetCowsLogs(aid);
                    break;
                default:
                    break;
            }
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
    }
}