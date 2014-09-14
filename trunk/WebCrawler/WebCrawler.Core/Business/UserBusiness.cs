using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using WebCrawler.Core.Framework;
using WebCrawler.Core.Info;

namespace WebCrawler.Core.Business
{
    public class UserBusiness : IUserBusiness
    {
        private ISessionFactory aISessionFactor;

        private ISession aISession;

        private ITransaction aTransaction = null;
        public UserBusiness()
        {
            aISessionFactor = NHibernateSessionFactory.InstanceSessionFactory();
        }

        public void Insert(UserInfo pUserInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Save(pUserInfo);
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

        public void Update(UserInfo pUserInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Update(pUserInfo);
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

        public void Delete(UserInfo pUserInfo)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();
                aTransaction = aISession.BeginTransaction();

                aISession.Delete(pUserInfo);
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

        public IList<UserInfo> SelectAll()
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.QueryOver<UserInfo>().List();
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


        public UserInfo SelectByPrimaryId(object pId)
        {

            try
            {
                aISession = aISessionFactor.OpenSession();

                return aISession.Get<UserInfo>(pId);
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

        public IList<UserInfo> SelectUserByRole(string RoleName)
        {
            try
            {
                aISession = aISessionFactor.OpenSession();
                string aHibernateQuery = "select A from UserInfo A, RoleInfo R, UserRoleInfo U where A.UserId = U.UserId and U.RoleId = R.RoleId and R.RoleName = '" + RoleName + "'";
                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                //aIQuery.SetParameter("RoleName", RoleName);
                return aIQuery.List<UserInfo>();
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

        public IList<UserInfo> SelectByParams(IList<ParameterInfo> pParams)
        {

            try
            {

                aISession = aISessionFactor.OpenSession();

                string aHibernateQuery = "from UserInfo A where 1 = 1 ";

                foreach (var aParams in pParams)
                {
                    aHibernateQuery += "and A." + aParams.Name + " = :" + aParams.Name + " ";
                }

                var aIQuery = aISession.CreateQuery(aHibernateQuery);
                foreach (var aParams in pParams)
                {
                    aIQuery.SetParameter(aParams.Name, aParams.Value);
                }

                return aIQuery.List<UserInfo>();
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
