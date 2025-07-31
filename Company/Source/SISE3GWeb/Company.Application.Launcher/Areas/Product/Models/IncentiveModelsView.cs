using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class IncentiveModelsView
    {
        /// <summary>
        /// Id Incentivo
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// Valor del Incentivo
        /// </summary>
        public decimal Incentive_Amt { get; set; }

    }
}