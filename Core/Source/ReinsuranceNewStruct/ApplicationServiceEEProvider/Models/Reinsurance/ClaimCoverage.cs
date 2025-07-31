using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ClaimCoverage
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int SubClaim { get; set; }

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public string RiskDescription { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int CoverageNumber { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public bool IsInsured { get; set; }

        [DataMember]
        public bool IsProspect { get; set; }

        [DataMember]
        public decimal? SubLimitAmount { get; set; }

        /// <summary>
        /// Tercero Afectado
        /// </summary>
        [DataMember]
        public ThirdAffected ThirdAffected { get; set; }

        /// <summary>
        /// Conductor
        /// </summary>
        [DataMember]
        public Driver Driver { get; set; }

        /// <summary>
        /// Vehiculo Tercero
        /// </summary>
        [DataMember]
        public ThirdPartyVehicle ThirdPartyVehicle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<Estimation> Estimations { get; set; }

        [DataMember]
        public TextOperation TextOperation { get; set; }
    }
}
