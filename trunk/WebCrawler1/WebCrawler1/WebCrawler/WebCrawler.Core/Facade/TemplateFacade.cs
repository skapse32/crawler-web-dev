using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Business;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Facade
{
    public class TemplateFacade : ITemplateFacade
    {
        public IList<TemplateInfo> GetAllTemplateInfos()
        {
            ITemplateBusiness aTemplateBusiness = new TemplateBusiness();
            var lTemplateInfos = aTemplateBusiness.SelectAll();
            return lTemplateInfos;
        }

        public TemplateInfo GetTemplateInfoById(int pId)
        {
            ITemplateBusiness aTemplateBusiness = new TemplateBusiness();
            return aTemplateBusiness.SelectByPrimaryId(pId);
        }

        public void Insert(TemplateInfo pTemplateInfo)
        {
            ITemplateBusiness aTemplateBusiness = new TemplateBusiness();
            aTemplateBusiness.Insert(pTemplateInfo);
        }

        public void Update(TemplateInfo pTemplateInfo)
        {
            ITemplateBusiness aTemplateBusiness = new TemplateBusiness();
            aTemplateBusiness.Update(pTemplateInfo);
        }

        public void Delete(TemplateInfo pTemplateInfo)
        {
            ITemplateBusiness aTemplateBusiness = new TemplateBusiness();
            aTemplateBusiness.Delete(pTemplateInfo);
        }
    }
}
