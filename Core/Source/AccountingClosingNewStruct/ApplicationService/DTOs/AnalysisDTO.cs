using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    /// <summary>
    ///     Modelo que representa el an�lisis contable
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
        ///     Clave del An�lisis
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        ///     Descripci�n
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}