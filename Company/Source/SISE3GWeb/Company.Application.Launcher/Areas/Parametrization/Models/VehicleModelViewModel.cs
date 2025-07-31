using Sistran.Core.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class VehicleModelViewModel
    {
        [Display(Name = "LabelIdModel", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 9999, ErrorMessage = "Debe ser mayor a 0 y menor a 10000 ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public int Id { get; set; }

        [Display(Name = "LabelLongDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionModel { get; set; }


        [Display(Name = "LabelSmallDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescriptionModel { get; set; }



        [Display(Name = "LabelMake", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int MakeId_Id { get; set; }
        public VehicleMakeViewModel VehicleMake { get; set; }

        public StatusTypeService StatusTypeService { get; set; }
    }
}