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
    class TempBrokerCheckingAccountTransactionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// GetTempBrokerCheckingAccountTransactionByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>BrokersCheckingAccountTransaction</returns>
        public BrokersCheckingAccountTransaction GetTempBrokerCheckingAccountTransactionByTempImputationId(int tempImputationId)
        {
            decimal debits = 0;
            decimal credits = 0;
            Amount amountCredit = new Amount();
            Amount amountDebit = new Amount();

            BrokersCheckingAccountTransaction tempBrokersCheckingAccountTransaction = new BrokersCheckingAccountTransaction();
            tempBrokersCheckingAccountTransaction.Id = tempImputationId;
            tempBrokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItem>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);
            BusinessCollection businessCollection = new BusinessCollection(
                _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), criteriaBuilder.GetPredicate()));

            foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
            {
                Branch branch = new Branch() { Id = Convert.ToInt32(tempBrokerCheckingAccountEntity.BranchCode) };
                SalePoint salePoint = new SalePoint() { Id = Convert.ToInt32(tempBrokerCheckingAccountEntity.SalePointCode) };
                Company company = new Company() { IndividualId = Convert.ToInt32(tempBrokerCheckingAccountEntity.AccountingCompanyCode) };

                Amount amount = new Amount()
                {
                    Currency = new Currency() { Id = Convert.ToInt32(tempBrokerCheckingAccountEntity.CurrencyCode) },
                    Value = Convert.ToDecimal(tempBrokerCheckingAccountEntity.Amount)
                };

                CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempBrokerCheckingAccountEntity.CheckingAccountConceptCode) };

                tempBrokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems.Add(new BrokersCheckingAccountTransactionItem()
                {
                    Id = tempBrokerCheckingAccountEntity.TempBrokerCheckingAccTransCode,
                    Comments = tempBrokerCheckingAccountEntity.Description,
                    Branch = branch,
                    SalePoint = salePoint,
                    Company = company,
                    Amount = amount,
                    AccountingNature = tempBrokerCheckingAccountEntity.AccountingNature == 1 ? AccountingNature.Credit: AccountingNature.Debit, 
                    CheckingAccountConcept = checkingAccountConcept
                });

                if (tempBrokerCheckingAccountEntity.AccountingNature == 1) //IncomeAmount < 0
                {
                    credits += Convert.ToDecimal(tempBrokerCheckingAccountEntity.Amount); //IncomeAmount
                }
                if (tempBrokerCheckingAccountEntity.AccountingNature == 2) //IncomeAmount > 0
                {
                    debits += Convert.ToDecimal(tempBrokerCheckingAccountEntity.Amount); //IncomeAmount
                }
            }

            amountCredit.Value = credits;
            amountDebit.Value = debits;

            tempBrokersCheckingAccountTransaction.TotalCredit = amountCredit;
            tempBrokersCheckingAccountTransaction.TotalDebit = amountDebit;

            return tempBrokersCheckingAccountTransaction;
        }
    }
}
