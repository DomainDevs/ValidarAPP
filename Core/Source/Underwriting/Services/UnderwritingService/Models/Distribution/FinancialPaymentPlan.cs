using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.UnderwritingServices.Models.Distribution
{
    public class FinancialPaymentPlan
    {

        public ComponentValue ComponentValue { get; set; }        
        public DateTime IssueDate { get; set; }
        public DateTime CurrentFrom { get; set; }
        public bool IsGreaterDate { get; set; }
        public bool IsIssueDate { get; set; }
        public short CalculationType { get; set; }
        public short Quantity { get; set; }
        public List<PaymentDistributionPlan> PaymentDistribution { get; set; }
    }
}
