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
using System.Windows.Forms;
using WebCrawler.Core;
using WebCrawler.Core.Facade;
using WebCrawler.Core.Info;
using CheckBox = System.Web.UI.WebControls.CheckBox;

namespace WebCrawler.Website
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            }
            if (!IsPostBack)
            {
                ITemplateFacade aTemplateFacade = new TemplateFacade();
                var aTemplateInfos = aTemplateFacade.GetAllTemplateInfos();
                cboTemplate.DataSource = aTemplateInfos;
                cboTemplate.DataTextField = "TemplateName";
                cboTemplate.DataValueField = "TemplateId";
                cboTemplate.DataBind();
                cboTemplate.SelectedIndex = 0;
                page1.Style.Add("display", "block");
                page2.Style.Add("display", "none");
                page3.Style.Add("display", "none");
                page4.Style.Add("display", "none");
                page5.Style.Add("display", "none");
            }
        }

       
        protected void chkChooser_OnCheckedChanged(object sender, EventArgs e)
        {
            var chkImageCheck = (CheckBox)sender;
            var imageSelected = (System.Web.UI.WebControls.Image)chkImageCheck.Parent.FindControl("imgImage");
            if (Session["AllImageLink"] != null)
            {
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLink"];
                var aImageInfo = aImageInfos.Where(x => x.ImagesLink.Trim() == imageSelected.ImageUrl).FirstOrDefault();
                if (chkImageCheck.Checked)
                {
                    if (aImageInfo == null)
                    {
                        aImageInfos.Add(new ImagesInfo() {ImagesLink = imageSelected.ImageUrl});
                        Session["AllImageLink"] = aImageInfos;
                    }
                }
                else
                {
                    if (aImageInfo != null)
                    {
                        aImageInfos.Remove(aImageInfo);
                    }
                }
            }
            else
            {
                var aImageInfos = new List<ImagesInfo>();
                if (chkImageCheck.Checked)
                {
                    aImageInfos.Add(new ImagesInfo() { ImagesLink = imageSelected.ImageUrl });
                    Session["AllImageLink"] = aImageInfos;
                }
            }
            return;
        }

        protected void chkChooserCrop_OnCheckedChanged(object sender, EventArgs e)
        {
            var chkImageCheckCrop = (CheckBox)sender;
            var chkImageCheck = (CheckBox)chkImageCheckCrop.Parent.FindControl("chkChooser");
            var imageSelected = (System.Web.UI.WebControls.Image)chkImageCheckCrop.Parent.FindControl("imgImage");
            if (Session["AllImageLinkCrop"] != null)
            {
                var aImageInfosCrop = (List<ImagesInfo>)Session["AllImageLinkCrop"];
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLink"];
                var aImageInfo = aImageInfos.Where(x => x.ImagesLink.Trim() == imageSelected.ImageUrl).FirstOrDefault();
                if (chkImageCheckCrop.Checked)
                {
                    if (aImageInfo == null)
                    {
                        chkImageCheck.Checked = true;
                        aImageInfos.Add(new ImagesInfo() { ImagesLink = imageSelected.ImageUrl });
                        aImageInfosCrop.Add(new ImagesInfo() { ImagesLink = imageSelected.ImageUrl });
                        Session["AllImageLinkCrop"] = aImageInfosCrop;
                        Session["AllImageLink"] = aImageInfos;
                    }
                }
                else
                {
                    if (aImageInfo != null)
                    {
                        aImageInfosCrop.Remove(aImageInfo);
                        Session["AllImageLinkCrop"] = aImageInfosCrop;
                    }
                }
            }
            else
            {
                var aImageInfos = new List<ImagesInfo>();
                if (chkImageCheckCrop.Checked)
                {
                    aImageInfos.Add(new ImagesInfo() { ImagesLink = imageSelected.ImageUrl });
                    Session["AllImageLinkCrop"] = aImageInfos;
                }
            }
            return;
        }

        protected void btnNextStep2_OnClick(object sender, EventArgs e)
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
                if (dtlListImage.Items.Count != 0)
                {
                    page1.Style.Add("display", "none");
                    page2.Style.Add("display", "block");
                    page3.Style.Add("display", "none");
                    page4.Style.Add("display", "none");
                    page5.Style.Add("display", "none");
                }
                else
                {
                    var aStringBuilder = new StringBuilder();
                    aStringBuilder.Append("<script type='text/javascript'>");
                    aStringBuilder.Append("showMessage('Get image have an problems. Please re-get image from url');");
                    aStringBuilder.Append("</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        "Alert", aStringBuilder.ToString(), false);
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
                    (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
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
            if (pUrl.Contains("www.cars.com"))
            {
                pUrl = pUrl.Replace("overview", "photo");
            }
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

        protected void btnSelectAllImage_OnClick(object sender, EventArgs e)
        {
            if (dtlListImage.Items.Count != 0)
            {
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLink"];
                if (aImageInfos == null || aImageInfos.Count == 0 || aImageInfos.Count < dtlListImage.Items.Count)
                {
                    aImageInfos = new List<ImagesInfo>();
                    for (int i = 0; i < dtlListImage.Items.Count; i++)
                    {
                        var chkSelectCheck = (CheckBox) dtlListImage.Items[i].FindControl("chkChooser");
                        var imageSelected =
                            (System.Web.UI.WebControls.Image) dtlListImage.Items[i].FindControl("imgImage");
                        aImageInfos.Add(new ImagesInfo() {ImagesLink = imageSelected.ImageUrl});
                        chkSelectCheck.Checked = true;
                    }
                }
                else
                {
                    for (int i = 0; i < dtlListImage.Items.Count; i++)
                    {
                        var chkSelectCheck = (CheckBox)dtlListImage.Items[i].FindControl("chkChooser");
                        chkSelectCheck.Checked = false;
                    }
                    aImageInfos = null;
                }
                
                Session["AllImageLink"] = aImageInfos;
            }
        }

        protected void btnCropallImage_OnClick(object sender, EventArgs e)
        {
            if (dtlListImage.Items.Count != 0)
            {
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLinkCrop"];
                if (aImageInfos == null || aImageInfos.Count == 0 || aImageInfos.Count < dtlListImage.Items.Count)
                {
                    aImageInfos = new List<ImagesInfo>();
                    for (int i = 0; i < dtlListImage.Items.Count; i++)
                    {
                        var chkSelectCheck = (CheckBox)dtlListImage.Items[i].FindControl("chkChooserCrop");
                        var imageSelected =
                            (System.Web.UI.WebControls.Image)dtlListImage.Items[i].FindControl("imgImage");
                        aImageInfos.Add(new ImagesInfo() { ImagesLink = imageSelected.ImageUrl });
                        chkSelectCheck.Checked = true;
                    }
                }
                else
                {
                    for (int i = 0; i < dtlListImage.Items.Count; i++)
                    {
                        var chkSelectCheck = (CheckBox)dtlListImage.Items[i].FindControl("chkChooserCrop");
                        chkSelectCheck.Checked = false;
                    }
                    aImageInfos = null;
                }

                Session["AllImageLinkCrop"] = aImageInfos;
            }
        }

        protected void btnNextStep3_OnClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtGalleryDescripton.Value.Trim()) ||
                String.IsNullOrEmpty(txtGalleryName.Value.Trim()))
            {
                if (String.IsNullOrEmpty(txtGalleryDescripton.Value.Trim()))
                {
                    txtGalleryDescripton.Style.Add("border", "1px solid red");
                }
                if (String.IsNullOrEmpty(txtGalleryName.Value.Trim()))
                {
                    txtGalleryName.Style.Add("border", "1px solid red");
                }
            }
            else
            {
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLink"];
                var aImageInfosCrop = (List<ImagesInfo>)Session["AllImageLinkCrop"];
                if (aImageInfos == null || aImageInfos.Count == 0)
                {
                    var aStringBuilder = new StringBuilder();
                    aStringBuilder.Append("<script type='text/javascript'>");
                    aStringBuilder.Append("showMessage('You must selected images before');");
                    aStringBuilder.Append("</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        "Alert", aStringBuilder.ToString(), false);
                }
                else
                {
                    if (aImageInfosCrop.Count == 0 || aImageInfosCrop == null)
                    {
                        page1.Style.Add("display", "none");
                        page2.Style.Add("display", "none");
                        page3.Style.Add("display", "none");
                        page4.Style.Add("display", "block");
                        page5.Style.Add("display", "none");
                    }
                    else
                    {
                        Session["ImageIndex"] = 0;
                        originalImage.Src = aImageInfosCrop[0].ImagesLink;
                        page1.Style.Add("display", "none");
                        page2.Style.Add("display", "none");
                        page3.Style.Add("display", "block");
                        page4.Style.Add("display", "none");
                        page5.Style.Add("display", "none");
                    }
                }
            }
        }

        protected void btnCropNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLinkCrop"];
                var Index = int.Parse(Session["ImageIndex"].ToString()) + 1;
                if (Index > aImageInfos.Count - 1)
                {
                    Index = 0;
                }
                Session["ImageIndex"] = Index;
                string Url = aImageInfos[Index].ImagesLink;
                originalImage.Src = Url;
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCropBack_OnClick(object sender, EventArgs e)
        {
            try
            {
                var aImageInfos = (List<ImagesInfo>)Session["AllImageLinkCrop"];
                var Index = int.Parse(Session["ImageIndex"].ToString()) - 1;
                if (Index < 0)
                {
                    Index = aImageInfos.Count - 1;
                }
                Session["ImageIndex"] = Index;
                string Url = aImageInfos[Index].ImagesLink;
                originalImage.Src = Url;
            }
            catch (Exception ex)
            {

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
            var aImageInfos = (List<ImagesInfo>)Session["AllImageLink"];
            var aImageInfosCrop = (List<ImagesInfo>)Session["AllImageLinkCrop"];

           
            System.Drawing.Image img = null;
            var aImageInfo = new ImagesInfo();
            var aImageInfo1 = new ImagesInfo();

            if (flag)
            {
                img = System.Drawing.Image.FromFile(originalFile);
                aImageInfo = aImageInfos.Where(x => x.ImagesLink.Trim() == originalImage.Src).FirstOrDefault();
                aImageInfo1 = aImageInfosCrop.Where(x => x.ImagesLink.Trim() == originalImage.Src).FirstOrDefault();
            }
            else
            {
                img = DownloadImageFromUrl(originalFile);
                aImageInfo = aImageInfos.Where(x => x.ImagesLink.Trim() == originalFile).FirstOrDefault();
                aImageInfo1 = aImageInfosCrop.Where(x => x.ImagesLink.Trim() == originalFile).FirstOrDefault();
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
            Session["AllImageLinkCrop"] = aImageInfosCrop;
            Session["AllImageLink"] = aImageInfos;
        }

        protected void btnNextStep4_OnClick(object sender, EventArgs e)
        {
            //ltrSourceItem
            if (Session["AllImageLink"] != null)
            {
                var aImageInfos = (List<ImagesInfo>) Session["AllImageLink"];
                if (aImageInfos.Count != 0)
                {
                    string aStringImageTarget = "";
                    foreach (var imagesInfo in aImageInfos)
                    {
                        aStringImageTarget += "<div class='sortable-item'>";
                        aStringImageTarget += " <button type='button' class='item-del' onclick='Del(this)'>&times;</button>";
                        aStringImageTarget += "<img width='400' height='150' src='" + imagesInfo.ImagesLink + "' /><input type='text' class='form-control' onmouseout='changr()' placeholder='Enter title here' />";
                        aStringImageTarget += "<br/>";
                        aStringImageTarget += "</div>";
                    }
                    ltrSourceItem.Text = aStringImageTarget;
                }
                var aStringBuilder = new StringBuilder();
                aStringBuilder.Append("<script type='text/javascript'>");
                aStringBuilder.Append("changr();");
                aStringBuilder.Append("</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    "Alert", aStringBuilder.ToString(), false);
                page1.Style.Add("display", "none");
                page2.Style.Add("display", "none");
                page3.Style.Add("display", "none");
                page4.Style.Add("display", "block");
                page5.Style.Add("display", "none");
            }
        }

        protected void btnPublish_OnClick(object sender, EventArgs e)
        {
            var lImageCompleted = new List<ImagesInfo>();
            var aTemplateId = int.Parse(cboTemplate.SelectedValue);
            ITemplateFacade aTemplateFacade = new TemplateFacade();
            var aTemplateInfo = aTemplateFacade.GetTemplateInfoById(aTemplateId);
            var aListDraggedImage = targetImage.Value.Split('|');
            var aListDraggedTitle = targetTitle.Value.Split('|');
            string path1 = Server.MapPath("~/" + aTemplateInfo.TemplateContent);
            string html = File.ReadAllText(path1);
            html = html.Replace("<!--TitleStatus-->", txtGalleryName.Value);
            html = html.Replace("<!--MainDescription-->", txtGalleryDescripton.Value);
            string tmphtml = "";
            var aImagesInfos = (List<ImagesInfo>)Session["AllImageLink"];
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

                    tmphtml += "<br/>";

                    if (!File.Exists(Server.MapPath("~/") + aImageInfo.ImagesLink))
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
                    html = html.Replace("<!--Image" + j + "-->", tmphtml);
                    tmphtml = "";
                }
            }
            //html = html.Replace("<div id='myTemplate'></div>", tmphtml);

            var aUserInfo = (UserInfo)Session["UserInfo"];
            if (!Directory.Exists(Server.MapPath("~/Galleries/" + aUserInfo.UserName + "/" + txtGalleryName.Value)))
            {
                Directory.CreateDirectory(Server.MapPath("~/Galleries/" + aUserInfo.UserName + "/" + txtGalleryName.Value));
            }
            string PathFile = "";
            try
            {

                if (!html.Contains("<!DOCTYPE html><html>"))
                {
                    html = "<!DOCTYPE html><html><body>" + html + "</body></html>";
                }

                if (lImageCompleted != null)
                {
                    foreach (var imagesInfo in lImageCompleted)
                    {
                        if (File.Exists(Server.MapPath(imagesInfo.ImagesLink)))
                        {
                            string[] tmp = imagesInfo.ImagesLink.Split('/');
                            string path =
                                Path.Combine(Server.MapPath("~/Galleries/" + aUserInfo.UserName + "/" + txtGalleryName.Value),
                                    tmp[tmp.Length - 1]);
                            if (PathFile == "")
                            {
                                PathFile = Path.Combine("~/Galleries/" + aUserInfo.UserName + "/" + txtGalleryName.Value, tmp[tmp.Length - 1]);
                            }

                            File.Move(Server.MapPath(string.Concat("~", imagesInfo.ImagesLink)), path);
                        }
                    }
                }

                var aImageInfos = (List<ImagesInfo>)Session["AllImageLink"];

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
            html = html.Replace("/Upload", ServerHostName() + "/Galleries/" + aUserInfo.UserName + "/" + txtGalleryName.Value);
            //aHtmlTool.StartBrowser(html);
            string folderTemplate = Server.MapPath("~/Galleries/" + aUserInfo.UserName + "/" + txtGalleryName.Value) + "/" + txtGalleryName.Value +
                                    @".html";
            File.WriteAllText(folderTemplate, html);
            IFolderMediaFacade aFolderMediaFacade = new FolderMediaFacade();
            var aFolderMediaInfo = new FolderMediaInfo();
            aFolderMediaInfo.FolderDateCreate = DateTime.Now;
            aFolderMediaInfo.FolderDescription = txtGalleryDescripton.Value;
            aFolderMediaInfo.FolderName = ServerHostName() + "/Galleries/" + aUserInfo.UserName + "/" +
                                          txtGalleryName.Value + "/" + txtGalleryName.Value + ".html";
            aFolderMediaInfo.UserId = aUserInfo != null ? aUserInfo.UserId : 0;
            aFolderMediaInfo.FolderImage = ServerHostName() + PathFile.Replace("~", "");
            aFolderMediaFacade.Insert(aFolderMediaInfo);
            txtUrlPublish.Value = aFolderMediaInfo.FolderName;
            page1.Style.Add("display", "none");
            page2.Style.Add("display", "none");
            page3.Style.Add("display", "none");
            page4.Style.Add("display", "none");
            page5.Style.Add("display", "block");
        }

        protected void btnCopy_OnClick(object sender, EventArgs e)
        {
            Clipboard.SetText(txtUrlPublish.Value);
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            Session["AllImageLinkCrop"] = null;
            Session["AllImageLink"] = null;
            Session["ImageIndex"] = null;
            Response.Redirect("Default.aspx");
        }

        protected void btnIndex_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Galleries.aspx");
        }

        protected void Button5_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Galleries.aspx");
        }
    }
}