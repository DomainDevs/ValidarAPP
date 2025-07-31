using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest
{
    [Serializable()]
    public class SalvageRecoveryView : BusinessView
    {
        public BusinessCollection PaymentRecoveries
        {
            get
            {
                return this["PaymentRecovery"];
            }
        }

        public BusinessCollection Salvages
        {
            get
            {
                return this["Salvage"];
            }
        }

        public BusinessCollection Recoveries
        {
            get
            {
                return this["Recovery"];
            }
        }
    }
}
