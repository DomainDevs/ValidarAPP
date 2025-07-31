using System.Collections.Generic;
using System.Linq;
using Entity = Sistran.Core.Application.Events.Entities;
using Model = Sistran.Core.Application.EventsServices.Models;

namespace Sistran.Core.Application.EventsServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        #region EventsGroup
        /// <summary>
        /// Crea un Entity.CoEventGroup a partir de un Model.EventsGroup
        /// </summary>
        /// <param name="model">Model.EventsGroup</param>
        /// <returns></returns>
        public static Entity.CoEventGroup CreateEventsGroup(Model.EventsGroup model)
        {
            return new Entity.CoEventGroup(model.GroupEventId)
            {
                Description = model.Description,
                ModuleCode = model.ModuleCode,
                SubmoduleCode = model.SubmoduleCode,
                EnabledInd = model.EnabledInd,
                AuthorizationReport = model.AuthorizationReport,
                ProcedureAuthorized = model.ProcedureAuthorized,
                ProcedureReject = model.ProcedureReject
            };
        }

        /// <summary>
        /// Crea una lista de Entity.CoEventGroup a partir de una lista de  Model.EventsGroup
        /// </summary>
        /// <param name="models">lista de Model.EventsGroup</param>
        /// <returns></returns>
        public static List<Entity.CoEventGroup> CreateEventsGroup(List<Model.EventsGroup> models)
        {
            return models.Select(model => new Entity.CoEventGroup(model.GroupEventId)
            {
                Description = model.Description,
                ModuleCode = model.ModuleCode,
                SubmoduleCode = model.SubmoduleCode,
                EnabledInd = model.EnabledInd,
                AuthorizationReport = model.AuthorizationReport,
                ProcedureAuthorized = model.ProcedureAuthorized,
                ProcedureReject = model.ProcedureReject
            }).ToList();
        }

     
        #endregion

        #region Module
        /// <summary>
        /// Crea un Entity.Modules a partir de un Model.Module
        /// </summary>
        /// <param name="model">Model.Module</param>
        /// <returns></returns>
        public static Entity.Modules CreateModule(Model.Module model)
        {
            return new Entity.Modules(model.ModuleCode)
            {
                Description = model.Description,
                Enabled = model.Enabled,
                ExpirationDate = model.ExpirationDate,
                VirtualFolder = model.VirtualFolder
            };
        }

        /// <summary>
        /// Crea una lista Entity.Modules a partir de una lista de Model.Module 
        /// </summary>
        /// <param name="models">lista de Model.Module</param>
        /// <returns></returns>
        public static List<Entity.Modules> CreateModule(List<Model.Module> models)
        {
            return models.Select(model => new Entity.Modules(model.ModuleCode)
            {
                Description = model.Description,
                Enabled = model.Enabled,
                ExpirationDate = model.ExpirationDate,
                VirtualFolder = model.VirtualFolder
            }).ToList();
        }
        #endregion

        #region SubModule
        /// <summary>
        /// Crea un Entity.Submodules a partir de un Model.SubModule
        /// </summary>
        /// <param name="model">Model.SubModule</param>
        /// <returns></returns>
        public static Entity.Submodules CreateSubModule(Model.SubModule model)
        {
            return new Entity.Submodules(model.ModuleCode, model.SubmoduleCode)
            {
                Description = model.Description,
                Enabled = model.Enabled,
                ExpirationDate = model.ExpirationDate,
                VirtualFolder = model.VirtualFolder,
                ParentModuleCode = model.ParentModuleCode,
                ParentSubmoduleCode = model.ParentSubmoduleCode
            };
        }

     

        /// <summary>
        /// Crea una lista Entity.Submodules a partir de una lista Model.SubModule
        /// </summary>
        /// <param name="models">lista de Model.SubModule</param>
        /// <returns></returns>
        public static List<Entity.Submodules> CreateSubModule(List<Model.SubModule> models)
        {
            return models.Select(model => new Entity.Submodules(model.ModuleCode, model.SubmoduleCode)
            {
                Description = model.Description,
                Enabled = model.Enabled,
                ExpirationDate = model.ExpirationDate,
                VirtualFolder = model.VirtualFolder,
                ParentModuleCode = model.ParentModuleCode,
                ParentSubmoduleCode = model.ParentSubmoduleCode
            }
          ).ToList();
        }

       
        #endregion

        #region EventEntity
        /// <summary>
        /// Crea un coEventEntity a a partir de un EventEntity
        /// </summary>
        /// <param name="entity">Entity.CoEventEntity</param>
        /// <returns></returns>
        public static Entity.CoEventEntity CreateEventEntity(Model.EventEntity entity)
        {
            return new Entity.CoEventEntity(entity.EntityId)
            {
                EntityId = entity.EntityId,
                Description = entity.Description,
                QueryTypeCode = entity.QueryType.QueryTypeCode,
                SourceTable = entity.SourceTable,
                SourceCode = entity.SourceCode,
                SourceDescription = entity.SourceDescription,
                JoinTable = entity.JoinTable,
                JoinSourceField = entity.JoinSourceField,
                JoinTargetField = entity.JoinTargetField,
                ParamWhere = entity.ParamWhere,
                LevelId = entity.LevelId,
                ValidationTypeCode = entity.ValidationType.ValidationTypeCode,
                ValidationProcedure = entity.ValidationProcedure,
                ValidationTable = entity.ValidationTable,
                ValidationKeyField = entity.ValidationKeyField,
                DataKeyTypeCode = entity.DataKeyType.DataTypeCode,
                Key1Field = entity.Key1Field,
                Key2Field = entity.Key2Field,
                Key3Field = entity.Key3Field,
                Key4Field = entity.Key4Field,
                ValidationField = entity.ValidationField,
                DataFieldTypeCode = entity.DataFieldType.DataTypeCode,
                WhereAdd = entity.WhereAdd,
                GroupByInd = entity.GroupByInd
            };
        }
        #endregion

        #region conditionGroup
        /// <summary>
        /// crea un  Entity.CoEventConditionGroup  a partir de un Model.EventConditionGroup
        /// </summary>
        /// <param name="conditionsGroup">Model.EventConditionGroup</param>
        /// <returns></returns>
        public static Entity.CoEventConditionGroup CreateConditionsGroupEntity(Model.EventConditionGroup conditionsGroup)
        {
            return new Entity.CoEventConditionGroup(conditionsGroup.ConditionId)
            {
                Description = conditionsGroup.Description
            };
        }
        #endregion

        #region dependence
        /// <summary>
        /// crea un Entity.CoEntityDependencies a partiur de un Model.EntityDependencies
        /// </summary>
        /// <param name="dependence">Model.EntityDependencies</param>
        /// <returns>Entity.CoEntityDependencies</returns>
        public static Entity.CoEntityDependencies CreateDependenceEntity(Model.EntityDependencies dependence)
        {
            return new Entity.CoEntityDependencies(dependence.ConditionId, dependence.EntityId, dependence.DependsId)
            {
                ColumnName = dependence.ColumnName
            };
        }

        
        #endregion

        #region EventCompany
        /// <summary>
        /// crea un Entity.CoEventCompany a partir de Model.EventCompany
        /// </summary>
        /// <param name="eventCompany">Model.EventCompany</param>
        /// <returns></returns>
        public static Entity.CoEventCompany CreateEventComapany(Model.EventCompany eventCompany)
        {
            return new Entity.CoEventCompany(eventCompany.EventsGroup.GroupEventId, eventCompany.EventId)
            {
                ConditionId = eventCompany.EventConditionGroup.ConditionId,
                Description = eventCompany.Description,
                DescriptionErrorMessage = eventCompany.DescriptionErrorMessage,
                Enabled = eventCompany.Enabled,
                EnabledAuthorize = eventCompany.EnabledAuthorize,
                EnabledStop = eventCompany.EnabledStop,
                ProcedureName = eventCompany.ProcedureName,
                TypeCode = eventCompany.TypeCode,
                ValidationTypeCode = eventCompany.ValidationType.ValidationTypeCode
            };
        }
        #endregion


        #region EventCondition
        /// <summary>
        /// crea un Entity.CoEventCondition a partir de un Model.EventCondition
        /// </summary>
        /// <param name="eventCondition">Model.EventCondition</param>
        /// <returns></returns>
        public static Entity.CoEventCondition CreateEventCondition(Model.EventCondition eventCondition)
        {
            return new Entity.CoEventCondition(eventCondition.GroupEventId, eventCondition.EventId, eventCondition.DelegationId, eventCondition.EntityId, eventCondition.ConditionQuantity, eventCondition.EventQuantity)
            {
                ConditionValue = eventCondition.ConditionValue,
                ComparatorCode = eventCondition.ComparatorCode
            };
        }
        #endregion

        #region AuthorizationUser
        /// <summary>
        /// crea un  Entity.CoEventAuthorizationUsers  a aprtir de un Model.EventAuthorizationUsers 
        /// </summary>
        /// <param name="authorizationUsers"></param>
        /// <returns></returns>
        public static Entity.CoEventAuthorizationUsers CreateEventAuthorizationUsers(Model.EventAuthorizationUsers authorizationUsers)
        {
            return new Entity.CoEventAuthorizationUsers(authorizationUsers.GroupEventId, authorizationUsers.EventId, authorizationUsers.DelegationId, authorizationUsers.UserId);
        }
        #endregion

        #region NotificationUser
        /// <summary>
        /// crea un  Entity.CoEventAuthorizationUsers  a aprtir de un Model.EventAuthorizationUsers 
        /// </summary>
        /// <param name="authorizationUsers"></param>
        /// <returns></returns>
        public static Entity.CoEventNotificationUsers CreateEventNotificationUsers(Model.EventNotificationUsers notificationUsers)
        {
            return new Entity.CoEventNotificationUsers(notificationUsers.GroupEventId, notificationUsers.EventId, notificationUsers.DelegationId, notificationUsers.UserId)
            {
                UserNotifDefault = notificationUsers.UserNotifDefault
            };
        }
        #endregion
    }
}
