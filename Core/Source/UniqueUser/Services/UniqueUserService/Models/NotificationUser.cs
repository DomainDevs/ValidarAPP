// -----------------------------------------------------------------------
// <copyright file="NotificationUser.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniqueUserServices.Models
{
    using Sistran.Core.Application.CommonService.Models.Base;
    using Sistran.Core.Application.UniqueUserServices.Models.Base;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de las notificaciones de usuario
    /// </summary>
    [DataContract]
    public class NotificationUser : BaseNotificationUser
    {
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad NotificationType
        /// </summary>
        [DataMember]
        public NotificationType NotificationType { get; set; }
    }
}