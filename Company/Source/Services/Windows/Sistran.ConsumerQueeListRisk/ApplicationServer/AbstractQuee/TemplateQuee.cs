using ApplicationServer.Helpers;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Queues;
using Sistran.Core.Framework.Transactions;
using System;
using System.Diagnostics;
using Utiles.Extentions;
using Utiles.Log;
using Utiles.Readers;


namespace ApplicationServer.AbstractQuee
{
    public abstract class TemplateQuee

    {
        protected bool RemoveMessage;

        protected string _PrincipalQueeName;

        protected string PrincipalQueeName
        {
            get
            {
                if (string.IsNullOrEmpty(_PrincipalQueeName))
                {
                    _PrincipalQueeName = GetType().Name;
                }
                return _PrincipalQueeName;
            }
            set { _PrincipalQueeName = value; }
        }

        private string _FailedQueAsigned;

        public string FailedQueAsigned
        {
            get
            {
                if (string.IsNullOrEmpty(_FailedQueAsigned))
                {
                    _FailedQueAsigned = string.Format("Fail{0}", PrincipalQueeName);
                }
                return _FailedQueAsigned;
            }
            set { _FailedQueAsigned = value; }
        }

        public void TransactionalSubscribe()
        {

            try
            {
                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = PrincipalQueeName,
                    RoutingKey = PrincipalQueeName,
                    PersistentMessages = true
                };

                IQueue queue = Helpers.Queue.QueueInstance(createQueueParameters);

                queue.SubscribeToQueue((messageBody, key) =>
                {
                    using (Context.Current)
                    {
                        using (Transaction transaction = new Transaction())
                        {
                            try
                            {
                                RemoveMessage = true;
                                ActionQueeToExcecute(messageBody);
                                transaction.Complete();

                            }
                            catch (Exception e)
                            {
                                transaction.Dispose();
                                ExceptionQueActions(messageBody, e);
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        //private void RemoveMessageFromQuee(IModel channel, Object ea)
        //{
        //    if (RemoveMessage)
        //    {
        //        //channel.BasicAck(ea.DeliveryTag, false);
        //    }
        //}

        protected virtual void ExceptionQueActions(Object body, Exception e)
        {

            WrapperObjectQuee jsonRetry = GetObjectToProcess((string)body);
            RemoveMessage = WouldRemoveMessage(jsonRetry);
            if (RemoveMessage)
            {
                QueueHelper.PutOnQueueJsonByQueue(jsonRetry.GetJson(), PrincipalQueeName);
            }
            else
            {
                CreateEventLog(body, e);
            }
        }

        protected void CreateEventLog(Object body, Exception e)
        {
            EventLog myLog = new EventLog();
            myLog.Source = "Application";
            string bodyT = (string)body;
            if (bodyT != null && bodyT.Length > 500)
            {
                bodyT = bodyT.Substring(0, 499);
            }

            var message = string.Format("SISTRAN.CORE.FRAMEWORK.QUEUES: {1}STACKTRACE: {0} BODY:{2}", e.StackTrace, Environment.NewLine, bodyT);
            myLog.WriteEntry(message, EventLogEntryType.Error);
        }

        protected abstract void ActionQueeToExcecute(Object messageBody);

        //private static ConnectionFactory GetConectionFactoryQuee()
        //{
        //    string configSection = ConfigurationReadAsistance.GetCustomSectionLikeJson("sistran.core.framework/queues");
        //    return new ConnectionFactory()
        //    {
        //        HostName = ConfigurationReadAsistance.GetConfigurationValue<string>(configSection, "HostName"),
        //        VirtualHost = ConfigurationReadAsistance.GetConfigurationValue<string>(configSection, "VirtualHost"),
        //        UserName = ConfigurationReadAsistance.GetConfigurationValue<string>(configSection, "UserName"),
        //        Password = ConfigurationReadAsistance.GetConfigurationValue<string>(configSection, "Password"),
        //        Port = ConfigurationReadAsistance.GetConfigurationValue<int>(configSection, "Port"),
        //    };
        //}

        protected WrapperObjectQuee GetObjectToProcess(string businessCollection)
        {
            WrapperObjectQuee wrapperObjectQuee = businessCollection.GetObject<WrapperObjectQuee>();
            if (wrapperObjectQuee != null && !string.IsNullOrEmpty(wrapperObjectQuee.JsonToProcess))
            {
                wrapperObjectQuee.TryCount++;
            }
            else
            {
                wrapperObjectQuee = new WrapperObjectQuee { JsonToProcess = businessCollection };
            }
            return wrapperObjectQuee;
        }

        protected string GetJsonToProcess(string businessCollection)
        {
            string jsonToProcess = businessCollection;
            WrapperObjectQuee wrapperObjectQuee = businessCollection.GetObject<WrapperObjectQuee>();
            if (wrapperObjectQuee != null && !string.IsNullOrEmpty(wrapperObjectQuee.JsonToProcess))
            {
                jsonToProcess = wrapperObjectQuee.JsonToProcess;
            }
            return jsonToProcess;
        }

        protected bool WouldRemoveMessage(WrapperObjectQuee jsonRetry)
        {
            int limitReTry = ConfigurationReadAsistance.GetConfigurationValue<int>("LimitRetry");
            return jsonRetry.TryCount < limitReTry;
        }

        
    }
}