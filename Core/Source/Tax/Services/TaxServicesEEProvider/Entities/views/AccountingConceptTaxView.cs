using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.TaxServices.views
{
    [Serializable()]
    public class AccountingConceptTaxView : BusinessView
    {
        public BusinessCollection AccountingConceptTaxes
        {
            get
            {
                return this["AccountingConceptTax"];
            }
        }

        public BusinessCollection Taxes
        {
            get
            {
                return this["Tax"];
            }
        }

        public BusinessCollection TaxCategories
        {
            get
            {
                return this["TaxCategory"];
            }
        }

        public BusinessCollection RateTypes
        {
            get
            {
                return this["RateType"];
            }
        }
    }
}
