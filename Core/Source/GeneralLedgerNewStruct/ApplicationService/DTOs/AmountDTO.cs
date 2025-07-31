using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class AmountDTO
    {
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public decimal Value { get; set; }
    }
}
