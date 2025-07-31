using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest

{
    [Serializable()]
    public class PaymentRequestClaimView : BusinessView
    {

        public BusinessCollection PaymentRequests
        {
            get
            {
                return this["PaymentRequest"];
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

        public BusinessCollection PaymentMethods
        {
            get
            {
                return this["PaymentMethod"];
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
