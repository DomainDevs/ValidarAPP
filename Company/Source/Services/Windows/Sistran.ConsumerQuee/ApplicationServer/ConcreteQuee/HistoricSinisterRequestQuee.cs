using ApplicationServer.AbstractQuee;
using ApplicationServer.Helpers;
using Sistran.Core.Framework.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Utiles.Extentions;
using Utiles.Log;

namespace ApplicationServer.ConcreteQuee
{
    class HistoricSinisterRequestQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            try
            {
                string businessCollection = (string)body;
                XElement xmlDocumentWithoutNs = XmlHelper.RemoveAllNamespaces(XElement.Parse(businessCollection));
                XmlNodeList nodes = XmlHelper.ToXmlElement(xmlDocumentWithoutNs).SelectNodes("//idCorrelacionConsumidor");

                List<string> result = new List<string>();
                foreach (XmlNode item in nodes)
                {
                    result.Add(item.InnerText);
                }
                if (!string.IsNullOrEmpty(businessCollection))
                {
                    string response = string.Empty;
                    if (DateTime.Now.Second > 30)
                    {
                        response = $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">http://axacolpatria.co/MensajeNegocio/DominioPersona/HistoricoSiniestrosResponse/1.0/</Action></s:Header><s:Body><s:Fault><faultcode>ET-100</faultcode><faultstring>La consulta no esta disponible por el momento, por favor intente más tarde</faultstring><detail><categoria>Error Técnico</categoria><idTransaccion>0f4ed27b-b37d-46eb-87d4-cfaddd0689ca</idTransaccion><idCorrelacionConsumidor>{result.First()}</idCorrelacionConsumidor></detail></s:Fault></s:Body></s:Envelope>";
                    }
                    else
                    {
                        response = $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">http://axacolpatria.co/MensajeNegocio/DominioPersona/HistoricoSiniestrosResponse/1.0/Canonico</Action></s:Header><s:Body><ns0:HistoricoSiniestrosResponse xmlns:ns0=\"http://axacolpatria.co/MensajeNegocio/DominioPersona/HistoricoSiniestrosResponse/1.0\"><ns0:HistoricoSiniestrosResult/><ns0:InformacionRespuesta><ns0:criticidad>Informacion</ns0:criticidad><ns0:idTransaccion>1fd8fac6-79d5-4be1-a63c-c5334283583d</ns0:idTransaccion><ns0:rtaCodCanal>0</ns0:rtaCodCanal><ns0:rtaDescCanal>Procedimiento Realizado Correctamente</ns0:rtaDescCanal><ns0:metodo>ConsultarHistSiniestros</ns0:metodo><ns0:idCorrelacionConsumidor>{result.First()}</ns0:idCorrelacionConsumidor></ns0:InformacionRespuesta></ns0:HistoricoSiniestrosResponse></s:Body></s:Envelope>";
                    }

                    var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("HistoricSinisterResponseQuee", serialization: "None");
                    queue.PutOnQueue(response);
                }
            }
            catch (Exception e)
            {
                EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}
