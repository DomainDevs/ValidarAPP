using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AnalysisTreatmentModel
    {
        /// <summary>
        /// Id de tratamiento de análisis
        /// </summary>
        public int AnalysisTreatmentId { get; set; }

        /// <summary>
        /// Descripción de tratamiento de análisis
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }
    }
}