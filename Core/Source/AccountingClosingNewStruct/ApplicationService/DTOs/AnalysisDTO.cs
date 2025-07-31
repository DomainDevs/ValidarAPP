using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    /// <summary>
    ///     Modelo que representa el análisis contable
    /// </summary>
    [DataContract]
    public class AnalysisDTO
    {
        /// <summary>
        ///     Codigo
        /// </summary>
        [DataMember]
        public int AnalysisId { get; set; }

        /// <summary>
        /// </summary>
        [DataMember]
        public AnalysisConceptDTO AnalysisConcept { get; set; }

        /// <summary>
        ///     Clave del Análisis
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}