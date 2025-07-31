using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    public class PaymentComponentModel
    {
        public int AppId { get; set; }
        public List<PayerPaymentComponentDTO> payerPaymentComponentDTOs { get; set; }
        public decimal ExchangeRate { get; set; }
        public int CurrencyId { get; set; }
        public decimal PercentageCoinsurance { get; set; }        
    }
}
