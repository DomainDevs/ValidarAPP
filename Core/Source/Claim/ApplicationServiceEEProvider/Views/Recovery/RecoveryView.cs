using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Recovery
{
    [Serializable()]
    public class RecoveryView : BusinessView
    {
        public BusinessCollection Recoveries
        {
            get
            {
                return this["Recovery"];
            }
        }

        public BusinessCollection PaymentPlans
        {
            get
            {
                return this["PaymentPlan"];
            }
        }
    }
}
