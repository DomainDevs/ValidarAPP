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
    public class ClaimCoverageView : BusinessView
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

        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }

        public BusinessCollection EstimationTypes
        {
            get
            {
                return this["EstimationType"];
            }
        }

        public BusinessCollection EstimationTypesStatuses
        {
            get
            {
                return this["EstimationTypeStatus"];
            }
        }

        public BusinessCollection EstimationTypeInternalStatuses
        {
            get
            {
                return this["EstimationTypeInternalStatus"];
            }
        }
    }
}
