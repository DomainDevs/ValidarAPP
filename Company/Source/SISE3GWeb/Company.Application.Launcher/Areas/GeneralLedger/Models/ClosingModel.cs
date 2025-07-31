using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class ClosingModel
    {
        /// <summary>
        /// Id del tipo de Ejercicio de Cierre
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "ClosingExercise")]
        public int ClosingTypeId { get; set; }

        /// <summary>
        /// Id del tipo de Ejercicio de Cierre
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "ClosingExercise")]
        public string Description { get; set; }

        /// <summary>
        /// Año del ejercicio de cierre
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Year")]
        public int Year { get; set; }

        /// <summary>
        /// Mes para el cierre de ingresos y egresos
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Month")]
        public int Month { get; set; }
    }
}