using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class IndividualDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int DocumentType { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string FullAddress { get; set; }

    }
}
