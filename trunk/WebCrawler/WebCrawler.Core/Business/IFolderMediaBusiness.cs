using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public interface IFolderMediaBusiness
    {
        void Insert(FolderMediaInfo pFolderMediaInfo);

        void Update(FolderMediaInfo pFolderMediaInfo);

        void Delete(FolderMediaInfo pFolderMediaInfo);

        IList<FolderMediaInfo> SelectAll();

        FolderMediaInfo SelectByPrimaryId(object pId);

        IList<FolderMediaInfo> SelectByParams(IList<ParameterInfo> pParams);

        IList<FolderMediaInfo> SelectUserByRole(string RoleName);
    }
}
