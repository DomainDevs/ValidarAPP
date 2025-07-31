namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationAccountingAnalysis
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public int AnalysisId { get; set; }
        ///// <summary>
        ///// AnalysisConceptId
        ///// </summary>
        //public int AnalysisConceptId { get; set; }
        /// <summary>
        /// ConceptKey
        /// </summary>
        public string ConceptKey { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        public AnalysisConcept AnalysisConcept { get; set; }
        
    }
}
