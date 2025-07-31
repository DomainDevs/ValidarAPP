using Sistran.Core.Framework.Queues;
using System;
using System.Collections.Generic;
using Utiles.Readers;

namespace ConsumerListRisksCacheQueue.Helpers
{
    public class Queue
    {
        private static readonly object _lockIQueueExchange = new object();
        private static IQueue exchangeInstance = null;
        private static Dictionary<string, IQueue> dictionaryExchange = new Dictionary<string, IQueue>();

        public static IQueue InstanceExchange(string exchangeName)
        {
            lock (_lockIQueueExchange)
            {
                string queueName = Environment.MachineName.ToString();
                queueName += " - " + ConfigurationReadAsistance.GetConfigurationValue<string>("QueueName");
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    { ArgumentsNames.DeadLetterExchange, exchangeName },
                    { ArgumentsNames.MaxLength, 500 }
                };
                CreateQueueParameters createQueueParameter = new CreateQueueParameters()
                {
                    QueueName = queueName,
                    ExchangeName = exchangeName,
                    ExchangeType = "fanout",
                    RoutingKey = string.Empty,
                    PersistentMessages = true,
                    Arguments = args
                };
                if (exchangeInstance == null || !dictionaryExchange.ContainsKey(createQueueParameter.ExchangeName))
                {
                    exchangeInstance = (new BaseQueueFactory()).CreateQueue(createQueueParameter);
                    dictionaryExchange.Add(createQueueParameter.ExchangeName, exchangeInstance);
                }
                else
                {
                    exchangeInstance = dictionaryExchange[createQueueParameter.ExchangeName];
                }
            }
            return exchangeInstance;
        }
    }
}