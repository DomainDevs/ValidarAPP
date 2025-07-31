using ConsumerQueueAudit.AbstractQuee;
using ConsumerQueueAudit.ConcreteQuee;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Utiles.Log;

namespace ConsumerQueueAudit
{
    public class InitializerConsumers
    {
        private static readonly string consumer = "ConsumerGeneral";
        private static List<TemplateQuee> consumers = new List<TemplateQuee>();

        public static void InitializeQuee()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("Start {0}", consumer));
            message.AppendLine(string.Format("Date: {0}", DateTime.Now.ToString()));
            EventViewerAsistant.SingleInstance.WriteInEventViewer(message.ToString(), EventLogEntryType.Information);

            consumers = new List<TemplateQuee>
            {
                new FailAuditQueue(),
                new AuditQueue()
            };

            foreach (TemplateQuee consumer in consumers)
            {
                Task.Run(() => consumer.TransactionalSubscribe());
            }
        }
    }
}