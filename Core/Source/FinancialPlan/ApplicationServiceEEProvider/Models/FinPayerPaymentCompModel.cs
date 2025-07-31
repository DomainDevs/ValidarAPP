using System.Collections.Generic;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Models
{
    public class FinPayerPaymentCompModel
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int Number { get; set; }
        public int ComponentId { get; set; }
        public decimal PaymentPct { get; set; }
        public decimal Amount { get; set; }
        public decimal LocalAmount { get; set; }
        public decimal MainAmount { get; set; }
        public decimal MainLocalAmount { get; set; }
        public List<FinPayerPaymentLbSbModel> FinPayerPaymentLbSbModel { get; set; }
    }
}
