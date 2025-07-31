using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.Models
{
    [DataContract]
    public class CacheListForVehicle
    {
        /// <summary>
        /// Lista de personas
        /// </summary>
        [DataMember]
        public List<VehicleFilterIndividual> VehicleFilterIndividuals { get; set; }

        /// <summary>
        /// Lista de Aliados
        /// </summary>
        //[DataMember]
        //public List<Alliance> Alliances { get; set; }

        [DataMember]
        public ConcurrentDictionary<int, int> InsuredForScoreList { get; set; }

        [DataMember]
        public ConcurrentDictionary<int, int> InsuredForSimitList { get; set; }
    }
}
