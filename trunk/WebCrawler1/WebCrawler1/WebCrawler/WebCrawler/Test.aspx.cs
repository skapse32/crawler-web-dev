using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core;

namespace WebCrawler
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           getImageFromUrl("http://dantri.com.vn/");

        }

        public void getImageFromUrl(string pUrl)
        {
            HtmlTool ahHtmlTool = new HtmlTool();
            string url = pUrl;
            string result = "";
            var listImageLink = ahHtmlTool.FetchLinksFromSource(url);
            DataTable dt = new DataTable();
            dt.Columns.Add("link");
            foreach (var alink in listImageLink)
            {
                dt.Rows.Add(alink);
            }
            ListView1.DataSource = dt;
            ListView1.DataBind();
        }
    }
}