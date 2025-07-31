using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Application.TempCommonServices.Models;
using AccountingConceptModel = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Assemblers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.GeneralLedgerServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class BaseController : Controller
    {
        
        #region Currency

        /// <summary>
        /// GetCurrencies
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCurrencies()
        {
            try
            {
                return new UifSelectResult(DelegateService.commonService.GetCurrencies());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCurrency
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCurrency(int currencyId)
        {
            try
            {
                return Json(GetCurrencyById(currencyId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCurrencyById
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>Currency</returns>
        private Currency GetCurrencyById(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currency = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currency[0];
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

        /// <summary>
        /// CurrentCulture
        /// </summary>
        private void CurrentCulture()
        {
            string urlReferrer = Request.UrlReferrer.ToString();
            if (urlReferrer.IndexOf("/en/") != -1)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
        }


        #endregion Currency

        #region Branch

        /// <summary>
        /// GetBranches
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult GetBranches()
        {
            try
            {
                var branches = (from Branch branch in
                                    DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId())
                                select branch).OrderBy(c => c.Description);
                return new UifSelectResult(branches);
            }       
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }          
        }

        /// <summary>
        /// GetSalePointsbyBranch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetSalePointsbyBranch(int branchId)
        {
            try
            {
                var salePoints = (from SalePoint salePoint in
                                      DelegateService.commonService.GetSalePointsByBranchId(branchId)
                                  select salePoint).OrderBy(c => c.Description);
                return new UifSelectResult(salePoints);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetBranchDescriptionById
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="user"></param>
        /// <returns>string</returns>
        public string GetBranchDescriptionById(int branchId, string user)
        {
            var branchs = DelegateService.uniqueUserService.GetBranchesByUserId(GetUserIdByName(user)).Where(sl => sl.Id == branchId).ToList();

            return branchs[0].Description;
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
            var branchs = DelegateService.uniqueUserService.GetBranchesByUserId(userId);

            if (type == 0)
            {
                return branchs[0].Id;
            }
            else
            {
                return branchs.Count;
            }
        }

        /// <summary>
        /// GetBranchesByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<Branch/></returns>
        public List<Branch> GetBranchesByUserId(int userId)
        {
            return DelegateService.uniqueUserService.GetBranchesByUserId(userId);
        }

        #endregion Branch

        #region Person

        /// <summary>
        /// GetPersonsByDocumentNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetPersonsByDocumentNumber(string query)
        {
            try
            {
                List<object> individuals = new List<object>();

                var persons = DelegateService.tempCommonService.GetPersonsByDocumentNumber(query);

                if (persons.Count > 0)
                {
                    foreach (var person in persons)
                    {
                        individuals.Add(new
                        {
                            DocumentName = person.DocumentNumber.Trim() + " : " + person.Name,
                            DocumentNumber = person.DocumentNumber.Trim(),
                            Name = person.Name,
                            IndividualId = person.IndividualId.ToString()
                        });
                    }
                    return Json(individuals, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    individuals.Add(new
                    {
                        DocumentName = Global.RegisterNotFound,
                        DocumentNumber = Global.RegisterNotFound,
                        IndividualId = 0
                    });
        
                    return Json(individuals, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Person

        #region ExchangeRate

        /// <summary>
        /// GetExchangeRateByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>ExchangeRate</returns>
        public ExchangeRate GetExchangeRateByCurrencyId(int currencyId)
        {
            try
            {
                return DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, currencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetExchangeByCurrency
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        public decimal GetExchangeByCurrency(int currencyId)
        {
            try
            {
                return System.Math.Round(DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, currencyId).SellAmount, 2);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion ExchangeRate

        #region AccountingCompany

        /// <summary>
        /// GetAccountingCompanies
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingCompanies()
        {
            try
            {
                var companies = (from AccountingCompanyDTO company in DelegateService.glAccountingApplicationService.GetAccountingCompanies() select company).OrderBy(c => c.AccountingCompanyId);
                return new UifSelectResult(companies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDefaultAccountingCompany
        /// </summary>
        /// <returns></returns>
        public int GetDefaultAccountingCompany()
        {
            int accountingCompanyId = 0;
            foreach (AccountingCompanyDTO accountingCompany in DelegateService.glAccountingApplicationService.GetAccountingCompanies())
            {
                if(accountingCompany.Default)
                {
                    accountingCompanyId = accountingCompany.AccountingCompanyId;
                    break;
                }
            }

            return accountingCompanyId;
        }

        #endregion AccountingCompany

        #region BankConciliation

        /// <summary>
        /// GetBankReconciliations
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankReconciliations()
        {
            try
            {
                return new UifSelectResult(DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion BankConciliation

        #region EntryDestination

        /// <summary>
        /// GetDestinations
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetDestinations()
        {
            try
            {
                var destinations = (from EntryDestinationDTO destination in DelegateService.glAccountingApplicationService.GetEntryDestinations() select destination).OrderBy(c => c.DestinationId);
                return new UifSelectResult(destinations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion EntryDestination

        #region AccountingMovementType

        /// <summary>
        /// GetManualAccountingMovementTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetManualAccountingMovementTypes()
        {
            try
            {
                // Ordenado por descripción
                var accountingMovementTypes = (from AccountingMovementTypeDTO accountingMovementType in DelegateService.glAccountingApplicationService.GetManualAccountingMovementTypes() select accountingMovementType).OrderBy(c => c.AccountingMovementTypeId);
                return new UifSelectResult(accountingMovementTypes);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingMovementTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingMovementTypes()
        {
            try
            {
                // Ordenado por descripción
                var accountingMovementTypes = (from AccountingMovementTypeDTO accountingMovementType in DelegateService.glAccountingApplicationService.GetAccountingMovementTypes() select accountingMovementType).OrderBy(c => c.AccountingMovementTypeId);
                return new UifSelectResult(accountingMovementTypes);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingMovementType

        #region AccountingNature

        /// <summary>
        /// GetNatures
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetNatures()
        {
            List<AccountingNaturesModel> accountingNatureModels = new List<AccountingNaturesModel>();
            
            foreach (AccountingNatures accountingNatureItem in Enum.GetValues(typeof(AccountingNatures)))
            {
                AccountingNaturesModel accountingNaturesModel = new AccountingNaturesModel();
                accountingNaturesModel.AccountingNatureId = (int)accountingNatureItem;
                accountingNaturesModel.Description = (int)accountingNatureItem == 1 ? Global.Credits : Global.Debits;

                accountingNatureModels.Add(accountingNaturesModel);
            }

            return new UifSelectResult(accountingNatureModels);
        }

        #endregion AccountingNature

        #region PostDates

        /// <summary>
        /// GetPostDatedTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPostDatedTypes()
        {
            List<PostDatedTypeModel> postdatedTypeModels = new List<PostDatedTypeModel>();
            
            foreach (PostDateTypes postDatedType in Enum.GetValues(typeof(PostDateTypes)))
            {
                PostDatedTypeModel postDatedTypeModel = new PostDatedTypeModel();
                postDatedTypeModel.Id = (int)postDatedType;
                postDatedTypeModel.Description = (int)postDatedType == Convert.ToInt32(PostDateTypes.Check) ? Global.Check : Global.Credits;

                postdatedTypeModels.Add(postDatedTypeModel);
            }

            return new UifSelectResult(postdatedTypeModels);
        }

        #endregion PostDates

        #region InformationAccountant

        /// <summary>
        /// GetInformationAccountant
        /// </summary>
        /// <param name="query"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetInformationAccountant(string query)
        {
            List<AccountingConceptModel.AccountingConceptDTO> concepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            try
            {
                if (query != String.Empty)
                {
                    concepts = DelegateService.glAccountingApplicationService.GetAccountingConcepts();
                }

                if (concepts.Count > 0)
                {
                    accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts
                                          where accountingConcept.AccountingAccount.AccountingAccountId != 0
                                          select accountingConcept).ToList();

                    accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Id.ToString().Contains(query)) select accountingConcept).ToList();

                    if (accountingConcepts.Count > 0)
                    {
                        foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts)
                        {
                            if (accountingConcept.AccountingAccount.Description == "" || accountingConcept.AccountingAccount.Description == null)
                            {
                                accountingConcept.AccountingAccount.Description = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Description;
                            }
                                
                            if (accountingConcept.AccountingAccount.Number == "" || accountingConcept.AccountingAccount.Number == null)
                            {
                                accountingConcept.AccountingAccount.Number = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Number;
                            }
                        }
                    }
                }
                if (concepts.Count == 0 || accountingConcepts.Count == 0)
                {
                    accountingConcepts.Add(new AccountingConceptModel.AccountingConceptDTO
                    {
                        Id = 0,
                        Description = Global.DataNotFound
                    });
                }
                
                return Json(accountingConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetInformationAccountantByDescrition
        /// </summary>
        /// <param name="query"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetInformationAccountantByDescrition(string query)
        {
            List<AccountingConceptModel.AccountingConceptDTO> concepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            try
            {
                if (query != String.Empty)
                {
                    concepts = DelegateService.glAccountingApplicationService.GetAccountingConcepts();
                }

                List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts
                                                                                     where accountingConcept.AccountingAccount.AccountingAccountId != 0
                                                                                     select accountingConcept).ToList();

                accountingConcepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts.Where(d => d.Description.ToString().ToUpper().Contains(query.ToUpper())) select accountingConcept).ToList();

                if (accountingConcepts.Count > 0)
                {
                    foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts)
                    {
                        if (accountingConcept.AccountingAccount.Description == "" || accountingConcept.AccountingAccount.Description == null)
                        {
                            accountingConcept.AccountingAccount.Description = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Description;
                        }
                            
                        if (accountingConcept.AccountingAccount.Number == "" || accountingConcept.AccountingAccount.Number == null)
                        {
                            accountingConcept.AccountingAccount.Number = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Number;
                        }
                    }
                }

                if (concepts.Count == 0 || accountingConcepts.Count == 0)
                {
                    accountingConcepts.Add(new AccountingConceptModel.AccountingConceptDTO
                    {
                        Id = 0,
                        Description = Global.DataNotFound
                    });
                }

                return Json(accountingConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryInformationAccountant
        /// </summary>
        /// <param name="query"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetEntryInformationAccountant(string query)
        {
            List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            List<AccountingConceptModel.AccountingConceptDTO> concepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            List<AccountingConceptModel.AccountingConceptDTO> accountings = new List<AccountingConceptModel.AccountingConceptDTO>();
            try
            {
                if (query != String.Empty)
                {
                    accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConcepts();
                }

                if (accountingConcepts.Count > 0)
                {
                    concepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountingConcepts
                                where accountingConcept.AccountingAccount.AccountingAccountId != 0
                                select accountingConcept).ToList();

                    concepts = (from AccountingConceptModel.AccountingConceptDTO newItem in concepts.Where(d => d.Id.ToString().Contains(query)) select newItem).ToList();

                    if (concepts.Count > 0)
                    {
                        foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts)
                        {
                            if (accountingConcept.AccountingAccount.Description == "" || accountingConcept.AccountingAccount.Description == null)
                            {
                                accountingConcept.AccountingAccount.Description = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Description;
                            }
                                
                            if (accountingConcept.AccountingAccount.Number == "" || accountingConcept.AccountingAccount.Number == null)
                            {
                                accountingConcept.AccountingAccount.Number = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Number;
                            }
                        }

                        foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts) 
                        {
                            if (DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).AccountingAccountApplication == (int)AccountingAccountApplications.Accounting)
                            {
                                accountings.Add(accountingConcept);
                            }
                        }
                    }                    
                }
                if (accountingConcepts.Count == 0 || concepts.Count == 0)
                {
                    accountings.Add(new AccountingConceptModel.AccountingConceptDTO
                    {
                        Id = 0,
                        Description = Global.DataNotFound
                    });
                }

                return Json(accountings, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetEntryInformationAccountantByDescrition
        /// </summary>
        /// <param name="query"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetEntryInformationAccountantByDescrition(string query)
        {
            List<AccountingConceptModel.AccountingConceptDTO> accountings = new List<AccountingConceptModel.AccountingConceptDTO>();
            List<AccountingConceptModel.AccountingConceptDTO> concepts = new List<AccountingConceptModel.AccountingConceptDTO>();
            List<AccountingConceptModel.AccountingConceptDTO> accountingConcepts = new List<AccountingConceptModel.AccountingConceptDTO>();

            try
            {
                if (query != String.Empty)
                {
                    accountings = DelegateService.glAccountingApplicationService.GetAccountingConcepts();
                }

                concepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in accountings
                           where accountingConcept.AccountingAccount.AccountingAccountId != 0
                           select accountingConcept).ToList();

                concepts = (from AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts.Where(d => d.Description.ToString().ToUpper().Contains(query.ToUpper())) select accountingConcept).ToList();

                if (concepts.Count > 0)
                {
                    foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts)
                    {
                        if (accountingConcept.AccountingAccount.Description == "" || accountingConcept.AccountingAccount.Description == null)
                        {
                            accountingConcept.AccountingAccount.Description = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Description;
                        }
                            
                        if (accountingConcept.AccountingAccount.Number == "" || accountingConcept.AccountingAccount.Number == null)
                        {
                            accountingConcept.AccountingAccount.Number = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).Number;
                        }
                    }

                    foreach (AccountingConceptModel.AccountingConceptDTO accountingConcept in concepts)
                    {
                        if (DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingConcept.AccountingAccount.AccountingAccountId).AccountingAccountApplication == (int)AccountingAccountApplications.Accounting)
                        {
                            accountingConcepts.Add(accountingConcept);
                        }
                    }
                }

                if (accountings.Count == 0 || concepts.Count == 0)
                {
                    accountingConcepts.Add(new AccountingConceptModel.AccountingConceptDTO
                    {
                        Id = 0,
                        Description = Global.DataNotFound
                    });
                }

                return Json(accountingConcepts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion InformationAccountant

        #region CostCenter

        /// <summary>
        /// SearchCostCenterById
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchCostCenterById(string query)
        {
            try
            {
                var jsonData = (from items in DelegateService.glAccountingApplicationService.GetCostCenters()
                                                                       .Where(d => d.CostCenterId.ToString()
                                                                       .Contains(query))
                                select new
                                {
                                    CostCenterId = items.CostCenterId.ToString(),
                                    Description = items.Description
                                }).ToList();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SearchCostCenterByName
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchCostCenterByName(string query)
        {
            try
            {
                if (!String.IsNullOrEmpty(query))
                {
                    var dataFiltered = DelegateService.glAccountingApplicationService.GetCostCenters().Where(d => d.Description.Contains(query.ToUpper()));
                    return Json(dataFiltered, JsonRequestBehavior.AllowGet);
                }
                return Json(DelegateService.glAccountingApplicationService.GetCostCenters(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetCostCenters
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCostCenters()
        {
            try
            {
                // Ordenado por descripción
                var costCenters = (from CostCenterDTO costCenter in DelegateService.glAccountingApplicationService.GetCostCenters() select costCenter).OrderBy(c => c.Description);

                return new UifSelectResult(costCenters);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion CostCenter

        #region CostCenterType

        /// <summary>
        /// GetCostCenterTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCostCenterTypes()
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetCostCenterTypes());
        }

        #endregion CostCenterType

        #region AnalysisCode

        /// <summary>
        /// GetAnalyses
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAnalyses()
        {
            try
            {
                // Ordenado por descripción
                var analysisCodes = (from AnalysisCodeDTO analysisCode in DelegateService.glAccountingApplicationService.GetAnalysisCodes() select analysisCode).OrderBy(c => c.AnalysisCodeId).ToList();
                return new UifSelectResult(analysisCodes);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AnalysisCode

        #region AnalysisConcept

        /// <summary>
        /// GetAnalysisConcepts
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAnalysisConcepts()
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetAnalysisConcepts());
        }

        #endregion AnalysisConcept

        #region PaymentConcepts

        /// <summary>
        /// GetPaymentConcepts
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentConcepts()
        {
            try
            {
                var accountingConcpets = (from AccountingConceptModel.AccountingConceptDTO accounitngConcept in DelegateService.glAccountingApplicationService.GetAccountingConcepts() select accounitngConcept).OrderBy(c => c.Description);

                return new UifSelectResult(accountingConcpets);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion PaymentConcepts

        #region Analysis

        /// <summary>
        /// GetAnalysis
        /// </summary>
        /// <param name="analisisId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysis(int analisisId)
        {
            try
            {
                var analyses = DelegateService.glAccountingApplicationService.GetAnalyses();
                AnalysisDTO analysis = new AnalysisDTO();
                foreach (var item in analyses)
                {
                    if (item.AnalysisConcept.AnalysisCode.AnalysisCodeId == analisisId)
                    {
                        analysis = item;
                    }
                }
                try
                {
                    analysis.AnalysisConcept = DelegateService.glAccountingApplicationService.GetAnalysisConcept(analysis.AnalysisConcept.AnalysisConceptId);
                }
                catch (Exception)
                {
                    analysis.AnalysisConcept = new AnalysisConceptDTO();
                    analysis.AnalysisConcept.AnalysisConceptId = 0;
                }
                return Json(analysis, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Analysis

        #region AnalysisConceptByCode

        /// <summary>
        /// GetPaymentConceptsByAnalysisCode
        /// </summary>
        /// <param name="analysisCodeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentConceptsByAnalysisCode(int analysisCodeId)
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetPaymentConceptsByAnalysisCode(analysisCodeId));
        }

        #endregion AnalysisConceptByCode

        #region AnalysisTreatment

        /// <summary>
        /// GetAnalysisTreatments
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAnalysisTreatments()
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetAnalysisTreatments());
        }

        #endregion AnalysisTreatment

        #region ConceptKey

        /// <summary>
        /// GetConceptKeys
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetConceptKeys()
        {
            return new UifSelectResult(null);
        }

        /// <summary>
        /// GetKeysByAnalysisAndConceptId
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        /// <returns></returns>
        public JsonResult GetKeysByAnalysisAndConceptId(int analysisId, int analysisConceptId)
        {
            try
            {
                List<AnalysisConceptKeyDTO> analysisConceptKeys = DelegateService.glAccountingApplicationService.GetAnalysisConceptKeysByAnalysisConcept(new AnalysisConceptDTO() { AnalysisConceptId = analysisConceptId });
                return Json(analysisConceptKeys, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion ConceptKey

        #region PaymentSource

        /// <summary>
        /// GetPaymentSources
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPaymentSources()
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetConceptSources());
        }

        #endregion PaymentSource

        #region AccountingAccount

        /// <summary>
        /// GetAccountingAccountsByNumberDescription
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountsByNumberDescription(string query)
        {
            AccountingAccountDTO accountingAccount = new AccountingAccountDTO();
            accountingAccount.Number = query;
            try
            {
                List<AccountingAccountDTO> accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountsByNumberDescription(accountingAccount);
                return Json(accountingAccounts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryAccountingAccountsByNumberDescription
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEntryAccountingAccountsByNumberDescription(string query)
        {
            try
            {
               List<AccountingAccountDTO> accountingAccounts = new List<AccountingAccountDTO>();

                accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccounts();
                accountingAccounts = (from AccountingAccountDTO accountingAccount in accountingAccounts
                                      where (accountingAccount.Number.Contains(query)) && (accountingAccount.AccountingAccountApplication == (int)AccountingAccountApplications.Accounting)
                                      select accountingAccount).ToList();

                return Json(accountingAccounts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccount
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccount(int accountingAccountId)
        {
            AccountingAccountDTO accountingAccount = new AccountingAccountDTO() { AccountingAccountId = Convert.ToInt32(accountingAccountId) };
            try
            {
                if (accountingAccount.AccountingAccountId > 0)
                {
                    accountingAccount = DelegateService.glAccountingApplicationService.GetAccountingAccount(accountingAccount.AccountingAccountId);
                }
            }
            catch (Exception)
            {
                accountingAccount = new AccountingAccountDTO();
            }

            return Json(accountingAccount, JsonRequestBehavior.AllowGet);
        }

        #endregion AccountingAccount

        #region Tax

        /// <summary>
        /// GetTaxes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTaxes()
        {
            List<Tax> taxes = DelegateService.taxService.GetTax();
            return new UifSelectResult(taxes);
        }

        #endregion Tax

        #region ModuleDates

        /// <summary>
        /// GetModuleDates
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetModuleDates()
        {
            try
            {
                // Ordenado por descripción
                var moduleDates = (from ModuleDate moduleDate in DelegateService.tempCommonService.GetModuleDates() select moduleDate).OrderBy(c => c.Description);

                return new UifSelectResult(moduleDates);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetMonths
        /// Carga los meses en DropDownlist
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetMonths()
        {
            CurrentCulture();
            var yearMonths = new List<object>();
            int monthNumber = 0;
            string monthDescription = "";

            monthNumber++;
            monthDescription = @Global.January;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.February;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.March;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.April;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.May;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.June;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.July;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.August;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.September;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.October;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.November;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = @Global.December;
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });


            return new UifSelectResult(yearMonths);
        }

        /// <summary>
        /// GetClosureMonth
        /// Obtiene el mes y año que se va a cerrar
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>Closing Date</returns>
        public JsonResult GetClosureMonth(int moduleId)
        {
            var closingDate = DelegateService.AccountingClosingService.GetClosingDate(moduleId);
            return Json(new { year = closingDate.Date.Year, month = closingDate.Date.Month, day = closingDate.Date.Day }, JsonRequestBehavior.AllowGet);
        }

        #endregion ModulesDates

        #region EntryConsultation

        /// <summary>
        /// GetCostCentersByEntryId
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="isDailyEntry"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetCostCentersByEntryId(int entryId, bool isDailyEntry)
        {
            List<CostCenterDTO> costCenters = DelegateService.glAccountingApplicationService.GetCostCentersByEntryId(entryId, isDailyEntry);
            List<CostCenterEntryModel> costCenterEntryModels = new List<CostCenterEntryModel>();

            foreach (CostCenterDTO costCenter in costCenters)
            {
                CostCenterEntryModel costCenterEntryModel = new CostCenterEntryModel();

                costCenterEntryModel.CostCenterId = costCenter.CostCenterId;
                costCenterEntryModel.Description = costCenter.Description;
                costCenterEntryModel.PercentageAmount = Convert.ToDecimal(costCenter.PercentageAmount);

                costCenterEntryModels.Add(costCenterEntryModel);
            }

            return new UifTableResult(costCenterEntryModels);
        }

        /// <summary>
        /// GetEntryAnalysesByEntryId
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="isDailyEntry"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetEntryAnalysesByEntryId(int entryId, bool isDailyEntry)
        {
            return new UifTableResult(DelegateService.glAccountingApplicationService.GetEntryAnalysesByEntryId(entryId, isDailyEntry));
        }

        /// <summary>
        /// GetPostdatedByEntryId
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="isDailyEntry"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetPostdatedByEntryId(int entryId, bool isDailyEntry)
        {
            List<PostDatedDTO> postDateds = DelegateService.glAccountingApplicationService.GetPostdatedByEntryId(entryId, isDailyEntry);
            List<PostDatedModel> postDatedModels = new List<PostDatedModel>();

            try
            {
                foreach (PostDatedDTO postDated in postDateds)
                {
                    PostDatedModel postDatedModel = new PostDatedModel();

                    postDatedModel.Id = postDated.PostDatedId;
                    postDatedModel.PostDateTypeId = Convert.ToInt32(postDated.PostDateType);
                    postDatedModel.PostDateTypeDescription = postDatedModel.PostDateTypeId == Convert.ToInt32(PostDateTypes.Check) ? Global.Check : Global.Credits;
                    postDatedModel.CurrencyId = postDated.Amount.Currency.Id;
                    postDatedModel.CurrencyDescription = GetCurrencyDescriptionById(postDated.Amount.Currency.Id);
                    postDatedModel.DocumentNumber = postDated.DocumentNumber;
                    postDatedModel.ExchangeRate = Convert.ToDecimal(postDated.ExchangeRate.SellAmount);
                    postDatedModel.LocalAmount = Convert.ToDecimal(postDated.LocalAmount.Value);
                    postDatedModel.IssueAmount = Convert.ToDecimal(postDated.Amount.Value);

                    postDatedModels.Add(postDatedModel);
                }

                return new UifTableResult(postDatedModels);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion EntryConsultation

        #region Date

        /// <summary>
        /// GetDate
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDate()
        {
            string dateToday = DateTime.Now.Date.ToShortDateString();
            return Json(dateToday, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingDate
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingDate(int moduleDateId)
        {
            return Json(Convert.ToString(DelegateService.AccountingClosingService.GetClosingDate(moduleDateId).ToString("dd/MM/yyyy")).Split()[0], JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingDateByModule
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public DateTime GetAccountingDateByModule(int module)
        {
            return DelegateService.AccountingClosingService.GetClosingDate(module);
        }

        #endregion Date

        #region Prefix

        /// <summary>
        /// GetPrefixes
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult GetPrefixes()
        {
            try
            {
                var prefixes = (from Prefix prefix in DelegateService.commonService.GetPrefixes() select prefix).OrderBy(c => c.Description);

                return new UifSelectResult(prefixes);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Prefix

        #region EntryNumber

        /// <summary>
        /// UpdateTransactionNumber
        /// </summary>
        /// <param name="number"></param>
        public void UpdateTransactionNumber(int number)
        {
            Parameter parameter = new Parameter();
            parameter.Id = Convert.ToInt32(ConfigurationManager.AppSettings["JournalEntryTransactionNumber"]); //id de parametro de número de transacción de Asiento Diario
            parameter.NumberParameter = number;
            parameter.Description = "Num Transacción Asiento Diario";
            parameter.NumberParameter = parameter.NumberParameter + 1;
            DelegateService.commonService.UpdateParameter(parameter);
        }

        #endregion EntryNumber

        #region BankMovement

        /// <summary>
        /// GetBankMovements
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankMovements()
        {
            try
            {
                var bankReconciliations = (from ReconciliationMovementTypeDTO bankReconciliation in DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes()
                                           select bankReconciliation).OrderBy(c => c.Description).ToList();

                return new UifSelectResult(bankReconciliations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion BankMovement

        #region AccountingAccountParent

        /// <summary>
        /// GetAccountingAccountParents
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingAccountParents()
        {
            try
            {
                return new UifSelectResult(DelegateService.glAccountingApplicationService.GetAccountingAccountParents());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingAccountParent

        #region User

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
        /// GetParameterMulticompany
        /// </summary>
        /// <returns>int</returns>
        public int GetParameterMulticompany()
        {
            //bool result = _imputationService.GetParameterMulticompany();descomentar hasta que añada la referencia 
            bool result = false;
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

        #endregion

    }
}