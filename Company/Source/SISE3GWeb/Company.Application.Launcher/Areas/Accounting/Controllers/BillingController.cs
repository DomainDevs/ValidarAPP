using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Mvc;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Retentions;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using AccountingRuleModels = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;

//Sistran Company
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.Enums;
using TEMPMOD = Sistran.Core.Application.TempCommonServices.DTOs;
using ACCMOD = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application;
using Newtonsoft.Json;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    //[FilterConfigHelper.NoDirectAccessAttribute]
    public class BillingController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion

        #region Instance Variables
        readonly BaseController _baseController = new BaseController();
        readonly CommonController _commonController = new CommonController();
        #endregion

        #region CommonFunctions

        /// <summary>
        /// GetDate
        /// Obtiene la fecha
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDate()
        {
            string dateToday;

            string currentCultureName = CultureInfo.CurrentCulture.Name;

            if (currentCultureName == "en-US")
            {
                dateToday = String.Format("{0:MM/dd/yyyy}", DateTime.Today);
            }
            else
            {
                dateToday = String.Format("{0:dd/MM/yyyy}", DateTime.Today);
            }

            string[] dateSplit = dateToday.Split();

            return Json(dateSplit[0], JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Views

        /// <summary>
        /// MainBilling
        /// Pantalla de caja
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBilling()
        {
            try
            {
                //Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.UserNick = _commonController.GetUserByName(User.Identity.Name)[0].AccountName;
                ViewBag.UserId = _commonController.GetUserByName(User.Identity.Name)[0].UserId;
                ViewBag.ImputationTypeBill = ConfigurationManager.AppSettings["ImputationTypeBill"];

                // Payment Methods 
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
                ViewBag.ParamPaymentConsignmentCheck = ConfigurationManager.AppSettings["ParamPaymentConsignmentCheck"];
                ViewBag.ParamPaymentMethodRetentionReceipt = ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"];
                ViewBag.ParamPaymentMethodDataphone = ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"];
                ViewBag.ParamPaymentMethodElectronicPayment = ConfigurationManager.AppSettings["ParamPaymentMethodElectronicPayment"];
                ViewBag.ParamPaymentMethodPaymentArea = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentArea"];
                ViewBag.ParamPaymentMethodPaymentCard = ConfigurationManager.AppSettings["ParamPaymentMethodPaymentCard"];

                // Variables para la aplicación de preliquidaciones
                ViewBag.IsApply = TempData["IsApply"] ?? 0;
                ViewBag.PreliquidationId = TempData["PreliquidationId"] ?? "";
                ViewBag.BranchId = TempData["BranchId"] ?? "";
                ViewBag.BranchDescription = TempData["BranchDescription"] ?? "";
                ViewBag.PreliquidationBranch = TempData["PreliquidationBranch"] ?? 0;
                ViewBag.DocumentNumber = TempData["DocumentNumber"] ?? "";
                ViewBag.Name = TempData["Name"] ?? "";
                ViewBag.TempImputationId = TempData["TempImputationId"] ?? "";
                ViewBag.Description = TempData["Description"] ?? "";
                ViewBag.TotalAmount = TempData["TotalAmount"] ?? 0;
                ViewBag.IndividualId = TempData["IndividualId"] ?? "";
                ViewBag.PreliquidationPersonTypeId = TempData["PreliquidationPersonType"] ?? 0;

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);
                DateTime lastOpenDate = DelegateService.accountingCollectControlService.GetLastOpenDateByUserIdBranchId(ViewBag.UserId, ViewBag.BranchDefault);
                ViewBag.DateAccounting = lastOpenDate.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                ViewBag.MessageWaiting = @Global.MessageWaiting;

                // Variables para tipos de beneficiarios
                ViewBag.SupplierCode = ConfigurationManager.AppSettings["SupplierCode"];
                ViewBag.InsuredCode = ConfigurationManager.AppSettings["InsuredCode"];
                ViewBag.OthersCode = ConfigurationManager.AppSettings["OthersCode"];
                ViewBag.CollectorCode = ConfigurationManager.AppSettings["CollectorCode"];
                ViewBag.TradeAdviserCode = ConfigurationManager.AppSettings["TradeAdviserCode"];
                ViewBag.EmployeeCode = ConfigurationManager.AppSettings["EmployeeCode"];
                ViewBag.ReinsurerCode = ConfigurationManager.AppSettings["ReinsurerCode"];
                ViewBag.AgentCode = ConfigurationManager.AppSettings["AgentCode"];
                ViewBag.CoinsurerCode = ConfigurationManager.AppSettings["CoinsurerCode"];
                ViewBag.ThirdCode = ConfigurationManager.AppSettings["ThirdCode"];
                ViewBag.DefaultCurrency = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCurrencyCode"]);

                return View("~/Areas/Accounting/Views/Billing/MainBilling.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// CashModal
        /// Modal de efectivo
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CashModal()
        {
            return View();
        }

        #endregion

        #region AccountBank

        /// <summary>
        /// GetAccountByBankId
        /// Obtiene las cuentas asignadas a cada banco registrado a la Compañía
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountByBankId(int bankId)
        {
            List<object> bankAccounts = new List<object>();
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))).ToList();

            foreach (BankAccountCompanyDTO companyBankAccount in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    Id = companyBankAccount.Id,
                    Description = companyBankAccount.Number
                });
            }

            return new UifSelectResult(bankAccounts);
        }

        public ActionResult GetBankAccountCompaniesByBankIdCurrencyCode(int bankId, int currencyCode)
        {
            List<object> bankAccounts = new List<object>();
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompaniesByCurrencyCode(currencyCode);
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))).ToList();

            foreach (BankAccountCompanyDTO companyBankAccount in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    Id = companyBankAccount.Id,
                    Description = companyBankAccount.Number
                });
            }

            return new UifSelectResult(bankAccounts);
        }

        #endregion

        #region Bank

        /// <summary>
        /// GetBanks
        /// Obtiene bancos
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBanks(string query)
        {
            List<object> bankResponses = new List<object>();

            if (query != null)
            {
                int length = query.Length;
                List<Bank> banks = DelegateService.commonService.GetBanks();
                foreach (Bank bank in banks)
                {
                    if ((length <= bank.Description.Length) && (((bank.Description).IndexOf(query.ToUpper())) > -1))
                    {
                        bankResponses.Add(new
                        {
                            Id = bank.Id,
                            Value = bank.Description
                        });
                    }
                }
            }

            return Json(bankResponses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Bill

        /// <summary>
        /// SaveReceiptRequest
        /// Graba un recibo de caja
        /// </summary>
        /// <param name="collectImputation"></param>
        /// <param name="billControlId"></param>
        /// <returns>CollectApplicationDTO</returns>        
        public CollectApplicationDTO SaveReceiptRequest(CollectApplicationDTO collectImputation, int billControlId)
        {
            //CollectDTO collect = collectImputation.Collect;
            //GC.GetGeneration(collect);
            //long request = GC.GetTotalMemory(false); //Recupera el número de bytes que se considera que están asignados en la actualidad.

            //Objetos mayores a 14191396 bytes puede generar que se caiga la aplicación por el FWK

            return DelegateService.accountingCollectService.SaveCollectImputation(collectImputation, billControlId, true);
        }


        #endregion

        #region BillingClosure

        /// <summary>
        /// BillingClosure
        /// Invoca a la vista BillingClosure
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BillingClosure()
        {
            try
            {

                int defaultValue = Convert.ToInt16(@Global.DefaultValue);
                string defaultDescription = @Global.DefaultDescription;

                int percentageQuotaParameter = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumPercentageQuota"]);
                List<Branch> branchs = _commonController.GetBranchesByUserId(_commonController.GetUserIdByName(User.Identity.Name));

                branchs.Insert(0, new Branch { Id = defaultValue, Description = defaultDescription });
                ViewBag.Branch = branchs;

                ViewBag.userNick = _commonController.GetUserByName(User.Identity.Name)[0].AccountName;
                ViewBag.UserId = _commonController.GetUserByName(User.Identity.Name)[0].UserId;

                List<Currency> currencies = DelegateService.commonService.GetCurrencies();
                ViewBag.Currency = currencies;

                // Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);
                ViewBag.idBillControl = TempData["BillControlId"];
                ViewBag.idBranch = TempData["BranchId"];

                //Recuper el porcentaje mínimo para pago de Cuotas
                double percentage = GetPercentageForPayQuota(percentageQuotaParameter);
                ViewBag.PercentageParameter = percentage;

                int localCurrencyId = 0;
                ViewBag.localCurrencyId = localCurrencyId;

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// BillingClosureId
        /// Pasa variables de una vista a otra
        /// </summary>
        /// <param name="billControlId"></param>
        /// <param name="branchId"></param>
        /// <returns>RedirectResult</returns>
        public RedirectResult BillingClosureId(int billControlId, int branchId)
        {
            TempData["BillControlId"] = billControlId;
            TempData["BranchId"] = branchId;

            string language = @Global.DailyCashClosingLink;

            return Redirect(language);
        }

        /// <summary>
        /// LoadBilling
        /// </summary>
        /// <param name="preliquidationId"></param>
        /// <param name="branchId"></param>
        /// <param name="branchDescription"></param>
        /// <param name="documentNumber"></param>
        /// <param name="name"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="description"></param>
        /// <param name="totalAmount"></param>
        /// <param name="individualId"></param>
        /// <param name="preliquidationBranch"></param>
        /// <param name="personTypeId"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadBilling(string preliquidationId, string branchId, string branchDescription,
                                                 string documentNumber, string name, string tempImputationId,
                                                 string description, decimal totalAmount, string individualId, int preliquidationBranch, int personTypeId)
        {
            TempData["IsApply"] = 1; //flag para indicar que es aplicación
            TempData["PreliquidationId"] = preliquidationId;
            TempData["BranchId"] = branchId;
            TempData["BranchDescription"] = branchDescription;
            TempData["DocumentNumber"] = documentNumber;
            TempData["Name"] = name;
            TempData["TempImputationId"] = tempImputationId;
            TempData["Description"] = description;
            TempData["TotalAmount"] = totalAmount;
            TempData["IndividualId"] = individualId;
            TempData["PreliquidationBranch"] = preliquidationBranch;
            TempData["PreliquidationPersonType"] = personTypeId;

            return RedirectToAction("MainBilling");
        }

        /// <summary>
        /// GenerateCollectItemsExcel
        /// Genera y Descarga en formato Excel los ingresos de Caja que no esten asignados a una boleta interna
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GenerateCollectItemsExcel(int collectControlId)
        {
            MemoryStream memoryStream = new MemoryStream();
            List<SEARCH.CollectItemWithoutPaymentTicketDTO> collectItemWithoutPaymentTickets = DelegateService.accountingCollectService.GetCollectItemsWithoutPaymentTicket(collectControlId);
            if (collectItemWithoutPaymentTickets.Count > 0)
            {

                try
                {
                    memoryStream = ExportCollectItem(collectItemWithoutPaymentTickets);
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + UploadReport(memoryStream.ToArray(), "IngresoDeCajaNoAsingnadoBoletaIn.xls"));
                }
                catch (Exception e)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportExcel);
                }
            }
            else
            {
                return new UifJsonResult(false, Global.NoRecordsFound);
            }
        }

        private string UploadReport(byte[] byteArray, string fileName)
        {
            string path = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + this.User.Identity.Name + @"\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += Guid.NewGuid() + Path.GetExtension(fileName);
                System.IO.File.WriteAllBytes(path, byteArray);

                return path;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                throw ex;
            }
        }

        #endregion

        #region CollectControl

        /// <summary>
        /// AllowOpenBill
        /// Permite aperturar la caja
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AllowOpenBill(int branchId, string accountingDate)
        {
            List<object> allowOpenCollectResponses = new List<object>();
            try
            {

                if (accountingDate == null)
                {
                    DateTime accountingDatePresent = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                    accountingDate = accountingDatePresent.ToString("dd/MM/yyyy");
                }

                int status = Convert.ToInt32(CollectControlStatus.Open);

                bool isAllowOpenCollect = DelegateService.accountingCollectControlService.AllowOpenCollect(
                                                        _commonController.GetUserIdByName(User.Identity.Name),
                                                        branchId, Convert.ToDateTime(accountingDate), status);

                allowOpenCollectResponses.Add(new
                {
                    resp = isAllowOpenCollect
                });
                return Json(allowOpenCollectResponses, JsonRequestBehavior.AllowGet);

            }
            catch (UnhandledException)
            {

                allowOpenCollectResponses.Add(new
                {
                    resp = false
                });
                return Json(allowOpenCollectResponses, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// NeedCloseBill
        /// Consulta si es necesario cerrar la Caja
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingDatePresent"></param>
        /// <returns>JsonResult</returns>
        public JsonResult NeedCloseBill(int? branchId, string accountingDatePresent)
        {
            try
            {
                if (accountingDatePresent == null)
                {
                    DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                    accountingDatePresent = accountingDate.ToString("dd/MM/yyyy");
                }

                int status = Convert.ToInt32(CollectControlStatus.Open);

                CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(
                                     _commonController.GetUserIdByName(User.Identity.Name),
                                    Convert.ToInt32(branchId), Convert.ToDateTime(accountingDatePresent).Date, status);

                List<object> collectControls = new List<object>();
                collectControls.Add(new
                {
                    resp = collectControl.Status,
                    Id = collectControl.Id,
                    success = true,
                    openDate = collectControl.OpenDate.ToString("dd/MM/yyyy"),
                    accountingDate = collectControl.AccountingDate.ToString("dd/MM/yyyy")
                });

                return Json(collectControls, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SaveBillControl
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBillControl(int branchId, DateTime accountingDate)
        {
            CollectControlDTO collectControl = new CollectControlDTO();

            try
            {
                collectControl.Branch = new SEARCH.BranchDTO();
                collectControl.Branch.Id = branchId;
                collectControl.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                collectControl.AccountingDate = accountingDate.Date;
                collectControl.OpenDate = DateTime.Now;
                collectControl.Status = Convert.ToInt32(CollectControlStatus.Open);

                collectControl = DelegateService.accountingCollectControlService.SaveCollectControl(collectControl);
                List<object> collectControls = new List<object>();

                DateTime openDate = DelegateService.accountingCollectControlService.GetLastOpenDateByUserIdBranchId(collectControl.UserId, branchId);
                collectControls.Add(new
                {
                    resp = collectControl.Status,
                    Id = collectControl.Id,
                    success = true,
                    openDate = openDate.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat),
                    accountingDate = String.Format("{0:MM/dd/yyyy hh:mm:ss }", collectControl.AccountingDate)
                });
                return Json(new
                {
                    success = true,
                    result = collectControls
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = unhandledException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// CloseBillControl
        /// Cambia el estado a cerrado de la Caja
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="billControlId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CloseBillControl(int billControlId, int branchId)
        {
            try
            {
                CollectControlDTO collectControl = new CollectControlDTO();

                collectControl.Branch = new SEARCH.BranchDTO
                {
                    Id = branchId
                };
                collectControl.UserId = _commonController.GetUserIdByName(User.Identity.Name); ;
                collectControl.CloseDate = DateTime.Now;
                collectControl.Status = Convert.ToInt32(CollectControlStatus.Close);
                collectControl.Id = billControlId;

                DelegateService.accountingCollectControlService.CloseCollectControl(collectControl);

                return Json(collectControl, JsonRequestBehavior.AllowGet);
            }
            // No se asigna una variable a la excepción ya que el método CloseCollectControl no devuelve nada es void
            catch (UnhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region CollectRequest

        /// <summary>
        /// SaveBillRequest
        /// </summary>
        /// <param name="frmBill"></param>
        /// <param name="itemsToPayGridModel"></param>
        /// <param name="branchId"></param>
        /// <param name="preliquidationBranch"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBillRequest(BillModel frmBill, ItemsToPayGridModel itemsToPayGridModel, int branchId, int preliquidationBranch, int tempImputationId = 0)
        {
            try
            {
                CollectApplicationDTO collectImputation = new CollectApplicationDTO();

                int billControlId = frmBill.BillControlId;
                int number = 0;
                bool showImputationMessage = true; //Se aumenta este flag para mostrar mensaje de contabilidad en EE
                bool validateComponents = false;   //Indica si una póliza tiene sus componentes registrados en el sistema.
                int invalidRecords = 0;
                int imputationTypeId = 0; //parámetro para la contabilización de la imputación
                string Message = "";


                // Se obtiene parámetro de la BDD
                number = _commonController.GetBillNumber();

                // Se actualiza parámetro de número de carátula.
                _commonController.UpdateBillNumber(number);

                CollectDTO collect = new CollectDTO();

                CollectConceptDTO billingConcept = new CollectConceptDTO();
                billingConcept.Id = frmBill.BillingConceptId;

                AmountDTO paymentsTotal = new AmountDTO();
                paymentsTotal.Value = frmBill.PaymentsTotal;

                PersonDTO payer = new PersonDTO()
                {
                    IndividualId = frmBill.PayerId,

                    IdentificationDocument = new IdentificationDocumentDTO()
                    {
                        Number = frmBill.PayerDocumentNumber,
                        DocumentType = new DocumentTypeDTO { Id = frmBill.PayerDocumentTypeId }
                    },
                    Name = frmBill.PayerName == null ? null : frmBill.PayerName.ToUpper(),
                    PersonType = new PersonTypeDTO() { Id = frmBill.PayerTypeId }
                };

                int statusId = Convert.ToInt16(CollectStatus.Active);
                int status = Convert.ToInt32(CollectControlStatus.Open);

                if (frmBill.SourcePaymentId > 0)
                {
                    statusId = Convert.ToInt16(CollectStatus.Applied);
                }

                SEARCH.BranchDTO branch = new SEARCH.BranchDTO() { Id = branchId };

                int userId = 0;

                if (User != null)
                {
                    userId = _commonController.GetUserIdByName(User.Identity.Name);
                }
                else
                {
                    // Viene de una generación masiva de recibos
                    userId = frmBill.UserId;
                }

                CompanyDTO accountingCompany = new CompanyDTO()
                {
                    IndividualId = -1 // Quemado por el momento.
                };

                collect.Description = frmBill.Description;

                CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(userId, Convert.ToInt32(branchId), Convert.ToDateTime(DateTime.Now).Date, status);
                collect.Date = collectControl != null ? collectControl.AccountingDate : DateTime.MinValue;
                collect.Concept = billingConcept;
                collect.PaymentsTotal = paymentsTotal;
                collect.Payer = payer;
                collect.Status = statusId;
                collect.Number = number;
                collect.CollectType = (int)CollectTypes.Incoming;
                collect.UserId = userId;
                collect.AccountingCompany = accountingCompany;
                collect.Branch = new SEARCH.BranchDTO();
                collect.Branch.Id = branch.Id;
                collect.CollectControlId = collectControl.Id;                

                collect.Payments = new List<PaymentDTO>();

                #region Payment

                if (frmBill.PaymentSummary != null)
                {
                    for (int j = 0; j < frmBill.PaymentSummary.Count; j++)
                    {

                        ACCMOD.Payments.PaymentMethodDTO paymentMethod = new ACCMOD.Payments.PaymentMethodDTO();
                        paymentMethod.Id = frmBill.PaymentSummary[j].PaymentMethodId;


                        #region PaymentMethodType

                        #region Cash

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = 0 },//asi este en dolares el pago se efectua en moneda local
                                Value = frmBill.PaymentSummary[j].Amount
                            };
                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = 1 };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };
                            collect.Payments.Add(new CashDTO()
                            {
                                PaymentMethod = paymentMethod,
                                Amount = amount,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region Check

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]) || paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"]) || paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDebit"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            BankDTO issuingBank = new BankDTO() { Id = frmBill.PaymentSummary[j].CheckPayments[0].IssuingBankId };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                            collect.Payments.Add(new CheckDTO()
                            {
                                PaymentMethod = paymentMethod,
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),
                                DocumentNumber = frmBill.PaymentSummary[j].CheckPayments[0].DocumentNumber,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                IssuerName = frmBill.PaymentSummary[j].CheckPayments[0].IssuerName,
                                IssuingAccountNumber = frmBill.PaymentSummary[j].CheckPayments[0].IssuingAccountNumber,
                                IssuingBank = issuingBank,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region CreditCard

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"]))
                        {
                            decimal taxBase = frmBill.PaymentSummary[j].CreditPayments[0].TaxBase;
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };
                            BankDTO issuingBank = new BankDTO() { Id = frmBill.PaymentSummary[j].CreditPayments[0].IssuingBankId };
                            CreditCardTypeDTO creditCardType = new CreditCardTypeDTO() { Id = frmBill.PaymentSummary[j].CreditPayments[0].CreditCardTypeId };
                            CreditCardValidThruDTO creditCardValidThru = new CreditCardValidThruDTO()
                            {
                                Month = frmBill.PaymentSummary[j].CreditPayments[0].ValidThruMonth,
                                Year = frmBill.PaymentSummary[j].CreditPayments[0].ValidThruYear
                            };

                            List<PaymentTaxDTO> paymentTaxs = DelegateService.accountingPaymentService.GetTaxCreditCard(creditCardType.Id, branchId).Taxes;

                            decimal ivaCardAmount = 0;
                            decimal tax = 0;
                            decimal retention = 0;

                            if (paymentTaxs != null)
                            {
                                for (int i = 0; i < paymentTaxs.Count; i++)
                                {
                                    if (paymentTaxs[i].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxCardIvaId"]))
                                    {
                                        ivaCardAmount = taxBase * paymentTaxs[i].Rate / 100;
                                    }
                                }

                                // Calcula la comisión                               
                                creditCardType.Commission = DelegateService.accountingParameterService.GetCreditCardType(creditCardType.Id).Commission * (Convert.ToDecimal(amount.Value) - ivaCardAmount) / 100;

                                // Asigna las bases del impuesto
                                for (int f = 0; f < paymentTaxs.Count; f++)
                                {
                                    paymentTaxs[f].TaxBase = new AmountDTO();
                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxCardIvaId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = taxBase;
                                    }

                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardIcaId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = (frmBill.PaymentSummary[j].LocalAmount - ivaCardAmount);
                                    }

                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardIvaId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = ivaCardAmount;
                                    }

                                    if (paymentTaxs[f].Tax.Id == Convert.ToInt32(ConfigurationManager.AppSettings["TaxRetentionCardSourceId"]))
                                    {
                                        paymentTaxs[f].TaxBase.Value = (frmBill.PaymentSummary[j].LocalAmount - ivaCardAmount);
                                    }
                                }

                                // Calcula el valor del impuesto total
                                for (int f = 0; f < paymentTaxs.Count; f++)
                                {
                                    /*TODO LFREIRE No existe campo en modelo en TaxService
                                    if (!paymentTaxs[f].Tax.IsRetention)
                                    {
                                        tax = tax + (paymentTaxs[f].TaxBase.Value * paymentTaxs[f].Rate / 100);
                                    }
                                    else
                                    {
                                        retention = retention + (paymentTaxs[f].TaxBase.Value * paymentTaxs[f].Rate / 100);
                                    }
                                    */
                                }
                            }

                            collect.Payments.Add(new CreditCardDTO()
                            {
                                Amount = amount,
                                AuthorizationNumber = frmBill.PaymentSummary[j].CreditPayments[0].AuthorizationNumber,
                                CardNumber = frmBill.PaymentSummary[j].CreditPayments[0].CardNumber,
                                Holder = frmBill.PaymentSummary[j].CreditPayments[0].Holder,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                IssuingBank = issuingBank,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                Type = creditCardType,
                                ValidThru = creditCardValidThru,
                                Voucher = frmBill.PaymentSummary[j].CreditPayments[0].Voucher,
                                Status = Convert.ToInt16(PaymentStatus.Active),
                                Taxes = paymentTaxs,
                                Tax = Convert.ToDecimal(tax),
                                Retention = Convert.ToDecimal(retention)
                            });
                        }

                        #endregion

                        #region Transfer

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"]) ||
                            paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPaymentArea"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };
                            BankDTO issuingBank = new BankDTO() { Id = frmBill.PaymentSummary[j].TransferPayments[0].IssuingBankId };
                            BankAccountPersonDTO receivingAccount = new BankAccountPersonDTO()
                            {
                                Bank = new BankDTO() { Id = frmBill.PaymentSummary[j].TransferPayments[0].ReceivingBankId },
                                Number = frmBill.PaymentSummary[j].TransferPayments[0].ReceivingAccountNumber
                            };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };

                            collect.Payments.Add(new TransferDTO()
                            {
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),//frmBill.PaymentSummary[j].TransferPayments[0].Date,
                                DocumentNumber = frmBill.PaymentSummary[j].TransferPayments[0].DocumentNumber,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                IssuerName = frmBill.PaymentSummary[j].TransferPayments[0].IssuerName,
                                IssuingAccountNumber = frmBill.PaymentSummary[j].TransferPayments[0].IssuingAccountNumber,
                                IssuingBank = issuingBank,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                ReceivingAccount = receivingAccount,
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region Deposit

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            AmountDTO localAmount = new AmountDTO();
                            if (frmBill.PaymentSummary[j].LocalAmount.Equals(0))
                            {
                                localAmount.Value = frmBill.PaymentsTotal;
                            }
                            else
                            {
                                localAmount.Value = frmBill.PaymentSummary[j].LocalAmount;
                            }

                            BankAccountCompanyDTO receivingAccount = new BankAccountCompanyDTO()
                            {
                                Bank = new BankDTO() { Id = frmBill.PaymentSummary[j].DepositVouchers[0].ReceivingAccountBankId },
                                Number = frmBill.PaymentSummary[j].DepositVouchers[0].ReceivingAccountNumber
                            };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            localAmount.Value = frmBill.PaymentSummary[j].LocalAmount;

                            collect.Payments.Add(new DepositVoucherDTO()
                            {
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),
                                DepositorName = frmBill.PaymentSummary[j].DepositVouchers[0].DepositorName,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                ReceivingAccount = receivingAccount,
                                VoucherNumber = frmBill.PaymentSummary[j].DepositVouchers[0].VoucherNumber,
                                Status = Convert.ToInt16(PaymentStatus.Active)
                            });
                        }

                        #endregion

                        #region ConsignmentCheck

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentConsignmentCheck"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };

                            AmountDTO localAmount = new AmountDTO();
                            if (frmBill.PaymentSummary[j].LocalAmount.Equals(0))
                            {
                                localAmount.Value = frmBill.PaymentsTotal;
                            }
                            else
                            {
                                localAmount.Value = frmBill.PaymentSummary[j].LocalAmount;
                            }
                            BankAccountCompanyDTO receivingAccount = new BankAccountCompanyDTO
                            {
                                AccountingAccount = new SEARCH.AccountingAccountDTO
                                {
                                    AccountingNumber = frmBill.PaymentSummary[j].ConsignmentChecks[0].ReceivingAccountNumber,
                                    Number = frmBill.PaymentSummary[j].ConsignmentChecks[0].ReceivingAccountNumber,
                                    AccountingAccountId = frmBill.PaymentSummary[j].ConsignmentChecks[0].BankAccountingAccountId
                                },
                                Bank = new BankDTO
                                {
                                    Id = frmBill.PaymentSummary[j].ConsignmentChecks[0].ReceivingAccountBankId,
                                    Description = frmBill.PaymentSummary[j].ConsignmentChecks[0].ReceivingAccountBankDescription
                                }
                            };


                            BankDTO issuingCheckBank = new BankDTO() { Id = frmBill.PaymentSummary[j].ConsignmentChecks[0].IssuingBankId };

                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };
                            localAmount.Value = frmBill.PaymentSummary[j].LocalAmount;

                            collect.Payments.Add(new ConsignmentCheckDTO()
                            {
                                Amount = amount,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                ExchangeRate = exchangeRate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                Status = Convert.ToInt16(PaymentStatus.Active),
                                DocumentNumber = frmBill.PaymentSummary[j].ConsignmentChecks[0].DocumentNumber,
                                IssuerName = frmBill.PaymentSummary[j].ConsignmentChecks[0].IssuerName,
                                IssuingAccountNumber = frmBill.PaymentSummary[j].ConsignmentChecks[0].IssuingAccountNumber,
                                IssuingBank = issuingCheckBank,
                                ReceivingAccount = receivingAccount
                            });
                        }

                        #endregion ConsignmentCheck

                        #region Retention

                        if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"]))
                        {
                            AmountDTO amount = new AmountDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = frmBill.PaymentSummary[j].CurrencyId },
                                Value = frmBill.PaymentSummary[j].Amount
                            };
                            AmountDTO localAmount = new AmountDTO() { Value = frmBill.PaymentSummary[j].LocalAmount };
                            ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = frmBill.PaymentSummary[j].ExchangeRate };

                            PolicyDTO policy = new PolicyDTO()
                            {
                                Branch = new SEARCH.BranchDTO() { Id = frmBill.PaymentSummary[j].BranchId },
                                DefaultBeneficiaries = new List<BeneficiaryDTO>()
                                {
                                    new BeneficiaryDTO()
                                    {
                                        CustomerType = (int)CustomerType.Individual,
                                        IndividualId = frmBill.PayerId
                                    }
                                },
                                DocumentNumber = Convert.ToInt32(frmBill.PaymentSummary[j].RetentionReceipts[0].PolicyNumber),
                                Endorsement = new ACCMOD.EndorsementDTO() { Number = frmBill.PaymentSummary[j].RetentionReceipts[0].EndorsementNumber },
                                Holder = new HolderDTO()
                                {
                                    CustomerType = (int)CustomerType.Individual,
                                    IndividualId = frmBill.PayerId,
                                    IndividualType = (int)IndividualType.Person,
                                    InsuredId = frmBill.PayerTypeId,
                                },
                                Id = Convert.ToInt32(frmBill.PaymentSummary[j].RetentionReceipts[0].SerialNumber), //policyId
                                IssueDate = frmBill.PaymentSummary[j].RetentionReceipts[0].IssueDate,
                                Prefix = new PrefixDTO() { Id = frmBill.PaymentSummary[j].PrefixId },
                                UserId = userId
                            };

                            collect.Payments.Add(new RetentionReceiptDTO()
                            {
                                Amount = amount,
                                AuthorizationNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].AuthorizationNumber,
                                BillNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].BillNumber,
                                Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),// frmBill.PaymentSummary[j].RetentionReceipts[0].Date,
                                ExchangeRate = exchangeRate,
                                ExpirationDate = frmBill.PaymentSummary[j].RetentionReceipts[0].ExpirationDate,
                                Id = frmBill.PaymentSummary[j].PaymentId,
                                InvoiceDate = frmBill.PaymentSummary[j].RetentionReceipts[0].InvoiceDate,
                                IssueDate = frmBill.PaymentSummary[j].RetentionReceipts[0].IssueDate,
                                LocalAmount = localAmount,
                                PaymentMethod = paymentMethod,
                                Policy = policy,
                                RetentionConcept = new RetentionConceptDTO() { Id = frmBill.PaymentSummary[j].RetentionReceipts[0].RetentionConceptId },
                                SerialNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].SerialNumber,
                                SerialVoucherNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].SerialVoucherNumber,
                                Status = Convert.ToInt16(PaymentStatus.Active),
                                VoucherNumber = frmBill.PaymentSummary[j].RetentionReceipts[0].VoucherNumber,
                            });
                        }

                        #endregion

                        #endregion
                    }
                }

                #endregion

                #region Imputation

                ApplicationDTO imputation = new ApplicationDTO();

                if (itemsToPayGridModel.BillItem.Count > 0)
                {
                    billControlId = collect.CollectControlId;
                    collect.Concept = new CollectConceptDTO() { Id = 1 };
                    ParamApplicationPremiumComponent paramApplicationPremiumComponent = new ParamApplicationPremiumComponent();
                    paramApplicationPremiumComponent = CreateApplicationCollect(itemsToPayGridModel.BillItem.FirstOrDefault());
                    imputationTypeId = Convert.ToInt32(ApplicationTypes.Collect);
                    if (frmBill.SourcePaymentId > 0)
                    {
                        imputationTypeId = Convert.ToInt32(ApplicationTypes.PreLiquidation);
                    }
                    PremiumReceivableTransactionDTO premiumReceivableTransaction = new PremiumReceivableTransactionDTO();
                    premiumReceivableTransaction.Id = 0;
                    premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();
                    ACCMOD.Application.PremiumRequestDTO premiumRequest = new ACCMOD.Application.PremiumRequestDTO();
                    List<ACCMOD.Application.PremiumRequestDTO> ListPremiumRequest = new List<ACCMOD.Application.PremiumRequestDTO>();
                    premiumRequest.UserId = userId;
                    premiumRequest.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                    
                    premiumRequest.PremiumReceivableTransaction = new PremiumReceivableTransactionDTO();
                    if (itemsToPayGridModel.BillItem != null)
                    {
                        for (int k = 0; k < itemsToPayGridModel.BillItem.Count; k++)
                        {
                            var quotaNum = DelegateService.underwritingIntegrationService.GetPaymentQuota(new Integration.UndewritingIntegrationServices.DTOs.FilterBaseDTO { Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column2) });
                            PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem = new PremiumReceivableTransactionItemDTO();
                            premiumReceivableTransactionItem.Policy = new PolicyDTO();
                            premiumReceivableTransactionItem.Id = 0;
                            premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<BeneficiaryDTO>()
                                {
                                    new BeneficiaryDTO{
                                    CustomerType = Convert.ToInt32(CustomerType.Individual),
                                    IndividualId = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column4)
                                }
                            };
                            premiumReceivableTransactionItem.Policy.Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column1);
                            premiumReceivableTransactionItem.Policy.Endorsement = new ACCMOD.EndorsementDTO() { Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column2) };
                            premiumReceivableTransactionItem.Policy.ExchangeRate = new ExchangeRateDTO()
                            {
                                Currency = new SEARCH.CurrencyDTO() { Id = Convert.ToInt32(itemsToPayGridModel.BillItem[k].CurrencyId) },
                                SellAmount = itemsToPayGridModel.BillItem[k].ExchangeRate
                            };
                            premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponentDTO>()
                            {
                                new PayerComponentDTO()
                                {
                                    Amount = itemsToPayGridModel.BillItem[k].Amount,
                                    BaseAmount = itemsToPayGridModel.BillItem[k].PaidAmount
                                    }
                                 };
                            premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlanDTO()
                            {
                                Quotas = new List<QuotaDTO>()
                                    {
                                    new QuotaDTO { Number = Convert.ToInt32(Convert.ToInt32(quotaNum)) }
                                    }
                            };


                            premiumReceivableTransactionItem.DeductCommission = new AmountDTO();
                            premiumReceivableTransactionItem.DeductCommission.Value = 0; //no se graba comisiones
                            premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);

                            //valido que las primas a aplicar tienen registrados sus componentes en el sistema.
                            if (!DelegateService.accountingApplicationService.ValidatePolicyComponents(Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column1), Convert.ToInt32(itemsToPayGridModel.BillItem[k].Column2)))
                            {
                                premiumRequest.PremiumReceivableTransaction = null;
                                var accountResult = new MainApplicationReceipt
                                {
                                    Message = Global.ComponentNotValid,
                                    TechnicalTransaction = "0",
                                };                                
                                invalidRecords++;
                                // Alguna de las pólizas no poseen todos sus componentes.
                                return Json(accountResult, JsonRequestBehavior.AllowGet);
                            }
                            else {
                                premiumRequest.ExchangeRate = itemsToPayGridModel.BillItem[k].ExchangeRate;
                                premiumRequest.PremiumReceivableTransaction = premiumReceivableTransaction;
                            }
                        }
                    }
                    PaymentOrderDTO application = null;

                    if (premiumRequest.PremiumReceivableTransaction != null)
                    {
                        application = DelegateService.accountingApplicationService.SaveTempApplicationData(premiumRequest);
                    }
                    if (application.Imputation != null)
                    {
                        collectImputation.Application = application.Imputation;
                    } else {
                        var accountResult = new MainApplicationReceipt
                        {
                            Message = application.Observation,
                            TechnicalTransaction = "0",
                        };
                        invalidRecords++;
                        // Alguna de las pólizas no poseen todos sus componentes.
                        return Json(accountResult, JsonRequestBehavior.AllowGet);
                    }

                    if (invalidRecords > 0)
                    {
                        validateComponents = false;
                    }
                    else
                    {
                        validateComponents = true;
                    }
                }
                else
                {
                    validateComponents = true;
                    imputation.Id = tempImputationId;
                    ApplicationDTO applicationTransaction = DelegateService.accountingApplicationService.GetTempApplication(imputation);
                    if (applicationTransaction != null && applicationTransaction.Id > 0)
                    {
                        collectImputation.Application = applicationTransaction;
                    }
                }
                #endregion


                if (validateComponents)
                {
                    // Para aplicación de preliquidación. 
                    if (frmBill.SourcePaymentId > 0)
                    {
                        // Recibo ya aplicado se borra par evitar conflictos en grabación.
                        collectImputation.Application = null;
                    }

                    // Grabo bill

                    collectImputation.Id = frmBill.SourcePaymentId;
                    collectImputation.Collect = collect;

                    CollectGeneralLedgerDTO collectGeneralLedgerDTO = new CollectGeneralLedgerDTO();
                    List<PaymentSummaryDTO> paymentSummaryDTO = new List<PaymentSummaryDTO>();

                    collectGeneralLedgerDTO.Bill = new BillDTO
                    {
                        BillControlId = frmBill.BillControlId,
                        BillId = frmBill.BillId,
                        BillingConceptId = frmBill.BillingConceptId,
                        Description = frmBill.Description,
                        PayerDocumentNumber = frmBill.PayerDocumentNumber,
                        PayerDocumentTypeId = frmBill.PayerDocumentTypeId,
                        PayerId = frmBill.PayerId,
                        PayerName = frmBill.PayerName,
                        PayerTypeId = frmBill.PayerTypeId,
                        PaymentsTotal = frmBill.PaymentsTotal,
                        RegisterDate = frmBill.RegisterDate,
                        SourcePaymentId = frmBill.SourcePaymentId,
                        UserId = userId,

                    };
                    collectGeneralLedgerDTO.Bill.PaymentSummary = new List<PaymentSummaryDTO>();

                    foreach (var paymentModel in frmBill.PaymentSummary)
                    {
                        var paymentDTO = new PaymentSummaryDTO()
                        {
                            Amount = paymentModel.Amount,
                            BillId = paymentModel.BillId,
                            CurrencyId = paymentModel.CurrencyId,
                            ExchangeRate = paymentModel.ExchangeRate,
                            LocalAmount = paymentModel.LocalAmount,
                            PaymentId = paymentModel.PaymentId,
                            PaymentMethodId = paymentModel.PaymentMethodId,
                            ConsignmentChecks = new List<ConsignmentCheckDTO>()
                        };

                        if (paymentModel.ConsignmentChecks != null)
                        {
                            foreach (var consignmentChecks in paymentModel.ConsignmentChecks)
                            {
                                var consignmentChecksDTO = new ConsignmentCheckDTO()
                                {
                                    CheckDate = consignmentChecks.CheckDate,
                                    Date = consignmentChecks.Date,
                                    DepositorName = consignmentChecks.DepositorName,
                                    DocumentNumber = consignmentChecks.DocumentNumber,
                                    VoucherNumber = consignmentChecks.VoucherNumber,
                                    IssuingBank = new BankDTO
                                    {
                                        Id = consignmentChecks.IssuingBankId,

                                    },
                                    IssuingAccountNumber = consignmentChecks.IssuingAccountNumber,
                                    IssuerName = consignmentChecks.IssuerName,
                                    ReceivingAccount = new BankAccountCompanyDTO
                                    {
                                        AccountingAccount = new SEARCH.AccountingAccountDTO
                                        {
                                            AccountingNumber = consignmentChecks.ReceivingAccountNumber,
                                            Number = consignmentChecks.ReceivingAccountNumber,
                                            AccountingAccountId = consignmentChecks.BankAccountingAccountId
                                        },
                                        Bank = new BankDTO
                                        {
                                            Id = consignmentChecks.ReceivingAccountBankId,
                                            Description = consignmentChecks.ReceivingAccountBankDescription
                                        }
                                    },
                                    PaymentMethod = new PaymentMethodDTO
                                    {
                                        Id = paymentModel.PaymentMethodId
                                    }
                                };

                                paymentDTO.ConsignmentChecks.Add(consignmentChecksDTO);
                            }

                        }

                        collectGeneralLedgerDTO.Bill.PaymentSummary.Add(paymentDTO);
                    }

                    collectGeneralLedgerDTO.CollectImputation = collectImputation;
                    collectGeneralLedgerDTO.BillControlId = billControlId;
                    collectGeneralLedgerDTO.PreliquidationBranch = preliquidationBranch;
                    collectGeneralLedgerDTO.StatusId = statusId;
                    collectGeneralLedgerDTO.UserId = userId;
                    collectGeneralLedgerDTO.CollectImputation.Transaction = new TransactionDTO();

                    MessageSuccessDTO messageSuccessDTO = DelegateService.accountingAccountService.SaveCollectGeneralLedgerPayment(collectGeneralLedgerDTO);


                    var accountResult = new MainApplicationReceipt
                    {
                        Message = messageSuccessDTO.RecordCollectMessage,
                        //BillId = Convert.ToString(collectImputation.Collect.Id),
                        BillId = Convert.ToString(messageSuccessDTO.BillId),
                        AccountingCompanyId = Convert.ToString(collectImputation.Collect.AccountingCompany.IndividualId),
                        BillingConceptId = Convert.ToString(collectImputation.Collect.Concept.Id),
                        Date = Convert.ToString(collectImputation.Collect.Date),
                        Description = Convert.ToString(collectImputation.Collect.Description),
                        IsTemporal = Convert.ToString(collectImputation.Collect.IsTemporal),
                        Number = Convert.ToString(collectImputation.Collect.Number),
                        PayerIndividualId = Convert.ToString(collectImputation.Collect.Payer.IndividualId),
                        PaymentsTotal = Convert.ToString(collectImputation.Collect.PaymentsTotal.Value),
                        StatusId = Convert.ToString(collectImputation.Collect.Status),
                        UserId = Convert.ToString(collectImputation.Collect.UserId),
                        TechnicalTransaction = Convert.ToString(messageSuccessDTO.TechnicalTransaction),
                        ImputationMessage = messageSuccessDTO.ImputationMessage,
                        ShowMessage = Convert.ToString(messageSuccessDTO.ShowMessage),
                        ShowImputationMessage = Convert.ToString(showImputationMessage),
                        GeneralLedgerSuccess = messageSuccessDTO.GeneralLedgerSuccess
                        //Branch = Convert.ToString(collectGeneralLedgerDTO.CollectImputation.Collect.Branch.Id),
                    };

                    return Json(accountResult, JsonRequestBehavior.AllowGet);
                }
                // Alguna de las pólizas no poseen todos sus componentes.
                return Json(new
                {
                    success = false,
                    result = -2,
                    message = Message
                }, JsonRequestBehavior.AllowGet);

            }

            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SaveBillRequestReciptApplication
        /// </summary>
        /// <param name="frmBill"></param>
        /// <param name="itemsToPayGridModel"></param>
        /// <param name="branchId"></param>
        /// <param name="preliquidationBranch"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBillRequestReciptApplication(BillModel frmBill, AutomaticReceiptApplication automaticReceiptApplication, int branchId, int preliquidationBranch, int tempImputationId = 0)
        {
            try
            {
                ItemsToPayGridModel itemsToPayGridModel = new ItemsToPayGridModel();
                itemsToPayGridModel.BillItem = new List<BillItemModel>();
                decimal totalCreditPremiums = 0;
                decimal totalDebitPremiums = 0;
                decimal discountedCommisson = 0;
                decimal depositPremiums = 0;

                if (automaticReceiptApplication.premiumReceivable != null
                    && automaticReceiptApplication.premiumReceivable.PremiumReceivableItems != null
                    && automaticReceiptApplication.premiumReceivable.PremiumReceivableItems.Count > 0)
                {
                    if (automaticReceiptApplication.premiumReceivable.PremiumReceivableItems != null)
                    {
                        foreach (var item in automaticReceiptApplication.premiumReceivable.PremiumReceivableItems)
                        {
                            itemsToPayGridModel.BillItem.Add(new BillItemModel()
                            {
                                Amount = item.Amount,
                                BillId = frmBill.BillId,
                                Description = frmBill.Description,
                                BillItemId = item.ItemId,
                                Column1 = Convert.ToString(item.PolicyId),
                                Column2 = Convert.ToString(item.EndorsementId),
                                Column3 = Convert.ToString(item.PaymentNum),
                                Column4 = Convert.ToString(item.PayerId),
                                Column5 = null,
                                Column6 = null,
                                Column7 = null,
                                CurrencyId = item.CurrencyCode,
                                ExchangeRate = item.ExchangeRate,
                                ItemTypeId = item.ItemId,
                                PaidAmount = item.Amount,
                            });

                            if (item.Amount >= 0)
                            {
                                totalCreditPremiums += item.Amount;
                            }
                            else
                            {
                                totalDebitPremiums += item.Amount;
                            }

                            discountedCommisson += item.DiscountedCommisson;
                        }
                    }
                }

                if (automaticReceiptApplication.usedDepositPremiumModel != null)
                {
                    if (automaticReceiptApplication.usedDepositPremiumModel.UsedAmounts != null)
                    {
                        foreach (var item in automaticReceiptApplication.usedDepositPremiumModel.UsedAmounts)
                        {
                            depositPremiums += item.Amount;
                        }
                    }
                }

                decimal totalAmountDebitAccountig = 0;
                decimal totalAmountCreditAccountig = 0;

                if (automaticReceiptApplication.accountingTransactionModel != null)
                {
                    foreach (var item in automaticReceiptApplication.accountingTransactionModel)
                    {
                        if (item.AccountingNature == 1)
                        {
                            totalAmountCreditAccountig += item.Amount;
                        }
                        else
                        {
                            totalAmountDebitAccountig += item.Amount;
                        };
                    }
                }

                var totalAmountReceipt = frmBill.PaymentsTotal;
                var totalDebit = totalDebitPremiums + totalAmountDebitAccountig + discountedCommisson + depositPremiums + totalAmountReceipt;
                var totalCredit = totalCreditPremiums + totalAmountCreditAccountig;
                var TotalAmountApplication = totalCredit - totalDebit;

                bool mustToApply = TotalAmountApplication == 0;
                try
                {
                    MainApplicationReceipt mainApplicationReceipt = ((MainApplicationReceipt)SaveBillRequest(frmBill, itemsToPayGridModel, branchId, preliquidationBranch, tempImputationId).Data);
                    if (mainApplicationReceipt != null)
                    {
                        var response = new
                        {
                            mainApplicationReceipt
                        };

                        return Json(new
                        {
                            success = true,
                            result = response
                        }, JsonRequestBehavior.AllowGet);
                        /*
                        int number = int.Parse(mainApplicationReceipt.BillId);
                        int imputationTypeId = Convert.ToInt32(Global.ImputationTypeBill);
                        int sourceCode = Convert.ToInt32(number);
                        int tempImputationId;

                        ReceiptApplicationController receiptApplicationController = new ReceiptApplicationController();
                        ApplicationDTO getTemporalImputation = ((ApplicationDTO)receiptApplicationController.GetTempImputationBySourceCode(imputationTypeId, sourceCode).Data);

                        if (getTemporalImputation.IsTemporal == false)
                        {
                            ApplicationDTO SaveTemporal = ((ApplicationDTO)receiptApplicationController.SaveTempImputation(imputationTypeId, sourceCode).Data);
                            tempImputationId = Convert.ToInt32(SaveTemporal.Id);
                        }
                        else
                        {
                            tempImputationId = Convert.ToInt32(getTemporalImputation.Id);
                        }

                        try
                        {
                            if (automaticReceiptApplication.premiumReceivable != null)
                            {
                                var premiumReceivable = automaticReceiptApplication.premiumReceivable;
                                premiumReceivable.ImputationId = tempImputationId;

                                PremiumsReceivableController premiumsReceivableController = new PremiumsReceivableController();
                                int saveTempPremiumReceivable = ((int)premiumsReceivableController.SaveTempPremiumReceivableRequest(premiumReceivable).Data);
                                if (saveTempPremiumReceivable > 0)
                                {
                                    AccountingController accountingController = new AccountingController();
                                    if (automaticReceiptApplication.accountingTransactionModel != null)
                                    {
                                        foreach (var item in automaticReceiptApplication.accountingTransactionModel)
                                        {
                                            item.TempImputationCode = tempImputationId;
                                            int saveTempAccountingTransaction = ((int)accountingController.SaveTempAccountingTransactionRequest(item).Data);
                                        }
                                    }
                                    string comments = frmBill.Description;
                                    int statusId = 3;
                                    if (TotalAmountApplication == 0)
                                    {
                                        var response = new
                                        {
                                            mainApplicationReceipt,
                                            //Metodo que realiza la aplicación de recibo
                                            //receiptApplication = receiptApplicationController.SaveReceiptApplication(sourceCode, tempImputationId, imputationTypeId, comments, statusId)
                                        };

                                        return Json(new
                                        {
                                            success = true,
                                            result = response
                                        }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            success = false,
                                            result = Resources.Global.Error
                                        }, JsonRequestBehavior.AllowGet);

                                    }
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        success = false,
                                        result = Resources.Global.Error
                                    }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    result = mainApplicationReceipt
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (BusinessException businessException)
                        {
                            return Json(new
                            {
                                success = false,
                                result = businessException.ExceptionMessages
                            }, JsonRequestBehavior.AllowGet);
                        }*/
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            result = Resources.Global.Error
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (BusinessException businessException)
                {
                    return Json(new
                    {
                        success = false,
                        result = businessException.ExceptionMessages
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(JsonRequestBehavior.AllowGet);
            }

            catch (UnhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// CancelBill 
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="billControlId"></param>
        /// <param name="authorizationUserId"></param>
        /// <returns>JsonResult</returns>
        /// 
        public JsonResult CancelBill(int billId, int billControlId, int authorizationUserId, string accountingDate)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            int canceled = 0;//technicaltrnsactioon del nuevo collect
            MessageDTO message = new MessageDTO();
            CollectDTO collect = new CollectDTO();
            try
            {
                canceled = DelegateService.accountingCollectService.CancelCollect(billId, billControlId, authorizationUserId);
                message.Success = true;
                message.Info = Global.BillingCancelBillSuccess + Global.AccountingTransactionNumberGenerated + " " + canceled;
                // Reverso el movimiento contable.
                if (canceled > 0)
                {
                    collect.Id = billId;
                    collect = DelegateService.accountingCollectService.GetCollectByCollectId(billId);

                    int accountingTransaction = ReverseBillEntry(billId, userId, Convert.ToDateTime(accountingDate), canceled);
                    collect.Transaction = new TransactionDTO() { TechnicalTransaction = accountingTransaction };
                    DelegateService.accountingCollectService.UpdateCollect(collect, billControlId);
                }
                else if (canceled == -1)
                {
                    message.Info = Global.BillingCancelBillWarning;
                    message.Success = false;
                }


            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException ex)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ExchangeRate

        /// <summary>
        /// GetCurrencyExchangeRate
        /// Obtiene el valor del cambio de la moneda
        /// </summary>
        /// <param name="rateDate">rateDate</param>
        /// <param name="currencyId">currencyId</param>
        /// <returns>decimal</returns>
        public decimal GetCurrencyExchangeRate(DateTime rateDate, int currencyId)
        {
            return DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId).SellAmount;
        }

        /// <summary>
        /// CalculateExchangeRateTolerance
        /// Obtener rango de tolerancia
        /// </summary>
        /// <param name="rate">rateDate</param>
        /// <param name="currencyId">currencyId</param>
        /// <returns>decimal</returns>
        public JsonResult CalculateExchangeRateTolerance(decimal newRate, int currencyId)
        {
            return Json(DelegateService.commonService.CalculateExchangeRateTolerance(newRate, currencyId), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// GetLatestCurrencyExchangeRate
        /// Obtiene el último cambio de la moneda
        /// </summary>
        /// <param name="rateDate">rateDate</param>
        /// <param name="currencyId">currencyId</param>
        /// <returns>decimal</returns>
        public decimal GetLatestCurrencyExchangeRate(DateTime rateDate, int currencyId)
        {
            return DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId).BuyAmount;
        }

        #endregion

        #region GeneralLedger

        /// <summary>
        /// Método de contabilización de ingreso de caja con parametrización dinámica
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="typeId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordBill(int billId, int typeId, int userId, int technicaltransaction)
        {
            string integrationMessage = "";

            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
            try
            {
                SaveBillParametersDTO saveBillParametersModel = new SaveBillParametersDTO()
                {
                    Collect = new CollectDTO
                    {
                        Id = billId,
                        Date = accountingDate,
                        Transaction = new TransactionDTO()
                        {
                            TechnicalTransaction = technicaltransaction
                        },
                    },
                    TypeId = typeId,
                    UserId = userId
                };
                int entryNumber = DelegateService.accountingAccountService.AccountingParametersRequest(saveBillParametersModel);

                if (entryNumber > 0)
                {
                    integrationMessage = Global.IntegrationSuccessMessage + " " + entryNumber;
                }
                if (entryNumber == 0)
                {
                    integrationMessage = Global.AccountingIntegrationUnbalanceEntry;
                }
                if (entryNumber == -2)
                {
                    integrationMessage = Global.EntryRecordingError;
                }

            }
            catch (Exception)
            {
                integrationMessage = Global.EntryRecordingError;
            }

            return integrationMessage;
        }

        /// <summary>
        /// Método de contabilización de ingreso de caja con parametrización dinámica
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="typeId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordBillCollect(CollectDTO collect, int typeId, int userId)
        {
            string integrationMessage = "";

            try
            {
                SaveBillParametersDTO saveBillParametersModel = new SaveBillParametersDTO()
                {
                    Collect = collect,
                    TypeId = typeId,
                    UserId = userId
                };

                int entryNumber = DelegateService.accountingAccountService.AccountingParametersRequest(saveBillParametersModel);

                if (entryNumber > 0)
                {
                    integrationMessage = Global.IntegrationSuccessMessage + " " + entryNumber;
                }
                if (entryNumber == 0)
                {
                    integrationMessage = Global.AccountingIntegrationUnbalanceEntry;
                }
                if (entryNumber == -2)
                {
                    integrationMessage = Global.EntryRecordingError;
                }

            }
            catch (Exception)
            {
                integrationMessage = Global.EntryRecordingError;
            }

            return integrationMessage;
        }

        /// <summary>
        /// Reversion de contabilidad mediante TechicalTransaction
        /// </summary>
        /// <param name="technicalTransaction"></param>
        /// <param name="technicalTransactionRevertion"></param>
        /// <returns></returns>
        public int RevertionJournalEntry(int technicalTransaction, int technicalTransactionRevertion)
        {
            int revertion = DelegateService.glAccountingApplicationService.GetJournalEntryTechnicalTransaction(technicalTransaction, technicalTransactionRevertion);

            return revertion;
        }

        /// <summary>
        /// RecordApplication
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordApplication(int billId, int userId)
        {
            string imputationMessage = "";
            int moduleDateId = 0;
            int moduleId = 0;
            int subModuleId = 0;

            try
            {
                List<SEARCH.ImputationParameterDTO> imputationParameters = DelegateService.accountingAccountService.GetImputationParameters(billId,
                                                                               Convert.ToInt32(ApplicationTypes.Collect), userId, moduleId, subModuleId, moduleDateId);
                imputationMessage = RecordImputation(imputationParameters, userId, 0);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return imputationMessage;
        }

        /// <summary>
        /// RecordPreliquidation
        /// </summary>
        /// <param name="billCd"></param>
        /// <param name="preLiquidationCd"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordPreliquidation(int billCd, int preLiquidationCd, int userId)
        {
            string imputationMessage = "";

            int moduleDateId = 0;
            int moduleId = preLiquidationCd;
            int subModuleId = 0;

            try
            {
                List<SEARCH.ImputationParameterDTO> imputationParameters = DelegateService.accountingAccountService.GetImputationParameters(billCd,
                                                                            Convert.ToInt32(ApplicationTypes.Collect), userId, moduleId, subModuleId, moduleDateId);
                imputationMessage = RecordImputation(imputationParameters, userId, 0);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return imputationMessage;
        }

        /// <summary>
        /// RecordImputation
        /// Método para contabilizar Imputación
        /// </summary>
        /// <param name="imputationParameters"></param>
        /// <param name="userId"></param>
        /// <param name="operation"></param>
        /// <returns>string</returns>
        public string RecordImputation(List<SEARCH.ImputationParameterDTO> imputationParameters, int userId, int operation)
        {
            string accountingDescription = "";
            string savedDailyEntryMessage = "";

            try
            {
                #region Parameters

                CollectApplicationDTO collectImputation = new CollectApplicationDTO();

                var originIdentifyId = Convert.ToInt32(imputationParameters[0].SourceCode);

                //Se generan las descripciones del asiento
                if (imputationParameters[0].ImputationTypeId == Convert.ToInt32(ConfigurationManager.AppSettings["ImputationTypeBill"]))
                {
                    accountingDescription = Global.AccountImputationBill + " " + originIdentifyId;
                }

                if (imputationParameters[0].ImputationTypeId == Convert.ToInt32(ConfigurationManager.AppSettings["ImputationTypeJournalEntry"]))
                {
                    accountingDescription = Global.AccountImputationJournalEntry + " " + originIdentifyId;
                }

                if (imputationParameters[0].ImputationTypeId == Convert.ToInt32(ConfigurationManager.AppSettings["ImputationTypePreliquidation"]))
                {
                    accountingDescription = Global.AccountImputationPreliquidation + " " + originIdentifyId;
                }

                if (imputationParameters[0].ImputationTypeId == Convert.ToInt32(ConfigurationManager.AppSettings["ImputationTypePaymentOrder"]))
                {
                    accountingDescription = Global.AccountImputationPaymentOrder + " " + originIdentifyId;
                }

                CollectApplicationDTO imputationOrigin = new CollectApplicationDTO();
                imputationOrigin.Collect = new CollectDTO() { Id = originIdentifyId };

                imputationOrigin.Application = new ApplicationDTO()
                {
                    ModuleId = imputationParameters[0].ImputationTypeId
                };

                List<CollectApplicationDTO> collectImputations = DelegateService.accountingCollectService.GetCollectImputations(imputationOrigin);
                int transactionNumber = collectImputations[0].Transaction.TechnicalTransaction;
                int moduleId = Convert.ToInt32(ConfigurationManager.AppSettings["ModuleCollectApplication"]);

                //Listado en donde se llevaran los grupos de parametros al servicio
                List<List<AccountingRuleModels.ParameterDTO>> parametersCollection = new List<List<AccountingRuleModels.ParameterDTO>>();

                #endregion Parameters

                #region DailyEntryHeader

                //Se arma cabecera de asiento 
                GeneralLedgerModels.JournalEntryDTO journalEntry = new GeneralLedgerModels.JournalEntryDTO();

                int accountingCompanyId = (from item in DelegateService.glAccountingApplicationService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

                journalEntry.AccountingCompany = new GeneralLedgerModels.AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId }; //este dato se graba con este valor en billing
                journalEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                journalEntry.AccountingMovementType = new GeneralLedgerModels.AccountingMovementTypeDTO(); //este dato solo pertenece a asientos de mayor
                journalEntry.Branch = new GeneralLedgerModels.BranchDTO() { Id = imputationParameters[0].BranchCode };
                journalEntry.Description = accountingDescription;
                journalEntry.EntryNumber = 0; //pendiente definición.
                journalEntry.Id = 0;
                journalEntry.ModuleDateId = moduleId;
                journalEntry.RegisterDate = DateTime.Now;
                journalEntry.SalePoint = new GeneralLedgerModels.SalePointDTO() { Id = 0 }; //no existe este dato en ingreso de caja
                journalEntry.Status = 1; //activo
                journalEntry.TechnicalTransaction = transactionNumber;
                journalEntry.UserId = userId;

                journalEntry.JournalEntryItems = new List<GeneralLedgerModels.JournalEntryItemDTO>();

                #endregion DailyEntryHeader

                #region PremiumReceivable

                #region PremiumReceivableParameters

                List<SEARCH.ImputationParameterDTO> premiumReceivableMovements = (from imputation in imputationParameters
                                                                                  where (imputation.MovementType == Convert.ToInt32(MovementTypes.PremiumReceivable) ||
                                                                                         imputation.MovementType == Convert.ToInt32(MovementTypes.DepositPremium) ||
                                                                                         imputation.MovementType == Convert.ToInt32(MovementTypes.DiscountedCommission))
                                                                                  select imputation).ToList();

                if (premiumReceivableMovements.Count > 0)
                {
                    foreach (var premiumReceivableItem in premiumReceivableMovements)
                    {
                        decimal primeAmount = 0;
                        decimal issuanceExpenses = 0;
                        decimal surcharges = 0;
                        decimal taxes = 0;
                        decimal bonuses = 0;
                        int prefixId = 0;
                        int businessTypeId = 0;
                        int branchId = 0;
                        int currencyId = 0;
                        int agentNumber = 1; //para pruebas
                        decimal orderAmount = 0;
                        decimal discountAmount = 0;
                        decimal interestAmount = 0;
                        decimal amount = 0;
                        int conceptCode = 0;
                        decimal taxInterestAmount = 0;
                        decimal discountedCommissionAmount = 0;
                        decimal depositPremiumAmount = 0;

                        if (premiumReceivableItem.Component == Convert.ToString(ConfigurationManager.AppSettings["Prime"])) //prima
                        {
                            primeAmount = premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.Component == Convert.ToString(ConfigurationManager.AppSettings["AdministrativeSurcharges"])) //recargos administrativos
                        {
                            surcharges = surcharges + premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.Component == Convert.ToString(ConfigurationManager.AppSettings["FinancialSurcharges"])) //recargos financieros
                        {
                            surcharges = surcharges + premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.Component == Convert.ToString(ConfigurationManager.AppSettings["IssuanceRights"])) //emisión
                        {
                            issuanceExpenses = premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.Component == Convert.ToString(ConfigurationManager.AppSettings["Taxes"])) //IVA
                        {
                            taxes = premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.Component == Convert.ToString(ConfigurationManager.AppSettings["Bonuses"])) //bonificación
                        {
                            bonuses = bonuses + premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.Component == null) //cuenta puente.
                        {
                            amount = amount + premiumReceivableItem.Amount;
                        }
                        if (premiumReceivableItem.MovementType == 2 /*Convert.ToInt32(MovementTypes.DepositPremium)*/) //primas en depósito
                        {
                            depositPremiumAmount = premiumReceivableItem.Amount;
                            amount = 0;
                        }
                        if (premiumReceivableItem.MovementType == 3 /*Convert.ToInt32(MovementTypes.DiscountedCommission)*/) //comisiones descontadas
                        {
                            discountedCommissionAmount = premiumReceivableItem.Amount;
                            amount = 0;
                        }

                        businessTypeId = Convert.ToInt32(premiumReceivableItem.BusinessTypeId);
                        prefixId = Convert.ToInt32(premiumReceivableItem.PrefixId);
                        currencyId = Convert.ToInt32(imputationParameters[0].CurrencyCode);
                        branchId = Convert.ToInt32(imputationParameters[0].BranchCode);

                        // Se arma la estructura de parámetros para su evaluación.
                        List<AccountingRuleModels.ParameterDTO> parameters = new List<AccountingRuleModels.ParameterDTO>();

                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(primeAmount, CultureInfo.InvariantCulture) }); //importe prima
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(prefixId, CultureInfo.InvariantCulture) }); //codigo de ramo
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(businessTypeId, CultureInfo.InvariantCulture) }); //codigo de operación
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(agentNumber, CultureInfo.InvariantCulture) }); //numero de agente
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(orderAmount, CultureInfo.InvariantCulture) }); //importe decreto
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(taxes, CultureInfo.InvariantCulture) }); //importe IVA
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(issuanceExpenses, CultureInfo.InvariantCulture) }); //importe derecho emisión
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(surcharges, CultureInfo.InvariantCulture) }); //importe recargos financieros
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(discountAmount, CultureInfo.InvariantCulture) }); //importe descuentos
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(interestAmount, CultureInfo.InvariantCulture) }); //importe intereses
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(branchId, CultureInfo.InvariantCulture) }); //código de sucursal
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(conceptCode, CultureInfo.InvariantCulture) }); //codigo de concepto
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(amount, CultureInfo.InvariantCulture) }); //valor total.
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(taxInterestAmount, CultureInfo.InvariantCulture) }); //valor total.
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(depositPremiumAmount, CultureInfo.InvariantCulture) }); //primas en depósito.
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(discountedCommissionAmount, CultureInfo.InvariantCulture) }); //comisiones descontadas.
                        parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(currencyId, CultureInfo.InvariantCulture) }); //comisiones descontadas.

                        parametersCollection.Add(parameters);

                        GeneralLedgerModels.ReconciliationMovementTypeDTO bankReconciliation = new GeneralLedgerModels.ReconciliationMovementTypeDTO();
                        GeneralLedgerModels.ReceiptDTO receipt = new GeneralLedgerModels.ReceiptDTO();

                        if (operation == 1) // Asignación manual y automática de cheques
                        {
                            bankReconciliation.Id = Convert.ToInt32(ConfigurationManager.AppSettings["BankReconciliationCheck"]);
                            receipt.Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                            receipt.Number = originIdentifyId;
                        }
                        if (operation == 2) // Transferencia
                        {
                            bankReconciliation.Id = Convert.ToInt32(ConfigurationManager.AppSettings["BankReconciliationDeposit"]);
                            receipt.Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                            receipt.Number = originIdentifyId;
                        }

                        //Items de asiento perteneciente a los movimientos de primas por cobrar, comisiones descontadas y primas en depósito.
                        GeneralLedgerModels.JournalEntryItemDTO journalEntryItem = new GeneralLedgerModels.JournalEntryItemDTO();

                        journalEntryItem.AccountingAccount = new GeneralLedgerModels.AccountingAccountDTO();
                        journalEntryItem.Amount = new GeneralLedgerModels.AmountDTO()
                        {
                            Currency = new GeneralLedgerModels.CurrencyDTO() { Id = currencyId }
                        };
                        decimal exchangeRate = 0;
                        exchangeRate = premiumReceivableItem.ExchangeRate;
                        journalEntryItem.ExchangeRate = new GeneralLedgerModels.ExchangeRateDTO() { SellAmount = exchangeRate };
                        journalEntryItem.Analysis = new List<GeneralLedgerModels.AnalysisDTO>();
                        journalEntryItem.ReconciliationMovementType = bankReconciliation;
                        journalEntryItem.CostCenters = new List<GeneralLedgerModels.CostCenterDTO>();
                        journalEntryItem.Currency = new GeneralLedgerModels.CurrencyDTO() { Id = currencyId };
                        journalEntryItem.Description = accountingDescription;
                        journalEntryItem.EntryType = new GeneralLedgerModels.EntryTypeDTO();
                        journalEntryItem.Id = 0;
                        journalEntryItem.Individual = new GeneralLedgerModels.IndividualDTO() { IndividualId = Convert.ToInt32(premiumReceivableMovements[0].PayerId) };
                        journalEntryItem.PostDated = new List<GeneralLedgerModels.PostDatedDTO>();
                        journalEntryItem.Receipt = receipt;

                        journalEntry.JournalEntryItems.Add(journalEntryItem);
                    }
                }

                #endregion PremiumReceivableParameters

                #endregion PremiumReceivable

                #region OtherMovements

                List<SEARCH.ImputationParameterDTO> otherMovements = imputationParameters.Except(premiumReceivableMovements).ToList();
                int row = 0;

                if (otherMovements.Count > 0)
                {
                    foreach (var parameter in otherMovements)
                    {
                        GeneralLedgerModels.ReconciliationMovementTypeDTO bankReconciliation = new GeneralLedgerModels.ReconciliationMovementTypeDTO();
                        GeneralLedgerModels.ReceiptDTO receipt = new GeneralLedgerModels.ReceiptDTO();

                        if (Convert.ToInt32(otherMovements[row].BankReconciliationId) > 0)
                        {
                            bankReconciliation.Id = Convert.ToInt32(otherMovements[row].BankReconciliationId);
                            receipt.Date = Convert.ToDateTime(otherMovements[row].ReceiptDate);
                            receipt.Number = Convert.ToInt32(otherMovements[row].ReceiptNumber);
                        }

                        if (operation == 1) // Asignación manual y automática de cheques
                        {
                            bankReconciliation.Id = Convert.ToInt32(ConfigurationManager.AppSettings["BankReconciliationCheck"]);
                            receipt.Date = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                            receipt.Number = originIdentifyId;
                        }

                        decimal incomeAmount = 0;
                        incomeAmount = parameter.IncomeAmount;
                        decimal exchangeRate = 0;
                        exchangeRate = parameter.ExchangeRate;
                        decimal localAmount = 0;
                        localAmount = parameter.Amount;
                        GeneralLedgerModels.AmountDTO accoutingAmount = new GeneralLedgerModels.AmountDTO()
                        {
                            Currency = new GeneralLedgerModels.CurrencyDTO() { Id = Convert.ToInt32(parameter.CurrencyCode) },
                            Value = System.Math.Abs(incomeAmount)
                        };
                        GeneralLedgerModels.IndividualDTO individual = new GeneralLedgerModels.IndividualDTO() { IndividualId = Convert.ToInt32(parameter.PayerId) };
                        GeneralLedgerModels.AccountingAccountDTO accountingAccount = new GeneralLedgerModels.AccountingAccountDTO() { AccountingAccountId = Convert.ToInt32(parameter.AccountingAccountId) };

                        //Items de asiento perteneciente al resto de movimientos de la imputación.
                        GeneralLedgerModels.JournalEntryItemDTO journalEntryItem = new GeneralLedgerModels.JournalEntryItemDTO();

                        journalEntryItem.AccountingAccount = accountingAccount;
                        journalEntryItem.AccountingNature = (int)(parameter.AccountingNature);
                        journalEntryItem.Amount = accoutingAmount;
                        journalEntryItem.ExchangeRate = new GeneralLedgerModels.ExchangeRateDTO() { SellAmount = exchangeRate };
                        journalEntryItem.LocalAmount = new GeneralLedgerModels.AmountDTO() { Value = System.Math.Abs(localAmount) };
                        journalEntryItem.Analysis = new List<GeneralLedgerModels.AnalysisDTO>(); //pendiente EF para grabar este campo.
                        journalEntryItem.ReconciliationMovementType = bankReconciliation;
                        journalEntryItem.CostCenters = new List<GeneralLedgerModels.CostCenterDTO>();
                        journalEntryItem.Currency = new GeneralLedgerModels.CurrencyDTO() { Id = parameter.CurrencyCode };
                        journalEntryItem.Description = accountingDescription;
                        journalEntryItem.EntryType = new GeneralLedgerModels.EntryTypeDTO();
                        journalEntryItem.Id = 0; //autonumérico
                        journalEntryItem.Individual = individual;
                        journalEntryItem.PostDated = new List<GeneralLedgerModels.PostDatedDTO>();
                        journalEntryItem.Receipt = receipt;

                        journalEntry.JournalEntryItems.Add(journalEntryItem);

                        List<AccountingRuleModels.ParameterDTO> parameters = new List<AccountingRuleModels.ParameterDTO>();
                        parametersCollection.Add(parameters);

                        row++;
                    }
                }

                #endregion OtherMovements

                #region ValidateAndSave

                int entryNumber = DelegateService.glAccountingApplicationService.Accounting(moduleId, parametersCollection, journalEntry);

                if (entryNumber > 0)
                {
                    savedDailyEntryMessage = Global.IntegrationSuccessMessage + " " + entryNumber;

                    collectImputation.Collect = new CollectDTO() { Id = originIdentifyId };
                    collectImputation.Application = new ApplicationDTO()
                    {
                        ModuleId = imputationParameters[0].ImputationTypeId
                    };

                    CollectApplicationDTO collectImputationList = DelegateService.accountingCollectService.GetCollectImputations(collectImputation)[0];

                    ApplicationDTO imputation = collectImputationList.Application;

                    collectImputation.Application = imputation;
                    collectImputation.Transaction = new TransactionDTO() { TechnicalTransaction = entryNumber };
                    DelegateService.accountingCollectService.UpdateCollectImputation(collectImputation);

                    // Se Actualiza el  parámetro de número de trasaccion.
                    _baseController.UpdateTransactionNumber(transactionNumber);
                }
                if (entryNumber == 0)
                {
                    savedDailyEntryMessage = Global.AccountingIntegrationUnbalanceEntry;
                }
                if (entryNumber == -2)
                {
                    savedDailyEntryMessage = Global.EntryRecordingError;
                }

                #endregion ValidateAndSave
            }
            catch (Exception)
            {
                savedDailyEntryMessage = Global.EntryRecordingError;
            }

            return savedDailyEntryMessage;
        }


        #region Payment

        /// <summary>
        /// ValidateCheckBankOrTransfer
        /// Validación de cheque por banco o transferencia
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="numberDoc"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateCheckBankOrTransfer(int bankId, string numberDoc, string accountNumber)
        {
            return Json(DelegateService.accountingPaymentService.ValidateCheckBankOrTransfer(bankId, numberDoc, accountNumber), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateVoucher
        /// Valida que no se ingrese el mismo número de voucher para un mismo número de tarjeta
        /// </summary>
        /// <param name="creditCardNumber"></param>
        /// <param name="voucherNumber"></param>
        /// <param name="conduitType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateVoucher(string creditCardNumber, string voucherNumber, int conduitType)
        {
            return Json(DelegateService.accountingPaymentService.ValidateVoucher(creditCardNumber, voucherNumber, conduitType), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateDepositVoucher
        /// Valida que no se ingrese el mismo número de cheque, cuenta para un mismo banco
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateDepositVoucher(int bankId, string documentNumber, string accountNumber)
        {
            return Json(DelegateService.accountingPaymentService.ValidateDepositVoucher(bankId, documentNumber, accountNumber), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Person

        /// <summary>
        /// GetPersonByDocumentNumber
        /// Obtiene persona por número de documento
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPersonByDocumentNumber(string query)
        {
            var personQuerys = DelegateService.tempCommonService.GetPersonsByDocumentNumber(query, true);

            List<object> personsResponse = new List<object>();

            if (personQuerys.Count > 0)
            {
                foreach (TEMPMOD.IndividualDTO person in personQuerys)
                {
                    personsResponse.Add(new
                    {
                        DocumentName = person.DocumentNumber.Trim().ToString() + " : " + person.Name.Trim().ToString(),
                        DocumentNumber = person.DocumentNumber.Trim().ToString(),
                        IndividualId = person.IndividualId.ToString(),
                        Name = person.Name.Trim().ToString(),
                        Id = person.IndividualId.ToString(),
                        DocumentTypeId = person.DocumentTypeId
                    });
                }
            }
            else
            {
                personsResponse.Add(new
                {
                    DocumentName = @Global.RegisterNotFound,
                    DocumentNumber = @Global.RegisterNotFound,
                    IndividualId = -1,
                    Name = @Global.RegisterNotFound,
                    Id = -1,
                    DocumentTypeId = -1
                });
            }

            return Json(personsResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPersonsByIndividualId
        /// Obtiene persona por número de documento
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPersonsByIndividualId(int individualId)
        {
            List<object> persons = new List<object>();
            IndividualDTO person = DelegateService.accountingAccountsPayableService.GetIndividualByIndividualId(individualId);

            if (person != null)
            {
                persons.Add(new
                {
                    DocumentName = person.IdentificationDocument.Number.Trim().ToString() + " : " + person.Name.Trim().ToString(),
                    DocumentNumber = person.IdentificationDocument.Number.Trim().ToString(),
                    Name = person.Name.Trim().ToString(),
                    Id = person.IndividualId.ToString(),
                    DocumentTypeId = person.IdentificationDocument.DocumentType.Id
                });
            }

            return Json(persons, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPersonByName
        /// Obtiene persona por nombres
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPersonByName(string query)
        {
            List<object> persons = new List<object>();
            var personQuerys = DelegateService.tempCommonService.GetPersonsByName(query);

            if (personQuerys.Count == 0)
            {
                persons.Add(new
                {
                    Id = 0,
                    IndividualId = 0,
                    DocumentNumber = @Global.RegisterNotFound,
                    Name = @Global.RegisterNotFound,
                    DocumentTypeId = -1
                });
            }
            else
            {
                foreach (TEMPMOD.IndividualDTO person in personQuerys)
                {
                    persons.Add(new
                    {
                        DocumentNumber = person.DocumentNumber.ToString().Trim(),
                        Id = person.IndividualId.ToString().Trim(),
                        IndividualId = person.IndividualId.ToString().Trim(),
                        Name = person.Name.Trim(),
                        DocumentTypeId = person.DocumentTypeId
                    });
                }
            }

            return Json(persons, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PolicyQuota

        /// <summary>
        /// GetPolicyQuota
        /// Obtiene las cuotas pendientes de pago de pólizas
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="documentNumber"></param>
        /// <param name="payerName"></param>
        /// <param name="prefix"></param>
        /// <param name="branch"></param>
        /// <param name="holderDocumentNumber"></param>
        /// <param name="holderName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetPolicyQuota(string policyNumber, string documentNumber, string payerName,
                                           string prefix, string branch, string holderDocumentNumber, string holderName)
        {
            List<object> premiumReceivableSearchPolicies = new List<object>();
            List<TEMPMOD.IndividualDTO> holders;
            List<TEMPMOD.IndividualDTO> payers;

            string insuredId = "";
            string payerId = "";

            List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs = new List<SEARCH.PremiumReceivableSearchPolicyDTO>();

            List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyOrder;

            if (String.IsNullOrEmpty(policyNumber) || !IsNumeric(policyNumber))
            {
                policyNumber = "";
            }
            if (String.IsNullOrEmpty(documentNumber) || !IsNumeric(documentNumber))
            {
                documentNumber = "";
            }
            else
            {
                payers = DelegateService.tempCommonService.GetPersonsByDocumentNumber(documentNumber);
                foreach (TEMPMOD.IndividualDTO payer in payers)
                {
                    payerId = Convert.ToString(payer.IndividualId);
                }
            }
            if (String.IsNullOrEmpty(holderDocumentNumber) || !IsNumeric(holderDocumentNumber))
            {
                holderDocumentNumber = "";
            }
            else
            {
                holders = DelegateService.tempCommonService.GetPersonsByDocumentNumber(holderDocumentNumber);
                foreach (TEMPMOD.IndividualDTO insured in holders)
                {
                    insuredId = Convert.ToString(insured.IndividualId);
                }
            }

            if (String.IsNullOrEmpty(payerName) || payerName == "-1")
            {
                payerName = "";
            }
            else
            {
                payers = DelegateService.tempCommonService.GetPersonsByName(payerName);
                foreach (TEMPMOD.IndividualDTO payer in payers)
                {
                    payerId = Convert.ToString(payer.IndividualId);
                }
            }
            if (String.IsNullOrEmpty(holderName) || holderName == "-1")
            {
                holderName = "";
            }
            else
            {
                holders = DelegateService.tempCommonService.GetPersonsByName(holderName);
                foreach (TEMPMOD.IndividualDTO insured in holders)
                {
                    insuredId = Convert.ToString(insured.IndividualId);
                }
            }

            premiumReceivableSearchPolicyDTOs = DelegateService.accountingApplicationService.PremiumReceivableSearchPolicy(insuredId, payerId, "", "", "", policyNumber, "",
                                                                                                                          branch, prefix, "", "", "", "", PageSize, PageIndex);

            //Ordenamiento por Cuota/Endoso/Póliza
            premiumReceivableSearchPolicyOrder = (from order in premiumReceivableSearchPolicyDTOs
                                                  orderby order.PaymentNumber, order.EndorsementId, order.PolicyId
                                                  select order).ToList();

            foreach (SEARCH.PremiumReceivableSearchPolicyDTO premiumReceivable in premiumReceivableSearchPolicyOrder)
            {
                premiumReceivableSearchPolicies.Add(new
                {
                    ItemId = premiumReceivable.ItemId,
                    PolicyId = premiumReceivable.PolicyId,
                    PolicyDocumentNumber = premiumReceivable.PolicyDocumentNumber,
                    EndorsementId = premiumReceivable.EndorsementId,
                    EndorsementDocumentNumber = premiumReceivable.EndorsementDocumentNumber,
                    PayerId = premiumReceivable.PayerId,
                    BillId = premiumReceivable.CollectGroupId,
                    ItemTypeId = premiumReceivable.ItemId,
                    DocumentType = premiumReceivable.BussinessTypeId,
                    PayerDocumentNumber = premiumReceivable.PayerDocumentNumber,
                    PayerName = premiumReceivable.PayerName,
                    PaymentNumber = premiumReceivable.PaymentNumber,
                    PaymentAmount = premiumReceivable.PaymentAmount,
                    Amount = premiumReceivable.Amount,
                    IndividualId = premiumReceivable.InsuredId,
                    CurrencyId = premiumReceivable.CurrencyId,
                    ExchangeRate = premiumReceivable.ExchangeRate
                });
            }

            return new UifTableResult(premiumReceivableSearchPolicies);
        }

        /// <summary>
        /// GetPolicyQuotaListView
        /// Obtiene las cuotas pendientes de pago de pólizas
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="documentNumber"></param>
        /// <param name="prefix"></param>
        /// <param name="branch"></param>
        /// <param name="payerName"></param>
        /// <param name="holderDocumentNumber"></param>
        /// <param name="holderName"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPolicyQuotaListView(string policyNumber, string documentNumber, string payerName, string prefix, string branch,
                                                 string holderDocumentNumber, string holderName)
        {
            List<object> premiumReceivableSearchPolicies = new List<object>();
            List<TEMPMOD.IndividualDTO> persons = new List<TEMPMOD.IndividualDTO>();

            string insuredId = "";
            string payerId = "";

            List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs;

            List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPoliciesOrders;

            //POR NO. DOCUMENTO DEL TITULAR DE LA PÓLIZA
            if (String.IsNullOrEmpty(holderDocumentNumber.Trim()) || !IsNumeric(holderDocumentNumber.Trim())) //- el documento viene con espacios
            {
                holderDocumentNumber = "";
            }
            else
            {
                insuredId = holderDocumentNumber;
            }

            //POR NOMBRE DEL TITULAR DE LA PÓLIZA
            if (String.IsNullOrEmpty(holderName.Trim()) || holderName == "-1")
            {
                holderName = "";
            }
            else
            {
                persons = DelegateService.tempCommonService.GetPersonsByName(holderName.Trim().ToUpper());

                if (persons.Count > 0)
                {
                    insuredId = persons[0].IndividualId.ToString();
                }
                else
                {
                    insuredId = "-1";
                }
            }

            //POR NO. DOCUMENTO DEL PAGADOR
            if (String.IsNullOrEmpty(documentNumber.Trim()) || !IsNumeric(documentNumber.Trim())) //- el documento viene con espacios
            {
                documentNumber = "";
            }
            else
            {
                persons = DelegateService.tempCommonService.GetPersonsByDocumentNumber(documentNumber);

                if (persons.Count > 0)
                {
                    payerId = persons[0].IndividualId.ToString();
                }
                else
                {
                    payerId = "-1";
                }
            }

            //POR NOMBRE DEL PAGADOR
            if (String.IsNullOrEmpty(payerName) || payerName == "-1")
            {
                payerName = "";
            }
            else
            {
                persons = DelegateService.tempCommonService.GetPersonsByName(payerName.Trim().ToUpper());

                if (persons.Count > 0)
                {
                    payerId = persons[0].IndividualId.ToString();
                }
                else
                {
                    payerId = "-1";
                }
            }

            if (persons.Count > 0)
            {
                foreach (TEMPMOD.IndividualDTO person in persons)
                {
                    if (!String.IsNullOrEmpty(payerName.Trim()))
                    {
                        payerId = person.IndividualId.ToString();
                    }
                    if (!String.IsNullOrEmpty(holderName.Trim()))
                    {
                        insuredId = person.IndividualId.ToString();
                    }

                    premiumReceivableSearchPolicyDTOs =
                    DelegateService.accountingApplicationService.PremiumReceivableSearchPolicy(insuredId, payerId, "", "", "", policyNumber, "",
                                                                    branch, prefix, "", "", "", "", PageSize, PageIndex);

                    if (premiumReceivableSearchPolicyDTOs.Count > 0)
                    {
                        //Ordenamiento por Cuota/Endoso/Póliza
                        premiumReceivableSearchPoliciesOrders = (from order in premiumReceivableSearchPolicyDTOs
                                                                 orderby order.PaymentNumber, order.EndorsementId, order.PolicyId
                                                                 select order).ToList();


                        foreach (SEARCH.PremiumReceivableSearchPolicyDTO premiumReceivable in premiumReceivableSearchPoliciesOrders)
                        {
                            premiumReceivableSearchPolicies.Add(new
                            {
                                PolicyId = premiumReceivable.PolicyId,
                                PolicyNumber = premiumReceivable.PolicyDocumentNumber,
                                EndorsementNumber = premiumReceivable.EndorsementDocumentNumber,
                                BranchId = premiumReceivable.BranchId,
                                BranchName = premiumReceivable.BranchDescription,
                                CurrencyId = premiumReceivable.CurrencyId,
                                CurrencyName = premiumReceivable.CurrencyDescription,
                                PayerId = premiumReceivable.PayerId,
                                PayerDocumentNumber = premiumReceivable.PayerDocumentNumber,
                                PayerName = premiumReceivable.PayerName,
                                QuotaNumber = premiumReceivable.PaymentNumber,
                                QuotaValue = premiumReceivable.PaymentAmount,
                                ItemId = premiumReceivable.ItemId,
                                EndorsementId = premiumReceivable.EndorsementId,
                                BillId = premiumReceivable.CollectGroupId,
                                ItemTypeId = premiumReceivable.ItemId,
                                DocumentType = premiumReceivable.BussinessTypeId,
                                Amount = premiumReceivable.Amount,
                                IndividualId = premiumReceivable.InsuredId,
                                ExchangeRate = premiumReceivable.ExchangeRate,
                                PrefixName = premiumReceivable.PrefixDescription
                            });
                        }

                        return Json(premiumReceivableSearchPolicies, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else  //esta buscando por #póliza
            {
                premiumReceivableSearchPolicyDTOs =
                DelegateService.accountingApplicationService.PremiumReceivableSearchPolicy(insuredId, payerId, "", "", "", policyNumber, "",
                                                                branch, prefix, "", "", "", "", PageSize, PageIndex);

                if (premiumReceivableSearchPolicyDTOs.Count > 0)
                {
                    //Ordenamiento por Cuota/Endoso/Póliza
                    premiumReceivableSearchPoliciesOrders = (from order in premiumReceivableSearchPolicyDTOs orderby order.PaymentNumber, order.EndorsementId, order.PolicyId select order).ToList();

                    foreach (SEARCH.PremiumReceivableSearchPolicyDTO premiumReceivable in premiumReceivableSearchPoliciesOrders)
                    {
                        premiumReceivableSearchPolicies.Add(new
                        {
                            PolicyId = premiumReceivable.PolicyId,
                            PolicyNumber = premiumReceivable.PolicyDocumentNumber,
                            EndorsementNumber = premiumReceivable.EndorsementDocumentNumber,
                            BranchId = premiumReceivable.BranchId,
                            BranchName = premiumReceivable.BranchDescription,
                            CurrencyId = premiumReceivable.CurrencyId,
                            CurrencyName = premiumReceivable.CurrencyDescription,
                            PayerId = premiumReceivable.PayerId,
                            PayerDocumentNumber = premiumReceivable.PayerDocumentNumber,
                            PayerName = premiumReceivable.PayerName,
                            QuotaNumber = premiumReceivable.PaymentNumber,
                            QuotaValue = premiumReceivable.PaymentAmount,
                            ItemId = premiumReceivable.ItemId,
                            EndorsementId = premiumReceivable.EndorsementId,
                            BillId = premiumReceivable.CollectGroupId,
                            ItemTypeId = premiumReceivable.ItemId,
                            DocumentType = premiumReceivable.BussinessTypeId,
                            Amount = premiumReceivable.Amount,
                            IndividualId = premiumReceivable.InsuredId,
                            ExchangeRate = premiumReceivable.ExchangeRate,
                            PrefixName = premiumReceivable.PrefixDescription
                        });
                    }
                }
            }

            return Json(premiumReceivableSearchPolicies, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// IsNumeric
        /// Determina si una cadena es un número
        /// </summary>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        public static bool IsNumeric(object value)
        {
            bool isNumber;
            double isItNumeric;
            isNumber = Double.TryParse(Convert.ToString(value), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out isItNumeric);
            return isNumber;
        }

        #endregion

        #region BillingClosure

        /// <summary>
        /// GetPercentageForPayQuota
        /// Obtiene el porcentaje mínimo para pago de cuotas en el Cierre de Caja
        /// </summary>
        /// <param name="numberParameter"></param>
        /// <returns>double</returns>
        public double GetPercentageForPayQuota(int numberParameter)
        {
            List<SEARCH.PercentagePayQuotaDTO> percentagePayQuotas = DelegateService.accountingCollectControlService.GetPercentageForPayQuota(numberParameter);

            return Convert.ToDouble(percentagePayQuotas[0].PercentageParameter);
        }

        #endregion

        #region DailyCashClosing

        /// <summary>
        /// DailyCashClosing
        /// Invoca a la vista DailyCashClosing
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DailyCashClosing()
        {
            try
            {

                int percentageQuotaParameter = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumPercentageQuota"]);//6015 en Enterprise

                //Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                int localCurrencyId = 0;
                ViewBag.localCurrencyId = localCurrencyId;

                ViewBag.UserNick = _commonController.GetUserByName(User.Identity.Name)[0].AccountName;

                ViewBag.idBillControl = TempData["BillControlId"];
                ViewBag.idBranch = TempData["BranchId"];

                // Recupera el porcentaje mínimo para pago de cuotas
                double percentage = GetPercentageForPayQuota(percentageQuotaParameter);
                ViewBag.PercentageParameter = percentage;

                //Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
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
        /// GetBranchByOpenStatus
        /// Obtiene las sucursales que mantienen un registro de control de caja en estado “Abierta” en la tabla BILL.BILL_CONTROL
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetBranchByOpenStatus()
        {
            List<Branch> branches = GetBranchesOpenStatus();

            return new UifSelectResult(from branch in branches orderby branch.Description select branch);
        }

        /// <summary>
        /// GetCashierByBranchId
        /// Obtiene los cajeros/usuarios en base a la sucursal que mantienen un registro de control de caja en estado "Abierta"
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCashierByBranchId(int branchId)
        {
            List<object> users = new List<object>();
            List<int> userIds = DelegateService.accountingCollectService.GetCashierByBranchId(branchId);
            foreach (int u in userIds)
            {
                if (DelegateService.uniqueUserService.GetUserById(u).AccountName.ToUpper() == User.Identity.Name.ToUpper())
                {
                    users.Add(new
                    {
                        Id = u,
                        Nick = DelegateService.uniqueUserService.GetUserById(u).AccountName
                    });
                }
            }

            return new UifSelectResult(users);
        }

        /// <summary>
        /// ValidateCheckCardDeposited
        /// Valida que todos los cheques y tarjetas ingresadas por caja (de la sucursal, usuario
        /// y hasta la fecha de proceso) estén con estado 
        /// "asignado a boleta" (es decir depositadas)
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateCheckCardDeposited(int branchId)
        {
            decimal checkCardDeposited = DelegateService.accountingCollectService.ValidateCheckCardDeposited(branchId, _commonController.GetUserIdByName(User.Identity.Name));

            return Json(checkCardDeposited, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RequiredCloseBill
        /// Consulta si es necesario cerrar la caja
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="accountingDatePresent"></param>
        /// <returns>JsonResult</returns>
        public JsonResult RequiredCloseBill(int branchId, int userId, DateTime accountingDatePresent)
        {
            int status = Convert.ToInt32(CollectControlStatus.Open);
            CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(userId, branchId, accountingDatePresent.Date, status);
            List<object> collectControlResponse = new List<object>();
            collectControlResponse.Add(new
            {
                resp = collectControl.Status,
                Id = collectControl.Id,
                AccountingDate = collectControl.AccountingDate.ToString("dd/MM/yyyy"),
                OpenDate = collectControl.OpenDate,
                OpenDateString = collectControl.OpenDate.ToString("G", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat)
            });
            return Json(collectControlResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AdmitOpenBill
        /// Permite aperturar la Caja
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AdmitOpenBill(int branchId, int userId, string accountingDate)
        {
            int status = Convert.ToInt32(CollectControlStatus.Open);
            bool isAllowOpenCollect = DelegateService.accountingCollectControlService.AllowOpenCollect(userId, branchId, Convert.ToDateTime(accountingDate), status);
            List<object> openCollects = new List<object>();
            openCollects.Add(new
            {
                resp = isAllowOpenCollect
            });
            return Json(openCollects, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateCashDeposited
        /// Permite validar que el total del monto efectivo registrado en el sistema se encuentre asignado
        /// a una boleta interna, para esto restar el total del efectivo registrado menos el efectivo
        /// asignado en boletas internas no anuladas que sean del mismo usuario, sucursal y fecha menor
        /// o igual a la del proceso
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateCashDeposited(int branchId, int currencyId)
        {
            decimal cashDeposited = DelegateService.accountingCollectService.ValidateCashDeposited(branchId, _commonController.GetUserIdByName(User.Identity.Name), currencyId);

            return Json(cashDeposited, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetRegisterDateBillControl
        /// </summary>
        /// <param name="billControlId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRegisterDateBillControl(int billControlId)
        {
            string registerDate = DelegateService.accountingCollectService.GetRegisterDateCollectControl(billControlId);
            List<object> billControls = new List<object>();
            billControls.Add(new
            {
                RegisterDateBC = registerDate,
                Id = billControlId
            });

            return Json(billControls, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetSummaryPaymentMethods
        /// Obtiene totales por tipo de pago para el cierre de caja
        /// </summary>
        /// <param name="billControlId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSummaryPaymentMethods(int billControlId)
        {
            List<object> summaryPayMethods = new List<object>();

            if (billControlId > 0)
            {
                List<SEARCH.SumaryPayMethodDTO> summaryPayMethodDTOs = DelegateService.accountingCollectControlService.GetSumaryPayMethod(billControlId);

                foreach (SEARCH.SumaryPayMethodDTO summaryPayMethodDTO in summaryPayMethodDTOs)
                {
                    summaryPayMethods.Add(new
                    {
                        Id = summaryPayMethodDTO.CollectControlCode.ToString(),
                        PaymentMethods = summaryPayMethodDTO.Description,
                        IdPaymentMethod = summaryPayMethodDTO.PaymentMethodCode.ToString(),
                        TotalAdmitted = summaryPayMethodDTO.Total,
                        TotalReceived = summaryPayMethodDTO.Total,
                        TotalDifference = 0
                    });
                }
                var summaryPayMethodResponses = new
                {
                    rows = summaryPayMethods
                };

                return new UifTableResult(summaryPayMethodResponses.rows);
            }
            return new UifTableResult(summaryPayMethods);
        }

        #endregion

        #region CancelApplicationReceipt
        /// <summary>
        /// llamado vista CancelAppliationReceipt
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelAppliationReceipt()
        {
            try
            {

                int percentageQuotaParameter = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumPercentageQuota"]);//6015 en Enterprise

                //Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                int localCurrencyId = 0;
                ViewBag.localCurrencyId = localCurrencyId;

                ViewBag.UserNick = _commonController.GetUserByName(User.Identity.Name)[0].AccountName;

                ViewBag.idBillControl = TempData["BillControlId"];
                ViewBag.idBranch = TempData["BranchId"];

                // Recupera el porcentaje mínimo para pago de cuotas
                double percentage = GetPercentageForPayQuota(percentageQuotaParameter);
                ViewBag.PercentageParameter = percentage;

                //Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();
            }

            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        public JsonResult GetTempApplicationsByUserId()
        {
            try
            {
                List<TempApplicationDTO> tempApplications = new List<TempApplicationDTO>();
                List<object> newtempApplications = new List<object>();
                tempApplications = DelegateService.accountingApplicationService.GetTempApplicationByUserId(_commonController.GetUserIdByName(User.Identity.Name)).OrderByDescending(x => x.Id).ToList();
                foreach (TempApplicationDTO item in tempApplications)
                {
                    var date = item.RegisterDate.ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);

                    newtempApplications.Add(new
                    {
                        SourceId = item.SourceId,
                        UserName = User.Identity.Name,
                        RegisterDate = date,
                        TempAppId = item.Id
                    });
                }
                return new UifTableResult(newtempApplications);
            }

            catch (UnhandledException)
            {
                return new UifTableResult(null);
            }
        }

        #endregion

        #region CollectControlPayment

        /// <summary>
        /// SaveBillControlPayment
        /// Graba el control de caja
        /// </summary>
        /// <param name="frmBillControl"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBillControlPayment(BillControlModel frmBillControl)
        {
            CollectControlDTO collectControl = new CollectControlDTO();
            collectControl.Id = frmBillControl.BillControlId;

            collectControl.CollectControlPayments = new List<CollectControlPaymentDTO>();
            if (frmBillControl.BillControlPayments != null)
            {
                for (int j = 0; j <= frmBillControl.BillControlPayments.Count - 1; j++)
                {
                    CollectControlPaymentDTO collectControlPayment = new CollectControlPaymentDTO();

                    collectControlPayment.Id = frmBillControl.BillControlId + j;
                    collectControlPayment.PaymentMethod = new PaymentMethodDTO() { Id = frmBillControl.BillControlPayments[j].PaymentMethodId };
                    collectControlPayment.PaymentTotalAdmitted = new AmountDTO() { Value = Convert.ToDecimal(frmBillControl.BillControlPayments[j].PaymentTotalAdmitted) };
                    collectControlPayment.PaymentsTotalReceived = new AmountDTO() { Value = Convert.ToDecimal(frmBillControl.BillControlPayments[j].PaymentsTotalReceived) };
                    collectControlPayment.PaymentsTotalDifference = new AmountDTO() { Value = Convert.ToDecimal(frmBillControl.BillControlPayments[j].PaymentsTotalDifference) };
                    collectControl.CollectControlPayments.Add(collectControlPayment);

                    DelegateService.accountingCollectControlService.SaveCollectControlPayment(collectControl, j);
                }
            }

            return Json(collectControl, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region IncomeChangeConcept

        /// <summary>
        /// GetReceiptForExchangeConcept
        /// Obtiene los recibos no anulados y que el mes de la fecha contable sea igual al mes de la fecha contable del módulo
        /// para cambiar el concepto de ingreso 
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReceiptForExchangeConcept(int billId)
        {
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
            SEARCH.IncomeChangeConceptDTO incomeChangeConcept = DelegateService.accountingCollectService.GetReceiptForExchangeConcept(billId, accountingDate);

            List<object> incomeChangeConceptResponse = new List<object>();

            if (incomeChangeConcept != null)
            {
                incomeChangeConceptResponse.Add(new
                {
                    RegisterDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(incomeChangeConcept.RegisterDate)),
                    BillControlCode = incomeChangeConcept.CollectControlCode,
                    BillingConceptCode = incomeChangeConcept.CollectConceptCode,
                    BillingConceptName = incomeChangeConcept.CollectConceptName
                });
            }

            return Json(incomeChangeConceptResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateIncomeChangeConcept
        /// Actualiza el campo BILLING_CONCEPT_CD en la tabla BILL.BILL 
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="billControlId"></param>
        /// <param name="billingConceptId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateIncomeChangeConcept(int billId, int billControlId, int billingConceptId)
        {
            try
            {
                CollectDTO collect = DelegateService.accountingCollectService.UpdateIncomeChangeConcept(billId, billControlId, billingConceptId);

                return Json(collect, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = unhandledException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Agent

        /// <summary>
        /// LoadAccountingCompanies
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<CompanyDTO/></returns>
        public List<CompanyDTO> LoadAccountingCompanies(int userId)
        {
            return DelegateService.accountingApplicationService.GetCompaniesByUserId(userId);
        }

        /// <summary>
        /// GetParameterMulticompany
        /// Obtiene parámetro de multicompanía
        /// </summary>
        /// <returns>is Multicompany</returns>
        public int GetParameterMulticompany()
        {
            bool existsParameterMulticompany = DelegateService.accountingApplicationService.GetParameterMulticompany();
            int isMulticompany = 0;

            if (existsParameterMulticompany)
            {
                isMulticompany = 1;
            }
            else
            {
                isMulticompany = 0;
            }

            return isMulticompany;
        }

        #endregion

        #region Imputation

        /// <summary>
        /// UpdateTempImputationSourceCode
        /// Actualiza el temporal de imputación de acuerdo al código
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="sourceId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateTempImputationSourceCode(int tempImputationId, int sourceId)
        {
            return Json(DelegateService.accountingApplicationService.UpdateTempApplicationSourceCode(tempImputationId, sourceId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReverseBillEntry
        /// Reversa el asiento contable asociado a la imputación
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int ReverseBillEntry(int billId, int userId, DateTime accountingDate, int entryReverseId, int technicalTransaction = 0)
        {
            int reversed = 0;

            try
            {
                // Se reaiza la reversión del asiento
                CollectDTO collect = new CollectDTO();
                collect.Id = billId;
                collect = DelegateService.accountingCollectService.GetCollect(collect);

                int entryId = collect.Transaction.TechnicalTransaction;

                if (entryId > 0)
                {
                    GeneralLedgerModels.JournalEntryDTO journalEntry = new GeneralLedgerModels.JournalEntryDTO();
                    journalEntry.Id = entryId;
                    journalEntry = DelegateService.glAccountingApplicationService.GetJournalEntry(journalEntry);

                    reversed = DelegateService.glAccountingApplicationService.ReverseJournalEntry(journalEntry, entryReverseId);

                    journalEntry.Id = entryId;
                }
            }
            catch (UnhandledException)
            {
                return -1;
            }

            return reversed;
        }

        #endregion

        #region Retention

        /// <summary>
        /// GetPolicyIdByBranchPrefixPolicyNumber
        /// Recupera la póliza si existe
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixIde"></param>
        /// <param name="policyNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPolicyIdByBranchPrefixPolicyNumber(int branchId, int prefixId, string policyNumber)
        {
            var jsonPolicies = new List<object>();
            jsonPolicies.Add(new
            {
                EndorsementNumber = -1,
                ExpirationDate = "",
                PolicyId = -1
            });

            if (DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(Convert.ToInt32(prefixId),
                                                                                    Convert.ToInt32(branchId),
                                                                                    Convert.ToDecimal(policyNumber)) != null)
            {
                var policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(Convert.ToInt32(prefixId),
                                                                                               Convert.ToInt32(branchId),
                                                                                               Convert.ToDecimal(policyNumber));

                jsonPolicies = new List<object>();
                jsonPolicies.Add(new
                {
                    EndorsementNumber = policy.Endorsement.Number,
                    ExpirationDate = policy.CurrentTo.ToString("dd/MM/yyyy"),
                    policy.Endorsement.PolicyId
                });
            }

            return Json(jsonPolicies, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Method Private

        /// <summary>
        /// ExportCollectItem
        /// </summary>
        /// <param name="collectItemWithoutPaymentTicketList"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportCollectItem(List<SEARCH.CollectItemWithoutPaymentTicketDTO> collectItemWithoutPaymentTicketList)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            int branchCd = collectItemWithoutPaymentTicketList.Select(sl => sl.BranchId).First();
            string branchDescription = collectItemWithoutPaymentTicketList.Select(sl => sl.BranchDescription).First();
            string user = collectItemWithoutPaymentTicketList.Select(sl => sl.User).First();

            #region Font

            font.FontName = "Arial";
            font.FontHeightInPoints = 8;
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Black.Index;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Arial";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            //TITLE FONT
            var fontTitle = workbook.CreateFont();
            fontTitle.FontName = "Arial";
            fontTitle.FontHeightInPoints = 20;
            fontTitle.Boldweight = (short)FontBoldWeight.Bold;
            fontTitle.Color = HSSFColor.Black.Index;

            //HEADER FONT
            var fontHeader = workbook.CreateFont();
            fontHeader.FontName = "Arial";
            fontHeader.FontHeightInPoints = 12;
            fontHeader.Boldweight = (short)FontBoldWeight.Bold;
            fontHeader.Color = HSSFColor.Black.Index;

            var fontHeaderDescription = workbook.CreateFont();
            fontHeaderDescription.FontName = "Arial";
            fontHeaderDescription.FontHeightInPoints = 12;
            fontHeaderDescription.Color = HSSFColor.Black.Index;

            #endregion

            #region style

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;
            styleHeader.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.RightBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.TopBorderColor = HSSFColor.Grey40Percent.Index;
            styleHeader.BorderBottom = BorderStyle.Double;
            styleHeader.BorderLeft = BorderStyle.Double;
            styleHeader.BorderRight = BorderStyle.Double;
            styleHeader.BorderTop = BorderStyle.Double;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.RightBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.TopBorderColor = HSSFColor.Grey40Percent.Index;
            styleDetail.BorderBottom = BorderStyle.Double;
            styleDetail.BorderLeft = BorderStyle.Double;
            styleDetail.BorderRight = BorderStyle.Double;
            styleDetail.BorderTop = BorderStyle.Double;

            ICellStyle Amountstyle = workbook.CreateCellStyle();
            Amountstyle.SetFont(fontDetail);
            Amountstyle.Alignment = HorizontalAlignment.Right;
            Amountstyle.BottomBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.LeftBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.RightBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.TopBorderColor = HSSFColor.Grey40Percent.Index;
            Amountstyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.##");
            Amountstyle.BorderBottom = BorderStyle.Double;
            Amountstyle.BorderLeft = BorderStyle.Double;
            Amountstyle.BorderRight = BorderStyle.Double;
            Amountstyle.BorderTop = BorderStyle.Double;

            ICellStyle styleTitle = workbook.CreateCellStyle();
            styleTitle.SetFont(fontTitle);
            styleTitle.FillForegroundColor = HSSFColor.White.Index;
            styleTitle.FillPattern = FillPattern.SolidForeground;

            ICellStyle Headerstyle = workbook.CreateCellStyle();
            Headerstyle.SetFont(fontHeader);

            ICellStyle HeaderDetailstyle = workbook.CreateCellStyle();
            HeaderDetailstyle.SetFont(fontHeaderDescription);

            #endregion

            #region Header

            var titleRow = sheet.CreateRow(0);
            titleRow.CreateCell(0).SetCellValue(Global.PendingValues);
            titleRow.GetCell(0).CellStyle = styleTitle;

            var sheetbranch = sheet.CreateRow(2);
            sheetbranch.CreateCell(0).SetCellValue(Global.Branch.ToUpper());
            sheetbranch.GetCell(0).CellStyle = Headerstyle;

            sheetbranch.CreateCell(1).SetCellValue(branchCd.ToString() + " - " + branchDescription);
            sheetbranch.GetCell(1).CellStyle = HeaderDetailstyle;

            var sheetuser = sheet.CreateRow(3);
            sheetuser.CreateCell(0).SetCellValue(Global.User.ToUpper());
            sheetuser.GetCell(0).CellStyle = Headerstyle;

            sheetuser.CreateCell(1).SetCellValue(user);
            sheetuser.GetCell(1).CellStyle = HeaderDetailstyle;

            #endregion

            #region Detail

            var headerRow = sheet.CreateRow(5);

            headerRow.CreateCell(0).SetCellValue(@Global.AccountingDate.ToUpper());
            headerRow.CreateCell(1).SetCellValue(@Global.OpenAccountingDate.ToUpper());
            headerRow.CreateCell(2).SetCellValue(@Global.TransactionNumber.ToUpper());
            headerRow.CreateCell(3).SetCellValue(@Global.MeansPayment.ToUpper());
            headerRow.CreateCell(4).SetCellValue(@Global.Currency.ToUpper());
            headerRow.CreateCell(5).SetCellValue(@Global.Status.ToUpper());
            headerRow.CreateCell(6).SetCellValue(@Global.PendingAmountAssigned.ToUpper());

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

            int rowNumber = 6;
            int i;
            foreach (SEARCH.CollectItemWithoutPaymentTicketDTO item in collectItemWithoutPaymentTicketList)
            {
                var row = sheet.CreateRow(rowNumber++);
                i = 0;
                row.CreateCell(i).SetCellValue(item.AccountingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.OpenDate.ToString("dd/MM/yyyy HH:mm:ss"));
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.TechnicalTransaction);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.PaymentMethod);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.CurrencyDescription);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(item.Status);
                row.GetCell(i).CellStyle = styleDetail;
                i++;
                row.CreateCell(i).SetCellValue(string.Format(new CultureInfo("en-US"), "{0:C2}", item.Amount));
                row.GetCell(i).CellStyle = Amountstyle;
            }

            #endregion

            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        /// <summary>
        /// GetBranchesOpenStatus
        /// Obtiene las sucursales que mantienen un registro de control de caja en estado “Abierta” en la tabla BILL.BILL_CONTROL
        /// </summary>
        /// <returns>List<Branch/></returns>       
        private List<Branch> GetBranchesOpenStatus()
        {
            List<Branch> branches = DelegateService.commonService.GetBranches();
            List<Branch> openBranches = new List<Branch>();

            List<SEARCH.BranchDTO> branchesOpens = DelegateService.accountingCollectService.GetBranchesOpenStatus();

            foreach (Branch branch in branches)
            {
                foreach (SEARCH.BranchDTO branchModel in branchesOpens)
                {
                    if (branch.Id == branchModel.Id)
                    {
                        openBranches.Add(branch);
                        break;
                    }
                }
            }

            return openBranches;
        }

        /// <summary>
        /// IsNumeric
        /// Determina si una cadena es un número y además tiene un espacio y/o -
        /// </summary>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        public bool IsNumeric(string value)
        {
            var valid = "0123456789 -";
            bool isNumber = true;
            var temp = "";

            for (var i = 0; i < value.Length; i++)
            {
                temp = value.Substring(i, 1);
                if (valid.IndexOf(temp) < 0)
                {
                    isNumber = false;
                    break;
                }
            }

            return isNumber;
        }

        /// <summary>
        /// UpdateTransactionNumber
        /// </summary>
        /// <param name="number"></param>
        private void UpdateTransactionNumber(int number)
        {
            Parameter parameter = new Parameter();
            parameter.Id = Convert.ToInt32(ConfigurationManager.AppSettings["TransactionNumber"]);
            parameter.NumberParameter = number;
            parameter.Description = "NÚMERO DE TRANSACCIÓN";
            parameter.NumberParameter = parameter.NumberParameter + 1;
            DelegateService.commonService.UpdateParameter(parameter);
        }


        #endregion

        #region functions
        public JsonResult GetControlKey(string controlName)
        {
            return Json(Convert.ToInt32(ConfigurationManager.AppSettings[controlName]), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Application
        private ParamApplicationPremiumComponent CreateApplicationCollect(BillItemModel billItemModel)
        {
            ParamApplicationPremiumComponent applicationDTO = new ParamApplicationPremiumComponent();
            //{
            //    Id = billItemModel,
            //    AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now),
            //    RegisterDate = DateTime.Now,
            //    ModuleId = Convert.ToInt32(ApplicationTypes.Collect),
            //    UserId = userId,
            //    ApplicationItems = new List<TransactionTypeDTO>()
            //};

            //if (sourcePaymentId > 0)
            //{
            //    applicationDTO.ModuleId = (int)ApplicationTypes.PreLiquidation;
            //}

            return applicationDTO;
        }
        #endregion

        public JsonResult GetLastDateByBranchId(int branchId)
        {
            try
            {
                DateTime date = DelegateService.accountingCollectControlService.GetLastOpenDateByUserIdBranchId(SessionHelper.GetUserId(), branchId);
                return new UifJsonResult(true, date);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDefaultCurrency);
            }
        }

        /// <summary>
        /// Get Avaible Currencies
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAvaibleCurrencies()
        {
            return new UifSelectResult(DelegateService.accountingCollectService.GetAvaibleCurrencies());
        }

        /// <summary>
        /// Get avaible banks
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAvaibleBanksByCurrencyId(int currencyId)
        {
            List<object> banks = new List<object>();
            var avaibleBanks = DelegateService.accountingCollectService.GetAvaibleBanksByCurrencyId(currencyId);
            avaibleBanks.ForEach(x =>
            {
                banks.Add(new
                {
                    x.Id,
                    x.Description
                });
            });
            return new UifSelectResult(banks);
        }

        /// <summary>
        /// Get avaible accouting banks
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAvaibleAccoutingBanksByCurrencyIdBankId(int currencyId, int bankId)
        {
            List<object> accountingBanks = new List<object>();
            var avaibleAccountBanks = DelegateService.accountingCollectService.GetAvaibleAccountsByCurrencyIdBankId(currencyId, bankId);
            avaibleAccountBanks.ForEach(x =>
            {
                accountingBanks.Add(new
                {
                    x.Id,
                    Description = x.Number
                });
            });
            return new UifSelectResult(accountingBanks);
        }

        public JsonResult GetPaymentTicketSequence()
        {
            try
            {
                int nextSequenceValue = DelegateService.accountingPaymentTicketService.GetPaymentTicketSequence();
                return Json(new { success = true, result = nextSequenceValue }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}