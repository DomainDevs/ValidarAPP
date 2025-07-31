using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class TechnicalPlanSearchViewModel
    {
        public int Id { get; set; }

        [Display(Name = "LabelTechnicalPlanDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ' \-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string Description { get; set; }


        [Display(Name = "LabelTechnicalPlanShortDescription", ResourceType = typeof(App_GlobalResources.Language))]        
        [StringLength(15, MinimumLength = 3, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMinMaxLengthCharacter")]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ' \-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string ShortDescription { get; set; }

        public CoveredRiskTypeViewModel CoveredRiskType { get; set; }

        public DateTime CurrentFrom { get; set; }

        public DateTime CurrentTo { get; set; }
    }
}