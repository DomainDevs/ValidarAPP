using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class HierarchyModelsView
    {
        /// <summary>
        /// Nombre Modulo
        /// </summary>
        [Display(Name = "LabelModule", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ModuleId { get; set; }


        /// <summary>
        /// Nombre SubModulo
        /// </summary>
        [Display(Name = "LabelSubmodule", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int SubModuleId { get; set; }


        /// <summary>
        /// Nombre Jerarquia
        /// </summary>
        [Display(Name = "Hierarchy", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HierarchyId { get; set; }

    }
}