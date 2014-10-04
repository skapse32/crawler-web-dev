using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using WebCrawler.Core.Framework;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public class TemplateBusiness : ITemplateBusiness
    {
        private ISessionFactory aISessionFactor;

        private ISession aISession;

        private ITransaction aTransaction = null;
        public TemplateBusiness()
        {
            aISessionFactor = NHibernateSessionFactory.InstanceSessionFactory();
        }

        public void Insert(TemplateInfo pTemplateInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Save(pTemplateInfo);
                aTransaction.Commit();
            }
            catch (Exception ex)
            {
                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }

        }

        public void Update(TemplateInfo pTemplateInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Update(pTemplateInfo);
                aTransaction.Commit();
            }
            catch (Exception ex)
            {
                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }

        }

        public void Delete(TemplateInfo pTemplateInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Delete(pTemplateInfo);
                aTransaction.Commit();
            }
            catch (Exception ex)
            {

                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }

        }

        public IList<TemplateInfo> SelectAll()
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.QueryOver<TemplateInfo>().List();
            }
            catch (Exception ex)
            {

                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }

        }


        public TemplateInfo SelectByPrimaryId(int pId)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.QueryOver<TemplateInfo>().Where(x=>x.TemplateId == pId).SingleOrDefault();
            }
            catch (Exception ex)
            {
                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }

        }

        public IList<TemplateInfo> SelectUserByRole(string RoleName)
        {
            try
            {
                aISession = aISessionFactor.OpenSession();
                string aHibernateQuery = "select A from TemplateInfo A, RoleInfo R, UserRoleInfo U where A.UserId = U.UserId and U.RoleId = R.RoleId and R.RoleName = '" + RoleName + "'";
                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                //aIQuery.SetParameter("RoleName", RoleName);
                return aIQuery.List<TemplateInfo>();
            }
            catch (Exception ex)
            {

                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }
        }

        public IList<TemplateInfo> SelectByParams(IList<ParameterInfo> pParams)
        {

            try
            {

                aISession = aISessionFactor.OpenSession();

                string aHibernateQuery = "from TemplateInfo A where 1 = 1 ";

                foreach (var aParams in pParams)
                {
                    aHibernateQuery += "and A." + aParams.Name + " = :" + aParams.Name + " ";
                }

                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                foreach (var aParams in pParams)
                {
                    aIQuery.SetParameter(aParams.Name, aParams.Value);
                }

                return aIQuery.List<TemplateInfo>();
            }
            catch (Exception ex)
            {
                aTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (aISession.IsOpen)
                {
                    aISession.Close();
                }
            }

        }
    }
}
