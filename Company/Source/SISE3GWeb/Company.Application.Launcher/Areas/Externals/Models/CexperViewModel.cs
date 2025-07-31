using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Externals.Models
{
    public class CexperViewModel
    {
        /// <summary>
        /// Placa
        /// </summary>
        [Display(Name = "LabelLicensePlate", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(6, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //[RegularExpression(@"[A-Z]+[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Tipo de Documento
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Numero de Documentos
        /// </summary>
        [Display(Name = "LabelDocumentNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Obtiene o establece el Captcha
        /// </summary>
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Captcha")]
        [MaxLength(64, ErrorMessage = "Longitud del código exedida")]
        public string Captcha { get; set; }



    }
}