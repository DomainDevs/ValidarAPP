using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.DTOs.Filter
{
    [DataContract]  
    public class ComponentValueDTO
    {
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Taxes { get; set; }
        [DataMember]
        public decimal Expenses { get; set; }
        [DataMember]
        public decimal Surcharges { get; set; }
        [DataMember]
        public decimal Discounts { get; set; }
        [DataMember]
        public decimal FullPremium { get; set; }
    }
}
