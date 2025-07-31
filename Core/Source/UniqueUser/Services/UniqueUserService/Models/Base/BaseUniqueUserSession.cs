using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseUniqueUserSession
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }


        [DataMember]
        public DateTime ExpirationDate { get; set; }

        [DataMember]
        public DateTime LastUpdateDate { get; set; }
    }
}
