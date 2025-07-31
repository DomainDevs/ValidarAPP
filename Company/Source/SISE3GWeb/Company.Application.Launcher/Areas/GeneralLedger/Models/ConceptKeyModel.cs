using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class ConceptKeyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ConceptKeyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Concepto de Análisis")]
        public int AnalysisConceptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AnalysisConceptDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Tabla")]
        public string Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Columna")]
        public string Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Descripción de Columna")]
        public string ColumnDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Query { get; set; }
    }
}