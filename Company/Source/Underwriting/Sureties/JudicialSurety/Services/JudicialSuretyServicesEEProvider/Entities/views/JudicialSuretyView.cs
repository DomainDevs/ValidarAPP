using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Entities.views
{
    [Serializable()]
    public class JudicialSuretyView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection Endorsement
        {
            get
            {
                return this["Endorsement"];
            }
        }
        public BusinessCollection RiskJudicialSurety
        {
            get
            {
                return this["RiskJudicialSurety"];
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

        public BusinessCollection RiskClauses
        {
            get
            {
                return this["RiskClause"];
            }
        }
    }
}
