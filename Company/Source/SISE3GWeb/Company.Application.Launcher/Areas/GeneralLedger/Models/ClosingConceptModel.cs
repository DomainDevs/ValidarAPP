using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class ClosingConceptModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ClosingConceptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Module")]
        public int ModuleDateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModuleDateDescription { get; set; }
    }
}