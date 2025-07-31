using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class PrefixCumulusModel
    {
        public int PrefixCumulusId { get; set; }

        [Required]
        public int PrefixCD { get; set; }
        public string PrefixDescription { get; set; }

        [Required]
        public int PrefixCumulusCD { get; set; }
        public string PrefixCumulusDescription { get; set; }

        [Required]
        public int TypeExercice { get; set; }
        public string TypeExerciceDescripcion { get; set; }

        [Required]
        public bool Location { get; set; }
    }
}