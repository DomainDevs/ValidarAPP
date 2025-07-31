using System.Runtime.Serialization;

namespace Sistran.Core.Integration.TempCommonService.DTOs
{
    [DataContract]
    public class AgentDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string AgentType { get; set; }
        [DataMember]
        public int AgentTypeId { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int IndividualType { get; set; }
    }
}
