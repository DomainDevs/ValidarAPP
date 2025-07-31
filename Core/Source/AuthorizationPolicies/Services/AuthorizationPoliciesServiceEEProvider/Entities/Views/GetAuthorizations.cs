using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AuthorizationPoliciesGetAuthorizations : BusinessView
    {
        public BusinessCollection AutorizarionRequest
        {
            get
            {
                return this["AutorizarionRequest"];
            }
        }

        public BusinessCollection AutorizarionAnswer
        {
            get
            {
                return this["AutorizarionAnswer"];
            }
        }

        public BusinessCollection Policies
        {
            get
            {
                return this["Policies"];
            }
        }

        public BusinessCollection Users
        {
            get
            {
                return this["Users"];
            }
        }
    }
}

