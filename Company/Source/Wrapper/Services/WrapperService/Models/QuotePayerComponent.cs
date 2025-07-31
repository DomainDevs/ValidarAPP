using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuotePayerComponent
    {
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal BaseAmount { get; set; }
        [DataMember]
        public Component Component { get; set; }
        [DataMember]
        public QuoteCoverage Coverage { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        [DataMember]
        public RateType? RateType { get; set; }
        [DataMember]
        public int? TaxConditionId { get; set; }
        [DataMember]
        public int? TaxId { get; set; }
    }
}
