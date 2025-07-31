//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class TempDailyAccountingTransactionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// GetTempDailyAccountingTransactionByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>DailyAccountingTransaction</returns>
        public DailyAccountingTransaction GetTempDailyAccountingTransactionByTempImputationId(int tempImputationId)
        {
            decimal debits = 0;
            decimal credits = 0;
            Amount amountCredit = new Amount();
            Amount amountDebit = new Amount();

            DailyAccountingTransaction tempDailyAccountingTransaction = new DailyAccountingTransaction();
            tempDailyAccountingTransaction.Id = tempImputationId;
            tempDailyAccountingTransaction.DailyAccountingTransactionItems = new List<DailyAccountingTransactionItem>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.TempImputationCode, tempImputationId);
            BusinessCollection businessCollection =
                new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempDailyAccountingTrans), filter.GetPredicate()));

            foreach (ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccounting in businessCollection.OfType<ACCOUNTINGEN.TempDailyAccountingTrans>())
            {
                Branch branch = new Branch() { Id = Convert.ToInt32(tempDailyAccounting.BranchCode) };
                SalePoint salePoint = new SalePoint() { Id = Convert.ToInt32(tempDailyAccounting.SalePointCode) };
                Company company = new Company() { IndividualId = Convert.ToInt32(tempDailyAccounting.CompanyCode) };
                Individual beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempDailyAccounting.BeneficiaryId) };

                AccountingNature accountingNature = (AccountingNature)tempDailyAccounting.AccountingNature;

                Amount amount = new Amount()
                {
                    Currency = new Currency() { Id = Convert.ToInt32(tempDailyAccounting.CurrencyCode) },
                    Value = Convert.ToDecimal(tempDailyAccounting.IncomeAmount)
                };

                BookAccount bookAccount = new BookAccount() { Id = Convert.ToInt32(tempDailyAccounting.BookAccountCode) };

                tempDailyAccountingTransaction.DailyAccountingTransactionItems.Add(new DailyAccountingTransactionItem()
                    {
                        Id = tempDailyAccounting.TempDailyAccountingTransId,
                        Branch = branch,
                        SalePoint = salePoint,
                        Company = company,
                        Beneficiary = beneficiary,
                        AccountingNature = accountingNature,
                        Amount = amount,
                        BookAccount = bookAccount
                    });

                if (tempDailyAccounting.AccountingNature == 1) //IncomeAmount < 0
                {
                    credits += Convert.ToDecimal(tempDailyAccounting.IncomeAmount); //IncomeAmount
                }
                    
                if (tempDailyAccounting.AccountingNature == 2) //IncomeAmount > 0
                {
                    debits += Convert.ToDecimal(tempDailyAccounting.IncomeAmount); //IncomeAmount
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
