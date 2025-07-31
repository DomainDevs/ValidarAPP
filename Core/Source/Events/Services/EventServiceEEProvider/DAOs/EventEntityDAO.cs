using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model = Sistran.Core.Application.EventsServices.Models;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventEntityDAO
    {
        /// <summary>
        /// Obtine todas las entidades de eventos
        /// </summary>
        /// <returns>lista de Model.EventEntity</returns>
        public List<Model.EventEntity> GetEventEntities()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.CoEventEntity)));
                return ModelAssembler.CreateListEventEntity(businessCollection).OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventEntity", ex);
            }
        }

        /// <summary>
        /// Obtiene un Model.EventEntity a partir de su id
        /// </summary>
        /// <param name="IdEventEntity">id del Model.EventEntity</param>
        /// <returns></returns>
        public Model.EventEntity GetEventEntityByIdEventEntity(int IdEventEntity)
        {
            try
            {
                PrimaryKey key = CoEventEntity.CreatePrimaryKey(IdEventEntity);
                return ModelAssembler.CreateEventEntity((CoEventEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventEntityByIdEventEntity", ex);
            }
        }

        /// <summary>
        /// Crea una nueva eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a crear</param>
        public void CreateEventEntity(Model.EventEntity eventEntity)
        {
            try
            {
                var index = 1;
                if (GetEventEntities().Count() != 0)
                {
                    index = GetEventEntities().Max(x => x.EntityId) + 1;
                }

                eventEntity.EntityId = index;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateEventEntity(eventEntity));

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateEventEntity", ex);
            }
        }

        /// <summary>
        /// actualiza una eventEntity
        /// </summary>
        /// <param name="eventEntity">eventEntity a actualizar</param>
        public void UpdateEventEntity(Model.EventEntity entity)
        {
            try
            {
                PrimaryKey key = CoEventEntity.CreatePrimaryKey(entity.EntityId);
                var entityEventEntity = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventEntity;

                entityEventEntity.Description = entity.Description;
                entityEventEntity.QueryTypeCode = entity.QueryType.QueryTypeCode;
                entityEventEntity.SourceTable = entity.SourceTable;
                entityEventEntity.SourceCode = entity.SourceCode;
                entityEventEntity.SourceDescription = entity.SourceDescription;
                entityEventEntity.JoinTable = entity.JoinTable;
                entityEventEntity.JoinSourceField = entity.JoinSourceField;
                entityEventEntity.JoinTargetField = entity.JoinTargetField;
                entityEventEntity.ParamWhere = entity.ParamWhere;
                entityEventEntity.LevelId = entity.LevelId;
                entityEventEntity.ValidationTypeCode = entity.ValidationType.ValidationTypeCode;
                entityEventEntity.ValidationProcedure = entity.ValidationProcedure;
                entityEventEntity.ValidationTable = entity.ValidationTable;
                entityEventEntity.ValidationKeyField = entity.ValidationKeyField;
                entityEventEntity.DataKeyTypeCode = entity.DataKeyType.DataTypeCode;
                entityEventEntity.Key1Field = entity.Key1Field;
                entityEventEntity.Key2Field = entity.Key2Field;
                entityEventEntity.Key3Field = entity.Key3Field;
                entityEventEntity.Key4Field = entity.Key4Field;
                entityEventEntity.ValidationField = entity.ValidationField;
                entityEventEntity.DataFieldTypeCode = entity.DataFieldType.DataTypeCode;
                entityEventEntity.WhereAdd = entity.WhereAdd;
                entityEventEntity.GroupByInd = entity.GroupByInd;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityEventEntity);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateEventEntity", ex);
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
                PrimaryKey key = CoEventEntity.CreatePrimaryKey(eventEntity);
                var entityEventEntity = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventEntity;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityEventEntity);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventEntity ", ex);
            }
        }

        /// <summary>
        /// obtiene el las entidades asociadas al grupo de condiciones
        /// </summary>
        /// <param name="IdConditionsGroup">id del gupo de condiciones</param>
        /// <returns></returns>
        public List<Model.EventEntity> GetEntitiesByIdConditionsGroup(int idConditionsGroup)
        {
            try
            {
                List<Model.EventEntity> list = new List<Model.EventEntity>();

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.EntityId, "EVENTEN")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.Description, "EVENTEN")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.QueryTypeCode, "EVENTEN")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventEntity), "EVENTEN"), new ClassNameTable(typeof(EVENTEN.CoEventConditionEntity), "cond"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventEntity.Properties.EntityId, "EVENTEN").Equal().Property(EVENTEN.CoEventConditionEntity.Properties.EntityId, "cond").GetPredicate();

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventConditionEntity.Properties.ConditionId, "cond").Equal().Constant(idConditionsGroup);

                select.Table = join;
                select.Where = where.GetPredicate();
                select.AddSortValue(new SortValue(new Column(EVENTEN.CoEventEntity.Properties.Description, "EVENTEN"), SortOrderType.Ascending));

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EventEntity
                        {
                            EntityId = (int)reader["EntityId"],
                            Description = (string)reader["Description"],
                            QueryType = new Model.EventQueryType { QueryTypeCode = (int)reader["QueryTypeCode"] },
                            ValidationType = new Model.EventValidationType { },
                            DataKeyType = new Model.EventDataType { },
                            DataFieldType = new Model.EventDataType { }
                        });
                    }
                }
                return list;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEntityByIdConditionsGroup ", ex);
            }
        }

        /// <summary>
        /// Trae la informacion de todas las entidades pertenecientes a un evento  
        /// </summary>
        /// <param name="idGroup">id grupo de eventos</param>
        /// <param name="idEvent">id del evento</param>
        /// <returns></returns>
        /// funcionalidad del SP EVE.CO_GET_EVENT_ENTITYS_EXT
        public List<Models.EventEntity> GetEventEntityExt(int idGroup, int idEvent)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventConditionEntity.Properties.ConditionId, "ECE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventConditionEntity.Properties.EntityId, "ECE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.Description, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.QueryTypeCode, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.SourceTable, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.SourceCode, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.SourceDescription, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.JoinTable, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.JoinSourceField, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.JoinTargetField, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.ParamWhere, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.ValidationTypeCode, "EE")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventEntity.Properties.DataFieldTypeCode, "EE")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventConditionEntity), "ECE"), new ClassNameTable(typeof(EVENTEN.CoEventEntity), "EE"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventConditionEntity.Properties.EntityId, "ECE").Equal().Property(EVENTEN.CoEventEntity.Properties.EntityId, "EE").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventConditionEntity.Properties.ConditionId, "ECE").Equal().Property(EVENTEN.CoEventCompany.Properties.ConditionId, "EC").GetPredicate());

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Constant(idEvent)
                    .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Constant(idGroup);

                select.Table = join;
                select.Where = where.GetPredicate();

                EntityDependenciesDAO entityDependenciesDAO = new EntityDependenciesDAO();
                List<Models.EventEntity> listEntity = new List<Models.EventEntity>();


                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        listEntity.Add(new Models.EventEntity
                        {
                            ConditionId = (int)reader["ConditionId"],
                            EntityId = (int)reader["EntityId"],
                            NumDependences = 0,
                            Description = (string)reader["Description"],
                            QueryType = new Model.EventQueryType { QueryTypeCode = (int)reader["QueryTypeCode"] },
                            DataFieldType = new Model.EventDataType { DataTypeCode = (int)reader["DataFieldTypeCode"] },
                            SourceTable = (string)reader["SourceTable"],
                            SourceCode = (string)reader["SourceCode"],
                            SourceDescription = (string)reader["SourceDescription"],
                            JoinTable = (string)reader["JoinTable"],
                            JoinSourceField = (string)reader["JoinSourceField"],
                            DataKeyType = new Model.EventDataType { },
                            JoinTargetField = (string)reader["JoinTargetField"],
                            ParamWhere = (string)reader["ParamWhere"],
                            ValidationType = new Model.EventValidationType { ValidationTypeCode = (int)reader["ValidationTypeCode"] }
                        });
                    }
                }

                foreach (var item in listEntity)
                {
                    item.NumDependences = entityDependenciesDAO.GetDependencesByIdGroupIdEventIdEntity(idGroup, idEvent, item.EntityId, 0).Count();
                }

                return listEntity.OrderBy(x => x.Description).ToList();

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventEntityExt", ex);
            }
        }

        public List<Model.EventEntity> GetEventEntitiesByDescription(string description)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(EVENTEN.CoEventEntity.Properties.Description, typeof(EVENTEN.CoEventEntity).Name);
                filter.Like();
                filter.Constant(description + "%");

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(EVENTEN.CoEventEntity), filter.GetPredicate());

                return ModelAssembler.CreateListEventEntity(businessCollection).OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventEntity", ex);
            }
        }
    }
}
