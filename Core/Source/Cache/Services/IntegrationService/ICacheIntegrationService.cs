using System.ServiceModel;

namespace Sistran.Core.Application.Cache.CacheIntegrationService
{
    [ServiceContract]
    public interface ICacheIntegrationService
    {
        [OperationContract]
        void LoadCacheByJson(string historyVersionJson);
    }
}