using Sistran.Core.Application.EntityServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class TechnicalPlanAllyCoverageViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de cobertura
        /// </summary>
        [Display(Name = "LabelTechnicalPlanDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        [Display(Name = "LabelTechnicalPlanPercentageSubLimit", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public decimal? CoveragePercentage { get; set; }

        public StatusTypeService Status { get; set; }
    }
}