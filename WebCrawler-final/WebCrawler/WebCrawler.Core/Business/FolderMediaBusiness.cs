using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using WebCrawler.Core.Framework;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public class FolderMediaBusiness : IFolderMediaBusiness
    {
        private ISessionFactory aISessionFactor;

        private ISession aISession;

        private ITransaction aTransaction = null;
        public FolderMediaBusiness()
        {
            aISessionFactor = NHibernateSessionFactory.InstanceSessionFactory();
        }

        public void Insert(FolderMediaInfo pFolderMediaInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Save(pFolderMediaInfo);
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

        public void Update(FolderMediaInfo pFolderMediaInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Update(pFolderMediaInfo);
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

        public void Delete(FolderMediaInfo pFolderMediaInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Delete(pFolderMediaInfo);
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

        public IList<FolderMediaInfo> SelectAll()
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.QueryOver<FolderMediaInfo>().List();
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


        public FolderMediaInfo SelectByPrimaryId(int pId)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.Get<FolderMediaInfo>(pId);
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

        public IList<FolderMediaInfo> SelectUserByRole(string RoleName)
        {
            try
            {
                aISession = aISessionFactor.OpenSession();
                string aHibernateQuery = "select A from FolderMediaInfo A, RoleInfo R, UserRoleInfo U where A.UserId = U.UserId and U.RoleId = R.RoleId and R.RoleName = '" + RoleName + "'";
                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                //aIQuery.SetParameter("RoleName", RoleName);
                return aIQuery.List<FolderMediaInfo>();
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

        public IList<FolderMediaInfo> SelectByParams(IList<ParameterInfo> pParams)
        {

            try
            {

                aISession = aISessionFactor.OpenSession();

                string aHibernateQuery = "from FolderMediaInfo A where 1 = 1 ";

                foreach (var aParams in pParams)
                {
                    aHibernateQuery += "and A." + aParams.Name + " = :" + aParams.Name + " ";
                }

                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                foreach (var aParams in pParams)
                {
                    aIQuery.SetParameter(aParams.Name, aParams.Value);
                }

                return aIQuery.List<FolderMediaInfo>();
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
