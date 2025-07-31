using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsurancePaymentLayerDTO : ReinsuranceClaimLayerDTO
    {
        [DataMember]
        public int PaymentRequestId { get; set; }

        [DataMember]
        public int PaymentLayerId { get; set; }

        [DataMember]
        public string UserName { get; set; }

    }
}