using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Company.Application.Event.ApplicationService;
using Sistran.Company.Application.Event.ApplicationService.DTOs;
using Sistran.Company.Application.Event.ApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Event.ApplicationServices.EEProvider;
using Sistran.Core.Application.EventsServices.Models;

namespace Sistran.Company.Application.Event.ApplicationService.EEProvider
{
    public class EventApplicationServiceProvider : IEventApplicationService
    {
        public List<EventGroupDTO> GetCompanyEventsGroups()
        {
            return DTOAssembler.CreateEvents(DelegateService._EventBusinessService.GetCompanyEventsGroups());
        }

        public List<ModuleDTO> GetModules()
        {
            return DTOAssembler.CreateModules(DelegateService._EventBusinessService.GetCompanyModules());
        }

        public List<SubModuleDTO> GetSubModules()
        {
            return DTOAssembler.CreateSubModules(DelegateService._EventBusinessService.GetCompanySubModules());
        }

        public List<SubModuleDTO> GetSubModulesByModuleId(int moduleId)
        {
            return DTOAssembler.CreateSubModules(DelegateService._EventBusinessService.GetCompanySubModulesByModuleId(moduleId));
        }

        public List<GenericListDTO> GetNameEntityByDescription(string description)
        {
            return DTOAssembler.CreateEntities(DelegateService._EventBusinessService.GetEventEntitiesByDescription(description));
        }

        public List<GenericListDTO> GetQueryTypes()
        {
            return DTOAssembler.CreateQueryTypes(DelegateService._EventBusinessService.GetQueryTypesCode());
        }

        public List<GenericListDTO> GetLevels()
        {
            return DTOAssembler.CreateLevels(DelegateService._EventBusinessService.GetLevels());
        }

        public List<EntityDTO> GetCompanyEntities()
        {
            return DTOAssembler.CreateGroupEntities(DelegateService._EventBusinessService.GetEventEntities());
        }

        public List<GenericListDTO> GetTablesNames(string schema, string table)
        {
            return DTOAssembler.CreateTables(DelegateService._EventBusinessService.GetTablesNames(schema, table));
        }

        public List<GenericListDTO> GetStoreProceduresBySPName(string schema, string table)
        {
            return DTOAssembler.CreateStoredProcedures(DelegateService._EventBusinessService.GetStoreProceduresNamesBySPName(schema, table));
        }

        public List<GenericListDTO> GetValidationTypes()
        {
            return DTOAssembler.CreateValidationTypes(DelegateService._EventBusinessService.GetValidationTypes());
        }

        public List<GenericListDTO> GetDataTypes()
        {
            return DTOAssembler.CreateDataTypes(DelegateService._EventBusinessService.GetDataTypes());
        }

        public List<GenericListDTO> GetColumnsTableByTableIdTableNameColumn(int tableId, string tableName, string column)
        {
            return DTOAssembler.CreateColumnsTables(DelegateService._EventBusinessService.GetColumnsTableByIdTableColumn(tableId, tableName, column));
        }

        public List<ConditionGroupDTO> GetConditionGroups()
        {
            return DTOAssembler.CreateConditions(DelegateService._EventBusinessService.GetConditionsGroups());
        }

        public List<DependenciesDTO> GetDependenciesByConditionId(int conditionId)
        {
            return DTOAssembler.CreateDependencies(DelegateService._EventBusinessService.GetDependencesByIdCondition(conditionId));
        }

        public List<EntityDTO> GetEntitiesByConditionGroupId(int conditionGroupId)
        {
            return DTOAssembler.CreateGroupEntities(DelegateService._EventBusinessService.GetEntitiesByIdConditionsGroup(conditionGroupId));
        }

        public List<EventDTO> GetEventsByEventIdStateIdPrefixId(int groupEventId, int statusEventId, int prefixId)
        {
            return DTOAssembler.CreateGroupEvents(DelegateService._EventBusinessService.GetEventsByIdEventGroupStateIdPrefix(groupEventId, statusEventId, prefixId));
        }

        public List<GenericListDTO> GetAccess()
        {
            return DTOAssembler.CreateAccess(DelegateService._EventBusinessService.GetAccesses());
        }

        public List<GroupEventDTO> GetPrefixesByGroupIdEventId(int groupId, int eventId)
        {
            return DTOAssembler.CreateGroupPrefixes(DelegateService._EventBusinessService.GetPrefixesByIdGroupIdEvent(groupId, eventId));
        }

        public List<GroupEventDTO> GetAccessByEventIdGroupId(int groupId, int eventId)
        {
            return DTOAssembler.CreateGroupAccess(DelegateService._EventBusinessService.GetAccessesByIdEventIdGroup(groupId, eventId));
        }
        public List<GroupDelegationDTO> GetDelegationsByGroupIdEventId(int groupId, int eventId)
        {
            return DTOAssembler.CreateGroupDelegations(DelegateService._EventBusinessService.GetDelegationsByIdGroupIdEvent(groupId, eventId));
        }

        public List<DelegationUserDTO> GetDelegationUsersByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            return DTOAssembler.CreateDelegationUsers(DelegateService._EventBusinessService.GetDelegationUsersByIdGroupIdEventIdHierarchy(groupId, eventId, hierarchyId));
        }

