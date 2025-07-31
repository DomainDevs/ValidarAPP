using ApplicationServer.AbstractQuee;
using ApplicationServer.Helpers;
using Sistran.Core.Framework.Queues;
using System;
using Utiles.Extentions;

namespace ApplicationServer.ConcreteQuee
{
    public class FailCreatePendingOperationQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            
            string businessCollection = (string)body;
            if (businessCollection != null)
            {
                //var pendingOperation = new PendingOperation();
                //string[] objectsToSave = businessCollection.Split(new[] { (char)007 });
                //pendingOperation = objectsToSave[0].Trim().GetObject<PendingOperation>();
                //var row = objectsToSave[1].Trim().GetObject<MassiveEmissionRow>();
                //try
                //{
                //    using (PendingOperationEntityServiceClient po = new PendingOperationEntityServiceClient("BasicHttpBinding_IPendingOperationEntityService"))
                //    {

                //        //response.AdditionalInformation = string.Format("Test Replica {0}", System.DateTime.Now);
                //        pendingOperation = po.CreatePendingOperation(pendingOperation);
                //    }

                //    using (MassiveUnderwritingServiceClient mr = new MassiveUnderwritingServiceClient("BasicHttpBinding_IMassiveUnderwritingService"))
                //    {

                //        row.Risk.Id = pendingOperation.Id;
                //        ConsumerQuee.MassiveUnderwritingServicesCore.MassiveEmissionRow msvRow = new MassiveEmissionRow();
                //        mr.UpdateMassiveEmissionRows(row);
                //    }

                //}
                //catch
                //{
                //    using (MassiveUnderwritingServiceClient mr = new MassiveUnderwritingServiceClient("BasicHttpBinding_IMassiveUnderwritingService"))
                //    {

                //        row.HasError = true;
                //        mr.UpdateMassiveEmissionRows(row);
                //    }
                //}
            }
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
