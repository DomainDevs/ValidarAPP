using Sistran.Core.Application.EntityServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class CoCoverageViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura de Impresion
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura  de Impresion es de asistencia
        /// </summary>
        public bool IsAssistance { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si  la cobertura de Impresion es de prima minima
        /// </summary>
        public bool IsAccMinPremium { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es de impresion
        /// </summary>
        public bool IsImpression { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de impresion de la cobertura
        /// </summary>
        [Display(Name = "LabelDescriptionCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        public string ImpressionNamePrint { get; set; }

        /// <summary>
        /// Obtiene o establece el valor asegurado a imprimir de la cobertura
        /// </summary>
        [Display(Name = "InsuredValuePrint", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        public string ImpressionValuePrint { get; set; }


        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService Status { get; set; }
    }
}