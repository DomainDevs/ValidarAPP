//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//Terceros
using Excel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AutomaticDebit;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReportingServices;
using Sistran.Core.Application.UnderwritingServices.Models;
using Dto = Sistran.Core.Application.AutomaticDebitServices.DTOs;
using AutomaticDebitModels = Sistran.Core.Application.AutomaticDebitServices.Models;
using Report = Sistran.Core.Application.ReportingServices.Models.Formats;
using UniquePersonModels = Sistran.Core.Application.UniquePersonService.V1.Models;
using AccountingDTO = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;


//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AutomaticDebit
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class AutomaticDebitController : Controller
    {
        #region Constants

        public const string PathTransfer = "~/Temp/transfers"; //~/App_Data/transfers


        #endregion

        #region Class

        /// <summary>
        /// CouponsStatusDTO
        /// </summary>
        public class CouponsStatusDto
        {
            public int CouponNumber { get; set; }                   // nro_cupon
            public int CouponIndicator { get; set; }                // indicador de cupon
            public int MethodTypeId { get; set; }                   // indicador de conducto
            public string PolicyId { get; set; }                    // id de póliza
            public int InsuredCode { get; set; }                    // código asegurado
            public int PayerIndicatorCode { get; set; }             // código de Indicador de Pagador
            public int PaymentNumber { get; set; }                  // número de cuota
            public string VoucherNumber { get; set; }               // número de comprobante
            public string RejectionCode { get; set; }               // código de rechazo
            public DateTime ExpirationDate { get; set; }            // fecha de expiración
            public decimal SendCode { get; set; }                   // id envio
            public int LotNumber { get; set; }                      // número de lote
            public DateTime PresentationDate { get; set; }          // fecha de presentación
            public DateTime SendDate { get; set; }                  // fecha de envío
            public int ReceiverBankId { get; set; }                 // bco_receptor
            public decimal PremiumTax { get; set; }                 // imp_premio_me
            public DateTime AplicationDate { get; set; }            // fecha de aplicación
            public string AccountNumberOrigin { get; set; }         // nro_cuenta_ori
            public string NameOrigin { get; set; }                  // nombre_ori*
            public string CompanyAccount { get; set; }              // Cuenta Compañia
            public string AccountNumber { get; set; }               // Número de Cuenta
            public string ClientName { get; set; }                  // Nombre Cliente
            public string ReferencesCurp { get; set; }              // Nit - rfc_curp_rec nit/cedula **preguntar definición
            public string ServiceTransmitterReference { get; set; } // ref_servicio_emisor 
            public string HolderName { get; set; }                  // nom_titular_servicio 
            public decimal Tax { get; set; }                        // imp_iva_me 
            public int OriginalReferenceNumber { get; set; }        // ref_num_ori 
            public string ReferenceLegend { get; set; }             // ref_leyenda_ori 
            public DateTime DetailPresentationDate { get; set; }    // fec_presentacion_det
            public int SummarySequenceNumber { get; set; }          // nro_secuencia_sumario
            public int SummaryLotNumber { get; set; }               // nro_lote_sumario
            public string SerialNumber { get; set; }                // nro_serie
            public decimal Premium { get; set; }                    // imp_premio_bco_me*
            public decimal TaxBase { get; set; }                    // imp_base_iva
            public string IndividualDocumentNumber { get; set; }    // nro_doc*
            public string DocumenTypeId { get; set; }               // tipo_doc
            public int AccountTypeId { get; set; }                  // tipo_cta
            public string PolicyNumber { get; set; }                // nro_pol
            public string ReferencesCurOrigin { get; set; }         // rfc_curp_ori
            public string CardAccountNumber { get; set; }           // nro_cta_tarjeta
            public string AuthorizationNumber { get; set; }         // nro_autorizacion
            public int AccountTypeReceivingCode { get; set; }       // tipo_cuenta_rec
        }

        /// <summary>
        /// LoadErrorDTO
        /// </summary>
        public class LoadErrorDto
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        #region WorkerFactory

        /// <summary>
        /// WorkerFactory
        /// </summary>
        public sealed class WorkerFactory
        {
            private static volatile WorkerFactory _instance;
            private static object syncRoot = new object();

            public static WorkerFactory Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (syncRoot)
                        {
                            if (_instance == null)
                                _instance = new WorkerFactory();
                        }
                    }
                    return _instance;
                }
            }

            /// <summary>
            /// CreateWorker
            /// Crear un empleado
            /// </summary>
            /// <param name="automaticDebit"></param>
            public int CreateWorker(AutomaticDebitModels.AutomaticDebit automaticDebit)
            {

                if (automaticDebit.RetriesNumber == 0)
                {
                    /*
                    Thread thread = new Thread(() => _automaticDebitService.GetAutomaticDebits(automaticDebit));
                    thread.Start();
                    */

                    var task = Task.Run(() => DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit));
                    try
                    {
                        return task.Result.Count;
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException(ex.Message);
                    }
                }

                if (automaticDebit.RetriesNumber == 1)
                {
                    Thread thread = new Thread(() => DelegateService.automaticDebitService.SaveAutomaticDebit(automaticDebit));
                    thread.Start();
                }
                return 1;
            }

            /// <summary>
            /// CreateWorker
            /// Crear un empleado
            /// </summary>
            /// <param name="automaticDebit"></param>
            /// <param name="processTypeId"></param>
            public void CreateWorker(AutomaticDebitModels.AutomaticDebit automaticDebit, int processTypeId)
            {
                
                if (processTypeId == 1)
                {
                    Thread thread = new Thread(() => DelegateService.automaticDebitService.SaveAutomaticDebit(automaticDebit));
                    thread.Start();
                }
                if (processTypeId == 6)
                {
                    Thread thread = new Thread(() => DelegateService.automaticDebitService.UpdateAutomaticDebit(automaticDebit));
                    thread.Start();
                }
            }
        }

        #endregion WorkerFactory

        #endregion Class

        #region Instance Variables


        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainGeneratingCoupons
        /// Genera cupones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainGeneratingCoupons()
        {
            try
            {

                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// MainSendingAdministration
        /// Envia a la pantalla principal
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainSendingAdministration()
        {
            try
            {
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                //TIPO IMPUTACION
                ViewBag.ImputationType = ConfigurationManager.AppSettings["ImputationTypeAutomaticDebit"];

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainLoadBankResponse
        /// Carga pantalla principal
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainLoadBankResponse()
        {
            try
            {
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainReportCouponsStatus
        /// Reporte del estado de cupones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainReportCouponsStatus()
        {
            try
            {
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ViewBag.Coupon = "";

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainReportCollectedCoupons
        /// Reporte de Collección de Cupones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainReportCollectedCoupons()
        {
            try
            {
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ViewBag.IsReport = "";

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainAutomaticDebitFormat
        /// Asignación de formatos a débitos automáticos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAutomaticDebitFormat()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region GeneratingCoupons

        /// <summary>
        /// GetShipmentStatus
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetShipmentStatus()
        {
            List<AutomaticDebitModels.AutomaticDebitStatus> shipmentStatus = DelegateService.automaticDebitService.GetAutomaticDebitStatus();

            return new UifSelectResult(shipmentStatus);
        }

        /// <summary>
        /// GetLotByStatus
        /// Trae el lote de Generaciòn de Cupones filtrado por el estado
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="processTypeId"></param>
        /// <param name="statusId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetLotByStatus(int bankNetworkId, int processTypeId, int? statusId)
        {
            List<object> lotByStatusResponses = new List<object>();
            JsonResult debitPendingProcessResult = _commonController.GetDebitPendingProcess(bankNetworkId, processTypeId) as JsonResult;

            List<object> debitPendingProcesses = (List<object>)Reflection.TypeModel.GetPropertyValue(debitPendingProcessResult.Data, "aaData");

            foreach (var debitPendingProcess in debitPendingProcesses)
            {
                object cupon = debitPendingProcess;

                if (statusId == (int)Reflection.TypeModel.GetPropertyValue(cupon, "statusCode"))
                {
                    lotByStatusResponses.Add(debitPendingProcess);
                }
            }

            return new UifTableResult(lotByStatusResponses);
        }

        /// <summary>
        /// GenerateCoupons
        /// Generación de Cupones
        /// </summary>
        /// <param name="netId"></param>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        /// <param name="policyNumber"></param>
        /// <param name="sendDateTime"></param>
        /// <param name="retryCount"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GenerateCoupons(int netId, int branchCode, int prefixCode, string policyNumber, string sendDateTime, int retryCount)
        {
            List<object> automaticDebits = new List<object>();

            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();
            List<AutomaticDebitModels.Coupon> coupons = new List<AutomaticDebitModels.Coupon>();
            Policy policy = new Policy();
            Branch branch = new Branch() { Id = branchCode, Description = User.Identity.Name.ToUpper() };
            Prefix prefix = new Prefix() { Id = prefixCode };
            policy.Branch = branch;
            policy.Prefix = prefix;
            policy.DocumentNumber = Convert.ToInt32(policyNumber);
            policy.Id = -2;
            policy.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            automaticDebit.BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = netId };
            coupons.Add(new AutomaticDebitModels.Coupon() { Id = 0, Policy = policy });
            automaticDebit.Coupons = coupons;
            automaticDebit.Date = Convert.ToDateTime(sendDateTime);
            automaticDebit.Description = "";
            automaticDebit.Id = Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]);
            automaticDebit.RetriesNumber = retryCount;

            // Se valida que se ejecute una vez 
            AutomaticDebitModels.AutomaticDebit newAutomaticDebit = DelegateService.automaticDebitService.SaveAutomaticDebit(automaticDebit);
            if (newAutomaticDebit.Id == 0)
            {
                policy.Id = 1;
                WorkerFactory.Instance.CreateWorker(automaticDebit, 1);

                automaticDebits.Add(new
                {
                    LotNumber = 1,
                    MessageError = "OK"
                });
            }
            else
            {
                automaticDebits.Add(new
                {
                    LotNumber = newAutomaticDebit.Id,
                    MessageError = newAutomaticDebit.Description
                });
            }

            return Json(automaticDebits, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// ApplyCollections
        /// Aplicación
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="sendDateTime"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ApplyCollections(int bankNetworkId, string sendDateTime)
        {
            string automaticDebitMessage = "";
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            List<AutomaticDebitModels.Coupon> coupons = new List<AutomaticDebitModels.Coupon>();
            Policy policy = new Policy();
            policy.Id = -1;
            policy.UserId = userId;

            coupons.Add(new AutomaticDebitModels.Coupon() { Id = 0, Policy = policy });
            automaticDebit.Coupons = coupons;
            automaticDebit.Date = Convert.ToDateTime(sendDateTime);
            automaticDebit.Description = "";
            automaticDebit.Id = bankNetworkId;
            automaticDebit.BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = bankNetworkId };
            automaticDebit.RetriesNumber = 1; //Aplicar
            automaticDebit.UserId = userId;

            try
            {
                WorkerFactory.Instance.CreateWorker(automaticDebit);
                automaticDebitMessage = "true";
            }
            catch (BusinessException ex)
            {
                automaticDebitMessage = ex.Message;
            }

            automaticDebitMessage = automaticDebit.Description;

            return Json(automaticDebitMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SendingAdministration

        /// <summary>
        /// GetSummaryByNet
        /// Obtiene el resumen de acuerdo a la red
        /// </summary>
        /// <param name="netId"></param>
        /// <param name="sendDate"></param>
        /// <param name="lotNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetSummaryByNet(int netId, string sendDate, string lotNumber)
        {
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = netId, Description = "" },
                Date = Convert.ToDateTime(sendDate),
                Description = lotNumber, //id_envio
                RetriesNumber = -1       //sn_agrupar
            };

            List<Dto.AutomaticDebitSummaryDTO> automaticDebitSummaries = DelegateService.automaticDebitService.GetAutomaticDebitSummary(automaticDebit);

            return new UifTableResult(automaticDebitSummaries);
        }

        /// <summary>
        /// GetLotStatus
        /// Obtiene los estados perdidos
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetLotStatus(int statusCode)
        {
            string statusDescription = "";
            AutomaticDebitModels.AutomaticDebit automaticDebitModel = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = 0, RetriesNumber = 1 },
                Date = DateTime.Now,
                Id = 2
            };

            List<AutomaticDebitModels.AutomaticDebit> automaticDebitsResults = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebitModel);

            if (automaticDebitsResults.Count > 0)
            {
                foreach (AutomaticDebitModels.AutomaticDebit automaticDebit in automaticDebitsResults)
                {
                    if (statusCode == automaticDebit.Id)
                    {
                        statusDescription = automaticDebit.Description;
                    }
                }
            }
            return Json(statusDescription, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GeneratePreNotificationExcel
        /// Genera prenotificación
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="sendDate"></param>
        /// <param name="lotNumber"></param>
        /// <param name="prefixes"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GeneratePreNotificationExcel(int bankNetworkId, string sendDate, string lotNumber,
                                                         string[] prefixes)
        {
            MemoryStream memoryStream = new MemoryStream();
            string prefix = "";

            foreach (string item in prefixes)
            {
                prefix = prefix + item;
            }

            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();
            automaticDebit.BankNetwork = new AutomaticDebitModels.BankNetwork()
            {
                Id = bankNetworkId,
                Description = prefix,
                RetriesNumber = 2
            };

            automaticDebit.Date = Convert.ToDateTime(sendDate);
            automaticDebit.RetriesNumber = 0; //sn_agrupar
            automaticDebit.Description = lotNumber;
            automaticDebit.Id = 2;

            automaticDebit.AutomaticDebitStatus = new AutomaticDebitModels.AutomaticDebitStatus();
            automaticDebit.AutomaticDebitStatus.Id = 1; // Número de página
            List<AutomaticDebitModels.Coupon> cupons = new List<AutomaticDebitModels.Coupon>();
            cupons.Add(new AutomaticDebitModels.Coupon() { Id = 3000 });       // PAGE_SIZE
            automaticDebit.Coupons = cupons;

            int userId = 0;
            if (User != null)
            {
                userId = _commonController.GetUserIdByName(User.Identity.Name);
            }
            else
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }
            automaticDebit.UserId = userId;

            int recordsNumber = 0;
            int pageSize = 0;
            int pageNumber = 0;

            List<AutomaticDebitModels.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);

            List<CouponsStatusDto> couponsStatus = new List<CouponsStatusDto>();
            if ((automaticDebits.Count > 0) && (automaticDebits[0].Coupons != null))
            {
                recordsNumber = automaticDebits[0].Coupons[0].Policy.PaymentPlan.Id;//total de cupones
                pageNumber = recordsNumber / 3000;
                if ((recordsNumber % 3000) < 3000)
                {
                    pageNumber++;
                }
                pageSize = automaticDebits.Count;

                foreach (AutomaticDebitModels.AutomaticDebit automaticDebitModel in automaticDebits)
                {
                    CouponsStatusDto couponsStatusDto = new CouponsStatusDto();
                    couponsStatusDto.InsuredCode = automaticDebitModel.Id;                                                  //Sucursal
                    couponsStatusDto.SummarySequenceNumber = automaticDebitModel.Coupons[0].CouponStatus.RetriesNumber;     //cod.ramo
                    couponsStatusDto.NameOrigin = automaticDebitModel.BankNetwork.Description;                              //Ramo
                    couponsStatusDto.PolicyNumber = automaticDebitModel.Coupons[0].Policy.DocumentNumber.ToString();        //Poliza
                    couponsStatusDto.CouponIndicator = automaticDebitModel.Coupons[0].CouponStatus.Id;                      //Sufijo
                    couponsStatusDto.LotNumber = automaticDebitModel.Coupons[0].Policy.Endorsement.Number;                  //N° Endoso
                    couponsStatusDto.PolicyId = automaticDebitModel.Coupons[0].CouponStatus.Description;                    //Id. SISE
                    couponsStatusDto.RejectionCode = automaticDebitModel.Coupons[0].CouponStatus.SmallDescription;          //Temporario
                    couponsStatusDto.MethodTypeId = automaticDebitModel.Coupons[0].Policy.Id;                               //Cod.Estado
                    couponsStatusDto.AccountNumber = automaticDebitModel.Coupons[0].CouponStatus.ExternalDescription;       //Cod Rechazo
                    couponsStatusDto.AccountNumberOrigin = automaticDebitModel.Coupons[0].Policy.PaymentPlan.Description;   //Motivo Rechazo
                    couponsStatusDto.TaxBase = automaticDebitModel.Coupons[0].Policy.PayerComponents[0].Amount;             //Total 
                    couponsStatus.Add(couponsStatusDto);
                }

                for (int i = 2; i <= pageNumber; i++)
                {
                    automaticDebit.AutomaticDebitStatus = new AutomaticDebitModels.AutomaticDebitStatus() { Id = i };  // Número de página
                    automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);
                    pageSize = pageSize + automaticDebits.Count;

                    foreach (AutomaticDebitModels.AutomaticDebit automaticDebitModel in automaticDebits)
                    {
                        CouponsStatusDto couponsStatusDto = new CouponsStatusDto();
                        couponsStatusDto.InsuredCode = automaticDebitModel.Id;                                                    //Sucursal
                        couponsStatusDto.SummarySequenceNumber = automaticDebitModel.Coupons[0].CouponStatus.RetriesNumber;       //cod.ramo
                        couponsStatusDto.NameOrigin = automaticDebitModel.BankNetwork.Description;                                //Ramo
                        couponsStatusDto.PolicyNumber = automaticDebitModel.Coupons[0].Policy.DocumentNumber.ToString();          //Poliza
                        couponsStatusDto.CouponIndicator = automaticDebitModel.Coupons[0].CouponStatus.Id;                        //Sufijo
                        couponsStatusDto.LotNumber = automaticDebitModel.Coupons[0].Policy.Endorsement.Number;                    //N° Endoso
                        couponsStatusDto.PolicyId = automaticDebitModel.Coupons[0].CouponStatus.Description;                      //Id. SISE
                        couponsStatusDto.RejectionCode = automaticDebitModel.Coupons[0].CouponStatus.SmallDescription;            //Temporario
                        couponsStatusDto.MethodTypeId = automaticDebitModel.Coupons[0].Policy.Id;                                 //Cod.Estado
                        couponsStatusDto.AccountNumber = automaticDebitModel.Coupons[0].CouponStatus.ExternalDescription;         //Cod Rechazo
                        couponsStatusDto.AccountNumberOrigin = automaticDebitModel.Coupons[0].Policy.PaymentPlan.Description;     //Motivo Rechazo
                        couponsStatusDto.TaxBase = automaticDebitModel.Coupons[0].Policy.PayerComponents[0].Amount;               //Total 
                        couponsStatus.Add(couponsStatusDto);
                    }

                    if ((i == pageNumber) && (recordsNumber > pageSize))
                    {
                        pageNumber++;
                    }
                }
            }

            if (automaticDebits.Count > 0)
            {
                memoryStream = ExportDebits(ConvertDebitsToDataTable(couponsStatus), lotNumber);
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "Lote" + lotNumber + ".xls");
        }

        /// <summary>
        /// GetRequiresNotification
        /// Genera la notificación 
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="sendDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRequiresNotification(int bankNetworkId, string sendDate)
        {
            string requiresNotificationMessage = "";
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = bankNetworkId, RetriesNumber = 1 },
                Date = Convert.ToDateTime(sendDate),
                Id = 3
            };

            List<AutomaticDebitModels.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);

            if (automaticDebits.Count > 0)
            {
                requiresNotificationMessage = automaticDebits.Select(sl => sl.Description).First().ToString();
            }

            return Json(requiresNotificationMessage, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GeneratePreNotification
        /// Generar prenotificación
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="sendDate"></param>
        /// <param name="processTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GeneratePreNotification(int bankNetworkId, string sendDate, int processTypeId)
        {
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = bankNetworkId, RetriesNumber = 3 },
                Date = Convert.ToDateTime(sendDate),
                Id = 0,
                RetriesNumber = 0,
                UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper())
            };

            string fileName = "";

            try
            {
                string accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["AccountingModule"]), DateTime.Now).ToString("dd/MM/yyyy");
                automaticDebit.AutomaticDebitStatus = new AutomaticDebitModels.AutomaticDebitStatus()
                {
                    Description = accountingDate,
                    Id = processTypeId
                };

                WorkerFactory.Instance.CreateWorker(automaticDebit);

                fileName = "true";
            }
            catch (BusinessException ex)
            {
                fileName = ex.Message;
            }

            return Json(fileName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene el detalle de cupones por numero de envio
        /// </summary>
        /// <param name="netId">id de la red</param>
        /// <param name="sendDate">Fecha de Envio</param>
        /// <param name="lotNumber">Número de Lote</param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDetailByNet(int netId, string sendDate, string lotNumber)
        {

            List<object> couponResponses = new List<object>();
            /*TODO LFREIRE Este método es utilizado en la Administración de Envíos / Cancelación Manual pendiente de implementación EE
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();

            automaticDebit.BankNetwork = new BankNetwork() { Id = netId, Description = "" };
            automaticDebit.Date = Convert.ToDateTime(sendDate);
            automaticDebit.RetriesNumber = 0;      //sn_agrupar
            automaticDebit.Description = lotNumber; //id_envio

            automaticDebit = _automaticDebitService.GetAutomaticDebit(automaticDebit);

            if (automaticDebit.Coupons != null)
            {
                foreach (Coupon coupon in automaticDebit.Coupons)
                {
                    couponResponses.Add(new
                    {
                        PaymentMethodCd = Convert.ToInt32(coupon.Policy.Risks[0].CoverageGroups[0].Coverages[0].Id),
                        RejectionBankId = 0,
                        RejectionInternalId = 0,
                        TotalPremium = Convert.ToDecimal(coupon.Policy.Risks[0].CoverageGroups[0].Accessories[0].PremiumAmt),
                        CouponsNumber = Convert.ToInt32(coupon.Id),
                        CouponSecuence = Convert.ToInt32(coupon.Policy.Product.Id),
                        DueDate = coupon.Policy.CurrentTo.ToString("dd/MM/yyyy").Substring(0, 10),
                        InsuredName = coupon.Policy.PolicyHolderPerson.Name,
                        Prefix = coupon.Policy.Prefix.Description,
                        Policy = coupon.Policy.DocumentNumber,
                        Endorsement = coupon.Policy.Endorsements[0].EndorsementNumber,
                        Quota = coupon.Policy.Risks[0].Number,
                        RejectionReasonBank = "",
                        RejectionReasonInternal = "",
                        Status = coupon.CouponStatus.Description
                    });
                }
            }
            */
            return new UifTableResult(couponResponses);
        }

        /// <summary>
        /// GetRejectionReason
        /// Obtiene motivos de rechazo por red
        /// </summary>
        /// <param name="networkId"></param>
        /// <returns>automaticDebitsResponse Objeto</returns>
        public ActionResult GetRejectionReason(int networkId)
        {
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork()
                {
                    Id = networkId,
                    RetriesNumber = 6
                },
            };

            List<AutomaticDebitModels.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);

            var automaticDebitsResponse = from items in automaticDebits
                                          select new
                                          {
                                              Id = items.BankNetwork.Description,
                                              Description = items.Description
                                          };

            return new UifSelectResult(automaticDebitsResponse);
        }

        /// <summary>
        /// SaveCouponRejectionManual
        /// </summary>
        /// <param name="rejectionCoupons"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCouponRejectionManual(List<CouponModel> rejectionCoupons)
        {
            string result = string.Empty;
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();

            try
            {
                automaticDebit.Id = 0;
                List<AutomaticDebitModels.Coupon> coupons = new List<AutomaticDebitModels.Coupon>();

                foreach (CouponModel rejection in rejectionCoupons)
                {
                    AutomaticDebitModels.Coupon coupon = new AutomaticDebitModels.Coupon()
                    {
                        CouponStatus = new AutomaticDebitModels.CouponStatus()
                        {
                            Description = rejection.RejectionBankId,
                            CouponStatusType = AutomaticDebitModels.CouponStatusTypes.Rejected
                        },
                        Id = rejection.CouponsNumber,
                        Policy = new Policy()
                        {
                            DocumentNumber = rejection.CouponSecuence,
                            Id = -1
                        }
                    };
                    coupons.Add(coupon);
                }
                automaticDebit.Coupons = coupons;

                DelegateService.automaticDebitService.UpdateAutomaticDebit(automaticDebit);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region LoadBankResponse

        /// <summary>
        /// UpdateBankResponse
        /// Actualiza respuesta del Banco
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="lotNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult UpdateBankResponse(int bankNetworkId, string lotNumber)
        {
            List<object> automaticDebitResponses = new List<object>();

            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();
            List<AutomaticDebitModels.Coupon> coupons = new List<AutomaticDebitModels.Coupon>();
            Policy policy = new Policy();
            Branch branch = new Branch()
            {
                Description = User.Identity.Name.ToUpper(),
                Id = _commonController.GetUserIdByName(User.Identity.Name),
            };
            policy.Branch = branch;
            policy.Id = 1;
            policy.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            automaticDebit.BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = bankNetworkId, Description = lotNumber };
            coupons.Add(new AutomaticDebitModels.Coupon() { Id = 0, Policy = policy });
            automaticDebit.Coupons = coupons;
            automaticDebit.Description = "";
            automaticDebit.Id = 0;

            WorkerFactory.Instance.CreateWorker(automaticDebit, 6);

            automaticDebitResponses.Add(new
            {
                LotNumber = 1,
                MessageError = "OK"
            });

            return Json(automaticDebitResponses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadFileInMemory
        /// Lee el registro en memoria
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="bankNetworkId"></param>
        /// <param name="lotNumber"></param>
        /// <param name="fileType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile, int? bankNetworkId,
                                           string lotNumber, string fileType)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ReadExcelFileToStream(uploadedFile, Convert.ToInt32(bankNetworkId), lotNumber, fileType);
            }
            if (data[1] == "txt")
            {
                return ReadTextFileToString(uploadedFile, Convert.ToInt32(bankNetworkId), lotNumber, fileType);
            }
            else
            {
                fileLocationName = "BadFileExtension";
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadExcelFileToStream
        /// Lee un documento excel
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="bankNetworkId"></param>
        /// <param name="lotNumber"></param>
        /// <param name="fileType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadExcelFileToStream(HttpPostedFileBase uploadedFile, int bankNetworkId, string lotNumber, string fileType)
        {
            bool successful = true;
            string fileLocationName = "";
            Byte[] arrayContent;

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            //Lee el archivo y guarda en arreglo de tipo byte y este a su vez a arrayContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            IExcelDataReader excelReader;
            List<LoadErrorDto> loadErrors = new List<LoadErrorDto>();

            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            try
            {
                if (data[1] == "xls")
                {
                    //1. Lee desde binary Excel  ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else
                {
                    //2. Lee desde binary OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                //3. DataSet - El resultado sera creado en result.Tables
                DataSet dataSet = excelReader.AsDataSet();

                int notification = 0;
                int formatId = 0;
                var bankNetworks = DelegateService.automaticDebitService.GetBankNetworks().Where(x => x.Id == bankNetworkId).ToList();
                bool requiresNotification = bankNetworks[0].RequiresNotification;

                List<AutomaticDebitModels.AutomaticDebitFormat> automaticDebitFormats;

                if (fileType.Equals("N"))
                {
                    if (!requiresNotification)
                    {
                        return Json("NotAllowedPreNotification", JsonRequestBehavior.AllowGet);
                    }
                    automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId).Where(sl => sl.FormatUsingType == AutomaticDebitModels.FormatUsingTypes.ReceptionNotification).ToList();
                    formatId = automaticDebitFormats[0].Format.Id;
                    notification = -1;
                }
                else
                {
                    automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId).Where(sl => sl.FormatUsingType == AutomaticDebitModels.FormatUsingTypes.Reception).ToList();
                    formatId = automaticDebitFormats[0].Format.Id;
                    notification = 0;
                }

                Report.Format format = new Report.Format()
                {
                    Id = formatId,
                    FileType = Report.FileTypes.Text
                };

                List<Report.FormatDetail> formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);

                if (formatDetails.Count == 0)
                {
                    return Json("NotParameterizedFormat", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dataSet.Tables[0].Rows[i];

                        string cardAccountNumber = "";
                        string applicationDate = "";
                        string rejectionCode = "";
                        string voucherNumber = "";
                        string documentNumber = "";
                        string amount = "";
                        string authorizationNumber = "";

                        foreach (Report.FormatDetail formats in formatDetails)
                        {
                            if (formats.FormatType == Report.FormatTypes.Head)
                            {
                                foreach (Report.FormatField detailformat in formats.Fields)
                                {
                                    string headerValue = row[detailformat.Id - 1].ToString();
                                }
                            }

                            if (formats.FormatType == Report.FormatTypes.Detail)
                            {
                                foreach (Report.FormatField detailformat in formats.Fields)
                                {
                                    try
                                    {
                                        if (detailformat.Field == ConfigurationManager.AppSettings["CardAccountNumber"].ToString())
                                        {
                                            cardAccountNumber = row[detailformat.Id - 1].ToString();
                                        }
                                        if (detailformat.Field == ConfigurationManager.AppSettings["ApplicationDate"].ToString())
                                        {
                                            applicationDate = SetFormatType(detailformat.Mask, row[detailformat.Id - 1].ToString());
                                        }
                                        if (detailformat.Field == ConfigurationManager.AppSettings["RejectionCode"].ToString())
                                        {
                                            rejectionCode = row[detailformat.Id - 1].ToString();
                                        }
                                        if (detailformat.Field == ConfigurationManager.AppSettings["VoucherNumber"].ToString())
                                        {
                                            voucherNumber = row[detailformat.Id - 1].ToString();
                                        }
                                        if (detailformat.Field == ConfigurationManager.AppSettings["DocumentNumber"].ToString())
                                        {
                                            documentNumber = row[detailformat.Id - 1].ToString();
                                        }
                                        if (detailformat.Field == ConfigurationManager.AppSettings["Amount"].ToString())
                                        {
                                            amount = SetFormatType(detailformat.Mask, row[detailformat.Id - 1].ToString());
                                        }
                                        if (detailformat.Field == ConfigurationManager.AppSettings["AuthorizationNumber"].ToString())
                                        {
                                            authorizationNumber = row[detailformat.Id - 1].ToString();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        fileLocationName = ex.Message;
                                        successful = false;
                                    }
                                }//end detailList

                                AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();
                                automaticDebit.BankNetwork = new AutomaticDebitModels.BankNetwork()
                                {
                                    Id = bankNetworkId,
                                    Description = lotNumber
                                };
                                List<AutomaticDebitModels.Coupon> coupons = new List<AutomaticDebitModels.Coupon>();
                                AutomaticDebitModels.Coupon coupon = new AutomaticDebitModels.Coupon();
                                Policy policy = new Policy();
                                policy.Branch = new Branch() { Description = voucherNumber };
                                policy.ExchangeRate = new ExchangeRate()
                                {
                                    Currency = new Currency() { Description = rejectionCode }
                                };
                                policy.Id = 0;
                                policy.Holder = new Holder() { IndividualId = userId, Name = User.Identity.Name };
                                policy.PolicyType = new PolicyType() { Description = documentNumber };
                                policy.Prefix = new Prefix() { Id = notification, Description = authorizationNumber };
                                policy.BillingGroup = new BillingGroup() { Description = amount };
                                coupon.Policy = policy;
                                coupons.Add(coupon);
                                automaticDebit.Coupons = coupons;
                                automaticDebit.Date = Convert.ToDateTime(applicationDate);
                                automaticDebit.Description = cardAccountNumber;
                                automaticDebit.UserId = userId;

                                automaticDebit = DelegateService.automaticDebitService.UpdateAutomaticDebit(automaticDebit);

                                if (automaticDebit.Description != "")
                                {
                                    loadErrors.Add(new LoadErrorDto() { Id = i + 1, Description = automaticDebit.Description });
                                }
                            }

                            if (formats.FormatType == Report.FormatTypes.Summary)
                            {
                                foreach (Report.FormatField detailformat in formats.Fields)
                                {
                                    string summaryValue = row[detailformat.Id - 1].ToString();
                                }
                            }
                        }
                    }//fin de bucle
                }
            }
            catch (FormatException)
            {
                fileLocationName = "FormatException";
                successful = false;
            }
            catch (OverflowException)
            {
                fileLocationName = "OverflowException";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                fileLocationName = "IndexOutOfRangeException";
                successful = false;
            }
            catch (InvalidCastException)
            {
                fileLocationName = "InvalidCastException";
                successful = false;
            }
            catch (Exception)
            {
                fileLocationName = "Exception";
                successful = false;
            }

            stream.Close();

            if (!successful)
            {
                if (loadErrors.Count > 0)
                {
                    fileLocationName = "ErrorLoadBankResponse";
                    TempData["dtoLoadError"] = loadErrors;
                }
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }

            fileLocationName = "SuccessfulLoadBankResponse";
            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadTextFileToString
        /// Lee un documento en txt para convertirlo en string
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="bankNetworkId"></param>
        /// <param name="lotNumber"></param>
        /// <param name="fileType"></param>
        /// <returns>JsonResult</returns>
        private JsonResult ReadTextFileToString(HttpPostedFileBase uploadedFile, int bankNetworkId,
                                                string lotNumber, string fileType)
        {
            List<Report.FormatField> formatHeaders = new List<Report.FormatField>();
            List<Report.FormatField> formatDetails = new List<Report.FormatField>();
            List<Report.FormatField> formatSummaries;

            string documentErrors = "";

            string fileLocationName = uploadedFile.FileName;

            // convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            //Lee el archivo y guarda en arreglo de tipo byte y este a su vez a arrayContent
            Byte[] arrayContent = buffer;

            List<LoadErrorDto> loadErrors = new List<LoadErrorDto>();
            Stream stream = new MemoryStream(arrayContent);
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            try
            {
                int index = 0;
                string line;

                int notification = 0;
                int formatId = 0;
                var bankNetworks = DelegateService.automaticDebitService.GetBankNetworks().Where(x => x.Id == bankNetworkId).ToList();
                bool requiresNotification = bankNetworks[0].RequiresNotification;

                StreamReader file = new StreamReader(stream);

                List<AutomaticDebitModels.AutomaticDebitFormat> automaticDebitFormats;

                if (fileType.Equals("N"))
                {
                    if (!requiresNotification)
                    {
                        return Json("NotAllowedPreNotification", JsonRequestBehavior.AllowGet);
                    }
                    automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId).Where(sl => sl.FormatUsingType == AutomaticDebitModels.FormatUsingTypes.ReceptionNotification).ToList();
                    formatId = automaticDebitFormats[0].Format.Id;
                    notification = -1;
                }
                else
                {
                    automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId).Where(sl => sl.FormatUsingType == AutomaticDebitModels.FormatUsingTypes.Reception).ToList();
                    formatId = automaticDebitFormats[0].Format.Id;
                    notification = 0;
                }

                Report.Format format = new Report.Format()
                {
                    Id = formatId,
                    FileType = Report.FileTypes.Text
                };

                List<Report.FormatDetail> formatDetail = DelegateService.reportingService.GetFormatDetailsByFormat(format);
                if (formatDetail.Count > 0)
                {
                    bool head = false;
                    string separatorHead = "";
                    string separatorDetail = "";
                    string separatorSumary = "";

                    foreach (Report.FormatDetail formats in formatDetail)
                    {
                        if (formats.FormatType == Report.FormatTypes.Head)
                        {
                            head = true;
                            separatorHead = formats.Separator.Trim();
                            formatHeaders = formats.Fields.OrderBy(o => o.Order).ToList();
                        }
                        if (formats.FormatType == Report.FormatTypes.Detail)
                        {
                            separatorDetail = formats.Separator.Trim();
                            formatDetails = formats.Fields.OrderBy(o => o.Order).ToList();
                        }
                        else
                        {
                            separatorSumary = formats.Separator.Trim();
                            formatSummaries = formats.Fields;
                        }
                    }

                    while ((line = file.ReadLine()) != null)
                    {
                        try
                        {
                            if (line != "")
                            {
                                string cardAccountNumber = "";
                                string applicationDate = "";
                                string rejectionCode = "";
                                string voucherNumber = "";
                                string documentNumber = "";
                                string amount = "";
                                string authorizationNumber = "";

                                #region "Format Type Head"
                                if (head)
                                {
                                    foreach (Report.FormatField detailformat in formatHeaders)
                                    {
                                        try
                                        {
                                            var boolFormat = (detailformat.Field == ConfigurationManager.AppSettings["ApplicationDate"].ToString());
                                            if (separatorHead != "")
                                            {
                                                string[] data = line.Split(new Char[] { Convert.ToChar(separatorHead) });
                                                if (boolFormat)
                                                {
                                                    applicationDate = SetFormatType(detailformat.Mask, data[detailformat.Id - 1]);
                                                }
                                            }
                                            else
                                            {
                                                string headerValue = line.Substring(detailformat.Start - 1, detailformat.Length).Trim();
                                                if (boolFormat)
                                                {
                                                    applicationDate = SetFormatType(detailformat.Mask, headerValue);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new BusinessException(ex.Message);
                                        }
                                    }
                                }
                                #endregion "Format Type Head"

                                #region "Format Type Detail"

                                if (!head)
                                {
                                    char filled;
                                    foreach (Report.FormatField detailformat in formatDetails)
                                    {
                                        try
                                        {
                                            if (separatorDetail != "")
                                            {
                                                if (line.IndexOf(separatorDetail) > -1)
                                                {
                                                    string[] data = line.Split(new Char[] { Convert.ToChar(separatorDetail) });

                                                    if (detailformat.Field == ConfigurationManager.AppSettings["CardAccountNumber"].ToString())
                                                    {
                                                        cardAccountNumber = data[detailformat.Id - 1];
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["ApplicationDate"].ToString())
                                                    {
                                                        applicationDate = SetFormatType(detailformat.Mask, data[detailformat.Id - 1]);
                                                        if (applicationDate == "01/01/0001 0:00:00")
                                                        {
                                                            loadErrors.Add(new LoadErrorDto() { Id = index + 1, Description = applicationDate + " " + Global.BillingInvalidDate });
                                                            break;
                                                        }
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["RejectionCode"].ToString())
                                                    {
                                                        rejectionCode = data[detailformat.Id - 1];
                                                    }

                                                    if (detailformat.Field == ConfigurationManager.AppSettings["VoucherNumber"].ToString())
                                                    {
                                                        voucherNumber = data[detailformat.Id - 1];
                                                    }

                                                    if (detailformat.Field == ConfigurationManager.AppSettings["DocumentNumber"].ToString())
                                                    {
                                                        documentNumber = data[detailformat.Id - 1];
                                                    }

                                                    if (detailformat.Field == ConfigurationManager.AppSettings["Amount"].ToString())
                                                    {
                                                        amount = SetFormatType(detailformat.Mask, data[detailformat.Id - 1]);
                                                    }

                                                    if (detailformat.Field == ConfigurationManager.AppSettings["AuthorizationNumber"].ToString())
                                                    {
                                                        authorizationNumber = data[detailformat.Id - 1];
                                                    }
                                                }
                                                else
                                                {
                                                    documentErrors = Global.DocumentErrors;
                                                    fileLocationName = "InvalidSeparator";
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                documentErrors = "";
                                                string value = line.Substring(detailformat.Start - 1, detailformat.Length).Trim();
                                                filled = Convert.ToChar(detailformat.Filled);
                                                if (value != "")
                                                {
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["CardAccountNumber"].ToString())
                                                    {
                                                        if ((detailformat.Filled == "" || detailformat.Filled == " ") && (detailformat.Align == "" || detailformat.Align == " "))
                                                        {
                                                            cardAccountNumber = value;
                                                        }
                                                        else
                                                        {
                                                            cardAccountNumber = (detailformat.Align == "I") ? value.TrimStart(filled) : value.TrimEnd(filled);
                                                        }
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["ApplicationDate"].ToString())
                                                    {
                                                        applicationDate = SetFormatType(detailformat.Mask, value);
                                                        if (applicationDate == "01/01/0001 0:00:00")
                                                        {
                                                            loadErrors.Add(new LoadErrorDto() { Id = index + 1, Description = applicationDate + " " + Global.BillingInvalidDate });
                                                            break;
                                                        }
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["RejectionCode"].ToString())
                                                    {
                                                        if ((detailformat.Filled == "" || detailformat.Filled == " ") && (detailformat.Align == "" || detailformat.Align == " "))
                                                        {
                                                            rejectionCode = value;
                                                        }
                                                        else
                                                        {
                                                            rejectionCode = (detailformat.Align == "I") ? value.TrimStart(filled) : value.TrimEnd(filled);
                                                        }
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["VoucherNumber"].ToString())
                                                    {
                                                        if ((detailformat.Filled == "" || detailformat.Filled == " ") && (detailformat.Align == "" || detailformat.Align == " "))
                                                        {
                                                            voucherNumber = value;
                                                        }
                                                        else
                                                        {
                                                            voucherNumber = (detailformat.Align == "I") ? value.TrimStart(filled) : value.TrimEnd(filled);
                                                        }
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["DocumentNumber"].ToString())
                                                    {
                                                        if ((detailformat.Filled == "" || detailformat.Filled == " ") && (detailformat.Align == "" || detailformat.Align == " "))
                                                        {
                                                            documentNumber = value;
                                                        }
                                                        else
                                                        {
                                                            documentNumber = (detailformat.Align == "I") ? value.TrimStart(filled) : value.TrimEnd(filled);
                                                        }
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["Amount"].ToString())
                                                    {
                                                        if ((detailformat.Filled == "" || detailformat.Filled == " ") && (detailformat.Align == "" || detailformat.Align == " "))
                                                        {
                                                            amount = value;
                                                        }
                                                        else
                                                        {
                                                            amount = (detailformat.Align == "I") ? value.TrimStart(filled) : value.TrimEnd(filled);
                                                        }

                                                        amount = SetFormatType(detailformat.Mask, amount);
                                                    }
                                                    if (detailformat.Field == ConfigurationManager.AppSettings["AuthorizationNumber"].ToString())
                                                    {
                                                        if ((detailformat.Filled == "" || detailformat.Filled == " ") && (detailformat.Align == "" || detailformat.Align == " "))
                                                        {
                                                            authorizationNumber = value;
                                                        }
                                                        else
                                                        {
                                                            authorizationNumber = (detailformat.Align == "I") ? value.TrimStart(filled) : value.TrimEnd(filled);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    documentErrors = Global.DocumentErrors;
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            fileLocationName = "ErrorLoadBankResponse";
                                            documentErrors = Global.DocumentErrors;
                                        }
                                    }// End detailList

                                    if (documentErrors != Global.DocumentErrors)
                                    {
                                        AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit();
                                        automaticDebit.BankNetwork = new AutomaticDebitModels.BankNetwork()
                                        {
                                            Id = bankNetworkId,
                                            Description = lotNumber
                                        };
                                        List<AutomaticDebitModels.Coupon> coupons = new List<AutomaticDebitModels.Coupon>();
                                        AutomaticDebitModels.Coupon coupon = new AutomaticDebitModels.Coupon();
                                        Policy policy = new Policy();
                                        policy.Branch = new Branch() { Description = voucherNumber };
                                        policy.ExchangeRate = new ExchangeRate() { Currency = new Currency() { Description = rejectionCode } };
                                        policy.Id = 0;
                                        policy.Holder = new Holder() { InsuredId = userId, Name = User.Identity.Name, IndividualId = index + 1 };
                                        policy.PolicyType = new PolicyType() { Description = documentNumber };
                                        policy.Prefix = new Prefix() { Id = notification, Description = authorizationNumber };
                                        policy.BillingGroup = new BillingGroup() { Description = amount };
                                        coupon.Policy = policy;
                                        coupons.Add(coupon);
                                        automaticDebit.Coupons = coupons;

                                        if (applicationDate != "")
                                        {
                                            automaticDebit.Date = Convert.ToDateTime(applicationDate);
                                            automaticDebit.Description = cardAccountNumber;
                                            automaticDebit.UserId = userId;

                                            automaticDebit = DelegateService.automaticDebitService.SaveAutomaticDebit(automaticDebit);
                                            if (automaticDebit.Id == 0)
                                            {
                                                loadErrors.Add(new LoadErrorDto() { Id = index + 1, Description = Global.DocumentErrors });
                                            }
                                        }
                                        else
                                        {
                                            loadErrors.Add(new LoadErrorDto() { Id = index + 1, Description = applicationDate + " " + Global.BillingInvalidDate });
                                        }
                                    }
                                }
                                head = false;

                                #endregion "Format Type Detail"

                            }
                        }
                        catch (FormatException)
                        {
                            fileLocationName = "FormatException";
                            break;
                        }
                        catch (OverflowException)
                        {
                            fileLocationName = "OverflowException";
                            break;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            fileLocationName = "IndexOutOfRangeException";
                            break;
                        }
                        catch (InvalidCastException)
                        {
                            fileLocationName = "InvalidCastException";
                            break;
                        }
                        catch (NullReferenceException)
                        {
                            fileLocationName = "NullReferenceException";
                            break;
                        }
                    }
                    file.Close();
                }
            }
            catch (Exception)
            {
                fileLocationName = "SuccessfulLoadBankResponse";
            }

            if (loadErrors.Count > 0)
            {
                TempData["dtoLoadError"] = loadErrors;

                fileLocationName = "ErrorLoadBankResponse";
            }
            else
            {
                if (fileLocationName == uploadedFile.FileName)
                {
                    fileLocationName = "SuccessfulLoadBankResponse";
                }
            }

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExportToExcelLogBankResponse
        /// Exporta a excel el log de la respuesta del banco
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExportToExcelLogBankResponse()
        {
            DataTable dataTable = new DataTable();
            List<LoadErrorDto> loadErrors = (List<LoadErrorDto>)TempData["dtoLoadError"];

            //Get the type of source object and create a new instance of that type

            var headerRow = new List<string>(2);
            headerRow.Add(@Global.RowNumber);
            headerRow.Add(@Global.ErrorDescription);

            for (int j = 0; j < headerRow.Count; j++)
            {
                dataTable.Columns.Add(headerRow[j]);
            }

            try
            {
                foreach (LoadErrorDto error in loadErrors)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow[0] = error.Id;
                    dataRow[1] = error.Description;

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(@Global.RowNumber);
            header.CreateCell(1).SetCellValue(@Global.ErrorDescription);

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);

            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            header.GetCell(0).CellStyle = styleHeader;
            header.GetCell(1).CellStyle = styleHeader;

            int rowNumber = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styleDetail;
                }
            }

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "ErroresCargaRespustaBanco.xls");
        }

        /// <summary>
        /// ExportToExcelErrorLoadBankResponse
        /// Exporta a excel los errores de la repuesta del banco
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="lotNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ExportToExcelErrorLoadBankResponse(int bankNetworkId, string lotNumber)
        {
            DataTable dataTable = new DataTable();
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = bankNetworkId, Description = lotNumber, RetriesNumber = 4 },
                Id = 7,
                UserId = _commonController.GetUserIdByName(User.Identity.Name)
            };

            List<AutomaticDebitModels.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);

            //Get the type of source object and create a new instance of that type

            var headerRows = new List<string>(3);
            headerRows.Add(@Global.LotNumber);
            headerRows.Add(@Global.RowNumber);
            headerRows.Add(@Global.ErrorDescription);

            for (int j = 0; j < headerRows.Count; j++)
            {
                dataTable.Columns.Add(headerRows[j]);
            }

            try
            {
                foreach (AutomaticDebitModels.AutomaticDebit automaticDebitItem in automaticDebits)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = lotNumber;
                    dataRow[1] = automaticDebitItem.Coupons[0].CouponStatus.Id;
                    dataRow[2] = automaticDebitItem.Description;
                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            var header = sheet.CreateRow(0);

            header.CreateCell(0).SetCellValue(@Global.LotNumber);
            header.CreateCell(1).SetCellValue(@Global.RowNumber);
            header.CreateCell(2).SetCellValue(@Global.ErrorDescription);

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 20 * 256);
            sheet.SetColumnWidth(1, 40 * 256);

            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            header.GetCell(0).CellStyle = styleHeader;
            header.GetCell(1).CellStyle = styleHeader;
            header.GetCell(2).CellStyle = styleHeader;

            int rowNumber = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styleDetail;
                }
            }

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "ErroresCargaRespuestaBanco_" + lotNumber + ".xls");
        }

        #endregion

        #region ReportCouponsStatus

        /// <summary>
        /// GenerateReportCouponsStatus
        /// Genera reporte del estado de los cupones
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GenerateReportCouponsStatus(string dateFrom, string dateTo)
        {
            List<Dto.AutomaticDebitsDTO> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebitsDetail(Convert.ToDateTime(dateTo), Convert.ToDateTime(dateFrom));

            MemoryStream memoryStream = new MemoryStream();

            //Si no hay registros devuelve el archivo vacío
            if (automaticDebits.Count > 0)
            {
                memoryStream = ExportReportCouponsStatus(ConvertCouponsStatusToDataTable(automaticDebits));
            }
            else
            {
                memoryStream = CreateNotFoundRowsReport();
            }
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "EstadoCuponesExcel.xls");
        }

        /// <summary>
        /// ConvertCouponsStatusToDataTable
        /// Convierte el estado de cupone en una tabla
        /// </summary>
        /// <param name="automaticDebits"></param>
        /// <returns>DataTable</returns>
        private DataTable ConvertCouponsStatusToDataTable(List<Dto.AutomaticDebitsDTO> automaticDebits)
        {
            DataTable dataTable = new DataTable();
            //Get the type of source object and create a new instance of that type

            var headerRows = new List<string>(7);
            headerRows.Add(@Global.BranchCode);
            headerRows.Add(@Global.Branch);
            headerRows.Add(@Global.NetCode);
            headerRows.Add(@Global.Net);
            headerRows.Add(@Global.SendingId);
            headerRows.Add(@Global.ProcessDate);
            headerRows.Add(@Global.Status);

            for (int j = 0; j < headerRows.Count; j++)
            {
                dataTable.Columns.Add(headerRows[j]);
            }

            try
            {
                foreach (Dto.AutomaticDebitsDTO item in automaticDebits)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = item.BranchId;
                    dataRow[1] = item.BranchDescription;
                    dataRow[2] = item.BankNetworkId;
                    dataRow[3] = item.BankNetworkDescription;
                    dataRow[4] = item.Id;
                    dataRow[5] = item.ProcessDate;
                    dataRow[6] = item.StatusDescription;

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return dataTable;
        }

        /// <summary>
        /// ExportReportCouponsStatus
        /// Exporta a reporte el estado de cupones
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportReportCouponsStatus(DataTable dataTable)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue(@Global.BranchCode);
            headerRow.CreateCell(1).SetCellValue(@Global.Branch);
            headerRow.CreateCell(2).SetCellValue(@Global.NetCode);
            headerRow.CreateCell(3).SetCellValue(@Global.Net);
            headerRow.CreateCell(4).SetCellValue(@Global.SendingId);
            headerRow.CreateCell(5).SetCellValue(@Global.ProcessDate);
            headerRow.CreateCell(6).SetCellValue(@Global.Status);
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(0).CellStyle = styleHeader;
            headerRow.GetCell(1).CellStyle = styleHeader;
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;

            int rowNumber = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styleDetail;
                }
            }

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        #endregion

        #region ReportCollectedCoupons

        /// <summary>
        /// GenerateReportCollectedCoupons
        /// Genera reporte de los cupones cobrados
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="prefixId"></param>
        /// <param name="agentId"></param>
        /// <param name="reportType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GenerateReportCollectedCoupons(string dateFrom, string dateTo, int prefixId, int agentId, string reportType)
        {
            Prefix prefix = new Prefix() { Id = prefixId };
            UniquePersonModels.Agent agent = new UniquePersonModels.Agent() { IndividualId = agentId };
            AutomaticDebitModels.CouponStatus couponStatus = new AutomaticDebitModels.CouponStatus()
            {
                CouponStatusType = AutomaticDebitModels.CouponStatusTypes.Applied,
                SmallDescription = reportType
            };

            List<Dto.CouponStatusDTO> CouponStatus = DelegateService.automaticDebitService.GetCouponStatusDetail(Convert.ToDateTime(dateFrom),
                                                                           Convert.ToDateTime(dateTo), prefix, agent, couponStatus);

            MemoryStream memoryStream = ExportReportCollectedCoupons(ConvertCollectedCouponsToDataTable(CouponStatus));

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "CuponesRecaudadosExcel.xls");
        }

        /// <summary>
        /// ConvertCollectedCouponsToDataTable
        /// Convierte los cupones cobrados en una tabla
        /// </summary>
        /// <param name = "couponStatus" ></ param >
        /// < returns > DataTable </ returns >
        private DataTable ConvertCollectedCouponsToDataTable(List<Dto.CouponStatusDTO> couponStatus)
        {
            DataTable dataTable = new DataTable();

            //Get the type of source object and create a new instance of that type

            var headerRow = new List<string>(11);

            headerRow.Add(@Global.InsuredDocumentNumber);
            headerRow.Add(@Global.InsuredName);
            headerRow.Add(@Global.Net);
            headerRow.Add(@Global.Policy);
            headerRow.Add(@Global.ValueAddedTaxValue);
            headerRow.Add(@Global.ValueAddedTax);
            headerRow.Add(@Global.CollectionResponse);
            headerRow.Add(@Global.ResponseDescription);
            headerRow.Add(@Global.AuthorizationNumber);
            headerRow.Add(@Global.Status);
            headerRow.Add(@Global.TransactionNumber);

            for (int j = 0; j < headerRow.Count; j++)
            {
                dataTable.Columns.Add(headerRow[j]);
            }

            try
            {
                foreach (Dto.CouponStatusDTO coupon in couponStatus)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow[0] = coupon.InsuredDocumentNumber;
                    dataRow[1] = coupon.InsuredName;
                    dataRow[2] = coupon.BankNetworkDescription;
                    dataRow[3] = coupon.PolicyId;
                    dataRow[4] = coupon.LocalAmount;
                    dataRow[5] = coupon.Amount;
                    dataRow[6] = coupon.StatusResponseId;
                    dataRow[7] = coupon.StatusResponse;
                    dataRow[8] = coupon.AuthorizationNumber;
                    dataRow[9] = coupon.StatusDescription;
                    dataRow[10] = coupon.ReceiptNumber;

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return dataTable;
        }

        /// <summary>
        /// ExportReportCollectedCoupons
        /// Exporta reporte de cupones cobrados
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportReportCollectedCoupons(DataTable dataTable)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;


            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue(@Global.InsuredDocumentNumber);
            headerRow.CreateCell(1).SetCellValue(@Global.InsuredName);
            headerRow.CreateCell(2).SetCellValue(@Global.Net);
            headerRow.CreateCell(3).SetCellValue(@Global.Policy);
            headerRow.CreateCell(4).SetCellValue(@Global.ValueAddedTaxValue);
            headerRow.CreateCell(5).SetCellValue(@Global.ValueAddedTax);
            headerRow.CreateCell(6).SetCellValue(@Global.CollectionResponse);
            headerRow.CreateCell(7).SetCellValue(@Global.ResponseDescription);
            headerRow.CreateCell(8).SetCellValue(@Global.AuthorizationNumber);
            headerRow.CreateCell(9).SetCellValue(@Global.Status);
            headerRow.CreateCell(10).SetCellValue(@Global.TransactionNumber);

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 20 * 256);
            sheet.SetColumnWidth(9, 20 * 256);
            sheet.SetColumnWidth(10, 20 * 256);

            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(0).CellStyle = styleHeader;
            headerRow.GetCell(1).CellStyle = styleHeader;
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            headerRow.GetCell(8).CellStyle = styleHeader;
            headerRow.GetCell(9).CellStyle = styleHeader;
            headerRow.GetCell(10).CellStyle = styleHeader;

            int rowNumber = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styleDetail;
                }
            }

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        /// <summary>
        /// ExportFileNotification
        /// Exporta el archivo de notificación
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="lotNumber"></param>
        /// <param name="processTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ExportFileNotification(int bankNetworkId, string lotNumber, int processTypeId)
        {
            AutomaticDebitModels.AutomaticDebit automaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = bankNetworkId, RetriesNumber = 3 },
                Date = DateTime.Now,
                Description = lotNumber,
                Id = 1,
                RetriesNumber = 1//número de página
            };

            string fileName = "";
            int RecordsNumber = 0;
            int pageSize = 0;
            int pageNumber = 0;

            try
            {
                automaticDebit.AutomaticDebitStatus = new AutomaticDebitModels.AutomaticDebitStatus() { Id = 3000 };
                automaticDebit.UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

                List<AutomaticDebitModels.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);

                if (automaticDebits.Count > 0)
                {
                    if (automaticDebits.Select(sl => sl.Id).First() == -4)
                    {
                        fileName = automaticDebits.Select(sl => sl.Description).First();
                        fileName = "-4" + fileName.Trim();
                    }
                    else
                    {
                        RecordsNumber = automaticDebits[0].Coupons[0].Policy.PayerComponents[0].Coverage.CoverNum;
                        pageNumber = RecordsNumber / 3000;
                        if ((RecordsNumber % 3000) < 3000)
                        {
                            pageNumber++;
                        }
                        pageSize = automaticDebits.Count;

                        List<CouponsStatusDto> couponsStatus = new List<CouponsStatusDto>();

                        foreach (AutomaticDebitModels.AutomaticDebit automatic in automaticDebits)
                        {
                            couponsStatus.Add(GetCouponsStatusLists(automatic));
                        }

                        for (int i = 2; i <= pageNumber; i++)
                        {
                            automaticDebit.RetriesNumber = i;
                            automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebit);
                            pageSize = pageSize + automaticDebits.Count;

                            foreach (AutomaticDebitModels.AutomaticDebit automatic in automaticDebits)
                            {
                                couponsStatus.Add(GetCouponsStatusLists(automatic));
                            }

                            if (i == pageNumber && RecordsNumber > pageSize)
                            {
                                pageNumber++;
                            }
                        }

                        fileName = GenerateNotification(couponsStatus, bankNetworkId, processTypeId, lotNumber);
                    }
                }
            }
            catch (BusinessException ex)
            {
                fileName = ex.Message;
            }

            return Json(fileName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GenerateNotification
        /// Genera notifiación
        /// </summary>
        /// <param name="couponsStatusList"></param>
        /// <param name="bankNetworkId"></param>
        /// <param name="processTypeId"></param>
        /// <param name="lotNumber"></param>
        /// <returns>string</returns>
        public string GenerateNotification(List<CouponsStatusDto> couponsStatusList, int bankNetworkId,
                                           int processTypeId, string lotNumber)
        {
            string fileName = "";
            string sendId = "";
            string separator = "";
            string textFormat = "";
            int insert;
            decimal sum;
            int formatId = 0;

            var bankNetworks = DelegateService.automaticDebitService.GetBankNetworks().Where(x => x.Id == bankNetworkId).ToList();
            bool requiresNotification = bankNetworks[0].RequiresNotification;

            List<AutomaticDebitModels.AutomaticDebitFormat> automaticDebitFormats;

            sendId = couponsStatusList.Select(sl => sl.SendCode).First().ToString();

            if (processTypeId == 2)
            {
                automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId).Where(sl => sl.FormatUsingType == AutomaticDebitModels.FormatUsingTypes.SendingNotification).ToList();
                formatId = automaticDebitFormats[0].Format.Id;
            }
            if (processTypeId == 3)
            {
                automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId).Where(sl => sl.FormatUsingType == AutomaticDebitModels.FormatUsingTypes.Sending).ToList();
                formatId = automaticDebitFormats[0].Format.Id;
            }

            Report.Format format = new Report.Format()
            {
                Id = formatId,
                FileType = Report.FileTypes.Text
            };
            List<Report.FormatDetail> formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);

            if (formatDetails.Count > 0)
            {
                fileName = lotNumber + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                if (System.IO.File.Exists(Path.Combine(Server.MapPath(PathTransfer), fileName)))
                {
                    System.IO.File.Delete(Path.Combine(Server.MapPath(PathTransfer), fileName));
                }

                FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\transfers\\" + fileName, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;// limpia el buffer después de cada llamada 

                foreach (Report.FormatDetail formats in formatDetails)
                {
                    separator = formats.Separator;
                    char filled;
                    char separators;
                    string insuredCode = "";
                    StringBuilder stringBuilder = new StringBuilder("");

                    foreach (CouponsStatusDto couponsStatusDto in couponsStatusList)
                    {
                        insert = -1;

                        if (processTypeId == 2)
                        {
                            insert = 0;
                            if (insuredCode != couponsStatusDto.InsuredCode.ToString())
                            {
                                insert = -1;
                            }
                        }

                        if (insert == -1)
                        {
                            foreach (Report.FormatField detailformat in formats.Fields)
                            {
                                textFormat = "";
                                insuredCode = couponsStatusDto.InsuredCode.ToString();
                                try
                                {
                                    if (detailformat.Field == " " || detailformat.Field == "")
                                    {
                                        textFormat = GetFormatType(detailformat.Mask, detailformat.Value);
                                        if (detailformat.Filled != "")
                                        {
                                            filled = Convert.ToChar(detailformat.Filled);
                                            textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                        }
                                        stringBuilder.Append(textFormat);

                                        if (formats.Separator != "")
                                        {
                                            stringBuilder.Append(formats.Separator);
                                        }
                                    }
                                    else
                                    {
                                        if (detailformat.Field == "COUPON_NUMBER")
                                        {
                                            if (detailformat.Mask.IndexOf("CONTAR") == 0)
                                            {
                                                textFormat = GetFormatType(detailformat.Mask, Convert.ToString(couponsStatusList.Count));
                                            }
                                            else
                                            {
                                                textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.CouponNumber.ToString());
                                            }

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(couponsStatusDto.CouponNumber.ToString(), detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);

                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "COUPON_INDEX")
                                        {
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.CouponIndicator.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PAYMENT_ID")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.MethodTypeId.ToString());

                                            //relleno y alineacion
                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(couponsStatusDto.MethodTypeId.ToString(), detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "ID_CARD_NO")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.LotNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "INDIVIDUAL_ID")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.InsuredCode.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PAYER_IND_CD")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.PayerIndicatorCode.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PAYMENT_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.PaymentNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "VOUCHER_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.VoucherNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(couponsStatusDto.VoucherNumber.ToString(), detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "REJECTION_CODE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.RejectionCode.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "EXPIRATION_DATE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.ExpirationDate.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "SEND_CODE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.SendCode.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "SHIPMENT_CD")
                                        {
                                            //formato de campos
                                            if (detailformat.Mask.IndexOf("CONTAR") == 0)
                                            {
                                                textFormat = GetFormatType(detailformat.Mask, Convert.ToString(couponsStatusList.Count));
                                            }
                                            else
                                            {
                                                textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.LotNumber.ToString());
                                            }

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PRESENTATION_DATE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.PresentationDate.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "SEND_DATE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.SendDate.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "RECEIVING_BANK")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.ReceiverBankId.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "AMOUNT")
                                        {
                                            //formato de campos
                                            if (detailformat.Mask.IndexOf("SUMAR") == 0)
                                            {
                                                sum = couponsStatusList.Select(sl2 => sl2.PremiumTax).Sum();
                                                textFormat = GetFormatType(detailformat.Mask, sum.ToString());
                                            }
                                            else
                                            {
                                                textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.PremiumTax.ToString());
                                            }

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "APPLICATION_DATE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.AplicationDate.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }
                                        if (detailformat.Field == "ACCOUNT_NUMBER_SOURCE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.AccountNumberOrigin.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "SOURCE_NAME")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.NameOrigin.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(couponsStatusDto.NameOrigin.ToString(), detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "CARD_ACCOUNT_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.AccountNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "RECEIVING_NAME")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.ClientName.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "rfc_curp_rec")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.ReferencesCurp.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "ISSUING_SERVICE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.ServiceTransmitterReference.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PAYER_NAME")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.HolderName.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "AMOUNT_TAX")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.Tax.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "NUMBER_SOURCE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.OriginalReferenceNumber.ToString());

                                            if (detailformat.Mask == "GENERAL" && detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }

                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "LEGEND_ORIGIN")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.ReferenceLegend.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PRESENTATION_DATE_DETAIL")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.DetailPresentationDate.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "SUMMARY_SEQUENCE_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.SummarySequenceNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "SUMMARY_SHIPPMENT")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.SummaryLotNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }
                                        if (detailformat.Field == "SERIAL_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.SerialNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "PRIME_AMOUNT")
                                        {
                                            //formato de campos
                                            if (detailformat.Mask.IndexOf("SUMAR") == 0)
                                            {
                                                sum = couponsStatusList.Select(sl2 => sl2.Premium).Sum();
                                                textFormat = GetFormatType(detailformat.Mask, sum.ToString());
                                            }
                                            else
                                            {
                                                textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.Premium.ToString());
                                            }

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            if (detailformat.Mask.IndexOf("+/-") == 0)
                                            {
                                                textFormat = couponsStatusDto.Premium >= 0 ? "+" : "-";
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }
                                        if (detailformat.Field == "BASE_AMOUNT_TAX")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.TaxBase.ToString());

                                            if (detailformat.Mask == "GENERAL" && detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }

                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }
                                        if (detailformat.Field == "DOCUMENT_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.IndividualDocumentNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }
                                        if (detailformat.Field == "DOCUMENT_TYPE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.DocumenTypeId.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "ACCOUNT_TYPE")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.AccountTypeId.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "POLICY_NUMBER")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.PolicyNumber.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }

                                        if (detailformat.Field == "ACCOUNT_TYPE_RECEIVING_CD")
                                        {
                                            //formato de campos
                                            textFormat = GetFormatType(detailformat.Mask, couponsStatusDto.AccountTypeReceivingCode.ToString());

                                            if (detailformat.Filled != "")
                                            {
                                                filled = Convert.ToChar(detailformat.Filled);
                                                textFormat = Align(textFormat, detailformat.Length, detailformat.Align, filled);
                                            }
                                            stringBuilder.Append(textFormat);
                                            if (formats.Separator != "")
                                            {
                                                stringBuilder.Append(formats.Separator);
                                            }
                                        }
                                    }
                                    //hasta aqui
                                }
                                catch (Exception ex)
                                {
                                    throw new BusinessException(ex.Message);
                                }
                            }//end detailList
                            streamWriter.WriteLine(stringBuilder.ToString());
                        }
                        insuredCode = couponsStatusDto.InsuredCode.ToString();
                        stringBuilder = new StringBuilder("");

                        if (formats.FormatType == Report.FormatTypes.Head)
                        {
                            break;
                        }
                        if (formats.FormatType == Report.FormatTypes.Summary)
                        {
                            break;
                        }
                    }
                }
                streamWriter.Close();
            }

            return fileName;
        }

        /// <summary>
        /// Download
        /// Descargar
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult Download(string fileName)
        {
            byte[] path = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath(PathTransfer), fileName));
            return new FileDownloadResult(fileName, path);
        }

        #endregion

        #region BankNetworkFormat

        /// <summary>
        /// GetAutomaticDebitFormatsByBankNetworkId
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAutomaticDebitFormatsByBankNetworkId(int bankNetworkId)
        {

            List<AutomaticDebitModels.AutomaticDebitFormat> automaticDebitFormats = DelegateService.automaticDebitService.GetFormatsbyBankNetworkId(bankNetworkId);

            var formats = from items in automaticDebitFormats
                          select new
                          {
                              Id = items.Id,
                              BankNetworkId = items.BankNetwork.Id,
                              FileUsing = GetFormatUsingTypeDescription(Convert.ToInt32(items.FormatUsingType)),
                              FileUsingId = items.Format.FileType,
                              FormatId = items.Format.Id,
                              FormatName = items.Format.Description,
                          };

            return new UifTableResult(formats);
        }

        /// <summary>
        /// SaveAutomaticDebitFormat
        /// Graba / actualiza la asignación de formato de red para débito automático
        /// </summary>
        /// <param name = "automaticDebitFormatModel" ></ param >
        /// < returns > ActionResult </ returns >
        public ActionResult SaveAutomaticDebitFormat(AutomaticDebitFormatModel automaticDebitFormatModel)
        {
            // SERVICIO SaveAutomaticDebitFormat
            List<object> bankNetworkFormats = new List<object>();

            AutomaticDebitModels.FormatUsingTypes formatUsingTypes;

            if (automaticDebitFormatModel.FileUsing == "1")
            {
                formatUsingTypes = AutomaticDebitModels.FormatUsingTypes.Sending;
            }
            else if (automaticDebitFormatModel.FileUsing == "2")
            {
                formatUsingTypes = AutomaticDebitModels.FormatUsingTypes.Reception;
            }
            else if (automaticDebitFormatModel.FileUsing == "3")
            {
                formatUsingTypes = AutomaticDebitModels.FormatUsingTypes.SendingNotification;
            }
            else
            {
                formatUsingTypes = AutomaticDebitModels.FormatUsingTypes.ReceptionNotification;
            }

            AutomaticDebitModels.AutomaticDebitFormat automaticDebitFormat = new AutomaticDebitModels.AutomaticDebitFormat();

            automaticDebitFormat.Id = automaticDebitFormatModel.Id;
            automaticDebitFormat.BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = automaticDebitFormatModel.BankNetworkId };
            automaticDebitFormat.Format = new Report.Format()
            {
                FileType = Report.FileTypes.Excel,
                Id = automaticDebitFormatModel.FormatId
            };
            automaticDebitFormat.FormatUsingType = formatUsingTypes;

            if (automaticDebitFormatModel.OperationType.Equals("I"))
            {
                try
                {
                    DelegateService.automaticDebitService.SaveAutomaticDebitFormat(automaticDebitFormat);

                    bankNetworkFormats.Add(new
                    {
                        AutomaticDebitFormatCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankNetworkFormats.Add(new
                    {
                        AutomaticDebitFormatCode = -1,
                        MessageError = Global.MessageErrorSaveReconciliation + ex.Message
                    });
                }
            }
            if (automaticDebitFormatModel.OperationType.Equals("U"))
            {
                try
                {
                    DelegateService.automaticDebitService.UpdateAutomaticDebitFormat(automaticDebitFormat);

                    bankNetworkFormats.Add(new
                    {
                        AutomaticDebitFormatCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankNetworkFormats.Add(new
                    {
                        AutomaticDebitFormatCode = -1,
                        MessageError = Global.MessageErrorUpdateReconciliation + ex.Message
                    });
                }
            }

            return Json(bankNetworkFormats, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAutomaticDebitFormat
        /// Borra la asignación de formato a la red para débito automático
        /// </summary>
        /// <param name="automaticDebitFormatModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteAutomaticDebitFormat(AutomaticDebitFormatModel automaticDebitFormatModel)
        {
            List<object> bankNetworkFormats = new List<object>();
            AutomaticDebitModels.AutomaticDebitFormat automaticDebitFormat = new AutomaticDebitModels.AutomaticDebitFormat()
            {
                Id = automaticDebitFormatModel.Id,
                Format = new Report.Format()
                {
                    FileType = Report.FileTypes.Excel
                },
                FormatUsingType = AutomaticDebitModels.FormatUsingTypes.Reception
            };
            try
            {
                if (automaticDebitFormatModel.OperationType.Equals("D"))
                {
                    DelegateService.automaticDebitService.DeleteAutomaticDebitFormat(automaticDebitFormat);

                    bankNetworkFormats.Add(new
                    {
                        AutomaticDebitFormatCode = 0,
                    });
                }
            }
            catch (Exception ex)
            {
                bankNetworkFormats.Add(new
                {
                    AutomaticDebitFormatCode = -1,
                    MessageError = Global.MessageErrorUpdateReconciliation + ex.Message
                });
            }

            return Json(bankNetworkFormats, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods Private

        /// <summary>
        /// GetCouponsStatusLists
        /// Obtiene el listado de los cupones con sus respectivos estados
        /// </summary>
        /// <param name="automaticDebits"></param>
        /// <returns>CouponsStatusDTO</returns>
        private CouponsStatusDto GetCouponsStatusLists(AutomaticDebitModels.AutomaticDebit automaticDebits)
        {
            CouponsStatusDto couponsStatus = new CouponsStatusDto();
            couponsStatus.CouponNumber = automaticDebits.Coupons[0].Id;                                                                 //nro_cupon
            couponsStatus.CouponIndicator = automaticDebits.Coupons[0].CouponStatus.Id;                                                 //ind_cupon
            couponsStatus.MethodTypeId = automaticDebits.Coupons[0].Policy.Prefix.Id;                                                   //ind_conducto            
            couponsStatus.PolicyId = automaticDebits.Coupons[0].Policy.DefaultBeneficiaries[0].BeneficiaryTypeDescription.ToString();   //rfc_curp_ori
            couponsStatus.InsuredCode = automaticDebits.Coupons[0].Policy.Holder.IndividualId;                                          //cod_aseg
            couponsStatus.PayerIndicatorCode = automaticDebits.Coupons[0].Policy.DefaultBeneficiaries[0].IndividualId;                  //cod_ind_pagador
            couponsStatus.PaymentNumber = automaticDebits.Coupons[0].Policy.BillingGroup.Id;                                            //nro_cuota
            couponsStatus.VoucherNumber = automaticDebits.Coupons[0].Policy.Prefix.Description;                                         //nro_comprobante
            couponsStatus.RejectionCode = automaticDebits.Coupons[0].CouponStatus.Description;                                          //cod_rechazo
            couponsStatus.ExpirationDate = automaticDebits.Date;                                                                        //fec_venc
            couponsStatus.SendCode = Convert.ToDecimal(automaticDebits.Coupons[0].Policy.PayerComponents[0].Amount);                    //id_envio
            couponsStatus.LotNumber = automaticDebits.Coupons[0].Policy.PaymentPlan.Id;
            couponsStatus.PresentationDate = automaticDebits.Coupons[0].Policy.CurrentFrom;                                             //fec_presentacion
            couponsStatus.SendDate = automaticDebits.Coupons[0].Policy.CurrentTo;                                                       //fec_envio            
            couponsStatus.PremiumTax = automaticDebits.Coupons[0].Policy.PaymentPlan.Quotas[0].Amount;                                  //imp_premio_me
            couponsStatus.AplicationDate = Convert.ToDateTime(automaticDebits.Coupons[0].Policy.PayerComponents[0].Coverage.CurrentTo); //fec_aplicacion
            couponsStatus.AccountNumberOrigin = automaticDebits.Coupons[0].Policy.PayerComponents[0].Coverage.CoverStatusName;          //nro_cuenta_ori
            couponsStatus.NameOrigin = automaticDebits.BankNetwork.Description;                                                         //nombre_ori
            couponsStatus.CompanyAccount = automaticDebits.Coupons[0].Policy.DefaultBeneficiaries[0].BeneficiaryTypeDescription;        //rfc_curp_ori
            couponsStatus.AccountNumber = automaticDebits.Coupons[0].Policy.PaymentPlan.Description;                                    //nro_cuenta_rec
            couponsStatus.ClientName = automaticDebits.Coupons[0].Policy.PolicyType.Description;                                        //nombre_rec
            couponsStatus.ReferencesCurp = automaticDebits.Coupons[0].Policy.Branch.Description;                                        //rfc_curp_rec
            couponsStatus.ServiceTransmitterReference = automaticDebits.Coupons[0].Policy.Branch.SmallDescription;                      //ref_servicio_emisor
            couponsStatus.HolderName = automaticDebits.Coupons[0].Policy.DefaultBeneficiaries[0].Name;                                  //nom_titular_servicio
            couponsStatus.Tax = automaticDebits.Coupons[0].Policy.PayerComponents[0].BaseAmount;                                        //imp_iva_me
            couponsStatus.OriginalReferenceNumber = automaticDebits.Coupons[0].Policy.ExchangeRate.Currency.Id;                         //ref_num_ori
            couponsStatus.ReferenceLegend = automaticDebits.Coupons[0].Policy.Branch.SalePoints[0].Description;                         //ref_leyenda_ori
            couponsStatus.DetailPresentationDate = automaticDebits.Coupons[0].Policy.PayerComponents[0].Coverage.CurrentFrom;           //fec_presentacion_det
            couponsStatus.SummarySequenceNumber = automaticDebits.Coupons[0].Policy.PolicyType.Id;                                      //nro_secuencia_sumario
            couponsStatus.SummaryLotNumber = automaticDebits.Coupons[0].Policy.PayerComponents[0].Id;                                   //nro_lote_sumario
            couponsStatus.SerialNumber = automaticDebits.Coupons[0].Policy.ExchangeRate.Currency.Description;                           //nro_serie	varchar
            couponsStatus.Premium = automaticDebits.Coupons[0].Policy.PayerComponents[0].Coverage.AccumulatedDeductAmount;              //imp_premio_bco_me
            couponsStatus.TaxBase = automaticDebits.Coupons[0].Policy.PaymentPlan.Quotas[0].Percentage;                                 //imp_base_iva 
            couponsStatus.IndividualDocumentNumber = automaticDebits.Coupons[0].Policy.Holder.Name;                                     //nro_doc
            couponsStatus.DocumenTypeId = automaticDebits.Description;                                                                  //tipo_doc
            couponsStatus.AccountTypeId = automaticDebits.Coupons[0].Policy.Branch.Id;                                                  //tipo_cta	
            couponsStatus.PolicyNumber = automaticDebits.Coupons[0].CouponStatus.SmallDescription;                                      //nro_pol
            couponsStatus.AccountTypeReceivingCode = automaticDebits.Coupons[0].Policy.PayerComponents[0].Coverage.Id;                  //ACCOUNT_TYPE_RECEIVING_CD tipo_cuenta_rec

            return couponsStatus;
        }

        /// <summary>
        /// Right
        /// Alineación a la derecha
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="filled"></param>
        /// <returns>string</returns>
        private static string Right(string value, int length, char filled)
        {
            value = value.Length <= length ? value.PadRight(length, filled) : value.Substring(0, length);
            return value;
        }

        /// <summary>
        /// Left
        /// Alineación a la izquierda
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="separator"></param>
        /// <returns>string</returns>
        private static string Left(string value, int length, char separator)
        {
            value = value.Length <= length ? value.PadLeft(length, separator) : value.Substring(value.Length - length);

            return value;
        }

        /// <summary>
        /// Align
        /// Alineación a la derecha
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="align"></param>
        /// <param name="filled"></param>
        /// <returns>string</returns>
        private string Align(string value, int length, string align, char filled)
        {
            if (String.IsNullOrEmpty(value))
                return string.Empty;
            {
                if (align == "D")
                {
                    value = value.TrimStart();
                    value = Right(value, length, filled);
                }
                else
                {
                    value = value.TrimEnd();
                    value = Left(value, length, filled);
                }
            }

            return value;
        }

        /// <summary>
        /// GetFormatType
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatText"></param>
        /// <returns>string</returns>
        private string GetFormatType(string format, string formatText)
        {
            string[] partFormat;
            decimal formatValue;
            DateTime formatDate;

            if (format == "+/-" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue >= 0 ? "+" : "-";
            }

            if (format == "+/-IMPORTE.00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue >= 0 ? "+" + System.Math.Round(formatValue, 2).ToString() : "-" + System.Math.Round(formatValue, 2).ToString();
            }

            if (format == "+/-IMPORTE00" && formatText != "" && formatText != " ") //IMPORTE CON SIGNO 2 DECIMALES SIN PUNTO
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue >= 0 ? "+" + System.Math.Round(formatValue, 2).ToString() : "-" + System.Math.Round(formatValue, 2).ToString();
                formatText = formatText.Replace(",", "");
            }

            if (format == "AAAAMM") //FECHA TIPO AAAAMM
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("yyyyMM");
            }

            if (format == "AAAAMM00")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("yyyyMM00");
            }

            if (format == "AAAAMMDD")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("yyyyMMdd");
            }


            if (format == "AAMM")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("yyMM");
            }

            if (format == "AAMMDD")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("yyMMdd");
            }

            if (format == "CONTAR::IMPORTE" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue.ToString();
            }

            if (format == "D/C" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue >= 0 ? "D" : "C";
            }

            if (format == "DDMMAAAA")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("ddMMyyyy");
            }

            if (format == "IMPORTE")
            {
                formatValue = Convert.ToDecimal(formatText);
                string[] parts = formatValue.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                formatText = parts[0];
            }

            if (format == "IMPORTE.00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue, 2).ToString();
                formatText = formatText.Replace(",", ".");
            }

            if (format == "IMPORTE::IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue).ToString();
                if (formatText.IndexOf(",") > -1)
                {
                    partFormat = formatText.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                    formatText = partFormat[0] + "00";
                }
                else
                {
                    formatText = formatText + "00";
                }
            }

            if (format == "IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue, 2).ToString();
                formatText = formatText.Replace(",", "");
            }

            if (format == "SUMAR::IMPORTE" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue.ToString();
                partFormat = formatText.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                formatText = partFormat[0];
            }

            if (format == "SUMAR::IMPORTE::IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue).ToString();
                if (formatText.IndexOf(",") > -1)
                {
                    partFormat = formatText.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                    formatText = partFormat[0] + "00";
                }
                else
                {
                    formatText = formatText + "00";
                }
            }

            if (format == "SUMAR::IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue, 2).ToString();
                formatText = formatText.Replace(".", "");
            }

            if (format == "MMDDAA")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatDate = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    formatDate = Convert.ToDateTime(formatText);
                }
                formatText = formatDate.ToString("MMddyy");
            }
            if (formatText == null)
            {
                formatText = "";
            }

            return formatText;
        }

        /// <summary>
        /// ConvertDebitsToDataTable
        /// Convierte los Debitos en Tabla
        /// </summary>
        /// <param name="couponStatusList"></param>
        /// <returns>DataTable</returns>
        private DataTable ConvertDebitsToDataTable(List<CouponsStatusDto> couponStatusList)
        {
            DataTable dataTable = new DataTable();
            CouponsStatusDto collectedCoupon = new CouponsStatusDto();

            //Get the type of source object and create a new instance of that type

            var headerRows = new List<string>(12);

            headerRows.Add(@Global.Branch);              //Sucursal          
            headerRows.Add(@Global.PrefixCode);          //cod.ramo   
            headerRows.Add(@Global.Prefix);              //Ramo   
            headerRows.Add(@Global.Policy);              //Poliza       
            headerRows.Add(@Global.Suffix);              //Sufijo           
            headerRows.Add(@Global.EndorsementNumber);   //N° Endoso           
            headerRows.Add(@Global.PolicyShareId);       //Id. SISE           
            headerRows.Add(@Global.Temporary);           //Temporario              
            headerRows.Add(@Global.LoadState);           //Cod.Estado          
            headerRows.Add(@Global.Status);              //Cod Rechazo     
            headerRows.Add(@Global.RejectionReason);     //Motivo Rechazo         
            headerRows.Add(@Global.TotalPremium);        //Total   

            for (int j = 0; j < headerRows.Count; j++)
            {
                dataTable.Columns.Add(headerRows[j]);
            }

            try
            {
                foreach (CouponsStatusDto item in couponStatusList)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = item.InsuredCode;                 //Sucursal 
                    dataRow[1] = item.SummarySequenceNumber;       //cod.ramo 
                    dataRow[2] = item.NameOrigin;                  //Ramo 
                    dataRow[3] = item.PolicyNumber;                //Poliza 
                    dataRow[4] = item.CouponIndicator;             //Sufijo     
                    dataRow[5] = item.LotNumber;                   //N° Endoso 
                    dataRow[6] = item.PolicyId;                    //Id. SISE 
                    dataRow[7] = item.RejectionCode;               //Temporario     
                    dataRow[8] = item.MethodTypeId;                //Cod.Estado         
                    dataRow[9] = item.AccountNumber;               //Cod Rechazo  OriginalReferenceNumber           
                    dataRow[10] = item.AccountNumberOrigin;        //Motivo Rechazo             
                    dataRow[11] = item.TaxBase;                    //Total  

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return dataTable;
        }

        /// <summary>
        /// ExportDebits
        /// Exporta Debitos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="lotNumber"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportDebits(DataTable dataTable, string lotNumber)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            ICellStyle styleLine = workbook.CreateCellStyle();
            styleLine.SetFont(fontDetail);
            styleLine.BottomBorderColor = HSSFColor.Black.Index;
            styleLine.BorderBottom = BorderStyle.Thin;

            ICellStyle styleDoubleLine = workbook.CreateCellStyle();
            styleDoubleLine.SetFont(fontDetail);
            styleDoubleLine.BottomBorderColor = HSSFColor.Black.Index;
            styleDoubleLine.BorderBottom = BorderStyle.Double;

            ICellStyle styleLetter = workbook.CreateCellStyle();
            styleLetter.SetFont(fontDetail);

            var fontTitle = workbook.CreateFont();
            fontTitle.FontName = "Tahoma";
            fontTitle.FontHeightInPoints = 18;
            fontTitle.Boldweight = 13;

            ICellStyle styleTitle = workbook.CreateCellStyle();
            styleTitle.SetFont(fontTitle);
            styleTitle.FillForegroundColor = HSSFColor.White.Index;
            styleTitle.FillPattern = FillPattern.SolidForeground;

            var titleRow = sheet.CreateRow(0);
            titleRow.CreateCell(5).SetCellValue(@Global.BatchSummary + " " + lotNumber);
            titleRow.GetCell(5).CellStyle = styleTitle;


            var headerRow = sheet.CreateRow(2);

            headerRow.CreateCell(0).SetCellValue(@Global.Branch);
            headerRow.CreateCell(1).SetCellValue(@Global.PrefixCode);
            headerRow.CreateCell(2).SetCellValue(@Global.Prefix);
            headerRow.CreateCell(3).SetCellValue(@Global.Policy);
            headerRow.CreateCell(4).SetCellValue(@Global.Suffix);
            headerRow.CreateCell(5).SetCellValue(@Global.EndorsementNumber);
            headerRow.CreateCell(6).SetCellValue(@Global.PolicyShareId);
            headerRow.CreateCell(7).SetCellValue(@Global.Temporary);
            headerRow.CreateCell(8).SetCellValue(@Global.LoadState);
            headerRow.CreateCell(9).SetCellValue(@Global.Status);
            headerRow.CreateCell(10).SetCellValue(@Global.RejectionReason);
            headerRow.CreateCell(11).SetCellValue(@Global.TotalPremium);

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 20 * 256);
            sheet.SetColumnWidth(9, 20 * 256);
            sheet.SetColumnWidth(10, 20 * 256);
            sheet.SetColumnWidth(11, 20 * 256);

            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(0).CellStyle = styleHeader;
            headerRow.GetCell(1).CellStyle = styleHeader;
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            headerRow.GetCell(8).CellStyle = styleHeader;
            headerRow.GetCell(9).CellStyle = styleHeader;
            headerRow.GetCell(10).CellStyle = styleHeader;
            headerRow.GetCell(11).CellStyle = styleHeader;

            int rowNumber = 3;

            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styleDetail;
                }
            }

            // Create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        /// <summary>
        /// SetFormatType
        /// Envia el tipo de formato
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatText"></param>
        /// <returns>string</returns>
        private string SetFormatType(string format, string formatText)
        {
            string[] partFormat;
            decimal formatValue;
            int length = formatText.Length;

            if (format == "+/-IMPORTE00" && formatText != "" && formatText != " ") //IMPORTE CON SIGNO 2 DECIMALES SIN PUNTO
            {
                formatText = formatText.Substring(0, length - 2) + "." + formatText.Substring(length - 2, 2);
            }

            if (format == "AAAAMM")//FECHA TIPO AAAAMM
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = "01/" + formatText.Substring(4, 2) + "/" + formatText.Substring(0, 4);
                }
            }

            if (format == "AAAAMM00")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = "01/" + formatText.Substring(4, 2) + "/" + formatText.Substring(0, 4);
                }
            }

            if (format == "AAAAMMDD")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = formatText.Substring(6, 2) + "/" + formatText.Substring(4, 2) + "/" + formatText.Substring(0, 4);
                }
            }


            if (format == "AAMM")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = "01/" + formatText.Substring(2, 2) + "/20" + formatText.Substring(0, 2);
                }
            }

            if (format == "AAMMDD")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = formatText.Substring(4, 2) + "/" + formatText.Substring(2, 2) + "/20" + formatText.Substring(0, 2);
                }
            }


            if (format == "DDMMAAAA")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = formatText.Substring(0, 2) + "/" + formatText.Substring(2, 2) + "/" + formatText.Substring(4, 4);
                }
            }

            if (format == "IMPORTE")
            {
                formatText = formatText.Substring(0, length);
            }

            if (format == "IMPORTE.00" && formatText != "" && formatText != " ")
            {
                formatText = formatText.Substring(0, length - 2) + formatText.Substring(length - 2, 2);
                formatText = formatText.Replace(",", ".");
            }

            if (format == "IMPORTE::IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatText = formatText.Substring(0, length - 2) + "." + formatText.Substring(length - 2, 2);
            }

            if (format == "IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatText = formatText.Substring(0, length - 2) + "." + formatText.Substring(length - 2, 2);
            }

            if (format == "SUMAR::IMPORTE" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = formatValue.ToString();
                partFormat = formatText.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                formatText = partFormat[0];
            }

            if (format == "SUMAR::IMPORTE::IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue).ToString();
                if (formatText.IndexOf(",") > -1)
                {
                    partFormat = formatText.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                    formatText = partFormat[0] + "00";
                }
                else
                {
                    formatText = formatText + "00";
                }
            }

            if (format == "SUMAR::IMPORTE00" && formatText != "" && formatText != " ")
            {
                formatValue = Convert.ToDecimal(formatText);
                formatText = System.Math.Round(formatValue, 2).ToString();
                formatText = formatText.Replace(".", "");
            }

            if (format == "MMDDAA")
            {
                if (formatText == "" || formatText == " ")
                {
                    formatText = "01/01/1900";
                }
                else
                {
                    formatText = formatText.Substring(2, 2) + "/" + formatText.Substring(0, 2) + "/20" + formatText.Substring(4, 2);
                }
            }

            return formatText;
        }

        /// <summary>
        /// GetFormatUsingTypeDescription
        /// Obtiene la descripción del Uso del Formato
        /// </summary>
        /// <param name="formatUsingTypeId"></param>
        /// <returns>string</returns>
        private string GetFormatUsingTypeDescription(int formatUsingTypeId)
        {
            string formatUsingTypeDescription;

            if (formatUsingTypeId == 1)
            {
                formatUsingTypeDescription = "ENVIO";
            }
            else if (formatUsingTypeId == 2)
            {
                formatUsingTypeDescription = "RECEPCION";
            }
            else if (formatUsingTypeId == 3)
            {
                formatUsingTypeDescription = "ENVIO PRENOTIFICACION";
            }
            else
            {
                formatUsingTypeDescription = "RESPUESTA PRENOTIFICACION";
            }

            return formatUsingTypeDescription;
        }

        /// <summary>
        /// Crea un reportes vacío que indica que no se encontraron registros
        /// </summary>
        /// <returns>Memory streams</returns>
        private MemoryStream CreateNotFoundRowsReport()
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(@Global.NoDataFound);

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        #endregion
    }
}