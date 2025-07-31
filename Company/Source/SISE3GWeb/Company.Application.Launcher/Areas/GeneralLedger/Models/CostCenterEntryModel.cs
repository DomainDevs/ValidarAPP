using System;
using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class CostCenterEntryModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Code")]
        public int CostCenterId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Percentage")]
        public Decimal PercentageAmount { get; set; }
    }
}