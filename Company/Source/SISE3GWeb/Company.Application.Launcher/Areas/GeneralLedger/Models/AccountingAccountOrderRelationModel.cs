using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingAccountOrderRelationModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AccountingAccountOrderRelationId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int AccountingAccountOrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccountOrderDescription")]
        public string AccountingAccountOrderDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccountingAccountRelationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccountOrderDescription")]
        public string AccountingAccountRelationDescription { get; set; }
    }
}