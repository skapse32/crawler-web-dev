using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Facade
{
    public interface IFolderMediaFacade
    {
        void Insert(FolderMediaInfo pFolderMediaInfo);

        void Update(FolderMediaInfo pFolderMediaInfo);

        void Delete(FolderMediaInfo pFolderMediaInfo);

        IList<FolderMediaInfo> Select();

        FolderMediaInfo SelectById(int pId);
    }
}
