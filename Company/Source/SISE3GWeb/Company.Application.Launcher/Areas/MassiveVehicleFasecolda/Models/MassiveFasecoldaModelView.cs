using Sistran.Core.Framework.UIF.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.MassiveFasecoldaModelView.Models
{
    public class MassiveFasecoldaModelView
    {
        //[Required]
        [Display(Name = "LabelProcessType", ResourceType = typeof(App_GlobalResources.Language))]
        public int ProcessTypeId { get; set; }

        /// <summary>
        /// Nombre Cargue
        /// </summary>
        [Required]
        [Display(Name = "LabelNameLoad", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60, MinimumLength = 3)]
        public string LoadName { get; set; }

        /// <summary>
        /// Nombre Archivo
        /// </summary>
        [Required]
        [Display(Name = "LabelNameLoad", ResourceType = typeof(App_GlobalResources.Language))]
        public string FileName{ get; set; }
    }
}