//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Grid;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;

// sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;


//Sistran Company
using Sistran.Core.Application.GeneralLedgerServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class CheckingAccountBrokersController : Controller
    {
        #region Views

        ///<summary>
        /// Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Actions

        ///<summary>
        /// SaveTempBrokersCheckingAccountRequest
        /// </summary>
        /// <param name="brokerCheckingAccount"></param>
        /// <param name="status"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempBrokersCheckingAccountRequest(BrokerCheckingAccountModel brokerCheckingAccount, int status)
        {
            bool isSaveBrokersCheckingAccount = false;
            int brokersCheckingAccountResponse = 0;
            int transactionNumber = 0;

            // Recupera fecha contable            
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            BrokersCheckingAccountTransactionDTO brokersCheckingAccountTransaction = new BrokersCheckingAccountTransactionDTO();

            brokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItemDTO>();

            if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems != null)
            {
                for (int i = 0; i < brokerCheckingAccount.BrokersCheckingAccountTransactionItems.Count; i++)
                {
                    if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokerCheckingAccountItemId == 0)
                    {
                        BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItemDTO();

                        brokersCheckingAccountTransactionItem.Amount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].CurrencyCode },
                            Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].Amount
                        };
                        brokersCheckingAccountTransactionItem.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                        {
                            SellAmount = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].ExchangeRate
                        };

                        if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AccountingNature == 1)
                        {
                            brokersCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            brokersCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }
                        brokersCheckingAccountTransactionItem.LocalAmount = new ACCDTO.AmountDTO();
                        brokersCheckingAccountTransactionItem.Branch = new SCRDTO.BranchDTO() { Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BranchId };
                        brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConceptDTO();
                        brokersCheckingAccountTransactionItem.CheckingAccountConcept.Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].CheckingAccountConceptId;
                        brokersCheckingAccountTransactionItem.Comments = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].Description;
                        brokersCheckingAccountTransactionItem.Company = new ACCDTO.CompanyDTO()
                        {
                            IndividualId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AccountingCompanyId
                        };
                        brokersCheckingAccountTransactionItem.SalePoint = new ACCDTO.SalePointDTO() { Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].SalePointId };
                        brokersCheckingAccountTransactionItem.PolicyId = 0;
                        brokersCheckingAccountTransactionItem.PrefixId = 0;
                        brokersCheckingAccountTransactionItem.InsuredId = 0;
                        brokersCheckingAccountTransactionItem.CommissionType = 0;
                        brokersCheckingAccountTransactionItem.CommissionAmount = new ACCDTO.AmountDTO() { Value = 0 };
                        brokersCheckingAccountTransactionItem.CommissionBalance = new ACCDTO.AmountDTO() { Value = 0 };
                        brokersCheckingAccountTransactionItem.CommissionPercentage = new ACCDTO.AmountDTO() { Value = 0 };
                        brokersCheckingAccountTransactionItem.DiscountedCommission = new ACCDTO.AmountDTO() { Value = 0 };

                        brokersCheckingAccountTransactionItem.Holder = new ACCDTO.AgentDTO()
                        {
                            IndividualId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AgentId,
                            FullName = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AgentTypeId.ToString(),
                            Agencies = new List<ACCDTO.AgencyDTO>()
                            {
                                new ACCDTO.AgencyDTO { Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AgentAgencyId }
                            }
                        };
                        brokersCheckingAccountTransactionItem.Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokerCheckingAccountItemId;

                        // Items a aplicar
                        brokersCheckingAccountTransactionItem.BrokersCheckingAccountTransactionChild = new List<BrokersCheckingAccountTransactionItemDTO>();
                        brokersCheckingAccountTransactionItem.BrokersCheckingAccountItems = new List<BrokerCheckingAccountItemDTO>();

                        if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild != null)
                        {
                            transactionNumber = 1;

                            for (int k = 0; k < brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild.Count; k++)
                            {
                                if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BrokerCheckingAccountItemId == 0)
                                {
                                    BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItemChild = new BrokersCheckingAccountTransactionItemDTO();

                                    brokersCheckingAccountTransactionItemChild.Amount = new ACCDTO.AmountDTO()
                                    {
                                        Currency = new SCRDTO.CurrencyDTO()
                                        {
                                            Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].CurrencyCode
                                        },
                                        Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].Amount
                                    };
                                    brokersCheckingAccountTransactionItemChild.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                                    {
                                        SellAmount = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].ExchangeRate
                                    };
                                    brokersCheckingAccountTransactionItemChild.LocalAmount = new ACCDTO.AmountDTO();

                                    if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].AccountingNature == 1)
                                    {
                                        brokersCheckingAccountTransactionItemChild.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                                    }
                                    else
                                    {
                                        brokersCheckingAccountTransactionItemChild.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                                    }
                                    brokersCheckingAccountTransactionItemChild.Branch = new SCRDTO.BranchDTO()
                                    {
                                        Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BranchId
                                    };
                                    brokersCheckingAccountTransactionItemChild.CheckingAccountConcept = new CheckingAccountConceptDTO()
                                    {
                                        Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].CheckingAccountConceptId
                                    };
                                    brokersCheckingAccountTransactionItemChild.Comments = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].Description;
                                    brokersCheckingAccountTransactionItemChild.Company = new ACCDTO.CompanyDTO()
                                    {
                                        IndividualId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].AccountingCompanyId
                                    };
                                    brokersCheckingAccountTransactionItemChild.SalePoint = new ACCDTO.SalePointDTO()
                                    {
                                        Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].SalePointId
                                    };
                                    brokersCheckingAccountTransactionItemChild.PolicyId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].PolicyId;
                                    brokersCheckingAccountTransactionItemChild.PrefixId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].PrefixId;
                                    brokersCheckingAccountTransactionItemChild.InsuredId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].InsuredId;
                                    brokersCheckingAccountTransactionItemChild.CommissionType = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].CommissionType;
                                    brokersCheckingAccountTransactionItemChild.CommissionAmount = new ACCDTO.AmountDTO()
                                    {
                                        Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].CommissionAmount
                                    };
                                    brokersCheckingAccountTransactionItemChild.CommissionBalance = new ACCDTO.AmountDTO()
                                    {
                                        Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].CommissionBalance
                                    };
                                    brokersCheckingAccountTransactionItemChild.CommissionPercentage = new ACCDTO.AmountDTO()
                                    {
                                        Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].CommissionPercentage
                                    };
                                    brokersCheckingAccountTransactionItemChild.DiscountedCommission = new ACCDTO.AmountDTO()
                                    {
                                        Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].DiscountedCommission
                                    };

                                    brokersCheckingAccountTransactionItemChild.Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BrokerCheckingAccountItemId;
                                    brokersCheckingAccountTransactionItem.BrokersCheckingAccountTransactionChild.Add(brokersCheckingAccountTransactionItemChild);

                                    BrokerCheckingAccountItemDTO brokerCheckingAccountItemChild = new BrokerCheckingAccountItemDTO()
                                    {
                                        BrokerCheckingAccountId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BrokerCheckingAccountId,
                                        Id = 0,
                                        TempBrokerCheckingAccountId = 0,
                                    };

                                    brokersCheckingAccountTransactionItem.BrokersCheckingAccountItems.Add(brokerCheckingAccountItemChild);
                                }
                            }
                        }
                        else
                        {
                            transactionNumber = 0;
                        }

                        brokersCheckingAccountTransactionItem.InsuredId = transactionNumber;

                        brokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems.Add(brokersCheckingAccountTransactionItem);
                    }
                    else
                    {
                        BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItemDTO();

                        brokersCheckingAccountTransactionItem.Amount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].CurrencyCode },
                            Value = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].Amount
                        };
                        if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AccountingNature == 1)
                        {
                            brokersCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            brokersCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }
                        brokersCheckingAccountTransactionItem.LocalAmount = new ACCDTO.AmountDTO();
                        brokersCheckingAccountTransactionItem.Branch = new SCRDTO.BranchDTO()
                        {
                            Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BranchId
                        };
                        brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConceptDTO()
                        {
                            Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].CheckingAccountConceptId
                        };
                        brokersCheckingAccountTransactionItem.Comments = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].Description;
                        brokersCheckingAccountTransactionItem.Company = new ACCDTO.CompanyDTO()
                        {
                            IndividualId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].AccountingCompanyId
                        };
                        brokersCheckingAccountTransactionItem.SalePoint = new ACCDTO.SalePointDTO()
                        {
                            Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].SalePointId
                        };

                        brokersCheckingAccountTransactionItem.Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokerCheckingAccountItemId;

                        brokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems.Add(brokersCheckingAccountTransactionItem);
                    }
                }
                isSaveBrokersCheckingAccount = DelegateService.accountingApplicationService.SaveBrokersCheckingAccount(brokersCheckingAccountTransaction, brokerCheckingAccount.ImputationId, accountingDate /*DateTime.Now*/);
            }

            if (isSaveBrokersCheckingAccount)
            {
                brokersCheckingAccountResponse = 1;
            }
            else
            {
                brokersCheckingAccountResponse = 0;
            }

            return Json(brokersCheckingAccountResponse, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// DeleteTempBrokersCheckingAccountItem
        /// </summary>
        /// <param name="tempImputationCode"></param>
        /// <param name="tempBrokersCheckingAccountCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempBrokersCheckingAccountItem(int tempImputationCode, int tempBrokersCheckingAccountCode)
        {
            bool isDeletedBrokersCheckingAccountItem = DelegateService.accountingApplicationService.DeleteBrokersCheckingAccountItem(tempImputationCode, tempBrokersCheckingAccountCode);

            return Json(isDeletedBrokersCheckingAccountItem, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateTempBrokerCheckingAccountTotal
        /// </summary>
        /// <param name="tempBrokerCheckingAccountCode"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateTempBrokerCheckingAccountTotal(int tempBrokerCheckingAccountCode, decimal selectedTotal)
        {
            DelegateService.accountingApplicationService.UpdateTempBrokersCheckingAccountTotal(tempBrokerCheckingAccountCode, selectedTotal);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteTempBrokerCheckingAccountItemChild
        /// </summary>
        /// <param name="tempBrokerCheckingAccountItemCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempBrokerCheckingAccountItemChild(int tempBrokerCheckingAccountItemCode)
        {
            bool isDeletedBrokerCheckingAccountItemChild = DelegateService.accountingApplicationService.DeleteBrokerCheckingAccountItemChild(tempBrokerCheckingAccountItemCode);

            return Json(isDeletedBrokerCheckingAccountItemChild, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetTempBrokerCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempBrokerCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            List<object> brokerCheckingAccounts = new List<object>();

            if (tempImputationId > 0)
            {
                List<SCRDTO.BrokerCheckingAccountItemDTO> brokerCheckingAccountItemDTOs =
                    DelegateService.accountingApplicationService.GetTempBrokerCheckingAccountItemByTempApplicationId(tempImputationId);

                foreach (SCRDTO.BrokerCheckingAccountItemDTO brokerCheckingAccount in brokerCheckingAccountItemDTOs)
                {
                    brokerCheckingAccounts.Add(new
                    {
                        BranchName = brokerCheckingAccount.BranchName,
                        SalePointName = brokerCheckingAccount.PosName,
                        CompanyName = brokerCheckingAccount.CompanyName,
                        AgentName = brokerCheckingAccount.AgentName,
                        ConceptName = brokerCheckingAccount.ConceptName,
                        NatureName = brokerCheckingAccount.DebitCreditName,
                        CurrencyName = brokerCheckingAccount.CurrencyName,
                        LocalAmount = brokerCheckingAccount.IncomeAmount,
                        ExchangeRate = brokerCheckingAccount.CurrencyChange,
                        Description = brokerCheckingAccount.Description,
                        BranchId = brokerCheckingAccount.BranchCode,
                        SalePointId = brokerCheckingAccount.PosCode,
                        CompanyId = brokerCheckingAccount.CompanyCode,
                        NatureId = brokerCheckingAccount.DebitCreditCode,
                        CurrencyId = brokerCheckingAccount.CurrencyCode,
                        BrokerCheckingAccountItemId = brokerCheckingAccount.BrokerCheckingAccountItemId,
                        AgentTypeCode = brokerCheckingAccount.AgentTypeCode,
                        AgentId = brokerCheckingAccount.AgentCode,
                        AgentDocumentNumber = brokerCheckingAccount.AgentDocumentNumber,
                        AgentAgencyCode = brokerCheckingAccount.AgentAgencyCode,
                        AccountingNature = brokerCheckingAccount.AccountNature,
                        ConceptId = brokerCheckingAccount.CheckingAccountConceptCode,
                        Amount = brokerCheckingAccount.Amount,
                        BillNumber = brokerCheckingAccount.CollectNumber,
                        TempBrokerParentId = brokerCheckingAccount.TempBrokerParentId,
                        Items = brokerCheckingAccount.Items,
                        Status = brokerCheckingAccount.Status
                    });
                }
            }

            return Json(brokerCheckingAccounts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetTempBrokerCheckingAccountItemChildByTempImputationId
        /// </summary>
        /// <param name="tempBrokerParentId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="grid"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempBrokerCheckingAccountItemChildByTempImputationId(int tempBrokerParentId, int tempImputationId, GridSettings grid)
        {
            List<SCRDTO.BrokerCheckingAccountItemDTO> brokerCheckingAccountItemDTOs =
                DelegateService.accountingApplicationService.GetTempBrokerCheckingAccountItemChildByTempBrokerParentId(tempBrokerParentId);

            var brokerCheckingAccountItems = new
            {

                total = brokerCheckingAccountItemDTOs.Count,
                records = brokerCheckingAccountItemDTOs,
                rows = brokerCheckingAccountItemDTOs
            };

            return Json(brokerCheckingAccountItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SearchBrokersCheckingAccount
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNum"></param>
        /// <param name="currencyId"></param>
        /// <param name="insuredId"></param>
        /// <param name="grid"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchBrokersCheckingAccount(int agentId, int branchId, int salePointId, int prefixId,
                                                       int policyNum, int currencyId, int insuredId, GridSettings grid)
        {
            SCRDTO.SearchParameterBrokersCheckingAccountDTO searchParameterBrokersCheckingAccount = new SCRDTO.SearchParameterBrokersCheckingAccountDTO();

            searchParameterBrokersCheckingAccount.AgentId = agentId;
            searchParameterBrokersCheckingAccount.Branch = new SCRDTO.BranchDTO();
            searchParameterBrokersCheckingAccount.Branch.Id = branchId;
            searchParameterBrokersCheckingAccount.Currency = new SCRDTO.CurrencyDTO();
            searchParameterBrokersCheckingAccount.Currency.Id = currencyId;
            searchParameterBrokersCheckingAccount.InsuredDocumentNumber = insuredId.ToString();
            searchParameterBrokersCheckingAccount.InsuredFullName = insuredId.ToString();
            searchParameterBrokersCheckingAccount.PolicyNumber = policyNum;
            searchParameterBrokersCheckingAccount.Prefix = new ACCDTO.PrefixDTO();
            searchParameterBrokersCheckingAccount.Prefix.Id = prefixId;
            searchParameterBrokersCheckingAccount.SalePoint = new ACCDTO.SalePointDTO();
            searchParameterBrokersCheckingAccount.SalePoint.Id = salePointId;

            List<SCRDTO.SearchAgentsItemsDTO> searchAgentsItemDTOs =
                DelegateService.accountingApplicationService.SearchBrokersCheckingAccount(searchParameterBrokersCheckingAccount);

            var searchAgentsItems = new
            {
                page = searchAgentsItemDTOs,//.Page,
                total = searchAgentsItemDTOs.Count,
                records = searchAgentsItemDTOs,//.Records,
                rows = searchAgentsItemDTOs//.Rows
            };

            return Json(searchAgentsItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveBrokerCheckingAccountItem
        /// </summary>
        /// <param name="brokerCheckingAccount"></param>
        /// <param name="status"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBrokerCheckingAccountItem(BrokerCheckingAccountModel brokerCheckingAccount, int status)
        {
            bool isSavedBrokersCheckingAccountItem = false;
            int brokersCheckingAccountItemResponse = 0;
            int transactionNumber = 0;

            BrokersCheckingAccountTransactionItemDTO brokerCheckingAccountTransaction = new BrokersCheckingAccountTransactionItemDTO();

            List<BrokerCheckingAccountItemDTO> brokerCheckingAccountItems = new List<BrokerCheckingAccountItemDTO>();

            brokerCheckingAccountTransaction.BrokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItemDTO>();

            if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems != null)
            {
                for (int i = 0; i < brokerCheckingAccount.BrokersCheckingAccountTransactionItems.Count; i++)
                {
                    if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokerCheckingAccountItemId > 0)
                    {
                        BrokersCheckingAccountTransactionItemDTO brokerCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItemDTO();

                        brokerCheckingAccountTransactionItem.Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokerCheckingAccountItemId;

                        // Items a aplicar
                        brokerCheckingAccountTransactionItem.BrokersCheckingAccountTransactionChild = new List<BrokersCheckingAccountTransactionItemDTO>();
                        brokerCheckingAccountTransactionItem.BrokersCheckingAccountItems = new List<BrokerCheckingAccountItemDTO>();

                        if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild != null)
                        {
                            transactionNumber = 1;

                            for (int k = 0; k < brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild.Count; k++)
                            {
                                if (brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BrokerCheckingAccountItemId > 0)
                                {
                                    BrokersCheckingAccountTransactionItemDTO brokerCheckingAccountTransactionItemChild = new BrokersCheckingAccountTransactionItemDTO();

                                    brokerCheckingAccountTransactionItemChild.Id = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BrokerCheckingAccountItemId;
                                    brokerCheckingAccountTransactionItem.BrokersCheckingAccountTransactionChild.Add(brokerCheckingAccountTransactionItemChild);

                                    BrokerCheckingAccountItemDTO brokerCheckingAccountItemChild = new BrokerCheckingAccountItemDTO();
                                    brokerCheckingAccountItemChild.Id = 0;
                                    brokerCheckingAccountItemChild.TempBrokerCheckingAccountId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokerCheckingAccountItemId;
                                    brokerCheckingAccountItemChild.BrokerCheckingAccountId = brokerCheckingAccount.BrokersCheckingAccountTransactionItems[i].BrokersCheckingAccountTransactionChild[k].BrokerCheckingAccountId;

                                    brokerCheckingAccountTransactionItem.BrokersCheckingAccountItems.Add(brokerCheckingAccountItemChild);

                                    brokerCheckingAccountItems.Add(brokerCheckingAccountItemChild);
                                }
                            }
                        }
                        else
                            transactionNumber = 0;

                        brokerCheckingAccountTransactionItem.InsuredId = transactionNumber;

                        brokerCheckingAccountTransaction.BrokersCheckingAccountTransactionItems.Add(brokerCheckingAccountTransactionItem);
                    }
                }

                isSavedBrokersCheckingAccountItem = DelegateService.accountingApplicationService.SaveBrokersCheckingAccountItem(brokerCheckingAccountItems);
            }

            if (isSavedBrokersCheckingAccountItem)
            {
                brokersCheckingAccountItemResponse = 1;
            }
            else
            {
                brokersCheckingAccountItemResponse = 0;
            }

            return Json(brokersCheckingAccountItemResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CurrencyDiference

        /// <summary>
        /// GetCurrencyDiferenceByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>double</returns>
        public double GetCurrencyDiferenceByCurrencyId(int currencyId)
        {
            List<SCRDTO.CurrencyDiferenceDTO> currencyDifferences = DelegateService.accountingApplicationService.GetCurrencyDiferenceByCurrencyId(currencyId);

            double percentageDiference = 0;

            if (currencyDifferences.Count > 0)
            {
                percentageDiference = currencyDifferences[0].PercentageDiference;
            }

            return percentageDiference;
        }

        /// <summary>
        /// GetPercentageDifferenceByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        public decimal GetPercentageDifferenceByCurrencyId(int currencyId)
        {
            return DelegateService.accountingApplicationService.GetPercentageDifferenceByCurrencyId(currencyId);
        }

        #endregion

        #region SearchAgentsItems

        /// <summary>
        /// ValidateDuplicateBrokerCheckingAccount
        /// Permite validar el ingreso de duplicados tanto en temporales como en reales de cuenta corriente agentes
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="companyId"></param>
        /// <param name="agentId"></param>
        /// <param name="accountingNatureId"></param>
        /// <param name="checkingAccountConceptId"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateDuplicateBrokerCheckingAccount(int branchId, int salePointId, int companyId, int agentId,
                                                                 int accountingNatureId, int checkingAccountConceptId, int currencyId)
        {
            SCRDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameterBrokerCoinsuranceReinsurance = new SCRDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO();
            validateParameterBrokerCoinsuranceReinsurance.AccountingNatureId = accountingNatureId;
            validateParameterBrokerCoinsuranceReinsurance.AgentId = agentId;
            validateParameterBrokerCoinsuranceReinsurance.Branch = new SCRDTO.BranchDTO() { Id = branchId };
            validateParameterBrokerCoinsuranceReinsurance.CheckingAccountConceptId = checkingAccountConceptId;
            validateParameterBrokerCoinsuranceReinsurance.Company = new ACCDTO.CompanyDTO() { IndividualId = companyId };
            validateParameterBrokerCoinsuranceReinsurance.Currency = new SCRDTO.CurrencyDTO() { Id = currencyId };
            validateParameterBrokerCoinsuranceReinsurance.SalePoint = new ACCDTO.SalePointDTO() { Id = salePointId };

            ApplicationDTO application = DelegateService.accountingApplicationService.ValidateDuplicateBrokerCheckingAccount(validateParameterBrokerCoinsuranceReinsurance);

            List<object> brokerCheckingAccounts = new List<object>();

            brokerCheckingAccounts.Add(new
            {
                source = application.VerificationValue.Value,
                imputationId = application.Id,
                type = application.ModuleId, //PL O OP
                isReal = application.IsTemporal
            });

            return Json(brokerCheckingAccounts, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}