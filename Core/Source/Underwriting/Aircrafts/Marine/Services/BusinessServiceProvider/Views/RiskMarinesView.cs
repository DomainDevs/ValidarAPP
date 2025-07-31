using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Views
{
    [Serializable]
    public class RiskMarinesView : BusinessView
    {

        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }
        public BusinessCollection EndorsementOperations
        {
            get
            {
                return this["EndorsementOperation"];
            }
        }
        public BusinessCollection EndoRiskCoverages
        {
            get
            {
                return this["EndoRiskCoverage"];
            }
        }
        public BusinessCollection RiskCoverages
        {
            get
            {
                return this["RiskCoverage"];
            }
        }
        public BusinessCollection RiskMarines
        {
            get
            {
                return this["RiskAircraft"];
            }
        }
        public BusinessCollection RiskMarineUses
        {
            get
            {
                return this["RiskAircraftUse"];
            }
        }
    }
}

    