using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class TempRiskSuretyView : BusinessView
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

        public BusinessCollection TempRiskSureties
        {
            get
            {
                return this["TempRiskSurety"];
            }
        }

        public BusinessCollection TempRiskSuretyGuarantees
        {
            get
            {
                return this["TempRiskSuretyGuarantee"];
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
