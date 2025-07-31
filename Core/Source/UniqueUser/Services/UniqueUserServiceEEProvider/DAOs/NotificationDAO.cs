using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Queues;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMMML = Sistran.Core.Application.UniqueUserServices.Models;//CommonService.Models;
using EnCommon = Sistran.Core.Application.Common.Entities;
using EnUniqueUser = Sistran.Core.Application.UniqueUser.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class NotificationDAO
    {
        /// <summary>
        /// Obtiene las notificacion por usuario
        /// </summary>
        /// <param name="idUser">Id del ususario</param>
        /// <param name="enabled">notififcaciones habilitadas</param>
        /// <param name="maxRow">cantidad limitada de registros</param>
        /// <returns>lista de notificacions</returns>
        public List<COMMML.NotificationUser> GetNotificationByUser(int idUser, bool? enabled, bool maxRow)
        {
            SelectQuery select = new SelectQuery();
            if (maxRow)
            {
                select.MaxRows = KeySettings.MaxRowNotificationsSetting();
            }
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.Id, "nu"), "IdNotification"));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.NotificationType, "nu")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.Message, "nu")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.UserId, "nu")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.Enabled, "nu")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.Parameters, "nu")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationUser.Properties.CreateDate, "nu")));

            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.Id, "nt"), "IdNotificationType"));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.Description, "nt")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.AccessId, "nt")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.Title, "nt")));

            select.AddSelectValue(new SelectValue(new Column(EnUniqueUser.Accesses.Properties.Url, "acc")));

            Join join = new Join(new ClassNameTable(typeof(EnCommon.NotificationUser), "nu"), new ClassNameTable(typeof(EnCommon.NotificationType), "nt"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnCommon.NotificationUser.Properties.NotificationType, "nu").Equal().Property(EnCommon.NotificationType.Properties.Id, "nt").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EnUniqueUser.Accesses), "acc"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnCommon.NotificationType.Properties.AccessId, "nt").Equal().Property(EnUniqueUser.Accesses.Properties.AccessId, "acc").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnCommon.NotificationUser.Properties.UserId, "nu").Equal().Constant(idUser);
            if (enabled != null)
            {
                where.And().Property(EnCommon.NotificationUser.Properties.Enabled, "nu").Equal().Constant(enabled);
            }
            select.Table = join; select.Where = where.GetPredicate();
            select.AddSortValue(new SortValue(new Column(EnCommon.NotificationType.Properties.Id, "nt"),SortOrderType.Descending));

            List<NotificationUser> notifications = new List<NotificationUser>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    notifications.Add(new NotificationUser
                    {
                        Id = (int)reader["IdNotification"],
                        NotificationType = new NotificationType
                        {
                            Id = (int)reader["IdNotificationType"],
                            Description = reader["Description"].ToString(),
                            Title = reader["Title"].ToString(),
                            Url = reader["Url"]?.ToString(),
                            AccessId = (int?)reader["AccessId"],
                            Type = (UniqueUserServices.Enums.NotificationTypes)Enum.Parse(typeof(UniqueUserServices.Enums.NotificationTypes), reader["IdNotificationType"].ToString())
                        },
                        Message = reader["Message"].ToString(),
                        UserId = (int)reader["UserId"],
                        Enabled = (bool)reader["Enabled"],
                        Parameters = reader["Parameters"] != null ? JsonConvert.DeserializeObject<Dictionary<string, object>>(reader["Parameters"].ToString()) : new Dictionary<string, object>(),
                        CreateDate = (DateTime)reader["CreateDate"]
                    });
                }
            }
            return notifications.OrderByDescending(x => x.CreateDate).ToList();
        }

        /// <summary>
        /// actualiza la notificacion
        /// </summary>
        /// <param name="notification">Notificacion a actualizar</param>
        public void UpdateNotification(COMMML.NotificationUser notification)
        {
            PrimaryKey key = EnCommon.NotificationUser.CreatePrimaryKey(notification.Id);
            EnCommon.NotificationUser entity = (EnCommon.NotificationUser)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            entity.Enabled = notification.Enabled;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
        }

        /// <summary>
        /// Crea una nueva notificacion
        /// </summary>
        /// <param name="notification">Notificacion a crear</param>
        public void CreateNotification(COMMML.NotificationUser notification)
        {
            string serialization = ConfigurationManager.AppSettings["Serialization"];
            string notificationQueue = ConfigurationManager.AppSettings["NotificationQueue"];


            notification.Enabled = true;
            notification.CreateDate = DateTime.Now;
            EnCommon.NotificationUser entity = new EnCommon.NotificationUser
            {
                NotificationType = (int)notification.NotificationType.Type,
                Message = notification.Message,
                UserId = notification.UserId,
                Enabled = notification.Enabled,
                Parameters = notification.Parameters == null ? null : notification.Parameters.Count == 0 ? null : JsonConvert.SerializeObject(notification.Parameters),
                CreateDate = DateTime.Now
            };

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            notification.NotificationType = this.GetNotificationType((int)notification.NotificationType.Type);
            if (KeySettings.ValidateNotificateQueeque())
            {
                TP.Task.Run(() =>
                {
                    try
                    {
                        IQueue queue = new BaseQueueFactory().CreateQueue(notificationQueue, routingKey: notificationQueue, serialization: serialization);
                        queue.PutOnQueue(notification);
                    }
                    catch (Exception ex)
                    {
                        EventLog eventLog = new EventLog("Application")
                        {
                            Source = "Application",

                        };
                        eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                    }

                });
            }
        }
        public COMMML.NotificationType GetNotificationType(int idNotificationType)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.Id, "nt"), "IdNotificationType"));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.Description, "nt")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.AccessId, "nt")));
            select.AddSelectValue(new SelectValue(new Column(EnCommon.NotificationType.Properties.Title, "nt")));

            select.AddSelectValue(new SelectValue(new Column(EnUniqueUser.Accesses.Properties.Url, "acc")));

            Join join = new Join(new ClassNameTable(typeof(EnCommon.NotificationType), "nt"), new ClassNameTable(typeof(EnUniqueUser.Accesses), "acc"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(EnCommon.NotificationType.Properties.AccessId, "nt")
                     .Equal().Property(EnUniqueUser.Accesses.Properties.AccessId, "acc").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnCommon.NotificationType.Properties.Id, "nt").Equal().Constant(idNotificationType);

            select.Table = join; select.Where = where.GetPredicate();

            var NotificationType = new List<COMMML.NotificationType>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    NotificationType.Add(new COMMML.NotificationType
                    {
                        Id = (int)reader["IdNotificationType"],
                        Description = reader["Description"].ToString(),
                        Title = reader["Title"].ToString(),
                        Url = reader["Url"]?.ToString(),
                        AccessId = (int?)reader["AccessId"],
                        Type = (Core.Application.UniqueUserServices.Enums.NotificationTypes)Enum.Parse(typeof(Core.Application.UniqueUserServices.Enums.NotificationTypes), reader["IdNotificationType"].ToString())
                    });
                }
            }
            return NotificationType.First();
        }

        /// <summary>
        /// Actualiza el parametro de la notificacion
        /// </summary>
        /// <param name="notificationId">id de la notificacion</param>
        public void UpdateNotificationParameter(int notificationId)
        {
            PrimaryKey key = EnCommon.NotificationUser.CreatePrimaryKey(notificationId);
            EnCommon.NotificationUser entity = (EnCommon.NotificationUser)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            entity.Parameters = null;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
        }

        /// <summary>
        /// Obtiene las notificacion por usuario
        /// </summary>
        /// <param name="idUser">Id del ususario</param>
        /// <param name="enabled">notififcaciones habilitadas</param>
        /// <returns>Cantidad de registros</returns>
        public int GetNotificationCountByUser(int idUser, bool? enabled)
        {
            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Count);
            function.AddParameter(new Column(EnCommon.NotificationUser.Properties.Id, typeof(EnCommon.NotificationUser).Name));
            select.AddSelectValue(new SelectValue(function, "IdNotification"));
            select.Table = new ClassNameTable(typeof(EnCommon.NotificationUser), typeof(EnCommon.NotificationUser).Name);

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(EnCommon.NotificationUser.Properties.UserId).Equal().Constant(idUser);
            if (enabled != null)
            {
                where.And().Property(EnCommon.NotificationUser.Properties.Enabled).Equal().Constant(enabled);
            }
            select.Where = where.GetPredicate();

            IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select);
            int count = 0;
            while (reader.Read())
            {
                count = (int)reader["IdNotification"];
            }

            return count;
        }

        /// <summary>
        /// Actualiza el parametro Enabled a false de todas las notificacion
        /// </summary>
        /// <param name="userId">id del usuario</param>
        public List<COMMML.NotificationUser> UpdateAllNotificationDisabledByUser(int userId)
        {
            IList<COMMML.NotificationUser> notifications = GetNotificationByUser(userId, true, false);

            foreach (COMMML.NotificationUser notification in notifications)
            {
                notification.Enabled = false;
                UpdateNotification(notification);
            }
            notifications = GetNotificationByUser(userId, true, true);
            return notifications.ToList();
        }

        public COMMML.NotificationUser GetNotificationQueue()
        {
            var serialization = ConfigurationManager.AppSettings["Serialization"];
            var notificationQueue = ConfigurationManager.AppSettings["notificationQueue"];
            IQueue queue = new BaseQueueFactory().CreateQueue(notificationQueue, routingKey: notificationQueue, serialization: serialization);

            var receiveFromQueue = queue.ReceiveFromQueue();
            return receiveFromQueue == null ? null : JsonConvert.DeserializeObject<COMMML.NotificationUser>(receiveFromQueue.ToString());
        }
    }
}