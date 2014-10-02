using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace WebCrawler.Core.Info
{
    public class UserInfo
    {
        public virtual int UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual bool? is_lock { get; set; }
    }
}
