using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.MassiveServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}
