using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core.Facade;
using WebCrawler.Core.Info;

namespace WebCrawler.Website
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            }
        }

        protected void btnUpload_OnClick(object sender, EventArgs e)
        {
            var aTemplateInfo = new TemplateInfo();

            string path = "";
            string filename = "";
            string Link ="Template/" + DateTime.Now.ToShortDateString().Replace("/", "").Replace("-", "") + "/" +
                          Guid.NewGuid();
            if (!Directory.Exists(Server.MapPath("~/") + Link))
            {
                path = Server.MapPath("~/") + Link;
                Directory.CreateDirectory(path);
            }
            else
            {
                path = Server.MapPath("~/") + Link;
            }
            if (fileUpload1.HasFile)
            {
                filename = path + "/" + fileUpload1.FileName;
                if (File.Exists(filename))
                {
                    filename = path + "/" + DateTime.Now.Minute + fileUpload1.FileName;
                    fileUpload1.PostedFile.SaveAs(filename);
                    aTemplateInfo.templateImageCover = Link + "/" + DateTime.Now.Minute + fileUpload1.FileName;
                }
                else
                {
                    fileUpload1.PostedFile.SaveAs(filename);
                    aTemplateInfo.templateImageCover = Link + "/" + fileUpload1.FileName;
                }
                
            }
            if (fileUpload.HasFile)
            {
                if (File.Exists(path + "/" + fileUpload.FileName))
                {
                    filename = path + "/" + DateTime.Now.Minute + fileUpload.FileName;
                    fileUpload.PostedFile.SaveAs(filename);
                    aTemplateInfo.TemplateContent = Link + "/" + DateTime.Now.Minute + fileUpload.FileName;
                }
                else
                {
                    fileUpload.PostedFile.SaveAs(path + "/" + fileUpload.FileName);
                    aTemplateInfo.TemplateContent = Link + "/" + fileUpload.FileName;
                }

                aTemplateInfo.TemplateName = txtName.Text;
                aTemplateInfo.TemplateStatus = false;

                ITemplateFacade aTemplateFacade = new TemplateFacade();
                aTemplateFacade.Insert(aTemplateInfo);
            }

        }
    }
}