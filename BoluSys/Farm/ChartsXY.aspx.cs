using BoluSys.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace BoluSys.Farm
{
    public partial class ChartsXY : System.Web.UI.Page
    {
        public static string user_id { get; set; }
        public int TotalCowsNumberInfo { get; set; }
        public int CowsUnderMonitoring { get; set; }
        public int CowsToCheck { get; set; }
        public int CowsAtRisk { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            user_id = User.Identity.GetUserId();
            GetTotalCountsForDashboard(user_id);
        }

        private void GetTotalCountsForDashboard(string user_id)
        {
            TotalCowsNumberInfo = 0;
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var bolusIdList = context.FarmCows.Where(x => x.AspNetUser_ID == user_id).Select(bl => bl.Bolus_ID).ToArray();
                TotalCowsNumberInfo = bolusIdList.Count();

                DateTime dt_to = DateTime.Now;
                DateTime dt_from = dt_to.AddHours(-12);

                CowsUnderMonitoring = context.MeasDatas.Where(x => x.bolus_full_date >= dt_from && x.bolus_full_date <= dt_to && bolusIdList.Contains(x.bolus_id)).Select(
                    x => new
                    {
                        bl = x.bolus_id
                    }).Distinct().Count();
                ;
            }
            CowsToCheck = 2;
            CowsAtRisk = 3;
        }

        [WebMethod]
        public static string GetDayBolusList(string DateSearch)//, int age)
        {
            // User ID
            DateTime dt = (!string.IsNullOrEmpty(DateSearch)) ? DateTime.Parse(DateSearch) : DateTime.Now;
            DateTime dtStart = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var bolusIdList = (from bl in context.Bolus
                                   join fc in context.FarmCows on bl.bolus_id equals fc.Bolus_ID
                                   join mes in context.MeasDatas on bl.bolus_id equals mes.bolus_id
                                   where fc.AspNetUser_ID == user_id
                                   && mes.bolus_full_date >= dtStart && mes.bolus_full_date <= dtEnd
                                   select new
                                   {
                                       bolus_id = bl.bolus_id,
                                       animal_id = bl.animal_id
                                   }
                             ).Distinct().OrderBy(x => x.animal_id).ToList();
                //-----------------------------------------------------
                if (bolusIdList.Count == 0)
                {
                    return null;
                }
                var res_json = JsonConvert.SerializeObject(bolusIdList);
                return res_json;
            }
        }

        [WebMethod]
        public static ArrayList GetDataAll(string DateSearch)
        {
            DateTime dt = (!string.IsNullOrEmpty(DateSearch)) ? DateTime.Parse(DateSearch) : DateTime.Now;
            DateTime dtStart = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);

            //--------------------------------------------------------------
            //object r = context.ChartsXY_All(dtStart, dtEnd);

            string cnnString = "Data Source=SQL7002.site4now.net;Initial Catalog=DB_A4A060_cs;User Id=DB_A4A060_cs_admin;Password=Nikita13;";

            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChartsXY_All";
            //add any parameters the stored procedure might require
            cmd.Parameters.Add("@dt1", SqlDbType.DateTime).Value = dtStart;
            cmd.Parameters.Add("@dt2", SqlDbType.DateTime).Value = dtEnd;
            cmd.Parameters.Add("@userid", SqlDbType.NVarChar).Value = user_id;

            cnn.Open();

            //string[] header = { "Time", "Bolus#587 ,T ℃", "Bolus#588 ,T ℃", "Bolus#589 ,T ℃", "Bolus#590 ,T ℃", "Bolus#591 ,T ℃" };
            string[] header = GetHeaderForAllChart(dtStart, dtEnd);

            if (header == null) { return null; }

            //-----------------------------------------------------------------------------------------------------------------
            ArrayList rowList = new ArrayList();
            rowList.Add(header);
            SqlDataReader r = cmd.ExecuteReader();
            // Process each result in the result set
            while (r.Read())
            {
                // Create an array big enough to hold the column values
                object[] values = new object[r.FieldCount];
                // Get the column values into the array
                r.GetValues(values);
                // Add the array to the ArrayList
                rowList.Add(values);
            }
            cnn.Close();
            //--------------------------------------------------------------
            //ArrayList rowListFull;
            ;
            if (rowList.Count == 1)
            {
                rowList.Remove(header);
            }
            return rowList;
            //var res_json = JsonConvert.SerializeObject(rowList);
            //return res_json;
        }
        private static string[] GetHeaderForAllChart(DateTime dtStart, DateTime dtEnd)
        {
            //string[] header = { "Time", "Bolus#587 ,T ℃", "Bolus#588 ,T ℃", "Bolus#589 ,T ℃", "Bolus#590 ,T ℃", "Bolus#591 ,T ℃" };
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var bolusIdList = context.FarmCows.Where(x => x.AspNetUser_ID == user_id).Select(bl => bl.Bolus_ID).ToArray();

                var b = context.MeasDatas.Where(a => a.bolus_full_date >= dtStart && a.bolus_full_date <= dtEnd).Select(x => new
                {
                    x.bolus_id,
                    x.animal_id
                    //}).Distinct().OrderBy(d => d.bolus_id).ToList();
                    }).Distinct().Where(bl => bolusIdList.Contains(bl.bolus_id)).OrderBy(d => d.bolus_id).ToList();

                if (b.Count == 0)
                {
                    return null;
                }
                string[] result = new string[b.Count + 1];
                int i = 0;
                result[i++] = "Time";
                foreach (var item in b)
                {
                    //result[i++] = "#" + item.bolus_id.ToString();
                    result[i++] = "#" + item.animal_id.ToString();
                }
                return result;
            }
        }


        [WebMethod]
        public static List<ChartsXY_Result> GetData(string DateSearch, int Bolus_id)
        {
            DateTime dt = (!string.IsNullOrEmpty(DateSearch)) ? DateTime.Parse(DateSearch) : DateTime.Now;

            DateTime dtStart = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            float Tmin = Properties.Settings.Default.Tmin;
            float Tmax = Properties.Settings.Default.Tmax;

            try
            {
                List<ChartsXY_Result> r = new List<ChartsXY_Result>();
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    r = context.ChartsXY(dtStart, dtEnd, Bolus_id, Tmin, Tmax).ToList();
                    return r;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        [WebMethod]
        public static string GetIndividualDescription(string Bolus_id)
        {
            string result = string.Empty;
            Int32 bid = 0;
            if (Int32.TryParse(Bolus_id, out bid))
            {

                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    result = context.Bolus.Where(x => x.bolus_id == bid).FirstOrDefault().AnimalInfo;
                }
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
    }
}