using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventConditionGroupDAO
    {
        /// <summary>
        /// obtiene los grupos de condiciones
        /// </summary>
        /// <returns></returns>
        public List<Models.EventConditionGroup> GetConditionsGroups()
        {
            try
            {
                EventEntityDAO eventEntityDAO = new EventEntityDAO();

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.CoEventConditionGroup)));

                List<Models.EventConditionGroup> eventsConditionGroup = ModelAssembler.CreateListConditionGroup(businessCollection).OrderBy(x => x.Description).ToList();

                foreach (Models.EventConditionGroup eventConditionGroup in eventsConditionGroup)
                {
                    eventConditionGroup.EventEntities = eventEntityDAO.GetEntitiesByIdConditionsGroup(eventConditionGroup.ConditionId);
                }

                return eventsConditionGroup;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetConditionsGroups", ex);
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
                PrimaryKey key = EVENTEN.CoEventConditionGroup.CreatePrimaryKey(IdConditionsGroup);
                return ModelAssembler.CreateConditionGroup(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventConditionGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetConditionsGroupByIdConditionsGroup", ex);
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
                var index = 1;
                if (GetConditionsGroups().Count() != 0)
                {
                    index = GetConditionsGroups().Max(x => x.ConditionId) + 1;
                }

                ConditionsGroup.ConditionId = index;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateConditionsGroupEntity(ConditionsGroup));

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateConditionsGroup", ex);
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
                PrimaryKey key = EVENTEN.CoEventConditionGroup.CreatePrimaryKey(ConditionsGroup.ConditionId);
                var eventGroup = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventConditionGroup;

                eventGroup.Description = ConditionsGroup.Description;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(eventGroup);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateConditionsGroup", ex);
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
                PrimaryKey key = EVENTEN.CoEventConditionGroup.CreatePrimaryKey(IdConditionsGroup);
                var eventGroup = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventConditionGroup;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(eventGroup);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == "FOREIGN_KEY")
                {
                    throw new BusinessException("FOREIGN_KEY:DeleteConditionsGroup", ex.InnerException);
                }
                else
                {
                    throw new BusinessException("Excepcion en: DeleteConditionsGroup", ex);
                }

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
                EventEntityDAO eventEntityDAO = new EventEntityDAO();
                var listEntidades = eventEntityDAO.GetEntitiesByIdConditionsGroup(IdCondition);

                if (IdEntities == null)
                {
                    IdEntities = new List<int>();
                }

                foreach (var item in listEntidades)
                {
                    if (!IdEntities.Contains(item.EntityId))
                    {


                        PrimaryKey key = EVENTEN.CoEventConditionEntity.CreatePrimaryKey(IdCondition, item.EntityId);
                        var eventGroup = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventConditionEntity;
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(eventGroup);
                    }
                    else
                    {
                        IdEntities.Remove(item.EntityId);
                    }
                }

                foreach (var item in IdEntities)
                {
                    var eventConditionEntity = new EVENTEN.CoEventConditionEntity(IdCondition, item);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(eventConditionEntity);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: AssignEntityByIdConditionIdEntity", ex);
            }
        }
    }
}
