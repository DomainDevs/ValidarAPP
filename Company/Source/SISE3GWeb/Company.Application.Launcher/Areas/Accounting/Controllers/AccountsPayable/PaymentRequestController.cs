//System
//Crystal
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.TaxServices.DTOs;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
//Sistran Company
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.PaymentRequest;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
//Sistran Core
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;
using AccountingRuleModels = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using GLADTO = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using GLDTO = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTOPAY = Sistran.Core.Application.AccountingServices.DTOs.Payments;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.AccountsPayable
{
    public class PaymentRequestController : Controller
    {
        #region Instance Variables        
        readonly CommonController _commonController = new CommonController();
        #endregion

        #region Public Methods 

        /// <summary>
        /// MainPaymentRequest
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPaymentRequest()
        {
            try
            {         

                ViewBag.PaymentSources = Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceOthers"]); // 4
                ViewBag.AccountingDate = _commonController.GetAccountingDate();
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

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

                //Payment_Methods 
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

                //Setear valor por defaul de la sucursal de acuerdo al dw que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                //OBTIENE SI ESTA CONFIGURADO COMO MULTICOMPANIA 1 TRUE 0 FALSE
                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
                //OBTIENE LA COMPANIA POR DEFECTO SEGÚN EL USUARIO
                ViewBag.AccountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));

                ViewBag.SalePointBranchUserDefault = DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(_commonController.GetUserIdByName(User.Identity.Name), ViewBag.BranchDefault);

                return View("~/Areas/Accounting/Views/AccountsPayable/PaymentRequest/MainPaymentRequest.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetIndividualTaxByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetIndividualTaxByIndividualId(int individualId)
        {
            List<object> individualTaxes = new List<object>();

            try
            {
                var taxes = DelegateService.taxService.GetIndividualTaxCategoryCondition(individualId);
                foreach (IndividualTaxCategoryConditionDTO tax in taxes)
                {
                    individualTaxes.Add(new
                    {
                        TaxId = tax.TaxId,
                        TaxDescription = tax.TaxDescription,
                        IndividualId = tax.IndividualId,
                        TaxConditionId = tax.TaxConditionId,
                        TaxConditionDescription = tax.TaxConditionDescription,
                        TaxCategoryDescription = tax.TaxCategoryDescription,
                        HasNationalRate = true,
                        Rate = tax.Rate,
                        RateTypeCode = tax.RateTypeId,
                        RateDescription = tax.RateTypeDescription,
                        StateDescription = tax.StateId == 0 ? @Global.National : @Global.Local,
                        IsRetention = tax.IsRetention
                    });
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
            
            return new UifTableResult(individualTaxes);
        }

        /// <summary>
        /// GetTotalTax
        /// </summary>
        /// <param name="payerTypeId"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <param name="amount"></param>
        /// <param name="conditionId"></param>
        /// <param name="agentTypeId"></param>
        /// <param name="correlativeNumber"></param>
        /// <param name="currencyId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="categories"></param>
        /// <param name="coverageTaxId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalTax(int payerTypeId, int branchId, int individualId, double amount, int conditionId,
                                      int agentTypeId, int correlativeNumber, int currencyId, double exchangeRate, 
                                      string categories, int coverageTaxId)
        {
            int lineBusinessCode = 0;  //Para pagos varios no se aplica línea de negocio por lo que se pasa parámentro 0.
            branchId = 0; //Hasta parametrizar bien los impuestos
            Dictionary<int, int> taxCategories = new Dictionary<int, int>();
            Dictionary<int, int> rateCategories;
            Random random = new Random();
            int randomValues;

            decimal totalTax = 0;
            List<object> totalTaxes = new List<object>();

            decimal retentionAmount = 0;

            try
            {
                if (categories != "")
                {
                    string[] splitTaxCategories = categories.Split(';');
                    string category = string.Empty;
                    int keyTax;
                    int keyCategory;

                    for (int i = 0; i < splitTaxCategories.Length - 1; i++)
                    {
                        category = splitTaxCategories[i];
                        if (category.Length > 0)
                        {
                            keyTax = Convert.ToInt32(category);
                        }
                        else
                        {
                            keyTax = 0;
                        }
                        category = splitTaxCategories[i];
                        if (category.Length > 0)
                        {
                            keyCategory = Convert.ToInt32(category);
                        }
                        else
                        {
                            keyCategory = 0;
                        }

                        if (!taxCategories.ContainsKey(keyTax))
                        {
                            taxCategories.Add(keyTax, keyCategory);
                        }
                    }
                }

                // Para la retención
                rateCategories = new Dictionary<int, int>();
                rateCategories.Add(0, -2);
                rateCategories.Add(1, _commonController.GetUserByName(User.Identity.Name)[0].UserId);
                rateCategories.Add(2, payerTypeId);
                rateCategories.Add(3, agentTypeId);
                rateCategories.Add(4, correlativeNumber);
                rateCategories.Add(5, currencyId);
                randomValues = random.Next(10, 50);
                rateCategories.Add(6, randomValues);

                if (categories != "")
                {
                    totalTax = DelegateService.taxService.GetTotalTax(individualId, conditionId, taxCategories,
                                                          branchId, lineBusinessCode, exchangeRate, amount);

                    retentionAmount = DelegateService.taxService.GetTotalTax(individualId, conditionId, taxCategories,
                                                          branchId, -1, exchangeRate, amount);
                   
                    totalTaxes.Add(new
                    {
                        TaxValue = totalTax,
                        RetentionValue = retentionAmount,
                        RandomValues = randomValues
                    });
                }
                else
                {
                    totalTaxes.Add(new
                    {
                        TaxValue = 0,
                        RetentionValue = 0,
                        RandomValues = 0
                    });
                }

                return Json(totalTaxes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception )
            {                
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetIndividualTaxesByIndividualId
        /// Obtiene el impuesto por el individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetIndividualTaxesByIndividualId(int individualId)
        {
            List<object> individualTaxes = new List<object>();

            try
            {
                var taxes = DelegateService.taxService.GetIndividualTaxCategoryCondition(individualId);
                foreach (IndividualTaxCategoryConditionDTO tax in taxes)
                {
                    individualTaxes.Add(new
                    {
                        TaxId = tax.TaxId,
                        TaxDescription = tax.TaxDescription,
                        IndividualId = tax.IndividualId,
                        TaxConditionId = tax.TaxConditionId,
                        TaxConditionDescription = tax.TaxConditionDescription,
                        TaxCategoryDescription = tax.TaxCategoryDescription,
                        HasNationalRate = true,
                        Rate = tax.Rate,
                        RateTypeCode = tax.RateTypeId,
                        RateDescription = tax.RateTypeDescription,
                        StateDescription = tax.StateId == 0 ? @Global.National : @Global.Local,
                        IsRetention = tax.IsRetention
                    });
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
            
            return Json(individualTaxes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetEnabledPaymentTypes
        /// Obtiene los tipos de pago habilitados para solicitud de pagos varios
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetEnabledPaymentTypes()
        {
            return new UifSelectResult(DelegateService.accountingParameterService.GetEnablePaymentMethodType(true, false, false).OrderBy(o => o.Description).ToList());
        }

        /// <summary>
        /// GetPaymentMovementTypesByPaymentSourceId
        /// Obtiene movimientos de pago por el origen de pago
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentMovementTypesByPaymentSourceIdFilter(int paymentSourceId)
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetMovementTypesByConceptSourceFilter(new GLADTO.ConceptSourceDTO() { Id  = paymentSourceId }));
        }

        /// <summary>
        /// GetPaymentMovementTypesByPaymentSourceId
        /// Obtiene movimientos de pago por el origen de pago
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentMovementTypesByPaymentSourceId(int paymentSourceId)
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetMovementTypesByConceptSource(new GLADTO.ConceptSourceDTO() { Id = paymentSourceId }));
        }

        /// <summary>
        /// GetCostCenterByAccountingAccountId
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCostCenterByAccountingAccountId(int accountingAccountId)
        {            
            try
            {
                var costCenterExist = DelegateService.accountingAccountsPayableService.GetCostCenterByAccountingAccountId(accountingAccountId);

                if (costCenterExist.Count > 0)
                {
                    return new UifJsonResult(true, costCenterExist);
                }
                else
                {
                    return new UifJsonResult(false, Global.ItIsNotParameterized + " " + Global.CostCenter);
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, Global.GeneratesError + " " + e.Message);
            }
            //SMT-1758 Fin
        }

        /// <summary>
        /// GetBankAccountsByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBankAccountsByIndividualId(int individualId)
        {
            List<Object> bankAccounts = new List<Object>();

            var bankAccountPersons = DelegateService.accountingParameterService.GetBankAccountPersons();
            var personBankAccounts = bankAccountPersons.Where(r => (r.Individual.IndividualId.Equals(individualId))).ToList();

            foreach (BankAccountPersonDTO bankAccountPerson in personBankAccounts)
            {
                bankAccounts.Add(new
                {
                    AccountBankId = bankAccountPerson.Id,
                    IndividualId = bankAccountPerson.Individual.IndividualId,
                    BankDescription = bankAccountPerson.Bank.Description,
                    AccountTypeDescription = bankAccountPerson.BankAccountType.Description,
                    Number = bankAccountPerson.Number,
                    AccountBankEnabled = bankAccountPerson.IsEnabled,
                    Default = bankAccountPerson.IsDefault,
                    AccountBankEnabledDescription = bankAccountPerson.IsEnabled ? @Global.Yes : @Global.No,
                    DefaultDescription = bankAccountPerson.IsDefault ? @Global.Yes : @Global.No
                });
            }

            return new UifTableResult(bankAccounts);
        }

        /// <summary>
        /// SavePaymentRequest
        /// Graba solicitud de pagos varios incluidos sus impuestos.
        /// </summary>
        /// <param name="paymentRequestModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SavePaymentRequest(PaymentRequestModel paymentRequestModel)
        {
            try
            {
                bool isEnabledGeneralLedger = true;
                string saveDailyEntryMessage = "";

                PaymentRequestDTO paymentRequest = new PaymentRequestDTO();
                paymentRequest.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                paymentRequest.Beneficiary = new ACCDTO.IndividualDTO() { IndividualId = paymentRequestModel.IndividualId };
                paymentRequest.Branch = new SCRDTO.BranchDTO()
                {
                    Description = paymentRequestModel.PersonBankAccountId.ToString(),
                    Id = paymentRequestModel.BranchId
                };
                paymentRequest.Company = new CompanyDTO() { IndividualId = paymentRequestModel.CompanyId };
                paymentRequest.Currency = new SCRDTO.CurrencyDTO() { Id = paymentRequestModel.CurrencyId };
                paymentRequest.Description = paymentRequestModel.Description;
                paymentRequest.EstimatedDate = paymentRequestModel.PaymentEstimateDate;
                paymentRequest.Id = 0;
                paymentRequest.MovementType = new ACCDTO.MovementTypeDTO()
                {
                    ConceptSource = new ACCDTO.ConceptSourceDTO() { Id = Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceOthers"]) },
                    Id = paymentRequestModel.PaymentMovementTypeId
                };

                paymentRequest.PaymentMethod = new ACCDTOPAY.PaymentMethodDTO()
                {
                    Description = paymentRequestModel.PaymentMovementTypeId.ToString(),
                    Id = paymentRequestModel.PaymentMethodId
                };
                paymentRequest.PaymentRequestNumber = new PaymentRequestNumberDTO() {  };

                PaymentRequestTypes paymentRequestTypes = new PaymentRequestTypes();
                if (paymentRequestModel.PaymentRequestTypeId == 1)
                {
                    paymentRequestTypes = PaymentRequestTypes.Payment;
                }
                if (paymentRequestModel.PaymentRequestTypeId == 2)
                {
                    paymentRequestTypes = PaymentRequestTypes.Recovery;
                }
                if (paymentRequestModel.PaymentRequestTypeId == 3)
                {
                    paymentRequestTypes = PaymentRequestTypes.Void;
                }
                paymentRequest.PaymentRequestType = Convert.ToInt32(paymentRequestTypes);
                paymentRequest.PersonType = new PersonTypeDTO()
                {
                    Id = paymentRequestModel.PersonTypeId
                };
                paymentRequest.SalePoint = new ACCDTO.SalePointDTO() { Id = paymentRequestModel.SalePointId };
                paymentRequest.TotalAmount = new ACCDTO.AmountDTO()
                {
                    Currency = new SCRDTO.CurrencyDTO() { Id = paymentRequestModel.CurrencyId },
                    Value = Convert.ToDecimal(paymentRequestModel.TotalAmount)
                };
                paymentRequest.ExchangeRate = new ACCDTO.ExchangeRateDTO() { BuyAmount = 1 };
                paymentRequest.LocalAmount = new ACCDTO.AmountDTO() { Value = Convert.ToDecimal(paymentRequestModel.TotalAmount) };
                paymentRequest.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                paymentRequest.Transaction = null;

                List<VoucherDTO> vouchers = new List<VoucherDTO>();
                foreach (VoucherModel voucherModel in paymentRequestModel.Vouchers)
                {
                    VoucherDTO voucher = new VoucherDTO();
                    voucher.Currency = new SCRDTO.CurrencyDTO() { Id = voucherModel.VoucherCurrencyId };
                    voucher.Date = voucherModel.VoucherDate;
                    voucher.ExchangeRate = voucherModel.VoucherExchangeRate;
                    voucher.Id = 0;
                    voucher.Number = voucherModel.VoucherNumber;
                    voucher.Type = new VoucherTypeDTO() { Id = voucherModel.VoucherType };

                    List<VoucherConceptDTO> voucherConcepts = new List<VoucherConceptDTO>();
                    foreach (VoucherConceptModel voucherConceptModel in voucherModel.VoucherConcepts)
                    {
                        VoucherConceptDTO voucherConcept = new VoucherConceptDTO();
                        voucherConcept.AccountingConcept = new AccountingConceptDTO() { Id = voucherConceptModel.VoucherConceptPaymentConcept };
                        voucherConcept.Amount = new ACCDTO.AmountDTO()
                        {                             
                             Value = voucherConceptModel.VoucherConceptValue,
                        };
                        voucherConcept.CostCenter = new Sistran.Core.Application.AccountingServices.DTOs.Search.CostCenterDTO() { CostCenterId = voucherConceptModel.VoucherConceptCostCenterId };
                        voucherConcept.Id = 0;

                        List<VoucherConceptTaxDTO> voucherConceptTaxes = new List<VoucherConceptTaxDTO>();
                        if (voucherConceptModel.VoucherConceptTaxes != null)
                        {
                            foreach (VoucherConceptTaxModel voucherConceptTaxModel in voucherConceptModel.VoucherConceptTaxes)
                            {
                                VoucherConceptTaxDTO voucherConceptTax = new VoucherConceptTaxDTO();
                                voucherConceptTax.Id = 0;
                                voucherConceptTax.Tax = new ACCDTO.TaxDTO() { Id = voucherConceptTaxModel.TaxId };
                                voucherConceptTax.TaxCategory = new TaxCategoryDTO() { Id = voucherConceptTaxModel.TaxCategoryId };
                                voucherConceptTax.TaxCondition = new TaxConditionDTO() { Id = voucherConceptTaxModel.TaxConditionId };
                                voucherConceptTax.TaxeBaseAmount = voucherConceptTaxModel.TaxBase;
                                voucherConceptTax.TaxeRate = voucherConceptTaxModel.TaxRate;
                                voucherConceptTax.TaxValue = System.Math.Round(voucherConceptTaxModel.TaxValue, 2);

                                voucherConceptTaxes.Add(voucherConceptTax);
                            }
                        }

                        voucherConcept.VoucherConceptTaxes = voucherConceptTaxes;

                        voucherConcepts.Add(voucherConcept);
                    }

                    voucher.VoucherConcepts = voucherConcepts;
                    vouchers.Add(voucher);
                }

                paymentRequest.Vouchers = vouchers;
                

                paymentRequest = DelegateService.accountingAccountsPayableService.SavePaymentRequest(paymentRequest);

                #region Accounting

                //Disparo el metodo de contabilidad
                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    saveDailyEntryMessage = RecordPaymentRequestByPaymentRequestId(Convert.ToInt32(paymentRequest.Id), paymentRequestModel.CompanyId, paymentRequestModel.SalePointId);
                }
                else
                {
                    saveDailyEntryMessage = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                    isEnabledGeneralLedger = false;
                }

                #endregion Accounting

                var paymentRequestVarious = new
                {
                    PaymentRequestId = paymentRequest.Id.ToString(),
                    PaymentRequestNumber = paymentRequest.PaymentRequestNumber.Number.ToString(),
                    SaveDailyEntryMessage = saveDailyEntryMessage,
                    IsEnabledGeneralLedger = isEnabledGeneralLedger
                };

                return Json(paymentRequestVarious, JsonRequestBehavior.AllowGet);
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
        /// ValidParameterAccountConcept
        /// Valida que los conceptos de pago seleccionados tengan la parametrización contable
        /// </summary>
        /// <param name="conceptId"></param>
        /// <returns>JsonResult</returns>        
        public JsonResult ValidParameterAccountConcept(int conceptId)
        {
            try
            {
                int moduleDateId = Convert.ToInt32(ConfigurationManager.AppSettings["CashExpenseModuleId"]);
                string msjAlert = "";
                bool resultEval = false;
                int countCondition = 0;
                int countCredit = 0;
                int countDebit = 0;
                int countAccount = 0;

                List<AccountingRuleModels.AccountingRulePackageDTO> accountingRulePackage =
                        DelegateService.entryParameterApplicationService.GetAccountingRulePackages(moduleDateId);

                List<AccountingRuleModels.ParameterDTO> parameters = DelegateService.entryParameterApplicationService.GetParameters(moduleDateId);
                var parameterId = parameters?.FirstOrDefault(p => p.Description == "CONCEPT_ID")?.Id;

                if (parameterId != null)
                {
                    foreach (AccountingRuleModels.AccountingRulePackageDTO rulePackageItem in accountingRulePackage)
                    {
                        foreach (AccountingRuleModels.AccountingRuleDTO accountingRuleItem in rulePackageItem.AccountingRules)
                        {
                            List<AccountingRuleModels.ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRuleItem);
                            var conditionFind = conditions.Where(c => c.Value == Convert.ToDecimal(conceptId) && c.Parameter.Id == parameterId).ToList();

                            if (conditionFind.Count > 0)
                            {
                                var conditionFind2 = conditions.Where(c => c.IdResult != 0).ToList();

                                if (conditionFind2.Count > 0)
                                {
                                    AccountingRuleModels.ResultDTO result = DelegateService.entryParameterApplicationService.GetResult(conditionFind2[0]);
                                    if (result.AccountingAccount == "")
                                    {
                                        countAccount++;
                                    }
                                    else if (result.AccountingNature == Convert.ToInt32(AccountingNatures.Credit))
                                    {
                                        countCredit++;
                                        countCondition++;
                                    }
                                    else if (result.AccountingNature == Convert.ToInt32(AccountingNatures.Debit))
                                    {
                                        countDebit++;
                                        countCondition++;
                                    }

                                }

                            }

                        }

                    }
                }
                else
                {
                    return Json(new { success = false, result = Language.WarningParameterNotFound }, JsonRequestBehavior.AllowGet);
                }
                
                if (countCondition > 0 && countCredit == countDebit)
                    {
                    resultEval = true;
                }
                else {
                    msjAlert = Global.ValidateParameterAccountConcept + " : " + conceptId;
                }

                return Json(new { success = resultEval , result = msjAlert }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, result = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        /// <summary>
        /// GetIndividualTaxCategoryConditionByIndividualId
        /// Obtiene categorias de impuestos de una persona.
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetIndividualTaxCategoryConditionByIndividualId(int individualId)
        {
            int taxCodeAux = 0;
            int selected = 0;
            int count = 0;

            List<object> individualTaxCategoryConditions = new List<object>();

            var taxes = DelegateService.taxService.GetIndividualTaxCategoryCondition(individualId);

            foreach (IndividualTaxCategoryConditionDTO tax in taxes)
            {
                count++;
                int taxCode = tax.TaxId;
                if (taxCode != taxCodeAux)
                {
                    selected = 1;
                    taxCodeAux = taxCode;
                }
                else
                {
                    selected = 0;
                }

                individualTaxCategoryConditions.Add(new
                {
                    TaxCode = tax.TaxId,
                    TaxDescription = tax.TaxDescription,
                    TaxCategoryCode = tax.TaxCategoryId,
                    TaxCategoryDescription = tax.TaxCategoryDescription,
                    TaxConditionCode = tax.TaxConditionId,
                    TaxConditionDescription = tax.TaxConditionDescription,
                    Rate = tax.Rate,
                    RateTypeCode = tax.RateTypeId,
                    RateDescription = tax.RateTypeDescription,
                    IsRetention = tax.IsRetention,
                    Select = selected,
                    TemporalId = count
                });
            }

            return new UifTableResult(individualTaxCategoryConditions);
        }

        /// <summary>
        /// GetPersonBankAccountsByIndividualId
        /// Se obtiene las cuentas bancarias de un individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPersonBankAccountsByIndividualId(int individualId)
        {
            //Se obtiene el listado de cuentas bancarias
            List<BankAccountPersonDTO> bankAccountPersons = new List<BankAccountPersonDTO>();
            bankAccountPersons = DelegateService.accountingParameterService.GetBankAccountPersons();

            //Se filtra el listado por IndividualId.
            bankAccountPersons = (from BankAccountPersonDTO item in bankAccountPersons where item.Individual.IndividualId == individualId && item.IsDefault select item).ToList();

            return Json(bankAccountPersons, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExcludeElectronicPaymentFromEnablePaymentMethods
        /// Excluye el pago electrónico de los métodos de pago habilitados
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExcludeElectronicPaymentFromEnablePaymentMethods()
        {
            List<SCRDTO.PaymentMethodTypeDTO> paymentMethodTypeDTOs = DelegateService.accountingParameterService.GetEnablePaymentMethodType(true, false, false).OrderBy(o => o.Description).ToList();
            var paymentMethodTypes = (from item in paymentMethodTypeDTOs where item.PaymentTypeCode != Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodElectronicPayment"]) select item).ToList();
            return new UifSelectResult(paymentMethodTypes);
        }

        /// <summary>
        /// LoadPaymentRequestReport
        /// Llena el reporte con los datos de la solicitud de pago
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public void LoadPaymentRequestReport(int paymentRequestId)
        {
            List<OtherPaymentsRequestReportModel> summariesReports = new List<OtherPaymentsRequestReportModel>();

            SCRDTO.OtherPaymentsRequestReportHeaderDTO paymentsRequest = DelegateService.accountingAccountsPayableService.OtherPaymentRequestReport(paymentRequestId);

            // Llena Resumen de Movimientos
            foreach (SCRDTO.OtherPaymentRequestReportDetails otherPaymentdetails in paymentsRequest.OtherPaymentRequestReportDetails)
            {
                OtherPaymentsRequestReportModel summaryMovement = new OtherPaymentsRequestReportModel();

                summaryMovement.PaymentRequestId = paymentsRequest.PaymentRequestId;
                summaryMovement.Number = paymentsRequest.Number;
                summaryMovement.EstimatedDate = paymentsRequest.EstimatedDate;
                summaryMovement.PersonTypeId = paymentsRequest.PersonTypeId;
                summaryMovement.PersonTypeDescription = paymentsRequest.PersonTypeDescription;
                summaryMovement.IndividualId = paymentsRequest.IndividualId;
                summaryMovement.DocumentNumber = paymentsRequest.DocumentNumber;
                summaryMovement.Name = paymentsRequest.Name;
                summaryMovement.CurrencyId = paymentsRequest.CurrencyId;
                summaryMovement.CurrencyDescription = paymentsRequest.CurrencyDescription;
                summaryMovement.RegistrationDate = paymentsRequest.RegistrationDate;
                summaryMovement.TotalAmountHeader = paymentsRequest.TotalAmount;
                summaryMovement.UserId = paymentsRequest.UserId;
                summaryMovement.UserAccountName = paymentsRequest.UserAccountName;
                summaryMovement.PaymentMethodId = paymentsRequest.PaymentMethodId;
                summaryMovement.PaymentMethodDescription = paymentsRequest.PaymentMethodDescription;
                summaryMovement.PaymentRequestDescription = paymentsRequest.PaymentRequestDescription;
                summaryMovement.BillId = paymentsRequest.CollectId;

                summaryMovement.VoucherTypeId = otherPaymentdetails.VoucherTypeId;
                summaryMovement.VoucherTypeDescription = otherPaymentdetails.VoucherTypeDescription;
                summaryMovement.VoucherNumber = otherPaymentdetails.VoucherNumber;
                summaryMovement.TotalAmount = otherPaymentdetails.TotalAmount;
                summaryMovement.Taxes = otherPaymentdetails.Taxes;
                summaryMovement.Retentions = otherPaymentdetails.Retentions;

                summariesReports.Add(summaryMovement);
            }

            var reportSource = summariesReports;
            var reportName = "Areas//Accounting//Reports//OtherPaymentsRequest//OtherPaymentsRequest.rpt";
            ReportDocument reportDocument = new ReportDocument();

            string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

            reportDocument.Load(reportPath);

            // Llena Reporte Principal
            reportDocument.SetDataSource(reportSource);

            reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "PaymentRequest");
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// RecordPaymentRequestByPaymentRequestId
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="salePointId"></param>
        /// <returns>string</returns>
        public string RecordPaymentRequestByPaymentRequestId(int paymentRequestId, int accountingCompanyId, int salePointId)
        {
            string message = "";
            string description = "";
            int paymentSource = Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceOthers"]);
            int userId = SessionHelper.GetUserId();
            int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["TransactionNumber"]);
            int transactionNumber = (int)DelegateService.commonService.GetParameterByParameterId(parameterId).NumberParameter;

            try
            {
                #region Parameters

                if (paymentSource == Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceOthers"]))
                {
                    description = Global.AccountOtherPaymentRequest + " " + Convert.ToString(paymentRequestId);
                }
                if (paymentSource == Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceClaimsPaymentRequest"]))
                {
                    description = Global.AccountClaimPaymentRequest + " " + Convert.ToString(paymentRequestId);
                }
                if (paymentSource == Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceRecovery"]))
                {
                    description = Global.AccountRecoveyPaymentRequest + " " + Convert.ToString(paymentRequestId);
                }
                if (paymentSource == Convert.ToInt32(ConfigurationManager.AppSettings["PaymentSourceSalvage"]))
                {
                    description = Global.AccountSalvagePaymentRequest + " " + Convert.ToString(paymentRequestId);
                }

                List<SCRDTO.PaymentRequestAccountingParameterDTO> paymentRequestParameters = DelegateService.accountingAccountService.GetPaymentRequestAccountingParameters(paymentRequestId, paymentSource, 0, 0, 0, 0);

                //Listado en donde se llevaran los grupos de parametros al servicio
                List<List<AccountingRuleModels.ParameterDTO>> parametersCollection = new List<List<AccountingRuleModels.ParameterDTO>>();

                #endregion Parameters

                #region DailyEntryHeader

                int moduleDateId = Convert.ToInt32(ConfigurationManager.AppSettings["CashExpenseModuleId"]);

                //Se arma la cabecera del asiento
                JournalEntryDTO journalEntry = new JournalEntryDTO();

                journalEntry.Id = 0;
                journalEntry.AccountingCompany = new AccountingCompanyDTO() { AccountingCompanyId = accountingCompanyId }; //-1
                journalEntry.AccountingMovementType = new AccountingMovementTypeDTO();
                journalEntry.ModuleDateId = moduleDateId;
                journalEntry.Branch = new BranchDTO() { Id = paymentRequestParameters[0].BranchCode };
                journalEntry.SalePoint = new GLDTO.SalePointDTO() { Id = salePointId }; //0
                journalEntry.EntryNumber = 0;
                journalEntry.TechnicalTransaction = transactionNumber; //pendiente definición de donde proviene este dato para solicitudes de pago.
                journalEntry.Description = description;
                journalEntry.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                journalEntry.RegisterDate = DateTime.Now;
                journalEntry.Status = 1; //activo
                journalEntry.UserId = userId;
                journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

                #endregion DailyEntryHeader

                #region DailyEntryItem

                foreach (SCRDTO.PaymentRequestAccountingParameterDTO parameter in paymentRequestParameters)
                {
                    // Cálculo de la cuenta contable y la naturaleza
                    // Armo la estructura de parámetros para su evaluación.
                    List<AccountingRuleModels.ParameterDTO> parameters = new List<AccountingRuleModels.ParameterDTO>();

                    parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(parameter.AccountingConceptId, CultureInfo.InvariantCulture) }); //concept_id
                    parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(parameter.CurrencyCode, CultureInfo.InvariantCulture) }); //currency_id
                    parameters.Add(new AccountingRuleModels.ParameterDTO() { Value = Convert.ToString(parameter.Amount, CultureInfo.InvariantCulture) }); //amount

                    parametersCollection.Add(parameters);

                    JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO();

                    journalEntryItem.AccountingAccount = new AccountingAccountDTO();
                    journalEntryItem.Amount = new GLDTO.AmountDTO()
                    {
                        Currency = new CurrencyDTO() { Id = parameter.CurrencyCode },
                    };
                    decimal exchangeRate = parameter.ExchangeRate;

                    journalEntryItem.ExchangeRate = new GLDTO.ExchangeRateDTO() { SellAmount = exchangeRate };
                    journalEntryItem.Analysis = new List<AnalysisDTO>();
                    journalEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();
                    journalEntryItem.CostCenters = new List<GLDTO.CostCenterDTO>();
                    journalEntryItem.Currency = new CurrencyDTO() { Id = parameter.CurrencyCode };
                    journalEntryItem.Description = description;
                    journalEntryItem.EntryType = new EntryTypeDTO();
                    journalEntryItem.Id = 0;
                    journalEntryItem.Individual = new GLDTO.IndividualDTO() { IndividualId = Convert.ToInt32(parameter.PayerId) };
                    journalEntryItem.PostDated = new List<PostDatedDTO>();
                    journalEntryItem.Receipt = new ReceiptDTO();

                    journalEntry.JournalEntryItems.Add(journalEntryItem);
                }

                #endregion DailyEntryItem

                #region ValidateAndSave

                int entryNumber = DelegateService.glAccountingApplicationService.Accounting(moduleDateId, parametersCollection, journalEntry);

                if (entryNumber > 0)
                {
                    message = Global.IntegrationSuccessMessage + " " + entryNumber;

                    // Actualizo parámetro de número de trasacción.
                    UpdateTransactionNumber(transactionNumber);

                    PaymentRequestDTO paymentRequest = new PaymentRequestDTO()
                    {
                        Id = paymentRequestId,
                        Transaction = new TransactionDTO()
                        {
                            TechnicalTransaction = transactionNumber
                        }
                    };

                    DelegateService.accountingAccountsPayableService.UpdatePaymentRequest(paymentRequest);
                }
                if (entryNumber == 0)
                {
                    message = Global.AccountingIntegrationUnbalanceEntry;
                }
                if (entryNumber == -2)
                {
                    message = Global.EntryRecordingError;
                }

                #endregion ValidateAndSave
            }
            catch (Exception)
            {
                message = Global.EntryRecordingError;
            }

            return message;
        }

        /// <summary>
        /// UpdateTransactionNumber
        /// </summary>
        /// <param name="number"></param>
        private void UpdateTransactionNumber(int number)
        {
            Parameter parameter = new Parameter();
            parameter.Id = Convert.ToInt32(ConfigurationManager.AppSettings["TransactionNumber"]); //id de parametro de número de transacción
            parameter.NumberParameter = number;
            parameter.Description = "NÚMERO DE TRANSACCIÓN";
            parameter.NumberParameter = parameter.NumberParameter + 1;
            DelegateService.commonService.UpdateParameter(parameter);
        }

        #endregion

    }
}