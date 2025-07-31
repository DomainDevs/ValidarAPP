using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class BinnacleViewModel
    {
        /// <summary>
        /// Id del afianzado
        /// </summary>
        public int ContractorId { get; set; }

        /// <summary>
        /// Nombre afianzado
        /// </summary>
        public string SecureName { get; set; }

        /// <summary>
        /// Número del documento (pagaré, CDT, escritura, etc.) del afianzado.
        /// </summary>
        public int NumberDocument { get; set; }

        /// <summary>
        /// id garantia
        /// </summary>
        public int GuaranteeId { get; set; }

        /// <summary>
        /// id status garantia
        /// </summary>
        public int GuaranteeStatusCode { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Display(Name = "LabelObservations", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Observation { get; set; }
    }
}