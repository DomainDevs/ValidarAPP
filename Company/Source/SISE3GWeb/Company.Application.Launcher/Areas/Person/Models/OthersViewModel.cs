using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class OthersViewModel
    {

        /// <summary>
        /// Sucursal
        /// </summary>
        public int Office { get; set; }

        /// <summary>
        /// Cerrada
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Id de la contragarantía
        /// </summary>
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Tipo de contragarantía
        /// </summary>
        public int GuaranteeTypeCode { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Display(Name = "LabelObservations", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(4000, ErrorMessageResourceName = "ErrorMaxlengthCode", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string Observations { get; set; }
    }
}