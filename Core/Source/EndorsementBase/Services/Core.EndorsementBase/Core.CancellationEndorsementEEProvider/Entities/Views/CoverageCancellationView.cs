using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Core.CancellationEndorsement3GProvider.Entities.Views
{
    [Serializable()]
    public class CoverageCancellationView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection EndorsementRiskCoverages
        {
            get
            {
                return this["EndorsementRiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverages
        {
            get
            {
                return this["RiskCoverage"];
            }
        }
    }
}
