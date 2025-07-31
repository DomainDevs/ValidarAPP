using System;

namespace Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Extentions
{
    public static class JsonStringExtention
    {
        public static Target GetObject<Target>(this string o)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Target>(o);
            }
            catch (Exception e)
            {
                return default(Target);
            }
        }
    }
}
