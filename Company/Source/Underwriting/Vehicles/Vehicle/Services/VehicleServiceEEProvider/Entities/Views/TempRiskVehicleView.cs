using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class TempRiskVehicleView : BusinessView
    {
        public BusinessCollection TempRisks
        {
            get
            {
                return this["TempRisk"];
            }
        }

        public BusinessCollection CoTempRisks
        {
            get
            {
                return this["CoTempRisk"];
            }
        }

        public BusinessCollection TempRiskVehicles
        {
            get
            {
                return this["TempRiskVehicle"];
            }
        }

        public BusinessCollection CoTempRiskVehicles
        {
            get
            {
                return this["CoTempRiskVehicle"];
            }
        }

        public BusinessCollection TempRiskBeneficiaries
        {
            get
            {
                return this["TempRiskBeneficiary"];
            }
        }

        public BusinessCollection TempRiskClauses
        {
            get
            {
                return this["TempRiskClause"];
            }
        }

        public BusinessCollection TempRiskCoverDetails
        {
            get
            {
                return this["TempRiskCoverDetail"];
            }
        }

        public BusinessCollection TempRiskDetails
        {
            get
            {
                return this["TempRiskDetail"];
            }
        }

        public BusinessCollection TempRiskDetailAccessories
        {
            get
            {
                return this["TempRiskDetailAccessory"];
            }
        }
    }
}
