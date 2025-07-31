namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    /// <summary>
    /// Creacion Planes de Pago
    /// </summary>
    public class ProductPaymentPlanModelsView
    {
        /// <summary>
        /// Identificador plan de pago
        /// </summary>        
        public int Id { get; set; }
        /// <summary>
        /// Identificador plan de pago
        /// </summary>        
        public CurrencyModelsView currencyModelsView { get; set; }
    }
}