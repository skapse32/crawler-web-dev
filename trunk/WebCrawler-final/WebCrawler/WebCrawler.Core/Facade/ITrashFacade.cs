using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler.Core.Info
{
    public interface ITrashFacade
    {
        void Insert(TrashInfo pTrashInfo);

        void Delete(TrashInfo pTrashInfo);

        IList<TrashInfo> GetTrashInfos();
    }
}
