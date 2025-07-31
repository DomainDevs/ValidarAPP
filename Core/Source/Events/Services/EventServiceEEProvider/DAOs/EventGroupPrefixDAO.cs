using Sistran.Core.Application.Events.Entities;
using Sistran.Core.Application.EventsServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventGroupPrefixDAO
    {
        /// <summary>
        /// obtine los Id de Ramos segun los parametros
        /// </summary>
        /// <param name="IdGroup">id del grupo de condiciones</param>
        /// <param name="IdEvent">id del evento</param>
        /// <returns></returns>
        public List<Models.EventGroupPrefix> GetPrefixesByIdGroupIdEvent(int IdGroup, int IdEvent)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventGroupPrefix.Properties.GroupEventId, "p")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventGroupPrefix.Properties.EventId, "p")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventGroupPrefix.Properties.PrefixCode, "p")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventGroupPrefix.Properties.GroupEventId, "p").Equal().Constant(IdGroup).And()
                    .Property(EVENTEN.CoEventGroupPrefix.Properties.EventId, "p").Equal().Constant(IdEvent);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventGroupPrefix), "p");
                select.Where = where.GetPredicate();

                List<Models.EventGroupPrefix> prefixes = new List<Models.EventGroupPrefix>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        prefixes.Add(
                            new Models.EventGroupPrefix
                            {
                                GroupEventId = (int)reader["GroupEventId"],
                                EventId = (int)reader["EventId"],
                                PrefixCode = (int)reader["PrefixCode"]
                            });
                    }
                }
                return prefixes;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetPrefixesByIdGroupIdEvent", ex);
            }
        }

        /// <summary>
        /// crea la relacion entre prefix y eventCompany
        /// </summary>
        /// <param name="eventCompany">eventCompany</param>
        /// <param name="listPrefixes">lista de prefixes</param>
        public void CreateGroupPrefixesToEventCompany(EventCompany eventCompany, List<int> listPrefixes)
        {
            try
            {
                var allPrefixes = GetPrefixesByIdGroupIdEvent(eventCompany.EventsGroup.GroupEventId, eventCompany.EventId);

                foreach (var item in allPrefixes)
                {
                    if (listPrefixes.Contains(item.PrefixCode))
                    {
                        listPrefixes.Remove(item.PrefixCode);
                    }
                    else
                    {
                        DeleteGroupPrefixesToEventCompany(item);
                    }
                }

                foreach (int item in listPrefixes)
                {
                    var GroupPrefix = new EVENTEN.CoEventGroupPrefix(eventCompany.EventsGroup.GroupEventId, eventCompany.EventId, item);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(GroupPrefix);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateGroupPrefixesToEventCompany", ex);
            }
        }

        /// <summary>
        /// elimina EventGroupPrefix 
        /// </summary>
        /// <param name="GroupPrefix">EventGroupPrefix a eliminar</param>
        public void DeleteGroupPrefixesToEventCompany(EventGroupPrefix GroupPrefix)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventGroupPrefix.CreatePrimaryKey(GroupPrefix.GroupEventId, GroupPrefix.EventId, GroupPrefix.PrefixCode);
                var EventGroupPrefix = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventGroupPrefix;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(EventGroupPrefix);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteGroupPrefixesToEventCompany", ex);
            }
        }

        /// <summary>
        /// elimina EventGroupPrefix 
        /// </summary>
        /// <param name="idEvent">id del evento</param>
        /// <param name="idGroup">id del grupo de eventos</param>
        public void DeleteGroupPrefixesToEventCompany(int idEvent, int idGroup)
        {
            try
            {
                var list = GetPrefixesByIdGroupIdEvent(idGroup, idEvent);

                foreach (var item in list)
                {
                    PrimaryKey key = EVENTEN.CoEventGroupPrefix.CreatePrimaryKey(item.GroupEventId, item.EventId, item.PrefixCode);
                    EVENTEN.CoEventGroupPrefix entityCoEventGroupPrefix = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventGroupPrefix;
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityCoEventGroupPrefix);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteGroupPrefixesToEventCompany", ex);
            }
        }
    }
}
