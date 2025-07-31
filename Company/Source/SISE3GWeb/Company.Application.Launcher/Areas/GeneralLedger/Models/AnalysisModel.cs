using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AnalysisModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Code")]
        public int AnalysisId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Code")]
        public string AnalysisDescription { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Concept")]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Concept")]
        public string AnalysisConceptDescription { get; set; }

        /// <summary>
        /// Clave del Análisis
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Key")]
        public string Key { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }
    }
}