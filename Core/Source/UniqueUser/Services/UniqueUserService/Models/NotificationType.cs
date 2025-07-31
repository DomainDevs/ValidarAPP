// -----------------------------------------------------------------------
// <copyright file="NotificationType.cs" company="SISTRAN">
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
    /// Modelo tipo de notificacion
    /// </summary>
    [DataContract]
    public class NotificationType : BaseNotificationType
    {
        /// <summary>
        /// Obtiene o establece el Atributo para la propiedad Type
        /// </summary>
        [DataMember]
        public Enums.NotificationTypes Type { get; set; }
    }
}