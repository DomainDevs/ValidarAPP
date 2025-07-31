using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class ExchangeRateDTO
    {
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public DateTime RateDate { get; set; }
        [DataMember]
        public decimal SellAmount { get; set; }
        [DataMember]
        public decimal BuyAmount { get; set; }
    }

}
