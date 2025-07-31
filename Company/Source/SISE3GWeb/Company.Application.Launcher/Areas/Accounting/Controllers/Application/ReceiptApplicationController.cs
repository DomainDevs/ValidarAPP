//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
//Sistran Core
using DTOs = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using GeneralLedgerDTOs = Sistran.Core.Application.GeneralLedgerServices.DTOs;
//Sistran Company
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class ReceiptApplicationController : Controller
    {
        #region Instance Variables
        readonly CommonController _commonController = new CommonController();
        readonly BillingController _billingController = new BillingController();
        #endregion

        #region Views

        /// <summary>
        /// MainApplicationReceipt
        /// Invoca a la vista MainApplicationReceipt
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainApplicationReceipt()
        {
            try
            {

                //Recupera fecha contable
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = TempData["AccountingDate"] ?? _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.RedirectToPage = TempData["RedirectToPage"] ?? "";
                ViewBag.ApplyCollecId = TempData["ApplyCollecId"] ?? 0;
                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;
                ViewBag.TransactionNumber = TempData["TransactionNumber"] ?? 0;
                ViewBag.Depositer = TempData["Depositer"] ?? "";
                ViewBag.Amount = TempData["Amount"] ?? 0;
                ViewBag.LocalAmount = TempData["LocalAmount"] ?? 0;
                ViewBag.Branch = TempData["Branch"] ?? "";
                ViewBag.IncomeConcept = TempData["IncomeConcept"] ?? "";
                ViewBag.PostedValue = TempData["PostedValue"] ?? "";
                ViewBag.Description = TempData["Description"] ?? "";
                ViewBag.Comments = TempData["Comments"] ?? "";
                ViewBag.TempImputationId = TempData["TempImputationId"] ?? 0;
                ViewBag.ReceiptTitle = TempData["ReceiptTitle"] ?? "";
                ViewBag.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                ViewBag.UserNick = User.Identity.Name;
                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
                ViewBag.AccountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name)) <= 0 ? 1 : DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));
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

                // Se deshabilita check de primas en depósito en la modificaciòn del importe a pagar(Primas por cobrar) no aplica en BE
                ViewBag.DepositPrimes = Convert.ToString(ConfigurationManager.AppSettings["DepositPrimes"]);

                // TIPO IMPUTACIÓN
                ViewBag.ImputationType = ConfigurationManager.AppSettings["ImputationTypeBill"];
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                //Obtiene si es el uso del tercero en 1 TRUE 0 FALSE
                ViewBag.ThirdAccountingUsed = (int)DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ThirdAccountingUsed"])).NumberParameter;

                return View("~/Areas/Accounting/Views/Application/ReceiptApplication/MainApplicationReceipt.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetApplicationIdByTechnicalTransaction
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetApplicationIdByTechnicalTransaction(int technicalTransaction)
        {
            int ApplicationId = DelegateService.accountingApplicationService.GetApplicationIdByTechnicalTransaction(technicalTransaction);
            return Json(ApplicationId, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadApplicationReceipt
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="incomeConcept"></param>
        /// <param name="pagetoredirect"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadApplicationReceipt(string incomeConcept, string technicalTransaction, string pagetoredirect)
        {
            if ((int)GetApplicationIdByTechnicalTransaction(Convert.ToInt32(technicalTransaction)).Data > 0)
            {
                if (pagetoredirect == Global.ToRedirectMainbillSearch)
                {
                    return RedirectToAction("MainBillSearch", "BillSearch");
                }
                if (pagetoredirect == Global.ToRedirectMainbill)
                {
                    return RedirectToAction("MainBilling", "Billing");
                }
            }

            int userId = SessionHelper.GetUserId();
            DTOs.CollectDTO collect = DelegateService.accountingCollectService.GetCollectByTechnicalTransaction(Convert.ToInt32(technicalTransaction));
            
            DTOs.CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(userId, Convert.ToInt32(collect.Branch.Id), Convert.ToDateTime(DateTime.Now).Date, Convert.ToInt32(CollectControlStatus.Open));
            
            if (collect != null)
            {
                ApplicationDTO application = new ApplicationDTO();
                TempData["TransactionNumber"] = Convert.ToInt32(technicalTransaction);
                TempData["Depositer"] = collect.Payer.IdentificationDocument.Number + " - " + collect.Payer.Name;
                TempData["Amount"] = collect.PaymentsTotal.Value;
                TempData["LocalAmount"] = collect.PaymentsTotal.Value;
                TempData["Branch"] = collect.Branch.Id;
                TempData["IncomeConcept"] = incomeConcept;
                TempData["PostedValue"] = "$0.00";//postedValue;
                TempData["Description"] = collect.Description;
                DateTime accountingDate = Convert.ToDateTime(collectControl.AccountingDate);
                TempData["AccountingDate"] = _commonController.DateFormat(accountingDate, 1);
                TempData["RedirectToPage"] = pagetoredirect;

                application = DelegateService.accountingApplicationService.GetTempApplicationBySourceCode((int)ApplicationTypes.Collect, collect.Id);
                if (application == null
                    || application.Id <= 0)
                {
                    int imputationId = 0;
                    DateTime registerDate = DateTime.Now;

                    

                    application.Id = imputationId;
                    application.RegisterDate = registerDate;
                    application.ModuleId = (int)ApplicationTypes.Collect;

                    application.IndividualId = collect.Payer.IndividualId;
                    application.UserId = userId;
                    application.AccountingDate = accountingDate;

                    application = DelegateService.accountingApplicationService.SaveTempApplication(application, collect.Id);
                    if (application == null)
                    {
                        application = new ApplicationDTO()
                        {
                            Id = 0
                        };
                    }
                }

                TempData["TempImputationId"] = application.Id;
                TempData["ReceiptTitle"] = @Global.ApplicationReceiptTitle + ": " + application.Id;
                TempData["Comments"] = "";//comments;
                TempData["BranchId"] = collect.Branch.Id;
                TempData["ApplyCollecId"] = collect.Id;
            }
            return RedirectToAction("MainApplicationReceipt");
        }

        /// <summary>
        /// PaymentRequestClaimsMovements
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentRequestClaimsMovements()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Application/PaymentClaims/PaymentRequestClaimsMovements.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region TempImputation

        /// <summary>
        /// SaveTempImputation
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempImputation(int imputationTypeId, int sourceCode, int individualId)
        {
            ApplicationDTO application = new ApplicationDTO();

            int imputationId = 0;
            DateTime registerDate = DateTime.Now;

            int userId = SessionHelper.GetUserId();

            application.Id = imputationId;
            application.RegisterDate = registerDate;
            application.ModuleId = imputationTypeId;
            application.IndividualId = individualId;
            application.UserId = userId;
            application.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            return Json(DelegateService.accountingApplicationService.SaveTempApplication(application, sourceCode), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetTempImputationBySourceCode
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempImputationBySourceCode(int imputationTypeId, int sourceCode)
        {
            return Json(DelegateService.accountingApplicationService.GetTempApplicationBySourceCode(imputationTypeId, sourceCode), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ReceiptApplication

        /// <summary>
        /// SaveReceiptApplication
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveReceiptApplication(int sourceCode, int tempImputationId, int imputationTypeId,
                                                 string comments, int statusId, int individualId)
        {
            try
            {
                string technicalTransaction = "";
                string recordImputationMessage = "";
                bool isEnabledGeneralLedger = true;
                DTOs.CollectDTO collect = new DTOs.CollectDTO();
                DTOs.CollectApplicationDTO collectImputation = new DTOs.CollectApplicationDTO();
                int userId = SessionHelper.GetUserId();

                DTOs.TransactionDTO transaction = new DTOs.TransactionDTO() { TechnicalTransaction = sourceCode };

                ApplicationDTO application = new ApplicationDTO()
                {
                    Id = tempImputationId,
                    AccountingDate = DateTime.Now,
                    RegisterDate = DateTime.Now,
                    ModuleId = imputationTypeId
                };

                //collect.Id = sourceCode;
                collect.Comments = comments;
                collect.Status = statusId;
                collect.UserId = userId;
                collect.Transaction = transaction;
                collect.AccountingCompany = new DTOs.CompanyDTO();
                collect.Payer = new DTOs.PersonDTO() { IndividualId = individualId };
                collectImputation.Collect = collect;
                collectImputation.Application = application;
                collectImputation.Transaction = transaction;
                // Internamente actualiza el estado de Bill
                collectImputation = DelegateService.accountingCollectService.SaveCollectImputation(collectImputation, 0, true);

                int imputationId = collectImputation.Application.Id;

                #region Accounting

                // Disparo la contabilización de la aplicación
                if (imputationId > 0)
                {
                    technicalTransaction = Convert.ToString(collectImputation.Transaction.TechnicalTransaction);

                    if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                    {
                        recordImputationMessage = RecordBillApplication(sourceCode, userId);
                    }
                    else
                    {
                        isEnabledGeneralLedger = false;
                    }
                }
                // DESPLIEGA MSJ DE ERROR EN PASO A REALES IMPUTACIÓN 
                else
                {
                    isEnabledGeneralLedger = false;
                }

                #endregion Accounting

                var applicationResponse = new
                {
                    TechnicalTransaction = technicalTransaction,
                    Message = recordImputationMessage,
                    IsEnabledGeneralLedger = isEnabledGeneralLedger
                };

                return Json(applicationResponse, JsonRequestBehavior.AllowGet);
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
        /// UpdateReceiptApplication
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceCode"> </param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="imputationTypeId"> </param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateReceiptApplication(int imputationId, int imputationTypeId, int sourceCode, string comments, int statusId)
        {
            string accountingDate;
            ApplicationDTO application = new ApplicationDTO();
            DTOs.CollectDTO collect = new DTOs.CollectDTO();

            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            application.Id = Convert.ToInt32(imputationId);
            application.RegisterDate = DateTime.Now;
            switch (imputationTypeId)
            {
                case 1:
                    application.ModuleId = (int)ApplicationTypes.Collect;
                    break;
                case 2:
                    application.ModuleId = (int)ApplicationTypes.JournalEntry;
                    break;
                case 3:
                    application.ModuleId = (int)ApplicationTypes.PreLiquidation;
                    break;
                case 4:
                    application.ModuleId = (int)ApplicationTypes.PaymentOrder;
                    break;
                default:
                    application.ModuleId = (int)ApplicationTypes.Collect;
                    break;
            }
            application.UserId = userId;

            if (imputationTypeId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                // SE ACTUALIZA ESTADO DEL RECIBO
                accountingDate = Convert.ToString(DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now));
                string[] accountingDateSplit = accountingDate.Split();

                collect.Id = Convert.ToInt32(sourceCode);
                collect.Date = Convert.ToDateTime(accountingDateSplit[0]);
                collect.UserId = userId;
                collect.Status = statusId;
                collect.Comments = comments == "" ? null : comments;
                collect.Transaction = new DTOs.TransactionDTO();

                DelegateService.accountingCollectService.UpdateCollect(collect, -1);
            }

            return Json(new ApplicationDTO(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDebitsAndCreditsMovementTypes
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="amount"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDebitsAndCreditsMovementTypes(int tempImputationId, decimal amount)
        {
            // Validación para descartar en el caso de que existan temporales de imputación grabados con 0 (ACC.TEMP_REINSURANCE_CHECKING_ACCOUNT)
            if (tempImputationId == 0)
            {
                tempImputationId = -1;
            }

            List<object> movementTypeResponses = new List<object>();
            ApplicationDTO application = new ApplicationDTO();
            DateTime registerDate = DateTime.Now;

            application.Id = tempImputationId;
            application.RegisterDate = registerDate;
            application.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            application = DelegateService.accountingApplicationService.GetDebitsAndCreditsMovementTypes(application, Convert.ToDecimal(amount));

            string[] movementType = new string[10];
            movementType[0] = @Global.PremiumReceivableLabel;
            movementType[1] = @Global.PremiumDepositLabel;
            movementType[2] = @Global.DiscountedCommissionsLabel;
            movementType[3] = @Global.AgentCheckingAccountLabel;
            movementType[4] = @Global.CoinsuranceCheckingAccountLabel;
            movementType[5] = @Global.ReinsuranceCheckingAccountLabel;
            movementType[6] = @Global.AccountingTitle;
            movementType[7] = @Global.VariousPaymentRequest;
            movementType[8] = @Global.ClaimsPaymentRequestLabel;
            movementType[9] = @Global.Credits;

            for (int i = 0; i < application.ApplicationItems.Count; i++)
            {
                decimal debits = 0;
                decimal credits = 0;

                if ((application.ApplicationItems[i].TotalDebit != null))
                {
                    debits = System.Math.Abs(Convert.ToDecimal(application.ApplicationItems[i].TotalDebit.Value));
                }
                if (application.ApplicationItems[i].TotalCredit != null)
                {
                    credits = System.Math.Abs(Convert.ToDecimal(application.ApplicationItems[i].TotalCredit.Value));
                }

                movementTypeResponses.Add(new
                {
                    Id = i + 1,
                    MovementType = movementType[i],
                    Debits = String.Format(new CultureInfo("en-US"), "{0:C}", debits),
                    Credits = String.Format(new CultureInfo("en-US"), "{0:C}", credits)
                });
            }

            return Json(movementTypeResponses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetReceiptApplicationInformationByBillId
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReceiptApplicationInformationByBillId(int billId)
        {
            List<DTOs.Search.SearchCollectDTO> searchCollects = DelegateService.accountingCollectService.GetReceiptApplicationInformation(billId);
            return Json(searchCollects, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetReceiptApplicationInformationByBillId
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReceiptApplicationInformationByTechnicalTransaction(int technicalTransaction)
        {
            List<DTOs.Search.SearchCollectDTO> searchCollects = DelegateService.accountingCollectService.GetReceiptApplicationTechnicalTransaction(technicalTransaction);
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
            DTOs.CollectDTO collectModel = new DTOs.CollectDTO();
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            imputation.Id = Convert.ToInt32(imputationId);
            imputation.RegisterDate = DateTime.Now;
            imputation.UserId = userId;

            // SE ACTUALIZA ESTADO DEL RECIBO
            accountingDate = Convert.ToString(DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now));
            string[] accountingDateSplit = accountingDate.Split();

            collectModel.Id = Convert.ToInt32(billCode);
            collectModel.Date = Convert.ToDateTime(accountingDateSplit[0]);
            collectModel.UserId = userId;
            collectModel.Status = statusId;
            collectModel.Transaction = new DTOs.TransactionDTO();
            return Json(DelegateService.accountingCollectService.UpdateCollect(collectModel, -1), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadReceiptCancel
        /// Redirecciona a la pantalla de búsqueda de recibos
        /// </summary>
        /// <param name="receiptNumber"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadReceiptCancel(string receiptNumber)
        {
            TempData["ReceiptNumber"] = receiptNumber;
            return RedirectToAction("../BillSearch/MainBillSearch");
        }

        #endregion

        #region ReverseImputation

        /// <summary>
        /// ReverseImputationRequest
        /// Método para reversar una imputación de recibo
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        ///<returns>JsonResult</returns>
        public JsonResult ReverseImputationRequest(int sourceId, int imputationTypeId, string accountingDate)
        {
            try
            {   
                int userId = _commonController.GetUserIdByName(User.Identity.Name);
                var message = DelegateService.accountingApplicationService.ReverseApplication(sourceId, imputationTypeId, userId);
                if (message.Success && message.GeneralLedgerSuccess)
                    return Json(new { success = true, technicalTransaction = message.Code, message = message.Info }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false, message = message.Info }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, message = businessException.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, message = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetPremiumRecievableAppliedByBillIdByImputationTypeId
        /// Método para mostrar las primas que forman parte de la aplicación a ser reversada.
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="imputationTypeId"></param>
        ///<returns>JsonResult</returns>
        public JsonResult GetPremiumRecievableAppliedByBillIdByImputationTypeId(int billId, int imputationTypeId)
        {
            try
            {
                List<DTOs.Search.PremiumReceivableItemDTO> premiumReceivableItems =
                DelegateService.accountingApplicationService.GetPremiumRecievableAppliedByCollectId(billId, imputationTypeId).OrderBy(o => o.PremiumReceivableItemId).ToList();
                return new UifTableResult(premiumReceivableItems);
            }
            catch (Exception)
            {
                return new UifTableResult(new object());
            }
        }

        /// <summary>
        /// HasPremiumReceivable
        /// Método que indica si el recibo tiene primas por cobrar.
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="imputationTypeId"></param>
        ///<returns>JsonResult</returns>
        public JsonResult HasPremiumReceivable(int billId, int imputationTypeId)
        {
            bool existsPremiumReceivableItem = false;

            try
            {
                List<DTOs.Search.PremiumReceivableItemDTO> premiumReceivableItems =
                DelegateService.accountingApplicationService.GetPremiumRecievableAppliedByCollectId(billId, imputationTypeId).OrderBy(o => o.PremiumReceivableItemId).ToList();

                if (premiumReceivableItems.Count > 0)
                {
                    existsPremiumReceivableItem = true;
                }
            }
            catch (Exception)
            {
                existsPremiumReceivableItem = false;
            }

            return Json(existsPremiumReceivableItem, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BranchAndPrefix

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

        #endregion

        #region AccountApplication

        /// <summary>
        /// RecordBillApplication
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordBillApplication(int sourceId, int userId)
        {
            string recordImputationMessage = "";

            int moduleDateId = 0;
            int moduleId = 0;
            int subModuleId = 0;

            try
            {
                List<DTOs.Search.ImputationParameterDTO> imputationParameters = DelegateService.accountingAccountService.GetImputationParameters(sourceId, Convert.ToInt32(@Global.ImputationTypeBill), userId, moduleId, subModuleId, moduleDateId);
                recordImputationMessage = _billingController.RecordImputation(imputationParameters, userId, 0);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
            return recordImputationMessage;
        }

        /// <summary>
        /// ReverseImputationEntry
        /// Reversa el asiento contable asociado a la imputación
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceId"></param>
        /// <returns>int</returns>
        public int ReverseImputationEntry(ApplicationDTO imputation, int sourceId, int branchIdJournalEntry, DateTime accountingDate)
        {
            DTOs.CollectApplicationDTO collectImputation = new DTOs.CollectApplicationDTO();

            int entryHeaderId = 0;

            try
            {
                collectImputation.Collect = new DTOs.CollectDTO() { Id = sourceId };
                collectImputation.Application = new ApplicationDTO();
                collectImputation.Application = imputation;
                // Se Obtiene el id de la imputación.
                List<DTOs.CollectApplicationDTO> collectImputations = DelegateService.accountingCollectService.GetCollectImputations(collectImputation);


                int journalEntryId = 0;

                if (collectImputations.Count > 0)
                {
                    // Se realiza la reversión del asiento
                    DTOs.CollectDTO collect = new DTOs.CollectDTO();
                    if (Convert.ToInt32(imputation.ModuleId) == (int)ApplicationTypes.Collect)// Convert.ToInt32(ConfigurationManager.AppSettings["ImputationTypeBill"]))
                    {
                        journalEntryId = collectImputations[0].Transaction.TechnicalTransaction;

                    }
                    else if (Convert.ToInt32(imputation.ModuleId) == (int)ApplicationTypes.JournalEntry)// Convert.ToInt32(ConfigurationManager.AppSettings["ImputationTypeJournalEntry"]))
                    {

                        journalEntryId = collectImputations[0].Transaction.TechnicalTransaction;
                    }

                    if (journalEntryId > 0)
                    {
                        GeneralLedgerDTOs.JournalEntryDTO journalEntry = new GeneralLedgerDTOs.JournalEntryDTO();
                        journalEntry.Id = journalEntryId;
                        entryHeaderId = DelegateService.glAccountingApplicationService.ReverseJournalEntry(journalEntry);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return entryHeaderId;
        }

        public JsonResult CancelAppliationReceiptByTempImputationId(int tempImputationId)
        {
            try
            {
                bool save = DelegateService.accountingApplicationService.CancelAppliationReceipt(tempImputationId);
                return Json(new { success = true, result = save }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveApplication(int tempApplicationId)
        {
            try
            {
                int userId = _commonController.GetUserIdByName(User.Identity.Name);
                DTOs.MessageDTO message = DelegateService.accountingApplicationService.SaveApplicationByTempApplicationIdUserId(tempApplicationId, userId);
                return Json( new { success = message.Success, result = message.Info, code = message.Code, sourceCode = message.SourceCode, generalLedgerSuccess = message.GeneralLedgerSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, result = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion AccountApplication
    }
}