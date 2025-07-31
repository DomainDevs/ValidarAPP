using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class AccessTypeModelsView
    {
        /// <summary>
        /// ModuleId
        /// </summary>      
        public int ModuleId { get; set; }

        /// <summary>
        /// SubModuleId
        /// </summary>      
        public int SubModuleId { get; set; }

        /// <summary>
        /// AccessType
        /// </summary>      
        public int AccessType { get; set; }

        /// <summary>
        /// ParentAccessTypeId
        /// </summary>      
        public int ParentAccessTypeId { get; set; }

    }
}