using Sistran.Company.Application.Event.ApplicationService.DTOs;
using Sistran.Company.Application.Event.BusinessService.Models;
using Sistran.Core.Application.EventsServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.Event.ApplicationService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        /*public static List<ModuleDTO> CreateModules(List<CompanyModule> modules)
        {
            List<ModuleDTO> listModules = new List<ModuleDTO>();

            foreach (CompanyModule module in modules)
            {
                listModules.Add(CreateModule(module));
            }

            return listModules;
        }*/
        public static EventsGroup CreateEventGroup(EventGroupDTO eventGroupDTO)
        {
            EventsGroup eventGroup = new EventsGroup()
            {
                EnabledInd = eventGroupDTO.Enabled,
                ModuleCode = eventGroupDTO.ModuleId,
                SubmoduleCode = eventGroupDTO.SubmoduleId,
                ProcedureReject = eventGroupDTO.ProcedureReject,
                Description = eventGroupDTO.GroupEventDescription,
                AuthorizationReport = eventGroupDTO.AuthorizationReport,
                ProcedureAuthorized = eventGroupDTO.ProcedureAuthorized,
            };

            if(eventGroupDTO.Id > 0)
            {
                eventGroup.GroupEventId = eventGroupDTO.Id;
            }

            return eventGroup;
        }

        public static EventEntity CreateEntity(EntityDTO entityDTO)
        {
            EventEntity eventEntity = new EventEntity()
            {
                DataFieldType = new EventDataType
                {
                    DataTypeCode = entityDTO.DataFieldTypeId,
                    Description = entityDTO.DataFieldTypeDescription
                },
                DataKeyType = new EventDataType
                {
                    DataTypeCode = entityDTO.DataKeyTypeId,
                    Description = entityDTO.DataKeyTypeDescription
                },
                QueryType = new EventQueryType
                {
                    Description = entityDTO.QueryTypeDescription,
                    QueryTypeCode = entityDTO.QueryTypeId
                },
                ValidationType = new EventValidationType
                {
                    ValidationTypeCode = entityDTO.ValidationTypeId,
                    Description = entityDTO.ValidationTypeDescription
                },
                GroupByInd = entityDTO.GroupBy,
                WhereAdd = entityDTO.ConditionWhere,
                ParamWhere = entityDTO.ConditionJoinWhere,
                Key1Field = entityDTO.Key1Field,
                Key2Field = entityDTO.Key2Field,
                Key3Field = entityDTO.Key3Field,
                Key4Field = entityDTO.Key4Field,
                LevelId = entityDTO.LevelId,
                ValidationField = entityDTO.ValidationField,
                ValidationKeyField = entityDTO.ValidationKeyField,
                ValidationProcedure = entityDTO.ValidationProcedure,
                ValidationTable = entityDTO.ValidationTable,
                SourceTable = entityDTO.SourceTable,
                SourceDescription = entityDTO.SourceDescription,
                SourceCode = entityDTO.SourceCode,
                JoinSourceField = entityDTO.JoinSourceField,
                JoinTable = entityDTO.JoinTable,
                JoinTargetField = entityDTO.JoinTargetField,
                Description = entityDTO.Description
            };

            if (entityDTO.Id > 0)
            {
                eventEntity.EntityId = entityDTO.Id;
            }

            return eventEntity;
        }

        public static EventConditionGroup CreateConditionGroup(ConditionGroupDTO conditionGroupDTO)
        {
            EventConditionGroup eventConditionGroup = new EventConditionGroup 
            {
                Description = conditionGroupDTO.Description
            };

            if(conditionGroupDTO.Id > 0)
            {
                eventConditionGroup.ConditionId = conditionGroupDTO.Id;
            }

            return eventConditionGroup;
        }

        public static List<int> CreateAssignEntity(EventConditionDTO entityConditions)
        {
            List<int> entities  = new List<int>();

            foreach (EntityDTO entityCondition in entityConditions.entities)
            {
                if(entityCondition.Id > 0)
                    entities.Add(entityCondition.Id);
            }

            return entities;
        }

        public static List<int> CreateListGeneric(List<GenericListDTO> genericList)
        {
            List<int> listInts = new List<int>();

            foreach (GenericListDTO element in genericList)
            {
                if (element.Id > 0)
                    listInts.Add(element.Id);
            }

            return listInts;
        }

        public static List<Objects> CreateObjectGeneric(List<GenericListDTO> genericList)
        {
            List<Objects> listInts = new List<Objects>();

            foreach (GenericListDTO element in genericList)
            {
                if (element.Id > 0)
                {
                    listInts.Add(new Objects() { 
                        Id = element.Id
                    });
                }
                    
            }

            return listInts;
        }

        public static EventCompany CreateEvent(EventDTO eventDTO)
        {
            EventCompany eventCompany = new EventCompany()
            {
                Description = eventDTO.Description,
                DescriptionErrorMessage = eventDTO.DescriptionErrorMessage,
                Enabled = eventDTO.Enabled,
                EnabledAuthorize = eventDTO.EnabledAuthorize,
                EnabledStop = eventDTO.EnabledStop,
                EventConditionGroup = new EventConditionGroup
                {
                    ConditionId = eventDTO.ConditionId
                },
                ValidationType = new EventValidationType
                {
                    ValidationTypeCode = eventDTO.ValidationTypeId
                },
                EventsGroup = new EventsGroup
                {
                    GroupEventId = eventDTO.GroupEventId
                },
                ProcedureName = eventDTO.ProcedureName,
                TypeCode = eventDTO.TypeCode
            };

            if (eventDTO.Id > 0)
            {
                eventCompany.EventId = eventDTO.Id;
            }

            return eventCompany;
        }

        public static DelegationUser CreateDelegationUser(DelegationUserDTO delegationUserDTO)
        {
            return new DelegationUser()
            {
                UserId = delegationUserDTO.UserId,
                UserName = delegationUserDTO.UserName,
                AuthorizedInd = delegationUserDTO.Authorized,
                NotificatedInd = delegationUserDTO.Notificated,
                NotificatedDefault = delegationUserDTO.NotificatedDefault,
                Email = delegationUserDTO.Email,
                Description = delegationUserDTO.Description,
                PersonId = delegationUserDTO.PersonId
            };
        }

        public static EntityDependencies CreateDependencies(DependenciesDTO dependenciesDTO)
        {
            return new EntityDependencies()
            {
                ColumnName = dependenciesDTO.Column,
                ConditionId = dependenciesDTO.ConditionsId,
                DependsDescription = dependenciesDTO.DependsDescription,
                DependsId = dependenciesDTO.DependesId,
                EntityDescription = dependenciesDTO.EntityDescription,
                EntityId = dependenciesDTO.EntityId
            };
        }
    }
}