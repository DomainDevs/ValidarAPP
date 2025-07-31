using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Entities.View
{
    [Serializable()]
    public class RiskSuretyView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection RiskSureties
        {
            get
            {
                return this["RiskSurety"];
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
        public BusinessCollection RiskBeneficiaries
        {
            get
            {
                return this["RiskBeneficiary"];
            }
        }
        public BusinessCollection RiskSuretyGuarantees
        {
            get
            {
                return this["RiskSuretyGuarantee"];
            }
        }

        public BusinessCollection RiskSuretyContracts
        {
            get
            {
                return this["RiskSuretyContract"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection CoRiskSurety
        {
            get
            {
                return this["CoRiskSurety"];
            }
        }

        public BusinessCollection RiskClauses
        {
            get
            {
                return this["RiskClause"];
            }
        }

        public BusinessCollection InsuredGuarantee
        {
            get
            {
                return this["InsuredGuarantee"];
            }
        }

    }
}
