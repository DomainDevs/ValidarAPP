using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AccountingModelsBankAccounts = Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using AccountPayableModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using System.Configuration;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System.Text;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingAccountServiceEEProvider : IAccountingAccountService
    {
        #region Constants

        // Variable para Id de módulo para generar la contabilización de imputación en EE
        private const int ImputationAccountingModule = 10;

        #endregion Constants

        #region Instance Viarables

        #region Interfaz

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        private readonly CheckPaymentOrderDAO _checkPaymentOrderDAO = new CheckPaymentOrderDAO();
        private readonly TransferPaymentOrderDAO _transferPaymentOrderDAO = new TransferPaymentOrderDAO();
        private readonly VoucherConceptDAO _voucherConceptDAO = new VoucherConceptDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region Parameters

        /// <summary>
        /// GetAccountingParameters
        /// Metodo para obtener los parámetros para armar la contabilización de un ingreso de caja
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<AccountingParameterDTO/></returns>
        public List<AccountingParameterDTO> GetAccountingParameters(int collectId)
        {
            try
            {
                AccountingAccountBusiness accountBusiness = new AccountingAccountBusiness();
                return accountBusiness.GetAccountingParameters(collectId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetApplicationParameters
        /// Metodo para obtener los parámetros para armar la contabilización de un ingreso de caja
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ApplicationParameterDTO></returns>
        public List<ApplicationParameterDTO> GetApplicationParameters(int collectId, int imputationTypeId)
        {
            List<ApplicationParameterDTO> applicationParameters = new List<ApplicationParameterDTO>();
            decimal totalAmount = 0;

            try
            {
                int rows;
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, imputationTypeId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, collectId);

                UIView parameters = _dataFacadeManager.GetDataFacade().GetView("ApplicationParametersView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (parameters.Count > 0)
                {
                    //calculo el total
                    foreach (DataRow row in parameters)
                    {
                        totalAmount = totalAmount + Convert.ToDecimal(row["Amount"]);
                    }

                    foreach (DataRow row in parameters)
                    {
                        applicationParameters.Add(new ApplicationParameterDTO()
                        {
                            ImputationTypeId = Convert.ToInt32(row["ImputationTypeCode"]),
                            SourceId = Convert.ToInt32(row["SourceCode"]),
                            EndorsementId = Convert.ToInt32(row["EndorsementId"]),
                            PolicyId = Convert.ToInt32(row["PolicyId"]),
                            BranchId = Convert.ToInt32(row["BranchCode"]),
                            BusinessTypeId = Convert.ToInt32(row["BusinessTypeCode"]),
                            ComponentCollectionId = Convert.ToInt32(row["ComponentCollectionCode"]),
                            ComponentId = Convert.ToInt32(row["ComponentId"]),
                            IncomeAmount = Convert.ToDecimal(row["IncomeAmount"]),
                            CurrencyId = Convert.ToInt32(row["CurrencyCode"]),
                            ExchangeRate = Convert.ToDecimal(row["ExchangeRate"]),
                            Amount = Convert.ToDecimal(row["Amount"]),
                            TotalAmount = totalAmount,
                            IndividualId = Convert.ToInt32(row["IndividualId"])
                        });
                    }
                }

                return applicationParameters;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPrefixComponentCollectionsByComponent
        /// Método para cargar los ramos y subramos de cada componente.
        /// </summary>
        /// <param name="applicationParameterDto"></param>
        /// <returns>List<ApplicationParameterDTO></returns>
        public List<ApplicationParameterDTO> GetPrefixComponentCollectionsByComponent(List<ApplicationParameterDTO> applicationParameterDto)
        {
            try
            {
                foreach (ApplicationParameterDTO applicationParameter in applicationParameterDto)
                {
                    applicationParameter.ComponentCollection = new List<PrefixComponentCollectionDTO>();

                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixComponentCollection.Properties.ComponentCollectionCode, applicationParameter.ComponentCollectionId);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PrefixComponentCollection), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (BusinessObject businessObject in businessCollection)
                        {
                            ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollection = (ACCOUNTINGEN.PrefixComponentCollection)businessObject;
                            applicationParameter.ComponentCollection.Add(new PrefixComponentCollectionDTO()
                            {
                                PrefixComponentId = prefixComponentCollection.PrefixComponentCollectionCode,
                                ComponentCollectionId = Convert.ToInt32(prefixComponentCollection.ComponentCollectionCode),
                                LineBusinessId = Convert.ToInt32(prefixComponentCollection.LineBusinessCode),
                                SubLineBusinessId = Convert.ToInt32(prefixComponentCollection.SubLineBusinessCode)
                            });
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return applicationParameterDto;
        }

        /// <summary>
        /// GetCheckBallotAccountingParameters
        /// Metodo para obtener los parámetros para armar la contabilización de boleta de depósito de cheques
        /// </summary>
        /// <param name="paymentBallotId"></param>
        /// <returns>List<CheckBallotAccountingParameterDTO/></returns>
        public List<CheckBallotAccountingParameterDTO> GetCheckBallotAccountingParameters(int paymentBallotId)
        {
            List<CheckBallotAccountingParameterDTO> checkBallotAccountingParameters = new List<CheckBallotAccountingParameterDTO>();
            decimal totalAmount = 0;
            decimal totalCashAmount = 0;
            decimal totalCommissionAmount = 0;

            try
            {
                int rows;
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.PaymentBallotCode, paymentBallotId);

                UIView accountingParameters = _dataFacadeManager.GetDataFacade().GetView("PaymentBallotPaymentTicketView",
                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (accountingParameters.Count > 0)
                {
                    foreach (DataRow row in accountingParameters)
                    {
                        totalAmount = totalAmount + Convert.ToDecimal(row["Amount"]);
                        totalCashAmount = totalCashAmount + Convert.ToDecimal(row["CashAmount"]);
                        totalCommissionAmount = totalCommissionAmount + Convert.ToDecimal(row["CommissionAmount"]);
                    }

                    decimal totalBallotAmount = Convert.ToDecimal(accountingParameters.Rows[0]["BallotAmount"]);

                    checkBallotAccountingParameters.Add(new CheckBallotAccountingParameterDTO()
                    {
                        PaymentBallotCode = Convert.ToInt32(accountingParameters.Rows[0]["PaymentBallotCode"]),
                        BankCode = Convert.ToInt32(accountingParameters.Rows[0]["BankCode"]),
                        AccountNumber = Convert.ToString(accountingParameters.Rows[0]["AccountNumber"]),
                        CurrencyCode = Convert.ToInt32(accountingParameters.Rows[0]["CurrencyCode"]),
                        RegisterDate = Convert.ToDateTime(accountingParameters.Rows[0]["RegisterDate"]),
                        BranchCode = Convert.ToInt32(accountingParameters.Rows[0]["BranchCode"]),
                        Amount = Convert.ToDecimal(totalAmount),
                        CommissionAmount = Convert.ToDecimal(totalCommissionAmount),
                        CashAmount = Convert.ToDecimal(totalCashAmount),
                        BallotAmount = Convert.ToDecimal(totalBallotAmount),
                        BallotNumber = Convert.ToString(accountingParameters.Rows[0]["PaymentBallotNumber"])
                    });
                }

                return checkBallotAccountingParameters;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputationParameters
        /// Obtiene la imputación, junto con todos los movimientos involucrados
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="moduleDateId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        public List<ImputationParameterDTO> GetImputationParameters(int sourceId, int imputationTypeId, int userId, int moduleId, int subModuleId, int moduleDateId)
        {
            int imputationId = 0;
            int branchId = 0;
            DateTime registerDate = DateTime.Now;
            string description = "";
            int individualId = 0;
            int preLiquidationCode = moduleId;

            moduleId = 0;

            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            try
            {
                #region Imputation

                // Obtengo id de imputación.
                imputationId = GetImputationParametersImputationId(sourceId, imputationTypeId);

                #endregion Imputation

                if (imputationId > 0)
                {
                    #region Collect

                    if (imputationTypeId == Convert.ToInt32(ApplicationTypes.Collect))
                    {
                        ImputationParameterDTO accountingParameterItem = GetCollectAccountingImputationParameter(sourceId, moduleId, subModuleId, moduleDateId, userId, imputationId, imputationTypeId);
                        accountingParameters.Add(accountingParameterItem);
                    }

                    #endregion Collect

                    #region DailyEntry

                    // Genero los registros para asiento diario
                    if (imputationTypeId == Convert.ToInt32(ApplicationTypes.JournalEntry))
                    {
                        // Obtengo algunos datos necesarios
                        ObjectCriteriaBuilder journalEntryFilter = new ObjectCriteriaBuilder();
                        journalEntryFilter.PropertyEquals(ACCOUNTINGEN.JournalEntry.Properties.JournalEntryCode, sourceId);

                        BusinessCollection journalEntryCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.JournalEntry), journalEntryFilter.GetPredicate()));

                        if (journalEntryCollection.Count > 0)
                        {
                            ACCOUNTINGEN.JournalEntry journalEntryEntity = journalEntryCollection.OfType<ACCOUNTINGEN.JournalEntry>().First();
                            branchId = Convert.ToInt32(journalEntryEntity.BranchCode);
                            registerDate = Convert.ToDateTime(journalEntryEntity.AccountingDate);
                            description = Convert.ToString(journalEntryEntity.Description);
                            individualId = Convert.ToInt32(journalEntryEntity.IndividualId);
                        }
                    }

                    #endregion DailyEntry

                    #region Preliquidation

                    // Genero los registros para asiento diario
                    if (imputationTypeId == Convert.ToInt32(ApplicationTypes.PreLiquidation))
                    {
                        // Obtengo algunos datos necesarios
                        ObjectCriteriaBuilder preliquidationFilter = new ObjectCriteriaBuilder();
                        preliquidationFilter.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.PreliquidationCode, preLiquidationCode);
                        BusinessCollection preliquidationCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Preliquidation), preliquidationFilter.GetPredicate()));

                        if (preliquidationCollection.Count > 0)
                        {
                            ACCOUNTINGEN.Preliquidation preliquidationEntity = preliquidationCollection.OfType<ACCOUNTINGEN.Preliquidation>().First();
                            branchId = Convert.ToInt32(preliquidationEntity.BranchCode);
                            registerDate = Convert.ToDateTime(preliquidationEntity.RegisterDate);
                            description = Convert.ToString(preliquidationEntity.Description);
                            individualId = Convert.ToInt32(preliquidationEntity.IndividualId);
                        }
                    }

                    #endregion Preliquidation

                    #region PaymentOrder

                    // Genero los registros para orden de pago
                    if (imputationTypeId == Convert.ToInt32(ApplicationTypes.PaymentOrder))
                    {
                        ImputationParameterDTO accountingParameterItem = GetPaymentOrderImputationParameter(sourceId, moduleId, subModuleId, moduleDateId, userId, imputationId, imputationTypeId);
                        accountingParameters.Add(accountingParameterItem);
                    }

                    #endregion PaymentOrder

                    #region PremiumReceivableTrans

                    List<ImputationParameterDTO> premiumReceivableTransImputationParameters = GetPremiumReceivableTransImputationParameters(imputationId, sourceId, branchId, moduleDateId, registerDate, description, userId, imputationTypeId);

                    if (premiumReceivableTransImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(premiumReceivableTransImputationParameters);
                    }

                    #endregion PremiumReceivable

                    #region BrokerCheckingAccount

                    //obtengo cuentas corrientes de agentes
                    List<ImputationParameterDTO> brokerCheckingAccountImputationParameters = GetBrokerCheckingAccountImputationParameters(imputationId, sourceId, branchId, moduleDateId, registerDate, description, userId, imputationTypeId);

                    if (brokerCheckingAccountImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(brokerCheckingAccountImputationParameters);
                    }

                    #endregion BrokerCheckingAccount

                    #region CoinsuranceCheckingAccount

                    //obtengo cuenta corriente de coaseguros
                    List<ImputationParameterDTO> coinsuranceCheckingAccountImputationParameters = GetCoinsuranceCheckingAccountImputationParameters(imputationId, sourceId, branchId, moduleDateId, registerDate, description, userId, imputationTypeId);

                    if (coinsuranceCheckingAccountImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(coinsuranceCheckingAccountImputationParameters);
                    }

                    #endregion CoinsuranceCheckingAccount

                    #region ReinsuranceCheckingAccountTransaction

                    //obtengo cuenta corriente de reaseguros
                    List<ImputationParameterDTO> reinsuranceCheckingAccountImputationParameters = GetReinsuranceCheckingAccountImputationParameters(imputationId, sourceId, branchId, moduleDateId, registerDate, description, userId, imputationTypeId);

                    if (reinsuranceCheckingAccountImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(reinsuranceCheckingAccountImputationParameters);
                    }

                    #endregion ReinsuranceCheckingAccountTransaction

                    #region DailyAccouting

                    //obtengo los movimientos de contabilidad
                    List<ImputationParameterDTO> dailyAccountingImputationParameters = GetDailyAccountingImputationParameters(imputationId, sourceId, branchId, moduleDateId, registerDate, description, userId, imputationTypeId);

                    if (dailyAccountingImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(dailyAccountingImputationParameters);
                    }

                    #endregion DailyAccouting

                    #region Claims

                    //obtengo los movimientos de solicitud de pagos (varios/siniestros)
                    List<ImputationParameterDTO> claimsImputationParameters = GetClaimsImputationParameters(imputationId, sourceId, branchId, moduleDateId, registerDate, description, userId, imputationTypeId);

                    if (claimsImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(claimsImputationParameters);
                    }

                    #endregion Claims

                    #region Balance

                    List<ImputationParameterDTO> balanceImputationParameters = BalanceImputationParameters(imputationTypeId, accountingParameters, moduleId, sourceId, branchId, individualId, moduleDateId, registerDate, description, userId, subModuleId, imputationId);

                    if (balanceImputationParameters.Count > 0)
                    {
                        accountingParameters.AddRange(balanceImputationParameters);
                    }

                    #endregion Balance
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingParameters;
        }

        #endregion Parameters        

        /// <summary>
        /// GetAccountingAccountIdByConceptId
        /// </summary>
        /// <param name="paymentConceptId"></param>
        /// <returns>int</returns>
        public int GetAccountingAccountIdByConceptId(int paymentConceptId)
        {
            int accountingAccountId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AccountingAccountConcept.Properties.AccountingConceptCode, paymentConceptId);//PaymentConceptCode

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.AccountingAccountConcept), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.AccountingAccountConcept accountingAccountConceptEntity in businessCollection.OfType<ACCOUNTINGEN.AccountingAccountConcept>())
                    {
                        accountingAccountId = accountingAccountConceptEntity.AccountingAccountId != null ? Convert.ToInt32(accountingAccountConceptEntity.AccountingAccountId) : 0;
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return accountingAccountId;
        }

        /// <summary>
        /// GetPaymentRequestAccountingParameters
        /// Método para obtener los parámetros para contabilización de solicitud de pagos/ solicitud de pagos varios
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="paymentSourceId"></param>
        /// <param name="moduleId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="userId"></param>
        /// <returns>List<PaymentRequestAccountingParameterDTO/></returns>
        public List<PaymentRequestAccountingParameterDTO> GetPaymentRequestAccountingParameters(int paymentRequestId, int paymentSourceId, int moduleId, int subModuleId, int moduleDateId, int userId)
        {
            List<PaymentRequestAccountingParameterDTO> paymentRequestParameters = new List<PaymentRequestAccountingParameterDTO>();

            try
            {
                int rows;
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequest.Properties.PaymentRequestId, paymentRequestId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequest.Properties.ConceptSourceCode, paymentSourceId);

                //armo el registro para la cabecera de la solicitud de pagos
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentRequest), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (BusinessObject businessObject in businessCollection)
                    {
                        ACCOUNTINGEN.PaymentRequest paymentRequestEntity = (ACCOUNTINGEN.PaymentRequest)businessObject;
                        PaymentRequestAccountingParameterDTO accountingParameterItem = new PaymentRequestAccountingParameterDTO();

                        accountingParameterItem.SourceCode = paymentRequestId;
                        accountingParameterItem.BranchCode = Convert.ToInt32(paymentRequestEntity.BranchCode);
                        accountingParameterItem.IncomeAmount = Math.Abs(Convert.ToDecimal(paymentRequestEntity.TotalAmount));
                        accountingParameterItem.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, Convert.ToInt32(paymentRequestEntity.CurrencyCode)).SellAmount;
                        accountingParameterItem.CurrencyCode = Convert.ToInt32(paymentRequestEntity.CurrencyCode);
                        accountingParameterItem.Amount = Math.Abs(Convert.ToDecimal(paymentRequestEntity.TotalAmount));
                        accountingParameterItem.PayerId = Convert.ToInt32(paymentRequestEntity.BeneficiaryCode);
                        accountingParameterItem.AccountingConceptId = 0; //se obtiene de las cuentas de orden
                        accountingParameterItem.AccountingAccountId = 0;

                        paymentRequestParameters.Add(accountingParameterItem);
                    }
                }

                // Armo los registros para los comprobantes de la solicitud de pagos
                List<ACCOUNTINGEN.GetbranchV> dataAccountingParameters = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.GetbranchV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetbranchV>().ToList();
                if (dataAccountingParameters.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetbranchV accountingParameters in dataAccountingParameters)
                    {
                        // Se obtiene los impuestos
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.VoucherConceptTax.Properties.VoucherConceptCode, accountingParameters.VoucherConceptId);

                        businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.VoucherConceptTax), criteriaBuilder.GetPredicate()));

                        decimal taxValue = 0;

                        foreach (ACCOUNTINGEN.VoucherConceptTax voucherConceptTaxEntity in businessCollection.OfType<ACCOUNTINGEN.VoucherConceptTax>())
                        {
                            taxValue += Convert.ToDecimal(voucherConceptTaxEntity.TaxValue);
                        }

                        PaymentRequestAccountingParameterDTO accountingParameterItem = new PaymentRequestAccountingParameterDTO();

                        accountingParameterItem.SourceCode = paymentRequestId;
                        accountingParameterItem.BranchCode = accountingParameters.BranchCode == 0 ? -1 : (int)accountingParameters.BranchCode;
                        accountingParameterItem.IncomeAmount = Math.Abs(accountingParameters.Amount == 0 ? 0 : (decimal)accountingParameters.Amount) + Math.Round(taxValue, 2); //Value
                        accountingParameterItem.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, Convert.ToInt32(accountingParameters.CurrencyCode)).SellAmount;
                        accountingParameterItem.CurrencyCode = Convert.ToInt32(accountingParameters.CurrencyCode);// *** EN LA MONEDA EL 0 ES EL VALOR DE PESOS CAMBIAR POR -1 PRODUCE DESCUADRE EN LA CONTABILIZACIÓN *** 
                        accountingParameterItem.Amount = Math.Abs(Convert.ToDecimal(accountingParameters.Amount == 0 ? 0 : (decimal)accountingParameters.Amount) + Math.Round(taxValue, 2)); //Value
                        accountingParameterItem.PayerId = accountingParameters.BeneficiaryCode == 0 ? -1 : (int)accountingParameters.BeneficiaryCode;
                        accountingParameterItem.AccountingConceptId = accountingParameters.AccountingConceptCode;
                        accountingParameterItem.AccountingAccountId = 0;
                        paymentRequestParameters.Add(accountingParameterItem);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return paymentRequestParameters;
        }

        /// <summary>
        /// Obtengo el concepto de pago a través del id de la solicitud de pagos.
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns>int</returns>
        public int GetPaymentConceptByPaymentRequestId(int paymentRequestId)
        {
            int conceptId = 0;
            int voucherConceptId = 0;

            try
            {
                //compruebo que la solicitud exista.
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequest.Properties.PaymentRequestId, paymentRequestId);
                BusinessCollection paymentRequestCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentRequest), criteriaBuilder.GetPredicate()));

                if (paymentRequestCollection.Count > 0)
                {
                    // Obtengo el id de voucher concept.
                    ObjectCriteriaBuilder paymentRequestClaimFilter = new ObjectCriteriaBuilder();
                    paymentRequestClaimFilter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.PaymentRequestCode, paymentRequestId);
                    BusinessCollection paymentRequestClaimCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(PAYMEN.PaymentRequestClaim), paymentRequestClaimFilter.GetPredicate()));

                    if (paymentRequestClaimCollection.Count > 0)
                    {
                        foreach (BusinessObject businessObject in paymentRequestClaimCollection)
                        {
                            PAYMEN.PaymentRequestClaim paymentRequestClaimEntity = (PAYMEN.PaymentRequestClaim)businessObject;
                            voucherConceptId = Convert.ToInt32(paymentRequestClaimEntity.PaymentVoucherConceptCode);
                        }
                    }

                    // Obtengo el concepto por voucher
                    if (voucherConceptId > 0)
                    {
                        List<AccountPayableModels.VoucherConcept> voucherConcepts = _voucherConceptDAO.GetVoucherConcepts();
                        voucherConcepts = voucherConcepts.Where(r => (r.Id.Equals(voucherConceptId))).ToList();

                        if (voucherConcepts.Count > 0)
                        {
                            foreach (AccountPayableModels.VoucherConcept voucherConcept in voucherConcepts)
                            {
                                conceptId = Convert.ToInt32(voucherConcept.AccountingConcept.Id);
                            }
                        }
                    }
                }
            }
            catch (BusinessException)
            {
                conceptId = 0;
            }

            return conceptId;
        }

        #region  Calculo  de  coaseguro cedido o coaseguro aceptado

        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="applicationParameterDto"></param>
        /// <returns>List<ApplicationParameterDTO/></returns>
        public List<ApplicationParameterDTO> Calculate(List<ApplicationParameterDTO> applicationParameterDto)
        {
            int rows;
            decimal givenPercetage = 0;
            decimal leftPercetage = 0;

            UIView issueCoinsurances = new UIView();

            List<ApplicationParameterDTO> calculatedParameters = new List<ApplicationParameterDTO>();

            ApplicationParameterDTO firstApplicationParameter = applicationParameterDto.First();

            if (firstApplicationParameter.BusinessTypeId == 2)
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssueCoinsuranceAccepted.Properties.PolicyId, firstApplicationParameter.PolicyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssueCoinsuranceAccepted.Properties.EndorsementId, firstApplicationParameter.EndorsementId);

                issueCoinsurances = _dataFacadeManager.GetDataFacade().GetView("IssueCoinsuranceAcceptedView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
            }

            if (firstApplicationParameter.BusinessTypeId == 3)
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssueCoinsuranceAssigned.Properties.PolicyId, firstApplicationParameter.PolicyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssueCoinsuranceAssigned.Properties.EndorsementId, firstApplicationParameter.EndorsementId);

                issueCoinsurances = _dataFacadeManager.GetDataFacade().GetView("IssueCoinsuranceAssignedView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
            }

            // Calculo el porcentaje que le queda a la compañía principal
            foreach (DataRow row in issueCoinsurances)
            {
                givenPercetage = givenPercetage + Convert.ToDecimal(row["PartCiaPercentage"]);
            }

            leftPercetage = 100 - givenPercetage;

            foreach (ApplicationParameterDTO application in applicationParameterDto)
            {

                if (application.BusinessTypeId == 2)
                {
                    foreach (DataRow row in issueCoinsurances)
                    {
                        calculatedParameters.Add(new ApplicationParameterDTO()
                        {
                            ImputationTypeId = application.ImputationTypeId,
                            SourceId = application.SourceId,
                            EndorsementId = application.EndorsementId,
                            PolicyId = application.PolicyId,
                            BranchId = application.BranchId,
                            BusinessTypeId = application.BusinessTypeId,
                            ComponentId = application.ComponentId,
                            CurrencyId = application.CurrencyId,
                            ExchangeRate = application.ExchangeRate,
                            Amount = Math.Round(Convert.ToDecimal(row["PartCiaPercentage"]) * application.Amount / 100, 2),
                            IncomeAmount = Math.Round(Convert.ToDecimal(row["PartCiaPercentage"]) * application.IncomeAmount / 100, 2)
                        });
                    }
                }

                if (application.BusinessTypeId == 3)
                {
                    foreach (DataRow row in issueCoinsurances)
                    {
                        calculatedParameters.Add(new ApplicationParameterDTO()
                        {
                            ImputationTypeId = application.ImputationTypeId,
                            SourceId = application.SourceId,
                            EndorsementId = application.EndorsementId,
                            PolicyId = application.PolicyId,
                            BranchId = application.BranchId,
                            BusinessTypeId = application.BusinessTypeId,
                            ComponentId = application.ComponentId,
                            CurrencyId = application.CurrencyId,
                            ExchangeRate = application.ExchangeRate,
                            Amount = Math.Round(Convert.ToDecimal(row["PartCiaPercentage"]) * application.Amount / 100, 2),
                            IncomeAmount = Math.Round(Convert.ToDecimal(row["PartCiaPercentage"]) * application.IncomeAmount / 100, 2)
                        });
                    }
                }

                // Aumento los porcentajes de la compañía principal
                calculatedParameters.Add(new ApplicationParameterDTO()
                {
                    ImputationTypeId = application.ImputationTypeId,
                    SourceId = application.SourceId,
                    EndorsementId = application.EndorsementId,
                    PolicyId = application.PolicyId,
                    BranchId = application.BranchId,
                    BusinessTypeId = application.BusinessTypeId,
                    ComponentId = application.ComponentId,
                    CurrencyId = application.CurrencyId,
                    ExchangeRate = application.ExchangeRate,
                    Amount = Math.Round(leftPercetage * application.Amount / 100, 2),
                    IncomeAmount = Math.Round(leftPercetage * application.IncomeAmount / 100, 2)
                });
            }
            return calculatedParameters;
        }

        #endregion

        public int AccountingParametersRequest(SaveBillParametersDTO saveBillParametersDTO)
        {
            try
            {
                AccountingAccountBusiness accounting = new AccountingAccountBusiness();
                return accounting.AccountingParametersRequest(saveBillParametersDTO);
            }
            catch (BusinessException businExc)
            {
                throw new BusinessException(businExc.Message);
            }
        }

        public int AccountingChecks(SaveBillParametersDTO saveBillParametersDTO)
        {
            decimal billIncomeAmount = 0;
            decimal billAmount = 0;
            string description = null;

            //Get transaction number
            CollectDTO collect = new CollectDTO();
            CollectApplicationDTO collectApplicationDTO = new CollectApplicationDTO();
            collect.Id = saveBillParametersDTO.Collect.Id;
            DTOs.Imputations.ApplicationDTO applicationDTO = new DTOs.Imputations.ApplicationDTO();
            //Se obtienen los parámetros para ejecutar 
            List<AccountingParameterDTO> accountingParameter = GetAccountingParameters(saveBillParametersDTO.Collect.Id);


            //Se generan los parámetro para la cabecera del recibo.
            foreach (AccountingParameterDTO accountingItem in accountingParameter)
            {
                billIncomeAmount = billIncomeAmount + accountingItem.IncomeAmount;
                billAmount = billAmount + accountingItem.Amount;
            }

            AccountingParametersDTO accountingParameters = new AccountingParametersDTO();
            accountingParameters.JournalEntryListParameters = new List<AccountingListParametersDTO>();
            foreach (AccountingParameterDTO accountingParam in accountingParameter)
            {
                AccountingListParametersDTO accountingListParametersDTO = new AccountingListParametersDTO();
                accountingListParametersDTO.CurrencyCode = accountingParam.CurrencyCode;
                accountingListParametersDTO.BranchCode = accountingParam.BranchCode;
                accountingListParametersDTO.Amount = accountingParam.Amount;
                accountingListParametersDTO.LocalAmount = accountingParam.IncomeAmount;
                accountingListParametersDTO.ExchangeRate = accountingParam.ExchangeRate;
                accountingListParametersDTO.PayerId = accountingParam.PayerId;
                accountingListParametersDTO.PaymentCode = accountingParam.PaymentCode;
                accountingListParametersDTO.PaymentMethodTypeCode = accountingParam.PaymentMethodTypeCode;


                if (accountingParam.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.DepositVoucher)
                    || accountingParam.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                {
                    int bankId = accountingParam.ReceivingBankCode;
                    string bankAccountNumber = accountingParam.ReceivingAccountingNumber;
                    if (accountingParam.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                    {
                        bankId = accountingParam.IssuingBankCode;
                        bankAccountNumber = accountingParam.IssuingAccountingNumber;
                    }

                    accountingListParametersDTO.AccountingNature = (int)AccountingNature.Debit;
                    accountingListParametersDTO.AccountingAccountId =
                        DelegateService.accountingApplicationService.GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(bankId, accountingParam.CurrencyCode, bankAccountNumber);
                }
                accountingParameters.JournalEntryListParameters.Add(accountingListParametersDTO);
            }

            CollectControlBusiness collectControlBusiness = new CollectControlBusiness();

            accountingParameters.BillId = saveBillParametersDTO.Collect.Id;
            accountingParameters.Description = description;
            accountingParameters.TypeId = saveBillParametersDTO.TypeId;//estado pago
            accountingParameters.UserId = saveBillParametersDTO.UserId;
            accountingParameters.CollectTechnicalTransaction = saveBillParametersDTO.Collect.Transaction.TechnicalTransaction;
            accountingParameters.CollectPaymentCode = saveBillParametersDTO.Collect.Payments.Where(x => x.Amount.Value > 0).FirstOrDefault().Id;//new COllect
            accountingParameters.PaymentCode = saveBillParametersDTO.PaymentCode;//rechazo
            accountingParameters.AccountingDate = collectControlBusiness.GetAccountingDateByCollectId(saveBillParametersDTO.Collect.Id);
            accountingParameters.TechnicalTransaction = saveBillParametersDTO.TechnicalTransaction;
            accountingParameters.BridgeAccoutingId = saveBillParametersDTO.BridgeAccoutingId;
            accountingParameters.BridgePackageCode = saveBillParametersDTO.BridgePackageCode;

            string parameters = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParameters);
            int entryNumber = DelegateService.accountingGeneralLedgerApplicationService.JournalEntryChecks(parameters);

            return entryNumber;
        }

        /// <summary>
        /// Realiza grabación (ingreso y contabilidad) incluido boleta interna y deposito (aplica solo cuando el metodo de pago es consignación de cheque)
        /// </summary>
        /// <param name="collectGeneralLedgerDTO"></param>
        /// <returns></returns>
        public MessageSuccessDTO SaveCollectGeneralLedgerPayment(CollectGeneralLedgerDTO collectGeneralLedgerDTO)
        {
            try
            {
                AccountingAccountBusiness accountBusiness = new AccountingAccountBusiness();
                return accountBusiness.SaveCollectGeneralLedgerPayment(collectGeneralLedgerDTO);

            }
            catch (BusinessException exc)
            {

                throw;
            }
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// GetGeneralLedgerCode
        /// Obtiene el Id de la cuenta bancaria usando el id de la orden de pago
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="paymentType"></param>
        /// <returns>string</returns>
        private string GetGeneralLedgerCode(int sourceId, int paymentType)
        {
            string accountingAccountId = "0";

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            AccountingModelsBankAccounts.BankAccountCompany bankAccountCompany = new AccountingModelsBankAccounts.BankAccountCompany();

            if (paymentType == (int)PaymentType.Check)
            {
                int checkPaymentOrderId = 0;
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderCheckPayment.Properties.PaymentOrderCode, sourceId);

                BusinessCollection checkBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentOrderCheckPayment), criteriaBuilder.GetPredicate()));

                if (checkBusinessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.PaymentOrderCheckPayment paymentOrderCheckPaymentEntity in checkBusinessCollection.OfType<ACCOUNTINGEN.PaymentOrderCheckPayment>())
                    {
                        checkPaymentOrderId = Convert.ToInt32(paymentOrderCheckPaymentEntity.CheckPaymentOrderCode);
                    }
                }

                AccountPayableModels.CheckPaymentOrder checkPaymentOrder = new AccountPayableModels.CheckPaymentOrder();
                checkPaymentOrder.Id = checkPaymentOrderId;
                checkPaymentOrder = _checkPaymentOrderDAO.GetCheckPaymentOrder(checkPaymentOrder);
                bankAccountCompany = new AccountingParameterServiceEEProvider().GetBankAccountCompany(checkPaymentOrder.BankAccountCompany.ToDTO()).ToModel();
                accountingAccountId = bankAccountCompany.AccountingAccount.AccountingAccountId.ToString();
            }
            if (paymentType == (int)PaymentType.Transfer)
            {
                int transferPaymentOrderId = 0;
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferPayment.Properties.PaymentOrderCode, sourceId);

                BusinessCollection transferBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentOrderTransferPayment), criteriaBuilder.GetPredicate()));

                if (transferBusinessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.PaymentOrderTransferPayment paymentOrderTransferPaymentEntity in transferBusinessCollection.OfType<ACCOUNTINGEN.PaymentOrderTransferPayment>())
                    {
                        transferPaymentOrderId = Convert.ToInt32(paymentOrderTransferPaymentEntity.TransferPaymentOrderCode);
                    }
                }

                AccountPayableModels.TransferPaymentOrder transferPaymentOrder = new AccountPayableModels.TransferPaymentOrder();

                if (transferPaymentOrderId > 0)
                {
                    transferPaymentOrder.Id = transferPaymentOrderId;
                    transferPaymentOrder = _transferPaymentOrderDAO.GetTransferPaymentOrder(transferPaymentOrder);

                    bankAccountCompany = new BankAccountCompanyDAO().GetBankAccountCompany(transferPaymentOrder.BankAccountCompany);

                    accountingAccountId = bankAccountCompany.AccountingAccount.AccountingAccountId.ToString();
                }
            }

            return accountingAccountId;
        }

        /// <summary>
        /// GetImputationParametersImputationId
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>int</returns>
        private int GetImputationParametersImputationId(int sourceId, int imputationTypeId)
        {
            int imputationId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, sourceId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, imputationTypeId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    imputationId = Convert.ToInt32(businessCollection.OfType<ACCOUNTINGEN.Application>().First().ApplicationCode);
                }
            }
            catch (Exception)
            {
                imputationId = 0;
            }

            return imputationId;
        }

        /// <summary>
        /// GetCollectAccountingImputationParameter
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="moduleId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="moduleDateId"></param>e
        /// <param name="userId"></param>
        /// <param name="imputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>ImputationParameterDTO</returns>
        private ImputationParameterDTO GetCollectAccountingImputationParameter(int sourceId, int moduleId, int subModuleId, int moduleDateId, int userId, int imputationId, int imputationTypeId)
        {
            ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

            // Obtengo el valor del collect
            ObjectCriteriaBuilder collectFilter = new ObjectCriteriaBuilder();
            collectFilter.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, sourceId);

            BusinessCollection collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), collectFilter.GetPredicate()));

            if (collectCollection.Count > 0)
            {
                int collectIndividualId = 0;
                decimal collectIncomeAmount = 0;
                decimal collectAmount = 0;
                int collectControlId = 0;
                int branchId = 0;
                string description;

                ACCOUNTINGEN.Collect collectEntity = collectCollection.OfType<ACCOUNTINGEN.Collect>().First();
                collectIncomeAmount = Convert.ToDecimal(collectEntity.PaymentsTotal);
                collectAmount = Convert.ToDecimal(collectEntity.PaymentsTotal);
                collectIndividualId = Convert.ToInt32(collectEntity.IndividualId);
                DateTime registerDate = Convert.ToDateTime(collectEntity.RegisterDate);
                description = "POR APLICACION DE RECIBO NO: ";
                collectControlId = Convert.ToInt32(collectEntity.CollectControlCode);

                // Obtengo el branch a partir del collectControlId
                ObjectCriteriaBuilder collectControlFilter = new ObjectCriteriaBuilder();
                collectControlFilter.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.CollectControlId, collectControlId);

                BusinessCollection collectControlCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), collectControlFilter.GetPredicate()));

                if (collectControlCollection.Count > 0)
                {
                    branchId = Convert.ToInt32(collectControlCollection.OfType<ACCOUNTINGEN.CollectControl>().First().BranchCode);
                }

                // Se arma el movimiento para el recibo

                accountingParameterItem.ModuleId = moduleId;
                accountingParameterItem.SubModuleId = subModuleId;
                accountingParameterItem.SourceCode = sourceId;
                accountingParameterItem.BranchCode = branchId;
                accountingParameterItem.IncomeAmount = collectIncomeAmount;
                accountingParameterItem.ExchangeRate = 1; //los recibos se están grabando en moneda local
                accountingParameterItem.CurrencyCode = 0; //los recibos se están grabando en moneda local
                accountingParameterItem.Amount = collectAmount;
                accountingParameterItem.PayerId = collectIndividualId;
                accountingParameterItem.ModuleDateId = moduleDateId;
                accountingParameterItem.RegisterDate = registerDate;
                accountingParameterItem.Description = description;
                accountingParameterItem.UserId = userId;
                accountingParameterItem.AccountingNature = (int)AccountingNature.Debit;
                accountingParameterItem.AccountingAccountId = 0;
                accountingParameterItem.MovementType = (int)MovementTypes.PremiumReceivable; //tipo de movimiento
                accountingParameterItem.ImputationId = imputationId;
                accountingParameterItem.ImputationTypeId = imputationTypeId;
                accountingParameterItem.Component = null;
                accountingParameterItem.BusinessTypeId = 0;
                accountingParameterItem.PrefixId = 0;
                accountingParameterItem.BankReconciliationId = 0;
                accountingParameterItem.ReceiptNumber = 0;
                accountingParameterItem.ReceiptDate = new DateTime();
            }

            return accountingParameterItem;
        }

        /// <summary>
        /// GetPaymentOrderImputationParameter
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="moduleId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="userId"></param>
        /// <param name="imputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>ImputationParameterDTO</returns>
        private ImputationParameterDTO GetPaymentOrderImputationParameter(int sourceId, int moduleId, int subModuleId, int moduleDateId, int userId, int imputationId, int imputationTypeId)
        {
            ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

            decimal paymentOrderAmount = 0;
            decimal paymentOrderIncomeAmount = 0;
            decimal paymentOrderExchangeRate = 0;
            int paymentOrderCurrencyId = 0;
            int branchId = 0;
            DateTime registerDate;
            string description;
            int individualId = 0;

            // Obtengo los datos necesarios
            ObjectCriteriaBuilder paymentOrderFilter = new ObjectCriteriaBuilder();
            paymentOrderFilter.PropertyEquals(ACCOUNTINGEN.PaymentOrder.Properties.PaymentOrderCode, sourceId);

            BusinessCollection paymentOrderCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentOrder), paymentOrderFilter.GetPredicate()));

            if (paymentOrderCollection.Count > 0)
            {
                ACCOUNTINGEN.PaymentOrder paymentOrderEntity = paymentOrderCollection.OfType<ACCOUNTINGEN.PaymentOrder>().First();
                branchId = Convert.ToInt32(paymentOrderEntity.BranchCode);
                registerDate = Convert.ToDateTime(paymentOrderEntity.RegisterDate);
                description = "POR IMPUTACIÓN DE LA OP: ";
                individualId = Convert.ToInt32(paymentOrderEntity.IndividualId);
                paymentOrderAmount = paymentOrderAmount + Convert.ToDecimal(paymentOrderEntity.Amount);
                paymentOrderIncomeAmount = paymentOrderIncomeAmount + Convert.ToDecimal(paymentOrderEntity.IncomeAmount);
                paymentOrderExchangeRate = paymentOrderExchangeRate + Convert.ToDecimal(paymentOrderEntity.ExchangeRate);
                paymentOrderCurrencyId = Convert.ToInt32(paymentOrderEntity.CurrencyCode);

                accountingParameterItem.ModuleId = moduleId;
                accountingParameterItem.SubModuleId = (int)MovementTypes.PremiumReceivable;
                accountingParameterItem.SourceCode = sourceId;
                accountingParameterItem.BranchCode = branchId;
                accountingParameterItem.IncomeAmount = paymentOrderIncomeAmount;
                accountingParameterItem.ExchangeRate = paymentOrderExchangeRate;
                accountingParameterItem.CurrencyCode = paymentOrderCurrencyId;
                accountingParameterItem.Amount = paymentOrderAmount;
                accountingParameterItem.PayerId = individualId;
                accountingParameterItem.ModuleDateId = moduleDateId;
                accountingParameterItem.RegisterDate = registerDate;
                accountingParameterItem.Description = description;
                accountingParameterItem.UserId = userId;
                accountingParameterItem.AccountingNature = (int)AccountingNature.Credit;
                accountingParameterItem.AccountingAccountId = Convert.ToInt32(GetGeneralLedgerCode(sourceId, subModuleId));
                accountingParameterItem.MovementType = (int)MovementTypes.Imputation; //tipo de movimiento
                accountingParameterItem.ImputationId = imputationId;
                accountingParameterItem.ImputationTypeId = imputationTypeId;
                accountingParameterItem.Component = null;
                accountingParameterItem.BusinessTypeId = 0;
                accountingParameterItem.PrefixId = 0;
                accountingParameterItem.BankReconciliationId = 0;
                accountingParameterItem.ReceiptNumber = 0;
                accountingParameterItem.ReceiptDate = new DateTime();
            }

            return accountingParameterItem;
        }

        /// <summary>
        /// GetPremiumReceivableTransImputationParameters
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetPremiumReceivableTransImputationParameters(int imputationId, int sourceId, int branchId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationTypeId)
        {

            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            // Obtengo las primas por cobrar
            ObjectCriteriaBuilder premiumRecievableFilter = new ObjectCriteriaBuilder();


            // premiumRecievableFilter.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.ImputationCode, imputationId);
            // BusinessCollection premiumReceivableCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PremiumReceivableTrans), premiumRecievableFilter.GetPredicate()));

            /*  if (premiumReceivableCollection.Count > 0)
              {
                  foreach (BusinessObject businessObject in premiumReceivableCollection)
                  {
                      ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableEntity = (ACCOUNTINGEN.PremiumReceivableTrans)businessObject;

                      int businessTypeId = 0;
                      int prefixId = 0;

                      ObjectCriteriaBuilder receivableFilter = new ObjectCriteriaBuilder();
                      receivableFilter.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyId, premiumReceivableEntity.PolicyId);
                      receivableFilter.And();
                      receivableFilter.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.EndorsementId, premiumReceivableEntity.EndorsementId);

                      BusinessCollection receivableCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PolicyPremiumReceivableTrans), receivableFilter.GetPredicate()));

                      if (receivableCollection.Any())
                      {
                          ACCOUNTINGEN.PolicyPremiumReceivableTrans receivablePremiumSearchPolicy = receivableCollection.OfType<ACCOUNTINGEN.PolicyPremiumReceivableTrans>().First();
                          businessTypeId = receivablePremiumSearchPolicy.BusinessTypeCode;
                          prefixId = receivablePremiumSearchPolicy.PrefixCode;
                      }

                      decimal exchangeRate = Convert.ToDecimal(premiumReceivableEntity.ExchangeRate);
                      int premiumReceivableCurrencyId = Convert.ToInt32(premiumReceivableEntity.CurrencyCode);
                      int premiumReceivableIndividualId = Convert.ToInt32(premiumReceivableEntity.PayerId);

                      if (premiumReceivableEntity.DiscountedCommission != 0)
                      {
                          ImputationParameterDTO accountingParameterItemCredits = new ImputationParameterDTO();

                          accountingParameterItemCredits.ModuleId = ImputationAccountingModule;
                          accountingParameterItemCredits.SubModuleId = (int)MovementTypes.DiscountedCommission;
                          accountingParameterItemCredits.SourceCode = sourceId;
                          accountingParameterItemCredits.BranchCode = branchId;
                          accountingParameterItemCredits.IncomeAmount = (decimal)premiumReceivableEntity.DiscountedCommission;
                          accountingParameterItemCredits.ExchangeRate = exchangeRate;
                          accountingParameterItemCredits.CurrencyCode = premiumReceivableCurrencyId;
                          accountingParameterItemCredits.Amount = Convert.ToDecimal(premiumReceivableEntity.DiscountedCommission) * Convert.ToDecimal(premiumReceivableEntity.ExchangeRate);
                          accountingParameterItemCredits.PayerId = premiumReceivableIndividualId;
                          accountingParameterItemCredits.ModuleDateId = moduleDateId;
                          accountingParameterItemCredits.RegisterDate = registerDate;
                          accountingParameterItemCredits.Description = description;
                          accountingParameterItemCredits.UserId = userId;
                          accountingParameterItemCredits.AccountingNature = (int)AccountingNature.Debit;
                          accountingParameterItemCredits.AccountingAccountId = 0;
                          accountingParameterItemCredits.MovementType = (int)MovementTypes.DiscountedCommission; //tipo de movimiento
                          accountingParameterItemCredits.ImputationId = imputationId;
                          accountingParameterItemCredits.ImputationTypeId = imputationTypeId;
                          accountingParameterItemCredits.Component = null;
                          accountingParameterItemCredits.BusinessTypeId = businessTypeId;
                          accountingParameterItemCredits.PrefixId = prefixId;
                          accountingParameterItemCredits.BankReconciliationId = 0;
                          accountingParameterItemCredits.ReceiptNumber = 0;
                          accountingParameterItemCredits.ReceiptDate = new DateTime();

                          accountingParameters.Add(accountingParameterItemCredits);
                      }

                      List<ImputationParameterDTO> depositPremiumTransactionParameters = GetDepositPremiumTransactionImputationParameters(premiumReceivableEntity.PremiumReceivableTransId, sourceId, branchId, exchangeRate, premiumReceivableCurrencyId, premiumReceivableIndividualId, moduleDateId, registerDate, description, userId, imputationId, imputationTypeId, businessTypeId, prefixId);

                      if (depositPremiumTransactionParameters.Count > 0)
                      {
                          accountingParameters.AddRange(depositPremiumTransactionParameters);
                      }

                      List<ImputationParameterDTO> componentCollectionTransactionParameters = GetComponentCollectionImputationParameters(premiumReceivableEntity.PremiumReceivableTransId, sourceId, branchId, exchangeRate, premiumReceivableCurrencyId, premiumReceivableIndividualId, moduleDateId, registerDate, description, userId, imputationId, imputationTypeId, businessTypeId, prefixId);

                      if (componentCollectionTransactionParameters.Count > 0)
                      {
                          accountingParameters.AddRange(componentCollectionTransactionParameters);
                      }
                  }
              }*/

            return accountingParameters;
        }

        /// <summary>
        /// GetDepositPremiumTransactionImputationParameters
        /// </summary>
        /// <param name="premiumReceivableTransId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="premiumReceivableCurrencyId"></param>
        /// <param name="premiumReceivableIndividualId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="businessTypeId"></param>
        /// <param name="prefixId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetDepositPremiumTransactionImputationParameters(int premiumReceivableTransId, int sourceId, int branchId, decimal exchangeRate, int premiumReceivableCurrencyId, int premiumReceivableIndividualId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationId, int imputationTypeId, int businessTypeId, int prefixId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            decimal depositPremiums = 0;
            decimal depositPremiumsLocalValue = 0;

            ObjectCriteriaBuilder depositFilter = new ObjectCriteriaBuilder();
            depositFilter.PropertyEquals(ACCOUNTINGEN.DepositPremiumTransaction.Properties.PremiumReceivableTransCode, premiumReceivableTransId);

            BusinessCollection depositCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.DepositPremiumTransaction), depositFilter.GetPredicate()));

            if (depositCollection.Count != 0)
            {
                foreach (BusinessObject depositBusinessObject in depositCollection)
                {
                    ACCOUNTINGEN.DepositPremiumTransaction depositPremiumTransaction = (ACCOUNTINGEN.DepositPremiumTransaction)depositBusinessObject;
                    depositPremiums = depositPremiums + Convert.ToDecimal(depositPremiumTransaction.IncomeAmount);
                    depositPremiumsLocalValue = depositPremiumsLocalValue + Convert.ToDecimal(depositPremiumTransaction.Amount);
                }

                ImputationParameterDTO accountingParameterItemCredits = new ImputationParameterDTO();

                accountingParameterItemCredits.ModuleId = ImputationAccountingModule;
                accountingParameterItemCredits.SubModuleId = (int)MovementTypes.DepositPremium;
                accountingParameterItemCredits.SourceCode = sourceId;
                accountingParameterItemCredits.BranchCode = branchId;
                accountingParameterItemCredits.IncomeAmount = depositPremiums;
                accountingParameterItemCredits.ExchangeRate = exchangeRate;
                accountingParameterItemCredits.CurrencyCode = premiumReceivableCurrencyId;
                accountingParameterItemCredits.Amount = depositPremiumsLocalValue;
                accountingParameterItemCredits.PayerId = premiumReceivableIndividualId;
                accountingParameterItemCredits.ModuleDateId = moduleDateId;
                accountingParameterItemCredits.RegisterDate = registerDate;
                accountingParameterItemCredits.Description = description;
                accountingParameterItemCredits.UserId = userId;
                accountingParameterItemCredits.AccountingNature = (int)AccountingNature.Debit;
                accountingParameterItemCredits.AccountingAccountId = 0;
                accountingParameterItemCredits.MovementType = (int)MovementTypes.DepositPremium; //tipo de movimiento
                accountingParameterItemCredits.ImputationId = imputationId;
                accountingParameterItemCredits.ImputationTypeId = imputationTypeId;
                accountingParameterItemCredits.Component = null;
                accountingParameterItemCredits.BusinessTypeId = businessTypeId;
                accountingParameterItemCredits.PrefixId = prefixId;
                accountingParameterItemCredits.BankReconciliationId = 0;
                accountingParameterItemCredits.ReceiptNumber = 0;
                accountingParameterItemCredits.ReceiptDate = new DateTime();

                accountingParameters.Add(accountingParameterItemCredits);
            }

            return accountingParameters;
        }

        /// <summary>
        /// GetComponentCollectionImputationParameters
        /// </summary>
        /// <param name="premiumReceivableTransId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="premiumReceivableCurrencyId"></param>
        /// <param name="premiumReceivableIndividualId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="businessTypeId"></param>
        /// <param name="prefixId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetComponentCollectionImputationParameters(int premiumReceivableTransId, int sourceId, int branchId, decimal exchangeRate, int premiumReceivableCurrencyId, int premiumReceivableIndividualId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationId, int imputationTypeId, int businessTypeId, int prefixId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            // Armo los movimientos a partir de los componentes de la póliza.
            ObjectCriteriaBuilder componentFilter = new ObjectCriteriaBuilder();
            componentFilter.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableTransId);

            BusinessCollection componentCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ComponentCollection), componentFilter.GetPredicate()));

            if (componentCollection.Count > 0)
            {
                foreach (BusinessObject componentBusinessObject in componentCollection)
                {
                    ACCOUNTINGEN.ComponentCollection componentEntity = (ACCOUNTINGEN.ComponentCollection)componentBusinessObject;
                    if (componentEntity.IncomeAmount != 0)
                    {
                        ImputationParameterDTO componentParameterItem = new ImputationParameterDTO();

                        componentParameterItem.ModuleId = ImputationAccountingModule;
                        componentParameterItem.SubModuleId = (int)MovementTypes.DepositPremium;
                        componentParameterItem.SourceCode = sourceId;
                        componentParameterItem.BranchCode = branchId;
                        componentParameterItem.IncomeAmount = (decimal)componentEntity.IncomeAmount;
                        componentParameterItem.ExchangeRate = exchangeRate;
                        componentParameterItem.CurrencyCode = premiumReceivableCurrencyId;
                        componentParameterItem.Amount = (decimal)componentEntity.Amount;
                        componentParameterItem.PayerId = premiumReceivableIndividualId;
                        componentParameterItem.ModuleDateId = moduleDateId;
                        componentParameterItem.RegisterDate = registerDate;
                        componentParameterItem.Description = description;
                        componentParameterItem.UserId = userId;
                        componentParameterItem.AccountingNature = componentEntity.Amount > 0 ? Convert.ToInt32(AccountingNature.Credit) : Convert.ToInt32(AccountingNature.Debit);
                        componentParameterItem.AccountingAccountId = 0;
                        componentParameterItem.MovementType = (int)MovementTypes.PremiumReceivable; //tipo de movimiento
                        componentParameterItem.ImputationId = imputationId;
                        componentParameterItem.ImputationTypeId = imputationTypeId;
                        componentParameterItem.Component = componentEntity.ComponentId.ToString();
                        componentParameterItem.BusinessTypeId = businessTypeId;
                        componentParameterItem.PrefixId = prefixId;
                        componentParameterItem.BankReconciliationId = 0;
                        componentParameterItem.ReceiptNumber = 0;
                        componentParameterItem.ReceiptDate = new DateTime();

                        accountingParameters.Add(componentParameterItem);
                    }
                }
            }

            return accountingParameters;
        }

        /// <summary>
        /// GetBrokerCheckingAccountImputationParameters
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetBrokerCheckingAccountImputationParameters(int imputationId, int sourceId, int branchId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationTypeId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            ObjectCriteriaBuilder brokerAccountFilter = new ObjectCriteriaBuilder();

            brokerAccountFilter.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.ApplicationCode, imputationId);
            brokerAccountFilter.And();
            brokerAccountFilter.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.IsAutomatic, 0); //excluye las liberaciones de agente que se hicieron por concepto de primas.

            BusinessCollection brokerAccountCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.BrokerCheckingAccountTrans), brokerAccountFilter.GetPredicate()));

            if (brokerAccountCollection.Count > 0)
            {
                foreach (BusinessObject businessObject in brokerAccountCollection)
                {
                    ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccount = (ACCOUNTINGEN.BrokerCheckingAccountTrans)businessObject;
                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = ImputationAccountingModule;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.BrokerCheckingAccount;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = Math.Abs(Convert.ToDecimal(brokerCheckingAccount.IncomeAmount));
                    accountingParameterItem.ExchangeRate = (decimal)brokerCheckingAccount.ExchangeRate;
                    accountingParameterItem.CurrencyCode = (int)brokerCheckingAccount.CurrencyCode;
                    accountingParameterItem.Amount = Math.Abs(Convert.ToDecimal(brokerCheckingAccount.Amount));
                    accountingParameterItem.PayerId = (int)brokerCheckingAccount.AgentId;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = (int)brokerCheckingAccount.AccountingNature;
                    accountingParameterItem.AccountingAccountId = GetAccountingAccountIdByConceptId(Convert.ToInt32(brokerCheckingAccount.CheckingAccountConceptCode));
                    accountingParameterItem.MovementType = (int)(MovementTypes.BrokerCheckingAccount); //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    accountingParameters.Add(accountingParameterItem);
                }
            }

            return accountingParameters;
        }

        /// <summary>
        /// GetCoinsuranceCheckingAccountImputationParameters
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetCoinsuranceCheckingAccountImputationParameters(int imputationId, int sourceId, int branchId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationTypeId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            ObjectCriteriaBuilder coinsuranceAccountFilter = new ObjectCriteriaBuilder();

            coinsuranceAccountFilter.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.ApplicationCode, imputationId);
            BusinessCollection coinsuranceAccountCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CoinsCheckingAccTrans), coinsuranceAccountFilter.GetPredicate()));

            if (coinsuranceAccountCollection.Count > 0)
            {
                foreach (BusinessObject businessObject in coinsuranceAccountCollection)
                {
                    ACCOUNTINGEN.CoinsCheckingAccTrans coinsCheckingAccTrans = (ACCOUNTINGEN.CoinsCheckingAccTrans)businessObject;
                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = ImputationAccountingModule;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.CoinsuranceCheckingAccount;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = (decimal)coinsCheckingAccTrans.IncomeAmount;
                    accountingParameterItem.ExchangeRate = (decimal)coinsCheckingAccTrans.ExchangeRate;
                    accountingParameterItem.CurrencyCode = (int)coinsCheckingAccTrans.CurrencyCode;
                    accountingParameterItem.Amount = (decimal)coinsCheckingAccTrans.Amount;
                    accountingParameterItem.PayerId = (int)coinsCheckingAccTrans.CoinsuredCompanyId;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = (int)coinsCheckingAccTrans.AccountingNatureCode;
                    accountingParameterItem.AccountingAccountId = GetAccountingAccountIdByConceptId(Convert.ToInt32(coinsCheckingAccTrans.CheckingAccountConceptCode));
                    accountingParameterItem.MovementType = Convert.ToInt32(MovementTypes.CoinsuranceCheckingAccount); //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    accountingParameters.Add(accountingParameterItem);
                }
            }

            return accountingParameters;
        }

        /// <summary>
        /// GetReinsuranceCheckingAccountImputationParameters
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetReinsuranceCheckingAccountImputationParameters(int imputationId, int sourceId, int branchId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationTypeId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            ObjectCriteriaBuilder reinsuranceAccountFilter = new ObjectCriteriaBuilder();

            reinsuranceAccountFilter.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ApplicationCode, imputationId);
            BusinessCollection reinsuranceAccountCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ReinsCheckingAccTrans), reinsuranceAccountFilter.GetPredicate()));

            if (reinsuranceAccountCollection.Count > 0)
            {
                foreach (BusinessObject businessObject in reinsuranceAccountCollection)
                {
                    ACCOUNTINGEN.ReinsCheckingAccTrans reinsuranceCheckingAccount = (ACCOUNTINGEN.ReinsCheckingAccTrans)businessObject;
                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = ImputationAccountingModule;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.ReinsuranceCheckingAccount;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = (decimal)reinsuranceCheckingAccount.IncomeAmount;
                    accountingParameterItem.ExchangeRate = (decimal)reinsuranceCheckingAccount.ExchangeRate;
                    accountingParameterItem.CurrencyCode = (int)reinsuranceCheckingAccount.CurrencyCode;
                    accountingParameterItem.Amount = (decimal)reinsuranceCheckingAccount.Amount;
                    accountingParameterItem.PayerId = (int)reinsuranceCheckingAccount.ReinsuranceCompanyId;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = (int)reinsuranceCheckingAccount.AccountingNature;
                    accountingParameterItem.AccountingAccountId = GetAccountingAccountIdByConceptId(Convert.ToInt32(reinsuranceCheckingAccount.CheckingAccountConceptCode));
                    accountingParameterItem.MovementType = (int)MovementTypes.ReinsuranceCheckingAccount; //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    accountingParameters.Add(accountingParameterItem);
                }
            }

            return accountingParameters;
        }

        /// <summary>
        /// GetDailyAccountingImputationParameters
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetDailyAccountingImputationParameters(int imputationId, int sourceId, int branchId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationTypeId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            ObjectCriteriaBuilder dailyAccountingFilter = new ObjectCriteriaBuilder();

            dailyAccountingFilter.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.AppCode, imputationId);
            BusinessCollection dailyAccountingCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ApplicationAccounting), dailyAccountingFilter.GetPredicate()));

            if (dailyAccountingCollection.Count > 0)
            {
                foreach (BusinessObject businessObject in dailyAccountingCollection)
                {
                    ACCOUNTINGEN.ApplicationAccounting entityApplicationAccounting = (ACCOUNTINGEN.ApplicationAccounting)businessObject;
                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = ImputationAccountingModule;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.DailyAccounting;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = (decimal)entityApplicationAccounting.Amount;
                    accountingParameterItem.ExchangeRate = (decimal)entityApplicationAccounting.ExchangeRate;
                    accountingParameterItem.CurrencyCode = (int)entityApplicationAccounting.CurrencyCode;
                    accountingParameterItem.Amount = (decimal)entityApplicationAccounting.Amount;
                    accountingParameterItem.PayerId = (int)entityApplicationAccounting.IndividualCode;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = (int)entityApplicationAccounting.AccountingNature;
                    accountingParameterItem.AccountingAccountId = Convert.ToInt32(entityApplicationAccounting.AccountingConceptCode) == 0 ? Convert.ToInt32(entityApplicationAccounting.AccountingAccountCode) : GetAccountingAccountIdByConceptId(Convert.ToInt32(entityApplicationAccounting.AccountingConceptCode));
                    accountingParameterItem.MovementType = (int)MovementTypes.DailyAccounting; //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = (int)entityApplicationAccounting.BankReconciliationCode;
                    accountingParameterItem.ReceiptNumber = (int)entityApplicationAccounting.ReceiptNumber;
                    accountingParameterItem.ReceiptDate = Convert.ToDateTime(entityApplicationAccounting.ReceiptDate);

                    accountingParameters.Add(accountingParameterItem);
                }
            }

            return accountingParameters;
        }

        /// <summary>
        /// GetClaimsImputationParameters
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> GetClaimsImputationParameters(int imputationId, int sourceId, int branchId, int moduleDateId, DateTime registerDate, string description, int userId, int imputationTypeId)
        {
            List<ImputationParameterDTO> accountingParameters = new List<ImputationParameterDTO>();

            ObjectCriteriaBuilder claimPaymentFilter = new ObjectCriteriaBuilder();

            claimPaymentFilter.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.ApplicationCode, imputationId);
            BusinessCollection claimPaymentColletion = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ClaimPaymentRequestTrans), claimPaymentFilter.GetPredicate()));

            if (claimPaymentColletion.Count > 0)
            {
                foreach (BusinessObject businessObject in claimPaymentColletion)
                {
                    ACCOUNTINGEN.ClaimPaymentRequestTrans claimPaymentRequestTrans = (ACCOUNTINGEN.ClaimPaymentRequestTrans)businessObject;
                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = ImputationAccountingModule;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.PaymentRequest;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = (decimal)claimPaymentRequestTrans.IncomeAmount;
                    accountingParameterItem.ExchangeRate = (decimal)claimPaymentRequestTrans.ExchangeRate;
                    accountingParameterItem.CurrencyCode = (int)claimPaymentRequestTrans.CurrencyCode;
                    accountingParameterItem.Amount = (decimal)claimPaymentRequestTrans.Amount;
                    accountingParameterItem.PayerId = (int)claimPaymentRequestTrans.BeneficiaryId;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = (int)AccountingNature.Debit;//confirmar
                    accountingParameterItem.AccountingAccountId = GetPaymentConceptByPaymentRequestId(Convert.ToInt32(claimPaymentRequestTrans.PaymentRequestCode)); //no existe cuenta contable en este movimiento
                    accountingParameterItem.MovementType = (int)MovementTypes.PaymentRequest; //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    accountingParameters.Add(accountingParameterItem);
                }
            }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(ACCOUNTINGEN.PaymentRequestTrans.Properties.ApplicationCode, imputationId);

            BusinessCollection paymentRequestTrans = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(ACCOUNTINGEN.PaymentRequestTrans), filter.GetPredicate()));

            if (paymentRequestTrans.Count > 0)
            {
                int paymentRequestId = 0;
                decimal exchangeRateTrans = 0;
                foreach (ACCOUNTINGEN.PaymentRequestTrans paymentRequestTransItem in paymentRequestTrans)
                {
                    paymentRequestId = Convert.ToInt32(paymentRequestTransItem.PaymentRequestCode);
                    exchangeRateTrans = Convert.ToDecimal(paymentRequestTransItem.ExchangeRate);
                }

                ObjectCriteriaBuilder filterPayment = new ObjectCriteriaBuilder();

                filterPayment.PropertyEquals(ACCOUNTINGEN.PaymentRequest.Properties.PaymentRequestId, paymentRequestId);
                BusinessCollection paymentColletion = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PaymentRequest), filterPayment.GetPredicate()));

                foreach (BusinessObject businessObject in paymentColletion)
                {
                    ACCOUNTINGEN.PaymentRequest paymentRequest = (ACCOUNTINGEN.PaymentRequest)businessObject;
                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = ImputationAccountingModule;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.PaymentRequest;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = (decimal)paymentRequest.TotalAmount;
                    accountingParameterItem.ExchangeRate = exchangeRateTrans;
                    accountingParameterItem.CurrencyCode = (int)paymentRequest.CurrencyCode;
                    accountingParameterItem.Amount = (decimal)paymentRequest.TotalAmount;
                    accountingParameterItem.PayerId = (int)paymentRequest.BeneficiaryCode;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = (int)AccountingNature.Debit;//confirmar
                    accountingParameterItem.AccountingAccountId = GetPaymentConceptByPaymentRequestId(Convert.ToInt32(paymentRequest.PaymentRequestId)); //no existe cuenta contable en este movimiento
                    accountingParameterItem.MovementType = (int)MovementTypes.PaymentRequest; //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    accountingParameters.Add(accountingParameterItem);
                }
            }

            return accountingParameters;
        }

        /// <summary>
        /// BalanceImputationParameters
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="accountingParameters"></param>
        /// <param name="moduleId"></param>
        /// <param name="sourceId"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <param name="moduleDateId"></param>
        /// <param name="registerDate"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <param name="subModuleId"></param>
        /// <param name="imputationId"></param>
        /// <returns>List<ImputationParameterDTO/></returns>
        private List<ImputationParameterDTO> BalanceImputationParameters(int imputationTypeId, List<ImputationParameterDTO> accountingParameters, int moduleId, int sourceId, int branchId, int individualId, int moduleDateId, DateTime registerDate, string description, int userId, int subModuleId, int imputationId)
        {
            List<ImputationParameterDTO> balanceAccountingParameters = new List<ImputationParameterDTO>();

            decimal debits = 0;
            decimal debitsLocal = 0;
            decimal credits = 0;
            decimal creditsLocal = 0;

            if (imputationTypeId == Convert.ToInt32(ApplicationTypes.PaymentOrder))
            {
                if (accountingParameters.Count == 1)// cuando se contabiliza la orden de pago a agentes.
                {
                    int accountingNature;

                    accountingNature = accountingParameters[0].AccountingNature == Convert.ToInt32(AccountingNature.Debit) ? Convert.ToInt32(AccountingNature.Credit) : Convert.ToInt32(AccountingNature.Debit);

                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = moduleId;
                    accountingParameterItem.SubModuleId = (int)MovementTypes.PremiumReceivable;
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = accountingParameters[0].IncomeAmount;
                    accountingParameterItem.ExchangeRate = accountingParameters[0].ExchangeRate;
                    accountingParameterItem.CurrencyCode = accountingParameters[0].CurrencyCode;
                    accountingParameterItem.Amount = accountingParameters[0].Amount;
                    accountingParameterItem.PayerId = individualId;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = accountingNature;
                    accountingParameterItem.AccountingAccountId = Convert.ToInt32(GetGeneralLedgerCode(sourceId, subModuleId));
                    accountingParameterItem.MovementType = Convert.ToInt32(MovementTypes.BrokerCheckingAccount); //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    balanceAccountingParameters.Add(accountingParameterItem);
                }

            }
            if (imputationTypeId == Convert.ToInt32(ApplicationTypes.PreLiquidation))
            {
                int accountingNature = 0;
                decimal counterPartIncomeAmount = 0;
                decimal counterPartLocalAmount = 0;

                if (accountingParameters.Count > 0)// calculo la contraparte
                {
                    //valido los debitos y los créditos
                    foreach (ImputationParameterDTO item in accountingParameters)
                    {
                        if (item.AccountingNature == Convert.ToInt32(AccountingNature.Debit))
                        {
                            debits = debits + Convert.ToDecimal(item.IncomeAmount);
                            debitsLocal = debitsLocal + Convert.ToDecimal(item.Amount);
                        }
                        if (item.AccountingNature == Convert.ToInt32(AccountingNature.Credit))
                        {
                            credits = credits + Convert.ToDecimal(item.IncomeAmount);
                            creditsLocal = creditsLocal + Convert.ToDecimal(item.Amount);
                        }
                    }

                    if (Math.Abs(debitsLocal) > Math.Abs(creditsLocal))
                    {
                        counterPartIncomeAmount = debits - credits;
                        counterPartLocalAmount = debitsLocal - creditsLocal;
                        accountingNature = Convert.ToInt32(AccountingNature.Credit);
                    }
                    if (Math.Abs(debitsLocal) < Math.Abs(creditsLocal))
                    {
                        counterPartIncomeAmount = credits - debits;
                        counterPartLocalAmount = creditsLocal - debitsLocal;
                        accountingNature = Convert.ToInt32(AccountingNature.Debit);
                    }

                    ImputationParameterDTO accountingParameterItem = new ImputationParameterDTO();

                    accountingParameterItem.ModuleId = moduleId;
                    accountingParameterItem.SubModuleId = Convert.ToInt32(MovementTypes.PremiumReceivable);
                    accountingParameterItem.SourceCode = sourceId;
                    accountingParameterItem.BranchCode = branchId;
                    accountingParameterItem.IncomeAmount = counterPartIncomeAmount;
                    accountingParameterItem.ExchangeRate = accountingParameters[0].ExchangeRate;
                    accountingParameterItem.CurrencyCode = accountingParameters[0].CurrencyCode;
                    accountingParameterItem.Amount = counterPartLocalAmount;
                    accountingParameterItem.PayerId = individualId;
                    accountingParameterItem.ModuleDateId = moduleDateId;
                    accountingParameterItem.RegisterDate = registerDate;
                    accountingParameterItem.Description = description;
                    accountingParameterItem.UserId = userId;
                    accountingParameterItem.AccountingNature = accountingNature;
                    accountingParameterItem.AccountingAccountId = 0;
                    accountingParameterItem.MovementType = (int)MovementTypes.PremiumReceivable; //tipo de movimiento
                    accountingParameterItem.ImputationId = imputationId;
                    accountingParameterItem.ImputationTypeId = imputationTypeId;
                    accountingParameterItem.Component = null;
                    accountingParameterItem.BusinessTypeId = 0;
                    accountingParameterItem.PrefixId = 0;
                    accountingParameterItem.BankReconciliationId = 0;
                    accountingParameterItem.ReceiptNumber = 0;
                    accountingParameterItem.ReceiptDate = new DateTime();

                    balanceAccountingParameters.Add(accountingParameterItem);
                }
            }

            return balanceAccountingParameters;
        }

        

        #endregion Private Methods

    }
}

