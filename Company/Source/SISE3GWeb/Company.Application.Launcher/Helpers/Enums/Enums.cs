// -----------------------------------------------------------------------
// <copyright file="Enums.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Helpers.Enums
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parametros para proveedores
    /// </summary>
    /// <returns></returns>
    public enum Supplier
    {
        Adjuster = 1,
        Analyst = 2,
    }

    public enum Concept
    {
        Indemnity = 1,
        Fee = 2,
        Expenses = 3,
    }

    public enum ClaimNoticeState
    {
        Created = 1,
        Objected = 2,
        Accepted = 3
    }

    public enum ClaimNoticeType
    {
        Vehicle = 1,
        Location = 2,
        Surety = 7
    }

    public enum CoveredRiskType
    {
        Vehicle = 1,
        Property = 2,
        Surety = 7
    }

    public enum AccountingDate
    {
       ModuleCode = 4
    }

    public enum Module
    {
        Claim = 27
    }

    public enum SubModule
    {
        Recovery = 6,
        Claim = 2,
        ClaimNotice = 1,
        PaymentRequest = 3
    }
    
    public enum PersonRole
    {
        Buyer = 1,
        Recuperator = 2,
        Debtor = 3
    }

    public enum DateRangeDays
    { 
        Notification = 15
    }

    public enum EventGroupsId
    {
        Claim = 100,
        AutomovilesEmision = 1
    }

    public enum Countries
    {
        Colombia = 1,
    }

    public enum States
    {
        Bogota = 17,
    }

    public enum Cities
    {
        Bogota = 1,
    }

    public enum PaymentRequestType 
    {
        Payment = 1,
        Recovery = 2,
        Void = 3
    };

    public enum EventsGroup
    {
        Claim = 100,
        PaymentRequest = 101
    };

    public enum EventsType
    {
        Vehicle = 1,
        Location = 2,
        Surety = 3,
        PaymentRequest = 4
    };

    public enum ClaimPrefix
    {
        Vehicle = 10,
        Location = 21,
        Surety1 = 30,
        Surety2 = 31
    }

    public enum ProsecutionProcessState
    {
        Active = 1,
        Cancelled = 0
    }

    public enum ConditionType
    {
        Prefix = 2,
        TypeRisk = 3,
        Coverage = 5,
        LineBusiness =4
    }


    /// <summary>
    /// Tipos de documentos Sise 3G
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// valor por defecto
        /// </summary>
        [EnumMember]
        Default = 0,

        /// <summary>
        /// cedula de ciudadania
        /// </summary>
        [EnumMember]
        Cc = 1,

        /// <summary>
        /// Cedula Extranjeria
        /// </summary>
        [EnumMember]
        Ce = 2,

        /// <summary>
        /// Tarjeta de identidad
        /// </summary>
        [EnumMember]
        Ti = 4,

        /// <summary>
        /// Identifica Pasaporte 
        /// </summary>
        [EnumMember]
        Ps = 5,

        /// <summary>
        /// Tarjeta de seguro social extr
        /// </summary>
        [EnumMember]
        Tss = 6,

        /// <summary>
        /// Sociedad extranj sin nit en colombia
        /// </summary>
        [EnumMember]
        Sen = 7,

        /// <summary>
        /// Identifica Fidecomiso 
        /// </summary>
        [EnumMember]
        Fdi = 8,

        /// <summary>
        /// Registro civil
        /// </summary>
        [EnumMember]
        Rc = 9,

        /// <summary>
        /// Sin documento
        /// </summary>
        [EnumMember]
        Sd = 10,

        /// <summary>
        /// Identifica SF
        /// </summary>
        [EnumMember]
        Soc = 11
    }
    public enum AutoServiceType
    {
        Particular = 1,       
    }
  public enum TransactionType
    {
        CreateLog = 1,
        UpdateLog = 2,
        DeleteLog = 3
    }
    public enum TarifationType
    {
        Porcentaje = 100,
        Millaje = 100,
        Importe = 3
    }

    public enum PrefixTypeMinPremium 
    {
        Lease=32,
        Surety = 30,
        Vehicle = 7,
        JudicialSurety = 31,
        Liability = 15
    }

    public enum VehServiceType
    {
        /// <summary>
        /// Tipo Servicio del Veh�culo
        /// </summary>
        NoAplica = 8

    }
   
    public enum FuelType
    {
        [Description("No Aplica")] NoAplica = 1,
        [Description("DSL")] DSL = 2,
        [Description("ELT")] ELT = 3,
        [Description("GAS")] GAS = 4,
        [Description("GSL")] GSL = 5,
    }
    public enum UseType
    {
        [Description("TPTE. PERSONAS")] Tpte_Personas = 1,
        [Description("TPTE. DE CARGA")] Tpte_De_Carga = 2,
        [Description("CUERPO DIPLOMAT")] Cuerpo_Diplomatico = 3,
        [Description("VEH.EN DEMOSTRA")] Veh_En_Demostra = 4,
        [Description("PARA ALQUILER")] Para_Alquiler = 5,
        [Description("PARA ENSEÑANZA")] Para_Enseñanza = 6,
        [Description("CUERPO DE BOMBE")] Cuerpo_De_Bombe = 7,
        [Description("AMBULANCIA")] Ambulancia = 8,
        [Description("FUERZAS MILITAR")] Fuerza_Militar = 9,
        [Description("RECOLECTORES DE")] Recolectores_De = 10,
        [Description("SERVICIO PUBLIC")] Servicio_Publico = 11,
        [Description("SERV.PUBLICO IN")] Servicio_Publico_In = 12,
        [Description("TRANPORTE PERSO")] Transporte_Personas = 13,
        [Description("FAMILIAR")] Familiar = 14,
        [Description("ESPECIAL")] Especial = 16,
        [Description("PUBLICO URBANO")] Publico_Urbano = 18,
        [Description("OTRO SERVICIO")] Otro_Servicio = 19,
        [Description("N/A")] NoAplica = 20

    }

    public enum ProductTypeRCE 
    {
        EXTRACONTRAC=69,
        PASAJEROS=75
    }
}
