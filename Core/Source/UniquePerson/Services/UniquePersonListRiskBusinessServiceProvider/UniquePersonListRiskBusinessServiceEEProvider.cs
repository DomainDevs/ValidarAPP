using Newtonsoft.Json;
using Sistran.Core.Application.UniquePersonListRiskBusinessService;
using Sistran.Core.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider.DAO;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queues;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider
{
    public class UniquePersonListRiskBusinessServiceEEProvider : IUniquePersonListRiskBusinessService
    {
        public void LoadOnMemoryListRisks(string userName)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            NodeListRiskStatusDAO nodeStatusDAO = new NodeListRiskStatusDAO();

            try
            {
                CacheManager.umbrals = listRiskLoadDAO.GetUmbral();
                CacheManager.entityViewListRisks = listRiskLoadDAO.GetViewListRiskPerson(null);

                nodeStatusDAO.CreateNewNodeVersion(userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al consultar las listas de riesgo", ex);
            }
        }

        public void SendToRefreshOnMemoryListRisks(string userName)
        {
            try
            {
                string exchangeName = "ListRiskCache.fanout";
                string json = JsonConvert.SerializeObject(new { UserName = userName });

                string queueName = $"{Environment.MachineName} - ListRiskCache";
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    { ArgumentsNames.DeadLetterExchange, exchangeName },
                    { ArgumentsNames.MaxLength, 500 }
                };

                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = queueName,
                    ExchangeName = exchangeName,
                    ExchangeType = "fanout",
                    RoutingKey = string.Empty,
                    PersistentMessages = true,
                    Arguments = args
                };

                createQueueParameters.Serialization = ConfigurationManager.AppSettings["Serialization"];
                IQueue queue = new BaseQueueFactory().CreateQueue(createQueueParameters);
                queue.PutOnQueue(json);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al enviar mensaje a Rabbit (ExchangeName \"ListRiskCache.fanout\")", ex);
            }
        }

        public List<RiskListMatch> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            return listRiskLoadDAO.ValidateListRiskPerson(documentNumber, fullName, riskListType);
        }
    }
}
