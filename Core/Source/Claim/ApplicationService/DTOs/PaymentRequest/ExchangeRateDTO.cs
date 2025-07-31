using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class ExchangeRateDTO
    {
        [DataMember]
        public DateTime RateDate { get; set; }

        [DataMember]
        public CurrencyDTO Currency { get; set; }

        [DataMember]
        public decimal SellAmount { get; set; }

        [DataMember]
        public decimal BuyAmount { get; set; }
    }
}
