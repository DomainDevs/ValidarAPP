using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class PaymentVoucherView : BusinessView
    {

        public BusinessCollection PaymentVouchers
        {
            get
            {
                return this["PaymentVoucher"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }

        public BusinessCollection VoucherTypes
        {
            get
            {
                return this["VoucherType"];
            }
        }
    }
}
