
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class ClaimPaymentsView : BusinessView
    {
        public BusinessCollection PaymentRequestClaims
        {
            get
            {
                return this["PaymentRequestClaim"];
            }
        }

        public BusinessCollection PaymentRequests
        {
            get
            {
                return this["PaymentRequest"];
            }
        }

        public BusinessCollection PaymentVoucherConcepts
        {
            get
            {
                return this["PaymentVoucherConcept"];
            }
        }
    }
}
