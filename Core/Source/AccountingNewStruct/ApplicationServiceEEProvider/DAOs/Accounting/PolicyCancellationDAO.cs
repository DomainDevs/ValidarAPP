//Sistran.Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Data;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PolicyCancellationDAO
    {
        #region Instance variables

        #region Interfaz

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region DAOs

        //private readonly CollectDAO _collectDAO = new CollectDAO();

        #endregion DAOs

        #endregion

        #region Public Methods

        /// <summary>
        /// SaveLogPolicyCancellation
        /// </summary>
        /// <param name="logPolicyCancellationId"></param>
        /// <param name="branch"></param>
        /// <param name="salePoint"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="intermediary"></param>
        /// <param name="grouper"></param>
        /// <param name="business"></param>
        /// <param name="dueDate"></param>
        /// <param name="issuanceDateFrom"></param>
        /// <param name="issuanceDateTo"></param>
        /// <param name="cancellationPolicyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SaveLogPolicyCancellation(int logPolicyCancellationId, Branch branch, SalePoint salePoint, Prefix prefix,
                                             Policy policy, Person insured, Person intermediary,
                                             int grouper, int business, DateTime dueDate,
                                             DateTime issuanceDateFrom, DateTime issuanceDateTo,
                                             CancellationPolicyType cancellationPolicyType, int userId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.LogPolicyCancellation policyCancellationEntity = EntityAssembler.CreateLogPolicyCancellation(logPolicyCancellationId, branch, salePoint,
                                                                           prefix, policy, insured, intermediary, grouper, business, dueDate,
                                                                           issuanceDateFrom, issuanceDateTo, cancellationPolicyType, userId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(policyCancellationEntity);

                return policyCancellationEntity.LogPolicyCancellationId;
            }
            catch (ArgumentException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GeneratePolicyCancellation
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="salePoint"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="intermediary"></param>
        /// <param name="grouper"></param>
        /// <param name="business"></param>
        /// <param name="dueDate"></param>
        /// <param name="issuanceDateFrom"></param>
        /// <param name="issuanceDateTo"></param>
        /// <param name="cancellationPolicyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GeneratePolicyCancellation(Branch branch, SalePoint salePoint, Prefix prefix,
                                              Policy policy, Person insured, Person intermediary,
                                              int grouper, int business, DateTime dueDate,
                                              DateTime issuanceDateFrom, DateTime issuanceDateTo,
                                              CancellationPolicyType cancellationPolicyType, int userId)
        {
            int processId = 0;

            try
            {
                int recordsProcessed = 0;
                decimal totalToApplyPositive = 0;
                decimal totalToApplyNegative = 0;

                // Busca todas las pólizas y endosos a cancelar 
                List<Policy> policies = GetPoliciesToCancel(branch, salePoint, prefix, policy, insured, intermediary, grouper, business, 
                                                            dueDate, issuanceDateFrom, issuanceDateTo, cancellationPolicyType, userId);


                if (policies.Count > 0)
                {
                    using (Context.Current)
                    {
                        Transaction transaction = new Transaction();

                        try
                        {
                            // Graba cabecera log
                            processId = SaveLogPolicyCancellation(0, branch, salePoint, prefix, policy, insured, intermediary, grouper, business,
                                                                  dueDate, issuanceDateFrom, issuanceDateTo, cancellationPolicyType, userId);

                            //DateTime accountingDate = _parameterServiceEEProvider.GetAccountingDate(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]));

                            foreach (Policy policyCancel in policies)
                            {
                                /*
                                //Se obtiene la tasa de cambio dado la moneda
                                decimal exchangeRate = _commonService.GetCurrencyExchangeRate(DateTime.Now, pendingPolicy.CurrencyId);

                                PremiumReceivableSearchPolicyDTO premiumReceivablePolicyDTO = new PremiumReceivableSearchPolicyDTO();
                                premiumReceivablePolicyDTO.PolicyId = pendingPolicy.PolicyId;
                                premiumReceivablePolicyDTO.EndorsementId = pendingPolicy.EndorsementId;
                                premiumReceivablePolicyDTO.PayerId = pendingPolicy.PayerId;
                                premiumReceivablePolicyDTO.CurrencyId = pendingPolicy.CurrencyId;
                                premiumReceivablePolicyDTO.PaymentNumber = pendingPolicy.QuotaNumber;
                                premiumReceivablePolicyDTO.Amount = pendingPolicy.Amount;
                                premiumReceivablePolicyDTO.PaidAmount = pendingPolicy.Amount;

                                SaveTempPremiumReceivableTransaction(premiumReceivablePolicyDTO, tempImputation.Id, exchangeRate, policy.userId);

                                totalToApplyPositive += pendingPolicy.Amount;
                                if (Convert.ToDecimal(pendingPolicy.Amount) < 0)
                                {
                                    //Crédito
                                    pendingPolicy.Amount = -pendingPolicy.Amount;

                                    SaveTempAccountingTransaction(branch.Id, pendingPolicy.PayerId, Convert.ToInt32(AccountingNature.Credit), creditAccountingAccountId,
                                                                  pendingPolicy.CurrencyId, pendingPolicy.Amount, exchangeRate, tempImputation.Id, creditAccountingConceptId);

                                    //totalToApplyNegative += pendingPolicy.Amount;
                                }
                                else
                                {
                                    //Débito
                                    SaveTempAccountingTransaction(branch.Id, pendingPolicy.PayerId, Convert.ToInt32(AccountingNature.Debit), debitAccountingAccountId,
                                                                  pendingPolicy.CurrencyId, pendingPolicy.Amount, exchangeRate, tempImputation.Id, debitAccountingConceptId);

                                    //totalToApplyPositive += pendingPolicy.Amount;
                                }

                                recordsProcessed++;
                                UpdateLogSpecialProcess(processId, totalToApplyPositive, exchangeRate, recordsProcessed, policies.Count, tempImputation.Id);
                                */
                            }

                            transaction.Complete();
                        }
                        catch (BusinessException exception)
                        {
                            transaction.Dispose();

                            processId = -1;
                        }
                    }
                }
                else
                {
                    //UpdateLogPolicyCancelaltion(amortization, 0, 0, DateTime.Now, 0, 0);
                }
            }
            catch
            {
                processId = -1;
            }

            return processId;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// GetPoliciesToCancel
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="salePoint"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="intermediary"></param>
        /// <param name="grouper"></param>
        /// <param name="business"></param>
        /// <param name="dueDate"></param>
        /// <param name="issuanceDateFrom"></param>
        /// <param name="issuanceDateTo"></param>
        /// <param name="cancellationPolicyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<Policy> GetPoliciesToCancel(Branch branch, SalePoint salePoint, Prefix prefix,
                                                 Policy policy, Person insured, Person intermediary,
                                                 int grouper, int business, DateTime dueDate,
                                                 DateTime issuanceDateFrom, DateTime issuanceDateTo,
                                                 CancellationPolicyType cancellationPolicyType, int userId)
        {
            try
            {
                int rows;
                List<Policy> policies = new List<Policy>();
                
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyToCancel.Properties.BusinessTypeCode, business);

                if (branch.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyToCancel.Properties.BranchCode, branch.Id);
                }
                if (prefix.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyToCancel.Properties.PrefixCode, prefix.Id);
                }
                if (policy.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyToCancel.Properties.PolicyId, policy.Id);
                }
                if (policy.DocumentNumber > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyToCancel.Properties.PolicyNumber, policy.DocumentNumber);
                }
                if (insured.IndividualId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyToCancel.Properties.InsuredId, insured.IndividualId);
                }
                if (policy.CurrentFrom.ToString("dd/MM/yyyy") != "01/01/1900")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyToCancel.Properties.IssueDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(policy.CurrentFrom);
                }
                if (policy.CurrentTo.ToString("dd/MM/yyyy") != "01/01/1900")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyToCancel.Properties.IssueDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(policy.CurrentTo);
                }

                UIView policyAmortization = _dataFacadeManager.GetDataFacade().GetView("PolicyAmortizationView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (policyAmortization.Count > 0)
                {
                    foreach (DataRow row in policyAmortization)
                    {
                        /*
                        if (Math.Abs(Convert.ToDecimal(row["PaymentAmount"])) <= amortizedAmount)
                        {
                            policies.Add(new Policy()
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
                        */
                    }

                }

                return policies;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
