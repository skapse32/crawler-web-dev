using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Facade
{
    public interface IUserFacade
    {
        IList<UserInfo> getAlluser();

        UserInfo getUser(string UserName);

        UserInfo getUserByKey(object UserId);

        UserInfo Authenticate(string UserName, string Password);
    }
}
