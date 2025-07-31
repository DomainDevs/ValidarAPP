using Sistran.Core.Framework.Queues;
using System.Collections.Generic;

namespace ConsumerQueueAudit.Helpers
{
    public class Queue
    {
        private static readonly object _lockIQueue = new object();
        private static IQueue queueInstance = null;
        private static Dictionary<string, IQueue> dictionary = new Dictionary<string, IQueue>();

        public static IQueue Instance(string queueName, ushort prefetchCount)
        {
            lock (_lockIQueue)
            {
                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = queueName,
                    RoutingKey = queueName,
                    PrefetchCount = prefetchCount,
                    PersistentMessages = true
                };
                if (queueInstance == null || !dictionary.ContainsKey(createQueueParameters.QueueName))
                {
                    queueInstance = new BaseQueueFactory().CreateQueue(createQueueParameters);
                    dictionary.Add(createQueueParameters.QueueName, queueInstance);
                }
                else
                {
                    queueInstance = dictionary[createQueueParameters.QueueName];
                }
            }
            return queueInstance;
        }
    }
}
