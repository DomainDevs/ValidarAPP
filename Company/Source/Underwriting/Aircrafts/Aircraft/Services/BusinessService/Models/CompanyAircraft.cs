using System.Runtime.Serialization;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models.Base;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base
{
    /// <summary>
    /// Aircrafte - Riesgo
    /// </summary>
    [DataContract]
    public class CompanyAircraft : BaseAircraft
    {
        
        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public CompanyRisk Risk { get; set; }

        /// <summary>
        /// Objetos del seguro
        /// </summary>
        [DataMember]
        public List<InsuredObject> InsuredObjects { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public CompanyModel Model { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public CompanyMake Make { get; set; }
    }
}
