using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Amortizations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

//Sistran Core

using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public  class AmortizationDAO
    {
        #region Class

        public class PendingPolicy
        {

            public int PolicyId { get; set; }
            public int BranchId { get; set; }
            public int PrefixId { get; set; }
            public string PolicyNumber { get; set; }
            public int CurrencyId { get; set; }
            public int BusinessTypeId { get; set; }
            public int EndorsementId { get; set; }
            public int EndorsementNumber { get; set; }
            public int PayerId { get; set; }
            public int QuotaNumber { get; set; }
            public int EndorsementYear { get; set; }
            public decimal Amount { get; set; }
        }

        #endregion

        #region Constants

        // Tipo de proceso utilizado para amortización
        private const int ProcessTypeId = 2;
        // Modulo administrativo contable
        private const int ModuleId = 2;

        #endregion

        #region Instance Variables

        #region Interfaz

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        readonly AccountingParameterServiceEEProvider _parameterServiceEEProvider = new AccountingParameterServiceEEProvider();
        //readonly AccountingImputationServiceEEProvider _imputationServiceEEProvider = new AccountingImputationServiceEEProvider();

        #endregion Interfaz

        #region DAOs

        readonly LogSpecialProcessDAO _logSpecialProcessDAO = new LogSpecialProcessDAO();
        //readonly TempImputationDAO _tempImputationDAO = new TempImputationDAO();
        readonly DAOs.Accounting.TempApplicationDAO tempApplicationDAO = new DAOs.Accounting.TempApplicationDAO();
        readonly TempPremiumReceivableTransactionDAO _tempPremiumReceivableTransactionDAO = new TempPremiumReceivableTransactionDAO();
        readonly TempPremiumReceivableTransactionItemDAO _tempPremiumReceivableTransactionItemDAO = new TempPremiumReceivableTransactionItemDAO();
        readonly TempJournalEntryDAO _tempJournalEntryDAO = new TempJournalEntryDAO();
        readonly TempDailyAccountingTransactionDAO _tempDailyAccountingTransactionDAO = new TempDailyAccountingTransactionDAO();
        readonly TempDailyAccountingTransactionItemDAO _tempDailyAccountingTransactionItemDAO = new TempDailyAccountingTransactionItemDAO();

        #endregion DAOs

        #endregion

        #region Public Methods

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
                Models.Imputations.Application tempImputation = new Models.Imputations.Application();

                // Busca todas las pólizas y endosos a amortizar 
                List<PendingPolicy> policies = GetPoliciesByAmortized(operationType, branch, prefix, policy, insured, amortizedValue);

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
                processId = _logSpecialProcessDAO.SaveLogSpecialProcess(amortization, ProcessTypeId, tempImputation.Id);

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
                            int creditAccountingConceptId = GetAccountingConceptByAccountingAccountId(creditAccountingAccountId);
                            int debitAccountingConceptId = GetAccountingConceptByAccountingAccountId(debitAccountingAccountId);

                            // Crea un temporal de asiento contable vacío
                            #region JournalEntry

                            JournalEntryDTO journalEntry = new JournalEntryDTO();

                            DateTime accountingDate = _parameterServiceEEProvider.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));

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
                            #region Imputation

                            DTOs.Imputations.ApplicationDTO imputation = new DTOs.Imputations.ApplicationDTO();
                            int imputationId = 0;
                            DateTime registerDate = DateTime.Now;

                            imputation.Id = imputationId;
                            imputation.RegisterDate = registerDate;
                            imputation.ModuleId = (int)Application.AccountingServices.Enums.ApplicationTypes.Collect;
                            
                            imputation.UserId = policy.UserId;

                            tempImputation = DelegateService.accountingApplicationService.SaveTempApplication(imputation, tempJournalEntry.Id).ToModel();

                            #endregion

                            foreach (PendingPolicy pendingPolicy in policies)
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

                                SaveTempPremiumReceivableTransaction(premiumReceivablePolicyDTO, tempImputation.Id, exchangeRate, policy.UserId);

                                totalToApplyPositive += pendingPolicy.Amount;
                                if (Convert.ToDecimal(pendingPolicy.Amount) < 0)
                                {
                                    //Crédito
                                    pendingPolicy.Amount = -pendingPolicy.Amount;

                                    SaveTempAccountingTransaction(branch.Id, pendingPolicy.PayerId,1 /*Convert.ToInt32(AccountingNature.Credit)*/, creditAccountingAccountId, 
                                                                  pendingPolicy.CurrencyId, pendingPolicy.Amount, exchangeRate, tempImputation.Id, creditAccountingConceptId);

                                }
                                else
                                {
                                    //Débito
                                    SaveTempAccountingTransaction(branch.Id, pendingPolicy.PayerId,2 /*Convert.ToInt32(AccountingNature.Debit)*/, debitAccountingAccountId, 
                                                                  pendingPolicy.CurrencyId, pendingPolicy.Amount, exchangeRate, tempImputation.Id, debitAccountingConceptId);

                                }

                                recordsProcessed++;
                                UpdateLogSpecialProcess(processId, totalToApplyPositive, exchangeRate, recordsProcessed, policies.Count, tempImputation.Id);
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
            int tempImputationId = 0;
            Amortization updateAmortization = GetAmortizationById(amortization.Id);

            try
            {
                bool exist = false;
                PremiumReceivableTransaction premiumReceivableTransaction;
                DailyAccountingTransaction dailyAccountingTransaction;
                Models.Imputations.Application tempImputation = new Models.Imputations.Application();

                if (amortization.Policies != null)
                {
                    if (amortization.Policies[0].Id > 0)
                    {
                        tempImputationId = amortization.Policies[0].Id;
                    }
                    else
                    {
                        tempImputationId = updateAmortization.Policies[0].BillingGroup.Id;
                    }
                }
                else {
                    if (updateAmortization.Policies != null)
                    {
                        tempImputationId = updateAmortization.Policies.Count > 0 ? updateAmortization.Policies[0].BillingGroup.Id : 0;
                    }
                }

                if (tempImputationId > 0) {
                    //Recupera id de asiento diario
                    tempImputation.Id = tempImputationId;
                    tempImputation = tempApplicationDAO.GetTempApplication(tempImputation);
                    exist = true;
                }

                //Proceso de baja: cambia estado cabecera y borra temporales
                if (amortization.AmortizationStatus == AmortizationStatus.Rejected)
                   {

                    if (exist)
                    {
                        //Recuperar los temporales de primas por cobrar por la temporal de imputacion
                        premiumReceivableTransaction =
                            _tempPremiumReceivableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(
                                                                      tempImputationId, (int)ImputationItemTypes.PremiumsReceivable);

                        foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in
                                                                        premiumReceivableTransaction.PremiumReceivableItems)
                        {
                            _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(
                                                                         premiumReceivableTransactionItem.Id);
                        }

                        //Recuperar los temporales de contabilidad por el temporal de imputación
                        dailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempImputationId);

                        foreach (DailyAccountingTransactionItem dailyAccountingTransactionItem in dailyAccountingTransaction.DailyAccountingTransactionItems)
                        {
                            _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                        }

                        if (tempImputation.ApplicationItems != null)
                        {
                            //Borra journal entry
                            _tempJournalEntryDAO.DeleteTempJournalEntry(tempImputation.ApplicationItems[0].Id);
                        }

                        //Borra temp imputation
                        tempApplicationDAO.DeleteTempApplication(tempImputationId);
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
                    premiumReceivableTransaction = _tempPremiumReceivableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(tempImputationId, (int)ImputationItemTypes.PremiumsReceivable);

                    //Si selecciona la totalidad de los registros borra también el asiento y la imputación temporales
                    if (premiumReceivableTransaction.PremiumReceivableItems.Count == 0)
                    {
                        //Recuperar los temporales de contabilidad por el temporal de imputación
                        dailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempImputationId);

                        foreach (DailyAccountingTransactionItem dailyAccountingTransactionItem in dailyAccountingTransaction.DailyAccountingTransactionItems)
                        {
                            _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                        }

                        if (tempImputation.ApplicationItems != null)
                        {
                            //Borra journal entry
                            _tempJournalEntryDAO.DeleteTempJournalEntry(tempImputation.ApplicationItems[0].Id);
                        }

                        //Borra temp imputation
                        tempApplicationDAO.DeleteTempApplication(tempImputationId);

                        updateAmortization.AmortizationStatus = AmortizationStatus.Rejected;

                        //Cambia estado de la cabecera
                        _logSpecialProcessDAO.UpdateLogSpecialProcess(updateAmortization, 0, 0, DateTime.Now, 0, 0);
                    }
                    else
                    {
                        updateAmortization.NegativeAppliedTotal.Value = 0;
                        decimal incomeAmountCredit = 0;
                        decimal incomeAmountDebit = 0;
                        decimal TotalCredit = 0;
                        decimal TotalDebit = 0;
                        foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in premiumReceivableTransaction.PremiumReceivableItems)
                        {
                            updateAmortization.NegativeAppliedTotal.Value = updateAmortization.NegativeAppliedTotal.Value + premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount;

                            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount > 0)
                            {
                                incomeAmountCredit += premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                            }
                            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < 0)
                            {
                                incomeAmountDebit += premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                            }
                        }
                        TotalCredit = premiumReceivableTransaction.TotalCredit.Value;  
                        TotalDebit = premiumReceivableTransaction.TotalDebit.Value;
 
                        //Recuperar los temporales de contabilidad por el temporal de imputación
                        dailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempImputationId);
                        foreach (DailyAccountingTransactionItem dailyAccountingTransactionItem in dailyAccountingTransaction.DailyAccountingTransactionItems)
                         {
                            dailyAccountingTransactionItem.LocalAmount = new Amount();
                            if (dailyAccountingTransactionItem.AccountingNature == AccountingNature.Credit)
                            {
                                if (TotalDebit == 0)
                                {
                                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                                }
                                else
                                {
                                    incomeAmountDebit = incomeAmountDebit < 0 ? incomeAmountDebit * (-1) : incomeAmountDebit;
                                    TotalDebit = TotalDebit < 0 ? TotalDebit * (-1) : TotalDebit;

                                    dailyAccountingTransactionItem.LocalAmount.Value = TotalDebit;
                                    dailyAccountingTransactionItem.Amount.Value = incomeAmountDebit;
                                }
                            }
                            else if (dailyAccountingTransactionItem.AccountingNature == AccountingNature.Debit)
                            {
                                if (TotalCredit == 0)
                                {
                                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(dailyAccountingTransactionItem.Id);
                                }
                                else
                                {
                                    incomeAmountCredit = incomeAmountCredit < 0 ? incomeAmountCredit * (-1) : incomeAmountCredit;
                                    TotalCredit = TotalCredit < 0 ? TotalCredit * (-1) : TotalCredit;

                                    dailyAccountingTransactionItem.LocalAmount.Value = TotalCredit;
                                    dailyAccountingTransactionItem.Amount.Value = incomeAmountCredit;
                                }
                            }
                         }

                        _logSpecialProcessDAO.UpdateLogSpecialProcess(updateAmortization, 0, 0, DateTime.Now, tempImputationId, 0);
                    }
                    updateAmortization.Id = 0;
                }
                // Aplicar: 
                if (amortization.AmortizationStatus == AmortizationStatus.Applied)
                {
                    ApplyAmortization(amortization);
                }
            }
            catch(Exception exception)
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
            Amortization aplicationAmortization = GetAmortizationById(amortization.Id);

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
                Models.Imputations.Application tempImputation = new Models.Imputations.Application() { Id = tempImputationId };

                tempImputation = tempApplicationDAO.GetTempApplication(tempImputation);
                
                //Proceso de aplicación: cambia estado aplicado y genera el id de imputación y número de recibo
                if (amortization.AmortizationStatus == AmortizationStatus.Applied)
                {
                    int newJournalEntry = 0;
                    int imputationId = 0;
                    int receiptNumber = 0;
                    int tempSourceCode = 0;
                    tempSourceCode = tempImputation.ApplicationItems[0].Id;

                    CollectApplicationDTO collectImputation = DelegateService.accountingApplicationService.SaveApplicationRequestJournalEntry(
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

        /// <summary>
        /// GetAmortizationById
        /// </summary>
        /// <param name="amortization"></param>
        /// <returns></returns>
        public Amortization GetAmortizationById(int amortizationId)
        {
            Amortization newAmortization = new Amortization();
            newAmortization.AmortizationStatus = AmortizationStatus.Actived;
              
            try
            {
                if (amortizationId > 0)
                {
                    //Crea la Primary key con el código de la entidad
                    PrimaryKey primaryKey = ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader.CreatePrimaryKey(amortizationId);

                    //Realizar las operaciones con los entities utilizando DAF
                    ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader logSpecialProcessEntity = (ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    if (logSpecialProcessEntity != null)
                    {
                        //Se obtiene la cabecera del proceso de amortización
                        newAmortization = ModelAssembler.CreateLogAmortizationProcess(logSpecialProcessEntity);

                        //Se obtiene el detalle del proceso de amortización
                        ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppAmortizationDetailV.Properties.LogSpecialProcessId, logSpecialProcessEntity.LogSpecialProcessId).And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppAmortizationDetailV.Properties.ProcessTypeId, ProcessTypeId).And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppAmortizationDetailV.Properties.UserId, logSpecialProcessEntity.UserId);

                        BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.AppAmortizationDetailV), criteriaBuilder.GetPredicate()));
                        List<Policy> policies = new List<Policy>();

                        foreach (ACCOUNTINGEN.AppAmortizationDetailV amortizationDetailEntity in businessCollection.OfType<ACCOUNTINGEN.AppAmortizationDetailV>())
                        {

                            Policy policy = new Policy();
                            policy.Agencies = new List<IssuanceAgency>()
                            {
                                new IssuanceAgency()
                                {
                                     Agent = new IssuanceAgent()
                                     {
                                         AgentType = new IssuanceAgentType()
                                         {
                                             Description = amortizationDetailEntity.AgentDocument
                                         },
                                         FullName = amortizationDetailEntity.AgentName,
                                         IndividualId = amortizationDetailEntity.AgentId,
                                     },
                                     Commissions = new List<IssuanceCommission>()
                                     {
                                         new IssuanceCommission()
                                         {
                                             Amount = Convert.ToDecimal(amortizationDetailEntity.Amount),
                                             CalculateBase = Convert.ToDecimal(amortizationDetailEntity.MainAmount)
                                         }
                                     },
                                }
                            };

                            policy.BillingGroup = new BillingGroup()
                            {
                                Description = amortizationDetailEntity.ImputationReceiptNumber.ToString(),
                                Id = amortizationDetailEntity.TempImputationId,
                            };
                            

                            policy.Branch = new Branch()
                            {
                                Description = amortizationDetailEntity.BranchName,
                                Id = Convert.ToInt32(amortizationDetailEntity.BranchCode)
                            };
                            policy.ExchangeRate = new ExchangeRate()
                            {
                                Currency = new Currency()
                                {
                                    Description = amortizationDetailEntity.CurrencyName,
                                    Id = Convert.ToInt32(amortizationDetailEntity.CurrencyCode)
                                },
                                SellAmount = Convert.ToDecimal(amortizationDetailEntity.ExchangeRate)
                            };

                            policy.DocumentNumber = Convert.ToInt32(amortizationDetailEntity.PolicyNumber);
                            policy.Endorsement = new Endorsement()
                            {
                                Id = Convert.ToInt32(amortizationDetailEntity.EndorsementId),
                                Number = amortizationDetailEntity.EndorsementNumber,
                                EndorsementReasonId = Convert.ToInt32(amortizationDetailEntity.CorrelativeNumber)
                            };
                            policy.Endorsement.EndorsementType = Enums.EndorsementType.AdjustmentEndorsement;


                            policy.Id = Convert.ToInt32(amortizationDetailEntity.PolicyId);
                            //Pagador
                            policy.DefaultBeneficiaries = new List<Beneficiary>()
                            {
                                new Beneficiary()
                                {
                                    IdentificationDocument = new IssuanceIdentificationDocument() { Number = amortizationDetailEntity.PayerDocument },
                                    IndividualId =  Convert.ToInt32(amortizationDetailEntity.PayerId),
                                    Name = amortizationDetailEntity.PayerName,
                                    CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual
                                }
                            };
                            //Asegurado
                            policy.Holder = new Holder()
                            {
                                IdentificationDocument = new IssuanceIdentificationDocument() { Number = amortizationDetailEntity.InsuredDocument },
                                IndividualId = Convert.ToInt32(amortizationDetailEntity.InsuredId),
                                Name = amortizationDetailEntity.InsuredName,
                                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Company,
                                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                            };

                            policy.PolicyType = new PolicyType()
                            {
                                Description = amortizationDetailEntity.ProcessTypeId.ToString(),
                                Id = amortizationDetailEntity.LogSpecialProcessId
                            };

                            policy.Prefix = new Prefix()
                            {
                                Description = amortizationDetailEntity.PrefixName,
                                Id = amortizationDetailEntity.PrefixCode
                            };

                            policy.UserId = amortizationDetailEntity.UserId;
                                                       
                            policies.Add(policy);
                        }
                        newAmortization.Policies = policies;
                    }
                }
            }
            catch (BusinessException exception)
            {
                newAmortization = new Amortization()
                {
                    AmortizationStatus = AmortizationStatus.Rejected,
                    Id = -4,
                    Policies = new List<Policy>() { new Policy() { TemporalTypeDescription = exception.Message } }
                };
            }

            return newAmortization;
        }

        /// <summary>
        /// GetAmortizations
        /// </summary>
        /// <returns></returns>
        public List<Amortization> GetAmortizations()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader.Properties.LogSpecialProcessId);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader.Properties.ProcessTypeId, ProcessTypeId);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(
                ACCOUNTINGEN.GetAutomaticCreditNoteProcessHeader), criteriaBuilder.GetPredicate()));

                // Return del model
                return ModelAssembler.CreateLogsAmortizationProcess(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// GetPoliciesByAmortized
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branch"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="amortizedAmount"></param>
        /// <returns></returns>
        internal List<PendingPolicy> GetPoliciesByAmortized(int operationType, Branch branch, Prefix prefix, Policy policy, Individual insured, decimal amortizedAmount)
        {
            try
            {
                int rows;
                List<PendingPolicy> pendingPolicies = new List<PendingPolicy>();
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyAmortization.Properties.BusinessTypeCode, operationType);

                if (branch.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyAmortization.Properties.BranchCode, branch.Id);
                }
                if (prefix.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyAmortization.Properties.PrefixCode, prefix.Id);
                }
                if (policy.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyAmortization.Properties.PolicyId, policy.Id);
                }
                if (policy.DocumentNumber > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyAmortization.Properties.PolicyNumber, policy.DocumentNumber);
                }
                if (insured.IndividualId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyAmortization.Properties.PayerId, insured.IndividualId);
                }
                if (policy.CurrentFrom.ToString("dd/MM/yyyy") != "01/01/1900")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyAmortization.Properties.IssueDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(policy.CurrentFrom);
                }
                if (policy.CurrentTo.ToString("dd/MM/yyyy") != "01/01/1900")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyAmortization.Properties.IssueDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(policy.CurrentTo);
                }

                UIView policyAmortization = _dataFacadeManager.GetDataFacade().GetView("PolicyAmortizationView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (policyAmortization.Count > 0)
                {
                    foreach (DataRow row in policyAmortization)
                    {
                        if (Math.Abs(Convert.ToDecimal(row["PaymentAmount"])) <= amortizedAmount)
                        {
                            pendingPolicies.Add(new PendingPolicy()
                            {
                                Amount = Convert.ToDecimal(row["PaymentAmount"]),
                                BranchId = Convert.ToInt32(row["BranchCode"]),
                                BusinessTypeId = Convert.ToInt32(row["BusinessTypeCode"]),
                                CurrencyId = Convert.ToInt32(row["CurrencyCode"]),
                                EndorsementId = Convert.ToInt32(row["EndorsementId"]),
                                EndorsementNumber = Convert.ToInt32(row["EndorsementNumber"]),
                                EndorsementYear = Convert.ToInt32(row["EndorsementYear"]),
                                PayerId = Convert.ToInt32(row["PayerId"]),
                                PolicyId = Convert.ToInt32(row["PolicyId"]),
                                PolicyNumber = Convert.ToString(row["PolicyNumber"]),
                                PrefixId = Convert.ToInt32(row["PrefixCode"]),
                                QuotaNumber = Convert.ToInt32(row["QuotaNumber"]),
                            });
                        }
                    }
                }

                return pendingPolicies;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempPremiumReceivableTransaction
        /// Graba las primas por cobrar 
        /// </summary>
        /// <param name="premiumReceivable"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        internal int SaveTempPremiumReceivableTransaction(PremiumReceivableSearchPolicyDTO premiumReceivable,
                                                         int tempImputationId, decimal exchangeRate, int userId)
        {
            try
            {
                PremiumReceivableTransaction premiumReceivableTransaction = SetPremiumReceivableTransaction(premiumReceivable);
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(ModuleId, DateTime.Now);
                return DelegateService.accountingApplicationService.SaveTempPremiumRecievableTransaction(premiumReceivableTransaction.ToDTO(), tempImputationId, 
                                                                                         exchangeRate, userId, DateTime.Now, accountingDate);
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// SetPremiumReceivableTransaction
        /// Setea las primas por cobrar
        /// </summary>
        /// <param name="premiumReceivableSearchPolicyDTO"></param>
        /// <returns>PremiumReceivableTransaction</returns>
        private PremiumReceivableTransaction SetPremiumReceivableTransaction(PremiumReceivableSearchPolicyDTO premiumReceivableSearchPolicyDTO)
        {
            try
            {
                PremiumReceivableTransaction premiumReceivableTransaction = new PremiumReceivableTransaction();

                premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItem>();

                PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();

                premiumReceivableTransactionItem.DeductCommission = new Amount() { Value = 0 };

                premiumReceivableTransactionItem.Policy = new Policy();
                premiumReceivableTransactionItem.Policy.Id = premiumReceivableSearchPolicyDTO.PolicyId;
                premiumReceivableTransactionItem.Policy.Endorsement = new Endorsement()
                {
                    Id = premiumReceivableSearchPolicyDTO.EndorsementId,
                };
                //Pagador
                premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                {
                    new Beneficiary()
                    {
                        IndividualId = premiumReceivableSearchPolicyDTO.PayerId,
                    }
                };
                premiumReceivableTransactionItem.Policy.ExchangeRate = new ExchangeRate()
                {
                    Currency = new Currency() { Id = premiumReceivableSearchPolicyDTO.CurrencyId }
                };

                premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponent>()
                {
                    new PayerComponent()
                    {
                         Amount = premiumReceivableSearchPolicyDTO.Amount,
                         BaseAmount = premiumReceivableSearchPolicyDTO.PaidAmount
                    }
                };
                premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                {
                    Quotas = new List<Quota>()
                    {
                        new Quota() { Number = premiumReceivableSearchPolicyDTO.PaymentNumber }
                    }
                };

                premiumReceivableTransactionItem.Id = 0;

                premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);

                return premiumReceivableTransaction;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// UpdateLogSpecialProcess
        /// Actualiza el proceso de amortización de pólizas
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="positiveValue"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="recordsProcessed"></param>
        /// <param name="totalRecords"></param>
        /// <param name="imputationId"></param>
        internal void UpdateLogSpecialProcess(int processId, decimal positiveValue, decimal exchangeRate,
                                             int recordsProcessed, int totalRecords, int imputationId)
        {
            try
            {
                Amortization amortization = new Amortization();
                amortization.Id = processId;
                amortization.PositiveAppliedTotal = new Amount();
                amortization.PositiveAppliedTotal.Value = positiveValue;
                amortization.AmortizationStatus = AmortizationStatus.Actived;
                amortization.NegativeAppliedTotal = new Amount();
                amortization.NegativeAppliedTotal.Value = 0;

                // Registra la finalización del proceso
                if (recordsProcessed == totalRecords)
                {
                    _logSpecialProcessDAO.UpdateLogSpecialProcess(amortization, exchangeRate, recordsProcessed, DateTime.Now, imputationId, 0);
                }
                else
                {
                    _logSpecialProcessDAO.UpdateLogSpecialProcess(amortization, exchangeRate, recordsProcessed, null, imputationId, 0);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// SaveTempAccountingTransaction
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="payerId"></param>
        /// <param name="accountingNature"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentAmount"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="accountingConceptId"></param>
        /// <returns></returns>
        internal int SaveTempAccountingTransaction(int branchId, int payerId, int accountingNature, int accountingAccountId, int currencyId,
                                                  decimal paymentAmount, decimal exchangeRate, int tempImputationId, int accountingConceptId)
        {
			int tempAccountingTransaction = 0;

            try
            {
                int count = 0;
                int tempDailyAccountingId = 0;
                decimal amount = 0;

                //Se valida si ya existe el registro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.TempImputationCode, tempImputationId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.AccountingNature, accountingNature).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.BookAccountCode, accountingAccountId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempDailyAccountingTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountngTransEntity in businessCollection.OfType<ACCOUNTINGEN.TempDailyAccountingTrans>())
                {
                    tempDailyAccountingId = tempDailyAccountngTransEntity.TempDailyAccountingTransId;
                    amount = Convert.ToDecimal(tempDailyAccountngTransEntity.Amount);
                    count++;
                }

                if (count == 0)
                {
                    ApplicationAccountingTransaction tempDailyAccountingTransaction = SetDailyAccountingTransaction(tempImputationId, branchId, payerId, accountingNature,
                                                                                      accountingAccountId, currencyId, paymentAmount, exchangeRate, accountingConceptId);
                    tempAccountingTransaction = DelegateService.accountingApplicationService.SaveTempAccountingTransaction(tempDailyAccountingTransaction.ToDTO());
                }
                else
                {
                    amount = amount + paymentAmount;
                    DailyAccountingTransactionItem tempDailyAccountingTransactionItem = SetDailyAccountingTransactionItem(tempDailyAccountingId, branchId, payerId,
                                                                                          accountingNature, accountingAccountId, currencyId, amount, exchangeRate);

                    tempAccountingTransaction = UpdateTempDailyAccountingTransactionItem(tempDailyAccountingTransactionItem, tempImputationId,
                                                                                                          accountingConceptId, "AMORTIZACIÓN AUTOMÁTICA DE PÓLIZAS",
                                                                                                          0, 0, null, 0);
                }
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
            return tempAccountingTransaction;
        }

        /// <summary>
        /// SetDailyAccountingTransaction
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="branchId"></param>
        /// <param name="payerId"></param>
        /// <param name="accountingNature"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentAmount"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="accountingConceptId"></param>
        /// <returns></returns>
        private ApplicationAccountingTransaction SetDailyAccountingTransaction(int tempApplicationId, int branchId, int payerId, int accountingNature, int accountingAccountId,
                                                                         int currencyId, decimal paymentAmount, decimal exchangeRate, int accountingConceptId)
        {
            try
            {
                ApplicationAccountingTransaction applicationAccountingTransaction = new ApplicationAccountingTransaction();
                applicationAccountingTransaction.Description = "AMORTIZACIÓN AUTOMÁTICA DE PÓLIZAS";
                applicationAccountingTransaction.ApplicationAccountingItems = new List<ApplicationAccounting>();

                ApplicationAccounting applicationAccountingItem = new ApplicationAccounting();

                applicationAccountingItem.AccountingAnalysisCodes = new List<ApplicationAccountingAnalysis>();
                applicationAccountingItem.AccountingCostCenters = new List<ApplicationAccountingCostCenter>();


                Branch branch = new Branch() { Id = branchId };
                SalePoint salePoint = new SalePoint() { Id = 0 };
                Company company = new Company() { IndividualId = 0 };
                Individual beneficiary = new Individual() { IndividualId = payerId };

                AccountingNature accountingNatures = (AccountingNature)accountingNature;

                Amount amount = new Amount()
                {
                    Currency = new Currency() { Id = currencyId },
                    Value = paymentAmount
                };
                ExchangeRate exchange = new ExchangeRate() { SellAmount = exchangeRate };
                Amount localAmount = new Amount() { Value = (exchangeRate * paymentAmount) };
                Models.Imputations.AccountingConcept accountingConcept = new Models.Imputations.AccountingConcept()
                {
                    Id = Convert.ToString(accountingConceptId)
                };

                BookAccount bookAccount = new BookAccount();
                bookAccount.Id = accountingAccountId;

                applicationAccountingItem.Id = 0;
                applicationAccountingItem.ApplicationAccountingId = tempApplicationId;
                applicationAccountingItem.Branch = branch;
                applicationAccountingItem.SalePoint = salePoint;
                //applicationAccountingItem.Company = company;
                applicationAccountingItem.Beneficiary = beneficiary;
                applicationAccountingItem.AccountingNature = accountingNature;
                applicationAccountingItem.Amount = amount;
                applicationAccountingItem.BookAccount = bookAccount;
                applicationAccountingItem.ExchangeRate = exchange;
                applicationAccountingItem.LocalAmount = localAmount;
                applicationAccountingItem.AccountingConcept = accountingConcept;

                applicationAccountingTransaction.ApplicationAccountingItems.Add(applicationAccountingItem);
                return applicationAccountingTransaction;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// GetAccountingConceptByAccountingAccountId
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns></returns>
        internal int GetAccountingConceptByAccountingAccountId(int accountingAccountId)
        {
            int accountingConceptId = 0;

            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AccountingAccountConcept.Properties.AccountingAccountId, accountingAccountId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.AccountingAccountConcept), criteriaBuilder.GetPredicate()));

                foreach(ACCOUNTINGEN.AccountingAccountConcept accountingConceptEntity in businessCollection.OfType<ACCOUNTINGEN.AccountingAccountConcept>())
                {
                    accountingConceptId = accountingConceptEntity.AccountingConceptCode;
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingConceptId;
        }

        /// <summary>
        /// SetDailyAccountingTransactionItem
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="payerId"></param>
        /// <param name="accountingNature"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentAmount"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        private DailyAccountingTransactionItem SetDailyAccountingTransactionItem(int id, int branchId, int payerId, int accountingNature, int accountingAccountId,
                                                                                 int currencyId, decimal paymentAmount, decimal exchangeRate)
        {
            try
            {
                DailyAccountingTransactionItem dailyAccountingTransactionItem = new DailyAccountingTransactionItem();

                Branch branch = new Branch() { Id = branchId };
                SalePoint salePoint = new SalePoint() { Id = 0 };
                Company company = new Company() { IndividualId = 0 };
                Individual beneficiary = new Individual() { IndividualId = payerId };

                AccountingNature accountingNatures = (AccountingNature)accountingNature;

                Amount amount = new Amount()
                {
                    Currency = new Currency() { Id = currencyId },
                    Value = paymentAmount
                };

                ExchangeRate exchange = new ExchangeRate() { SellAmount = exchangeRate };
                Amount localAmount = new Amount() { Value = (exchangeRate * paymentAmount) };
                BookAccount bookAccount = new BookAccount() { Id = accountingAccountId };

                dailyAccountingTransactionItem.Id = id;
                dailyAccountingTransactionItem.Branch = branch;
                dailyAccountingTransactionItem.SalePoint = salePoint;
                dailyAccountingTransactionItem.Company = company;
                dailyAccountingTransactionItem.Beneficiary = beneficiary;
                dailyAccountingTransactionItem.AccountingNature = accountingNatures;
                dailyAccountingTransactionItem.Amount = amount;
                dailyAccountingTransactionItem.BookAccount = bookAccount;
                dailyAccountingTransactionItem.ExchangeRate = exchange;
                dailyAccountingTransactionItem.LocalAmount = localAmount;

                return dailyAccountingTransactionItem;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        /// <summary>
        /// UpdateTempDailyAccountingTransactionItem
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItem"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        private int UpdateTempDailyAccountingTransactionItem(DailyAccountingTransactionItem tempDailyAccountingTransactionItem, int tempImputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            try
            {
                _dataFacadeManager.GetDataFacade().ClearObjectCache();

                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingTrans.CreatePrimaryKey(tempDailyAccountingTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity = (ACCOUNTINGEN.TempDailyAccountingTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempDailyAccountingEntity.TempImputationCode = tempImputationId;
                tempDailyAccountingEntity.BranchCode = tempDailyAccountingTransactionItem.Branch.Id;
                tempDailyAccountingEntity.SalePointCode = tempDailyAccountingTransactionItem.SalePoint.Id;
                tempDailyAccountingEntity.CompanyCode = tempDailyAccountingTransactionItem.Company.IndividualId;
                tempDailyAccountingEntity.PaymentConceptCode = paymentConceptCode;
                tempDailyAccountingEntity.BeneficiaryId = tempDailyAccountingTransactionItem.Beneficiary.IndividualId;
                tempDailyAccountingEntity.BookAccountCode = tempDailyAccountingTransactionItem.BookAccount.Id;
                tempDailyAccountingEntity.AccountingNature = Convert.ToInt32(tempDailyAccountingTransactionItem.AccountingNature);
                tempDailyAccountingEntity.CurrencyCode = tempDailyAccountingTransactionItem.Amount.Currency.Id;
                tempDailyAccountingEntity.IncomeAmount = tempDailyAccountingTransactionItem.Amount.Value;
                tempDailyAccountingEntity.ExchangeRate = tempDailyAccountingTransactionItem.ExchangeRate.SellAmount;
                tempDailyAccountingEntity.Amount = tempDailyAccountingTransactionItem.LocalAmount.Value;
                tempDailyAccountingEntity.Description = description;
                tempDailyAccountingEntity.BankReconciliationId = bankReconciliationId;
                tempDailyAccountingEntity.ReceiptNumber = receiptNumber;
                tempDailyAccountingEntity.ReceiptDate = receiptDate;
                tempDailyAccountingEntity.PostdatedAmount = postdatedAmount;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempDailyAccountingEntity);

                // Return del model
                return tempDailyAccountingTransactionItem.Id;
            }
            catch
            {
                return 0;
            }
        }

        #endregion


    }
}
