using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utiles.Log;
using Utiles.Readers;

namespace ConsumerQueueAudit.AbstractQuee
{
    public abstract class TemplateQuee
    {
        private int actualTask = -1;

        private Task[] tasksConsumer;

        protected ushort _PrefetchCount;

        protected ushort PrefetchCount
        {
            get
            {
                _PrefetchCount = ConfigurationReadAsistance.GetConfigurationValue<ushort>("PrefetchCount");
                return _PrefetchCount;
            }
            set { _PrefetchCount = value; }
        }

        protected ushort _PrefetchCountFail;

        protected ushort PrefetchCountFail
        {
            get
            {
                _PrefetchCountFail = ConfigurationReadAsistance.GetConfigurationValue<ushort>("PrefetchCountFail");
                return _PrefetchCountFail;
            }
            set { _PrefetchCountFail = value; }
        }

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
                ushort prefetchCount = PrincipalQueeName.Contains("Fail") ? PrefetchCountFail : PrefetchCount;
                tasksConsumer = new Task[prefetchCount];

                Helpers.Queue.Instance(PrincipalQueeName, prefetchCount).SubscribeToQueue((messageBody, key) =>
                {
                    actualTask++;
                    if (actualTask > (prefetchCount - 1))
                    {
                        try
                        {
                            tasksConsumer = tasksConsumer.Where(p => p != null).ToArray();
                            Task.WaitAll(tasksConsumer);
                            foreach (Task task in tasksConsumer)
                            {
                                task.Dispose();
                            }
                            tasksConsumer = null;
                        }
                        catch { }
                        finally
                        {
                            tasksConsumer = new Task[prefetchCount];
                            actualTask = 0;
                        }
                    }
                    tasksConsumer[actualTask] = Task.Run(() =>
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
                    });
                }, noAck: false);
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
            if (PrincipalQueeName.Contains("Fail"))
            {
                System.Threading.Thread.Sleep(ConfigurationReadAsistance.GetConfigurationValue<int>("TimeRetry"));
                Helpers.Queue.Instance(PrincipalQueeName, PrefetchCount).PutOnQueue(body);
            }
            else
            {
                Helpers.Queue.Instance(FailedQueAsigned, PrefetchCountFail).PutOnQueue(body);
            }
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