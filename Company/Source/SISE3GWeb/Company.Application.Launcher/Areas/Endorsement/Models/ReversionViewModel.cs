using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ReversionViewModel : EndorsementViewModel
    {
        [Display(Name = "Nroregistration", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? Nroregistration { get; set; }
        
    }
}