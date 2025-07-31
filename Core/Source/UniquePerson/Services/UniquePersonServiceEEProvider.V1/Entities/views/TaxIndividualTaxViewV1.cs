using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class TaxIndividualTaxViewV1 : BusinessView
    {
        public BusinessCollection IndividualTaxs
        {
            get
            {
                return this["IndividualTax"];
            }
        }

        public BusinessCollection Taxs
        {
            get
            {
                return this["Tax"];
            }
        }

        public BusinessCollection TaxConditions
        {
            get
            {
                return this["TaxCondition"];
            }
        }
    }
}
