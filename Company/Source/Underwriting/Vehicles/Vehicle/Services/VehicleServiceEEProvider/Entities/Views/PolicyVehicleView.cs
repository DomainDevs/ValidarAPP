using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.views
{
    public class PolicyVehicleView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection RiskVehicles
        {
            get
            {
                return this["RiskVehicle"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }
    }

}
