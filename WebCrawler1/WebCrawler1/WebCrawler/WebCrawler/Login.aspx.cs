using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core.Facade;

namespace WebCrawler
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_OnClick(object sender, EventArgs e)
        {
            var aUserFacade = new UserFacade();
            var aUserInfo = aUserFacade.Authenticate(txtUserName.Value.Trim(), txtPassword.Value.Trim());
            if (aUserInfo != null)
            {
                Session["UserInfo"] = aUserInfo;
                Response.Redirect(Request.QueryString["continue"]);
            }
        }
    }
}