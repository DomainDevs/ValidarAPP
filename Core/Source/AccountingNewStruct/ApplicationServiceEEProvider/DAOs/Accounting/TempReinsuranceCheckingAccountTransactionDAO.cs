//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
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
    class TempReinsuranceCheckingAccountTransactionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion


        /// <summary>
        /// GetTempReinsuranceCheckingAccountTransactionByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>ReInsuranceCheckingAccountTransaction</returns>
        public ReInsuranceCheckingAccountTransaction GetTempReinsuranceCheckingAccountTransactionByTempImputationId(int tempImputationId)
        {
            try
            {
                decimal debits = 0;
                decimal credits = 0;
                Amount amountCredit = new Amount();
                Amount amountDebit = new Amount();

                ReInsuranceCheckingAccountTransaction tempReinsuranceCheckingAccountTransaction = new ReInsuranceCheckingAccountTransaction();
                tempReinsuranceCheckingAccountTransaction.Id = tempImputationId;
                tempReinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems = new List<ReInsuranceCheckingAccountTransactionItem>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempImputationId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                    SelectObjects(typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                {
                    Branch branch = new Branch() { Id = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.BranchCode) };
                    SalePoint salePoint = new SalePoint() { Id = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.SalePointCode) };
                    Company company = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.AccountingCompanyCode) };

                    Amount amount = new Amount()
                    {
                        Currency = new Currency() { Id = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.CurrencyCode) },
                        Value = Convert.ToDecimal(tempReinsuranceCheckingAccountEntity.Amount)
                    };

                    CheckingAccountConcept checkingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.CheckingAccountConceptCode) };

                    Prefix prefix = new Prefix()
                    {
                        Id = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.LineBusinessCode),
                        LineBusinessId = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.SubLineBusinessCode)
                    };

                    Company reinsurer = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.ReinsuranceCompanyId) };
                    Company broker = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccountEntity.AgentId) };

                    tempReinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems.Add(new ReInsuranceCheckingAccountTransactionItem()
                    {
                        Id = tempReinsuranceCheckingAccountEntity.TempReinsCheckingAccTransCode,
                        Comments = tempReinsuranceCheckingAccountEntity.Description,
                        Branch = branch,
                        SalePoint = salePoint,
                        Company = company,
                        Amount = amount,
                        AccountingNature = tempReinsuranceCheckingAccountEntity.AccountingNature == 1 ? AccountingNature.Credit : AccountingNature.Debit,
                        CheckingAccountConcept = checkingAccountConcept,
                        Holder = reinsurer,
                        Prefix = prefix,
                        Broker = broker
                    });

                    if (tempReinsuranceCheckingAccountEntity.AccountingNature == 1) //IncomeAmount < 0
                    {
                        credits += Convert.ToDecimal(tempReinsuranceCheckingAccountEntity.Amount); //IncomeAmount
                    }

                    if (tempReinsuranceCheckingAccountEntity.AccountingNature == 2) //IncomeAmount > 0
                    {
                        debits += Convert.ToDecimal(tempReinsuranceCheckingAccountEntity.Amount); //IncomeAmount
                    }
                }

                amountCredit.Value = credits;
                amountDebit.Value = debits;

                tempReinsuranceCheckingAccountTransaction.TotalCredit = amountCredit;
                tempReinsuranceCheckingAccountTransaction.TotalDebit = amountDebit;

                return tempReinsuranceCheckingAccountTransaction;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
