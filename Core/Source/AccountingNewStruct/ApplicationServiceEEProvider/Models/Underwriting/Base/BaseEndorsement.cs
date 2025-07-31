using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.Enums;
namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    /// <summary>
    /// Endoso Base
    /// </summary>
    [DataContract]
    public class BaseEndorsement : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Vigencia Inicial
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Vigencia Final
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Id Póliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Id Temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Id Cotización
        /// </summary>
        [DataMember]
        public int QuotationId { get; set; }

        /// <summary>
        /// Versión Cotización
        /// </summary>
        [DataMember]
        public int QuotationVersion { get; set; }
        /// <summary>
        /// Descripción del Tipo de Endoso
        /// </summary>
        [DataMember]
        public string EndorsementTypeDescription { get; set; }

        /// <summary>
        /// Id Motivo del Endoso
        /// </summary>
        [DataMember]
        public int EndorsementReasonId { get; set; }

        /// <summary>
        /// Id Endoso Anulado
        /// </summary>
        [DataMember]
        public int ReferenceEndorsementId { get; set; }

        /// <summary>
        /// Es actual?
        /// </summary>
        [DataMember]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Tipo de Cancelacion
        /// </summary>
        [DataMember]
        public int CancellationTypeId { get; set; }

        /// <summary>
        /// Renovación si es en identicas condiciones true
        /// en distintas condiciones false
        /// </summary>
        [DataMember]
        public bool IsUnderIdenticalConditions { get; set; }

        // <summary>
        /// Dias que aplican al endoso
        /// </summary>
        [DataMember]
        public int EndorsementDays { get; set; }

        /// <summary>
        /// Indicador de endoso generado a través de proceso masivo
        /// </summary>
        [DataMember]
        public bool? IsMassive { get; set; }

        /// <summary>
        /// Tipo de Endoso
        /// </summary>
        [DataMember]
        public Enums.EndorsementType? EndorsementType { get; set; }


        /// <summary>
        /// Motivo del Endoso
        /// </summary>
        [DataMember]
        public string EndorsementReasonDescription { get; set; }

    }
}
