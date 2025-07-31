
using Sistran.Core.Application.Cache.CacheBusinessService;

namespace Sistran.Core.Application.Cache.CacheIntegrationService.EEProvider
{
    public class CacheIntegrationServiceProvider : ICacheIntegrationService
    { 
        public void LoadCacheByJson(string historyVersionJson)
        {
            DelegateService.cacheBusinessService.LoadCacheByJson(historyVersionJson);   
        }

    }
}