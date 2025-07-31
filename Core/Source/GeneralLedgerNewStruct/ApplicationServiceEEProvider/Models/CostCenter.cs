namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Modelo que representa los Centros de Costos
    /// </summary>
    public class CostCenter
    {
        /// <summary>
        ///     Codigo
        /// </summary>
        public int CostCenterId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Prorrateado
        /// </summary>
        public bool IsProrated { get; set; }

        /// <summary>
        ///     Tipo
        /// </summary>
        public CostCenterType CostCenterType { get; set; }

        /// <summary>
        ///     Porcentaje
        /// </summary>
        public decimal PercentageAmount { get; set; }

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