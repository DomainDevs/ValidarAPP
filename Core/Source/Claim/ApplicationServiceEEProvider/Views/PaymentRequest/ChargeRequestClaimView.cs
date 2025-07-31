using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest

{
    [Serializable()]
    public class ChargeRequestClaimView : BusinessView
    {

        public BusinessCollection PaymentRequests
        {
            get
            {
                return this["PaymentRequest"];
            }
        }

        public BusinessCollection PaymentRecovereis
        {
            get
            {
                return this["PaymentRecovery"];
            }
        }

        public BusinessCollection Claims
        {
            get
            {
                return this["Claim"];
            }
        }

        public BusinessCollection PaymentRequestClaims
        {
            get
            {
                return this["PaymentRequestClaim"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }

        public BusinessCollection PaymentRequestCoinsurances
        {
            get
            {
                return this["PaymentRequestCoinsurance"];
            }
        }
    }
}
