using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsurancePaymentLayer : ReinsuranceClaimLayer
    {
        [DataMember]
        public int PaymentRequestId { get; set; }
        [DataMember]
        public int PaymentLayerId { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }
}
