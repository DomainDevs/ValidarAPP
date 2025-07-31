using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class DriverInformationDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string LicenseType { get; set; }

        [DataMember]
        public string LicenseNumber { get; set; }

        [DataMember]
        public DateTime? LicenseValidThru { get; set; }

        [DataMember]
        public int? Age { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int DocumentType { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
