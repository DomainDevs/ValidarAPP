using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class DebtorDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Phone { get; set; }
    }
}
