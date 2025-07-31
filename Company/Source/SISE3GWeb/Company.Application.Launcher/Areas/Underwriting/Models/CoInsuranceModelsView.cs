using System.ComponentModel.DataAnnotations;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class CoInsuranceModelsView
    {
        /// <summary>
        /// Id temporal
        /// </summary>
        public int TemporalId { get; set; }

        /// <summary>
        /// Tipo de negocio
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelBusinessType", ResourceType = typeof(App_GlobalResources.Language))]
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// Porcentaje de participacion
        /// </summary>
        [Range(typeof(decimal), "1", "99,99", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelOwnParticipationRate", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AcceptedParticipationPercentageOwn { get; set; }

        /// <summary>
        /// Id aseguradora lider
        /// </summary>
        [Display(Name = "LabelLeadingInsurer", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? AcceptedCoinsurerId { get; set; }

        /// <summary>
        /// Aseguradora lider
        /// </summary>
        [Display(Name = "LabelLeadingInsurer", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100)]
        public string AcceptedCoinsurerName { get; set; }

        /// <summary>
        /// Porcentaje de participacion
        /// </summary>
        [Range(typeof(decimal), "1","99,99", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelTableParticipation", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AcceptedParticipationPercentage { get; set; }

        /// <summary>
        /// Porcentaje de gastos
        /// </summary>
        //[Range(1,100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelExpenses", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AcceptedExpensesPercentage { get; set; }

        /// <summary>
        /// Poliza lider
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelLeadingPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(20)]
        public string AcceptedPolicyNumber { get; set; }

        /// <summary>
        /// Numero de endoso
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelEndorsement", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(20)]
        public string AcceptedEndorsementNumber { get; set; }

        /// <summary>
        /// Porcentaje de participacion
        /// </summary>
        [Range(typeof(decimal),"1", "99,99", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelOwnParticipationRate", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AssignedParticipationPercentageOwn { get; set; }

        /// <summary>
        /// Id aseguradora lider
        /// </summary>
        [Display(Name = "LabelLeadingInsurer", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? AssignedCoinsurerId { get; set; }

        /// <summary>
        /// Aseguradora lider
        /// </summary>
        [Display(Name = "LabelLeadingInsurer", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100)]
        public string AssignedCoinsurerName { get; set; }

        /// <summary>
        /// Porcentaje de participacion
        /// </summary>
        [Range(typeof(decimal),"1","99,99", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelTableParticipation", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AssignedParticipationPercentage { get; set; }

        /// <summary>
        /// Porcentaje de gastos
        /// </summary>
        [Range(0, 100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelExpenses", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AssignedExpensesPercentage { get; set; }

        /// <summary>
        /// Porcentaje de participacion del agente
        /// </summary>
        [Range(typeof(decimal), "1", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelParticipation", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AssignedAgentPercentage { get; set; }

        /// <summary>
        /// Agente a seleccionar
        /// </summary>
        [Display(Name = "LabelLeadingInsurer", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(100)]
        public string AcceptedAgentName { get; set; }
    }
}