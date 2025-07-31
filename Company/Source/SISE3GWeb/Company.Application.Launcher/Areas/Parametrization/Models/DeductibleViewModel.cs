using Sistran.Core.Application.EntityServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class DeductibleViewModel 
    {
        public int DeductibleId { get; set; }

        [Display(Name = "LabelLineBusiness", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int LineBusinessId { get; set; }

        public string TotalDescription { get; set; }

        [Display(Name = "Value", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Value { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int UnitId { get; set; }

        [Display(Name = "ApplyOn", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ApplyOnId { get; set; }

        [Display(Name = "Min", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Min { get; set; }

        [Display(Name = "UnitMin", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int UnitMinId { get; set; }

        [Display(Name = "ApplyOnMin", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ApplyOnMinId { get; set; }

        [Display(Name = "Max", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Max { get; set; }

        [Display(Name = "UnitMax", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? UnitMaxId { get; set; }

        [Display(Name = "ApplyOnMax", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? ApplyOnMaxId { get; set; }
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Type { get; set; }
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Rate { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
        public string LineDescription { get; set; }
        public string UnitDescription { get; set; }
        public string UnitMinDescription { get; set; }
        public string UnitMaxDescription { get; set; }
        public string DeductibleSubjectDescription { get; set; }
        public string ApplyOnMinDescription { get; set; }
        public string ApplyOnMaxDescription { get; set; }
        public string TypeDescription { get; set; }
        public StatusTypeService Status { get; set; }
    }
}