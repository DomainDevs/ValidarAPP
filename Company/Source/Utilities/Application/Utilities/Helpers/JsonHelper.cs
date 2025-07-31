using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Utilities.Helpers
{
    public static class JsonHelper
    {
        public static T JsonConvertModel<T>(string data, string PropertyName)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(PropertyName))
            {
                throw new ArgumentException("Parametros Vacios");
            }
            JObject objectData = JObject.Parse(data);
            JToken tokenData = objectData[PropertyName];
            return JsonConvert.DeserializeObject<T>(tokenData.ToString());
        }

        public static T DeserializeJson<T>(string serializedObject)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            T respuestaDeserializada = JsonConvert.DeserializeObject<T>(serializedObject, jsonSettings);
            return respuestaDeserializada;
        }

        public static string SerializeObjectToJson(object objectToJson)
        {
            if (objectToJson == null)
            {
                return string.Empty;
            }

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string jsonObject = JsonConvert.SerializeObject(objectToJson, jsonSettings);
            return jsonObject;
        }

        public static T DeserializeJsonByProperty<T>(string serializedObject, string propertyName)
        {
            JObject jObject = JObject.Parse(serializedObject);
            JToken jToken = jObject[propertyName];
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.DeserializeObject<T>(jToken.ToString(), jsonSettings);
        }
    }
}

