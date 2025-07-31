//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Helpers;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;

//Sistran Company
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class JournalEntryController : Controller
    {
        #region Instance Variables
        readonly BillingController _billingController = new BillingController();
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// MainJournalEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainJournalEntry()
        {
            try
            {           
                int dataJournalId;

                // Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

                ViewBag.AccountingDate = TempData["AccountingDate"] ?? _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;
                ViewBag.Depositer = TempData["Depositer"] ?? "";
                ViewBag.Amount = TempData["Amount"] ?? "";
                ViewBag.LocalAmount = TempData["LocalAmount"] ?? "";
                ViewBag.Branch = TempData["Branch"] ?? "";
                ViewBag.IncomeConcept = TempData["IncomeConcept"] ?? "";
                ViewBag.PostedValue = TempData["PostedValue"] ?? "";
                ViewBag.Description = TempData["Description"] ?? "";
                ViewBag.TempImputationId = TempData["TempImputationId"] ?? 0;

                ViewBag.dataId = 0;
                if (!Convert.ToBoolean(TempData["IsEdit"]))
                {
                    dataJournalId = TempIdJournal(0);
                    ViewBag.JournalId = dataJournalId;
                    ViewBag.dataId = DelegateService.accountingApplicationService.GetTempApplicationBySourceCode(Convert.ToInt32(@Global.ImputationTypeJournalEntry), dataJournalId).Id;
                    if (ViewBag.dataId != 0)
                    {
                        ViewBag.ReceiptTitle = @Global.ApplicationJournalTitle + " 0";
                    }

                    ViewBag.dataImputation = SaveTempApplication(Convert.ToInt32(@Global.ImputationTypeJournalEntry), dataJournalId, 0);
                    ViewBag.ReceiptTitle = @Global.ApplicationJournalTitle + " " + ViewBag.dataImputation.Data.Id;
                    ViewBag.TempImputationId = ViewBag.dataImputation.Data.Id;
                }
                else
                {
                    ViewBag.ReceiptTitle = @Global.ApplicationJournalTitle + " " + Convert.ToInt32(TempData["TempImputationId"]);
                    ViewBag.JournalId = Convert.ToInt32(TempData["JournalEntryId"]);
                }

                ViewBag.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                ViewBag.UserNick = User.Identity.Name;
                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
            
                int defaultAccounting = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(ViewBag.UserId);

                if (defaultAccounting <= 0)
                {
                    ViewBag.AccountingCompanyDefault = 1;
                }
                else
                {
                    ViewBag.AccountingCompanyDefault = defaultAccounting;
                }
                ViewBag.AccountingCompany = _billingController.LoadAccountingCompanies(_commonController.GetUserIdByName(ViewBag.UserNick));

                int branchUserDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchUserDefault = branchUserDefault;
                ViewBag.SalePointBranchUserDefault = DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(_commonController.GetUserIdByName(User.Identity.Name), branchUserDefault);

                // Tipo de beneficiario
                ViewBag.SupplierCode = Convert.ToInt32(ConfigurationManager.AppSettings["SupplierCode"]);
                ViewBag.InsuredCode = Convert.ToInt32(ConfigurationManager.AppSettings["InsuredCode"]);
                ViewBag.CoinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"]);
                ViewBag.ThirdPartyCode = Convert.ToInt32(ConfigurationManager.AppSettings["ThirdPartyCode"]);
                ViewBag.AgentCode = Convert.ToInt32(ConfigurationManager.AppSettings["AgentCode"]);
                ViewBag.ProducerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ProducerCode"]);
                ViewBag.EmployeeCode = Convert.ToInt32(ConfigurationManager.AppSettings["EmployeeCode"]);
                ViewBag.ReinsurerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ReinsurerCode"]);
                ViewBag.TradeConsultant = Convert.ToInt32(ConfigurationManager.AppSettings["TradeConsultant"]);
                ViewBag.ContractorCode = Convert.ToInt32(ConfigurationManager.AppSettings["ContractorCode"]);

                // Variables para la edición de Asiento Diario
                ViewBag.BranchId = TempData["BranchId"] ?? 0;
                ViewBag.SalePointId = TempData["SalePointId"] ?? 0;
                ViewBag.CompanyId = TempData["CompanyId"] ?? 0;
                ViewBag.GenerationDate = TempData["GenerationDate"] ?? 0;
                ViewBag.PersonTypeId = TempData["PersonTypeId"] ?? 0;
                ViewBag.DocumentNumber = TempData["DocumentNumber"] ?? "";
                ViewBag.Name = TempData["Name"] ?? "";
                ViewBag.BeneficiaryId = TempData["BeneficiaryId"] ?? 0;
                ViewBag.PreliquidationId = TempData["PreliquidationId"] ?? 0;
                ViewBag.Description = TempData["Description"] ?? "";
                ViewBag.Comments = TempData["Comments"] ?? "";
                ViewBag.JournalEntryId = TempData["JournalEntryId"] ?? 0;
                ViewBag.IsEdit = TempData["IsEdit"] ?? "False";
                ViewBag.ApplyCollecId = TempData["ApplyCollecId"] ?? 0;
                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;

                // Se deshabilita check de primas en depósito en la modificaciòn del importe a pagar(Primas por cobrar) no aplica en BE
                ViewBag.DepositPrimes = Convert.ToString(ConfigurationManager.AppSettings["DepositPrimes"]);

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                //Tipo Imputación
                ViewBag.ImputationType = ConfigurationManager.AppSettings["ImputationTypeJournalEntry"];

                //Estados de Asientos de Diario
                ViewBag.StatusApplied = ConfigurationManager.AppSettings["JournalEntryStatusApplied"];

                //Obtiene si es el uso del tercero en 1 TRUE 0 FALSE
                ViewBag.ThirdAccountingUsed = (int)DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ThirdAccountingUsed"])).NumberParameter;

                return View("~/Areas/Accounting/Views/JournalEntry/MainJournalEntry.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        #endregion

        #region Actions

        /// <summary>
        /// TempIdJournal
        /// Inserta un registro en la tabla ACC.TEMP_JOURNAL_ENTRY
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>int</returns>
        public int TempIdJournal(int journalEntryId)
        {
            int tempJournalId;
            JournalEntryDTO journalEntry = new JournalEntryDTO();
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            journalEntry.Id = journalEntryId;
            journalEntry.AccountingDate = accountingDate;
            journalEntry.Branch = new SCRDTO.BranchDTO() { Id = 0 };
            journalEntry.Comments = "";
            journalEntry.Company = new CompanyDTO() { IndividualId = 0 };
            journalEntry.Description = "";
            journalEntry.Payer = new IndividualDTO() { IndividualId = 0 };
            journalEntry.Payer.Name = User.Identity.Name;
            journalEntry.PersonType = new PersonTypeDTO() { Id = 0 };
            journalEntry.SalePoint = new SalePointDTO() { Id = 0 };
            journalEntry.Status = 0;

            journalEntry = TempJournalEntry(journalEntry);
            tempJournalId = journalEntry.Id;

            return tempJournalId;
        }

        /// <summary>
        /// SaveTempJournalEntry
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>JournalEntry</returns>
        public JsonResult SaveTempJournalEntry(int journalEntryId)
        {
            JournalEntryDTO journalEntry = new JournalEntryDTO();

            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            journalEntry.Id = journalEntryId;
            journalEntry.AccountingDate = accountingDate;
            journalEntry.Branch = new SCRDTO.BranchDTO() { Id = 0 };
            journalEntry.Comments = "";
            journalEntry.Company = new CompanyDTO() { IndividualId = 0 };
            journalEntry.Description = "";
            journalEntry.Payer = new IndividualDTO() { IndividualId = 0 };
            journalEntry.Payer.Name = User.Identity.Name;
            journalEntry.PersonType = new PersonTypeDTO() { Id = 0 };
            journalEntry.SalePoint = new SalePointDTO() { Id = 0 };
            journalEntry.Status = 0;

            return Json(DelegateService.accountingApplicationService.SaveTempJournalEntry(journalEntry), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// TempJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntryDTO TempJournalEntry(JournalEntryDTO journalEntry)
        {
            return DelegateService.accountingApplicationService.SaveTempJournalEntry(journalEntry);
        }

        /// <summary>
        /// LoadApplicationReceipt
        /// </summary>
        /// <param name="receiptNumber"></param>
        /// <param name="depositer"></param>
        /// <param name="amount"></param>
        /// <param name="localAmount"></param>
        /// <param name="branch"></param>
        /// <param name="incomeConcept"></param>
        /// <param name="postedValue"></param>
        /// <param name="description"></param>
        /// <param name="accountingDate"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadApplicationReceipt(string receiptNumber, string depositer, string amount,
                                                            string localAmount, string branch, string incomeConcept,
                                                            string postedValue, string description, string accountingDate,
                                                            string tempImputationId)
        {
            TempData["ReceiptNumber"] = receiptNumber;
            TempData["Depositer"] = depositer;
            TempData["Amount"] = amount;
            TempData["LocalAmount"] = localAmount;
            TempData["Branch"] = branch;
            TempData["IncomeConcept"] = incomeConcept;
            TempData["PostedValue"] = postedValue;
            TempData["Description"] = description;
            TempData["AccountingDate"] = accountingDate;
            TempData["TempImputationId"] = tempImputationId;
            TempData["ReceiptTitle"] = @Global.ApplicationReceiptTitle + tempImputationId;

            return RedirectToAction("MainApplicationReceipt");
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
        /// LoadPrefix
        /// Obtiene las ramos existentes en la bdd
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadPrefix()
        {
            return new UifSelectResult(_commonController.LoadPrefix());
        }

        /// <summary>
        /// UpdateTempJournalEntry
        /// Actualiza un registro existente en la tabla ACC.TEMP_JOURNAL_ENTRY
        /// </summary>
        /// <param name="journalEntryModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateTempJournalEntry(JournalEntryModel journalEntryModel)
        {
            try
            {
                int journalEntryTransactionId = 0;
                JournalEntryDTO journalEntryTransaction = new JournalEntryDTO();
                BillingController billingController = new BillingController();

                journalEntryTransaction.Id = journalEntryModel.JournalEntryItemId;
                journalEntryTransaction.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                journalEntryTransaction.Branch = new SCRDTO.BranchDTO() { Id = journalEntryModel.BranchId };
                journalEntryTransaction.Comments = journalEntryModel.Comments;
                journalEntryTransaction.Company = new CompanyDTO() { IndividualId = journalEntryModel.CompanyId };
                journalEntryTransaction.Description = journalEntryModel.Description;
                journalEntryTransaction.Payer = new IndividualDTO() { IndividualId = journalEntryModel.IndividualId };
                journalEntryTransaction.PersonType = new PersonTypeDTO() { Id = journalEntryModel.PersonTypeId };
                journalEntryTransaction.SalePoint = new SalePointDTO() { Id = journalEntryModel.SalePointId };
                journalEntryTransaction.Status = journalEntryModel.StatusId;
                journalEntryTransaction = DelegateService.accountingApplicationService.UpdateTempJournalEntry(journalEntryTransaction);
                journalEntryTransactionId = journalEntryTransaction.Id;

                return Json(new { success = true, code = journalEntryTransactionId }, JsonRequestBehavior.AllowGet);
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
        /// SaveJournalEntryApplication
        /// Inserta un registro en la tabla BILL.TEMP_JOURNAL_ENTRY
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="statusId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveJournalEntryApplication(int tempJournalEntryId, int tempImputationId,
                                                      int imputationTypeId, int statusId)
        {
            try
            {
                string journalEntryMessage = "";
                bool isEnabledGeneralLedger = true;
                CollectApplicationDTO collectImputation = new CollectApplicationDTO();

                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                CollectDTO collectJurnalEntry = new CollectDTO()
                {
                    Transaction = new TransactionDTO() { Id = tempJournalEntryId },
                    UserId = userId
                };

                collectImputation.Application = new ApplicationDTO() { Id = tempImputationId, ModuleId = imputationTypeId };
                collectImputation.Collect = collectJurnalEntry;
                collectImputation.Id = tempJournalEntryId;
                collectImputation = DelegateService.accountingCollectService.SaveCollectImputation(collectImputation, -1, true);
                int journalEntryId = collectImputation.Transaction.Id;

                #region Accounting

                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    journalEntryMessage = RecordJournalEntry(journalEntryId, userId);
                }
                else
                {
                    journalEntryMessage = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                    isEnabledGeneralLedger = false;
                }

                #endregion Accounting

                //Para Obtener el número de recibo del Asiento de Diario
                CollectDTO collect = new CollectDTO();
                collect.Id = DelegateService.accountingCollectService.GetCollectIdByJournalEntryId(journalEntryId);

                var journalEntryResponse = new
                {
                    JournalEntryId = Convert.ToString(journalEntryId),
                    Message = Convert.ToString(journalEntryMessage),
                    IsEnabledGeneralLedger = Convert.ToString(isEnabledGeneralLedger),
                    ReceiptNumber = Convert.ToString(collect.Id),
                    TechnicalTransaction = Convert.ToString(collectImputation.Transaction.TechnicalTransaction)
                };

                return Json(journalEntryResponse, JsonRequestBehavior.AllowGet);
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
        /// SaveJournalEntryApplication
        /// Inserta un registro en la tabla BILL.TEMP_JOURNAL_ENTRY
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="statusId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveRealJournalEntryApplication(int tempJournalEntryId, int tempImputationId,
                                                      int imputationTypeId, int statusId)
        {
            try
            {
                string journalEntryMessage = "";
                bool isEnabledGeneralLedger = true;

                CollectApplicationDTO journalentryApplication = new CollectApplicationDTO(); 
                int userId = _commonController.GetUserIdByName(User.Identity.Name);
                journalentryApplication = DelegateService.accountingApplicationService.SaveApplicationRequestJournalEntry(tempImputationId, userId, tempJournalEntryId);
                int journalEntryId = journalentryApplication.Transaction.Id;

                #region Accounting

                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    journalEntryMessage = RecordJournalEntry(journalEntryId, userId);
                }
                else
                {
                    journalEntryMessage = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                    isEnabledGeneralLedger = false;
                }

                #endregion Accounting

                //Para Obtener el número de recibo del Asiento de Diario
                CollectDTO collect = new CollectDTO();
                collect.Id = DelegateService.accountingCollectService.GetCollectIdByJournalEntryId(journalEntryId);

                var journalEntryResponse = new
                {
                    JournalEntryId = Convert.ToString(journalEntryId),
                    Message = Convert.ToString(journalEntryMessage),
                    IsEnabledGeneralLedger = Convert.ToString(isEnabledGeneralLedger),
                    ReceiptNumber = Convert.ToString(collect.Id),
                    TechnicalTransaction = Convert.ToString(journalentryApplication.Transaction.TechnicalTransaction)
                };

                return Json(journalEntryResponse, JsonRequestBehavior.AllowGet);
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
        /// RecordJournalEntry
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordJournalEntry(int journalEntryId, int userId)
        {
            string imputationMessage = "";

            int moduleDateId = 0;
            int moduleId = 0;
            int subModuleId = 0;

            try
            {
                List<SCRDTO.ImputationParameterDTO> imputationParameters = DelegateService.accountingAccountService.GetImputationParameters(journalEntryId,
                                       Convert.ToInt32(@Global.ImputationTypeJournalEntry), userId, moduleId, subModuleId, moduleDateId);
                imputationMessage = _billingController.RecordImputation(imputationParameters, userId, 0);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
            return imputationMessage;
        }

        #endregion

        #region TempImputation

        /// <summary>
        /// SaveTempImputation
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempApplication(int imputationTypeId, int sourceCode, int individualId)
        {
            ApplicationDTO application = new ApplicationDTO();
            int imputationId = 0;
            DateTime registerDate = DateTime.Now;

            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            application.Id = imputationId;
            application.RegisterDate = registerDate;
            application.ModuleId = imputationTypeId;
            application.IndividualId = individualId;    
            application.UserId = userId;
            application.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            return Json(DelegateService.accountingApplicationService.SaveTempApplication(application, sourceCode), JsonRequestBehavior.AllowGet);
            //return Json(DelegateService.accountingApplicationService.SaveTempApplication(application, sourceCode), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ReceiptApplication

        /// <summary>
        /// UpdateReceiptApplication
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceCode"> </param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateReceiptApplication(int imputationId, int imputationTypeId,
                                                   int sourceCode, string comments, int statusId)
        {
            string accountingDate;

            ApplicationDTO application = new ApplicationDTO();
            CollectDTO CollectModel = new CollectDTO();

            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            application.Id = Convert.ToInt32(imputationId);
            application.RegisterDate = DateTime.Now;
            application.ModuleId = imputationTypeId;
            application.UserId = userId;

            if (imputationTypeId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                //Actualiza el estado de Bill
                accountingDate = Convert.ToString(DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now));

                string[] accountingDateSplit = accountingDate.Split();

                CollectModel.Id = Convert.ToInt32(sourceCode);
                CollectModel.Date = Convert.ToDateTime(accountingDateSplit[0]);
                CollectModel.UserId = userId;
                CollectModel.Status = statusId;
                CollectModel.Comments = comments == "" ? null : comments;
                CollectModel.Transaction = new TransactionDTO();

                DelegateService.accountingCollectService.UpdateCollect(CollectModel, -1);
            }

            return Json(DelegateService.accountingApplicationService.UpdateTempApplication(application, sourceCode), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetReceiptApplicationInformation
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReceiptApplicationInformation(int billId)
        {
            List<SCRDTO.SearchCollectDTO> searchCollects = DelegateService.accountingCollectService.GetReceiptApplicationInformation(billId);

            return Json(searchCollects, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateBillStatus
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="billCode"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateBillStatus(int imputationId, int billCode, string comments, int statusId)
        {
            string accountingDate;

            ApplicationDTO imputation = new ApplicationDTO();
            CollectDTO collectModel = new CollectDTO();

            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            imputation.Id = Convert.ToInt32(imputationId);
            imputation.RegisterDate = DateTime.Now;
            imputation.UserId = userId;

            //Actualiza el estado de bill
            accountingDate = Convert.ToString(DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now));
            string[] accountingDateSplit = accountingDate.Split();

            collectModel.Id = Convert.ToInt32(billCode);
            collectModel.Date = Convert.ToDateTime(accountingDateSplit[0]);
            collectModel.UserId = userId;
            collectModel.Status = statusId;
            collectModel.Transaction = new TransactionDTO();

            return Json(DelegateService.accountingCollectService.UpdateCollect(collectModel, -1), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ReverseImputation

        /// <summary>
        /// GetPremiumRecievableAppliedByBillId
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPremiumRecievableAppliedByBillId(int billId, int imputationTypeId)
        {
            try
            {
                List<SCRDTO.PremiumReceivableItemDTO> premiumReceivableItems =
                DelegateService.accountingApplicationService.GetPremiumRecievableAppliedByCollectId(billId, imputationTypeId).OrderBy(o => o.PremiumReceivableItemId).ToList();

                return new UifTableResult(premiumReceivableItems);
            }
            catch (Exception)
            {
                return new UifTableResult(new object());
            }
        }

        #endregion

        #region TemporaryJournalEntry

        /// <summary>
        /// GetTempJournalEntryByTempId
        /// Obtiene el registro temporal de un asiento de diario dado el id
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempJournalEntryByTempId(int tempJournalEntryId)
        {
            SCRDTO.TempJournalEntryDTO tempJournalEntry = DelegateService.accountingApplicationService.GetTempJournalEntryByTempId(tempJournalEntryId);

            return Json(tempJournalEntry, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadJournalEntry
        /// Método para recuperar asientos contables
        /// </summary>
        /// <param name="generationDate"></param>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="companyId"></param>
        /// <param name="personTypeId"></param>
        /// <param name="documentNumber"></param>
        /// <param name="name"></param>
        /// <param name="beneficiaryId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="description"></param>
        /// <param name="comments"></param>
        /// <param name="journalEntryId"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadJournalEntry(string generationDate, string branchId, string salePointId,
                                                      string companyId, string personTypeId, string documentNumber,
                                                      string name, string beneficiaryId, string tempImputationId,
                                                      string description, string comments, string journalEntryId)
        {
            TempData["GenerationDate"] = generationDate;
            TempData["BranchId"] = branchId;
            TempData["SalePointId"] = salePointId;
            TempData["CompanyId"] = companyId;
            TempData["PersonTypeId"] = personTypeId;
            TempData["DocumentNumber"] = documentNumber;
            TempData["Name"] = name;
            TempData["BeneficiaryId"] = beneficiaryId;
            TempData["TempImputationId"] = tempImputationId;
            TempData["Description"] = description;
            TempData["Comments"] = comments;
            TempData["JournalEntryId"] = journalEntryId;
            TempData["IsEdit"] = "True";//para que reconozca el script

            return RedirectToAction("MainJournalEntry");
        }

        #endregion

        #region CreditNote

        /// <summary>
        /// MainCreditNotes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCreditNotes()
        {
            ViewBag.Branch = _commonController.LoadBranch(User.Identity.Name);
            ViewBag.Prefix = _commonController.LoadPrefix();

            ViewBag.Pos = _commonController.GetSalesPointByBranchId(0);
            ViewBag.SalesPoint = _commonController.GetSalesPointByBranchId(0);
            ViewBag.PersonType = _commonController.LoadPersonType();
            ViewBag.LineBusiness = _commonController.GetTechnicalPrefixes();
            ViewBag.SubPrefix = _commonController.GetTechnicalSubPrefixesByPrefixId(0);
            ViewBag.ContractStretch = _commonController.GetContractStretchs(0);

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

            //Recupera fecha contable
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
            ViewBag.DateAccounting = _commonController.DateFormat(accountingDate, 1).Substring(0, 10);
            ViewBag.BankReconciliation = _commonController.LoadBankReconciliations();

            ViewBag.ParameterMulticompany = _billingController.GetParameterMulticompany();
            ViewBag.AccountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));
            int branchUserDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
            ViewBag.BranchUserDefault = branchUserDefault;
            ViewBag.SalePointBranchUserDefault = DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(_commonController.GetUserIdByName(User.Identity.Name), branchUserDefault);

            return View("MainCreditNotes");
        }

        ///<summary>
        /// GenerateCreditNoteRequest
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="journalEntryModel"></param>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="prefixId"></param>
        ///<returns>JsonResult</returns>
        public JsonResult GenerateCreditNoteRequest(JournalEntryModel journalEntryModel, string policyDocumentNumber, int prefixId)
        {
            try
            {
                bool isGenerateCreditNote = false;
                JournalEntryDTO journalEntry = new JournalEntryDTO();
                journalEntry.Id = journalEntryModel.JournalEntryItemId;
                journalEntry.AccountingDate = journalEntryModel.AccountingDate;
                journalEntry.Branch = new SCRDTO.BranchDTO() { Id = journalEntryModel.BranchId };
                journalEntry.Comments = journalEntryModel.Comments == null ? "" : journalEntryModel.Comments;
                journalEntry.Company = new CompanyDTO() { IndividualId = journalEntryModel.CompanyId };
                journalEntry.Description = journalEntryModel.Description == null ? "" : journalEntryModel.Description;
                journalEntry.Payer = new IndividualDTO() { IndividualId = journalEntryModel.IndividualId };
                journalEntry.PersonType = new PersonTypeDTO() { Id = journalEntryModel.PersonTypeId };
                journalEntry.SalePoint = new SalePointDTO();
                journalEntry.SalePoint.Id = journalEntryModel.SalePointId;
                journalEntry.Status = journalEntryModel.StatusId;

                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                string branchCode;
                if (journalEntryModel.BranchId != -1)
                {
                    branchCode = Convert.ToString(journalEntryModel.BranchId);
                }
                else
                {
                    branchCode = "";
                }

                string prefixCode;
                if (prefixId != -1)
                {
                    prefixCode = Convert.ToString(prefixId);
                }
                else
                {
                    prefixCode = "";
                }

                List<JournalEntryDTO> journalEntries = DelegateService.accountingApplicationService.GenerateCreditNoteRequest(journalEntry, policyDocumentNumber, branchCode, prefixCode, userId);

                if (journalEntries.Count > 0)
                {
                    isGenerateCreditNote = true;
                }

                #region Accounting

                //Se dispara la contabilización de la nota de crédito.
                foreach (JournalEntryDTO journalEntryItem in journalEntries)
                {
                    RecordJournalEntry(journalEntryItem.Id, userId);
                }

                #endregion Accounting

                return Json(isGenerateCreditNote, JsonRequestBehavior.AllowGet);
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

        ///<summary>
        /// ValidateCreditNoteGeneration
        /// Alejo
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="branchId"> </param>
        /// <param name="prefixId"></param>
        ///<returns>JsonResult</returns>
        public JsonResult ValidateCreditNoteGeneration(string policyDocumentNumber, string branchId, string prefixId)
        {
            bool existsCreditNote = DelegateService.accountingApplicationService.ValidateCreditNoteGeneration(policyDocumentNumber, branchId, prefixId);

            return Json(existsCreditNote, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}