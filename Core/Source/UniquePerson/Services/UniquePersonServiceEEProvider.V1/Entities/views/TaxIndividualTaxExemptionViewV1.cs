using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class TaxIndividualTaxExemptionViewV1 : BusinessView
    {
        public BusinessCollection IndividualTax
        {
            get
            {
                return this["IndividualTax"];
            }
        }

        public BusinessCollection Role
        {
            get
            {
                return this["Role"];
            }
        }

        public BusinessCollection TaxRate
        {
            get
            {
                return this["TaxRate"];
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

        public BusinessCollection TaxConditions
        {
            get
            {
                return this["TaxCondition"];
            }
        }

    }
}
