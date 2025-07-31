using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class PaymentRequestModificationView : BusinessView
    {
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

        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }
    }
}
