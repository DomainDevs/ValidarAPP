using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Helper;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.UtilitiesServices.Enums;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using COMMML = Sistran.Core.Application.UniqueUserServices.Models;
using MlCommon = Sistran.Core.Application.CommonService.Models;
using TM = System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniqueUserServiceEEProviderCore : IUniqueUserServiceCore
    {


        /// <summary>
        /// Obtener usuario por accountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>Models User</returns>
        public async TM.Task<List<Models.User>> GetUserByAccountName(string accountName, int personId, int userId, bool getAllData)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return await TP.Task.Run(() =>
                {
                    var result = userDAO.GetUserByAccountName(accountName, personId, userId, getAllData);
                    DataFacadeManager.Dispose();
                    return result;
                }
                );
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda el objeto User
        /// </summary>
        /// <param name="User">Model User</param>
        /// <returns>User</returns>
        public int CreateUniqueUser(User user)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return userDAO.CreateUniqueUser(user);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the person by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public UserPerson GetPersonByUserIdOrPersonId(int userId, int personId)
        {
            try
            {
                UniquePersonDAO dao = new UniquePersonDAO();
                return dao.GetPersonByUserIdOrPersonId(userId, personId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Modulos
        /// </summary>
        /// <returns>Lista de Module</returns>  
        public List<Models.Module> GetModulesByDescription(string description)
        {
            try
            {
                ModuleDAO moduleDAO = new ModuleDAO();
                return moduleDAO.GetModulesByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar Modulos
        /// </summary>
        /// <returns>Lista de Module</returns>  
        public List<Models.Module> CreateModules(List<Models.Module> modules)
        {
            try
            {
                ModuleDAO moduleDAO = new ModuleDAO();
                return moduleDAO.CreateModules(modules);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar Modulos
        /// </summary>
        /// <returns>Lista de Module</returns>  
        public List<Models.SubModule> CreateSubModules(List<Models.SubModule> submodules)
        {
            try
            {
                SubModuleDAO submoduleDAO = new SubModuleDAO();
                return submoduleDAO.CreateSubModules(submodules);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener SubModule
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        public List<Models.SubModule> GetSubModulesByModuleId(int moduleId)
        {
            try
            {
                SubModuleDAO submoduleDAO = new SubModuleDAO();
                return submoduleDAO.GetSubModulesByModuleId(moduleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener SubModule
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        public List<Models.CoHierarchyAssociation> GetCoHierarchiesAssociationByModuleSubModule(int moduleId, int subModuleId)
        {
            try
            {
                CoHierarchyAssociationDAO cohierarchyassociationDAO = new CoHierarchyAssociationDAO();
                return cohierarchyassociationDAO.GetCoHierarchiesAssociationByModuleSubModule(moduleId, subModuleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene el Listado de perfiles
        /// </summary>
        /// <param name="tempId">id del temporal</param>
        /// <returns></returns>
        public List<Models.Profile> GetProfilesByDescription(string description, int idProfile)
        {
            try
            {
                ProfileDAO profileDAO = new ProfileDAO();
                return profileDAO.GetProfilesByDescription(description, idProfile);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar Perfil
        /// </summary>
        /// <returns>bool</returns>  
        public int CreateProfile(Models.Profile profile)
        {
            try
            {
                ProfileDAO profileDAO = new ProfileDAO();
                return profileDAO.SaveProfile(profile);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los Datos Completos del Usuario 
        /// </summary>
        /// <param name="account">la cuenta de usuario.</param>
        /// <returns></returns>
        public List<User> GetUserPersonsByAccount(string account)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetUserPersonsByAccount(account);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Persona por Id de Usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// Person
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public UserPerson GetPersonByUserId(int userId)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetPersonByUserIdOrPersonId(userId, 0);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Lista De Agencias</returns>
        public List<UserAgency> GetAgenciesByUserIdAgentIdDescription(int userId, int agentId, string description)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetAgenciesByUserIdAgentIdDescription(userId, agentId, description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Lista De Agencias</returns>CreateUniqueUser
        public List<UserAgency> GetAgenciesByUserId(int userId)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetAgenciesByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>
        /// Listado de Bitacora de asegurado y garantia
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        //public List<MlPerson.InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId)
        //{
        //    try
        //    {
        //        UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
        //        return uniquepersonDAO.GetInsuredGuaranteeLogs(individualId, guaranteeId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Obtener lista de AccessProfile
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetAccessObject(bool onlyButtons)
        {
            try
            {
                AccessProfileDAO accessprofileDAO = new AccessProfileDAO();
                return accessprofileDAO.GetAccessObject(onlyButtons);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener lista de GetAccessObjectByModuleIdSubModuleId
        /// </summary>
        /// <param name="moduleId">Id modulo/param>
        /// <param name="subModuleId">Id subModule/param>
        /// <param name="typeId">Id type/param>
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetAccessObjectByModuleIdSubModuleId(int moduleId, int subModuleId, int typeId, int profileId, int parentId)
        {
            try
            {
                if (typeId == (int)AccessObjectType.PERMISION)
                {
                    AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
                    var permissions = accessPermissionsDAO.GetAccessPermissionsBysubmoduleCode(moduleId, subModuleId);
                    List<AccessObject> accessObjects = ModelAssembler.CreateAccessObjects(permissions);
                    var accessPermissionsProfiles = accessPermissionsDAO.GetAccessPermissionsByModuleIdSubModuleIdProfileId(moduleId, subModuleId, profileId);
                    foreach (var accessPermissionsProfile in accessPermissionsProfiles)
                    {
                        foreach (var accessObject in accessObjects)
                        {
                            if (accessObject.AccessId == accessPermissionsProfile.AccessPermissionsId)
                            {
                                accessObject.Assigned = true;
                                accessObject.AccessObjectId = accessPermissionsProfile.Id;
                                break;
                            }
                        }
                    }
                    return accessObjects;
                }
                else
                {
                    AccessProfileDAO accessprofileDAO = new AccessProfileDAO();
                    return accessprofileDAO.GetAccessObjectByModuleIdSubModuleId(moduleId, subModuleId, typeId, profileId, parentId);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda los accesos
        /// </summary>
        /// <param name="accesses">list AccessObject</param>
        /// <returns>bool</returns>
        public List<Models.AccessObject> CreateAccessObject(List<AccessObject> accesses)
        {
            try
            {
                AccessProfileDAO accessprofileDAO = new AccessProfileDAO();
                return accessprofileDAO.CreateAccessObject(accesses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Copia un perfil y crea uno nuevo
        /// </summary>
        /// <param name="profile">profile</param>
        /// <returns>string</returns>
        public bool CopyProfile(Profile profile)
        {
            try
            {
                ProfileDAO accessprofileDAO = new ProfileDAO();
                return accessprofileDAO.CopyProfile(profile);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Bloqueo de usuario
        /// </summary>
        /// <param name="userName">userName</param>
        public void UpdateLockPasswordByUserName(string userName)
        {
            try
            {
                UserDAO.UpdateLockPasswordByUserName(userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta de usuarios
        /// </summary>
        /// <param name="userName">userName</param>
        public List<User> GetUserByTextPersonId(string text, int personId)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return userDAO.GetUserByTextPersonId(text, personId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listado de Usuario Por Filtro
        /// </summary>
        /// <param name="user">Filtro</param>
        /// <returns>Users</returns>
        public List<User> GetUsersByUser(User user)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return userDAO.GetUsersByUser(user);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de ButtonsByModuleIdSubModuleId
        /// </summary>       
        /// <returns>Lista De AccessObject</returns>
        public List<AccessObject> GetButtonsByUserName(string userName)
        {
            try
            {
                AccessProfileDAO accessprofileDAO = new AccessProfileDAO();
                return accessprofileDAO.GetButtonsByUserName(userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// consulta si existe perfil con el nombre
        /// </summary>
        /// <param name="description">description</param>
        /// <returns>bool</returns>
        public bool GetProfileByDescription(string description)
        {
            try
            {
                ProfileDAO profileDAO = new ProfileDAO();
                return profileDAO.GetProfileByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de AccessObject por filtro
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        public List<AccessObject> GetAccessesByAccess(AccessObject accessObject)
        {
            try
            {
                AccessProfileDAO accessprofileDAO = new AccessProfileDAO();
                return accessprofileDAO.GetAccessesByAccess(accessObject, true);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Asigna todos los accesos al perfil
        /// </summary>       
        public void AssingAllAccess(int moduleId, int subModuleId, int profileId, bool active)
        {
            try
            {
                AccessProfileDAO accessprofileDAO = new AccessProfileDAO();
                accessprofileDAO.AssingAllAccess(moduleId, subModuleId, profileId, active);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Agencias del usuario
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<UserAgency> GetAgenciesByAgentIdDescriptionUserId(int agentId, string description, int userId)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetAgenciesByAgentIdDescriptionUserId(agentId, description, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de agencias por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<UserAgency> GetAgenciesByAgentIdUserId(int agentId, int userId)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetAgenciesByAgentIdUserId(agentId, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        ///// <summary>
        ///// Asigna todos los pdv por sucursal y usuario
        ///// </summary>       
        //public void AssingAllPdvByBranchIdUserId(int branchId, int userId, bool active)
        //{
        //    try
        //    {
        //        UserDAO userDAO = new UserDAO();
        //        userDAO.AssingAllPdvByBranchIdUserId(branchId, userId, active);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Asigna todos las sucursal por usuario
        /// </summary>       
        public void AssingAllBranchByUserId(int userId, bool active)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                userDAO.AssingAllBranchByUserId(userId, active);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar Archivo Modulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToModules(List<Module> modules, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToModules(modules, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar Archivo Modulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToSubmodules(List<SubModule> modules, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToSubmodules(modules, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar Archivo Modulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToAccess(List<AccessObject> access, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToAccess(access, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar Archivo perfiles
        /// </summary>
        /// <param name="perfiles">perfiles</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToProfiles(List<Profile> profiles, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToProfiles(profiles, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener cantidad Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>numero agencias</returns>
        public int GetCountAgenciesByUserId(int userId)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetCountAgenciesByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generate sha256
        /// </summary> 
        /// <param name="password">clear password</param>
        /// <param name="salt">random number</param>
        /// <returns>password hashed</returns>
        public string GetHashSha256(string password, string salt)
        {
            try
            {
                return SecurityHelper.GetHashSha256(password, salt);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// controla los intentos de autenticación correctos
        /// </summary>
        /// <param name="UserId">UserId</param>
        public void UnlockPassword(int UserId)
        {
            try
            {
                UniqueUsersLoginDAO.UnlockPassword(UserId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// controla los intentos de autenticación correctos
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="SessionID"></param>
        public void UnlockPasswordR2(int UserId, string SessionID)
        {
            try
            {
                UniqueUsersLoginDAO.UnlockPasswordR2(UserId, SessionID);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public bool UpdateFailedPassword(int UserId, out int attempts)
        {
            try
            {
                return UniqueUsersLoginDAO.UpdateFailedPassword(UserId, out attempts);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="attempts"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public bool UpdateFailedPasswordR2(int UserId, out int attempts, string SessionID)
        {
            try
            {
                return UniqueUsersLoginDAO.UpdateFailedPasswordR2(UserId, out attempts, SessionID);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualización de password
        /// </summary>
        /// <param name="UniqueUserssLogin"></param>
        public void UpdatePassword(int userId, string password, int expirationDays)
        {
            try
            {
                UniqueUsersLoginDAO dao = new UniqueUsersLoginDAO();
                dao.UpdatePassword(userId, password, expirationDays);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de sucursales
        /// </summary>
        /// <param name="userId">Identificador usuario</param>
        /// <returns></returns>
        public List<MlCommon.Branch> GetBranchesByUserId(int userId)
        {
            try
            {
                UserBranchDAO userBranchProvider = new UserBranchDAO();
                return userBranchProvider.GetBranchesByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Lista de puntos de venta</returns>
        public List<MlCommon.SalePoint> GetSalePointsByBranchIdUserId(int branchId, int userId)
        {
            try
            {
                UserSalePointDAO salePointProvider = new UserSalePointDAO();
                return salePointProvider.GetSalePointsByBranchIdUserId(branchId, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MlCommon.Branch GetDefaultBranchesByUserId(int userId)
        {
            try
            {
                UserBranchDAO userBranch = new UserBranchDAO();
                return userBranch.GetDefaultBranchesByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Puntos de Venta por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Puntos de Venta</returns>
        public List<MlCommon.SalePoint> GetSalePointsByUserId(int userId)
        {
            try
            {
                UserSalePointDAO salePoint = new UserSalePointDAO();
                return salePoint.GetSalePointsByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el usuario por el Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            UserDAO dao = new UserDAO();
            return dao.GetUserById(userId);
        }

        public List<User> GetUserByName(string name)
        {
            UserDAO dao = new UserDAO();
            return dao.GetUserByName(name);
        }

        public List<User> GetCountUsersById(int userId)
        {
            UserDAO dao = new UserDAO();
            return dao.GetCountUsersById(userId);
        }

        /// <summary>
        /// Consulta los usuarios en la tabla CoHierarchyAccesses segun los parametros
        /// </summary>
        /// <param name="idHierarchy">id de jerarquia</param>
        /// <param name="idModule">id modulo</param>
        /// <param name="idSubmodule">id del submodulo</param>
        /// <returns></returns>
        public List<Models.User> GetUsersByHierarchyModuleSubmodule(int idHierarchy, int idModule, int idSubmodule)
        {
            try
            {
                UserDAO userDao = new UserDAO();
                return userDao.GetUsersByHierarchyModuleSubmodule(idHierarchy, idModule, idSubmodule);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error en GetUsersByHierarchyModuleSubmodule", e);
            }
        }

        public Models.User GetUserByLogin(string login)
        {
            try
            {
                UserDAO dao = new UserDAO();
                return dao.GetUserByLogin(login);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool ChangePassword(bool changePassword, string login, string oldPassword, string newPassword, out Dictionary<int, int> errors)
        {
            try
            {
                UniqueUsersLoginDAO dao = new UniqueUsersLoginDAO();
                return dao.ChangePassword(changePassword, login, oldPassword, newPassword, out errors);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get User By individualId
        /// </summary>
        /// <param name="individualId">individual Id</param>
        /// <returns>retorna el usuario</returns>
        public User GetUserByIndividualId(int individualId)
        {
            try
            {
                UserDAO dao = new UserDAO();
                return dao.GetUserByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida que el usuario tenga al menos un perfil habilitado
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>valor verdadero: perfil habilitado, falso: perfil deshabilitado</returns>
        public bool ValidateProfileByUserId(int userId)
        {
            try
            {
                ProfileUserDAO profileUserDAO = new ProfileUserDAO();
                return profileUserDAO.ValidateProfileByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtiene los usuario de acuerdo a los filtros
        /// </summary>
        /// <param name="accountName">Nombre de usuario</param>
        /// <param name="userId">Id de usuario</param>
        /// <param name="identificationNumber">Nro de identificacion de la persona</param>
        /// <param name="creationDate">Fecha de creacion</param>
        /// <param name="status">Estado del usuario (Activado, Desactivado o Todos)</param>
        /// <param name="lastModificationDate">Ultima Fecha de modificacion</param>
        /// <returns>Listado de usuarios</returns>
        public List<User> GetUserByAdvancedSearch(string accountName, int? userId, string identificationNumber, DateTime? creationDate, int? status, DateTime? lastModificationDate)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return userDAO.GetUserByAdvancedSearch(accountName, userId, identificationNumber, creationDate, status, lastModificationDate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el listado de productos validando cuales pertenecen al usuario seleccionado.
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>Listado de productos</returns>
        public List<UniqueUsersProduct> GetUniqueUserProductsStatusByUserId(int userId)
        {
            UniqueUsersProductDAO uniqueUsersProductDAO = new UniqueUsersProductDAO();
            List<UniqueUsersProduct> listUniqueUsersProduct = uniqueUsersProductDAO.GetUniqueUsersProductsStatusByUserId(userId);
            return listUniqueUsersProduct;
        }

        /// <summary>
        /// Get User By AccountName 
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>Models User</returns>
        public List<User> GetUsersByAccountName(string accountName, int userId, int personId)
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return userDAO.GetUsersByAccountName(accountName, userId, personId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Notification

        /// <summary>
        /// Crea una nueva notificacion
        /// </summary>
        /// <param name="notification">Notificacion a crear</param>
        public void CreateNotification(COMMML.NotificationUser notification)
        {
            try
            {
                NotificationDAO notificationDao = new NotificationDAO();
                notificationDao.CreateNotification(notification);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea una lista de notificaciones
        /// </summary>
        /// <param name="notifications">Notificaciones a crear</param>
        public void CreateNotifications(List<COMMML.NotificationUser> notifications)
        {
            try
            {
                TP.Parallel.ForEach(notifications, x => { this.CreateNotification(x); DataFacadeManager.Dispose(); });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene las notificacion por usuario
        /// </summary>
        /// <param name="userId">Id del ususario</param>
        /// <param name="enabled">notififcaciones habilitadas</param>
        /// <param name="maxRow">cantidad limitada de registros</param>
        /// <returns>lista de notificacions</returns>
        public List<COMMML.NotificationUser> GetNotificationByUser(int userId, bool? enabled, bool maxRow)
        {
            try
            {
                var notificationDao = new NotificationDAO();
                return notificationDao.GetNotificationByUser(userId, enabled, maxRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Obtiene las notificacion por usuario
        /// </summary>
        /// <param name="idUser">Id del ususario</param>
        /// <param name="enabled">notififcaciones habilitadas</param>
        /// <returns>Cantidad de registros</returns>
        public int GetNotificationCountByUser(int userId, bool? enabled)
        {
            try
            {
                NotificationDAO notificationDao = new NotificationDAO();
                return notificationDao.GetNotificationCountByUser(userId, enabled);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza la notificacion
        /// </summary>
        /// <param name="notification">Notificacion a actualizar</param>
        public void UpdateNotification(COMMML.NotificationUser notification)
        {
            try
            {
                NotificationDAO notificationDao = new NotificationDAO();
                notificationDao.UpdateNotification(notification);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el parametro de la notificacion
        /// </summary>
        /// <param name="notificationId">id de la notificacion</param>
        public void UpdateNotificationParameter(int notificationId)
        {
            try
            {
                NotificationDAO notificationDao = new NotificationDAO();
                notificationDao.UpdateNotificationParameter(notificationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el parametro el habilitado
        /// </summary>
        /// <param name="userId">id del usuario</param>
        public List<COMMML.NotificationUser> UpdateAllNotificationDisabledByUser(int userId)
        {
            try
            {
                var notificationDao = new NotificationDAO();
                return notificationDao.UpdateAllNotificationDisabledByUser(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public COMMML.NotificationUser GetNotificationQueue()
        {
            var notificationDao = new NotificationDAO();
            return notificationDao.GetNotificationQueue();
        }


        #endregion

        public PrefixUser SavePrefixesByUserId(PrefixUser prefixUser)
        {
            try
            {
                PrefixUserDAO prefixDAO = new PrefixUserDAO();
                return prefixDAO.SavePrefixUser(prefixUser);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeletePrefixesByUserId(PrefixUser prefixUser)
        {
            try
            {
                PrefixUserDAO prefixDAO = new PrefixUserDAO();
                prefixDAO.DeletePrefixUser(prefixUser);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region date
        /// <summary>
        /// Get services date
        /// </summary>
        /// <returns>Services date</returns>
        public DateTime GetDate()
        {
            return BusinessServices.GetDate();
        }
        #endregion

        public List<AccessPermissions> GetAccessPermissionsByUserId(int userId)
        {
            AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
            return accessPermissionsDAO.GetAccessPermissionsByUserId(userId);
        }

        public List<AccessPermissions> GetAccessPermissionsByProfileId(int profileId)
        {
            AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
            return accessPermissionsDAO.GetAccessPermissionsByProfileId(profileId);
        }

        public List<AccessPermissions> GetAccessPermissionsBysubmoduleCode(int moduleCode, int submoduleCode)
        {
            AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
            return accessPermissionsDAO.GetAccessPermissionsBysubmoduleCode(moduleCode, submoduleCode);
        }

        /// <summary>
        /// Obtener lista de GetAccessObjectByModuleIdSubModuleId
        /// </summary>
        /// <param name="moduleId">Id modulo/param>
        /// <param name="subModuleId">Id subModule/param>
        /// <param name="typeId">Id type/param>
        /// <returns>Lista De Accesos</returns>
        public List<SecurityContext> GetSecurityContextByProfileIdpermissionsId(int profileId, int permissionsId)
        {
            try
            {
                AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
                var securityContexts = accessPermissionsDAO.GetSecurityContexts();

                var contextPermissionsUser = accessPermissionsDAO.GetcontextPermissionsByProfileIdPermissionsId(profileId, permissionsId);
                foreach (var contextPermissions in contextPermissionsUser)
                {
                    foreach (var securityContext in securityContexts)
                    {
                        if (securityContext.Id == contextPermissions.SecurityContext.Id)
                        {
                            securityContext.Assigned = true;
                            break;
                        }
                    }
                }
                return securityContexts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public int SaveContextPermissions(List<ContextProfileAccessPermissions> contextPermissions)
        {
            int contextPermissionsId = 0;
            AccessPermissionsDAO accessPermissionsDAO = new AccessPermissionsDAO();
            foreach (var item in contextPermissions)
            {
                contextPermissionsId = accessPermissionsDAO.SaveContextPermissions(item);
            };
            return contextPermissionsId;
        }
        public UniqueUserSession TryInitSession(Models.UniqueUserSession uniqueUserSession)
        {
            var uniqueUserSessionDAO = new UniqueUserSessionDAO();
            return uniqueUserSessionDAO.TryInitSession(uniqueUserSession);
        }
        public UniqueUserSession GetUserInSessionBySessionId(string SessionId)
        {
            var uniqueUserSessionDAO = new UniqueUserSessionDAO();
            return uniqueUserSessionDAO.GetUserInSessionBySessionId(SessionId);
        }

        public bool DeletetUserSession(int userId)
        {
            bool result = false;
            UniqueUserSessionDAO uniqueUserSessionDAO = new UniqueUserSessionDAO();
            result = uniqueUserSessionDAO.DeletetUserSession(userId);
            return result;
        }
        public UniqueUserSession GetUserInSession(string accountName)
        {
            UniqueUserSessionDAO uniqueUserSessionDAO = new UniqueUserSessionDAO();
            return uniqueUserSessionDAO.GetUserInSession(accountName);
        }
        //public Models.UniqueUserSession UpdateExpirationDate(int userId, DateTime ExpirationDate)
        //{
        //    var uniqueUserSessionDAO = new UniqueUserSessionDAO();
        //    return uniqueUserSessionDAO.UpdateExpirationDate(userId, ExpirationDate);
        //}


        /// <summary>
        /// Obtiene el listado de estados de contragarantías por perfiles 
        /// </summary>
        /// <returns>
        /// Listado de estados de contragarantías por perfiles 
        /// </returns>
        public List<Models.ProfileGuaranteeStatus> GetProfileGuaranteeStatus(int profileId)
        {
            try
            {
                ProfileDAO profileDAO = new ProfileDAO();
                return profileDAO.GetProfileGuaranteeStatus(profileId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene usuarios
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            try
            {
                UserDAO userDAO = new UserDAO();
                return userDAO.GetUsers();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta de intermediario por documento
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<UserAgency> GetAgenciesByIndividualIdAgentId(string description)
        {
            try
            {
                UniquePersonDAO uniquepersonDAO = new UniquePersonDAO();
                return uniquepersonDAO.GetAgenciesByIndividualIdAgentId(description);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
