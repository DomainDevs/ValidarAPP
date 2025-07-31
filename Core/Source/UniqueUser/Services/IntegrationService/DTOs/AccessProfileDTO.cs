using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
    [DataContract]
    public class AccessProfileDTO
    {
        [DataMember]
        public int ProfileId { get; set; }
        [DataMember]
        public int AccessId { get; set; }
        [DataMember]
        public int DatabaseId { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public bool IsExpirationDateNull { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public bool Assigned { get; set; }
        [DataMember]
        public bool InDatabase { get; set; }
        [DataMember]
        public int AccessObjectId { get; set; }
        [DataMember]
        public int AccessType { get; set; }
    }
}
