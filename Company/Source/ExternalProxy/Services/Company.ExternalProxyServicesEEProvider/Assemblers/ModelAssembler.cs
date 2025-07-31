using Newtonsoft.Json;
using Sistran.Company.Application.ExternalProxyServices.DatacreditoBizTalkService;
using Sistran.Company.Application.ExternalProxyServices.ExperienciaAseguradoBiztalkService;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Company.Application.ExternalProxyServices.SisaFasecoldaBizTalkService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using ExIn = Sistran.Company.Application.ExternalProxyServices.InfraccionesBizTalkService;
using CONSULTASISA = Sistran.Company.Application.ExternalProxyServices.ConsultaSisa;
using CONSULTACEXPER = Sistran.Company.Application.ExternalProxyServices.ConsultaCexper;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region HeaderRequestScoreCredit
        public static EncabezadoEntrada CreateHeaderMessageRequest(Guid guidProcess)
        {
            return new EncabezadoEntrada
            {
                canal = ConfigurationManager.AppSettings["CanalDataCreditoBizTalk"],
                idCorrelacionConsumidor = guidProcess.ToString(),
                peticionFecha = DateTime.Now.Date
            };
        }
        #endregion HeaderRequestScoreCredit

        #region BodyRequestScoreCredit
        public static BODYType CreateBodyMessageRequest(RequestScoreCredit requestScoreCredit)
        {
            return new BODYType
            {
                personaConsulta = new CriteriosBusquedaPersona_TYPE
                {
                    llaveIdentificacion = new Persona_TYPE
                    {
                        identificacion = new IdentificacionPersona_TYPE
                        {
                            identificacion = requestScoreCredit.DocumentNumber,
                            tipoIdentificacion = requestScoreCredit.DocumentTypeDataCredit.ToString()
                        }
                    },
                    llaveNombre = requestScoreCredit.LastName
                }
            };
        }
        #endregion BodyRequestScoreCredit

        #region BodyRequestScoreCredit
        public static ResponseScoreCredit CreateResponseScoreCredit(ConsultaRiesgoRespType consultaRiesgoRespType)
        {
            return new ResponseScoreCredit
            {
                Error = new Error
                {
                    Code = Convert.ToInt32(consultaRiesgoRespType.HEADER.rtaCodCanal),
                    ErrorMessage = consultaRiesgoRespType.HEADER.rtaDescCanal,
                    IsError = false
                },
                Score = consultaRiesgoRespType?.BODY?.RespuestaDataCredito[0]?.valor
            };
        }
        #endregion BodyRequestScoreCredit


        public static ResponseInfringement CreateResponseInfringement(ExIn.consultarInfraccionesResp consultaInfracciones)
        {

            List<ExternalInfrigement> listExternalInfrigement = new List<ExternalInfrigement>();
            ExIn.InfraccionInfo_TYPE[] infraccionInfo_TYPE;
            infraccionInfo_TYPE = consultaInfracciones.BODY;

            foreach (var item in infraccionInfo_TYPE)
            {
                ExternalInfrigement externalInfrigement = new ExternalInfrigement
                {
                    Number = item.IdentificacionComparendo.IdComparendo,
                    Code = item.IdentificacionInfraccion.Codigo,
                    InfringementState = item.EstadoComparendo,
                    InfringementDate = item.FechaComparendo,
                    LicensePlate = item.Vehiculo.Placa,
                    Price = (decimal)item.Monto.monto,
                    DateRequest = DateTime.Now
                };
                listExternalInfrigement.Add(externalInfrigement);
            }

            return new ResponseInfringement
            {
                Error = new Error
                {
                    Code = Convert.ToInt32(consultaInfracciones.HEADER.rtaCodCanal),
                    ErrorMessage = consultaInfracciones.HEADER.rtaDescCanal,
                    IsError = false
                },
                ExternalInfrigements = listExternalInfrigement,
            };

        }




        #region Fasecolda
        internal static HistoricoPolizas CreateHistoricoPolizas(int documentTypeFasecolda, string documentNumber, string licensePlate)
        {
            return new HistoricoPolizas
            {
                numeroDocumento = documentNumber,
                tipoDocumento = documentTypeFasecolda.ToString(),
                placa = licensePlate
            };
        }

        internal static HistoricoSiniestros CreateHistoricoSiniestros(int documentTypeFasecolda, string documentNumber, string licensePlate)
        {
            return new HistoricoSiniestros
            {
                numeroDocumento = documentNumber,
                tipoDocumento = documentTypeFasecolda.ToString(),
                placa = licensePlate
            };
        }

        #endregion Fasecolda

        public static ExIn.Persona_TYPE GetBodyRequestForBizTalkIngementSimit(int documentType, string documentNumber)
        {
            return new ExIn.Persona_TYPE
            {
                identificacion = new ExIn.IdentificacionPersona_TYPE
                {
                    identificacion = documentNumber,
                    tipoIdentificacion = Convert.ToString(documentType)
                }
            };
        }

        public static ExIn.EncabezadoEntrada GetHeaderForBizTalkIngementSimit(Guid guid)
        {

            return new ExIn.EncabezadoEntrada
            {
                canal = ConfigurationManager.AppSettings["CanalSimitBizTalk"],
                idCorrelacionConsumidor = guid.ToString(),
                peticionFecha = DateTime.Now.Date
            };
        }

        internal static ExternalInformationLogDTO CreateExternalInformationLogDTO(Stopwatch timer, string serviceName, string correlationId, object[] parameters)
        {
            ExternalInformationLogDTO thirdParties = new ExternalInformationLogDTO();
            thirdParties.TotalTimeResponse = timer.ElapsedMilliseconds;
            thirdParties.ServiceMethod = serviceName;
            thirdParties.ServerClient = Environment.MachineName;
            thirdParties.LocalRequestDate = DateTime.Now;
            thirdParties.SuccessInvoke = true;
            thirdParties.JsonRequestParams = GetParamsLikeJsonArray(parameters);
            thirdParties.GuidProcess = new Guid(correlationId);
            return thirdParties;
        }

        private static string GetParamsLikeJsonArray(object[] parameters)
        {
            string response = string.Empty;
            if (parameters != null && parameters.Length > 0)
            {
                response = JsonConvert.SerializeObject(parameters);
            }
            return response;
        }

        public static UsuarioHeader CreateHeaderMessageSisaRequest(Guid guidProcess)
        {
            return new UsuarioHeader
            {
                Clave = ConfigurationManager.AppSettings["WebServiceFasecoldaUserPassword"],
                Usuario = ConfigurationManager.AppSettings["WebServiceFasecoldaUser"]
            };
        }


        public static CONSULTASISA.UsuarioHeader CreateHeaderMessageConsultaSisaRequest(Guid guidProcess)
        {
            return new CONSULTASISA.UsuarioHeader
            {
                Clave = ConfigurationManager.AppSettings["WebServiceFasecoldaUserPassword"],
                Usuario = ConfigurationManager.AppSettings["WebServiceFasecoldaUser"]
            };
        }

        public static CONSULTACEXPER.UsuarioHeader CreateHeaderMessageConsultaCEXPERRequest()
        {
            return new CONSULTACEXPER.UsuarioHeader
            {
                Clave = ConfigurationManager.AppSettings["WebServiceFasecoldaUserPassword"],
                Usuario = ConfigurationManager.AppSettings["WebServiceFasecoldaUser"]
            };
        }

        public static List<ResponsePoliciesInfo> CreateResponsePoliciesInfo(HistoricoPolizaSisaV201306[] sisaPoliciesResponse)
        {
            List<ResponsePoliciesInfo> lstPolicies = new List<ResponsePoliciesInfo>();
            foreach (HistoricoPolizaSisaV201306 item in sisaPoliciesResponse)
            {
                lstPolicies.Add(new ResponsePoliciesInfo()
                {
                    BeneficiaryDocumentNumber = item.NumeroDocumentoBeneficiario,
                    BeneficiaryName = item.NombreBeneficiario,
                    BeneficiaryTypeDocument = item.TipoDocumentoBeneficiario,
                    Brand = item.Marca,
                    Chassis = item.Chasis,
                    Class = item.Clase,
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    DocumentNumber = item.NumeroDocumento,
                    EffectiveDate = item.FechaVigencia,
                    EndEffectiveDate = item.FechaFinVigencia,
                    Engine = item.Motor,
                    GuiedCode = item.CodigoGuia,
                    HolderDocumentNumber = item.NumeroDocumentoTomador,
                    HolderName = item.NombreTomador,
                    HolderTypeDocument = item.TipoDocumentoTomador,
                    Insured = item.Asegurado,
                    InsuredAmount = item.ValorAsegurado,
                    InsuredTypeDocument = item.TipoDocumentoAsegurado,
                    InterchangeType = item.TipoCruce,
                    Model = item.Modelo,
                    Order = item.Orden,
                    PH = item.PH,
                    Plate = item.Placa,
                    PolicyNumber = item.NumeroPoliza,
                    PPD = item.PPD,
                    PPH = item.PPH,
                    PTD = item.PTD,
                    RC = item.RC,
                    Service = item.Servicio,
                    Type = item.Tipo,
                    Valid = item.Vigente
                });
            }
            return lstPolicies;
        }


        public static List<ResponsePoliciesInfo> CreateResponsePoliciesInfo(CONSULTASISA.HistoricoPolizaSisaV201306[] sisaPoliciesResponse)
        {
            List<ResponsePoliciesInfo> lstPolicies = new List<ResponsePoliciesInfo>();
            foreach (CONSULTASISA.HistoricoPolizaSisaV201306 item in sisaPoliciesResponse)
            {
                lstPolicies.Add(new ResponsePoliciesInfo()
                {
                    BeneficiaryDocumentNumber = item.NumeroDocumentoBeneficiario,
                    BeneficiaryName = item.NombreBeneficiario,
                    BeneficiaryTypeDocument = item.TipoDocumentoBeneficiario,
                    Brand = item.Marca,
                    Chassis = item.Chasis,
                    Class = item.Clase,
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    DocumentNumber = item.NumeroDocumento,
                    EffectiveDate = item.FechaVigencia,
                    EndEffectiveDate = item.FechaFinVigencia,
                    Engine = item.Motor,
                    GuiedCode = item.CodigoGuia,
                    HolderDocumentNumber = item.NumeroDocumentoTomador,
                    HolderName = item.NombreTomador,
                    HolderTypeDocument = item.TipoDocumentoTomador,
                    Insured = item.Asegurado,
                    InsuredAmount = item.ValorAsegurado,
                    InsuredTypeDocument = item.TipoDocumentoAsegurado,
                    InterchangeType = item.TipoCruce,
                    Model = item.Modelo,
                    Order = item.Orden,
                    PH = item.PH,
                    Plate = item.Placa,
                    PolicyNumber = item.NumeroPoliza,
                    PPD = item.PPD,
                    PPH = item.PPH,
                    PTD = item.PTD,
                    RC = item.RC,
                    Service = item.Servicio,
                    Type = item.Tipo,
                    Valid = item.Vigente
                });
            }
            return lstPolicies;
        }

        public static List<ResponsePoliciesInfo> CreateResponsePoliciesInfo(CONSULTACEXPER.HistoricoPolizaCexperV201306[] cexperPoliciesResponse)
        {
            List<ResponsePoliciesInfo> lstPolicies = new List<ResponsePoliciesInfo>();
            foreach (CONSULTACEXPER.HistoricoPolizaCexperV201306 item in cexperPoliciesResponse)
            {
                lstPolicies.Add(new ResponsePoliciesInfo()
                {
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    PolicyNumber = item.NumeroPoliza,
                    Order = item.Orden,
                    Plate = item.Placa,
                    Engine = item.Motor,
                    Chassis = item.Chasis,
                    EffectiveDate = item.FechaVigencia,
                    EndEffectiveDate = item.FechaFinVigencia,
                    Valid = item.Vigente, 
                    GuiedCode = item.CodigoGuia,
                    Brand = item.Marca,
                    Class = item.Clase,
                    Type = item.Tipo,
                    Model = item.Modelo,
                    Service = item.Servicio,
                    InsuredTypeDocument = item.TipoDocumentoAsegurado,
                    DocumentNumber = item.NumeroDocumento,
                    Insured = item.Asegurado,
                    InsuredAmount = item.ValorAsegurado,
                    PTD = item.PTD,
                    PPD = item.PPD,
                    PH = item.PH,
                    PPH = item.PPH,
                    RC = item.RC,
                    InterchangeType = item.TipoCruce,
                    Color = item.Color,
                    PolicyClass = item.ClasePoliza
                    //BeneficiaryDocumentNumber = item.NumeroDocumentoBeneficiario,
                    //BeneficiaryName = item.NombreBeneficiario,
                    //BeneficiaryTypeDocument = item.TipoDocumentoBeneficiario,
                    //HolderDocumentNumber = item.NumeroDocumentoTomador,
                    //HolderName = item.NombreTomador,
                    //HolderTypeDocument = item.TipoDocumentoTomador,
                });
            }
            return lstPolicies;
        }

        public static List<ResponseSinisterInfo> CreateResponseSinisterInfo(CONSULTACEXPER.HistoricoSiniestroCexperV201306[] cexperPoliciesResponse)
        {
            List<ResponseSinisterInfo> lstSinister = new List<ResponseSinisterInfo>();
            foreach (CONSULTACEXPER.HistoricoSiniestroCexperV201306 item in cexperPoliciesResponse)
            {
                lstSinister.Add(new ResponseSinisterInfo()
                {
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    SinisterNumber = item.NumeroSiniestro,
                    PolicyNumber = item.NumeroPoliza,
                    Order = item.Orden,
                    Plate = item.Placa,
                    Engine = item.Motor,
                    Chassis = item.Chasis,
                    SinistereDate = item.FechaSiniestro,
                    GuiedCode = item.CodigoGuia,
                    Brand = item.Marca,
                    Class = item.Clase,
                    Type = item.Tipo,
                    Model = item.Modelo,
                    InsuredTypeDocument = item.TipoDocumentoAsegurado,
                    DocumentNumber = item.NumeroDocumento,
                    Insured = item.Asegurado,
                    InsuredAmount = item.ValorAsegurado,
                    TypeCruce = item.TipoCruce,
                    Color = item.Color,
                    SinisterSucursal = item.SucursalSin,
                    PolicyClass = item.ClasePoliza,
                    DriverIdentificarion = item.IdentificacionConductor,
                    DriverName = item.NombreConductor,
                    Plate1 = item.Placa1,
                    Service = item.Servicio,
                    Amparos = item.Amparos
                });
            }
            
            return lstSinister;
        }
            public static List<ResponseSimitSISA> CreateResponseSimit(ComparendoSIMITV201401[] sisaSimitResponse)
        {
            List<ResponseSimitSISA> lstSimit = new List<ResponseSimitSISA>();
            foreach (ComparendoSIMITV201401 item in sisaSimitResponse)
            {
                lstSimit.Add(new ResponseSimitSISA()
                {
                    InfringementCode = item.CodigoInfraccion,
                    Name = item.Nombre,
                    Number = item.Numero,
                    PenaltyDate = item.FechaComparendo,
                    Plate = item.Placa,
                    Secretary = item.Secretaria,
                    State = item.Estado,
                    Value = item.Valor
                });
            }
            return lstSimit;
        }


        public static List<ResponseSimitSISA> CreateResponseSimit(CONSULTASISA.ComparendoSIMITV201401[] sisaSimitResponse)
        {
            List<ResponseSimitSISA> lstSimit = new List<ResponseSimitSISA>();
            foreach (CONSULTASISA.ComparendoSIMITV201401 item in sisaSimitResponse)
            {
                lstSimit.Add(new ResponseSimitSISA()
                {
                    InfringementCode = item.CodigoInfraccion,
                    Name = item.Nombre,
                    Number = item.Numero,
                    PenaltyDate = item.FechaComparendo,
                    Plate = item.Placa,
                    Secretary = item.Secretaria,
                    State = item.Estado,
                    Value = item.Valor
                });
            }
            return lstSimit;
        }

        public static List<ResponseClaims> CreateResponseClaims(HistoricoSiniestroSisaV201306[] sisaClaimsResponse)
        {
            List<ResponseClaims> lstClaims = new List<ResponseClaims>();
            foreach (HistoricoSiniestroSisaV201306 item in sisaClaimsResponse)
            {
                lstClaims.Add(new ResponseClaims()
                {
                    Protection = CreateResponseProtectionClaims(item.Amparos),
                    Insured = item.Asegurado,
                    Chassis = item.Chasis,
                    Class = item.Clase,
                    CompanyCode = item.CodigoCompania,
                    GuiedCode = item.CodigoGuia,
                    ClaimDate = item.FechaSiniestro,
                    Brand = item.Marca,
                    Model = item.Modelo,
                    Engine = item.Motor,
                    CompanyName = item.NombreCompania,
                    DocumentNumber = item.NumeroDocumento,
                    PolicyNumber = item.NumeroPoliza,
                    ClaimNumber = item.NumeroSiniestro,
                    Order = item.Orden,
                    Plate = item.Placa,
                    Type = item.Tipo,
                    InterchangeType = item.TipoCruce,
                    InsuredDocumentType = item.TipoDocumentoAsegurado,
                    InsuredAmount = item.ValorAsegurado
                });
            }
            return lstClaims;
        }


        public static List<ResponseClaims> CreateResponseClaims(CONSULTASISA.HistoricoSiniestroSisaV201306[] sisaClaimsResponse)
        {
            List<ResponseClaims> lstClaims = new List<ResponseClaims>();
            foreach (CONSULTASISA.HistoricoSiniestroSisaV201306 item in sisaClaimsResponse)
            {
                lstClaims.Add(new ResponseClaims()
                {
                    Protection = CreateResponseProtectionClaims(item.Amparos),
                    Insured = item.Asegurado,
                    Chassis = item.Chasis,
                    Class = item.Clase,
                    CompanyCode = item.CodigoCompania,
                    GuiedCode = item.CodigoGuia,
                    ClaimDate = item.FechaSiniestro,
                    Brand = item.Marca,
                    Model = item.Modelo,
                    Engine = item.Motor,
                    CompanyName = item.NombreCompania,
                    DocumentNumber = item.NumeroDocumento,
                    PolicyNumber = item.NumeroPoliza,
                    ClaimNumber = item.NumeroSiniestro,
                    Order = item.Orden,
                    Plate = item.Placa,
                    Type = item.Tipo,
                    InterchangeType = item.TipoCruce,
                    InsuredDocumentType = item.TipoDocumentoAsegurado,
                    InsuredAmount = item.ValorAsegurado
                });
            }
            return lstClaims;
        }


        public static ResponseGuideInfo CreateResponseGuideInfo(GuiaSisa sisaResponse)
        {
            return new ResponseGuideInfo()
            {
                Brand = sisaResponse.Marca,
                Class = sisaResponse.Clase,
                CurrentValue = sisaResponse.ValorActual,
                Guied = sisaResponse.Guia,
                GuiedCode = sisaResponse.CodigoGuia,
                GuiedDate = sisaResponse.FechaGuia,
                InitialValue = sisaResponse.ValorInicio,
                Model = sisaResponse.Modelo,
                Type = sisaResponse.Tipo
            };
        }

        public static ResponseGuideInfo CreateResponseGuideInfo(CONSULTASISA.GuiaSisa sisaResponse)
        {
            return new ResponseGuideInfo()
            {
                Brand = sisaResponse.Marca,
                Class = sisaResponse.Clase,
                CurrentValue = sisaResponse.ValorActual,
                Guied = sisaResponse.Guia,
                GuiedCode = sisaResponse.CodigoGuia,
                GuiedDate = sisaResponse.FechaGuia,
                InitialValue = sisaResponse.ValorInicio,
                Model = sisaResponse.Modelo,
                Type = sisaResponse.Tipo
            };
        }


        public static ResponseVINVerification CreateResponseVINVerification(WMISisa sisaVinResponse)
        {
            return new ResponseVINVerification()
            {
                Country = sisaVinResponse.Pais,
                CreationDate = sisaVinResponse.FechaCreacion,
                MakerAddress1 = sisaVinResponse.Direccion1Fabricante,
                MakerAddress2 = sisaVinResponse.Direccion2Fabricante,
                MakerName = sisaVinResponse.NombreFabricante,
                ModificationDate = sisaVinResponse.FechaModificacion,
                WMICode = sisaVinResponse.CodigoWMI
            };
        }


        public static ResponseVINVerification CreateResponseVINVerification(CONSULTASISA.WMISisa sisaVinResponse)
        {
            return new ResponseVINVerification()
            {
                Country = sisaVinResponse.Pais,
                CreationDate = sisaVinResponse.FechaCreacion,
                MakerAddress1 = sisaVinResponse.Direccion1Fabricante,
                MakerAddress2 = sisaVinResponse.Direccion2Fabricante,
                MakerName = sisaVinResponse.NombreFabricante,
                ModificationDate = sisaVinResponse.FechaModificacion,
                WMICode = sisaVinResponse.CodigoWMI
            };
        }

        public static List<ResponseProtection> CreateResponseProtectionClaims(AmparoSisaV201306[] sisaClaimsResponse)
        {
            List<ResponseProtection> lstProtections = new List<ResponseProtection>();
            foreach (AmparoSisaV201306 item in sisaClaimsResponse)
            {
                lstProtections.Add(new ResponseProtection()
                {
                    ClaimDate = item.FechaSiniestro,
                    ClaimNumber = item.NumeroSiniestro,
                    CompanyCode = item.CodigoCompania,
                    NoticeDate = item.FechaAviso,
                    Plate = item.Placa,
                    ProtectedClaimValue = item.ValorReclamaAmparo,
                    ProtectedName = item.NombreAmparado,
                    ProtectedPaidValue = item.ValorPagadoAmparo,
                    State = item.Estado
                });
            }

            return lstProtections;
        }


        public static List<ResponseProtection> CreateResponseProtectionClaims(CONSULTASISA.AmparoSisaV201306[] sisaClaimsResponse)
        {
            List<ResponseProtection> lstProtections = new List<ResponseProtection>();
            foreach (CONSULTASISA.AmparoSisaV201306 item in sisaClaimsResponse)
            {
                lstProtections.Add(new ResponseProtection()
                {
                    ClaimDate = item.FechaSiniestro,
                    ClaimNumber = item.NumeroSiniestro,
                    CompanyCode = item.CodigoCompania,
                    NoticeDate = item.FechaAviso,
                    Plate = item.Placa,
                    ProtectedClaimValue = item.ValorReclamaAmparo,
                    ProtectedName = item.NombreAmparado,
                    ProtectedPaidValue = item.ValorPagadoAmparo,
                    State = item.Estado
                });
            }

            return lstProtections;
        }

        public static List<ResponsePoliciesHistorical> CreateResponsePoliciesHistorical(HistoricoPolizaCexper[]  cexperPoliciesResponse)
        {
            List<ResponsePoliciesHistorical> lstPolicies = new List<ResponsePoliciesHistorical>();
            foreach (HistoricoPolizaCexper item in cexperPoliciesResponse)
            {
                lstPolicies.Add(new ResponsePoliciesHistorical()
                {
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    PolicyNumber = item.NumeroPoliza,
                    Order = item.Orden,
                    LicensePlate = item.Placa,
                    Motor = item.Motor,
                    Chassis = item.Chasis,
                    EffectiveDate = item.FechaVigencia,
                    EndEffectiveDate = item.FechaFinVigencia,
                    Current = item.Vigente,
                    GuideCode = item.CodigoGuia,
                    Make = item.Marca,
                    Class = item.Clase,
                    Type = item.Tipo,
                    Model = item.Modelo,
                    Service = item.Servicio,
                    DocumentTypeInsured = item.TipoDocumentoAsegurado,
                    DocumentNumber = item.NumeroDocumento,
                    Insured = item.Asegurado,
                    InsuredValue = item.ValorAsegurado,
                    PTD = item.PTD,
                    PPD = item.PPD,
                    PH = item.PH,
                    PPH = item.PPH,
                    RC = item.RC,
                    TypeCrossing = item.TipoCruce
                    //Color = item.Color,
                    //ClassPolicy = item.ClasePoliza
                });
            }
            return lstPolicies;
        }

        public static List<ResponseHistoricalSinister> CreateResponseHistoricalSinister(HistoricoSiniestroCexper[]  cexperSinisterResponse)
        {
            List<ResponseHistoricalSinister> lstSinisters = new List<ResponseHistoricalSinister>();
            foreach (HistoricoSiniestroCexper item in cexperSinisterResponse)
            {
                lstSinisters.Add(new ResponseHistoricalSinister()
                {
                    ProtectionCexper = CreateResponseProtectionCexper(item.Amparos),
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    SinisterNumber = item.NumeroSiniestro,
                    PolicyNumber = item.NumeroPoliza,
                    Order = item.Orden,
                    LicensePlate = item.Placa,
                    Motor = item.Motor,
                    Chassis = item.Chasis,
                    SinisterDate = item.FechaSiniestro,
                    GuideCode = item.CodigoGuia,
                    Make = item.Marca,
                    Class = item.Clase,
                    Type = item.Tipo,
                    Model = item.Modelo,
                    DocumentTypeInsured = item.TipoDocumentoAsegurado,
                    DocumentNumber = item.NumeroDocumento,
                    Insured = item.Asegurado,
                    InsuredValue = item.ValorAsegurado,
                    TypeCrossing = item.TipoCruce
                });
            }
            return lstSinisters;
        }

        public static List<ResponseProtectionCexper> CreateResponseProtectionCexper(AmparoCexper[]  cexperSinisterResponse)
        {
            List<ResponseProtectionCexper> lstProtections = new List<ResponseProtectionCexper>();
            foreach (AmparoCexper item in cexperSinisterResponse)
            {
                lstProtections.Add(new ResponseProtectionCexper()
                {
                    CompanyCode = item.CodigoCompania,
                    SinisterNumber = item.NumeroSiniestro,
                    SinisterDate = item.FechaSiniestro,
                    Plate = item.Placa,
                    State = item.Estado,
                    ProtectedName = item.NombreAmparado,
                    ProtectedClaimValue = item.ValorPagadoAmparo,
                    ProtectedPaidValue = item.ValorReclamaAmparo

                });
            }
            return lstProtections;
        }

        public static List<ResponsePoliciesInfo> CreateResponseHistoricoPolicies(CONSULTASISA.HistoricoPolizaSisa[] sisaPoliciesResponse)
        {
            List<ResponsePoliciesInfo> lstPolicies = new List<ResponsePoliciesInfo>();
            foreach (CONSULTASISA.HistoricoPolizaSisa item in sisaPoliciesResponse)
            {
                lstPolicies.Add(new ResponsePoliciesInfo()
                {
                    BeneficiaryDocumentNumber = item.NumeroDocumento,
                    BeneficiaryName = item.Asegurado,
                    BeneficiaryTypeDocument = item.TipoDocumentoAsegurado,
                    Brand = item.Marca,
                    Chassis = item.Chasis,
                    Class = item.Clase,
                    CompanyCode = item.CodigoCompania,
                    CompanyName = item.NombreCompania,
                    DocumentNumber = item.NumeroDocumento,
                    EffectiveDate = item.FechaVigencia,
                    EndEffectiveDate = item.FechaFinVigencia,
                    Engine = item.Motor,
                    GuiedCode = item.CodigoGuia,
                    Insured = item.Asegurado,
                    InsuredAmount = item.ValorAsegurado,
                    InsuredTypeDocument = item.TipoDocumentoAsegurado,
                    InterchangeType = item.TipoCruce,
                    Model = item.Modelo,
                    Order = item.Orden,
                    PH = item.PH,
                    Plate = item.Placa,
                    PolicyNumber = item.NumeroPoliza,
                    PPD = item.PPD,
                    PPH = item.PPH,
                    PTD = item.PTD,
                    RC = item.RC,
                    Service = item.Servicio,
                    Type = item.Tipo,
                    Valid = item.Vigente
                });
            }
            return lstPolicies;
        }
    }
}