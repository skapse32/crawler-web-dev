using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core;
using WebCrawler.Core.Info;
using Image = System.Drawing.Image;

namespace WebCrawler
{
    public partial class TemplateBuilder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserInfo"] == null)
            //{
            //    Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            //}
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
            string filename = DateTime.Now.ToString().Replace("/", "").Replace("-","").Replace(":","") + Session["Index"].ToString();
            var aImageTool = new ImageTool();
            Stream fileLogo = null;
            string title = txtText.Value;
            if (fileUpload.HasFile)
            {
                fileLogo = fileUpload.PostedFile.InputStream;
            }

            Image image = aImageTool.DownloadImageFromUrl(imgContent.ImageUrl);
            int x1 = Convert.ToInt32(X.Value);
            int y1 = Convert.ToInt32(Y.Value);
            int x2 = Convert.ToInt32(X2.Value);
            int y2 = Convert.ToInt32(Y2.Value);
            int x = System.Math.Min(x1, x2);
            int y = System.Math.Min(y1, y2) + 20;
            int w = Convert.ToInt32(W.Value) + 30;
            int h = Convert.ToInt32(H.Value);

            filename = aImageTool.CropImage(image, filename, imgContent.ImageUrl, Server.MapPath("~/Upload/"),
                new Rectangle(x, y, w, h),
                title, fileLogo);
            imgContent.ImageUrl = filename;
            StringBuilder sb = new StringBuilder();
            sb.Append(" <div class='ns-box ns-other ns-effect-thumbslider ns-type-notice ns-hide'>");
            sb.Append("<div class='ns-box-inner'>");
            sb.Append("<div class='ns-thumb'>");
            sb.Append("<img src='" + filename + "'></div>");
            sb.Append("<div class='ns-content'><p>" + txtTitle.Value + "</p> </div> </div>");
            sb.Append("<span class='ns-close'></span>");
            sb.Append("</div>");
            imgResult.Text += sb.ToString();
            var lImageLink = (Dictionary<int, ImagesInfo>)Session["ImageLink"];
            var aImageInfo = new ImagesInfo();
            lImageLink.TryGetValue((int) Session["Index"], out aImageInfo);
            aImageInfo.ImagesLink = filename;
            aImageInfo.Title = txtTitle.Value;
        }

        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            if (Session["ImageLink"] == null)
            {
                Dictionary<int, ImagesInfo> lImageLink = new Dictionary<int, ImagesInfo>();
                var lImageChoose = imagelink.Value.Split('|');
                for (int i = 1; i < lImageChoose.Length; i++)
                {
                    lImageLink.Add(i, new ImagesInfo() {ImagesLink = lImageChoose[i]});
                }
                Session["Index"] = 0;
                Session["ImageLink"] = lImageLink;
            }

            var aImageLink = (Dictionary<int, ImagesInfo>) Session["ImageLink"];
            var aImageInfo = new ImagesInfo();
            if (aImageLink.Count - 1 > (int)Session["Index"])
            {
                Session["Index"] = (int)Session["Index"] + 1;
            }
            else
            {
                Session["Index"] = 0;
            }
            aImageLink.TryGetValue((int)Session["Index"], out aImageInfo);
            imgContent.ImageUrl = aImageInfo.ImagesLink;
        }

        protected void btnPrview_OnClick(object sender, EventArgs e)
        {
            if (Session["ImageLink"] == null)
            {
                Dictionary<int, ImagesInfo> lImageLink = new Dictionary<int, ImagesInfo>();
                var lImageChoose = imagelink.Value.Split('|');
                for (int i = 1; i < lImageChoose.Length; i++)
                {
                    lImageLink.Add(i, new ImagesInfo() {ImagesLink = lImageChoose[i]});
                }
                Session["Index"] = 0;
                Session["ImageLink"] = lImageLink;
            }
             var aImageLink = (Dictionary<int, ImagesInfo>) Session["ImageLink"];
            var aImageInfo = new ImagesInfo();
            if ((int) Session["Index"] == 0)
            {
                Session["Index"] = aImageLink.Count - 1;
            }
            else
            {
                Session["Index"] = (int) Session["Index"] - 1;
            }
            aImageLink.TryGetValue((int)Session["Index"], out aImageInfo);
            imgContent.ImageUrl = aImageInfo.ImagesLink;
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            Session["Index"] = 0;
            Session["ImageLink"] = null;
            imgResult.Text = "";
        }
    }
}