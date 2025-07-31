using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CurrencyDifferenceDTO 
    {
        [DataMember]
        public decimal MaximumDifference { get; set; }
        [DataMember]
        public decimal PercentageDifference { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Rows { get; set; }

    }
}
