using System;
using ApplicationServer.AbstractQuee;
using ApplicationServer.QueueBrokerService;
using Utiles.Extentions;

namespace ApplicationServer.ConcreteQuee
{
    public class ListRiskQueue : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            string businessCollection = (string)body;

            if (businessCollection != null)
            {
                try
                {
                    using (QueueBrokerServiceClient ub = new QueueBrokerServiceClient("BasicHttpBinding_IQueueBrokerService"))
                    {
                        ub.CreateListRisk(businessCollection);
                    }

                }
                catch (Exception e)
                {
                    ExceptionQueActions(body, e);
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
                queue.PutOnQueue(jsonRetry.JsonToProcess.GetJson());

            }
            CreateEventLog(body, e);
        }
    }
}
