using System;
using System.Collections.Generic;
using System.Data;
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
using WebCrawler.Core.Facade;
using WebCrawler.Core.Info;
using Image = System.Web.UI.WebControls.Image;

namespace WebCrawler.Website
{
    public partial class Template : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            }
            if (dtlTemplate.Items.Count == 0)
            {
                originalImage.Src = originalImage.Src;
                ITemplateFacade aTemplateFacade = new TemplateFacade();
                var lTemplateInfos = aTemplateFacade.GetAllTemplateInfos();
                dtlTemplate.DataSource = lTemplateInfos;
                dtlTemplate.DataBind();
                //if (!IsPostBack)
                //{
                //    if (lTemplateInfos.Count != 0)
                //    {
                //        RadioButton rbo = (RadioButton)dtlTemplate.Items[0].FindControl("rboChooser");
                //        rbo.Checked = true;
                //    }
                //}
            }
            
        }

        protected void btnFirstStepNext_OnClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txturl.Value.Trim()))
            {
                txturl.Style.Add("border", "1px solid red");
                return;

            }
            else
            {
                txturl.Style.Add("border", "1px solid #ccc");
                GetImageFromUrl(txturl.Value.Trim());
                fieldStep1.Style.Add("display", "none");
                fieldStep2.Style.Add("display", "block");
                fieldStep3.Style.Add("display", "none");
                fieldStep4.Style.Add("display", "none");
                fieldStep5.Style.Add("display", "none");
            }
        }

        protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
        {
            var ImageInfos = new List<ImagesInfo>();
            if (chkSelectAll.Checked == true)
            {
                for (int i = 0; i < dtlListImage.Items.Count; i++)
                {
                    CheckBox chkCheckBoxItem = (CheckBox)dtlListImage.Items[i].FindControl("chkChooser");
                    Image imgImage = (Image)dtlListImage.Items[i].FindControl("imgImage");
                    chkCheckBoxItem.Checked = true;
                    ImageInfos.Add(new ImagesInfo() { ImagesLink = imgImage.ImageUrl });
                }
                Session["ImageLink"] = ImageInfos;
            }
            else
            {
                for (int i = 0; i < dtlListImage.Items.Count; i++)
                {
                    CheckBox chkCheckBoxItem = (CheckBox)dtlListImage.Items[i].FindControl("chkChooser");
                    chkCheckBoxItem.Checked = false;
                    
                }

                if (Session["ImageLink"] != null)
                {
                    Session["ImageLink"] = null;
                    ImageInfos = null;
                }
            }
            dtlListImageCrop.DataSource = ImageInfos;
            dtlListImageCrop.DataBind();
        }

        protected void chkChooser_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkChoose = (CheckBox)sender;
            var ImageInfos = new List<ImagesInfo>();
            var imgImage = (Image)chkChoose.Parent.FindControl("imgImage");
            var aImageLink = imgImage.ImageUrl;
            if (!chkChoose.Checked)
            {
                if (chkSelectAll.Checked)
                {
                    chkSelectAll.Checked = false;
                }
            }
            if (Session["ImageLink"] != null)
            {
                ImageInfos = (List<ImagesInfo>) Session["ImageLink"];
                if (chkChoose.Checked)
                {
                    var aImageInfo = ImageInfos.Where(x => x.ImagesLink.Trim() == aImageLink.Trim());
                    if (aImageInfo.ToList().Count == 0 || ImageInfos.Count == 0)
                    {
                        ImageInfos.Add(new ImagesInfo() {ImagesLink = aImageLink});
                    }
                }
                else
                {
                    var aImageInfo = ImageInfos.Where(x => x.ImagesLink.Trim() == aImageLink.Trim()).FirstOrDefault();
                    ImageInfos.Remove(aImageInfo);
                }
            }
            else
            {
                ImageInfos.Add(new ImagesInfo() { ImagesLink = aImageLink });
            }
            Session["ImageLink"] = ImageInfos;
           
        }

        protected void btnSecondPrevious_OnClick(object sender, EventArgs e)
        {
            fieldStep1.Style.Add("display", "block");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnSecondNext_OnClick(object sender, EventArgs e)
        {
            var aImageInfos = new List<ImagesInfo>();
            if (Session["ImageLink"] == null)
            {
                for (int i = 0; i < dtlListImage.Items.Count; i++)
                {
                    Image imgImage = (Image)dtlListImage.Items[i].FindControl("imgImage");
                    aImageInfos.Add(new ImagesInfo() { ImagesLink = imgImage.ImageUrl });
                }
                Session["ImageLink"] = aImageInfos;
            }
            else
            {
                aImageInfos = (List<ImagesInfo>)Session["ImageLink"];
            }
            dtlListImageCrop.DataSource = aImageInfos;
            dtlListImageCrop.DataBind();
            if (aImageInfos.Count != 0)
            {
                Session["ImageIndex"] = 0;
                originalImage.Src = aImageInfos[0].ImagesLink;
            }
            
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "block");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "none");
            var aStringBuilder = new StringBuilder();
            aStringBuilder.Append("<script type='text/javascript'>");
            aStringBuilder.Append("$('#Div1').modal('show');");
            aStringBuilder.Append("</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "ModalScript", aStringBuilder.ToString(), false);
        }

        protected void btnPreviousImage_OnClick(object sender, EventArgs e)
        {
            try
            {
                var aImageInfos = new List<ImagesInfo>();
                if (Session["ImageLinkCrop"] != null)
                {
                    aImageInfos = (List<ImagesInfo>)Session["ImageLinkCrop"];
                }
                else
                {
                    aImageInfos = (List<ImagesInfo>)Session["ImageLink"];
                }
                
                if (int.Parse(Session["ImageIndex"].ToString()) < 1)
                {
                    Session["ImageIndex"] = aImageInfos.Count - 1;
                }
                else
                {
                    Session["ImageIndex"] = int.Parse(Session["ImageIndex"].ToString()) - 1;
                }
                string Url = aImageInfos[int.Parse(Session["ImageIndex"].ToString())].ImagesLink;
                originalImage.Src = Url;
            }
            catch (Exception ex)
            {

            }
           
        }

        protected void btnNextImage_OnClick(object sender, EventArgs e)
        {
            try
            {
                var aImageInfos = new List<ImagesInfo>();
                if (Session["ImageLinkCrop"] != null)
                {
                    aImageInfos = (List<ImagesInfo>)Session["ImageLinkCrop"];
                }
                else
                {
                    aImageInfos = (List<ImagesInfo>)Session["ImageLink"];
                }
                Session["ImageIndex"] = int.Parse(Session["ImageIndex"].ToString()) + 1;
                if (int.Parse(Session["ImageIndex"].ToString()) > aImageInfos.Count - 1)
                {
                    Session["ImageIndex"] = 0;
                }
                string Url = aImageInfos[int.Parse(Session["ImageIndex"].ToString())].ImagesLink;
                originalImage.Src = Url;
            }
            catch (Exception ex)
            {

            }
           
        }

        protected void btnThirdPrevious_OnClick(object sender, EventArgs e)
        {
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "block");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnThirdNext_OnClick(object sender, EventArgs e)
        {
            var aListDraggedImage = targetImage.Value.Split('|');
            string aStringImageSource = "";
            string aStringImageTarget = "";
            var aImageInfos = (List<ImagesInfo>)Session["ImageLink"];

            if (aListDraggedImage != null || aListDraggedImage.Length != 0)
            {
                for (int i = 0; i < aListDraggedImage.Length; i++)
                {
                    var aImages =
                        aImageInfos.Where(x => x.ImagesLink.Trim() == aListDraggedImage[i].Trim()).FirstOrDefault();
                    if (aImages != null)
                    {
                        aImageInfos.Remove(aImages);
                        aStringImageTarget += "<div class='sortable-item'>";
                        aStringImageTarget += " <button type='button' class='item-del' onclick='Del(this)'>&times;</button>";
                        aStringImageTarget += "<img width='400' height='150' src='" + aImages.ImagesLink + "' /><input type='text' onmouseout='changr()' placeholder='Enter title here' />";
                        aStringImageTarget += "<br/>";
                        aStringImageTarget += "</div>";
                    }
                }
            }
            ltrTargetItem.Text = aStringImageTarget;
            foreach (var imagesInfo in aImageInfos)
            {
                aStringImageSource += "<div class='sortable-item'>";
                aStringImageSource += " <button type='button' class='item-del' onclick='Del(this)'>&times;</button>";
                aStringImageSource += "<img width='400' height='150' src='" + imagesInfo.ImagesLink + "' /><input type='text' onmouseout='changr()' placeholder='Enter title here' />";
                aStringImageSource += "<br/>";
                aStringImageSource += "</div>";
            }
            ltrSourceItem.Text = aStringImageSource;
            var aStringBuilder = new StringBuilder();
            aStringBuilder.Append("<script type='text/javascript'>");
            aStringBuilder.Append("$('#largeModal').modal('show');");
            aStringBuilder.Append("</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "ModalScript", aStringBuilder.ToString(), false);
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "block");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnFourPrevious_OnClick(object sender, EventArgs e)
        {
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "block");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnFourNext_OnClick(object sender, EventArgs e)
        {
            if (Session["NewTemplate"] == null)
            {
                var lImageCompleted = new List<ImagesInfo>();
                string aTemplateId = "0";
                for (int i = 0; i < dtlTemplate.Items.Count; i++)
                {
                    var chkTemplateChoose = (RadioButton)dtlTemplate.Items[i].FindControl("rboChooser");

                    if (chkTemplateChoose.Checked)
                    {
                        aTemplateId = ((HiddenField)chkTemplateChoose.Parent.FindControl("hdIdTemplate")).Value;
                        break;

                    }
                }

                aTemplateId = ((HiddenField)dtlTemplate.Items[0].FindControl("hdIdTemplate")).Value;

                if (!String.IsNullOrEmpty(aTemplateId) && aTemplateId != "0")
                {
                    ITemplateFacade aTemplateFacade = new TemplateFacade();
                    var aTemplateInfo = aTemplateFacade.GetTemplateInfoById(int.Parse(aTemplateId));
                    var aListDraggedImage = targetImage.Value.Split('|');
                    var aListDraggedTitle = targetTitle.Value.Split('|');
                    string path1 = Server.MapPath("~/" + aTemplateInfo.TemplateContent);
                    string html = File.ReadAllText(path1);
                    string tmphtml = "";
                    var aImagesInfos = (List<ImagesInfo>)Session["ImageLink"];
                    for (int j = 0; j < aListDraggedImage.Length; j++)
                    {
                        var aImageInfo = aImagesInfos.Where(x => x.ImagesLink.Trim() == aListDraggedImage[j].Trim()).FirstOrDefault();

                        if (aImageInfo != null)
                        {
                            if (!String.IsNullOrEmpty(aListDraggedTitle[j]))
                            {
                                aImageInfo.Title = aListDraggedTitle[j];
                            }
                            tmphtml += "<div class='sortable-item' style='width:100%'>";

                            if (!File.Exists(Server.MapPath("~/Upload/") + "/" + aImageInfo.ImagesLink))
                            {
                                System.Drawing.Image img = DownloadImageFromUrl(aImageInfo.ImagesLink);
                                string extension = Path.GetExtension(aImageInfo.ImagesLink);
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
                                    encoderParameters.Param[0] =
                                        new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                                            value: 100L);
                                    img.Save(newFullPathName, GetImageCodec(extension), encoderParameters);
                                }
                                aImageInfo.ImagesLink =
                                    string.Concat("/Upload/", croppedFileName, extension);
                                tmphtml += "<img src='" + aImageInfo.ImagesLink + "' />";

                                lImageCompleted.Add(aImageInfo);
                            }
                            else
                            {
                                tmphtml += "<img src='" + aImageInfo.ImagesLink + "' />";
                                lImageCompleted.Add(aImageInfo);
                            }

                            if (!String.IsNullOrEmpty(aImageInfo.Title))
                            {
                                tmphtml += "<br/><div><span style='text-align:center; font-weight:bold;'>" + aImageInfo.Title + "</span></div>";
                            }
                            tmphtml += "</div>";
                        }
                    }
                    html = html.Replace("<div id='myTemplate'></div>", tmphtml);
                    Session["NewTemplate"] = html;
                    Session["AllImageUsing"] = lImageCompleted;
                }
            }
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "block");
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
            string oldFile = "";
            try
            {
                originalFile = Server.MapPath(originalImage.Src);
                oldFile = Server.MapPath(originalImage.Src);
            }
            catch (Exception ex)
            {
                flag = false;
                originalFile = originalImage.Src;
            }
            var aImageInfos = (List<ImagesInfo>) Session["ImageLink"];
            var aImageInfos1 = (List<ImagesInfo>)Session["ImageLinkCrop"];

            var aImageInfo = aImageInfos.Where(x => x.ImagesLink.Trim() == originalFile).FirstOrDefault();
            var aImageInfo1 = aImageInfos1.Where(x => x.ImagesLink.Trim() == originalFile).FirstOrDefault();
            System.Drawing.Image img = null;
            if (flag)
            {
                img = System.Drawing.Image.FromFile(originalFile);
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
                    aImageInfo.ImagesLink = filename;
                    aImageInfo1.ImagesLink = filename;
                    originalImage.Src = filename;
                    if (File.Exists(oldFile))
                    {
                        File.Delete(oldFile);
                    }
                }
            }
            Session["ImageLinkCrop"] = aImageInfos1;
            Session["ImageLink"] = aImageInfos;
        }

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            var lImageCompleted = new List<ImagesInfo>();
            string aTemplateId = "0";
            bool flag = false;
            for (int i = 0; i < dtlTemplate.Items.Count; i++)
            {
                var chkTemplateChoose = (RadioButton)dtlTemplate.Items[i].FindControl("rboChooser");
               
                if (chkTemplateChoose.Checked)
                {
                    aTemplateId = ((HiddenField)chkTemplateChoose.Parent.FindControl("hdIdTemplate")).Value;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                aTemplateId = "1";
            }
            if (!String.IsNullOrEmpty(aTemplateId) || aTemplateId != "0")
            {
                ITemplateFacade aTemplateFacade = new TemplateFacade();
                var aTemplateInfo = aTemplateFacade.GetTemplateInfoById(int.Parse(aTemplateId));
                var aListDraggedImage = targetImage.Value.Split('|');
                var aListDraggedTitle = targetTitle.Value.Split('|');
                string path1 = Server.MapPath("~/" + aTemplateInfo.TemplateContent);
                string html = File.ReadAllText(path1);
                string tmphtml = "";
                var aImagesInfos = (List<ImagesInfo>)Session["ImageLink"];
                for (int j = 0; j < aListDraggedImage.Length; j++)
                {
                    var aImageInfo = aImagesInfos.Where(x => x.ImagesLink.Trim() == aListDraggedImage[j].Trim()).FirstOrDefault();
                    
                    if (aImageInfo != null)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(aListDraggedTitle[j]))
                            {
                                aImageInfo.Title = aListDraggedTitle[j];
                            }
                        }
                        catch (Exception)
                        {
                        }
                        
                        tmphtml += "<div class='sortable-item' style='width:100%'>";

                        if (!File.Exists(Server.MapPath("~/Upload/") + "/" + aImageInfo.ImagesLink))
                        {
                            System.Drawing.Image img = DownloadImageFromUrl(aImageInfo.ImagesLink);
                            string extension = Path.GetExtension(aImageInfo.ImagesLink);
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
                                encoderParameters.Param[0] =
                                    new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                                        value: 100L);
                                img.Save(newFullPathName, GetImageCodec(extension), encoderParameters);
                            }
                            aImageInfo.ImagesLink =
                                string.Concat("/Upload/", croppedFileName, extension);
                            tmphtml += "<img src='" + aImageInfo.ImagesLink + "' />";

                            lImageCompleted.Add(aImageInfo);
                        }
                        else
                        {
                            tmphtml += "<img src='" + aImageInfo.ImagesLink + "' />";
                            lImageCompleted.Add(aImageInfo);
                        }

                        if (!String.IsNullOrEmpty(aImageInfo.Title))
                        {
                            tmphtml += "<br/><div style='text-align:center;'><span style='font-weight:bold;'>" + aImageInfo.Title + "</span></div>";
                        }
                        tmphtml += "</div>";
                    }
                }
                html = html.Replace("<div id='myTemplate'></div>", tmphtml);
                Session["NewTemplate"] = html;
                Session["AllImageUsing"] = lImageCompleted;
               
                if (Session["NewTemplate"] != null)
                {
                    var aStringBuilder = new StringBuilder();
                    aStringBuilder.Append("<script type='text/javascript'>");
                    aStringBuilder.Append("window.open('View.aspx','_blank');");
                    aStringBuilder.Append("</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "Modal", aStringBuilder.ToString(), false);
                }
            }
        }


        #region Common function

        public string ServerHostName()
        {
            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
            {
                port = "";
            }
            else
            {
                port = ":" + port;
            }

            string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
            {
                protocol = "http://";
            }
            else
            {
                protocol = "https://";
            }

            return protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port;
        }

        private System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest =
                    (System.Net.HttpWebRequest) System.Net.HttpWebRequest.Create(imageUrl);
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

        public void GetImageFromUrl(string pUrl)
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
            dtlListImage.DataSource = dt;
            dtlListImage.DataBind();
        }

        private System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, Size size)
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
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }
        #endregion

        protected void btnFive_OnClick(object sender, EventArgs e)
        {
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "block");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            var aUserInfo = (UserInfo)Session["UserInfo"];
            if (!Directory.Exists(Server.MapPath("~/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value)))
            {
                Directory.CreateDirectory(Server.MapPath("~/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value));
            }
            string PathFile = "";// Path.Combine("~/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value, Guid.NewGuid().ToString() + ".jpg");
            //string FileName = Path.Combine(Server.MapPath(PathFile));
            //var aHtmlTool = new HtmlTool();
            //aHtmlTool.FileName = FileName;
            var html = Session["NewTemplate"].ToString();
            try
            {

                if (!html.Contains("<!DOCTYPE html><html>"))
                {
                    html = "<!DOCTYPE html><html><body>" + html + "</body></html>";
                }

                var lImageCompleted = (List<ImagesInfo>)Session["AllImageUsing"];
                if (lImageCompleted != null)
                {
                    foreach (var imagesInfo in lImageCompleted)
                    {
                        if (File.Exists(Server.MapPath(imagesInfo.ImagesLink)))
                        {
                            string[] tmp = imagesInfo.ImagesLink.Split('/');
                            string path =
                                Path.Combine(Server.MapPath("~/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value),
                                    tmp[tmp.Length - 1]);
                            if (PathFile == "")
                            {
                                PathFile = Path.Combine("~/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value, tmp[tmp.Length - 1]);
                            }

                            File.Move(Server.MapPath(string.Concat("~", imagesInfo.ImagesLink)), path);
                        }
                    }
                }

                var aImageInfos = (List<ImagesInfo>)Session["ImageLink"];

                foreach (var imagesInfo in aImageInfos)
                {
                    if (!imagesInfo.ImagesLink.Contains("http") || !imagesInfo.ImagesLink.Contains(".com"))
                    {
                        if (File.Exists(Server.MapPath(string.Concat("~", imagesInfo.ImagesLink))))
                        {
                            File.Delete(Server.MapPath(string.Concat("~", imagesInfo.ImagesLink)));
                        }
                    }

                }

            }
            catch (Exception exception)
            {

            }
            html = html.Replace("/Upload", ServerHostName() + "/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value);
            //aHtmlTool.StartBrowser(html);
            string folderTemplate = Server.MapPath("~/Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value) +
                                    @"\template.html";
            File.WriteAllText(folderTemplate, html);
            IFolderMediaFacade aFolderMediaFacade = new FolderMediaFacade();
            var aFolderMediaInfo = new FolderMediaInfo();
            aFolderMediaInfo.FolderDateCreate = DateTime.Now;
            aFolderMediaInfo.FolderDescription = txtDescription.Value;
            aFolderMediaInfo.FolderName = "Upload/" + aUserInfo.UserName + "/" + txtFolderName.Value + "/template.html";
            aFolderMediaInfo.UserId = aUserInfo != null ? aUserInfo.UserId : 0;
            aFolderMediaInfo.FolderImage = ServerHostName() + PathFile.Replace("~", "");
            aFolderMediaFacade.Insert(aFolderMediaInfo);

            Session["NewTemplate"] = null;
            Session["ImageIndex"] = null;
            Session["ImageLink"] = null;
            Response.Redirect("Default.aspx");
        }

        protected void rboChooser_OnCheckedChanged(object sender, EventArgs e)
        {
            var aStringBuilder = new StringBuilder();

            for (int i = 0; i < dtlTemplate.Items.Count; i++)
            {
                var chkTemplateChoose = (RadioButton)dtlTemplate.Items[i].FindControl("rboChooser");
                chkTemplateChoose.Checked = false;
            }
            var rbo = (RadioButton) sender;
            rbo.Checked = true;
            
            aStringBuilder.Append("<script type='text/javascript'>");
            aStringBuilder.Append("remove();");
            //aStringBuilder.Append("$('#btnShowPopup').trigger('click');");
            aStringBuilder.Append("</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "ModalScript", aStringBuilder.ToString(), false);
        }

        protected void btnOK_OnClick(object sender, EventArgs e)
        {
            var ImageInfos = new List<ImagesInfo>();
            for (int i = 0; i < dtlListImageCrop.Items.Count; i++)
            {
                CheckBox chkCheckBoxItem = (CheckBox)dtlListImageCrop.Items[i].FindControl("rboChooserCrop");
                
                if (chkCheckBoxItem.Checked)
                {
                    Image imgImage = (Image)dtlListImageCrop.Items[i].FindControl("imgImageTemplateCrop");
                    ImageInfos.Add(new ImagesInfo() { ImagesLink = imgImage.ImageUrl });
                }
                
            }
            if (ImageInfos.Count != 0)
            {
                Session["ImageLinkCrop"] = ImageInfos;
                Session["ImageIndex"] = 0;
                originalImage.Src = ImageInfos[0].ImagesLink;
            }
            else
            {
                ImageInfos = (List<ImagesInfo>) Session["ImageLink"];
                Session["ImageIndex"] = 0;
                originalImage.Src = ImageInfos[0].ImagesLink;
            }
            var aStringBuilder = new StringBuilder();
            aStringBuilder.Append("<script type='text/javascript'>");
            aStringBuilder.Append("remove();");
            //aStringBuilder.Append("$('#btnShowPopup').trigger('click');");
            aStringBuilder.Append("</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "ModalScript", aStringBuilder.ToString(), false);
        }
    }
}