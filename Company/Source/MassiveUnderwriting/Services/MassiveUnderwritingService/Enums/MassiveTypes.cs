using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveUnderwritingServices.Enums
{
    public class MassiveTypes
    {
        [Flags]
        public enum MassiveState
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
        public enum MassiveLoadType
        {
            [EnumMember]
            Individual = 1,
            [EnumMember]
            Colective = 2,
        }

    }
}
