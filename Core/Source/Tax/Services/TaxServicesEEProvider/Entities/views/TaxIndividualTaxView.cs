using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.TaxServices.views
{
    [Serializable()]
    public class TaxIndividualTaxView : BusinessView
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
    }
}