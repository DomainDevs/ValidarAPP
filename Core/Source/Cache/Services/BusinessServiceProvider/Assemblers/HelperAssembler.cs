using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Assemblers
{
    public class HelperAssembler
    {
        public static T DeserializeJson<T>(string serializedObject)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            T respuestaDeserializada = JsonConvert.DeserializeObject<T>(serializedObject, jsonSettings);
            return respuestaDeserializada;
        }
    }
}
