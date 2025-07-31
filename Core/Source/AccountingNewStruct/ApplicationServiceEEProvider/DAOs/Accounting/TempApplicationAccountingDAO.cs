using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class TempApplicationAccountingDAO
    {

        /// <summary>
        /// GetTempDailyAccountingTransactionByTempImputationId
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>DailyAccountingTransaction</returns>
        public DailyAccountingTransaction GetTempApplicationAccountingByTempApplicationId(int tempApplicationId)
        {
            decimal debits = 0;
            decimal credits = 0;
            Amount amountCredit = new Amount();
            Amount amountDebit = new Amount();

            DailyAccountingTransaction tempDailyAccountingTransaction = new DailyAccountingTransaction();
            tempDailyAccountingTransaction.Id = tempApplicationId;
            tempDailyAccountingTransaction.DailyAccountingTransactionItems = new List<DailyAccountingTransactionItem>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.TempAppCode, tempApplicationId);

            List<ACCOUNTINGEN.TempApplicationAccounting> entityTempApplicationAccountings = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationAccounting), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationAccounting>().ToList();

            if (entityTempApplicationAccountings != null && entityTempApplicationAccountings.Count > 0)
            {
                foreach (ACCOUNTINGEN.TempApplicationAccounting tempApplicationAccounting in entityTempApplicationAccountings)
                {
                    Branch branch = new Branch() { Id = Convert.ToInt32(tempApplicationAccounting.BranchCode) };
                    SalePoint salePoint = new SalePoint() { Id = Convert.ToInt32(tempApplicationAccounting.SalePointCode) };
                    Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempApplicationAccounting.IndividualCode) };

                    AccountingNature accountingNature = (AccountingNature)tempApplicationAccounting.AccountingNature;

                    Amount amount = new Amount()
                    {
                        Currency = new Currency() { Id = Convert.ToInt32(tempApplicationAccounting.CurrencyCode) },
                        Value = Convert.ToDecimal(tempApplicationAccounting.Amount)
                    };

                    BookAccount bookAccount = new BookAccount() { Id = Convert.ToInt32(tempApplicationAccounting.AccountingAccountCode) };

                    tempDailyAccountingTransaction.DailyAccountingTransactionItems.Add(new DailyAccountingTransactionItem()
                    {
                        Id = tempApplicationAccounting.TempAppAccountingCode,
                        Branch = branch,
                        SalePoint = salePoint,
                        Beneficiary = beneficiary,
                        AccountingNature = accountingNature,
                        Amount = amount,
                        BookAccount = bookAccount
                    });

                    if (tempApplicationAccounting.AccountingNature == 1) //IncomeAmount < 0
                    {
                        credits += Convert.ToDecimal(tempApplicationAccounting.LocalAmount); //IncomeAmount
                    }

                    if (tempApplicationAccounting.AccountingNature == 2) //IncomeAmount > 0
                    {
                        debits += Convert.ToDecimal(tempApplicationAccounting.LocalAmount); //IncomeAmount
                    }
                }
            }

            amountCredit.Value = credits;
            amountDebit.Value = debits;

            tempDailyAccountingTransaction.TotalCredit = amountCredit;
            tempDailyAccountingTransaction.TotalDebit = amountDebit;

            return tempDailyAccountingTransaction;
        }
    }
}
