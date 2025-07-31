using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class RejectionCausesParametrizationView : BusinessView
    {
        public BusinessCollection RejectionCauses
        {
            get
            {
                return this["RejectionCauses"];
            }
        }

        public BusinessCollection GroupPolicies
        {
            get
            {
                return this["GroupPolicies"];
            }
        }


    }
}
