using Sistran.Core.Application.AccountingServices.EEProvider.Models.Amortizations;
using Sistran.Core.Framework.BAF;
using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using Sistran.Core.Framework.Contexts;

using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class AmortizationApplicationDAO
    {
        // Tipo de proceso utilizado para amortización
        private const int ProcessTypeId = 2;

        readonly LogSpecialProcessDAO _logSpecialProcessDAO = new LogSpecialProcessDAO();
        AmortizationDAO amortizationDAO = new AmortizationDAO();
        readonly TempApplicationDAO tempApplicationDAO = new TempApplicationDAO();

        /// <summary>
        /// GenerateAmortization
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branch"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="amortizedValue"></param>
        /// <returns>Amortization</returns>
        public Amortization GenerateAmortization(int operationType, Branch branch, Prefix prefix, Policy policy, Individual insured, decimal amortizedValue)
        {
            Amortization amortization = new Amortization();

            amortization.AmortizationStatus = AmortizationStatus.Actived;

            try
            {
                int processId = 0;
                int recordsProcessed = 0;
                decimal totalToApplyPositive = 0;
                Models.Imputations.Application tempApplication = new Models.Imputations.Application();

                // Busca todas las pólizas y endosos a amortizar 
                List<AmortizationDAO.PendingPolicy> policies = amortizationDAO.GetPoliciesByAmortized(operationType, branch, prefix, policy, insured, amortizedValue);

                //ARMA CABECERA DE LOG
                #region LogHeader

                amortization.AmortizationStatus = AmortizationStatus.Actived;
                amortization.Date = DateTime.Now;
                amortization.Id = 0;
                amortization.NegativeAppliedTotal = new Amount() { Value = 0 };
                amortization.Policies = new List<Policy>();
                Policy policyItem = new Policy();
                policyItem.Branch = new Branch() { Id = branch.Id };
                amortization.Policies.Add(policyItem);
                amortization.PositiveAppliedTotal = new Amount() { Value = 0 };
                amortization.UserId = policy.UserId;

                // Graba cabecera log
                processId = _logSpecialProcessDAO.SaveLogSpecialProcess(amortization, ProcessTypeId, tempApplication.Id);

                #endregion


                if (policies.Count > 0)
                {
                    using (Context.Current)
                    {
                        Framework.Transactions.Transaction transaction = new Framework.Transactions.Transaction();

                        try
                        {
                            int creditAccountingAccountId = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_CREDIT_AMORTIZATION));
                            int debitAccountingAccountId = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_DEBIT_AMORTIZATION));
                            int creditAccountingConceptId = amortizationDAO.GetAccountingConceptByAccountingAccountId(creditAccountingAccountId);
                            int debitAccountingConceptId = amortizationDAO.GetAccountingConceptByAccountingAccountId(debitAccountingAccountId);

                            // Crea un temporal de asiento contable vacío
                            #region JournalEntry

                            JournalEntryDTO journalEntry = new JournalEntryDTO();

                            DateTime accountingDate = DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));

                            journalEntry.Id = 0;
                            journalEntry.AccountingDate = accountingDate;
                            journalEntry.Branch = new BranchDTO();
                            journalEntry.Branch.Id = branch.Id;
                            journalEntry.Comments = "";
                            journalEntry.Company = new DTOs.CompanyDTO() { IndividualId = 1 }; //DEFAULT EN 3G
                            journalEntry.Description = "AMORTIZACIÓN AUTOMÁTICA DE PÓLIZAS";
                            journalEntry.Payer = new DTOs.IndividualDTO() { IndividualId = 0 };
                            journalEntry.Payer.Name = "";
                            journalEntry.PersonType = new DTOs.PersonTypeDTO();
                            journalEntry.PersonType.Id = 0;
                            journalEntry.SalePoint = new DTOs.SalePointDTO();
                            journalEntry.SalePoint.Id = 0;
                            journalEntry.Status = 0;

                            JournalEntryDTO tempJournalEntry = DelegateService.accountingApplicationService.SaveTempJournalEntry(journalEntry);

                            #endregion

                            //CREA UN TEMPORAL DE IMPUTACION VACIO 
                            #region Application

                            ApplicationDTO application = new ApplicationDTO();
                            int applicationId = 0;
                            DateTime registerDate = DateTime.Now;

                            application.Id = applicationId;
                            application.RegisterDate = registerDate;
                            //application.ImputationType = 2;
                            application.ModuleId = 2; // ???
                            application.UserId = policy.UserId;

                            tempApplication = tempApplicationDAO.SaveTempApplication(application.ToModel(), tempJournalEntry.Id);

                            #endregion

                            foreach (AmortizationDAO.PendingPolicy pendingPolicy in policies)
                            {
                                //Se obtiene la tasa de cambio dado la moneda
                                decimal exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, pendingPolicy.CurrencyId).SellAmount;

                                DTOs.Search.PremiumReceivableSearchPolicyDTO premiumReceivablePolicyDTO = new DTOs.Search.PremiumReceivableSearchPolicyDTO();
                                premiumReceivablePolicyDTO.PolicyId = pendingPolicy.PolicyId;
                                premiumReceivablePolicyDTO.EndorsementId = pendingPolicy.EndorsementId;
                                premiumReceivablePolicyDTO.PayerId = pendingPolicy.PayerId;
                                premiumReceivablePolicyDTO.CurrencyId = pendingPolicy.CurrencyId;
                                premiumReceivablePolicyDTO.PaymentNumber = pendingPolicy.QuotaNumber;
                                premiumReceivablePolicyDTO.Amount = pendingPolicy.Amount;
                                premiumReceivablePolicyDTO.PaidAmount = pendingPolicy.Amount;

                                amortizationDAO.SaveTempPremiumReceivableTransaction(premiumReceivablePolicyDTO, tempApplication.Id, exchangeRate, policy.UserId);

                                totalToApplyPositive += pendingPolicy.Amount;
                                if (Convert.ToDecimal(pendingPolicy.Amount) < 0)
                                {
                                    //Crédito
                                    pendingPolicy.Amount = -pendingPolicy.Amount;

                                    amortizationDAO.SaveTempAccountingTransaction(branch.Id, pendingPolicy.PayerId, 1 /*Convert.ToInt32(AccountingNature.Credit)*/, creditAccountingAccountId,
                                                                  pendingPolicy.CurrencyId, pendingPolicy.Amount, exchangeRate, tempApplication.Id, creditAccountingConceptId);

                                }
                                else
                                {
                                    //Débito
                                    amortizationDAO.SaveTempAccountingTransaction(branch.Id, pendingPolicy.PayerId, 2 /*Convert.ToInt32(AccountingNature.Debit)*/, debitAccountingAccountId,
                                                                  pendingPolicy.CurrencyId, pendingPolicy.Amount, exchangeRate, tempApplication.Id, debitAccountingConceptId);

                                }

                                recordsProcessed++;
                                amortizationDAO.UpdateLogSpecialProcess(processId, totalToApplyPositive, exchangeRate, recordsProcessed, policies.Count, tempApplication.Id);
                            }

                            transaction.Complete();
                        }
                        catch (BusinessException exception)
                        {
                            transaction.Dispose();

                            amortization.Id = -1;
                            amortization.Policies = new List<Policy>() { new Policy() { TemporalTypeDescription = exception.Message } };
                        }
                    }
                }
                else
                {
                    amortization.Id = processId;
                    amortization.AmortizationStatus = AmortizationStatus.NoData;

                    _logSpecialProcessDAO.UpdateLogSpecialProcess(amortization, 0, 0, DateTime.Now, 0, 0);
                }
            }
            catch (BusinessException exception)
            {
                amortization.Id = -1;
                amortization.Policies = new List<Policy>() { new Policy() { TemporalTypeDescription = exception.Message } };
            }

            return amortization;
        }



        /// <summary>
        /// UpdateAmortization
        /// </summary>
        /// <param name="amortization"></param>
        /// <returns>Amortization</returns>
        public Amortization UpdateAmortization(Amortization amortization)
        {
            int tempApplicationId = 0;
            Amortization updateAmortization = amortizationDAO.GetAmortizationById(amortization.Id);
            TempApplicationPremiumDAO tempApplicationPremium = new TempApplicationPremiumDAO();
            TempPremiumReceivableTransactionItemDAO _tempPremiumReceivableTransactionItemDAO = new TempPremiumReceivableTransactionItemDAO();
            TempDailyAccountingTransactionDAO _tempDailyAccountingTransactionDAO = new TempDailyAccountingTransactionDAO();
            TempDailyAccountingTransactionItemDAO _tempDailyAccountingTransactionItemDAO = new TempDailyAccountingTransactionItemDAO();
            TempJournalEntryDAO _tempJournalEntryDAO = new TempJournalEntryDAO();

            try
            {
                bool exist = false;
                ApplicationPremiumTransaction premiumReceivableTransaction;
                DailyAccountingTransaction dailyAccountingTransaction;
                Models.Imputations.Application tempApplication = new Models.Imputations.Application();

                if (amortization.Policies != null)
                {
                    if (amortization.Policies[0].Id > 0)
                    {
                        tempApplicationId = amortization.Policies[0].Id;
                    }
                    else
                    {
                        tempApplicationId = updateAmortization.Policies[0].BillingGroup.Id;
                    }
                }
                else
                {
                    if (updateAmortization.Policies != null)
                    {
                        tempApplicationId = updateAmortization.Policies.Count > 0 ? updateAmortization.Policies[0].BillingGroup.Id : 0;
                    }
                }

                if (tempApplicationId > 0)
                {
                    //Recupera id de asiento diario
                    tempApplication.Id = tempApplicationId;
                    tempApplication = tempApplicationDAO.GetTempApplication(tempApplication);
                    exist = true;
                }

                //Proceso de baja: cambia estado cabecera y borra temporales
                if (amortization.AmortizationStatus == AmortizationStatus.Rejected)
                {

                    if (exist)
                    {
                        //Recuperar los temporales de primas por cobrar por la temporal de imputacion
                        premiumReceivableTransaction =
                            tempApplicationPremium.GetTempApplicationPremiumByTempApplicationId(
                                                                      //tempApplicationId, (int)ImputationItemTypes.PremiumsReceivable);
                                                                      tempApplicationId, 1);

                        foreach (ApplicationPremiumTransactionItem premiumReceivableTransactionItem in
                                                                        premiumReceivableTransaction.PremiumReceivableItems)
                        {
                            _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(
                                                                         premiumReceivableTransactionItem.Id);
                        }

                        //Recuperar los temporales de contabilidad por el temporal de imputación
                        dailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempApplicationId);

                        foreach (DailyAccountingTransactionItem dailyAccountingTransactionItem in dailyAccountingTransaction.DailyAccountingTransactionItems)
                        {
                            _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                        }

                        if (tempApplication.ApplicationItems != null)
                        {
                            //Borra journal entry
                            _tempJournalEntryDAO.DeleteTempJournalEntry(tempApplication.ApplicationItems[0].Id);
                        }

                        //Borra temp imputation
                        tempApplicationDAO.DeleteTempApplication(tempApplicationId);
                    }

                    //Cambia estado de la cabecera
                    _logSpecialProcessDAO.UpdateLogSpecialProcess(amortization, 0, 0, DateTime.Now, 0, 0);
                    updateAmortization.Id = 0;
                }
                //Eliminar: solo borra temporales seleccionados
                else if (amortization.AmortizationStatus == AmortizationStatus.Actived)
                {
                    foreach (Policy policy in amortization.Policies)
                    {
                        //Borra temporal de primas x cobrar
                        _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(policy.Endorsement.Id);
                    }
                    //Recuperar los temporales de primas por cobrar por la temporal de imputación
                    //premiumReceivableTransaction = tempApplicationPremium.GetTempPremiumRecievableTransactionByTempImputationId(tempApplicationId, (int)ImputationItemTypes.PremiumsReceivable);
                    premiumReceivableTransaction = tempApplicationPremium.GetTempApplicationPremiumByTempApplicationId(tempApplicationId, 1);

                    //Si selecciona la totalidad de los registros borra también el asiento y la imputación temporales
                    //if (premiumReceivableTransaction.PremiumReceivableItems.Count == 0)
                    //{
                    //    //Recuperar los temporales de contabilidad por el temporal de imputación
                    //    dailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempApplicationId);

                    //    foreach (DailyAccountingTransactionItem dailyAccountingTransactionItem in dailyAccountingTransaction.DailyAccountingTransactionItems)
                    //    {
                    //        _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                    //    }

                    //    if (tempApplication.Items != null)
                    //    {
                    //        //Borra journal entry
                    //        _tempJournalEntryDAO.DeleteTempJournalEntry(tempApplication.Items[0].Id);
                    //    }

                    //    //Borra temp imputation
                    //    tempApplicationDAO.DeleteTempApplication(tempApplicationId);

                    //    updateAmortization.AmortizationStatus = AmortizationStatus.Rejected;

                    //    //Cambia estado de la cabecera
                    //    _logSpecialProcessDAO.UpdateLogSpecialProcess(updateAmortization, 0, 0, DateTime.Now, 0, 0);
                    //}
                    //else
                    //{
                    //    updateAmortization.NegativeAppliedTotal.Value = 0;
                    //    decimal incomeAmountCredit = 0;
                    //    decimal incomeAmountDebit = 0;
                    //    decimal TotalCredit = 0;
                    //    decimal TotalDebit = 0;
                    //    foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in premiumReceivableTransaction.PremiumReceivableItems)
                    //    {
                    //        updateAmortization.NegativeAppliedTotal.Value = updateAmortization.NegativeAppliedTotal.Value + premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount;

                    //        if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount > 0)
                    //        {
                    //            incomeAmountCredit += premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                    //        }
                    //        if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < 0)
                    //        {
                    //            incomeAmountDebit += premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                    //        }
                    //    }
                    //    TotalCredit = premiumReceivableTransaction.TotalCredit.Value;
                    //    TotalDebit = premiumReceivableTransaction.TotalDebit.Value;

                    //    //Recuperar los temporales de contabilidad por el temporal de imputación
                    //    dailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempApplicationId);
                    //    foreach (DailyAccountingTransactionItem dailyAccountingTransactionItem in dailyAccountingTransaction.DailyAccountingTransactionItems)
                    //    {
                    //        dailyAccountingTransactionItem.LocalAmount = new Amount();
                    //        if (dailyAccountingTransactionItem.AccountingNature == AccountingNature.Credit)
                    //        {
                    //            if (TotalDebit == 0)
                    //            {
                    //                _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                    //            }
                    //            else
                    //            {
                    //                incomeAmountDebit = incomeAmountDebit < 0 ? incomeAmountDebit * (-1) : incomeAmountDebit;
                    //                TotalDebit = TotalDebit < 0 ? TotalDebit * (-1) : TotalDebit;

                    //                dailyAccountingTransactionItem.LocalAmount.Value = TotalDebit;
                    //                dailyAccountingTransactionItem.Amount.Value = incomeAmountDebit;
                    //            }
                    //        }
                    //        else if (dailyAccountingTransactionItem.AccountingNature == AccountingNature.Debit)
                    //        {
                    //            if (TotalCredit == 0)
                    //            {
                    //                _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                    //            }
                    //            else
                    //            {
                    //                incomeAmountCredit = incomeAmountCredit < 0 ? incomeAmountCredit * (-1) : incomeAmountCredit;
                    //                TotalCredit = TotalCredit < 0 ? TotalCredit * (-1) : TotalCredit;

                    //                dailyAccountingTransactionItem.LocalAmount.Value = TotalCredit;
                    //                dailyAccountingTransactionItem.Amount.Value = incomeAmountCredit;
                    //            }
                    //        }
                    //    }

                    //    _logSpecialProcessDAO.UpdateLogSpecialProcess(updateAmortization, 0, 0, DateTime.Now, tempApplicationId, 0);
                    //}
                    updateAmortization.Id = 0;
                }
                // Aplicar: 
                if (amortization.AmortizationStatus == AmortizationStatus.Applied)
                {
                    amortizationDAO.ApplyAmortization(amortization);
                }
            }
            catch (Exception exception)
            {
                updateAmortization.AmortizationStatus = AmortizationStatus.Rejected;
                updateAmortization.Id = -1;
                updateAmortization.Policies = new List<Policy>() { new Policy() { TemporalTypeDescription = exception.Message } };
            }

            return updateAmortization;
        }

        /// <summary>
        /// ApplyAmortization
        /// </summary>
        /// <param name="amortization"></param>
        /// <returns>Amortization</returns>
        public Amortization ApplyAmortization(Amortization amortization)
        {
            int tempImputationId = 0;
            Amortization aplicationAmortization = amortizationDAO.GetAmortizationById(amortization.Id);

            try
            {
                //Obtener el temporal de imputación
                if (amortization.Policies[0].Id > 0)
                {
                    tempImputationId = amortization.Policies[0].Id;
                }
                else
                {
                    tempImputationId = aplicationAmortization.Policies[0].BillingGroup.Id;
                }

                //Recupera id de asiento diario
                Models.Imputations.Application tempApplication = new Models.Imputations.Application() { Id = tempImputationId };

                tempApplication = tempApplicationDAO.GetTempApplication(tempApplication);

                //Proceso de aplicación: cambia estado aplicado y genera el id de imputación y número de recibo
                if (amortization.AmortizationStatus == AmortizationStatus.Applied)
                {
                    int newJournalEntry = 0;
                    int imputationId = 0;
                    int receiptNumber = 0;
                    int tempSourceCode = 0;
                    tempSourceCode = tempApplication.ApplicationItems[0].Id;

                    CollectApplicationDTO collectImputation =  DelegateService.accountingApplicationService.SaveApplicationRequestJournalEntry(
                                                        tempImputationId, amortization.UserId, tempSourceCode);

                    newJournalEntry = collectImputation.Transaction.TechnicalTransaction;

                    aplicationAmortization.AmortizationStatus = AmortizationStatus.Applied;

                    if (newJournalEntry > 0)
                    {
                        imputationId = collectImputation.Application.Id;
                        receiptNumber = collectImputation.Transaction.TechnicalTransaction;
                        aplicationAmortization.Policies[0].PolicyType.Id = newJournalEntry;
                    }

                    _logSpecialProcessDAO.UpdateLogSpecialProcess(aplicationAmortization, 0, 0, DateTime.Now, imputationId, receiptNumber);
                }
            }
            catch (BusinessException exception)
            {
                aplicationAmortization.AmortizationStatus = AmortizationStatus.Rejected;
                aplicationAmortization.Id = -1;
                aplicationAmortization.Policies = new List<Policy>() { new Policy() { TemporalTypeDescription = exception.Message } };
            }
            return aplicationAmortization;
        }
    }
}
