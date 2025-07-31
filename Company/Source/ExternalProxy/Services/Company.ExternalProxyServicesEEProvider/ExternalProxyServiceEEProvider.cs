using Sistran.Company.Application.ExternalProxyServices;
using Sistran.Company.Application.ExternalProxyServices.DatacreditoBizTalkService;
using ExAs = Sistran.Company.Application.ExternalProxyServices.ExperienciaAseguradoBiztalkService;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Company.Application.ExternalProxyServicesEEProvider.Assemblers;
using Sistran.Company.Application.ExternalProxyServicesEEProvider.Business;
using Sistran.Company.Application.ExternalProxyServicesEEProvider.DAOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ExIn = Sistran.Company.Application.ExternalProxyServices.InfraccionesBizTalkService;
using Newtonsoft.Json;
using CONSULTASISA = Sistran.Company.Application.ExternalProxyServices.ConsultaSisa;
using CONSULTACEXPER = Sistran.Company.Application.ExternalProxyServices.ConsultaCexper;
using System.Net.Http;
using System.Text;
using Sistran.Company.Application.ExternalProxyServicesEEProvider.Services;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider
{
    public class ExternalProxyServiceEEProvider : IExternalProxyService
	{

        /// <summary>
        /// Metodo que realiza el grabado del log de consulta a externos.
        /// </summary>
        /// <param name="externalInfromationLog">datos de grabado</param>
        /// <returns></returns>
        public ExternalInformationLogDTO RegisterExternalInformationLog(ExternalInformationLogDTO externalInfromationLogDTO)
		{
			try
			{
				ExternalInformationLogDAO externalInformationLogDAO = new ExternalInformationLogDAO();
				externalInformationLogDAO.RegisterExternalInformationLog(externalInfromationLogDTO);
			}
			catch (Exception) { }
			return externalInfromationLogDTO;
		}

		/// <summary>
		/// Metodo que realiza la consulta de datacredito a través de Biztalk.
		/// </summary>
		/// <param name="RequestScoreCredit">Solicitud score crediticio</param>
		/// <returns>Objeto que contiene el resultado de la consulta de crediscore.</returns>
		public ResponseScoreCredit ExecuteWebServiceScoreCreditBiztalk(RequestScoreCredit requestScoreCredit)
		{
			ResponseScoreCredit responseScoreCredit = new ResponseScoreCredit();
			EncabezadoEntrada headerMessage = ModelAssembler.CreateHeaderMessageRequest(requestScoreCredit.GuidProcess);
			BODYType bodyMessage = ModelAssembler.CreateBodyMessageRequest(requestScoreCredit);
			responseScoreCredit.Date = headerMessage.peticionFecha;

			ConsultaRiesgoReqType consultaRiesgoReqType = new ConsultaRiesgoReqType();
			consultaRiesgoReqType.HEADER = headerMessage;
			consultaRiesgoReqType.BODY = bodyMessage;

			int codError = 0;
			Stopwatch stopWatch = new Stopwatch();
			try
			{
				stopWatch.Start();
				ConsultaRiesgoRespType consultaRiesgoRespType = null;
				using (DatacreditoClient datacreditoClient = new DatacreditoClient())
				{
					consultaRiesgoRespType = datacreditoClient.ConsultarRiesgo(consultaRiesgoReqType);
				}
				stopWatch.Stop();
				responseScoreCredit = ModelAssembler.CreateResponseScoreCredit(consultaRiesgoRespType);
				responseScoreCredit.Date = headerMessage.peticionFecha;
				if (string.IsNullOrEmpty(responseScoreCredit.Score))
				{
					codError = responseScoreCredit.Error.Code;
					throw new Exception(responseScoreCredit.Error.ErrorMessage);
				}
				BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarRiesgo", consultaRiesgoReqType.HEADER.idCorrelacionConsumidor, consultaRiesgoReqType);
			}
			catch (Exception ex)
			{
				stopWatch.Stop();
				responseScoreCredit.Error = new Error
				{
					IsError = true,
					ErrorMessage = ex.Message,
					Code = codError
				};
				BusinessLog.RegisterFailRequest(stopWatch, "ConsultarRiesgo", consultaRiesgoReqType.HEADER.idCorrelacionConsumidor, ex.Message, consultaRiesgoReqType);
			}

			return responseScoreCredit;
		}

		public ResponseGoodExperienceYear CalculateGoodExperienceYears(RequestGoodExperienceYear requestGoodExperienceYear)
		{
			ResponseGoodExperienceYear responseGoodExperienceYear = new ResponseGoodExperienceYear();
			try
			{

                string isEffectiveDate = "NO";
				DateTime? maximumVigency = null;
				responseGoodExperienceYear.GoodExpNumRate = "0P";
				responseGoodExperienceYear.IsEffectiveDate = isEffectiveDate;
				if (string.IsNullOrEmpty(requestGoodExperienceYear.LicensePlate) || !ImplementWSFasecolda())
				{
					return responseGoodExperienceYear;
				}

				BusinessFasecolda businessFasecolda = new BusinessFasecolda();
				int documentTypeFasecolda = businessFasecolda.GetTypeDocumentFasecolda(requestGoodExperienceYear.IdCardTypeCode);

                


                ExAs.HistoricoPolizasResponse historicoPolizasResponse = null;
				ExAs.HistoricoSiniestrosResponse historicoSiniestrosResponse = null;
				ExAs.HistoricoPolizas historicoPolizas = ModelAssembler.CreateHistoricoPolizas(documentTypeFasecolda, requestGoodExperienceYear.IdCardNo, requestGoodExperienceYear.LicensePlate);
				ExAs.HistoricoSiniestros historicoSiniestros = ModelAssembler.CreateHistoricoSiniestros(documentTypeFasecolda, requestGoodExperienceYear.IdCardNo, requestGoodExperienceYear.LicensePlate);

                Parallel.Invoke
                (
                    () => { historicoPolizasResponse = businessFasecolda.InvokeMethod(historicoPolizas, requestGoodExperienceYear.GuidProcess); },
                    () => { historicoSiniestrosResponse = businessFasecolda.InvokeMethod(historicoSiniestros, requestGoodExperienceYear.GuidProcess); }
                );

                Stopwatch stopWatch = new Stopwatch();
				if (historicoPolizasResponse.HistoricoPolizasResult == null
					|| historicoPolizasResponse.HistoricoPolizasResult.Length == 0)
				{
					return responseGoodExperienceYear;
				}

				responseGoodExperienceYear.PoliciesHistorical = ModelAssembler.CreateResponsePoliciesHistorical(historicoPolizasResponse.HistoricoPolizasResult);
				responseGoodExperienceYear.HistoricalSinister = ModelAssembler.CreateResponseHistoricalSinister(historicoSiniestrosResponse.HistoricoSiniestrosResult);

				int resetYears = requestGoodExperienceYear.ResetYears;

				IList<ExAs.HistoricoPolizaCexper> tempHistoricoPolicy = historicoPolizasResponse.HistoricoPolizasResult;
				DateTime maxDate = tempHistoricoPolicy.AsEnumerable()
													  .Max(x => x.FechaFinVigencia);
				isEffectiveDate = tempHistoricoPolicy.OrderByDescending(t => t.FechaFinVigencia).First().Vigente;
				responseGoodExperienceYear.IsEffectiveDate = isEffectiveDate;
				maximumVigency = tempHistoricoPolicy.OrderByDescending(t => t.FechaFinVigencia).First().FechaFinVigencia;
				responseGoodExperienceYear.MaximumVigency = maximumVigency;

				bool shouldReturnResetExperienceYears = BusinessFasecolda.ResetGoodExperienceYearWhenIsNotInForce(resetYears, responseGoodExperienceYear, maxDate);
				if (shouldReturnResetExperienceYears)
				{
					return responseGoodExperienceYear;
				}

				responseGoodExperienceYear = BusinessFasecolda.BuildGoodExperienceYears(historicoSiniestrosResponse.HistoricoSiniestrosResult, resetYears, tempHistoricoPolicy);
				responseGoodExperienceYear.IsEffectiveDate = isEffectiveDate;
				responseGoodExperienceYear.MaximumVigency = maximumVigency;
			}
			catch (Exception ex)
			{
				responseGoodExperienceYear.Error = new Error
				{
					IsError = true,
					ErrorMessage = ex.Message
				};
            }
            return responseGoodExperienceYear;
        }

		public bool ImplementWSFasecolda()
		{
			try
			{
				return Convert.ToBoolean(ConfigurationManager.AppSettings["ImplementWebServices"].ToString());
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool ImplementWSDataCredit()
		{
			try
			{
				return Convert.ToBoolean(ConfigurationManager.AppSettings["ImplementWSBizTalkDataCredito"].ToString());
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool ImplementWSSimit()
		{
			try
			{
				return Convert.ToBoolean(ConfigurationManager.AppSettings["ImplementWSInfringementSimitBizTalk"].ToString());
			}
			catch (Exception)
			{
				return false;
			}
		}

		public ResponseInfringement ExecuteWebServiceBizTalkInfringementSimit(int DocumentType, String DocumentNumber, Guid Guid)
		{
			ResponseInfringement responseInfringement = new ResponseInfringement();
			ExIn.consultarInfraccionesReq request = new ExIn.consultarInfraccionesReq();
			ExternalInfrigement externalInfrigement = new ExternalInfrigement();
			request.BODY = ModelAssembler.GetBodyRequestForBizTalkIngementSimit(DocumentType, DocumentNumber);
			request.HEADER = ModelAssembler.GetHeaderForBizTalkIngementSimit(Guid);

			Stopwatch stopWatch = new Stopwatch();

			try
			{
				stopWatch.Start();
				ExIn.consultarInfraccionesResp response = null;
				using (ExIn.InfraccionClient biztalkInfraccionClient = new ExIn.InfraccionClient())
				{
					response = biztalkInfraccionClient.ConsultarInfraccion(request);
				}
				stopWatch.Stop();
				responseInfringement = ModelAssembler.CreateResponseInfringement(response);
				BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarInfraccion", request.HEADER.idCorrelacionConsumidor, request);
			}
			catch (Exception ex)
			{
				stopWatch.Stop();
				responseInfringement.Error = new Error
				{
					IsError = true,
					ErrorMessage = ex.Message
				};
				BusinessLog.RegisterFailRequest(stopWatch, "ConsultarInfraccion", request.HEADER.idCorrelacionConsumidor, ex.Message, request);
			}

			return responseInfringement;
		}

        public ResponseCexper ExecuteWebServiceCEXPERQueryBiztalk(RequestCexper requestCexper)
        {
            //Cliente del servicio
            ResponseCexper responseCexper = new ResponseCexper();
            CONSULTACEXPER.UsuarioHeader headerMessage = ModelAssembler.CreateHeaderMessageConsultaCEXPERRequest();
            CONSULTASISA.UsuarioHeader headerMessageSisa = ModelAssembler.CreateHeaderMessageConsultaSisaRequest(requestCexper.GuidProcess);
            
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                CONSULTACEXPER.HistoricoPolizaCexperV201306[] cexperHistoricoPoliza = null;
                CONSULTACEXPER.HistoricoSiniestroCexperV201306[] cexperHistoricoSiniestro = null;
                CONSULTASISA.ComparendoSIMITV201401[] simitResponse = null;
                using (CONSULTACEXPER.CexperSoapClient cexperClient = new CONSULTACEXPER.CexperSoapClient())
                {
                    cexperHistoricoPoliza = cexperClient.HistoricoPolizasV201306(headerMessage, requestCexper.Plate, Convert.ToString(requestCexper.DocumentType), requestCexper.DocumentNumber);
                    if (cexperHistoricoPoliza != null)
                    {
                        using (CONSULTASISA.SisaSoapClient sisaClient = new CONSULTASISA.SisaSoapClient())
                        {
                            simitResponse = sisaClient.ConsultaExternaSIMITV201401(headerMessageSisa,/* //sisaPoliciesResponse[0].TipoDocumentoTomador */"1", cexperHistoricoPoliza[0].NumeroDocumento);
                        }
                    }
                    cexperHistoricoSiniestro = cexperClient.HistoricoSiniestrosV201306(headerMessage, requestCexper.Plate, Convert.ToString(requestCexper.DocumentType), requestCexper.DocumentNumber);
                }
                stopWatch.Stop();
                BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarCEXPER", requestCexper.GuidProcess.ToString(), requestCexper);
                responseCexper = new ResponseCexper()
                {
                    PoliciesInfo = ModelAssembler.CreateResponsePoliciesInfo(cexperHistoricoPoliza),
                    SinisterInfo = ModelAssembler.CreateResponseSinisterInfo(cexperHistoricoSiniestro),
                    Simit = ModelAssembler.CreateResponseSimit(simitResponse)
                };
                return responseCexper;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                if (ex.Data.Count == 0)
                {
                    responseCexper = new ResponseCexper()
                    {
                        PoliciesInfo = null,
                        SinisterInfo = null,
                        Simit = null
                    };
                    return responseCexper;
                }
                else
                {
                    return null;
                }
            }

        }


        public ResponseFasecoldaSISA ExecuteWebServiceSISAQueryBiztalk(RequestFasecoldaSISA requestFasecoldaSISA)
		{
            
            //Cliente del servicio
			ResponseFasecoldaSISA responseSisa = new ResponseFasecoldaSISA();
            CONSULTASISA.UsuarioHeader headerMessage = ModelAssembler.CreateHeaderMessageConsultaSisaRequest(requestFasecoldaSISA.GuidProcess);

			Stopwatch stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                CONSULTASISA.GuiaSisa sisaGuiedResponse = null;
                CONSULTASISA.HistoricoPolizaSisaV201306[] sisaPoliciesResponse = null;
                CONSULTASISA.HistoricoSiniestroSisaV201306[] sisaClaimsResponse = null;
                CONSULTASISA.WMISisa sisaVinResponse = null;
                using (CONSULTASISA.SisaSoapClient sisaClient = new CONSULTASISA.SisaSoapClient())
                {
                    sisaGuiedResponse = sisaClient.InformacionGuias(headerMessage, requestFasecoldaSISA.Plate, requestFasecoldaSISA.Engine, requestFasecoldaSISA.Chassis);
                    sisaPoliciesResponse = sisaClient.HistoricoPolizasV201306(headerMessage, requestFasecoldaSISA.Plate, requestFasecoldaSISA.Engine, requestFasecoldaSISA.Chassis);
                    sisaClaimsResponse = sisaClient.HistoricoSiniestrosV201306(headerMessage, requestFasecoldaSISA.Plate, requestFasecoldaSISA.Engine, requestFasecoldaSISA.Chassis);
                    sisaVinResponse = sisaClient.InformacionCodigosWMI(headerMessage, requestFasecoldaSISA.Plate, requestFasecoldaSISA.Engine, requestFasecoldaSISA.Chassis);
                }
                stopWatch.Stop();
                BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarSISA", requestFasecoldaSISA.GuidProcess.ToString(), requestFasecoldaSISA);
                responseSisa = new ResponseFasecoldaSISA()
                {
                    PoliciesInfo = ModelAssembler.CreateResponsePoliciesInfo(sisaPoliciesResponse),
                    Claims = ModelAssembler.CreateResponseClaims(sisaClaimsResponse),
                    GuideInfo = ModelAssembler.CreateResponseGuideInfo(sisaGuiedResponse),
                    VINVerification = sisaVinResponse != null ? ModelAssembler.CreateResponseVINVerification(sisaVinResponse) : new ResponseVINVerification()
                };
                return responseSisa;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                if (ex.Data.Count == 0)
                {
                    responseSisa = new ResponseFasecoldaSISA()
                    {
                        PoliciesInfo = null,
                        Claims = null,
                        GuideInfo = null,
                        VINVerification = null
                    };
                    return responseSisa;
                }
                else
                {
                    return null;
                }
            }
		}

        //public ResponseFasecoldaSISA ExecuteWebServiceSISATes(RequestFasecoldaSISA requestFasecoldaSISA)
        //{
        //    CONSULTASISA.HistoricoPolizaSisa 
        //}

        public ResponseFasecoldaSISA ExecuteWebServiceSISA(RequestFasecoldaSISA requestFasecoldaSISA)
        {

            //Cliente del servicio
            ResponseFasecoldaSISA responseSisa = new ResponseFasecoldaSISA();
            CONSULTASISA.UsuarioHeader headerMessage = ModelAssembler.CreateHeaderMessageConsultaSisaRequest(requestFasecoldaSISA.GuidProcess);

            Stopwatch stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                CONSULTASISA.HistoricoPolizaSisa[] result = null;
                using (CONSULTASISA.SisaSoapClient sisaClient = new CONSULTASISA.SisaSoapClient())
                {

                    result = sisaClient.HistoricoPolizas(headerMessage, requestFasecoldaSISA.Plate, requestFasecoldaSISA.Engine, requestFasecoldaSISA.Chassis);
                }
                stopWatch.Stop();
                BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarSISA", requestFasecoldaSISA.GuidProcess.ToString(), requestFasecoldaSISA);
                responseSisa = new ResponseFasecoldaSISA()
                {
                    PoliciesInfo = ModelAssembler.CreateResponseHistoricoPolicies(result)
                };
                return responseSisa;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                if (ex.Data.Count == 0)
                {
                    responseSisa = new ResponseFasecoldaSISA()
                    {
                        PoliciesInfo = null,
                        Simit = null,
                        Claims = null,
                        GuideInfo = null,
                        VINVerification = null
                    };
                    return responseSisa;
                }
                else
                {
                    return null;
                }
            }
        }

        public ResponseCexper ExecuteWebServiceCEXPER(RequestCexper requestCexper)
        {
            //Cliente del servicio
            ResponseCexper responseCexper = new ResponseCexper();
            CONSULTACEXPER.UsuarioHeader headerMessage = ModelAssembler.CreateHeaderMessageConsultaCEXPERRequest();
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                CONSULTACEXPER.HistoricoPolizaCexperV201306[] cexperHistoricoPoliza = null;
                CONSULTACEXPER.HistoricoSiniestroCexperV201306[] cexperHistoricoSiniestro = null;
                using (CONSULTACEXPER.CexperSoapClient cexperClient = new CONSULTACEXPER.CexperSoapClient())
                {
                    cexperHistoricoPoliza = cexperClient.HistoricoPolizasV201306(headerMessage, requestCexper.Plate, Convert.ToString(requestCexper.DocumentType), requestCexper.DocumentNumber);
                    cexperHistoricoSiniestro = cexperClient.HistoricoSiniestrosV201306(headerMessage, requestCexper.Plate, Convert.ToString(requestCexper.DocumentType), requestCexper.DocumentNumber);
                }
                stopWatch.Stop();
                BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarCEXPER", requestCexper.GuidProcess.ToString(), requestCexper);
                responseCexper = new ResponseCexper()
                {
                    PoliciesInfo = ModelAssembler.CreateResponsePoliciesInfo(cexperHistoricoPoliza),
                    SinisterInfo = ModelAssembler.CreateResponseSinisterInfo(cexperHistoricoSiniestro)
                };
                return responseCexper;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                if (ex.Data.Count == 0)
                {
                    responseCexper = new ResponseCexper()
                    {
                        PoliciesInfo = null,
                        SinisterInfo = null
                    };
                    return responseCexper;
                }
                else
                {
                    return null;
                }
            }

        }

        public List<ResponsePerson> GetPerson2gByDocumentNumber(string documentNumber, bool company)
        {
            try
            {
                string uriApiPersonServices = ConfigurationSettings.AppSettings["UrlApiPersonServices"].ToString();
                List<ResponsePerson> responsePeople = new List<ResponsePerson>();
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uriApiPersonServices + "/api/Person/GetPerson2gByDocumentNumber")
                };
                HttpResponseMessage response = client.GetAsync("?documentNumber=" + documentNumber + "&company=" + company).Result;
                if (response.IsSuccessStatusCode)
                {
                    responsePeople = JsonConvert.DeserializeObject<List<ResponsePerson>>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
                client.Dispose();
                return responsePeople;

            }
            catch (Exception ex)
            {
                //TODO:Definir con funcional salida de error
                EventLog.WriteEntry("Application", string.Format("Servicio no disponible GetPerson2gByDocumentNumber : Ex: {0}", ex.Message), EventLogEntryType.Warning);
                return new List<ResponsePerson>();
            }
            
        }

        public ResponsePerson GetPerson2gByPersonId(int personId, bool company)
        {
            try
            {
                string uriApiPersonServices = ConfigurationSettings.AppSettings["UrlApiPersonServices"].ToString();
                ResponsePerson responsePeople = new ResponsePerson();
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uriApiPersonServices + "/api/Person/GetPerson2gByPersonId")
                };
                HttpResponseMessage response = client.GetAsync("?personId=" + personId + "&company=" + company).Result;
                if (response.IsSuccessStatusCode)
                {
                    responsePeople = JsonConvert.DeserializeObject<List<ResponsePerson>>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
                }
                else
                {
                    Console.WriteLine(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
                client.Dispose();

                return responsePeople;

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format("Servicio no disponible GetPerson2gByPersonId : Ex: {0}", ex.Message), EventLogEntryType.Warning);
                return new ResponsePerson();
            }
          
        }

        public ResponseCompany GetCompany2gByPersonId(int companyId, bool company)
        {
            try
            {
                string uriApiPersonServices = ConfigurationSettings.AppSettings["UrlApiPersonServices"].ToString();
                ResponseCompany responseCompany = new ResponseCompany();
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uriApiPersonServices + "/api/Person/GetCompany2gByCompanyId")
                };
                HttpResponseMessage response = client.GetAsync("?companyId=" + companyId + "&company=" + company).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseCompany = JsonConvert.DeserializeObject<List<ResponseCompany>>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
                }
                else
                {
                    Console.WriteLine(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
                client.Dispose();

                return responseCompany;

            }
            catch (Exception ex)
            {
                //TODO:Definir con funcional salida de error
                EventLog.WriteEntry("Application", string.Format("Servicio no disponible GetCompany2gByPersonId : Ex: {0}", ex.Message),EventLogEntryType.Warning);
                return new ResponseCompany();
            }
          
        }

        public ResponseProvider GetProvider2g(int personId)
        {
            try
            {
                string uriApiPersonServices = ConfigurationSettings.AppSettings["UrlApiPersonServices"].ToString();
                ResponseProvider responseProvider = new ResponseProvider();
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uriApiPersonServices + "/api/Person/GetProvider2g")
                };
                HttpResponseMessage response = client.GetAsync("?personId=" + personId).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseProvider = JsonConvert.DeserializeObject<ResponseProvider>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
                client.Dispose();

                return responseProvider;
            }
            catch (Exception ex)
            {
                //TODO:Definir con funcional salida de error
                EventLog.WriteEntry("Application", string.Format("Servicio no disponible GetProvider2g : Ex: {0}", ex.Message),EventLogEntryType.Warning);
                return new ResponseProvider();
            }
            
        }

        public List<ResponseBankTransfer> GetBankTransfer2g(int personId)
        {
            try
            {
                string uriApiPersonServices = ConfigurationSettings.AppSettings["UrlApiPersonServices"].ToString();
                List<ResponseBankTransfer> responseBankTransfers = new List<ResponseBankTransfer>();
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uriApiPersonServices + "/api/Person/GetBankTransfer2g")
                };
                HttpResponseMessage response = client.GetAsync("?transferPersonId=" + personId).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseBankTransfers = JsonConvert.DeserializeObject<List<ResponseBankTransfer>>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
                client.Dispose();

                return responseBankTransfers;

            }
            catch (Exception ex)
            {
                //TODO:Definir con funcional salida de error
                EventLog.WriteEntry("Application", string.Format("Servicio no disponible GetProvider2g : Ex: {0}", ex.Message));
                return new List<ResponseBankTransfer>();
            }
           
        }

        public List<ResponseTax> GetTax2g(int personId)
        {
            try
            {
                string uriApiPersonServices = ConfigurationSettings.AppSettings["UrlApiPersonServices"].ToString();
                List<ResponseTax> responseTaxes = new List<ResponseTax>();
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uriApiPersonServices + "/api/Person/GetTax2g")
                };
                HttpResponseMessage response = client.GetAsync("?taxPersonId=" + personId).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseTaxes = JsonConvert.DeserializeObject<List<ResponseTax>>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
                client.Dispose();

                return responseTaxes;

            }
            catch (Exception ex)
            {
                //TODO:Definir con funcional salida de error
                EventLog.WriteEntry("Application", string.Format("Servicio no disponible GetTax2g : Ex: {0}", ex.Message));
                return new List<ResponseTax>();
            }
          
        }

        public ResponsePolicyPayment GetPolicyPayment(RequestPolicyPayment requestPolicyPayment)
        {
            try
            {
                string request = JsonConvert.SerializeObject(requestPolicyPayment);
                ResponsePolicyPayment responsePolicyPayment = new ResponsePolicyPayment();
                PaymentClientServices paymentClientServices = new PaymentClientServices();

                return responsePolicyPayment = JsonConvert.DeserializeObject<ResponsePolicyPayment>(paymentClientServices.GetPolicyPayment(request));
            }
            catch (Exception)
            {
                throw new BusinessException("Error conectando Api");
            }
            
        }

        public ResponseDocumentPayments GetPaymentsPolicyByDocuments(RequestDocumentPayments requestDocumentPayments)
        {
            try
            {
                string request = JsonConvert.SerializeObject(requestDocumentPayments);
                PaymentClientServices paymentClientServices = new PaymentClientServices();
                ResponseDocumentPayments responseDocumentPayments = new ResponseDocumentPayments();

                return responseDocumentPayments = JsonConvert.DeserializeObject<ResponseDocumentPayments>(paymentClientServices.GetPaymentsPolicyByDocuments(request));
            }
            catch (Exception)
            {

                throw new BusinessException("Error conectando Api");
            }
            
        }

        public ResponseReinsurance GetReinsurancePolicy(RequestReinsurance requestDocumentPayments)
        {
            try
            {
                string request = JsonConvert.SerializeObject(requestDocumentPayments);
                ReinsurancePolicyServices reinsurancePolicyServices = new ReinsurancePolicyServices();
                ResponseReinsurance responseReinsurance = new ResponseReinsurance();

                return responseReinsurance = JsonConvert.DeserializeObject<ResponseReinsurance>(reinsurancePolicyServices.GetReinsurancePolicy(request));
            }
            catch (Exception)
            {

                throw new BusinessException("Error conectando Api");
            }

        }
    }
}