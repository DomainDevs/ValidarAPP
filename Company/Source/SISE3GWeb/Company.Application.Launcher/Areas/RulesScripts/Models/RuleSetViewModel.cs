using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    public class RuleSetViewModel
    {
        public int PackageId { get; set; }

        public string PackageDescription { get; set; }

        public int LevelId { get; set; }

        public string LevelDescription { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string RuleDescription { get; set; }

        public string RuleSetDescription { get; set; }

        public int RuleSetId { get; set; }

        public string SearchCombo { get; set; }

        public SearchComboViewMode SearchComboVM { get; set; }
    }
}
