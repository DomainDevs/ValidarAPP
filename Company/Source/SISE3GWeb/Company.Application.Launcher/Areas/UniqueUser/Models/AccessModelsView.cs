using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class AccessModelsView
    {
        /// <summary>
        /// Id
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// AccessObjectId
        /// </summary>      
        public int AccessObjectId { get; set; }

        /// <summary>
        /// AccessObjectId
        /// </summary>   
        public int ParentAccessTypeId { get; set; }

        /// <summary>
        /// ModuleId
        /// </summary>      
        
        public int ModuleId { get; set; }

        /// <summary>
        /// Descripción modulo
        /// </summary>      
        public string ModuleDescription { get; set; }

        /// <summary>
        /// SubModuleId
        /// </summary>  
         
        public int SubModuleId { get; set; }

        /// <summary>
        /// Descripción modulo
        /// </summary>      
        public string SubModuleDescription { get; set; }

        /// <summary>
        /// AccessTypeId
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AccessTypeId { get; set; }

        // <summary>
        /// AccessTypeDescription
        /// </summary>      
        public string AccessTypeDescription { get; set; }

        /// <summary>
        /// Path
        /// </summary>    
        [Display(Name = "Path", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(256)]
        public string Path { get; set; }

        /// <summary>
        /// Descripction
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(30)]
        public string Description { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string EnabledDescription { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ParentAccessId
        /// </summary>      
        public int ParentAccessId { get; set; }
    }
}