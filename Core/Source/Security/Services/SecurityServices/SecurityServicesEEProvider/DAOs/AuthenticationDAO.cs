using Sistran.Core.Application.SecurityServices.EEProvider.Assemblers;
using Sistran.Core.Application.SecurityServices.EEProvider.Helper;
using Sistran.Core.Application.SecurityServices.Enums;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.SecurityServices.EEProvider.DAOs
{
    /// <summary>
    /// Estado Acceso
    /// </summary>
    public class AccessIdState
    {
        public int AccessId { get; set; }
        public bool AccessEnable { get; set; }
    }

    /// <summary>
    /// Dao Autenticacion
    /// </summary>
    internal class AuthenticationDAO
    {
        List<AccessIdState> accessStates = new List<AccessIdState>();

        /// <summary>
        /// Autenticacion Por Usuario y clave
        /// </summary>
        /// <param name="loginName">Nombre Usuario</param>
        /// <param name="password">Clave</param>
        /// <param name="domain">Dominio</param>
        /// <returns></returns>
        internal AuthenticationResult Autenthicate(string loginName, string password, string domain)
        {
            AuthenticationResult authenticationResult = new AuthenticationResult();
            UserDAO userDao = new UserDAO();
            try
            {
                authenticationResult = userDao.GetUserByLoginByPassword(loginName, password);
            }
            catch (Exception ex)
            {
                authenticationResult.isAuthenticated = false;
                authenticationResult.Result = AuthenticationEnum.isInvalidPassword;
                throw;
            }
            return authenticationResult;
        }

        /// <summary>
        /// Autenticacion Por Usuario y clave
        /// </summary>
        /// <param name="loginName">Nombre Usuario</param>
        /// <param name="password">Clave</param>
        /// <param name="domain">Dominio</param>
        /// <param name="SessionID">Session</param>
        /// <returns></returns>
        internal AuthenticationResult AutenthicateR2(string loginName, string password, string domain, string SessionID)
        {
            AuthenticationResult authenticationResult = new AuthenticationResult();
            UserDAO userDao = new UserDAO();
            try
            {
                authenticationResult = userDao.GetUserByLoginByPasswordR2(loginName, password, SessionID);
            }
            catch (Exception)
            {
                authenticationResult.isAuthenticated = false;
                authenticationResult.Result = AuthenticationEnum.isInvalidPassword;
                throw;
            }
            return authenticationResult;
        }



        /// <summary>
        /// Obtener Accesos por Perfil de Usuario
        /// </summary>
        /// <param name="viewID">Nombre Vista</param>
        /// <param name="userName">Nombre Usuaio</param>
        /// <returns></returns>
        internal IList<ControlSecurity> GetControlsSecurity(string viewID, string userName)
        {
            int auxUserId = UserHelper.GetUserIdLogOn(userName);
            User usrAccess = GetUserOperation(auxUserId, viewID);
            List<ControlSecurity> controlList = (from usr in usrAccess.OperationObjects
                                                 select new ControlSecurity
                                                 {
                                                     ControlID = usr.Route,
                                                     Enabled = usr.Enable,
                                                     Visible = usr.Visible
                                                 }).ToList();

            return controlList;
        }


        /// <summary>
        /// Obtener Usuario
        /// </summary>
        /// <param name="userId">Id Usuario.</param>
        /// <param name="viewID">Nombre Vista.</param>
        /// <returns></returns>
        private User GetUserOperation(int userId, string viewID)
        {
            User user = new User();
            ArrayList listaValues = new ArrayList();
            AuthorizationDAO authorizationDAO = new AuthorizationDAO();
            UniqueUser.Entities.UniqueUsers userEntities = authorizationDAO.FindUserByUserId(userId);
            if (userEntities != null)
            {
                user = new User()
                {
                    Id = userEntities.UserId,
                    Nick = userEntities.AccountName,
                    OperationObjects = new List<OperationObject>()
                };
                ObjectCriteriaBuilder filterProfile = new ObjectCriteriaBuilder();
                filterProfile.Property("UserId");
                filterProfile.Equal();
                filterProfile.Constant(userId);
                List<Profile> profileUserList = authorizationDAO.ListProfile(filterProfile.GetPredicate());

                if (profileUserList.Count > 0)
                {
                    ObjectCriteriaBuilder filterUsrAccess = new ObjectCriteriaBuilder();
                    filterUsrAccess.Property("ProfileId");
                    filterUsrAccess.In();
                    filterUsrAccess.ListValue();
                    foreach (Profile profile in profileUserList)
                    {
                        if (!listaValues.Contains(profile.ProfileId))
                        {
                            filterUsrAccess.Constant(profile.ProfileId);
                            listaValues.Add(profile.ProfileId);

                        }
                    }
                    filterUsrAccess.EndList();
                    List<Models.OperationProfile> operationProfileList = authorizationDAO.ListOperationProfile(filterUsrAccess.GetPredicate());
                    listaValues = new ArrayList();
                    if (operationProfileList.Count > 0)
                    {
                        ObjectCriteriaBuilder filterAccess = new ObjectCriteriaBuilder();
                        filterAccess.Property("Url");
                        filterAccess.Like();
                        filterAccess.Constant(viewID + "%");
                        filterAccess.And();
                        filterAccess.Property("AccessId");
                        filterAccess.In();
                        filterAccess.ListValue();
                        foreach (Models.OperationProfile usrAcc in operationProfileList)
                        {
                            if (!listaValues.Contains(usrAcc.OperationId))
                            {
                                filterAccess.Constant(usrAcc.OperationId);
                                listaValues.Add(usrAcc.OperationId);
                                accessStates.Add(new AccessIdState() { AccessId = usrAcc.OperationId, AccessEnable = usrAcc.Enabled });
                            }
                        }
                        filterAccess.EndList();
                        List<Models.Operation> accessList = authorizationDAO.ListOperations(filterAccess.GetPredicate());
                        user.OperationObjects = (from Models.Operation acc in accessList
                                                 select new OperationObject
                                                 {
                                                     Id = acc.OperationId,
                                                     Description = acc.Description,
                                                     Route = acc.Route.Replace(viewID, ""),
                                                     Enable = GetStateAccess(acc.OperationId, acc.Enabled),
                                                     Visible = true
                                                 }).ToList();
                    }
                }
            }

            return user;
        }


        /// <summary>
        /// Obtener Estado del Acceso
        /// </summary>
        /// <param name="AccessId">Id Acceso.</param>
        /// <param name="AccessEnable">si el valor es <c>true</c> [access enable].</param>
        /// <returns></returns>
        public bool GetStateAccess(int AccessId, bool AccessEnable)
        {
            AccessIdState stateProfile = (from a in accessStates
                                          where a.AccessId == AccessId
                                          select new AccessIdState
                                          {
                                              AccessId = a.AccessId,
                                              AccessEnable = a.AccessEnable
                                          }).FirstOrDefault();

            return (stateProfile != null && AccessEnable && stateProfile.AccessEnable);
        }

        /// <summary>
        /// Obtiene el id del usuario  a travez del nombre
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal int GetUserId(string name)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
            filter.Equal();
            filter.Constant(name);
            BusinessCollection users = new
                            BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));
            if (users != null)
            {
                foreach (UniqueUser.Entities.UniqueUsers uniqueUser in users)
                    return uniqueUser.UserId;
            }
            return 0;
        }

        /// <summary>
        /// Obtener el Usuario por nombre
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal User GetUserByName(string name)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName);
            filter.Equal();
            filter.Constant(name);
            UniqueUser.Entities.UniqueUsers user = (UniqueUser.Entities.UniqueUsers)DataFacadeManager.Instance.GetDataFacade().List(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()).FirstOrDefault();
            if (user != null)
            {
                return ModelAssembler.CreateUniqueUser(user);
            }
            else
            {
                return null;
            }
        }
    }
}
