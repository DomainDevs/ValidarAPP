using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Views
{
    [Serializable()]
    public class ClaimRiskMarineView : BusinessView
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

        public BusinessCollection RiskAircraftUses
        {
            get
            {
                return this["RiskAircraftUse"];
            }
        }
    }
}
