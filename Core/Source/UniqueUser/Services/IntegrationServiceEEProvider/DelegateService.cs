using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserIntegrationService.EEProvider
{
    public class DelegateService
    {
        public static readonly IUniqueUserServiceCore uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();

    }
}
