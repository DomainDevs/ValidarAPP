
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PrefixComponentCollectionDTO 
    {
        [DataMember]
        public int PrefixComponentId { get; set; }

        [DataMember]
        public int ComponentCollectionId { get; set; }

        [DataMember]
        public int LineBusinessId { get; set; }

        [DataMember]
        public int SubLineBusinessId { get; set; }
    }
}
