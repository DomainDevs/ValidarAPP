using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.ReportsFasecolda.Models
{
    public class SisaViewModel
    {
        /// <summary>
        /// Gets or sets Placa
        /// </summary>
        [StringLength(15)]
        [Display(Name = "LabelLicencesePlate", ResourceType = typeof(App_GlobalResources.Language))] 
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string DesPlate { get; set; }


        /// <summary>
        /// Gets or sets Motor
        /// </summary>
        [StringLength(50)]
        [Display(Name = "LabelEngine", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]

        public string labelEngine { get; set; }


        /// <summary>
        /// Gets or sets Chasis
        /// </summary>
        [StringLength(50)]
        [Display(Name = "LabelChassis", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string LabelChassis { get; set; }


        /// <summary>
        /// Gets or sets captcha
        /// </summary>
        [StringLength(10)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Captcha { get; set; }

    }
}