using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler.Core.Info
{
    public class FolderMediaInfo
    {
        public virtual int FolderId { get; set; }

        public virtual int UserId { get; set; }

        public virtual string FolderName { get; set; }

        public virtual string FolderDescription { get; set; }

        public virtual DateTime FolderDateCreate { get; set; }
    }
}
