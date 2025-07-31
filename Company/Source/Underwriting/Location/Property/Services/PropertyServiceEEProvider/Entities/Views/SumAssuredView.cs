using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.View
{
    [Serializable()]
    public class SumAssuredView : BusinessView
    {
        public BusinessCollection EndoRiskCoverages
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
