using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimCoverageActivePanelDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public string PrintDescription { get; set; }

        [DataMember]
        public bool IsEnabledDriver { get; set; }

        [DataMember]
        public bool IsEnabledThirdPartyVehicle { get; set; }

        [DataMember]
        public bool IsEnabledThird { get; set; }

        [DataMember]
        public bool IsEnabledAffectedProperty { get; set; }
    }
}
