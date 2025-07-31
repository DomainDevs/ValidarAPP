using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Enums
{
    public class Types
    {
        [Flags]
        public enum RateType
        {
            [EnumMember]
            Percentage = 0,
            [EnumMember]
            Permillage = 1,
            [EnumMember]
            FixedAmount = 2,
        }

        [Flags]
        public enum FacadeType
        {
            /// <summary>
            /// Clasificación de conceptos a nivel general de poliza
            /// </summary> 
            [EnumMember]
            General = 83,
            /// <summary>
            ///  Clasificación de conceptos a nivel de covertura
            /// </summary>
            [EnumMember]
            Coverage = 84,
            /// <summary>
            ///  Clasificación de conceptos a nivel de Riesgo
            /// </summary>
            [EnumMember]
            Risk = 85,
            /// <summary>
            ///  Clasificación de conceptos a nivel de componente
            /// </summary>
            [EnumMember]
            Component = 86,
            /// <summary>
            ///  Clasificación de conceptos a nivel comisión
            /// </summary>
            [EnumMember]
            Commission = 87
        }

        [Flags]
        public enum FieldSetType
        {
            [EnumMember]
            AutoEmission = 101, //EMISIONAUTO = 101,
            [EnumMember]
            RequestAutoEmission = 102, //EMISIONAUTOSOLICITUD 
            [EnumMember]
            RCEmission = 103, //EMISIONRC 
            [EnumMember]
            RequestRCEmission = 104, //EMISIONRCSOLICITUD = 104,
            [EnumMember]
            AURisksInclusion = 105, //INCLUSIONRIESGOSAU = 105,
            [EnumMember]
            RCRiskInclusion = 106, //INCLUSIONRIESGOSRC = 106,
            [EnumMember]
            RisksExclusion = 107,      //EXCLUSIONRIESGOS=107
            [EnumMember]
            ExcelTariffed = 108
        }

        [Flags]
        public enum LoadMassiveStateType
        {
            /// <summary>
            /// Estado inicial del proceso de cargue
            /// </summary>
            [EnumMember]
            Initial = 0,
            /// <summary>
            /// El cargue ha sido terminado
            /// </summary>
            [EnumMember]
            ChargeCompleted = 1,
            /// <summary>
            /// El cargue se encuentra en proceso de tarifacion
            /// </summary>
            [EnumMember]
            Tariffing = 2,
            /// <summary>
            /// El proceso de tarifacion ha concluido correctamente
            /// </summary>
            [EnumMember]
            TariffedWithOutEvents = 3,
            /// <summary>
            /// El proceso de tarifacion ha conluido correctamente con eventos
            /// </summary>
            [EnumMember]
            TariffedWithEvents = 4,
            /// <summary>
            /// El cargue ya ha sido emitido
            /// </summary>
            [EnumMember]
            Issued = 5,
            /// <summary>
            /// El cargue se encuentra en proceso de emision
            /// </summary>
            [EnumMember]
            Issuing = 6,
            /// <summary>
            /// El cargue se encuentra trasladando informacion a temporales
            /// </summary>
            [EnumMember]
            MovingTemporary = 7,
            /// <summary>
            /// El proceso de cargue ha terminado con errores
            /// </summary>
            [EnumMember]
            CompletedWithErrors = 8,
            /// <summary>
            /// El proceso de cargue se ecuentra en temporales
            /// </summary>
            [EnumMember]
            Intotemporals = 9,
            /// <summary>
            /// El proceso de cargue tarifado con errores
            /// </summary>
            [EnumMember]
            TariffedWithErrors = 10,
            /// <summary>
            /// El proceso de cargue tarifado con eventos autorizados
            /// </summary>
            [EnumMember]
            TariffedWithEventsAuthorized = 11,
            /// <summary>
            /// El proceso de cargue emitido con errores
            /// </summary>
            [EnumMember]
            IssuedWithErrors = 13,
            /// <summary>
            /// El proceso de cargue emitido con eventos
            /// </summary>
            [EnumMember]
            IssuedWithEvents = 14,
            /// <summary>
            /// El proceso de cargue con riesgos vigentes
            /// </summary>
            [EnumMember]
            RiksCurrent = 15
        }

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
            Temporary = 4
        }

        [Flags]
        public enum ReportType
        {
            [EnumMember]
            CompletePolicy = 1,
            [EnumMember]
            OnlyPolicy = 2,
            [EnumMember]
            PaymentConvention = 3,
            [EnumMember]
            Temporary = 4,
            [EnumMember]
            Quotation = 5,
            [EnumMember]
            CompleteRequest = 6,
            [EnumMember]
            OnlyRequest = 7,
            [EnumMember]
            OnlyRequestPolicies = 8,
            [EnumMember]
            FormatCollect = 9,
            [EnumMember]
            MassLoad = 10,
            [EnumMember]
            License = 11,
            [EnumMember]
            LicenseBlank = 12,
            [EnumMember]
            MassIverenwal = 13,
            [EnumMember]
            PromissoryNote = 14,
            [EnumMember]
            OnlyPromissoryNote = 15
        }

        [Flags]
        public enum SenderPage
        {
            [EnumMember]
            MigratedPrinter = 1,
            [EnumMember]
            PolicyPrinter = 2,
            [EnumMember]
            QuotationPrinter = 3,
            [EnumMember]
            RequestPrinter = 4,
            [EnumMember]
            TempPrinter = 5,
            [EnumMember]
            MassLoadPrinter = 6
        }

        [Flags]
        public enum PendingPrintStatus
        {
            [EnumMember]
            PendingPrint = 1,
            [EnumMember]
            Printed = 2
        }

        [Flags]
        public enum PrefixCodeCPT
        {
            [EnumMember]
            Caucion = 31
        }

        [Flags]
        public enum DocumentTypes
        {
            [EnumMember]
            CC = 1,
            [EnumMember]
            NIT = 2,
            [EnumMember]
            CE = 3,
            [EnumMember]
            TI = 4,
            [EnumMember]
            Others = 5
        }

        [Flags]
        public enum PrinterRateType
        {
            [EnumMember]
            FixedValue = 3,
            [EnumMember]
            Percentage = 1,
            [EnumMember]
            Permilage = 2
        }

        [Flags]
        public enum DetailClass
        {
            [EnumMember]
            Accessory = 1,
            [EnumMember]
            Description = 2
        }

        [Flags]
        public enum CoveredRiskType
        {
            [EnumMember]
            Aircraft = 9,
            [EnumMember]
            Crop = 6,
            [EnumMember]
            DriverLicense = 3,
            [EnumMember]
            Location = 2,
            [EnumMember]
            Surety = 7,
            [EnumMember]
            Transport = 8,
            [EnumMember]
            Vehicle = 1,
            [EnumMember]
            VehicleAgency = 4,
            [EnumMember]
            VehicleTrip = 5,
        }

        [Flags]
        public enum MassiveLoadType
        {
            [EnumMember]
            Individual = 1,
            [EnumMember]
            Colective = 2,
        }

    }
}
