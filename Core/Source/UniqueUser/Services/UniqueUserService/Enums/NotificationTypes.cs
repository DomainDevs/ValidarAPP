// -----------------------------------------------------------------------
// <copyright file="NotificationTypes.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniqueUserServices.Enums
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
        RejectPoliciesMassive = 8,

        /// <summary>
        /// Emision Colectiva
        /// </summary>
        [EnumMember]
        ErrorEmission = 9,

        /// <summary>
        /// Work Flow
        /// </summary>
        [EnumMember]
        WorkFlow = 10,

        /// <summary>
        /// Denuncia
        /// </summary>
        [EnumMember]
        Claim = 11,

        /// <summary>
        /// Política de denuncia aceptada
        /// </summary>
        [EnumMember]
        AcceptPoliciesClaim = 12,

        /// <summary>
        /// Política de denuncia rechazada
        /// </summary>
        [EnumMember]
        RejectPoliciesClaim = 13,

        /// <summary>
        /// Solicitud de pago
        /// </summary>
        [EnumMember]
        PaymentRequest = 14,

        /// <summary>
        /// Política de solicitud de pago aceptada
        /// </summary>
        [EnumMember]
        AcceptPoliciesPaymentRequest = 15,

        /// <summary>
        /// Política de solicitud de pago rechazada
        /// </summary>
        [EnumMember]
        RejectPoliciesPaymentRequest = 16,

        /// <summary>
        /// Aviso
        /// </summary>
        [EnumMember]
        ClaimNotice = 17,

        /// <summary>
        /// Política de aviso aceptada
        /// </summary>
        [EnumMember]
        AcceptPoliciesClaimNotice = 18,

        /// <summary>
        /// Política de aviso rechazada
        /// </summary>
        [EnumMember]
        RejectPoliciesClaimNotice = 19,

        /// <summary>
        /// Política reasignada
        /// </summary>
        [EnumMember]
        PoliciesReassigned = 20,

        /// <summary>
        /// Persona aceptada
        /// </summary>
        [EnumMember]
        PersonAccept = 21,

        /// <summary>
        /// Persona aceptada
        /// </summary>
        [EnumMember]
        SarlaftPersonAccept = 22,

        /// <summary>
        /// Solicitud de cobro
        /// </summary>
        [EnumMember]
        ChargeRequest = 23,

        /// <summary>
        /// Política de solicitud de cobro aceptada
        /// </summary>
        [EnumMember]
        AcceptPoliciesChargeRequest = 24,

        /// <summary>
        /// Política de solicitud de cobro rechazada
        /// </summary>
        [EnumMember]
        RejectPoliciesChargeRequest = 25

    }
}
