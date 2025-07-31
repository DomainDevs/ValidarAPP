namespace Sistran.Core.Framework.UIF.Web.Areas.Externals.Models
{
    using Sistran.Company.Application.ModelServices.Enums;
    using System.ComponentModel.DataAnnotations;

    public class QuerySISAViewModel
    {
        [Display(Name = "LabelPlate", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(6, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //[RegularExpression(@"[A-Z]+[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string Plate { get; set; }

        [Display(Name = "LabelEngine", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [RegularExpression(@"[A-Z0-9]+", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string Engine { get; set; }

        [Display(Name = "LabelChassis", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [RegularExpression(@"[A-Z0-9]+", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string Chassis { get; set; }

        public StatusTypeService StatusTypeService { get; set; }
    }
}