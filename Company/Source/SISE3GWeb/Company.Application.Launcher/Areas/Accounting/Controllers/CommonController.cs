//System
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.CommonService.Models;
//using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
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
using System.Threading;
using System.Web.Mvc;
using AccountingConceptModel = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using AccountingPaymentModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using AutomaticDebitModels = Sistran.Core.Application.AutomaticDebitServices.Models;

using Debit = Sistran.Core.Application.AutomaticDebitServices.Models;
// Sistran Core
//using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayable;
using DTOs = Sistran.Core.Application.AccountingServices.DTOs;
using GeneralLedgerModel = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using reinsDTOs = Sistran.Core.Application.ReinsuranceServices.DTOs;
//Sistran Company
using tempCommonDTO = Sistran.Core.Application.TempCommonServices.DTOs;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    public class CommonController : Controller
    {
        private static List<UPV1.CoInsuranceCompany> coInsuranceCompanies = new List<UPV1.CoInsuranceCompany>();

        #region Class

        /// <summary>
        /// SearchBy
        /// </summary>
        public class SearchBy
        {
            public int Id { get; set; }
            public String Description { get; set; }
            public String SmallDescription { get; set; }
        }

        /// <summary>
        /// NameGeneral
        /// </summary>
        public class NameGeneral
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        /// <summary>
        /// TransactionType
        /// </summary>
        public class TransactionType
        {
            public int Id { get; set; }
            public String Description { get; set; }
        }


        /// <summary>
        /// DebitCredit
        /// </summary>
        public class DebitCredit
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// Generic
        /// </summary>
        public class Generic
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// BankReconciliation
        /// </summary>
        public class BankReconciliation
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// AnalysisCode
        /// </summary>
        public class AnalysisCode
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// AnalysisConcept
        /// </summary>
        public class AnalysisConcept
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// AnalysisKey
        /// </summary>
        public class AnalysisKey
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// CostCenter
        /// </summary>
        public class CostCenter
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public string Porcentage { get; set; }
        }

        /// <summary>
        /// ValueNumber
        /// </summary>
        public class ValueNumber
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        #endregion

        #region Instance Variables


        #endregion

        #region Actions

        /// <summary>
        /// GetAllBanks
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAllBanks()
        {
            List<Bank> banks = DelegateService.commonService.GetBanks();

            return new UifSelectResult(banks);
        }

        /// <summary>
        /// GetBanks
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBanks()
        {
            List<object> bankResponses = new List<object>();

            List<Bank> banks = DelegateService.commonService.GetBanks();

            foreach (Bank bank in banks)
            {
                List<BankAccountCompanyDTO> bankAccountCompanies;
                bankAccountCompanies = GetCompanyBankAccountsByBankId(bank.Id);
                foreach (BankAccountCompanyDTO bankAccountCompany in bankAccountCompanies)
                {
                    bankResponses.Add(new
                    {
                        Id = bankAccountCompany.Id,
                        Description = bank.Description + " - " + bankAccountCompany.BankAccountType.Description + " - " + bankAccountCompany.Number
                    });
                }
            }

            return new UifSelectResult(bankResponses);
        }

        /// <summary>
        /// GetCompanyBankAccountsByBankId
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>List<BankAccountCompany></returns>
        public List<BankAccountCompanyDTO> GetCompanyBankAccountsByBankId(int bankId)
        {
            List<BankAccountCompanyDTO> bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();

            return bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId) && r.IsEnabled.Equals(true))).ToList();
        }

        /// <summary>
        /// GetBanksByName
        /// </summary>
        /// <param name="query"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetBanksByName(string query)
        {
            List<object> bankResponses = new List<object>();
            List<Bank> banks = DelegateService.commonService.GetBanks();

            foreach (Bank bank in banks)
            {
                if (((bank.Description).IndexOf(query.ToUpper())) > -1)
                {
                    bankResponses.Add(new
                    {
                        id = bank.Id,
                        description = bank.Description
                    });
                }
            }

            return Json(bankResponses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDate
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDate()
        {
            string dateToday;
            dateToday = DateTime.Today.ToString("dd/MM/yyyy");
            return Json(dateToday, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetNets
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetNets()
        {
            List<object> networks = new List<object>();

            List<Debit.BankNetwork> bankNetworks = DelegateService.automaticDebitService.GetBankNetworks();

            foreach (Debit.BankNetwork bankNetwork in bankNetworks)
            {
                networks.Add(new
                {
                    Id = bankNetwork.Id,
                    Description = bankNetwork.Description
                });
            }

            return new UifSelectResult(networks);
        }

        /// <summary>
        /// GetBranchs
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBranchs()
        {
            var branchs = (from result in GetBranchesByUserId(GetUserIdByName(User.Identity.Name.ToUpper()))
                           select new
                           {
                               result.Id,
                               result.Description
                           }).OrderBy(x => x.Description).ToList();

            return new UifSelectResult(branchs);
        }

        /// <summary>
        /// GetBranchDescriptionById
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>string</returns>
        public string GetBranchDescriptionById(int branchId)
        {
            var branchs = GetBranchesByUserId(GetUserIdByName(User.Identity.Name.ToUpper())).Where(sl => sl.Id == branchId).ToList();

            return branchs[0].Description;
        }

        /// <summary>
        /// GetBranchDescriptionByIdUserId
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public string GetBranchDescriptionByIdUserId(int branchId, int userId)
        {
            var branchs = GetBranchesByUserId(userId).Where(sl => sl.Id == branchId).ToList();

            return branchs[0].Description;
        }

        /// <summary>
        /// GetBranchesByUserName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<Branch/></returns>
        public List<Branch> GetBranchesByUserName(string name)
        {
            return DelegateService.uniqueUserService.GetBranchesByUserId(GetUserIdByName(name));
        }

        /// <summary>
        /// GetBranchDescriptionByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public string GetBranchDescriptionByBranchId(int branchId)
        {
            var branchs = GetBranchesByUserId(Convert.ToInt32(ConfigurationManager.AppSettings["UserPaymentService"])).Where(sl => sl.Id == branchId).ToList();

            return branchs[0].Description;
        }

        /// <summary>
        /// GetPrefixes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPrefixes()
        {
            List<object> prefixesResponses = new List<object>();

            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes().OrderBy(x => x.Description).ToList();

            foreach (Prefix prefix in prefixes)
            {
                prefixesResponses.Add(new
                {
                    Id = prefix.Id,
                    Description = prefix.Description
                });
            }

            return new UifSelectResult(prefixesResponses);
        }

        /// <summary>
        /// GetPrefixDescriptionById
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>string</returns>
        public string GetPrefixDescriptionById(int prefixId)
        {
            var prefixes = DelegateService.commonService.GetPrefixes();
            var prefix = prefixes.Where(sl => sl.Id == prefixId).ToList();

            return prefix[0].Description;
        }

        /// <summary>
        /// GetLotNumbersByNetId
        /// </summary>
        /// <param name="netId"></param>
        /// <param name="sendDate"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetLotNumbersByNetId(int netId, string sendDate)
        {
            List<object> automaticDebitsResponse = new List<object>();
            AutomaticDebitModels.AutomaticDebit newAutomaticDebit = new AutomaticDebitModels.AutomaticDebit()
            {
                BankNetwork = new AutomaticDebitModels.BankNetwork() { Id = netId, RetriesNumber = 1 },
                Date = Convert.ToDateTime(sendDate),
                Id = 1
            };

            List<AutomaticDebitModels.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(newAutomaticDebit);

            if (automaticDebits.Count > 0)
            {
                foreach (AutomaticDebitModels.AutomaticDebit automaticDebit in automaticDebits)
                {
                    automaticDebitsResponse.Add(new
                    {
                        Id = automaticDebit.Id,
                        Description = automaticDebit.Description
                    });
                }
            }

            return new UifSelectResult(automaticDebitsResponse);
        }

        /// <summary>
        /// GetLotNumbersByBankNetworkId
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="sendDate"></param>
        /// <param name="statusCode"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetLotNumbersByBankNetworkId(int bankNetworkId, string sendDate, int statusCode)
        {
            List<object> automaticDebitsResponse = new List<object>();
            Debit.AutomaticDebit newAutomaticDebit = new Debit.AutomaticDebit()
            {
                BankNetwork = new Debit.BankNetwork() { Id = bankNetworkId, RetriesNumber = 1 },
                Date = Convert.ToDateTime(sendDate),
                Id = 1
            };

            List<Debit.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(newAutomaticDebit);

            if (automaticDebits.Count > 0)
            {
                foreach (Debit.AutomaticDebit automaticDebit in automaticDebits)
                {
                    if (automaticDebit.AutomaticDebitStatus.Id == statusCode)
                    {
                        automaticDebitsResponse.Add(new
                        {
                            Id = automaticDebit.Id,
                            Description = automaticDebit.Description
                        });
                    }
                }
            }

            return new UifSelectResult(automaticDebitsResponse);
        }

        /// <summary>
        /// GenerateLotNumber
        /// </summary>
        /// <param name="netId"></param>
        /// <param name="sendDateTime"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GenerateLotNumber(int netId, string sendDateTime)
        {
            string lotNumber = netId.ToString() + Convert.ToDateTime(sendDateTime).ToString("yyyyMMddHHmm");

            return Json(lotNumber, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetTaxCategory
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTaxCategory()
        {
            List<object> taxCategories = new List<object>();

            taxCategories.Add(new
            {
                Id = 1,
                Description = "Importe"
            });
            taxCategories.Add(new
            {
                Id = 100,
                Description = "Porcentaje"
            });
            taxCategories.Add(new
            {
                Id = 1000,
                Description = "Por Milaje"
            });

            return new UifSelectResult(taxCategories);
        }

        /// <summary>
        /// GetPaymentMethods
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentMethods()
        {
            List<object> paymentMethodDebits = new List<object>();

            List<PaymentMethod> paymentMethods = DelegateService.commonService.GetPaymentMethods();
            List<PaymentMethod> debitPaymentMethods = paymentMethods.Where(sl => sl.Id != Convert.ToInt32(ConfigurationManager.AppSettings["PaymentMethodCash"])).ToList();

            foreach (PaymentMethod paymentMethod in debitPaymentMethods)
            {
                paymentMethodDebits.Add(new
                {
                    Id = paymentMethod.Id,
                    Description = paymentMethod.Description
                });
            }

            return new UifSelectResult(paymentMethodDebits);
        }

        /// <summary>
        /// GetPaymentMethodTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentMethodTypes()
        {
            List<object> paymentMethodsResponses = new List<object>();

            //EN EE DEBE SACAR DE PARAM.PAYMENT_METHOD_TYPE PARA LLENAR EL COMBO
            List<AccountingPaymentModels.PaymentMethodDTO> paymentMethodTypes = DelegateService.accountingParameterService.GetPaymentMethods();

            foreach (AccountingPaymentModels.PaymentMethodDTO paymentMethodType in paymentMethodTypes)
            {
                if (paymentMethodType.Id == Convert.ToUInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                    paymentMethodType.Id == Convert.ToUInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"]))
                {
                    paymentMethodsResponses.Add(new
                    {
                        Id = paymentMethodType.Id,
                        Description = paymentMethodType.Description
                    });
                }
            }

            return new UifSelectResult(paymentMethodsResponses);
        }

        /// <summary>
        /// GetTransferAccountTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTransferAccountTypes()
        {
            List<object> bankAccountTypesResponse = new List<object>();

            List<BankAccountTypeDTO> bankAccountTypes = DelegateService.accountingParameterService.GetBankAccountTypes();

            foreach (BankAccountTypeDTO bankAccountType in bankAccountTypes)
            {
                bankAccountTypesResponse.Add(new
                {
                    Id = bankAccountType.Id,
                    Description = bankAccountType.Description
                });
            }

            return new UifSelectResult(bankAccountTypesResponse);
          
        }

        /// <summary>
        /// GetDebitPendingProcess
        /// Trae el estatus de registros procesados de los procesos pendientes de débitos 
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <param name="processTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDebitPendingProcess(int bankNetworkId, int processTypeId)
        {
            List<object> automaticDebitResponse = new List<object>();

            double recordsNumber = 0;
            double recordProcessed = 0;
            double recordFailed = 0;
            double progress = 0;
            try
            {
                int userId = 0;

                if (User != null)
                {
                    userId = GetUserIdByName(User.Identity.Name.ToUpper());
                }
                else
                {
                    userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
                }


                Debit.AutomaticDebit automaticDebitModel = new Debit.AutomaticDebit()
                {
                    BankNetwork = new Debit.BankNetwork()
                    {
                        Id = bankNetworkId,
                        Description = "0",
                        RetriesNumber = 5
                    },
                    Id = processTypeId,
                    UserId = userId
                };

                List<Debit.AutomaticDebit> automaticDebits = DelegateService.automaticDebitService.GetAutomaticDebits(automaticDebitModel);
                List<Debit.AutomaticDebit> automaticDebitsOrder = (from cupon in automaticDebits orderby cupon.Id descending select cupon).ToList();

                if (automaticDebits.Count > 0)
                {
                    foreach (Debit.AutomaticDebit automaticDebit in automaticDebitsOrder)
                    {
                        // RecordsProcessed, RecordsFailed, RecordsNumber
                        recordsNumber = Convert.ToDouble(automaticDebit.Coupons[0].CouponStatus.RetriesNumber);
                        recordProcessed = Convert.ToDouble(automaticDebit.Coupons[0].CouponStatus.Description);
                        recordFailed = Convert.ToDouble(automaticDebit.Coupons[0].CouponStatus.SmallDescription);

                        double elapsed = 0;

                        if (recordsNumber > 0)
                        {
                            progress = ((recordProcessed + recordFailed) / recordsNumber);
                        }
                        else
                        {
                            progress = 1;
                        }

                        if (Convert.ToDateTime(automaticDebit.Coupons[0].Policy.CurrentTo) == Convert.ToDateTime("01/01/0001 0:00:00"))
                        {
                            elapsed = System.Math.Round((DateTime.Now.TimeOfDay.TotalHours -
                                                  automaticDebit.Coupons[0].Policy.CurrentFrom.TimeOfDay.TotalHours), 2);
                        }
                        else
                        {
                            elapsed = System.Math.Round((automaticDebit.Coupons[0].Policy.CurrentTo.TimeOfDay.TotalHours -
                                                  automaticDebit.Coupons[0].Policy.CurrentFrom.TimeOfDay.TotalHours), 2);
                        }

                        double minimumElapsed = elapsed - System.Math.Truncate(elapsed);
                        minimumElapsed = minimumElapsed * 60;

                        automaticDebitResponse.Add(new
                        {
                            Id = automaticDebit.Id,
                            Description = automaticDebit.Description,
                            BankNetworkId = automaticDebit.BankNetwork.Id,
                            LotNumber = automaticDebit.BankNetwork.Description,
                            RecordsNumber = automaticDebit.Coupons[0].CouponStatus.RetriesNumber,
                            RecordsProcessed = automaticDebit.Coupons[0].CouponStatus.Description,
                            RecordsFailed = automaticDebit.Coupons[0].CouponStatus.SmallDescription,
                            Progress = progress.ToString("P", CultureInfo.InvariantCulture),
                            Elapsed = System.Math.Truncate(elapsed) + " h " + System.Math.Truncate(minimumElapsed) + " m",
                            processTypeId = automaticDebit.Coupons[0].CouponStatus.Id,
                            statusCode = automaticDebit.Coupons[0].Policy.Id
                        });
                    }
                }
                return new UifTableResult(automaticDebitResponse);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<object>());
            }
        }

        /// <summary>
        /// GetIncomeConcepts
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetIncomeConcepts()
        {
            List<object> concepts = new List<object>();
            List<CollectConceptDTO> collectConcepts;
            collectConcepts = DelegateService.accountingParameterService.GetCollectConcepts();

            if (collectConcepts.Count > 0)
            {
                foreach (CollectConceptDTO collectConcept in collectConcepts)
                {
                    concepts.Add(new
                    {
                        Id = collectConcept.Id,
                        Description = collectConcept.Description
                    });
                }
            }

            return new UifSelectResult(concepts);
        }

        /// <summary>
        /// GetCurrencies
        /// Obtiene las descripciones de las monedas
        /// </summary>
        /// <returns>List<Currency/></returns>
        public ActionResult GetCurrencies()
        {
            return new UifSelectResult(DelegateService.commonService.GetCurrencies());
        }
        /// <summary>
        /// GetAccountTypes
        /// Obtiene los tipos de cuentas
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountTypes()
        {
            List<BankAccountTypeDTO> bankAccountTypes = DelegateService.accountingParameterService.GetBankAccountTypes();
            var bankAccountTypesResponse = (from bankAccountType in bankAccountTypes
                                            select new
                                            {
                                                AccountTypeId = bankAccountType.Id,
                                                AccountTypeDescription = bankAccountType.Description
                                            });

            return new UifSelectResult(bankAccountTypesResponse);
        }

        /// <summary>
        /// GetCreditCardTypes
        /// Obtiene los tipos de tarjeta de crédito
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCreditCardTypes()
        {
            return new UifSelectResult(DelegateService.accountingParameterService.GetCreditCardTypes());
        }

        /// <summary>
        /// GetValidMonths
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidMonths()
        {
            List<object> months = new List<object>();

            for (int i = 1; i <= 12; i++)
            {
                months.Add(new
                {
                    Id = i,
                    Description = i
                });
            }

            return new UifSelectResult(months);
        }

        /// <summary>
        /// GetAccountBanks
        /// Obtiene el Id/Descripción de los bancos registrados a la Compañía
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountBanks()
        {
            return new UifSelectResult(GetDistinctBanks());
        }

        /// <summary> 
        /// SearchAccountBank
        /// Obtiene las cuentas bancarias para autocomplete en transferencias
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchAccountBank(string query, string param)
        {

            int searchType = Convert.ToInt16(param);

            List<object> bankAccounts = new List<object>();
            List<BankAccountCompanyDTO> companyBankAccounts = new List<BankAccountCompanyDTO>();

            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
            // Búsqueda por número de cuenta
            if (searchType == 1)
            {
                companyBankAccounts = bankAccountCompanies.Where(r => (r.Number.Contains(query))).ToList();
            }
            // Búsqueda por nombre banco
            if (searchType == 2)
            {
                companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Description.Contains(query.ToUpper()))).ToList();
            }

            foreach (BankAccountCompanyDTO bankAccountCompany in companyBankAccounts)
            {
                bankAccounts.Add(new
                {
                    AccountBankId = bankAccountCompany.Id,
                    BankId = bankAccountCompany.Bank.Id,
                    BankDescription = bankAccountCompany.Bank.Description,
                    AccountNumber = bankAccountCompany.Number,
                    AccountTypeId = bankAccountCompany.BankAccountType.Id,
                    AccountTypeDescription = bankAccountCompany.BankAccountType.Description,
                    CurrencyId = bankAccountCompany.Currency.Id,
                    CurrencyDescription = bankAccountCompany.Currency.Description,
                    LongDescription = bankAccountCompany.Bank.Description + " - " + bankAccountCompany.BankAccountType.Description + " - " + bankAccountCompany.Number
                });
            }

            return Json(bankAccounts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingDateByModuleId
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <returns>DateTime</returns>
        public DateTime GetAccountingDateByModuleId(int moduleDateId)
        {
            return DelegateService.commonService.GetModuleDateIssue(moduleDateId, DateTime.Now);
        }

        /// <summary>
        /// GetUserNick
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetUserNick()
        {
            string userNick;
            List<User> users = GetUserByName(User.Identity.Name);
            userNick = users[0].AccountName;

            if (userNick == null)
            {
                userNick = ""; //aumentado para que botón de pagos sin autenticación funcione en firefox
            }

            return Json(userNick, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingDate
        /// Devuelve la fecha contable
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingDate()
        {
            string accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now).ToString("dd/MM/yyyy");

            return Json(accountingDate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetListBranchesbyUserName
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetListBranchesbyUserName()
        {
            try
            {
                List<Branch> branchs = GetBranchesByUserName(User.Identity.Name);

                return new UifSelectResult(from branch in branchs orderby branch.Description select branch);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetCurrencyDescriptionByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCurrencyDescriptionByCurrencyId(int currencyId)
        {
            string descriptionCurrency = GetCurrencyDescriptionById(currencyId);

            return Json(descriptionCurrency, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCurrencyDescriptionById
        /// Obtiene la descripción de la moneda dado su id
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>string</returns>
        public string GetCurrencyDescriptionById(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currencyNames = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currencyNames[0].Description;
        }

        #endregion

        #region Agent

        /// <summary>
        /// GetAgentByDocumentNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAgentByDocumentNumber(string query)
        {
            List<object> agents = new List<object>();
            var agentQuerys = DelegateService.tempCommonService.GetAgentByDocumentNumber(query);

            int agentAgencyId = 0;

            if (agentQuerys.Count == 0)
            {
                agents.Add(new
                {
                    Id = 0,
                    IndividualId = 0,
                    AgentType = "",
                    AgentTypeId = -1,
                    AgentId = -1,
                    DocumentNumber = @Global.RegisterNotFound,
                    DocumentTypeId = -1,
                    Name = "",
                    AgentAgencyId = -1,
                    BranchId = -1,
                    DocumentNumberName = @Global.RegisterNotFound
                });
            }
            else
            {
                foreach (tempCommonDTO.AgentDTO agent in agentQuerys)
                {
                    int agentId = agent.IndividualId;
                    agentAgencyId = agent.AgentAgencyId;

                    agents.Add(new
                    {
                        Id = agent.IndividualId,
                        IndividualId = agent.IndividualId,
                        AgentType = agent.AgentType,
                        AgentTypeId = agent.AgentTypeId,
                        AgentId = agentId,
                        DocumentNumber = agent.DocumentNumber,
                        DocumentTypeId = -1,
                        Name = agent.Name,
                        AgentAgencyId = agentAgencyId,
                        BranchId = agent.BranchId,
                        DocumentNumberName = agent.DocumentNumber + " : " + agent.Name
                    });
                }
            }

            return Json(agents, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAgentByName
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAgentByName(string query)
        {
            List<object> agents = new List<object>();
            var agentQuerys = DelegateService.tempCommonService.GetAgentByName(query);

            if (agentQuerys.Count == 0)
            {
                agents.Add(new
                {
                    Id = 0,
                    IndividualId = 0,
                    AgentType = "",
                    AgentTypeId = -1,
                    AgentId = -1,
                    DocumentNumber = "",
                    DocumentTypeId = -1,
                    Name = @Global.RegisterNotFound,
                    AgentAgencyId = -1,
                    BranchId = -1,
                    DocumentNumberName = @Global.RegisterNotFound
                });
            }
            else
            {
                foreach (tempCommonDTO.AgentDTO agent in agentQuerys)
                {
                    agents.Add(new
                    {
                        Id = agent.IndividualId,
                        IndividualId = agent.IndividualId,
                        AgentType = agent.AgentType,
                        AgentTypeId = agent.AgentTypeId,
                        AgentId = agent.IndividualId,
                        DocumentNumber = agent.DocumentNumber,
                        DocumentTypeId = -1,
                        Name = agent.Name,
                        AgentAgencyId = agent.AgentAgencyId,
                        BranchId = agent.BranchId,
                        DocumentNumberName = agent.DocumentNumber + " : " + agent.Name
                    });
                }
            }

            return Json(agents, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAgentTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAgentTypes()
        {
            try
            {
                var agentTypes = DelegateService.uniquePersonServiceV1.GetAgentTypes();

                return new UifSelectResult(from agentType in agentTypes orderby agentType.Description select agentType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Insured

        /// <summary>
        /// GetInsuredByDocumentNumber
        /// Obtiene listado de personas aseguradas (AUTOCOMPLETE)
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsuredByDocumentNumber(string query)
        {
            try
            {
                List<object> persons = new List<object>();
                var personQuerys = DelegateService.tempCommonService.GetInsuredByDocumentNumber(query);

                if (personQuerys.Count > 0)
                {
                    foreach (tempCommonDTO.IndividualDTO insured in personQuerys)
                    {
                        persons.Add(new
                        {
                            Name = insured.Name.Trim(),
                            Id = insured.IndividualId,
                            DocumentNumber = insured.DocumentNumber,
                            InsuredCode = insured.IndividualId,
                            DocumentTypeId = insured.DocumentTypeId,
                            DocumentNumberName = insured.DocumentNumber + " - " + insured.Name.Trim()
                        });
                    }
                }
                else
                {
                    persons.Add(new
                    {
                        Name = @Global.RegisterNotFound,
                        Id = -1,
                        DocumentNumber = @Global.RegisterNotFound,
                        InsuredCode = -1,
                        DocumentTypeId = -1,
                        DocumentNumberName = @Global.RegisterNotFound
                    });
                }

                return Json(persons, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        

        /// <summary>
        /// GetInsuredByInsuredName
        /// Obtiene listado de personas aseguradas
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsuredByInsuredName(string query)
        {
            try
            {
                List<object> persons = new List<object>();

                var personQuerys = DelegateService.tempCommonService.GetInsuredByName(query);

                if (personQuerys.Count > 0)
                {
                    foreach (tempCommonDTO.IndividualDTO insured in personQuerys)
                    {
                        persons.Add(new
                        {
                            Name = insured.Name.Trim(),
                            Id = insured.IndividualId,
                            DocumentNumber = insured.DocumentNumber,
                            DocumentTypeId = insured.DocumentTypeId,
                            InsuredCode = insured.IndividualId
                        });
                    }
                }
                else
                {
                    persons.Add(new
                    {
                        Name = @Global.RegisterNotFound,
                        Id = -1,
                        DocumentNumber = @Global.RegisterNotFound,
                        DocumentTypeId = -1,
                        InsuredCode = -1
                    });
                }

                return Json(persons, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetInsuredByInsuredName
        /// Obtiene listado de personas aseguradas
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsurersByInsuredName(string query)
        {
            try
            {
                List<object> persons = new List<object>();

                var personQuerys = DelegateService.uniquePersonServiceV1.GetInsuredsByName(query);

                if (personQuerys.Count > 0)
                {
                    foreach (var insured in personQuerys)
                    {
                        persons.Add(new
                        {
                            Name = insured.FullName.Trim(),
                            Id = insured.IndividualId,
                            DocumentNumber = insured.DocumentNumber,
                            InsuredCode = insured.IndividualId
                        });
                    }
                }
                else
                {
                    persons.Add(new
                    {
                        Name = @Global.RegisterNotFound,
                        Id = -1,
                        DocumentNumber = @Global.RegisterNotFound,
                        InsuredCode = -1
                    });
                }

                return Json(persons, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Employee

        /// <summary>
        /// GetEmployeeByDocumentNumber
        /// Obtiene listado de empleados
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEmployeeByDocumentNumber(string query)
        {
            try
            {
                List<object> employees = new List<object>();
                var persons = DelegateService.uniquePersonServiceV1.GetEmployeePersons().Where(em => em.IdCardNo.Contains(query)).ToList();

                if (persons.Count > 0)
                {
                    foreach (var person in persons)
                    {
                        employees.Add(new
                        {
                            Name = person.Name.Trim(),
                            Id = person.Id,
                            DocumentNumber = person.IdCardNo,
                            InsuredCode = person.Id
                        });
                    }
                }
                else
                {
                    employees.Add(new
                    {
                        Name = @Global.RegisterNotFound,
                        Id = -1,
                        DocumentNumber = @Global.RegisterNotFound,
                        InsuredCode = -1
                    });
                }

                return Json(employees, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetEmployeeByName
        /// Obtiene listado de empleados
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEmployeeByName(string query)
        {
            try
            {
                List<object> employees = new List<object>();
                var persons = DelegateService.uniquePersonServiceV1.GetEmployeePersons().Where(em => em.Name.Contains(query)).ToList();

                if (persons.Count > 0)
                {
                    foreach (var person in persons)
                    {
                        employees.Add(new
                        {
                            Name = person.Name.Trim(),
                            Id = person.Id,
                            DocumentNumber = person.IdCardNo,
                            InsuredCode = person.Id
                        });
                    }
                }
                else
                {
                    employees.Add(new
                    {
                        Name = @Global.RegisterNotFound,
                        Id = -1,
                        DocumentNumber = @Global.RegisterNotFound,
                        InsuredCode = -1
                    });
                }

                return Json(employees, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Person

        /// <summary>       
        /// LoadPersonType
        /// Obtiene el tipo de beneficiario
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult LoadPersonType()
        {
            var personTypes = DelegateService.tempCommonService.GetPersonTypeByPaymentOrderEnable();

            return new UifSelectResult(from personType in personTypes orderby personType.Description select personType);
        }

        /// <summary>
        /// GetPersonTypes
        /// </summary>
        /// <returns>UifSelectResult</returns>
        public JsonResult GetPersonTypes()
        {
            var personTypes = DelegateService.tempCommonService.GetPersonTypeByPaymentOrderEnable();
            return new UifSelectResult(from personType in personTypes orderby personType.Description select personType);
        }

        /// <summary>
        /// GetPersonTypes
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetPersonTypesByBillEnabled()
        {
            var personTypes = DelegateService.tempCommonService.GetPersonTypesByBillEnabled().OrderBy(pt => pt.Description).ToList();

            return new UifSelectResult(personTypes);
        }

        /// <summary>
        /// GetPersonTypeByPaymentOrderEnable
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPersonTypeByPaymentOrderEnable()
        {
            var personTypes = (from personType in DelegateService.tempCommonService.GetPersonTypeByPaymentOrderEnable()
                               orderby personType.Description
                               select new
                               {
                                   Id = personType.Id,
                                   Description = personType.Description
                               });

            return new UifSelectResult(personTypes);
        }
        public ActionResult GetPersonTypeByPaymentOrderEnableFilter()
        {
            var personTypes = (from personType in DelegateService.tempCommonService.GetPersonTypeByPaymentOrderEnableFilter()
                               orderby personType.Description
                               select new
                               {
                                   Id = personType.Id,
                                   Description = personType.Description
                               });

            return new UifSelectResult(personTypes);
        }

        #endregion

        #region Application

        /// <summary>
        /// GetSalesPointByBranchId
        /// Obtiene los puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetSalesPointByBranchId(int branchId)
        {
            var salesPoints = DelegateService.commonService.GetSalePointsByBranchId(branchId);

            var salePointsResponse = (from salePoint in salesPoints
                                      orderby salePoint.Description
                                      select new
                                      {
                                          Id = salePoint.Id,
                                          Description = salePoint.Description
                                      });

            return new UifSelectResult(salePointsResponse);
        }

        /// <summary>
        /// GetAccountingCompanies
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingCompanies()
        {
            User user = new User()
            {
                AccountName = User.Identity.Name,
                UserId = GetUserIdByName(User.Identity.Name.ToUpper()),
            };

            var companyQuerys = DelegateService.accountingApplicationService.GetCompaniesByUserId(user.UserId);

            var companies = (from company in companyQuerys
                             select new
                             {
                                 Id = company.IndividualId,
                                 Description = company.Name
                             });

            return new UifSelectResult(companies);
        }

        /// <summary>
        /// GetNatures
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetNatures()
        {
            List<object> natures = new List<object>();

            natures.Add(new { Id = 1, Description = @Global.Credit });
            natures.Add(new { Id = 2, Description = @Global.Debit });

            return new UifSelectResult(natures);
        }

        /// <summary>
        /// GetTechnicalPrefixes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTechnicalPrefixes()
        {
            var prefixes = DelegateService.commonService.GetLinesBusiness().OrderBy(pr => pr.Description).ToList();

            var technicalPrefixes = (from prefix in prefixes
                                     select new
                                     {
                                         Id = prefix.Id,
                                         Description = prefix.Description
                                     });
            return new UifSelectResult(technicalPrefixes);
        }

        /// <summary>
        /// GetTechnicalSubPrefixesByPrefixId
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTechnicalSubPrefixesByPrefixId(int prefixId)
        {
            var subprefixes = DelegateService.commonService.GetSubLinesBusinessByLineBusinessId(prefixId);

            var technicalSubPrefixes = (from subprefix in subprefixes
                                        select new
                                        {
                                            Id = subprefix.Id,
                                            Description = subprefix.Description
                                        });

            return new UifSelectResult(technicalSubPrefixes);
        }

        /// <summary>
        /// GetReinsurerByDocumentNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReinsurerByDocumentNumber(string query)
        {
            List<object> reinsurers = new List<object>();
            var companies = DelegateService.tempCommonService.GetReinsurerByDocumentNumber(query, Convert.ToInt32(ConfigurationManager.AppSettings["ReinsuranceAutocomplete"]));

            if (companies.Count == 0)
            {
                reinsurers.Add(new
                {
                    Id = -1,
                    ReinsurerIndividualId = @Global.RegisterNotFound,//-1,
                    ReinsurerName = @Global.RegisterNotFound,
                    ReinsurerDocumentNumber = @Global.RegisterNotFound,//-1,
                    Name = @Global.RegisterNotFound,
                    DocumentNumber = @Global.RegisterNotFound,//-1
                    DocumentTypeId = -1,
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO company in companies)
                {
                    reinsurers.Add(new
                    {
                        Id = company.IndividualId,
                        ReinsurerIndividualId = company.IndividualId,
                        ReinsurerName = company.Name,
                        ReinsurerDocumentNumber = company.DocumentNumber,
                        Name = company.Name,
                        DocumentNumber = company.DocumentNumber,
                        DocumentTypeId = company.DocumentTypeId,
                    });
                }
            }

            return Json(reinsurers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetReinsurerByName
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReinsurerByName(string query)
        {
            List<object> reinsurers = new List<object>();
            List<tempCommonDTO.IndividualDTO> companies = DelegateService.tempCommonService.GetReinsurerByName(query, Convert.ToInt32(ConfigurationManager.AppSettings["Reinsurance"]), Convert.ToInt32(ConfigurationManager.AppSettings["ForeignReinsurance"]));

            if (companies.Count == 0)
            {
                reinsurers.Add(new
                {
                    Id = -1,
                    ReinsurerIndividualId = -1,
                    ReinsurerName = @Global.RegisterNotFound,
                    ReinsurerDocumentNumber = -1,
                    Name = @Global.RegisterNotFound,
                    DocumentNumber = -1,
                    DocumentTypeId = -1,
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO company in companies)
                {
                    reinsurers.Add(new
                    {
                        Id = company.IndividualId,
                        ReinsurerIndividualId = company.IndividualId,
                        ReinsurerName = company.Name,
                        ReinsurerDocumentNumber = company.DocumentNumber,
                        Name = company.Name,
                        DocumentNumber = company.DocumentNumber,
                        DocumentTypeId = company.DocumentTypeId,
                    });
                }
            }

            return Json(reinsurers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetContractNumberByNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetContractNumberByNumber(string query, int contractTypeId)
        {
            List<object> contractNumbers = new List<object>();
            var contracts = DelegateService.reinsuranceService.GetContractsByYearAndContractTypeId(0, contractTypeId);
            int contractCount = contracts.Count;

            if (contractCount == 0)
            {
                contractNumbers.Add(new
                {
                    Id = -1,
                    Name = @Global.RegisterNotFound
                });
            }
            else
            {
                foreach (reinsDTOs.ContractDTO contract in contracts)
                {
                    contractNumbers.Add(new
                    {
                        Id = contract.ContractId,
                        Name = contract.Description
                    });
                }
            }

            return Json(contractNumbers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetContractStretchs
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractStretchs(int contractId)
        {
            var levels = DelegateService.reinsuranceService.GetLevelsByContractId(contractId);
            List<object> levelsResponse = new List<object>();
            foreach (reinsDTOs.LevelDTO itemsTypes in levels)
            {
                levelsResponse.Add(new
                {
                    Id = itemsTypes.Number,
                    Description = itemsTypes.Number
                });
            }
            return new UifSelectResult(levelsResponse);

        }

        /// <summary>
        /// GetBrokerByDocumentNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBrokerByDocumentNumber(string query)
        {
            List<object> brokers = new List<object>();
            var companies = DelegateService.tempCommonService.GetReinsurerByDocumentNumber(query, Convert.ToInt32(ConfigurationManager.AppSettings["BrokerAutocomplete"]));

            if (companies.Count == 0)
            {
                brokers.Add(new
                {
                    Id = -1,
                    BrokerIndividualId = -1,
                    BrokerName = @Global.RegisterNotFound,
                    BrokerDocumentNumber = -1,
                    BrokerDocumentTypeId = -1,
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO company in companies)
                {
                    brokers.Add(new
                    {
                        Id = company.IndividualId,
                        BrokerIndividualId = company.IndividualId,
                        BrokerName = company.Name,
                        BrokerDocumentNumber = company.DocumentNumber,//company.Nit
                        BrokerDocumentTypeId = company.DocumentTypeId,
                    });
                }
            }

            return Json(brokers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetBrokerByName
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBrokerByName(string query)
        {
            List<object> brokers = new List<object>();
            var companies = DelegateService.tempCommonService.GetReinsurerByName(query, Convert.ToInt32(ConfigurationManager.AppSettings["BrokerAutocomplete"]), 0);

            if (companies.Count == 0)
            {
                brokers.Add(new
                {
                    Id = -1,
                    BrokerIndividualId = -1,
                    BrokerName = @Global.RegisterNotFound,
                    BrokerDocumentNumber = -1,
                    BrokerDocumentTypeId = -1,
                });
            }

            else
            {
                foreach (tempCommonDTO.IndividualDTO company in companies)
                {
                    brokers.Add(new
                    {
                        Id = company.IndividualId,
                        BrokerIndividualId = company.IndividualId,
                        BrokerName = company.Name,
                        BrokerDocumentNumber = company.DocumentNumber,//company.Nit
                        BrokerDocumentTypeId = company.DocumentTypeId,
                    });
                }
            }

            return Json(brokers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetYearMonths
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetYearMonths()
        {
            string urlReferrer = Request.UrlReferrer.ToString();

            if (urlReferrer.IndexOf("en/") > -1)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-us");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-GT");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-GT");
            }

            List<Generic> yearMonths = new List<Generic>();
            yearMonths.Add(new Generic() { Id = 1, Description = @Global.January });
            yearMonths.Add(new Generic() { Id = 2, Description = @Global.February });
            yearMonths.Add(new Generic() { Id = 3, Description = @Global.March });
            yearMonths.Add(new Generic() { Id = 4, Description = @Global.April });
            yearMonths.Add(new Generic() { Id = 5, Description = @Global.May });
            yearMonths.Add(new Generic() { Id = 6, Description = @Global.June });
            yearMonths.Add(new Generic() { Id = 7, Description = @Global.July });
            yearMonths.Add(new Generic() { Id = 8, Description = @Global.August });
            yearMonths.Add(new Generic() { Id = 9, Description = @Global.September });
            yearMonths.Add(new Generic() { Id = 10, Description = @Global.October });
            yearMonths.Add(new Generic() { Id = 11, Description = @Global.November });
            yearMonths.Add(new Generic() { Id = 12, Description = @Global.December });

            return new UifSelectResult(yearMonths);
        }

        /// <summary>
        /// GetBranchDefaultByUserId
        /// Obtiene la sucursal por defecto del usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns>int</returns>
        public int GetBranchDefaultByUserId(int userId, int type)
        {

            if (type == 0)
            {
                return DelegateService.uniqueUserService.GetBranchesByUserId(userId).Where(br => br.IsDefault).ToList()[0].Id;
            }
            return DelegateService.uniqueUserService.GetBranchesByUserId(userId).Count;
        }

        /// <summary>
        /// GetBranchesByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<Branch></returns>
        public List<Branch> GetBranchesByUserId(int userId)
        {
            return DelegateService.uniqueUserService.GetBranchesByUserId(userId).OrderBy(b => b.Description).ToList();
        }

        /// <summary>
        /// GetParameterMulticompany
        /// </summary>
        /// <returns>int</returns>
        public int GetParameterMulticompany()
        {
            bool result = DelegateService.accountingApplicationService.GetParameterMulticompany();
            int isMulticompany = 0;

            if (result)
            {
                isMulticompany = 1;
            }
            else
            {
                isMulticompany = 0;
            }

            return isMulticompany;
        }

        /// <summary>
        /// GetSlipNumbers
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="reinsurerId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetSlipNumbers(int endorsementId)
        {
            List<Generic> slipNumbers = new List<Generic>();

            List<reinsDTOs.EndorsementReinsuranceDTO> endorsementReinsurances = DelegateService.reinsuranceService.GetReinsuranceByEndorsement(endorsementId);

            foreach (reinsDTOs.EndorsementReinsuranceDTO reinsure in endorsementReinsurances)
            {
                List<reinsDTOs.ReinsuranceDistributionDTO> reinsuranceDistributions = DelegateService.reinsuranceService.GetDistributionByReinsurance(reinsure.IssueLayerId);

                foreach (reinsDTOs.ReinsuranceDistributionDTO distribution in reinsuranceDistributions)
                {
                    slipNumbers.Add(new Generic()
                    {
                        Id = Convert.ToInt32(distribution.Contract),
                        Description = distribution.Contract
                    });
                }
            }

            return new UifSelectResult(slipNumbers.GroupBy(x => x.Id).Select(grp => grp.First()).ToList());
        }

        /// <summary>
        /// GetCoinsuranceTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCoinsuranceTypes()
        {
            List<Generic> coinsuranceTypes = new List<Generic>();
            coinsuranceTypes.Add(new Generic() { Id = 1, Description = "Aceptado" });
            coinsuranceTypes.Add(new Generic() { Id = 2, Description = "Cedido" });

            return new UifSelectResult(coinsuranceTypes);
        }

        /// <summary>
        /// GetCoinsuranceByDocumentNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCoinsuranceByDocumentNumber(string query)
        {
            List<object> coinsurances = new List<object>();
            var companies = DelegateService.tempCommonService.GetReinsurerByDocumentNumber(query, Convert.ToInt32(ConfigurationManager.AppSettings["CoinsuranceAutocomplete"]));

            if (companies.Count == 0)
            {
                coinsurances.Add(new
                {
                    Id = -1,
                    DocumentTypeId = -1,
                    DocumentNumber = @Global.RegisterNotFound,
                    Name = @Global.RegisterNotFound,
                    CoinsuranceIndividualId = -1,
                    CoinsuranceName = @Global.RegisterNotFound,
                    CoinsuranceDocumentNumber = @Global.RegisterNotFound
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO company in companies)
                {
                    coinsurances.Add(new
                    {
                        Id = company.IndividualId,
                        CoinsuranceIndividualId = company.IndividualId,
                        CoinsuranceName = company.Name,
                        Name = company.Name,
                        CoinsuranceDocumentNumber = company.DocumentNumber,
                        DocumentNumber = company.DocumentNumber,
                        DocumentTypeId = company.DocumentTypeId,
                    });
                }
            }

            return Json(coinsurances, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCoinsurerByName
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCoinsurerByName(string query)
        {
            List<object> coinsurances = new List<object>();
            List<tempCommonDTO.IndividualDTO> companies = DelegateService.tempCommonService.GetReinsurerByName(query, Convert.ToInt32(ConfigurationManager.AppSettings["CoinsuranceAutocomplete"]), 0);

            if (companies.Count == 0)
            {
                coinsurances.Add(new
                {
                    Id = -1,
                    Name = @Global.RegisterNotFound,
                    DocumentNumber = -1,
                    DocumentTypeId = -1,
                    CoinsuranceIndividualId = -1,
                    CoinsuranceName = @Global.RegisterNotFound,
                    CoinsuranceDocumentNumber = -1
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO company in companies)
                {
                    coinsurances.Add(new
                    {
                        Id = company.IndividualId,
                        Name = company.Name,
                        DocumentNumber = company.DocumentNumber,
                        DocumentTypeId = company.DocumentTypeId,
                        CoinsuranceIndividualId = company.IndividualId,
                        CoinsuranceName = company.Name,
                        CoinsuranceDocumentNumber = company.DocumentNumber
                    });
                }
            }

            return Json(coinsurances, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCoInsuranceCompaniesByDescription
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetCoInsuranceCompaniesByDescription(string query)
        {
            if (coInsuranceCompanies.Count == 0)
            {
                coInsuranceCompanies = DelegateService.uniquePersonServiceV1.GetCoInsuranceCompanies();
            }
            var dataFiltered = coInsuranceCompanies.Where(x => x.Description.Contains(query.ToUpper()) || x.TributaryIdCardNo.ToString().Contains(query));

            return Json(dataFiltered, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// GetRequestTypes
        /// Obtiene los tipos de solictud
        /// </summary>
        /// <returns>List<PaymentSource/></returns>
        public ActionResult GetRequestTypes()
        {
            var requestTypes = DelegateService.glAccountingApplicationService.GetConceptSources();

            var requestTypesResponse = (from requestType in requestTypes
                                        select new
                                        {
                                            Id = requestType.Id,
                                            Description = requestType.Description
                                        });

            return new UifSelectResult(requestTypesResponse);
        }

        /// <summary>
        /// GetAnalysis
        /// </summary>
        /// <returns>List<AnalysisCode/></returns>
        public ActionResult GetAnalysis()
        {
            var analysisCodes = (from GeneralLedgerModel.AnalysisCodeDTO analysisCode in DelegateService.glAccountingApplicationService.GetAnalysisCodes()
                                 select analysisCode).OrderBy(c => c.AnalysisCodeId).ToList();

            return new UifSelectResult(analysisCodes);
        }

        /// <summary>
        /// GetAnalysisConceptByAnalysisId
        /// </summary>
        /// <param name="analysisId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAnalysisConceptByAnalysisId(int analysisId)
        {
            var analysisConceptAnalysis = (from GeneralLedgerModel.AnalysisConceptAnalysisDTO analysisConcept in DelegateService.glAccountingApplicationService.GetPaymentConceptsByAnalysisCode(analysisId)
                                           select analysisConcept).OrderBy(c => c.AnalysisConceptId).ToList();

            return new UifSelectResult(analysisConceptAnalysis);
        }

        /// <summary>
        /// GetCostCenters
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCostCenters()
        {
            var costCenterResponse = (from GeneralLedgerModel.CostCenterDTO costCenter in DelegateService.glAccountingApplicationService.GetCostCenters()
                                      select costCenter).OrderBy(c => c.CostCenterId).ToList();

            return new UifSelectResult(costCenterResponse);
        }

        /// <summary>
        /// GetConceptKeysByAnalysisConceptId
        /// </summary>
        /// <param name="analysisConceptId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetConceptKeysByAnalysisConceptId(int analysisConceptId)
        {
            try
            {
                List<GeneralLedgerModel.AnalysisConceptKeyDTO> analysisConceptKeys = DelegateService.glAccountingApplicationService.GetAnalysisConceptKeysByAnalysisConcept(new Sistran.Core.Application.GeneralLedgerServices.DTOs.AnalysisConceptDTO() { AnalysisConceptId = analysisConceptId });
                return Json(analysisConceptKeys, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region ConceptCode

        /// <summary>
        /// GetConceptCurrentAccountCode
        /// </summary>
        /// <param name="query"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetConceptCurrentAccountCode(string query, int branchId)
        {
            List<object> conceptsResponse = new List<object>();
            int userId = GetUserIdByName(User.Identity.Name);
            var concepts = DelegateService.accountingApplicationService.GetConceptCurrentAccountCode(branchId, userId, query, 1);
            int conceptCount = concepts.Count;

            if (conceptCount == 0)
            {
                conceptsResponse.Add(new
                {
                    ConceptId = 0,
                    ConceptDescription = @Global.RegisterNotFound,
                    AgentEnabled = -1,
                    CurrencyId = -1,
                    PercentageDiference = 0,
                    ItemsEnabled = -1,
                    PaymentConceptId = -1
                });
            }
            else
            {
                foreach (ConceptCurrentAccountDTO concept in concepts)
                {
                    conceptsResponse.Add(new
                    {
                        ConceptId = concept.ConceptId,
                        ConceptDescription = concept.ConceptDescription,
                        AgentEnabled = concept.AgentEnabled,
                        CurrencyId = concept.CurrencyId,
                        PercentageDiference = concept.PercentageDiference,
                        ItemsEnabled = concept.ItemsEnabled,
                        PaymentConceptId = ""
                    });
                }
            }

            return Json(conceptsResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetConceptCurrentAccountDescription
        /// </summary>
        /// <param name="query"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetConceptCurrentAccountDescription(string query, int branchId)
        {
            List<object> conceptsResponse = new List<object>();
            int userId = GetUserIdByName(User.Identity.Name);
            var concepts = DelegateService.accountingApplicationService.GetConceptCurrentAccountDescription(branchId, userId, Convert.ToInt32(query), 1);
            int conceptCount = concepts.Count;

            if (conceptCount == 0)
            {
                conceptsResponse.Add(new
                {
                    ConceptId = 0,
                    ConceptDescription = @Global.RegisterNotFound,
                    AgentEnabled = -1,
                    CurrencyId = -1,
                    PercentageDiference = 0,
                    ItemsEnabled = -1,
                    PaymentConceptId = -1
                });
            }
            else
            {
                foreach (ConceptCurrentAccountDTO concept in concepts)
                {
                    conceptsResponse.Add(new
                    {
                        ConceptId = concept.ConceptId,
                        ConceptDescription = concept.ConceptDescription,
                        AgentEnabled = concept.AgentEnabled,
                        CurrencyId = concept.CurrencyId,
                        PercentageDiference = concept.PercentageDiference,
                        ItemsEnabled = concept.ItemsEnabled,
                        PaymentConceptId = concept.AccountingConceptId
                    });
                }
            }

            return Json(conceptsResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingAccountConceptByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="sourceId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingAccountConceptByBranchId(int branchId, int sourceId)
        {
            int userId = GetUserIdByName(User.Identity.Name);
            List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            var concepts = DelegateService.glAccountingApplicationService.GetAccountingConceptsByCriteria(userId, branchId, 0);

            foreach (var accountingConcept in concepts)
            {
                if ((sourceId == 1) && (accountingConcept.AgentEnabled)) // Agente
                {
                    accountingConcepts.Add(accountingConcept);
                }
                if ((sourceId == 2) && (accountingConcept.CoInsurancedEnabled)) // Coaseguro
                {
                    accountingConcepts.Add(accountingConcept);
                }
                if ((sourceId == 3) && (accountingConcept.ReInsuranceEnabled)) // Reaseguro
                {
                    accountingConcepts.Add(accountingConcept);
                }
            }

            return new UifSelectResult(accountingConcepts);
        }

        /// <summary>
        /// GetAdditionalInformationConcept
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="conceptId"></param>
        /// <param name="sourceId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAdditionalInformationConcept(int branchId, int conceptId, int sourceId)
        {
            int currencyId = -1;

            AccountingConceptModel.AccountingConceptDTO accountingConcept = DelegateService.glAccountingApplicationService.GetAccountingConcept(
                                                                            new AccountingConceptModel.AccountingConceptDTO() { Id = conceptId });

            List<object> concepts = new List<object>();

            if (accountingConcept != null)
            {
                if (accountingConcept.AccountingAccount.AccountingAccountId > 0)
                {
                    currencyId = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Currency.Id;
                }

                concepts.Add(new
                {
                    AgentEnabled = accountingConcept.AgentEnabled,
                    ConceptDescription = accountingConcept.Description,
                    ConceptId = accountingConcept.Id,
                    CurrencyId = currencyId,
                    ItemsEnabled = accountingConcept.ItemEnabled,
                    PaymentConceptId = accountingConcept.Id,
                    PercentageDiference = 0
                });
            }

            return Json(concepts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPaymentSources
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentSources()
        {
            var paymentSources = DelegateService.glAccountingApplicationService.GetConceptSources();
            var paymentSourcesResponse = (from paymentSource in paymentSources
                                          where paymentSource.Id != Convert.ToInt32(ConfigurationManager.AppSettings["ConceptSourceId"]) //8
                                          select new
                                          {
                                              Id = paymentSource.Id,
                                              Description = paymentSource.Description
                                          }
                                          );

            return new UifSelectResult(paymentSourcesResponse);
        }

        /// <summary>
        /// GetConceptSources
        /// </summary>
        /// <returns>List<ConcpetSource/></returns>
        public List<AccountingConceptModel.ConceptSourceDTO> GetConceptSources()
        {
            return DelegateService.glAccountingApplicationService.GetConceptSources();
        }

        #endregion

        #region DateFormat

        /// <summary>
        /// DateFormat
        /// Permite formatear la fecha a dd/MM/yyyy independientemente del idioma seleccionado
        /// </summary>
        /// <param name="date"></param>
        /// <param name="operation"></param>
        /// <returns>string</returns>
        public string DateFormat(DateTime date, int operation)
        {
            string dateFormat = "";
            string hourFormat = "";
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            int hour = date.Hour;
            int minute = date.Minute;
            int second = date.Second;

            if (day.ToString().Length == 1)
            {
                dateFormat = "0" + day.ToString() + "/";
            }
            else
            {
                dateFormat = day.ToString() + "/";
            }

            if (month.ToString().Length == 1)
            {
                dateFormat = dateFormat + "0" + month.ToString() + "/";
            }
            else
            {
                dateFormat = dateFormat + month.ToString() + "/";
            }

            if (operation.Equals(1))
            {
                if (hour.ToString().Length == 1)
                {
                    hourFormat = "0" + hour.ToString() + ":";
                }
                else
                {
                    hourFormat = hour.ToString() + ":";
                }
                if (minute.ToString().Length == 1)
                {
                    hourFormat = hourFormat + "0" + minute.ToString() + ":";
                }
                else
                {
                    hourFormat = hourFormat + minute.ToString() + ":";
                }
                if (second.ToString().Length == 1)
                {
                    hourFormat = hourFormat + "0" + second.ToString();
                }
                else
                {
                    hourFormat = hourFormat + second.ToString();
                }
                dateFormat = dateFormat + year.ToString() + " " + hourFormat;
            }
            else
            {
                dateFormat = dateFormat + year.ToString();
            }

            return dateFormat;
        }

        #endregion

        #region CheckInternalBallot

        /// <summary>
        /// GetAccountBankSelect
        /// Carga DropDown Banco Receptor
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountBankSelect()
        {
            return new UifSelectResult(GetDistinctBanks());
        }

        /// <summary>
        /// GetAccountBankNumber
        /// Carga DropDown Número de Cuenta
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountBankNumber()
        {
            return new UifSelectResult(GetAccountByBankId(-1));
        }

        /// <summary>
        /// LoadContractNumbers
        /// Contratos de prueba
        /// </summary>
        /// <returns>List<NameGeneral></returns>
        public static List<NameGeneral> LoadContractNumbers()
        {
            List<NameGeneral> contractNumbers = new List<NameGeneral>();

            contractNumbers.Add(new NameGeneral() { Id = 1, Name = "CONTRATO DE RETENCION GENERAL" });
            contractNumbers.Add(new NameGeneral() { Id = 5, Name = "CUOTA PARTE AUTO NUEVO" });
            contractNumbers.Add(new NameGeneral() { Id = 7, Name = "CUOTA PARTE PARA AUTOMOVILES 2018 60% CESIÓN" });
            return contractNumbers;
        }


        /// <summary>
        /// GetAccountByBankId
        /// Obtiene las cuentas de un banco asociado a la compañía
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountByBankIdSelect(int bankId)
        {
            return new UifSelectResult(GetAccountByBankId(bankId));
        }


        /// <summary>
        /// GetAccountDistinctByBankId
        /// Obtiene los números de cuenta eliminando duplicados filtrados por el banco
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountDistinctByBankId(int bankId)
        {

            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();

            List<BankAccountCompanyDTO> companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))).ToList();

            var filterAccounts = companyBankAccounts
                        .Select(m => new { m.Number, m.Currency.Id })
                        .Distinct()
                        .ToList();

            return new UifSelectResult(filterAccounts.OrderBy(x => x.Number));
        }

        public JsonResult GetAccountingDistinctByBankIdCurrencyCode(int bankId, int currencyCode)
        {

            var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();

            List<BankAccountCompanyDTO> companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))&& r.Currency.Id.Equals(currencyCode)).ToList();

            var filterAccounts = companyBankAccounts
                        .Select(m => new { m.Number, m.AccountingAccount.AccountingAccountId })
                        .Distinct()
                        .ToList();

            return new UifSelectResult(filterAccounts.OrderBy(x => x.Number));
        }

        /// <summary>
        /// GetAccountCurrencyByBankIdSelect
        /// Obtiene la moneda de la cuenta de un banco asociado a la compañía y carga en dropdown
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountCurrencyByBankIdSelect(int bankId, string accountNumber)
        {
            BankAccountCompanyDTO bankAccountCompany = GetAccountCurrencyByBankId(bankId, accountNumber);

            return Json(bankAccountCompany, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Parameter

        /// <summary>
        /// GetBillNumber
        /// </summary>
        /// <returns>int</returns>
        public int GetBillNumber()
        {
            try
            {
                int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["MaskReceiptNumber"]);

                return (int)DelegateService.commonService.GetParameterByParameterId(parameterId).NumberParameter;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateBillNumber
        /// </summary>
        /// <param name="number"></param>
        public void UpdateBillNumber(int number)
        {
            int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["MaskReceiptNumber"]);
            Parameter parameter = new Parameter();
            parameter.Id = parameterId;
            parameter.NumberParameter = number;
            parameter.Description = "Numero de Caratula de Recibo";
            parameter.NumberParameter = parameter.NumberParameter + 1;
            DelegateService.commonService.UpdateParameter(parameter);
        }

        /// <summary>
        /// GetTransactionNumber
        /// </summary>
        /// <returns>int</returns>
        public int GetTransactionNumber()
        {
            int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["JournalEntryTransactionNumber"]);

            return (int)DelegateService.commonService.GetParameterByParameterId(parameterId).NumberParameter;
        }

        /// <summary>
        /// GetLimitAmountAmortization
        /// </summary>
        /// <returns>int</returns>
        public int GetLimitAmountAmortization()
        {
            try
            {
                int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["LimitAmountAmortization"]);
                int number = (int)DelegateService.commonService.GetParameterByParameterId(parameterId).NumberParameter;

                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// UpdateTransactionNumber
        /// </summary>
        /// <param name="number"></param>
        public void UpdateTransactionNumber(int number)
        {
            int parameterId = Convert.ToInt32(ConfigurationManager.AppSettings["JournalEntryTransactionNumber"]);
            Parameter parameter = new Parameter();
            parameter.Id = parameterId;
            parameter.NumberParameter = number;
            parameter.Description = "Num Transacción Asiento Diario";
            parameter.NumberParameter = parameter.NumberParameter + 1;

            DelegateService.commonService.UpdateParameter(parameter);
        }

        /// <summary>
        /// GetSearchBy
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetSearchBy()
        {
            List<object> searchs = new List<object>();
            searchs.Add(new { Id = 1, Description = "POLIZA" });
            searchs.Add(new { Id = 2, Description = "DOCUMENTO TITULAR POLIZA" });
            searchs.Add(new { Id = 5, Description = "NOMBRE TITULAR POLIZA" });
            searchs.Add(new { Id = 3, Description = "DOCUMENTO PAGADOR" });
            searchs.Add(new { Id = 4, Description = "NOMBRE PAGADOR" });

            return new UifSelectResult(searchs);
        }

        /// <summary>
        /// GetSectors
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetSectors()
        {
            List<object> sectors = new List<object>();
            sectors.Add(new { Id = 4000, Description = "RECLAMOS", SystemId = 4 });
            return new UifSelectResult(sectors);
        }

        /// <summary>
        /// GetRejectionsSelect
        /// </summary>
        /// <returns>List<Rejection/></returns>
        public ActionResult GetRejectionsSelect()
        {
            return new UifSelectResult(DelegateService.accountingParameterService.GetRejections());
        }

        /// <summary>
        /// GetPaymentSearchBy
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentSearchBy()
        {
            List<object> paymentResponse = new List<object>();
            paymentResponse.Add(new { Id = 1, Description = "ASEGURADO" });
            paymentResponse.Add(new { Id = 2, Description = "POLIZA" });

            return new UifSelectResult(paymentResponse);
        }

        #endregion

        #region BankAccounts


        /// <summary>
        /// GetAccountByBankId
        /// Obtiene las cuentas de un banco
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>List<Models.AccountBank/></returns>
        public List<Models.BankAccount> GetAccountByBankId(int bankId)
        {
            try
            {
                var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
                var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))).ToList();
                List<Models.BankAccount> bankAccounts = new List<Models.BankAccount>();

                foreach (BankAccountCompanyDTO companyBankAccount in companyBankAccounts)
                {
                    bankAccounts.Add(new Models.BankAccount
                    {
                        Description = companyBankAccount.Number,
                        Id = companyBankAccount.Id
                    });
                }

                return (bankAccounts);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetAccountCurrencyByBankId
        /// Obtiene las cuentas corrientes de un banco
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="accountNumber"></param>
        /// <returns>BankAccountCompany</returns>
        public BankAccountCompanyDTO GetAccountCurrencyByBankId(int bankId, string accountNumber)
        {
            try
            {
                BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();

                var bankAccountCompanies = DelegateService.accountingParameterService.GetBankAccountCompanies();
                var companyBankAccounts = bankAccountCompanies.Where(r => (r.Bank.Id.Equals(bankId))).ToList();

                if (companyBankAccounts.Count > 0)
                {
                    companyBankAccounts = (from i in companyBankAccounts where i.Number == accountNumber select i).ToList();

                    if (companyBankAccounts.Count > 0)
                    {
                        DTOs.Search.CurrencyDTO currency = new DTOs.Search.CurrencyDTO();
                        currency.Id = companyBankAccounts[0].Currency.Id;
                        currency.Description = companyBankAccounts[0].Currency.Description;

                        bankAccountCompany.Currency = currency;
                    }
                }

                return bankAccountCompany;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetAccountBank
        /// Obtiene las cuentas de un banco
        /// </summary>
        /// <returns>List<Bank/></returns>
        public List<BankDTO> GetAccountBank()
        {
            try
            {
                List<BankDTO> banks = new List<BankDTO>();
                var distinctBankAccounts = DelegateService.accountingParameterService.GetBankAccountPersons()
                    .GroupBy(p => new { p.Bank.Id, p.Bank.Description })
                    .Select(g => g.First())
                    .ToList();

                foreach (BankAccountPersonDTO person in distinctBankAccounts)
                {
                    banks.Add(person.Bank);
                }

                return banks;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetPersonBankAccounts
        /// </summary>
        /// <returns>List<BankAccountPerson></returns>
        public List<BankAccountPersonDTO> GetPersonBankAccounts()
        {
            try
            {
                return DelegateService.accountingParameterService.GetBankAccountPersons();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        /// <summary>
        /// ReplaceWithAsterisks
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns>string</returns>
        public string ReplaceWithAsterisks(string cardNumber)
        {
            int row = 0;

            string valueWithAsterisk = "";

            for (row = 0; row < cardNumber.Length - 2; row++)
            {
                valueWithAsterisk += "*";
            }

            return valueWithAsterisk;
        }

        #endregion

        #region User

        ///<summary>
        /// GetUserAuthenticated
        /// Obtiene el usuario autenticado.
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetUserAuthenticated()
        {
            try
            {
                string user = User.Identity.Name;
                user = user.ToUpper();
                int userId = GetUserIdByName(User.Identity.Name);

                object users;

                users = new
                {
                    UserId = userId,
                    UserName = user
                };

                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetUserIdByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>int</returns>
        public int GetUserIdByName(string name)
        {
            return DelegateService.uniqueUserService.GetUserByName(name)[0].UserId;
        }

        /// <summary>
        /// GetUserByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<User/></returns>
        public List<User> GetUserByName(string name)
        {
            if (name == string.Empty)
            {
                throw new BusinessException("User Name is Empty");
            }

            return DelegateService.uniqueUserService.GetUserByName(name);
        }

        #endregion        

        #region AccountingConcept

        public List<object> GetAccountingConceptsByFilter(Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingAccountFilterDTO accountingAccountFilterDTO)
        {
            List<object> paymentConcepts = new List<object>();
            List<Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts.AccountingConceptDTO> concepts =
                DelegateService.glAccountingApplicationService.GetAccountingConceptsByFilter(accountingAccountFilterDTO);

            if (concepts != null && concepts.Count > 0)
            {
                concepts.ForEach(c =>
                {
                    paymentConcepts.Add(new
                    {
                        Id = Convert.ToString(c.Id),
                        c.Description,
                        GeneralLedgerId = c.AccountingAccount.AccountingAccountId,
                        c.AccountingAccount.CurrencyId,
                        AccountingNumber = c.AccountingAccount.Number,
                        AccountingName = c.AccountingAccount.Description,
                        c.AccountingAccount.RequiresAnalysis,
                        c.AccountingAccount.RequiresCostCenter,
                        c.AccountingAccount.MultiCurrency,
                        c.AccountingAccount.AccountingNature,
                        AnalysisId = (c.AccountingAccount.Analysis != null) ? c.AccountingAccount.Analysis.AnalysisId : 0
                    });
                });
            }
            else
            {
                paymentConcepts.Add(new
                {
                    Name = Resources.Global.RegisterNotFound,
                    Id = Resources.Global.RegisterNotFound,
                });
            }
            return paymentConcepts;
        }

        /// <summary>
        /// GetPaymentConceptById
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="payerId"></param>
        /// <returns>List<object/></returns>
        public List<object> GetAccountingConceptById(int accountingConceptId, int branchId, int userId, int payerId)
        {
            List<object> concepts = new List<object>();

            List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConceptsByUserIdBranchIdIndividualId(userId, branchId, payerId);

            if (accountingConcepts.Count > 0)
            {
                accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in
                           accountingConcepts.Where(d => d.Id.ToString().StartsWith(accountingConceptId.ToString()))
                                      select accountingConcept).ToList();

                foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts)
                {
                    concepts.Add(new
                    {
                        Id = Convert.ToString(accountingConcept.Id),
                        accountingConcept.Description,
                        GeneralLedgerId = accountingConcept.AccountingAccount.AccountingAccountId,
                        accountingConcept.AccountingAccount.CurrencyId,
                        AccountingNumber = accountingConcept.AccountingAccount.Number,
                        AccountingName = accountingConcept.AccountingAccount.Description,
                        accountingConcept.AccountingAccount.RequiresAnalysis,
                        accountingConcept.AccountingAccount.RequiresCostCenter
                    });
                }
            }
            return concepts;
        }

        /// <summary>
        /// GetPaymentConceptByDescription
        /// </summary>
        /// <param name="accountingConceptDescription"></param>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="payerId"></param>
        /// <returns>List<object/></returns>
        public List<object> GetAccountingConceptByDescription(string accountingConceptDescription, int branchId, int userId, int payerId)
        {
            List<object> concepts = new List<object>();

            List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConceptsByUserIdBranchIdIndividualId(userId, branchId, payerId);

            accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Description.ToString().ToUpper().Contains(accountingConceptDescription.ToUpper())) select accountingConcept).ToList();

            foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts)
            {
                concepts.Add(new
                {
                    Id = Convert.ToString(accountingConcept.Id),
                    accountingConcept.Description,
                    GeneralLedgerId = accountingConcept.AccountingAccount.AccountingAccountId,
                    accountingConcept.AccountingAccount.CurrencyId,
                    accountingConcept.AccountingAccount.Number,
                    AccountingName = accountingConcept.AccountingAccount.Description,
                    accountingConcept.AccountingAccount.RequiresAnalysis,
                    accountingConcept.AccountingAccount.RequiresCostCenter
                });
            }

            return concepts;
        }

        /// <summary>
        /// GetPaymentsConceptById
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingConceptsById(string query)
        {
            string[] param = query.Split('/');
            int paymentBranchCode = Convert.ToInt32(param[2]);
            string dataSearch = param[0];
            int typeSearch = Convert.ToInt32(param[3]);

            List<object> concepts = new List<object>();

            try
            {
                List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConceptsByCriteria(GetUserIdByName(User.Identity.Name), paymentBranchCode, 0);

                //Búsqueda por código de concepto contable
                if (typeSearch == 1)
                {
                    accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Id.ToString().Contains(dataSearch)) select accountingConcept).ToList();
                }

                //Búsqueda por descripcion del concepto contable
                if (typeSearch == 2)
                {
                    accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Description.ToString().ToUpper().Contains(dataSearch.ToUpper())) select accountingConcept).ToList();
                }

                foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts)
                {
                    concepts.Add(new
                    {
                        PaymentConceptId = accountingConcept.Id.ToString(),
                        PaymentConceptDescription = accountingConcept.Description,
                        AccountingAccountId = accountingConcept.AccountingAccount.AccountingAccountId
                    });
                }

                return Json(concepts, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetPaymentsConceptByIdDescription
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingConceptstByIdDescription(string query)
        {
            string[] param = query.Split('/');
            int paymentBranchCode = Convert.ToInt32(param[5]);
            string dataSearch = param[0];
            int typeSearch = Convert.ToInt32(param[6]);

            List<object> concepts = new List<object>();

            try
            {
                List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConceptsByCriteria(GetUserIdByName(User.Identity.Name), paymentBranchCode, 0);

                //Búsqueda por código de concepto contable
                if (typeSearch == 1)
                {
                    accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Id.ToString().Contains(dataSearch)) select accountingConcept).ToList();
                }

                //Búsqueda por descripcion del concepto contable
                if (typeSearch == 2)
                {
                    accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Description.ToString().ToUpper().Contains(dataSearch.ToUpper())) select accountingConcept).ToList();
                }

                foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts)
                {
                    concepts.Add(new
                    {
                        PaymentConceptId = accountingConcept.Id.ToString(),
                        PaymentConceptDescription = accountingConcept.Description,
                        AccountingAccountId = accountingConcept.AccountingAccount.AccountingAccountId
                    });
                }

                return Json(concepts, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion AccountingConcept

        #region LoadFromDataBase

        /// <summary>
        /// LoadBranch
        /// Obtiene las sucursales
        /// </summary>
        /// <returns>List<Branch/></returns>
        public List<Branch> LoadBranch(string userName)
        {
            var branches = GetBranchesByUserName(userName);
            return branches.OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        /// LoadPrefix
        /// Obtiene los ramos
        /// </summary>
        /// <returns>List<Prefix/></returns>
        public List<Prefix> LoadPrefix()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();

            return prefixes.OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        /// GetCurrencyExchangeRate
        /// </summary>
        /// <param name="rateDate"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCurrencyExchangeRate(DateTime rateDate, int currencyId)
        {
            decimal exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId).SellAmount; // valor por defecto
            return Json(exchangeRate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadTypeDelivery
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult LoadTypeDelivery()
        {
            List<SearchBy> searchBys = new List<SearchBy>();

            searchBys.Add(new SearchBy() { Id = 1, Description = @Global.Billing });
            searchBys.Add(new SearchBy() { Id = 2, Description = @Global.Beneficiary });
            searchBys.Add(new SearchBy() { Id = 3, Description = @Global.Repayment });

            return new UifSelectResult(searchBys);
        }

        #endregion

        #region VoucherType

        /// <summary>
        /// GetVoucherTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetVoucherTypes()
        {
            return new UifSelectResult(DelegateService.accountingAccountsPayableService.GetVoucherTypes().OrderBy(o => o.Description).ToList());
        }

        #endregion

        #region LoadFromDataBurned

        /// <summary>
        /// LoadSearchByForPolicies
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadSearchByForPolicies()
        {
            List<SearchBy> searchBys = new List<SearchBy>();

            searchBys.Add(new SearchBy() { Id = 1, Description = @Global.Insured, SmallDescription = "ASEG" });
            searchBys.Add(new SearchBy() { Id = 2, Description = @Global.Payer, SmallDescription = "PAGA" });
            searchBys.Add(new SearchBy() { Id = 3, Description = @Global.Agent, SmallDescription = "AGEN" });
            searchBys.Add(new SearchBy() { Id = 4, Description = @Global.Group, SmallDescription = "GRUP" });
            searchBys.Add(new SearchBy() { Id = 5, Description = @Global.Policy, SmallDescription = "POLI" });
            searchBys.Add(new SearchBy() { Id = 6, Description = @Global.Bill, SmallDescription = "FACT" });

            return new UifSelectResult(searchBys);
        }

        #endregion

        #region BankConciliation

        /// <summary>
        /// LoadBankReconciliations
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadBankReconciliations()
        {
            var bankReconciliations = (from GeneralLedgerModel.ReconciliationMovementTypeDTO bankReconciliation in DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes()
                                       select bankReconciliation).OrderBy(c => c.Description).ToList();

            return new UifSelectResult(bankReconciliations);
        }

        /// <summary>
        /// GetBankById
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>Bank</returns>
        public Bank GetBankById(int bankId)
        {
            var banks = DelegateService.commonService.GetBanks();
            var bank = banks.Where(sl => sl.Id == bankId).ToList();

            return bank[0];
        }

        /// <summary>
        /// GetBusinessTypes
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetBusinessTypes()
        {
            List<object> businessTypes = new List<object>();
            try
            {
                foreach (SelectListItem item in EnumsHelper.GetItems<BusinessType>())
                {
                    businessTypes.Add(new
                    {
                        Description = item.Text,
                        Id = item.Value
                    });
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return businessTypes;
        }

        #endregion BankConciliation

        #region Supplier

        /// <summary>
        /// GetSuppliersByDocumentNumber
        /// Obtiene listado de provedores
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSuppliersByDocumentNumber(string query)
        {
            List<object> suppliers = new List<object>();
            var providers = DelegateService.tempCommonService.GetSuppliersByDocumentNumber(query);

            if (providers.Count == 0)
            {
                suppliers.Add(new
                {
                    Id = -1,
                    IndividualId = -1,
                    Name = @Global.RegisterNotFound,
                    DocumentNumber = @Global.RegisterNotFound, //- 1,
                    SupplierCode = -1,
                    DocumentTypeId = -1
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO provider in providers)
                {
                    suppliers.Add(new
                    {
                        Id = provider.IndividualId,
                        IndividualId = provider.IndividualId,
                        Name = provider.Name,
                        DocumentNumber = provider.DocumentNumber,
                        SupplierCode = -1,
                        DocumentTypeId = provider.DocumentTypeId
                    });
                }
            }

            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetSuppliersByName
        /// Obtiene listado de proveedores
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSuppliersByName(string query)
        {
            List<object> suppliers = new List<object>();
            var providers = DelegateService.tempCommonService.GetSuppliersByName(query);

            if (providers.Count == 0)
            {
                suppliers.Add(new
                {
                    Id = -1,
                    IndividualId = -1,
                    Name = @Global.RegisterNotFound,
                    DocumentNumber = -1,
                    SupplierCode = -1,
                    DocumentTypeId = -1
                });
            }
            else
            {
                foreach (tempCommonDTO.IndividualDTO supplier in providers)
                {
                    suppliers.Add(new
                    {
                        Id = supplier.IndividualId,
                        IndividualId = supplier.IndividualId,
                        Name = supplier.Name,
                        DocumentNumber = supplier.DocumentNumber,
                        SupplierCode = supplier.IndividualId,
                        DocumentTypeId = supplier.DocumentTypeId
                    });
                }
            }

            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BillingGroup

        /// <summary>
        /// GetBillingGroup
        /// Obtiene una lista de grupos al que pertenecen las pólizas
        /// </summary>
        /// <param name="dataSearch"></param>
        /// <param name="typeSearch"></param>
        /// <returns>List<object/></returns>
        public List<object> GetBillingGroup(string dataSearch, int typeSearch)
        {
            List<object> billingGroups = new List<object>();

            List<BillingGroup> billingGroupQuerys = DelegateService.underwritingService.GetBillingGroupsByDescription(dataSearch);

            foreach (BillingGroup billingGroup in billingGroupQuerys)
            {
                billingGroups.Add(new
                {
                    Id = billingGroup.Id,
                    Description = billingGroup.Description
                });
            }

            return billingGroups;
        }

        #endregion

        #region AccountBank

        /// <summary>
        /// GetAccountBanksByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountBanksByIndividualId(int individualId)
        {
            List<Object> bankAccounts = new List<Object>();

            var bankAccountPersons = DelegateService.accountingParameterService.GetBankAccountPersons();
            var personBankAccounts = bankAccountPersons.Where(r => (r.Individual.IndividualId.Equals(individualId))).ToList();

            foreach (BankAccountPersonDTO bankAccountPerson in personBankAccounts)
            {
                bankAccounts.Add(new
                {
                    AccountBankId = bankAccountPerson.BankAccountType.Id,
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

            return Json(bankAccounts, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ValueType

        /// <summary>
        /// GetValueTypes
        /// Método quemado para ingreso de postfechados en movimientos de contabilidad
        /// </summary>
        /// <returns>UifSelectResult</returns>
        public UifSelectResult GetValueTypes()
        {
            List<ValueTypeDTO> valueTypes = new List<ValueTypeDTO>();
            valueTypes.Add(new ValueTypeDTO() { Id = 1, Description = "CHEQUE POSTFECHADO" });
            valueTypes.Add(new ValueTypeDTO() { Id = 2, Description = "TARJETA POSTFECHADA" });

            return new UifSelectResult(valueTypes);
        }

        /// <summary>
        /// GetValueNumber
        /// Método quemado para ingreso de valores en movimientos de contabilidad
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetValueNumber(string query)
        {
            List<ValueNumber> valueNumbers = new List<ValueNumber>();
            valueNumbers.Add(new ValueNumber { Id = 1, Description = "112522" });
            valueNumbers.Add(new ValueNumber { Id = 2, Description = "365211" });
            valueNumbers.Add(new ValueNumber { Id = 3, Description = "985111" });

            List<object> valueNumbersResponse = new List<object>();

            if (query != null)
            {
                int length = query.Length;
                foreach (ValueNumber value in valueNumbers)
                {
                    if ((length <= value.Description.Length) && (((value.Description).IndexOf(query.ToUpper())) > -1))
                    {
                        valueNumbersResponse.Add(new
                        {
                            Id = value.Id,
                            Value = value.Description
                        });
                    }
                }
            }

            return Json(valueNumbersResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion ValueType

        #region CostCenter

        /// <summary>
        /// GetCostCenter
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetCostCenter(string query)
        {
            List<CostCenter> costCenters = new List<CostCenter>();
            costCenters.Add(new CostCenter { Id = 1, Description = "CENTRO DE COSTOS 1", Porcentage = "25%" });
            costCenters.Add(new CostCenter { Id = 2, Description = "CENTRO DE COSTOS 2", Porcentage = "50%" });
            costCenters.Add(new CostCenter { Id = 3, Description = "CENTRO DE COSTOS 3", Porcentage = "75" });

            List<object> costCentersResponse = new List<object>();

            if (query != null)
            {
                int length = query.Length;
                foreach (CostCenter costCenter in costCenters)
                {
                    if ((length <= costCenter.Description.Length) && (((costCenter.Description).IndexOf(query.ToUpper())) > -1))
                    {
                        costCentersResponse.Add(new
                        {
                            Id = costCenter.Id,
                            Value = costCenter.Description
                        });
                    }
                }
            }

            return Json(costCentersResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion CostCenter

        #region ForeignExchangeRate

        /// <summary>
        /// GetForeignCurrencyExchangeRate
        /// Obtiene la tasa de cambio para moneda extranjera
        /// </summary>
        /// <returns>List<ForeignCurrencyExchangeRate/></returns>
        public List<DTOs.Search.ForeignCurrencyExchangeRate> GetForeignCurrencyExchangeRate()
        {
            List<DTOs.Search.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates = new List<DTOs.Search.ForeignCurrencyExchangeRate>();
            // Se obtiene las monedas
            List<Currency> currencies = DelegateService.commonService.GetCurrencies();

            foreach (Currency currency in currencies)
            {
                if (currency.Id != 0)
                {
                    DTOs.Search.ForeignCurrencyExchangeRate foreignCurrencyExchangeRate = new DTOs.Search.ForeignCurrencyExchangeRate();
                    foreignCurrencyExchangeRate.CurrencyId = currency.Id;
                    foreignCurrencyExchangeRate.ExchangeRate = GetForeignCurrencyExchangeRate(currency.Id);

                    foreignCurrencyExchangeRates.Add(foreignCurrencyExchangeRate);
                }
            }

            return foreignCurrencyExchangeRates;
        }

        #endregion

        #region Format

        /// <summary>
        /// FileNameFormat
        /// Concatena el nombre del archivo con el formato de fecha y hora yyyymmddhh24miss.xls
        /// </summary>
        /// <param name="date"></param>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        public string FileNameFormat(DateTime date, string fileName)
        {
            string dateFormat = "";
            string hourFormat = "";
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            int hour = date.Hour;
            int minute = date.Minute;
            int second = date.Second;

            if (day.ToString().Length == 1)
            {
                dateFormat = "0" + day.ToString() + "";
            }
            else
            {
                dateFormat = day.ToString() + "";
            }

            if (month.ToString().Length == 1)
            {
                dateFormat = dateFormat + "0" + month.ToString() + "";
            }
            else
            {
                dateFormat = dateFormat + month.ToString() + "";
            }

            if (hour.ToString().Length == 1)
            {
                hourFormat = "0" + hour.ToString() + "";
            }
            else
            {
                hourFormat = hour.ToString() + "";
            }
            if (minute.ToString().Length == 1)
            {
                hourFormat = hourFormat + "0" + minute.ToString() + "";
            }
            else
            {
                hourFormat = hourFormat + minute.ToString() + "";
            }
            if (second.ToString().Length == 1)
            {
                hourFormat = hourFormat + "0" + second.ToString();
            }
            else
            {
                hourFormat = hourFormat + second.ToString();
            }
            dateFormat = dateFormat + year.ToString() + "" + hourFormat;

            dateFormat = fileName + dateFormat;

            return dateFormat;
        }

        /// <summary>
        /// FormatDecimal
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>decimal</returns>
        public decimal FormatDecimal(string amount)
        {
            if (amount.Contains("."))
            {
                return Convert.ToDecimal(amount.Replace(".", ","));
            }
            else
            {
                return Convert.ToDecimal(amount.Replace(",", "."));
            }
        }

        #endregion

        #region AccountImputation

        /// <summary>
        /// GetPremiumRecievableAppliedByBillId
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<imputation.PremiumReceivableItemDTO/></returns>
        public List<DTOs.Search.PremiumReceivableItemDTO> GetPremiumRecievableAppliedByBillId(int billId, int imputationTypeId)
        {
            try
            {
                return DelegateService.accountingApplicationService.GetPremiumRecievableAppliedByCollectId(billId, imputationTypeId).OrderBy(o => o.PremiumReceivableItemId).ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion AccountImputation

        #region DocumentType

        /// <summary>
        /// GetDocumentTypes
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDocumentTypes()
        {
            var documentTypes = DelegateService.uniquePersonServiceV1.GetDocumentTypes(3);
            return new UifSelectResult(documentTypes);
        }

        #endregion

        #region Method Private

        /// <summary>
        /// GetForeignCurrencyExchangeRate
        /// Obtiene la tasa de cambio del día de la moneda extranjera 
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        private decimal GetForeignCurrencyExchangeRate(int currencyId)
        {
            DateTime rateDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            decimal exchangeRate = 0;
            try
            {

                exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId).BuyAmount;

                if (exchangeRate == 0 || exchangeRate == 1)
                {
                    exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(rateDate, currencyId).BuyAmount; // valor por defecto
                }

                return exchangeRate;

            }
            catch (BusinessException)
            {

                return exchangeRate;
            }
            catch (UnhandledException)
            {

                return exchangeRate;
            }
        }

        /// <summary>
        /// GetDistinctBanks
        /// </summary>
        /// <returns></returns>
        private List<object> GetDistinctBanks()
        {
            List<object> banks = new List<object>();
            //Se obtiene las cuentas bancarias de la compañía
            var companyBankAccounts = DelegateService.accountingParameterService.GetBankAccountCompanies();

            //Se agrupa por banco
            var groupBanks = from p in companyBankAccounts
                             group p by new
                             {
                                 p.Bank.Id
                             } into groupBank
                             select groupBank;

            foreach (var groupBank in groupBanks)
            {
                banks.Add(new
                {
                    Description = companyBankAccounts.Where(x => x.Bank.Id == groupBank.Key.Id).FirstOrDefault()?.Bank.Description,
                    Id = Convert.ToInt32(groupBank.Key.Id)
                });
            }

            return banks;
        }

        //public JsonResult GetBankBranches(int bankId)
        //{
        //    List<BankBranch> bankBranches = DelegateService.commonService.GetBankBranches(bankId);
        //    return new UifJsonResult(true, bankBranches.OrderBy(x => x.Description));
        //}

        //public JsonResult GetCountries()
        //{
        //    List<Country> countries = DelegateService.commonService.GetCountries();
        //    return new UifJsonResult(true, countries.OrderBy(x => x.Description));
        //}

        //public JsonResult GetCities(int countryId)
        //{
        //    Country country = new Country();
        //    country.Id = countryId;
        //    List<City> cities = DelegateService.commonService.GetCitiesByCountry(country);
        //    return new UifJsonResult(true, cities.OrderBy(x => x.Description));
        //}

        //public JsonResult GetStatesByCountry(int idCountry)
        //{
        //    List<State> states = DelegateService.commonService.GetStatesByCountryId(idCountry);
        //    return new UifJsonResult(true, states.OrderBy(x => x.Description));
        //}
        

        #endregion
    }
}