using Sistran.Company.Application.Event.BusinessService.Models;
using Sistran.Core.Application.EventsServices.Models;
using Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.Event.BusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        internal static List<CompanyModule> CreateModules(ModuleSubmoduleServicesQueryModel modules)
        {
            List<CompanyModule> modulesList = new List<CompanyModule>();

            foreach (ModuleSubmoduleServiceQueryModel module in modules.ModuleSubModuleQueryModel)
            {
                modulesList.Add(CreateModule(module));
            }

            return modulesList;
        }

        internal static CompanyModule CreateModule(ModuleSubmoduleServiceQueryModel module)
        {
            return new CompanyModule
            {
                Description = module.Description,
                Id = module.Id
            };
        }

        internal static List<CompanySubModule> CreateSubModules(SubModulesServiceQueryModel subModules)
        {
            List<CompanySubModule> subModuleList = new List<CompanySubModule>();

            foreach (SubModuleServicesQueryModel subModule in subModules.SubModuleServiceQueryModels)
            {
                subModuleList.Add(CreatesubModule(subModule));
            }

            return subModuleList;
        }

        internal static CompanySubModule CreatesubModule(SubModuleServicesQueryModel subModule)
        {
            return new CompanySubModule
            {
                Description = subModule.Description,
                Id = subModule.Id,
                ModuleId = Convert.ToInt32(subModule.ModuleId)
            };
        }

        internal static List<CompanyEventGroup> CreateEventGroups(List<EventsGroup> eventsGroup)
        {
            List<CompanyEventGroup> companyEventGroupList = new List<CompanyEventGroup>();

            foreach (EventsGroup eventGroup in eventsGroup)
            {
                companyEventGroupList.Add(CreateEventGroup(eventGroup));
            }

            return companyEventGroupList;
        }

        internal static CompanyEventGroup CreateEventGroup(EventsGroup eventGroup)
        {
            return new CompanyEventGroup
            {
                Id = eventGroup.GroupEventId,
                ModuleId = eventGroup.ModuleCode,
                SubmoduleId = eventGroup.SubmoduleCode,
                GroupEventDescription = eventGroup.Description,
                Enabled = eventGroup.EnabledInd,
                AuthorizationReport = eventGroup.AuthorizationReport,
                ProcedureAuthorized = eventGroup.ProcedureAuthorized,
                ProcedureReject = eventGroup.ProcedureReject
            };
        }

        internal static List<CompanyEntity> CreateEntities(List<EventEntity> entities)
        {
            List<CompanyEntity> companyEntitiesList = new List<CompanyEntity>();

            foreach (EventEntity entity in entities)
            {
                companyEntitiesList.Add(CreateEntity(entity));
            }

            return companyEntitiesList;
        }

        internal static CompanyEntity CreateEntity(EventEntity entity)
        {
            return new CompanyEntity
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
    }
}