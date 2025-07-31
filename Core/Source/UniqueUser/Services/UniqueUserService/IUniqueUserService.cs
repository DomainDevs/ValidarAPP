using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using MlCommon = Sistran.Core.Application.CommonService.Models;



namespace Sistran.Core.Application.UniqueUserServices
{
    [ServiceContract]
    public interface IUniqueUserServiceCore
    {

        /// <summary>
        /// Obtener usuario por accountName
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>Models User</returns>
        [OperationContract]
        Task<List<Models.User>> GetUserByAccountName(string accountName, int personId, int userId, bool getAllData);

        /// <summary>
        /// Guarda el objeto User
        /// </summary>
        /// <param name="User">Model User</param>
        /// <returns>User</returns>
        [OperationContract]
        int CreateUniqueUser(User user);

        /// <summary>
        /// Gets the person by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [OperationContract]
        UserPerson GetPersonByUserIdOrPersonId(int userId, int personId);

        /// <summary>
        /// Obtener Modulos
        /// </summary>
        /// <returns>Lista de Module</returns>  
        [OperationContract]
        List<Module> GetModulesByDescription(string description);

        /// <summary>
        /// Obtener SubModule
        /// </summary>
        /// <param name="moduleId">moduleId.</param>
        /// <returns>Lista de SubModule</returns>  
        [OperationContract]
        List<SubModule> GetSubModulesByModuleId(int moduleId);

        /// <summary>
        /// Obtener SubModule
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        [OperationContract]
        List<Models.CoHierarchyAssociation> GetCoHierarchiesAssociationByModuleSubModule(int moduleId, int subModuleId);

        /// <summary>
        /// Obtener Perfiles
        /// </summary>
        /// <returns>Models Profile</returns>
        [OperationContract]
        List<Models.Profile> GetProfilesByDescription(string description, int idProfile);

        /// <summary>
        /// Guardar Modulos
        /// </summary>
        /// <returns>Lista de Module</returns>  
        [OperationContract]
        List<Models.Module> CreateModules(List<Models.Module> modules);

        /// <summary>
        /// Guardar SubModulos
        /// </summary>
        /// <returns>Lista de SubModule</returns>  
        [OperationContract]
        List<Models.SubModule> CreateSubModules(List<Models.SubModule> submodules);

        /// <summary>
        /// Guardar Perfil
        /// </summary>
        /// <returns>bool</returns>  
        [OperationContract]
        int CreateProfile(Models.Profile profile);

        /// <summary>
        /// Obtiene los Datos Completos del Usuario 
        /// </summary>
        /// <param name="account">la cuenta de usuario.</param>
        /// <returns></returns>
        [OperationContract]
        List<User> GetUserPersonsByAccount(string account);

        /// <summary>
        /// Actualiza datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        UserPerson GetPersonByUserId(int userId);

        /// <summary>
        /// Obtener Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Lista De Agencias</returns>
        [OperationContract]
        List<UserAgency> GetAgenciesByUserIdAgentIdDescription(int userId, int agentId, string description);

        /// <summary>
        /// Obtener Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Lista De Agencias</returns>
        [OperationContract]
        List<UserAgency> GetAgenciesByUserId(int userId);


        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        //[OperationContract]
        //List<MlPerson.InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId);

        /// <summary>
        /// Obtener lista de AccessProfile
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        [OperationContract]
        List<AccessObject> GetAccessObject(bool onlyButtons);

        /// <summary>
        /// Obtener lista de GetAccessObjectByModuleIdSubModuleId
        /// </summary>
        /// <param name="moduleId">Id modulo/param>
        /// <param name="subModuleId">Id subModule/param>
        /// <param name="typeId">Id type/param>
        /// <returns>Lista De Accesos</returns>
        [OperationContract]
        List<AccessObject> GetAccessObjectByModuleIdSubModuleId(int moduleId, int subModuleId, int typeId, int profileId, int parentId);

        /// <summary>
        /// Guarda los accesos
        /// </summary>
        /// <param name="accesses">list AccessObject</param>
        /// <returns>bool</returns>
        [OperationContract]
        List<Models.AccessObject> CreateAccessObject(List<AccessObject> accesses);
        /// <summary>
        /// Copia un perfil y crea uno nuevo
        /// </summary>
        /// <param name="profile">profile</param>
        /// <returns>string</returns>
        [OperationContract]
        bool CopyProfile(Profile profile);

        /// <summary>
        /// Bloqueo de usuario
        /// </summary>
        /// <param name="userName">userName</param>
        [OperationContract]
        void UpdateLockPasswordByUserName(string userName);

        /// <summary>
        /// Consulta de usuarios
        /// </summary>
        /// <param name="userName">userName</param>
        [OperationContract]
        List<User> GetUserByTextPersonId(string text, int personId);

        /// <summary>
        /// Listado de Usuario Por Filtro
        /// </summary>
        /// <param name="user">Filtro</param>
        /// <returns>Users</returns>
        [OperationContract]
        List<User> GetUsersByUser(User user);

