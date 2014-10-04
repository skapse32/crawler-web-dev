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

        public UserInfo getUserByKey(int UserId)
        {
            return aUserBusiness.SelectByPrimaryId(UserId);
        }

        public bool CheckPassWord(string username, string passwod)
        {
            var aParams = new List<ParameterInfo>();
            aParams.Add(new ParameterInfo() { Name = "Password", Value = passwod});
            aParams.Add(new ParameterInfo() { Name = "UserName", Value = username});
            var aUserInfo = aUserBusiness.SelectByParams(aParams).FirstOrDefault();
            if (aUserInfo != null)
            {
                return true;
            }
            return false;
        }

        public void Update(UserInfo pUserInfo)
        {
            aUserBusiness.Update(pUserInfo);
        }
    }
}
