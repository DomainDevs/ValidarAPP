using Sistran.Core.Framework.UIF.Web.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class CostCenterModel
    {
        public int Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        public bool IsProrated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "CostCenterType")]
        public int CostCenterTypeId { get; set; }
    }
}