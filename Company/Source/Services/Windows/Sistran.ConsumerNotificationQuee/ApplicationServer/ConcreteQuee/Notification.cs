using Utiles.Extentions;
using System;
using ApplicationServer.AbstractQuee;
using ApplicationServer.Helpers;
using System.Linq;
using ApplicationServer.ManagerSection;

namespace ApplicationServer.ConcreteQuee
{
    public class NotificationQueue : TemplateQuee
    {
        public const char Separator = (char)007;

        protected override void ActionQueeToExcecute(Object body)
        {
            var sectionn = ConnectionManagerDataSection.GetInstance();

            if (sectionn != null)
            {
                foreach (ConnectionManagerEndpointElement item in sectionn.ConnectionManagerEndpoints.AsParallel())
                {
                    ApiProxy.PostAsync(item.Address, item.Api, body);
                }
            }
        }

        protected override void ExceptionQueActions(Object body, Exception e)
        {
            WrapperObjectQuee jsonRetry = GetObjectToProcess((string)body);
            RemoveMessage = WouldRemoveMessage(jsonRetry);
            if (RemoveMessage)
            {
                var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue(PrincipalQueeName, serialization: "JSON");
                queue.PutOnQueue(jsonRetry.GetJson());

            }
            CreateEventLog(body, e);
        }

    }
}