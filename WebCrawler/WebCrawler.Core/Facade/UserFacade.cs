using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Business;
using WebCrawler.Core.Info;


namespace WebCrawler.Core.Facade
{
    public class UserFacade : IUserFacade
    {
        IUserBusiness aUserBusiness = new UserBusiness();
        public IList<UserInfo> getAlluser()
        {
            return aUserBusiness.SelectAll();
        }

        public UserInfo Authenticate(string UserName, string Password)
        {
            var lParameterInfo = new List<ParameterInfo>();
            lParameterInfo.Add(new ParameterInfo() { Name = "UserName", Value = UserName });
            lParameterInfo.Add(new ParameterInfo() { Name = "Password", Value = Password });
            lParameterInfo.Add(new ParameterInfo() { Name = "is_lock", Value = false });
            var aUserInfo = aUserBusiness.SelectByParams(lParameterInfo).FirstOrDefault();
            return aUserInfo;
        }

        public UserInfo getUser(string UserName)
        {
            var lParameterInfo = new List<ParameterInfo>();
            lParameterInfo.Add(new ParameterInfo() { Name = "UserName", Value = UserName });
            return aUserBusiness.SelectByParams(lParameterInfo).FirstOrDefault();
        }

        public UserInfo getUserByKey(object UserId)
        {
            return aUserBusiness.SelectByPrimaryId(UserId);
        }
    }
}
