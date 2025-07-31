using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class RiskRenewalView : BusinessView
    {
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

        public BusinessCollection RiskVehicles
        {
            get
            {
                return this["RiskVehicle"];
            }
        }

        public BusinessCollection RiskLocations
        {
            get
            {
                return this["RiskLocation"];
            }
        }

        public BusinessCollection RiskSureties
        {
            get
            {
                return this["RiskSurety"];
            }
        }
    }
}