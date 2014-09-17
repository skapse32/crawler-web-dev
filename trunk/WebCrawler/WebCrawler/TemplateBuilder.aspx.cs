using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core;
using WebCrawler.Core.Info;
using Encoder = System.Text.Encoder;
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
            if (!IsPostBack)
            {
                originalImage.Src = originalImage.Src;
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


        protected void btnCrop_OnClick(object sender, EventArgs e)
        {
            int X1 = Convert.ToInt32(Request.Form["x1"]);
            int Y1 = Convert.ToInt32(Request["y1"]);
            int X2 = Convert.ToInt32(Request.Form["x2"]);
            int Y2 = Convert.ToInt32(Request.Form["y2"]);
            int X = System.Math.Min(X1, X2);
            int Y = System.Math.Min(Y1, Y2);
            int w = Convert.ToInt32(Request.Form["w"]);
            int h = Convert.ToInt32(Request.Form["h"]);

            // That can be any image type (jpg,jpeg,png,gif) from any where in the local server
            bool flag = true;
            string originalFile = "";
            try
            {
                originalFile = Server.MapPath(originalImage.Src);
            }
            catch (Exception ex)
            {
                flag = false;
                originalFile = originalImage.Src;
            }

            var lImageUrlCompleted = (List<string>) ViewState["imagecompleted"];
            var aIndexImageCopleted = lImageUrlCompleted.IndexOf(originalFile);

            Image img = null;
            if (flag)
            {
                img = Image.FromFile(originalFile);
            }
            else
            {
                img = DownloadImageFromUrl(originalFile);
            }
            using (Bitmap _bitmap = new Bitmap(w, h))
            {
                _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = CompositingQuality.HighQuality;
                    _graphic.DrawImage(img, 0, 0, w, h);
                    _graphic.DrawImage(img, new Rectangle(0, 0, w, h), X, Y, w, h, GraphicsUnit.Pixel);


                    if (txtAdditionalText.Value.Trim() != "")
                    {
                        // For Transparent Watermark Text 
                        int opacity = 255; // range from 0 to 255

                        //SolidBrush brush = new SolidBrush(Color.Red);
                        SolidBrush brush = new SolidBrush(Color.FromArgb(opacity, Color.White));
                        Font font = new Font("Arial", 16);
                        _graphic.DrawString(txtAdditionalText.Value, font, brush, new PointF(0, 10));
                    }

                    if (fileUpload.HasFile)
                    {
                        Image logoImage = Image.FromStream(fileUpload.PostedFile.InputStream);
                        logoImage = resizeImage(logoImage, new Size(50, 20));
                        _graphic.DrawImage(logoImage,
                            new Point(w - logoImage.Width - 10, h - logoImage.Height - 10));
                    }


                    string extension = Path.GetExtension(originalFile);
                    string croppedFileName = Guid.NewGuid().ToString();
                    string path = Server.MapPath("~/Upload/");


                    // If the image is a gif file, change it into png
                    if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = ".png";
                    }

                    string newFullPathName = string.Concat(path, croppedFileName, extension);

                    using (EncoderParameters encoderParameters = new EncoderParameters(1))
                    {
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                            value: 100L);
                        _bitmap.Save(newFullPathName, GetImageCodec(extension), encoderParameters);
                    }
                    string filename = string.Concat("/Upload/", croppedFileName + extension);

                    lImageUrlCompleted[aIndexImageCopleted] = filename;
                    ViewState["imagecompleted"] = lImageUrlCompleted;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" <div class='ns-box ns-other ns-effect-thumbslider ns-type-notice ns-hide'>");
                    sb.Append("<div class='ns-box-inner'>");
                    sb.Append("<div class='ns-thumb'>");
                    sb.Append("<img src='" + filename + "'></div>");
                    sb.Append("<div class='ns-content'><p>" + txtAdditionalText.Value + "</p> </div> </div>");
                    sb.Append("<span class='ns-close'></span>");
                    sb.Append("</div>");
                    lblCroppedImage.Text += sb.ToString();

                    string listImageCompleted = "";
                    foreach (var aUrl in lImageUrlCompleted)
                    {
                        listImageCompleted += "|" + aUrl;
                    }
                    imageCompleted.Value = listImageCompleted.Remove(0, 1);
                }
            }

        }

        private Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        /// <summary>
        /// Find the right codec
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetImageCodec(string extension)
        {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.Contains(extension))
                {
                    return codec;
                }
            }
            return codecs[1];
        }

        private Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        protected void btnPrevButton_OnClick(object sender, EventArgs e)
        {
            string Url = ((List<string>)ViewState["imagecompleted"])[int.Parse(imdex.Value)];
            originalImage.Src = Url;
        }

        protected void btnNextButton_OnClick(object sender, EventArgs e)
        {
            string Url = ((List<string>)ViewState["imagecompleted"])[int.Parse(imdex.Value)];
            originalImage.Src = Url;
        }

        protected void btnBegin_OnClick(object sender, EventArgs e)
        {
            btnBegin.Text = "Reset all";
            List<string> lImageUrlCompleted = new List<string>();
            lImageUrlCompleted.AddRange(imagelink.Value.Split('|'));
            ViewState["imagecompleted"] = lImageUrlCompleted;

            btnPrevButton.Enabled = true;
            btnNextButton.Enabled = true;
            originalImage.Src = ((List<string>) ViewState["imagecompleted"])[0];
        }
    }
}