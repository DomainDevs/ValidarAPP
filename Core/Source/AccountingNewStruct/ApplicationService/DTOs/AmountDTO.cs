using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
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
