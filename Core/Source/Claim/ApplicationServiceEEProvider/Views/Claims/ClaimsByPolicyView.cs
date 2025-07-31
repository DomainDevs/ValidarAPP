using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims
{
    [Serializable()]
    public class ClaimsByPolicyView : BusinessView
    {
        public BusinessCollection ClaimCoverages
        {
            get
            {
                return this["ClaimCoverage"];
            }
        }

        public BusinessCollection ClaimCoverageAmounts
        {
            get
            {
                return this["ClaimCoverageAmount"];
            }
        }
    }
}
