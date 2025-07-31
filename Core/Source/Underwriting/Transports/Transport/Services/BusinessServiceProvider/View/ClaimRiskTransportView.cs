using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Transports.TransportBusinessService.EEProvider.View
{
    public class ClaimRiskTransportView:BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get { return this["EndorsementRisk"]; }
        }

        public BusinessCollection Policies
        {
            get { return this["Policy"]; }
        }

        public BusinessCollection Risks
        {
            get { return this["Risk"]; }
        }

        public BusinessCollection RiskTransports
        {
            get { return this["RiskTransport"]; }
        }

        public BusinessCollection RiskTransportMeans
        {
            get { return this["RiskTransportMean"]; }
        }

        public BusinessCollection TransportMeans
        {
            get { return this["TransportMean"]; }
        }

        public BusinessCollection TransportsCargoTypes
        {
            get { return this["TransportCargoType"]; }
        }

        public BusinessCollection TransportsPackagingTypes
        {
            get { return this["TransportPackagingType"]; }
        }

        public BusinessCollection Countries
        {
            get { return this["Country"]; }
        }

        public BusinessCollection States
        {
            get { return this["State"]; }
        }

        public BusinessCollection Cities
        {
            get { return this["City"]; }
        }

        public BusinessCollection TransportsViaTypes
        {
            get { return this["TransportViaType"]; }
        }
    }
}

   