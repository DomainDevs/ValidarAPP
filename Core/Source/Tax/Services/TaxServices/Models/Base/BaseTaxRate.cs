using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.Models.Base
{
    [DataContract]
    public class BaseTaxRate
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Tax Tax { get; set; } 
        [DataMember]
        public TaxCondition TaxCondition { get; set; }
        [DataMember]
        public TaxCategory TaxCategory { get; set; }
    }
}
