using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class AdditionalDataTemporalModelsView
    {
        public bool CalculateMinimumPremium { get; set; }

        [Display(Name = "LabelCorrelationPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public int? CorrelativePolicyNumber { get; set; } //max 12


        [Display(Name = "LabelBusinessAdditionalData", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        public string Bussiness { get; set; }

        [Display(Name = "LabelQuotationGroup", ResourceType = typeof(App_GlobalResources.Language))]
        public int? GroupQuoteId { get; set; } // max 50
    }
}