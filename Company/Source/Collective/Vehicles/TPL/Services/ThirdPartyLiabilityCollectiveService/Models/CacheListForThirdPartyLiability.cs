using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.TPLCollectiveServices.Models
{
    [DataContract]
    public class CacheListForThirdPartyLiability
    {
        /// <summary>
        /// Lista de personas
        /// </summary>
        [DataMember]
        public List<ThirdPartyLiabilityFilterIndividual> VehicleFilterIndividuals { get; set; }

        [DataMember]
        public List<int> InsuredForScoreList { get; set; }

        [DataMember]
        public List<int> InsuredForSimitList { get; set; }
    }
}
