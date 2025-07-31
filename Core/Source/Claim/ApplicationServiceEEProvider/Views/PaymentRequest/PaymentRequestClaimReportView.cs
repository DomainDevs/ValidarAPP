using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest

{
    [Serializable()]
    public class PaymentRequestClaimReportView : BusinessView
    {
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

        public BusinessCollection PaymentRequests
        {
            get
            {
                return this["PaymentRequest"];
            }
        }

        public BusinessCollection PersonTypes
        {
            get
            {
                return this["PersonType"];
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

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
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
    }
}
