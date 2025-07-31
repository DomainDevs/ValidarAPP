using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationService.Models
{
    [Serializable]
    [DataContract]
    public class ThirdPartyLiabilityFilterIndividual : FilterIndividual
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        public IdentificationDocument IdentificationDocument { get; set; }

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

        /// <summary>
        /// Lista de multas 
        /// </summary>
        [DataMember]
        public List<InfringementCount> InfringementCounts { get; set; }

    }
}
