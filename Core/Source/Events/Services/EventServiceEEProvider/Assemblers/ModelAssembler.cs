using Sistran.Core.Framework.DAF;
using System.Collections;
using System.Collections.Generic;
using Entity = Sistran.Core.Application.Events.Entities;
using EVENTEN = Sistran.Core.Application.Events.Entities;
using Model = Sistran.Core.Application.EventsServices.Models;

namespace Sistran.Core.Application.EventsServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        #region Objects
        /// <summary>
        /// Crea una lista de Model.Objects a partir de una lista string 
        /// </summary>
        /// <param name="businessCollection">lista de  Entity.CoEventGroup </param>
        /// <returns></returns>
        public static List<Model.Objects> CreateListObjects(ArrayList lista)
        {
            List<Model.Objects> list = new List<Model.Objects>();
            foreach (object[] item in lista)
            {
                list.Add(
                    new Model.Objects
                    {
                        Id = long.Parse(item[0].ToString()),
                        Description = item[1].ToString()
                    });
            }
            return list;
        }
        #endregion

        #region EventsGroup
        /// <summary>
        /// Crea un Model.EventsGroup a partir de un Entity.CoEventGroup 
        /// </summary>
        /// <param name="entity">Entity.CoEventGroup</param>
        /// <returns></returns>
        public static Model.EventsGroup CreateEventsGroup(Entity.CoEventGroup entity)
        {
            return new Model.EventsGroup
            {
                GroupEventId = entity.GroupEventId,
                Description = entity.Description,
                ModuleCode = entity.ModuleCode,
                SubmoduleCode = entity.SubmoduleCode,
                EnabledInd = entity.EnabledInd,
                AuthorizationReport = entity.AuthorizationReport,
                ProcedureAuthorized = entity.ProcedureAuthorized,
                ProcedureReject = entity.ProcedureReject
            };
        }

        /// <summary>
        /// Crea una lista de Model.EventsGroup a partir de una lista de  businessCollection 
        /// </summary>
        /// <param name="businessCollection">lista de  Entity.CoEventGroup </param>
        /// <returns></returns>
        public static List<Model.EventsGroup> CreateListEventsGroup(BusinessCollection businessCollection)
        {
            List<Model.EventsGroup> listEventsGroup = new List<Model.EventsGroup>();

            foreach (Entity.CoEventGroup entity in businessCollection)
            {
                listEventsGroup.Add(
                    new Model.EventsGroup
                    {
                        GroupEventId = entity.GroupEventId,
                        Description = entity.Description,
                        ModuleCode = entity.ModuleCode,
                        SubmoduleCode = entity.SubmoduleCode,
                        EnabledInd = entity.EnabledInd,
                        AuthorizationReport = entity.AuthorizationReport,
                        ProcedureAuthorized = entity.ProcedureAuthorized,
                        ProcedureReject = entity.ProcedureReject
                    });
            }

            return listEventsGroup;
        }
        #endregion

        #region Module
        /// <summary>
        /// Crea un Model.Module a partir de un Entity.Modules 
        /// </summary>
        /// <param name="entity">Entity.Modules</param>
        /// <returns></returns>
        public static Model.Module CreateModule(Entity.Modules entity)
        {
            return new Model.Module
            {
                ModuleCode = entity.ModuleCode,
                Description = entity.Description,
                Enabled = entity.Enabled,
                ExpirationDate = entity.ExpirationDate,
                VirtualFolder = entity.VirtualFolder
            };
        }

        /// <summary>
        /// Crea una lista Model.Module a partir de una businessCollection 
        /// </summary>
        /// <param name="businessCollection">lista de Entity.Modules </param>
        /// <returns></returns>
        public static List<Model.Module> CreateListModule(BusinessCollection businessCollection)
        {
            List<Model.Module> moduleList = new List<Model.Module>();

            foreach (Entity.Modules entity in businessCollection)
            {
                moduleList.Add(
                    new Model.Module
                    {
                        ModuleCode = entity.ModuleCode,
                        Description = entity.Description,
                        Enabled = entity.Enabled,
                        ExpirationDate = entity.ExpirationDate,
                        VirtualFolder = entity.VirtualFolder
                    });
            }
            return moduleList;
        }
        #endregion

        #region SubModule
        /// <summary>
        /// Crea un Model.SubModule a partir de un Entity.Submodules 
        /// </summary>
        /// <param name="entity">Entity.Submodules</param>
        /// <returns></returns>
        public static Model.SubModule CreateSubModule(Entity.Submodules entity)
        {
            return new Model.SubModule
            {
                ModuleCode = entity.ModuleCode,
                SubmoduleCode = entity.SubmoduleCode,
                Description = entity.Description,
                Enabled = entity.Enabled,
                ExpirationDate = entity.ExpirationDate,
                VirtualFolder = entity.VirtualFolder,
                ParentModuleCode = entity.ParentModuleCode,
                ParentSubmoduleCode = entity.ParentSubmoduleCode
            };
        }

        /// <summary>
        /// Crea una lista Model.SubModule a partir de una businessCollection
        /// </summary>
        /// <param name=" businessCollection">lista de Entity.Submodules </param>
        /// <returns></returns>
        public static List<Model.SubModule> CreateListSubModule(BusinessCollection businessCollection)
        {
            List<Model.SubModule> subModuleList = new List<Model.SubModule>();

            foreach (Entity.Submodules entity in businessCollection)
            {
                subModuleList.Add(
                    new Model.SubModule
                    {
                        ModuleCode = entity.ModuleCode,
                        SubmoduleCode = entity.SubmoduleCode,
                        Description = entity.Description,
                        Enabled = entity.Enabled,
                        ExpirationDate = entity.ExpirationDate,
                        VirtualFolder = entity.VirtualFolder,
                        ParentModuleCode = entity.ParentModuleCode,
                        ParentSubmoduleCode = entity.ParentSubmoduleCode
                    });
            }

            return subModuleList;
        }
        #endregion

        #region Entity
        /// <summary>
        /// Crea una lista de Model.EventEntity a partir de un businessCollection
        /// </summary>
        /// <param name="businessCollection"> lista de Entity.CoEventEntity</param>
        /// <returns>List de Model.EventEntity</returns>
        public static List<Model.EventEntity> CreateListEventEntity(BusinessCollection businessCollection)
        {
            List<Model.EventEntity> listEntity = new List<Model.EventEntity>();
            foreach (Entity.CoEventEntity entity in businessCollection)
            {
                listEntity.Add(CreateEventEntity(entity));
            }
            return listEntity;
        }

        /// <summary>
        /// crea un Model.EventEntity a partir de un Entity.CoEventEntity
        /// </summary>
        /// <param name="coEventEntity">Entity.CoEventEntity</param>
        /// <returns></returns>
        public static Model.EventEntity CreateEventEntity(Entity.CoEventEntity entity)
        {
            return new Model.EventEntity
            {
                EntityId = entity.EntityId,
                Description = entity.Description,
                QueryType = new Model.EventQueryType { QueryTypeCode = entity.QueryTypeCode },
                SourceTable = entity.SourceTable,
                SourceCode = entity.SourceCode,
                SourceDescription = entity.SourceDescription,
                JoinTable = entity.JoinTable,
                JoinSourceField = entity.JoinSourceField,
                JoinTargetField = entity.JoinTargetField,
                ParamWhere = entity.ParamWhere,
                LevelId = entity.LevelId,
                ValidationType = new Model.EventValidationType { ValidationTypeCode = entity.ValidationTypeCode },
                ValidationProcedure = entity.ValidationProcedure,
                ValidationTable = entity.ValidationTable,
                ValidationKeyField = entity.ValidationKeyField,
                DataKeyType = new Model.EventDataType { DataTypeCode = entity.DataKeyTypeCode },
                Key1Field = entity.Key1Field,
                Key2Field = entity.Key2Field,
                Key3Field = entity.Key3Field,
                Key4Field = entity.Key4Field,
                ValidationField = entity.ValidationField,
                DataFieldType = new Model.EventDataType { DataTypeCode = entity.DataFieldTypeCode },
                WhereAdd = entity.WhereAdd,
                GroupByInd = entity.GroupByInd,
            };
        }

        public static List<Model.EntityDependencies> CreateEntitiesDependencies(BusinessCollection businessCollection)
        {
            List<Model.EntityDependencies> EntityDependencie = new List<Models.EntityDependencies>();

            foreach (Entity.CoEntityDependencies entityDependencies in businessCollection)
            {
                EntityDependencie.Add(CreateEntityDependencies(entityDependencies));
            }
            return EntityDependencie;
        }

        public static Models.EntityDependencies CreateEntityDependencies(Entity.CoEntityDependencies entityDependencies)
        {
            return new Models.EntityDependencies
            {
                ConditionId = entityDependencies.ConditionId,
                EntityId = entityDependencies.EntityId,
                DependsId = entityDependencies.DependsId,
                ColumnName = entityDependencies.ColumnName,
            };
        }
        #endregion

        #region QueryType 
        /// <summary>
        /// Crea una lista de Model.EventQueryType a partir de un businessCollection
        /// </summary>
        /// <param name="businessCollection"> lista de Entity.CoEventQueryType</param>
        /// <returns>List de Model.EventEntity</returns>
        public static List<Model.EventQueryType> CreateEventQueryType(BusinessCollection businessCollection)
        {
            List<Model.EventQueryType> listEventQueryType = new List<Model.EventQueryType>();
            foreach (Entity.CoEventQueryType entity in businessCollection)
            {
                listEventQueryType.Add(
                    new Model.EventQueryType
                    {
                        QueryTypeCode = entity.QueryTypeCode,
                        Description = entity.Description
                    });
            }

            return listEventQueryType;
        }
        #endregion

        #region ValidationType
        /// <summary>
        /// Crea una lista de Model.EventValidationType a partir de un businessCollection
        /// </summary>
        /// <param name="businessCollection"> lista de Entity.CoEventValidationType</param>
        /// <returns>List de Model.EventEntity</returns>
        public static List<Models.EventValidationType> CreateListValidationTypes(BusinessCollection businessCollection)
        {
            List<Models.EventValidationType> list = new List<Models.EventValidationType>();
            foreach (Entity.CoEventValidationType entity in businessCollection)
            {
                list.Add(
                    new Models.EventValidationType
                    {
                        ValidationTypeCode = entity.ValidationTypeCode,
                        Description = entity.Description,
                        ProcedureInd = entity.ProcedureInd,
                    });
            }
            return list;
        }
        #endregion

        #region DataType
        /// <summary>
        /// Crea una lista de Model.EventDataType a partir de un businessCollection
        /// </summary>
        /// <param name="businessCollection"> lista de Entity.coEventDataType</param>
        /// <returns>List de Model.EventEntity</returns>
        public static List<Models.EventDataType> CreateListDataType(BusinessCollection businessCollection)
        {
            List<Models.EventDataType> list = new List<Models.EventDataType>();
            foreach (Entity.CoEventDataType entity in businessCollection)
            {
                list.Add(
                    new Models.EventDataType
                    {
                        DataTypeCode = entity.DataTypeCode,
                        Description = entity.Description,
                        NumericInd = entity.NumericInd,
                        SqlDataType = entity.SqlDataType
                    });
            }
            return list;
        }
        #endregion

        #region Levels
        /// <summary>
        /// crea una lista de Models.EventLevels a partir de un BusinessCollection
        /// </summary>
        /// <param name="businessCollection">lista de Entity.CoEventLevels </param>
        /// <returns></returns>
        public static List<Models.EventLevels> CreateListEventLevels(BusinessCollection businessCollection)
        {
            List<Models.EventLevels> list = new List<Models.EventLevels>();
            foreach (Entity.CoEventLevels entity in businessCollection)
            {
                list.Add(
                    new Models.EventLevels
                    {
                        LevelId = entity.LevelId,
                        Description = entity.LevelId.ToString() + " - " + entity.Description

                    });
            }
            return list;
        }
        #endregion

        #region ConditionGroup
        /// <summary>
        /// genera una lista de Models.EventConditionGroup a partir de un businessCollection
        /// </summary>
        /// <param name="businessCollection">lista de Entity.CoEventConditionGroup</param>
        /// <returns></returns>
        public static List<Models.EventConditionGroup> CreateListConditionGroup(BusinessCollection businessCollection)
        {
            List<Models.EventConditionGroup> listConditionGroup = new List<Models.EventConditionGroup>();
            foreach (Entity.CoEventConditionGroup entity in businessCollection)
            {
                listConditionGroup.Add(CreateConditionGroup(entity));
            }
            return listConditionGroup;
        }

        /// <summary>
        /// crea un  Model.EventConditionGroup  a partir de un Entity.CoEventConditionGroup
        /// </summary>
        /// <param name="entity">Entity.CoEventConditionGroup</param>
        /// <returns></returns>
        public static Model.EventConditionGroup CreateConditionGroup(Entity.CoEventConditionGroup entity)
        {
            return new Models.EventConditionGroup
            {
                ConditionId = entity.ConditionId,
                Description = entity.Description
            };
        }
        #endregion

        #region EventCompany
        /// <summary>
        /// Crea un Model.EventCompany a apartir de una Entity.CoEventCompany
        /// </summary>
        /// <param name="entity">Entity.CoEventCompany </param>
        /// <returns></returns>
        public static Model.EventCompany CreateEventCompany(Entity.CoEventCompany entity)
        {
            return new Models.EventCompany
            {
                EventsGroup = new Model.EventsGroup { GroupEventId = entity.GroupEventId },
                EventId = entity.EventId,
                Description = entity.Description,
                ValidationType = new Model.EventValidationType { ValidationTypeCode = entity.ValidationTypeCode },
                ProcedureName = entity.ProcedureName,
                EventConditionGroup = new Model.EventConditionGroup { ConditionId = entity.ConditionId },
                EnabledStop = entity.EnabledStop,
                EnabledAuthorize = entity.EnabledAuthorize,
                DescriptionErrorMessage = entity.DescriptionErrorMessage,
                Enabled = entity.Enabled,
                TypeCode = entity.TypeCode
            };
        }

        public static Model.EventCompany CreateCoEventCompany(EVENTEN.CoEventCompany entity)
        {
            return new Models.EventCompany
            {
                EventsGroup = new Model.EventsGroup { GroupEventId = entity.GroupEventId },
                EventId = entity.EventId,
                Description = entity.Description,
                ValidationType = new Model.EventValidationType { ValidationTypeCode = entity.ValidationTypeCode },
                ProcedureName = entity.ProcedureName,
                EventConditionGroup = new Model.EventConditionGroup { ConditionId = entity.ConditionId },
                EnabledStop = entity.EnabledStop,
                EnabledAuthorize = entity.EnabledAuthorize,
                DescriptionErrorMessage = entity.DescriptionErrorMessage,
                Enabled = entity.Enabled,
                TypeCode = entity.TypeCode
            };
        }

        public static List<Model.EventCompany> CreateEventsCompanies(BusinessCollection businessCollection)
        {
            List<Model.EventCompany> listEventsCompanies = new List<Model.EventCompany>();

            foreach (EVENTEN.CoEventCompany coEventCompany in businessCollection)
            {
                listEventsCompanies.Add(CreateCoEventCompany(coEventCompany));
            }

            return listEventsCompanies;
        }


        #endregion

        #region ComparationType
        /// <summary>
        /// crea un   Model.EventComparisonType  a apartir de un Entity.CoEventComparisonType
        /// </summary>
        /// <param name="coEventComparisonType">Entity.CoEventComparisonType</param>
        /// <returns></returns>
        public static Model.EventComparisonType CreateComparisonType(Entity.CoEventComparisonType coEventComparisonType)
        {
            return new Model.EventComparisonType
            {
                ComparatorCode = coEventComparisonType.ComparatorCode,
                Description = coEventComparisonType.Description,
                SmallDesc = coEventComparisonType.SmallDesc,
                Symbol = coEventComparisonType.Symbol,
                TextInd = coEventComparisonType.TextInd,
                ComboInd = coEventComparisonType.ComboInd,
                QueryInd = coEventComparisonType.QueryInd,
                NumValues = coEventComparisonType.NumValues
            };
        }
        #endregion

        #region AuthorizatioUser
        /// <summary>
        /// crea un Model.EventAuthorizationUsers a partir de un Entity.CoEventAuthorizationUsers entity
        /// </summary>
        /// <param name="entity">Entity.CoEventAuthorizationUsers entity</param>
        /// <returns></returns>
        public static Model.EventAuthorizationUsers CreateEventAuthorizationUsers(Entity.CoEventAuthorizationUsers entity)
        {
            return new Models.EventAuthorizationUsers
            {
                EventId = entity.EventId,
                DelegationId = entity.DelegationId,
                GroupEventId = entity.GroupEventId,
                UserId = entity.UserId,
            };
        }
        #endregion

        #region NotificationUser
        /// <summary>
        /// crea un Model.EventNotificationUsers a partir de un Entity.CoEventNotificationUsers entity
        /// </summary>
        /// <param name="entity">Entity.CoEventNotificationUsers </param>
        /// <returns></returns>
        public static Model.EventNotificationUsers CreateEventNotificationUsers(Entity.CoEventNotificationUsers entity)
        {
            return new Models.EventNotificationUsers
            {
                EventId = entity.EventId,
                DelegationId = entity.DelegationId,
                GroupEventId = entity.GroupEventId,
                UserId = entity.UserId,
                UserNotifDefault = entity.UserNotifDefault
            };
        }
        #endregion
    }
}
