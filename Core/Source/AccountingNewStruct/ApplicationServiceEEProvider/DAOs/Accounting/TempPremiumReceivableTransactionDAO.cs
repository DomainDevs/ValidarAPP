//System
using System;
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempPremiumReceivableTransactionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion


        /// <summary>
        /// GetTempPremiumRecievableTransactionByImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>PremiumReceivableTransaction</returns>
        public PremiumReceivableTransaction GetTempPremiumRecievableTransactionByTempImputationId(int tempImputationId, int imputationTypeId)
        {
            try
            {
                decimal debits = 0;
                decimal credits = 0;
                Amount amountCredit = new Amount();
                Amount amountDebit = new Amount();

                decimal excessPayment = 0;
                decimal upd = 0;

                PremiumReceivableTransaction tempPremiumReceivableTransaction = new PremiumReceivableTransaction();
                tempPremiumReceivableTransaction.Id = tempImputationId;
                tempPremiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItem>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempApplicationCode, tempImputationId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                                                    SelectObjects(typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                {
                    Policy policy = new Policy();
                    policy.Id = Convert.ToInt32(tempPremiumReceivableEntity.PolicyId);

                    policy.Endorsement = new Endorsement()
                    {
                        Id = Convert.ToInt32(tempPremiumReceivableEntity.EndorsementId)
                    };
                    //Pagador
                    policy.DefaultBeneficiaries = new List<Beneficiary>()
                    {
                        new Beneficiary()
                        {
                            CustomerType =  CustomerType.Individual,
                            IndividualId = Convert.ToInt32(tempPremiumReceivableEntity.PayerId),
                        }
                    };
                    policy.ExchangeRate = new ExchangeRate()
                    {
                        BuyAmount = Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate),
                        Currency = new Currency() { Id = Convert.ToInt32(tempPremiumReceivableEntity.CurrencyCode) }
                    };
                    policy.PayerComponents = new List<PayerComponent>()
                    {
                        new PayerComponent()
                        {
                            Amount = Convert.ToDecimal(tempPremiumReceivableEntity.PaymentAmount) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate),
                            BaseAmount = Convert.ToDecimal(tempPremiumReceivableEntity.IncomeAmount)
                        }
                    };
                    policy.PaymentPlan = new PaymentPlan()
                    {
                        Quotas = new List<Quota>()
                        {
                            new Quota() { Number = Convert.ToInt32(tempPremiumReceivableEntity.PaymentNum) }
                        }
                    };

                    tempPremiumReceivableTransaction.PremiumReceivableItems.Add(new PremiumReceivableTransactionItem()
                    {
                        Id = tempPremiumReceivableEntity.TempPremiumReceivableTransCode,
                        Policy = policy
                    });

                    #region PremiumReceivable

                    if (imputationTypeId == Convert.ToInt32(ImputationItemTypes.PremiumsReceivable))
                    {
                        if (tempPremiumReceivableEntity.IncomeAmount > 0)
                        {
                            credits += tempPremiumReceivableEntity.IncomeAmount > tempPremiumReceivableEntity.PaymentAmount ? Convert.ToDecimal(tempPremiumReceivableEntity.PaymentAmount) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate) : Convert.ToDecimal(tempPremiumReceivableEntity.IncomeAmount) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate);
                        }
                        if (tempPremiumReceivableEntity.IncomeAmount < 0)
                        {
                            debits += Convert.ToDecimal(tempPremiumReceivableEntity.IncomeAmount) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate);
                        }
                    }

                    #endregion

                    #region DepositPremiums

                    if (imputationTypeId == Convert.ToInt32(ImputationItemTypes.DepositPremiums))
                    {
                        if (tempPremiumReceivableEntity.PaymentAmount > 0)
                        {
                            excessPayment = Convert.ToDecimal(tempPremiumReceivableEntity.IncomeAmount) - Convert.ToDecimal(tempPremiumReceivableEntity.PaymentAmount);
                            if (excessPayment > 0)
                            {
                                debits += Convert.ToDecimal(upd);            //el uso de primas en depósito va para el débito
                                credits += Convert.ToDecimal(excessPayment) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate); //el pago en exceso va al crédito
                            }
                            else
                            {
                                TempUsedDepositPremiumDAO tempUsedDepositPremium = new TempUsedDepositPremiumDAO();
                                tempUsedDepositPremium.GetTempUsedDepositPremiumByTempPremiumReceivableId((int)tempPremiumReceivableEntity.TempPremiumReceivableTransCode);
                                foreach (var itemUPD in tempUsedDepositPremium.GetTempUsedDepositPremiumByTempPremiumReceivableId((int)tempPremiumReceivableEntity.TempPremiumReceivableTransCode))
                                {
                                    debits += Convert.ToDecimal(itemUPD.Amount);
                                }
                                
                            }

                            upd = 0;
                        }

                        if (tempPremiumReceivableEntity.PaymentAmount < 0)
                        {
                            debits += 0;
                        }
                    }

                    #endregion

                    #region CommissionRetained

                    if (imputationTypeId == Convert.ToInt32(ImputationItemTypes.CommissionRetained))
                    {
                        if (tempPremiumReceivableEntity.DiscountedCommission > 0)
                        {
                            credits += Convert.ToDecimal(tempPremiumReceivableEntity.DiscountedCommission) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate);
                        }

                        if (tempPremiumReceivableEntity.DiscountedCommission < 0)
                        {
                            debits += Convert.ToDecimal(tempPremiumReceivableEntity.DiscountedCommission) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate);
                        }
                    }

                    #endregion
                }

                amountCredit.Value = credits;
                amountDebit.Value = debits;

                tempPremiumReceivableTransaction.TotalCredit = amountCredit;
                tempPremiumReceivableTransaction.TotalDebit = amountDebit;

                return tempPremiumReceivableTransaction;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
