using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

using System;

namespace Sistran.Core.Application.TaxServices.views
{
    [Serializable()]
    public class IndividualTaxCategoryConditionView : BusinessView
    {
        public BusinessCollection Taxes
        {
            get
            {
                return this["Tax"];
            }
        }

        public BusinessCollection IndividualTaxes
        {
            get
            {
                return this["IndividualTax"];
            }
        }

        public BusinessCollection TaxConditions
        {
            get
            {
                return this["TaxCondition"];
            }
        }

        public BusinessCollection TaxCategories
        {
            get
            {
                return this["TaxCategory"];
            }
        }
    }
}
