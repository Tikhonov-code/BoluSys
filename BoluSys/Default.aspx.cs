using BoluSys.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dashboard = Request.QueryString["dashboard"];
            if (dashboard=="go")
            {
                string user_id = User.Identity.GetUserId();
                if (user_id != null)
                {
                    int farm_id = FindFarmId(user_id);
                    //http://193.27.73.63/BoluSys/Dashboard/dashboard.html?farmId=2
                    string url = "~/Dashboard/dashboard.html?farmId=" + farm_id;
                    Response.Redirect(url);
                }
            }
        }

        private int FindFarmId(string user_id)
        {
            int result = 0;
            try
            {
                using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
                {
                    result = context.Farms.Where(x => x.AspNetUser_Id == user_id).Select(x => new { id = x.id }).Single().id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

    }
}