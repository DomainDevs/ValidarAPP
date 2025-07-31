using Sistran.Company.Application.AuthorizationPoliciesServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.EEProvider
{
    public class DelegateService
    {
        internal static IAuthorizationPoliciesService authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesService>();
    }
}
