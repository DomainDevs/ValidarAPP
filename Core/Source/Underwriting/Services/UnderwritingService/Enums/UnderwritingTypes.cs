using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Enums
{
    [Flags]
    public enum TemporalType
    {
        [EnumMember]
        Quotation = 1,
        [EnumMember]
        Policy = 2,
        [EnumMember]
        Endorsement = 3,
        [EnumMember]
        TempQuotation = 4
    }

    [Flags]
    public enum RiskStatusType
    {
        /// <summary>
        /// Constante Original
        /// </summary>
        [EnumMember]
        Original = 1,

        /// <summary>
        /// Constante Included
        /// </summary>

        [EnumMember]
        Included = 2,

        /// <summary>
        /// Constante Excluded
        /// </summary>
        [EnumMember]
        Excluded = 3,

        /// <summary>
        /// Constante Rehabilitated
        /// </summary>
        [EnumMember]
        Rehabilitated = 4,

        /// <summary>
        /// Constante Modified
        /// </summary>
        [EnumMember]
        Modified = 5,

        /// <summary>
        /// Constante NotModified
        /// </summary>
        [EnumMember]
        NotModified = 6,

        /// <summary>
        /// Constante NotModified
        /// </summary>
        [EnumMember]
        Cancelled = 7
    }

    [Flags]
    public enum EmissionLevel
    {
        [EnumMember]
        General = 1,
        [EnumMember]
        Risk = 2,
        [EnumMember]
        Coverage = 3,
        [EnumMember]
        Component = 4,
        [EnumMember]
        Commission = 5
    }

    [Flags]
    public enum JsonType
    {
        [EnumMember]
        TEMPORAL = 1,
        [EnumMember]
        POLICY = 2
    }

    [Flags]
    public enum PaymentCalculationType
    {
        [EnumMember]
        Day = 1,
        [EnumMember]
        Fortnight = 2,
        [EnumMember]
        Month = 3
    }

    [Flags]
    public enum FirstRiskType
    {
        [EnumMember]
        None = 1,
        [EnumMember]
        FirstLost = 2
    }

    [Flags]
    public enum EndorsementType
    {
        [EnumMember]
        [Description("Emisión")]
        Emission = 1,
        [EnumMember]
        [Description("Modificación")]
        Modification = 2,
        [EnumMember]
        [Description("Cancelación")]
        Cancellation = 3,
        [EnumMember]
        Nominative_cancellation = 23,
        [EnumMember]
        Rehabilitation = 4,
        [EnumMember]
        EffectiveExtension = 5,
        [EnumMember]
        [Description("Renovación")]
        Renewal = 6,
        [EnumMember]
        Rebill = 7,
        [EnumMember]
        WithoutPremiumMovement = 8,
        [EnumMember]
        ClaimExclusion = 9,
        [EnumMember]
        RestaTMPENt = 10,
        [EnumMember]
        PremiumAmount = 11,
        [EnumMember]
        AutomaticCancellation = 12,
        [EnumMember]
        AutomaticRehabilitation = 13,
        [EnumMember]
        IncrementPremiumMovement = 14,
        [EnumMember]
        DecrementPremiumMovement = 15,
        [EnumMember]
        ClaimCancellation = 16,
        [EnumMember]
        ProrrateRehabilitation = 17,
        [EnumMember]
        TableRehabilitation = 18,
        [EnumMember]
        WithoutRecalculationRehabilitation = 19,
        [EnumMember]
        RequestDateCancellation = 20,
        [EnumMember]
        RequestBalanceCancellation = 21,
        [EnumMember]
        LastEndorsementCancellation = 22,
        [EnumMember]
        DeclarationEndorsement = 24,
        [EnumMember]
        AdjustmentEndorsement = 25,
        [EnumMember]
        CreditNoteEndorsement = 26,
        [EnumMember]
        ChangeTermEndorsement = 27,
        [EnumMember]
        ChangeAgentEndorsement = 29,
        [EnumMember]
        ChangePolicyHolderEndorsement = 30,
        [EnumMember]
        ChangeConsolidationEndorsement = 31,
        [EnumMember]
        ChangeCoinsuranceEndorsement = 32
    }

    [Flags]
    public enum CoverageStatusType
    {
        /// <summary>
        /// Constante Original
        /// </summary>
        [EnumMember]
        Original = 1,

        /// <summary>
        /// Constante Included
        /// </summary>
        [EnumMember]
        Included = 2,

        /// <summary>
        /// Constante Excluded
        /// </summary>
        [EnumMember]
        Excluded = 3,

        /// <summary>
        /// Constante Modified
        /// </summary>
        [EnumMember]
        Modified = 4,

        /// <summary>
        /// Constante NotModified
        /// </summary>
        [EnumMember]
        NotModified = 5,

        /// <summary>
        /// Constante Cancellated
        /// </summary>
        [EnumMember]
        Cancelled = 6,

        /// <summary>
        /// Constante Rehabilitated
        /// </summary>
        [EnumMember]
        Rehabilitated = 7
    }

    [Flags]
    public enum ConditionLevelType
    {
        [EnumMember]
        Independent = 1,
        [EnumMember]
        Prefix = 2,
        [EnumMember]
        Risk = 3,
        [EnumMember]
        Linebusiness = 4,
        [EnumMember]
        Coverage = 5
    }

    [Flags]
    public enum ComponentType
    {
        /// <summary>
        /// Prima
        /// </summary>
        [EnumMember]
        Premium = 1,

        /// <summary>
        /// Gastos
        /// </summary>
        [EnumMember]
        Expenses = 2,

        /// <summary>
        /// Sobrecargos
        /// </summary>
        [EnumMember]
        Surcharges = 3,

        /// <summary>
        /// Descuentos
        /// </summary>
        [EnumMember]
        Discounts = 4,

        /// <summary>
        /// Impuestos
        /// </summary>
        [EnumMember]
        Taxes = 5
    }

    [Flags]
    public enum ComponentClassType
    {
        /// <summary>
        /// Componente base
        /// </summary>
        [EnumMember]
        HardComponent = 1,

        /// <summary>
        /// Componente impuesto
        /// </summary>
        [EnumMember]
        TaxComponent = 2,

        /// <summary>
        /// Gastos
        /// </summary>
        [EnumMember]
        Expenses = 3,

        /// <summary>
        /// Descuentos
        /// </summary>
        [EnumMember]
        Discounts = 4,

        /// <summary>
        /// Sobrecargos
        /// </summary>
        [EnumMember]
        Surcharges = 5
    }

    [Flags]
    public enum CalculationType
    {
        /// <summary>
        /// Constante Prorrata
        /// </summary>
        [EnumMember]
        Prorate = 1,

        /// <summary>
        /// Constante Directo
        /// </summary>
        [EnumMember]
        Direct = 2,

        /// <summary>
        /// Constante Corto Plazo
        /// </summary>
        [EnumMember]
        ShortTerm = 3
    }

    [Flags]
    public enum BusinessType
    {
        [EnumMember]
        [Description("100 % Compañia")]
        CompanyPercentage = 1,
        [EnumMember]
        [Description("Coaseguro aceptado")]
        Accepted = 2,
        [EnumMember]
        [Description("Coaseguro cedido")]
        Assigned = 3
    }

    [Flags]
    public enum Facade
    {
        [EnumMember]
        General = 83,
        [EnumMember]
        Risk = 85,
        [EnumMember]
        Coverage = 84,
        [EnumMember]
        Component = 86,
        [EnumMember]
        Event = 139
    }

    [Flags]
    public enum ProcessTypes
    {
        [EnumMember]
        ShortTerm = 1
    }
    [Flags]
    public enum RepaimentTax
    {

        Unica = 1
    }

    [Flags]
    public enum DocumentTypes
    {
        [EnumMember]
        [Description("CEDULA DE CIUDADANIA")]
        CC = 1,
        [EnumMember]
        [Description("NIT")]
        NIT = 2,
        [EnumMember]
        [Description("CEDULA DE EXTRANJERIA")]
        CE = 3,
        [EnumMember]
        [Description("TARJETA DE IDENTIDAD")]
        TI = 4,
        [EnumMember]
        [Description("No. UNICO IDENTIFICACION TRIBU")]
        NUI = 5,
        [EnumMember]
        [Description("REGISTRO CIVIL")]
        RC = 6
    }
    [Flags]
    public enum AppSource
    {
        [EnumMember]
        R1 = 1,
        [EnumMember]
        R2 = 2
    }

    [Flags]
    public enum PolicyOrigin
    {
        [EnumMember]
        Individual = 1,
        Massive = 2,
        Collective = 3
    }


    [Flags]
    public enum PrefixRc
    {
        [EnumMember]
        Liability = 15
    }
}