        /// <summary>
        /// Obtener lista de ButtonsByModuleIdSubModuleId
        /// </summary>       
        /// <returns>Lista De AccessObject</returns>
        [OperationContract]
        List<AccessObject> GetButtonsByUserName(string userName);
        /// <summary>
        /// consulta si existe perfil con el nombre
        /// </summary>
        /// <param name="description">description</param>
        /// <returns>bool</returns>
        [OperationContract]
        bool GetProfileByDescription(string description);

        /// <summary>
        /// Obtener lista de AccessObject por filtro
        /// </summary>       
        /// <returns>Lista De Accesos</returns>
        [OperationContract]
        List<AccessObject> GetAccessesByAccess(AccessObject accessObject);

        /// <summary>
        /// Asigna todos los accesos al perfil
        /// </summary>       
        [OperationContract]
        void AssingAllAccess(int moduleId, int subModuleId, int profileId, bool active);

        /// <summary>
        /// Obtener Agencias del usuario
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<UserAgency> GetAgenciesByAgentIdDescriptionUserId(int agentId, string description, int userId);

        /// <summary>
        /// Obtener lista de agencias por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        [OperationContract]
        List<UserAgency> GetAgenciesByAgentIdUserId(int agentId, int userId);

        ///// <summary>
        ///// Asigna todos los pdv por sucursal y usuario
        ///// </summary>       
        //[OperationContract]
        //void AssingAllPdvByBranchIdUserId(int branchId, int userId, bool active);

        /// <summary>
        /// Asigna todos las sucursal por usuario
        /// </summary>       
        [OperationContract]
        void AssingAllBranchByUserId(int userId, bool active);


        /// <summary>
        /// Generar Archivo Modulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        [OperationContract]
        string GenerateFileToModules(List<Module> modules, string fileName);

        /// <summary>
        /// Generar Archivo subModulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        [OperationContract]
        string GenerateFileToSubmodules(List<SubModule> modules, string fileName);

        /// <summary>
        /// Generar Archivo Modulo
        /// </summary>
        /// <param name="module">Modulos</param>
        /// <returns>Ruta Archivo</returns>
        [OperationContract]
        string GenerateFileToAccess(List<AccessObject> modules, string fileName);

        /// <summary>
        /// Generar Archivo Profile
        /// </summary>
        /// <param name="access">accesos</param>
        /// <returns>Ruta Archivo</returns>
        [OperationContract]
        string GenerateFileToProfiles(List<Profile> profiles, string fileName);

        /// <summary>
        /// Obtener cantidad Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>numero agencias</returns>
        [OperationContract]
        int GetCountAgenciesByUserId(int userId);

        /// <summary>
        /// Generate sha256
        /// </summary> 
        /// <param name="password">clear password</param>
        /// <param name="salt">random number</param>
        /// <returns>password hashed</returns>
        [OperationContract]
        string GetHashSha256(string password, string salt);

        /// <summary>
        /// controla los intentos de autenticación correctos
        /// </summary>
        /// <param name="UserId">UserId</param>
        [OperationContract]
        void UnlockPassword(int UserId);

