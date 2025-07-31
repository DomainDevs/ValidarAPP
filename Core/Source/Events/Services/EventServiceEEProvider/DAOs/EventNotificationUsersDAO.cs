using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
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
    public class EventNotificationUsersDAO
    {

        /// <summary>
        /// obtiene el usuario notificador para el evento
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdDelegation">id de la delegacion</param>
        /// <param name="IdUser">id del usuario</param>
        /// <returns></returns>
        public Models.EventNotificationUsers GetEventNotificationUsersByIdGroupIdEventIdDelegationIdUser(int IdGroup, int IdEvent, int IdDelegation, int IdUser)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventNotificationUsers.CreatePrimaryKey(IdGroup, IdEvent, IdDelegation, IdUser);
                EVENTEN.CoEventNotificationUsers entityCoEventNotificationUsers = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventNotificationUsers;

                if (entityCoEventNotificationUsers == null)
                {
                    return null;
                }
                else
                {
                    return ModelAssembler.CreateEventNotificationUsers(entityCoEventNotificationUsers);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventNotificationUsersByIdGroupIdEventIdDelegationIdUser", ex);
            }
        }

        /// <summary>
        /// Crea un usuario notificador
        /// </summary>
        /// <param name="notificationUsers">usuario notificador</param>
        public void CreateEventNotificationUser(Models.EventNotificationUsers notificationUsers)
        {
            try
            {
                var EVENTEN = EntityAssembler.CreateEventNotificationUsers(notificationUsers);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EVENTEN);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateEventNotificationUser", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario notificador
        /// </summary>
        /// <param name="notificationUsers">Usuario notoficador a eliminar</param>
        public void DeleteEventNotificationUser(Models.EventNotificationUsers notificationUsers)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventNotificationUsers.CreatePrimaryKey(notificationUsers.GroupEventId, notificationUsers.EventId, notificationUsers.DelegationId, notificationUsers.UserId);
                EVENTEN.CoEventNotificationUsers entityCoEventNotificationUsers = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventNotificationUsers;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityCoEventNotificationUsers);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventNotificationUser", ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario notificador
        /// </summary>
        /// <param name="notificationUsers">Usuario a actualizar</param>
        public void UpdateEventNotificationUser(EventNotificationUsers notificationUsers)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventNotificationUsers.CreatePrimaryKey(notificationUsers.GroupEventId, notificationUsers.EventId, notificationUsers.DelegationId, notificationUsers.UserId);
                EVENTEN.CoEventNotificationUsers entityCoEventNotificationUsers = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventNotificationUsers;

                entityCoEventNotificationUsers.UserNotifDefault = notificationUsers.UserNotifDefault;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCoEventNotificationUsers);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateEventNotificationUser", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario notificador
        /// </summary>
        /// <param name="idGroup">id grupo de eventos</param>
        /// <param name="idEvent">id del evento</param>
        /// <param name="delegationId">id de la delegacion</param>
        public void DeleteEventAuthorizationUserByIdGroupIdEventIdDelegation(int idGroup, int idEvent, int delegationId)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventNotificationUsers.Properties.UserId, "Noti")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventNotificationUsers.Properties.GroupEventId, "Noti").Equal().Constant(idGroup)
                    .And().Property(EVENTEN.CoEventNotificationUsers.Properties.EventId, "Noti").Equal().Constant(idEvent)
                    .And().Property(EVENTEN.CoEventNotificationUsers.Properties.DelegationId, "Noti").Equal().Constant(delegationId);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventNotificationUsers), "Noti");
                select.Where = where.GetPredicate();

                List<EventNotificationUsers> List = new List<EventNotificationUsers>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        List.Add(new EventNotificationUsers
                        {
                            DelegationId = delegationId,
                            UserId = (int)reader["UserId"],
                            GroupEventId = idGroup,
                            EventId = idEvent,
                        });
                    }
                }

                foreach (var AutUser in List)
                {
                    DeleteEventNotificationUser(AutUser);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventAuthorizationUserByIdGroupIdEventIdDelegation", ex);
            }
        }
    }
}
