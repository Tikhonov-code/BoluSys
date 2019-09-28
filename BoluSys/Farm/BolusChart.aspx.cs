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
    public partial class BolusChart : System.Web.UI.Page
    {
        public int Bolus_id { get; set; }
        public string Animal_id { get; set; }
        public string DateSearch { get; set; }
        public static string user_id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            Animal_id = Request.QueryString["Animal_id"];
            string SP = Request.QueryString["SP"];
            DateSearch = Request.QueryString["DateSearch"];
            switch (SP)
            {
                case "GetBolusIDList":
                    GetBolusIDList();
                    break;
                case "ShowChart":
                    // ShowChart(Request.QueryString["DateSearch"], Request.QueryString["Animal_id"], Request.QueryString["Bolus_id"]);
                    break;
                default:
                    break;
            }

            string DateFrom = Request.QueryString["DateFrom"];
            string DateTo = Request.QueryString["DateTo"];
            Bolus_id = Convert.ToInt16(Request.QueryString["Bolus_id"]);
            Animal_id = Request.QueryString["Animal_id"];

            Page.DataBind();

            if (DateFrom != null && DateTo != null && Bolus_id != 0)
            {
                GetDataForChart(DateFrom, DateTo, Bolus_id);
            }
        }
        [WebMethod]
        public void GetDataForChart(string DateFrom, string DateTo, int Bolus_id)
        {
            DateTime dt_from = DateTime.Parse(DateFrom);
            DateTime dt_to = DateTime.Parse(DateTo);
            //---------------------
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var result = context.MeasDatas.Where(x => x.bolus_id == Bolus_id &&
                            (x.bolus_full_date >= dt_from && x.bolus_full_date <= dt_to)).Select(
                        y => new
                        {
                            t = y.bolus_full_date.Value,
                            Temperature = y.temperature
                        }).ToList();
                    //----------------------------------------------------
                    List<BolusIDChart> res = new List<BolusIDChart>();
                    foreach (var item in result)
                    {
                        var it = new BolusIDChart();
                        it.t = item.t.ToShortDateString() + " " + item.t.ToShortTimeString();
                        it.Temperature = item.Temperature;
                        res.Add(it);
                        it = null;
                    }
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
                    var result = context.Bolus.Distinct().Select(
                        y => new
                        {
                            bolus_id = y.bolus_id,
                            animal_id = y.animal_id
                        }).Where(x => bolusIdList.Contains(x.bolus_id)).ToArray();
                    //----------------------------------------------------
                    String[] res_array = new String[result.Length];
                    int i = 0;
                    foreach (var item in result)
                    {
                        res_array[i++] = item.animal_id.ToString();
                    }

                    //return res_array;
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
        public void ShowChart(string DateSearch, string Animal_id, string Bolus_id)
        {
            //1. Set dateboxes and selecBox
            //2. create chart
            ;
            string DateFrom = "9/22/2019";
            string DateTo = "9/22/2019 23:00:00";
            GetDataForChart(DateFrom, DateTo, Convert.ToInt16(Bolus_id));
        }
    }
}