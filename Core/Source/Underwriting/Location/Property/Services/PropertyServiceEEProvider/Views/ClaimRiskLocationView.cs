using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Location.PropertyServices.EEProvider.Views
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
