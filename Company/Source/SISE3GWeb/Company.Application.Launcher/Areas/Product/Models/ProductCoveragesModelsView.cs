using Sistran.Company.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductCoveragesModelsView
    {
        /// <summary>
        /// 
        /// </summary>
        public ProductCoverageModelsView Coverage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProductCoverageModelsView> CoverageAllied { get; set; }
    }
}