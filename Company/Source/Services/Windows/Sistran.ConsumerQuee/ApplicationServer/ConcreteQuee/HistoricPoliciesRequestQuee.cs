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
    public class HistoricPoliciesRequestQuee : TemplateQuee
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
                        response = string.Format("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">   <s:Body>      <ns0:HistoricoPolizasResponse xmlns:ns0=\"http://axacolpatria.co/MensajeNegocio/DominioPersona/HistoricoPolizasResponse/1.0\">         <ns0:HistoricoPolizasResult>            <ns0:HistoricoPolizaCexper>               <ns0:CodigoCompania>441</ns0:CodigoCompania>               <ns0:NombreCompania>LIBERTY</ns0:NombreCompania>               <ns0:NumeroPoliza>00000057079</ns0:NumeroPoliza>               <ns0:Orden>0</ns0:Orden>               <ns0:Placa>UCT883</ns0:Placa>               <ns0:Motor>MR20315777W</ns0:Motor>               <ns0:Chasis>SJNFBAJ11Z1182465</ns0:Chasis>               <ns0:FechaVigencia>2016-12-16T00:00:00</ns0:FechaVigencia>               <ns0:FechaFinVigencia>2017-12-16T00:00:00</ns0:FechaFinVigencia>               <ns0:Vigente>SI</ns0:Vigente>               <ns0:CodigoGuia>06406120</ns0:CodigoGuia>              <ns0:Marca>NISSAN</ns0:Marca>               <ns0:Clase>CAMIONETA PASAJ.</ns0:Clase>               <ns0:Tipo>QASHQAI +2 [2] 2.0L 2WD TP 2000CC 4X2 2</ns0:Tipo>               <ns0:Modelo>2015</ns0:Modelo>               <ns0:Servicio>PARTICULAR</ns0:Servicio>               <ns0:TipoDocumentoAsegurado>CÉDULA DE CIUDADANIA</ns0:TipoDocumentoAsegurado>               <ns0:NumeroDocumento>51692525</ns0:NumeroDocumento>               <ns0:Asegurado>FLOR STELLA RODRIGUEZ GARCIA</ns0:Asegurado>               <ns0:ValorAsegurado>71500000</ns0:ValorAsegurado>               <ns0:PTD>SI</ns0:PTD>               <ns0:PPD>SI</ns0:PPD>               <ns0:PH>SI</ns0:PH>               <ns0:PPH>SI</ns0:PPH>               <ns0:RC>SI</ns0:RC>               <ns0:TipoCruce>Cruzo por identificacion y tipo de identificación con placa diferente - 182/1</ns0:TipoCruce>            </ns0:HistoricoPolizaCexper>            <ns0:HistoricoPolizaCexper>               <ns0:CodigoCompania>431</ns0:CodigoCompania>               <ns0:NombreCompania>AXA COLPATRIA</ns0:NombreCompania>               <ns0:NumeroPoliza>1002507</ns0:NumeroPoliza>               <ns0:Orden>0</ns0:Orden>               <ns0:Placa>BKE308</ns0:Placa>               <ns0:Motor>KA24816757X</ns0:Motor>              <ns0:Chasis>JNLCCUD22Z0001043</ns0:Chasis>               <ns0:FechaVigencia>2016-10-01T00:00:00</ns0:FechaVigencia>               <ns0:FechaFinVigencia>2017-10-01T00:00:00</ns0:FechaFinVigencia>               <ns0:Vigente>SI</ns0:Vigente>               <ns0:CodigoGuia>06421008</ns0:CodigoGuia>               <ns0:Marca>NISSAN</ns0:Marca>               <ns0:Clase>PICKUP DOBLE CAB</ns0:Clase>               <ns0:Tipo>D22 DX MT 2400CC 4X4 S</ns0:Tipo>               <ns0:Modelo>1998</ns0:Modelo>               <ns0:Servicio>PARTICULAR</ns0:Servicio>               <ns0:TipoDocumentoAsegurado>CÉDULA DE CIUDADANIA</ns0:TipoDocumentoAsegurado>               <ns0:NumeroDocumento>51692525</ns0:NumeroDocumento>               <ns0:Asegurado>RODRIGUEZ GARCIA FLOR STELLA</ns0:Asegurado>               <ns0:ValorAsegurado>14200000</ns0:ValorAsegurado>               <ns0:PTD>SI</ns0:PTD>               <ns0:PPD>SI</ns0:PPD>               <ns0:PH>NO</ns0:PH>               <ns0:PPH>NO</ns0:PPH>               <ns0:RC>SI</ns0:RC>               <ns0:TipoCruce>Cruzo por identificacion y tipo de identificación con placa diferente - 258/1</ns0:TipoCruce>            </ns0:HistoricoPolizaCexper>         </ns0:HistoricoPolizasResult>         <ns0:InformacionRespuesta>            <ns0:criticidad>Informacion</ns0:criticidad>            <ns0:idTransaccion>39c40096-2725-473d-a5fa-ff7caa1707cc</ns0:idTransaccion>            <ns0:rtaCodCanal>0</ns0:rtaCodCanal>            <ns0:rtaDescCanal>Procedimiento Realizado Correctamente</ns0:rtaDescCanal>            <ns0:metodo>ConsultarHistPolizas</ns0:metodo>            <ns0:idCorrelacionConsumidor>{0}</ns0:idCorrelacionConsumidor>         </ns0:InformacionRespuesta>      </ns0:HistoricoPolizasResponse>   </s:Body></s:Envelope>", result.First());
                    }
                    else
                    {
                        response = string.Format("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">http://axacolpatria.co/MensajeNegocio/DominioPersona/HistoricoPolizasResponse/1.0/</Action></s:Header><s:Body><s:Fault> <faultcode>ET-408</faultcode>  <faultstring>Ocurrio un error de TimeOut al consumir el servicio externo, por favor intente más tarde</faultstring>  <detail>    <categoria>Error Técnico</categoria>    <idTransaccion>5c0a9fca-c315-4ead-a77a-31993b3e0d74</idTransaccion>    <idCorrelacionConsumidor>{0}</idCorrelacionConsumidor>  </detail></s:Fault></s:Body></s:Envelope>", result.First());
                    }
                    

                    var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("HistoricPoliciesResponseQuee", serialization: "None");
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
