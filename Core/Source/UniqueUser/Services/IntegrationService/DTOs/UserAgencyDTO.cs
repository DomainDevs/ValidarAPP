using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
    [DataContract]
    public class UserAgencyDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public DateTime? DateDeclined { get; set; }
        
    }
}
