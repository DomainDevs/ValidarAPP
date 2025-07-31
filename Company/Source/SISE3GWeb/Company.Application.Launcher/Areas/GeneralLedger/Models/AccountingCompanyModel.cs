using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingCompanyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AccountingCompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Default")]
        public bool Default { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultDescription { get; set; }
    }
}