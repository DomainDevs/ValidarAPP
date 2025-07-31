using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class ModuleModel
    {
        [Display(ResourceType = typeof(Global), Name = "SystemModules")]
        public int ModuleCd { get; set; }


        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string ModuleDescription { get; set; }
    }
}