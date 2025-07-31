using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsurancePaymentDistributionDTO : ReinsuranceClaimDistributionDTO
    {
        [DataMember]
        public int PaymentLayerId { get; set; }
    }
}