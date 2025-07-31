using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class PaymentModel
    {
        public int PaymentRequestId { get; set; }
        public int PaymentSourceId { get; set; }
        public int BranchId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PersonType { get; set; }
        public int IndividualId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentRequestNumber { get; set; }
        public string Description { get; set; }
        public int PaymentMovementTypeId { get; set; }
        public int PrefixId { get; set; }
        public int ClaimId { get; set; }
        public int PaymentVoucherId { get; set; }
        public string PaymentVoucherNumber { get; set; }
    }
}
