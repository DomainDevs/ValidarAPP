using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Views
{
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
