using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ClaimPolicyView : BusinessView
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

        public BusinessCollection BusinessTypes
        {
            get
            {
                return this["BusinessType"];
            }
        }

        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection PolicyAgents
        {
            get
            {
                return this["PolicyAgent"];
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

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }

        public BusinessCollection CoPolicyTypes
        {
            get
            {
                return this["CoPolicyType"];
            }
        }

        public BusinessCollection EndorsementTypes
        {
            get
            {
                return this["EndorsementType"];
            }
        }

        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }
    }
}
