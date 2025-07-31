using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities.views
{
    [Serializable()]
    public class RiskThirdPartyLiabilityView : BusinessView
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

        public BusinessCollection Risks
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
                return this["CoRisk"];
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

        public BusinessCollection CiaRiskVehicles
        {
            get
            {
                return this["CiaRiskVehicle"];
            }
        }
    }
}
