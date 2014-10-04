using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Business;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Facade
{
    public class FolderMediaFacade :IFolderMediaFacade
    {
        public void Insert(FolderMediaInfo pFolderMediaInfo)
        {
            IFolderMediaBusiness aFolderMediaBusiness = new FolderMediaBusiness();
            aFolderMediaBusiness.Insert(pFolderMediaInfo);
        }

        public void Update(FolderMediaInfo pFolderMediaInfo)
        {
            IFolderMediaBusiness aFolderMediaBusiness = new FolderMediaBusiness();
            aFolderMediaBusiness.Update(pFolderMediaInfo);
        }

        public void Delete(FolderMediaInfo pFolderMediaInfo)
        {
            IFolderMediaBusiness aFolderMediaBusiness = new FolderMediaBusiness();
            aFolderMediaBusiness.Delete(pFolderMediaInfo);
        }

        public IList<FolderMediaInfo> Select()
        {
            IFolderMediaBusiness aFolderMediaBusiness = new FolderMediaBusiness();
            return aFolderMediaBusiness.SelectAll();
        }

        public FolderMediaInfo SelectById(int pId)
        {
            IFolderMediaBusiness aFolderMediaBusiness = new FolderMediaBusiness();
            return aFolderMediaBusiness.SelectByPrimaryId(pId);
        }
    }
}
