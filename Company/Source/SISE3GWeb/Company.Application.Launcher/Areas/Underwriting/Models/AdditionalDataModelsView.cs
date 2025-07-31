using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class AdditionalDataModelsView
    {
        /// <summary>
        /// Id Asegurado
        /// </summary>        
        [Display(Name = "LabelInsuredSecondary", ResourceType = typeof(App_GlobalResources.Language))]
        public int? InsuredId { get; set; }

        /// <summary>
        /// Nombre Asegurado
        /// </summary>
        [Display(Name = "LabelInsuredSecondary", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        public string InsuredName { get; set; }

        /// <summary>
        /// Tipo combustible
        /// </summary>
        [Display(Name = "LabelFuelType", ResourceType = typeof(App_GlobalResources.Language))]
        public int FuelType { get; set; }

        /// <summary>
        /// Tipo carrocería
        /// </summary>
        [Display(Name = "LabelBodyType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BodyType { get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        [Display(Name = "LabelVehiclePrice", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string NewPrice { get; set; }


        /// <summary>
        /// Fecha Nacimiento Hijo Mayor
        /// </summary>
        [Display(Name = "LabelDateEldestSon", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime DateEldestSon { get; set; }

        /// <summary>
        /// Tiene Siniestro
        /// </summary>
        [Display(Name = "LabelHasSinister", ResourceType = typeof(App_GlobalResources.Language))]
        public Boolean HasSinister { get; set; }
    }
}