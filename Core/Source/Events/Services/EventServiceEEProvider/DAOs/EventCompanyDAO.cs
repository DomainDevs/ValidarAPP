using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.EventsServices.EEProvider.Views;
using EVENTEN = Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventCompanyDAO
    {
        /// <summary>
        /// obtiene EventCore que cumplan con las condiciones
        /// </summary>
        /// <param name="IdEventGroup"> id del grupo de eventos</param>
        /// <param name="State">estado del evento
        ///     <value val=-1>todos los eventos</value>
        ///     <value val=0>inactivos</value>
        ///     <value val=1>activos</value>
        /// </param>
        /// <param name="IdPrefix"></param>
        /// <returns></returns>
        public List<Models.EventCompany> GetEventsByIdEventGroupStateIdPrefix(int IdEventGroup, int State, int IdPrefix)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(EVENTEN.CoEventCompany.Properties.GroupEventId, typeof(EVENTEN.CoEventCompany).Name, IdEventGroup);

                if (State > -1)
                {
                    filter.And();
                    filter.PropertyEquals(EVENTEN.CoEventCompany.Properties.Enabled, typeof(EVENTEN.CoEventCompany).Name, State);
                }
                if (IdPrefix > -1)
                {
                    filter.And();
                    filter.PropertyEquals(EVENTEN.CoEventGroupPrefix.Properties.PrefixCode, typeof(EVENTEN.CoEventGroupPrefix).Name, IdPrefix);
                }

                EventsView eventsView = new EventsView();
                ViewBuilder view = new ViewBuilder("EventsView");
                view.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(view, eventsView);
                List<Models.EventCompany> listEventsCompanies = new List<Models.EventCompany>();

                if (eventsView.CoEventCompanies.Count > 0)
                {
                    List<EVENTEN.CoEventGroup> entityCoEventsGroup = eventsView.CoEventsGroup.Cast<EVENTEN.CoEventGroup>().ToList();
                    List<EVENTEN.CoEventConditionGroup> entityCoEventConditionGroup = eventsView.CoEventsConditionGroup.Cast<EVENTEN.CoEventConditionGroup>().ToList();

                    listEventsCompanies = ModelAssembler.CreateEventsCompanies(eventsView.CoEventCompanies);

                    foreach (Models.EventCompany eventCompany in listEventsCompanies)
                    {
                        eventCompany.EventConditionGroup.Description = entityCoEventConditionGroup.Where(X => X.ConditionId == eventCompany.EventConditionGroup.ConditionId).FirstOrDefault().Description;
                        eventCompany.EventsGroup.Description = entityCoEventsGroup.Where(X => X.GroupEventId == eventCompany.EventsGroup.GroupEventId).FirstOrDefault().Description;
                    }
                }
                    /*
                    SelectQuery select = new SelectQuery();
                    select.AddSelectValue(new SelectValue(new Column(Entity.CoEventCompany.Properties.EventId, "e")));
                    select.AddSelectValue(new SelectValue(new Column(Entity.CoEventCompany.Properties.GroupEventId, "e")));
                    select.AddSelectValue(new SelectValue(new Column(Entity.CoEventCompany.Properties.Description, "e")));
                    select.AddSelectValue(new SelectValue(new Column(Entity.CoEventCompany.Properties.Enabled, "e")));

                    ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                    where.Property(Entity.CoEventCompany.Properties.GroupEventId, "e").Equal().Constant(IdEventGroup);

                    if (State > -1)
                    {
                        where.And().Property(Entity.CoEventCompany.Properties.Enabled, "e").Equal().Constant(State);
                    }

                    if (IdPrefix > -1)
                    {
                        where.And().Property(Entity.CoEventGroupPrefix.Properties.PrefixCode, "p").Equal().Constant(IdPrefix);

                        Join join = new Join(new ClassNameTable(typeof(Entity.CoEventCompany), "e"), new ClassNameTable(typeof(Entity.CoEventGroupPrefix), "p"), JoinType.Inner);
                        join.Criteria = (new ObjectCriteriaBuilder().Property(Entity.CoEventCompany.Properties.GroupEventId, "e").Equal().Property(Entity.CoEventGroupPrefix.Properties.GroupEventId, "p")
                            .And().Property(Entity.CoEventCompany.Properties.EventId, "e").Equal().Property(Entity.CoEventGroupPrefix.Properties.EventId, "p").GetPredicate());

                        select.Table = join;
                    }
                    else
                    {
                        select.Table = new ClassNameTable(typeof(Entity.CoEventCompany), "e");
                    }
                    
                    select.Where = where.GetPredicate();

                    List<Models.EventCompany> listEventCore = new List<Models.EventCompany>();
                    using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                    {
                        while (reader.Read())
                        {
                            listEventCore.Add(
                                new Models.EventCompany
                                {
                                    EventId = (int)reader["EventId"],
                                    EventsGroup = new Models.EventsGroup { GroupEventId = (int)reader["GroupEventId"] },
                                    EventConditionGroup = new Models.EventConditionGroup { },
                                    ValidationType = new Models.EventValidationType { },
                                    Description = (string)reader["Description"],
                                    Enabled = (Boolean)reader["Enabled"],
                                });
                        }
                    }
                    */
                    return listEventsCompanies.OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventsByIdEventGroupStateIdPrefix ", ex);
            }
        }

        /// <summary>
        /// obtiene un evento 
        /// </summary>
        /// <param name="IdEventGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <returns>Models.EventCore</returns>
        public Models.EventCompany GetEventByIdEventGroupIdEvent(int IdEventGroup, int IdEvent)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventCompany.CreatePrimaryKey(IdEventGroup, IdEvent);
                return ModelAssembler.CreateEventCompany(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventCompany);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventByIdEventGroupIdEvent", ex);
            }
        }

        /// <summary>
        /// Crea el evento
        /// </summary>
        /// <param name="eventCompany">evento a crear</param>
        public Models.EventCompany CreateEventCompany(Models.EventCompany eventCompany)
        {
            try
            {
                var index = 1;
                if (GetEventsByIdEventGroupStateIdPrefix(eventCompany.EventsGroup.GroupEventId, -1, -1).Count() != 0)
                {
                    index = GetEventsByIdEventGroupStateIdPrefix(eventCompany.EventsGroup.GroupEventId, -1, -1).Max(x => x.EventId) + 1;
                }

                eventCompany.EventId = index;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EntityAssembler.CreateEventComapany(eventCompany));

                return eventCompany;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateEventCompany ", ex);
            }
        }

        /// <summary>
        /// actualiza el evento
        /// </summary>
        /// <param name="eventCompany">evento a actualizar</param>
        public Models.EventCompany UpdateEventCompany(Models.EventCompany eventCompany)
        {
            try
            {
                PrimaryKey key = CoEventCompany.CreatePrimaryKey(eventCompany.EventsGroup.GroupEventId, eventCompany.EventId);
                var Event = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventCompany;

                Event.ConditionId = eventCompany.EventConditionGroup.ConditionId;
                Event.Description = eventCompany.Description;
                Event.DescriptionErrorMessage = eventCompany.DescriptionErrorMessage;
                Event.Enabled = eventCompany.Enabled;
                Event.EnabledAuthorize = eventCompany.EnabledAuthorize;
                Event.EnabledStop = eventCompany.EnabledStop;
                Event.ProcedureName = eventCompany.ProcedureName;
                Event.TypeCode = eventCompany.TypeCode;
                Event.ValidationTypeCode = eventCompany.ValidationType.ValidationTypeCode;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(Event);

                return eventCompany;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateEventCompany", ex);
            }
        }

        /// <summary>
        /// Elimina el evento
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        public void DeleteEventCompany(int IdGroup, int IdEvent)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventCompany.CreatePrimaryKey(IdGroup, IdEvent);
                var entity = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventCompany;
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventCompany", ex);
            }
        }
    }
}
