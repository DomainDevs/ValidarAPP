using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CheckingRejection
{
    [KnownType("CheckingRejectionModel")]
    public class CheckingRejectionModel
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int RejectionId { get; set; }
        public string RejectionDate { get; set; }
        public decimal Commission { get; set; }
        public decimal TaxCommission { get; set; }
        public string Description { get; set; }
    }


}