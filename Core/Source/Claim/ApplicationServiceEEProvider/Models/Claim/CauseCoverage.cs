using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class CauseCoverage
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int LackPeriod { get; set; }

        [DataMember]
        public Cause Cause { get; set; }
    }
}
