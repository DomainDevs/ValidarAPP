using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingRuleModel
    {
        [Display(ResourceType = typeof(Global), Name = "Module")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Concept")]
        public int AccountingRuleId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string AccountingRuleDescription { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Observations")]
        public string AccountingRuleObservations { get; set; }
    }
}