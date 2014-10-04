using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core.Facade;

namespace WebCrawler.Website
{
    public partial class Galleries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IFolderMediaFacade aFolderMediaFacade= new FolderMediaFacade();
                grid.DataSource = aFolderMediaFacade.Select();
                grid.DataBind();
            }
        }

        protected void grid_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            IFolderMediaFacade aFolderMediaFacade = new FolderMediaFacade();
            grid.DataSource = aFolderMediaFacade.Select();
            grid.PageIndex = e.NewPageIndex;
            grid.DataBind();
        }

        protected void grid_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delGalleries")
            {
                var aId = int.Parse(e.CommandArgument.ToString());
                IFolderMediaFacade aFolderMediaFacade = new FolderMediaFacade();
                var aFolderMediaInfo = aFolderMediaFacade.SelectById(aId);
                aFolderMediaFacade.Delete(aFolderMediaInfo);
                grid.DataSource = aFolderMediaFacade.Select();
                grid.DataBind();
            }
        }
    }
}