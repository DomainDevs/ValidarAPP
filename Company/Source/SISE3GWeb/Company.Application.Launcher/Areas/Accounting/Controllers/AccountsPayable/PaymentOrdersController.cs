//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Threading;

//NPOI
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using PaymentMethod = Sistran.Core.Application.AccountingServices.DTOs.Payments.PaymentMethodDTO;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Enums;
using AccountingConceptModel = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;

//Sistran Company


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class PaymentOrdersController : Controller
    {
        #region Instance Variables
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region Views

        /// <summary>
        /// MainPaymentOrdersGeneration
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPaymentOrdersGeneration()
        {
            try
            {  
                
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = TempData["AccountingDate"] ?? _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                ViewBag.ApplyCollecId = TempData["ApplyCollecId"] ?? 0;
                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;

                ViewBag.PaymentOrderNumber = TempData["PaymentOrderNumber"] ?? 0;
                ViewBag.BranchId = TempData["BranchId"] ?? "";
                ViewBag.Amount = TempData["TempPaymentOrderAmount"] ?? 0;
                ViewBag.LocalAmount = TempData["LocalAmount"] ?? "";
                ViewBag.Branch = TempData["Branch"] ?? "";
                ViewBag.IncomeConcept = TempData["IncomeConcept"] ?? "";
                ViewBag.PostedValue = TempData["PostedValue"] ?? "";
                ViewBag.Description = TempData["Description"] ?? "";
                ViewBag.TempImputationId = TempData["TempImputationId"] ?? 0;
                ViewBag.ReceiptTitle = TempData["ReceiptTitle"] ?? "";
                ViewBag.TempPaymentOrder = TempData["TempPaymentOrderId"] ?? 0;
                ViewBag.TempSearchId = TempData["TempSearchId"] ?? 0;

                // Tipo de beneficiario
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

                List<PaymentMethod> paymentMethods = GetEnabledPaymentMethodTypes();
                ViewBag.PaymentMethod = paymentMethods;
                ViewBag.UserId = _commonController.GetUserIdByName(User.Identity.Name);

                //OBTIENE SI ESTA CONFIGURADO COMO MULTICOMPANIA 1 TRUE 0 FALSE
                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
                //OBTIENE LA COMPANIA POR DEFECTO SEGÚN EL USUARIO
                ViewBag.AccountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));

                int branchUserDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchUserDefault = branchUserDefault;

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
                ViewBag.ShowPaymentOrderId = ConfigurationManager.AppSettings["ShowPaymentOrderId"];
                ViewBag.EnabledEvents = ConfigurationManager.AppSettings["EnabledEvents"];
                ViewBag.CancellationNotices = ConfigurationManager.AppSettings["CancellationNotices"];

                //Tipo de Imputación
                ViewBag.AplicationPaymentOrder = ConfigurationManager.AppSettings["AplicationPaymentOrder"];

                //Se deshabilita check de primas en depósito en la modificaciòn del importe a pagar(Primas por cobrar) no aplica en BE
                ViewBag.DepositPrimes = Convert.ToString(ConfigurationManager.AppSettings["DepositPrimes"]);

                //Setear valor por defaul de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                //Setea el tipo de imputacion
                ViewBag.ImputationTypePaymentOrder = ConfigurationManager.AppSettings["ImputationTypePaymentOrder"];
                //Setea el punto de venta por default
                ViewBag.SalePointBranchUserDefault = DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(_commonController.GetUserIdByName(User.Identity.Name), ViewBag.BranchDefault);

                //Obtiene si es el uso del tercero en 1 TRUE 0 FALSE
                ViewBag.ThirdAccountingUsed = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ThirdAccountingUsed"])).NumberParameter;

                return View("~/Areas/Accounting/Views/AccountsPayable/PaymentOrders/MainPaymentOrdersGeneration.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// LoadPaymentOrdersApplication
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="paymentOrderNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="amount"></param>
        /// <param name="tempSearchId"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadPaymentOrdersApplication(int paymentOrderId, int paymentOrderNumber,
                                                                  int branchId, decimal amount, int tempSearchId)
        {
            TempData["TempPaymentOrderId"] = paymentOrderId != 0 ? paymentOrderId : 0;
            TempData["TempPaymentOrderAmount"] = amount != 0 ? amount : 0;
            TempData["PaymentOrderNumber"] = paymentOrderNumber != 0 ? paymentOrderNumber : 0;
            TempData["BranchId"] = branchId != 0 ? branchId : 0;
            TempData["TempImputationId"] = paymentOrderId != 0 ? paymentOrderId : 0;

            TempData["TempSearchId"] = tempSearchId != 0 ? tempSearchId : 0;

            return RedirectToAction("MainPaymentOrdersGeneration");
        }


        /// <summary>
        /// PaymentOrderSearch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentOrderSearch()
        {
            try
            {
                List<AccountingConceptModel.ConceptSourceDTO> conceptSources = _commonController.GetConceptSources();
                ViewBag.PaymentSource = conceptSources;
                ViewBag.TypeRequest = conceptSources;

                List<PaymentMethod> paymentMethods = GetEnabledPaymentMethodTypes();
                ViewBag.PaymentMethod = paymentMethods;

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
                DateTime accountingDate = DateTime.Now;
                ViewBag.DateAccounting = accountingDate.Date.ToString("dd/MM/yyyy");

                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
                ViewBag.AccountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));
                int branchUserDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchUserDefault = branchUserDefault;
                ViewBag.CancellationNotices = ConfigurationManager.AppSettings["CancellationNotices"];

                // Setear valor por defaul de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                //Identifica que una OP fue creada automáticamente del proceso de liberación de comisiones 
                ViewBag.ParamConceptSourceId = Convert.ToInt32(ConfigurationManager.AppSettings["ConceptSourceId"]); //8

                return View("~/Areas/Accounting/Views/AccountsPayable/PaymentOrders/PaymentOrderSearch.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PaymentOrderAuthorization
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentOrderAuthorization()
        {
            try
            {
      
                // Setear valor por defaul de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/AccountsPayable/PaymentOrders/PaymentOrderAuthorization.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }
        #endregion

        #region PaymentOrdersGeneration

        /// <summary>
        /// SaveTempPaymentOrder
        /// Graba órdenes de pago en temporales
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempPaymentOrder(int paymentOrderId)
        {
            PaymentOrderDTO paymentOrder = new PaymentOrderDTO();

            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            paymentOrder.BankAccountPerson = new BankAccountPersonDTO() { Id = -1 };
            paymentOrder.AccountingDate = accountingDate;
            paymentOrder.Amount = new AmountDTO()
            {
                Currency = new SCRDTO.CurrencyDTO() { Id = -1 },
                Value = 0
            };
            paymentOrder.LocalAmount = new AmountDTO() { Value = 0 };
            paymentOrder.ExchangeRate = new ExchangeRateDTO() { BuyAmount = 0 };
            paymentOrder.Beneficiary = new IndividualDTO() { IndividualId = -1 };
            paymentOrder.Branch = new SCRDTO.BranchDTO() { Id = -1 };
            paymentOrder.BranchPay = new SCRDTO.BranchDTO() { Id = -1 };
            paymentOrder.Company = new CompanyDTO() { IndividualId = -1 };
            paymentOrder.EstimatedPaymentDate = accountingDate;
            paymentOrder.Id = paymentOrderId;
            paymentOrder.IsTemporal = true;
            paymentOrder.PaymentMethod = new PaymentMethod() { Id = -1 };
            paymentOrder.PaymentSource = new ConceptSourceDTO() { Id = -1 };
            paymentOrder.PayTo = "";
            paymentOrder.PersonType = new PersonTypeDTO() { Id = -1 };
            paymentOrder.Status = Convert.ToInt32(PaymentOrderStatus.Active);
            paymentOrder.Observation = "";
            paymentOrder.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            return Json(DelegateService.accountingAccountsPayableService.SaveTempPaymentOrder(paymentOrder), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// UpdateTempPaymentOrder
        /// Actualiza las órdenes de pago temporales
        /// </summary>
        /// <param name="paymentOrderModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateTempPaymentOrder(PaymentOrderModel paymentOrderModel)
        {
            try
            {
                int paymentOrderTransactionId = 0;
                DateTime? date = null;

                PaymentOrderDTO paymentOrderTransaction = new PaymentOrderDTO();
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

                if (paymentOrderModel.EstimatedPaymentDate == Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    date = null;
                }
                else
                {
                    date = paymentOrderModel.EstimatedPaymentDate;
                }

                paymentOrderTransaction.BankAccountPerson = new BankAccountPersonDTO() { Id = paymentOrderModel.AccountBankId };
                paymentOrderTransaction.AccountingDate = accountingDate;
                paymentOrderTransaction.Amount = new AmountDTO()
                {
                    Currency = new SCRDTO.CurrencyDTO() { Id = paymentOrderModel.CurrencyId },
                    Value = paymentOrderModel.PaymentAmount                    
                };
                paymentOrderTransaction.LocalAmount = new AmountDTO() { Value = paymentOrderModel.PaymentIncomeAmount  };
                paymentOrderTransaction.ExchangeRate = new ExchangeRateDTO() { BuyAmount = paymentOrderModel.ExchangeRate };
                paymentOrderTransaction.Beneficiary = new IndividualDTO() { IndividualId = paymentOrderModel.IndividualId };
                paymentOrderTransaction.Branch = new SCRDTO.BranchDTO() { Id = paymentOrderModel.BranchId };
                paymentOrderTransaction.BranchPay = new SCRDTO.BranchDTO() { Id = paymentOrderModel.BranchPayId };
                paymentOrderTransaction.Company = new CompanyDTO() { IndividualId = paymentOrderModel.CompanyId };
                paymentOrderTransaction.EstimatedPaymentDate = Convert.ToDateTime(date);
                paymentOrderTransaction.Id = paymentOrderModel.PaymentOrderItemId;
                paymentOrderTransaction.IsTemporal = true;
                paymentOrderTransaction.PaymentMethod = new PaymentMethod() { Id = paymentOrderModel.PaymentMethodId };
                paymentOrderTransaction.PaymentSource = new ConceptSourceDTO() { Id = paymentOrderModel.PaymentSourceId };
                paymentOrderTransaction.PayTo = paymentOrderModel.PayTo;
                paymentOrderTransaction.PersonType = new PersonTypeDTO() { Id = paymentOrderModel.PersonTypeId };
                paymentOrderTransaction.Status = Convert.ToInt32(PaymentOrderStatus.Active);
                paymentOrderTransaction.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                paymentOrderTransaction.Observation = paymentOrderModel.Observation;

                paymentOrderTransaction = DelegateService.accountingAccountsPayableService.UpdateTempPaymentOrder(paymentOrderTransaction);

                paymentOrderTransactionId = paymentOrderTransaction.Id;

                return Json(paymentOrderTransactionId, JsonRequestBehavior.AllowGet);
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
        /// SavePaymentOrderApplication
        /// Graba la aplicación de órdenes de pago
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="statusId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SavePaymentOrderApplication(int tempPaymentOrderId, int tempImputationId,
                                                      int imputationTypeId, int statusId)
        {
            int paymentOrdeId = 0;
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            paymentOrdeId = DelegateService.accountingAccountsPayableService.SavePaymentOrderImputationRequest(tempPaymentOrderId, tempImputationId, imputationTypeId, userId);

            return Json(paymentOrdeId, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// CancellationPaymentOrder
        /// Cancelación de órdenes de pago
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CancellationPaymentOrder(int paymentOrderId, int tempImputationId)
        {
            bool isCancellationPaymentOrder = false;

            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            isCancellationPaymentOrder = DelegateService.accountingAccountsPayableService.CancellationPaymentOrder(paymentOrderId, tempImputationId, userId);

            return Json(isCancellationPaymentOrder, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GetBankAccountByBeneficiaryId
        /// Recupera cuenta bancaria por el beneficiario
        /// </summary>
        /// <param name="beneficiaryId"></param>
        /// <param name="isSearchPaymentOrders"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBankAccountByBeneficiaryId(string beneficiaryId, int isSearchPaymentOrders)
        {

            if (String.IsNullOrEmpty(beneficiaryId))
            {
                beneficiaryId = "-1";
            }

            List<SCRDTO.BeneficiaryBankAccountsDTO> beneficiaryBankAccountDTOs =
            DelegateService.accountingAccountsPayableService.GetBankAccountByBeneficiaryId(beneficiaryId);

            List<object> beneficiaryBankAccounts = new List<object>();

            foreach (SCRDTO.BeneficiaryBankAccountsDTO beneficiaryBankAccount in beneficiaryBankAccountDTOs)
            {
                beneficiaryBankAccounts.Add(new
                {
                    BankName = beneficiaryBankAccount.BankName,
                    AccountNumber = beneficiaryBankAccount.AccountNumber,
                    AccountTypeName = beneficiaryBankAccount.AccountTypeName,
                    CurrencyName = beneficiaryBankAccount.CurrencyName,
                    AccountTypeCode = beneficiaryBankAccount.AccountTypeCode,
                    AccountBankCode = beneficiaryBankAccount.AccountBankCode,
                    BankCode = beneficiaryBankAccount.BankCode,
                    CurrencyCode = beneficiaryBankAccount.CurrencyCode,
                    IndividualId = beneficiaryBankAccount.IndividualId,
                    TinyDescription = beneficiaryBankAccount.TinyDescription
                });
            }

            return new UifTableResult(beneficiaryBankAccounts);
        }


        #endregion

        #region PaymentOrdersSearch

        /// <summary>
        /// GetEnabledPaymentMethodTypes
        /// Obtiene los tipos de pago habilitados para orden de pago
        /// </summary>
        /// <returns>List<PaymentMethod/></returns>
        public List<PaymentMethod> GetEnabledPaymentMethodTypes()
        {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

            List<SCRDTO.PaymentMethodTypeDTO> paymentMethodTypes = DelegateService.accountingParameterService.GetEnablePaymentMethodType(false, true, false);

            if (paymentMethodTypes.Count != 0)
            {
                foreach (SCRDTO.PaymentMethodTypeDTO enablebPaymentMethodType in paymentMethodTypes)
                {
                    PaymentMethod paymentMethod = new PaymentMethod();

                    paymentMethod.Id = enablebPaymentMethodType.PaymentTypeCode;
                    paymentMethod.Description = enablebPaymentMethodType.Description;

                    paymentMethods.Add(paymentMethod);
                }
            }

            return paymentMethods;
        }

        /// <summary>
        /// GetEnabledPaymentMethodTypesSelect
        /// Obtiene los tipos de pago habilitados para orden de pago y los 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetEnabledPaymentMethodTypesSelect()
        {
            return new UifSelectResult(GetEnabledPaymentMethodTypes());
        }

        /// <summary>
        /// GetSearchPaymentOrders
        /// Búsqueda de órdenes de pago
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentOrderNumber"></param>
        /// <param name="personTypeId"></param>
        /// <param name="beneficiaryIndividualId"></param>
        /// <param name="beneficiaryDocumentNumber"></param>
        /// <param name="beneficiaryName"></param>
        /// <param name="status"></param>
        /// <param name="IsDelivered"></param>
        /// <param name="IsReconciled"></param>
        /// <param name="IsAccounting"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSearchPaymentOrders(string branchId, string userId, string paymentMethodId, string startDate,
                                                 string endDate, string paymentOrderNumber, string personTypeId,
                                                 string beneficiaryIndividualId, string beneficiaryDocumentNumber,
                                                 string beneficiaryName, int status, bool? IsDelivered,
                                                 bool? IsReconciled, bool? IsAccounting)
        {

            if (String.IsNullOrEmpty(branchId))
            {
                branchId = "-3";
            }

            if (String.IsNullOrEmpty(userId))
            {
                userId = "-1";
            }
            if (String.IsNullOrEmpty(paymentMethodId))
            {
                paymentMethodId = "-1";
            }

            if (String.IsNullOrEmpty(startDate))
            {
                startDate = "*";
            }
            if (String.IsNullOrEmpty(endDate))
            {
                endDate = "*";
            }
            if (String.IsNullOrEmpty(paymentOrderNumber))
            {
                paymentOrderNumber = "*";
            }
            if (String.IsNullOrEmpty(personTypeId))
            {
                personTypeId = "-1";
            }
            if (String.IsNullOrEmpty(beneficiaryDocumentNumber))
            {
                beneficiaryDocumentNumber = "*";
            }
            if (String.IsNullOrEmpty(beneficiaryName))
            {
                beneficiaryName = "*";
            }

            if (endDate != null && endDate != "*")
            {
                endDate = endDate + " 23:59:59";
            }

            int userNameId = Convert.ToInt32(userId);


            //**************************************

            SCRDTO.SearchParameterPaymentOrdersDTO searchParameter = new SCRDTO.SearchParameterPaymentOrdersDTO();

            searchParameter.BeneficiaryDocumentNumber = beneficiaryDocumentNumber;
            searchParameter.BeneficiaryFullName = beneficiaryName;
            searchParameter.Branch = new SCRDTO.BranchDTO() { Id = Convert.ToInt32(branchId) };
            searchParameter.EndDate = endDate;
            searchParameter.PaymentMethod = new PaymentMethod() { Id = Convert.ToInt32(paymentMethodId) };
            searchParameter.PaymentOrderNumber = paymentOrderNumber;
            searchParameter.PersonType = new PersonTypeDTO() { Id = Convert.ToInt32(personTypeId) };
            searchParameter.StartDate = startDate;
            searchParameter.UserId = userNameId;
            searchParameter.StatusId = status;
            searchParameter.IsDelivered = IsDelivered;
            searchParameter.IsReconciled = IsReconciled;
            searchParameter.IsAccounting = IsAccounting;

            List<SCRDTO.PaymentOrderDTO> paymentOrderDTOs = DelegateService.accountingAccountsPayableService.SearchPaymentOrders(searchParameter);

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;

            List<object> paymentOrders = new List<object>();
            foreach (SCRDTO.PaymentOrderDTO paymentOrder in paymentOrderDTOs)
            {

                string statusPaymentOrder = "";

                switch (paymentOrder.Status)
                {
                    case (int)PaymentOrderStatus.Active:
                        statusPaymentOrder = @Global.Active;
                        break;
                    case (int)PaymentOrderStatus.Applied:
                        statusPaymentOrder = @Global.Applied;
                        break;
                    case (int)PaymentOrderStatus.Authorized:
                        statusPaymentOrder = @Global.Authorized;
                        break;
                    case (int)PaymentOrderStatus.Canceled:
                        statusPaymentOrder = @Global.Annulled;
                        break;
                    case (int)PaymentOrderStatus.Forwarded:
                        statusPaymentOrder = @Global.Forwarded;
                        break;
                    case (int)PaymentOrderStatus.Paid:
                        statusPaymentOrder = @Global.Paid;
                        break;
                    case (int)PaymentOrderStatus.Rejected:
                        statusPaymentOrder = @Global.Rejected;
                        break;
                    default:
                        statusPaymentOrder = @Global.Active;
                        break;
                }

                paymentOrders.Add(new
                {
                    PaymentOrderCode = paymentOrder.PaymentOrderCode,
                    PaymentSourceCode = paymentOrder.PaymentSourceCode,
                    PaymentSourceName = paymentOrder.PaymentSourceName,
                    IncomeAmount = paymentOrder.IncomeAmount < 0 ?
                           paymentOrder.IncomeAmount.ToString("C") :
                           string.Format(new CultureInfo("en-US"), "{0:C}", paymentOrder.IncomeAmount),
                    UserName = paymentOrder.UserName,
                    UserId = paymentOrder.UserId,
                    BranchCode = paymentOrder.BranchCode,
                    BranchName = paymentOrder.BranchName,
                    AccountingDate = paymentOrder.AccountingDate,
                    AdmissionDate = paymentOrder.AdmissionDate,
                    IndividualId = paymentOrder.IndividualId,
                    PayerName = paymentOrder.PayerName,
                    BeneficiaryDocumentNumber = paymentOrder.BeneficiaryDocumentNumber,
                    BeneficiaryName = paymentOrder.BeneficiaryName,
                    CurrencyCode = paymentOrder.CurrencyCode,
                    CurrencyName = paymentOrder.CurrencyName,
                    ExchangeRate = paymentOrder.ExchangeRate,
                    CheckNumber = paymentOrder.CheckNumber,
                    EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate,
                    BranchPayCode = paymentOrder.BranchPayCode,
                    BranchPayName = paymentOrder.BranchPayName,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentOrder.Amount),
                    TempImputationCode = paymentOrder.TempImputationCode,
                    CompanyCode = paymentOrder.CompanyCode,
                    CompanyName = paymentOrder.CompanyName,
                    BankAccountNumber = paymentOrder.BankAccountNumber,
                    AccountBankCode = paymentOrder.AccountBankCode,
                    PersonTypeCode = paymentOrder.PersonTypeCode,
                    PersonTypeName = paymentOrder.PersonTypeName,
                    CancellationDate = paymentOrder.CancellationDate,
                    PaymentMethodCode = paymentOrder.PaymentMethodCode,
                    PaymentMethodName = paymentOrder.PaymentMethodName,
                    BankName = paymentOrder.BankName,
                    PayTo = paymentOrder.PayTo,
                    Status = paymentOrder.Status,
                    StatusDescription = statusPaymentOrder,
                    Rows = paymentOrder.Rows,
                    SectorName = paymentOrder.SectorName,

                    //ACB
                    TechnicalAuthorization = paymentOrder.TechnicalAuthorization,
                    TransferAccount = paymentOrder.TransferAccount,
                    TransferNumber = paymentOrder.TransferNumber,
                    DeliveryDate = paymentOrder.DeliveryDate,
                    ImputationId = paymentOrder.ImputationId,
                    Comments = paymentOrder.Comments,
                    CancellationNumber = paymentOrder.CancellationNumber,
                    Observation = paymentOrder.Observation,

                    BankAccountNumberPerson = paymentOrder.BankAccountNumberPerson,
                    BankNamePerson = paymentOrder.BankNamePerson
                });
            }

            return Json(new { aaData = paymentOrders, total = paymentOrders.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdatePaymentOrder
        /// Actualiza órdenes de pago
        /// </summary>
        /// <param name="paymentOrderModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdatePaymentOrder(PaymentOrderModel paymentOrderModel)
        {
            bool isUpdatedPaymentOrder = true;
            int returned = 0;

            PaymentOrderDTO paymentOrderTransaction = new PaymentOrderDTO();

            paymentOrderTransaction.BankAccountPerson = new BankAccountPersonDTO() { Id = paymentOrderModel.AccountBankId };
            paymentOrderTransaction.AccountingDate = paymentOrderModel.AccountingDate;
            paymentOrderTransaction.Amount = new AmountDTO()
            {
                Currency = new SCRDTO.CurrencyDTO() { Id = paymentOrderModel.CurrencyId },
                Value = paymentOrderModel.PaymentAmount
            };
            paymentOrderTransaction.LocalAmount = new AmountDTO() { Value = paymentOrderModel.PaymentIncomeAmount };
            paymentOrderTransaction.ExchangeRate = new ExchangeRateDTO() { BuyAmount = paymentOrderModel.ExchangeRate };
            paymentOrderTransaction.Beneficiary = new IndividualDTO() { IndividualId = paymentOrderModel.IndividualId };
            paymentOrderTransaction.Branch = new SCRDTO.BranchDTO() { Id = paymentOrderModel.BranchId };
            paymentOrderTransaction.BranchPay = new SCRDTO.BranchDTO() { Id = paymentOrderModel.BranchPayId };
            paymentOrderTransaction.Company = new CompanyDTO() { IndividualId = paymentOrderModel.CompanyId };
            paymentOrderTransaction.EstimatedPaymentDate = paymentOrderModel.EstimatedPaymentDate;
            paymentOrderTransaction.Id = paymentOrderModel.PaymentOrderItemId;
            paymentOrderTransaction.IsTemporal = true;
            paymentOrderTransaction.PaymentMethod = new PaymentMethod() { Id = paymentOrderModel.PaymentMethodId };
            paymentOrderTransaction.PaymentSource = new ConceptSourceDTO() { Id = paymentOrderModel.PaymentSourceId };
            paymentOrderTransaction.PayTo = paymentOrderModel.PayTo;
            paymentOrderTransaction.PersonType = new PersonTypeDTO();
            paymentOrderTransaction.PersonType.Id = paymentOrderModel.PersonTypeId;
            paymentOrderTransaction.Status = Convert.ToInt32(PaymentOrderStatus.Active);
            paymentOrderTransaction.UserId = _commonController.GetUserIdByName(User.Identity.Name);
            paymentOrderTransaction.Observation = paymentOrderModel.Observation;

            isUpdatedPaymentOrder = DelegateService.accountingAccountsPayableService.UpdatePaymentOrder(paymentOrderTransaction);

            if (isUpdatedPaymentOrder)
            {
                returned = 1;
            }
            else
            {
                returned = -1;
            }

            return Json(returned, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdatePaymentOrderStatus
        /// Actualiza el estado de las órdenes de pago temporales
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdatePaymentOrderStatus(int paymentOrderId, string comments, int statusId)
        {
            PaymentOrderDTO paymentOrder = new PaymentOrderDTO();

            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            paymentOrder.BankAccountPerson = new BankAccountPersonDTO() { Id = -1 };
            paymentOrder.AccountingDate = accountingDate;
            paymentOrder.Amount = new AmountDTO()
            {
                Currency = new SCRDTO.CurrencyDTO() { Id = -1 },
                Value = 0
            };
            paymentOrder.LocalAmount = new AmountDTO() { Value = 0 };
            paymentOrder.ExchangeRate = new ExchangeRateDTO() { BuyAmount = 0 };
            paymentOrder.Beneficiary = new IndividualDTO() { IndividualId = -1 };
            paymentOrder.Branch = new SCRDTO.BranchDTO() { Id = -1 };
            paymentOrder.BranchPay = new SCRDTO.BranchDTO() { Id = -1 };
            paymentOrder.Company = new CompanyDTO() { IndividualId = -1 };
            paymentOrder.EstimatedPaymentDate = accountingDate;
            paymentOrder.Id = paymentOrderId;
            paymentOrder.IsTemporal = true;
            paymentOrder.PaymentMethod = new PaymentMethod() { Id = -1 };
            paymentOrder.PaymentSource = new ConceptSourceDTO() { Id = -1 };
            paymentOrder.PayTo = "";
            paymentOrder.PersonType = new PersonTypeDTO() { Id = -1 };
            paymentOrder.Status = Convert.ToInt32(PaymentOrderStatus.Active);

            return Json(DelegateService.accountingAccountsPayableService.UpdatePaymentOrder(paymentOrder), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GeneratePaymentOrdersToExcel
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentOrderNumber"></param>
        /// <param name="personTypeId"></param>
        /// <param name="beneficiaryIndividualId"></param>
        /// <param name="beneficiaryDocumentNumber"></param>
        /// <param name="beneficiaryName"></param>
        /// <param name="status"></param>
        /// <param name="IsDelivered"></param>
        /// <param name="IsReconciled"></param>
        /// <param name="IsAccounting"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GeneratePaymentOrdersToExcel(string branchId, string userId, string paymentMethodId, string startDate,
                                                 string endDate, string paymentOrderNumber, string personTypeId,
                                                 string beneficiaryIndividualId, string beneficiaryDocumentNumber,
                                                 string beneficiaryName, int status, bool? IsDelivered,
                                                 bool? IsReconciled, bool? IsAccounting)
        {
            if (String.IsNullOrEmpty(branchId))
            {
                branchId = "-3";
            }
            if (String.IsNullOrEmpty(userId))
            {
                userId = "-1";
            }
            if (String.IsNullOrEmpty(paymentMethodId))
            {
                paymentMethodId = "-1";
            }
            if (String.IsNullOrEmpty(startDate))
            {
                startDate = "*";
            }
            if (String.IsNullOrEmpty(endDate))
            {
                endDate = "*";
            }
            if (String.IsNullOrEmpty(paymentOrderNumber))
            {
                paymentOrderNumber = "*";
            }
            if (String.IsNullOrEmpty(personTypeId))
            {
                personTypeId = "-1";
            }
            if (String.IsNullOrEmpty(beneficiaryDocumentNumber))
            {
                beneficiaryDocumentNumber = "*";
            }
            if (String.IsNullOrEmpty(beneficiaryName))
            {
                beneficiaryName = "*";
            }

            if (endDate != null && endDate != "*")
            {
                endDate = endDate + " 23:59:59";
            }

            int userNameId = Convert.ToInt32(userId);

            //**************************************

            SCRDTO.SearchParameterPaymentOrdersDTO searchParameterPaymentOrder = new SCRDTO.SearchParameterPaymentOrdersDTO();

            searchParameterPaymentOrder.BeneficiaryDocumentNumber = beneficiaryDocumentNumber;
            searchParameterPaymentOrder.BeneficiaryFullName = beneficiaryName;
            searchParameterPaymentOrder.Branch = new SCRDTO.BranchDTO() { Id = Convert.ToInt32(branchId) };
            searchParameterPaymentOrder.EndDate = endDate;
            searchParameterPaymentOrder.PaymentMethod = new PaymentMethod() { Id = Convert.ToInt32(paymentMethodId) };
            searchParameterPaymentOrder.PaymentOrderNumber = paymentOrderNumber;
            searchParameterPaymentOrder.PersonType = new PersonTypeDTO() { Id = Convert.ToInt32(personTypeId) };
            searchParameterPaymentOrder.StartDate = startDate;
            searchParameterPaymentOrder.UserId = userNameId;
            searchParameterPaymentOrder.StatusId = status;
            searchParameterPaymentOrder.IsDelivered = IsDelivered;
            searchParameterPaymentOrder.IsReconciled = IsReconciled;
            searchParameterPaymentOrder.IsAccounting = IsAccounting;

            List<SCRDTO.PaymentOrderDTO> paymentOrders = DelegateService.accountingAccountsPayableService.SearchPaymentOrders(searchParameterPaymentOrder);

            MemoryStream dataStream = ExportPaymentOrder(ConvertPaymentOrderToDataTable(paymentOrders));

            return File(dataStream.ToArray(), "application/vnd.ms-excel", "ListadoOrdenDePagoExcel.xls");
        }

        /// <summary>
        /// ExportPaymentOrder
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportPaymentOrder(DataTable dataTable)
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
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
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

            var headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0).SetCellValue(@Global.PaymentOrderNumber);
            headerRow.CreateCell(1).SetCellValue(@Global.Sector);
            headerRow.CreateCell(2).SetCellValue(@Global.PaymentMethod);
            headerRow.CreateCell(3).SetCellValue(@Global.LocalAmount);
            headerRow.CreateCell(4).SetCellValue(@Global.User);
            headerRow.CreateCell(5).SetCellValue(@Global.Branch);
            headerRow.CreateCell(6).SetCellValue(@Global.AccountingDate);
            headerRow.CreateCell(7).SetCellValue(@Global.PayerName);
            headerRow.CreateCell(8).SetCellValue(@Global.CheckNumber);
            headerRow.CreateCell(9).SetCellValue(@Global.BankAccountNumber);
            headerRow.CreateCell(10).SetCellValue(@Global.CancellationDate);
            headerRow.CreateCell(11).SetCellValue(@Global.Currency);
            headerRow.CreateCell(12).SetCellValue(@Global.Status);

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
        /// ConvertPaymentOrderToDataTable
        /// </summary>
        /// <param name="paymentOrders"></param>
        /// <returns>DataTable</returns>
        private DataTable ConvertPaymentOrderToDataTable(List<SCRDTO.PaymentOrderDTO> paymentOrders)
        {
            DataTable dataTable = new DataTable();
            string statusPaymentOrder = "";

            var headerRows = new List<string>(7);

            headerRows.Add(@Global.PaymentOrderNumber);
            headerRows.Add(@Global.Sector);
            headerRows.Add(@Global.PaymentMethod);
            headerRows.Add(@Global.LocalAmount);
            headerRows.Add(@Global.User);
            headerRows.Add(@Global.Branch);
            headerRows.Add(@Global.AccountingDate);
            headerRows.Add(@Global.PayerName);
            headerRows.Add(@Global.CheckNumber);
            headerRows.Add(@Global.BankAccountNumber);
            headerRows.Add(@Global.CancellationDate);
            headerRows.Add(@Global.Currency);
            headerRows.Add(@Global.Status);

            for (int j = 0; j < headerRows.Count; j++)
            {
                dataTable.Columns.Add(headerRows[j]);
            }

            try
            {
                foreach (SCRDTO.PaymentOrderDTO paymentOrder in paymentOrders)
                {
                    switch (paymentOrder.Status)
                    {
                        case (int)PaymentOrderStatus.Active:
                            statusPaymentOrder = @Global.Active.ToString();
                            break;
                        case (int)PaymentOrderStatus.Applied:
                            statusPaymentOrder = @Global.Applied.ToString();
                            break;
                        case (int)PaymentOrderStatus.Authorized:
                            statusPaymentOrder = @Global.Authorized.ToString();
                            break;
                        case (int)PaymentOrderStatus.Canceled:
                            statusPaymentOrder = @Global.Annulled.ToString();
                            break;
                        case (int)PaymentOrderStatus.Forwarded:
                            statusPaymentOrder = @Global.Forwarded.ToString();
                            break;
                        case (int)PaymentOrderStatus.Paid:
                            statusPaymentOrder = @Global.Paid.ToString();
                            break;
                        case (int)PaymentOrderStatus.Rejected:
                            statusPaymentOrder = @Global.Rejected.ToString();
                            break;
                        default:
                            statusPaymentOrder = PaymentOrderStatus.Active.ToString();
                            break;
                    }

                    DataRow dataRow = dataTable.NewRow();

                    dataRow[0] = paymentOrder.PaymentOrderCode;
                    dataRow[1] = paymentOrder.SectorName;
                    dataRow[2] = paymentOrder.PaymentMethodName;
                    dataRow[3] = String.Format(new CultureInfo("en-US"), "{0:C}", paymentOrder.IncomeAmount);
                    dataRow[4] = paymentOrder.UserName;
                    dataRow[5] = paymentOrder.BranchName;
                    dataRow[6] = paymentOrder.AccountingDate;
                    dataRow[7] = paymentOrder.PayerName;
                    dataRow[8] = paymentOrder.CheckNumber;
                    dataRow[9] = paymentOrder.BankAccountNumber;
                    dataRow[10] = paymentOrder.CancellationDate;
                    dataRow[11] = paymentOrder.CurrencyName;
                    dataRow[12] = statusPaymentOrder;

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return dataTable;
        }
        #endregion

        #region TempPayment Order

        /// <summary>
        /// GetTempPaymentOrderByTempPaymentOrderId
        /// Recupera órdenes de pago temporales por el Id temporal
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempPaymentOrderByTempPaymentOrderId(int tempPaymentOrderId)
        {


            SCRDTO.TempPaymentOrderDTO tempPaymentOrder = DelegateService.accountingAccountsPayableService.GetTempPaymentOrderByTempId(tempPaymentOrderId);

            var person = DelegateService.uniquePersonService.GetPersonByIndividualId(tempPaymentOrder.IndividualId);

            if (person != null)
            {
                tempPaymentOrder.BeneficiaryDocumentNumber = person.IdentificationDocument.Number;
                tempPaymentOrder.BeneficiaryName = person.SurName + " " + person.Name;
            }
            else
            {
                var company = DelegateService.uniquePersonService.GetCompanyByIndividualId(tempPaymentOrder.IndividualId);

                tempPaymentOrder.BeneficiaryDocumentNumber = company.IdentificationDocument.Number;
                tempPaymentOrder.BeneficiaryName = company.FullName;
            }         


            return Json(tempPaymentOrder, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// IsBoolean
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult IsBoolean()
        {
            List<object> booleans = new List<object>();

            booleans.Add(new { Description = @Global.Yes, Id = 1 });
            booleans.Add(new { Description = @Global.No, Id = 0 });
            return new UifSelectResult(booleans);
        }

        /// <summary>
        /// GetPaymentOrderStatus
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentOrderStatus()
        {
            List<object> paymentOrderStatus = new List<object>();
            paymentOrderStatus.Add(new { Id = 0, Description = PaymentOrderStatus.Canceled.ToString() });
            paymentOrderStatus.Add(new { Id = 1, Description = PaymentOrderStatus.Active.ToString() });
            paymentOrderStatus.Add(new { Id = 2, Description = PaymentOrderStatus.Authorized.ToString() });
            paymentOrderStatus.Add(new { Id = 3, Description = PaymentOrderStatus.Applied.ToString() });
            paymentOrderStatus.Add(new { Id = 4, Description = PaymentOrderStatus.Paid.ToString() });
            paymentOrderStatus.Add(new { Id = 5, Description = PaymentOrderStatus.Rejected.ToString() });
            paymentOrderStatus.Add(new { Id = 6, Description = PaymentOrderStatus.Forwarded.ToString() });

            return new UifSelectResult(paymentOrderStatus);
        }

        #endregion

        #region PaymentOrderAuthorization

        /// <summary>
        /// GetPaymentOrderAuthorization
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentOrderAuthorization(int branchId, int paymentMethodId, string estimatedPaymentDate)
        {
            //int userPayOrder;
            int userId = _commonController.GetUserIdByName(User.Identity.Name);


            DateTime? estimatedPaymentDates = new DateTime();

            if (estimatedPaymentDate == null)
            {
                estimatedPaymentDates = Convert.ToDateTime("01/01/1900");
            }
            else
            {
                estimatedPaymentDates = Convert.ToDateTime(estimatedPaymentDate);
            }

            List<object> authorizedPaymentOrder = new List<object>();

            try
            {
                List<PaymentOrderDTO> paymentOrders = DelegateService.accountingAccountsPayableService.GetPaymentOrderAuthorization(branchId, paymentMethodId, Convert.ToDateTime(estimatedPaymentDates), userId);

                if (paymentOrders.Count > 0)
                {
                    foreach (PaymentOrderDTO paymentOrder in paymentOrders)
                    {

                        authorizedPaymentOrder.Add(new
                        {
                            PaymentOrderCode = paymentOrder.Id,
                            PersonType = paymentOrder.PersonType.Description,
                            BeneficiaryName = paymentOrder.Beneficiary.Name,
                            Branch = paymentOrder.Branch.Description,
                            PaymentMethod = paymentOrder.PaymentMethod.Description,
                            EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate.ToShortDateString(),
                            Currencies = paymentOrder.Amount.Currency.Description,
                            Sarlaft = "",
                            UserId = paymentOrder.UserId,
                            UserName = DelegateService.uniqueUserService.GetUserById(paymentOrder.UserId).AccountName
                        });
                    }
                }

                return new UifTableResult(authorizedPaymentOrder);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// SavePaymentOrderAuthorization
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SavePaymentOrderAuthorization(TransferPaymentOrderModel transferPaymentOrderModel)
        {
            try
            {
                /*el que está logeado es el que autoriza*/
                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                PaymentOrderDTO paymentOrder = new PaymentOrderDTO();

                foreach (PaymentOrderTransferModel PaymentOrderTransferModel in transferPaymentOrderModel.PaymentOrdersItems)
                {
                    paymentOrder = new PaymentOrderDTO()
                    {
                        Id = PaymentOrderTransferModel.PaymentOrderId,
                        UserId = userId
                    };

                    DelegateService.accountingAccountsPayableService.SavePaymentOrderAuthorization(paymentOrder);
                }                

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

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

    }
}