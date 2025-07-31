using Newtonsoft.Json;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers
{
    public static class HelperAssembler
    {
        public static ObjetoDestino CreateObjectMappingEqualProperties<ObjetoOrigen, ObjetoDestino>(ObjetoOrigen origen)
        {
            ObjetoDestino destino = Activator.CreateInstance<ObjetoDestino>();
            var objetoOrigenSerializado = SerializeJSON(origen);
            destino = DeserializeJSON<ObjetoDestino>(objetoOrigenSerializado);
            return destino;
        }

        public static T DeserializeJSON<T>(string json)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var respuestaDeserializada = JsonConvert.DeserializeObject<T>(json, jsonSettings);
            return respuestaDeserializada;
        }

        public static string SerializeJSON(object elemento)
        {
            if (elemento == null)
            {
                return string.Empty;
            }
            var cadenaJson = JsonConvert.SerializeObject(elemento);
            return cadenaJson;
        }
    }
}