using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class PaymentMovementTypeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int PaymentMovementTypeId { get; set; }

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
        [Display(ResourceType = typeof(Global), Name = "PaymentSource")]
        public int PaymentSourceCd { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "PaymentSource")]
        public string PaymentSourceDescription { get; set; }
    }
}