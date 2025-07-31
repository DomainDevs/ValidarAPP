using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Endorsement.CreditNoteBusinessService.Models
{
    [DataContract]
    public class CreditNote
    {
        /// <summary>
        /// Identificador del temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Listado de riesgos asociados al endos
        /// </summary>
        [DataMember]
        public CompanyRisk[] Risks { get; set; }

        /// <summary>
        /// Texto
        /// </summary>
        [DataMember]
        public string Texts { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// Fecha de inicio de la vigencia
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha de finalización de la vigencia
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Información de resumen
        /// </summary>
        [DataMember]
        public CompanySummary Summary { get; set; }
    }
}
