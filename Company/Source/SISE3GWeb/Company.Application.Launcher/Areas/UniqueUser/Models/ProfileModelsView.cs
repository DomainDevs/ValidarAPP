using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class ProfileModelsView
    {
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
        /// Enabled
        /// </summary>
        public bool Static { get; set; }

        /// <summary>
        /// Descripction
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool HasAccess { get; set; }

        /// <summary>
        /// Accesses asignados
        /// </summary>
        public List<ProfileAccessView> profileAccesses { get; set; }

        /// <summary>
        /// Accesses asignados
        /// </summary>
        public List<GuaranteeStatusModelsView> guaranteeStatus { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string EnabledDescription { get; set; }

        /// <summary>
        /// tipo de acceso 
        /// </summary>
        public int  AccessType { get; set; }

    }
}