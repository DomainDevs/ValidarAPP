using ConsumerListRisksCacheQueue.ConcreteQuee;
using System;
using System.Diagnostics;
using System.Text;
using Utiles.Log;

namespace ConsumerListRisksCacheQueue
{
    public class InitializerConsumers
    {
        private static readonly string consumer = "ConsumerListRisk";
        public static void InitializeQuee()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("Start {0}", consumer));
            message.AppendLine(string.Format("Date: {0}", DateTime.Now.ToString()));
            EventViewerAsistant.SingleInstance.WriteInEventViewer(message.ToString(), EventLogEntryType.Information, 200);
            new SaveListRiskCacheQuee().TransactionalSubscribe();
        }
    }
}