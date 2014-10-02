using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public interface ITrashBusiness
    {
        void Insert(TrashInfo pTrashInfo);

        void Update(TrashInfo pTrashInfo);

        void Delete(TrashInfo pTrashInfo);

        IList<TrashInfo> SelectAll();

        TrashInfo SelectByPrimaryId(object pId);

        IList<TrashInfo> SelectByParams(IList<ParameterInfo> pParams);

        IList<TrashInfo> SelectUserByRole(string RoleName);
    }
}
