using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Services
{
    public partial class TTNrawConverter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_ConvertData_Click(object sender, EventArgs e)
        {
            //1. check time
            string tz = txb_TimeZ.Text;
            //2. check raw data
            var raw = txb_Raw.Text;
            //3. convert temperature
            double[] temp_arr = GetTemperatureArr(raw);
            //4. convert time
            DateTime[] time_arr = GetTimePointArr(tz);
            //5. print results
            for (int i = 0; i < time_arr.Length; i++)
            {
                txb_Results.Text += "time = " + time_arr[i] + "  temperature =" + temp_arr[i] +"\r\n";
            }

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