        /// <summary>
        /// controla los intentos de autenticación correctos
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="SessionID"></param>
        [OperationContract]
        void UnlockPasswordR2(int UserId, string SessionID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateFailedPassword(int UserId, out int attempts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="attempts"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateFailedPasswordR2(int UserId, out int attempts, string SessionID);


        /// <summary>
        /// Actualización de password
        /// </summary>
        /// <param name="UniqueUserssLogin"></param>
        [OperationContract]
        void UpdatePassword(int userId, string password, int expirationDays);

        /// <summary>
        /// Obtener puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Lista de puntos de venta</returns>
        [OperationContract]
        List<MlCommon.SalePoint> GetSalePointsByBranchIdUserId(int branchId, int userId);


        /// <summary>
        /// Obtener lista de sucursales asociadas a un usuario
        /// </summary>
        /// <param name="userId">Identificador usuario</param>
        /// <returns></returns>
        [OperationContract]
        List<MlCommon.Branch> GetBranchesByUserId(int userId);

        [OperationContract]
        MlCommon.Branch GetDefaultBranchesByUserId(int userId);

        /// <summary>
        /// Obtener Puntos de Venta por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Puntos de Venta</returns>
        [OperationContract]
        List<MlCommon.SalePoint> GetSalePointsByUserId(int userId);

        /// <summary>
        /// Obtiene el usuario por el Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        User GetUserById(int userId);

        [OperationContract]
        List<User> GetUserByName(string name);

        [OperationContract]
        List<User> GetCountUsersById(int userId);

        /// <summary>
        /// Consulta los usuarios en la tabla CoHierarchyAccesses segun los parametros
        /// </summary>
        /// <param name="idHierarchy">id de jerarquia</param>
        /// <param name="idModule">id modulo</param>
        /// <param name="idSubmodule">id del submodulo</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.User> GetUsersByHierarchyModuleSubmodule(int idHierarchy, int idModule, int idSubmodule);


        [OperationContract]
        bool ChangePassword(bool changePassword, string login, string oldPassword, string newPassword, out Dictionary<int, int> errors);

        [OperationContract]
        Models.User GetUserByLogin(string login);

        /// <summary>
        /// Get User By individualId
        /// </summary>
        /// <param name="individualId">individual Id</param>
        /// <returns>retorna el usuario</returns>
        [OperationContract]
        User GetUserByIndividualId(int individualId);

        /// <summary>
        /// Valida que el usuario tenga al menos un perfil habilitado
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>valor verdadero: perfil habilitado, falso: perfil deshabilitado</returns>
        [OperationContract]
        bool ValidateProfileByUserId(int userId);


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
        [OperationContract]
        List<User> GetUserByAdvancedSearch(string accountName, int? userId, string identificationNumber, DateTime? creationDate, int? status, DateTime? lastModificationDate);
        /// <summary>
        /// Obtiene el listado de productos validando cuales pertenecen al usuario seleccionado.
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>Listado de productos</returns>
        [OperationContract]
        List<UniqueUsersProduct> GetUniqueUserProductsStatusByUserId(int userId);


        /// <summary>
        /// Get User By AccountName 
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>Models User</returns>
        [OperationContract]
        List<User> GetUsersByAccountName(string accountName, int userId, int personId);

        #region Notification

        [OperationContract]
        void CreateNotifications(List<NotificationUser> notifications);

        [OperationContract]
        void CreateNotification(NotificationUser notification);

        [OperationContract]
        List<NotificationUser> GetNotificationByUser(int userId, bool? enabled, bool maxRow);

        [OperationContract]
        NotificationUser GetNotificationQueue();

        [OperationContract]
        void UpdateNotification(NotificationUser notification);
        [OperationContract]
        void UpdateNotificationParameter(int notificationId);
        /// <summary>
        /// Obtiene las notificacion por usuario
        /// </summary>
        /// <param name="idUser">Id del ususario</param>
        /// <param name="enabled">notififcaciones habilitadas</param>
        /// <returns>Cantidad de registros</returns>
        [OperationContract]
        int GetNotificationCountByUser(int userId, bool? enabled);

        [OperationContract]
        List<NotificationUser> UpdateAllNotificationDisabledByUser(int userId);
        #endregion


        /// <summary>
        /// guardar los Ramos Por Usuario
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        PrefixUser SavePrefixesByUserId(PrefixUser prefixUser);


        /// <summary>
        /// eliminar los Ramos Por Usuario
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void DeletePrefixesByUserId(PrefixUser prefixUser);

        /// <summary>
        /// Get services date
        /// </summary>
        /// <returns>Services date</returns>
        [OperationContract]
        DateTime GetDate();

        /// <summary>
        /// Get AccessPermissions
        /// </summary>
        /// <returns>AccessPermissions</returns>
        [OperationContract]
        List<AccessPermissions> GetAccessPermissionsByUserId(int userId);

        /// <summary>
        /// Get GetAccessPermissionsByProfileId
        /// </summary>
        /// <returns>AccessPermissions</returns>
        [OperationContract]
        List<AccessPermissions> GetAccessPermissionsByProfileId(int profileId);



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns>AccessPermissions</returns>
        //[OperationContract]

        //List<AccessPermissions> GetAccessPermissionsByModuleIdSubModuleIdProfileId(int moduleId, int subModuleId, int ProfileId);


        /// <summary>
        /// 
        /// </summary>
        /// <returns>AccessPermissions</returns>
        [OperationContract]
        List<AccessPermissions> GetAccessPermissionsBysubmoduleCode(int moduleCode, int submoduleCode);

        /// <summary>
        /// contextos  permitidods por usuario 
        /// </summary>
        /// <returns>AccessObject</returns>
        [OperationContract]
        List<SecurityContext> GetSecurityContextByProfileIdpermissionsId(int userId, int permissionsId);

        /// <summary>
        /// Guardar contextos permistidos 
        /// </summary>
        /// <returns>Lista de Module</returns>  
        [OperationContract]
        int SaveContextPermissions(List<Models.ContextProfileAccessPermissions> ContextPermissions);

        [OperationContract]
        UniqueUserSession TryInitSession(Models.UniqueUserSession uniqueUserSession);

        [OperationContract]
        UniqueUserSession GetUserInSessionBySessionId(string SessionId);

        [OperationContract]
        bool DeletetUserSession(int userId);

        [OperationContract]
        UniqueUserSession GetUserInSession(string accountName);

        /// <summary>
        /// Obtiene listado de estados de contragarantias  por perfiles
        /// </summary>
        /// <returns> Listado de estados de contragarantías por perfiles </returns>
        [OperationContract]
        List<ProfileGuaranteeStatus> GetProfileGuaranteeStatus(int profileId);

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        List<UserAgency> GetAgenciesByIndividualIdAgentId(string description);

    }

}
