using Sistran.Core.Framework.Queues;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;

namespace ApplicationServer.Helpers
{

    public class QueueHelper
    {

        private static void PutOnQueue(CreateQueueParameters createQueueParameters, object item)
        {
            try
            {
                Queue.QueueInstance(createQueueParameters).PutOnQueue(item);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                throw ex;
            }
        }

        private static object ReceiveFromQueue(CreateQueueParameters createQueueParameters)
        {
            try
            {
                createQueueParameters.Serialization = "JSON";
                return Queue.QueueInstance(createQueueParameters).ReceiveFromQueue();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                throw ex;
            }
        }

        public static void PutOnQueueJsonByQueue(object item, string keyQueue, string serialization = "JSON")
        {
            string queueName = ConfigurationManager.AppSettings[keyQueue];
            if (string.IsNullOrEmpty(queueName))
            {
                queueName = keyQueue;
            }
            CreateQueueParameters createQueueParameters = new CreateQueueParameters
            {
                QueueName = queueName,
                RoutingKey = queueName,
                Serialization = serialization,
                PersistentMessages = true
            };
            PutOnQueue(createQueueParameters, item);
        }

        public static void PutOnQueueJsonByExchange(object item, string keyExchangeName)
        {
            try
            {
                string exchangeName = ConfigurationManager.AppSettings[keyExchangeName];
                if (string.IsNullOrEmpty(exchangeName))
                {
                    exchangeName = keyExchangeName;
                }
                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = string.Empty,
                    ExchangeName = exchangeName,
                    ExchangeType = ExchangeType.Fanout,
                    RoutingKey = string.Empty
                };
                PutOnQueue(createQueueParameters, item);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                throw ex;
            }
        }

        public static object ReceiveFromQueue(string keyQueue)
        {
            string queueName = ConfigurationManager.AppSettings[keyQueue];

            if (string.IsNullOrEmpty(queueName))
            {
                queueName = keyQueue;
            }
            CreateQueueParameters createQueueParameters = new CreateQueueParameters
            {
                QueueName = queueName,
                RoutingKey = queueName
            };
            return ReceiveFromQueue(createQueueParameters);
        }
    }

    public class Queue
    {
        private static readonly object _lockIQueue = new object();
        private static IQueue queueInstance = null;
        private static Dictionary<string, IQueue> dictionary = new Dictionary<string, IQueue>();

        public static IQueue QueueInstance(CreateQueueParameters createQueueParameters)
        {
            lock (_lockIQueue)
            {
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

