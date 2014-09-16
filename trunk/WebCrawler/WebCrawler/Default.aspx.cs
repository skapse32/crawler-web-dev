using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using WebCrawler.Core;
using Image = System.Drawing.Image;

namespace WebCrawler
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            }
        }

        public void getImageFromUrl(string pUrl)
        {
            HtmlTool ahHtmlTool = new HtmlTool();
            string url = pUrl;
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
                    result += "<label class='label2' for='" + i + "'>Choose image</label>";
                    result += "</div>";
                    result += " <div class='grid_img'><div class='css3'>";
                    result += "<img src='" + alink + "' alt='' />";
                    result += " </div> </div> </div> </div> </div>";
                }
            }
            lblResult.Text = result;
        }
        protected void btnGetImage_OnClick(object sender, EventArgs e)
        {
            getImageFromUrl(txturl.Value);
        }

        protected void btnCropAndSave_OnClick(object sender, EventArgs e)
        {
            var aImageTool = new ImageTool();
            Stream fileLogo = null;
            string title = txtText.Value;
            if (fileUpload.HasFile)
            {
                fileLogo = fileUpload.PostedFile.InputStream;
            }

            Image image = aImageTool.DownloadImageFromUrl(imgContent.ImageUrl);
            int x = X.Value.Trim() != "" ? int.Parse(X.Value) : 0;
            X.Value = "0";
            int y = Y.Value.Trim() != "" ? int.Parse(Y.Value) : 0;
            Y.Value = "0";
            int w = W.Value.Trim() != "" ? int.Parse(W.Value) : image.Width;
            W.Value = "0";
            int h = H.Value.Trim() != "" ? int.Parse(H.Value) : image.Height;
            H.Value = "0";
            //string s = aImageTool.CropAndAddTitle(image, "test", Server.MapPath("~/Upload"), new Rectangle(x, y, w, h),
            //    title, fileLogo);
            //imgResult.Text += "";
        }
    }

}
