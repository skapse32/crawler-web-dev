using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCrawler.Core.Facade;
using WebCrawler.Core.Info;

namespace WebCrawler.Website
{
    public partial class Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInfo"] == null)
            {
                Response.Redirect("Login.aspx?continue=" + Request.RawUrl);
            }
        }

        protected void btnChange_OnClick(object sender, EventArgs e)
        {
            var aUserInfo = (UserInfo) Session["UserInfo"];
            IUserFacade aUserFacade = new UserFacade();
            if (aUserFacade.CheckPassWord(aUserInfo.UserName, txtOldPass.Value.Trim()))
            {
                aUserInfo.Password = txtNewPass.Value.Trim();
                aUserFacade.Update(aUserInfo);

            }
        }
    }
}