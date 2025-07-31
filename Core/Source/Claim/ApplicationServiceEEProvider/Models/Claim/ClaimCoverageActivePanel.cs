using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class ClaimCoverageActivePanel
    {
        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public bool EnabledDriver { get; set; }

        [DataMember]
        public bool EnabledThirdPartyVehicle { get; set; }

        [DataMember]
        public bool EnabledThird { get; set; }

        [DataMember]
        public string CoverageDescription { get; set; }

        [DataMember]
        public bool EnabledAffectedProperty { get; set; }

        [DataMember]
        public bool? IsInsured { get; set; }

        [DataMember]
        public bool? IsProspect { get; set; }
    }
}
