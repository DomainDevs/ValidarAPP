using ConsumerQueueRuleSetCache.WrapperServiceWCF;
using ConsumerRuleSetCacheQueue.AbstractQuee;
using System;
using Utiles.Readers;

namespace ConsumerRuleSetCacheQueue.ConcreteQuee
{
    public class SaveRuleSetCacheQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(object body)
        {
            try
            {
                ConsumeService(body);
            }
            catch (Exception e)
            {
                ExceptionQueActions(body, e);
                System.Threading.Thread.Sleep(ConfigurationReadAsistance.GetConfigurationValue<int>("TimeRetry"));
                ActionQueeToExcecute(body);
            }
        }

        private void ConsumeService(object body)
        {
            if (body != null)
            {
                using (WrapperServiceClient wrapperServiceClient = new WrapperServiceClient("BasicHttpBinding_IWrapperService"))
                {
                    wrapperServiceClient.loadCacheByJson(body.ToString());
                }
            }
        }
    }
}