using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler.Core.Info
{
    public class TemplateInfo
    {
        public virtual int TemplateId { get; set; }

        public  virtual  string TemplateName { set; get; }

        public  virtual string TemplateContent { get; set; }

        public virtual string templateImageCover { get; set; }

        public  virtual bool TemplateStatus { get; set; }
    }
}
