using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.View
{
    [Serializable()]
    public class AccessoryView : BusinessView
    {
        public BusinessCollection EndorsementRiskCoverages
        {
            get
            {
                return this["EndorsementRiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverDetails
        {
            get
            {
                return this["RiskCoverDetail"];
            }
        }
        public BusinessCollection RiskDetails
        {
            get
            {
                return this["RiskDetail"];
            }
        }

        public BusinessCollection RiskDetailAccessories
        {
            get
            {
                return this["RiskDetailAccessory"];
            }
        }
        public BusinessCollection RiskDetailDescriptions
        {
            get
            {
                return this["RiskDetailDescription"];
            }
        }

    }
}
