using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities.views
{
    [Serializable()]
    public class TempThirdPartyLiabilityView : BusinessView
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
    }
}
