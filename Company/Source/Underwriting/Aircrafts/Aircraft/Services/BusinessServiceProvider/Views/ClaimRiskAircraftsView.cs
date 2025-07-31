using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Views
{
    [Serializable()]
    public class ClaimRiskAircraftsView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection Risks
        {
            get
            {
                return this["Risk"];
            }
        }

        public BusinessCollection RiskAircrafts
        {
            get
            {
                return this["RiskAircraft"];
            }
        }
    }
}
