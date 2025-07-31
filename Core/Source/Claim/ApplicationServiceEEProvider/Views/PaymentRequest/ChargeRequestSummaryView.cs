using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class ChargeRequestSummaryView : BusinessView
    {
        public BusinessCollection PaymentRecoveries
        {
            get
            {
                return this["PaymentRecovery"];
            }
        }

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

        public BusinessCollection ClaimBranches
        {
            get
            {
                return this["ClaimBranch"];
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
