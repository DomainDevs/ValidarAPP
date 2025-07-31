using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Vehicles.EEProvider.Entities.Views
{
    public class PolicyRiskSuretyView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection RiskSureties
        {
            get
            {
                return this["RiskSurety"];
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