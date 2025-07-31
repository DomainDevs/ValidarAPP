using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Models
{
    /// <summary>
    /// Plan Financiero
    /// </summary>
    public class FinanPlanModel
    {
        public int Id { get; set; }
        public int endorsementId { get; set; }
        public int IndividualId { get; set; }
        public int PaymentPlanId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime AccountingDate { get; set; }
        public bool isByPaymentUpdate { get; set; }
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal CoinsurancePct { get; set; }
        public string ReasonforChange { get; set; }
        public List<FinPayerPaymentModel> QuotaPlan { get; set; }
        public List<FinPayerPaymentCompModel> QuotaPlanComponents { get; set; }
    }
}
