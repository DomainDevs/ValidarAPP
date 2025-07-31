using Sistran.Co.Application.Data;
using Sistran.Core.Application.UniqueUser.Entities;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao User
    /// </summary>
    public class UniqueUserSessionDAO
    {
        public bool DeletetUserSession(int UserId)
        {
            try
            {
                PrimaryKey key = UniqueUser.Entities.UniqueUserSession.CreatePrimaryKey(UserId);
                UniqueUser.Entities.UniqueUserSession uniqueUserSession = (UniqueUser.Entities.UniqueUserSession)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(uniqueUserSession);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public Models.UniqueUserSession GetUserInSession(string accountName)
        {
            Models.UniqueUserSession uniqueUserSession = new Models.UniqueUserSession();
            int userId = UserDAO.GetUserId(accountName);

            PrimaryKey key = UniqueUser.Entities.UniqueUserSession.CreatePrimaryKey(userId);
            UniqueUser.Entities.UniqueUserSession uniqueUserSessionEntities = (UniqueUser.Entities.UniqueUserSession)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (uniqueUserSessionEntities != null)
            {
                uniqueUserSession.UserId = uniqueUserSessionEntities.UserId;
                uniqueUserSession.AccountName = accountName;
                uniqueUserSession.RegistrationDate = uniqueUserSessionEntities.RegistrationDate;
                uniqueUserSession.ExpirationDate = uniqueUserSessionEntities.ExpirationDate;
                uniqueUserSession.LastUpdateDate = uniqueUserSessionEntities.LastUpdateDate;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId, typeof(UniqueUser.Entities.ProfileUniqueUser).Name, uniqueUserSessionEntities.UserId);
                uniqueUserSession.ProfileId = ((UniqueUser.Entities.ProfileUniqueUser)DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.ProfileUniqueUser), filter.GetPredicate()).First()).ProfileId;
            }
            else
            {
                uniqueUserSession = null;
            }

            return uniqueUserSession;
        }
        public Models.UniqueUserSession TryInitSession(Models.UniqueUserSession uniqueUserSession)
        {
            bool result = false;

            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("USER_ID", uniqueUserSession.UserId, DbType.Int16);
            parameters[1] = new NameValue("REGISTRATION_DATE", uniqueUserSession.RegistrationDate, DbType.DateTime2);
            parameters[2] = new NameValue("EXPIRATION_DATE", uniqueUserSession.ExpirationDate, DbType.DateTime2);
            parameters[3] = new NameValue("LAST_UPDATE_DATE", uniqueUserSession.LastUpdateDate, DbType.DateTime2);
            parameters[4] = new NameValue("SESSION_ID", uniqueUserSession.SessionId, DbType.DateTime2);
            DataTable resultSP;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                resultSP = pdb.ExecuteSPDataTable("UU.UNIQUE_USER_SESSION_INSERT", parameters);

            }
            if (resultSP != null && resultSP.Rows.Count > 0)
            {
                switch ((int)resultSP.Rows[0][0])
                {
                    case 0:
                        result = true;
                        break;
                    default:
                        break;
                }
            }
            if (result)
            {
                uniqueUserSession.UserId = uniqueUserSession.UserId;
                uniqueUserSession.AccountName = uniqueUserSession.AccountName;
                uniqueUserSession.RegistrationDate = uniqueUserSession.RegistrationDate;
                uniqueUserSession.ExpirationDate = uniqueUserSession.ExpirationDate;
                uniqueUserSession.LastUpdateDate = uniqueUserSession.LastUpdateDate;
                uniqueUserSession.SessionId = uniqueUserSession.SessionId;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId, typeof(UniqueUser.Entities.ProfileUniqueUser).Name, uniqueUserSession.UserId);
                uniqueUserSession.ProfileId = ((UniqueUser.Entities.ProfileUniqueUser)DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.ProfileUniqueUser), filter.GetPredicate()).First()).ProfileId;
            }
            else
            {
                uniqueUserSession = null;
            }
            return uniqueUserSession;
        }

        public Models.UniqueUserSession GetUserInSessionBySessionId(string SessionId)
        {
            Models.UniqueUserSession uniqueUserSession = new Models.UniqueUserSession();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUserSession.Properties.SessionId, typeof(UniqueUser.Entities.UniqueUserSession).Name);
            filter.Equal();
            filter.Constant(SessionId);
            UniqueUser.Entities.UniqueUserSession uniqueUserSessionEntities = DataFacadeManager.Instance.GetDataFacade().List<UniqueUser.Entities.UniqueUserSession>(filter.GetPredicate()).Cast<UniqueUser.Entities.UniqueUserSession>().FirstOrDefault();
            if (uniqueUserSessionEntities != null)
            {
                UserDAO userDAO = new UserDAO();
                var user = userDAO.GetUserByAccountNameUserId("", uniqueUserSessionEntities.UserId);
                uniqueUserSession.UserId = uniqueUserSessionEntities.UserId;
                uniqueUserSession.AccountName = user.AccountName;
                uniqueUserSession.RegistrationDate = uniqueUserSessionEntities.RegistrationDate;
                uniqueUserSession.ExpirationDate = uniqueUserSessionEntities.ExpirationDate;
                uniqueUserSession.LastUpdateDate = uniqueUserSessionEntities.LastUpdateDate;
                uniqueUserSession.SessionId = uniqueUserSessionEntities.SessionId;
                filter.Clear();
                filter.PropertyEquals(UniqueUser.Entities.ProfileUniqueUser.Properties.UserId, typeof(UniqueUser.Entities.ProfileUniqueUser).Name, uniqueUserSessionEntities.UserId);
                uniqueUserSession.ProfileId = ((UniqueUser.Entities.ProfileUniqueUser)DataFacadeManager.GetObjects(typeof(UniqueUser.Entities.ProfileUniqueUser), filter.GetPredicate()).First()).ProfileId;
            }
            else
            {
                uniqueUserSession = null;
            }

            return uniqueUserSession;
        }

        //public Models.UniqueUserSession UpdateExpirationDate(int userId, DateTime ExpirationDate)
        //{
        //    try
        //    {


        //        var primaryKey = UUEN.UniqueUserSession.CreatePrimaryKey(userId);
        //        var uniqueUserSession = (UUEN.UniqueUserSession)DataFacadeManager.GetObject(primaryKey);
        //        if (uniqueUserSession != null)
        //        {
        //            uniqueUserSession.ExpirationDate = ExpirationDate;
        //            DataFacadeManager.Update(uniqueUserSession);
        //            return ModelAssembler.CreateUniqueUserSession(uniqueUserSession);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }
        //}
    }
}