using Sistran.Core.Application.ModelServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class CommisionAgentModelsView
    {
        /// <summary>
        /// Id agente
        /// </summary>
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]     
        public int? AgentId { get; set; }

        /// <summary>
        /// Nombre agente
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
        /// Nombre Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgencyName { get; set; }           
     
        
        /// <summary>
        /// Porcentaje de Comision
        /// </summary>   
        [Range(typeof(decimal), "1", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelPercentageCommission", ResourceType = typeof(App_GlobalResources.Language))]       
        public decimal Percentage { get; set; }

        /// <summary>
        /// Porcentaje de Comision Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelCommisionAdditional", ResourceType = typeof(App_GlobalResources.Language))]      
        public decimal? PercentageAdditional { get; set; }


        /// <summary>
        /// Porcentaje de Comision
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelPercentageCommission", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? SchCommissPercentage { get; set; }

        /// <summary>
        /// Porcentaje de Comision Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelCommisionAdditional", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? StDisCommissPercentage { get; set; }

        /// <summary>
        /// Porcentaje de Comision Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelCommisionAdditional", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AdditDisCommissPercentage { get; set; }


        /// <summary>
        /// Codigo Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentCode { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public int ProductId { get; set; }

        public StatusTypeService Status { get; set; }

    }
}