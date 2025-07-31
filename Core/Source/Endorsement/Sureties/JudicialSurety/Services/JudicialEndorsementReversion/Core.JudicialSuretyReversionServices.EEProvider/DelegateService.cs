using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.JudicialEndorsementReversionService3GProvider
{
    public class DelegateService
    {
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
    }
}
