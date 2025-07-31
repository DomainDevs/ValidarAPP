using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.Models
{
    [DataContract]
    public class CacheListForVehicle
    {
        /// <summary>
        /// Lista de personas
        /// </summary>
        [DataMember]
        public List<VehicleFilterIndividual> VehicleFilterIndividuals { get; set; }

        [DataMember]
        public List<int> InsuredForScoreList { get; set; }

        [DataMember]
        public List<int> InsuredForSimitList { get; set; }
    }
}
