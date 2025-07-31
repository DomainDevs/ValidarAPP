using System;
using ApplicationServer.AbstractQuee;
using ApplicationServer.UnderwritingBrokerService;
using Utiles.Log;

namespace ApplicationServer.ConcreteQuee
{
    public class ScoreResponseQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            string businessCollection = (string)body;
            if (businessCollection != null)
            {
                try
                {
                    using (UnderwritingBrokerServiceClient ub = new UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService"))
                    {
                        ub.ProcessResponseFromScoreService(businessCollection);
                    }
                }
                catch (Exception e)
                {
                    EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
                }
            }

        }
    }
}
