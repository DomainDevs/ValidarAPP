using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class PaymentRequestSummaryView : BusinessView
    {
        public BusinessCollection PaymentRequests
        {
            get
            {
                return this["PaymentRequest"];
            }
        }

        public BusinessCollection PaymentRequestsClaim
        {
            get
            {
                return this["PaymentRequestClaim"];
            }
        }

        public BusinessCollection Claims
        {
            get
            {
                return this["Claim"];
            }
        }
        
        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }

        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection PaymentVoucherConcepts
        {
            get
            {
                return this["PaymentVoucherConcept"];
            }
        }

        public BusinessCollection PaymentVouchers
        {
            get
            {
                return this["PaymentVoucher"];
            }
        }
    }
}
