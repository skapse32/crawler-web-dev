using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace WebCrawler.Core.Info
{
    public class TrashInfo
    {
        public virtual int TrashId { get; set; }

        public virtual DateTime TrashDate { get; set; }

        public virtual string TrashFolderName { get; set; }

        public virtual string TrashImageLink { get; set; }
    }
}
