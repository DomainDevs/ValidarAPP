using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class AgentModelsView
    {
        /// <summary>
        /// Nombre Agente
        /// </summary>
        [Display(Name = "Agent", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgentName { get; set; }

        /// <summary>
        /// Id Agencia
        /// </summary>       
        public int AgencyId { get; set; }


        /// <summary>
        /// AgentCode
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Id Relation
        /// </summary>
        public int IndividualRelationAppId { get; set; }

        /// <summary>
        /// Id Agente
        /// </summary>
        public int ChildIndividual { get; set; }


        /// <summary>
        /// Id Individual
        /// </summary>
        public int ParentIndividualId { get; set; }
        
        public string FullName { get; set; }

        public List<AgencyModelsView> Agencies { get; set; }

        public int IndividualId { get; set; }

    }
}