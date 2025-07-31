using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimSuretyDTO : ClaimDTO
    {
        [DataMember]
        public string BidNumber { get; set; }

        [DataMember]
        public string CourtNum { get; set; }

    }
}
