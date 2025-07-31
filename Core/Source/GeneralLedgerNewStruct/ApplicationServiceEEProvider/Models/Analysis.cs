namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Modelo que representa el análisis contable
    /// </summary>
    public class Analysis
    {
        /// <summary>
        ///     Codigo
        /// </summary>
        public int AnalysisId { get; set; }

        /// <summary>
        /// </summary>
        public AnalysisConcept AnalysisConcept { get; set; }

        /// <summary>
        ///     Clave del Análisis
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Row id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Row parent id
        /// </summary>
        public int EntryItemId { get; set; }

        /// <summary>
        /// Indicates journal entry
        /// </summary>
        public bool IsJournalEntry { get; set; }
    }
}