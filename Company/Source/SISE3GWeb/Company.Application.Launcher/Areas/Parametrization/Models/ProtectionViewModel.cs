using Sistran.Core.Application.EntityServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class ProtectionViewModel
    {
        /// <summary>
        /// Descripcion Larga
        /// </summary>
        [Display(Name = "LabelLongDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionLong { get; set; }

        /// <summary>
        /// Descripcion Corta
        /// </summary>
        [Display(Name = "LabelShortDescription",ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionShort { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        public StatusTypeService Status { get; set; }

    }
}