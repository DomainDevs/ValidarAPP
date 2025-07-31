using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.Entities.views
{
    [Serializable()]
    public class TempFidelityRiskView : BusinessView
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

        public BusinessCollection TempFidelityRisks
        {
            get
            {
                return this["TempRiskFidelity"];
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
