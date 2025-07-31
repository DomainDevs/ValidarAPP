using ApplicationServer.AbstractQuee;
using ApplicationServer.UnderwritingBrokerService;
using System;
using Utiles.Log;

namespace ApplicationServer.ConcreteQuee
{
    public class HistoricPoliciesResponseQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            try
            {
                string businessCollection = (string)body;
                if (businessCollection != null)
                {
                    try
                    {
                        using (UnderwritingBrokerServiceClient ub = new UnderwritingBrokerServiceClient("BasicHttpBinding_IUnderwritingBrokerService"))
                        {
                            ub.ProcessResponseFromExperienceServiceHistoricPolicies(businessCollection);
                        }
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
