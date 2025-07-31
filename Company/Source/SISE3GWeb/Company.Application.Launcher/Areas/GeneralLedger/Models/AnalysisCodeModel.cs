using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AnalysisCodeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AnalysisCodeId { get; set; }

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
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public int AccountingNature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccountingNatureDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "RequireOrigin")]
        public bool RequireOrigin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequireOriginDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "ControlBalance")]
        public bool ControlBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ControlBalanceDescription { get; set; }
    }
}