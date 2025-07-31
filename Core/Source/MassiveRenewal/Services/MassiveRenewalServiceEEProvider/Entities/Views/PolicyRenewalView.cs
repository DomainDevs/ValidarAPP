using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PolicyRenewalView : BusinessView
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

        public BusinessCollection CoRequest
        {
            get
            {
                return this["CoRequest"];
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

        public BusinessCollection Products
        {
            get
            {
                return this["Product"];
            }
        }

        public BusinessCollection PolicyAgents
        {
            get
            {
                return this["PolicyAgent"];
            }
        }

        public BusinessCollection TempSubscriptions
        {
            get
            {
                return this["TempSubscription"];
            }
        }
        public BusinessCollection BillinGroup
        {
            get
            {
                return this["BillinGroup"];
            }
        }
    }
}