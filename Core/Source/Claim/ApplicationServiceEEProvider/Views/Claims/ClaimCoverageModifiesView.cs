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
    public class ClaimCoverageModifiesView : BusinessView
    {
        public BusinessCollection ClaimModifies
        {
            get
            {
                return this["ClaimModify"];
            }
        }

        public BusinessCollection ClaimCoverages
        {
            get
            {
                return this["ClaimCoverage"];
            }
        }

        public BusinessCollection ClaimCoveragesAmount
        {
            get
            {
                return this["ClaimCoverageAmount"];
            }
        }

        public BusinessCollection EstimationTypes
        {
            get
            {
                return this["EstimationType"];
            }
        }

    }
}
