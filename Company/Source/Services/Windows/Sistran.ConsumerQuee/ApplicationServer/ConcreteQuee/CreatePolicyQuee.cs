using ApplicationServer.AbstractQuee;
using Principal = ApplicationServer.UnderwritingBrokerService;
using Replica = ApplicationServer.UnderwritingBrokerServiceReplica;
using System;
using Utiles.Log;
using ApplicationServer;

namespace ApplicationServer.ConcreteQuee
{
    public class CreatePolicyQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            try
            {
                string businessCollection = (string)body;
                string pendingOperationPolicy = string.Empty;
                if (businessCollection != null)
                {
                    WrapperObjectQuee json = GetObjectToProcess((string)body);
                    try
                    {
                        using (Principal.UnderwritingBrokerServiceClient ub = new Principal.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService"))
                        {
                            pendingOperationPolicy = ub.ExecuteCreatePolicy(json.JsonToProcess);
                        }

                        //if (!String.IsNullOrEmpty(pendingOperationPolicy))
                        //{
                            
                        //    using (Replica.UnderwritingBrokerServiceClient ub = new Replica.UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService1"))
                        //    {
                        //        ub.UpdatePOAndRecordEndorsementOperation(pendingOperationPolicy);
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception e)
            {
                EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}
