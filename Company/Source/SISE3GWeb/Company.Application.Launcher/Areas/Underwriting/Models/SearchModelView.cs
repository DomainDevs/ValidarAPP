using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class SearchModelView
    {
        /// <summary>
        /// Id Sucursal
        /// </summary>
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BranchId { get; set; }

        /// <summary>
        /// Id Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixId { get; set; }

        /// <summary>
        /// Número de Poliza
        /// </summary>
        [Display(Name = "LabelNumberPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(12)]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>
        [Display(Name = "LabelEndorsement", ResourceType = typeof(App_GlobalResources.Language))]
        public int? EndorsementId { get; set; }

        /// <summary>
        /// Id Temporal
        /// </summary>
        [Display(Name = "LabelTemporal", ResourceType = typeof(App_GlobalResources.Language))]
        public int? TemporalId { get; set; }
    }
}