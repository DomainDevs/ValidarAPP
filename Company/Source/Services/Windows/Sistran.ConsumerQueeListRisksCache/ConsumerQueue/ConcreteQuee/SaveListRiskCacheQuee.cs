using ConsumerListRisksCacheQueue.AbstractQuee;
using ConsumerListRisksCacheQueue.WrapperServiceWCF;
using Newtonsoft.Json;
using System;
using Utiles.Readers;

namespace ConsumerListRisksCacheQueue.ConcreteQuee
{
    public class SaveListRiskCacheQuee : TemplateQuee
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
                    var obj = JsonConvert.DeserializeObject<dynamic>(body.ToString());

                    wrapperServiceClient.ReloadListRisksCache(obj.UserName.ToString());
                }
            }
        }
    }
}