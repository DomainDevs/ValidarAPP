using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Helpers;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using TempCommonModels = Sistran.Core.Application.TempCommonServices.Models;
using DTOPAYENT = Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Parameters
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class ParametersController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion

        #region Instance Variables 

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region PaymentType

        /// <summary>
        /// MainPaymentType
        /// Muestra la pantalla principal de métodos de pagos
        /// </summary>
        /// <returns>ActionResult</returns>        
        public ActionResult MainPaymentType()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/PaymentType/MainPaymentType.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        /// <summary>
        /// GetPaymentTypes
        /// Llena la tabla con los tipos de pagos
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentTypes()
        {
            var paymentMethodType = DelegateService.accountingParameterService.GetPaymentMethodType();

            var paymentMethodTypes = from items in paymentMethodType
                                     select new
                                     {
                                         Id = items.PaymentTypeCode,
                                         Description = items.Description,
                                         Billing = Convert.ToBoolean(items.CollectEnabled),
                                         PaymentOrder = Convert.ToBoolean(items.EnabledPaymentOrder),
                                         InternalBallot = Convert.ToBoolean(items.EnabledTicket),
                                         PaymentRequest = Convert.ToBoolean(items.EnabledPaymentRequest)
                                     };

            return new UifTableResult(paymentMethodTypes);
        }

        /// <summary>
        /// PaymentTypeModal
        /// Lanza el modal de agreagar y editar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="internalBallot"></param>
        /// <param name="billing"></param>
        /// <param name="paymentOrder"></param>
        /// <param name="paymentRequest"></param>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentTypeModal(int id, string description, bool internalBallot,
                                             bool billing, bool paymentOrder, bool paymentRequest)
        {
            try
            {   

                var paymentMethodType = new PaymentMethodTypeDTO();

                paymentMethodType.PaymentTypeCode = id;
                paymentMethodType.Description = description;
                paymentMethodType.CollectEnabled = (billing) ? 1 : 0;
                paymentMethodType.EnabledTicket = (internalBallot) ? 1 : 0;
                paymentMethodType.EnabledPaymentOrder = (paymentOrder) ? 1 : 0;
                paymentMethodType.EnabledPaymentRequest = (paymentRequest) ? 1 : 0;

                return PartialView("~/Areas/Accounting/Views/Parameters/PaymentType/PaymentTypeModal.cshtml", paymentMethodType);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// UpdatePaymentMethodType
        /// Guarda o actualiza un tipo de pago
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult UpdatePaymentMethodType(PaymentMethodTypeDTO model)
        {
            var updateCollect = 0;
            updateCollect = DelegateService.accountingParameterService.UpdateCollectPaymentMethodType(model.PaymentTypeCode, 1, model.EnabledTicket,
                                            model.CollectEnabled, model.EnabledPaymentOrder, model.EnabledPaymentRequest);

            return Json(updateCollect, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region IncomeConcept

        /// <summary>
        /// MainIncomeConcept
        /// Muestra la pantalla principal de conceptos de ingreso
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainIncomeConcept()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/IncomeConcept/MainIncomeConcept.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        /// <summary>
        /// GetIncomeConcepts
        /// Obtiene los conceptos de ingreso
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetIncomeConcepts()
        {
            List<object> incomeConcepts = new List<object>();
            var collectConcepts = DelegateService.accountingParameterService.GetCollectConcepts();

            foreach (CollectConceptDTO collectConcept in collectConcepts)
            {
                incomeConcepts.Add(new
                {
                    Id = collectConcept.Id,
                    Description = collectConcept.Description.ToUpper()

                });
            }

            return Json(incomeConcepts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// MainIncomeConcept
        /// Guarda un nuevo concepto de ingreso
        /// </summary>
        /// <param name="concept"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult MainIncomeConcept(CollectConceptDTO concept)
        {
            try
            {
                var collectConcept = DelegateService.accountingParameterService.SaveCollectConcept(concept);
                return Json(new { success = true, result = collectConcept.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateIncomeConcept
        /// Actualiza un registro de concepto de ingreso
        /// </summary>
        /// <param name="concept"></param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult UpdateIncomeConcept(CollectConceptDTO concept)
        {
            try
            {
                var collectConcept = DelegateService.accountingParameterService.UpdateCollectConcept(concept);
                return Json(new { success = true, result = collectConcept }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteIncomeConcept
        /// Elimina un registro de concepto de ingreso
        /// </summary>
        /// <param name="concept"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteIncomeConcept(CollectConceptDTO concept)
        {
            try
            {
                DelegateService.accountingParameterService.DeleteCollectConcept(concept);
                return Json(new { success = true, result = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region AccountBank

        /// <summary>
        /// MainAccountBank
        /// Muestra la pantalla principal de cuentas bancarias
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankAccount()
        {
            try
            {

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/Parameters/BankAccount/MainBankAccount.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// SaveBankAccount
        /// </summary>
        /// <param name="accountBankComm"></param>
        /// <returns>int</returns>
        public int SaveBankAccount(AccountBankComm accountBankComm)
        {
            if (ValidateAccountNumber(accountBankComm.Number, accountBankComm.AccountTypeId,
                accountBankComm.BranchId, accountBankComm.BankId) == 0)
            {
                BankAccountTypeDTO bankAccountType = new BankAccountTypeDTO();
                bankAccountType.Id = accountBankComm.AccountTypeId;
                bankAccountType.Description = accountBankComm.GeneralLedgerNumber;

                BankDTO bank = new BankDTO();
                bank.Id = accountBankComm.BankId;

                CurrencyDTO currency = new CurrencyDTO();
                currency.Id = accountBankComm.CurrencyId;

                DateTime? disabledDate;
                if (accountBankComm.DisabledDate == null)
                {
                    disabledDate = null;
                }
                else
                {
                    disabledDate = Convert.ToDateTime(accountBankComm.DisabledDate);
                }

                BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
                bankAccountCompany.Id = 0;
                bankAccountCompany.BankAccountType = bankAccountType;
                bankAccountCompany.Number = accountBankComm.Number;
                bankAccountCompany.Bank = bank;
                bankAccountCompany.IsEnabled = Convert.ToBoolean(accountBankComm.Enabled);
                bankAccountCompany.IsDefault = Convert.ToBoolean(accountBankComm.Default);
                bankAccountCompany.Currency = currency;
                bankAccountCompany.DisableDate = Convert.ToDateTime(disabledDate);
                bankAccountCompany.Branch = new BranchDTO();
                bankAccountCompany.Branch.Id = accountBankComm.BranchId;

                return DelegateService.accountingParameterService.SaveBankAccountCompany(bankAccountCompany).Id;
            }

            return -1;
        }

        /// <summary>
        /// UpdateBankAccount
        /// </summary>
        /// <param name="bankAccountId"></param>
        /// <param name="description"></param>
        /// <param name="disabledDate"></param>
        /// <param name="enabled"></param>
        /// <returns>int</returns>
        public int UpdateBankAccount(int bankAccountId, string description, string disabledDate, int enabled)
        {
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
            bankAccountCompany.Id = bankAccountId;
            bankAccountCompany.IsEnabled = Convert.ToBoolean(enabled);
            bankAccountCompany.Number = "-1";

            if (disabledDate == "")
            {
                disabledDate = null;
            }

            bankAccountCompany.DisableDate = Convert.ToDateTime(disabledDate);

            return DelegateService.accountingParameterService.UpdateBankAccountCompany(bankAccountCompany).Id;
        }

        /// <summary>
        /// GetBanksByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetBanksByBranchId(int branchId)
        {
            List<object> banks = new List<object>();
            var bankBranches = DelegateService.tempCommonService.GetBankBranchsByBranchId(branchId);

            foreach (TempCommonModels.BankBranch bankBranch in bankBranches)
            {
                banks.Add(new
                {
                    Id = bankBranch.Id,
                    Description = bankBranch.Description
                });
            }
            
            return new UifSelectResult(banks);
        }

        /// <summary>
        /// GetAccountBankByBranchIdBankId
        /// Obtiene las cuentas bancarias por sucursal y banco
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountBankByBranchIdBankId(int branchId, int bankId)
        {
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.Branch.Id.Equals(branchId))).ToList();

            var bankAccounts = (from bankAccountCompany in companyBankAccounts
                                select new
                                {
                                    AccountBankId = bankAccountCompany.Id,
                                    AccountTypeDescription = bankAccountCompany.BankAccountType.Description,
                                    AccountNumber = bankAccountCompany.Number,
                                    CurrencyDescription = bankAccountCompany.Currency.Description,
                                    CurrencyId = bankAccountCompany.Currency.Id,
                                    EnabledDescription = bankAccountCompany.IsEnabled ? @Global.Enabled : @Global.Disabled,                                    
                                    AccountingNumber = "",
                                    AccountingName = "",
                                    Description = "",
                                    Enabled = bankAccountCompany.IsEnabled,
                                    AccountTypeId = bankAccountCompany.BankAccountType.Id,
                                    DisableDate = bankAccountCompany.DisableDate
                                });

            return Json(bankAccounts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountBankByBranchIdBankId
        /// Obtiene las cuentas bancarias por sucursal y banco
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountBankByBranchIdBankIdReport(string query, string param)
        {
            int branchId;
            int bankId;
            string[] result = param.Split('/');

            branchId = Convert.ToInt32(result[0]);
            bankId = Convert.ToInt32(result[1]);

            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.Branch.Id.Equals(branchId))).ToList();

            var bankAccounts = (from bankAccountCompany in companyBankAccounts
                                select new
                                {
                                    AccountBankId = bankAccountCompany.Id,
                                    AccountTypeDescription = bankAccountCompany.BankAccountType.Description,
                                    AccountNumber = bankAccountCompany.Number,
                                    CurrencyDescription = bankAccountCompany.Currency.Description,
                                    CurrencyId = bankAccountCompany.Currency.Id,
                                    EnabledDescription = bankAccountCompany.IsEnabled ? @Global.Enabled : @Global.Disabled,
                                    AccountingNumber = bankAccountCompany.AccountingAccount.Number,
                                    AccountingName = bankAccountCompany.AccountingAccount.Description,
                                    Description = "",
                                    Enabled = bankAccountCompany.IsEnabled,
                                    AccountTypeId = bankAccountCompany.BankAccountType.Id
                                });

            return Json(bankAccounts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AccountBankModal
        /// Levanta un modal para editar la cuenta bancaria
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="status"></param>
        /// <param name="description"></param>
        /// <returns>ActionResult</returns>
        public ActionResult BankAccountModal(int accountBankId, int status, string description)
        {
            var model = new BankAccountModel()
            {
                BankAccountId = accountBankId,
                Enabled = status,
                Description = description.ToUpper()
            };

            return PartialView("~/Areas/Accounting/Views/Parameters/BankAccount/BankAccountModal.cshtml", model);
        }

        /// <summary>
        /// GetStatus
        /// Obtiene los estados
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetStatus()
        {
            var status = new List<object>();
            status.Add(new { Enabled = 0, EnabledDescription = @Global.Disabled.ToString() });
            status.Add(new { Enabled = 1, EnabledDescription = @Global.Enabled.ToString() });

            return new UifSelectResult(status);
        }

        /// <summary>
        /// GetAccountintAccountByNumber
        /// Obtiene los datos de la cuenta contable a partir del número
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountintAccountByNumber(string query, string filter)
        {
            try
            {
                string[] data = filter.Split('/');
                List<object> paymentConcepts = new List<object>();

                if (data.Length > 0)
                {
                    Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingAccountFilterDTO accountingAccountFilterDTO = 
                        new Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingAccountFilterDTO()
                    {
                        AccountingNumber = query,
                        BranchId = Convert.ToInt32(data[0]),
                        UserId = _commonController.GetUserIdByName(User.Identity.Name)
                    };
                    paymentConcepts = _commonController.GetAccountingConceptsByFilter(accountingAccountFilterDTO);
                }
                return Json(paymentConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetAccountintAccountByName
        /// Obtiene los datos de la cuenta contable a partir del nombre
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountintAccountByName(string query)
        {
            var accountingAccounts = DelegateService.accountingApplicationService.GetAccountingAccountByDescription(query, -1);
            var accountingAccountsResponse = (from accountingAccount in accountingAccounts
                                              select new
                                              {
                                                  accountingAccount.AccountingAccountId,
                                                  AccountingNumber = accountingAccount.AccountingNumber.Trim(),
                                                  accountingAccount.AccountingName,
                                                  accountingAccount.IsMulticurrency,
                                                  accountingAccount.DefaultCurrency
                                              });

            return Json(accountingAccountsResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CheckBookControl

        /// <summary>
        /// MainCheckBookControl
        /// Muestra la pantalla principal de control de chequeras
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCheckBookControl()
        {
            try
            {
                // Setear valor por defaul de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/Parameters/CheckBookControl/MainCheckBookControl.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainCheckBookControl
        /// Guarda un registro de control de chequeras
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveCheckBookControl(CheckBookControlModel model, int accountBankId)
        {
            try
            {
                var accountBank = new BankAccountCompanyDTO()
                {
                    Id = accountBankId,
                    Number = model.Number
                };

                var checkBookControl = new DTOPAYENT.CheckBookControlDTO()
                {
                    Id = 0,
                    IsAutomatic = Convert.ToBoolean(model.IsAutomaticId),
                    AccountBank = accountBank,
                    CheckFrom = model.CheckFrom,
                    CheckTo = model.CheckTo,
                    LastCheck = model.LastCheck,
                    Status = model.StatusId
                };

                var checkBookControlResult = DelegateService.accountingAccountsPayableService.SaveCheckBookControl(checkBookControl);

                if (checkBookControlResult == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                return Json(checkBookControlResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateCheckBookControl
        /// Actualiza el registro selecionado de control de chequeras
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult UpdateCheckBookControl(CheckBookControlModel model)
        {
            try
            {
                var accountBank = new BankAccountCompanyDTO()
                {
                    Id = model.AccountBankId
                };

                DTOPAYENT.CheckBookControlDTO checkBookControl = new DTOPAYENT.CheckBookControlDTO()
                {
                    Id = model.Id,
                    AccountBank = accountBank,
                    CheckFrom = model.CheckFrom,
                    Status = model.StatusId,
                    DisabledDate = model.DisabledDate == null ? (DateTime?)null : Convert.ToDateTime(model.DisabledDate),
                    LastCheck = model.LastCheck
                };

                var checkBookControlResult = DelegateService.accountingAccountsPayableService.SaveCheckBookControl(checkBookControl);

                if (checkBookControlResult == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                return Json(checkBookControlResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// CheckBookControlModal
        /// Levanta el modal de edicion de control de chequeras
        /// </summary>
        /// <param name="checkBookControlId"></param>
        /// <param name="checkNumberFrom"></param>
        /// <param name="accountBankId"></param>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <returns>ActionResult</returns>
        public ActionResult CheckBookControlModal(int checkBookControlId, int checkNumberFrom, int accountBankId, string date, int status)
        {
            try
            {
                var checkBookControlModel = new CheckBookControlModel()
                {
                    Id = checkBookControlId,
                    CheckFrom = checkNumberFrom,
                    StatusId = status,
                    DisabledDate = string.IsNullOrEmpty(date) ? "" : date,
                    AccountBankId = accountBankId
                };
                return PartialView("~/Areas/Accounting/Views/Parameters/CheckBookControl/CheckBookControlModal.cshtml", checkBookControlModel);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }    
        }

        /// <summary>
        /// GetCheckBookControls
        /// Obtiene las cuentas bancarias a partir de accountBankId
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GetCheckBookControls(int accountBankId)
        {
            var checkBooksControl = DelegateService.accountingAccountsPayableService.GetCheckBookControlsByAccountBankId(accountBankId);

            var checkBooksControlResponse = (from checkBookControl in checkBooksControl
                                             select new
                                             {
                                                 Id = checkBookControl.Id,
                                                 AccountBankId = checkBookControl.AccountBank.Id,
                                                 IsAutomaticId = (checkBookControl.IsAutomatic) ? 1 : 0,
                                                 IsAutomatic = (checkBookControl.IsAutomatic) ? @Global.Automatic : @Global.Manual,
                                                 CheckFrom = checkBookControl.CheckFrom,
                                                 CheckTo = checkBookControl.CheckTo,
                                                 LastCheck = checkBookControl.LastCheck,
                                                 Status = (checkBookControl.Status == 0) ? @Global.Disabled : ((checkBookControl.Status == 1) ? @Global.Enabled : @Global.Annulled),
                                                 StatusId = checkBookControl.Status,
                                                 DisabledDate = (checkBookControl.DisabledDate == new DateTime()) ? "" : String.Format("{0:dd/MM/yyyy}", checkBookControl.DisabledDate)
                                             });

            return Json(checkBooksControlResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountByBranchIdBankId
        /// Devuelve los números de cuentas bancarias de cheques a partir de la sucursal y banco
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountByBranchIdBankId(int branchId, int bankId)
        {
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.Branch.Id.Equals(branchId))).ToList();

            var bankAccounts = (from companyBankAccount in companyBankAccounts
                                select new
                                {
                                    Id = companyBankAccount.Id,
                                    Description = companyBankAccount.Number
                                });

            return new UifSelectResult(bankAccounts);
        }

        /// <summary>
        /// GetTypesCheck
        /// Obtiene los tipos de chequeras
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCheckTypes()
        {
            var checkTypes = new List<object>();
            checkTypes.Add(new { Id = 0, Description = @Global.Manual.ToString() });
            checkTypes.Add(new { Id = 1, Description = @Global.Automatic.ToString() });

            return new UifSelectResult(checkTypes);
        }

        /// <summary>
        /// GetCheckStatus
        /// Obtiene los estados de chequeras
        /// Autor: Saidel Concepcion
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCheckStatus()
        {
            var checkStatus = new List<object>();
            checkStatus.Add(new { Id = 0, Description = @Global.Disabled.ToString() });
            checkStatus.Add(new { Id = 1, Description = @Global.Enabled.ToString() });
            checkStatus.Add(new { Id = 2, Description = @Global.Annulled.ToString() });

            return new UifSelectResult(checkStatus);
        }

        /// <summary>
        /// ShowCheckBookControlReport
        /// Visualiza el reporte en formato pdf
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        public void ShowCheckBookControlReport(int branchId, int bankId)
        {
            List<CheckBookControlModel> collectDetails;

            // Obtiene el detalle chequeras
            List<CheckBookControlDTO> checks = DelegateService.accountingAccountsPayableService.GetCheckBookControlsByBankIdBranchId(bankId, branchId);

            var result = checks.Where(m => m != null);
            try
            {
                collectDetails = (from CheckBookControlDTO report in result
                                  select new CheckBookControlModel()
                                  {
                                      Id = report.CheckbookControlCode,
                                      AccountBankId = report.AccountBankCode,
                                      IsAutomatic = report.IsAutomatic,
                                      CheckFrom = report.CheckFrom,
                                      CheckTo = report.CheckTo,
                                      LastCheck = report.LastCheck,
                                      State = report.Status,
                                      DisabledDate = report.DisabledDate,
                                      Description = report.DescriptionBank,
                                      SmallDescriptionBranch = report.SmallDescriptionBranch,
                                      Number = report.Number,
                                      User = User.Identity.Name,
                                      DescriptionState = (report.Status == 0)
                                      ? @Global.Disabled : (report.Status == 1)
                                      ? @Global.Enabled : @Global.Annulled,
                                      DescriptionIsAutomatic = (report.IsAutomatic) == 0
                                      ? @Global.Manual : @Global.Automatic
                                  }).ToList();

                var reportSource = collectDetails;
                var reportName = "Areas//Accounting//Reports//CheckBookControlReport.rpt";
                var reportDocument = new ReportDocument();
                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                reportDocument.Load(reportPath);

                if (reportSource.Count > 0)
                {
                    // Llena Reporte Principal
                    reportDocument.SetDataSource(reportSource);
                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "AccountingMonthReport");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region  AccountingCompany

        /// <summary>
        /// AccountingCompany
        /// Invoca a la vista AccountingCompany
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingCompany()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/AccountingCompany/AccountingCompany.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }           
        }

        /// <summary>
        /// SaveAccountingCompany
        /// Graba en la tabla de compañías contables
        /// </summary>
        /// <param name="companyDescription"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveAccountingCompany(string companyDescription)
        {
            try
            {
                CompanyDTO accountingCompany = new CompanyDTO() { Name = companyDescription };
                DelegateService.accountingAccountsPayableService.SaveAccountingCompany(accountingCompany);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateAccountingCompany
        /// Actualiza en la tabla BILL.Accounting_Company
        /// </summary>
        /// <param name="cd"> </param>
        /// <param name="description"> </param>
        /// <returns>Company</returns>
        public JsonResult UpdateAccountingCompany(int cd, string description)
        {
            try
            {
                CompanyDTO accountingCompany = new CompanyDTO()
                {
                    IndividualId = cd,
                    Name = description
                };
                accountingCompany = DelegateService.accountingAccountsPayableService.UpdateAccountingCompany(accountingCompany);

                return Json(new { success = true, result = accountingCompany }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetAccountingCompanies
        /// Obtiene todas las compañias contables
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingCompanies()
        {
            var companies = DelegateService.accountingAccountsPayableService.GetAccountingCompanies();

            var companiesResponse = (from company in companies
                                     select new
                                     {
                                         Id = company.IndividualId,
                                         Description = company.Name,
                                         General = (company.IndividualId == -1)
                                     }).ToList();

            return Json(companiesResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAccountingCompany
        /// Elimina una compañia contable
        /// </summary>
        /// <param name="frmAccountingCompany"> </param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteAccountingCompany(int frmAccountingCompany)
        {
            bool IsDeletedAccountingCompany = false;
            IsDeletedAccountingCompany = DelegateService.accountingAccountsPayableService.DeleteAccountingCompany(frmAccountingCompany);
            return Json(IsDeletedAccountingCompany);
        }

        #endregion

        #region BankAccountsSettings

        /// <summary>
        /// BankAccountsSettings
        /// Invoca a la vista BankAccountsSettings
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BankAccountsSettings()
        {
            try
            {

                int defaultValue = Convert.ToInt16(@Global.DefaultValue);
                string defaultDescription = @Global.DefaultDescription;

                List<BankAccountsController.State> states = new List<BankAccountsController.State>();
                states.Add(new BankAccountsController.State() { Id = defaultValue, Description = defaultDescription });
                states.Add(new BankAccountsController.State() { Id = 0, Description = @Global.Disabled });
                states.Add(new BankAccountsController.State() { Id = 1, Description = @Global.Enabled });
                ViewBag.State = states;

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// SearchBankAccounts
        /// Busca cuentas por banco
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchBankAccounts(string documentNumber)
        {
            if (documentNumber != null)
            {
                var bankAccountPersons = DelegateService.accountingParameterService.GetBankAccountPersons();
                var personBankAccounts = bankAccountPersons.Where(r => (r.Individual.EconomicActivity.Description.Equals(documentNumber))).ToList();
                List<object> bankAccounts = new List<object>();
                foreach (BankAccountPersonDTO bankAccountPerson in personBankAccounts)
                {
                    bankAccounts.Add(new
                    {
                        BankAccountCode = bankAccountPerson.Id,
                        IndividualId = bankAccountPerson.Individual.IndividualId,
                        AccountTypeCode = bankAccountPerson.BankAccountType.Id,
                        AccountNumber = bankAccountPerson.Number,
                        BankCode = bankAccountPerson.Bank.Id,
                        Enabled = bankAccountPerson.IsEnabled,
                        Default = bankAccountPerson.IsDefault,
                        CurrencyCode = bankAccountPerson.Currency.Id,
                        CurrencyDescription = bankAccountPerson.Currency.Description,
                        BankDescription = bankAccountPerson.Bank.Description,
                        AccountTypeDescription = bankAccountPerson.BankAccountType.Description,
                        DocumentNumber = bankAccountPerson.Individual.EconomicActivity.Description,
                        PersonName = bankAccountPerson.Individual.Name
                    });
                }

                return new UifTableResult(bankAccounts);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAccountBank
        /// </summary>
        /// <param name="accountBank"> </param>
        /// <returns>newBankAccountPerson.Id</returns>
        public int SaveAccountBank(BankAccountPersonModel accountBank)
        {
            BankAccountTypeDTO accountType = new BankAccountTypeDTO() { Id = accountBank.AccountTypeCode };
            BankDTO bank = new BankDTO() { Id = accountBank.BankCode };
            CurrencyDTO currency = new CurrencyDTO() { Id = accountBank.CurrencyCode };
            IndividualDTO individual = new IndividualDTO() { IndividualId = accountBank.IndividualId };

            BankAccountPersonDTO bankAccount = new BankAccountPersonDTO()
            {
                Bank = bank,
                BankAccountType = accountType,
                Currency = currency,
                Id = accountBank.BankAccountCode,//autonumerico
                Individual = individual,
                IsEnabled = Convert.ToBoolean(accountBank.Enabled),
                IsDefault = Convert.ToBoolean(accountBank.Default),
                Number = accountBank.Number
            };

            return DelegateService.accountingParameterService.SaveBankAccountPerson(bankAccount).Id;
        }


        /// <summary>
        /// DeleteAccountBank
        /// Borra cuenta bancaria Persona
        /// </summary>
        /// <param name="accountBank"></param>
        /// <returns>isDeletedBankAccountPerson</returns>
        public JsonResult DeleteAccountBank(BankAccountPersonModel accountBank)
        {
            bool isDeletedBankAccountPerson = false;

            try
            {
                BankAccountTypeDTO accountType = new BankAccountTypeDTO() { Id = accountBank.AccountTypeCode };
                BankDTO bank = new BankDTO() { Id = accountBank.BankCode };
                CurrencyDTO currency = new CurrencyDTO() { Id = accountBank.CurrencyCode };
                IndividualDTO individual = new IndividualDTO() { IndividualId = accountBank.IndividualId };

                BankAccountPersonDTO bankAccountPerson = new BankAccountPersonDTO()
                {
                    Bank = bank,
                    BankAccountType = accountType,
                    Currency = currency,
                    Id = accountBank.BankAccountCode,
                    Individual = individual,
                    IsEnabled = Convert.ToBoolean(accountBank.Enabled),
                    IsDefault = Convert.ToBoolean(accountBank.Default),
                    Number = accountBank.Number
                };

                DelegateService.accountingParameterService.DeleteBankAccountPerson(bankAccountPerson);

                isDeletedBankAccountPerson = true;
            }
            catch (Exception)
            {
                isDeletedBankAccountPerson = false;
            }

            return Json(isDeletedBankAccountPerson, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// UpdateAccountBankSettings
        ///// Actualiza cuenta bancaria Persona
        ///// </summary>
        ///// <param name="accountBank"></param>
        ///// <returns>newBankAccountPerson.Id</returns>
        public int UpdateAccountBankSettings(BankAccountPersonModel accountBank)
        {
            BankAccountTypeDTO accountType = new BankAccountTypeDTO() { Id = accountBank.AccountTypeCode };
            BankDTO bank = new BankDTO() { Id = accountBank.BankCode };
            CurrencyDTO currency = new CurrencyDTO() { Id = accountBank.CurrencyCode };

            IndividualDTO individual = new IndividualDTO() { IndividualId = accountBank.IndividualId };

            BankAccountPersonDTO bankAccountPerson = new BankAccountPersonDTO()
            {
                Bank = bank,
                BankAccountType = accountType,
                Currency = currency,
                Id = accountBank.BankAccountCode,
                Individual = individual,
                IsEnabled = Convert.ToBoolean(accountBank.Enabled),
                IsDefault = Convert.ToBoolean(accountBank.Default),
                Number = accountBank.Number
            };

            return DelegateService.accountingParameterService.UpdateBankAccountPerson(bankAccountPerson).Id;
        }

        #endregion

        #region CurrencyDifference

        /// <summary>
        /// CurrencyDifference
        /// Invoca a la vista CurrencyDifference
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CurrencyDifference()
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

        /// <summary>
        /// GetCurrencyDifferences
        /// Obtiene la diferencia de moneda
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetCurrencyDifferences()
        {
            List<CurrencyDifferenceDTO> currencyDifferences = DelegateService.accountingParameterService.GetCurrencyDifferences();

            var currencyDifferencesResponse = new
            {

                total = currencyDifferences.Count,
                records = currencyDifferences,
                rows = currencyDifferences
            };

            return Json(new { aaData = currencyDifferencesResponse.rows }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveCurrencyDifference
        /// Graba la diferencia de moneda
        /// </summary>
        /// <param name="currencyCode"> </param>
        /// <param name="maxDifference"> </param>
        /// <param name="percentageDifference"> </param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCurrencyDifference(int currencyCode, decimal maxDifference,
                                                 decimal percentageDifference)
        {
            int currencySaved = DelegateService.accountingParameterService.SaveCurrencyDifference(currencyCode, maxDifference, percentageDifference);

            return Json(currencySaved, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateCurrencyDifference
        /// Actualiza la diferencia de moneda
        /// </summary>
        /// <param name="currencyCode"> </param>
        /// <param name="maxDifference"> </param>
        /// <param name="percentageDifference"> </param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateCurrencyDifference(int currencyCode, decimal maxDifference,
                                                   decimal percentageDifference)
        {
            int currencyUpdated = DelegateService.accountingParameterService.UpdateCurrencyDifference(currencyCode, maxDifference, percentageDifference);
            return Json(currencyUpdated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteCurrencyDifference
        /// Elimina la diferencia de moneda
        /// </summary>
        /// <param name="currencyCode"> </param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteCurrencyDifference(int currencyCode)
        {
            bool currencyDeleted = DelegateService.accountingParameterService.DeleteCurrencyDifference(currencyCode);

            return Json(currencyDeleted, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AssociateBankBranch

        /// <summary>
        /// MainAssociateBankBranch
        /// Muestra la pantalla principal de asociación de bancos y sucursales
        /// </summary>
        /// <returns>View</returns>
        public ActionResult MainAssociateBankBranch()
        {
            try
            {
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View("~/Areas/Accounting/Views/Parameters/AssociateBankBranch/MainAssociateBankBranch.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }     
        }

        /// <summary>
        /// GetBankBranchsByBranchId
        /// Trae la asociación de bancos y sucursales
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankBranchsByBranchId(int branchId)
        {
            List<object> bankBranchesResponses = new List<object>();
            List<TempCommonModels.BankBranch> bankBranches = DelegateService.tempCommonService.GetBankBranchsByBranchId(branchId);

            foreach (TempCommonModels.BankBranch bankBranch in bankBranches)
            {
                bankBranchesResponses.Add(new
                {
                    Id = bankBranch.Id,
                    Description = bankBranch.Description,
                    IsEnabled = bankBranch.IsEnabled,
                    IsEnabledDescription = bankBranch.IsEnabled ? @Global.Enabled : @Global.Disabled
                });
            }
            
            return Json(new { aaData = bankBranchesResponses, total = bankBranchesResponses.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveBankBranch
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isEdit"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveBankBranch(int branchId, int bankId, bool isEnabled, bool isEdit)
        {
            try
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// DeleteBankBranch
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        public void DeleteBankBranch(int branchId, int bankId)
        {
            try
            {
                BankDTO bank = new BankDTO() { Id = bankId };
                BankBranch bankBranch = new BankBranch() { Id = branchId };
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region ReportCollection

        /// <summary>
        /// RangeParametrizationDetail
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult RangeParametrizationDetail()
        {
            try
            {
                ViewBag.LimitedRange = ConfigurationManager.AppSettings["LimitedRange"];

                return View("~/Areas/Accounting/Views/Parameters/Range/RangeParametrizationDetail.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }       
        }

        /// <summary>
        /// GetRanges
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetRanges()
        {
            List<RangeDTO> ranges = DelegateService.accountingParameterService.GetRanges();

            List<object> rangeResponses = new List<object>();

            foreach (RangeDTO range in ranges)
            {
                rangeResponses.Add(new
                {
                    Id = range.Id,
                    Description = range.Description,
                    Default = range.IsDefault
                });
            }

            return Json(new { aaData = rangeResponses, total = ranges.Count }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// GetRangesItem
        /// </summary>
        /// <param name="rangeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetRangesItem(int rangeId)
        {
            RangeDTO range = new RangeDTO() { Id = rangeId };
            range = DelegateService.accountingParameterService.GetRange(range);

            var rangeResponse = from RangeItem in range.RangeItems
                                select new
                                {
                                    Id = RangeItem.Id,
                                    ColumnNumber = RangeItem.Order,
                                    DayFrom = RangeItem.RangeFrom,
                                    DayTo = RangeItem.RangeTo
                                };

            return new UifTableResult(rangeResponse);
        }

        /// <summary>
        /// GetCountRangeItem
        /// gets the number of items in a range
        /// </summary>
        /// <param name="rangeId">int</param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCountRangeItem(int rangeId)
        {
            try
            {
                RangeDTO range = new RangeDTO() { Id = rangeId };
                range = DelegateService.accountingParameterService.GetRange(range);

                return Json(range.RangeItems.Count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveRangeDebit
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult SaveRanges(RangeDTO fieldRangeModel)
        {
            try
            {
                RangeDTO range = DelegateService.accountingParameterService.SaveRange(fieldRangeModel);

                return Json(range.Id, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateRanges
        /// </summary>
        /// <param name="oRangeControl"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateRanges(RangeDTO oRangeControl)
        {
            try
            {
                RangeDTO range = DelegateService.accountingParameterService.UpdateRange(oRangeControl);

                return Json((range.Id > 0), JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// DeleteRanges
        /// </summary>
        /// <param name="rangeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteRanges(int rangeId)
        {
            try
            {
                bool isDeletedRange;
                RangeDTO oRangeControl = new RangeDTO() { Id = rangeId };
                isDeletedRange = DelegateService.accountingParameterService.DeleteRange(oRangeControl);
                return Json(isDeletedRange, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        /// <summary>
        /// DeleteRangesDetails
        /// </summary>
        /// <param name="rangeControl"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteRangesDetails(RangeItemDTO rangeControl)
        {
            try
            {
                bool isDeletedRangeItem = DelegateService.accountingParameterService.DeleteRangeItem(rangeControl);

                return Json(isDeletedRangeItem, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetSelectListRanges
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetSelectListRanges()
        {
            try
            {
                RangeDTO range;

                List<RangeDTO> Ranges = DelegateService.accountingParameterService.GetRanges();

                List<object> rangesResponses = new List<object>();

                foreach (RangeDTO rangeItem in Ranges)
                {
                    range = new RangeDTO();
                    range.Id = rangeItem.Id;
                    range = DelegateService.accountingParameterService.GetRange(range);
                    if (range.RangeItems.Count > 0)
                    {
                        rangesResponses.Add(new
                        {
                            Id = rangeItem.Id,
                            Description = rangeItem.Description
                        });
                    }
                }

                return new UifSelectResult(rangesResponses);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetDefaultRange
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDefaultRange()
        {
            List<RangeDTO> defaultRanges = DelegateService.accountingParameterService.GetRanges();

            return Json(defaultRanges, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OutputTemplate

        /// <summary>
        /// TemplateGroup
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult TemplateGroup()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/OutputTemplate/TemplateGroup.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }           
        }

        /// <summary>
        /// TemplateParametrization
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult TemplateParametrization()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/OutputTemplate/TemplateParametrization.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }           
        }

        /// <summary>
        /// TemplateFormat
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult TemplateFormat()
        {
            try
            {
                ViewBag.DateNow = DateTime.Now.ToShortDateString();

                return View("~/Areas/Accounting/Views/Parameters/OutputTemplate/TemplateFormat.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetStructureList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetStructureList()
        {
            List<FormatModule> formatModules = DelegateService.reportingService.GetFormatModules();
            var formatModulesResponse = from formatModule in formatModules
                                        select new
                                        {
                                            Id = formatModule.Id,
                                            Description = formatModule.Description
                                        };

            return new UifSelectResult(formatModulesResponse);
        }

        /// <summary>
        /// GetFileTypeList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFileTypeList()
        {
            List<object> fileTypes = new List<object>();
            fileTypes.Add(new { Id = 1, Description = FileTypes.Text.ToString() });
            fileTypes.Add(new { Id = 2, Description = FileTypes.Excel.ToString() });
            fileTypes.Add(new { Id = 3, Description = FileTypes.ExcelTemplate.ToString() });
            return new UifSelectResult(fileTypes);
        }

        #region FormatModule

        /// <summary>
        /// SaveFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveFormatModule(FormatModule formatModule)
        {
            try
            {
                var formatModuleResult = DelegateService.reportingService.SaveFormatModule(formatModule);

                return Json(new { success = true, result = formatModuleResult.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateFormatModule(FormatModule formatModule)
        {
            try
            {
                var formatModuleResponse = DelegateService.reportingService.UpdateFormatModule(formatModule);
                return Json(new { success = true, result = formatModuleResponse }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteFormatModule
        /// </summary>
        /// <param name="formatModule"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteFormatModule(FormatModule formatModule)
        {
            try
            {
                var formatModuleResponse = DelegateService.reportingService.DeleteFormatModule(formatModule);

                return Json(new { success = (formatModuleResponse.Description != "relatedObject"), result = formatModuleResponse }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetFormatModules
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetFormatModules()
        {
            List<FormatModule> formatModules = DelegateService.reportingService.GetFormatModules();

            List<object> formatModuleResponses = new List<object>();

            foreach (FormatModule items in formatModules)
            {
                formatModuleResponses.Add(new
                {
                    Id = items.Id,
                    Description = items.Description.ToUpper()
                });
            }

            return Json(formatModuleResponses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Format

        /// <summary>
        /// GetFormatItems
        /// </summary>
        /// <param name="idModule"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetFormatItems(int idModule)
        {
            FormatModule formatModule = new FormatModule();
            formatModule.Id = idModule;
            List<Format> formats = DelegateService.reportingService.GetFormatsByFormatModule(formatModule);

            var formatsResponse = from format in formats
                                  select new
                                  {
                                      Id = format.Id,
                                      Description = format.Description,
                                      FileTypeCd = Convert.ToInt32(format.FileType),
                                      FileTypeDescription = format.FileType.ToString(),
                                      CurrentFrom = format.DateFrom.ToString("dd/MM/yyyy"),
                                      CurrentTo = format.DateTo.ToString("dd/MM/yyyy")
                                  };

            return new UifTableResult(formatsResponse);
        }

        /// <summary>
        /// GetFormatSelect
        /// </summary>
        /// <param name="idStructure"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetFormatSelect(int idStructure)
        {
            FormatModule formatModule = new FormatModule();
            formatModule.Id = idStructure;
            List<Format> formats = DelegateService.reportingService.GetFormatsByFormatModule(formatModule);

            var formatsResponse = from format in formats
                                  select new
                                  {
                                      Id = format.Id,
                                      Description = format.Description,
                                  };

            return new UifSelectResult(formatsResponse);
        }

        /// <summary>
        /// SaveFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveFormat(Format format)
        {
            try
            {
                var saveFormat = DelegateService.reportingService.SaveFormat(format);

                return Json(new { success = true, result = saveFormat }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateFormat(Format format)
        {
            try
            {
                var updateFormat = DelegateService.reportingService.UpdateFormat(format);

                return Json(new { success = true, result = updateFormat }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteFormat
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteFormat(int idTemplate)
        {
            try
            {
                Format format = new Format();
                format.Id = idTemplate;
                format.FileType = FileTypes.Excel;
                var deleteFormat = DelegateService.reportingService.DeleteFormat(format);
                return Json(new { success = (deleteFormat.Description != "relatedObject"), result = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                string[] messages = businessException.GetMesssages();

                return Json(new { success = false, result = messages }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region FormatDetail

        /// <summary>
        /// GetFormatTypeList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFormatTypeList()
        {
            List<object> formatTypes = new List<object>();

            formatTypes.Add(new { Id = 1, Description = Global.ResourceManager.GetString(FormatTypes.Head.ToString()) });
            formatTypes.Add(new { Id = 2, Description = Global.ResourceManager.GetString(FormatTypes.Detail.ToString()) });
            formatTypes.Add(new { Id = 3, Description = Global.ResourceManager.GetString(FormatTypes.Summary.ToString()) });

            return new UifSelectResult(formatTypes);
        }

        /// <summary>
        /// GetFormatTypeByFormat
        /// </summary>
        /// <param name="idFormat"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetFormatTypeByFormat(int idFormat)
        {
            Format format = new Format();
            format.Id = idFormat;
            format.FileType = FileTypes.Text;

            List<FormatDetail> formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);

            var formatDetailsResult = from formatDetail in formatDetails
                                      select new
                                      {
                                          Id = formatDetail.Id,
                                          FormatTypeCd = Convert.ToInt32(formatDetail.FormatType),
                                          FormatTypeDescription = Global.ResourceManager.GetString(formatDetail.FormatType.ToString()),
                                          Separator = formatDetail.Separator
                                      };

            return new UifTableResult(formatDetailsResult);
        }

        /// <summary>
        /// SaveFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveFormatDetail(FormatDetail formatDetail)
        {
            try
            {
                formatDetail.Format.FileType = FileTypes.Text;

                var saveFormatDetail = DelegateService.reportingService.SaveFormatDetail(formatDetail);

                return Json(new { success = true, result = saveFormatDetail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateFormatDetail
        /// </summary>
        /// <param name="formatDetail"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateFormatDetail(FormatDetail formatDetail)
        {
            try
            {
                Format format = new Format();
                format.FileType = FileTypes.Text;
                formatDetail.Format = format;

                var editFormatDetail = DelegateService.reportingService.UpdateFormatDetail(formatDetail);

                return Json(new { success = true, result = editFormatDetail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DelteFormatDetail
        /// </summary>
        /// <param name="formatTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteFormatDetail(int formatTypeId)
        {
            try
            {
                FormatDetail formatDetail = new FormatDetail();
                formatDetail.Id = formatTypeId;
                Format format = new Format();
                format.FileType = FileTypes.Text;
                formatDetail.Format = format;
                formatDetail.FormatType = FormatTypes.Head;

                var deleteFormatDetail = DelegateService.reportingService.DeleteFormatDetail(formatDetail);

                return Json(new { success = true, result = deleteFormatDetail }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                string[] messages = businessException.GetMesssages();

                return Json(new { success = false, result = messages }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region FormatField

        /// <summary>
        /// GetFormatFieldByFormatTypeId
        /// </summary>
        /// <param name="formatTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetFormatFieldByFormatTypeId(int formatTypeId)
        {
            try
            {
                FormatDetail formatDetail = new FormatDetail();
                Format format = new Format();
                format.Id = formatTypeId;
                format.FileType = FileTypes.Text;
                formatDetail.FormatType = FormatTypes.Head;

                formatDetail.Format = format;

                List<FormatField> formatFields = DelegateService.reportingService.GetFormatFieldsByFormatDetail(formatDetail);

                List<object> formatFieldsResponses = new List<object>();

                foreach (FormatField formatField in formatFields)
                {
                    formatFieldsResponses.Add(new
                    {
                        Id = formatField.Id,
                        Order = formatField.Order,
                        Description = formatField.Description,
                        Start = formatField.Start,
                        Length = formatField.Length,
                        Value = formatField.Value,
                        ColumnNumber = formatField.ColumnNumber,
                        RowNumber = formatField.RowNumber,
                        Filled = formatField.Filled,
                        Align = formatField.Align,
                        Field = formatField.Field,
                        Mask = formatField.Mask,
                        IsRequired = formatField.IsRequired
                    });
                }

                return Json(new { aaData = formatFieldsResponses, total = formatFieldsResponses.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SaveFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveFormatField(FormatField formatField)
        {
            try
            {
                var saveFormatField = DelegateService.reportingService.SaveFormatField(formatField);

                return Json(new { success = true, result = saveFormatField }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateFormatField
        /// </summary>
        /// <param name="formatField"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateFormatField(FormatField formatField)
        {
            try
            {
                var updateFormatField = DelegateService.reportingService.UpdateFormatField(formatField);

                return Json(new { success = true, result = updateFormatField }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteFormatField
        /// </summary>
        /// <param name="idFieldFormat"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteFormatField(int idFieldFormat)
        {
            try
            {
                FormatField formatField = new FormatField();
                formatField.Id = idFieldFormat;
                var deletedFormatField = DelegateService.reportingService.DeleteFormatField(formatField);
                return Json(new { success = true, result = deletedFormatField }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                string[] messages = businessException.GetMesssages();
                return Json(new { success = false, result = messages }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        /// <summary>
        /// GetRowsNumber
        /// Obtiene el maximo de fila de cabecera o detalle
        /// </summary>
        /// <param name="idFormatType"></param>
        /// <param name="documentArea"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRowsNumber(int idFormatType, int documentArea)
        {
            try
            {
                int maxRowFormat = 0;

                FormatDetail formatDetail = new FormatDetail();
                Format format = new Format();
                format.Id = idFormatType;
                format.FileType = FileTypes.Text;
                formatDetail.FormatType = FormatTypes.Head;

                formatDetail.Format = format;

                List<FormatField> formatFields = DelegateService.reportingService.GetFormatFieldsByFormatDetail(formatDetail);

                if (formatFields.Count > 0)
                {
                    if (documentArea == Convert.ToInt16(FormatTypes.Head))
                    {
                        maxRowFormat = formatFields.Max(x => x.RowNumber);
                    }
                    else
                    {
                        maxRowFormat = formatFields[0].RowNumber;
                    }
                }

                return Json(new { success = true, result = maxRowFormat }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ByPrefixSubPrefix

        /// <summary>
        /// GetLineBusinessByPrefixId
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetLineBusinessByPrefixId(int prefixId)
        {
            try
            {
                List<LineBusiness> prefixes = DelegateService.commonService.GetLineBusinessByPrefixId(prefixId).
                                               OrderBy(x => x.Description).ToList();

                List<object> lineBusiness = new List<object>();
                foreach (LineBusiness prefix in prefixes)
                {
                    lineBusiness.Add(new
                    {
                        Id = prefix.Id,
                        Description = prefix.Description
                    });
                }

                return new UifSelectResult(lineBusiness);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Cancelación Automática de Pólizas

        /// <summary>
        /// MainDayPrefix
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainDayPrefix()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/CancellationPolicy/MainDayPrefix.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        /// <summary>
        /// MainExclusion
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainExclusion()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/CancellationPolicy/MainExclusion.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        #region CancellationDayPrefix

        /// <summary>
        /// SaveCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCancellationLimit(CancellationLimitDTO cancellationLimit)
        {
            try
            {
                var newCancellationLimit = DelegateService.accountingParameterService.SaveCancellationLimit(cancellationLimit);

                return Json(new { success = true, result = newCancellationLimit.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateCancellationLimit(CancellationLimitDTO cancellationLimit)
        {
            try
            {
                var updateCancellationLimit = DelegateService.accountingParameterService.UpdateCancellationLimit(cancellationLimit);

                return Json(new { success = true, result = updateCancellationLimit }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteCancellationLimit(CancellationLimitDTO cancellationLimit)
        {
            try
            {
                DelegateService.accountingParameterService.DeleteCancellationLimit(cancellationLimit);

                return Json(new { success = true, result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetCancellationLimits
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetCancellationLimits()
        {
            List<CancellationLimitDTO> cancellationLimits = DelegateService.accountingParameterService.GetCancellationLimits();

            var cancellationLimitsResponse = from cancellationLimit in cancellationLimits
                                             select new
                                             {
                                                 Id = cancellationLimit.Id,
                                                 PrefixCode = cancellationLimit.Branch.Id,
                                                 PrefixDescription = cancellationLimit.Branch.Description.ToUpper(),
                                                 Days = cancellationLimit.CancellationLimitDays
                                             };

            return Json(new { aaData = cancellationLimitsResponse, total = cancellationLimitsResponse.Count() }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CancellationExclusion

        /// <summary>
        /// LoadPersonType
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult LoadPersonType()
        {
            List<object> personTypes = new List<object>();

            personTypes.Add(new { Id = 1, Description = Global.Policy });
            personTypes.Add(new { Id = 2, Description = Global.Agent });
            personTypes.Add(new { Id = 3, Description = Global.Insured });

            return new UifSelectResult(personTypes);
        }

        /// <summary>
        /// GetPolicyId
        /// Recupera la póliza si existe
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        /// <param name="documentNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPolicyId(int branchCode, int prefixCode, string documentNumber)
        {
            int policyId = -1;

            if (DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(Convert.ToInt32(prefixCode),
                                                                                    Convert.ToInt32(branchCode),
                                                                                    Convert.ToDecimal(documentNumber)) != null)
            {
                policyId = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(Convert.ToInt32(prefixCode),
                                                                                               Convert.ToInt32(branchCode),
                                                                                               Convert.ToDecimal(documentNumber)).Endorsement.PolicyId;
            }

            return Json(new { aaData = policyId }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveExclusion
        /// </summary>
        /// <param name="exclusionType"></param>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        /// <param name="policyId"></param>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveExclusion(int exclusionType, int branchCode, int prefixCode,
                                        int policyId, int individualId)
        {
            try
            {
                ExclusionDTO exclusion = new ExclusionDTO();
                PersonDTO agent = new PersonDTO();
                PersonDTO insured = new PersonDTO();
                PolicyDTO policy = new PolicyDTO();
                BranchDTO branch = new BranchDTO();
                PrefixDTO prefix = new PrefixDTO();
                if (User != null)
                {
                    policy.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                }
                else
                {
                    policy.UserId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
                }

                if (exclusionType == 1)
                {
                    policy.Id = policyId;
                    branch.Id = branchCode;
                    prefix.Id = prefixCode;
                    policy.Branch = branch;
                    policy.Prefix = prefix;
                }
                else if (exclusionType == 2)
                {
                    agent.IndividualId = individualId;
                }
                else
                {
                    insured.IndividualId = individualId;
                }

                exclusion.Agent = agent;
                exclusion.Insured = insured;
                exclusion.Policy = policy;

                var newExclusion = DelegateService.accountingParameterService.SaveExclusion(exclusion);

                return Json(new { success = true, result = newExclusion.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteExclusion
        /// </summary>
        /// <param name="idExclusion"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteExclusion(int idExclusion)
        {
            try
            {
                ExclusionDTO exclusion = new ExclusionDTO() { Id = idExclusion };
                DelegateService.accountingParameterService.DeleteExclusion(exclusion);

                return Json(new { success = true, result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetExlusionPolicyByType
        /// CONSULTA LISTA DE ECLUSIONES
        /// </summary>
        /// <param name="type"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetExlusionPolicyByType(int type)
        {
            List<object> exclusionsResponses = new List<object>();

            ExclusionTypes exclusionType;
            exclusionType = (ExclusionTypes)type;

            List<ExclusionDTO> exclusions = DelegateService.accountingParameterService.GetExclusions((int)exclusionType);
            foreach (ExclusionDTO exclusion in exclusions)
            {
                if (type == 1)
                {
                    exclusionsResponses.Add(new
                    {
                        Id = exclusion.Id,
                        DescriptionInsuredType = Global.Policy,
                        BranchCd = exclusion.Policy.Branch.Id,
                        BranchDescription = exclusion.Policy.Branch.Description.ToUpper(),
                        PrefixCd = exclusion.Policy.Prefix.Id,
                        PrefixDescription = exclusion.Policy.Prefix.Description.ToUpper(),
                        PolicyNumber = exclusion.Policy.DocumentNumber
                    });
                }
                else
                {
                    exclusionsResponses.Add(new
                    {
                        Id = exclusion.Id,
                        DescriptionInsuredType = (type == 2 ? Global.Agent : Global.Insured),
                        Name = (type == 2 ? exclusion.Agent.Name : exclusion.Insured.Name),
                        DocumentNumber = (type == 2 ? exclusion.Agent.IdentificationDocument.Number : exclusion.Insured.IdentificationDocument.Number)
                    });
                }
            }

            return Json(new { aaData = exclusionsResponses, total = exclusionsResponses.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateAccountNumber
        /// Valida la existencia de una cuenta bancaria en base al tipo de cuenta, sucursal y banco
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="accountTypeId"></param>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <returns>int</returns>
        private int ValidateAccountNumber(string accountNumber, int accountTypeId, int branchId, int bankId)
        {
            int validated = 0;

            try
            {
                var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();

                validated = bankAccountCompanies.Where(r => (r.Number.Equals(accountNumber) &&
                                                            r.BankAccountType.Id.Equals(accountTypeId) &&
                                                            r.Branch.Id.Equals(branchId) && r.Bank.Id.Equals(bankId))).ToList().Count;

                return validated;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #endregion

    }
}