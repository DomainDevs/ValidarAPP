using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingMovementTypeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string AccountingMovementTypeDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Automatic")]
        public bool IsAutomatic { get; set; }
    }
}