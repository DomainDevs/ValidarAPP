using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.AuthorizationPoliciesServices.EEProvider.Entities.Views
{
    [Serializable()]
    class WorkFlowPolicyView : BusinessView
    {
        public BusinessCollection CoEventAuthorizations
        {
            get
            {
                return this["CoEventAuthorization"];
            }
        }

        public BusinessCollection EndorsementsWorkFlow
        {
            get
            {
                return this["EndorsementWorkFlow"];
            }
        }

        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }

        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection UniqueUsers
        {
            get
            {
                return this["UniqueUsers"];
            }
        }
    }
}
