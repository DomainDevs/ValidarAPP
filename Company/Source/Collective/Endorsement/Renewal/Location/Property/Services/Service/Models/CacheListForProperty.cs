using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.UniquePersonServices.Models;

namespace Sistran.Company.Application.Location.CollectivePropertyRenewalService.Models
{
    [DataContract]
    public class CacheListForProperty
    {
        /// <summary>
        /// Lista de personas
        /// </summary>
        [DataMember]
        public List<FilterIndividual> FilterIndividuals { get; set; }

        /// <summary>
        /// Lista de Aliados
        /// </summary>
        [DataMember]
        public List<Alliance> Alliances { get; set; }

        [DataMember]
        public List<int> InsuredForScoreList { get; set; }
    }
}
