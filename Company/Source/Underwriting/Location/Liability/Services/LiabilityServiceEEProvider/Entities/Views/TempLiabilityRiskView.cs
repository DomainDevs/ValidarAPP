using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.Entities.views
{
    [Serializable()]
    public class TempLiabilityRiskView : BusinessView
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

        public BusinessCollection TempLiabilityRisks
        {
            get
            {
                return this["TempRiskLocation"];
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
