using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class PayerComponentDTO
    {
        [DataMember]
        public decimal BaseAmount { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int Id { get; set; }
    }
}
