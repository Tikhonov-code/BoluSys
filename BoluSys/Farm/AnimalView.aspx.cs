using BoluSys.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Farm
{
    public partial class AnimalView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user_id = User.Identity.GetUserId();
            string SP = Request.QueryString["SP"];
            switch (SP)
            {
                case "GetAnimalList":
                    GetAnimalList(user_id);
                    break;
                case "GetDataForChart":
                    GetDataForChart(Request.QueryString["dt0"], Request.QueryString["dt1"], Request.QueryString["bid"]);
                    break;
                default:
                    break;
            }
        }

        private void GetDataForChart(string dt0, string dt1, string bid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(connstr);
            SqlCommand cmd = new SqlCommand("SP_GET_DataTempWIntakes", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@dt0", dt0));
            cmd.Parameters.Add(new SqlParameter("@dt1", dt1));
            cmd.Parameters.Add(new SqlParameter("@bolus_id", Convert.ToInt16(bid)));
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

        private void GetAnimalList(string user_id)
        {
            string res_json;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    var res = (from b in context.Bolus
                               join fc in context.FarmCows on b.bolus_id equals fc.Bolus_ID
                               where (fc.AspNetUser_ID == user_id && b.status == true)
                               select new
                               {
                                   bolus_id = b.bolus_id,
                                   animal_id = b.animal_id
                               }
                              ).ToList();
                    res_json = JsonConvert.SerializeObject(res);
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
    }
}