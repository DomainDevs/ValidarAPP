using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class BillingGroupViewModel
    {
        /// <summary>
        /// Id Del grupo
        /// </summary>        
        public int? Id { get; set; }

        /// <summary>
        /// Descripcion 
        /// </summary>
        [Display(Name = "LabelDescriptionGroup", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(60, MinimumLength = 3)]
        public string Description { get; set; }
    }
}