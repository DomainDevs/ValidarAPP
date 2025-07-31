using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PaymentDistributionCompDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }
}
