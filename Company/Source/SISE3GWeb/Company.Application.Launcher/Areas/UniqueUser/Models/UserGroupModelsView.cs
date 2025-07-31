using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class UserGroupModelsView
    {
        /// <summary>
        /// Id Group
        /// </summary>       
        public int IdGroup { get; set; }

        /// <summary>
        /// Description Group
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Check User Group
        /// </summary>       
        public bool Check { get; set; }        
    }
}