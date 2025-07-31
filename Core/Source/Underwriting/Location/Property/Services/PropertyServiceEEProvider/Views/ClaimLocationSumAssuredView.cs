using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Location.PropertyServices.EEProvider.Views
{
    [Serializable()]
    public class ClaimLocationSumAssuredView : BusinessView
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
