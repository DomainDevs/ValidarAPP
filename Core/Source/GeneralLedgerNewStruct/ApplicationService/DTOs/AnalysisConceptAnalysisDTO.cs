using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     DTO para la relación de Análisis y Concepto de Análisis
    /// </summary>
    [DataContract]
    public class AnalysisConceptAnalysisDTO 
    {
        /// <summary>
        ///     Identificador de la tabla
        /// </summary>
        [DataMember]
        public int AnalysisConceptAnalysisId { get; set; }

        /// <summary>
        ///     Id de Análisis
        /// </summary>
        [DataMember]
        public int AnalysisId { get; set; }

        /// <summary>
        ///     Id de concepto de análisis
        /// </summary>
        [DataMember]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        ///     Descripción de concepto de análisis
        /// </summary>
        [DataMember]
        public string AnalysisConceptDescription { get; set; }
    }
}