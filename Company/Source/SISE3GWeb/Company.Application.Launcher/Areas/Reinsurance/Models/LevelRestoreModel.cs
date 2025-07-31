using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class LevelRestoreModel
    {

        public int LevelRestoreId { get; set; }

        [Required]
        public int LevelId { get; set; }
        
        [Required]
        public int LevelRestoreNumber { get; set; }

        public decimal RestorePercentage { get; set; }

        public decimal NoticePercentage { get; set; }


    }
}