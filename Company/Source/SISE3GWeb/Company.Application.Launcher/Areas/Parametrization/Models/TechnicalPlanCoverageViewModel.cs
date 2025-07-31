using Sistran.Core.Application.EntityServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class TechnicalPlanCoverageViewModel
    {
        [Display(Name = "LabelTechnicalPlanInsuredObject", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredObjectId { get; set; }

        public string InsuredObjectDescription { get; set; }

        [Display(Name = "LabelTechnicalPlanCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int CoverageId { get; set; }

        public string CoverageDescription { get; set; }

        [Display(Name = "LabelTechnicalPlanPrincipalCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrincipalCoverageId { get; set; }

        [Display(Name = "LabelTechnicalPlanPercentage", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal CoveragePercentage { get; set; }

        public List<TechnicalPlanAllyCoverageViewModel> AllyCoverages { get; set; }

        public StatusTypeService Status { get; set; }
    }
}