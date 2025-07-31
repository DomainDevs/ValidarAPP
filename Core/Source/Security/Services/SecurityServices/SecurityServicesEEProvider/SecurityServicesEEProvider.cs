using Sistran.Core.Application.SecurityServices.EEProvider.DAOs;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Configuration;
using MOD = Sistran.Core.Application.SecurityServices.Models;

namespace Sistran.Core.Application.SecurityServices.EEProvider
{

    /// <summary>
    /// Modulo Seguridad Usuarios
    /// </summary>
    /// <seealso cref="Sistran.Core.Framework.SAF.Integration.BAF.ServiceBase" />
    /// <seealso cref="Sistran.Core.Application.SecurityServices.IAuthenticationService" />
    /// <seealso cref="Sistran.Core.Application.SecurityServices.IAuthorizationService" />
    public class SecurityServicesEEProvider : IAuthenticationService, IAuthorizationService
    {
        /// <summary>
        /// Logueo de Usuario
        /// </summary>
        /// <param name="loginName">Nombre Usuario</param>
        /// <param name="password">Clave</param>
        /// <param name="domain">Dominio</param>
        /// <returns></returns>
        public AuthenticationResult Autenthicate(string loginName, string password, string domain)
        {
            try
            {
                AuthenticationDAO authenticationProvider = new AuthenticationDAO();
                return authenticationProvider.Autenthicate(loginName, password, domain);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Logueo de Usuario
        /// </summary>
        /// <param name="loginName">Nombre Usuario</param>
        /// <param name="password">Clave</param>
        /// <param name="domain">Dominio</param>
        /// <param name="SessionID">Session</param>
        /// <returns></returns>
        public AuthenticationResult AutenthicateR2(string loginName, string password, string domain, string SessionID)
        {
            try
            {
                AuthenticationDAO authenticationProvider = new AuthenticationDAO();
                return authenticationProvider.AutenthicateR2(loginName, password, domain, SessionID);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene El usuario por Nombre
        /// </summary>
        /// <param name="name">Nombre Usuario</param>
        /// <returns></returns>
        public int GetUserId(string name)
        {
            try
            {
                AuthenticationDAO authenticationProvider = new AuthenticationDAO();
                return authenticationProvider.GetUserId(name);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene Controles de Seguridad
        /// </summary>
        /// <param name="viewID">Nombre de la Vista</param>
        /// <param name="userName">Nombre del Usuario</param>
        /// <returns></returns>
        public IList<ControlSecurity> GetControlsSecurity(string viewID, string userName)
        {
            try
            {
                AuthenticationDAO authenticationDAO = new AuthenticationDAO();
                return authenticationDAO.GetControlsSecurity(viewID, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los Modulos
        /// </summary>
        /// <param name="userName">Nombre del USuario</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Usuario Vacio</exception>
        public IList<Module> GetModules(string userName)
        {
            try
            {
                AuthorizationDAO authorizationDAO = new AuthorizationDAO();
                return authorizationDAO.GetModulesByUserName(userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nivel de operacion 
        /// </summary>
        public string GetLevelOperation()
        {
            try
            {
                string levelOperation=string.IsNullOrEmpty(ConfigurationManager.AppSettings["LevelOperation"]) ? "" : ConfigurationManager.AppSettings["LevelOperation"];
                List<string> List = new List<string>(levelOperation.Split('/'));
                if (List.Count>1)
                {
                    if (List[List.Count - 1] != "" && List[List.Count - 1] != "..")
                    {
                        List.Remove(List[List.Count - 1]);
                    }
                    levelOperation = string.Join("/", List);
                }
                return levelOperation;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el Usuario Por Nombre
        /// </summary>
        /// <param name="name">Nombre Usuario</param>
        /// <returns>Modelo de usuario</returns>
        /// <exception cref="BusinessException"></exception>
        public MOD.User GetUserByName(string name)
        {
            try
            {
                AuthenticationDAO authenticationDAO = new AuthenticationDAO();
                return authenticationDAO.GetUserByName(name);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void UnlockPassword(int UserId)
        {
            try
            {
                DelegateService.uniqueUserServiceCore.UnlockPassword(UserId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo que obtiene la informacion de lo accesos por usuairo y con llave AccessObjectType par el tipo de menu
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IList<Module> GetModulesAccess(string userName)
        {
            try
            {
                int accessObjectsType = int.Parse(ConfigurationManager.AppSettings.Get("AccessObjectType"));
                AuthorizationDAO authorizationDAO = new AuthorizationDAO();
                return authorizationDAO.GetModulesByUserNameSql(userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


    }
}