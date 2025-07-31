using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de Ramo Comercial
    /// </summary>
    public class BusinessBranchViewModel
    {


        /// <summary>
        /// Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefix", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, 999, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileLengthMil")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public int IdPrefix { get; set; }
        
        /// <summary>
        /// Nombre ramo Comercial
        /// </summary>
        [Display(Name = "LabelNameofbranchCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string BranchCommercialName { get; set; }

        /// <summary>
        /// Descripción Larga
        /// </summary>
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }


        /// <summary>
        /// Descripción Corta
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary>
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(3)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string TinyDescription { get; set; }

        [Display(Name = "LabelPrefixType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixTypeCode { get; set; }

        [Display(Name = "LabelTechnicalBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int LineBusinessId { get; set; }

        public PrefixType PrefixType { get; set; }

        public List<LineBusinessViewModel> LineBusiness { get; set; }

        public List<PrefixLineBusiness> PrefixLineBusiness { get; set; }

        public string Status { get; set; }
    }
}