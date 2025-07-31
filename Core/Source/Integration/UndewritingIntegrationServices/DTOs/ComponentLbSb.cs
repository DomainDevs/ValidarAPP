using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class ComponentLbSb
    {
        [DataMember]
        public int LineBusiness { get; set; }

        [DataMember]
        public int SubLineBusiness { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
