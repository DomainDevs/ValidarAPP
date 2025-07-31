using Sistran.Co.Application.Data;
using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
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
    public class EventAuthorizationUserDAO
    {
        /// <summary>
        /// obtiene el email de un usuario 
        /// </summary>
        /// <param name="idUser">id de usuario</param>
        /// <returns></returns>
        public string GetEmailByIdUser(int idUser)
        {
            try
            {
                SelectQuery select = new SelectQuery();

                select.AddSelectValue(new SelectValue(new Column(EVENTEN.Email.Properties.Address, "M")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.UniqueUsers), "UU"), new ClassNameTable(typeof(EVENTEN.Email), "M"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.UniqueUsers.Properties.PersonId, "UU").Equal().Property(EVENTEN.Email.Properties.IndividualId, "M").GetPredicate();

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.UniqueUsers.Properties.UserId, "UU").Equal().Constant(idUser);

                select.Table = join;
                select.Where = where.GetPredicate();

                string email = "";
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        email = (string)reader["Address"];
                    }
                }

                return email;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// obtiene la lista de Id de personas y id de usuarios
        /// </summary>
        /// <param name="IdGroup">id del grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <returns></returns>
        public List<Models.DelegationUser> GetListPersonIdUserIdByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                SelectQuery select = new SelectQuery();

                select.AddSelectValue(new SelectValue(new Column(EVENTEN.UniqueUsers.Properties.PersonId, "UU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.UniqueUsers.Properties.UserId, "UU"), "UUUserId"));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorizationUsers.Properties.UserId, "EAU"), "EAUUserId"));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventNotificationUsers.Properties.UserId, "ENU"), "ENUUserId"));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventNotificationUsers.Properties.UserNotifDefault, "ENU")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoHierarchyAssociation.Properties.Description, "HAS")));
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.Email.Properties.Address, "M")));

                Join join = new Join(new ClassNameTable(typeof(EVENTEN.CoHierarchyAccesses), "HA"), new ClassNameTable(typeof(EVENTEN.UniqueUsers), "UU"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAccesses.Properties.UserId, "HA").Equal().Property(EVENTEN.UniqueUsers.Properties.UserId, "UU").GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoHierarchyAssociation), "HAS"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAssociation.Properties.HierarchyCode, "HAS").Equal().Property(EVENTEN.CoHierarchyAccesses.Properties.HierarchyCode, "HA")
                    .And().Property(EVENTEN.CoHierarchyAssociation.Properties.ModuleCode, "HAS").Equal().Property(EVENTEN.CoHierarchyAccesses.Properties.ModuleCode, "HA")
                    .And().Property(EVENTEN.CoHierarchyAssociation.Properties.SubmoduleCode, "HAS").Equal().Property(EVENTEN.CoHierarchyAccesses.Properties.SubmoduleCode, "HA")
                    .GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventGroup), "EG"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAssociation.Properties.ModuleCode, "HAS").Equal().Property(EVENTEN.CoEventGroup.Properties.ModuleCode, "EG")
                       .And().Property(EVENTEN.CoHierarchyAssociation.Properties.SubmoduleCode, "HAS").Equal().Property(EVENTEN.CoEventGroup.Properties.SubmoduleCode, "EG")
                       .GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventCompany), "EC"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoEventGroup.Properties.GroupEventId, "EG").Equal().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventDelegation), "ED"), JoinType.Left);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAccesses.Properties.HierarchyCode, "HA").Equal().Property(EVENTEN.CoEventDelegation.Properties.HierarchyCode, "ED")
                    .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Property(EVENTEN.CoEventDelegation.Properties.GroupEventId, "ED")
                    .And().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Property(EVENTEN.CoEventDelegation.Properties.EventId, "ED")
                    .GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventAuthorizationUsers), "EAU"), JoinType.Left);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAccesses.Properties.UserId, "HA").Equal().Property(EVENTEN.CoEventAuthorizationUsers.Properties.UserId, "EAU")
                     .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Property(EVENTEN.CoEventAuthorizationUsers.Properties.GroupEventId, "EAU")
                     .And().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Property(EVENTEN.CoEventAuthorizationUsers.Properties.EventId, "EAU")
                     .And().Property(EVENTEN.CoEventDelegation.Properties.DelegationId, "ED").Equal().Property(EVENTEN.CoEventAuthorizationUsers.Properties.DelegationId, "EAU")
                     .GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.CoEventNotificationUsers), "ENU"), JoinType.Left);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.CoHierarchyAccesses.Properties.UserId, "HA").Equal().Property(EVENTEN.CoEventNotificationUsers.Properties.UserId, "ENU")
                    .And().Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Property(EVENTEN.CoEventNotificationUsers.Properties.GroupEventId, "ENU")
                    .And().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Property(EVENTEN.CoEventNotificationUsers.Properties.EventId, "ENU")
                    .And().Property(EVENTEN.CoEventDelegation.Properties.DelegationId, "ED").Equal().Property(EVENTEN.CoEventNotificationUsers.Properties.DelegationId, "ENU")
                    .GetPredicate();

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventCompany.Properties.GroupEventId, "EC").Equal().Constant(IdGroup);
                where.And().Property(EVENTEN.CoEventCompany.Properties.EventId, "EC").Equal().Constant(IdEvent);
                where.And().Property(EVENTEN.CoHierarchyAccesses.Properties.HierarchyCode, "HA").Equal().Constant(IdHierarchy);

                join = new Join(join, new ClassNameTable(typeof(EVENTEN.Email), "M"), JoinType.Left);
                join.Criteria = new ObjectCriteriaBuilder().Property(EVENTEN.UniqueUsers.Properties.PersonId, "UU").Equal().Property(EVENTEN.Email.Properties.IndividualId, "M").GetPredicate();

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Models.DelegationUser> List = new List<Models.DelegationUser>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        List.Add(
                            new DelegationUser
                            {
                                PersonId = (int)reader["PersonId"],
                                UserId = (int)reader["UUUserId"],
                                NotificatedInd = (reader["ENUUserId"]) == null ? false : true,
                                AuthorizedInd = (reader["EAUUserId"]) == null ? false : true,
                                NotificatedDefault = (reader["UserNotifDefault"]) == null ? false : (bool)reader["UserNotifDefault"],
                                Description = (string)reader["Description"],
                                Email = (string)reader["Address"]
                            });
                    }
                }

                return List;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetListPersonIdUserIdByIdGroupIdEventIdHierarchy", ex);
            }
        }

        /// <summary>
        /// ejecuta el sp (REPORT.CO_INDIVIDUAL_NAME), para obtener el nombre del usuario
        /// </summary>
        /// <param name="IdPerson">id de person</param>
        /// <returns>nombre del usuario</returns>
        public string GetUserNameByIdPerson(int IdPerson)
        {
            try
            {
                string name = "";
                NameValue[] parameters = new NameValue[3];
                parameters[0] = new NameValue("INDIVIDUAL_ID ", IdPerson);
                parameters[1] = new NameValue("CUSTOMER_TYPE", 1);
                parameters[2] = new NameValue("NAME", name);

                DataTable result = null;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("REPORT.CO_INDIVIDUAL_NAME", parameters);
                }


                if (result!=null && result.Rows.Count>0)
                {
                    return result.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetUserNameByIdPerson", ex);
            }
        }

        /// <summary>
        /// retorna los usuarios pertenecientes a la delegacion
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdHierarchy">id de la delegacion</param>
        /// <returns></returns>
        public List<Models.DelegationUser> GetDelegationUsersByIdGroupIdEventIdHierarchy(int IdGroup, int IdEvent, int IdHierarchy)
        {
            try
            {
                NameValue[] parameters = new NameValue[3];
                parameters[0] = new NameValue("GROUP_EVENT_ID", IdGroup);
                parameters[1] = new NameValue("EVENT_ID", IdEvent);
                parameters[2] = new NameValue("HIERARCHY_CD", IdHierarchy);

                DataTable result = null;
                using (DynamicDataAccess pdb = new DynamicDataAccess()) {
                    result = pdb.ExecuteSPDataTable("EVE.CO_GET_DELEGATION_USERS", parameters);
                }
                List<Models.DelegationUser> list = new List<Models.DelegationUser>();
                foreach (DataRow item in result.Rows)
                {
                    list.Add(new Models.DelegationUser
                    {
                        AuthorizedInd = Convert.ToBoolean(item[0]),
                        NotificatedInd = Convert.ToBoolean(item[1]),
                        NotificatedDefault = Convert.ToBoolean(item[2]),
                        UserId = Convert.ToInt32(item[3].ToString()),
                        Description = item[4].ToString(),
                        UserName = item[5].ToString(),
                        Email = item[6].ToString()
                    });
                }
                return list.OrderBy(x => !x.AuthorizedInd && !x.NotificatedInd && !x.NotificatedDefault).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetDelegationUsersByIdGroupIdEventIdHierarchy", ex);
            }
        }

        /// <summary>
        /// obtioene el usuario autorizador
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdDelegation">id de la delegacion</param>
        /// <param name="IdUser">id del usuario</param>
        /// <returns></returns>
        public Models.EventAuthorizationUsers GetEventAuthorizationUserByIdGroupIdEventIdDelegationIdUser(int IdGroup, int IdEvent, int IdDelegation, int IdUser)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventAuthorizationUsers.CreatePrimaryKey(IdGroup, IdEvent, IdDelegation, IdUser);
                EVENTEN.CoEventAuthorizationUsers entityCoEventAuthorizationUsers = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventAuthorizationUsers;

                if (entityCoEventAuthorizationUsers == null)
                {
                    return null;
                }
                else
                {
                    return ModelAssembler.CreateEventAuthorizationUsers(entityCoEventAuthorizationUsers);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetEventAuthorizationUserByIdGroupIdEventIdDelegationIdUser", ex);
            }
        }

        /// <summary>
        /// Crea un usuario autorizador
        /// </summary>
        /// <param name="authorizationUsers">usuario a crear</param>
        public void CreateEventAuthorizationUser(EventAuthorizationUsers authorizationUsers)
        {
            try
            {
                var EVENTEN = EntityAssembler.CreateEventAuthorizationUsers(authorizationUsers);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EVENTEN);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: CreateEventAuthorizationUser", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario autorizador
        /// </summary>
        /// <param name="authorizationUsers">usuario a eliminar</param>
        public void DeleteEventAuthorizationUser(EventAuthorizationUsers authorizationUsers)
        {
            try
            {
                PrimaryKey key = EVENTEN.CoEventAuthorizationUsers.CreatePrimaryKey(authorizationUsers.GroupEventId, authorizationUsers.EventId, authorizationUsers.DelegationId, authorizationUsers.UserId);
                EVENTEN.CoEventAuthorizationUsers entityCoEventAuthorizationUsers = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as EVENTEN.CoEventAuthorizationUsers;

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityCoEventAuthorizationUsers);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventAuthorizationUser", ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario autorizador
        /// </summary>
        /// <param name="delegationUser">usuario a actualizar</param>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdDelegation">id de la delegacion</param>
        public void UpdateDelegationUser(Models.DelegationUser delegationUser, int IdGroup, int IdEvent, int IdDelegation)
        {
            try
            {
                var eventAuth = GetEventAuthorizationUserByIdGroupIdEventIdDelegationIdUser(IdGroup, IdEvent, IdDelegation, delegationUser.UserId);
                var model = new EventAuthorizationUsers
                {
                    DelegationId = IdDelegation,
                    UserId = delegationUser.UserId,
                    GroupEventId = IdGroup,
                    EventId = IdEvent
                };

                //Autorizacion
                if (delegationUser.AuthorizedInd == true && eventAuth == null)
                {
                    CreateEventAuthorizationUser(model);
                }
                else if (delegationUser.AuthorizedInd == false && eventAuth != null)
                {
                    DeleteEventAuthorizationUser(model);
                }

                //Notificacion
                EventNotificationUsersDAO notificationDao = new EventNotificationUsersDAO();
                var eventNotif = notificationDao.GetEventNotificationUsersByIdGroupIdEventIdDelegationIdUser(IdGroup, IdEvent, IdDelegation, delegationUser.UserId);
                var modelNotif = new EventNotificationUsers
                {
                    DelegationId = IdDelegation,
                    UserId = delegationUser.UserId,
                    GroupEventId = IdGroup,
                    EventId = IdEvent,
                    UserNotifDefault = delegationUser.NotificatedDefault
                };

                if (delegationUser.NotificatedInd == true && eventNotif == null)
                {
                    notificationDao.CreateEventNotificationUser(modelNotif);
                }
                else if (delegationUser.NotificatedInd == true && eventNotif != null)
                {
                    notificationDao.UpdateEventNotificationUser(modelNotif);
                }
                else if (delegationUser.NotificatedInd == false && eventNotif != null)
                {
                    notificationDao.DeleteEventNotificationUser(modelNotif);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: UpdateDelegationUser", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario autorizador
        /// </summary>
        /// <param name="IdGroup">id grupo de eventos</param>
        /// <param name="IdEvent">id del evento</param>
        /// <param name="IdDelegation">id de la delegacion</param>
        public void DeleteEventAuthorizationUserByIdGroupIdEventIdDelegation(int idGroup, int idEvent, int delegationId)
        {
            try
            {
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(EVENTEN.CoEventAuthorizationUsers.Properties.UserId, "Auth")));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(EVENTEN.CoEventAuthorizationUsers.Properties.GroupEventId, "Auth").Equal().Constant(idGroup)
                    .And().Property(EVENTEN.CoEventAuthorizationUsers.Properties.EventId, "Auth").Equal().Constant(idEvent)
                    .And().Property(EVENTEN.CoEventAuthorizationUsers.Properties.DelegationId, "Auth").Equal().Constant(delegationId);

                select.Table = new ClassNameTable(typeof(EVENTEN.CoEventAuthorizationUsers), "Auth");
                select.Where = where.GetPredicate();

                List<EventAuthorizationUsers> list = new List<EventAuthorizationUsers>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        list.Add(new EventAuthorizationUsers
                        {
                            DelegationId = delegationId,
                            UserId = (int)reader["UserId"],
                            GroupEventId = idGroup,
                            EventId = idEvent,
                        });
                    }
                }

                foreach (var AutUser in list)
                {
                    DeleteEventAuthorizationUser(AutUser);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: DeleteEventAuthorizationUserByIdGroupIdEventIdDelegation", ex);
            }
        }
    }
}
