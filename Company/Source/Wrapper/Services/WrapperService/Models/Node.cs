using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class Node
    {
        [DataMember]
        public string Hostname { get; set; }

        [DataMember]
        public Guid Version { get; set; }

        [DataMember]
        public DateTime StartDateLoad { get; set; }

        [DataMember]
        public DateTime? EndDateLoad { get; set; }
    }
}
