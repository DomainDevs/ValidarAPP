using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteSummary
    {
        [DataMember]
        public decimal AmountInsured { get; set; }

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Expenses { get; set; }
        [DataMember]
        public decimal FullPremium { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public int RiskCount { get; set; }
        [DataMember]
        public decimal Taxes { get; set; }
        [DataMember]
        public int TemporalId { get; set; }
    }
}
