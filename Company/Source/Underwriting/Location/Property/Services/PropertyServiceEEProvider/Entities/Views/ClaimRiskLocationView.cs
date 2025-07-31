using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.View
{
    [Serializable()]
    public class ClaimRiskLocationView : BusinessView
    {
        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection Risks
        {
            get
            {
                return this["Risk"];
            }
        }

        public BusinessCollection RiskLocations
        {
            get
            {
                return this["RiskLocation"];
            }
        }

        public BusinessCollection Cities
        {
            get
            {
                return this["City"];
            }
        }

        public BusinessCollection Countries
        {
            get
            {
                return this["Country"];
            }
        }

        public BusinessCollection States
        {
            get
            {
                return this["State"];
            }
        }
    }
}
