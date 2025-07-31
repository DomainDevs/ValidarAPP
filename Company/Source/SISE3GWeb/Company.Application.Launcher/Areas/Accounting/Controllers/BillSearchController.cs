using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniqueUserServices;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using UserModels = Sistran.Core.Application.UniqueUserServices.Models;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using DebitCredit = Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.CommonController.DebitCredit;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using  Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class BillSearchController : Controller
    {
        #region Class

        /// <summary>
        /// ReceiptSearchDTO
        /// </summary>
        public class ReceiptSearchDTO
        {
            public int BillCode { get; set; }
            public int BillStatus { get; set; }
            public string StatusDescription { get; set; }
            public string PaymentsTotal { get; set; }
            public string AccountName { get; set; }
            public string RegisterDate { get; set; }
            public string AccountingDate { get; set; }
            public int PayerId { get; set; }
            public string Payer { get; set; }
            public int BranchCode { get; set; }
            public string BranchDescription { get; set; }
            public int BillControlCode { get; set; }
            public string DocumentNumber { get; set; }
            public string Comments { get; set; }
            public int JournalEntryId { get; set; }
            public int TechnicalTransaction { get; set; }
        }

        #endregion

        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion

        #region Instance Variables

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region MainBillSearch

        /// <summary>
        /// MainBillSearch
        /// Pantalla Busqueda de recibos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBillSearch()
        {

            try
            {       

                List<TransactionTypeDTO> transactionTypes = new List<TransactionTypeDTO>();

                transactionTypes.Add(new TransactionTypeDTO() { Id = 1, Description = Global.Credit});
                transactionTypes.Add(new TransactionTypeDTO() { Id = 2, Description = Global.Debit });
                ViewBag.TransactionType = transactionTypes;

                List<DebitCredit> debitCredits = new List<DebitCredit>();

                debitCredits.Add(new DebitCredit() { Id = 1, Description = Global.Credit });
                debitCredits.Add(new DebitCredit() { Id = 2, Description = Global.Debit });
                ViewBag.DebitCredit = debitCredits;

                List<ValueTypeDTO> valueTypes = new List<ValueTypeDTO>();

                valueTypes.Add(new ValueTypeDTO() { Id = 1, Description = "CHEQUE POSTFECHADO" });
                valueTypes.Add(new ValueTypeDTO() { Id = 2, Description = "TARJETA POSTFECHADA" });
                ViewBag.valueTypes = valueTypes;

                //Payment_Methods 
                //Se utilizan los parámetros definidos en el web.config en lugar de los definidos en el archivo de recursos.
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

                //TIPO IMPUTACION
                ViewBag.ImputationType = ConfigurationManager.AppSettings["ImputationTypeBill"];

                //Tipo de Beneficiario
                ViewBag.SupplierCode = Convert.ToInt32(ConfigurationManager.AppSettings["SupplierCode"]); // 10
                ViewBag.InsuredCode = Convert.ToInt32(ConfigurationManager.AppSettings["InsuredCode"]); //7
                ViewBag.CoinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"]); //2
                ViewBag.ThirdPartyCode = Convert.ToInt32(ConfigurationManager.AppSettings["ThirdPartyCode"]);//8
                ViewBag.AgentCode = Convert.ToInt32(ConfigurationManager.AppSettings["AgentCode"]); //1
                ViewBag.ProducerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ProducerCode"]); //1
                ViewBag.EmployeeCode = Convert.ToInt32(ConfigurationManager.AppSettings["EmployeeCode"]);//11
                ViewBag.ReinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ReinsurerCode"]); //2
                ViewBag.TradeConsultant = Convert.ToInt32(ConfigurationManager.AppSettings["TradeConsultant"]); //8
                ViewBag.ContractorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ContractorCode"]); //13

                //habilita o deshabilita el botón de Reversión en la búsqueda de recibos
                ViewBag.HasRevertImputation = ConfigurationManager.AppSettings["HasRevertImputation"];

                //Viene de cancelación del recibo
                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;
                ViewBag.BranchId = TempData["BranchId"] ?? 0;

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
        /// LoadBranch
        /// Obtiene las sucursales por usuario logueado
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadBranch()
        {
            return new UifSelectResult(_commonController.LoadBranch(User.Identity.Name));
        }

        /// <summary>
        /// GetBillingConceptsList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBillingConceptsList()
        {
            List<CollectConceptDTO> collectConcepts = DelegateService.accountingParameterService.GetCollectConcepts();
            return new UifSelectResult(collectConcepts);
        }

        #endregion

        #region BillingSearch

        /// <summary>
        /// GetPaymentTicketItemByPaymentId
        /// Busca si existe el Pago
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentTicketItemByPaymentId(int paymentId)
        {
            bool existPayment = true;
            List<PaymentTicketItemDTO> paymentTicketItems = new List<PaymentTicketItemDTO>();
            if (paymentTicketItems.Count > 0)
            {
                foreach (PaymentTicketItemDTO paymentTicketItem in paymentTicketItems)
                {
                    if (paymentTicketItem.PaymentCode == paymentId)
                    {
                        existPayment = false;
                        break;
                    }
                }
            }
            return Json(existPayment, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SearchBills
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billingConceptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="billId"></param>
        /// <param name="receiptStatus"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchBills(int branchId, int billingConceptId, string startDate, string endDate, int userId,
                                      int? billId, int receiptStatus, int? imputationTypeId)
        {
            try
            {


                if (endDate != null && endDate != "")
                {
                    endDate = endDate + " 23:59:59";
                }

                List<SearchCollectDTO> searchCollects =
                DelegateService.accountingCollectService.SearchCollects(branchId, billingConceptId, startDate, endDate, userId, Convert.ToInt32(billId), (imputationTypeId ?? 0), -1).OrderBy(o => o.CollectCode).ToList();

                List<ReceiptSearchModel> receipts = new List<ReceiptSearchModel>();

                string statusDescription = "";
                
                foreach (SearchCollectDTO searchCollect in searchCollects)
                {
                    statusDescription = Enum.GetName(typeof(CollectStatus), searchCollect.CollectStatus);

                    receipts.Add(new ReceiptSearchModel
                    {
                        BillCode = searchCollect.CollectCode,
                        BillStatus = searchCollect.CollectStatus,
                        StatusDescription = Global.ResourceManager.GetString(statusDescription),
                        PaymentsTotal = String.Format(new CultureInfo("en-US"), "{0:C}", searchCollect.PaymentsTotal),
                        AccountName = String.IsNullOrEmpty(searchCollect.AccountName) ? DelegateService.uniqueUserService.GetUserById(Convert.ToInt32(searchCollect.UserId)).AccountName : searchCollect.AccountName,
                        RegisterDate = Convert.ToDateTime(searchCollect.RegisterDate).ToShortDateString(),
                        AccountingDate = Convert.ToDateTime(searchCollect.AccountingDate).ToShortDateString(),
                        PayerId = searchCollect.PayerId,
                        Payer = searchCollect.Payer,
                        BranchCode = searchCollect.BranchCode,
                        BranchDescription = searchCollect.BranchDescription,
                        BillControlCode = searchCollect.CollectControlCode,
                        DocumentNumber = searchCollect.DocumentNumber,
                        Comments = searchCollect.Comments,
                        JournalEntryId = searchCollect.JournalEntryId,
                        TechnicalTransaction = searchCollect.TechnicalTransaction
                    });
                }

                if (receiptStatus != -1)
                {
                    receipts = receipts.Where(sl => sl.BillStatus == receiptStatus).ToList();
                }

                return Json(new { aaData = receipts, total = receipts.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                List<object> resultError = new List<object>();
                resultError.Add(new
                {
                    BillCode = "",
                    BillStatus = "",
                    StatusDescription = Global.UnhandledExceptionMsj,
                    PaymentsTotal = "",
                    AccountName = "",
                    RegisterDate = "",
                    AccountingDate = "",
                    PayerId = "",
                    Payer = "",
                    BranchCode = "",
                    BranchDescription = "",
                    BillControlCode = "",
                    DocumentNumber = "",
                    Comments = "",
                    JournalEntryId = "",
                    TechnicalTransaction = ""
                });

                return Json(new { aaData = resultError, total = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SearchJurnalEntry
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="journalEntryStatusId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="billId"></param>
        /// <param name="receiptStatus"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchJournalEntry(int branchId, int journalEntryStatusId, string startDate, string endDate, int userId, int billId, int receiptStatus, int? imputationTypeId)
        {
            List<object> searchCollects = new List<object>();

            try
            {

                if (endDate != null && endDate != "")
                {
                    endDate = endDate + " 23:59:59";
                }

                List<SearchCollectDTO> searchCollectDTOs =
                DelegateService.accountingCollectService.SearchCollects(branchId, -1, startDate, endDate, userId, billId, (imputationTypeId ?? 0), journalEntryStatusId).OrderBy(o => o.ImputationId).ToList();

                string statusDescription = "";

                foreach (SearchCollectDTO searchCollectDTO in searchCollectDTOs)
                {
                    statusDescription = searchCollectDTO.JournalEntryStatus == 0 ? Global.Reverted : Global.Applied;

                    searchCollects.Add(new
                    {
                        ImputationCode = searchCollectDTO.ImputationId,
                        JournalEntryStatus = searchCollectDTO.JournalEntryStatus,
                        StatusDescription = statusDescription,
                        AccountName = String.IsNullOrEmpty(searchCollectDTO.AccountName) ? DelegateService.uniqueUserService.GetUserById(Convert.ToInt32(searchCollectDTO.UserId)).AccountName : searchCollectDTO.AccountName,
                        RegisterDate = Convert.ToDateTime(searchCollectDTO.RegisterDate).ToShortDateString(),
                        AccountingDate = Convert.ToDateTime(searchCollectDTO.AccountingDate).ToShortDateString(),
                        PayerId = searchCollectDTO.PayerId,
                        Payer = searchCollectDTO.Payer,
                        BranchCode = searchCollectDTO.BranchCode,
                        BranchDescription = searchCollectDTO.BranchDescription,
                        DocumentNumber = searchCollectDTO.DocumentNumber,
                        Comments = searchCollectDTO.Comments,
                        JournalEntryId = searchCollectDTO.JournalEntryId,
                        TechnicalTransaction = searchCollectDTO.TechnicalTransaction,
                        AccountingTransaction = searchCollectDTO.AccountingTransaction
                    });
                }

                return Json(new { success = true, aaData = searchCollects, total = searchCollects.Count }, JsonRequestBehavior.AllowGet);

            }

            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
           
        }

        /// <summary>
        /// DetailValues
        /// Devuelve el detalle de un recibo enviando como parametro el numero de recibo
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DetailValues(int billId)
        {
            List<object> detailValues = new List<object>();

            List<DetailValuesDTO> detailValuesDTOs = DelegateService.accountingPaymentService.DetailValues(billId).OrderBy(o => o.PaymentTypeId).ToList();

            foreach (DetailValuesDTO detailValuesDTO in detailValuesDTOs)
            {
                detailValues.Add(new
                {
                    PaymentTypeId = detailValuesDTO.PaymentTypeId,
                    PaymentTypeDescription = detailValuesDTO.PaymentTypeDescription,
                    CurrencyId = detailValuesDTO.CurrencyId,
                    Currency = detailValuesDTO.Currency,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", detailValuesDTO.Amount),
                    Exchange = detailValuesDTO.Exchange,
                    LocalAmount = String.Format(new CultureInfo("en-US"), "{0:C}", detailValuesDTO.LocalAmount),
                    DocumentNumber = detailValuesDTO.DocumentNumber,
                    Voucher = detailValuesDTO.Voucher,
                    CardType = detailValuesDTO.CardType,
                    CardTypeName = detailValuesDTO.CardTypeName,
                    AuthorizationNumber = detailValuesDTO.AuthorizationNumber,
                    ValidThruMonth = detailValuesDTO.ValidThruMonth,
                    ValidThruYear = detailValuesDTO.ValidThruYear,
                    IssuingBankId = detailValuesDTO.IssuingBankId,
                    IssuinBankName = detailValuesDTO.IssuinBankName,
                    IssuingBankAccountNumber = detailValuesDTO.IssuingBankAccountNumber,
                    RecievingBankId = detailValuesDTO.RecievingBankId,
                    RecievingBankAccountNumber = detailValuesDTO.RecievingBankAccountNumber,
                    BillCode = detailValuesDTO.CollectCode,
                    Date = detailValuesDTO.Date,
                    IssuerName = detailValuesDTO.IssuerName,
                    Tax = String.Format(new CultureInfo("en-US"), "{0:C}", detailValuesDTO.Tax),
                    SerialVoucher = detailValuesDTO.SerialVoucher,
                    PaymentStatus = detailValuesDTO.PaymentStatus,
                    StatusDescription = detailValuesDTO.StatusDescription,
                    SerialNumber = detailValuesDTO.SerialNumber
                });
            }

            return new UifTableResult(detailValues);
        }

        /// <summary>
        /// GetReceiptStatus
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetReceiptStatus()
        {
            List<object> receiptStatus = new List<object>();
            receiptStatus.Add(new
            {
                Id = 1,                
                Description = @Global.Active
            });
            receiptStatus.Add(new
            {
                Id = 3,
                Description = @Global.Applied
            });
            receiptStatus.Add(new
            {
                Id = 0,                
                Description = @Global.Cancelled1
            });

            string billStatusPartiallyApplied = @Global.BillStatusPartiallyApplied;
            receiptStatus.Add(new
            {
                Id = 2,                
                Description = billStatusPartiallyApplied.ToLower()
            });

            return new UifSelectResult(receiptStatus);
        }

        /// <summary>
        /// GetJurnalEntryStatus
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetJurnalEntryStatus()
        {
            List<object> jurnalEntryStatus = new List<object>();
            jurnalEntryStatus.Add(new
            {
                Id = 0,
                Description = Global.Reverted
            });
            jurnalEntryStatus.Add(new
            {
                Id = 1,
                Description = Global.Applied
            });

            return new UifSelectResult(jurnalEntryStatus);
        }

        /// <summary>
        /// GenerateReceiptsToExcel
        /// Genera archivo excel desde la consulta de recibos y asientos de diario
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billingConceptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="billId"></param>
        /// <param name="receiptStatus"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GenerateReceiptsToExcel(int branchId, int billingConceptId, string startDate,
                                                    string endDate, int userId, int billId,
                                                    int receiptStatus, int? imputationTypeId)
        {

            if (endDate != null && endDate != "")
            {
                endDate = endDate + " 23:59:59";
            }

            List<SearchCollectDTO> searchCollects =
            DelegateService.accountingCollectService.SearchCollects(branchId, billingConceptId, startDate, endDate, userId, billId, (imputationTypeId ?? 0), -1).OrderBy(o => o.CollectCode).ToList();

            List<ReceiptSearchModel> receipts = new List<ReceiptSearchModel>();

            string statusDescription = "";
            string titleReport = "";
            int statusId = 0;


            foreach (SearchCollectDTO searchCollect in searchCollects)
            {
                statusId = searchCollect.CollectStatus;

                if (imputationTypeId == 2) //para asiento diario
                {
                    if (searchCollect.JournalEntryStatus == 1)
                    {
                        statusDescription = Global.Applied;
                    }
                    else if (searchCollect.JournalEntryStatus == 0)
                    {
                        statusDescription = Global.Reverted;
                    }

                    titleReport = Global.ListJournalEntries;
                }
                else //para recibo
                {
                    if (searchCollect.CollectStatus == 1)
                    {
                        statusDescription = CollectStatus.Active.ToString();
                    }
                    else if (searchCollect.CollectStatus == 0)
                    {
                        statusDescription = CollectStatus.Canceled.ToString();
                    }
                    else if (searchCollect.CollectStatus == 2)
                    {
                        statusDescription = CollectStatus.PartiallyApplied.ToString();
                    }
                    else if (searchCollect.CollectStatus == 3)
                    {
                        statusDescription = CollectStatus.Applied.ToString();
                    }

                    titleReport = Global.List + " " + Global.CashReceipt;
                }

                receipts.Add(new ReceiptSearchModel
                {
                    BillCode = searchCollect.CollectCode,
                    BillStatus = statusId,
                    StatusDescription = Global.ResourceManager.GetString(statusDescription),
                    PaymentsTotal = String.Format(new CultureInfo("en-US"), "{0:C}", searchCollect.PaymentsTotal),
                    AccountName = searchCollect.AccountName,
                    RegisterDate = Convert.ToDateTime(searchCollect.RegisterDate).ToShortDateString(),
                    AccountingDate = Convert.ToDateTime(searchCollect.AccountingDate).ToShortDateString(),
                    PayerId = searchCollect.PayerId,
                    Payer = searchCollect.Payer,
                    BranchCode = searchCollect.BranchCode,
                    BranchDescription = searchCollect.BranchDescription,
                    BillControlCode = searchCollect.CollectControlCode,
                    DocumentNumber = searchCollect.DocumentNumber,
                    JournalEntryId = searchCollect.JournalEntryId,
                    TechnicalTransaction = searchCollect.TechnicalTransaction
                });
            }

            if (receiptStatus != -1)
            {
                receipts = receipts.Where(sl => sl.BillStatus == receiptStatus).ToList();
            }          

            MemoryStream dataStream = ExportReceiptsStatus(ConvertReceiptsStatusToDataTable(receipts), titleReport);

            return File(dataStream.ToArray(), "application/vnd.ms-excel", "ListadoAsientoDiarioExcel.xls");
        }

        #endregion BillingSearch

        #region User

        /// <summary>
        /// GetUsers
        /// Obtiene usuarios
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetUsers(string query)
        {
            var userQuerys = DelegateService.uniqueUserService.GetUserByTextPersonId(query, 0);

            List<object> users = new List<object>();

            if (userQuerys != null)
            {
                if (userQuerys.Count > 0)
                {
                    foreach (UserModels.User user in userQuerys)
                    {
                        users.Add(new
                        {
                            id = user.UserId.ToString(),
                            value = user.AccountName
                        });
                    }
                }
            }
            else
            {
                users.Add(new
                {
                    id = -1,
                    value = @Global.RegisterNotFound
                });
            }

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateUserNick
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userNick"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateUserNick(int userId, string userNick)
        {
            bool isValid = false;

            List<UserModels.User> users = DelegateService.uniqueUserService.GetUserByName(userNick);

            if ((users.Count > 0) && (users[0].UserId == userId))
            {
                isValid = true;
            }

            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ModuleDate

        /// <summary>
        /// ConfirmModuleDate
        /// Confirma si la fecha enviada como parámetro esta en el mes contable actual
        /// </summary>
        /// <param name="date"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ConfirmModuleDate(string date)
        {
            List<object> accountingDates = new List<object>();
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
            DateTime dateBillControl = Convert.ToDateTime(date);

            if ((dateBillControl.Month == accountingDate.Month) && (dateBillControl.Year == accountingDate.Year))
            {
                accountingDates.Add(new
                {
                    Id = 1
                });
            }
            else
            {
                accountingDates.Add(new
                {
                    Id = -1
                });
            }

            return Json(accountingDates, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Items

        /// <summary>
        /// GetPaymentTicketItemsByBillId
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentTicketItemsByBillId(int billId)
        {
            List<object> paymentTicketItems = new List<object>();
            List<PaymentTicketItemDTO> paymentTicketItemDTOs = DelegateService.accountingPaymentTicketService.GetPaymentTicketItemsByCollectId(billId);

            if (paymentTicketItemDTOs.Count > 0)
            {
                paymentTicketItems.Add(new
                {
                    Id = -1
                });
            }
            else
            {
                paymentTicketItems.Add(new
                {
                    Id = 1
                });
            }

            return Json(paymentTicketItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ItemsBill
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ItemsBill(int billId)
        {
            bool existItemsCollect;
            List<object> itemsCollect = new List<object>();
            existItemsCollect = DelegateService.accountingPaymentService.ItemsCollect(billId);
            if (existItemsCollect)
            {
                itemsCollect.Add(new
                {
                    Id = 1
                });
            }
            else
            {
                itemsCollect.Add(new
                {
                    Id = -1
                });
            }

            return Json(itemsCollect, JsonRequestBehavior.AllowGet);
        }

        #endregion Items

        #region ExportToExcel

        /// <summary>
        /// ExportToExcel
        /// Exporta a excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataTable"></param>
        public static void ExportToExcel(string fileName, DataTable dataTable)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";

            string tab = "";
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                System.Web.HttpContext.Current.Response.Write(tab + dataColumn.ColumnName);
                tab = "\t";
            }
            System.Web.HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                tab = "";
                for (i = 0; i < dataTable.Columns.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(tab + dataRow[i].ToString());
                    tab = "\t";
                }
                System.Web.HttpContext.Current.Response.Write("\n");
            }

            System.Web.HttpContext.Current.Response.End();
        }

        #endregion ExportToExcel

        #region Validate

        /// <summary>
        /// ValidateDeposited
        /// Valida que los pagos de un recibo no hayan sido depositados
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateDeposited(int billId)
        {
            bool isDeposited = false;

            try
            {
                // Obtengo los items de la boleta interna a través del id de recibo
                List<PaymentTicketItemDTO> tickets = DelegateService.accountingPaymentTicketService.GetPaymentTicketItemsByCollectId(billId);

                // Reviso si la boleta externa contiene boletas internas, para validar si el pago está en boleta de depósito.
                if (tickets.Count > 0)
                {
                    foreach (PaymentTicketItemDTO ticket in tickets)
                    {
                        if (DelegateService.accountingPaymentBallotService.ValidateExternalBallotDeposited(ticket.PaymentTicketCode))
                        {
                            isDeposited = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return Json(isDeposited, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetImputationBySourceCodeImputationTypeId
        /// Valida que los pagos de un recibo no hayan sido depositados
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetImputationBySourceCodeImputationTypeId(int sourceId, int imputationTypeId)
        {
            bool existsCollectImputations = false;
            CollectApplicationDTO collectImputation = new CollectApplicationDTO();

            collectImputation.Collect = new CollectDTO() { Id = sourceId };
            collectImputation.Application = new ApplicationDTO() { ModuleId = imputationTypeId };

            List<CollectApplicationDTO> collectImputations = DelegateService.accountingCollectService.GetCollectImputations(collectImputation);

            if (collectImputations.Count > 0)
            {
                existsCollectImputations = true;
            }

            return Json(existsCollectImputations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidatePaymentRevertion
        /// Valida que no se revierta un cuota que no sea la última que se haya cobrado
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidatePaymentRevertion(int sourceId, int imputationTypeId)
        {
            bool isValidPayment = true;
            int paymentNumber = 0;
            int lastPaymentNumber = 0;

            try
            {
                List<PremiumReceivableItemDTO> premiumReceivableItems = _commonController.GetPremiumRecievableAppliedByBillId(sourceId, imputationTypeId);

                if (premiumReceivableItems.Any())
                {
                    premiumReceivableItems.ForEach(premium =>
                    {
                        paymentNumber = premium.PaymentNumber;

                        // Último pago cobrado
                        lastPaymentNumber = DelegateService.accountingApplicationService.GetApplicationPremiumsByEndorsementId(premium.EndorsementId).Max(x => x.PaymentNumber);

                        if (paymentNumber != lastPaymentNumber)
                        {
                            isValidPayment = false;
                        }
                    });
                }
                else
                {
                    // Si no hay cuotas, no es necesario validar orden de eliminación
                    isValidPayment = true;
                }
            }
            catch (Exception ex)
            {
                isValidPayment = false;
            }

            return Json(isValidPayment, JsonRequestBehavior.AllowGet);
        }

        #endregion Validate

        #region JournalEntry

        /// <summary>
        /// MainJournalEntryBillSearch
        /// Pantalla Busqueda de recibos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainJournalEntryBillSearch()
        {
            try
            {

                List<TransactionTypeDTO> transactionTypes = new List<TransactionTypeDTO>();

                transactionTypes.Add(new TransactionTypeDTO() { Id = 1, Description = "Crédito" });
                transactionTypes.Add(new TransactionTypeDTO() { Id = 2, Description = "Débito" });
                ViewBag.TransactionType = transactionTypes;

                List<DebitCredit> debitCredits = new List<DebitCredit>();

                debitCredits.Add(new DebitCredit() { Id = 1, Description = "Crédito" });
                debitCredits.Add(new DebitCredit() { Id = 2, Description = "Débito" });
                ViewBag.DebitCredit = debitCredits;

                List<ValueTypeDTO> valueTypes = new List<ValueTypeDTO>();

                valueTypes.Add(new ValueTypeDTO() { Id = 1, Description = "CHEQUE POSTFECHADO" });
                valueTypes.Add(new ValueTypeDTO() { Id = 2, Description = "TARJETA POSTFECHADA" });
                ViewBag.valueTypes = valueTypes;

                //Payment_Methods 
                //Se utilizan los parámetros definidos en el web.config en lugar de los definidos en el archivo de recursos.
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

                //TIPO IMPUTACION
                ViewBag.ImputationType = ConfigurationManager.AppSettings["ImputationTypeJournalEntry"];

                //Tipo de Beneficiario
                ViewBag.SupplierCode = Convert.ToInt32(ConfigurationManager.AppSettings["SupplierCode"]); // 10
                ViewBag.InsuredCode = Convert.ToInt32(ConfigurationManager.AppSettings["InsuredCode"]); //7
                ViewBag.CoinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"]); //2
                ViewBag.ThirdPartyCode = Convert.ToInt32(ConfigurationManager.AppSettings["ThirdPartyCode"]);//8
                ViewBag.AgentCode = Convert.ToInt32(ConfigurationManager.AppSettings["AgentCode"]); //1
                ViewBag.ProducerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ProducerCode"]); //1
                ViewBag.EmployeeCode = Convert.ToInt32(ConfigurationManager.AppSettings["EmployeeCode"]);//11
                ViewBag.ReinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ReinsurerCode"]); //2
                ViewBag.TradeConsultant = Convert.ToInt32(ConfigurationManager.AppSettings["TradeConsultant"]); //8
                ViewBag.ContractorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ContractorCode"]); //13

                //habilita o deshabilita el botón de Reversión en la búsqueda de recibos
                ViewBag.HasRevertImputation = ConfigurationManager.AppSettings["HasRevertImputation"];

                //Viene de cancelación del recibo
                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;
                ViewBag.BranchId = TempData["BranchId"] ?? 0;

                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/JournalEntrySearch/MainJournalEntryBillSearch.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion JournalEntry

        #region Method Private

        /// <summary>
        /// ConvertReceiptsStatusToDataTable
        /// Convierte el estado de recibos en una tabla
        /// </summary>
        /// <param name="receipts"></param>
        /// <returns>DataTable</returns>
        private DataTable ConvertReceiptsStatusToDataTable(List<ReceiptSearchModel> receipts)
        {
            DataTable dataTable = new DataTable();

            var headerRow = new List<string>(7);

            headerRow.Add(@Global.TransactionNumber);
            headerRow.Add(@Global.Status);
            headerRow.Add(@Global.BillTotal);
            headerRow.Add(@Global.User);
            headerRow.Add(@Global.Branch);
            headerRow.Add(@Global.RegisterDate);
            headerRow.Add(@Global.AccountingDate);
            headerRow.Add(@Global.Depositor);

            for (int j = 0; j < headerRow.Count; j++)
            {
                dataTable.Columns.Add(headerRow[j]);
            }

            try
            {
                foreach (ReceiptSearchModel receiptSearch in receipts)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow[0] = receiptSearch.TechnicalTransaction;
                    dataRow[1] = receiptSearch.StatusDescription;
                    dataRow[2] = receiptSearch.PaymentsTotal;
                    dataRow[3] = receiptSearch.AccountName;
                    dataRow[4] = receiptSearch.BranchDescription;
                    dataRow[5] = receiptSearch.RegisterDate;
                    dataRow[6] = receiptSearch.AccountingDate;
                    dataRow[7] = receiptSearch.Payer;

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
        /// Exporta a excel el resultado de la búsqueda de recibos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportReceiptsStatus(DataTable dataTable, string titleReport)
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
            titleRow.CreateCell(4).SetCellValue(titleReport); 
            titleRow.GetCell(4).CellStyle = styleTitle;

            var headerRow = sheet.CreateRow(2);

            headerRow.CreateCell(0).SetCellValue(@Global.TransactionNumber);
            headerRow.CreateCell(1).SetCellValue(@Global.Status);
            headerRow.CreateCell(2).SetCellValue(@Global.BillTotal);
            headerRow.CreateCell(3).SetCellValue(@Global.User);
            headerRow.CreateCell(4).SetCellValue(@Global.Branch);
            headerRow.CreateCell(5).SetCellValue(@Global.RegisterDate);
            headerRow.CreateCell(6).SetCellValue(@Global.AccountingDate);
            headerRow.CreateCell(7).SetCellValue(@Global.Depositor);
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 40 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 50 * 256);
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