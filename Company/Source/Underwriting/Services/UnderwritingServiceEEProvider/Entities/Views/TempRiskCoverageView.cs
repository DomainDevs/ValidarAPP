using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class TempRiskCoverageView : BusinessView
    {
        public BusinessCollection TempRiskCoverages
        {
            get
            {
                return this["TempRiskCoverage"];
            }
        }

        public BusinessCollection TempRiskCoverDeducts
        {
            get
            {
                return this["TempRiskCoverDeduct"];
            }
        }

        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }

        public BusinessCollection GroupCoverages
        {
            get
            {
                return this["GroupCoverage"];
            }
        }
    }
}
