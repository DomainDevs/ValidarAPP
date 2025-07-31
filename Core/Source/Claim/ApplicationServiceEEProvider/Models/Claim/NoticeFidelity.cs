using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class NoticeFidelity
    {
        [DataMember]
        public int RiskCommercialClassId { get; set; }

        [DataMember]
        public int OccupationId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime DiscoveryDate { get; set; }

        /// <summary>
        /// Notice
        /// </summary>
        [DataMember]
        public Notice Notice { get; set; }
    }
}
