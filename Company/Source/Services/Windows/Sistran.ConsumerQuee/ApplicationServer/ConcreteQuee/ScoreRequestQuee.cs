using ApplicationServer.AbstractQuee;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Utiles.Log;

namespace ApplicationServer.ConcreteQuee
{
    public class ScoreRequestQuee : TemplateQuee
    {
        protected override void ActionQueeToExcecute(Object body)
        {
            try
            {
                string businessCollection = (string)body;
                string[] header = null;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(businessCollection);
                XmlElement root = xml.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("//idCorrelacionConsumidor");

                List<string> result = new List<string>();
                foreach (XmlNode item in nodes)
                {
                    result.Add(item.InnerText);
                }

                //string response = $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">http://axacolpatria.co/MensajeNegocio/DominioPersona/consultarRiesgoResp/1.0/</Action></s:Header><s:Body><ns0:ConsultaRiesgoResp xmlns:ns0=\"http://axacolpatria.co/MensajeNegocio/DominioPersona/consultarRiesgoResp/1.0\" xmlns:ns4=\"http://axacolpatria.co/DPW/Servicios/Negocio/Producto/2017/03/31\" xmlns:ns8=\"http://axacolpatria.co/DPW/Servicios/Negocio/Servicios/2017/03/31\" xmlns:enc=\"http://axacolpatria.co/Servicios/Base/EncabezadosSOA\" xmlns:ns3=\"http://axacolpatria.co/DPW/Servicios/Negocio/Vehiculo/2017/03/31\" xmlns:dat=\"http://AXAColpatria.Datacredito.Esquemas.RiesgoDatacredito/2017/06/22\" xmlns:ns6=\"http://axacolpatria.co/DPW/xsd/base/contacto/2017/03/31\" xmlns:ns5=\"http://axacolpatria.co/DPW/xsd/base/metodopago/2017/03/31\" xmlns:ns11=\"http://axacolpatria.co/DPW/Servicios/Negocio/Contrato/2017/03/31\" xmlns:ns2=\"http://axacolpatria.co/DPW/xsd/base/2017/03/31\" xmlns:snp=\"http://axacolpatria.co/DPW/Servicios/Negocio/Persona/2017/03/31\" xmlns:ns9=\"http://axacolpatria.co/DPW/Servicios/Negocio/Documento/2017/03/31\" xmlns:ns1=\"http://axacolpatria.co/DPW/Servicios/Negocio/Siniestro/2017/03/31\" xmlns:ns7=\"http://axacolpatria.co/DPW/Servicios/Negocio/Persona/Cliente/2017/03/31\" xmlns:ns10=\"http://axacolpatria.co/DPW/xsd/base/direccion/2017/03/31\"><HEADER><criticidad>Error</criticidad><idCorrelacionConsumidor>{result.First()}</idCorrelacionConsumidor><idTransaccion>e4320f73-7715-43ee-9c4c-38de26fa00b8</idTransaccion><rtaCodCanal>408</rtaCodCanal><rtaCodHost>408</rtaCodHost><rtaDescCanal>Ocurrio un error de TimeOut al consumir el servicio externo, por favor intente más tarde</rtaDescCanal><rtaDescHost>Ocurrio un error de TimeOut al consumir el servicio externo, por favor intente más tarde</rtaDescHost><metodo>InterfaceConsultarRiesgo</metodo><proceso>Scope_ConsultaDatacredito</proceso><categoria>Error Técnico</categoria></HEADER></ns0:ConsultaRiesgoResp></s:Body></s:Envelope>";
                string response = $"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><ns0:ConsultaRiesgoResp xmlns:ns0=\"http://axacolpatria.co/MensajeNegocio/DominioPersona/consultarRiesgoResp/1.0\" xmlns:ns4=\"http://axacolpatria.co/DPW/Servicios/Negocio/Producto/2017/03/31\" xmlns:ns8=\"http://axacolpatria.co/DPW/Servicios/Negocio/Servicios/2017/03/31\" xmlns:enc=\"http://axacolpatria.co/Servicios/Base/EncabezadosSOA\" xmlns:ns3=\"http://axacolpatria.co/DPW/Servicios/Negocio/Vehiculo/2017/03/31\" xmlns:dat=\"http://AXAColpatria.Datacredito.Esquemas.RiesgoDatacredito/2017/06/22\" xmlns:ns6=\"http://axacolpatria.co/DPW/xsd/base/contacto/2017/03/31\" xmlns:ns5=\"http://axacolpatria.co/DPW/xsd/base/metodopago/2017/03/31\" xmlns:ns11=\"http://axacolpatria.co/DPW/Servicios/Negocio/Contrato/2017/03/31\" xmlns:ns2=\"http://axacolpatria.co/DPW/xsd/base/2017/03/31\" xmlns:snp=\"http://axacolpatria.co/DPW/Servicios/Negocio/Persona/2017/03/31\" xmlns:ns9=\"http://axacolpatria.co/DPW/Servicios/Negocio/Documento/2017/03/31\" xmlns:ns1=\"http://axacolpatria.co/DPW/Servicios/Negocio/Siniestro/2017/03/31\" xmlns:ns7=\"http://axacolpatria.co/DPW/Servicios/Negocio/Persona/Cliente/2017/03/31\" xmlns:ns10=\"http://axacolpatria.co/DPW/xsd/base/direccion/2017/03/31\"><HEADER><criticidad>Información</criticidad><idCorrelacionConsumidor>{result.First()}</idCorrelacionConsumidor><idTransaccion>1</idTransaccion><rtaCodCanal>0</rtaCodCanal><rtaCodHost>0</rtaCodHost><rtaDescCanal>Procedimiento Realizado Correctamente</rtaDescCanal><rtaDescHost>Procedimiento Realizado Correctamente</rtaDescHost><metodo>InterfaceConsultaRiesgo</metodo><proceso>Scope_ConsultaDatacredito</proceso><categoria/></HEADER><BODY><personaDetalle><contactoPrincipal/><informacionFinanciera><puntajeCrediticio><tipo>10</tipo><puntaje>0.0</puntaje><clasificacion>P</clasificacion></puntajeCrediticio></informacionFinanciera></personaDetalle><RespuestaDataCredito><RespuestaPersonalisadaDatacredito nombre=\"PUNTAJE_SCORE_ACIERTA\" valor=\"444\"/></RespuestaDataCredito></BODY></ns0:ConsultaRiesgoResp></s:Body></s:Envelope>";
                var scoreQueue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("ScoreResponseQuee", routingKey: "ScoreResponseQuee", serialization: "None");
                scoreQueue.PutOnQueue(response);
            }
            catch (Exception e)
            {
                EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }

        }
    }
}
