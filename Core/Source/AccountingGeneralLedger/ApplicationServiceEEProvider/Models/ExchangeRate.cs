using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
    [DataContract]
    public class ExchangeRate
    {
        [DataMember]
        public DateTime RateDate { get; set; }
        [DataMember]
        public decimal SellAmount { get; set; }
        [DataMember]
        public decimal BuyAmount { get; set; }
        [DataMember]
        public Currency Currency { get; set; }
    }
}
