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
    public class EstimationTypesClaimView : BusinessView
    {
        public BusinessCollection ClaimModifications
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

        public BusinessCollection ClaimCoverageAmounts
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

        public BusinessCollection EstimationTypeStatuses
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

        public BusinessCollection EstimationTypeStatusReasons
        {
            get
            {
                return this["EstimationTypeStatusReason"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
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
    }
}
