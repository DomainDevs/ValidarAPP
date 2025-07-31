//System
using System.Collections.Generic;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF.Web.Resources;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;
using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [NoDirectAccess]
    public class InsuredLoansController : Controller
    {   

        #region Views

        /// <summary>
        /// InsuredLoans
        /// </summary>
        /// <returns>View</returns>
        public ActionResult InsuredLoans()
        {
            return View();
        }

        #endregion

        #region Actions

        //TODO ESTA FUNCIONALIDAD DE PRESTAMOS SE MANTIENE YA QUE SE IMPLEMENTARÀ A FUTURO SEGUN FUNCIONAL

        ///<summary>
        /// SaveTempInsuredLoansRequest
        /// Graba la transacción de préstamos de un asegurado
        /// </summary>
        /// <param name="insuredLoanModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempInsuredLoansRequest(InsuredLoanModel insuredLoanModel)
        {
            int transactionId = 0;

            InsuredLoanTransactionDTO insuredLoansTransaction = new InsuredLoanTransactionDTO();

            insuredLoansTransaction.InsuredLoanTransactionItems = new List<InsuredLoanTransactionItemDTO>();

            if (insuredLoanModel.InsuredLoansTransactionItems != null)
            {
                for (int i = 0; i < insuredLoanModel.InsuredLoansTransactionItems.Count; i++)
                {
                    if (insuredLoanModel.InsuredLoansTransactionItems[i].InsuredLoanItemId == 0)
                    {
                        InsuredLoanTransactionItemDTO insuredLoansTransactionItem = new InsuredLoanTransactionItemDTO();

                        if (insuredLoanModel.InsuredLoansTransactionItems[i].AccountingNature == 1)
                        {
                            insuredLoansTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            insuredLoansTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }

                        insuredLoansTransactionItem.Capital = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = insuredLoanModel.InsuredLoansTransactionItems[i].CurrencyId },
                            Value = insuredLoanModel.InsuredLoansTransactionItems[i].Capital
                        };
                        insuredLoansTransactionItem.ExchangeRate = new ACCDTO.ExchangeRateDTO() { SellAmount = insuredLoanModel.InsuredLoansTransactionItems[i].ExchangeRate };

                        insuredLoansTransactionItem.CurrentInterest = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = insuredLoanModel.InsuredLoansTransactionItems[i].CurrencyId },
                            Value = insuredLoanModel.InsuredLoansTransactionItems[i].CurrentInterest
                        };
                        insuredLoansTransactionItem.ExchangeRateCurrent = new ACCDTO.ExchangeRateDTO()
                        {
                            SellAmount = insuredLoanModel.InsuredLoansTransactionItems[i].ExchangeRate
                        };

                        insuredLoansTransactionItem.Id = insuredLoanModel.InsuredLoansTransactionItems[i].InsuredLoanItemId;
                        insuredLoansTransactionItem.Imputation = new ApplicationDTO();
                        insuredLoansTransactionItem.Imputation.Id = insuredLoanModel.ImputationId;
                        insuredLoansTransactionItem.Insured = new ACCDTO.IndividualDTO()
                        {
                            IdentificationDocument = new ACCDTO.IdentificationDocumentDTO()
                            {
                                
                                Number = insuredLoanModel.InsuredLoansTransactionItems[i].InsuredDocumentNumber
                            },
                            IndividualId = insuredLoanModel.InsuredLoansTransactionItems[i].IndividualId,
                            Name = insuredLoanModel.InsuredLoansTransactionItems[i].InsuredName
                        };
                        insuredLoansTransactionItem.LoanNumber = insuredLoanModel.InsuredLoansTransactionItems[i].LoanNumber;

                        insuredLoansTransactionItem.PreviousInterest = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = insuredLoanModel.InsuredLoansTransactionItems[i].CurrencyId },
                            Value = insuredLoanModel.InsuredLoansTransactionItems[i].PreviousInterest
                        };
                        insuredLoansTransactionItem.ExchangeRatePrevious = new ACCDTO.ExchangeRateDTO()
                        {
                            SellAmount = insuredLoanModel.InsuredLoansTransactionItems[i].ExchangeRate
                        };

                        insuredLoansTransaction.InsuredLoanTransactionItems.Add(insuredLoansTransactionItem);
                    }
                    else
                    {
                        InsuredLoanTransactionItemDTO insuredLoansTransactionItem = new InsuredLoanTransactionItemDTO();

                        if (insuredLoanModel.InsuredLoansTransactionItems[i].AccountingNature == 1)
                        {
                            insuredLoansTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Credit);
                        }
                        else
                        {
                            insuredLoansTransactionItem.AccountingNature = Convert.ToInt32(AccountingNatures.Debit);
                        }
                        insuredLoansTransactionItem.Capital = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = insuredLoanModel.InsuredLoansTransactionItems[i].CurrencyId },
                            Value = insuredLoanModel.InsuredLoansTransactionItems[i].Capital
                        };

                        insuredLoansTransactionItem.CurrentInterest = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = insuredLoanModel.InsuredLoansTransactionItems[i].CurrencyId },
                            Value = insuredLoanModel.InsuredLoansTransactionItems[i].CurrentInterest
                        };

                        insuredLoansTransactionItem.Id = insuredLoanModel.InsuredLoansTransactionItems[i].InsuredLoanItemId;
                        insuredLoansTransactionItem.Imputation = new ApplicationDTO() { Id = insuredLoanModel.ImputationId };
                        insuredLoansTransactionItem.Insured = new ACCDTO.IndividualDTO();
                        insuredLoansTransactionItem.LoanNumber = insuredLoanModel.InsuredLoansTransactionItems[i].LoanNumber;
                        insuredLoansTransactionItem.PreviousInterest = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = insuredLoanModel.InsuredLoansTransactionItems[i].CurrencyId },
                            Value = insuredLoanModel.InsuredLoansTransactionItems[i].CurrentInterest
                        };

                        insuredLoansTransaction.InsuredLoanTransactionItems.Add(insuredLoansTransactionItem);
                    }
                }
                transactionId = DelegateService.accountingApplicationService.SaveTempInsuredLoanTransaction(insuredLoansTransaction);
            }

            return Json(transactionId, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        /// DeleteTempInsuredLoansItem
        /// Elimina una transacción de préstamos de un asegurado
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="tempInsuredLoanId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteTempInsuredLoansItem(int tempImputationId, int tempInsuredLoanId)
        {
            bool isDeletedTempInsuredLoanTransactionItem = DelegateService.accountingApplicationService.DeleteTempInsuredLoanTransactionItem(tempImputationId, tempInsuredLoanId);

            return Json(isDeletedTempInsuredLoanTransactionItem, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetTempBrokerCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempInsuredLoansItemByTempImputationId(int tempImputationId)
        {
            List<object> insuredLoanTransactionItems = new List<object>();
            var insuredLoans = DelegateService.accountingApplicationService.GetTmpInsuredLoansByTempApplicationId(tempImputationId);

            foreach (InsuredLoanTransactionDTO insuredLoanTransaction in insuredLoans)
            {
                foreach (InsuredLoanTransactionItemDTO insuredLoanTransactionItem in insuredLoanTransaction.InsuredLoanTransactionItems)
                {
                    insuredLoanTransactionItems.Add(new
                    {
                        AccountingNature = insuredLoanTransactionItem.AccountingNature == Convert.ToInt32(AccountingNatures.Credit) ? 1 : 2,
                        Amount = insuredLoanTransactionItem.Capital.Value,
                        BillNumber = -1,
                        CurrencyId = insuredLoanTransactionItem.PreviousInterest.Currency.Id,
                        CurrencyName = insuredLoanTransactionItem.PreviousInterest.Currency.Description,
                        CurrentInterest = insuredLoanTransactionItem.CurrentInterest.Value,
                        Description = "",
                        ExchangeRate = insuredLoanTransactionItem.ExchangeRatePrevious.BuyAmount,
                        ImputationId = insuredLoanTransactionItem.Imputation.Id,
                        IndividualId = insuredLoanTransactionItem.Insured.IndividualId,
                        InsuredDocumentNumber = insuredLoanTransactionItem.Insured.EconomicActivity.Description,
                        InsuredName = insuredLoanTransactionItem.Insured.Name,
                        InsuredLoanItemId = insuredLoanTransactionItem.Id,
                        LoanNumber = insuredLoanTransactionItem.LoanNumber,
                        LocalAmount = insuredLoanTransactionItem.LocalAmountPrevious.Value,
                        NatureId = insuredLoanTransactionItem.AccountingNature == Convert.ToInt32(AccountingNatures.Credit) ? 1 : 2,
                        NatureName = insuredLoanTransactionItem.AccountingNature == Convert.ToInt32(AccountingNatures.Credit) ? "Crédito" : "Débito",
                        PreviousInterest = insuredLoanTransactionItem.PreviousInterest.Value,
                        Status = "1"
                    });
                }
            }

            return Json(insuredLoanTransactionItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetInsuredLoanByNumber
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsuredLoanByNumber(string query)
        {
            int loanCount = 0;
            List<object> insuredLoanTransactionItems = new List<object>();
            InsuredLoanTransactionDTO insuredLoanTransaction = new InsuredLoanTransactionDTO();
            insuredLoanTransaction.Description = query;
            insuredLoanTransaction.Id = 0;

            var insuredLoans = DelegateService.accountingApplicationService.GetInsuredLoanTransaction(insuredLoanTransaction);

            if (insuredLoans.InsuredLoanTransactionItems != null)
            {
                loanCount = insuredLoans.InsuredLoanTransactionItems.Count;
            }

            if (loanCount == 0)
            {
                insuredLoanTransactionItems.Add(new
                {
                    CurrencyId = -1,
                    DocumentNumber = @Global.RegisterNotFound,
                    IndividualId = 0,
                    Name = "No existen datos",
                    LoanNumber = "-1"
                });
            }
            else
            {
                foreach (InsuredLoanTransactionItemDTO insuredLoanItems in insuredLoans.InsuredLoanTransactionItems)
                {
                    insuredLoanTransactionItems.Add(new
                    {
                        CurrencyId = insuredLoanItems.PreviousInterest.Currency.Id,
                        DocumentNumber = insuredLoanItems.Insured.EconomicActivity.Description,
                        IndividualId = insuredLoanItems.Insured.IndividualId,
                        Name = insuredLoanItems.Insured.Name,
                        LoanNumber = insuredLoanItems.LoanNumber.ToString()
                    });
                }
            }

            return Json(insuredLoanTransactionItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateDuplicateInsuredLoan
        /// Valida el ingreso de duplicados tanto en temporales como en reales de préstamos de asegurados
        /// </summary>
        /// <param name="loanNumber"></param>
        /// <param name="individualId"></param>
        /// <param name="accountingNatureId"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateDuplicateInsuredLoan(int loanNumber, int individualId, int accountingNatureId, int currencyId)
        {
            int loanCount = 0;
            int[] response = new int[4];

            response[0] = -1;
            response[1] = -1;
            response[2] = -1; //para saber si es una OP o PL
            response[3] = -1; //y para saber si ya esta en reales o no

            InsuredLoanTransactionDTO insuredLoanTransaction = new InsuredLoanTransactionDTO();
            insuredLoanTransaction.Id = -1;
            insuredLoanTransaction.InsuredLoanTransactionItems = new List<InsuredLoanTransactionItemDTO>();
            insuredLoanTransaction.InsuredLoanTransactionItems.Add(new InsuredLoanTransactionItemDTO()
            {
                AccountingNature = accountingNatureId,
                Capital = new ACCDTO.AmountDTO()
                {
                    Currency = new SCRDTO.CurrencyDTO() { Id = currencyId }
                },
                Insured = new ACCDTO.IndividualDTO() { IndividualId = individualId },
                LoanNumber = loanNumber
            });

            var insuredLoans = DelegateService.accountingApplicationService.GetInsuredLoanTransaction(insuredLoanTransaction);//No está implementado

            if (insuredLoans.InsuredLoanTransactionItems != null)
            {
                loanCount = insuredLoans.InsuredLoanTransactionItems.Count;
            }

            if (loanCount > 0)
            {
                response[0] = 0;
                response[1] = insuredLoans.InsuredLoanTransactionItems[0].Imputation.Id;
                response[2] = 2;
                response[3] = 0;
            }

            List<object> insuredLoanTransactionItems = new List<object>();
            insuredLoanTransactionItems.Add(new
            {
                source = response[0],
                imputationId = response[1],
                type = response[2], //PL O OP
                isReal = response[3]
            });

            return Json(insuredLoanTransactionItems, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}