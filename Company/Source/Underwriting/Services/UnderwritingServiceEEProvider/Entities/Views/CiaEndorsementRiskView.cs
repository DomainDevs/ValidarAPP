using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class CiaEndorsementRiskView : BusinessView
    {
        public BusinessCollection EndorsementRiskCoverages
        {
            get { return this["EndorsementRiskCoverage"]; }
        }

        public BusinessCollection RiskCoverages
        {
            get { return this["RiskCoverage"]; }
        }
    }
}
