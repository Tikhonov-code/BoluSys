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

namespace BoluSys.Services
{
    public partial class AdminCowsLogs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SP = Request.QueryString["SP"];
            switch (SP)
            {
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
                default:
                    break;
            }
        }
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
                var userid = User.Identity.GetUserId();

                result = (from f in context.FarmCows
                          join b in context.Bolus on f.Bolus_ID equals b.bolus_id
                          where f.AspNetUser_ID == userid
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
            //string dt = Request.Form["Event_Date"];

            //dt = "Sun Nov 24 2019 00:00:00 GMT + 0200(Eastern European Standard Time)";

            //bool dtct = DateTime.TryParse(dt, out Evnt_Date);
            //if (!dtct)
            //{
            //    string date = dt.Substring(4, 11);
            //    string s = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            //    Evnt_Date = (DateTime.TryParse(s, out Evnt_Date)) ? Evnt_Date :  DateTime.Now ;
            //}


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
                var userid = User.Identity.GetUserId();

                result = (from cl in context.Cows_log
                          join b in context.Bolus on cl.animal_id equals b.animal_id
                          join f in context.FarmCows on b.bolus_id equals f.Bolus_ID
                          where f.AspNetUser_ID == userid
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
    }
}