using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class AccountingVoucher
    {
        [DataMember]
        public List<AccountingConcept> Concepts { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }
    }
}
