using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class TechnicalSubBranchViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del largo del SubRamo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionLong", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion del corto del SubRamo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Nombre SubRamo Técnico
        /// </summary>
        [Display(Name = "NameTechnicalSubBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50)]
        public string NameTechnicalSubBranch { get; set; }

        public string LineBusinessDescription { get; set; }

        public int LineBusinessId { get; set; }

        public string Status { get; set; }

        public List<LineBusinessViewModel> LineBusiness { get; set; }
    }
}