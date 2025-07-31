using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class PaymentConceptBranchView : BusinessView
    {
        public BusinessCollection PaymentConcepts
        {
            get
            {
                return this["PaymentConcept"];
            }
        }
        public BusinessCollection BranchPaymentConcepts
        {
            get
            {
                return this["BranchPaymentConcept"];
            }
        }
    }
}
