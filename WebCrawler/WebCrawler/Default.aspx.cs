using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using WebCrawler.Core;

namespace WebCrawler
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_OnClick(object sender, EventArgs e)
        {
            getImageFromUrl(TextBox1.Text);
        }

        public void getImageFromUrl(string pUrl)
        {
            HtmlTool ahHtmlTool = new HtmlTool();
            string url = TextBox1.Text;
            string result = "";
            var listImageLink = ahHtmlTool.FetchLinksFromSource(url);
            int i = 0, j = 0;
            foreach (var alink in listImageLink)
            {
                j++;
                i++;
                if (j == 1)
                {
                    result += "<div class='box1'>";
                }
                else if (j == 5)
                {
                    result += "<div class='clear'></div>";
                    result += "</div>";
                    j = 0;
                }
                else
                {
                    result += "<div class='col_1_of_single1 span_1_of_single1'>";
                    result += "<div class='view1 view-fifth1'>";
                    result += "<div class='top_box'>";
                    result += "<div class='m_2'>";
                    result += "<input class='second' name='option2' type='checkbox' id='" + i + "' value='" + alink + "' />";
                    result+="<label class='label2' for='"+ i +"'>Choose image</label>";
                    result += "</div>";
                    result += " <div class='grid_img'><div class='css3'>";
                    result += "<img src='" + alink + "' alt='' />";
                    result += " </div> </div> </div> </div> </div>";
                }
            }
            Literal1.Text = result;
        }
    }

}
