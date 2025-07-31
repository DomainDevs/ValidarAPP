using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    public class ScriptViewModel
    {
        [Required]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelScriptName")]
        public string Name { get; set; }

        [Required]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelPackages")]
        public int Package { get; set; }

        [Required]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LabelLevels")]
        public int Level { get; set; }
    }
}