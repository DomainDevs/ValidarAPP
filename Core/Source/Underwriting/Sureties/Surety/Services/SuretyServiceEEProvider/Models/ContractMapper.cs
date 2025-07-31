using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Models
{
    public class ContractMapper
    {
        /// <summary>
        /// entitiesRisk
        /// </summary>
        public ISSEN.Risk entityRisk { get; set; }
        /// <summary>
        /// entityEndorsementRisk
        /// </summary>
        public ISSEN.EndorsementRisk entityEndorsementRisk { get; set; }
        /// <summary>
        /// entityRiskSurety
        /// </summary>
        public ISSEN.RiskSurety entityRiskSurety { get; set; }
        /// <summary>
        /// entityPolicy
        /// </summary>
        public ISSEN.Policy entityPolicy { get; set; }
        /// <summary>
        /// entityRiskSuretyContract
        /// </summary>
        public ISSEN.RiskSuretyContract entityRiskSuretyContract { get; set; }
    }
}
