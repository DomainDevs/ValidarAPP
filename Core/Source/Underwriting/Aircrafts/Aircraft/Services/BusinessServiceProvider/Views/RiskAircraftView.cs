using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Transports.TransportBusinessService.EEProvider.Views
{
    [Serializable]
    public class RiskTransportView : BusinessView
    {
        public BusinessCollection EndorsementOperations
        {
            get
            {
                return this["EndorsementOperations"];
            }
        }
    }
}
