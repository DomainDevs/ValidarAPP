using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class IncentivesForAgentsModelsView
    {
        /// <summary>
        /// Id agente
        /// </summary>
        //[Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre agente
        /// </summary>
        //[Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string FullName { get; set; }

        /// <summary>
        /// Codigo Agencia
        /// </summary>
        //[Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentCode { get; set; }

        /// <summary>
        /// Id Agencia
        /// </summary>
        //[Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyId { get; set; }

        ///// <summary>
        ///// Listado de Incntivos por intermediario
        ///// </summary>        
        //public List<Incentive> ListIncentivesForAgents { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>       
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        public int ProductId { get; set; }

        /// <summary>
        ///Valor del Incentivo
        /// </summary>   
        [Range(0, 9999999999999999.99)]
        [Display(Name = "LabelPercentageCommission", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal IncentiveValue { get; set; }

       
    }
}