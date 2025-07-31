using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectiveServices.Models
{
    [DataContract]
    public class IssuedCollectiveLoad
    {
        [DataMember]
        public List<CollectiveEmission> CollectiveEmissions { get; set; }

        [DataMember]
        public List<CollectiveEmissionRow> CollectiveEmissionRows { get; set; }

        [DataMember]
        public decimal Premium { get; set; }

        [DataMember]
        public decimal AmmountInsured { get; set; }

        [DataMember]
        public int Errors { get; set; }

        [DataMember]
        public int RiskEvents { get; set; }

        [DataMember]
        public int PolicyEvents { get; set; }
    }
}
