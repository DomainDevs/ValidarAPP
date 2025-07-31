//using ApplicationServer.AbstractQuee;
//using ApplicationServer.WrapperServiceWCF;
//using System;
//using Utiles.Extentions;

//namespace ApplicationServer.ConcreteQuee
//{
//    public class FailThirdPartiesLogQuee : TemplateQuee
//    {
//        protected override void ActionQueeToExcecute(Object body)
//        {

//            WrapperObjectQuee objectToProcess = GetObjectToProcess((string)body);
//            if (objectToProcess.JsonToProcess != null)
//            {
//                using (WrapperServiceClient client = new WrapperServiceClient("BasicHttpBinding_IWrapperService"))
//                {
//                    client.RegisterExternalInformationLog(objectToProcess.JsonToProcess);
//                }
//            }
//        }

//        protected override void ExceptionQueActions(Object body, Exception e)
//        {

//            WrapperObjectQuee jsonRetry = GetObjectToProcess((string)body);
//            RemoveMessage = WouldRemoveMessage(jsonRetry);
//            if (RemoveMessage)
//            {
//                var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue(PrincipalQueeName, serialization: "JSON");
//                queue.PutOnQueue(jsonRetry.GetJson());
//            }
//            CreateEventLog(body, e);
//        }
//    }
//}