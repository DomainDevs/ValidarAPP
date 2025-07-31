using Sistran.Company.Application.Event.ApplicationService.DTOs;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Event.ApplicationService
{
    [ServiceContract]
    public interface IEventApplicationService 
    {
        /// <summary>
        /// Obtiene los modulos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ModuleDTO> GetModules();
        /// <summary>
        /// Obtiene los sub modulos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SubModuleDTO> GetSubModules();
        /// <summary>
        /// Consulta los grupos de eventos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EventGroupDTO> GetCompanyEventsGroups();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubModuleDTO> GetSubModulesByModuleId(int moduleId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetNameEntityByDescription(string description);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetQueryTypes();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetLevels();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDTO> GetCompanyEntities();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetTablesNames(string schema, string table);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetStoreProceduresBySPName(string schema, string table);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetValidationTypes();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetDataTypes();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetColumnsTableByTableIdTableNameColumn(int tableId, string tableName, string column);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ConditionGroupDTO> GetConditionGroups();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionId"></param>
        /// <returns></returns>
        [OperationContract]
        List<DependenciesDTO> GetDependenciesByConditionId(int conditionId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionGroupId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDTO> GetEntitiesByConditionGroupId(int conditionGroupId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupEventId"></param>
        /// <param name="statusEventId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EventDTO> GetEventsByEventIdStateIdPrefixId(int groupEventId, int statusEventId, int prefixId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetAccess();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [OperationContract]
        List<GroupEventDTO> GetPrefixesByGroupIdEventId(int groupId, int eventId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [OperationContract]
        List<GroupEventDTO> GetAccessByEventIdGroupId(int groupId, int eventId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [OperationContract]
        List<GroupDelegationDTO> GetDelegationsByGroupIdEventId(int groupId, int eventId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<DelegationUserDTO> GetDelegationUsersByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ConditionDTO> GetConditionsByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        [OperationContract]
        ArrayList GetEventConditionsByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="queryTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<OperatorConditionDTO> GetOperatorConditionByEntityIdQueryTypeId(int entityId, int queryTypeId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="entityId"></param>
        /// <param name="eventId"></param>
        /// <param name="anotherDependences"></param>
        /// <returns></returns>
        [OperationContract]
        List<GenericListDTO> GetValuesByGroupIdEntityIdEventIdOperatorId(int groupId, int entityId, int eventId, List<GenericListDTO> anotherDependences);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventGroupDTO"></param>
        /// <returns></returns>
        [OperationContract]
        EventGroupDTO CreateGroupEvent(EventGroupDTO eventGroupDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventGroupDTO"></param>
        /// <returns></returns>
        [OperationContract]
        EventGroupDTO DeleteGroupEvent(EventGroupDTO eventGroupDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventGroupDTO"></param>
        /// <returns></returns>
        [OperationContract]
        EventGroupDTO UpdateGroupEvent(EventGroupDTO eventGroupDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityDTO"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDTO CreateEntity(EntityDTO entityDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDTO DeleteEntity(int entityId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionGroupDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ConditionGroupDTO CreateConditionGroup(ConditionGroupDTO conditionGroupDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventConditionDTO"></param>
        /// <returns></returns>
        [OperationContract]
        EventConditionDTO CreateAssignEntity(EventConditionDTO eventConditionDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionId"></param>
        /// <returns></returns>
        [OperationContract]
        EventConditionDTO DeleteCondition(int conditionId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="prefixes"></param>
        /// <param name="executions"></param>
        /// <param name="rejectCauses"></param>
        /// <returns></returns>
        [OperationContract]
        EventDTO CreateCompanyEvent(EventDTO events, List<GenericListDTO> prefixes, List<GenericListDTO> executions, List<GenericListDTO> rejectCauses);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="prefixes"></param>
        /// <param name="executions"></param>
        /// <param name="rejectCauses"></param>
        /// <returns></returns>
        [OperationContract]
        EventDTO UpdateCompanyEvent(EventDTO events, List<GenericListDTO> prefixes, List<GenericListDTO> executions, List<GenericListDTO> rejectCauses);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupEventId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [OperationContract]
        EventDTO DeleteEvent(int groupEventId, int eventId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="groupId"></param>
        /// <param name="eventId"></param>
        /// <param name="hierarchyId"></param>
        /// <returns></returns>
        [OperationContract]
        DelegationUserDTO UpdateDelegationUser(DelegationUserDTO user, int groupId, int eventId, int hierarchyId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependenciesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        DependenciesDTO UpdateDependencies(DependenciesDTO dependenciesDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependenciesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        DependenciesDTO CreateDependencies(DependenciesDTO dependenciesDTO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependenciesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        DependenciesDTO DeleteDependencies(DependenciesDTO dependenciesDTO);

        [OperationContract]
        ConditionDTO DeleteConditionEntity(ConditionDTO conditionDTO);
    }
}