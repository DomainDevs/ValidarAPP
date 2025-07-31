using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationService.Models
{
    [DataContract]
    public class CacheListForThirdPartyLiability
    {
        /// <summary>
        /// Lista de personas
        /// </summary>
        [DataMember]
        public List<ThirdPartyLiabilityFilterIndividual> TplFilterIndividuals { get; set; }

        [DataMember]
        public List<int> InsuredForScoreList { get; set; }

        [DataMember]
        public List<int> InsuredForSimitList { get; set; }
    }
}
