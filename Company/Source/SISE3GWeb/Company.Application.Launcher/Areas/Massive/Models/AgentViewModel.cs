using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class AgentViewModel
    {
        /// <summary>
        /// Id agente
        /// </summary>
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? AgentId { get; set; }

        /// <summary>
        /// Nombre agente
        /// </summary>
        [Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgentName { get; set; }

        /// <summary>
        /// Id Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyId { get; set; }

        /// <summary>
        /// Nombre Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgencyName { get; set; }

        /// <summary>
        /// Sucursal agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyBranchId { get; set; }

        /// <summary>
        /// Porcentaje de participación
        /// </summary>             
        [Display(Name = "LabelParticipation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(typeof(decimal), "0,01", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public decimal Participation { get; set; }

        /// <summary>
        /// Id Ramo Técnico
        /// </summary>    
        [Display(Name = "LabelLineBusiness", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Id Sub Ramo técnico
        /// </summary>   
        [Display(Name = "LabelSubLineBusiness", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Es intermediario principal?
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Código Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentCode { get; set; }   
    }
}