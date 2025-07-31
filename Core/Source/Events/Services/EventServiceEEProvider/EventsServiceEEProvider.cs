using Sistran.Core.Application.EventsServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Sistran.Core.Application.EventsServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class EventsServiceEEProvider : IEventServiceCore
    {

        #region DbObject
        /// <summary>
        /// Obtiene el nombre de las tablas que cumplan con los parametros
        /// </summary>
        /// <param name="schema">nombre del schema</param>
        /// <param name="table">nombre de la tabla</param>
        /// <returns></returns>
        public List<Models.Objects> GetTablesNames(string schema, string table)
        {
            try
            {
                EventDAO EventDAO = new EventDAO();
                return EventDAO.GetTablesNames(schema, table);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nombre columnas de una tabla que cumplan con los parametros
        /// </summary>
        /// <param name="idTable">id de la tabla a consultar</param>
        /// <param name="column">nombre de la columna</param>
        /// <returns></returns>
        public List<Models.Objects> GetColumnsTableByIdTableColumn(long IdTable, string TableName, string Column)
        {
            try
            {
                EventDAO EventDAO = new EventDAO();
                return EventDAO.GetColumnsTableByIdTableColumn(IdTable, TableName, Column);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el nombre de los procedimientos almacenados en la BD que cumpla con el parametro
        /// </summary>
        /// <param name="SPName">Nombre del sp a consultar</param>
        /// <returns></returns>
        public List<Models.Objects> GetStoreProceduresNamesBySPName(string schema, string SPName)
        {
            try
            {
                EventDAO EventDAO = new EventDAO();
                return EventDAO.GetStoreProceduresNamesBySPName(schema, SPName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Accesses
        /// <summary>
        /// obtiene la lista de accesos TEMP
        /// </summary>
        /// <returns></returns>
        public List<Models.Objects> GetAccesses()
        {
            try
            {
                EventDAO EventDAO = new EventDAO();
                return EventDAO.GetAccesses();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene los id de los aacesos segun el id evento y id grupo
        /// </summary>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <returns></returns>
        public List<Models.EventAsocObj> GetAccessesByIdEventIdGroup(int IdEvent, int IdGroup)
        {
            try
            {
                EventAsocObjDAO eventAsocObjDAO = new EventAsocObjDAO();
                return eventAsocObjDAO.GetAccessesByIdEventIdGroup(IdEvent, IdGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EventsGroup
        /// <summary>
        /// obtiene la lista de Models.EventsGroup
        /// </summary>
        /// <returns></returns>
        public List<Models.EventsGroup> GetEventsGroups()
        {
            try
            {
                EventsGroupDAO eventsGroupDAO = new EventsGroupDAO();
                return eventsGroupDAO.GetEventsGroup();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// EventsGroup por IdEventGroup
        /// </summary>
        /// <param name="IdEventGroup">id del grupo de eventos</param>
        /// <returns></returns>
        public Models.EventsGroup GetEventsGroupByIdEventGroup(int IdEventGroup)
        {
            try
            {
                EventsGroupDAO eventsGroupDAO = new EventsGroupDAO();
                return eventsGroupDAO.GetEventsGroupByIdEventGroup(IdEventGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear un nuevo grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        public void CreateEventsGroup(Models.EventsGroup eventsGroup)
        {
            try
            {
                EventsGroupDAO eventsGroupDAO = new EventsGroupDAO();
                eventsGroupDAO.CreateEventGroups(eventsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualizar un grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        public void UpdateEventsGroup(Models.EventsGroup eventsGroup)
        {
            try
            {
                EventsGroupDAO eventsGroupDAO = new EventsGroupDAO();
                eventsGroupDAO.UpdateEventGroups(eventsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        public void DeleteEventsGroup(Models.EventsGroup eventsGroup)
        {
            try
            {
                EventsGroupDAO eventsGroupDAO = new EventsGroupDAO();
                var entityEventsGroup = GetEventsGroupByIdEventGroup(eventsGroup.GroupEventId);
                eventsGroupDAO.DeleteEventGroups(entityEventsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Module
        /// <summary>
        /// Lista de modulos
        /// </summary>
        /// <returns></returns>
        public List<Models.Module> GetModules()
        {
            try
            {
                ModuleDAO moduleDAO = new ModuleDAO();
                return moduleDAO.GetModules();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        ///  modulo por id del modulo  
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <returns></returns>
        public Models.Module GetModuleByIdModule(int IdModule)
        {
            try
            {
                ModuleDAO moduleDAO = new ModuleDAO();
                return moduleDAO.GetModuleByIdModule(IdModule);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Submodule
        /// <summary>
        /// obtiene la lista de Models.SubModule qie pertenecen al IdModule
        /// </summary>
        /// <param name="IdModule">id de modulo</param>
        /// <returns></returns>
        public List<Models.SubModule> GetSubModulesByIdModule(int IdModule)
        {
            try
            {
                SubModuleDAO subModuleDAO = new SubModuleDAO();
                return subModuleDAO.GetSubModulesByIdModule(IdModule);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene Models.SubModule por IdModule y IdSubModule
        /// </summary>
        /// <param name="IdModule">id de modulo</param>
        /// <param name="IdSubModule">id de submodulo</param>
        /// <returns></returns>
        public Models.SubModule GetSubModuleByIdModuleIdSubModule(int IdModule, int IdSubModule)
        {
            try
            {
                SubModuleDAO subModuleDAO = new SubModuleDAO();
                return subModuleDAO.GetSubModuleByIdModuleIdSubModule(IdModule, IdSubModule);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EventEntity
        /// <summary>
        /// Obtine todas las entidades
        /// </summary>
        /// <returns>lista de Models.EventEntity</returns>
        public List<Models.EventEntity> GetEventEntities()
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                return eventEntityDAO.GetEventEntities();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene un Models.EventEntity a partir de su id
        /// </summary>
        /// <param name="IdEventEntity">id del Models.EventEntity</param>
        /// <returns></returns>
        public Models.EventEntity GetEventEntityByIdEventEntity(int IdEventEntity)
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                return eventEntityDAO.GetEventEntityByIdEventEntity(IdEventEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea una nueva eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a crear</param>
        public void CreateEventEntity(Models.EventEntity eventEntity)
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                eventEntityDAO.CreateEventEntity(eventEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza una eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a actualizar</param>
        public void UpdateEventEntity(Models.EventEntity entity)
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                eventEntityDAO.UpdateEventEntity(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// elimina una nueva eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a crear</param>
        public void DeleteEventEntity(int eventEntity)
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                eventEntityDAO.DeleteEventEntity(eventEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene el las entidades asociadas al grupo de condiciones
        /// </summary>
        /// <param name="IdConditionsGroup">id del gupo de condiciones</param>
        /// <returns></returns>
        public List<Models.EventEntity> GetEntitiesByIdConditionsGroup(int IdConditionsGroup)
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                return eventEntityDAO.GetEntitiesByIdConditionsGroup(IdConditionsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene las entidades según la descripción de la entidad
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Models.EventEntity> GetEventEntitiesByDescription(string description)
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                return eventEntityDAO.GetEventEntitiesByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        
        #endregion

        #region QueryType
        /// <summary>
        /// consulta los tipos de consultas
        /// </summary>
        /// <returns></returns>
        public List<Models.EventQueryType> GetQueryTypesCode()
        {
            try
            {
                EventQueryTypeDAO eventQueryTypeDAO = new EventQueryTypeDAO();
                return eventQueryTypeDAO.GetQueryTypesCode();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region ValidationType
        /// <summary>
        /// retorna la lista de tipos de validacion
        /// </summary>
        /// <returns></returns>
        public List<Models.EventValidationType> GetValidationTypes()
        {
            try
            {
                EventValidationTypeDAO eventValidationTypeDAO = new EventValidationTypeDAO();
                return eventValidationTypeDAO.GetValidationTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region DataType
        /// <summary>
        /// obtiene los tipos de datos
        /// </summary>
        /// <returns></returns>
        public List<Models.EventDataType> GetDataTypes()
        {
            try
            {
                EventDataTypeDAO eventDataTypeDAO = new EventDataTypeDAO();
                return eventDataTypeDAO.GetDataTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Levels
        /// <summary>
        /// obtiene la lista de niveles
        /// </summary>
        /// <returns></returns>
        public List<Models.EventLevels> GetLevels()
        {
            try
            {
                EventLevelsDAO eventLevelsDAO = new EventLevelsDAO();
                return eventLevelsDAO.GetLevels();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region ConditionGroups
        /// <summary>
        /// obtiene los grupos de condiciones
        /// </summary>
        /// <returns></returns>
        public List<Models.EventConditionGroup> GetConditionsGroups()
        {
            try
            {
                EventConditionGroupDAO eventConditionGroupDAO = new EventConditionGroupDAO();
                return eventConditionGroupDAO.GetConditionsGroups();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene un grupo de condiciones por el id de la condicion
        /// </summary>
        /// <param name="IdConditionsGroup">id del grupo de condiciones</param>
        /// <returns></returns>
        public Models.EventConditionGroup GetConditionsGroupByIdConditionsGroup(int IdConditionsGroup)
        {
            try
            {
                EventConditionGroupDAO eventConditionGroupDAO = new EventConditionGroupDAO();
                return eventConditionGroupDAO.GetConditionsGroupByIdConditionsGroup(IdConditionsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// crea un nuevo grupo de condiciones
        /// </summary>
        /// <param name="ConditionsGroup">grupo de condiciones a crear</param>
        /// <returns></returns>
        public void CreateConditionsGroup(Models.EventConditionGroup ConditionsGroup)
        {
            try
            {
                EventConditionGroupDAO eventConditionGroupDAO = new EventConditionGroupDAO();
                eventConditionGroupDAO.CreateConditionsGroup(ConditionsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza un nuevo grupo de condiciones
        /// </summary>
        /// <param name="ConditionsGroup">grupo de condiciones a crear</param>
        /// <returns></returns>
        public void UpdateConditionsGroup(Models.EventConditionGroup ConditionsGroup)
        {
            try
            {
                EventConditionGroupDAO eventConditionGroupDAO = new EventConditionGroupDAO();
                eventConditionGroupDAO.UpdateConditionsGroup(ConditionsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// alimina un nuevo grupo de condiciones
        /// </summary>
        /// <param name="ConditionsGroup">grupo de condiciones a crear</param>
        /// <returns></returns>
        public void DeleteConditionsGroup(int IdConditionsGroup)
        {
            try
            {
                EventConditionGroupDAO eventConditionGroupDAO = new EventConditionGroupDAO();
                eventConditionGroupDAO.DeleteConditionsGroup(IdConditionsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// asigna las entidades al grupo de condiciones
        /// </summary>
        /// <param name="IdCondition">id del grupo de condiciones</param>
        /// <param name="IdEntities">lsista de id de entidades</param>
        public void AssignEntitiesByIdConditionIdEntities(int IdCondition, List<int> IdEntities)
        {
            try
            {
                EventConditionGroupDAO eventConditionGroupDAO = new EventConditionGroupDAO();
                eventConditionGroupDAO.AssignEntitiesByIdConditionIdEntities(IdCondition, IdEntities);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region dependencies
        /// <summary>
        /// obtine las dependencias 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdEntity">id de la entidad</param>
        /// <returns></returns>
        public List<Models.EntityDependencies> GetDependencesByIdGroupIdEventIdEntity(int IdGroup, int IdEvent, int IdEntity, int conditional)
        {
            try
            {
                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                return entityDependenciesDAO.GetDependencesByIdGroupIdEventIdEntity(IdGroup, IdEvent, IdEntity, conditional);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene la lista de dependenciad de un grupo de condiciones
        /// </summary>
        /// <param name="IdCondition"></param>
        /// <returns></returns>
        public List<Models.EntityDependencies> GetDependencesByIdCondition(int IdCondition)
        {
            try
            {
                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                return entityDependenciesDAO.GetDependencesByIdCondition(IdCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// emimina una dependencia que cumpla con las caracteristicas
        /// </summary>
        public void DeleteDependence(Models.EntityDependencies Dependence)
        {
            try
            {
                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                entityDependenciesDAO.DeleteDependence(Dependence);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza una dependencia que cumpla con las caracteristicas
        /// </summary>
        public void UpdateDependence(Models.EntityDependencies Dependence)
        {
            try
            {
                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                entityDependenciesDAO.UpdateDependence(Dependence);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea una dependencia 
        /// </summary>
        public void CreateDependence(Models.EntityDependencies Dependence)
        {
            try
            {
                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                entityDependenciesDAO.CreateDependence(Dependence);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EventCompany
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
        public List<Models.EventCompany> GetEventsByIdEventGroupStateIdPrefix(int IdEventGroup, int State, int IdPrefix)
        {
            try
            {
                EventCompanyDAO eventCompanyDAO = new EventCompanyDAO();
                return eventCompanyDAO.GetEventsByIdEventGroupStateIdPrefix(IdEventGroup, State, IdPrefix);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene un evento 
        /// </summary>
        /// <param name="IdEventGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <returns>Models.EventCompany</returns>
        public Models.EventCompany GetEventByIdEventGroupIdEvent(int IdEventGroup, int IdEvent)
        {
            try
            {
                EventCompanyDAO eventCompanyDAO = new EventCompanyDAO();
                return eventCompanyDAO.GetEventByIdEventGroupIdEvent(IdEventGroup, IdEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea el evento, le asigna los ramos, puntos de ejecucion y motivos de rechazo
        /// </summary>
        /// <param name="eventCompany">evento a crear</param>
        /// <param name="listPrefixes">lista de id de ramos</param>
        /// <param name="listAccesses">lista de id de accesses</param>
        /// <param name="listRejectCauses">lista de causas de devolucion</param>
        public void CreateEventCompany(Models.EventCompany eventCompany, List<int> listPrefixes, List<int> listAccesses, List<Models.Objects> listRejectCauses)
        {
            try
            {
                EventCompanyDAO eventCompanyDAO = new EventCompanyDAO();
                eventCompany = eventCompanyDAO.CreateEventCompany(eventCompany);

                EventGroupPrefixDAO eventGroupPrefixDAO = new EventGroupPrefixDAO();
                eventGroupPrefixDAO.CreateGroupPrefixesToEventCompany(eventCompany, listPrefixes);

                EventAsocObjDAO eventAsocObjDAO = new EventAsocObjDAO();
                eventAsocObjDAO.CreateAsocObjToEventCompany(eventCompany, listAccesses);

                EventRejectCausesDAO eventRejectCausesDAO = new EventRejectCausesDAO();
                eventRejectCausesDAO.CreateRejectCausesToEventCompany(eventCompany, listRejectCauses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza el evento
        /// </summary>
        /// <param name="eventCompany">evento a actualizar</param>
        /// <param name="listPrefixes">lista de id de ramos</param>
        /// <param name="listAccesses">lista de id de accesses</param>
        /// <param name="listRejectCauses">lista de causas de devolucion</param>
        public void UpdateEventCompany(Models.EventCompany eventCompany, List<int> listPrefixes, List<int> listAccesses, List<Models.Objects> listRejectCauses)
        {
            try
            {
                EventCompanyDAO eventCompanyDAO = new EventCompanyDAO();
                eventCompany = eventCompanyDAO.UpdateEventCompany(eventCompany);

                EventGroupPrefixDAO eventGroupPrefixDAO = new EventGroupPrefixDAO();
                eventGroupPrefixDAO.CreateGroupPrefixesToEventCompany(eventCompany, listPrefixes);

                EventAsocObjDAO eventAsocObjDAO = new EventAsocObjDAO();
                eventAsocObjDAO.CreateAsocObjToEventCompany(eventCompany, listAccesses);

                EventRejectCausesDAO eventRejectCausesDAO = new EventRejectCausesDAO();
                eventRejectCausesDAO.CreateRejectCausesToEventCompany(eventCompany, listRejectCauses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

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
        public void DeleteEventCompany(int IdGroup, int IdEvent)
        {
            bool createdRaised = false;
            bool disposedRaised = false;
            bool completedRaised = false;

            EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
            var authorization = eventAuthorizationDAO.GetEventAuthorizationByIdGroupIdEvent(IdGroup, IdEvent);
            if (authorization.Count != 0)
            {
                throw new BusinessException("FOREIGN_KEY:EventAuthorization");
            }

            Transaction.Created += delegate (object sender, TransactionEventArgs e) { createdRaised = true; };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e) { completedRaised = true; };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e) { disposedRaised = true; };

                    try
                    {
                        EventRejectCausesDAO eventRejectCausesDAO = new EventRejectCausesDAO();
                        eventRejectCausesDAO.DeleteRejectCausesToEventCompany(IdEvent, IdGroup);

                        EventAsocObjDAO eventAsocObjDAO = new EventAsocObjDAO();
                        eventAsocObjDAO.DeleteAsocObjToEventCompany(IdEvent, IdGroup);

                        EventGroupPrefixDAO eventGroupPrefixDAO = new EventGroupPrefixDAO();
                        eventGroupPrefixDAO.DeleteGroupPrefixesToEventCompany(IdEvent, IdGroup);

                        EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                        var DelegationsList = eventDelegationDAO.GetDelegationsByIdGroupIdEvent(IdGroup, IdEvent);

                        EventConditionDAO eventConditionDAO = new EventConditionDAO();
                        EventAuthorizationUserDAO eventAuthorizationUserDAO = new EventAuthorizationUserDAO();
                        EventNotificationUsersDAO eventNotificationUsersDAO = new EventNotificationUsersDAO();

                        foreach (var item in DelegationsList)
                        {
                            if (item.DelegationId != -1)
                            {
                                var listCondition = eventConditionDAO.GetConditionsByIdGroupIdEventIdDelegationIdEntity(IdGroup, IdEvent, item.DelegationId);
                                foreach (var item2 in listCondition)
                                {
                                    eventConditionDAO.DeleteConditionsEntity(item2.GroupEventId, item2.EventId, item2.DelegationId, item2.ConditionQuantity);
                                }

                                eventAuthorizationUserDAO.DeleteEventAuthorizationUserByIdGroupIdEventIdDelegation(IdGroup, IdEvent, item.DelegationId);
                                eventNotificationUsersDAO.DeleteEventAuthorizationUserByIdGroupIdEventIdDelegation(IdGroup, IdEvent, item.DelegationId);

                                eventDelegationDAO.DeleteDelegationByIdGroupIdEventIdDelegation(IdGroup, IdEvent, item.DelegationId);
                            }
                        }

                        EventCompanyDAO EventCompanyDAO = new EventCompanyDAO();
                        EventCompanyDAO.DeleteEventCompany(IdGroup, IdEvent);

                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException(ex.Message, ex);
                    }
                }
            }
        }
        #endregion

        #region prefixes
        /// <summary>
        /// obtine los Id de Ramos segun los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo de condiciones</param>
        /// <param name="IdEvent">id del evento</param>
        /// <returns></returns>
        public List<Models.EventGroupPrefix> GetPrefixesByIdGroupIdEvent(int IdGroup, int IdEvent)
        {
            try
            {
                EventGroupPrefixDAO eventGroupPrefixDAO = new EventGroupPrefixDAO();
                return eventGroupPrefixDAO.GetPrefixesByIdGroupIdEvent(IdGroup, IdEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region rejectCauses
        /// <summary>
        /// obtiene los id de los motivos de rechazo segun el id evento y id grupo
        /// </summary>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <returns></returns>
        public List<Models.EventRejectCauses> GetRejectCausesByIdEventIdGroup(int IdEvent, int IdGroup)
        {
            try
            {
                EventRejectCausesDAO eventRejectCausesDAO = new EventRejectCausesDAO();
                return eventRejectCausesDAO.GetRejectCausesByIdEventIdGroup(IdEvent, IdGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Delegation
        /// <summary>
        /// obtiene las delegaciones a partir de IdGroup y IdEvent
        /// </summary>
        /// <param name="IdGroup">grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// funcionalidad del sp --EVE.CO_GET_DELEGATIONS--
        public List<Models.EventDelegationSP> GetDelegationsByIdGroupIdEvent(int IdGroup, int IdEvent)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                return eventDelegationDAO.GetDelegationsByIdGroupIdEvent(IdGroup, IdEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene las delegaciones de nivel igual o superior a la especificada que tengas usuarios autorizadores
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la jerarquia</param>
        /// <returns></returns>
        public List<Models.EventDelegationSP> GetTopDelegationsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                return eventDelegationDAO.GetTopDelegationsByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region conditionEvent
        /// <summary>
        /// Crea una nueva condicion del evento
        /// </summary>
        /// <param name="conditions">condicion a crear</param>
        public void CreateConditionsEntity(List<Models.EventCondition> conditions)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                for (int i = 0; i < conditions.Count; i++)
                {
                    conditions[i].DelegationId = eventDelegationDAO.CreateDelegationByIdGroupIdEventIdHierarchy(conditions[i].GroupEventId, conditions[i].EventId, conditions[i].DelegationId);
                }
                EventConditionDAO eventConditionDAO = new EventConditionDAO();
                eventConditionDAO.CreateConditionsEntity(conditions);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// edita una condicion del evento
        /// </summary>
        /// <param name="conditions">condicion a editar</param>
        public void UpdateConditionsEntity(List<Models.EventCondition> conditions)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();

                for (int i = 0; i < conditions.Count; i++)
                {
                    conditions[i].DelegationId = eventDelegationDAO.CreateDelegationByIdGroupIdEventIdHierarchy(conditions[i].GroupEventId, conditions[i].EventId, conditions[i].DelegationId);
                }

                EventConditionDAO eventConditionDAO = new EventConditionDAO();
                eventConditionDAO.UpdateConditionsEntity(conditions);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// elimina una condicion 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <param name="IdContidion">id de la condicion</param>
        public void DeleteConditionsEntity(int IdGroup, int IdEvent, int IdHierarchy, int IdContidion)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                var DelegationId = eventDelegationDAO.CreateDelegationByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy);

                EventConditionDAO eventConditionDAO = new EventConditionDAO();
                eventConditionDAO.DeleteConditionsEntity(IdGroup, IdEvent, DelegationId, IdContidion);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene la lista de valores para la condicion que cumpla con los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo</param>
        /// <param name="Entity">entidad</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="Dependences">valores actuales de las dependencias</param>
        /// <returns></returns>
        public List<Models.Objects> GetValuesByIdGroupIdEntityIdEventIdOperator(int IdGroup, int IdEntity, int IdEvent, List<Models.Objects> Dependences)
        {
            try
            {
                var Entity = GetEventEntityByIdEventEntity(IdEntity);

                EventConditionDAO eventConditionDAO = new EventConditionDAO();
                return eventConditionDAO.GetValuesByIdGroupEntityIdEventIdOperator(IdGroup, Entity, IdEvent, Dependences);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtine las condiciones asignadas a un evento 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id delegacion</param>
        /// <returns>ArrayList</returns>
        public ArrayList GetEventConditionsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                var delegationId = eventDelegationDAO.CreateDelegationByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy);

                EventConditionDAO eventConditionDAO = new EventConditionDAO();
                return eventConditionDAO.GetEventConditionsByIdGroupIdEventIdDelegation(IdGroup, IdEvent, delegationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id delegacion</param>
        /// <param name="IdEntity">id de la entidad</param>
        /// <returns></returns>
        public List<Models.EventCondition> GetConditionsByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                var delegationId = eventDelegationDAO.CreateDelegationByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy);

                EventConditionDAO eventConditionDAO = new EventConditionDAO();
                return eventConditionDAO.GetConditionsByIdGroupIdEventIdDelegationIdEntity(IdGroup, IdEvent, delegationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region ComparitionType
        /// <summary>
        /// obtitne los tipos de comparadores segun la entidad y el tipo de query
        /// </summary>
        /// <param name="IdEntity">id de la entidad</param>
        /// <param name="IdQueryType">id del tipo del query</param>
        /// <returns></returns>
        public List<Models.EventComparisonType> GetOperatorConditionByIdEntityIdQueryType(int IdEntity, int IdQueryType)
        {
            try
            {
                EventComparisonTypeDAO eventComparisonTypeDAO = new EventComparisonTypeDAO();
                return eventComparisonTypeDAO.GetOperatorConditionByIdEntityIdQueryType(IdEntity, IdQueryType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtiene el comparador 
        /// </summary>
        /// <param name="IdComparator">id comparador</param>
        /// <returns></returns>
        public Models.EventComparisonType GetComparatorTypeByIdComparator(int IdComparator)
        {
            try
            {
                EventComparisonTypeDAO eventComparisonTypeDAO = new EventComparisonTypeDAO();
                return eventComparisonTypeDAO.GetComparatorTypeByIdComparator(IdComparator);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region DelegationUser

        /// <summary>
        /// obtiene el email de un usuario 
        /// </summary>
        /// <param name="idUser">id de usuario</param>
        /// <returns></returns>
        public string GetEmailByIdUser(int idUser)
        {
            try
            {
                EventAuthorizationUserDAO eventAuthorizationDAO = new EventAuthorizationUserDAO();
                return eventAuthorizationDAO.GetEmailByIdUser(idUser);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// retorna los usuarios pertenecientes a la delegacion
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <returns></returns>
        public List<Models.DelegationUser> GetDelegationUsersByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                EventAuthorizationUserDAO eventAuthorizationDAO = new EventAuthorizationUserDAO();
                return eventAuthorizationDAO.GetDelegationUsersByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario autorizador
        /// </summary>
        /// <param name="delegationUser">usuario a actualizar</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        public void UpdateDelegationUser(Models.DelegationUser delegationUser, int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                EventDelegationDAO eventDelegationDAO = new EventDelegationDAO();
                var DelegationId = eventDelegationDAO.CreateDelegationByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy);

                EventAuthorizationUserDAO eventAuthorizationDAO = new EventAuthorizationUserDAO();
                eventAuthorizationDAO.UpdateDelegationUser(delegationUser, IdGroup, IdEvent, DelegationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
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
        public List<Models.EventAuthorization> GetEventAuthorizationByIdGroupStateIdUserDateStartDateEnd(int IdGroup, int State, int IdUser, string DateStart, string DateEnd)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();

                List<Models.EventAuthorization> list = new List<Models.EventAuthorization>();

                if (IdGroup == -1)
                {
                    EventsGroupDAO eventsGroupDAO = new EventsGroupDAO();
                    var groups = eventsGroupDAO.GetEventsGroup();
                    foreach (var group in groups)
                    {
                        list.AddRange(eventAuthorizationDAO.GetEventAuthorizationByIdGroupStateIdUserDateStartDateEnd(group.GroupEventId, State, IdUser, DateStart, DateEnd));
                    }
                }
                else
                {
                    list = eventAuthorizationDAO.GetEventAuthorizationByIdGroupStateIdUserDateStartDateEnd(IdGroup, State, IdUser, DateStart, DateEnd);
                }
                return list.OrderByDescending(x => x.EventDate).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene una evento segun si id de autorizacion
        /// </summary>
        /// <param name="IdAuthorization">id de la autorizacion</param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetEventAuthorizationByIdAuthorization(string IdAuthorization)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                var list = eventAuthorizationDAO.GetEventAuthorizationByIdAuthorization(IdAuthorization);
                return eventAuthorizationDAO.GetAnotherValuesToAuthorization(list);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza un evento
        /// </summary>
        /// <param name="authorization">evento a autorizar</param>
        public void UpdateEventAuthorization(List<Models.EventAuthorization> authorization)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();

                foreach (var item in authorization)
                {
                    eventAuthorizationDAO.UpdateEventAuthorization(item);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// retorna los eventos que fueron reasignados, segun los parametros 
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdUser">id del usuario</param>
        /// <param name="DateStart">fecha inicial</param>
        /// <param name="DateEnd">fecha final</param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetEventAuthorizationReassignByIdGroupIdUserDateStartDateEnd(int IdGroup, int IdUser, DateTime DateStart, DateTime DateEnd)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                return eventAuthorizationDAO.GetEventAuthorizationReassignByIdGroupIdUserDateStartDateEnd(IdGroup, IdUser, DateStart, DateEnd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// crea las autorizaciones para cada uno de los eventos del temporal
        /// </summary>
        /// <param name="delegation">lista de delegation result a crear</param>
        /// <param name="IdUser">id del usuario</param>
        public void CreateEventAuthorizationByIdTempIduser(List<Models.EventDelegationResult> delegation, int IdUser)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                eventAuthorizationDAO.CreateEventAuthorizationByIdTempIduser(delegation, IdUser);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtiene los eventos pendientes de autorizacion para el usuario
        /// </summary>
        /// <param name="IdUser">id del usuario</param>
        /// <returns></returns>
        public List<Models.EventAuthorization> GetCountEventsByIdUser(int IdUser)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                return eventAuthorizationDAO.GetCountEventsByIdUser(IdUser);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// obtiene todos los eventos generados para el temporal
        /// </summary>
        /// <param name="idOperation">id del temporal</param>
        /// <returns>lsita de Models.EventAuthorization</returns>
        public List<Models.EventAuthorization> GetEventAuthorizationByIdOperation(int idOperation)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                return eventAuthorizationDAO.GetEventAuthorizationByIdOperation(idOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Dictionary<string, int> GetCountEventsRisk(int idTemp)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                return eventAuthorizationDAO.GetCountEventsRisk(idTemp);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region ReassignUser
        /// <summary>
        /// reasigna el evento a otro osuario
        /// </summary>
        /// <param name="IdAuthorization">id de la autorizacion</param>
        /// <param name="IdHieriachy">id de la nueva delegacion</param>
        /// <param name="IdAuthUser">id del nuevo usuario</param>
        public void ReassignmentUserByIdAuthorizationIdHieriachyIdAuthUser(Models.EventAuthorization Authorization, int IdHieriachy, int IdAuthUser)
        {
            try
            {
                EventAuthorizationDAO eventAuthorizationDAO = new EventAuthorizationDAO();
                EventReassignmentUserDAO eventReassignmentUserDAO = new EventReassignmentUserDAO();

                var auth = eventAuthorizationDAO.GetEventAuthorizationByIdAuthorization(Authorization.AuthorizationId).Where(x => x.AuthorizationId == Authorization.AuthorizationId).First();

                eventReassignmentUserDAO.ReassignmentUserByAuthorizationIdHieriachyIdAuthUser(auth, IdHieriachy, IdAuthUser);
                auth.HierachyCd = IdHieriachy;
                auth.AuthUserID = IdAuthUser;
                eventAuthorizationDAO.UpdateEventAuthorization(auth);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
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
        public List<Models.EventNotification> GetEventsNotificationByEventsCriteria(Models.EventsCriteria eventsCriteria)
        {
            try
            {
                EventDAO eventDAO = new EventDAO();
                return eventDAO.GetEventsNotificationByEventsCriteria(eventsCriteria);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
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
        public List<Models.EventDelegationResult> GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp(int IdModule, int IdSubModule, int IdUser, string IdTemp)
        {
            try
            {
                EventDelegationResultDAO eventDelegationResultDAO = new EventDelegationResultDAO();
                return eventDelegationResultDAO.GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp(IdModule, IdSubModule, IdUser, IdTemp);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion
    }
}
