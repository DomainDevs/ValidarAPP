using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class DelegationParametrizationView : BusinessView
    {
        public BusinessCollection Hierarchy_Association
        {
            get
            {
                return this["Hierarchy_Association"];
            }
        }

        public BusinessCollection SubModules
        {
            get
            {
                return this["SubModules"];
            }
        }

        public BusinessCollection Modules
        {
            get
            {
                return this["Modules"];
            }
        }

        public BusinessCollection Hierarchy
        {
            get
            {
                return this["Hierarchy"];
            }
        }


    }
}
