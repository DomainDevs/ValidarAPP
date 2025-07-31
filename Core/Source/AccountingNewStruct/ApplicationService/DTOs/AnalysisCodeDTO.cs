using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Código de análisis
    /// </summary>
    [DataContract]
    public class AnalysisCodeDTO
    {
        /// <summary>
        /// Código de análisis
        /// </summary>
        [DataMember]
        public int AnalysisCodeId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Listado de conceptos de análisis
        /// </summary>
        [DataMember]
        public List<AnalysisConceptDTO> AnalisisConcepts { get; set; }
    }
}
