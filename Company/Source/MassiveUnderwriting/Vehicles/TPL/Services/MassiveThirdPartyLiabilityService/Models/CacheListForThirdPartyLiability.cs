using Sistran.Company.Application.UniquePersonParamService.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.Models
{
    [DataContract]
    public class CacheListForThirdPartyLiability
    {
        /// <summary>
        /// Lista de personas
        /// </summary>
        [DataMember]
        public List<ThirdPartyLiabilityFilterIndividual> VehicleFilterIndividuals { get; set; }

        /// <summary>
        /// Lista de Aliados
        /// </summary>
        [DataMember]
        public List<Alliance> Alliances { get; set; }

        [DataMember]
        public ConcurrentDictionary<int, int> InsuredForScoreList { get; set; }

        [DataMember]
        public ConcurrentDictionary<int, int> InsuredForSimitList { get; set; }
    }
}
