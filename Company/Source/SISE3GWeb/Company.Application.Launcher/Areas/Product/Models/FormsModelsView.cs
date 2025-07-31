using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class FormsModelsView
    {
        /// <summary>
        /// No. de Forma
        /// </summary>
        [Display(Name = "LabelFormNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string FormNumber { get; set; }

        /// <summary>
        /// Fecha de Inicio
        /// </summary>
        [Display(Name = "LabelCurrentDate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string CurrentFrom { get; set; }

        /// <summary>
        /// Id Grupo de Cobertura
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int CoverageGroupId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public int FormId { get; set; }
    }
}