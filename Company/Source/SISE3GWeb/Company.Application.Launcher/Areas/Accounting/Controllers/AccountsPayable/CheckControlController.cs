//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

//Crystal
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Helpers;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.TempCommonServices;
using AccountingPaymentModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;

//Sistran Company
using Sistran.Company.Application.CommonServices;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class CheckControlController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion

        #region Instance Variables
        readonly BillingController _billingController = new BillingController();
        readonly CommonController _commonController = new CommonController();
        #endregion

        #region CheckManualAssignment

        /// <summary>
        /// CheckManualAssignment
        /// Invoca a la vista CheckManualAssignment
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CheckManualAssignment()
        {
            // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
            ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
            ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);
            return View("~/Areas/Accounting/Views/AccountsPayable/CheckControl/CheckManualAssignment.cshtml");
        }

        /// <summary>
        /// GetCheckBookByAccountBankId
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="isAutomatic"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCheckBookByAccountBankId(int accountBankId, int isAutomatic)
        {
            List<object> checkBooks = new List<object>();                        
            List<CheckBookControlDTO> checkBookControls = DelegateService.accountingAccountsPayableService.GetCheckBookControlActiveByAccountBankId(accountBankId, isAutomatic);


            if (checkBookControls.Count > 0)
            {
                int checkAssigment = checkBookControls[0].LastCheck;

                // Control para asignar el cheque que corresponde 

                if (checkBookControls[0].LastCheck == 0)
                {
                    checkAssigment = checkBookControls[0].CheckFrom;
                }
                else
                {
                    if (checkAssigment < checkBookControls[0].CheckTo)
                    {
                        checkAssigment++;
                    }
                    else
                    {
                        checkAssigment = -1;
                    }
                }

                checkBooks.Add(new
                {
                    Id = checkBookControls[0].Id,
                    CheckAssigment = checkAssigment,
                    CheckTo = checkBookControls[0].CheckTo,
                    Status = checkBookControls[0].Status,
                    CheckFrom = checkBookControls[0].CheckFrom
                });
            }

            var checkBooksResult = new
            {
                rows = checkBooks
            };

            return Json(checkBooksResult, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// SaveCheckPaymentOrder
        /// Graba la asignación de cheques a órdenes de pago
        /// </summary>
        /// <param name="checkPaymentOrderModel"></param>
        /// <param name="typeAssignment"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCheckPaymentOrder(CheckPaymentOrderModel checkPaymentOrderModel, int typeAssignment)
        {
            try
            {
                bool isSavedCheckPaymentOrder = false;
                int checkBookControlId = 0;
                string paymentOrderMessage = "";
                bool isEnabledGeneralLedger = true;

                if (checkPaymentOrderModel.PaymentOrdersItems != null)
                {
                    for (int i = 0; i <= checkPaymentOrderModel.PaymentOrdersItems.Count - 1; i++)
                    {
                        CheckPaymentOrderDTO checkPaymentOrder = new CheckPaymentOrderDTO();
                        BankAccountCompanyDTO accountBank = new BankAccountCompanyDTO();
                        int statusId = 0;
                        PersonTypeDTO personType = new PersonTypeDTO();
                        IndividualDTO individual = new IndividualDTO();

                        if (typeAssignment == 1) // Automática  
                        {
                            accountBank.Id = checkPaymentOrderModel.AccountBankId;
                            accountBank.Bank = new BankDTO();
                            accountBank.Bank.Id = checkPaymentOrderModel.BankId;
                            accountBank.Number = checkPaymentOrderModel.AccountBankNumber;
                            statusId = checkPaymentOrderModel.Status;
                            checkPaymentOrder.CheckNumber = checkPaymentOrderModel.CheckNumber;
                            checkPaymentOrder.IsCheckPrinted = Convert.ToBoolean(checkPaymentOrderModel.IsCheckPrinted);
                            personType.Id = checkPaymentOrderModel.PaymentOrdersItems[i].BeneficiaryTypeId;
                            individual.IndividualId = checkPaymentOrderModel.PaymentOrdersItems[i].IndividualId;
                            checkBookControlId = checkPaymentOrderModel.CheckBookControlId;
                        }
                        else if (typeAssignment == 0) // Manual
                        {
                            accountBank.Id = checkPaymentOrderModel.PaymentOrdersItems[i].AccountBankId;
                            accountBank.Bank = new BankDTO();
                            accountBank.Bank.Id = checkPaymentOrderModel.PaymentOrdersItems[i].BankId;
                            accountBank.Number = checkPaymentOrderModel.PaymentOrdersItems[i].AccountBankNumber;
                            statusId = checkPaymentOrderModel.PaymentOrdersItems[i].Status;
                            checkPaymentOrder.CheckNumber = checkPaymentOrderModel.PaymentOrdersItems[i].CheckNumber;
                            checkPaymentOrder.IsCheckPrinted = Convert.ToBoolean(checkPaymentOrderModel.PaymentOrdersItems[i].IsCheckPrinted);
                            personType.Id = checkPaymentOrderModel.BeneficiaryTypeId;
                            individual.IndividualId = checkPaymentOrderModel.IndividualId;
                            checkBookControlId = checkPaymentOrderModel.PaymentOrdersItems[i].CheckBookControlId;
                        }

                        int userId = _commonController.GetUserIdByName(User.Identity.Name);

                        checkPaymentOrder.Id = 0;
                        checkPaymentOrder.BankAccountCompany = accountBank;
                        checkPaymentOrder.Status = statusId;
                        checkPaymentOrder.PrintedUser = userId;
                        checkPaymentOrder.PersonType = personType;
                        checkPaymentOrder.Delivery = individual;
                        checkPaymentOrder.CancellationUser = userId;

                        checkPaymentOrder.PaymentOrders = new List<PaymentOrderDTO>();

                        PaymentOrderDTO paymentOrderCheck = new PaymentOrderDTO();
                        paymentOrderCheck.Id = checkPaymentOrderModel.PaymentOrdersItems[i].PaymentOrderId;
                        checkPaymentOrder.PaymentOrders.Add(paymentOrderCheck);

                        isSavedCheckPaymentOrder = DelegateService.accountingAccountsPayableService.SaveCheckPaymentOrderRequest(checkPaymentOrder, checkBookControlId);

                        // Consulta TechnicalTransaction
                        int technicalTransaction = (int)DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["TransactionNumber"])).NumberParameter;
                        technicalTransaction = technicalTransaction - 1;

                        #region Accounting

                        // Disparo el método de contabilidad
                        if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                        {
                            paymentOrderMessage = RecordPaymentOrder(checkPaymentOrderModel.PaymentOrdersItems[i].PaymentOrderId, userId);
                        }
                        else
                        {
                            paymentOrderMessage = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                            isEnabledGeneralLedger = false;
                        }

                        #endregion Accounting

                        paymentOrderMessage = paymentOrderMessage + "; " + @Global.TransactionNumber + ": " + Convert.ToString(technicalTransaction);
                    }
                }

                var newPaymentOrder = new
                {
                    PaymentOrderMessage = string.Empty + paymentOrderMessage,
                    success = isSavedCheckPaymentOrder,
                    IsEnabledGeneralLedger = isEnabledGeneralLedger
                };

                return Json(newPaymentOrder, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region DeliveryChecks

        /// <summary>
        /// CheckDelivery
        /// Invoca a la vista CheckDelivery
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CheckDelivery()
        {
            try
            {
                ViewBag.DateNow = DateTime.Now.Date.ToString("dd/MM/yyyy");
                ViewBag.PersonType = _commonController.LoadPersonType();
                ViewBag.DeliveryType = ConfigurationManager.AppSettings["DeliveryType"];

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AccountsPayable/CheckControl/CheckDelivery.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region PrintChecks

        /// <summary>
        /// CheckPrinting
        /// Invoca a la vista CheckPrinting
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CheckPrinting()
        {
            try
            {
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AccountsPayable/CheckControl/CheckPrinting.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetBankBranchsByBranchId
        /// Obtiene sucursales bancarias 
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBankBranchsByBranchId(int branchId)
        {
            return new UifSelectResult(DelegateService.tempCommonService.GetBankBranchsByBranchId(branchId));
        }

        /// <summary>
        /// GetAccountByBankIdByBranchId
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountByBankIdByBranchId(int bankId, int branchId)
        {
            List<object> bankAccounts = new List<object>();
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.Branch.Id.Equals(branchId))).ToList();

            foreach (BankAccountCompanyDTO bankAccountCompany in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    Id = bankAccountCompany.Id,
                    Number = bankAccountCompany.Number
                });
            }

            return new UifSelectResult(bankAccounts);
        }

        /// <summary>
        /// GetAccountBankByAccountBankId
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountBankByAccountBankId(int accountBankId)
        {
            List<object> bankAccountCompanies = new List<object>();
            string currencyName = String.Empty;

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
            bankAccountCompany.Id = accountBankId;

            bankAccountCompany = DelegateService.accountingParameterService.GetBankAccountCompany(bankAccountCompany);

            currencyName = _commonController.GetCurrencyDescriptionById(bankAccountCompany.Currency.Id);

            bankAccountCompanies.Add(new
            {
                Id = bankAccountCompany.Id,
                CurrencyId = bankAccountCompany.Currency.Id,
                CurrencyName = currencyName
            });

            return Json(bankAccountCompanies, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateCheckAlreadyRegistered
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountBankId"></param>
        /// <param name="checkNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateCheckAlreadyRegistered(int branchId, int accountBankId, string checkNumber)
        {
            SCRDTO.SearchParameterPaymentOrdersDTO searchParameterPaymentOrders = new SCRDTO.SearchParameterPaymentOrdersDTO();
            searchParameterPaymentOrders.BeneficiaryDocumentNumber = "*";
            searchParameterPaymentOrders.BeneficiaryFullName = "*";
            searchParameterPaymentOrders.Branch = new SCRDTO.BranchDTO() { Id = Convert.ToInt32(branchId) };
            searchParameterPaymentOrders.EndDate = "*";
            searchParameterPaymentOrders.PaymentMethod = new AccountingPaymentModels.PaymentMethodDTO()
            {
                Id = Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"])
            };
            searchParameterPaymentOrders.PaymentOrderNumber = "*";
            searchParameterPaymentOrders.PersonType = new PersonTypeDTO() { Id = -1 };
            searchParameterPaymentOrders.StartDate = "*";
            searchParameterPaymentOrders.UserId = -1;
            searchParameterPaymentOrders.StatusId = -1;
            searchParameterPaymentOrders.IsDelivered = false;
            searchParameterPaymentOrders.IsReconciled = false;
            searchParameterPaymentOrders.IsAccounting = true;
            searchParameterPaymentOrders.AccountBankId = accountBankId;
            searchParameterPaymentOrders.CheckNumber = checkNumber;

            List<SCRDTO.PaymentOrderDTO> paymentOrders = DelegateService.accountingAccountsPayableService.SearchPaymentOrders(searchParameterPaymentOrders);

            if (paymentOrders.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPaymentOrderByPaymentOrderId
        /// Obtiene órdenes de pago por el id y currencyId
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentOrderByPaymentOrderId(int paymentOrderId, string currencyId)
        {

            List<SCRDTO.TempPaymentOrderDTO> tempPaymentOrders =
                DelegateService.accountingAccountsPayableService.GetPaymentOrderByPaymentSourceIdPayDate(-1, DateTime.Now, Int32.Parse(currencyId), paymentOrderId);
            var tempPaymentOrder = new
            {
                total = tempPaymentOrders.Count,
                records = tempPaymentOrders,
                rows = tempPaymentOrders
            };

            return Json(tempPaymentOrder, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPrintCheck
        /// Obtiene cheques impresos
        /// </summary>
        /// <param name="paymentSourceId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="numberCheck"></param>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPrintCheck(int paymentSourceId, string estimatedPaymentDate, string numberCheck, int accountBankId)
        {
            DateTime payDate = new DateTime();
            if (estimatedPaymentDate != "")
            {
                payDate = Convert.ToDateTime(estimatedPaymentDate);
            }

            if (numberCheck == "")
            {
                numberCheck = "-1";
            }

            SCRDTO.SearchParameterCheckPaymentOrderDTO searchParameterCheckPaymentOrder = new SCRDTO.SearchParameterCheckPaymentOrderDTO();
            searchParameterCheckPaymentOrder.PaymentSource = new AccountingPaymentModels.PaymentMethodDTO() { Id = paymentSourceId };
            searchParameterCheckPaymentOrder.EstimatedPaymentDate = payDate;
            searchParameterCheckPaymentOrder.NumberCheck = Convert.ToInt32(numberCheck);
            searchParameterCheckPaymentOrder.BankAccountCompany = new BankAccountCompanyDTO() { Id = accountBankId };
            searchParameterCheckPaymentOrder.IsPrinted = 0;
            searchParameterCheckPaymentOrder.CheckFrom = -1;
            searchParameterCheckPaymentOrder.CheckTo = -1;
            searchParameterCheckPaymentOrder.PaymentOrderNumber = -1;
            searchParameterCheckPaymentOrder.Amount = -1;
            searchParameterCheckPaymentOrder.BeneficiaryFullName = null;
            searchParameterCheckPaymentOrder.DeliveryType = -1;

            List<SCRDTO.PrintCheckDTO> printCheckDTOs = DelegateService.accountingAccountsPayableService.GetPrintCheck(searchParameterCheckPaymentOrder);

            List<object> printChecks = new List<object>();

            foreach (SCRDTO.PrintCheckDTO printCheckDTO in printCheckDTOs)
            {
                printChecks.Add(new
                {
                    CheckNumber = printCheckDTO.CheckNumber,
                    NumberPaymentOrder = printCheckDTO.NumberPaymentOrder,
                    BeneficiaryName = printCheckDTO.BeneficiaryName,
                    EstimatedPaymentDate = printCheckDTO.EstimatedPaymentDate,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", printCheckDTO.Amount),
                    CompanyName = printCheckDTO.CompanyName,
                    AddressCompany = printCheckDTO.DescriptionCity,
                    DescriptionCity = printCheckDTO.DescriptionCity,
                    CheckPaymentOrderCode = printCheckDTO.CheckPaymentOrderCode,
                });
            }

            return new UifTableResult(printChecks);
        }

        /// <summary>
        /// GetDeliveryCheck
        /// Obtiene cheque impreso
        /// </summary>
        /// <param name="deliveryTypeId"></param>
        /// <param name="paymentOrderNumber"></param>
        /// <param name="accountBankId"></param>
        /// <param name="checkNumber"></param>
        /// <param name="amount"></param>
        /// <param name="payTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDeliveryCheck(int accountBankId, int deliveryTypeId, string paymentOrderNumber, string checkNumber, string amount, string payTo)
        {
            if (paymentOrderNumber == "")
            {
                paymentOrderNumber = "-1";
            }
            if (checkNumber == "")
            {
                checkNumber = "-1";
            }
            if (amount == "")
            {
                amount = "-1";
            }
            if (payTo == "")
            {
                payTo = null;
            }

            SCRDTO.SearchParameterCheckPaymentOrderDTO searchParameterCheckPaymentOrder = new SCRDTO.SearchParameterCheckPaymentOrderDTO();
            searchParameterCheckPaymentOrder.PaymentSource = new AccountingPaymentModels.PaymentMethodDTO() { Id = -1 };
            searchParameterCheckPaymentOrder.EstimatedPaymentDate = new DateTime();
            searchParameterCheckPaymentOrder.NumberCheck = int.Parse(checkNumber);
            searchParameterCheckPaymentOrder.BankAccountCompany = new BankAccountCompanyDTO() { Id = accountBankId };
            searchParameterCheckPaymentOrder.IsPrinted = 1;
            searchParameterCheckPaymentOrder.CheckFrom = -1;
            searchParameterCheckPaymentOrder.CheckTo = -1;
            searchParameterCheckPaymentOrder.PaymentOrderNumber = int.Parse(paymentOrderNumber);
            searchParameterCheckPaymentOrder.Amount = Convert.ToDecimal(amount);
            searchParameterCheckPaymentOrder.BeneficiaryFullName = payTo;
            searchParameterCheckPaymentOrder.DeliveryType = deliveryTypeId;

            List<SCRDTO.PrintCheckDTO> printChecksDTOs = DelegateService.accountingAccountsPayableService.GetPrintCheck(searchParameterCheckPaymentOrder);

            List<object> printChecks = new List<object>();

            foreach (SCRDTO.PrintCheckDTO printCheck in printChecksDTOs)
            {
                printChecks.Add(new
                {
                    CheckNumber = printCheck.CheckNumber,
                    NumberPaymentOrder = printCheck.NumberPaymentOrder,
                    BeneficiaryName = printCheck.BeneficiaryName,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", printCheck.Amount),
                    DeliveryDate = printCheck.DeliveryDate,
                    CourierName = printCheck.CourierName,
                    CheckPaymentOrderCode = printCheck.CheckPaymentOrderCode,
                });
            }

            return new UifTableResult(printChecks);
        }

        ///<summary>
        /// UpdateCheckPaymentOrder
        /// Actualiza la orden de pago
        /// </summary>
        /// <param name="listPrintCheckModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateCheckPaymentOrder(ListPrintCheckModel listPrintCheckModel)
        {
            try
            {
                bool isUpdated = false;

                if (listPrintCheckModel.PrintCheckModels.Count > 0)
                {
                    for (int i = 0; i < listPrintCheckModel.PrintCheckModels.Count; i++)
                    {
                        CheckPaymentOrderDTO checkPaymentOrder = new CheckPaymentOrderDTO();
                        if (listPrintCheckModel.PrintCheckModels[i].DeliveryTypeId == 1)
                        {
                            checkPaymentOrder.DeliveryDate = DateTime.Now;
                            checkPaymentOrder.PersonType = new PersonTypeDTO();
                            checkPaymentOrder.PersonType.Id = -1;
                            checkPaymentOrder.CourierName = null;
                            checkPaymentOrder.RefundDate = new DateTime();
                            checkPaymentOrder.CancellationDate = new DateTime();
                            checkPaymentOrder.Status = 1;
                            checkPaymentOrder.Id = listPrintCheckModel.PrintCheckModels[i].CheckPaymentOrderCode;
                            isUpdated = DelegateService.accountingAccountsPayableService.UpdateCheckPaymentOrder(checkPaymentOrder);
                        }
                        if (listPrintCheckModel.PrintCheckModels[i].DeliveryTypeId == 2)
                        {
                            checkPaymentOrder.DeliveryDate = DateTime.Now;
                            checkPaymentOrder.PersonType = new PersonTypeDTO();
                            checkPaymentOrder.PersonType.Id = listPrintCheckModel.PrintCheckModels[i].PersonTypeId;
                            checkPaymentOrder.CourierName = listPrintCheckModel.PrintCheckModels[i].CourierName;
                            checkPaymentOrder.RefundDate = new DateTime();
                            checkPaymentOrder.CancellationDate = new DateTime();
                            checkPaymentOrder.Status = 1;
                            checkPaymentOrder.Id = listPrintCheckModel.PrintCheckModels[i].CheckPaymentOrderCode;
                            isUpdated = DelegateService.accountingAccountsPayableService.UpdateCheckPaymentOrder(checkPaymentOrder);

                        }
                        if (listPrintCheckModel.PrintCheckModels[i].DeliveryTypeId == 3)
                        {
                            checkPaymentOrder.DeliveryDate = new DateTime();
                            checkPaymentOrder.PersonType = new PersonTypeDTO();
                            checkPaymentOrder.PersonType.Id = -1;
                            checkPaymentOrder.CourierName = null;
                            checkPaymentOrder.RefundDate = DateTime.Now;
                            checkPaymentOrder.CancellationDate = new DateTime();
                            checkPaymentOrder.Status = 1;
                            checkPaymentOrder.Id = listPrintCheckModel.PrintCheckModels[i].CheckPaymentOrderCode;
                            isUpdated = DelegateService.accountingAccountsPayableService.UpdateCheckPaymentOrder(checkPaymentOrder);
                        }

                        if (isUpdated) {
                            PaymentOrderDTO paymentOrderUpdate = DelegateService.accountingAccountsPayableService.GetPaymentOrder(listPrintCheckModel.PrintCheckModels[i].NumberPaymentOrder);
                            paymentOrderUpdate.Status = Convert.ToInt32(PaymentOrderStatus.Paid);
                            isUpdated = DelegateService.accountingAccountsPayableService.UpdatePaymentOrder(paymentOrderUpdate);
                        }
                    }
                }

                var isUpdatedCheckPaymentOrder = new
                {
                    rows = isUpdated
                };

                return Json(isUpdatedCheckPaymentOrder, JsonRequestBehavior.AllowGet);
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
        /// LoadReportPrintCheck
        /// Llena el reporte con los datos correspondientes
        /// </summary>
        /// <param name="printCheckModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult LoadReportPrintCheck(ListPrintCheckModel printCheckModel)
        {
            bool isUpdatedCheckPaymentOrder = false;
            List<PrintingCheckModel> printingChecks = new List<PrintingCheckModel>();

            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            if (printCheckModel.PrintCheckModels != null)
            {
                if (printCheckModel.PrintCheckModels.Count > 0)
                {
                    for (int i = 0; i < printCheckModel.PrintCheckModels.Count; i++)
                    {
                        CheckPaymentOrderDTO checkPaymentOrder = new CheckPaymentOrderDTO();
                        checkPaymentOrder.PrintedDate = DateTime.Now;
                        checkPaymentOrder.IsCheckPrinted = true;
                        checkPaymentOrder.PrintedUser = userId;
                        checkPaymentOrder.PersonType = new PersonTypeDTO();
                        checkPaymentOrder.PersonType.Id = -1;
                        checkPaymentOrder.DeliveryDate = new DateTime();
                        checkPaymentOrder.RefundDate = new DateTime();
                        checkPaymentOrder.CancellationDate = new DateTime();
                        checkPaymentOrder.Status = 1;
                        checkPaymentOrder.Id = printCheckModel.PrintCheckModels[i].CheckPaymentOrderCode;
                        isUpdatedCheckPaymentOrder = DelegateService.accountingAccountsPayableService.UpdateCheckPaymentOrder(checkPaymentOrder);

                        PrintingCheckModel printCheck = new PrintingCheckModel();
                        printCheck.AccountBankId = printCheckModel.PrintCheckModels[i].AccountBankId;
                        printCheck.AccountCurrentNumber = printCheckModel.PrintCheckModels[i].AccountCurrentNumber;
                        printCheck.AddressCompany = printCheckModel.PrintCheckModels[i].AddressCompany;
                        printCheck.DescriptionCity = printCheckModel.PrintCheckModels[i].DescriptionCity;
                        printCheck.Amount = decimal.Parse(printCheckModel.PrintCheckModels[i].Amount);
                        printCheck.BankName = printCheckModel.PrintCheckModels[i].BankName;
                        printCheck.BeneficiaryName = printCheckModel.PrintCheckModels[i].BeneficiaryName;
                        printCheck.BranchId = printCheckModel.PrintCheckModels[i].BranchId;
                        printCheck.CheckNumber = printCheckModel.PrintCheckModels[i].CheckNumber;
                        printCheck.CompanyName = printCheckModel.PrintCheckModels[i].CompanyName;
                        printCheck.EstimatedPaymentDate = printCheckModel.PrintCheckModels[i].EstimatedPaymentDate;
                        printCheck.NumberPaymentOrder = printCheckModel.PrintCheckModels[i].NumberPaymentOrder;
                        printCheck.PaymentSourceId = printCheckModel.PrintCheckModels[i].PaymentSourceId;
                        printCheck.CurrencyName = printCheckModel.PrintCheckModels[i].CurrencyName;
                        printingChecks.Add(printCheck);
                    }
                }

                TempData["billRptSource"] = printingChecks;
                TempData["BillingReportName"] = "Areas//Accounting//Reports//AccountsPayable//PrintCheckReport.rpt";
                TempData["PdfName"] = "Content\\file.pdf";
                var reportSource = printingChecks;
                var reportName = "Areas//Accounting//Reports//AccountsPayable//PrintCheckReport.rpt";
                var pdfName = "Content\\file.pdf";
                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                string pdfPath = System.Web.HttpContext.Current.Server.MapPath("~/") + pdfName;

                // Create and load rpt in memory
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.SetDataSource(reportSource);

                ExportOptions exportOptions = reportDocument.ExportOptions;
                PdfRtfWordFormatOptions pdfOptions = new PdfRtfWordFormatOptions();
                DiskFileDestinationOptions diskOptions = new DiskFileDestinationOptions();
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                exportOptions.FormatOptions = pdfOptions;
                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                diskOptions.DiskFileName = pdfPath;
                exportOptions.DestinationOptions = diskOptions;
                reportDocument.Export();
            }

            return Json(isUpdatedCheckPaymentOrder, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ShowPrintCheckReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowPrintCheckReport()
        {
            try
            {
                bool isValidated = true;
                var reportSource = TempData["billRptSource"];
                var reportName = TempData["BillingReportName"];

                if (reportSource == null)
                {
                    isValidated = false;
                }

                if (isValidated)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    //Llena Reporte Principal
                    reportDocument.SetDataSource(reportSource);

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "PrintCheckReport");

                    // Clear all sessions value
                    TempData["billRptSource"] = null;
                    TempData["BillingReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region ReprintChecks

        /// <summary>
        /// CheckReprinting
        /// Invoca a la vista CheckReprinting
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CheckReprinting()
        {
            try
            {
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AccountsPayable/CheckControl/CheckReprinting.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetReprintCheck
        /// Obtiene cheques reimpresos
        /// </summary>
        /// <param name="paymentSourceId"></param>
        /// <param name="checkFrom"></param>
        /// <param name="checkTo"></param>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReprintCheck(int paymentSourceId, int accountBankId, int? checkFrom, int? checkTo)
        {
            SCRDTO.SearchParameterCheckPaymentOrderDTO searchParameterCheckPaymentOrder = new SCRDTO.SearchParameterCheckPaymentOrderDTO();
            searchParameterCheckPaymentOrder.PaymentSource = new AccountingPaymentModels.PaymentMethodDTO() { Id = paymentSourceId };
            searchParameterCheckPaymentOrder.EstimatedPaymentDate = new DateTime();
            searchParameterCheckPaymentOrder.NumberCheck = -1;
            searchParameterCheckPaymentOrder.BankAccountCompany = new BankAccountCompanyDTO();
            searchParameterCheckPaymentOrder.BankAccountCompany.Id = accountBankId;
            searchParameterCheckPaymentOrder.IsPrinted = 1;

            if (checkFrom.HasValue)
            {
                searchParameterCheckPaymentOrder.CheckFrom = checkFrom.Value;
            }
            if (checkTo.HasValue)
            {
                searchParameterCheckPaymentOrder.CheckTo = checkTo.Value;
            }

            searchParameterCheckPaymentOrder.PaymentOrderNumber = -1;
            searchParameterCheckPaymentOrder.Amount = -1;
            searchParameterCheckPaymentOrder.BeneficiaryFullName = null;
            searchParameterCheckPaymentOrder.DeliveryType = -1;

            List<SCRDTO.PrintCheckDTO> printCheckDTOs = DelegateService.accountingAccountsPayableService.GetPrintCheck(searchParameterCheckPaymentOrder);

            List<object> printChecks = new List<object>();

            foreach (SCRDTO.PrintCheckDTO printCheckDTO in printCheckDTOs)
            {
                printChecks.Add(new
                {
                    CheckNumber = printCheckDTO.CheckNumber,
                    NumberPaymentOrder = printCheckDTO.NumberPaymentOrder,
                    BeneficiaryName = printCheckDTO.BeneficiaryName,
                    EstimatedPaymentDate = Convert.ToDateTime(printCheckDTO.EstimatedPaymentDate).ToShortDateString(),
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", printCheckDTO.Amount),
                    CompanyName = printCheckDTO.CompanyName,
                    AddressCompany = printCheckDTO.DescriptionCity,
                    CheckPaymentOrderCode = printCheckDTO.CheckPaymentOrderCode,
                });
            }

            return new UifTableResult(printChecks);
        }

        #endregion

        #region CheckNullification

        /// <summary>
        /// CheckNullification
        /// Invoca a la vista CheckNullification
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CheckNullification()
        {
            try
            {
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AccountsPayable/CheckControl/CheckNullification.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetAccountNullifyByBankIdByBranchId
        /// Obtiene las cuentas de un banco anuladas
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountNullifyByBankIdByBranchId(int bankId, int branchId)
        {
            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.Branch.Id.Equals(branchId))).ToList();
            List<object> bankAccounts = new List<object>();

            foreach (BankAccountCompanyDTO bankAccountCompany in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    Id = bankAccountCompany.Id,
                    Number = bankAccountCompany.Number
                });
            }

            return new UifSelectResult(bankAccounts);
        }

        /// <summary>
        /// GetCanceledCheckByAccountBankIdByCheckNumber
        /// Obtiene cheque cancelado
        /// </summary>
        /// <param name="checkNumber"></param>
        /// <param name="accountBankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCanceledCheckByAccountBankIdByCheckNumber(int accountBankId, int checkNumber)
        {
            List<SCRDTO.CancelCheckDTO> cancelCheckDTOs = DelegateService.accountingAccountsPayableService.GetCancelCheckPaymentOrder(accountBankId, checkNumber);

            List<object> cancelChecks = new List<object>();

            foreach (SCRDTO.CancelCheckDTO cancelCheckDTO in cancelCheckDTOs)
            {
                
                cancelChecks.Add(new
                {
                    PaymentOrderCode = cancelCheckDTO.PaymentOrderCode,
                    PayTo = cancelCheckDTO.PayTo,
                    PaymentDate = cancelCheckDTO.PaymentDate == "" ? "" : Convert.ToDateTime(cancelCheckDTO.PaymentDate).ToShortDateString(),
                    CheckPaymentOrderCode = cancelCheckDTO.CheckPaymentOrderCode,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", cancelCheckDTO.Amount),
                    PaymetOrderStatusDescription = PaymetOrderStatusDescription(cancelCheckDTO.PaymetOrderStatus),
                    IndividualId = cancelCheckDTO.IndividualId,
                    TempImputationCode = cancelCheckDTO.TempImputationCode,
                    ExchangeRate = cancelCheckDTO.ExchangeRate
                });
            }

            return new UifTableResult(cancelChecks);
        }


        private string PaymetOrderStatusDescription(int PaymentOrderStatus)
        {
            switch ((PaymentOrderStatus)PaymentOrderStatus)
            {
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Canceled:
                    return Global.Canceled;
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Active:
                    return Global.Active;
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Authorized:
                    return Global.Authorized;
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Applied:
                    return Global.Applied;
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Paid:
                    return Global.Paid;
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Rejected:
                    return Global.Rejected;
                case Core.Application.AccountingServices.Enums.PaymentOrderStatus.Forwarded:
                    return Global.Forwarded;
                default:
                    return "";
            }
        }
        ///<summary>
        /// SaveCancelCheckRequest
        /// Graba la anulación de un cheque
        /// </summary>
        /// <param name="checkPaymentOrderModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCancelCheckRequest(CheckPaymentOrderModel checkPaymentOrderModel)
        {
            try
            {
                bool isSavedCancelCheck = false;
                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                CheckPaymentOrderDTO checkPaymentOrder = new CheckPaymentOrderDTO();
                checkPaymentOrder.Id = checkPaymentOrderModel.Id;
                checkPaymentOrder.CancellationUser = userId;

                checkPaymentOrder.CancellationDate = DateTime.Now;
                checkPaymentOrder.IsCheckPrinted = Convert.ToBoolean(checkPaymentOrderModel.IsCheckPrinted);
                checkPaymentOrder.Status = checkPaymentOrderModel.Status;
                checkPaymentOrder.PaymentOrders = new List<PaymentOrderDTO>();
                if (checkPaymentOrderModel.PaymentOrdersItems.Count > 0)
                {
                    foreach (PaymentOrderCheckModel paymentOrder in checkPaymentOrderModel.PaymentOrdersItems)
                    {
                        PaymentOrderDTO paymentOrderCheck = new PaymentOrderDTO();
                        paymentOrderCheck.Id = paymentOrder.PaymentOrderId;
                        paymentOrderCheck.BankAccountPerson = new BankAccountPersonDTO()
                        {
                            Id = checkPaymentOrderModel.AccountBankId
                        };
                        paymentOrderCheck.PaymentDate = Convert.ToDateTime(paymentOrder.PaymentDate);
                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = paymentOrder.CurrencyId },
                            Value = Convert.ToDecimal(paymentOrder.TotalAmount)
                        };
                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO()
                        {
                            BuyAmount = paymentOrder.ExchangeRate
                        };

                        paymentOrderCheck.Amount = amount;
                        paymentOrderCheck.ExchangeRate = exchangeRate;
                        paymentOrderCheck.LocalAmount = amount;
                        ApplicationDTO imputation = new ApplicationDTO();
                        imputation.Id = paymentOrder.TempImputationCode;
                        paymentOrderCheck.Imputation = new ApplicationDTO();
                        paymentOrderCheck.Imputation = imputation;
                        paymentOrderCheck.UserId = userId;
                        checkPaymentOrder.PaymentOrders.Add(paymentOrderCheck);
                    }
                }

                var message = "";

                if (message.IndexOf("CORRECTA") != -1 || message == "")
                {
                    isSavedCancelCheck = DelegateService.accountingAccountsPayableService.SaveCancelCheckRequest(checkPaymentOrder);
                }
                else
                {
                    isSavedCancelCheck = false;
                }

                return new UifTableResult(isSavedCancelCheck);
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

        #endregion

        #region CheckAutomaticAssignment

        /// <summary>
        /// CheckAutomaticAssignment
        /// Invoca a la vista AutomaticAssigment
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CheckAutomaticAssignment()
        {
            try
            {
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AccountsPayable/CheckControl/CheckAutomaticAssignment.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }   
        }

        /// <summary>
        /// GetPaymentOrderByPaymentSourceIdPayByDate
        /// Obtiene órdenes de pago por origne de pago y fecha
        /// </summary>
        /// <param name="paymentSourceId"></param>
        /// <param name="payDate"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentOrderByPaymentSourceIdPayByDate(string paymentSourceId, string payDate, string currencyId)
        {
            DateTime paymentDate = new DateTime(1900, 1, 1);
            if (payDate != String.Empty)
            {
                paymentDate = Convert.ToDateTime(payDate);
            }

            List<SCRDTO.TempPaymentOrderDTO> tempPaymentOrderDTOs =
                DelegateService.accountingAccountsPayableService.GetPaymentOrderByPaymentSourceIdPayDate(
                    int.Parse(paymentSourceId), paymentDate, int.Parse(currencyId), -1).OrderBy(o => o.TempPaymentOrderId).ToList();

            List<object> tempPaymentOrders = new List<object>();

            foreach (SCRDTO.TempPaymentOrderDTO item in tempPaymentOrderDTOs)
            {
                tempPaymentOrders.Add(new
                {
                    TempPaymentOrderId = item.TempPaymentOrderId,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", item.Amount),                    
                    BeneficiaryDocumentNumber = item.BeneficiaryDocumentNumber,
                    BeneficiaryName = item.BeneficiaryName,
                    BeneficiaryType = item.BeneficiaryType,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                    BranchPayId = item.BranchPayId,
                    BranchPayName = item.BranchPayName,
                    CompanyId = item.CompanyId,
                    CompanyName = item.CompanyName,
                    CurrencyId = item.CurrencyId,
                    CurrencyName = item.CurrencyName,
                    Exchange = item.Exchange,
                    IndividualId = item.IndividualId,
                    PaymentDate = item.PaymentDate,
                    EstimatedPaymentDate = item.EstimatedPaymentDate,
                    PaymentMethodId = item.PaymentMethodId,
                    PaymentMethodName = item.PaymentMethodName,
                    PaymentSourceId = item.PaymentSourceId,
                    PaymentSourceName = item.PaymentSourceName,
                    PayTo = item.PayTo,
                    PersonTypeId = item.PersonTypeId,
                    PersonTypeName = item.PersonTypeName,
                    NumberAssociate = ""
                });
            }

            return new UifTableResult(tempPaymentOrders);
        }

        #endregion

        #region AccountPaymentOrder

        /// <summary>
        /// RecordPaymentOrder
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string RecordPaymentOrder(int paymentOrderId, int userId)
        {
            string recordPaymentOrderMessage = "";

            int moduleDateId = 0;
            int moduleId = 0;
            int subModuleId = (int)PaymentType.Check;

            try
            {
                List<SCRDTO.ImputationParameterDTO> ImputationParameters = DelegateService.accountingAccountService.GetImputationParameters(paymentOrderId, Convert.ToInt32(ApplicationTypes.PaymentOrder), userId, moduleId, subModuleId, moduleDateId);
                recordPaymentOrderMessage = _billingController.RecordImputation(ImputationParameters, userId, 1);
            }
            catch (Exception exception)
            {
                recordPaymentOrderMessage = exception.Message;
            }
            return recordPaymentOrderMessage;
        }

        #endregion AccountPaymentOrder

    }
}