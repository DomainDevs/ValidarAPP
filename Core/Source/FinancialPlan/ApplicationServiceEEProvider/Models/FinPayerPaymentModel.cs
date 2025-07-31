using System;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Models
{
    /// <summary>
    /// Plan de pago
    /// </summary>
    public class FinPayerPaymentModel
    {
        public int Id { get; set; }
        public int PaymentNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal PaymentPct { get; set; }
        public decimal Amount { get; set; }
        public decimal LocalAmount { get; set; }
        public decimal MainAmount { get; set; }
        public decimal MainLocalAmount { get; set; }
        public short PaymentState { get; set; }
        public short OriginalPaymentState { get; set; }
        public bool IsByPaymentFinancial { get; set; }
        public bool IsCurrent { get; set; }
        public int FinPayerId { get; set; }
    }
}
