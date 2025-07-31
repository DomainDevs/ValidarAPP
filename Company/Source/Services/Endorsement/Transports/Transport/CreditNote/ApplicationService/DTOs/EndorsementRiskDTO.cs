using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs
{
    [DataContract]
    public class EndorsementRiskDTO
    {
        /// <summary>
        /// Identificador del riesgo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Coberturas asociadas al riesgo
        /// </summary>
        [DataMember]
        public List<EndorsementCoverageDTO> Coverages { get; set; }
    }
}
