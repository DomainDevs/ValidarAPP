using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.TaxServices.views
{
    [Serializable()]
    public class IndividualTaxExemptionView : BusinessView
    {
        public BusinessCollection TaxExemptions
        {
            get
            {
                return this["IndividualTaxExemption"];
            }
        }

    }
}