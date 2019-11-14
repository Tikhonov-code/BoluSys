using BoluSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoluSys.Services
{
    public partial class AdminCowsData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetDataFromDB();
            }
        }

        private void GetDataFromDB()
        {
            using (DB_A4A060_csEntities context = new DB_A4A060_csEntities())
            {
                var cowsdata = context.Bolus.ToList();
                GridView1.DataSource = cowsdata;
                GridView1.DataBind();
            }
        }


        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            //Response.Write("<javascript>alert('Done');</javascript>");
        }
    }
}