using Sistran.Company.Application.MassiveServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VSMO = Sistran.Company.Application.Vehicles.VehicleServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.Models
{
    [Serializable]
    [DataContract]
    public class VehicleFilterIndividual : FilterIndividual
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        //public IdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// Años de buena experiencia
        /// </summary>
        [DataMember]
        public VSMO.GoodExperienceYear GoodExperienceYear { get; set; }

        /// <summary>
        /// Tiene multas
        /// </summary>
        [DataMember]
        public bool? HasInfrigementSimit { get; set; }

        /// <summary>
        /// Lista de multas 
        /// </summary>
        //[DataMember]
        //public List<InfringementCount> InfringementCounts { get; set; }

    }
}
