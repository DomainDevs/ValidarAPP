using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class EndorsementViewModel
    {
        /// <summary>
        /// Id Sucursal
        /// </summary>
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        public int? BranchId { get; set; }

        /// <summary>
        /// Id Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        public int? PrefixId { get; set; }

        /// <summary>
        /// Número de Poliza
        /// </summary>
        [Display(Name = "LabelPolicyNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(12)]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Id póliza
        /// </summary>
        public int? PolicyId { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>
        [Display(Name = "LabelEndorsement", ResourceType = typeof(App_GlobalResources.Language))]
        public int? EndorsementId { get; set; }

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
        /// Id Motivo de modificacion
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Id Motivo de modificacion
        /// </summary>
        public int EndorsementReasonId { get; set; }

        /// <summary>
        /// Texto
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Titulo
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tipo de endoso
        /// </summary>
        public int EndorsementType { get; set; }

        /// <summary>
        /// Id temporal
        /// </summary>
        public int? TemporalId { get; set; }

        /// <summary>
        /// EndorsementControler
        /// </summary>
        public string EndorsementController { get; set; }

        /// <summary>
        /// Inicio del Endoso
        /// </summary>
        public string EndorsementFrom { get; set; }

        /// <summary>
        /// Fin del Endoso
        /// </summary>
        public string EndorsementTo { get; set; }

        /// <summary>
        /// Id Producto
        /// </summary>
        public int? PolicyType { get; set; }

        /// <summary>
        /// IsCollective
        /// </summary>
        public bool ProductIsCollective { get; set; }

        /// <summary>
        /// Numero de Radicación
        /// </summary>
        [Range(1,int.MaxValue, ErrorMessage = "No es un valor valido para número de radicación.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No es un valor valido para número de radicación")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorFilingNumber")]
        public int? TicketNumber { get; set; }
        /// <summary>
        /// Fecha Radicación
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorFilingDate")]
        public string TicketDate { get; set; }
        /// <summary>
        /// Es masivo?
        /// </summary>
        public bool IsMassive { get; set; }
        /// <summary>
        ///Endoso Actual
        /// </summary>
        public string EndorsementCurrent { get; set; }
        /// <summary>
        /// Periodos (Días) de ajuste y declaración
        /// </summary>
        public string EndorsementDays { get; set; }
        /// <summary>
        /// Tipo de Negocio de la poliza
        /// </summary>
        public int BusinessTypeDescription { get; set; }
        /// <summary>
        ///  Motivo de modificacion
        /// </summary>
        public string EndorsementReasonDescription { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; }

        public bool HasEvent { get; set; }
    }
}