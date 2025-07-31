using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PolicyView : BusinessView
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

        public BusinessCollection TempPolicyControl
        {
            get
            {
                return this["TempPolicyControl"];
            }
        }
        public BusinessCollection PayerFinancialPremiums
        {
            get
            {
                return this["PayerFinancialPremium"];
            }
        }

    }
}
