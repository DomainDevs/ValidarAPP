using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Sureties.SuretyServices.Models
{
    public class ContractDto
    { /// <summary>
      /// Gets or sets the risk.
      /// </summary>
      /// <value>
      /// The risk.
      /// </value>
        public ISSEN.Risk Risk { get; set; }

        /// <summary>
        /// Gets or sets the risk.
        /// </summary>
        /// <value>
        /// The risk.
        /// </value>
        public ISSEN.RiskSurety RiskSurety { get; set; }

        /// <summary>
        /// Gets or sets the risk surety contract.
        /// </summary>
        /// <value>
        /// The risk surety contract.
        /// </value>
        public ISSEN.RiskSuretyContract RiskSuretyContract { get; set; }

        /// <summary>
        /// Gets or sets the endorsement risk.
        /// </summary>
        /// <value>
        /// The endorsement risk.
        /// </value>
        public ISSEN.EndorsementRisk EndorsementRisk { get; set; }

        /// <summary>
        /// Gets or sets the policy.
        /// </summary>
        /// <value>
        /// The policy.
        /// </value>
        public ISSEN.Policy Policy { get; set; }

    }
}
