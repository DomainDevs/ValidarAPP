// -----------------------------------------------------------------------
// <copyright file="NotificationTypes.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonService.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enumerador para los tipos de notificaciones
    /// </summary>
    [DataContract]
    public enum NotificationTypes
    {
        /// <summary>
        /// Tipo general
        /// </summary>
        [EnumMember]
        General = 1,

        /// <summary>
        /// Tipo Politica de autorizacion
        /// </summary>
        [EnumMember]
        AutorizationPolicies = 2,

        /// <summary>
        /// Tipo Politica de notificacion
        /// </summary>
        [EnumMember]
        NotificationPolicies = 3,

        /// <summary>
        /// Tipo Politica aceptada
        /// </summary>
        [EnumMember]
        AcceptPolicies = 4,

        /// <summary>
        /// Tipo politica rechazada
        /// </summary>
        [EnumMember]
        RejectPolicies = 5,

        /// <summary>
        /// Tipo Emision
        /// </summary>
        [EnumMember]
        Emission = 6,

        /// <summary>
        /// Tipo Emision
        /// </summary>
        [EnumMember]
        AcceptPoliciesMassive = 7,

        /// <summary>
        /// Tipo Emision
        /// </summary>
        [EnumMember]
        RejectPoliciesMassive = 8
    }
}
