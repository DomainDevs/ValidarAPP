using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class UniqueUserSession : BaseUniqueUserSession
    {

        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public int AgencyId { get; set; }

        [DataMember]
        public int ProfileId { get; set; }
    }
}
