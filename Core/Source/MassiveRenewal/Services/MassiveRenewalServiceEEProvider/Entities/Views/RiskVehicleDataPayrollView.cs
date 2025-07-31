using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class RiskVehicleDataPayrollView : BusinessView
    {
        public BusinessCollection RiskVehicles
        {
            get
            {
                return this["RiskVehicle"];
            }
        }

        public BusinessCollection CoRiskVehicles
        {
            get
            {
                return this["CoRiskVehicle"];
            }
        }

        public BusinessCollection VehicleTypes
        {
            get
            {
                return this["VehicleType"];
            }
        }

        public BusinessCollection VehicleMakes
        {
            get
            {
                return this["VehicleMake"];
            }
        }

        public BusinessCollection VehicleModels
        {
            get
            {
                return this["VehicleModel"];
            }
        }
    }
}