using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class RiskVehicleView : BusinessView
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
        public BusinessCollection CompanyVehicles
        {
            get
            {
                return this["Risk"];
            }
        }

        public BusinessCollection CoRisks
        {
            get
            {
                return this["CompanyCoRisk"];
            }
        }
        public BusinessCollection RiskPayers
        {
            get
            {
                return this["RiskPayer"];
            }
        }
        public BusinessCollection CoRiskVehicles
        {
            get
            {
                return this["CoRiskVehicle"];
            }
        }
        public BusinessCollection RiskBeneficiaries
        {
            get
            {
                return this["RiskBeneficiary"];
            }
        }

        public BusinessCollection RiskClause
        {
            get
            {
                return this["RiskClause"];
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
        public BusinessCollection VehicleVersions
        {
            get
            {
                return this["VehicleVersion"];
            }
        }
        public BusinessCollection VehicleColors
        {
            get
            {
                return this["VehicleColor"];
            }
        }
        public BusinessCollection VehicleUses
        {
            get
            {
                return this["VehicleUse"];
            }
        }

        public BusinessCollection VehicleTypes
        {
            get
            {
                return this["VehicleType"];
            }
        }

        public BusinessCollection RatingZones
        {
            get
            {
                return this["RatingZone"];
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
