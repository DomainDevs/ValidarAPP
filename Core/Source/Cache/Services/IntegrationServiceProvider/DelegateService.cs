using Sistran.Core.Application.Cache.CacheBusinessService;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Cache.CacheIntegrationService.EEProvider
{
    class DelegateService
    {
        internal static ICacheBusinessService cacheBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICacheBusinessService>();
    }
}