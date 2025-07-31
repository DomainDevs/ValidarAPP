#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Modelo utilizado para el Concepto de Análisis
    ///     Implementación inicial aún no definida hasta que se verifique el uso del ABM.
    /// </summary>
    [DataContract]
    public class AnalysisConceptDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Codigo de Concepto
        /// </summary>
        [DataMember]
        public AnalysisCodeDTO AnalysisCode { get; set; }

        /// <summary>
        ///     Tratamiento de Analisis
        /// </summary>
        [DataMember]
        public AnalysisTreatmentDTO AnalysisTreatment { get; set; }
    }
}