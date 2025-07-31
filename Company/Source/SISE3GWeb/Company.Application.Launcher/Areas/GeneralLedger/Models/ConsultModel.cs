using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class ConsultModel
    {
        [Required]
        [Display(ResourceType = typeof(Global), Name = "SystemModules")]
        public int ModuleId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "TransactionNumber")]
        public string TransactionNumber { get; set; }
    }
}