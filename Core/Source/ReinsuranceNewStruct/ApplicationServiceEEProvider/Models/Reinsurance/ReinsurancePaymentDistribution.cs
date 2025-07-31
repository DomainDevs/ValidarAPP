using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsurancePaymentDistribution : ReinsuranceClaimDistribution
    {
        [DataMember]
        public int PaymentLayerId { get; set; }
    }
}
