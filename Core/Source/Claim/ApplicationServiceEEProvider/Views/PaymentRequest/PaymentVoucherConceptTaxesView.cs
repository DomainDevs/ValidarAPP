using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class PaymentVoucherConceptTaxesView : BusinessView
    {

        public BusinessCollection PaymentVoucherConceptTaxes
        {
            get
            {
                return this["PaymentVoucherConceptTax"];
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

        public BusinessCollection TaxConditions
        {
            get
            {
                return this["TaxCondition"];
            }
        }
    }
}
