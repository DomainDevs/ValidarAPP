using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest

{
    [Serializable()]
    public class PaymentRequestClaimCoverageView : BusinessView
    {
        public BusinessCollection ClaimCoverages
        {
            get
            {
                return this["ClaimCoverage"];
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
