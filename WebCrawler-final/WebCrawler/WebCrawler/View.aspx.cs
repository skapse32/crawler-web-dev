using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebCrawler
{
    public partial class View : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["NewTemplate"] != null)
                {
                    lblView.Text = Session["NewTemplate"].ToString();
                }
                else
                {
                    lblView.Text = "Not create Template";
                }
            }
        }
    }
}