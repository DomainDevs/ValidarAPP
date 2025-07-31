using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Concepto de análisis
    /// </summary>
    [DataContract]
    public class AnalysisConceptDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        /// Descripición
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
