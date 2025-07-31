using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class CacheStatus
    {
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Guid Version { get; set; }
        [DataMember]
        public DateTime VersionDatetime { get; set; }
        [DataMember]
        public List<Node> Nodes { get; set; }
    }
}
