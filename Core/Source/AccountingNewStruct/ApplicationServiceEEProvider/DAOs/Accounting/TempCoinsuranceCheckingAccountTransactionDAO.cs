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
    class TempCoinsuranceCheckingAccountTransactionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// GetTempCoinsuranceCheckingAccountTransactionByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>CoInsuranceCheckingAccountTransaction</returns>
        public CoInsuranceCheckingAccountTransaction GetTempCoinsuranceCheckingAccountTransactionByTempImputationId(int tempImputationId)
        {
            decimal debits = 0;
            decimal credits = 0;
            Amount amountCredit = new Amount();
            Amount amountDebit = new Amount();

            CoInsuranceCheckingAccountTransaction tempCoinsuranceCheckingAccountTransaction = new CoInsuranceCheckingAccountTransaction();
            tempCoinsuranceCheckingAccountTransaction.Id = tempImputationId;
            tempCoinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems = new List<CoInsuranceCheckingAccountTransactionItem>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);
            BusinessCollection businessCollection = new BusinessCollection(
                _dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

            foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
            {
                Branch branch = new Branch() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccountEntity.BranchCode) };
                SalePoint salePoint = new SalePoint() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccountEntity.SalePointCode) };
                Company company = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccountEntity.AccountingCompanyCode) };
                Amount amount = new Amount()
                {
                    Currency = new Currency() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccountEntity.CurrencyCode) },
                    Value = Convert.ToDecimal(tempCoinsuranceCheckingAccountEntity.Amount)
                };

                CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccountEntity.CheckingAccountConceptCode) };
                Company coinsurer = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccountEntity.CoinsuredCompanyId) };

                tempCoinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems.Add(new CoInsuranceCheckingAccountTransactionItem()
                {
                    Id = tempCoinsuranceCheckingAccountEntity.TempCoinsCheckingAccTransCode,
                    Comments = tempCoinsuranceCheckingAccountEntity.Description,
                    Branch = branch,
                    SalePoint = salePoint,
                    Company = company,
                    Amount = amount,
                    AccountingNature = tempCoinsuranceCheckingAccountEntity.AccountingNatureCode == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                    CheckingAccountConcept = checkingAccountConcept,
                    Holder = coinsurer,
                    AccountingDate = Convert.ToDateTime(tempCoinsuranceCheckingAccountEntity.AccountingDate)
                });

                if (tempCoinsuranceCheckingAccountEntity.AccountingNatureCode == 1) //IncomeAmount < 0
                {
                    credits += Convert.ToDecimal(tempCoinsuranceCheckingAccountEntity.Amount); //IncomeAmount
                }
                    
                if (tempCoinsuranceCheckingAccountEntity.AccountingNatureCode == 2) //IncomeAmount > 0
                {
                    debits += Convert.ToDecimal(tempCoinsuranceCheckingAccountEntity.Amount); //IncomeAmount
                }
            }

            amountCredit.Value = credits;
            amountDebit.Value = debits;

            tempCoinsuranceCheckingAccountTransaction.TotalCredit = amountCredit;
            tempCoinsuranceCheckingAccountTransaction.TotalDebit = amountDebit;

            return tempCoinsuranceCheckingAccountTransaction;
        }

    }
}
