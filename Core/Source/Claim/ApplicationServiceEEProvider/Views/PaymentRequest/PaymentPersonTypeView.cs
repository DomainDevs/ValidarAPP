using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class PaymentPersonTypeView : BusinessView
    {

        public BusinessCollection ClaimPersonTypes
        {
            get
            {
                return this["ClaimPersonType"];
            }
        }
        public BusinessCollection PersonTypes
        {
            get
            {
                return this["PersonType"];
            }
        }
    }
}
