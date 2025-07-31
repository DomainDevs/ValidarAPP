using ConsumerQueueAudit.AbstractQuee;
using ConsumerQueueAudit.WrapperServiceWCF;

namespace ConsumerQueueAudit.ConcreteQuee
{
    public class FailAuditQueue : TemplateQuee
    {
        protected override void ActionQueeToExcecute(object body)
        {
            if (body != null)
            {
                using (WrapperServiceClient client = new WrapperServiceClient("BasicHttpBinding_IWrapperService"))
                {
                    client.AuditData(body.ToString());
                }
            }
        }
    }
}