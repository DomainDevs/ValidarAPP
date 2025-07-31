using System.Runtime.Serialization;

namespace Sistran.Company.Application.PrintingServices.Enums
{
    public enum Printing_Type
    {
        [EnumMember]
        Massive = 1,
        [EnumMember]
        Collective = 2,
        [EnumMember]
        Request = 3,
    }

    public enum ReportType
    {
        [EnumMember]
        COMPLETE_POLICY = 1,
        [EnumMember]
        ONLY_POLICY = 2,
        [EnumMember]
        PAYMENT_CONVENTION = 3,
        [EnumMember]
        TEMPORARY = 4,
        [EnumMember]
        QUOTATION = 5,
        [EnumMember]
        COMPLETE_REQUEST = 6,
        [EnumMember]
        ONLY_REQUEST = 7,
        [EnumMember]
        ONLY_POLICIES_REQUEST = 8,
        [EnumMember]
        FORMAT_COLLECT = 9,
        [EnumMember]
        MASS_LOAD = 10
    };

    public enum FileReportVersion
    {
        [EnumMember]
        VERSION_1 = 1,
        [EnumMember]
        VERSION_2 = 2
    }

    public enum TextLines
    {
        [EnumMember]
        COMPLIANCE_TEXT_LINES = 15,
        [EnumMember]
        MAX_CHAR_PER_LINE = 135
    };

    public enum PrefixCode
    {
        [EnumMember]
        MANAGE = 1,
        [EnumMember]
        SURETY = 2,
        [EnumMember]
        FIRE = 3,
        [EnumMember]
        SUBTRACTION = 4,
        [EnumMember]
        TRANSPORT = 5,
        [EnumMember]
        AUTOS = 10,
        [EnumMember]
        RCV = 13,
        [EnumMember]
        CROP = 15,
        [EnumMember]
        JUDICIAL_SURETY = 29,
        [EnumMember]
        COMPLIANCE = 30,
        [EnumMember]
        WEAK_FLOW = 83        
    };

    public enum PrefixTypeCode
    {
        /// <summary>
        /// Pólizas generales: Corriente débil, Incendio, Responsabilidad Civil, Sustracción
        /// </summary>
        [EnumMember]
        GENERALS = 1,

        /// <summary>
        /// Pólizas de autos
        /// </summary>
        [EnumMember]
        AUTOS = 2,

        /// <summary>
        /// Pólizas de transporte
        /// </summary>
        [EnumMember]
        TRANSPORTS = 5,

        /// <summary>
        /// Pólizas de caución judicial
        /// </summary>
        [EnumMember]
        JUDICIAL_SURETY = 6,

        /// <summary>
        /// Pólizas modulares: PreviPyme, PreviHogar
        /// </summary>
        [EnumMember]
        MODULAR = 12,

        /// <summary>
        /// Pólizas de cumplimiento
        /// </summary>
        [EnumMember]
        SURETY = 14,

        /// <summary>
        /// Pólizas de manejo
        /// </summary>
        [EnumMember]
        MANAGE = 15,

        /// <summary>
        /// Pólizas de responsabilidad civil
        /// </summary>
        [EnumMember]
        LIABILITY = 13
    }

    public enum PolicyType
    {
        [EnumMember]
        INDIVIDUAL = 1,
        [EnumMember]
        COLLECTIVE = 2
    };

    public enum RiskType
    {
        [EnumMember]
        AUTO = 1,
        [EnumMember]
        UBICACION = 2,
        [EnumMember]
        FIANZA = 7,
        [EnumMember]
        TRANSPORTE = 8
    };

    public enum RiskStatus
    {
        [EnumMember]
        NULL = 0,
        [EnumMember]
        ORIGINAL = 1,
        [EnumMember]
        INCLUDED = 2,
        [EnumMember]
        EXCLUDED = 3,
        [EnumMember]
        REHABILITATED = 4,
        [EnumMember]
        MODIFIED = 5,
        [EnumMember]
        NOTMODIFIED = 6,
        [EnumMember]
        CANCELLATED = 7,
    };

    public enum BarCode
    {
        [EnumMember]
        CONSTANT = 415,
        [EnumMember]
        COUNTRY_CD = 770,
        [EnumMember]
        COMPANY_CD = 733688,
        [EnumMember]
        SERVICE_CD = 001,
        [EnumMember]
        CONVENTION = 8,
        [EnumMember]
        FIELD_ID = 8020,
        [EnumMember]
        PARITY = 0
    }
}
