using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class RenewalViewModel
    {
        [Display(Name = "LabelProcess", ResourceType = typeof(App_GlobalResources.Language))]
        public int ProcessId { get; set; }

        [Required]
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        public int PrefixId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "DueDateFrom", ResourceType = typeof(App_GlobalResources.Language))]
        public string DueDateFrom { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "DueDateTo", ResourceType = typeof(App_GlobalResources.Language))]
        public string DueDateTo { get; set; }

        [Display(Name = "IntermediaryId", ResourceType = typeof(App_GlobalResources.Language))]
        public int? AgentId { get; set; }

        [Display(Name = "CodIntermediary", ResourceType = typeof(App_GlobalResources.Language))]
        public int? AgencyId { get; set; }

        [Display(Name = "Agent", ResourceType = typeof(App_GlobalResources.Language))]
        public string AgencyName { get; set; }

        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        public int? BranchId { get; set; }

        [Display(Name = "HolderId", ResourceType = typeof(App_GlobalResources.Language))]
        public int? HolderId { get; set; }

        [Display(Name = "LabelHolder", ResourceType = typeof(App_GlobalResources.Language))]
        public string HolderName { get; set; }

        [Display(Name = "LabelRequestGroup", ResourceType = typeof(App_GlobalResources.Language))]
        public int? RequestGroupId { get; set; }

        [Display(Name = "LabelRequestGroup", ResourceType = typeof(App_GlobalResources.Language))]
        public string RequestGroupDescription { get; set; }
    }
}