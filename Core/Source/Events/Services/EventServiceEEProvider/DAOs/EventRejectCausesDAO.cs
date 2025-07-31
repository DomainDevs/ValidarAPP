using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.EventsServices.Models;
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
    public class EventRejectCausesDAO
    {
        /// <summary>
        /// obtiene los id de los motivos de rechazo segun el id evento y id grupo
        /// </summary>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <returns></returns>
        public List<Models.EventRejectCauses> GetRejectCausesByIdEventIdGroup(int IdEvent, int IdGroup)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventRejectCauses.Properties.RejectId, "r")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventRejectCauses.Properties.GroupEventId, "r")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventRejectCauses.Properties.EventId, "r")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventRejectCauses.Properties.Description, "r")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventRejectCauses.Properties.GroupEventId, "r").Equal().Constant(IdGroup)
                    .And().Property(EVENTEN.CoEventRejectCauses.Properties.EventId, "r").Equal().Constant(IdEvent);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventRejectCauses), "r");
                select.Where = where.GetPredicate();

                List<Models.EventRejectCauses> lista = new List<Models.EventRejectCauses>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        lista.Add(new Models.EventRejectCauses
                        {
                            GroupEventId = (int)reader["GroupEventId"],
                            EventId = (int)reader["EventId"],
                            RejectId = (int)reader["RejectId"],
                            Description = (string)reader["Description"]
                        });
                    }
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetRejectCausesByIdEventIdGroup", ex);
            }
        }

        /// <summary>
        /// crea RejectCauses al eventCompany
        /// </summary>
        /// <param name="eventCompany">EventCompany</param>
        /// <param name="listRejectCauses">lista de RejectCauses </param>
        public void CreateRejectCausesToEventCompany(EventCompany eventCompany, List<Objects> listRejectCauses)
        {
            try
            {
                var allRejectCauses = GetRejectCausesByIdEventIdGroup(eventCompany.EventId, eventCompany.EventsGroup.GroupEventId);

                foreach (var item in allRejectCauses)
                {
                    if (listRejectCauses.Where(x => x.Description == item.Description && x.Id == item.RejectId).Count() > 0)
                    {
                        listRejectCauses.Remove(listRejectCauses.Where(x => x.Description == item.Description && x.Id == item.RejectId).First());
                    }
                    else if (listRejectCauses.Where(x => x.Id == item.RejectId).Count() > 0)
                    {
                        UpdateRejectCausesToEventCompany(item, listRejectCauses.Where(x => x.Id == item.RejectId).First().Description);
                        listRejectCauses.Remove(listRejectCauses.Where(x => x.Id == item.RejectId).First());
                    }
                }

                foreach (Objects item in listRejectCauses)
                {
                    var index = 1;
                    if (GetRejectCausesByIdEventIdGroup(eventCompany.EventId, eventCompany.EventsGroup.GroupEventId).Count() != 0)
                    {
                        index = GetRejectCausesByIdEventIdGroup(eventCompany.EventId, eventCompany.EventsGroup.GroupEventId).Max(x => x.RejectId) + 1;
                    }

                    var RejectCause = new EVENTEN.CoEventRejectCauses(eventCompany.EventsGroup.GroupEventId, eventCompany.EventId, item.Id == 0 ? index : Convert.ToInt32(item.Id))
                    {
                        Description = item.Description
                    };
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(RejectCause);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateRejectCausesToEventCompany", ex);
            }
        }

        /// <summary>
        /// actualiza el EventRejectCauses
        /// </summary>
        /// <param name="RejectCauses">EventRejectCauses a actualizar</param>
        /// <param name="description">descripcion nueva</param>
        public void UpdateRejectCausesToEventCompany(EventRejectCauses RejectCauses, string description)
        {
            try
            {
                PrimaryKey key = CoEventRejectCauses.CreatePrimaryKey(RejectCauses.GroupEventId, RejectCauses.EventId, RejectCauses.RejectId);
                var reject = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventRejectCauses;

                reject.Description = description;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(reject);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateRejectCausesToEventCompany", ex);
            }
        }

        /// <summary>
        /// elimina el EventRejectCauses
        /// </summary>
        /// <param name="RejectCauses">EventRejectCauses a actualizar</param>
        /// <param name="description">descripcion nueva</param>
        public void DeleteRejectCausesToEventCompany(int IdEvent, int IdGroup)
        {
            try
            {
                var list = GetRejectCausesByIdEventIdGroup(IdEvent, IdGroup);

                foreach (var item in list)
                {
                    PrimaryKey key = CoEventRejectCauses.CreatePrimaryKey(item.GroupEventId, item.EventId, item.RejectId);
                    var reject = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventRejectCauses;

                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(reject);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteRejectCausesToEventCompany", ex);
            }
        }
    }
}
