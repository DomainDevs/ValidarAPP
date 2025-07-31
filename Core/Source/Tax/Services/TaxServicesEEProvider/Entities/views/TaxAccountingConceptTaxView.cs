using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

using System;

namespace Sistran.Core.Application.TaxServices.views
{
    [Serializable()]
    public class TaxAccountingConceptTaxView : BusinessView
    {
        public BusinessCollection TaxRates
        {
            get
            {
                return this["TaxRate"];
            }
        }

        public BusinessCollection TaxPeriodRates
        {
            get
            {
                return this["TaxPeriodRate"];
            }
        }
    }
}
