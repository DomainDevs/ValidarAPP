using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class ProviderPaymentConceptViewV1 : BusinessView
    {
        public BusinessCollection PaymentConcepts
        {
            get
            {
                return this["PaymentConcept"];
            }
        }

        public BusinessCollection ProviderPaymentConcepts
        {
            get
            {
                return this["ProviderPaymentConcept"];
            }
        }
    }
}
