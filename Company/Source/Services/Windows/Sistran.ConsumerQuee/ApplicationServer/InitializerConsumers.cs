using ApplicationServer.AbstractQuee;
using ApplicationServer.ConcreteQuee;
using System.Collections.Generic;
using Utiles.Readers;

namespace ApplicationServer
{
    public class InitializerConsumers
    {
        private static List<TemplateQuee> consumers = new List<TemplateQuee>();

        public static void InitializeQuee()
        {
            consumers = new List<TemplateQuee>();

            if (consumers.Count == 0)
            {
                bool checkQuotationQueues = ConfigurationReadAsistance.GetConfigurationValue<bool>("CheckQuotationQueues", false);
                if (checkQuotationQueues)
                {
                    //consumers.Add(new FailSaveQuotationQuee());
                    //consumers.Add(new FailThirdPartiesLogQuee());
                    //consumers.Add(new SaveQuotationQuee());
                    //consumers.Add(new ThirdPartiesLogQuee());
                    consumers.Add(new FailUpdatePendingOperationQuee());
                    consumers.Add(new FailCreatePendingOperationQuee());
                }

                bool SimulateResponses = ConfigurationReadAsistance.GetConfigurationValue<bool>("SimulateResponse", false);

                if (SimulateResponses)
                {
                    //consumers.Add(new ScoreRequestQuee());
                    //consumers.Add(new SimitRequestQuee());
                    //consumers.Add(new HistoricPoliciesRequestQuee());
                    //consumers.Add(new HistoricSinisterRequestQuee());
                }

                consumers.Add(new CreatePendingOperationQuee());
                consumers.Add(new UpdatePendingOperationQuee());
                consumers.Add(new CreatePolicyQuee());

                foreach (var consumer in consumers)
                {
                    consumer.TransactionalSubscribe();
                }
            }
        }
    }
}