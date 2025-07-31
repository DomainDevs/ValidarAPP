using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class TaxIndividualTaxExemptionView : BusinessView
    {
        public BusinessCollection IndividualTax
        {
            get
            {
                return this["IndividualTax"];
            }
        }
        public BusinessCollection TaxCondition
        {
            get
            {
                return this["TaxCondition"];
            }
        }

        public BusinessCollection Tax
        {
            get
            {
                return this["Tax"];
            }
        }
        public BusinessCollection IndividualTaxExemption
        {
            get
            {
                return this["IndividualTaxExemption"];
            }
        }
        public BusinessCollection State
        {
            get
            {
                return this["State"];
            }
        }


        public BusinessCollection TaxCategory
        {
            get
            {
                return this["TaxCategory"];
            }
        }

    }
}
