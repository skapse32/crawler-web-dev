using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Business;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Facade
{
    public interface ITemplateFacade
    {
        IList<TemplateInfo> GetAllTemplateInfos();

        TemplateInfo GetTemplateInfoById(int pId);

        void Insert(TemplateInfo pTemplateInfo);

        void Update(TemplateInfo pTemplateInfo);

        void Delete(TemplateInfo pTemplateInfo);
    }
}
