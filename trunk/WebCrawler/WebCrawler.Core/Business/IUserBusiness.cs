using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public interface IUserBusiness
    {
        void Insert(UserInfo pUserInfo);

        void Update(UserInfo pUserInfo);

        void Delete(UserInfo pUserInfo);

        IList<UserInfo> SelectAll();

        UserInfo SelectByPrimaryId(object pId);

        IList<UserInfo> SelectByParams(IList<ParameterInfo> pParams);

        IList<UserInfo> SelectUserByRole(string RoleName);
    }
}
