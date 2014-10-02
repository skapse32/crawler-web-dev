using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Business;

namespace WebCrawler.Core.Info
{
    public class TrashFacade : ITrashFacade
    {
        public void Insert(TrashInfo pTrashInfo)
        {
            ITrashBusiness aTrashBusiness = new TrashBusiness();
           aTrashBusiness.Insert(pTrashInfo);
        }

        public void Delete(TrashInfo pTrashInfo)
        {
            ITrashBusiness aTrashBusiness = new TrashBusiness();
            aTrashBusiness.Update(pTrashInfo);
        }

        public IList<TrashInfo> GetTrashInfos()
        {
            ITrashBusiness aTrashBusiness = new TrashBusiness();
            return aTrashBusiness.SelectAll();
        }
    }
}
