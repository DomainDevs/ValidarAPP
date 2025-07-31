using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PaymentSummaryDTO
    {
        [DataMember]
        public int PaymentId { get; set; }
        [DataMember]
        public int BillId { get; set; }
        [DataMember]
        public int PaymentMethodId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public List<ConsignmentCheckDTO> ConsignmentChecks { get; set; }
    }
}
