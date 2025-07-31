using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalService.Models
{
    [Serializable]
    [DataContract]
    public class VehicleFilterIndividual : FilterIndividual
    {
       

        /// <summary>
        /// Años de buena experiencia
        /// </summary>
        [DataMember]
        public GoodExperienceYear GoodExperienceYear { get; set; }

        /// <summary>
        /// Tiene multas
        /// </summary>
        [DataMember]
        public bool? HasInfrigementSimit { get; set; }

        

    }
}
