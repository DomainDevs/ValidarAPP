using Sistran.Core.Framework.UIF.Web.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AnalysisCodesModel
    {
        public int Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public int AccountingNatureId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        public bool CheckBalance { get; set; }

        public bool CheckModule { get; set; }
    }
}