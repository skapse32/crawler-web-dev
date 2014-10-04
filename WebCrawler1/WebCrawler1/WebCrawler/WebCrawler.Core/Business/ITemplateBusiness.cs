using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public interface ITemplateBusiness
    {
        void Insert(TemplateInfo pTemplateInfo);

        void Update(TemplateInfo pTemplateInfo);

        void Delete(TemplateInfo pTemplateInfo);

        IList<TemplateInfo> SelectAll();

        TemplateInfo SelectByPrimaryId(int pId);

        IList<TemplateInfo> SelectByParams(IList<ParameterInfo> pParams);
    }
}
