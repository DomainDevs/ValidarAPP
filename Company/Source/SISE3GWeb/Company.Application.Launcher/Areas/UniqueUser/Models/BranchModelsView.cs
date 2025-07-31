using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class BranchModelsView
    {
        /// <summary>
        /// Nombre Modulo
        /// </summary>
        [Display(Name = "LabelModule", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BranchId { get; set; }
        
        /// <summary>
        /// Nombre SubModulo
        /// </summary>
        [Display(Name = "LabelSubmodule", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int SalesPointId { get; set; }
        
        [Display(Name = "LabelDefault", ResourceType = typeof(App_GlobalResources.Language))]        
        public bool IsDefault { get; set; }

        [Display(Name = "LabelDefault", ResourceType = typeof(App_GlobalResources.Language))]        
        public bool DefaultBranch { get; set; }

    }
}