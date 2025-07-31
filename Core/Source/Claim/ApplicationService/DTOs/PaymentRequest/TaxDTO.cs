using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class TaxDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}