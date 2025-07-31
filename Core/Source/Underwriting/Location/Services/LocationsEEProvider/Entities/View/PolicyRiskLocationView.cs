using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Locations.EEProvider.Entities.Views
{
    public class PolicyRiskLocationView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection RiskLocations
        {
            get
            {
                return this["RiskLocation"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }
        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }
        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }
    }
}