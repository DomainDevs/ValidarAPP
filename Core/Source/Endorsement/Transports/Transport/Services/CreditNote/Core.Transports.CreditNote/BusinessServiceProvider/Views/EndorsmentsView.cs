using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Transports.CreditNote.BusinessService.EEProvider.Views
{
    [Serializable()]
    public class EndorsmentsView : BusinessView
    {
        
        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }
        public BusinessCollection CoPolicies
        {
            get
            {
                return this["CoPolicy"];
            }
        }

        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }
        public BusinessCollection GroupEndorsements
        {
            get
            {
                return this["GroupEndorsement"];
            }
        }
        public BusinessCollection PolicyAgents
        {
            get
            {
                return this["PolicyAgent"];
            }
        }

        public BusinessCollection CommissAgents
        {
            get
            {
                return this["CommissAgent"];
            }
        }
        public BusinessCollection EndorsementPayers
        {
            get
            {
                return this["EndorsementPayer"];
            }
        }

        public BusinessCollection PayerPayments
        {
            get
            {
                return this["PayerPayment"];
            }
        }

        public BusinessCollection PayerComponents
        {
            get
            {
                return this["PayerComp"];
            }
        }
        public BusinessCollection CoinsurancesAccepted
        {
            get
            {
                return this["CoinsuranceAccepted"];
            }
        }
        public BusinessCollection CoinsurancesAssigned
        {
            get
            {
                return this["CoinsuranceAssigned"];
            }
        }

        public BusinessCollection PolicyClauses
        {
            get
            {
                return this["PolicyClause"];
            }
        }
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }
        public BusinessCollection RiskTransports
        {
            get
            {
                return this["RiskTransport"];
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

        public BusinessCollection CompanyTransports
        {
            get
            {
                return this["Risk"];
            }
        }
        public BusinessCollection RiskClause
        {
            get
            {
                return this["RiskClause"];
            }
        }

        public BusinessCollection EndorsementOperations
        {
            get
            {
                return this["EndorsementOperation"];
            }
        }

        public BusinessCollection EndorsementTypes
        {
            get
            {
                return this["EndorsementType"];
            }
        }
    }
}