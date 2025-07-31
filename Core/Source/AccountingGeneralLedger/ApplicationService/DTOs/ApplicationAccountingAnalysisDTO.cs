using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class ApplicationAccountingAnalysisDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador del análisis
        /// </summary>
        [DataMember]
        public int AnalysisId { get; set; }

        /// <summary>
        /// Identificador del concepto de análisis
        /// </summary>
        [DataMember]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        /// Llave del concepto
        /// </summary>
        [DataMember]
        public string ConceptKey { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
