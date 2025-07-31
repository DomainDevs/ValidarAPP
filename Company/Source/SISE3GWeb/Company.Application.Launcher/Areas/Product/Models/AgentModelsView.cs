using Sistran.Core.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class AgentModelsView
    {
        /// <summary>
        /// Id agente
        /// </summary>
        [Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre agente
        /// </summary>
        [Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string FullName { get; set; }

        /// <summary>
        /// Codigo Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentCode { get; set; }

        public List<CommisionAgentModelsView> ProductAgencyCommiss { get; set; }

        //public IncentivesForAgentsModelsView IncentivesForAgents { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>       
        public int ProductId { get; set; }

        public StatusTypeService Status { get; set; }
    }
}