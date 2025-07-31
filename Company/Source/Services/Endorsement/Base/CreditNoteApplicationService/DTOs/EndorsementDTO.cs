using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs
{
    [DataContract]
    public class EndorsementDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Fecha de finalización de vigencia
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Fecha de inicio de la vigencia
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Días entre las fechas de vigencia
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        [DataMember]
        public decimal PolicyNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IdEndorsement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public EndorsementType? EndorsementType { get; set; }
    }
}
