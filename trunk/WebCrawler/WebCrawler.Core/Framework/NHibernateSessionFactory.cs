using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;

namespace WebCrawler.Core.Framework
{

    public class NHibernateSessionFactory
    {
        private static volatile ISessionFactory _aSessionFactory = null;

        private static void InitMapper()
        {
            var config = new Configuration();
            config.Configure();
            //var schema = new SchemaExport(config);
            //schema.Execute(false, true, false);
            _aSessionFactory = config.BuildSessionFactory();
        }

        public static ISessionFactory InstanceSessionFactory()
        {
            if (_aSessionFactory == null)
            {
                InitMapper();
            }

            return _aSessionFactory;
        }
    }
}
