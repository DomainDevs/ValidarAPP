using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class AgentModelsView
    {
        /// <summary>
        /// Id Agente
        /// </summary>
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentId { get; set; }

        /// <summary>
        /// Nombre Agente
        /// </summary>
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgentName { get; set; }

        /// <summary>
        /// Id Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyId { get; set; }

        /// <summary>
        /// Id Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyCode { get; set; }

        /// <summary>
        /// Nombre Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgencyName { get; set; }

        /// <summary>
        /// Id Sucursal Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyBranchId { get; set; }
        
        /// <summary>
        /// Participación
        /// </summary>             
        [Display(Name = "LabelParticipation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(typeof(decimal), "0,01", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public decimal Participation { get; set; }
        
        /// <summary>
        /// Porcentaje
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelPercentageCommission", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal Percentage { get; set; }

        /// <summary>
        /// Porcentaje Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelCommisionAdditional", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? PercentageAdditional { get; set; }

        /// <summary>
        /// Es Principal?
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Es Principal?
        /// </summary>
        public int SalePointId { get; set; }
    }
}