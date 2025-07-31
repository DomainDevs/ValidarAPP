using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest

{
    [Serializable()]
    public class AccountBankView : BusinessView
    {
        public BusinessCollection UpAccountBank
        {
            get
            {
                return this["AccountBank"];
            }
        }
        public BusinessCollection AccountBank
        {
            get
            {
                return this["AccountBank"];
            }
        }


    }
}
