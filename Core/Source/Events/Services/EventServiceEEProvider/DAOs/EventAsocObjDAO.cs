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
    public class EventAsocObjDAO
    {
        /// <summary>
        /// obtiene los id de los aacesos segun el id evento y id grupo
        /// </summary>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <returns></returns>
        public List<Models.EventAsocObj> GetAccessesByIdEventIdGroup(int IdEvent, int IdGroup)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAsocObj.Properties.AccessId, "a")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAsocObj.Properties.GroupEventId, "a")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAsocObj.Properties.EventId, "a")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAsocObj.Properties.GroupEventId, "a").Equal().Constant(IdGroup)
                    .And().Property(EVENTEN.CoEventAsocObj.Properties.EventId, "a").Equal().Constant(IdEvent);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventAsocObj), "a");
                select.Where = where.GetPredicate();

                List<Models.EventAsocObj> lista = new List<Models.EventAsocObj>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        lista.Add(new Models.EventAsocObj
                        {
                            AccessId = (int)reader["AccessId"],
                            GroupEventId = (int)reader["GroupEventId"],
                            EventId = (int)reader["EventId"]
                        });
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetAccessesByIdEventIdGroup", ex);
            }
        }

        /// <summary>
        /// crea los accesos (id usuarios) para los eventos
        /// </summary>
        /// <param name="eventCompany">evento</param>
        /// <param name="listAccesses">Lista de accesos</param>
        public void CreateAsocObjToEventCompany(EventCompany eventCompany, List<int> listAccesses)
        {
            try
            {
                var allAsocObj = GetAccessesByIdEventIdGroup(eventCompany.EventId, eventCompany.EventsGroup.GroupEventId);

                foreach (var item in allAsocObj)
                {
                    if (listAccesses.Contains(item.AccessId))
                    {
                        listAccesses.Remove(item.AccessId);
                    }
                    else
                    {
                        DeleteAsocObjToEventCompany(item);
                    }
                }

                foreach (int item in listAccesses)
                {
                    var AsocObj = new EVENTEN.CoEventAsocObj(eventCompany.EventsGroup.GroupEventId, eventCompany.EventId, item);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(AsocObj);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateAsocObjToEventCompany", ex);
            }
        }

        /// <summary>
        /// elimina EventAsocObj
        /// </summary>
        /// <param name="AsocObj">EventAsocObj a eliminar</param>
        public void DeleteAsocObjToEventCompany(EventAsocObj AsocObj)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventAsocObj.CreatePrimaryKey(AsocObj.GroupEventId, AsocObj.EventId, AsocObj.AccessId);
                var EventGroupPrefix = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as CoEventAsocObj;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(EventGroupPrefix);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteAsocObjToEventCompany", ex);
            }
        }

        /// <summary>
        /// elimina un EventAsocObj
        /// </summary>
        /// <param name="idEvent">ID del evento</param>
        /// <param name="idGroup">id del grupo de eventos</param>
        public void DeleteAsocObjToEventCompany(int idEvent, int idGroup)
        {
            try
            {
                var list = GetAccessesByIdEventIdGroup(idEvent, idGroup);

                foreach (var item in list)
                {
                    PrimaryKey key = EVENTEN.CoEventAsocObj.CreatePrimaryKey(item.GroupEventId,item.EventId,item.AccessId);
                    var Entity = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventAsocObj;
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(Entity);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteAsocObjToEventCompany" , ex);
            }
        }
    }
}
