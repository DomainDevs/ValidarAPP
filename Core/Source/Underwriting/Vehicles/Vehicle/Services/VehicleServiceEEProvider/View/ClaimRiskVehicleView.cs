using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.View
{
    [Serializable()]
    public class ClaimRiskVehicleView : BusinessView
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

        public BusinessCollection RiskVehicles
        {
            get
            {
                return this["RiskVehicle"];
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
        public BusinessCollection VehicleColors
        {
            get
            {
                return this["VehicleColor"];
            }
        }
    }
}
