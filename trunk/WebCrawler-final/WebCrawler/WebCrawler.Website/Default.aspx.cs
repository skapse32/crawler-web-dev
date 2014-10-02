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
                IFolderMediaFacade aFolderMediaFacade = new FolderMediaFacade();
                dtlListTemplate.DataSource = aFolderMediaFacade.Select();
                dtlListTemplate.DataBind();
            }
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            var aUserInfo = (UserInfo) Session["UserInfo"];
            Button btnDelete = (Button)sender;
            //var ImageInfos = new List<ImagesInfo>();
            var imgImage = (HiddenField)btnDelete.Parent.FindControl("hdtemplateId");

            int aFolderId = int.Parse(imgImage.Value);

            IFolderMediaFacade aFolderMediaFacade = new FolderMediaFacade();
            var aFolderMediaInfo = aFolderMediaFacade.SelectById(aFolderId);
            ITrashFacade aTrashFacade = new TrashFacade();
            var aTrashInfo = new TrashInfo();
            aTrashInfo.TrashDate = DateTime.Now;
            if (!Directory.Exists(Server.MapPath("~/Upload/Trash/") + aUserInfo.UserName + DateTime.Now.ToShortDateString() ))
            {
                Directory.CreateDirectory(Server.MapPath("~/Upload/Trash/") + aUserInfo.UserName +
                                          DateTime.Now.ToShortDateString());
            }
            aTrashInfo.TrashFolderName = "/Upload/Trash/" + aUserInfo.UserName +
                                             DateTime.Now.ToShortDateString();
           
            string filename = aFolderMediaInfo.FolderImage.Replace(ServerHostName(), "");
             string desfile = Server.MapPath(aTrashInfo.TrashFolderName) + "/" +
                             filename.Split('/')[filename.Split('/')
                             .Length - 1].Replace(@"\", "").Replace(@"\", "").Replace(@"\", "");
            if (File.Exists(Server.MapPath(filename)))
            {
                File.Move(Server.MapPath(filename), desfile);
            }
            aTrashInfo.TrashImageLink = aTrashInfo.TrashFolderName + "/" + filename.Split('/')[filename.Split('/').Length - 1];
            aTrashFacade.Insert(aTrashInfo);
            if (Directory.Exists(Server.MapPath("~/" + aFolderMediaInfo.FolderName.Replace("template.html",""))))
            {
                Directory.Delete(Server.MapPath("~/" + aFolderMediaInfo.FolderName.Replace("template.html", "")), true);
            }
            aFolderMediaFacade.Delete(aFolderMediaInfo);
            dtlListTemplate.DataSource = aFolderMediaFacade.Select();
            dtlListTemplate.DataBind();
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