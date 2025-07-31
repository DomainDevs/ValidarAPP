using ApplicationServer.AbstractQuee;
using ApplicationServer.ConcreteQuee;
using System.Collections.Generic;

namespace ApplicationServer
{
    public class InitializerConsumers
    {
        private static List<TemplateQuee> consumers = new List<TemplateQuee>();

        public static void InitializeQuee()
        {
            var listRisk = new ListRiskQueue();
            listRisk.TransactionalSubscribe();
            var recordListRisk = new RecordListRiskQueue();
            recordListRisk.TransactionalSubscribe();
            var listRiskOfac = new ListRiskOfacQueue();
            listRiskOfac.TransactionalSubscribe();
            var recordListRiskOfac = new RecordListRiskOfacQueue();
            recordListRiskOfac.TransactionalSubscribe();
        }
    }
}