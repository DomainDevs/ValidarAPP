using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using System;
using System.Diagnostics;
using System.Text;
using Utiles.Log;

namespace ConsumerListRisksCacheQueue.AbstractQuee
{
    public abstract class TemplateQuee
    {
        private static readonly string exchangeName = "ListRiskCache.fanout";

        protected string _PrincipalQueeName;

        protected string PrincipalQueeName
        {
            get
            {
                if (string.IsNullOrEmpty(_PrincipalQueeName))
                {
                    _PrincipalQueeName = Environment.MachineName;
                }
                return _PrincipalQueeName;
            }
            set { _PrincipalQueeName = value; }
        }

        public void TransactionalSubscribe()
        {
            try
            {
                Helpers.Queue.InstanceExchange(exchangeName).SubscribeToQueue((messageBody, key) =>
                {
                    using (Context.Current)
                    {
                        using (Transaction transaction = new Transaction())
                        {
                            try
                            {
                                ActionQueeToExcecute(messageBody);
                                transaction.Complete();
                            }
                            catch (Exception e)
                            {
                                ExceptionQueActions(messageBody, e);
                                transaction.Dispose();
                            }
                        }
                    }
                }, noAck: true);
                StringBuilder message = new StringBuilder();
                message.AppendLine(string.Format("Consumer Queue: {0}", PrincipalQueeName));
                message.AppendLine(string.Format("Date: {0}", DateTime.Now.ToString()));
                EventViewerAsistant.SingleInstance.WriteInEventViewer(message.ToString(), EventLogEntryType.Information);
            }
            catch (Exception e)
            {
                CreateEventLog(null, e);
            }
        }

        protected virtual void ExceptionQueActions(object body, Exception e)
        {
            CreateEventLog(body, e);
        }

        protected void CreateEventLog(object body, Exception e)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("Error Queue: {0}", PrincipalQueeName));
            message.AppendLine(string.Format("Exception: {0}", e.Message));
            if (body != null)
            {
                int maxSixeLog = 30000;
                string dataObject = body.ToString();
                if (dataObject.Length > maxSixeLog)
                {
                    dataObject = dataObject.Substring(0, maxSixeLog);
                }
                message.AppendLine(Environment.NewLine);
                message.AppendLine(string.Format("Object: {0}", dataObject));
            }
            message.AppendLine(Environment.NewLine);
            message.AppendLine(string.Format("Date: {0}", DateTime.Now.ToString()));
            EventViewerAsistant.SingleInstance.WriteInEventViewer(message.ToString(), EventLogEntryType.Error);
        }

        protected abstract void ActionQueeToExcecute(object messageBody);
    }
}