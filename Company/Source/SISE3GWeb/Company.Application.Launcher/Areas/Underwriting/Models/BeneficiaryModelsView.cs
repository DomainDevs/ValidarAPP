using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class BeneficiaryModelsView
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Display(Name = "LabelBeneficiary", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(130)]
        public string Name { get; set; }
        
        /// <summary>
        /// Tipo de Beneficiario
        /// </summary>
        [Display(Name = "LabelBeneficiaryType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BeneficiaryType { get; set; }

        /// <summary>
        /// Porcentaje de Participación
        /// </summary>
        [Display(Name = "LabelParticipation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(5, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorRangeValue")]
        [RegularExpression(@"(^(100(?:\,0{1,2})?))|(?!^0*$)(?!^0*\,0*$)^\d{1,2}(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorRangeValue")]
        public string Participation { get; set; }
    }
}