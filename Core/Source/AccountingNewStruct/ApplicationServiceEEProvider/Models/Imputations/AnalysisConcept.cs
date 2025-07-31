namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class AnalysisConcept
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int AnalysisConceptId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Codigo de Concepto
        /// </summary>
        public AnalysisCode AnalysisCode { get; set; }

        /// <summary>
        /// Tratamiento de Analisis
        /// </summary>
        public AnalysisTreatment AnalysisTreatment { get; set; }
    }
}
