using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServer.Helpers
{
    static class ApiProxy
    {
        /// <summary>
        /// Realiza peticiones GET
        /// </summary>
        /// <param name="url">Url de llamado</param>
        /// <param name="parameters">especificacion de parametros</param>
        /// <returns>tipo de objeto esperado</returns>
        public static async Task GetAsync(string baseUrl, string url, Dictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                url = BuildUrl(url + "Get", parameters);
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    await ResolveResultApi(response);
                }
            }
        }

        /// <summary>
        ///  Realiza peticiones POST
        /// </summary>
        /// <typeparam name="T">tipo de objeto a retornar</typeparam>
        /// <param name="url">Url de llamado</param>
        /// <param name="parameters">especificacion de parametros</param>
        /// <param name="apiContentType">Tipo del contenido de los parametros</param>
        /// <returns>tipo de objeto esperado</returns>
        public static async Task PostAsync(string baseUrl, string url, dynamic parameters)
        {
            HttpContent contentParameters = new StringContent(parameters, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                // client.Timeout = new TimeSpan(Convert.ToInt64(ConfigurationManager.AppSettings["TimeOut"]));
                client.BaseAddress = new Uri(baseUrl);
                using (HttpResponseMessage response = await client.PostAsync(url, contentParameters))
                {
                    await ResolveResultApi(response);
                }
            }
        }

        /// <summary>
        /// Crea la url absoluta del api
        /// </summary>
        /// <param name="url">url relativa del api</param>
        /// <param name="parameters">lista deparametros enviados por url</param>
        /// <returns>Url absoluta del API</returns>
        private static string BuildUrl(string url, Dictionary<string, string> parameters)
        {
            int count = 0;
            foreach (KeyValuePair<string, string> item in parameters)
            {
                url += count == 0 ? "?" : "&";
                url += item.Key + "=" + item.Value;
                count++;
            }

            return url;
        }

        /// <summary>
        /// Deserializa la respuesta de la API
        /// </summary>
        /// <param name="response">Respuesta del llamado del API</param>
        /// <exception cref="ApiException">Se se presenta una excepcion controlada</exception>
        /// <returns>Tarea que resulve el API</returns>
        private static async Task ResolveResultApi(HttpResponseMessage response)
        {
            using (HttpContent content = response.Content)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string a = await content.ReadAsStringAsync();
                        break;

                    case HttpStatusCode.BadRequest:
                        throw new Exception(await content.ReadAsStringAsync());

                    default:
                        throw new ArgumentOutOfRangeException(nameof(response.StatusCode), response.StatusCode, null);
                }
            }
        }
    }
}
