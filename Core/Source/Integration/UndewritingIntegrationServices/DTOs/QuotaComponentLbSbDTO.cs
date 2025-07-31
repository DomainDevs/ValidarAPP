using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class QuotaComponentLbSbDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int QuotaNumber { get; set; }
        [DataMember]
        public int ComponentId { get; set; }     
        public int LbId { get; set; }
        [DataMember]
        public int SbId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
