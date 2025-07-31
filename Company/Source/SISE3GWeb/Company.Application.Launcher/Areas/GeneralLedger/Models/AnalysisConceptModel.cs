using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AnalysisConceptModel
    {
        /// <summary>
        /// AnalysisConceptId
        /// </summary>
        public int AnalysisConceptId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// AnalysisTreatmentId
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AnalisysTreatment")]
        public int AnalysisTreatmentId { get; set; }

        /// <summary>
        /// AnalysisTreatmentDescription
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "AnalisysTreatment")]
        public string AnalysisTreatmentDescription { get; set; }
    }
}