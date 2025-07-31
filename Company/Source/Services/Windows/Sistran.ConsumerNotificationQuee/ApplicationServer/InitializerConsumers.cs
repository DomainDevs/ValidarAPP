using ApplicationServer.AbstractQuee;
using ApplicationServer.ConcreteQuee;
using System.Collections.Generic;


namespace ApplicationServer
{
    public class InitializerConsumers
    {
        private static List<TemplateQuee> consumers = new List<TemplateQuee>();

        public static void InitializeQuee()
        {
            var notification = new NotificationQueue();
            notification.TransactionalSubscribe();
        }
    }
}