using BoluSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Services
{
    public partial class DownloadData : System.Web.UI.Page
    {
        //public string DtFrom { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Download_Click(object sender, EventArgs e)
        {
            //begin of day
            DateTime d_from = DateTime.Parse(DtFrom.Value);
            // End of day
            DateTime d_to = DateTime.Parse(DtTo.Value);
            d_to = new DateTime(d_to.Year, d_to.Month, d_to.Day, 23, 59, 59);

            List<MeasData> dl = new List<MeasData>();
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                dl = context.MeasDatas.Where(x => x.bolus_full_date >= d_from && x.bolus_full_date <= d_to).ToList();
            }
            //--------------------------------------------------------
            string file_name = Server.MapPath("~/" + Properties.Settings.Default.DownLoadFile);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file_name))
            {
                foreach (var item in dl)
                {
                    string line = string.Empty;
                    line =  item.id + "," +
                            item.user_id + "," +
                            item.bolus_id + "," +
                            item.animal_id + "," +
                            item.temperature + "," +
                            item.bolus_timestamp + "," +
                            item.bolus_full_date + "," +
                            item.base_station_id + "," +
                            item.is_average + "," +
                            item.raw;
                    sw.WriteLine(line);
                }
            }
            //--------------------------------------------------------
            DownloadFile(Properties.Settings.Default.DownLoadFile);
            // delete file
            System.IO.File.Delete(file_name);

        }
        protected bool DownloadFile(string FileName)
        {
            string path = Server.MapPath("~/" + FileName);
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            string Outgoingfile = FileName;
            if (file.Exists)
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Outgoingfile);
                Response.AddHeader("Content-Length", file.Length.ToString());
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.ContentType = "application/csv";
                Response.WriteFile(file.FullName);
                Response.Flush();
                Response.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}