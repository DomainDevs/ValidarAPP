using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ChangeTextViewModel
    {
        /// <summary>
        /// Fecha de emision
        /// </summary>
        public string IssueDate { get; set; }

        /// <summary>
        /// Vigencia desde
        /// </summary>
        public string CurrentFrom { get; set; }

        /// <summary>
        /// Vigencia hasta
        /// </summary>
        public string CurrentTo { get; set; }

        /// <summary>
        /// Numero de Radicación
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "No es un valor valido para número de radicación.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No es un valor valido para número de radicación")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorFilingNumber")]
        public int? TicketNumber { get; set; }

        /// <summary>
        /// Fecha Radicación
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorFilingDate")]
        public string TicketDate { get; set; }

        /// <summary>
        /// Texto Precatalogado
        /// </summary>      
        public string TextPrecataloged { get; set; }

        /// <summary>
        /// Texto
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        public string Observations { get; set; }


        /// <summary>
        /// Dias
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>
        [Display(Name = "LabelEndorsement", ResourceType = typeof(App_GlobalResources.Language))]
        public int? EndorsementId { get; set; }

        /// <summary>
        /// Tipo de Negocio de la poliza
        /// </summary>
        public int BusinessTypeDescription { get; set; }
    }
}