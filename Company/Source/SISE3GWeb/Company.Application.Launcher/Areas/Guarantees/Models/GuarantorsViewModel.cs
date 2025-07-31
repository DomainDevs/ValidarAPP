using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class GuarantorsViewModel
    {
        /// <summary>
        /// Nombre afianzado
        /// </summary>
        public string SecureName { get; set; }

        /// <summary>
        /// Número del pagaré
        /// </summary>
        public int NumberDocument { get; set; }

        /// <summary>
        /// Si es persona natural o jurídica
        /// </summary>
        public int PersonType { get; set; }

        /// <summary>
        /// Nombre o razón social
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelName", ResourceType = typeof(App_GlobalResources.Language))]
        public string Name { get; set; }

        /// <summary>
        /// Número de documento o NIT
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelBusinessNameOrDocumentNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(15)]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "Address", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(30)]
        public string Address { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        [Required]
        public int City { get; set; }

        /// <summary>
        /// Telefóno
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelPhone", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(15)]
        public int Phone { get; set; }

    }
}