using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
    [DataContract]
    public class UniqueUserLoginDTO
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public int ExpirationsDays { get; set; }
        [DataMember]
        public bool? MustChangePassword { get; set; }
    }
}
