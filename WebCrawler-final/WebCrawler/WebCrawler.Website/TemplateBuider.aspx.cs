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
    public partial class TemplateBuider : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                }
            }
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
            dtlListImage.DataSource = dt;
            dtlListImage.DataBind();
        }

        protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
        {
            var ImageInfos = new List<ImagesInfo>();
            if (chkSelectAll.Checked == true)
            {
                for (int i = 0; i < dtlListImage.Items.Count; i++)
                {
                    CheckBox chkCheckBoxItem = (CheckBox) dtlListImage.Items[i].FindControl("chkChooser");
                    Image imgImage = (Image) dtlListImage.Items[i].FindControl("imgImage");
                    chkCheckBoxItem.Checked = true;
                    ImageInfos.Add(new ImagesInfo() {ImagesLink = imgImage.ImageUrl});
                    Session["ImageLink"] = ImageInfos;
                }
            }
            else
            {
                for (int i = 0; i < dtlListImage.Items.Count; i++)
                {
                    CheckBox chkCheckBoxItem = (CheckBox)dtlListImage.Items[i].FindControl("chkChooser");
                    chkCheckBoxItem.Checked = false;
                    if (Session["ImageLink"] != null)
                    {
                        Session["ImageLink"] = null;
                        ImageInfos = null;
                    }
                }
            }
            dtlImageCanCut.DataSource = ImageInfos;
            dtlImageCanCut.DataBind();
        }

        protected void btnGetImage_OnClick(object sender, EventArgs e)
        {
            Session["ImageLink"] = null;
            getImageFromUrl(txturl.Value);
            btnFirstStepNext.Enabled = true;
        }

        protected void chkChooser_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkChoose = (CheckBox) sender;
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
                        ImageInfos.Add(new ImagesInfo() { ImagesLink = aImageLink });
                    }
                }
                else
                {
                    var aImageInfo = ImageInfos.Where(x => x.ImagesLink.Trim() == aImageLink.Trim()).FirstOrDefault();
                    ImageInfos.Remove(aImageInfo);
                }
            }
            Session["ImageLink"] = ImageInfos;
            dtlImageCanCut.DataSource = ImageInfos;
            dtlImageCanCut.DataBind();
        }

        protected void btnFirstStepNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                var ImageInfos = new List<ImagesInfo>();
                if (Session["ImageLink"] == null)
                {
                    for (int i = 0; i < dtlListImage.Items.Count; i++)
                    {
                        //CheckBox chkCheckBoxItem = (CheckBox) dtlListImage.Items[i].FindControl("chkChooser");
                        Image imgImage = (Image)dtlListImage.Items[i].FindControl("imgImage");
                        //chkCheckBoxItem.Checked = true;
                        ImageInfos.Add(new ImagesInfo() { ImagesLink = imgImage.ImageUrl });
                    }
                    Session["ImageLink"] = ImageInfos;
                }
                else
                {
                    ImageInfos = (List<ImagesInfo>)Session["ImageLink"];
                }
                if (Session["ImageLink"] != null)
                {
                    Session["ImageIndex"] = 0;
                    originalImage.Src = ImageInfos[0].ImagesLink;
                    fieldStep1.Style.Add("display", "none");
                    fieldStep2.Style.Add("display", "block");
                    fieldStep3.Style.Add("display", "none");
                    fieldStep4.Style.Add("display", "none");
                    fieldStep5.Style.Add("display", "none");
                    dtlImageCanCut.DataSource = ImageInfos;
                    dtlImageCanCut.DataBind();
                }
            }
            catch (Exception)
            {
               //select image or get image
            }
            
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
            var aImageInfos1 = (List<ImagesInfo>)Session["ImageCroped"];

            var aImageInfo = aImageInfos.Where(x => x.ImagesLink.Trim() == originalFile).FirstOrDefault();
            var aImageInfo1 = aImageInfos1.Where(x => x.ImagesLink.Trim() == originalFile).FirstOrDefault();
            aImageInfo.Title = txtTitle.Value;
            aImageInfo1.Title = txtTitle.Value;
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

                    if (fileUpload.HasFile)
                    {
                        System.Drawing.Image logoImage = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream);
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

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" <div class='ns-box ns-other ns-effect-thumbslider ns-type-notice ns-hide'>");
                    sb.Append("<div class='ns-box-inner'>");
                    sb.Append("<div class='ns-thumb'>");
                    sb.Append("<img src='" + filename + "'></div>");
                    sb.Append("<div class='ns-content'><p>" + txtAdditionalText.Value + "</p> </div> </div>");
                    sb.Append("<span class='ns-close'></span>");
                    sb.Append("</div>");
                    lblCroppedImage.Text += sb.ToString();
                    aImageInfo.ImagesLink = filename;
                    aImageInfo1.ImagesLink = filename;
                    originalImage.Src = filename;
                    if (File.Exists(oldFile))
                    {
                        File.Delete(oldFile);
                    }
                }
            }

        }

        private System.Drawing.Image DownloadImageFromUrl(string imageUrl)
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

        private System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
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
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "block");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnThirdPrevious_OnClick(object sender, EventArgs e)
        {
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "block");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "none");
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
                        aStringImageTarget += "<img width='400' height='150' src='" + aImages.ImagesLink + "' />";
                        if (!String.IsNullOrEmpty(aImages.Title))
                        {
                            aStringImageTarget += "<br/><span style='color:red'>" + aImages.Title + "</span>";
                        }
                        aStringImageTarget += "</div>";
                    }
                }
            }
            ltrTargetItem.Text = aStringImageTarget;
            foreach (var imagesInfo in aImageInfos)
            {
                aStringImageSource += "<div class='sortable-item'>";
                aStringImageSource += "<img width='400' height='150' src='" + imagesInfo.ImagesLink + "' />";
                if (!String.IsNullOrEmpty(imagesInfo.Title))
                {
                    aStringImageSource += "<br/><span style='color:red'>" + imagesInfo.Title + "</span>";
                }
                aStringImageSource += "</div>";
            }
            ltrSourceItem.Text = aStringImageSource;
            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "block");
            fieldStep5.Style.Add("display", "none");
        }

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            for (int i = 0; i < dtlTemplate.Items.Count; i++)
            {
                var chkTemplateChoose = (CheckBox) dtlTemplate.Items[i].FindControl("chkTemplate");
                var lImageCompleted = new List<ImagesInfo>();
                if (chkTemplateChoose.Checked)
                {
                    var aTemplateId = ((HiddenField) chkTemplateChoose.Parent.FindControl("hdIdTemplate")).Value;
                    if (!String.IsNullOrEmpty(aTemplateId) || aTemplateId != "0")
                    {
                        ITemplateFacade aTemplateFacade = new TemplateFacade();
                        var aTemplateInfo = aTemplateFacade.GetTemplateInfoById(int.Parse(aTemplateId));
                        var aListDraggedImage = targetImage.Value.Split('|');
                        string html = aTemplateInfo.TemplateContent;
                        string tmphtml = "";
                        var aImagesInfos = (List<ImagesInfo>)Session["ImageLink"];
                        for (int j = 0; j < aListDraggedImage.Length; j++)
                        {
                            var aImageInfo = aImagesInfos.Where(x => x.ImagesLink.Trim() == aListDraggedImage[j].Trim()).FirstOrDefault();
                            if (aImageInfo != null)
                            {
                                tmphtml += "<div class='sortable-item' style='width:100%'>";

                                if (!File.Exists(Server.MapPath("~/Upload/") + "/" +aImageInfo.ImagesLink))
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
                                    tmphtml += "<br/><span style='color:red'>" + aImageInfo.Title + "</span>";
                                }
                                tmphtml += "</div>";
                            }
                        }
                        html = html.Replace("<div id='myTemplate'></div>", tmphtml);
                        Session["NewTemplate"] = html;
                        Session["AllImageUsing"] = lImageCompleted;
                    }
                }
            }
            
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
                for (int i = 0; i < dtlTemplate.Items.Count; i++)
                {
                    var chkTemplateChoose = (CheckBox)dtlTemplate.Items[i].FindControl("chkTemplate");
                    var lImageCompleted = new List<ImagesInfo>();
                    if (chkTemplateChoose.Checked)
                    {
                        var aTemplateId = ((HiddenField)chkTemplateChoose.Parent.FindControl("hdIdTemplate")).Value;
                        if (!String.IsNullOrEmpty(aTemplateId) || aTemplateId != "0")
                        {
                            ITemplateFacade aTemplateFacade = new TemplateFacade();
                            var aTemplateInfo = aTemplateFacade.GetTemplateInfoById(int.Parse(aTemplateId));
                            var aListDraggedImage = targetImage.Value.Split('|');
                            string html = aTemplateInfo.TemplateContent;
                            string tmphtml = "";
                            var aImagesInfos = (List<ImagesInfo>)Session["ImageLink"];
                            for (int j = 0; j < aListDraggedImage.Length; j++)
                            {
                                var aImageInfo = aImagesInfos.Where(x => x.ImagesLink.Trim() == aListDraggedImage[j].Trim()).FirstOrDefault();
                                if (aImageInfo != null)
                                {
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
                                        tmphtml += "<br/><span style='color:red'>" + aImageInfo.Title + "</span>";
                                    }
                                    tmphtml += "</div>";
                                }
                            }
                            html = html.Replace("<div id='myTemplate'></div>", tmphtml);
                            Session["NewTemplate"] = html;
                            Session["AllImageUsing"] = lImageCompleted;
                        }
                    }
                }
            }

            fieldStep1.Style.Add("display", "none");
            fieldStep2.Style.Add("display", "none");
            fieldStep3.Style.Add("display", "none");
            fieldStep4.Style.Add("display", "none");
            fieldStep5.Style.Add("display", "block");
        }

        protected void btnBeginCropImage_OnClick(object sender, EventArgs e)
        {
            try
            {
                var ImageInfos = new List<ImagesInfo>();
                if (Session["ImageCroped"] == null)
                {
                    for (int i = 0; i < dtlImageCanCut.Items.Count; i++)
                    {
                        CheckBox chkCheckBoxItem = (CheckBox)dtlImageCanCut.Items[i].FindControl("chkChooserCancut");
                        if (chkCheckBoxItem.Checked)
                        {
                            Image imgImage = (Image)chkCheckBoxItem.Parent.FindControl("imgImageCanCut");
                            //chkCheckBoxItem.Checked = true;
                            ImageInfos.Add(new ImagesInfo() { ImagesLink = imgImage.ImageUrl });
                        }
                    }
                }
                Session["ImageCroped"] = ImageInfos;
                Session["ImageIndex"] = 0;
                originalImage.Src = ImageInfos[0].ImagesLink;
                btnCrop.Enabled = true;
                btnNextImage.Enabled = true;
                PreviousImage.Enabled = true;
            }
            catch (Exception ex)
            {

            }
           
        }

        protected void btnPreviousImage_Click(object sender, EventArgs e)
        {
            try
            {
                var ImageInfos = (List<ImagesInfo>)Session["ImageCroped"];
                if (int.Parse(Session["ImageIndex"].ToString()) < 1)
                {
                    Session["ImageIndex"] = ImageInfos.Count - 1;
                }
                else
                {
                    Session["ImageIndex"] = int.Parse(Session["ImageIndex"].ToString()) - 1;
                }
                string Url = ImageInfos[int.Parse(Session["ImageIndex"].ToString())].ImagesLink;
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
                var ImageInfos = (List<ImagesInfo>)Session["ImageCroped"];
                Session["ImageIndex"] = int.Parse(Session["ImageIndex"].ToString()) + 1;
                if (int.Parse(Session["ImageIndex"].ToString()) > ImageInfos.Count - 1)
                {
                    Session["ImageIndex"] = 0;
                }
                string Url = ImageInfos[int.Parse(Session["ImageIndex"].ToString())].ImagesLink;
                originalImage.Src = Url;
            }
            catch (Exception ex)
            {
                
            }
           
        }

        protected void btnFive_OnClick(object sender, EventArgs e)
        {
            var aListDraggedImages = targetImage.Value.Split('|');
            string aStringImageSource = "";
            string aStringImageTarget = "";
            var aImageInfos = (List<ImagesInfo>)Session["ImageLink"];

            if (aListDraggedImages != null || aListDraggedImages.Length != 0)
            {
                for (int i = 0; i < aListDraggedImages.Length; i++)
                {
                    var aImages =
                        aImageInfos.Where(x => x.ImagesLink.Trim() == aListDraggedImages[i].Trim()).FirstOrDefault();
                    if (aImages != null)
                    {
                        aImageInfos.Remove(aImages);
                        aStringImageTarget += "<div class='sortable-item'>";
                        aStringImageTarget += "<img width='400' height='150' src='" + aImages.ImagesLink + "' />";
                        if (!String.IsNullOrEmpty(aImages.Title))
                        {
                            aStringImageTarget += "<br/><span style='color:red'>" + aImages.Title + "</span>";
                        }
                        aStringImageTarget += "</div>";
                    }
                }
            }
            ltrTargetItem.Text = aStringImageTarget;
            foreach (var imagesInfo in aImageInfos)
            {
                aStringImageSource += "<div class='sortable-item'>";
                aStringImageSource += "<img width='400' height='150' src='" + imagesInfo.ImagesLink + "' />";
                if (!String.IsNullOrEmpty(imagesInfo.Title))
                {
                    aStringImageSource += "<br/><span style='color:red'>" + imagesInfo.Title + "</span>";
                }
                aStringImageSource += "</div>";
            }
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
            aFolderMediaInfo.FolderImage = ServerHostName() +  PathFile.Replace("~","");
            aFolderMediaFacade.Insert(aFolderMediaInfo);

            Session["NewTemplate"] = null;
            Session["ImageIndex"] = null;
            Session["ImageCroped"] = null;
            Session["ImageLink"] = null;
            Response.Redirect("Default.aspx");
        }

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
    }
}