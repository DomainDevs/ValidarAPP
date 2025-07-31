using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.EventsServices
{
    [ServiceContract]
    public interface IEventServiceCore
    {
        #region DbObjects
        /// <summary>
        /// Obtiene el nombre de las tablas que cumpaln con los parametros
        /// </summary>
        /// <param name="schema">nombre del schema</param>
        /// <param name="table">nombre de la tabla</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Objects> GetTablesNames(string schema, string table);

        /// <summary>
        /// Obtiene el nombre columnas de una tabla que cumplan con los parametros
        /// </summary>
        /// <param name="idTable">id de la tabla a consultar</param>
        /// <param name="column">nombre de la columna</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Objects> GetColumnsTableByIdTableColumn(long IdTable, string TableName, string Column);

        /// <summary>
        /// Obtiene el nombre de los procedimientos almacenados en la BD que cumpla con el parametro
        /// </summary>
        /// <param name="SPName">Nombre del sp a consultar</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Objects> GetStoreProceduresNamesBySPName(string schema, string SPName);
        #endregion

        #region Accesses
        /// <summary>
        /// obtiene la lista de accesos TEMP
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Objects> GetAccesses();

        /// <summary>
        /// obtiene los id de los aacesos segun el id evento y id grupo
        /// </summary>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventAsocObj> GetAccessesByIdEventIdGroup(int IdEvent, int IdGroup);
        #endregion

        #region EventsGroup
        /// <summary>
        /// obtiene la lista de Models.EventsGroup
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventsGroup> GetEventsGroups();

        /// <summary>
        /// EventsGroup por IdEventGroup
        /// </summary>
        /// <param name="IdEventGroup">id del grupo de eventos</param>
        /// <returns></returns>
        [OperationContract]
        Models.EventsGroup GetEventsGroupByIdEventGroup(int IdEventGroup);

        /// <summary>
        /// Crear un nuevo grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        [OperationContract]
        void CreateEventsGroup(Models.EventsGroup eventsGroup);

        /// <summary>
        /// actualizar un grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        [OperationContract]
        void UpdateEventsGroup(Models.EventsGroup eventsGroup);

        /// <summary>
        /// Elimina un grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        [OperationContract]
        void DeleteEventsGroup(Models.EventsGroup eventsGroup);
        #endregion

        #region Module
        /// <summary>
        /// Lista de modulos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Module> GetModules();

        /// <summary>
        ///  modulo por id del modulo  
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <returns></returns>
        [OperationContract]
        Models.Module GetModuleByIdModule(int IdModule);
        #endregion

        #region Submodule
        /// <summary>
        /// obtiene la lista de Models.SubModule qie pertenecen al IdModule
        /// </summary>
        /// <param name="IdModule">id de modulo</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.SubModule> GetSubModulesByIdModule(int IdModule);

        /// <summary>
        /// obtiene Models.SubModule por IdModule y IdSubModule
        /// </summary>
        /// <param name="IdModule">id de modulo</param>
        /// <param name="IdSubModule">id de submodulo</param>
        /// <returns></returns>
        [OperationContract]
        Models.SubModule GetSubModuleByIdModuleIdSubModule(int IdModule, int IdSubModule);
        #endregion

        #region EventEntity
        /// <summary>
        /// Obtine todas las entidades
        /// </summary>
        /// <returns>lista de Models.EventEntity</returns>
        [OperationContract]
        List<Models.EventEntity> GetEventEntities();

        /// <summary>
        /// Obtiene un Models.EventEntity a partir de su id
        /// </summary>
        /// <param name="IdEventEntity">id del Models.EventEntity</param>
        /// <returns></returns>
        [OperationContract]
        Models.EventEntity GetEventEntityByIdEventEntity(int IdEventEntity);

        /// <summary>
        /// Crea una nueva eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a crear</param>
        [OperationContract]
        void CreateEventEntity(Models.EventEntity eventEntity);


        /// <summary>
        /// actualiza una eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a actualizar</param>
        [OperationContract]
        void UpdateEventEntity(Models.EventEntity entity);

        /// <summary>
        /// elimina una nueva eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a crear</param>
        [OperationContract]
        void DeleteEventEntity(int eventEntity);

        /// <summary>
        /// obtiene el las entidades asociadas al grupo de condiciones
        /// </summary>
        /// <param name="IdConditionsGroup">id del gupo de condiciones</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventEntity> GetEntitiesByIdConditionsGroup(int IdConditionsGroup);

        [OperationContract]
        List<Models.EventEntity> GetEventEntitiesByDescription(string description);
        #endregion

        #region QueryType
        /// <summary>
        /// consulta los tipos de consultas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventQueryType> GetQueryTypesCode();

        #endregion

        #region ValidationType
        /// <summary>
        /// retorna la lista de tipos de validacion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventValidationType> GetValidationTypes();
        #endregion

        #region GetDataTypes
        /// <summary>
        /// obtiene los tipos de datos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventDataType> GetDataTypes();
        #endregion

        #region Levels
        /// <summary>
        /// obtiene la lista de niveles
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventLevels> GetLevels();

        #endregion

        #region ConditionGroups
        /// <summary>
        /// obtiene los grupos de condiciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventConditionGroup> GetConditionsGroups();

        /// <summary>
        /// obtiene un grupo de condiciones por el id de la condicion
        /// </summary>
        /// <param name="IdConditionsGroup">id del grupo de condiciones</param>
        /// <returns></returns>
        [OperationContract]
        Models.EventConditionGroup GetConditionsGroupByIdConditionsGroup(int IdConditionsGroup);

        /// <summary>
        /// crea un nuevo grupo de condiciones
        /// </summary>
        /// <param name="ConditionsGroup">grupo de condiciones a crear</param>
        /// <returns></returns>
        [OperationContract]
        void CreateConditionsGroup(Models.EventConditionGroup ConditionsGroup);

        /// <summary>
        /// actualiza un nuevo grupo de condiciones
        /// </summary>
        /// <param name="ConditionsGroup">grupo de condiciones a crear</param>
        /// <returns></returns>
        [OperationContract]
        void UpdateConditionsGroup(Models.EventConditionGroup ConditionsGroup);

        /// <summary>
        /// alimina un nuevo grupo de condiciones
        /// </summary>
        /// <param name="ConditionsGroup">grupo de condiciones a crear</param>
        /// <returns></returns>
        [OperationContract]
        void DeleteConditionsGroup(int IdConditionsGroup);


        /// <summary>
        /// asigna las entidades al grupo de condiciones
        /// </summary>
        /// <param name="IdCondition">id del grupo de condiciones</param>
        /// <param name="IdEntities">lsista de id de entidades</param>
        [OperationContract]
        void AssignEntitiesByIdConditionIdEntities(int IdCondition, List<int> IdEntities);
        #endregion

        #region dependencies

        /// <summary>
        /// obtine las dependencias 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdEntity">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EntityDependencies> GetDependencesByIdGroupIdEventIdEntity(int IdGroup, int IdEvent, int IdEntity, int conditional);

        /// <summary>
        /// obtiene la lista de dependenciad de un grupo de condiciones
        /// </summary>
        /// <param name="IdCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EntityDependencies> GetDependencesByIdCondition(int IdCondition);

        /// <summary>
        /// emimina una dependencia que cumpla con las caracteristicas
        /// </summary>
        [OperationContract]
        void DeleteDependence(Models.EntityDependencies Dependence);

        /// <summary>
        /// actualiza una dependencia que cumpla con las caracteristicas
        /// </summary>
        [OperationContract]
        void UpdateDependence(Models.EntityDependencies Dependence);

        /// <summary>
        /// Crea una dependencia 
        /// </summary>
        [OperationContract]
        void CreateDependence(Models.EntityDependencies Dependence);
        #endregion

        #region EventCOmpany
        /// <summary>
        /// obtiene EventCompany que cunplan con las condiciones
        /// </summary>
        /// <param name="IdEventGroup"> id del grupo de eventos</param>
        /// <param name="State">estado del evento
        ///     <value val=-1>todos los eventos</value>
        ///     <value val=0>inactivos</value>
        ///     <value val=1>activos</value>
        /// </param>
        /// <param name="IdPrefix"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventCompany> GetEventsByIdEventGroupStateIdPrefix(int IdEventGroup, int State, int IdPrefix);

        /// <summary>
        /// obtiene un evento 
        /// </summary>
        /// <param name="IdEventGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <returns>Models.EventCompany</returns>
        [OperationContract]
        Models.EventCompany GetEventByIdEventGroupIdEvent(int IdEventGroup, int IdEvent);

        /// <summary>
        /// Crea el evento, le asigna los ramos, puntos de ejecucion y motivos de rechazo
        /// </summary>
        /// <param name="eventCompany">evento a crear</param>
        /// <param name="listPrefixes">lista de id de ramos</param>
        /// <param name="listAccesses">lista de id de accesses</param>
        /// <param name="listRejectCauses">lsita de causas de devolucion</param>
        [OperationContract]
        void CreateEventCompany(Models.EventCompany eventCompany, List<int> listPrefixes, List<int> listAccesses, List<Models.Objects> listRejectCauses);

        /// <summary>
        /// actualiza el evento
        /// </summary>
        /// <param name="eventCompany">evento a actualizar</param>
        /// <param name="listPrefixes">lista de id de ramos</param>
        /// <param name="listAccesses">lista de id de accesses</param>
        /// <param name="listRejectCauses">lista de causas de devolucion</param>
        [OperationContract]
        void UpdateEventCompany(Models.EventCompany eventCompany, List<int> listPrefixes, List<int> listAccesses, List<Models.Objects> listRejectCauses);

        /// <summary>
        /// realiza una transaccion para eliminar
        ///     eventRejectCauses
        ///     eventAsocObj
        ///     eventGroupPrefix
        ///     eventDelegation
        ///     eventCondition
        ///     eventAuthorizationUser
        ///     NotificationUsers
        ///     EventCompany
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        [OperationContract]
        void DeleteEventCompany(int IdGroup, int IdEvent);
        #endregion

        #region prefixes
        /// <summary>
        /// obtine los Id de Ramos segun los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo de condiciones</param>
        /// <param name="IdEvent">id del evento</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventGroupPrefix> GetPrefixesByIdGroupIdEvent(int IdGroup, int IdEvent);

        #endregion

        #region rejectCauses
        /// <summary>
        /// obtiene los id de los motivos de rechazo segun el id evento y id grupo
        /// </summary>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventRejectCauses> GetRejectCausesByIdEventIdGroup(int IdEvent, int IdGroup);
        #endregion

        #region Delegation
        /// <summary>
        /// obtiene las delegaciones a partir de IdGroup y IdEvent
        /// </summary>
        /// <param name="IdGroup">grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// funcionalidad del sp --EVE.CO_GET_DELEGATIONS--
        [OperationContract]
        List<Models.EventDelegationSP> GetDelegationsByIdGroupIdEvent(int IdGroup, int IdEvent);

        /// <summary>
        /// obtiene las delegaciones de nivel igual o superior a la especificada que tengas usuarios autorizadores
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la jerarquia</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventDelegationSP> GetTopDelegationsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy);
        #endregion

        #region conditionEvent

        /// <summary>
        /// Crea una nueva condicion del evento
        /// </summary>
        /// <param name="conditions">condicion a crear</param>
        [OperationContract]
        void CreateConditionsEntity(List<Models.EventCondition> conditions);

        /// <summary>
        /// edita una condicion del evento
        /// </summary>
        /// <param name="conditions">condicion a editar</param>
        [OperationContract]
        void UpdateConditionsEntity(List<Models.EventCondition> conditions);

        /// <summary>
        /// elimina una condicion 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <param name="IdContidion">id de la condicion</param>
        [OperationContract]
        void DeleteConditionsEntity(int IdGroup, int IdEvent, int IdHierarchy, int IdContidion);

        /// <summary>
        /// obtiene la lista de valores para la condicion que cumpla con los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo</param>
        /// <param name="Entity">entidad</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="Dependences">valores actuales de las dependencias</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Objects> GetValuesByIdGroupIdEntityIdEventIdOperator(int IdGroup, int IdEntity, int IdEvent, List<Models.Objects> Dependences);

        /// <summary>
        /// obtine las condiciones asignadas a un evento 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id delegacion</param>
        /// <returns>ArrayList</returns>
        [OperationContract]
        ArrayList GetEventConditionsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id delegacion</param>
        /// <param name="IdEntity">id de la entidad</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventCondition> GetConditionsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy);
        #endregion

        #region ComparitionType
        /// <summary>
        /// obtitne los tipos de comparadores segun la entidad y el tipo de query
        /// </summary>
        /// <param name="IdEntity">id de la entidad</param>
        /// <param name="IdQueryType">id del tipo del query</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventComparisonType> GetOperatorConditionByIdEntityIdQueryType(int IdEntity, int IdQueryType);

        /// <summary>
        /// Obtiene el comparador 
        /// </summary>
        /// <param name="IdComparator">id comparador</param>
        /// <returns></returns>
        [OperationContract]
        Models.EventComparisonType GetComparatorTypeByIdComparator(int IdComparator);
        #endregion

        #region DelegationUser
        /// <summary>
        /// obtiene el email de un usuario 
        /// </summary>
        /// <param name="idUser">id de usuario</param>
        /// <returns></returns>
        [OperationContract]
        string GetEmailByIdUser(int idUser);

        /// <summary>
        /// retorna los usuarios pertenecientes a la delegacion
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.DelegationUser> GetDelegationUsersByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy);

        /// <summary>
        /// Actualiza un usuario autorizador
        /// </summary>
        /// <param name="delegationUser">usuario a actualizar</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        [OperationContract]
        void UpdateDelegationUser(Models.DelegationUser delegationUser, int IdGroup, int IdEvent, int IdHierarchy);
        #endregion

        #region EventAuthorization
        /// <summary>
        /// consulta los eventos que cumplen con los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="State">stado del evento
        ///     <value val="1">pendientes</value>
        ///     <value val="2">autorizados</value>
        ///     <value val="3">rechazados</value>
        ///     <value val="4">Reasignados</value>
        /// </param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="DateStart">fecha inicial</param>
        /// <param name="DateEnd">fecha final</param>
        /// <returns>sp EVE.CO_GET_EVENT_AUTHORIZATION</returns>
        [OperationContract]
        List<Models.EventAuthorization> GetEventAuthorizationByIdGroupStateIdUserDateStartDateEnd(int IdGroup, int State, int IdUser, string DateStart, string DateEnd);

        /// <summary>
        /// obtiene una evento segun si id de autorizacion
        /// </summary>
        /// <param name="IdAuthorization">id de la autorizacion</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventAuthorization> GetEventAuthorizationByIdAuthorization(string IdAuthorization);

        /// <summary>
        /// actualiza un evento
        /// </summary>
        /// <param name="authorization">evento a autorizar</param>
        [OperationContract]
        void UpdateEventAuthorization(List<Models.EventAuthorization> authorization);

        /// <summary>
        /// retorna los eventos que fueron reasignados, segun los parametros 
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="DateStart">fecha inicial</param>
        /// <param name="DateEnd">fecha final</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventAuthorization> GetEventAuthorizationReassignByIdGroupIdUserDateStartDateEnd(int IdGroup, int IdUser, DateTime DateStart, DateTime DateEnd);

        /// <summary>
        /// crea las autorizaciones para cada uno de los eventos del temporal
        /// </summary>
        /// <param name="delegation">lista de delegation result a crear</param>
        /// <param name="IdUser">id del usuario</param>
        [OperationContract]
        void CreateEventAuthorizationByIdTempIduser(List<Models.EventDelegationResult> delegation,  int IdUser);

        /// <summary>
        /// obtiene los eventos pendientes de autorizacion para el usuario
        /// </summary>
        /// <param name="IdUser">id del usuario</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventAuthorization> GetCountEventsByIdUser(int IdUser);


        [OperationContract]
        Dictionary<string, int> GetCountEventsRisk(int idTemp);
        #endregion

        #region ReassignUser
        /// <summary>
        /// reasigna el evento a otro osuario
        /// </summary>
        /// <param name="IdAuthorization">id de la autorizacion</param>
        /// <param name="IdHieriachy">id de la nueva delegacion</param>
        /// <param name="IdAuthUser">id del nuevo usuario</param>
        [OperationContract]
        void ReassignmentUserByIdAuthorizationIdHieriachyIdAuthUser(Models.EventAuthorization Authorization, int IdHieriachy, int IdAuthUser);

        /// <summary>
        /// obtiene todos los eventos generados para el temporal
        /// </summary>
        /// <param name="idOperation">id del temporal</param>
        /// <returns>lsita de Models.EventAuthorization</returns>
        [OperationContract]
        List<Models.EventAuthorization> GetEventAuthorizationByIdOperation(int idOperation);

        #endregion

        #region EventNotification

        /// <summary>
        /// obtiene los eventos que fueron ejecutados en la pantalla
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdSubmodule">id del submodulo</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="ObjectName">nombre de la pantalla</param>
        /// <param name="IdTemp">id del temporal</param>
        /// <param name="key_1"></param>
        /// <param name="key_2"></param>
        /// <param name="key_3"></param>
        /// <param name="key_4"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventNotification> GetEventsNotificationByEventsCriteria(Models.EventsCriteria eventsCriteria);
        #endregion

        #region DelegationResult
        /// <summary>
        /// obtine el resumen de los eventos que se lanzaron 
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdSubmodule">id del submodulo</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="IdTemp">id del temporal</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.EventDelegationResult> GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp(int IdModule, int IdSubModule, int IdUser, string IdTemp);
        #endregion
    }
}
