using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PolicyVehicleRiskSummaryView : BusinessView
    {
        public BusinessCollection EndorsementRisk
        {
            get { return this["EndorsementRisk"]; }
        }

        public BusinessCollection Endorsement
        {
            get { return this["Endorsement"]; }
        }

        public BusinessCollection RiskVehicle
        {
            get { return this["RiskVehicle"]; }
        }
        public BusinessCollection Risks
        {
            get { return this["Risk"]; }
        }
    }
}
