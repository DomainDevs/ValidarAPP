using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class ReinsurancePaymentModel
    {
        public int PaymentReinsuranceId { get; set; }
        public int PaymentRequestId { get; set; }
        public int ReinsuranceNumber { get; set; }
        public int MovementTypeId { get; set; }
    }
}
