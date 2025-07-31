using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using RUModels = Sistran.Core.Application.RulesScriptsServices.Models;
using UUModels = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices
{
    [ServiceContract]
    public interface IAuthorizationPoliciesServiceCore
    {
        /// <summary>
        /// Realiza la validacion de las politicas
        /// </summary>
        /// <param name="idPackage">Id del package de politicas</param>
        /// <param name="key">llave filtro</param>
        /// <param name="parameters">Lista de facades</param>
        /// <param name="facadeType"></param>
        /// <returns>Lista de politicas infringidas</returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPolicies(int idPackage, object key, object parameter, FacadeType facadeType);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesMassive(int idPackage, object key, object parameter, FacadeType facadeType, int hierarchy, List<int> ruleToValidate);

        [OperationContract]
        int GetHierarchyByIdUser(int idPackage, int idUser);

        [OperationContract]
        List<int> GetRulesToValidate(int idPackage, int hierarchy, string key, FacadeType facadeType);

        /// <summary>
        /// Obtiene la lista de usuarios autorizadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns>lista de usuarios autorizadores</returns>
        [OperationContract]
        List<UserAuthorization> GetUsersAutorizationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, List<UserGroupModel> usersGroup = null);

        /// <summary>
        /// Obtiene la lista de usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns>lista de usuarios notificadores</returns>
        [OperationContract]
        List<UserNotification> GetUsersNotificationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, List<UserGroupModel> usersGroup = null);

        /// <summary>
        /// Crea los usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <returns></returns>
        [OperationContract]
        void CreateUsersNotification(int idPolicies, List<UserNotification> users);

        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion 
        /// </summary>
        /// <param name="authorizationRequests">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        [OperationContract]
        void CreateAutorizationRequest(List<AuthorizationRequest> authorizationRequests);

        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion 
        /// </summary>
        /// <param name="authorizationRequests">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        [OperationContract]
        void CreateMassiveAutorizationRequest(List<AuthorizationRequest> authorizationRequests);

        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <returns></returns>
        [OperationContract]
        List<AuthorizationRequest> GetAuthorizationRequestsByKey(string key);

        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <param name="status">estado de la solicitud de autorizacion</param>
        /// <returns></returns>
        [OperationContract]
        List<AuthorizationRequest> GetAuthorizationRequestsByKeyStatus(string key, int status);

        /// <summary>
        /// Consulta las solicitudes de autorizacion por el usuario que solicita, fecha inicio/fin y los estados
        /// </summary>
        /// <param name="idUser">id usuario que solicita</param>
        /// <param name="status">lista de estados</param>
        /// <param name="dateInit">fecha inicial</param>
        /// <param name="dateEnd">fecha final</param>
        /// <returns></returns>
        [OperationContract]
        List<AuthorizationRequestGroup> GetAuthorizationRequestGroups(int idUser, List<int> status, DateTime dateInit, DateTime dateEnd);

        /// <summary>
        /// Consulta las solicitudes pendientes por autorizar
        /// </summary>
        /// <param name="groupPolicies"></param>
        /// <param name="status"></param>
        /// <param name="policies"></param>
        /// <param name="idUser"></param>
        /// <param name="userAuthorization"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.AuthorizationRequestGroup> GetAuthorizationRequestPendingGroups(int groupPolicies, int policies, int idUser, int userAuthorization, DateTime dateInit, DateTime dateEnd);

        [OperationContract]
        List<Models.AuthorizationRequestGroup> GetDetailsAuthorizationRequestGroups(int idUser, string key, int policiesId);

        ///  <summary>
        /// consulta las autorizacion de politicas segun el filtro
        ///  </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="idUser">Id del usuuario autorizador</param>
        /// <param name="status"> estado de la politica</param>
        /// <param name="dateInit">  fecha inicial</param>
        /// <param name="dateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        [OperationContract]
        List<AuthorizationAnswerGroup> GetAuthorizationAnswersByFilter(int? idGroup, int? idPolicies, int idUser, int status, DateTime? dateInit, DateTime? dateEnd, string sort);

        /// <summary>
        /// Actualiza Enabled =0
        /// </summary>
        /// <param name="key1">key1</param>
        /// <param name="key2">key2</param>
        [OperationContract]
        void UpdateAuthorizationAnswer(string key1, string key2);

        /// <summary>
        /// Actualiza todos los autorizatios answer Enabled = 0
        /// </summary>
        /// <param name="authorizationRequests"></param>
        [OperationContract]
        void UpdateAuthorizationAnswersByAuthorizationRequests(List<AuthorizationRequest> authorizationRequests, string userName);

        ///  <summary>
        /// consulta las autorizacion de politicas que han sido reasignadas por el usuario
        ///  </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="idUser">Id del usuuario autorizador</param>
        /// <param name="dateInit">  fecha inicial</param>
        /// <param name="dateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        [OperationContract]
        List<AuthorizationAnswerGroup> GetAuthorizationAnswersReasignByFilter(int? idGroup, int? idPolicies, int idUser, DateTime? dateInit, DateTime? dateEnd, string sort);

        [OperationContract]
        List<string> GetAuthorizationAnswerDescriptions(int idPolicies, int idUser, int status, string key);

        [OperationContract]
        List<string> GetAuthorizationAnswerDescription(int idPolicies, string key);

        /// <summary>
        /// consultar las jerarquias superiores parametrizadas a la politica
        /// </summary>
        /// <param name="policiesId">id de la politica</param>
        /// <param name="hierarchyId">jerarquia del usuario actual</param>
        /// <param name="userId">id del usuario actual</param>
        /// <returns>lista de las jerarquias autorizadoras</returns>
        [OperationContract]
        List<UUModels.CoHierarchyAssociation> GetAuthorizationHierarchy(int policiesId, int hierarchyId, int userId);

        /// <summary>
        /// Obtiene el listado del temporales que estan autorizados sin ser emitidos
        /// </summary>
        /// <param name="temporalId">numero de la operacion</param>
        /// <param name="userId">id del usuario</param>
        /// <returns>lista de temporales sin emitir</returns>
        [OperationContract]
        List<Models.IssueWithPolicies> GetIssueWithPolicies(int? temporalId, int userId);

        /// <summary>
        /// consultar los usuarios autorizadores de la politica en esa jerarquia
        /// </summary>
        /// <param name="autorizatioAnswerId">id de la autorizacion</param>
        /// <param name="hierarchyId">jerarquia autorizadora</param>
        /// <returns>lista usuarios autorizadores de la jerarquia</returns>
        [OperationContract]
        List<UUModels.User> GetUsersAuthorizationHierarchy(int autorizatioAnswerId, int hierarchyId);


        /// <summary>
        /// Reasigna una politica
        /// </summary>
        /// <param name="policiesId"></param>
        /// <param name="userAnswerId"></param>
        /// <param name="key"></param>
        /// <param name="hierarchyId"></param>
        /// <param name="userReasignId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [OperationContract]
        void ReasignAuthorizationAnswer(int policiesId, int userAnswerId, string key, int hierarchyId, int userReasignId, string reason, List<int> policiesToReassign, int userReassigning, Enums.TypeFunction functionType);

        /// <summary>
        /// Obtiene el historias de las autorizaciones reasignadas
        /// </summary>
        /// <param name="policiesId"></param>
        /// <param name="userAnswerId"></param>
        /// <param name="key"></param>
        /// <param name="userId">usuario de la consulta</param>
        /// <returns></returns>
        [OperationContract]
        List<Reasign> GetHistoryReasign(int policiesId, int userAnswerId, string key);

        /// <summary>
        /// Obtiene las jerarquias por grupo de politicas
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoHierarchyAssociation> GetHierarchyByGroupPolicies(int groupId);

        /// <summary>
        /// Obtiene los conceptos asignados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <returns></returns>
        [OperationContract]
        List<ConceptDescription> GetConceptDescriptionsByIdPolicies(int idPolicies);

        /// <summary>
        /// Guarda los conceptos asociados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="conceptDescriptions">lista de conceptos</param>
        /// <returns></returns>
        [OperationContract]
        void SaveConceptDescriptions(int idPolicies, List<ConceptDescription> conceptDescriptions);

        /// <summary>
        /// Autoriza  autorizacions pendientes
        /// </summary>
        /// <param name="policiesId"></param>
        /// <param name="userAnswerId"></param>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [OperationContract]
        void AcceptAuthorization(int policiesId, int userAnswerId, string key, string reason, List<int> policiesToAccept, Enums.TypeFunction functionType);


        /// <summary>
        /// rechazar autorizaciones pendientes
        /// </summary>
        /// <param name="answers">lista de autorizaciones a rechazar</param>
        /// <returns></returns>
        [OperationContract]
        void RejectAuthorization(int policiesId, int userAnswerId, string key, string reason, int idRejection, List<int> policiesToReject, Enums.TypeFunction functionType);

        [OperationContract]
        List<AuthorizationAnswer> GetAuthorizationAnswersByRequestId(int requestId);

        /// <summary>
        /// obtiene los grupos de politicas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GroupPolicies> GetGroupsPolicies();

        /// <summary>
        /// Obtiene usuarios autorizadores
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<User> GetUsersAuthorization(int groupId, int policiesId);

        /// <summary>
        /// Realiza el filtro de las politicas
        /// </summary>
        /// <param name="groupPolicyId">Id grupo de politicas</param>
        /// <param name="typePolicyId">tipo de la politica</param>
        /// <param name="levelId">nivel de la politica</param>
        /// <param name="name">Nombre de la politica</param>
        /// <param name="message">mensaja de la politica</param>
        /// <param name="enable">si la politica esta habilitada</param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> GetPoliciesByFilter(int? groupPolicyId, int? typePolicyId, int? levelId, string name, string message, bool enable);

        /// <summary>
        /// obtiene las de politicas del grupo 
        /// </summary>
        /// <param name="idGroup">id del grupo de politicas</param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> GetPoliciesByIdGroup(int idGroup);

        /// <summary>
        /// Obtiene las politicas con su respectiva regla segun el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="idGroup">id del grupo</param>
        /// <param name="type">tipo de politica</param>
        /// <param name="position">posicion de la politica</param>
        /// <param name="filter">filtro tipo like</param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> GetRulesPoliciesByFilter(int? idPackage, int idGroup, int? type, int? position, string filter);

        /// <summary>
        /// Realiza la creacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a crear</param>
        /// <param name="ruleSet">regla a crear</param>
        [OperationContract]
        void CreateRulePolicies(Models.PoliciesAut policies, RUModels._RuleSet ruleSet);

        /// <summary>
        /// Realiza la modificacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a modificar</param>
        /// <param name="ruleSet">regla a modificar</param>
        [OperationContract]
        void UpdateRulePolicies(Models.PoliciesAut policies, RUModels._RuleSet ruleSet);

        /// <summary>
        /// guarda la politica regla
        /// </summary>
        /// <param name="policies">politica a guardar</param>
        /// <param name="idHierarchyDt">id de la tabla de decision</param>
        /// <returns></returns>
        [OperationContract]
        void UpdateRulesPolicies(PoliciesAut policies, int? idHierarchyDt);

        /// <summary>
        /// Elimina una politica y su respectiva regla
        /// </summary>
        /// <param name="idPolicies"></param>
        [OperationContract]
        void DeleteRulePolicies(int idPolicies);

        /// <summary>
        /// consulta los paquetes asociados a politicas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RUModels.Package> GetPackagePolicies();

        /// <summary>
        /// Obtierne los tipos de politicas 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<TypePolicies> GetTypePolicies();

        /// <summary>
        /// Obtiene los niveles asociados al grupo de politicas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int[], string> GetLevelsByIdGroupPolicies(int idGroupPolicies, int? level);

        /// <summary>
        /// Crea los usuarios autorizadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <param name="countMin">numero minimo de autorizadores</param>
        /// <returns></returns>
        [OperationContract]
        void CreateUsersAutorization(int idPolicies, List<UserAuthorization> users, int countMin);

        /// <summary>
        /// Elimina un grupo de Politicas
        /// </summary>
        /// <param name="GroupPoliciesId"></param>
        [OperationContract]
        int DeleteGroupPolicies(int groupPoliciesId);

        /// <summary>
        /// Crea un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        [OperationContract]
        void CreateGroupPolicies(GroupPolicies groupPolicy);


        /// <summary>
        /// Actualiza un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        [OperationContract]
        void UpdateGroupPolicies(GroupPolicies groupPolicy);

        /// <summary>
        /// Genera el reporte de eventos
        /// </summary>
        /// <param name="policiesList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToPolicies(List<string> policiesList, string fileName);

        /// <summary>
        /// Actualiza el Identificador del proceso, en los eventos asociados
        /// </summary>
        /// <param name="key">llave de identificacion 1</param>
        /// <param name="key2">llave de identificacion 2</param>
        /// <param name="processId">identificador del proceso</param>
        [OperationContract]
        void UpdateProcessIdByKeyKey2(Enums.TypeFunction typeFunction, string key, string key2, string processId);

        [OperationContract]
        List<PoliciesAut> ValidateInfringementPolicies(List<PoliciesAut> infringementPolicies);

        /// <summary>
        /// Obtiene Riesgos por ramo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        [OperationContract]
        List<CoveredRisk> GetCoveredRiskByPrefix(int Prefix);

        /// <summary>
        /// Obtiene grupo de politicas con filtro
        /// </summary>
        /// <param name="GroupPolicies"></param>
        [OperationContract]
        List<GroupPolicies> GetGroupPoliciesByDescription(string description, int module, int subModule, string prefix);

        /// <summary>
        /// Realiza el proceso de importar la regla de la politica
        /// </summary>
        /// <param name="policies">politica a importar</param>
        /// <returns>Politica importada</returns>
        [OperationContract]
        PoliciesAut ImportRulePolicies(PoliciesAut policies);

        /// <summary>
        /// Método que envía correos. Se crea para poderlo llamar desde el Launcher.
        /// </summary>
        /// <param name="objEmailCriteria"></param>
        [OperationContract]
        void SendEmail(EmailCriteria objEmailCriteria);

        /// <summary>
        /// Método que elimina la informacion de las notificaciones de una temporal
        /// </summary>
        /// <param name="id de la temporal"></param>
        [OperationContract]
        bool DeleteNotificationByTemporalId(int id, int functionId);
    }
}