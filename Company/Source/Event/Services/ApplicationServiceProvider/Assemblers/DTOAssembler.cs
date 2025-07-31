using Sistran.Company.Application.Event.ApplicationService.DTOs;
using Sistran.Company.Application.Event.BusinessService.Models;
using Sistran.Core.Application.EventsServices.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.Event.ApplicationService.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        public static List<ModuleDTO> CreateModules(List<CompanyModule> modules)
        {
            List<ModuleDTO> listModules = new List<ModuleDTO>();

            foreach (CompanyModule module in modules)
            {
                listModules.Add(CreateModule(module));
            }

            return listModules;
        }
        public static ModuleDTO CreateModule(CompanyModule module)
        {
            return new ModuleDTO
            {
                Description = module.Description,
                Id = module.Id
            };
        }

        public static List<SubModuleDTO> CreateSubModules(List<CompanySubModule> subModules)
        {
            List<SubModuleDTO> listSubModules = new List<SubModuleDTO>();

            foreach (CompanySubModule module in subModules)
            {
                listSubModules.Add(CreateSubModule(module));
            }

            return listSubModules;
        }
        public static SubModuleDTO CreateSubModule(CompanySubModule module)
        {
            return new SubModuleDTO
            {
                Description = module.Description,
                Id = module.Id,
                ModuleId = module.ModuleId
            };
        }

        public static List<EventGroupDTO> CreateEvents(List<CompanyEventGroup> eventGroups)
        {
            List<EventGroupDTO> listSubModules = new List<EventGroupDTO>();

            foreach (CompanyEventGroup eventGroup in eventGroups)
            {
                listSubModules.Add(CreateEvent(eventGroup));
            }

            return listSubModules;
        }
        public static EventGroupDTO CreateEvent(CompanyEventGroup eventGroup)
        {
            return new EventGroupDTO
            {
                Id = eventGroup.Id,
                ModuleId = eventGroup.ModuleId,
                SubmoduleId = eventGroup.SubmoduleId,
                GroupEventDescription = eventGroup.GroupEventDescription,
                Enabled = eventGroup.Enabled,
                AuthorizationReport = eventGroup.AuthorizationReport,
                ProcedureAuthorized = eventGroup.ProcedureAuthorized,
                ProcedureReject = eventGroup.ProcedureReject
            };
        }

        public static List<GenericListDTO> CreateEntities(List<EventEntity> entities)
        {
            List<GenericListDTO> listSubModules = new List<GenericListDTO>();

            foreach (EventEntity entity in entities)
            {
                listSubModules.Add(CreateEntity(entity));
            }

            return listSubModules;
        }
        public static GenericListDTO CreateEntity(EventEntity entity)
        {
            return new GenericListDTO
            {
                Id = entity.EntityId,
                Description = entity.Description
            };
        }

        public static List<GenericListDTO> CreateQueryTypes(List<EventQueryType> queryTypes)
        {
            List<GenericListDTO> listQueryTypes = new List<GenericListDTO>();

            foreach (EventQueryType queryType in queryTypes)
            {
                listQueryTypes.Add(CreateQueryType(queryType));
            }

            return listQueryTypes;
        }

        public static GenericListDTO CreateQueryType(EventQueryType queryType)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(queryType.QueryTypeCode),
                Description = queryType.Description
            };
        }

        public static List<GenericListDTO> CreateLevels(List<EventLevels> levels)
        {
            List<GenericListDTO> listLevels = new List<GenericListDTO>();

            foreach (EventLevels level in levels)
            {
                listLevels.Add(CreateLevel(level));
            }

            return listLevels;
        }

        public static GenericListDTO CreateLevel(EventLevels level)
        {
            return new GenericListDTO
            {
                Id = level.LevelId,
                Description = level.Description
            };
        }

        public static List<EntityDTO> CreateGroupEntities(List<EventEntity> entities)
        {
            List<EntityDTO> listEntities = new List<EntityDTO>();

            foreach (EventEntity entity in entities)
            {
                listEntities.Add(CreateGroupEntity(entity));
            }

            return listEntities;
        }

        public static EntityDTO CreateGroupEntity(EventEntity entity)
        {
            return new EntityDTO
            {
                Id = entity.EntityId,
                EntityDescription = entity.Description,
                QueryTypeId = Convert.ToInt32(entity.QueryType.QueryTypeCode),
                QueryTypeDescription = entity.QueryType.Description,
                SourceTable = entity.SourceTable,
                SourceCode = entity.SourceCode,
                SourceDescription = entity.SourceDescription,
                JoinTable = entity.JoinTable,
                JoinSourceField = entity.JoinSourceField,
                JoinTargetField = entity.JoinTargetField,
                ConditionJoinWhere = entity.ParamWhere,
                LevelId = entity.LevelId,
                ValidationTypeId = entity.ValidationType.ValidationTypeCode,
                ValidationTypeDescription = entity.ValidationType.Description,
                Procedure = entity.ValidationType.ProcedureInd,
                ValidationProcedure = entity.ValidationProcedure,
                ValidationTable = entity.ValidationTable,
                ValidationKeyField = entity.ValidationKeyField,
                DataKeyTypeId = entity.DataKeyType.DataTypeCode,
                DataKeyTypeDescription = entity.DataKeyType.Description,
                Key1Field = entity.Key1Field,
                Key2Field = entity.Key2Field,
                Key3Field = entity.Key3Field,
                Key4Field = entity.Key4Field,
                ValidationField = entity.ValidationField,
                DataFieldTypeId = entity.DataFieldType.DataTypeCode,
                DataFieldTypeDescription = entity.DataFieldType.Description,
                ConditionWhere = entity.WhereAdd,
                GroupBy = entity.GroupByInd
            };
        }

        public static List<GenericListDTO> CreateTables(List<Objects> tables)
        {
            List<GenericListDTO> listLevels = new List<GenericListDTO>();

            foreach (Objects table in tables)
            {
                listLevels.Add(CreateTable(table));
            }

            return listLevels;
        }

        public static GenericListDTO CreateTable(Objects table)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(table.Id),
                Description = table.Description
            };
        }
        
        public static List<GenericListDTO> CreateStoredProcedures(List<Objects> storedProcedures)
        {
            List<GenericListDTO> listStoredProcedures = new List<GenericListDTO>();

            foreach (Objects storedProcedure in storedProcedures)
            {
                listStoredProcedures.Add(CreateStoredProcedure(storedProcedure));
            }

            return listStoredProcedures;
        }

        public static GenericListDTO CreateStoredProcedure(Objects storedProcedure)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(storedProcedure.Id),
                Description = storedProcedure.Description
            };
        }

        public static List<GenericListDTO> CreateValidationTypes(List<EventValidationType> validationTypes)
        {
            List<GenericListDTO> listValidationTypes = new List<GenericListDTO>();

            foreach (EventValidationType validationType in validationTypes)
            {
                listValidationTypes.Add(CreateValidationType(validationType));
            }

            return listValidationTypes;
        }

        public static GenericListDTO CreateValidationType(EventValidationType validationType)
        {
            return new GenericListDTO
            {
                Id = validationType.ValidationTypeCode,
                Description = validationType.Description
            };
        }

        public static List<GenericListDTO> CreateDataTypes(List<EventDataType> dataTypes)
        {
            List<GenericListDTO> listDataTypes = new List<GenericListDTO>();

            foreach (EventDataType dataType in dataTypes)
            {
                listDataTypes.Add(CreateDataType(dataType));
            }

            return listDataTypes;
        }

        public static GenericListDTO CreateDataType(EventDataType dataType)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(dataType.DataTypeCode),
                Description = dataType.Description
            };
        }

        public static List<GenericListDTO> CreateColumnsTables(List<Objects> columnsTables)
        {
            List<GenericListDTO> listDataTypes = new List<GenericListDTO>();

            foreach (Objects columnsTable in columnsTables)
            {
                listDataTypes.Add(CreateColumnsTable(columnsTable));
            }

            return listDataTypes;
        }

        public static GenericListDTO CreateColumnsTable(Objects columnsTable)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(columnsTable.Id),
                Description = columnsTable.Description
            };
        }

        public static List<ConditionGroupDTO> CreateConditions(List<EventConditionGroup> conditions)
        {
            List<ConditionGroupDTO> listConditions = new List<ConditionGroupDTO>();

            foreach (EventConditionGroup condition in conditions)
            {
                listConditions.Add(CreateCondition(condition));
            }

            return listConditions;
        }

        public static ConditionGroupDTO CreateCondition(EventConditionGroup condition)
        {
            ConditionGroupDTO conditionGroup = new ConditionGroupDTO
            {
                Id = condition.ConditionId,
                Description = condition.Description
            };

            foreach (EventEntity entity in condition.EventEntities)
            {
                conditionGroup.RelatedEntities += entity.Description + ", ";
                conditionGroup.RelatedEntitiesId += entity.EntityId+ ",";
            }

            return conditionGroup;
        }

        public static List<DependenciesDTO> CreateDependencies(List<EntityDependencies> dependencies)
        {
            List<DependenciesDTO> listDependencies = new List<DependenciesDTO>();

            foreach (EntityDependencies dependence in dependencies)
            {
                listDependencies.Add(CreateDependence(dependence));
            }

            return listDependencies;
        }

        public static DependenciesDTO CreateDependence(EntityDependencies dependence)
        {
            return new DependenciesDTO
            {
                EntityId = dependence.EntityId,
                EntityDescription = dependence.EntityDescription,
                DependesId = dependence.DependsId,
                DependsDescription = dependence.DependsDescription,
                Column = dependence.ColumnName,
                ConditionsId = dependence.ConditionId
            };
        }

        public static List<EventDTO> CreateGroupEvents(List<EventCompany> eventCompanies)
        {
            List<EventDTO> listEvents = new List<EventDTO>();

            foreach (EventCompany eventCompany in eventCompanies)
            {
                listEvents.Add(CreateEvent(eventCompany));
            }

            return listEvents;
        }

        public static EventDTO CreateEvent(EventCompany eventCompany)
        {
            return new EventDTO
            {
                Id = eventCompany.EventId,
                Description = eventCompany.Description,
                GroupEventId = eventCompany.EventsGroup.GroupEventId,
                GroupEventDescription = eventCompany.EventsGroup.Description,
                ValidationTypeId = eventCompany.ValidationType.ValidationTypeCode,
                ValidationTypeDescription = eventCompany.ValidationType.Description,
                ProcedureName = eventCompany.ProcedureName,
                ConditionDescription = eventCompany.EventConditionGroup.Description,
                ConditionId = eventCompany.EventConditionGroup.ConditionId,
                Enabled = eventCompany.Enabled,
                EnabledStop = eventCompany.EnabledStop,
                EnabledAuthorize = eventCompany.EnabledAuthorize,
                DescriptionErrorMessage = eventCompany.DescriptionErrorMessage,
                TypeCode = eventCompany.TypeCode
            };
        }

        public static List<GenericListDTO> CreateAccess(List<Objects> objectsAccess)
        {
            List<GenericListDTO> listAccess = new List<GenericListDTO>();

            foreach (Objects access in objectsAccess)
            {
                listAccess.Add(CreateAccess(access));
            }

            return listAccess;
        }

        public static GenericListDTO CreateAccess(Objects access)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(access.Id),
                Description = access.Description
            };
        }

        public static List<GroupEventDTO> CreateGroupPrefixes(List<EventGroupPrefix> groupPrefixes)
        {
            List<GroupEventDTO> listGroupPrefix = new List<GroupEventDTO>();

            foreach (EventGroupPrefix groupPrefix in groupPrefixes)
            {
                listGroupPrefix.Add(CreateGroupPrefix(groupPrefix));
            }

            return listGroupPrefix;
        }

        public static GroupEventDTO CreateGroupPrefix(EventGroupPrefix groupPrefix)
        {
            return new GroupEventDTO
            {
                EventId = groupPrefix.EventId,
                GroupEventId = groupPrefix.GroupEventId,
                PrefixId = groupPrefix.PrefixCode
            };
        }
        
        public static List<GroupEventDTO> CreateGroupAccess(List<EventAsocObj> groupAccess)
        {
            List<GroupEventDTO> listGroupAccess = new List<GroupEventDTO>();

            foreach (EventAsocObj access in groupAccess)
            {
                listGroupAccess.Add(CreateGroupAccess(access));
            }

            return listGroupAccess;
        }

        public static GroupEventDTO CreateGroupAccess(EventAsocObj access)
        {
            return new GroupEventDTO
            {
                EventId = access.EventId,
                GroupEventId = access.GroupEventId,
                AccessId = access.AccessId
            };
        }

        public static List<GroupDelegationDTO> CreateGroupDelegations(List<EventDelegationSP> delgations)
        {
            List<GroupDelegationDTO> listDelegations = new List<GroupDelegationDTO>();

            foreach (EventDelegationSP delegation in delgations)
            {
                listDelegations.Add(CreateGroupDelegation(delegation));
            }

            return listDelegations;
        }

        public static GroupDelegationDTO CreateGroupDelegation(EventDelegationSP delegation)
        {
            return new GroupDelegationDTO
            {
                Id = delegation.DelegationId,
                Description = delegation.Description,
                HierarchyId = delegation.HierarchyId
            };
        }

        public static List<DelegationUserDTO> CreateDelegationUsers(List<DelegationUser> delgationUsers)
        {
            List<DelegationUserDTO> listDelegationUsers = new List<DelegationUserDTO>();

            foreach (DelegationUser delgationUser in delgationUsers)
            {
                listDelegationUsers.Add(CreateDelegationUser(delgationUser));
            }

            return listDelegationUsers;
        }

        public static DelegationUserDTO CreateDelegationUser(DelegationUser delgationUser)
        {
            return new DelegationUserDTO
            {
                Authorized = delgationUser.AuthorizedInd,
                Notificated = delgationUser.NotificatedInd,
                NotificatedDefault = delgationUser.NotificatedDefault,
                Description = delgationUser.Description,
                Email = delgationUser.Email,
                PersonId = delgationUser.PersonId,
                UserId = delgationUser.UserId,
                UserName = delgationUser.UserName
            };
        }

        public static List<ConditionDTO> CreateGroupConditions(List<EventCondition> conditions)
        {
            List<ConditionDTO> listDelegations = new List<ConditionDTO>();

            foreach (EventCondition condition in conditions)
            {
                listDelegations.Add(CreateGroupCondition(condition));
            }

            return listDelegations;
        }

        public static ConditionDTO CreateGroupCondition(EventCondition condition)
        {
            return new ConditionDTO
            {
                GroupEventId = condition.GroupEventId,
                EventId = condition.EntityId,
                DelegationId = condition.DelegationId,
                EntityId = condition.EntityId,
                ConditionQuantity = condition.ConditionQuantity,
                EventQuantity = condition.EventQuantity,
                ComparatorId = condition.ComparatorCode,
                Condition = condition.ConditionValue
            };
        }

        public static List<OperatorConditionDTO> CreateOperators(List<EventComparisonType> comparisonTypes)
        {
            List<OperatorConditionDTO> listComparisonTypes = new List<OperatorConditionDTO>();

            foreach (EventComparisonType comparisonType in comparisonTypes)
            {
                listComparisonTypes.Add(CreateOperator(comparisonType));
            }

            return listComparisonTypes;
        }

        public static OperatorConditionDTO CreateOperator(EventComparisonType comparisonType)
        {
            return new OperatorConditionDTO
            {
                ComparatorId = comparisonType.ComparatorCode,
                Combo = comparisonType.ComboInd,
                Description = comparisonType.Description,
                NumValues = comparisonType.NumValues,
                Query = comparisonType.QueryInd,
                SmallDesc = comparisonType.SmallDesc
            };
        }

        public static List<GenericListDTO> CreateValues(List<Objects> values)
        {
            List<GenericListDTO> listValues = new List<GenericListDTO>();

            foreach (Objects value in values)
            {
                listValues.Add(CreateValue(value));
            }

            return listValues;
        }

        public static GenericListDTO CreateValue(Objects value)
        {
            return new GenericListDTO
            {
                Id = Convert.ToInt32(value.Id),
                Description = value.Description
            };
        }
        
    }
}