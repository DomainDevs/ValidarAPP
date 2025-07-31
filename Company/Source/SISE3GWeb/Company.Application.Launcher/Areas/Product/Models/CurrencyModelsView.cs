using Sistran.Company.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class CurrencyModelsView
    {
        /// <summary>
        /// Identificador
        /// </summary>

        public int Id { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>

        public string Description { get; set; }


        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DecimalQuantity { get; set; }
    }
}