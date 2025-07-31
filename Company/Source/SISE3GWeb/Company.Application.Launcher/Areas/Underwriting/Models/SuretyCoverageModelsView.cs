using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class SuretyCoverageModelsView : CoverageModelsView
    {         

        /// <summary>
        /// Obtener o seterar el valor del contrato
        /// </summary>
        /// <value>
        /// Valor del contrato
        /// </value>
        public decimal ContractValue { get; set; }   
      
        /// <summary>
        /// Gets or sets the percentage contract.
        /// </summary>
        /// <value>
        /// The percentage contract.
        /// </value>
        [Range(0.1, 100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorRangeContractAmountPercentage")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorContractAmountPercentage")]
        public decimal ContractAmountPercentage { get; set; }

        public string PolicyFrom { get; set; }

        public string PolicyTo { get; set; }

    }
}