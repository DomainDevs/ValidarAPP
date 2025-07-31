using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class AccessoryModelsView
    {        
        /// <summary>
        /// Identificador
        /// </summary>
        [Display(Name = "LabelAccessory", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentAccessory")]
        public int Id { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [Display(Name = "LabelAccessoryMake", ResourceType = typeof(App_GlobalResources.Language))]        
        [StringLength(50)]
        public string Make { get; set; }

        /// <summary>
        /// Es original?
        /// </summary>        
        public bool IsOriginal { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [Display(Name = "LabelSum", ResourceType = typeof(App_GlobalResources.Language))]        
        [RegularExpression(@"^\-?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string Amount { get; set; }

        /// <summary>
        /// Prima
        /// </summary>        
        public string Premium { get; set; }

        /// <summary>
        /// Valor de la Tasa
        /// </summary>  
        [DisplayFormat(DataFormatString = "{{{0:### ##}}}")]
        public decimal? Rate { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>  
        public int RateType { get; set; }

        /// <summary>
        /// Prima acumulada
        /// </summary>
        public decimal? AccumulatedPremium { get; set; }
    }
}