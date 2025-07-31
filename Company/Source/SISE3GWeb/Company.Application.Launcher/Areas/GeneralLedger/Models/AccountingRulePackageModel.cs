using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingRulePackageModel
    {
        [Display(ResourceType = typeof(Global), Name = "Id")]
        public int AccountingRulePackageId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Module")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string AccountingRulePackageDescription { get; set; }
        
        [Display(ResourceType = typeof(Global), Name = "Concepts")]
        public List<AccountingRuleModel> AccountingRules { get; set; }

        public int RulePackageId { get; set; }
    }
}