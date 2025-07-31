namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class CostCenter
    {
        /// <summary>
        /// Código
        /// </summary>
        public int CostCenterId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indica si es prorrateado
        /// </summary>
        public bool IsProrated { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        public CostCenterType CostCenterType { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        public decimal PercentageAmount { get; set; }
    }
}
