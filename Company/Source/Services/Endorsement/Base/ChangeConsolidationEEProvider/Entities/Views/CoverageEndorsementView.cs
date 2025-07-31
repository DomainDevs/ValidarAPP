using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.ChangeConsolidationEndorsement.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageConsolidationEndorsementView : BusinessView
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

        public BusinessCollection RiskSureties
        {
            get
            {
                return this["RiskSurety"];
            }
        }
    }
}
