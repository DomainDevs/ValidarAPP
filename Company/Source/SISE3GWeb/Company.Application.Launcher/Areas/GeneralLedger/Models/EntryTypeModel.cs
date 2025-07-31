using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class EntryTypeModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [ScaffoldColumn(false)]
        public int EntryTypeId { get; set; }

        /// <summary>
        /// Descripción Reducida
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "SmallDescription")]
        public string EntryTypeSmallDescription { get; set; }

        /// <summary>
        /// Descripción Extendida
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string EntryTypeDescription { get; set; }

        /// <summary>
        /// Cuentas Asociadas
        /// </summary>
        public List<EntryTypeAccountingModel> EntryTypeAccountingModels { get; set; }
    }
}