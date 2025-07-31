using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EntityDependenciesDAO
    {
        /// <summary>
        /// actualiza una dependencia que cumpla con las caracteristicas
        /// </summary>
        public void UpdateDependence(Models.EntityDependencies Dependence)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEntityDependencies.CreatePrimaryKey(Dependence.ConditionId, Dependence.EntityId, Dependence.DependsId);
                var DependenceUpdate = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEntityDependencies;

                DependenceUpdate.ColumnName = Dependence.ColumnName;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(DependenceUpdate);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateDependence" , ex);
            }
        }

        /// <summary>
        /// emimina una dependencia que cumpla con las caracteristicas
        /// </summary>
        public void DeleteDependence(Models.EntityDependencies Dependence)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEntityDependencies.CreatePrimaryKey(Dependence.ConditionId, Dependence.EntityId, Dependence.DependsId);
                var DependenceDelete = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEntityDependencies;
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(DependenceDelete);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteDependence" , ex);
            }
        }

        /// <summary>
        /// Crea una dependencia 
        /// </summary>
        public void CreateDependence(Models.EntityDependencies Dependence)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateDependenceEntity(Dependence));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateDependence", ex);
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
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                List<EVENTEN.CoEventEntity> coEventEntity = DataFacadeManager.GetObjects(typeof(EVENTEN.CoEventEntity)).Cast<EVENTEN.CoEventEntity>().ToList();

                filter.PropertyEquals(EVENTEN.CoEntityDependencies.Properties.ConditionId, typeof(EVENTEN.CoEntityDependencies).Name, IdCondition);
                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(EVENTEN.CoEntityDependencies), filter.GetPredicate());

                List<Models.EntityDependencies> EntityDependencies = ModelAssembler.CreateEntitiesDependencies(businessCollection);

                foreach (Models.EntityDependencies entityDependencies in EntityDependencies)
                {
                    entityDependencies.EntityDescription = coEventEntity.Where(x => x.EntityId == entityDependencies.EntityId).FirstOrDefault().Description;
                    entityDependencies.DependsDescription = coEventEntity.Where(x => x.EntityId == entityDependencies.DependsId).FirstOrDefault().Description;
                }
                /*
                List<Models.EntityDependencies> list = new List<Models.EntityDependencies>();

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.ConditionId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.EntityId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.DependsId, "c")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.ColumnName, "c")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEntityDependencies.Properties.ConditionId, "c").Equal().Constant(IdCondition);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEntityDependencies), "c");
                select.Where = where.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EntityDependencies
                        {
                            ConditionId = (int)reader["ConditionId"],
                            DependsId = (int)reader["DependsId"],
                            EntityId = (int)reader["EntityId"],
                            ColumnName = (string)reader["ColumnName"],
                        });
                    }
                }
                return list;*/
                return EntityDependencies;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetDependencesByIdCondition", ex);
            }
        }

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

                List<Models.EntityDependencies> list = new List<Models.EntityDependencies>();

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.ConditionId, "ED")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.EntityId, "ED")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.DependsId, "ED")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEntityDependencies.Properties.ColumnName, "ED")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoEventConditionEntity), "ECE"), new ClassNameTable(typeof(EVENTEN.CoEntityDependencies), "ED"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventConditionEntity.Properties.EntityId, "ECE").Equal().Property(EVENTEN.CoEntityDependencies.Properties.EntityId, "ED")
                    .And().Property(EVENTEN.CoEventConditionEntity.Properties.ConditionId, "ECE").Equal().Property(EVENTEN.CoEntityDependencies.Properties.ConditionId, "ED").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoEventCompany.Properties.ConditionId, "EC").Equal().Property(EVENTEN.CoEntityDependencies.Properties.ConditionId, "ED").GetPredicate());

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Constant(IdEvent)
                    .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Constant(IdGroup);

                if (conditional == -1)
                {
                    where.And().Property(EVENTEN.CoEntityDependencies.Properties.DependsId, "ED").Equal().Constant(IdEntity);
                }
                else
                {
                    where.And().Property(EVENTEN.CoEntityDependencies.Properties.EntityId, "ED").Equal().Constant(IdEntity);
                }

                select.Table = join;
                select.Where = where.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new Models.EntityDependencies
                        {
                            ConditionId = (int)reader["ConditionId"],
                            DependsId = (int)reader["DependsId"],
                            EntityId = (int)reader["EntityId"],
                            ColumnName = (string)reader["ColumnName"],
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetDependencesByIdGroupIdEventIdEntity", ex);
            }
        }
    }
}
