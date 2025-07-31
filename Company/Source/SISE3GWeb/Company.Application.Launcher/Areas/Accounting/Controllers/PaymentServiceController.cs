using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Excel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
//Sistran Company
using DTOs = Sistran.Core.Application.AccountingServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [NoDirectAccess]
    public class PaymentServiceController : Controller
    {
        #region Constants
        private const int PageSize = 1000;
        #endregion

        #region Instance Variables
        readonly CommonController _commonController = new CommonController();
        #endregion

        #region View
        /// <summary>
        /// MainPaymentService
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPaymentService()
        {
            try
            {     
                List<DTOs.Payments.PaymentMethodDTO> paymentMethods = DelegateService.accountingParameterService.GetPaymentMethods().FindAll(s => s.Id.Equals(4));
                ViewBag.PaymentMethod = paymentMethods;
                ViewBag.AgentInsured = 0;

                List<Currency> currencies = DelegateService.commonService.GetCurrencies();
                ViewBag.Currency = currencies;

                List<DTOs.Payments.CreditCardTypeDTO> creditCardTypes = DelegateService.accountingParameterService.GetCreditCardTypes();
                ViewBag.CreditCardType = creditCardTypes;

                List<object> validationMonth = new List<object>();

                for (int i = 1; i <= 12; i++)
                {
                    validationMonth.Add(new { Id = i, Description = i });
                }
                ViewBag.ValidationMonth = validationMonth;
                ViewBag.localCurrencyId = 0;

                List<Bank> banks = new List<Bank>();
                ViewBag.Bank = banks;

                // Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                var accountBanks = new TransactionController().GetAccountBank();
                ViewBag.AccountBank = accountBanks;
                ViewBag.AccountBankBankDepositVoucher = accountBanks;

                var personBankAccounts = _commonController.GetPersonBankAccounts();
                ViewBag.AccountNumberBank = personBankAccounts;
                ViewBag.AccountNumberBankDepositVoucher = personBankAccounts;

                List<object> receivingBanks = GetAccountBank();
                ViewBag.ReceivingBank = receivingBanks;

                List<Generic> fileTypes = new List<Generic>();

                fileTypes.Add(new Generic() { Id = 1, Description = "CSV" });
                fileTypes.Add(new Generic() { Id = 2, Description = "EXCEL" });
                fileTypes.Add(new Generic() { Id = 3, Description = "XML" });
                ViewBag.TypeFile = fileTypes;

                List<FormDocument> formDocuments = new List<FormDocument>();
                ViewBag.FilesList = formDocuments;

                if (TempData["FilesList"] == null)
                {
                    TempData["FilesList"] = formDocuments;
                    ViewBag.FilesList = formDocuments;
                }
                else
                {                
                    formDocuments = (List<FormDocument>)(TempData["FilesList"]);

                    TempData["FilesList"] = formDocuments;
                    ViewBag.FilesList = formDocuments;
                }

                List<object> creditCardExpirationMonths = GetCreditCardExpirationMonth();

                ViewBag.CreditCardExpirationMonthList = creditCardExpirationMonths;

                ViewBag.TitleRel = @Global.Hi + " " + User.Identity.Name.ToUpper() + ", " + @Global.WhatYouNeedHelp;

                // Se utilizan los parámetros definidos en el web.config en lugar de los definidos en el archivo de recursos.
                ViewBag.ParamPaymentMethodPostdatedCheck = ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"];
                ViewBag.ParamPaymentMethodCurrentCheck = ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"];
                ViewBag.ParamPaymentMethodCash = ConfigurationManager.AppSettings["ParamPaymentMethodCash"];
                ViewBag.ParamPaymentMethodCreditableCreditCard = ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"];
                ViewBag.ParamPaymentMethodUncreditableCreditCard = ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"];
                ViewBag.ParamPaymentMethodDebit = ConfigurationManager.AppSettings["ParamPaymentMethodDebit"];
                ViewBag.ParamPaymentMethodDirectConection = ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"];
                ViewBag.ParamPaymentMethodTransfer = ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"];
                ViewBag.ParamPaymentMethodDepositVoucher = ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"];
                ViewBag.ParamPaymentMethodRetentionReceipt = ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"];
                ViewBag.ParamPaymentMethodDataphone = ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"];
                ViewBag.ParamPaymentMethodElectronicPayment = ConfigurationManager.AppSettings["ParamPaymentMethodElectronicPayment"];
                ViewBag.ParamPaymentMethodPaymentArea = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentArea"];
                ViewBag.ParamPaymentMethodPaymentCard = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentCard"];

                string branchPaymentService = _commonController.GetBranchDescriptionByBranchId(Convert.ToInt32(ConfigurationManager.AppSettings["BranchPaymentService"]));

                if (String.IsNullOrWhiteSpace(branchPaymentService))
                {
                    branchPaymentService = Global.BranchNotExistPaymentService;
                }

                // Botón de Pagos
                ViewBag.BranchPaymentService = Convert.ToInt32(ConfigurationManager.AppSettings["BranchPaymentService"]);
                ViewBag.BranchDescriptionPaymentService = branchPaymentService;
                ViewBag.TransferReceivingBankPaymentService = ConfigurationManager.AppSettings["TransferReceivingBankPaymentService"];
                ViewBag.AccountNumberReceivingBankPaymentService = ConfigurationManager.AppSettings["AccountNumberReceivingBankPaymentService"];
                ViewBag.PaymentConceptPaymentService = ConfigurationManager.AppSettings["PaymentConceptPaymentService"];
                ViewBag.IssuingBankPaymentService = ConfigurationManager.AppSettings["IssuingBankPaymentService"];
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// LoadMainPaymentService
        /// </summary>
        /// <param name="id"> </param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadMainPaymentService(string id)
        {
            TempData["id"] = id;
            return RedirectToAction("MainPaymentService", "PaymentService");
        }
        #endregion

        #region PersonalData

        /// <summary>
        /// Obtiene los agentes dado el número de documento
        /// BCARDENAS
        /// </summary>
        /// <param name="number"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAgentByDocumentNumber(string number)
        {
            return Json(_commonController.GetAgentByDocumentNumber(number), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene los agentes dado el nombre
        /// BCARDENAS
        /// </summary>
        /// <param name="name"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAgentByName(string name)
        {
            return Json(_commonController.GetAgentByName(name), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetInsured
        /// Obtiene listado de personas aseguradas (AUTOCOMPLETE)
        /// BCARDENAS
        /// </summary>
        /// <param name="dataSearch"></param>
        /// <param name="typeSearch"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsured(string dataSearch, int typeSearch)
        {
            try
            {
                List<object> insurers = new List<object>();
                List<IndividualDTO> persons;

                if (typeSearch == 1)
                {
                    persons = DelegateService.tempCommonService.GetInsuredByDocumentNumber(dataSearch);
                }
                else
                {
                    persons = DelegateService.tempCommonService.GetInsuredByName(dataSearch);
                }

                foreach (var person in persons)
                {
                    insurers.Add(new
                    {
                        Name = person.Name.Trim(),
                        Id = person.IndividualId,
                        DocumentNumber = person.DocumentNumber
                    });
                }
                
                return Json(insurers, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region UploadFile

        /// <summary>
        /// UploadFileInServer
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UploadFileInServer(string fileName)
        {
            HttpPostedFileBase file = Request.Files["file_upfile"];

            if (file != null && file.ContentLength > 0)
            {
                string fname = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", fname)));
            }

            foreach (HttpPostedFile postedFile in Request.Files)
            {
                if (postedFile.ContentLength > 0)
                {
                    string finame = Path.GetFileName(postedFile.FileName);
                    if (postedFile.ContentType.Contains("image/")) //as pointed out in comment
                    {
                        string fimagename = Path.GetFileName(postedFile.FileName);
                    }
                }
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PremiumReceivableSearchPolicy

        ///<summary>
        /// PremiumReceivableSearchPolicy
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="payerId"></param>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="salesTicket"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementDocumentNumber"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pathFile"></param>
        /// <param name="pageNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult PremiumReceivableSearchPolicy(string insuredId, string payerId, string agentId, string groupId,
                                                        string policyDocumentNumber, string salesTicket,
                                                        string branchId, string prefixId, string endorsementDocumentNumber,
                                                        string dateFrom, string dateTo, string pathFile, int pageNumber)
        {
            List<object> premiumReceivablePolicies = new List<object>();
            List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();
            List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();
            if (pathFile != "" && pathFile != null)
            {
                try
                {
                    string[] data = pathFile.Split(new char[] { '.' });

                    List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivables;
                    
                    if (data[1] == "csv")
                    {
                        premiumReceivables = (List<DTOs.Search.PremiumReceivableSearchPolicyDTO>)TempData["dtoCsv"];
                    }
                    if (data[1] == "xml")
                    {
                        premiumReceivables = (List<DTOs.Search.PremiumReceivableSearchPolicyDTO>)TempData["dtoXml"];
                    }
                    if ((data[1] == "xlsx") || (data[1] == "xls"))
                    {
                        premiumReceivables = (List<DTOs.Search.PremiumReceivableSearchPolicyDTO>)TempData["dtoExel"];
                        TempData["dtoExel"] = premiumReceivables;
                    }
                    premiumReceivableSearchPolicyDTOs = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();
                }
                catch (BusinessException exception)
                {
                    throw new BusinessException(exception.Message);
                }
            }
            else
            {
                premiumReceivableSearchPolicyDTOs = DelegateService.accountingApplicationService.PremiumReceivableSearchPolicy(insuredId, payerId, agentId, groupId,"",
                                                       policyDocumentNumber, salesTicket, branchId,
                                                       prefixId, endorsementDocumentNumber, dateFrom, dateTo,"", PageSize, pageNumber);
                //NO BAJA LOGICA ORDENAMIENTO ESPECIFICO PARA ESTE REPORTE . ESTE METODO ES COMPARTDO - Ordenamiento por Cuota/Endoso/Póliza
                premiumReceivableSearchPolicies = (from order in premiumReceivableSearchPolicyDTOs
                                                   orderby order.PaymentNumber, order.EndorsementId, order.PolicyId
                                                   select order).ToList();
            }

            double totalPages = System.Math.Ceiling(Convert.ToDouble(Convert.ToDouble(premiumReceivableSearchPolicyDTOs.Count) / PageSize));
            foreach (DTOs.Search.PremiumReceivableSearchPolicyDTO premiumReceivableSearchPolicy in premiumReceivableSearchPolicies)
            {
                int payerIndividualId = 0;
                int insuredIndividualId = 0;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;
                insuredIndividualId = premiumReceivableSearchPolicy.InsuredId;
                payerIndividualId = premiumReceivableSearchPolicy.PayerId;

                premiumReceivablePolicies.Add(new
                {
                    PolicyId = premiumReceivableSearchPolicy.PolicyId,
                    EndorsementId = premiumReceivableSearchPolicy.EndorsementId,
                    BranchPrefixPolicyEndorsement = premiumReceivableSearchPolicy.BranchPrefixPolicyEndorsement,
                    PolicyDocumentNumber = premiumReceivableSearchPolicy.PolicyDocumentNumber,
                    EndorsementDocumentNumber = premiumReceivableSearchPolicy.EndorsementDocumentNumber,
                    PaymentNumber = premiumReceivableSearchPolicy.PaymentNumber,
                    CurrencyId = premiumReceivableSearchPolicy.CurrencyId,
                    CurrencyDescription = premiumReceivableSearchPolicy.CurrencyDescription,
                    PaymentAmount = premiumReceivableSearchPolicy.PaymentAmount < 0 ? premiumReceivableSearchPolicy.PaymentAmount.ToString("C") : String.Format(new CultureInfo("en-US"), "{0:C}", premiumReceivableSearchPolicy.PaymentAmount),
                    InsuredId = premiumReceivableSearchPolicy.InsuredId,
                    InsuredName = premiumReceivableSearchPolicy.InsuredName,
                    PolicyAgentName = premiumReceivableSearchPolicy.PolicyAgentName,
                    PayerId = premiumReceivableSearchPolicy.PayerId,
                    PayerIndividualId = payerIndividualId,
                    InsuredIndividualId = insuredIndividualId,
                    ExchangeRate = premiumReceivableSearchPolicy.ExchangeRate,
                    PaymentExpirationDate = premiumReceivableSearchPolicy.PaymentExpirationDate
                });
            }

            return Json(new { aaData = premiumReceivablePolicies, pages = totalPages }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GeneratePremiumReceivablePolicy
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="agentId"></param>
        /// <param name="payerId"></param>
        /// <param name="pathFile"></param>
        /// <param name="groupId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="salesTicket"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GeneratePremiumReceivablePolicy(string insuredId, string agentId, string payerId = "",
                                                            string pathFile = "", string groupId = "", string policyDocumentNumber = "",
                                                            string salesTicket = "", string branchId = "", string prefixId = "",
                                                            string endorsementId = "", string dateFrom = "", string dateTo = "")

        {
            int pageSize = 1000;
            int pageIndex = 0;
            List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPoliciesOrders;
            List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivablePolicies = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();
            if (pathFile != "" && pathFile != null)
            {
                try
                {
                    string[] data = pathFile.Split(new char[] { '.' });

                    if ((data[1] == "xlsx") || (data[1] == "xls"))
                    {
                        List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivables;
                        premiumReceivables = (List<DTOs.Search.PremiumReceivableSearchPolicyDTO>)TempData["dtoExel"];
                        var index = premiumReceivables.Count;
                        premiumReceivablePolicies = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();
                    }
                }
                catch (BusinessException exception)
                {
                    throw new BusinessException(exception.Message);
                }
            }
            else
            {
                premiumReceivablePolicies = DelegateService.accountingApplicationService.PremiumReceivableSearchPolicy(
                                insuredId, payerId, agentId, groupId,"", policyDocumentNumber,
                                salesTicket, branchId,prefixId, endorsementId,
                                dateFrom, dateTo,"", pageSize, pageIndex);

            }
            // Ordenamiento por Cuota/Endoso/Póliza
            premiumReceivableSearchPoliciesOrders = (from order in premiumReceivablePolicies
                                                     orderby order.PaymentNumber,
                           order.EndorsementId, order.PolicyId
                                                     select order).ToList();

            MemoryStream memoryStream = GetCurrentAuxiliaryData(ConvertPaginatedResponseToDataTable(premiumReceivableSearchPoliciesOrders));
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "ReportExcel.xls");
        }

        /// <summary>
        /// ConvertPaginatedResponseToDataTable
        /// <param name="list"></param>
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ConvertPaginatedResponseToDataTable(List<DTOs.Search.PremiumReceivableSearchPolicyDTO> list)
        {
            DataTable dataDecoded = new DataTable();
            DTOs.Search.PremiumReceivableSearchPolicyDTO listPremiumReceivableSearchPolicyDto = new DTOs.Search.PremiumReceivableSearchPolicyDTO();

            //Get the type of source object and create a new instance of that type
            Type typeSource = listPremiumReceivableSearchPolicyDto.GetType();
            
            var headerRow = new List<string>(22);
            headerRow.Add("Sucursal ID");
            headerRow.Add("Sucursal");
            headerRow.Add("Ramo");
            headerRow.Add("Ramo Id");
            headerRow.Add("Pòliza");
            headerRow.Add("Nro. Póliza");
            headerRow.Add("Endoso ID");
            headerRow.Add("Nro. Endoso");
            headerRow.Add("Moneda Id");
            headerRow.Add("Moneda");
            headerRow.Add("Cuota");
            headerRow.Add("Importe");
            headerRow.Add("Fecha Expiration");
            headerRow.Add("Id Individual Asegurado");
            headerRow.Add("Nombre Asegurado");
            headerRow.Add("Id Individual Agente");
            headerRow.Add("Nombre Agente");
            headerRow.Add("Pje. Comisión");
            headerRow.Add("Comisión");
            headerRow.Add("Payer Id");
            headerRow.Add("Payer Individual Id");

            for (int j = 0; j < headerRow.Count; j++)
            {
                dataDecoded.Columns.Add(headerRow[j]);
            }

            try
            {
                foreach (DTOs.Search.PremiumReceivableSearchPolicyDTO row in list)
                {
                    DataRow dr = dataDecoded.NewRow();

                    dr[0] = row.BranchId;
                    dr[1] = row.BranchDescription;
                    dr[2] = row.PrefixId;
                    dr[3] = row.PrefixDescription;
                    dr[4] = row.PolicyId;
                    dr[5] = row.PolicyDocumentNumber;
                    dr[6] = row.EndorsementId;
                    dr[7] = row.EndorsementDocumentNumber;
                    dr[8] = row.CurrencyId;
                    dr[9] = row.CurrencyDescription;
                    dr[10] = row.PaymentNumber;
                    dr[11] = row.PaymentAmount == 0 ? "0" : row.PaymentAmount.ToString("n");
                    dr[12] = row.PaymentExpirationDate.ToString("dd/MM/yyyy");
                    dr[13] = row.InsuredId;
                    dr[14] = row.InsuredName;
                    dr[15] = row.PolicyAgentId;
                    dr[16] = row.PolicyAgentName;
                    dr[18] = row.AgentParticipationPercentage == 0 ? "0" : row.AgentParticipationPercentage.ToString("n");
                    dr[17] = row.StandarCommissionPercentage == 0 ? "0" : (row.StandarCommissionPercentage / 100).ToString("n");
                    dr[19] = row.PayerId;
                    dr[20] = row.PayerId;

                    dataDecoded.Rows.Add(dr);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return dataDecoded;
        }

        /// <summary>
        /// GetCreditCardExpirationMonth
        /// Obtiene el mes de expiración de una tarjeta de crédito
        /// </summary>
        /// <returns>List<object/></returns>
        public List<object> GetCreditCardExpirationMonth()
        {
            try
            {
                string[] expirationMonth = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
                List<object> creditCardExpirationMonths = new List<object>();

                for (int i = 0; i < expirationMonth.Length; i++)
                {
                    creditCardExpirationMonths.Add(new
                    {
                        Id = i + 1,
                        Description = expirationMonth[i]
                    });
                }

                return creditCardExpirationMonths;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Report

        /// <summary>
        /// LoadPrintInvoiceReport
        /// LFR
        /// </summary>
        /// <param name="invoiceTotal"></param>
        /// <param name="invoiceClientName"></param>
        /// <param name="invoiceClientRuc"></param>
        public void LoadPrintInvoiceReport(decimal invoiceTotal, string invoiceClientName, string invoiceClientRuc)
        {
            try
            {
                string numberBarCode = "038000356216";
                List<InvoiceModel> InvoiceModels = new List<InvoiceModel>();
                InvoiceModel invoiceModel = new InvoiceModel();

                invoiceModel.Id = 1;
                invoiceModel.InvoiceBankName = "PRODUBANCO";
                invoiceModel.InvoiceClientAddress = "AV DIEGO DE ALMAGRO N32-437 Y SHYRIS";
                invoiceModel.InvoiceClientName = invoiceClientName;
                invoiceModel.InvoiceClientPhone = "2237170";
                invoiceModel.InvoiceClientRuc = invoiceClientRuc;
                invoiceModel.InvoiceDate = DateTime.Now;
                invoiceModel.InvoiceIva = 0;
                invoiceModel.InvoiceNumber = "FACTURA NRO. 0000001";
                invoiceModel.InvoiceSubTotal = invoiceTotal;
                invoiceModel.InvoiceTotal = invoiceTotal;

                var path = Path.Combine(Server.MapPath("~/Content/images"), "barcode_a.jpg");
                invoiceModel.InvoiceBarcode = numberBarCode + CheckDigit(numberBarCode).ToString();

                //Detalle
                List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                invoiceModel.InvoiceDetails = invoiceDetails;

                InvoiceModels.Add(invoiceModel);

                var rptSource = InvoiceModels;
                var strReportName = "Areas//Accounting//Reports//PaymentService//BarcodeInvoiceReport.rpt";

                bool isValid = true;

                if (isValid)
                {
                    ReportDocument rd = new ReportDocument();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + strReportName;

                    rd.Load(strRptPath);
                    // Lena Reporte Principal
                    rd.SetDataSource(rptSource);
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "BarcodeInvoiceReport");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region AccountBank

        /// <summary>
        /// GetAccountBank
        /// Obtiene el Id/Descripción de los bancos registrados a la Compañía
        /// </summary>
        /// <returns>List<object/></returns>
        public List<object> GetAccountBank()
        {
            List<object> banks = new List<object>();
            var distinctBankAccounts = DelegateService.accountingParameterService.GetBankAccountPersons()
                .GroupBy(p => new { p.Bank.Id, p.Bank.Description })
                .Select(g => g.First())
                .ToList();

            foreach (DTOs.BankAccounts.BankAccountPersonDTO personBankAccount in distinctBankAccounts)
            {
                banks.Add(new
                {
                    Description = personBankAccount.Bank.Description,
                    Id = personBankAccount.Bank.Id
                });
            }
            return banks;
        }

        #endregion

        #region ReadFilesInMemory
        /// <summary>
        /// ReadFileInMemory
        /// Lee un archivo sin guardar
        /// aurresta
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ExelToStream(uploadedFile);
            }
            if (data[1] == "xml")
            {
                return XmlToStream(uploadedFile);
            }
            if (data[1] == "csv")
            {
                return CsvToStream(uploadedFile);
            }
            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExelToStream
        /// Lee un archivo en formato xls, xlsx sin guardar
        /// aUrresta
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExelToStream(HttpPostedFileBase uploadedFile)
        {
            string fileLocationName = "";
            string infoAgent = "";
            Byte[] arrayContent;            
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // convertir a Bytes
            var buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);

            //Lee el archivo y  guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buf;
            Stream stream = new MemoryStream(arrayContent);
            IExcelDataReader excelReader;

            if (data[1] == "xls")
            {
                //1. Lee desde  binary Excel  ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //2. Lee desde  binary OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            //3. DataSet - El resultado sera creado en result.Tables
            DataSet result = excelReader.AsDataSet();

            //4. DataSet - Crea column names en primera fila
            excelReader.IsFirstRowAsColumnNames = true;

            List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();
            int payerId = 0;
            int payerIndividualId = 0;
            int insuredIndividualId = 0;

            for (int i = 1; i < result.Tables[0].Rows.Count; i++)
            {
                DataRow row = result.Tables[0].Rows[i];
                infoAgent = DelegateService.uniquePersonServiceV1.GetAgentByQuery(row[16].ToString())[0].EmployeePerson.IdCardNo + "-" +
                            row[16].ToString() + "/" + row[15].ToString();

                try
                {
                    payerId = row[19] == DBNull.Value ? -1 : Convert.ToInt32(row[19]);
                    payerIndividualId = row[19] == DBNull.Value ? -1 : Convert.ToInt32(row[19]);
                    insuredIndividualId = row[13] == DBNull.Value ? -1 : Convert.ToInt32(row[13]);
                                        
                    if (IsNegative(Convert.ToInt32(row[0])) || IsNegative(Convert.ToInt32(row[8])) ||
                        IsNegative(Convert.ToInt32(row[6])) || IsNegative(Convert.ToInt32(row[4])) ||
                        IsNegative(Convert.ToInt32(row[2])) || IsNegative(Convert.ToInt32(row[13])) ||
                        IsNegative(Convert.ToInt32(row[15])))
                    {

                        fileLocationName = "NegativeId";
                        break;
                    }

                    premiumReceivableSearchPolicies.Add(new DTOs.Search.PremiumReceivableSearchPolicyDTO()
                    {
                        PaymentAmount = row[11] == DBNull.Value ? 0 : Convert.ToDecimal(row[11]),
                        BranchId = row[0] == DBNull.Value ? 0 : Convert.ToInt32(row[0]),
                        BranchDescription = row[1] == DBNull.Value ? "" : Convert.ToString(row[1]),
                        CurrencyId = row[8] == DBNull.Value ? 0 : Convert.ToInt32(row[8]),
                        CurrencyDescription = row[9] == DBNull.Value ? "" : Convert.ToString(row[9]),
                        EndorsementId = row[6] == DBNull.Value ? 0 : Convert.ToInt32(row[6]),
                        EndorsementDocumentNumber = row[7] == DBNull.Value ? "" : Convert.ToString(row[7]),
                        PolicyId = row[4] == DBNull.Value ? 0 : Convert.ToInt32(row[4]),
                        PolicyDocumentNumber = row[5] == DBNull.Value ? "" : Convert.ToString(row[5]),
                        PrefixId = row[2] == DBNull.Value ? 0 : Convert.ToInt32(row[2]),
                        PrefixDescription = row[3] == DBNull.Value ? "" : Convert.ToString(row[3]),
                        PaymentNumber = row[10] == DBNull.Value ? -1 : Convert.ToInt32(row[10]),
                        PaymentExpirationDate = Convert.ToDateTime(row[12]),
                        InsuredId = row[13] == DBNull.Value ? -1 : Convert.ToInt32(row[13]),
                        InsuredName = row[14] == DBNull.Value ? "" : Convert.ToString(row[14]),
                        PolicyAgentId = row[15] == DBNull.Value ? -1 : Convert.ToInt32(row[15]),
                        PolicyAgentName = row[16] == DBNull.Value ? "" : Convert.ToString(row[16]),
                        BranchPrefixPolicyEndorsement =
                            Convert.ToString(row[1]).Substring(0, 3) + '-' + Convert.ToString(row[3]).Substring(0, 3) +
                            '-' + Convert.ToString(row[5]) + '-' + Convert.ToString(row[7]),
                        ExchangeRate = 1,
                        StandarCommissionPercentage = row[17] == DBNull.Value ? 0 : Convert.ToDecimal(row[17]),
                        AgentParticipationPercentage = row[18] == DBNull.Value ? 0 : Convert.ToDecimal(row[18]),

                        PayerId = payerId,
                        PayerIndividualId = payerIndividualId,
                        InsuredIndividualId = insuredIndividualId

                    });
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
            }
            TempData["dtoExel"] = premiumReceivableSearchPolicies;
            stream.Close();
            stream.Dispose();
            if (fileLocationName != "FormatException" && fileLocationName != "OverflowException" &&
                fileLocationName != "IndexOutOfRangeException" && fileLocationName != "InvalidCastException")
            {
                fileLocationName = fileLocationName + "/" + infoAgent;
            }
            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// XmlToStream
        /// Lee un archivo en formato xml sin guardar
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns>JsonResult</returns>
        public JsonResult XmlToStream(HttpPostedFileBase uploadedFile)
        {
            try
            {
                string fileLocationName = "";
                Byte[] arrayContent;

                fileLocationName = uploadedFile.FileName;
                string[] data = fileLocationName.Split(new char[] { '.' });

                // convertir a Bytes
                var buf = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);

                //Leer el archivo y lo guarda en arreglo de typo byte y este a su vez a arrContent

                arrayContent = buf;
                Stream stream = new MemoryStream(arrayContent);
                stream.Position = 0;
                List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies;

                int index = 0;

                premiumReceivableSearchPolicies = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(stream);

                XmlNodeList policies = xDoc.GetElementsByTagName("policies");
                XmlNodeList policyNode = ((XmlElement)policies[0]).GetElementsByTagName("policy");

                foreach (XmlElement node in policyNode)
                {
                    try
                    {
                        XmlNodeList payerId;
                        XmlNodeList insuredIndividualId;
                        XmlNodeList payerIndividualId;
                        XmlNodeList branchId = node.GetElementsByTagName("branchId");
                        XmlNodeList branch = node.GetElementsByTagName("branch");
                        XmlNodeList prefixId = node.GetElementsByTagName("prefixId");
                        XmlNodeList prefix = node.GetElementsByTagName("prefix");
                        XmlNodeList policyId = node.GetElementsByTagName("policyId");
                        XmlNodeList number = node.GetElementsByTagName("number");
                        XmlNodeList endorsementId = node.GetElementsByTagName("endorsementId");
                        XmlNodeList endorsement = node.GetElementsByTagName("endorsement");
                        XmlNodeList currencyId = node.GetElementsByTagName("currencyId");
                        XmlNodeList currency = node.GetElementsByTagName("currency");
                        XmlNodeList quota = node.GetElementsByTagName("quota");
                        XmlNodeList amount = node.GetElementsByTagName("amount");
                        XmlNodeList expirationDate = node.GetElementsByTagName("expirationdate");
                        XmlNodeList insuredId = node.GetElementsByTagName("insuredId");
                        XmlNodeList insured = node.GetElementsByTagName("insured");
                        XmlNodeList agentId = node.GetElementsByTagName("agentId");
                        XmlNodeList agent = node.GetElementsByTagName("agent");
                        XmlNodeList percentage = node.GetElementsByTagName("percentage");
                        XmlNodeList commission = node.GetElementsByTagName("commission");

                        payerId = node.GetElementsByTagName("payerId");
                        insuredIndividualId = node.GetElementsByTagName("payerId");
                        payerIndividualId = node.GetElementsByTagName("payerIndividualId");

                        if (IsNegative(Convert.ToInt32(branchId[0].InnerText)) || IsNegative(Convert.ToInt32(currencyId[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(endorsementId[0].InnerText)) || IsNegative(Convert.ToInt32(policyId[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(prefixId[0].InnerText)) || IsNegative(Convert.ToInt32(insuredId[0].InnerText)) ||
                        IsNegative(Convert.ToInt32(agentId[0].InnerText)))
                        {
                            fileLocationName = "NegativeId";
                            break;
                        }

                        premiumReceivableSearchPolicies.Add(new DTOs.Search.PremiumReceivableSearchPolicyDTO()
                        {
                            PaymentAmount = Convert.ToDecimal(amount[0].InnerText.Replace(".", ",")),
                            BranchId = Convert.ToInt32(branchId[0].InnerText),
                            BranchDescription = Convert.ToString(branch[0].InnerText),
                            CurrencyId = Convert.ToInt32(currencyId[0].InnerText),
                            CurrencyDescription = Convert.ToString(currency[0].InnerText),
                            EndorsementId = Convert.ToInt32(endorsementId[0].InnerText),
                            EndorsementDocumentNumber = Convert.ToString(endorsement[0].InnerText),
                            PolicyId = Convert.ToInt32(policyId[0].InnerText),
                            PolicyDocumentNumber = Convert.ToString(number[0].InnerText),
                            PrefixId = Convert.ToInt32(prefixId[0].InnerText),
                            PrefixDescription = Convert.ToString(prefix[0].InnerText),
                            PaymentNumber = Convert.ToInt32(quota[0].InnerText),
                            PaymentExpirationDate = Convert.ToDateTime(expirationDate[0].InnerText),
                            InsuredId = Convert.ToInt32(insuredId[0].InnerText),
                            InsuredName = Convert.ToString(insured[0].InnerText),
                            PolicyAgentId = Convert.ToInt32(agentId[0].InnerText),
                            PolicyAgentName = Convert.ToString(agent[0].InnerText),
                            BranchPrefixPolicyEndorsement = Convert.ToString(branch[0].InnerText).Substring(0, 3) + '-' + Convert.ToString(prefix[0].InnerText).Substring(0, 3) + '-' + Convert.ToString(number[0].InnerText) + '-' + Convert.ToString(endorsement[0].InnerText),
                            ExchangeRate = 1,
                            StandarCommissionPercentage = Convert.ToDecimal(percentage[0].InnerText.Replace(".", ",")),
                            AgentParticipationPercentage = Convert.ToDecimal(commission[0].InnerText.Replace(".", ",")),
                            PayerId = Convert.ToInt32(payerId[0].InnerText),
                            PayerIndividualId = Convert.ToInt32(payerIndividualId[0].InnerText),
                            InsuredIndividualId = Convert.ToInt32(insuredIndividualId[0].InnerText)
                        });
                        index++;
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

                TempData["dtoXml"] = premiumReceivableSearchPolicies;
                stream.Close();
                stream.Dispose();
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
            catch (XmlException)
            {
                string fileLocationName = "";
                fileLocationName = "XmlException";
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
            catch (NullReferenceException)
            {
                string fileLocationName = "";
                fileLocationName = "XmlException";
                return Json(fileLocationName, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Commissions

        ///Alejo
        /// obtiene la comision pendiente, a partir de la póliza y el endoso
        /// <summary>
        /// GetPendingCommission
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPendingCommission(int policyId, int endorsementId)
        {
           DTOs.Search.PendingCommissionDTO commissions = DelegateService.accountingApplicationService.GetPendingCommission(policyId, endorsementId);
           return Json(commissions, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods

        /// <summary>
        /// LoadImage
        /// Carga imágenes para usarlos en reportes y en excel.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="workbook"></param>
        /// <returns>int</returns>
        public static int LoadImage(string path, HSSFWorkbook workbook)
        {           
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);

                return workbook.AddPicture(buffer, PictureType.JPEG);
            }
        }

        #endregion

        #region Method Private

        #region ExportToExcel

        /// <summary>
        /// GetCurrentAuxiliaryData
        /// Función para armar excel 
        /// </summary>
        /// <param name="premiumReceivableSearchPolicies"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GetCurrentAuxiliaryData(DataTable premiumReceivableSearchPolicies)
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

            var fontdetail = workbook.CreateFont();
            fontdetail.FontName = "Tahoma";
            fontdetail.FontHeightInPoints = 8;
            fontdetail.Boldweight = 3;

            ICellStyle styledetalle = workbook.CreateCellStyle();
            styledetalle.SetFont(fontdetail);
            styledetalle.BottomBorderColor = HSSFColor.Black.Index;
            styledetalle.LeftBorderColor = HSSFColor.Black.Index;
            styledetalle.RightBorderColor = HSSFColor.Black.Index;
            styledetalle.TopBorderColor = HSSFColor.Black.Index;
            styledetalle.BorderBottom = BorderStyle.Thin;
            styledetalle.BorderLeft = BorderStyle.Thin;
            styledetalle.BorderRight = BorderStyle.Thin;
            styledetalle.BorderTop = BorderStyle.Thin;

            var headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0).SetCellValue("Sucursal ID");
            headerRow.CreateCell(1).SetCellValue("Sucursal");
            headerRow.CreateCell(2).SetCellValue("Ramo Id");
            headerRow.CreateCell(3).SetCellValue("Ramo");
            headerRow.CreateCell(4).SetCellValue("Póliza");
            headerRow.CreateCell(5).SetCellValue("Nro. Póliza");
            headerRow.CreateCell(6).SetCellValue("Endorsement ID");
            headerRow.CreateCell(7).SetCellValue("Nro. Endoso");
            headerRow.CreateCell(8).SetCellValue("Moneda Id");
            headerRow.CreateCell(9).SetCellValue("Moneda");
            headerRow.CreateCell(10).SetCellValue("Cuota");
            headerRow.CreateCell(11).SetCellValue("Importe");
            headerRow.CreateCell(12).SetCellValue("Fecha Expiration");
            headerRow.CreateCell(13).SetCellValue("Id Individual Asegurado");
            headerRow.CreateCell(14).SetCellValue("Nombre Asegurado");
            headerRow.CreateCell(15).SetCellValue("Id Individual Agente");
            headerRow.CreateCell(16).SetCellValue("Nombre Agente");
            headerRow.CreateCell(17).SetCellValue("Pje. Comisión");
            headerRow.CreateCell(18).SetCellValue("Comisión");
            headerRow.CreateCell(19).SetCellValue("Payer Id");
            headerRow.CreateCell(20).SetCellValue("Payer Individual Id");

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
            sheet.SetColumnWidth(12, 20 * 256);
            sheet.SetColumnWidth(13, 20 * 256);
            sheet.SetColumnWidth(14, 20 * 256);
            sheet.SetColumnWidth(15, 20 * 256);
            sheet.SetColumnWidth(16, 20 * 256);
            sheet.SetColumnWidth(17, 20 * 256);
            sheet.SetColumnWidth(18, 20 * 256);
            sheet.SetColumnWidth(19, 20 * 256);
            sheet.SetColumnWidth(20, 20 * 256);

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
            headerRow.GetCell(12).CellStyle = styleHeader;
            headerRow.GetCell(13).CellStyle = styleHeader;
            headerRow.GetCell(14).CellStyle = styleHeader;
            headerRow.GetCell(15).CellStyle = styleHeader;
            headerRow.GetCell(16).CellStyle = styleHeader;
            headerRow.GetCell(17).CellStyle = styleHeader;
            headerRow.GetCell(18).CellStyle = styleHeader;
            headerRow.GetCell(19).CellStyle = styleHeader;
            headerRow.GetCell(20).CellStyle = styleHeader;

            int rowNumber = 1;

            foreach (DataRow premiumReceivableSearchPolicie in premiumReceivableSearchPolicies.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < premiumReceivableSearchPolicies.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(premiumReceivableSearchPolicie.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styledetalle;
                }

            }

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;
        }

        #endregion

        #region Calculations

        /// <summary>
        /// CheckDigit
        /// Dígito de control. Para comprobar el dígito de control (por ejemplo, inmediatamente después de leer un código de barras mediante un escáner), 
        /// numeramos los dígitos de derecha a izquierda. A continuación se suman los dígitos de las posiciones impares, el resultado se multiplica por 3, 
        /// y se le suman los dígitos de las posiciones pares. Se busca decena inmediatamente superior y se le resta el resultado obtenido. El resultado 
        /// final es el dígito de control. Si el resultado es múltiplo de 10 el dígito de control será 0.
        /// EAN (European Article Number)
        /// </summary>
        /// <param name="numberBarCode"></param>
        /// <returns></returns>
        private int CheckDigit(string numberBarCode)
        {
            int checkDigit = 0;

            try
            {
                int oddSum = 0;
                int sumPairs = 0;
                string numberRightToLeft = "";

                numberRightToLeft = ConvertStringRightToLeft(numberBarCode);

                while (numberRightToLeft.Length > 0)
                {
                    oddSum += Convert.ToInt32(numberRightToLeft.Substring(0, 1));
                    sumPairs += Convert.ToInt32(numberRightToLeft.Substring(1, 1));
                    numberRightToLeft = Mid(numberRightToLeft, 2);
                }

                int increasedThreeFold = oddSum * 3;
                int addOddPairs = increasedThreeFold + sumPairs;
                int topTen = Convert.ToInt32(addOddPairs / 10);
                topTen = (topTen + 1) * 10;
                checkDigit = topTen - addOddPairs;

            }
            catch (Exception ex)
            {
                string messageError = ex.ToString(); //mensaje para conocer la descripción del error.
            }

            return checkDigit;
        }

        /// <summary>
        /// Mid
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns>string</returns>
        private string Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex, length);
            return result;
        }

        /// <summary>
        /// Mid
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <returns>string</returns>
        private string Mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex);
            return result;
        }

        /// <summary>
        /// ConvertStringRightToLeft
        /// </summary>
        /// <param name="numberBarCode"></param>
        /// <returns>string</returns>
        private string ConvertStringRightToLeft(string numberBarCode)
        {
            string result = "";

            for (int i = numberBarCode.Length; i > 0; --i)
            {
                result += Mid(numberBarCode, i - 1, 1);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Generic
        /// </summary>
        private class Generic
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// IsNegative
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private bool IsNegative(int id)
        {
            if (id < 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// CsvToStream
        /// Lee un archivo en formato csv 
        /// </summary>
        /// <param name="uploadedFile"> </param>
        /// <returns>JsonResult</returns>
        private JsonResult CsvToStream(HttpPostedFileBase uploadedFile)
        {
            string fileLocationName = "";
            Byte[] arrayContent;

            fileLocationName = uploadedFile.FileName;

            var buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y  guarda en arreglo de typo byte y este a su vez a arrContent
            arrayContent = buf;

            Stream stream = new MemoryStream(arrayContent);
            List<DTOs.Search.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies = new List<DTOs.Search.PremiumReceivableSearchPolicyDTO>();

            int payerId = 0;
            int payerIndividualId = 0;
            int insuredIndividualId = 0;

            try
            {
                int index = 0;
                StreamReader objReader = new StreamReader(stream);
                string sLine = "";

                while (sLine != null)
                {
                    try
                    {
                        sLine = objReader.ReadLine();
                        if ((sLine != null) && (index > 0))
                        {
                            string[] data = sLine.Split(new char[] { ';' });
                            if (IsNegative(Convert.ToInt32(data[0])) || IsNegative(Convert.ToInt32(data[8])) ||
                                IsNegative(Convert.ToInt32(data[6])) || IsNegative(Convert.ToInt32(data[4])) ||
                                IsNegative(Convert.ToInt32(data[2])) || IsNegative(Convert.ToInt32(data[13])) ||
                                IsNegative(Convert.ToInt32(data[15])))
                            {
                                fileLocationName = "NegativeId";
                                break;
                            }
                            payerId = Convert.ToInt32(data[19]);
                            payerIndividualId = Convert.ToInt32(data[19]);
                            insuredIndividualId = Convert.ToInt32(data[13]);

                            premiumReceivableSearchPolicies.Add(new DTOs.Search.PremiumReceivableSearchPolicyDTO()
                            {
                                PaymentAmount = Convert.ToDecimal(data[11]),
                                BranchId = Convert.ToInt32(data[0]),
                                BranchDescription = Convert.ToString(data[1]),
                                CurrencyId = Convert.ToInt32(data[8]),
                                CurrencyDescription = Convert.ToString(data[9]),
                                EndorsementId = Convert.ToInt32(data[6]),
                                EndorsementDocumentNumber = Convert.ToString(data[7]),
                                PolicyId = Convert.ToInt32(data[4]),
                                PolicyDocumentNumber = Convert.ToString(data[5]),
                                PrefixId = Convert.ToInt32(data[2]),
                                PrefixDescription = Convert.ToString(data[3]),
                                PaymentNumber = Convert.ToInt32(data[10]),
                                PaymentExpirationDate = Convert.ToDateTime(data[12]),
                                InsuredId = Convert.ToInt32(data[13]),
                                InsuredName = Convert.ToString(data[14]),
                                PolicyAgentId = Convert.ToInt32(data[15]),
                                PolicyAgentName = Convert.ToString(data[16]),
                                BranchPrefixPolicyEndorsement = Convert.ToString(data[1]).Substring(0, 3) + '-' + Convert.ToString(data[3]).Substring(0, 3) + '-' + Convert.ToString(data[5]) + '-' + Convert.ToString(data[7]),
                                ExchangeRate = 1,
                                StandarCommissionPercentage = Convert.ToDecimal(data[17]),
                                AgentParticipationPercentage = Convert.ToDecimal(data[18]),
                                PayerId = payerId,
                                PayerIndividualId = payerIndividualId,
                                InsuredIndividualId = insuredIndividualId
                            });

                        }
                        index++;
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
                objReader.Close();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            TempData["dtoCsv"] = premiumReceivableSearchPolicies;
            stream.Close();
            stream.Dispose();

            return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }
        #endregion Private
    }
}
