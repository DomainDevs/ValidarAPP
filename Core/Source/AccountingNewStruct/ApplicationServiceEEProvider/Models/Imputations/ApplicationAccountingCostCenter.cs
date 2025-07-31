namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationAccountingCostCenter
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Centro de costos 
        /// </summary>
        public CostCenter CostCenter { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        public decimal Percentage { get; set; }
    }
}
