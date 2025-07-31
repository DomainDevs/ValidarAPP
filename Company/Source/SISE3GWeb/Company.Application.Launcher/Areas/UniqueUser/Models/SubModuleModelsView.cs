using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class SubModuleModelsView
    {
        /// <summary>
        /// Descripction
        /// </summary>
        [Display(Name = "DescriptionSubModule", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(30)]
        public string Description { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Descripction
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// ModuleId
        /// </summary>      
        public int ModuleId { get; set; }
        
        /// <summary>
        /// Descripction
        /// </summary>      
        public string ModuleDescription { get; set; }

        ///// <summary>
        ///// VirtualFolder
        ///// </summary>
        //public string VirtualFolder { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string EnabledDescription { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
    }
}