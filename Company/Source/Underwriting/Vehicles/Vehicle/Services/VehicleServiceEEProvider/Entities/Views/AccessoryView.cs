using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.Views
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
