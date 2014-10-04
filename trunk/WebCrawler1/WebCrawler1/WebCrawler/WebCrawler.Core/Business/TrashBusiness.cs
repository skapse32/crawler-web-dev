using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using WebCrawler.Core.Framework;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public class TrashBusiness : ITrashBusiness
    {
        private ISessionFactory aISessionFactor;

        private ISession aISession;

        private ITransaction aTransaction = null;
        public TrashBusiness()
        {
            aISessionFactor = NHibernateSessionFactory.InstanceSessionFactory();
        }

        public void Insert(TrashInfo pTrashInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Save(pTrashInfo);
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

        public void Update(TrashInfo pTrashInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Update(pTrashInfo);
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

        public void Delete(TrashInfo pTrashInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Delete(pTrashInfo);
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

        public IList<TrashInfo> SelectAll()
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.QueryOver<TrashInfo>().List();
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


        public TrashInfo SelectByPrimaryId(object pId)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.Get<TrashInfo>(pId);
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

        public IList<TrashInfo> SelectUserByRole(string RoleName)
        {
            try
            {
                aISession = aISessionFactor.OpenSession();
                string aHibernateQuery = "select A from TrashInfo A, RoleInfo R, UserRoleInfo U where A.UserId = U.UserId and U.RoleId = R.RoleId and R.RoleName = '" + RoleName + "'";
                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                //aIQuery.SetParameter("RoleName", RoleName);
                return aIQuery.List<TrashInfo>();
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

        public IList<TrashInfo> SelectByParams(IList<ParameterInfo> pParams)
        {

            try
            {

                aISession = aISessionFactor.OpenSession();

                string aHibernateQuery = "from TrashInfo A where 1 = 1 ";

                foreach (var aParams in pParams)
                {
                    aHibernateQuery += "and A." + aParams.Name + " = :" + aParams.Name + " ";
                }

                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                foreach (var aParams in pParams)
                {
                    aIQuery.SetParameter(aParams.Name, aParams.Value);
                }

                return aIQuery.List<TrashInfo>();
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
