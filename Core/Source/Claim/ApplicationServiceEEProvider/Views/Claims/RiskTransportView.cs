using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims
{
    [Serializable()]
    public class RiskTransportView : BusinessView
    {
        public BusinessCollection RiskTransports
        {
            get
            {
                return this["RiskTransport"];
            }
        }

        public BusinessCollection RiskTransportMeans
        {
            get
            {
                return this["RiskTransportMean"];
            }
        }

        public BusinessCollection TransportMeans
        {
            get
            {
                return this["TransportMean"];
            }
        }

        public BusinessCollection TransportsCargoTypes
        {
            get
            {
                return this["TransportCargoType"];
            }
        }
    }
}
