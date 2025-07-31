using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.MassiveServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Core.Application.CollectiveServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IUniqueUserServiceCore uniqueUSerServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IMassiveServiceCore massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}
