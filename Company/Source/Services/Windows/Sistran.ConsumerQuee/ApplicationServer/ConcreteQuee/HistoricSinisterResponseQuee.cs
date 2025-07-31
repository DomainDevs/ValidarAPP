using ApplicationServer.AbstractQuee;
using ApplicationServer.UnderwritingBrokerService;
using System;
using Utiles.Log;

namespace ApplicationServer.ConcreteQuee
{
    class HistoricSinisterResponseQuee : TemplateQuee
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
                            ub.ProcessResponseFromExperienceServiceHistoricSinister(businessCollection);
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
