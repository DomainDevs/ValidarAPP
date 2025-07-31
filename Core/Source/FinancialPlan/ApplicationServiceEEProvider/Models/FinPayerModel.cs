using System;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Models
{
    public class FinPayerModel
    {
        public int endorsementId { get; set; }
        public int IndividualId { get; set; }
        public int SequentialId { get; set; }
        public int PaymentPlanId { get; set; }
        public int PaymentMethodId { get; set; }
        public int MailAddressId { get; set; }
        public DateTime AccountingDate { get; set; }
        public DateTime FinancialDate { get; set; }
        public bool isByPaymentUpdate { get; set; }
        public int PaymentId { get; set; }
        public string ReasonforChange { get; set; }
        public int UserId { get; set; }
    }
}
