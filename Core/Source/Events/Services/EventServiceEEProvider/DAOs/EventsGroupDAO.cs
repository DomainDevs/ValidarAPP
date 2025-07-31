using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = Sistran.Core.Application.EventsServices.Models;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventsGroupDAO
    {
        /// <summary>
        /// obtiene la lista de Model.EventsGroup
        /// </summary>
        /// <returns></returns>
        public List<Model.EventsGroup> GetEventsGroup()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.CoEventGroup)));
                return ModelAssembler.CreateListEventsGroup(businessCollection).OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventsGroup", ex);
            }
        }

        /// <summary>
        /// EventsGroup por IdEventGroup
        /// </summary>
        /// <param name="IdEventGroup">id del grupo de eventos</param>
        /// <returns></returns>
        public Model.EventsGroup GetEventsGroupByIdEventGroup(int IdEventGroup)
        {
            try
            {
                PrimaryKey key = CoEventGroup.CreatePrimaryKey(IdEventGroup);
                return ModelAssembler.CreateEventsGroup(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventGroup);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventsGroupByIdEventGroup", ex);
            }
        }

        /// <summary>
        /// Crear un nuevo grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        public void CreateEventGroups(Model.EventsGroup eventsGroup)
        {
            try
            {
                var index = 1;
                if (GetEventsGroup().Count() != 0)
                {
                    index = GetEventsGroup().Max(x => x.GroupEventId) + 1;
                }

                eventsGroup.GroupEventId = index;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateEventsGroup(eventsGroup));

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateEventGroups", ex);
            }
        }

        /// <summary>
        /// actualizar un grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        public void UpdateEventGroups(Model.EventsGroup eventsGroup)
        {
            try
            {
                PrimaryKey key = CoEventGroup.CreatePrimaryKey(eventsGroup.GroupEventId);
                var entityEventsGroup = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventGroup;

                entityEventsGroup.Description = eventsGroup.Description;
                entityEventsGroup.ModuleCode = eventsGroup.ModuleCode;
                entityEventsGroup.SubmoduleCode = eventsGroup.SubmoduleCode;
                entityEventsGroup.EnabledInd = eventsGroup.EnabledInd;
                entityEventsGroup.AuthorizationReport = eventsGroup.AuthorizationReport;
                entityEventsGroup.ProcedureAuthorized = eventsGroup.ProcedureAuthorized;
                entityEventsGroup.ProcedureReject = eventsGroup.ProcedureReject;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityEventsGroup);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateEventGroups", ex);
            }
        }

        /// <summary>
        /// Elimina un grupo de eventos
        /// </summary>
        /// <param name="eventsGroup">Grupo de eventos</param>
        public void DeleteEventGroups(Model.EventsGroup eventsGroup)
        {
            try
            {
                PrimaryKey key = CoEventGroup.CreatePrimaryKey(eventsGroup.GroupEventId);
                var entityEventsGroup = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventGroup;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityEventsGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventGroups", ex);
            }
        }
    }
}
