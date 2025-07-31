using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs
{
    [DataContract]
    public class RiskDTO
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
        public List<CoverageDTO> Coverages { get; set; }
    }
}
