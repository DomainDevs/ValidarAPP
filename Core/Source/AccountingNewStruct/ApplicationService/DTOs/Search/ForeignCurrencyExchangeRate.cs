using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ForeignCurrencyExchangeRate
    {
        [DataMember]
        public int CurrencyId { get; set; }
		
        [DataMember]
        public decimal ExchangeRate { get; set; }
    }
}
