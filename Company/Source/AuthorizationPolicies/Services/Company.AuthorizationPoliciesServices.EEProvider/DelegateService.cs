using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider
{
    public class DelegateService
    {
        //called as a service 
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}
