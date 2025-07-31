using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Services
{
    public static class ClientServices
    {
        public static string ExecuteRequest(bool hassProxy, string proxy, int port, string uriServices, string validatePersonRequest, TimeSpan timeoutSeconds)
        {


            List<string> errors = new List<string>();

            string urlAPI = uriServices;
            try
            {
                HttpWebResponse response = ExecuteAPIRequest(hassProxy, proxy, port, urlAPI, validatePersonRequest, timeoutSeconds);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JObject jObject = JObject.Parse(reader.ReadToEnd());

                        return (jObject.ToString());
                    }
                }
                else
                {
                    string errorMessage = "Error al conectarse al servicio api   - Response.StatusCode: " + response.StatusCode;
                    errors.Add("Error al conectarse al servicio api   - Response.StatusCode: " + response.StatusCode);
                    //new LogUtilities().RegisterException(errors, null, System.Diagnostics.EventLogEntryType.Error);
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al conectarse al servicio Api ";
                errors.Add("Error al conectarse al servicio Api ");
                //new LogUtilities().RegisterException(errors, ex, System.Diagnostics.EventLogEntryType.Error);
                return errorMessage;
            }
        }

        private static HttpWebResponse ExecuteAPIRequest(bool HassProxy, string proxy, int port, string urlAPI, string parametros, TimeSpan TimeoutSeconds)
        {

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            HttpWebRequest request = WebRequest.Create(urlAPI) as HttpWebRequest;



            if (HassProxy)
            {
                string MyProxyHostString = proxy;
                int MyProxyPort = port;
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);

            }

            //HttpClient client = new HttpClient(handler);
            //var encodedContent = new StringContent(parametros, System.Text.Encoding.UTF8, "application/json");
            //client.Timeout = TimeoutSeconds;

            //Task<HttpResponseMessage> result = client.PostAsync(new Uri(urlAPI), encodedContent);
            //result.Wait();

            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Timeout = TimeoutSeconds.Seconds;

            string postData = parametros;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;


            return response;

        }

    }
}