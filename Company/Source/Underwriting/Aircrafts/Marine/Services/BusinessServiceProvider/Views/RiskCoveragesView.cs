using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Views
{
    [Serializable()]
    class CompanyMarineRiskCoveragesView : BusinessView
    {
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
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
