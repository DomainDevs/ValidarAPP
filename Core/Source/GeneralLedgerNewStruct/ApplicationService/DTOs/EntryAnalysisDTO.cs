using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     DTO para listado de análisis por asiento
    /// </summary>
    [DataContract]
    public class EntryAnalysisDTO 
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int EntryAnalysisId { get; set; }

        /// <summary>
        ///     Id de análisis
        /// </summary>
        [DataMember]
        public int AnalysisId { get; set; }

        /// <summary>
        ///     Descripción de análisis
        /// </summary>
        [DataMember]
        public string AnalysisDescription { get; set; }

        /// <summary>
        ///     Id de concepto de análisis
        /// </summary>
        [DataMember]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        ///     Descripción de análisis de concepto
        /// </summary>
        [DataMember]
        public string AnalysisConceptDescription { get; set; }

        /// <summary>
        ///     Id de asiento
        /// </summary>
        [DataMember]
        public int EntryId { get; set; }

        /// <summary>
        ///     Llave de concepto
        /// </summary>
        [DataMember]
        public string ConceptKey { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string EntryAnalysisDescription { get; set; }
    }
}