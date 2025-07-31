using ApplicationServer.AbstractQuee;
using ApplicationServer.Helpers;
using Sistran.Core.Framework.Queues;
using System;
using Utiles.Extentions;

namespace ApplicationServer.ConcreteQuee
{
    public class FailUpdatePendingOperationQuee: TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            
            string businessCollection = (string)body;
            //if (businessCollection != null)
            //{
            //    using (PendingOperationEntityServiceClient client = new PendingOperationEntityServiceClient("BasicHttpBinding_IPendingOperationEntityService"))
            //    {
            //        var response = businessCollection.GetObject<PendingOperation>();
            //        client.UpdatePendingOperation(response);
            //    }
            //}
        }

        protected override void ExceptionQueActions(Object body, Exception e)
        {
            
            WrapperObjectQuee jsonRetry = GetObjectToProcess((string)body);
            RemoveMessage = WouldRemoveMessage(jsonRetry);
            if (RemoveMessage)
            {
                QueueHelper.PutOnQueueJsonByQueue(jsonRetry.GetJson(), PrincipalQueeName);
            }
            CreateEventLog(body, e);
        }
    }
}
