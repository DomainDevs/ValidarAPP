using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Views
{
    [Serializable()]
    public class SumAssuredMarineView : BusinessView
    {
        public BusinessCollection EndoRiskCoverages
        {
            get { return this["EndorsementRiskCoverage"]; }
        }

        public BusinessCollection RiskCoverages
        {
            get { return this["RiskCoverage"]; }
        }
    }
}
