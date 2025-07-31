using Newtonsoft.Json;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using System;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider.DAO
{
    internal class NodeListRiskStatusDAO
    {

        public void CreateNewNodeVersion(string userName)
        {
            try
            {
                var listCurrentVersion = CacheManager.entityViewListRisks
                    .GroupBy(x => new { x.RiskListCode, x.Description, x.ProcessId },
                    (key, item) => new
                    {
                        ListId = key.RiskListCode,
                        ListName = key.Description,
                        Version = key.ProcessId,
                        Count = item.Count()
                    }).OrderBy(x => x.ListId);

                var umbralCurrentVersion = CacheManager.umbrals.Select(x => new { ListType = x.Key, Umbral = x.Value })
                    .OrderBy(x => x.ListType);


                var entity = new NodeListRiskStatus()
                {
                    Node = Environment.MachineName,
                    Username = userName,
                    Creationdate = DateTime.UtcNow,
                    Lists = JsonConvert.SerializeObject(listCurrentVersion),
                    Umbrals = JsonConvert.SerializeObject(umbralCurrentVersion)
                };

                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error insertando un registro NodeListRiskStatus", ex);
            }
        }
    }
}
