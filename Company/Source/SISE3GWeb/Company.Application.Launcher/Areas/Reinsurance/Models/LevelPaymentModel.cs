using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class LevelPaymentModel
    {

        public int LevelPaymentId { get; set; }

        [Required]
        public int LevelId { get; set; }
        
        [Required]
        public int LevelPaymentNumber { get; set; }
              

        public decimal PaymentAmount { get; set; }


        [DataType(DataType.Date)]
        public string PaymentDate { get; set; }

    }
}