using Sistran.Core.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class AgentDataTableView
    {
        /// <summary>
        /// Id agente
        /// </summary>
        [Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IndividualId { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>       
        public int ProductId { get; set; }

        /// <summary>
        /// Nombre agente
        /// </summary>
        [Display(Name = "LabelAgent", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string FullName { get; set; }

        public List<ProductAgencyCommissData> AgencyComiss { get; set; }

        /// <summary>
        /// Codigo Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public AgentTypeData AgentType { get; set; }
        public StatusTypeService StatusTypeService { get; set; }
        public int LockerId { get; set; }
        public string CommisionText { get; set; }
        public string DataItem { get; set; }
    }
    public class AgentTypeData
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string SmallDescription { get; set; }
    }
    public class ProductAgencyCommissData
    {
        public int ProductId { get; set; }
        public string AgencyName { get; set; }
        public AgentTypeData AgentType { get; set; }
        public int Code { get; set; }
        public string PercentageAdditional { get; set; }
        public string Percentage { get; set; }
        public int AgentCode { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
    }
}