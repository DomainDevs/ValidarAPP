//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;

// Sistran FWk
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;


//Sistran Company
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class CheckingAccountCoinsurancesController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 0;
        public const string SortOrder = "ASC";

        #endregion

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

        /// <summary>
        /// ValidateDuplicateCoinsuranceCheckingAccount
        /// Permite validar el ingreso de duplicados tanto en temporales como en reales en las tablas: BILL.TEMP_COINSURANCE_CHECKING_ACCOUNT
        /// BILL.COINSURANCE_CHECKING_ACCOUNT
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="companyId"></param>
        /// <param name="coinsuranceId"></param>
        /// <param name="accountingNatureId"></param>
        /// <param name="checkingAccountConceptId"></param>
        /// <param name="currencyId"></param>
        /// <param name="coinsuranceTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateDuplicateCoinsuranceCheckingAccount(int branchId, int salePointId, int companyId, int coinsuranceId, int accountingNatureId, int checkingAccountConceptId, int currencyId, int coinsuranceTypeId)
        {
            ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter = new ValidateParameterBrokerCoinsuranceReinsuranceDTO();
            validateParameter.AccountingNatureId = accountingNatureId;
            validateParameter.Branch = new SCRDTO.BranchDTO() { Id = branchId };
            validateParameter.CheckingAccountConceptId = checkingAccountConceptId;
            validateParameter.Company = new CompanyDTO() { IndividualId = companyId };
            validateParameter.CoinsuranceId = coinsuranceId;
            validateParameter.CoinsuranceTypeId = coinsuranceTypeId;
            validateParameter.Currency = new SCRDTO.CurrencyDTO() { Id = currencyId };
            validateParameter.SalePoint = new ACCDTO.SalePointDTO() { Id = salePointId };

            ApplicationDTO application = DelegateService.accountingApplicationService.ValidateDuplicateCoinsuranceCheckingAccount(validateParameter);

            List<object> coinsuranceCheckingAccounts = new List<object>();

            coinsuranceCheckingAccounts.Add(new
            {
                source = application.VerificationValue.Value,
                imputationId = application.Id
            });

            return Json(coinsuranceCheckingAccounts, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// SaveTempReinsuranceCheckingAccountRequest
        /// </summary>
        /// <param name="coinsuranceCheckingAccount"></param>
        /// <param name="status"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempCoinsuranceCheckingAccountRequest(CoinsuranceCheckingAccountModel coinsuranceCheckingAccount, int status)
        {
            bool isSavedCoinsuranceCheckingAccount = false;
            int coinsuranceCheckingAccountResponse = 0;

            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

            CoInsuranceCheckingAccountTransactionDTO coinsuranceCheckingAccountTransaction = new CoInsuranceCheckingAccountTransactionDTO();

            coinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems = new List<CoInsuranceCheckingAccountTransactionItemDTO>();

            if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems != null)
            {
                for (int i = 0; i < coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems.Count; i++)
                {
                    if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountItemId == 0)
                    {
                        CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItemDTO();

                        coinsuranceCheckingAccountTransactionItem.Amount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CurrencyCode },
                            Value = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].Amount
                        };
                        coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                        {
                            BuyAmount = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].ExchangeRate
                        };

                        if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].AccountingNatureId == 1)
                        {
                            coinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            coinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }

                        if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceType == 1)
                        {
                            coinsuranceCheckingAccountTransactionItem.CoInsuranceType = Convert.ToInt32(CoInsuranceTypes.Accepted);
                        }
                        else
                        {
                            coinsuranceCheckingAccountTransactionItem.CoInsuranceType = Convert.ToInt32(CoInsuranceTypes.Given);
                        }

                        coinsuranceCheckingAccountTransactionItem.AccountingDate = accountingDate;
                        coinsuranceCheckingAccountTransactionItem.LocalAmount = new ACCDTO.AmountDTO();
                        coinsuranceCheckingAccountTransactionItem.Branch = new SCRDTO.BranchDTO()
                        {
                            Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].BranchId
                        };
                        coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConceptDTO()
                        {
                            Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CheckingAccountConceptId
                        };
                        coinsuranceCheckingAccountTransactionItem.Comments = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].Description;
                        coinsuranceCheckingAccountTransactionItem.Company = new CompanyDTO()
                        {
                            IndividualId = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].AccountingCompanyId
                        };
                        coinsuranceCheckingAccountTransactionItem.SalePoint = new ACCDTO.SalePointDTO()
                        {
                            Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].SalePointId
                        };
                        coinsuranceCheckingAccountTransactionItem.Holder = new CompanyDTO()
                        {
                            IndividualId = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuredCompanyId
                        };

                        coinsuranceCheckingAccountTransactionItem.Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountItemId;

                        // Items a aplicar
                        coinsuranceCheckingAccountTransactionItem.CoInsuranceCheckingAccountTransactionChild = new List<CoInsuranceCheckingAccountTransactionItemDTO>();
                        coinsuranceCheckingAccountTransactionItem.CoinsurancesCheckingAccountItems = new List<CoInsuranceCheckingAccountItemDTO>();

                        if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild != null)
                        {
                            for (int k = 0; k < coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild.Count; k++)
                            {
                                if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CoinsuranceCheckingAccountItemId == 0)
                                {
                                    CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItemChild = new CoInsuranceCheckingAccountTransactionItemDTO();

                                    coinsuranceCheckingAccountTransactionItemChild.Amount = new ACCDTO.AmountDTO()
                                    {
                                        Currency = new SCRDTO.CurrencyDTO() { Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CurrencyCode },
                                        Value = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].Amount
                                    };
                                    coinsuranceCheckingAccountTransactionItemChild.ExchangeRate = new ACCDTO.ExchangeRateDTO()
                                    {
                                        BuyAmount = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].ExchangeRate
                                    };

                                    if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].AccountingNatureId == 1)
                                    {
                                        coinsuranceCheckingAccountTransactionItemChild.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                                    }
                                    else
                                    {
                                        coinsuranceCheckingAccountTransactionItemChild.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                                    }
                                    if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CoinsuranceType == 1)
                                    {
                                        coinsuranceCheckingAccountTransactionItemChild.CoInsuranceType = Convert.ToInt32(CoInsuranceTypes.Accepted);
                                    }
                                    else
                                    {
                                        coinsuranceCheckingAccountTransactionItemChild.CoInsuranceType = Convert.ToInt32(CoInsuranceTypes.Given);
                                    }
                                    coinsuranceCheckingAccountTransactionItemChild.LocalAmount = new ACCDTO.AmountDTO();
                                    coinsuranceCheckingAccountTransactionItemChild.Branch = new SCRDTO.BranchDTO()
                                    {
                                        Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].BranchId
                                    };
                                    coinsuranceCheckingAccountTransactionItemChild.CheckingAccountConcept = new CheckingAccountConceptDTO()
                                    {
                                        Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CheckingAccountConceptId
                                    };
                                    coinsuranceCheckingAccountTransactionItemChild.Comments = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].Description;
                                    coinsuranceCheckingAccountTransactionItemChild.Company = new CompanyDTO()
                                    {
                                        IndividualId = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].AccountingCompanyId
                                    };
                                    coinsuranceCheckingAccountTransactionItemChild.SalePoint = new ACCDTO.SalePointDTO()
                                    {
                                        Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].SalePointId
                                    };
                                    coinsuranceCheckingAccountTransactionItemChild.Holder = new CompanyDTO()
                                    {
                                        IndividualId = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CoinsuredCompanyId
                                    };

                                    coinsuranceCheckingAccountTransactionItemChild.Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CoinsuranceCheckingAccountItemId;
                                    coinsuranceCheckingAccountTransactionItem.CoInsuranceCheckingAccountTransactionChild.Add(coinsuranceCheckingAccountTransactionItemChild);

                                    CoInsuranceCheckingAccountItemDTO coinsuranceCheckingAccountItemChild = new CoInsuranceCheckingAccountItemDTO()
                                    {
                                        CoinsuranceCheckingAccountId = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountTransactionChild[k].CoinsuranceCheckingAccountId,
                                        Id = 0,
                                        TempCoinsuranceCheckingAccountId = 0
                                    };

                                    coinsuranceCheckingAccountTransactionItem.CoinsurancesCheckingAccountItems.Add(coinsuranceCheckingAccountItemChild);
                                }
                            }
                        }

                        coinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems.Add(coinsuranceCheckingAccountTransactionItem);
                    }
                    else
                    {
                        CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItemDTO();

                        coinsuranceCheckingAccountTransactionItem.Amount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CurrencyCode },
                            Value = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].Amount
                        };
                        if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].AccountingNatureId == 1)
                        {
                            coinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            coinsuranceCheckingAccountTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }

                        if (coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceType == 1)
                        {
                            coinsuranceCheckingAccountTransactionItem.CoInsuranceType = Convert.ToInt32(CoInsuranceTypes.Accepted);
                        }
                        else
                        {
                            coinsuranceCheckingAccountTransactionItem.CoInsuranceType = Convert.ToInt32(CoInsuranceTypes.Given);
                        }

                        coinsuranceCheckingAccountTransactionItem.LocalAmount = new ACCDTO.AmountDTO();
                        coinsuranceCheckingAccountTransactionItem.Branch = new SCRDTO.BranchDTO()
                        {
                            Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].BranchId
                        };
                        coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConceptDTO()
                        {
                            Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CheckingAccountConceptId
                        };
                        coinsuranceCheckingAccountTransactionItem.Comments = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].Description;
                        coinsuranceCheckingAccountTransactionItem.Company = new CompanyDTO()
                        {
                            IndividualId = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].AccountingCompanyId
                        };
                        coinsuranceCheckingAccountTransactionItem.SalePoint = new ACCDTO.SalePointDTO()
                        {
                            Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].SalePointId
                        };

                        coinsuranceCheckingAccountTransactionItem.Id = coinsuranceCheckingAccount.CoinsuranceCheckingAccountTransactionItems[i].CoinsuranceCheckingAccountItemId;

                        coinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems.Add(coinsuranceCheckingAccountTransactionItem);
                    }
                }

                isSavedCoinsuranceCheckingAccount = DelegateService.accountingApplicationService.SaveCoinsuranceCheckingAccount(coinsuranceCheckingAccountTransaction, coinsuranceCheckingAccount.ImputationId/*, status*/);
            }

            if (isSavedCoinsuranceCheckingAccount)
            {
                coinsuranceCheckingAccountResponse = 1;
            }
            else
            {
                coinsuranceCheckingAccountResponse = 0;
            }

            return Json(coinsuranceCheckingAccountResponse, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// GetTempCoinsuranceCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempCoinsuranceCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            List<object> coinsurerCheckingAccounts = new List<object>();

            if (tempImputationId > 0)
            {
                List<CoinsuranceCheckingAccountItemDTO> coinsuranceCheckingAccountItemDTOs =
                DelegateService.accountingApplicationService.GetTempCoinsuranceCheckingAccountItemByTempApplicationId(tempImputationId);

                foreach (CoinsuranceCheckingAccountItemDTO coinsurerCheckingAccount in coinsuranceCheckingAccountItemDTOs)
                {
                    coinsurerCheckingAccounts.Add(new
                    {
                        BranchId = coinsurerCheckingAccount.BranchCode,
                        BranchName = coinsurerCheckingAccount.BranchName,
                        SalePointId = coinsurerCheckingAccount.PosCode,
                        SalePointName = coinsurerCheckingAccount.PosName,
                        CompanyId = coinsurerCheckingAccount.CompanyCode,
                        CompanyName = coinsurerCheckingAccount.CompanyName,
                        CoinsuranceTypeId = coinsurerCheckingAccount.CoinsuranceType,
                        CoinsuranceTypeName = coinsurerCheckingAccount.CoinsuranceTypeName,
                        Description = coinsurerCheckingAccount.Description,
                        CoinsurerId = coinsurerCheckingAccount.CoinsuranceCompanyCode,
                        CoinsurerName = coinsurerCheckingAccount.CoinsurerName,
                        //CoinsurerDocumentNumber = coinsurerCheckingAccount.CoinsurerDocumentNumber,
                        ConceptId = coinsurerCheckingAccount.CheckingAccountConceptCode,
                        ConceptName = coinsurerCheckingAccount.ConceptName,
                        NatureId = coinsurerCheckingAccount.DebitCreditCode,
                        NatureName = coinsurerCheckingAccount.DebitCreditName,
                        AccountingNature = coinsurerCheckingAccount.AccountingNature,
                        CurrencyId = coinsurerCheckingAccount.CurrencyCode,
                        Currency = coinsurerCheckingAccount.CurrencyName,
                        ExchangeRate = coinsurerCheckingAccount.CurrencyChange,
                        Amount = coinsurerCheckingAccount.Amount,
                        LocalAmount = coinsurerCheckingAccount.IncomeAmount,
                        CoinsurerCheckingAccountItemId = coinsurerCheckingAccount.CoinsuranceCheckingAccountItemId,
                        AgentAgencyCode = coinsurerCheckingAccount.AgentAgencyCode,
                        BillNumber = coinsurerCheckingAccount.CollectNumber,
                        TempCoinsuranceParentId = coinsurerCheckingAccount.TempCoinsuranceParentId,
                        Items = coinsurerCheckingAccount.Items,
                        Status = coinsurerCheckingAccount.Status
                    });
                }
            }

            return Json(coinsurerCheckingAccounts, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// DeleteTempCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempImputationCode"></param>
        /// <param name="tempCoinsuranceCheckingAccountCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempCoinsuranceCheckingAccountItem(int tempImputationCode, int tempCoinsuranceCheckingAccountCode)
        {
            bool isDeletedCoinsuranceCheckingAccountItem = DelegateService.accountingApplicationService.DeleteCoinsuranceCheckingAccountItem(tempImputationCode, tempCoinsuranceCheckingAccountCode);

            return Json(isDeletedCoinsuranceCheckingAccountItem, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}