        public List<ConditionDTO> GetConditionsByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            return DTOAssembler.CreateGroupConditions(DelegateService._EventBusinessService.GetConditionsByIdGroupIdEventIdHierarchy(groupId, eventId, hierarchyId));
        }

        public ArrayList GetEventConditionsByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            return DelegateService._EventBusinessService.GetEventConditionsByIdGroupIdEventIdHierarchy(groupId, eventId, hierarchyId);
        }

        public List<OperatorConditionDTO> GetOperatorConditionByEntityIdQueryTypeId(int entityId, int queryTypeId)
        {
            return DTOAssembler.CreateOperators(DelegateService._EventBusinessService.GetOperatorConditionByIdEntityIdQueryType(entityId, queryTypeId));
        }

        public List<GenericListDTO> GetValuesByGroupIdEntityIdEventIdOperatorId(int groupId, int entityId, int eventId, List<GenericListDTO> anotherDependences)
        {
            List<Objects> objectAnotherDependences = new List<Objects>();
            if(anotherDependences != null)
            {
                foreach (GenericListDTO item in anotherDependences)
                {
                    objectAnotherDependences.Add(new Objects
                    {
                        Id = item.Id,
                        Description = item.Description
                    });
                }
            }

            return DTOAssembler.CreateValues(DelegateService._EventBusinessService.GetValuesByIdGroupIdEntityIdEventIdOperator(groupId, entityId, eventId, objectAnotherDependences));
        }

        public EventGroupDTO CreateGroupEvent(EventGroupDTO eventGroupDTO)
        {
            DelegateService._EventBusinessService.CreateEventsGroup(ModelAssembler.CreateEventGroup(eventGroupDTO));

            return eventGroupDTO;
        }

        public EventGroupDTO DeleteGroupEvent(EventGroupDTO eventGroupDTO)
        {
            DelegateService._EventBusinessService.DeleteEventsGroup(ModelAssembler.CreateEventGroup(eventGroupDTO));

            return eventGroupDTO;
        }

        public EventGroupDTO UpdateGroupEvent(EventGroupDTO eventGroupDTO)
        {
            DelegateService._EventBusinessService.UpdateEventsGroup(ModelAssembler.CreateEventGroup(eventGroupDTO));

            return eventGroupDTO;
        }

        public EntityDTO DeleteEntity(int entityId)
        {
            EntityDTO entityDTO = new EntityDTO();

            DelegateService._EventBusinessService.DeleteEventEntity(entityId);

            return entityDTO;
        }

        public EntityDTO CreateEntity(EntityDTO entityDTO)
        {
            DelegateService._EventBusinessService.CreateEventEntity(ModelAssembler.CreateEntity(entityDTO));

            return entityDTO;
        }

        public ConditionGroupDTO CreateConditionGroup(ConditionGroupDTO conditionGroupDTO)
        {
            DelegateService._EventBusinessService.CreateConditionsGroup(ModelAssembler.CreateConditionGroup(conditionGroupDTO));

            return conditionGroupDTO;
        }

        public EventConditionDTO CreateAssignEntity(EventConditionDTO eventConditionDTO)
        {
            DelegateService._EventBusinessService.AssignEntitiesByIdConditionIdEntities(eventConditionDTO.Id, ModelAssembler.CreateAssignEntity(eventConditionDTO));

            return eventConditionDTO;
        }

        public EventConditionDTO DeleteCondition(int conditionId)
        {
            EventConditionDTO conditionDTO = new EventConditionDTO();

            DelegateService._EventBusinessService.DeleteConditionsGroup(conditionId);

            return conditionDTO;
        }

        public EventDTO CreateCompanyEvent(EventDTO events, List<GenericListDTO> prefixes, List<GenericListDTO> executions, List<GenericListDTO> rejectCauses)
        {
            DelegateService._EventBusinessService.CreateEventCompany(ModelAssembler.CreateEvent(events), ModelAssembler.CreateListGeneric(prefixes), ModelAssembler.CreateListGeneric(executions), ModelAssembler.CreateObjectGeneric(rejectCauses));

            return events;
        }

        public EventDTO UpdateCompanyEvent(EventDTO events, List<GenericListDTO> prefixes, List<GenericListDTO> executions, List<GenericListDTO> rejectCauses)
        {
            DelegateService._EventBusinessService.UpdateEventCompany(ModelAssembler.CreateEvent(events), ModelAssembler.CreateListGeneric(prefixes), ModelAssembler.CreateListGeneric(executions), ModelAssembler.CreateObjectGeneric(rejectCauses));

            return events;
        }

        public EventDTO DeleteEvent(int groupEventId, int eventId)
        {
            EventDTO eventDTO = new EventDTO();

            DelegateService._EventBusinessService.DeleteEventCompany(groupEventId, eventId);

            return eventDTO;
        }

        public DelegationUserDTO UpdateDelegationUser(DelegationUserDTO user, int groupId, int eventId, int hierarchyId)
        {
            DelegateService._EventBusinessService.UpdateDelegationUser(ModelAssembler.CreateDelegationUser(user), groupId, eventId, hierarchyId);

            return user;
        }

        public DependenciesDTO UpdateDependencies(DependenciesDTO dependencies)
        {
            DelegateService._EventBusinessService.UpdateDependence(ModelAssembler.CreateDependencies(dependencies));

            return dependencies;
        }

        public DependenciesDTO CreateDependencies(DependenciesDTO dependencies)
        {
            DelegateService._EventBusinessService.CreateDependence(ModelAssembler.CreateDependencies(dependencies));

            return dependencies;
        }

        public DependenciesDTO DeleteDependencies(DependenciesDTO dependencies)
        {
            DelegateService._EventBusinessService.DeleteDependence(ModelAssembler.CreateDependencies(dependencies));

            return dependencies;
        }

        public ConditionDTO DeleteConditionEntity(ConditionDTO condition)
        {
            DelegateService._EventBusinessService.DeleteConditionsEntity(condition.GroupEventId, condition.EventId, condition.DelegationId, condition.ComparatorId);

            return condition;
        }
    }
}