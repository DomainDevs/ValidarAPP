using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class ProviderPaymentConceptView : BusinessView
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
