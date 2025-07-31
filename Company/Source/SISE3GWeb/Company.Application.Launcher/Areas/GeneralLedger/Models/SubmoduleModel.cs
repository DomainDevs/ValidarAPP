using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class SubmoduleModel
    {
        [Display(ResourceType = typeof(Global), Name = "SystemModules")]
        public int ModuleCd { get; set; }

        [Display(ResourceType = typeof(Global), Name = "SubModule")]
        public int SubmoduleCd { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string SubmoduleDescription { get; set; }
    }
}