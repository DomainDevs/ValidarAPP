//System
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using System;
using System.Linq;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.Exceptions;

// sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using TempCommonDTOs = Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class CheckingAccountReinsurancesController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 0;
        public const string SortOrder = "ASC";

        #endregion

        #region Views

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Actions

        /// <summary>
        /// ValidateDuplicateReinsuranceCheckingAccount
        /// Permite validar el ingreso de duplicados tanto en temporales como en reales en las tablas: BILL.TEMP_REINSURANCE_CHECKING_ACCOUNT
        /// BILL.REINSURANCE_CHECKING_ACCOUNT
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="companyId"></param>
        /// <param name="reinsuranceId"></param>
        /// <param name="accountingNatureId"></param>
        /// <param name="checkingAccountConceptId"></param>
        /// <param name="currencyId"></param>
        /// <param name="agentId"></param>
        /// <param name="prefixId"></param>
        /// <param name="subprefixId"></param>
        /// <param name="contractTypeId"></param>
        /// <param name="contractNumber"></param>
        /// <param name="sectionId"></param>
        /// <param name="applicationYear"></param>
        /// <param name="applicationMonth"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateDuplicateReinsuranceCheckingAccount(int branchId, int salePointId, int companyId, int reinsuranceId,
                                                                      int accountingNatureId, int checkingAccountConceptId, int currencyId,
                                                                      int agentId, int prefixId, int subprefixId, int contractTypeId,
                                                                      string contractNumber, int sectionId, int applicationYear, int applicationMonth)
        {
            SCRDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameterBrokerCoinsuranceReinsurance = new SCRDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO();
            validateParameterBrokerCoinsuranceReinsurance.AccountingNatureId = accountingNatureId;
            validateParameterBrokerCoinsuranceReinsurance.AgentId = agentId;
            validateParameterBrokerCoinsuranceReinsurance.ApplicationMonth = applicationMonth;
            validateParameterBrokerCoinsuranceReinsurance.ApplicationYear = applicationYear;
            validateParameterBrokerCoinsuranceReinsurance.Branch = new SCRDTO.BranchDTO() { Id = branchId };
            validateParameterBrokerCoinsuranceReinsurance.CheckingAccountConceptId = checkingAccountConceptId;
            validateParameterBrokerCoinsuranceReinsurance.Company = new CompanyDTO() { IndividualId = companyId };
            validateParameterBrokerCoinsuranceReinsurance.ContractNumber = contractNumber;
            validateParameterBrokerCoinsuranceReinsurance.ContractTypeId = contractTypeId;
            validateParameterBrokerCoinsuranceReinsurance.Currency = new SCRDTO.CurrencyDTO() { Id = currencyId };
            validateParameterBrokerCoinsuranceReinsurance.Prefix = new LineBusinessDTO() { Id = prefixId };
            validateParameterBrokerCoinsuranceReinsurance.ReinsuranceId = reinsuranceId;
            validateParameterBrokerCoinsuranceReinsurance.SalePoint = new ACCDTO.SalePointDTO() { Id = salePointId };
            validateParameterBrokerCoinsuranceReinsurance.StretchId = sectionId;
            validateParameterBrokerCoinsuranceReinsurance.SubPrefix = new SubLineBusinessDTO() { Id = subprefixId };

            ApplicationDTO application = DelegateService.accountingApplicationService.ValidateDuplicateReinsuranceCheckingAccount(validateParameterBrokerCoinsuranceReinsurance);

            List<object> reinsuranceCheckingAccounts = new List<object>();

            reinsuranceCheckingAccounts.Add(new
            {
                source = application.VerificationValue.Value,
                imputationId = application.Id
            });

            return Json(reinsuranceCheckingAccounts, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// SaveTempReinsuranceCheckingAccountRequest
        /// </summary>
        /// <param name="reinsuranceCheckingAccount"></param>
        /// <param name="status"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempReinsuranceCheckingAccountRequest(ReinsuranceCheckingAccountModel reinsuranceCheckingAccount, int status)
        {
            bool isSavedReinsuranceCheckingAccount = false;
            int reinsuranceCheckingAccountResponse = 0;
            int transactionNumber = 0;

            ReInsuranceCheckingAccountTransactionDTO reinsuranceCheckingAccountTransaction = new ReInsuranceCheckingAccountTransactionDTO();

            reinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems = new List<ReInsuranceCheckingAccountTransactionItemDTO>();

            if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems != null)
            {
                for (int i = 0; i < reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems.Count; i++)
                {
                    if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountItemId == 0)
                    {
                        ReInsuranceCheckingAccountTransactionItemDTO reinsuranceCheckingAccountTransactionItem = new ReInsuranceCheckingAccountTransactionItemDTO();

                        reinsuranceCheckingAccountTransactionItem.Amount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].CurrencyCode },
                            Value = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].Amount
                        };
                        reinsuranceCheckingAccountTransactionItem.LocalAmount = new ACCDTO.AmountDTO();
                        reinsuranceCheckingAccountTransactionItem.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                        {
                            BuyAmount = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ExchangeRate
                        };

                        if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].AccountingNature == 1)
                        {
                            reinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            reinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }
                        reinsuranceCheckingAccountTransactionItem.Branch = new SCRDTO.BranchDTO()
                        {
                            Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].BranchId
                        };
                        reinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConceptDTO()
                        {
                            Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].CheckingAccountConceptId
                        };
                        reinsuranceCheckingAccountTransactionItem.Comments = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].Description;
                        reinsuranceCheckingAccountTransactionItem.Company = new CompanyDTO()
                        {
                            IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].AccountingCompanyId
                        };
                        reinsuranceCheckingAccountTransactionItem.SalePoint = new ACCDTO.SalePointDTO()
                        {
                            Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].SalePointId
                        };
                        reinsuranceCheckingAccountTransactionItem.PolicyId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].PolicyId;
                        reinsuranceCheckingAccountTransactionItem.EndorsementId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].EndorsementId;
                        reinsuranceCheckingAccountTransactionItem.Holder = new CompanyDTO()
                        {
                            IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCompanyId
                        };
                        reinsuranceCheckingAccountTransactionItem.Broker = new CompanyDTO()
                        {
                            IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].AgentId
                        };
                        reinsuranceCheckingAccountTransactionItem.Prefix = new ACCDTO.PrefixDTO()
                        {
                            Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].LineBusinessId,
                            LineBusinessId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].SubLineBusinessId
                        };
                        reinsuranceCheckingAccountTransactionItem.IsFacultative = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].IsFacultative != 1;
                        reinsuranceCheckingAccountTransactionItem.Year = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ApplicationYear;
                        reinsuranceCheckingAccountTransactionItem.Month = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ApplicationMonth;
                        reinsuranceCheckingAccountTransactionItem.ContractTypeId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ContractTypeId;
                        reinsuranceCheckingAccountTransactionItem.ContractNumber = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ContractNumber;
                        reinsuranceCheckingAccountTransactionItem.Period = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].Period;
                        // El campo region hace referencia tipo de compania (en este caso se coloca default COMPAÑÍA REASEGURADORA NACIONAL)
                        // se valida con Rommel. Para que funcione ACE-1502                       
                         reinsuranceCheckingAccountTransactionItem.Region = ConfigurationManager.AppSettings["ReinsuranceAutocomplete"];

                        reinsuranceCheckingAccountTransactionItem.Section = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].Section;
                        reinsuranceCheckingAccountTransactionItem.SlipNumber = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].SlipNumber;

                        reinsuranceCheckingAccountTransactionItem.Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountItemId;

                        reinsuranceCheckingAccountTransactionItem.PolicyId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].PolicyId;
                        reinsuranceCheckingAccountTransactionItem.EndorsementId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].EndorsementId;



                        // Items a aplicar
                        reinsuranceCheckingAccountTransactionItem.ReInsuranceCheckingAccountTransactionChild = new List<ReInsuranceCheckingAccountTransactionItemDTO>();
                        reinsuranceCheckingAccountTransactionItem.ReinsurancesCheckingAccountItems = new List<ReinsuranceCheckingAccountItemDTO>();

                        if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild != null)
                        {
                            transactionNumber = 1;

                            for (int k = 0; k < reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild.Count; k++)
                            {
                                if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ReinsuranceCheckingAccountItemId == 0)
                                {
                                    ReInsuranceCheckingAccountTransactionItemDTO reinsuranceCheckingAccountTransactionItemChild = new ReInsuranceCheckingAccountTransactionItemDTO();

                                    reinsuranceCheckingAccountTransactionItemChild.Amount = new ACCDTO.AmountDTO()
                                    {
                                        Currency = new SCRDTO.CurrencyDTO() { Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].CurrencyCode },
                                        Value = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].Amount
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                                    {
                                        BuyAmount = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ExchangeRate
                                    };

                                    if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].AccountingNature == 1)
                                    {
                                        reinsuranceCheckingAccountTransactionItemChild.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                                    }
                                    else
                                    {
                                        reinsuranceCheckingAccountTransactionItemChild.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                                    }
                                    reinsuranceCheckingAccountTransactionItemChild.LocalAmount = new ACCDTO.AmountDTO();
                                    reinsuranceCheckingAccountTransactionItemChild.Branch = new SCRDTO.BranchDTO()
                                    {
                                        Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].BranchId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.CheckingAccountConcept = new CheckingAccountConceptDTO()
                                    {
                                        Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].CheckingAccountConceptId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.Comments = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].Description;
                                    reinsuranceCheckingAccountTransactionItemChild.Company = new CompanyDTO()
                                    {
                                        IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].AccountingCompanyId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.SalePoint = new ACCDTO.SalePointDTO()
                                    {
                                        Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].SalePointId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.PolicyId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].PolicyId;
                                    reinsuranceCheckingAccountTransactionItemChild.EndorsementId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].EndorsementId;
                                    reinsuranceCheckingAccountTransactionItemChild.Holder = new CompanyDTO()
                                    {
                                        IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ReinsuranceCompanyId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.Broker = new CompanyDTO()
                                    {
                                        IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].AgentId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.Prefix = new ACCDTO.PrefixDTO()
                                    {
                                        Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].LineBusinessId,
                                        LineBusinessId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].SubLineBusinessId
                                    };
                                    reinsuranceCheckingAccountTransactionItemChild.IsFacultative = (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].IsFacultative == 1);
                                    reinsuranceCheckingAccountTransactionItemChild.ContractTypeId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ContractTypeId;
                                    reinsuranceCheckingAccountTransactionItemChild.ContractNumber = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ContractNumber;
                                    reinsuranceCheckingAccountTransactionItemChild.Month = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ApplicationMonth;
                                    reinsuranceCheckingAccountTransactionItemChild.Period = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].Period;
                                    reinsuranceCheckingAccountTransactionItemChild.Region = ConfigurationManager.AppSettings["ReinsuranceAutocomplete"];
                                    reinsuranceCheckingAccountTransactionItemChild.Section = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].Section;
                                    reinsuranceCheckingAccountTransactionItemChild.SlipNumber = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].SlipNumber;
                                    reinsuranceCheckingAccountTransactionItemChild.Year = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ApplicationYear;

                                    reinsuranceCheckingAccountTransactionItemChild.Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ReinsuranceCheckingAccountItemId;
                                    reinsuranceCheckingAccountTransactionItem.ReInsuranceCheckingAccountTransactionChild.Add(reinsuranceCheckingAccountTransactionItemChild);

                                    ReinsuranceCheckingAccountItemDTO reinsuranceCheckingAccountItemChild = new ReinsuranceCheckingAccountItemDTO();
                                    reinsuranceCheckingAccountItemChild.Id = 0;
                                    reinsuranceCheckingAccountItemChild.TempReinsuranceCheckingAccountId = 0;
                                    reinsuranceCheckingAccountItemChild.ReinsuranceCheckingAccountId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountTransactionChild[k].ReinsuranceCheckingAccountId;

                                    reinsuranceCheckingAccountTransactionItem.ReinsurancesCheckingAccountItems.Add(reinsuranceCheckingAccountItemChild);
                                }
                            }
                        }
                        else
                        {
                            transactionNumber = 0;
                        }

                        reinsuranceCheckingAccountTransactionItem.TransactionNumber = transactionNumber;

                        reinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems.Add(reinsuranceCheckingAccountTransactionItem);
                    }
                    else
                    {
                        ReInsuranceCheckingAccountTransactionItemDTO reinsuranceCheckingAccountTransactionItem = new ReInsuranceCheckingAccountTransactionItemDTO();

                        reinsuranceCheckingAccountTransactionItem.Amount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].CurrencyCode },
                            Value = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].Amount
                        };
                        if (reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].AccountingNature == 1)
                        {
                            reinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            reinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }
                        reinsuranceCheckingAccountTransactionItem.LocalAmount = new ACCDTO.AmountDTO();
                        reinsuranceCheckingAccountTransactionItem.Branch = new SCRDTO.BranchDTO()
                        {
                            Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].BranchId
                        };
                        reinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConceptDTO()
                        {
                            Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].CheckingAccountConceptId
                        };
                        reinsuranceCheckingAccountTransactionItem.Comments = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].Description;
                        reinsuranceCheckingAccountTransactionItem.Company = new CompanyDTO()
                        {
                            IndividualId = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].AccountingCompanyId
                        };
                        reinsuranceCheckingAccountTransactionItem.SalePoint = new ACCDTO.SalePointDTO();
                        reinsuranceCheckingAccountTransactionItem.SalePoint.Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].SalePointId;

                        reinsuranceCheckingAccountTransactionItem.Id = reinsuranceCheckingAccount.ReinsuranceCheckingAccountTransactionItems[i].ReinsuranceCheckingAccountItemId;

                        reinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems.Add(reinsuranceCheckingAccountTransactionItem);
                    }
                }

                isSavedReinsuranceCheckingAccount = DelegateService.accountingApplicationService.SaveReinsuranceCheckingAccount(reinsuranceCheckingAccountTransaction, reinsuranceCheckingAccount.ImputationId/*, status*/);
            }

            if (isSavedReinsuranceCheckingAccount)
            {
                reinsuranceCheckingAccountResponse = 1;
            }
            else
            {
                reinsuranceCheckingAccountResponse = 0;
            }

            return Json(reinsuranceCheckingAccountResponse, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// DeleteTempReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempImputationCode"></param>
        /// <param name="tempReinsuranceCheckingAccountCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempReinsuranceCheckingAccountItem(int tempImputationCode, int tempReinsuranceCheckingAccountCode)
        {
            bool isDeletedReinsuranceCheckingAccountItem = DelegateService.accountingApplicationService.DeleteReinsuranceCheckingAccountItem(tempImputationCode, tempReinsuranceCheckingAccountCode);

            return Json(isDeletedReinsuranceCheckingAccountItem, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// GetTempReinsuranceCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempReinsuranceCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            List<object> reinsurerCheckingAccounts = new List<object>();

            try
            {          

            if (tempImputationId > 0)
            {
                List<SCRDTO.ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItemDTOs =
                DelegateService.accountingApplicationService.GetTempReinsuranceCheckingAccountItemByTempApplicationId(tempImputationId);

                foreach (SCRDTO.ReinsuranceCheckingAccountItemDTO reinsurerCheckingAccount in reinsuranceCheckingAccountItemDTOs)
                {
                    reinsurerCheckingAccounts.Add(new
                    {
                        BranchId = reinsurerCheckingAccount.BranchCode,
                        BranchName = reinsurerCheckingAccount.BranchName,
                        SalePointId = reinsurerCheckingAccount.PosCode,
                        SalePointName = reinsurerCheckingAccount.PosName,
                        CompanyId = reinsurerCheckingAccount.CompanyCode,
                        CompanyName = reinsurerCheckingAccount.CompanyName,
                        LineBusinessId = reinsurerCheckingAccount.LineBusinessCode,
                        LineBusinessName = reinsurerCheckingAccount.PrefixName,
                        SubLineBusinessId = reinsurerCheckingAccount.SubLineBusinessCode,
                        SubLineBusinessName = reinsurerCheckingAccount.SubPrefixName,
                        Description = reinsurerCheckingAccount.Description,
                        BrokerId = reinsurerCheckingAccount.AgentCode,
                        BrokerName = reinsurerCheckingAccount.BrokerName,
                        ReinsurerId = reinsurerCheckingAccount.ReinsuranceCompanyCode,
                        ReinsurerName = reinsurerCheckingAccount.ReinsurerName,
                        ConceptId = reinsurerCheckingAccount.CheckingAccountConceptCode,
                        ConceptName = reinsurerCheckingAccount.ConceptName,
                        NatureId = reinsurerCheckingAccount.DebitCreditCode,
                        NatureName = reinsurerCheckingAccount.DebitCreditName,
                        AccountingNature = reinsurerCheckingAccount.AccountingNature,
                        CurrencyId = reinsurerCheckingAccount.CurrencyCode,
                        Currency = reinsurerCheckingAccount.CurrencyName,
                        ExchangeRate = reinsurerCheckingAccount.CurrencyChange,
                        Amount = reinsurerCheckingAccount.Amount,
                        LocalAmount = reinsurerCheckingAccount.IncomeAmount,
                        ContractTypeId = reinsurerCheckingAccount.ContractTypeCode,
                        ContractId = reinsurerCheckingAccount.Contract,
                        ContractNumber = reinsurerCheckingAccount.Contract,
                        StretchId = reinsurerCheckingAccount.Stretch,
                        StretchName = reinsurerCheckingAccount.Stretch,
                        ReinsurerCheckingAccountItemId = reinsurerCheckingAccount.ReinsuranceCheckingAccountItemId,
                        AgentTypeCode = reinsurerCheckingAccount.AgentTypeCode,
                        AgentAgencyCode = reinsurerCheckingAccount.AgentAgencyCode,
                        BillNumber = reinsurerCheckingAccount.CollectNumber,
                        Region = reinsurerCheckingAccount.Region,
                        Excercise = reinsurerCheckingAccount.Excercise,
                        ApplicationYear = reinsurerCheckingAccount.ApplicationYear,
                        ApplicationMonth = reinsurerCheckingAccount.ApplicationMonth,
                        SlipNumber = reinsurerCheckingAccount.SlipNumber,
                        PolicyEndorsement = reinsurerCheckingAccount.PolicyEndorsement,
                        ReinsurerPolicyId = reinsurerCheckingAccount.ReinsurancePolicyId,
                        ReinsurerEndorsementId = reinsurerCheckingAccount.ReinsuranceEndorsementId,
                        TempReinsuranceParentId = reinsurerCheckingAccount.TempReinsuranceParentId,
                        YearMonthApplies = reinsurerCheckingAccount.YearMonthApplies,
                        FacultativeId = reinsurerCheckingAccount.FacultativeCode,
                        Items = reinsurerCheckingAccount.Items,
                        Status = reinsurerCheckingAccount.Status
                    });
                }
            }
            }
            catch (UnhandledException)
            {

                return Json(reinsurerCheckingAccounts, JsonRequestBehavior.AllowGet);
            }

            return Json(reinsurerCheckingAccounts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateInsurancePolicyEndorsement
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateInsurancePolicyEndorsement(string policyNumber, int endorsementNumber, int branchId, int prefixId)
        {
            int validateEndorsement = DelegateService.accountingApplicationService.ValidateInsurancePolicyEndorsement(policyNumber, endorsementNumber, branchId, prefixId);

            return Json(validateEndorsement, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GetEndorsementIdByPolicyEndorsement
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetEndorsementIdByPolicyEndorsement(string policyNumber, int endorsementNumber, int branchId, int prefixId)
        {
                int endorsementId = 0;
                int policyId = 0;
                if (branchId != 0 && prefixId != 0 && !String.IsNullOrEmpty(policyNumber) )
                {
                    var policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, Convert.ToDecimal(policyNumber));
                    if (policy != null)
                    {
                        policyId = policy.Endorsement.PolicyId;
                        if (policyId > 0)
                        {
                            List<TempCommonDTOs.EndorsementDTO> allEndorsemets;
                            allEndorsemets = (from TempCommonDTOs.EndorsementDTO endorsement in DelegateService.tempCommonService.GetEndorsementByPolicyId(policyId)
                                              select endorsement).Where(x => x.EndorsementNumber == endorsementNumber).ToList();
                            if (allEndorsemets.Count > 0)
                            {
                                endorsementId = allEndorsemets[0].EndorsementId;
                            }
                        }

                    }
                }
               return Json(endorsementId, JsonRequestBehavior.AllowGet);
               
       }



        #endregion

    }
}