using Sistran.Core.Application.EntityServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class TechnicalPlanViewModel
    {
        public int Id { get; set; }

        [Display(Name = "LabelTechnicalPlanDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ' \-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string Description { get; set; }

        [Display(Name = "LabelTechnicalPlanShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(15, MinimumLength = 3, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMinMaxLengthCharacter")]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ' \-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string ShortDescription { get; set; }

        [Display(Name = "LabelTechnicalPlanCoveredRiskType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]        
        public int RiskTypeId { get; set; }
        
        public string RiskTypeSmallDescription { get; set; }
        public System.DateTime CurrentFrom { get; set; }

        public System.DateTime CurrentTo { get; set; }

        public List<TechnicalPlanCoverageViewModel> Coverages { get; set; }

        public StatusTypeService Status { get; set; }

    }
}