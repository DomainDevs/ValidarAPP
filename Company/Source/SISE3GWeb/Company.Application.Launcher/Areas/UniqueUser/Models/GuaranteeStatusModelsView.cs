using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class GuaranteeStatusModelsView
    {
        /// <summary>
        /// Id 
        /// </summary>      
        public int Id { get; set; }
        /// <summary>
        /// Id 
        /// </summary>      
        public int IdGuaranteeStatus { get; set; }
        
        /// <summary>
        /// PerfilId 
        /// </summary>      
        public int ProfileId { get; set; }

        /// <summary>
        /// StatusId 
        /// </summary>      
        public int StatusId { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>      
        public bool Enabled { get; set; }
    }
}