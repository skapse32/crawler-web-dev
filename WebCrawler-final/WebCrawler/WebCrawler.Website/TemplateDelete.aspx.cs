using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core.Facade;
using WebCrawler.Core.Info;

namespace WebCrawler.Website
{
    public partial class TemplateDelete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            }
            if (!IsPostBack)
            {
                ITrashFacade aTrashFacade = new TrashFacade();
                dtlListTemplate.DataSource = aTrashFacade.GetTrashInfos();
                dtlListTemplate.DataBind();
            }
        }
    }
}