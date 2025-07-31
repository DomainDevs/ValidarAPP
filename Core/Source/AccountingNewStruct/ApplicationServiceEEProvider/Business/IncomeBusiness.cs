using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Framework.Queries;
using MODACC = Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Framework.Views;
using System.Data;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Framework.BAF;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PRMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using claimsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting.Reversion;
using Sistran.Core.Framework.Contexts;
using CoreTransaction = Sistran.Core.Framework.Transactions;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class IncomeBusiness
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        readonly TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
        readonly TempBrokerCheckingAccountTransactionDAO _tempBrokerCheckingAccountTransactionDAO = new TempBrokerCheckingAccountTransactionDAO();
        readonly TempCoinsuranceCheckingAccountTransactionDAO _tempCoinsuranceCheckingAccountTransactionDAO = new TempCoinsuranceCheckingAccountTransactionDAO();
        readonly TempReinsuranceCheckingAccountTransactionDAO _tempReinsuranceCheckingAccountTransactionDAO = new TempReinsuranceCheckingAccountTransactionDAO();
        readonly TempApplicationAccountingDAO tempApplicationAccountingDAO = new TempApplicationAccountingDAO();
        readonly TempPaymentRequestTransactionDAO _tempPaymentRequestTransactionDAO = new TempPaymentRequestTransactionDAO();
        readonly TempClaimsPaymentRequestTransactionDAO _tempClaimsPaymentRequestTransactionDAO = new TempClaimsPaymentRequestTransactionDAO();
        readonly TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();
        readonly ApplicationPremiumItemDAO applicationPremiumItemDAO = new ApplicationPremiumItemDAO();
        readonly TempUsedDepositPremiumDAO _tempUsedDepositPremiumDAO = new TempUsedDepositPremiumDAO();
        readonly UsedAmountDAO _usedAmountDAO = new UsedAmountDAO();
        readonly BrokerCheckingAccountTransactionItemDAO _brokerCheckingAccountTransactionItemDAO = new BrokerCheckingAccountTransactionItemDAO();
        readonly CoinsuranceCheckingAccountTransactionItemDAO _coinsuranceCheckingAccountTransactionItemDAO = new CoinsuranceCheckingAccountTransactionItemDAO();
        readonly AgentCoinsuranceCheckingAccountDAO _agentCoinsuranceCheckingAccountDAO = new AgentCoinsuranceCheckingAccountDAO();
        readonly ComponentCollectionDAO _componentCollectionDAO = new ComponentCollectionDAO();
        readonly PrefixComponentCollectionDAO _prefixComponentCollectionDAO = new PrefixComponentCollectionDAO();
        readonly DepositPremiumTransactionDAO _depositPremiumTransactionDAO = new DepositPremiumTransactionDAO();
        readonly ReinsuranceCheckingAccountTransactionItemDAO _reinsuranceCheckingAccountTransactionItemDAO = new ReinsuranceCheckingAccountTransactionItemDAO();
        readonly TempClaimPaymentRequestDAO _tempClaimPaymentRequestDAO = new TempClaimPaymentRequestDAO();
        readonly PaymentRequestClaimDAO _paymentRequestClaimDAO = new PaymentRequestClaimDAO();
        readonly PaymentRequestDAO _paymentRequestDAO = new PaymentRequestDAO();
        readonly TempApplicationDAO tempApplicationDAO = new TempApplicationDAO();
        readonly ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();

        public MODACC.Application GetTempApplicationByTempApplicationId(int tempApplicationId, int imputationTypeId = 0, int typePayment = 0, bool deeply = true)
        {
            MODACC.Application application = new MODACC.Application();
            MODACC.Application temApplication = tempApplicationDAO.GetTempApplication(new MODACC.Application { Id = tempApplicationId });
            if (temApplication != null)
            {
                application = temApplication;
            }
            if (!deeply)
                return application;

            ApplicationPremiumTransaction applicationPremium = tempApplicationPremiumDAO.GetTempApplicationPremiumByTempApplicationId(tempApplicationId, imputationTypeId);
            if (applicationPremium != null)
            {
                application.ApplicationPremiumTransaction = applicationPremium;
            }
            BrokersCheckingAccountTransaction tempBrokersCheckingAccountTransaction = _tempBrokerCheckingAccountTransactionDAO.GetTempBrokerCheckingAccountTransactionByTempImputationId(tempApplicationId);
            if (tempBrokersCheckingAccountTransaction != null)
            {
                application.BrokersCheckingAccountTransaction = tempBrokersCheckingAccountTransaction;
            }
            CoInsuranceCheckingAccountTransaction tempCoinsuranceCheckingAccountTransaction = _tempCoinsuranceCheckingAccountTransactionDAO.GetTempCoinsuranceCheckingAccountTransactionByTempImputationId(tempApplicationId);
            if (tempCoinsuranceCheckingAccountTransaction != null)
            {
                application.CoInsuranceCheckingAccountTransaction = tempCoinsuranceCheckingAccountTransaction;
            }
            ReInsuranceCheckingAccountTransaction tempReinsuranceCheckingAccountTransaction = _tempReinsuranceCheckingAccountTransactionDAO.GetTempReinsuranceCheckingAccountTransactionByTempImputationId(tempApplicationId);
            if (tempReinsuranceCheckingAccountTransaction != null)
            {
                application.ReInsuranceCheckingAccountTransaction = tempReinsuranceCheckingAccountTransaction;
            }
            DailyAccountingTransaction tempDailyAccountingTransaction = tempApplicationAccountingDAO.GetTempApplicationAccountingByTempApplicationId(tempApplicationId);
            if (tempDailyAccountingTransaction != null)
            {
                application.DailyAccountingTransaction = tempDailyAccountingTransaction;
            }
            PaymentRequestTransaction paymentRequestTransaction = _tempPaymentRequestTransactionDAO.GetTempPaymentRequestTransactionByTempImputationId(tempApplicationId);
            if (paymentRequestTransaction != null)
            {
                application.PaymentRequestTransaction = paymentRequestTransaction;
            }
            ClaimsPaymentRequestTransaction claimsPaymentRequestTransaction = _tempClaimsPaymentRequestTransactionDAO.GetTempClaimsPaymentRequestTransactionByTempImputationId(tempApplicationId, typePayment);
            if (claimsPaymentRequestTransaction != null)
            {
                application.ClaimsPaymentRequestTransaction = claimsPaymentRequestTransaction;
            }
            return application;
        }

        public bool ConvertTempApplicationtoRealApplication(int tempSourceCode, int moduleId, int temApplicationId, DateTime acountingDate, int userId = 0)
        {
            bool isConverted = false;
            isConverted = ConvertTemptoRealPremiumReceivableTransaction(tempSourceCode, moduleId, temApplicationId, acountingDate);
            if (isConverted)
            {
                // GRABA MOVIMIENTOS CUENTA CORRIENTE AGENTES EN REALES Y ELIMINA LAS TEMPORALES
                isConverted = ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(tempSourceCode, moduleId, temApplicationId);
                if (isConverted)
                {
                    // GRABA MOVIMIENTOS CUENTA CORRIENTE REASEGURADORAS EN REALES Y ELIMINA LAS TEMPORALES
                    isConverted = ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(tempSourceCode, moduleId, temApplicationId);
                    if (isConverted)
                    {
                        // Solcitudes de pago siniestros 
                        isConverted = ConvertTempClaimPaymentRequestToClaimPaymentRequest(tempSourceCode, moduleId, temApplicationId, userId);
                        if (isConverted)
                        {
                            // GRABA MOVIMIENTOS CUENTA CORRIENTE COASEGUROS EN REALES
                            isConverted = ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(tempSourceCode, moduleId, temApplicationId);
                            if (isConverted)
                            {
                                // GRABA MOVIMIENTOS CONTABILIDAD EN REALES
                                isConverted = ConvertTempAccountingTransactionToAccountingTransaction(tempSourceCode, moduleId, temApplicationId);

                                if (isConverted)
                                {
                                    // Solcitudes de pagos varios 
                                    isConverted = ConvertTempPaymentRequestToPaymentRequest(tempSourceCode, moduleId, temApplicationId, userId);
                                }
                            }
                        }
                    }
                }
            }
            return isConverted;
        }

        ///<summary>
        /// ConvertTemptoRealPremiumReceivableTransaction
        /// Proceso de conversión de primas x cobrar
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>

        private bool ConvertTemptoRealPremiumReceivableTransaction(int sourceId, int imputationTypeId, int imputationId, DateTime accountingDate)
        {
            bool isConverted = false;

            try
            {
                bool isPremiumSaved = false;
                int depositPremiumsTransactionSaved = 0;
                bool isUsedAmountSaved = false;
                bool isComponentCollectionSaved = false;
                bool isDiscountedCommissionSaved = false;
                bool isSaved = false;
                int decimalPlaces = 2;

                //DateTime accountingDate = Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]);

                DateTime dateFrom = Convert.ToDateTime("01" + "/" + accountingDate.Month + "/" + accountingDate.Year);
                DateTime dateTo = accountingDate;

                int payerTypeId = 1; // Quemado hasta definir
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();

                try
                {
                    // Obtengo el temporal.
                    Models.Imputations.Application tempImputation = GetTempApplicationBySourceCode(imputationTypeId, sourceId).ToModel();

                    tempImputation.ApplicationItems = new List<TransactionType>();
                    ApplicationPremiumTransaction _tempApplicationPremium = tempApplicationPremiumDAO.GetTempApplicationPremiumByTempApplicationId(tempImputation.Id, Convert.ToInt32(ImputationItemTypes.PremiumsReceivable));
                    //Reversion Primas
                    ApplicationPremiumTransaction tempApplicationPremiumRev = TempApplicationPremiumReversionDAO.GetTempApplicationPremiumByTempApplicationId(tempImputation.Id);
                    DebitCreditDTO debitCreditDTO = ApplicationPremiumBusiness.CalculatePremium(Convert.ToInt32(ImputationItemTypes.PremiumsReceivable), tempApplicationPremiumRev.PremiumReceivableItems);
                    tempApplicationPremiumRev.TotalDebit = new Amount { Value = debitCreditDTO.Debit };
                    tempApplicationPremiumRev.TotalCredit = new Amount { Value = debitCreditDTO.Credit };
                    _tempApplicationPremium.TotalCredit.Value += debitCreditDTO.Credit;
                    _tempApplicationPremium.TotalDebit.Value += debitCreditDTO.Debit;
                    if (_tempApplicationPremium != null && _tempApplicationPremium.PremiumReceivableItems.Any())
                    {
                        _tempApplicationPremium.PremiumReceivableItems.AddRange(tempApplicationPremiumRev.PremiumReceivableItems);
                    }
                    else
                    {
                        _tempApplicationPremium = tempApplicationPremiumRev;
                    }
                    if (tempApplicationPremiumRev.PremiumReceivableItems != null && tempApplicationPremiumRev.PremiumReceivableItems.Any())
                    {
                        ReversionDAO.ConvertTemptoRealPremiumReversion(imputationId, tempApplicationPremiumRev.PremiumReceivableItems.Select(m => m.Id).ToList());

                    }
                    //Fin Reversion
                    tempImputation.ApplicationItems.Add(_tempApplicationPremium);

                    ApplicationDTO application = new ApplicationDTO();

                    application.UserId = tempImputation.UserId;
                    application.RegisterDate = DateTime.Now;
                    application.Id = imputationId;

                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempImputation.Id);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.TempApplicationPremium tempApplicationPremium in businessCollection.OfType<ACCOUNTINGEN.TempApplicationPremium>())
                        {
                            // Determina valores en exceso y valores a pagar.
                            decimal excessPayment = 0;
                            decimal excessPaymentLocalAmount = 0;
                            decimal collectionAmount = 0;
                            var components = applicationBusiness.GetPayableComponentstByEndorsementIdQuoutaId(
                                tempApplicationPremium.TempAppPremiumCode, tempApplicationPremium.EndorsementCode, tempApplicationPremium.PaymentNum);
                            var tempApplicationPremiumComponents = tempApplicationPremiumComponentDAO.
                                GetTempApplicationPremiumComponentsByTemAppPremium(tempApplicationPremium.TempAppPremiumCode);

                            decimal premiumAmount = Math.Round(tempApplicationPremiumComponents.Where(x => x.ComponentTinyDescription == "P").Sum(x => Math.Round(x.Amount, decimalPlaces)), decimalPlaces);
                            decimal payableAmount = Math.Round(components.Where(x => x.ComponentTinyDescription == "P").Sum(x => Math.Round(x.Amount, decimalPlaces)), decimalPlaces);
                            collectionAmount = premiumAmount;

                            // Controlar si los 2 valores son negativos
                            if (premiumAmount > 0 && payableAmount > 0)
                            {
                                if (premiumAmount > payableAmount)
                                {
                                    collectionAmount = premiumAmount;
                                    excessPayment = premiumAmount - payableAmount;
                                    excessPaymentLocalAmount = Math.Round(excessPayment * tempApplicationPremium.ExchangeRate, decimalPlaces);

                                    UpdTempApplicationPremiumComponent updateTempApplicationSourceCode = new UpdTempApplicationPremiumComponent()
                                    {
                                        ComponentCurrencyCode = tempApplicationPremium.CurrencyCode,
                                        ExchangeRate = tempApplicationPremium.ExchangeRate,
                                        TempApplicationPremiumCode = tempApplicationPremium.TempAppPremiumCode,
                                        ExpensesLocalAmount = tempApplicationPremiumComponents.Where(x => x.ComponentTinyDescription == "G").Sum(x => x.LocalAmount),
                                        TaxLocalAmount = tempApplicationPremiumComponents.Where(x => x.ComponentTinyDescription == "I").Sum(x => x.LocalAmount),
                                        PremiumAmount = payableAmount
                                    };

                                    TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                                    tempApplicationBusiness.UpdTempApplicationPremiumComponents(updateTempApplicationSourceCode);

                                    //TODO: Crear movimiento contable para exceso de prima
                                }
                            }
                            else
                            {
                                collectionAmount = Convert.ToDecimal(tempApplicationPremium.Amount);
                            }

                            PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();
                            premiumReceivableTransactionItem.Policy = new Policy();
                            premiumReceivableTransactionItem.Policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(tempApplicationPremium.EndorsementCode) };
                            premiumReceivableTransactionItem.Policy.ExchangeRate = new ExchangeRate()
                            {
                                Currency = new Currency() { Id = Convert.ToInt32(tempApplicationPremium.CurrencyCode) }
                            };
                            premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                            {
                                new Beneficiary() { IndividualId = Convert.ToInt32(tempApplicationPremium.PayerCode) }
                            };
                            premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponent>()
                            {
                                new PayerComponent()
                                {
                                    Amount = Convert.ToDecimal(payableAmount),
                                    BaseAmount = Convert.ToDecimal(collectionAmount)
                                }
                            };
                            premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                            {
                                Quotas = new List<Quota>()
                                {
                                    new Quota() { Number = Convert.ToInt32(tempApplicationPremium.PaymentNum) }
                                }
                            };

                            premiumReceivableTransactionItem.DeductCommission = new Amount() { Value = Convert.ToDecimal(tempApplicationPremium.DiscountedCommission) };

                            ApplicationPremium applicationPremiumModel = tempApplicationPremiumDAO.GetTempApplicationPremiumByTempApplicationPremiumId(tempApplicationPremium.TempAppPremiumCode);
                            applicationPremiumModel.Id = 0;
                            applicationPremiumModel.ApplicationId = imputationId;
                            applicationPremiumModel.AccountingDate = accountingDate;
                            applicationPremiumModel = applicationPremiumItemDAO.SaveApplicationPremium(applicationPremiumModel);

                            List<ApplicationPremiumCommision> applicationPremiumCommissions = applicationPremiumCommisionDAO.GetTempApplicationPremiumCommissByTempAppId(tempApplicationPremium.TempAppPremiumCode);
                            ApplicationPremiumCommision applicationPremiumCommission = new ApplicationPremiumCommision();
                            var AccountingComponentDistributionServiceEEProvider = new AccountingComponentDistributionServiceEEProvider();
                            //AccountingComponentDistributionServiceEEProvider.CreateApplicationPremiumComponent(new ParamApplicationPremiumComponent { EndorsementId = tempApplicationPremium.EndorsementCode, QuotaNumber = tempApplicationPremium.PaymentNum, PremiumId = premiumReceivableTransactionItem.Id == 0 ? 0 : premiumReceivableTransactionItem.Id , TempApplicationPremiumCode = tempApplicationPremium.TempAppPremiumCode, ApplicationAmount = collectionAmount });


                            if (applicationPremiumModel.Id != 0)
                            {
                                ApplicationPremiumBusiness applicationPremiumBussines = new ApplicationPremiumBusiness();
                                applicationPremiumBussines.SaveApplicationPremiumComponents(new ParamApplicationPremiumComponent
                                {
                                    EndorsementId = applicationPremiumModel.EndorsementId,
                                    QuotaNumber = applicationPremiumModel.PaymentNumber,
                                    PremiumId = applicationPremiumModel.Id,
                                    TempApplicationPremiumCode = tempApplicationPremium.TempAppPremiumCode,
                                    ApplicationAmount = applicationPremiumModel.Amount
                                });

                                isPremiumSaved = true;
                            }

                            // Si graba item, pasa a grabar exceso en pago, valores usados en primas en depósito, y componentes de cobranza
                            if (isPremiumSaved)
                            {
                                #region ExcessPayment

                                // GRABA PAGO EN EXCESO SI EXISTE - EXCESS_PAYMENT
                                if (excessPayment > 0)
                                {
                                    DepositPremiumTransaction depositPremiumTransaction = new DepositPremiumTransaction();
                                    depositPremiumTransaction.Id = 0;
                                    depositPremiumTransaction.Collect = new Collect();
                                    depositPremiumTransaction.Collect.Id = sourceId;
                                    depositPremiumTransaction.Collect.Payer = new Person();
                                    depositPremiumTransaction.Collect.Payer.IndividualId = Convert.ToInt32(tempApplicationPremium.PayerCode);
                                    depositPremiumTransaction.Date = Convert.ToDateTime(tempApplicationPremium.RegisterDate);
                                    depositPremiumTransaction.Amount = new Amount()
                                    {
                                        Currency = new Currency() { Id = Convert.ToInt32(tempApplicationPremium.CurrencyCode) },
                                        Value = Math.Round(excessPayment, 2)
                                    };
                                    depositPremiumTransaction.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempApplicationPremium.ExchangeRate) };
                                    depositPremiumTransaction.LocalAmount = new Amount();
                                    depositPremiumTransaction.LocalAmount.Value = Math.Round(excessPaymentLocalAmount, 2);

                                    depositPremiumsTransactionSaved = SaveDepositPremiumTransaction(depositPremiumTransaction.ToDTO(), premiumReceivableTransactionItem.Id, payerTypeId);
                                    // Si no graba el pago en exceso, borra el item grabado y la cobranza
                                    if (depositPremiumsTransactionSaved < 0)
                                    {
                                        applicationPremiumItemDAO.DeleteApplicationPremium(premiumReceivableTransactionItem.Id);
                                        isConverted = false;
                                    }
                                }

                                #endregion

                                #region UsedAmounts

                                // GRABA AMOUNTS USADOS
                                List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = _tempUsedDepositPremiumDAO.GetTempUsedDepositPremiumByTempPremiumReceivableId(tempApplicationPremium.TempAppPremiumCode);

                                if (tempUsedDepositPremiums != null)
                                {
                                    foreach (SEARCH.TempUsedDepositPremiumDTO tempUsedDepositPremiumDto in tempUsedDepositPremiums)
                                    {
                                        int usedAmount = 0;
                                        Amount amountModel = new Amount()
                                        {
                                            Currency = new Currency(),
                                            Value = tempUsedDepositPremiumDto.Amount
                                        };
                                        ExchangeRate exchangeRateModel = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempApplicationPremium.ExchangeRate) };
                                        Amount incomeAmount = new Amount() { Value = Convert.ToDecimal(tempUsedDepositPremiumDto.Amount * tempApplicationPremium.ExchangeRate) };

                                        usedAmount = _usedAmountDAO.SaveUsedAmount(amountModel, tempUsedDepositPremiumDto.DepositPremiumTransactionId, exchangeRateModel, incomeAmount);
                                        if (usedAmount != 0)
                                        {
                                            isUsedAmountSaved = true;
                                        }

                                        if (!isUsedAmountSaved)
                                        {
                                            _usedAmountDAO.DeleteUsedAmountsByDepositPremiumTransactionId(tempUsedDepositPremiumDto.DepositPremiumTransactionId);
                                            isConverted = false;
                                        }
                                    }
                                }

                                #endregion

                                #region CollectionComponents

                                isComponentCollectionSaved = SaveComponentCollectionRequest(premiumReceivableTransactionItem.Id, Convert.ToDecimal(tempApplicationPremium.ExchangeRate), Convert.ToInt32(tempApplicationPremium.CurrencyCode));
                                if (isComponentCollectionSaved)
                                {
                                    isConverted = true;
                                }

                                #endregion

                                #region DiscountedCommision

                                //se comenta debido a que se implementa las comisiones a la nueva tabla de APPLICATION PREMIUM
                                //if (applicationPremiumCommissions.Count() > 0)
                                //{
                                //    applicationPremiumCommissions = applicationPremiumCommisionDAO.CreateApplicationPremiumCommisions(applicationPremiumCommissions);           
                                //    if (applicationPremiumCommissions != null && applicationPremiumCommissions.Count() > 0 && applicationPremiumCommissions.FirstOrDefault().Id > 0)
                                //    {
                                //        isConverted = true;
                                //    }
                                //}

                                #endregion

                                //VALIDA SI ESTÁ ACTIVADA LA LIBERACIÓN EN LÍNEA
                                if (UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_RELEASE_COMMISSIONS_INLINE).ToString() == "1")
                                {
                                    //VALIDA EL TIPO DE LIBERACIÓN DE COMISIÓN
                                    if (UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_RELEASE_COMMISSIONS_PRORATE).ToString() == "1") //Liberacion de comisiones a prorrata
                                    {
                                        //Liberación comisión Prorrata
                                        /*La liberación de las coutas es por cada cobranza*/
                                        BrokersCommission(premiumReceivableTransactionItem.ToDTO(), application, sourceId);
                                    }
                                    else
                                    {
                                        //Código Original
                                        #region Liberación comisión Normal

                                        if (tempApplicationPremium.PaymentNum == 1) // La liberación de comisión solo se lo hace el momento de pagar la primera cuota.
                                        {
                                            bool isValidated = false;

                                            isValidated = ValidateAgentCommissionRelease(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id);

                                            if (isValidated)
                                            {
                                                // Obtengo los ramos y subramos
                                                criteriaBuilder = new ObjectCriteriaBuilder();
                                                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppPrefixPremiumReceivable.Properties.AppPremiumCode, premiumReceivableTransactionItem.Id);

                                                decimal exchangeRate = 0;
                                                exchangeRate = premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount;

                                                decimal amount = 0;
                                                amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;

                                                int agentTypeId = 0;

                                                UIView premiumReceivablePrefixes = _dataFacadeManager.GetDataFacade().GetView("GetAppPrefixPremiumReceivableView",
                                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                                                if (premiumReceivablePrefixes.Count > 0)
                                                {
                                                    foreach (DataRow dataRow in premiumReceivablePrefixes)
                                                    {
                                                        // Obtengo las comisiones de agente
                                                        List<SEARCH.BrokerCheckingAccountItemDTO> commissions = GetAgentCommissions(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id,
                                                            premiumReceivableTransactionItem.DeductCommission.Value, premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount, Convert.ToInt32(dataRow["LineBusinessCode"]),
                                                            Convert.ToInt32(dataRow["SubLineBusinessCode"]), application.UserId);

                                                        foreach (SEARCH.BrokerCheckingAccountItemDTO commision in commissions)
                                                        {
                                                            // Grabación en la tabla de cuentas corrientes de agentes.
                                                            BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                                            brokersCheckingAccountTransactionItem.Id = 0;
                                                            brokersCheckingAccountTransactionItem.Holder = new Agent();
                                                            brokersCheckingAccountTransactionItem.Holder.IndividualId = commision.AgentCode;
                                                            brokersCheckingAccountTransactionItem.Holder.FullName = commision.AgentName;
                                                            brokersCheckingAccountTransactionItem.Agencies = new List<Agency>()
                                                    {
                                                        new Agency() { Id = commision.AgentAgencyCode }
                                                    };
                                                            brokersCheckingAccountTransactionItem.InsuredId = commision.InsuredId;
                                                            brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                                            brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = commision.CommissionDiscounted };
                                                            brokersCheckingAccountTransactionItem.Policy = new Policy();
                                                            brokersCheckingAccountTransactionItem.Policy.Id = commision.PolicyId;
                                                            brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                                    {
                                                        new Beneficiary()
                                                        {
                                                            IndividualId = commision.InsuredId,
                                                        }
                                                    };
                                                            brokersCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = commision.EndorsementId };
                                                            brokersCheckingAccountTransactionItem.Policy.ExchangeRate = new ExchangeRate()
                                                            {
                                                                Currency = new Currency() { Id = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id }
                                                            };
                                                            brokersCheckingAccountTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                                                            {
                                                                Quotas = new List<Quota>()
                                                        {
                                                            new Quota() { Number = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number }
                                                        }
                                                            };
                                                            brokersCheckingAccountTransactionItem.PrefixId = commision.PrefixCode;
                                                            brokersCheckingAccountTransactionItem.AccountingNature = (AccountingNature)commision.AccountNature;
                                                            brokersCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = commision.CommissionAmount };
                                                            brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = commision.CommissionBalance };
                                                            brokersCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = commision.CommissionPercentage };
                                                            brokersCheckingAccountTransactionItem.CommissionType = Convert.ToInt32(commision.CommissionTypeCode);
                                                            brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = commision.CommissionDiscounted };
                                                            brokersCheckingAccountTransactionItem.Branch = new Branch() { Id = commision.BranchCode };
                                                            brokersCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = commision.SalePointCode };
                                                            brokersCheckingAccountTransactionItem.Company = new Company() { IndividualId = commision.CompanyCode };
                                                            brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = commision.CheckingAccountConceptCode };
                                                            brokersCheckingAccountTransactionItem.Amount = new Amount()
                                                            {
                                                                Currency = new Currency() { Id = Convert.ToInt32(commision.CurrencyCode) },
                                                                Value = commision.Amount
                                                            };
                                                            brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = commision.ExchangeRate };
                                                            brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                                            brokersCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = Convert.ToInt32(commision.LineBusiness) };
                                                            brokersCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = Convert.ToInt32(commision.SubLineBusiness) };
                                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem,
                                                                imputationId, 0, sourceId, commision.AgentTypeCode,
                                                                Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]));

                                                            agentTypeId = commision.AgentTypeCode;
                                                            if (brokersCheckingAccountTransactionItem.Id > 0)
                                                            {
                                                                isSaved = true;
                                                            }
                                                        }

                                                        if (isSaved)
                                                        {
                                                            int businessTypeId = GetBussinessTypeIdByPremiumReceivableId(premiumReceivableTransactionItem.Id);

                                                            #region CoinsuranceAccepted

                                                            if (businessTypeId == 2) // Coaseguro Aceptado
                                                            {
                                                                isSaved = false;

                                                                List<SEARCH.CoinsuranceCheckingAccountItemDTO> acceptedCoinsurances = GetCoinsuredAccepted(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                                                if (acceptedCoinsurances.Count > 0)
                                                                {
                                                                    foreach (SEARCH.CoinsuranceCheckingAccountItemDTO acceptedCoinsurance in acceptedCoinsurances)
                                                                    {
                                                                        List<BrokersCheckingAccountTransactionItem> brokerCheckingAccounts = GetBrokerChekingAccounts(-1, dateTo, dateTo, acceptedCoinsurance.CoinsurancePolicyId);

                                                                        if (brokerCheckingAccounts.Count > 0)
                                                                        {
                                                                            collectionAmount = 0;
                                                                            decimal collectionIncomeAmount = 0;
                                                                            foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccount in brokerCheckingAccounts)
                                                                            {
                                                                                collectionAmount = collectionAmount + Convert.ToDecimal(brokerCheckingAccount.LocalAmount.Value);
                                                                                collectionIncomeAmount = collectionIncomeAmount + Convert.ToDecimal(brokerCheckingAccount.Amount.Value);
                                                                            }
                                                                            // Grabación en la tabla de cuentas corrientes de agentes.
                                                                            BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                                                            brokersCheckingAccountTransactionItem.Id = 0;
                                                                            brokersCheckingAccountTransactionItem.InsuredId = Convert.ToInt32(acceptedCoinsurance.InsuredId);
                                                                            brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                                                            brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccounts[0].DiscountedCommission.Value) };
                                                                            brokersCheckingAccountTransactionItem.Policy = new Policy();
                                                                            brokersCheckingAccountTransactionItem.Policy.Id = Convert.ToInt32(acceptedCoinsurance.CoinsurancePolicyId);
                                                                            brokersCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(brokerCheckingAccounts[0].Policy.Endorsement.Id) };
                                                                            brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                                                    {
                                                                        new Beneficiary()
                                                                        {
                                                                            IndividualId = brokerCheckingAccounts[0].Policy.DefaultBeneficiaries[0].IndividualId,
                                                                        }
                                                                    };
                                                                            brokersCheckingAccountTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                                                                            {
                                                                                Quotas = new List<Quota>()
                                                                        {
                                                                            new Quota() { Number = 0 }
                                                                        }
                                                                            };
                                                                            brokersCheckingAccountTransactionItem.PrefixId = brokerCheckingAccounts[0].PrefixId;
                                                                            brokersCheckingAccountTransactionItem.AccountingNature = brokerCheckingAccounts[0].AccountingNature;
                                                                            brokersCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = collectionIncomeAmount };
                                                                            brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccounts[0].CommissionBalance.Value) };
                                                                            brokersCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccounts[0].CommissionPercentage.Value) };
                                                                            brokersCheckingAccountTransactionItem.CommissionType = Convert.ToInt32(brokerCheckingAccounts[0].CommissionType);
                                                                            brokersCheckingAccountTransactionItem.Holder = new Agent();
                                                                            brokersCheckingAccountTransactionItem.Holder.IndividualId = Convert.ToInt32(brokerCheckingAccounts[0].Holder.IndividualId);
                                                                            brokersCheckingAccountTransactionItem.Agencies = new List<Agency>();
                                                                            brokersCheckingAccountTransactionItem.Agencies.Add(new Agency() { Id = brokerCheckingAccounts[0].Agencies[0].Id });
                                                                            brokersCheckingAccountTransactionItem.Branch = new Branch();
                                                                            brokersCheckingAccountTransactionItem.Branch.Id = brokerCheckingAccounts[0].Branch.Id;
                                                                            brokersCheckingAccountTransactionItem.SalePoint = new SalePoint();
                                                                            brokersCheckingAccountTransactionItem.SalePoint.Id = brokerCheckingAccounts[0].SalePoint.Id;
                                                                            brokersCheckingAccountTransactionItem.Company = new Company();
                                                                            brokersCheckingAccountTransactionItem.Company.IndividualId = brokerCheckingAccounts[0].Company.IndividualId;
                                                                            brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept();
                                                                            brokersCheckingAccountTransactionItem.CheckingAccountConcept.Id = brokerCheckingAccounts[0].CheckingAccountConcept.Id;
                                                                            brokersCheckingAccountTransactionItem.Amount = new Amount()
                                                                            {
                                                                                Currency = new Currency() { Id = Convert.ToInt32(brokerCheckingAccounts[0].Amount.Currency.Id) },
                                                                                Value = collectionIncomeAmount
                                                                            };
                                                                            brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = brokerCheckingAccounts[0].ExchangeRate.SellAmount };
                                                                            brokersCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = brokerCheckingAccounts[0].PrefixId };
                                                                            brokersCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = brokerCheckingAccounts[0].SubPrefix.Id };
                                                                            brokersCheckingAccountTransactionItem.IsPayed = true;
                                                                            sourceId = GetSourceIdByApplication(application);

                                                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);
                                                                            if (brokersCheckingAccountTransactionItem.Id > 0)
                                                                            {
                                                                                isSaved = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                List<BrokersCheckingAccountTransactionItem> brokerCheckingAccountCollections = GetBrokerChekingAccountCollections(Convert.ToInt32(premiumReceivableTransactionItem.Policy.Id), dateTo, dateTo);
                                                                if (brokerCheckingAccountCollections.Count > 0)
                                                                {
                                                                    foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccountCollection in brokerCheckingAccountCollections)
                                                                    {
                                                                        // Grabación en la tabla de cuentas corrientes de agentes.
                                                                        BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                                                        brokersCheckingAccountTransactionItem = brokerCheckingAccountCollection;
                                                                        sourceId = GetSourceIdByApplication(application);

                                                                        brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);
                                                                        if (brokersCheckingAccountTransactionItem.Id > 0)
                                                                        {
                                                                            isSaved = true;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            #endregion

                                                            #region CoinsuranceGiven

                                                            if (businessTypeId == 3) // Coaseguro Cedido 
                                                            {
                                                                isSaved = false;
                                                                List<SEARCH.CoinsuranceCheckingAccountItemDTO> cededCoinsurances = GetCoinsuredAsigned(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                                                foreach (SEARCH.CoinsuranceCheckingAccountItemDTO cededCoinsurance in cededCoinsurances)
                                                                {
                                                                    CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();

                                                                    coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                                                                    coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Given;
                                                                    coinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                                                                    {
                                                                        Currency = new Currency() { Id = Convert.ToInt32(cededCoinsurance.CurrencyCode) },
                                                                        Value = Convert.ToDecimal(cededCoinsurance.Amount)
                                                                    };
                                                                    coinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(cededCoinsurance.BranchCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(cededCoinsurance.CheckingAccountConceptCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.Comments = "";
                                                                    coinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(cededCoinsurance.CompanyCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(cededCoinsurance.SalePointCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(cededCoinsurance.ExchangeRate) };
                                                                    coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(cededCoinsurance.CoinsuranceCompanyCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.Policy = new Policy();
                                                                    coinsuranceCheckingAccountTransactionItem.Policy.Id = cededCoinsurance.CoinsurancePolicyId;
                                                                    coinsuranceCheckingAccountTransactionItem.AccountingDate = accountingDate;
                                                                    coinsuranceCheckingAccountTransactionItem.LineBusiness = new LineBusiness() { Id = Convert.ToInt32(cededCoinsurance.LineBusinessCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.SubLineBusiness = new SubLineBusiness() { Id = Convert.ToInt32(cededCoinsurance.SubLineBusinessCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(cededCoinsurance.AdministrativeExpenses) };
                                                                    coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(cededCoinsurance.TaxAdministrativeExpenses) };
                                                                    coinsuranceCheckingAccountTransactionItem.ExtraCommission = new Amount() { Value = 0 };
                                                                    coinsuranceCheckingAccountTransactionItem.OverCommission = new Amount() { Value = 0 };

                                                                    // GRABA A REALES
                                                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, application.Id, 0);

                                                                    if (coinsuranceCheckingAccountTransactionItem.Id > 0)
                                                                    {
                                                                        isSaved = true;
                                                                        // Punto 5.10

                                                                        SEARCH.AgentCoinsuranceCheckingAccountDTO agentCoinsuranceCheckingAccountDto = new SEARCH.AgentCoinsuranceCheckingAccountDTO();
                                                                        agentCoinsuranceCheckingAccountDto.AgentCode = cededCoinsurance.AgentId;
                                                                        agentCoinsuranceCheckingAccountDto.AgentTypeCode = agentTypeId;
                                                                        agentCoinsuranceCheckingAccountDto.CommissionAmount = cededCoinsurance.AgentCommissionAmount;
                                                                        agentCoinsuranceCheckingAccountDto.CurrencyCode = cededCoinsurance.CurrencyCode;
                                                                        agentCoinsuranceCheckingAccountDto.IncomeCommissionAmount = cededCoinsurance.AgentCommissionIncomeAmount;

                                                                        isSaved = _agentCoinsuranceCheckingAccountDAO.SaveAgentCoinsuranceCheckingAccount(agentCoinsuranceCheckingAccountDto);
                                                                    }
                                                                }

                                                                List<SEARCH.CoinsuranceCheckingAccountItemDTO> acceptedCoinsurances = GetCoinsuredAccepted(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                                                foreach (SEARCH.CoinsuranceCheckingAccountItemDTO acceptedCoinsurance in acceptedCoinsurances)
                                                                {
                                                                    CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();

                                                                    coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                                                                    coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Given;
                                                                    coinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                                                                    {
                                                                        Currency = new Currency() { Id = Convert.ToInt32(acceptedCoinsurance.CurrencyCode) },
                                                                        Value = Convert.ToDecimal(acceptedCoinsurance.Amount)
                                                                    };
                                                                    coinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(acceptedCoinsurance.BranchCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(acceptedCoinsurance.CheckingAccountConceptCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.Comments = "";
                                                                    coinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(acceptedCoinsurance.CompanyCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(acceptedCoinsurance.SalePointCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(acceptedCoinsurance.ExchangeRate) };
                                                                    coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(acceptedCoinsurance.CoinsuranceCompanyCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.Policy = new Policy();
                                                                    coinsuranceCheckingAccountTransactionItem.Policy.Id = acceptedCoinsurance.CoinsurancePolicyId;
                                                                    coinsuranceCheckingAccountTransactionItem.AccountingDate = accountingDate;
                                                                    coinsuranceCheckingAccountTransactionItem.LineBusiness = new LineBusiness() { Id = Convert.ToInt32(acceptedCoinsurance.LineBusinessCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.SubLineBusiness = new SubLineBusiness() { Id = Convert.ToInt32(acceptedCoinsurance.SubLineBusinessCode) };
                                                                    coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(acceptedCoinsurance.AdministrativeExpenses) };
                                                                    coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(acceptedCoinsurance.TaxAdministrativeExpenses) };
                                                                    coinsuranceCheckingAccountTransactionItem.ExtraCommission = new Amount() { Value = 0 };
                                                                    coinsuranceCheckingAccountTransactionItem.OverCommission = new Amount() { Value = 0 };

                                                                    // GRABA A REALES
                                                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, application.Id, 0);

                                                                    if (coinsuranceCheckingAccountTransactionItem.Id > 0)
                                                                    {
                                                                        isSaved = true;
                                                                    }
                                                                }
                                                            }

                                                            #endregion
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                isConverted = false;
                            }
                        }
                    }

                    isConverted = true;
                }
                catch (BusinessException ex)
                {
                    throw new BusinessException(ex.Message);
                }

                return isConverted;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        ///<summary>
        /// ConvertTempBrokerCheckingAccountToBrokerCheckingAccount
        /// Proceso de conversión de temporales a reales cuenta de agente
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        private bool ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(int sourceId, int imputationTypeId, int imputationId)
        {
            bool isConverted = true;

            try
            {
                int tempBrokerParentId = 0;
                int brokerParentId = 0;

                // Obtengo el temporal.
                Models.Imputations.Application tempImputation = GetTempApplicationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ApplicationItems = new List<TransactionType>();
                BrokersCheckingAccountTransaction tempBrokersCheckingAccountTransaction = _tempBrokerCheckingAccountTransactionDAO.GetTempBrokerCheckingAccountTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ApplicationItems.Add(tempBrokersCheckingAccountTransaction);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempImputation.Id);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
                {
                    BrokersCheckingAccountTransactionItem brokerCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                    if (tempBrokerCheckingAccount.AccountingNature == 1)
                    {
                        brokerCheckingAccountTransactionItem.AccountingNature = AccountingNature.Credit;
                    }
                    else
                    {
                        brokerCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                    }

                    brokerCheckingAccountTransactionItem.Amount = new Amount()
                    {
                        Currency = new Currency() { Id = Convert.ToInt32(tempBrokerCheckingAccount.CurrencyCode) },
                        Value = Convert.ToDecimal(tempBrokerCheckingAccount.IncomeAmount.Value)
                    };
                    brokerCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(tempBrokerCheckingAccount.BranchCode) };
                    brokerCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempBrokerCheckingAccount.CheckingAccountConceptCode) };
                    brokerCheckingAccountTransactionItem.Comments = tempBrokerCheckingAccount.Description;
                    brokerCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(tempBrokerCheckingAccount.AccountingCompanyCode) };
                    brokerCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(tempBrokerCheckingAccount.SalePointCode) };
                    brokerCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempBrokerCheckingAccount.ExchangeRate) };
                    brokerCheckingAccountTransactionItem.Holder = new Agent()
                    {
                        IndividualId = Convert.ToInt32(tempBrokerCheckingAccount.AgentId),
                        FullName = tempBrokerCheckingAccount.AgentTypeCode.ToString(),

                    };
                    brokerCheckingAccountTransactionItem.Agencies = new List<Agency>()
                        {
                            new Agency() { Id = Convert.ToInt32(tempBrokerCheckingAccount.AgentAgencyId) }
                        };
                    brokerCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = 0 };
                    brokerCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = 0 };
                    brokerCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = 0 };
                    brokerCheckingAccountTransactionItem.CommissionType = 0;
                    brokerCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = 0 };
                    brokerCheckingAccountTransactionItem.InsuredId = 0;
                    brokerCheckingAccountTransactionItem.PolicyId = 0;
                    brokerCheckingAccountTransactionItem.PrefixId = 0;
                    brokerCheckingAccountTransactionItem.IsAutomatic = false;
                    brokerCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = 0 };
                    brokerCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = 0 };
                    brokerCheckingAccountTransactionItem.Policy = new Policy();
                    brokerCheckingAccountTransactionItem.Policy.Id = 0;
                    brokerCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = 0 };
                    brokerCheckingAccountTransactionItem.Policy.DocumentNumber = 0;
                    brokerCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                    {
                        new Beneficiary()
                        {
                            IndividualId = 0,
                        }
                    };
                    brokerCheckingAccountTransactionItem.Policy.PayerComponents = new List<PayerComponent>()
                    {
                        new PayerComponent()
                        {
                            Amount = 0,
                            BaseAmount = 0
                        }
                    };
                    brokerCheckingAccountTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                    {
                        Quotas = new List<Quota>()
                        {
                            new Quota { Number = 0 } // No existe este campo en la temporal de cuenta corriente de agentes.
                        }
                    };

                    // GRABA A REALES
                    brokerCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokerCheckingAccountTransactionItem, imputationId, tempBrokerParentId, sourceId, Convert.ToInt32(tempBrokerCheckingAccount.AgentTypeCode),
                                                                                                   Convert.ToDateTime(tempBrokerCheckingAccount.AccountingDate));

                    brokerParentId = brokerCheckingAccountTransactionItem.Id;

                    // ACTUALIZA A REALES
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccountItem.Properties.TempBrokerCheckingAccTransCode, tempBrokerCheckingAccount.TempBrokerCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempBrokerCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItem in businessCollectionItems.OfType<ACCOUNTINGEN.TempBrokerCheckingAccountItem>())
                    {
                        BrokersCheckingAccountTransactionItem updateBrokerCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();
                        updateBrokerCheckingAccountTransactionItem.Id = Convert.ToInt32(tempBrokerCheckingAccountItem.BrokerCheckingAccountCode);

                        _brokerCheckingAccountTransactionItemDAO.UpdateBrokerCheckingAccountTransactionItem(updateBrokerCheckingAccountTransactionItem, brokerParentId);
                    }
                }

                isConverted = true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return isConverted;
        }
        ///<summary>
        /// ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount
        /// Convierte temporal de cheque de reaseguros a reales
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        private bool ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(int sourceId, int imputationTypeId, int imputationId)
        {
            try
            {
                int tempReinsuranceParentId = 0;
                int reinsuranceParentId = 0;

                // Obtengo el temporal.
                Models.Imputations.Application tempImputation;

                tempImputation = GetTempApplicationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ApplicationItems = new List<TransactionType>();
                ReInsuranceCheckingAccountTransaction tempReinsuranceCheckingAccountTransaction;
                tempReinsuranceCheckingAccountTransaction = _tempReinsuranceCheckingAccountTransactionDAO.GetTempReinsuranceCheckingAccountTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ApplicationItems.Add(tempReinsuranceCheckingAccountTransaction);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempImputation.Id);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                {
                    ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem = new ReInsuranceCheckingAccountTransactionItem();

                    if (tempReinsuranceCheckingAccount.AccountingNature == 1)
                    {
                        reinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Credit;
                    }
                    else
                    {
                        reinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                    }
                    reinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                    {
                        Currency = new Currency() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.CurrencyCode) },
                        Value = Convert.ToDecimal(tempReinsuranceCheckingAccount.IncomeAmount.Value)
                    };
                    reinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.BranchCode) };
                    reinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.CheckingAccountConceptCode) };
                    reinsuranceCheckingAccountTransactionItem.Comments = tempReinsuranceCheckingAccount.Description;
                    reinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.AccountingCompanyCode) };
                    reinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(tempReinsuranceCheckingAccount.SalePointCode) };
                    reinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempReinsuranceCheckingAccount.ExchangeRate) };
                    reinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.ReinsuranceCompanyId) };
                    reinsuranceCheckingAccountTransactionItem.Broker = new Company() { IndividualId = Convert.ToInt32(tempReinsuranceCheckingAccount.AgentId) };
                    reinsuranceCheckingAccountTransactionItem.Prefix = new Prefix()
                    {
                        Id = Convert.ToInt32(tempReinsuranceCheckingAccount.LineBusinessCode),
                        LineBusinessId = Convert.ToInt32(tempReinsuranceCheckingAccount.SubLineBusinessCode)
                    };
                    reinsuranceCheckingAccountTransactionItem.ContractTypeId = Convert.ToInt32(tempReinsuranceCheckingAccount.ContractTypeCode);
                    reinsuranceCheckingAccountTransactionItem.ContractNumber = Convert.ToString(tempReinsuranceCheckingAccount.ContractCode); // No es ContractNumber es ContractId
                    reinsuranceCheckingAccountTransactionItem.Section = tempReinsuranceCheckingAccount.Section;
                    reinsuranceCheckingAccountTransactionItem.Region = tempReinsuranceCheckingAccount.Region;
                    reinsuranceCheckingAccountTransactionItem.Period = Convert.ToInt32(tempReinsuranceCheckingAccount.Period);
                    reinsuranceCheckingAccountTransactionItem.Year = Convert.ToInt32(tempReinsuranceCheckingAccount.ApplicationYear);
                    reinsuranceCheckingAccountTransactionItem.Month = Convert.ToInt32(tempReinsuranceCheckingAccount.ApplicationMonth);
                    reinsuranceCheckingAccountTransactionItem.PolicyId = Convert.ToInt32(tempReinsuranceCheckingAccount.PolicyId);
                    reinsuranceCheckingAccountTransactionItem.EndorsementId = Convert.ToInt32(tempReinsuranceCheckingAccount.EndorsementId);

                    // GRABA A REALES
                    reinsuranceCheckingAccountTransactionItem = _reinsuranceCheckingAccountTransactionItemDAO.SaveReinsuranceCheckingAccountTransactionItem(reinsuranceCheckingAccountTransactionItem, imputationId, tempReinsuranceParentId);

                    reinsuranceParentId = reinsuranceCheckingAccountTransactionItem.Id;

                    // ACTUALIZA A REALES
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem.Properties.TempReinsCheckingAccTransCode, tempReinsuranceCheckingAccount.TempReinsCheckingAccTransCode);
                    BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItem in businessCollectionItems.OfType<ACCOUNTINGEN.TempReinsuranceCheckingAccountItem>())
                    {
                        ReInsuranceCheckingAccountTransactionItem updateReinsuranceCheckingAccountTransactionItem = new ReInsuranceCheckingAccountTransactionItem();
                        updateReinsuranceCheckingAccountTransactionItem.Id = Convert.ToInt32(tempReinsuranceCheckingAccountItem.ReinsCheckingAccTransCode);

                        _reinsuranceCheckingAccountTransactionItemDAO.UpdateReinsuranceCheckingAccountTransactionItem(updateReinsuranceCheckingAccountTransactionItem, reinsuranceParentId);
                    }
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }
        ///<summary>
        /// ConvertTempClaimPaymentRequestToClaimPaymentRequest
        /// Convierte los temporales de pago siniestro a reales 
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="newImputationId"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        private bool ConvertTempClaimPaymentRequestToClaimPaymentRequest(int sourceId, int imputationTypeId, int newImputationId, int userId)
        {
            try
            {
                bool isConverted = true;
                int paymentNumber = 0;
                DateTime firstPaymentDue = DateTime.Now;

                // Obtengo el temporal.
                ApplicationDTO tempImputation = GetTempApplicationBySourceCode(imputationTypeId, sourceId);

                List<ACCOUNTINGEN.TempClaimPaymentReqTrans> tempClaimPayments = _tempClaimPaymentRequestDAO.GetTempClaimPayment(0, 0, tempImputation.Id, 0);

                foreach (ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPayment in tempClaimPayments)
                {
                    // Arma objeto para grabar a reales
                    PRMOD.PaymentRequest paymentRequest = new PRMOD.PaymentRequest();
                    paymentRequest.Claim = new claimsModels.Claim() { Id = Convert.ToInt32(tempClaimPayment.ClaimCode) };//pendente
                    paymentRequest.Id = Convert.ToInt32(tempClaimPayment.PaymentRequestCode);
                    paymentRequest.IndividualId = Convert.ToInt32(tempClaimPayment.BeneficiaryId);
                    paymentRequest.Currency = new Currency() { Id = Convert.ToInt32(tempClaimPayment.CurrencyCode) };
                    paymentRequest.TotalAmount = Convert.ToDecimal(tempClaimPayment.IncomeAmount);
                    paymentRequest.RegistrationDate = Convert.ToDateTime(tempClaimPayment.RegistrationDate);
                    paymentRequest.EstimatedDate = Convert.ToDateTime(tempClaimPayment.EstimationDate);
                    paymentRequest.MovementType = new PRMOD.MovementType()
                    {

                        ConceptSource = new PRMOD.ConceptSource() { Id = Convert.ToInt32(tempClaimPayment.RequestType) }
                    };

                    // Recobro
                    if (tempClaimPayment.RequestType == 3) //Convert.ToInt32(CommonModelPayments.PaymentSources.Recovery)
                    {
                        paymentNumber = Convert.ToInt32(tempClaimPayment.PaymentNum);
                        firstPaymentDue = Convert.ToDateTime(tempClaimPayment.PaymentExpirationDate);
                    }
                    // Salvamento
                    else if (tempClaimPayment.RequestType == 2) //Convert.ToInt32(CommonModelPayments.PaymentSources.Salvage)
                    {
                        paymentNumber = Convert.ToInt32(tempClaimPayment.PaymentNum);
                        firstPaymentDue = Convert.ToDateTime(tempClaimPayment.PaymentExpirationDate);
                    }

                    ClaimsPaymentRequestTransactionItem claimsPaymentRequestTransactionItem = new ClaimsPaymentRequestTransactionItem()
                    {
                        BussinessType = Convert.ToInt32(tempClaimPayment.BussinessType),
                        Id = 0,
                        PaymentRequest = paymentRequest
                    };

                    bool isClaimSaved = SaveClaimsPaymentRequest(claimsPaymentRequestTransactionItem.ToDTO(), newImputationId,
                                                               Convert.ToDecimal(tempClaimPayment.ExchangeRate), Convert.ToInt32(tempClaimPayment.RequestType),
                                                               paymentNumber, firstPaymentDue);

                    // En caso de que un real no se haya grabado borra todos los reales grabados anteriormente
                    if (!isClaimSaved)
                    {
                        DeleteClaimPaymentRequestByImputationId(tempImputation.Id);
                        isConverted = false;
                        break;
                    }
                    else
                    {
                        // Se actualiza la fecha y estado de pago de la solicitud de pagos siniestros                        
                        _paymentRequestClaimDAO.UpdateClaimPaymentRequest(Convert.ToInt32(tempClaimPayment.PaymentRequestCode), userId);
                    }

                    isConverted = true;
                }

                return isConverted;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        ///<summary>
        /// ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount
        /// Convierte cuenta de cheques de coaseguros a reales
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        private bool ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(int sourceId, int imputationTypeId, int imputationId)
        {
            try
            {
                int tempCoinsuranceParentId = 0;
                int coinsuranceParentId = 0;

                // Obtengo el temporal.
                Models.Imputations.Application tempImputation = GetTempApplicationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ApplicationItems = new List<TransactionType>();
                CoInsuranceCheckingAccountTransaction tempCoinsuranceCheckingAccountTransaction = _tempCoinsuranceCheckingAccountTransactionDAO.GetTempCoinsuranceCheckingAccountTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ApplicationItems.Add(tempCoinsuranceCheckingAccountTransaction);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempImputation.Id);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
                {
                    CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();

                    if (tempCoinsuranceCheckingAccount.AccountingNatureCode == 1)
                    {
                        coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Credit;
                    }
                    else
                    {
                        coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                    }

                    if (tempCoinsuranceCheckingAccount.CoinsuranceType == 1)
                    {
                        coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Accepted;
                    }
                    else
                    {
                        coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Given;
                    }
                    coinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                    {
                        Currency = new Currency() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.CurrencyCode) },
                        Value = Convert.ToDecimal(tempCoinsuranceCheckingAccount.IncomeAmount.Value)
                    };
                    coinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.BranchCode) };
                    coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.CheckingAccountConceptCode) };
                    coinsuranceCheckingAccountTransactionItem.Comments = tempCoinsuranceCheckingAccount.Description;
                    coinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccount.AccountingCompanyCode) };
                    coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(tempCoinsuranceCheckingAccount.SalePointCode) };
                    coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempCoinsuranceCheckingAccount.ExchangeRate) };
                    coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(tempCoinsuranceCheckingAccount.CoinsuredCompanyId) };
                    coinsuranceCheckingAccountTransactionItem.Policy = new Policy();
                    coinsuranceCheckingAccountTransactionItem.Policy.Id = 0;
                    coinsuranceCheckingAccountTransactionItem.AccountingDate = Convert.ToDateTime(tempCoinsuranceCheckingAccount.AccountingDate);
                    coinsuranceCheckingAccountTransactionItem.LineBusiness = new LineBusiness() { Id = 0 };
                    coinsuranceCheckingAccountTransactionItem.SubLineBusiness = new SubLineBusiness() { Id = 0 };
                    coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses = new Amount() { Value = 0 };
                    coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses = new Amount() { Value = 0 };
                    coinsuranceCheckingAccountTransactionItem.ExtraCommission = new Amount() { Value = 0 };
                    coinsuranceCheckingAccountTransactionItem.OverCommission = new Amount() { Value = 0 };

                    // GRABA A REALES
                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputationId, tempCoinsuranceParentId);

                    coinsuranceParentId = coinsuranceCheckingAccountTransactionItem.Id;

                    // ACTUALIZA A REALES
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem.Properties.TempCoinsCheckingAccTransCode, tempCoinsuranceCheckingAccount.TempCoinsCheckingAccTransCode);
                    BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(
                        ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItem in businessCollectionItems.OfType<ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem>())
                    {
                        CoInsuranceCheckingAccountTransactionItem updateCoinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();
                        updateCoinsuranceCheckingAccountTransactionItem.Id = Convert.ToInt32(tempCoinsuranceCheckingAccountItem.CoinsuranceCheckingAccountCode);

                        _coinsuranceCheckingAccountTransactionItemDAO.UpdateCoinsuranceCheckingAccountTransactionItem(updateCoinsuranceCheckingAccountTransactionItem, coinsuranceParentId);
                    }
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }
        ///<summary>
        /// ConvertTempDailyAccountingToDailyDailyAccounting
        /// Convierte un temporal de transacción contable dada la imputación
        /// </summary>
        /// <param name="sourceCodeId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        public bool ConvertTempAccountingTransactionToAccountingTransaction(int sourceCodeId, int imputationTypeId, int applicationId)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.ConvertTempAccountingTransactionToAccountingTransaction(sourceCodeId, imputationTypeId, applicationId);
            }
            catch (BusinessException)
            {
                return false;
            }
        }
        /// <summary>
        /// ConvertTempPaymentRequestToPaymentRequest
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="newImputationId"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        private bool ConvertTempPaymentRequestToPaymentRequest(int sourceId, int imputationTypeId, int newImputationId, int userId)
        {
            try
            {
                bool isConverted = true;

                // Obtengo el temporal.
                ApplicationDTO tempImputation = GetTempApplicationBySourceCode(imputationTypeId, sourceId);

                List<ACCOUNTINGEN.TempPaymentRequestTrans> entityTempPaymentRequestTrans = _tempPaymentRequestTransactionDAO.GetTempPaymentRequestTrans(0, tempImputation.Id);

                foreach (ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestEntity in entityTempPaymentRequestTrans)
                {
                    PaymentRequestTransactionItem paymentRequestTransactionItem = new PaymentRequestTransactionItem()
                    {
                        Id = 0,
                        PaymentRequest = new PaymentRequest()
                        {
                            AccountingDate = DateTime.Now,
                            Beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempPaymentRequestEntity.BeneficiaryId) },
                            Branch = new Branch(),
                            Company = new Company(),
                            Currency = new Currency() { Id = Convert.ToInt32(tempPaymentRequestEntity.CurrencyCode) },
                            Description = "",
                            EstimatedDate = Convert.ToDateTime(tempPaymentRequestEntity.EstimationDate),
                            Id = Convert.ToInt32(tempPaymentRequestEntity.PaymentRequestCode),
                            MovementType = null,
                            PaymentMethod = null,
                            PaymentRequestNumber = null,
                            PaymentRequestType = PaymentRequestTypes.Payment,
                            PersonType = null,
                            RegisterDate = DateTime.Now,
                            SalePoint = null,
                            TotalAmount = new Amount()
                            {
                                Currency = new Currency() { Id = Convert.ToInt32(tempPaymentRequestEntity.CurrencyCode) },
                                Value = Convert.ToDecimal(tempPaymentRequestEntity.IncomeAmount)
                            },
                            ExchangeRate = new ExchangeRate() { BuyAmount = Convert.ToDecimal(tempPaymentRequestEntity.ExchangeRate) },
                            LocalAmount = new Amount() { Value = Convert.ToDecimal(tempPaymentRequestEntity.Amount) },
                            Transaction = null,
                            UserId = userId,
                            Vouchers = null
                        }
                    };

                    bool isSaved = _tempPaymentRequestTransactionDAO.SavePaymentRequestTransaction(paymentRequestTransactionItem, newImputationId,
                                                               Convert.ToDecimal(tempPaymentRequestEntity.ExchangeRate));

                    // En caso de que un real no se haya grabado borra todos los reales grabados anteriormente
                    if (!isSaved)
                    {
                        _tempPaymentRequestTransactionDAO.DeletePaymentRequestTransactionByImputationId(newImputationId);
                        isConverted = false;
                        break;
                    }
                    else
                    {
                        // Se actualiza el estado y fecha de pago de la solicitud de pagos varios
                        _paymentRequestDAO.UpdatePaymentRequest(Convert.ToInt32(tempPaymentRequestEntity.PaymentRequestCode), userId);
                    }

                    isConverted = true;
                }

                return isConverted;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetCoinsuredAccepted
        /// Obtiene las comisiones de agente, a partir de la poliza y el endoso para coaseguro Aceptado
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="discountedCommission"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="amount"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.CoinsuranceCheckingAccountItemDTO> GetCoinsuredAccepted(int policyId, int endorsementId, decimal discountedCommission, decimal exchangeRate, decimal amount)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.EndorsementId, endorsementId);

                UIView acceptCoinsurances = _dataFacadeManager.GetDataFacade().GetView("CoinsuredAcceptedView",
                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.CoinsuranceCheckingAccountItemDTO> coinsuranceCheckingAccountItemDTOs = new List<SEARCH.CoinsuranceCheckingAccountItemDTO>();

                if (acceptCoinsurances.Count > 0)
                {
                    foreach (DataRow dataRow in acceptCoinsurances)
                    {
                        SEARCH.CoinsuranceCheckingAccountItemDTO coinsuranceCheckingAccountItemDTO = new SEARCH.CoinsuranceCheckingAccountItemDTO();

                        coinsuranceCheckingAccountItemDTO.CoinsuranceCheckingAccountId = 0; // Autonumérico
                        coinsuranceCheckingAccountItemDTO.TempImputationId = 0; // No se usa el dato por el momento
                        coinsuranceCheckingAccountItemDTO.TempCoinsuranceParentId = 0; // Dato no usado
                        coinsuranceCheckingAccountItemDTO.AgentAgencyCode = 0; // No se usa el dato por el momento
                        coinsuranceCheckingAccountItemDTO.BranchCode = Convert.ToInt32(dataRow["BranchCode"]);
                        coinsuranceCheckingAccountItemDTO.SalePointCode = 0; // No se usa el dato por el momento
                        coinsuranceCheckingAccountItemDTO.CompanyCode = Convert.ToInt32(dataRow["InsuranceCompanyId"]);
                        coinsuranceCheckingAccountItemDTO.CoinsuranceCompanyCode = Convert.ToInt32(dataRow["InsuranceCompanyId"]);
                        coinsuranceCheckingAccountItemDTO.AccountingNature = Convert.ToInt32(AccountingNature.Debit); // Las comisiones son débitos
                        coinsuranceCheckingAccountItemDTO.CheckingAccountConceptCode = 0;
                        coinsuranceCheckingAccountItemDTO.CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]);
                        coinsuranceCheckingAccountItemDTO.ExchangeRate = exchangeRate;
                        coinsuranceCheckingAccountItemDTO.Description = "";
                        coinsuranceCheckingAccountItemDTO.CoinsurancePolicyId = Convert.ToInt32(dataRow["PolicyId"]);
                        coinsuranceCheckingAccountItemDTO.CoinsuranceType = 2; // En dos porque es coaseguro aceptado
                        coinsuranceCheckingAccountItemDTO.CompanyParticipation = (Convert.ToDecimal(dataRow["AgentParticipationPercentage"]) / 100) * amount;
                        coinsuranceCheckingAccountItemDTO.IncomeAmount = coinsuranceCheckingAccountItemDTO.CompanyParticipation * coinsuranceCheckingAccountItemDTO.CommissionFactor;
                        coinsuranceCheckingAccountItemDTO.Amount = coinsuranceCheckingAccountItemDTO.IncomeAmount * coinsuranceCheckingAccountItemDTO.ExchangeRate;
                        coinsuranceCheckingAccountItemDTO.AdministrativeExpenses = coinsuranceCheckingAccountItemDTO.CompanyParticipation * (Convert.ToDecimal(dataRow["ExpencesAdmin"]) / 100);
                        coinsuranceCheckingAccountItemDTO.TaxAdministrativeExpenses = coinsuranceCheckingAccountItemDTO.AdministrativeExpenses * 0; // En cero hasta saber de donde viene el iva
                        coinsuranceCheckingAccountItemDTO.InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]);

                        coinsuranceCheckingAccountItemDTOs.Add(coinsuranceCheckingAccountItemDTO);
                    }
                }

                return coinsuranceCheckingAccountItemDTOs;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        /// <summary>
        /// GetCoinsuredAsigned
        /// Obtiene las comisiones de agente, a partir de la poliza y el endoso para coaseguro cedido
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="discountedCommission"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="amount"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.CoinsuranceCheckingAccountItemDTO> GetCoinsuredAsigned(int policyId, int endorsementId, decimal discountedCommission, decimal exchangeRate, decimal amount)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.EndorsementId, endorsementId);

                UIView asignedCoinsurances = _dataFacadeManager.GetDataFacade().GetView("CoinsuredAssignedView",
                                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.CoinsuranceCheckingAccountItemDTO> coinsuranceCheckingAccountItemDTOs = new List<SEARCH.CoinsuranceCheckingAccountItemDTO>();

                if (asignedCoinsurances.Count > 0)
                {
                    foreach (DataRow dataRow in asignedCoinsurances)
                    {
                        SEARCH.CoinsuranceCheckingAccountItemDTO coinsuranceCheckingAccountItemDTO = new SEARCH.CoinsuranceCheckingAccountItemDTO();

                        coinsuranceCheckingAccountItemDTO.CoinsuranceCheckingAccountId = 0; // Autonumerico
                        coinsuranceCheckingAccountItemDTO.TempImputationId = 0; // No se usa el dato por el momento
                        coinsuranceCheckingAccountItemDTO.TempCoinsuranceParentId = 0; // Dato no usado
                        coinsuranceCheckingAccountItemDTO.AgentAgencyCode = 0; // No se usa el dato por el momento
                        coinsuranceCheckingAccountItemDTO.BranchCode = Convert.ToInt32(dataRow["BranchCode"]);
                        coinsuranceCheckingAccountItemDTO.SalePointCode = 0; // No se usa el dato por el momento
                        coinsuranceCheckingAccountItemDTO.CompanyCode = 0;
                        coinsuranceCheckingAccountItemDTO.CoinsuranceCompanyCode = Convert.ToInt32(dataRow["InsuranceCompanyId"]);
                        coinsuranceCheckingAccountItemDTO.AgentId = Convert.ToInt32(dataRow["AgentIndividualId"]);
                        coinsuranceCheckingAccountItemDTO.AccountingNature = Convert.ToInt32(AccountingNature.Debit); // Las comisiones son débitos
                        coinsuranceCheckingAccountItemDTO.CheckingAccountConceptCode = 0;
                        coinsuranceCheckingAccountItemDTO.CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]);
                        coinsuranceCheckingAccountItemDTO.ExchangeRate = exchangeRate;
                        coinsuranceCheckingAccountItemDTO.Description = "";
                        coinsuranceCheckingAccountItemDTO.CoinsurancePolicyId = Convert.ToInt32(dataRow["PolicyId"]);
                        coinsuranceCheckingAccountItemDTO.CoinsuranceType = 3; // En tres porque es coaseguro cedido
                        coinsuranceCheckingAccountItemDTO.CompanyParticipation = (Convert.ToDecimal(dataRow["ParticipationPercentage"]) / 100) * amount;
                        coinsuranceCheckingAccountItemDTO.CommissionFactor = (Convert.ToDecimal(dataRow["AgentParticipationPercentage"]) * Convert.ToDecimal(dataRow["CommissionPercentage"])) / 10000;
                        coinsuranceCheckingAccountItemDTO.AgentCommissionIncomeAmount = coinsuranceCheckingAccountItemDTO.CompanyParticipation * coinsuranceCheckingAccountItemDTO.CommissionFactor; // Esto es para agente
                        coinsuranceCheckingAccountItemDTO.AgentCommissionAmount = coinsuranceCheckingAccountItemDTO.AgentCommissionIncomeAmount * coinsuranceCheckingAccountItemDTO.ExchangeRate;
                        coinsuranceCheckingAccountItemDTO.IncomeAmount = coinsuranceCheckingAccountItemDTO.CompanyParticipation;
                        coinsuranceCheckingAccountItemDTO.Amount = coinsuranceCheckingAccountItemDTO.IncomeAmount * coinsuranceCheckingAccountItemDTO.ExchangeRate;
                        coinsuranceCheckingAccountItemDTO.AdministrativeExpenses = coinsuranceCheckingAccountItemDTO.CompanyParticipation * (Convert.ToDecimal(dataRow["ExpencesAdmin"]) / 100);
                        coinsuranceCheckingAccountItemDTO.TaxAdministrativeExpenses = coinsuranceCheckingAccountItemDTO.AdministrativeExpenses * 0; // En cero hasta saber de donde viene el iva
                        coinsuranceCheckingAccountItemDTO.LineBusinessCode = Convert.ToInt32(dataRow["LineBusinessCode"]);
                        coinsuranceCheckingAccountItemDTO.SubLineBusinessCode = Convert.ToInt32(dataRow["SubLineBusinessCode"]);

                        coinsuranceCheckingAccountItemDTOs.Add(coinsuranceCheckingAccountItemDTO);
                    }
                }

                return coinsuranceCheckingAccountItemDTOs;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        /// <summary>
        /// GetSourceIdByApplication
        /// Obtiene el origen de imputación por la imputación
        /// </summary>
        ///<param name="applicationDTO"> </param>
        ///<returns>int</returns>
        public int GetSourceIdByApplication(ApplicationDTO applicationDTO)
        {
            int sourceId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ApplicationCode, applicationDTO.Id);
                criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, applicationDTO.ImputationType);
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, (int)applicationDTO.ImputationType);

                //Asignamos BusinessCollection 
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.Application imputationEntity in businessCollection.OfType<ACCOUNTINGEN.Application>())
                {
                    sourceId = Convert.ToInt32(imputationEntity.SourceCode);
                }

                return sourceId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        /// <summary>
        /// GetBrokerChekingAccountCollections
        /// Obtiene cuenta corrientes de agentes 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<BrokersCheckingAccountTransactionItem/></returns>
        public List<BrokersCheckingAccountTransactionItem> GetBrokerChekingAccountCollections(int policyId, DateTime dateFrom, DateTime dateTo)
        {
            List<BrokersCheckingAccountTransactionItem> brokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItem>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.PolicyId, policyId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.BrokerCheckingAccountTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.BrokerCheckingAccountTrans>())
                    {
                        BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                        brokersCheckingAccountTransactionItem.Id = brokerCheckingAccount.BrokerCheckingAccountTransId;
                        brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.GetBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem);

                        brokersCheckingAccountTransactionItem.Id = 0; // Seteo para nuevos registros

                        brokersCheckingAccountTransactionItem.Amount = new Amount()
                        {
                            Currency = new Currency { Id = Convert.ToInt32(brokerCheckingAccount.CurrencyCode) },
                            Value = Convert.ToDecimal(brokerCheckingAccount.IncomeAmount * -1)
                        };
                        brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate()
                        {
                            SellAmount = Convert.ToDecimal(brokerCheckingAccount.ExchangeRate)
                        };
                        brokersCheckingAccountTransactionItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccount.Amount * -1) };

                        Amount commissionAmount = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccount.StCommissionAmount * -1) };
                        brokersCheckingAccountTransactionItem.CommissionAmount = commissionAmount;
                        brokersCheckingAccountTransactionItem.DiscountedCommission = brokerCheckingAccount.DiscountedCommission > 0 ? new Amount() { Value = Convert.ToDecimal(brokerCheckingAccount.StCommissionAmount) } : new Amount() { Value = Convert.ToDecimal(brokerCheckingAccount.DiscountedCommission) };

                        Amount commissionBalance = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccount.CommissionBalance * -1) };
                        brokersCheckingAccountTransactionItem.CommissionBalance = commissionBalance;
                        brokersCheckingAccountTransactionItem.IsPayed = true; // Como pagados segun EF

                        brokersCheckingAccountTransactionItems.Add(brokersCheckingAccountTransactionItem);
                    }
                }

                return brokersCheckingAccountTransactionItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        /// <summary>
        /// GetBrokerChekingAccounts
        /// Obtiene un agente cuenta corriente
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="policyId"></param>
        /// <returns>List<BrokersCheckingAccountTransactionItem/></returns>
        public List<BrokersCheckingAccountTransactionItem> GetBrokerChekingAccounts(int agentId, DateTime dateFrom, DateTime dateTo, int policyId)
        {
            List<BrokersCheckingAccountTransactionItem> brokersCheckingAccountTransactionItems = new List<BrokersCheckingAccountTransactionItem>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);

                if (agentId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AgentId, agentId);
                }
                if (policyId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.PolicyId, policyId);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.BrokerCheckingAccountTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.BrokerCheckingAccountTrans>())
                    {
                        BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                        brokersCheckingAccountTransactionItem.Id = Convert.ToInt32(brokerCheckingAccount.BrokerCheckingAccountTransId);
                        brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.GetBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem);

                        brokersCheckingAccountTransactionItems.Add(brokersCheckingAccountTransactionItem);
                    }
                }

                return brokersCheckingAccountTransactionItems;
            }

            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        /// <summary>
        /// GetBussinessTypeIdByPremiumReceivableId
        /// Obtiene el tipo de negocio de una póliza dado el id de la cobranza
        /// </summary>
        /// <param name="premiumReceivableId"></param>
        /// <returns>int</returns>
        public int GetBussinessTypeIdByPremiumReceivableId(int premiumReceivableId)
        {
            int businessTypeId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                //criteriaBuilder.Property(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId);
                //En la linea 1565 la variable creriaaBuilder requiere de la propiedad PremiumReceivableTransId de la entidad PremiumReceivableTrans 
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(premiumReceivableId);

                UIView premiumReceivableCoinsurances = _dataFacadeManager.GetDataFacade().GetView("PremiumReceivableBusinessView",
                                                       criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (premiumReceivableCoinsurances.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in premiumReceivableCoinsurances)
                    {
                        businessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]);
                    }
                }

                return businessTypeId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        /// <summary>
        /// GetAgentCommissions
        /// Obtiene las comisiones de agente, a partir de la poliza y el endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="discountedCommission"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="lineBusinessCode"></param>
        /// <param name="subLineBusinessCode"></param>
        /// <param name="userId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.BrokerCheckingAccountItemDTO> GetAgentCommissions(int policyId, int endorsementId, decimal discountedCommission,
                                                                      decimal exchangeRate, int lineBusinessCode, int subLineBusinessCode, int userId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.EndorsementId, endorsementId);
                if (lineBusinessCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.LineBusinessCode, lineBusinessCode);
                }
                if (subLineBusinessCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.SubLineBusinessCode, subLineBusinessCode);
                }

                UIView agentCommissions = _dataFacadeManager.GetDataFacade().GetView("AgentCommissionView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.BrokerCheckingAccountItemDTO> brokerCheckingAccountItemDTOs = new List<SEARCH.BrokerCheckingAccountItemDTO>();

                if (agentCommissions.Count > 0)
                {
                    foreach (DataRow dataRow in agentCommissions)
                    {
                        SEARCH.BrokerCheckingAccountItemDTO brokerCheckingAccountItemDTO = new SEARCH.BrokerCheckingAccountItemDTO();

                        brokerCheckingAccountItemDTO.BrokerCheckingAccountId = 0;// Autonumérico
                        brokerCheckingAccountItemDTO.TempImputationId = 0; // No se usa el dato por el momento
                        brokerCheckingAccountItemDTO.TempBrokerParentId = 0; // Dato no usado
                        brokerCheckingAccountItemDTO.AgentTypeCode = Convert.ToInt32(dataRow["AgentTypeCode"]);
                        brokerCheckingAccountItemDTO.AgentName = Convert.ToString(dataRow["AgentTypeDescription"]);
                        brokerCheckingAccountItemDTO.AgentCode = Convert.ToInt32(dataRow["AgentIndividualId"]);
                        brokerCheckingAccountItemDTO.AgentAgencyCode = 0; // No se usa el dato por el momento
                        brokerCheckingAccountItemDTO.BranchCode = Convert.ToInt32(dataRow["BranchCode"]);
                        brokerCheckingAccountItemDTO.SalePointCode = GetSalePointDefaultByUserIdAndBranchId(userId, brokerCheckingAccountItemDTO.BranchCode);// Aumentado para la generación de órdenes de pago 27/11/2013
                        brokerCheckingAccountItemDTO.CompanyCode = GetAccountingCompanyDefaultByUserId(userId);
                        brokerCheckingAccountItemDTO.AccountNature = Convert.ToInt32(AccountingNature.Debit); // Las comisiones son débitos
                        brokerCheckingAccountItemDTO.CheckingAccountConceptCode = 0;
                        brokerCheckingAccountItemDTO.CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]);
                        brokerCheckingAccountItemDTO.IncomeAmount = Convert.ToDecimal(dataRow["AgentCommissionAmount"]);
                        brokerCheckingAccountItemDTO.ExchangeRate = exchangeRate;
                        brokerCheckingAccountItemDTO.Amount = brokerCheckingAccountItemDTO.IncomeAmount * brokerCheckingAccountItemDTO.ExchangeRate;
                        brokerCheckingAccountItemDTO.Description = "";
                        brokerCheckingAccountItemDTO.PolicyId = Convert.ToInt32(dataRow["PolicyId"]);
                        brokerCheckingAccountItemDTO.PrefixCode = Convert.ToInt32(dataRow["PrefixCode"]);
                        brokerCheckingAccountItemDTO.EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]);
                        brokerCheckingAccountItemDTO.InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]);
                        brokerCheckingAccountItemDTO.CommissionTypeCode = 0;
                        brokerCheckingAccountItemDTO.CommissionPercentage = Convert.ToDecimal(dataRow["CommissionPercentage"]);
                        brokerCheckingAccountItemDTO.CommissionAmount = Convert.ToDecimal(dataRow["AgentCommissionAmount"]);

                        brokerCheckingAccountItemDTO.CommissionDiscounted = Convert.ToDecimal(discountedCommission * (brokerCheckingAccountItemDTO.CommissionPercentage / 100)); // Revisar este cálculo    

                        brokerCheckingAccountItemDTO.CommissionBalance = brokerCheckingAccountItemDTO.CommissionAmount - brokerCheckingAccountItemDTO.CommissionDiscounted;

                        brokerCheckingAccountItemDTO.Payed = (brokerCheckingAccountItemDTO.CommissionAmount - brokerCheckingAccountItemDTO.CommissionDiscounted) > 0 ? 0 : 1;

                        brokerCheckingAccountItemDTO.LineBusiness = Convert.ToInt32(dataRow["LineBusinessCode"]);
                        brokerCheckingAccountItemDTO.SubLineBusiness = Convert.ToInt32(dataRow["SubLineBusinessCode"]);
                        brokerCheckingAccountItemDTO.AgentParticipationPercentage = Convert.ToDecimal(dataRow["AgentParticipationPercentage"]);
                        brokerCheckingAccountItemDTO.AdditionalCommissionPercentage = Convert.ToDecimal(dataRow["AdditionalCommissionPercentage"]);
                        brokerCheckingAccountItemDTOs.Add(brokerCheckingAccountItemDTO);
                    }
                }

                return brokerCheckingAccountItemDTOs;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        /// <summary>
        /// GetSalePointDefaultByUserIdAndBranchId
        /// Obtiene el punto de venta por default de una sucursal y usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <returns>int</returns>
        public int GetSalePointDefaultByUserIdAndBranchId(int userId, int branchId)
        {
            int salePointId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(COMMEN.UserSalePoint.Properties.UserId, userId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(COMMEN.UserSalePoint.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(COMMEN.UserSalePoint.Properties.DefaultSalePoint, 1);

                BusinessCollection businessCollection =
                    new BusinessCollection(
                        _dataFacadeManager.GetDataFacade().SelectObjects(typeof(COMMEN.UserSalePoint),
                                                                        criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (COMMEN.UserSalePoint userSalePoint in businessCollection.OfType<COMMEN.UserSalePoint>())
                    {
                        salePointId = userSalePoint.SalePointCode;
                    }
                }
                else
                {
                    salePointId = -1;
                }
            }
            catch (BusinessException)
            {
                salePointId = -1;
            }

            return salePointId;
        }
        /// <summary>
        /// GetAccountingCompanyDefaultByUserId
        /// Obtiene la compañía por defecto del usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int GetAccountingCompanyDefaultByUserId(int userId)
        {
            int accountingCompanyId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.UserAccountingCompany.Properties.UserId, userId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.UserAccountingCompany.Properties.DefaultAccountingCompany, true);

                BusinessCollection businessCollection =
                    new BusinessCollection(
                        _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.UserAccountingCompany),
                                                                        criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.UserAccountingCompany userAccountingCompany in businessCollection.OfType<ACCOUNTINGEN.UserAccountingCompany>())
                    {
                        accountingCompanyId = userAccountingCompany.AccountingCompanyCode;
                    }
                }
                else
                {
                    accountingCompanyId = -1;
                }
            }
            catch (BusinessException)
            {
                accountingCompanyId = -1;
            }

            return accountingCompanyId;
        }
        /// <summary>
        /// ValidateAgentCommissionRelease
        /// Valida las condiciones para la liberacion de comisiones de agente
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>bool</returns>
        public bool ValidateAgentCommissionRelease(int policyId, int endorsementId)
        {
            decimal paymentAmount = 0;
            decimal paidAmount = 0;
            bool isValidated = false;

            try
            {
                // Obtengo el valor de la primera cuota (independiente de si es crédito o contado, se hará la validación con la 1era cuota)
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ISSEN.PayerPayment.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ISSEN.PayerPayment.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ISSEN.PayerPayment.Properties.PaymentNum, 1); // indico que es la primera cuota

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ISSEN.PayerPayment), criteriaBuilder.GetPredicate()));

                foreach (ISSEN.PayerPayment payment in businessCollection.OfType<ISSEN.PayerPayment>())
                {
                    paymentAmount = Convert.ToDecimal(payment.Amount);
                }

                // Compruebo que dicha cuota ha sido pagada en su totalidad (select a la tabla ACC.PREMIUM_RECEIVABLE).
                criteriaBuilder = new ObjectCriteriaBuilder();
                //? En la linea 1796 se requiere la propiedad PolicyId , 1798 endorsementId y 1800 PaymentNum de la identidad PremiumReceivableTrans
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PaymentNum, 1); //se indica que es la primera cuota

                //BusinessCollection businessCollectionPayment = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));
                //businessCollectionPayment se instancia y se identifica que el criteriaBuilder sea del mismo tipo de identy PremiumReceivableTrans 

                //? Al realizar el recorrido se requiere Amount de la identidad PremiumReceivableTrans para la variable paidmount
                /*foreach (ACCOUNTINGEN.PremiumReceivableTrans collection in businessCollectionPayment.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                {
                    paidAmount = paidAmount + Convert.ToDecimal(collection.Amount);
                }*/

                if (paidAmount >= paymentAmount)
                {
                    isValidated = true;
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return isValidated;
        }
        /// <summary>
        /// BrokersCommission
        /// La liberación de las coutas es por cada cobranza
        /// </summary>
        /// <param name="premiumReceivableTransactionItem"></param>
        /// <param name="imputation"></param>
        /// <param name="sourceId"></param>
        /// <returns>bool</returns>
        private bool BrokersCommission(PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem, ApplicationDTO application, int sourceId)
        {
            bool isSaved = new bool();
            try
            {
                DateTime accountingDate = Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]);

                DateTime dateFrom = Convert.ToDateTime("01" + "/" + accountingDate.Month + "/" + accountingDate.Year);
                DateTime dateTo = accountingDate;

                decimal collectionAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                decimal exchangeRate = 0;
                exchangeRate = premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount;

                decimal amount = 0;
                amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;

                int agentTypeId = 0;

                // Obtengo los ramos y subramos
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableTransactionItem.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.ComponentId, 1); //con respecto a prima neta

                UIView prefixComponentCollection = _dataFacadeManager.GetDataFacade().GetView("PrefixComponentCollection",
                   criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);


                if (prefixComponentCollection.Count > 0)
                {
                    foreach (DataRow dataRow in prefixComponentCollection)
                    {
                        // Obtengo las comisiones de agente
                        List<SEARCH.BrokerCheckingAccountItemDTO> commissions = GetAgentCommissions(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id,
                            premiumReceivableTransactionItem.DeductCommission.Value, premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount, Convert.ToInt32(dataRow["LineBusinessCode"]),
                            Convert.ToInt32(dataRow["SubLineBusinessCode"]), application.UserId);

                        foreach (SEARCH.BrokerCheckingAccountItemDTO commision in commissions)
                        {
                            // Grabación en la tabla de cuentas corrientes de agentes.
                            BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                            brokersCheckingAccountTransactionItem.Id = 0;
                            brokersCheckingAccountTransactionItem.Holder = new Agent();
                            brokersCheckingAccountTransactionItem.Holder.IndividualId = commision.AgentCode;
                            brokersCheckingAccountTransactionItem.Holder.FullName = commision.AgentName;
                            brokersCheckingAccountTransactionItem.Agencies = new List<Agency>()
                                                    {
                                                        new Agency() { Id = commision.AgentAgencyCode }
                                                    };
                            brokersCheckingAccountTransactionItem.InsuredId = commision.InsuredId;
                            brokersCheckingAccountTransactionItem.IsAutomatic = true;
                            brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = commision.CommissionDiscounted };
                            brokersCheckingAccountTransactionItem.Policy = new Policy();
                            brokersCheckingAccountTransactionItem.Policy.Id = commision.PolicyId;
                            brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                                    {
                                                        new Beneficiary()
                                                        {
                                                            IndividualId = commision.InsuredId,
                                                        }
                                                    };
                            brokersCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = commision.EndorsementId };
                            brokersCheckingAccountTransactionItem.Policy.ExchangeRate = new ExchangeRate()
                            {
                                Currency = new Currency() { Id = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id }
                            };
                            brokersCheckingAccountTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                            {
                                Quotas = new List<Quota>()
                                                        {
                                                            new Quota() { Number = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number }
                                                        }
                            };
                            brokersCheckingAccountTransactionItem.PrefixId = commision.PrefixCode;
                            brokersCheckingAccountTransactionItem.AccountingNature = (AccountingNature)commision.AccountNature;



                            //Comision Normal => (%comis.normal * %ParticipaciónAgente * Cobranza(ramo/subramo) )
                            brokersCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = commision.CommissionPercentage };
                            brokersCheckingAccountTransactionItem.CommissionAmount = new Amount()
                            {
                                Value = Math.Round(((commision.CommissionPercentage / 100) * (commision.AgentParticipationPercentage / 100) * collectionAmount), 2)
                            };

                            //Comision Adicional => (%comis.extra * %ParticipaciónAgente * Cobranza(ramo/subramo))
                            brokersCheckingAccountTransactionItem.AdditionalCommissionPercentage = commision.AdditionalCommissionPercentage;
                            if (commision.AdditionalCommissionPercentage > 0)
                            {
                                brokersCheckingAccountTransactionItem.AdditionalCommissionAmount = Math.Round(((commision.AdditionalCommissionPercentage / 100) *
                                                                                               (commision.AgentParticipationPercentage / 100) * collectionAmount), 2);
                            }

                            brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = commision.CommissionBalance };
                            brokersCheckingAccountTransactionItem.CommissionType = Convert.ToInt32(commision.CommissionTypeCode);
                            brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = commision.CommissionDiscounted };
                            brokersCheckingAccountTransactionItem.Branch = new Branch() { Id = commision.BranchCode };
                            brokersCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = commision.SalePointCode };
                            brokersCheckingAccountTransactionItem.Company = new Company() { IndividualId = commision.CompanyCode };
                            brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept()
                            {
                                Id = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_CHECKING_ACCOUNT_CONCEPTID))
                            };
                            brokersCheckingAccountTransactionItem.Amount = new Amount()
                            {
                                Currency = new Currency() { Id = Convert.ToInt32(commision.CurrencyCode) },
                                Value = commision.Amount
                            };
                            brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = commision.ExchangeRate };
                            brokersCheckingAccountTransactionItem.IsAutomatic = true;
                            brokersCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = Convert.ToInt32(commision.LineBusiness) };
                            brokersCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = Convert.ToInt32(commision.SubLineBusiness) };
                            brokersCheckingAccountTransactionItem.Comments = "GENERACIÓN AUTOMÁTICA";

                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem,
                                application.Id, 0, sourceId, commision.AgentTypeCode,
                                Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]));


                            agentTypeId = commision.AgentTypeCode;
                            if (brokersCheckingAccountTransactionItem.Id > 0)
                            {
                                isSaved = true;
                            }
                        }

                        if (isSaved)
                        {
                            int businessTypeId = GetBussinessTypeIdByPremiumReceivableId(premiumReceivableTransactionItem.Id);

                            #region CoinsuranceAccepted

                            if (businessTypeId == 2) // Coaseguro Aceptado
                            {
                                isSaved = false;

                                List<SEARCH.CoinsuranceCheckingAccountItemDTO> acceptedCoinsurances = GetCoinsuredAccepted(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                if (acceptedCoinsurances.Count > 0)
                                {
                                    foreach (SEARCH.CoinsuranceCheckingAccountItemDTO acceptedCoinsurance in acceptedCoinsurances)
                                    {
                                        List<BrokersCheckingAccountTransactionItem> brokerCheckingAccounts = GetBrokerChekingAccounts(-1, dateTo, dateTo, acceptedCoinsurance.CoinsurancePolicyId);

                                        if (brokerCheckingAccounts.Count > 0)
                                        {
                                            collectionAmount = 0;
                                            decimal collectionIncomeAmount = 0;
                                            foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccount in brokerCheckingAccounts)
                                            {
                                                collectionAmount = collectionAmount + Convert.ToDecimal(brokerCheckingAccount.LocalAmount.Value);
                                                collectionIncomeAmount = collectionIncomeAmount + Convert.ToDecimal(brokerCheckingAccount.Amount.Value);
                                            }
                                            // Grabación en la tabla de cuentas corrientes de agentes.
                                            BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                            brokersCheckingAccountTransactionItem.Id = 0;
                                            brokersCheckingAccountTransactionItem.InsuredId = Convert.ToInt32(acceptedCoinsurance.InsuredId);
                                            brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                            brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccounts[0].DiscountedCommission.Value) };
                                            brokersCheckingAccountTransactionItem.Policy = new Policy();
                                            brokersCheckingAccountTransactionItem.Policy.Id = Convert.ToInt32(acceptedCoinsurance.CoinsurancePolicyId);
                                            brokersCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(brokerCheckingAccounts[0].Policy.Endorsement.Id) };
                                            brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                                                    {
                                                                        new Beneficiary()
                                                                        {
                                                                            IndividualId = brokerCheckingAccounts[0].Policy.DefaultBeneficiaries[0].IndividualId,
                                                                        }
                                                                    };
                                            brokersCheckingAccountTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                                            {
                                                Quotas = new List<Quota>()
                                                                        {
                                                                            new Quota() { Number = 0 }
                                                                        }
                                            };
                                            brokersCheckingAccountTransactionItem.PrefixId = brokerCheckingAccounts[0].PrefixId;
                                            brokersCheckingAccountTransactionItem.AccountingNature = brokerCheckingAccounts[0].AccountingNature;
                                            brokersCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = collectionIncomeAmount };
                                            brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccounts[0].CommissionBalance.Value) };
                                            brokersCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccounts[0].CommissionPercentage.Value) };
                                            brokersCheckingAccountTransactionItem.CommissionType = Convert.ToInt32(brokerCheckingAccounts[0].CommissionType);
                                            brokersCheckingAccountTransactionItem.Holder = new Agent();
                                            brokersCheckingAccountTransactionItem.Holder.IndividualId = Convert.ToInt32(brokerCheckingAccounts[0].Holder.IndividualId);
                                            brokersCheckingAccountTransactionItem.Agencies = new List<Agency>();
                                            brokersCheckingAccountTransactionItem.Agencies.Add(new Agency() { Id = brokerCheckingAccounts[0].Agencies[0].Id });
                                            brokersCheckingAccountTransactionItem.Branch = new Branch();
                                            brokersCheckingAccountTransactionItem.Branch.Id = brokerCheckingAccounts[0].Branch.Id;
                                            brokersCheckingAccountTransactionItem.SalePoint = new SalePoint();
                                            brokersCheckingAccountTransactionItem.SalePoint.Id = brokerCheckingAccounts[0].SalePoint.Id;
                                            brokersCheckingAccountTransactionItem.Company = new Company();
                                            brokersCheckingAccountTransactionItem.Company.IndividualId = brokerCheckingAccounts[0].Company.IndividualId;
                                            brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept();
                                            brokersCheckingAccountTransactionItem.CheckingAccountConcept.Id = brokerCheckingAccounts[0].CheckingAccountConcept.Id;
                                            brokersCheckingAccountTransactionItem.Amount = new Amount()
                                            {
                                                Currency = new Currency() { Id = Convert.ToInt32(brokerCheckingAccounts[0].Amount.Currency.Id) },
                                                Value = collectionIncomeAmount
                                            };
                                            brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = brokerCheckingAccounts[0].ExchangeRate.SellAmount };
                                            brokersCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = brokerCheckingAccounts[0].PrefixId };
                                            brokersCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = brokerCheckingAccounts[0].SubPrefix.Id };
                                            brokersCheckingAccountTransactionItem.IsPayed = true;
                                            sourceId = GetSourceIdByApplication(application);

                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);
                                            if (brokersCheckingAccountTransactionItem.Id > 0)
                                            {
                                                isSaved = true;
                                            }
                                        }
                                    }
                                }

                                List<BrokersCheckingAccountTransactionItem> brokerCheckingAccountCollections = GetBrokerChekingAccountCollections(Convert.ToInt32(premiumReceivableTransactionItem.Policy.Id), dateTo, dateTo);
                                if (brokerCheckingAccountCollections.Count > 0)
                                {
                                    foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccountCollection in brokerCheckingAccountCollections)
                                    {
                                        // Grabación en la tabla de cuentas corrientes de agentes.
                                        BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                        brokersCheckingAccountTransactionItem = brokerCheckingAccountCollection;
                                        sourceId = GetSourceIdByApplication(application);

                                        brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);
                                        if (brokersCheckingAccountTransactionItem.Id > 0)
                                        {
                                            isSaved = true;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region CoinsuranceGiven

                            if (businessTypeId == 3) // Coaseguro Cedido 
                            {
                                isSaved = false;
                                List<SEARCH.CoinsuranceCheckingAccountItemDTO> cededCoinsurances = GetCoinsuredAsigned(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                foreach (SEARCH.CoinsuranceCheckingAccountItemDTO cededCoinsurance in cededCoinsurances)
                                {
                                    CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();

                                    coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                                    coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Given;
                                    coinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                                    {
                                        Currency = new Currency() { Id = Convert.ToInt32(cededCoinsurance.CurrencyCode) },
                                        Value = Convert.ToDecimal(cededCoinsurance.Amount)
                                    };
                                    coinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(cededCoinsurance.BranchCode) };
                                    coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(cededCoinsurance.CheckingAccountConceptCode) };
                                    coinsuranceCheckingAccountTransactionItem.Comments = "";
                                    coinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(cededCoinsurance.CompanyCode) };
                                    coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(cededCoinsurance.SalePointCode) };
                                    coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(cededCoinsurance.ExchangeRate) };
                                    coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(cededCoinsurance.CoinsuranceCompanyCode) };
                                    coinsuranceCheckingAccountTransactionItem.Policy = new Policy();
                                    coinsuranceCheckingAccountTransactionItem.Policy.Id = cededCoinsurance.CoinsurancePolicyId;
                                    coinsuranceCheckingAccountTransactionItem.AccountingDate = accountingDate;
                                    coinsuranceCheckingAccountTransactionItem.LineBusiness = new LineBusiness() { Id = Convert.ToInt32(cededCoinsurance.LineBusinessCode) };
                                    coinsuranceCheckingAccountTransactionItem.SubLineBusiness = new SubLineBusiness() { Id = Convert.ToInt32(cededCoinsurance.SubLineBusinessCode) };
                                    coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(cededCoinsurance.AdministrativeExpenses) };
                                    coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(cededCoinsurance.TaxAdministrativeExpenses) };
                                    coinsuranceCheckingAccountTransactionItem.ExtraCommission = new Amount() { Value = 0 };
                                    coinsuranceCheckingAccountTransactionItem.OverCommission = new Amount() { Value = 0 };

                                    // GRABA A REALES
                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, application.Id, 0);

                                    if (coinsuranceCheckingAccountTransactionItem.Id > 0)
                                    {
                                        isSaved = true;
                                        // Punto 5.10

                                        SEARCH.AgentCoinsuranceCheckingAccountDTO agentCoinsuranceCheckingAccountDto = new SEARCH.AgentCoinsuranceCheckingAccountDTO();
                                        agentCoinsuranceCheckingAccountDto.AgentCode = cededCoinsurance.AgentId;
                                        agentCoinsuranceCheckingAccountDto.AgentTypeCode = agentTypeId;
                                        agentCoinsuranceCheckingAccountDto.CommissionAmount = cededCoinsurance.AgentCommissionAmount;
                                        agentCoinsuranceCheckingAccountDto.CurrencyCode = cededCoinsurance.CurrencyCode;
                                        agentCoinsuranceCheckingAccountDto.IncomeCommissionAmount = cededCoinsurance.AgentCommissionIncomeAmount;

                                        isSaved = _agentCoinsuranceCheckingAccountDAO.SaveAgentCoinsuranceCheckingAccount(agentCoinsuranceCheckingAccountDto);
                                    }
                                }

                                List<SEARCH.CoinsuranceCheckingAccountItemDTO> acceptedCoinsurances = GetCoinsuredAccepted(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                foreach (SEARCH.CoinsuranceCheckingAccountItemDTO acceptedCoinsurance in acceptedCoinsurances)
                                {
                                    CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();

                                    coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                                    coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Given;
                                    coinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                                    {
                                        Currency = new Currency() { Id = Convert.ToInt32(acceptedCoinsurance.CurrencyCode) },
                                        Value = Convert.ToDecimal(acceptedCoinsurance.Amount)
                                    };
                                    coinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(acceptedCoinsurance.BranchCode) };
                                    coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(acceptedCoinsurance.CheckingAccountConceptCode) };
                                    coinsuranceCheckingAccountTransactionItem.Comments = "";
                                    coinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(acceptedCoinsurance.CompanyCode) };
                                    coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(acceptedCoinsurance.SalePointCode) };
                                    coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(acceptedCoinsurance.ExchangeRate) };
                                    coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(acceptedCoinsurance.CoinsuranceCompanyCode) };
                                    coinsuranceCheckingAccountTransactionItem.Policy = new Policy();
                                    coinsuranceCheckingAccountTransactionItem.Policy.Id = acceptedCoinsurance.CoinsurancePolicyId;
                                    coinsuranceCheckingAccountTransactionItem.AccountingDate = accountingDate;
                                    coinsuranceCheckingAccountTransactionItem.LineBusiness = new LineBusiness() { Id = Convert.ToInt32(acceptedCoinsurance.LineBusinessCode) };
                                    coinsuranceCheckingAccountTransactionItem.SubLineBusiness = new SubLineBusiness() { Id = Convert.ToInt32(acceptedCoinsurance.SubLineBusinessCode) };
                                    coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(acceptedCoinsurance.AdministrativeExpenses) };
                                    coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(acceptedCoinsurance.TaxAdministrativeExpenses) };
                                    coinsuranceCheckingAccountTransactionItem.ExtraCommission = new Amount() { Value = 0 };
                                    coinsuranceCheckingAccountTransactionItem.OverCommission = new Amount() { Value = 0 };

                                    // GRABA A REALES
                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, application.Id, 0);

                                    if (coinsuranceCheckingAccountTransactionItem.Id > 0)
                                    {
                                        isSaved = true;
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {

                throw new BusinessException(Resources.Resources.BusinessException);
            }
            return isSaved;
        }
        /// <summary>
        /// SaveComponentCollectionRequest
        /// Graba la colección de componentes
        /// </summary>
        /// <param name="premiumReceivableId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="currencyId"></param>
        /// <returns>bool</returns>
        public bool SaveComponentCollectionRequest(int premiumReceivableId, decimal exchangeRate, int currencyId)
        {
            bool isSaved = false;
            bool isComponentSaved = false;
            bool isPrefixSaved = false;
            int policyId = 0;
            int endorsementId = 0;
            decimal premiumTotal = 0;
            decimal paidAmount = 0;
            decimal netPremium = 0;

            try
            {
                // Obtengo policyId, endorsementId y valor de la cobranza a partir de la prima por cobrar.
                //Dehibyb
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                //? Se compara la propiedad del parametro premiumReceivableId con la identy PremiumReceivableTrans
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId, premiumReceivableId);

                //? para traer una nueva colleccion de negocio se requiere un comparar con el tipo de PremiumReceivableTrans
                //BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                //typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                /* if (businessCollection.Count > 0)
                 {
                     foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                     {
                         policyId = Convert.ToInt32(premiumReceivableEntity.PolicyId);
                         endorsementId = Convert.ToInt32(premiumReceivableEntity.EndorsementId);6
                         paidAmount = Convert.ToDecimal(premiumReceivableEntity.Amount);
                     }
                 }*/

                // Obtengo el valor total de la poliza.
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeTotal.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeTotal.Properties.EndorsementId, endorsementId);

                UIView premiumTotals = _dataFacadeManager.GetDataFacade().GetView("PrimeTotalView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);
                if (premiumTotals.Rows.Count > 0)
                {
                    premiumTotal = Convert.ToDecimal(premiumTotals.Rows[0]["PolicyTotal"]);
                }

                // Obtengo el valor de la prima Neta.
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeComponentAmount.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeComponentAmount.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeComponentAmount.Properties.ComponentCode, Convert.ToInt32(Components.Prime));

                UIView premiumComponents = _dataFacadeManager.GetDataFacade().GetView("PrimeComponentAmountView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (premiumComponents.Rows.Count > 0)
                {
                    netPremium = Convert.ToDecimal(premiumComponents.Rows[0]["ComponentAmount"]);
                }

                // Grabo los valores de los componentes
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeComponentAmount.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimeComponentAmount.Properties.EndorsementId, endorsementId);

                premiumComponents = _dataFacadeManager.GetDataFacade().GetView("PrimeComponentAmountView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (premiumComponents.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in premiumComponents)
                    {
                        int componentCollectionId = 0;
                        decimal componentAmount = 0;

                        if (premiumTotal != 0)
                        {
                            componentAmount = Math.Round((paidAmount * (Convert.ToDecimal(dataRow["ComponentAmount"]) / premiumTotal)), 2);
                        }

                        Amount amount = new Amount()
                        {
                            Currency = new Currency() { Id = currencyId },
                            Value = componentAmount
                        };
                        ExchangeRate exchange = new ExchangeRate() { SellAmount = Math.Round(exchangeRate, 2) };
                        Amount localAmount = new Amount() { Value = Math.Round((componentAmount * exchangeRate), 2) };

                        componentCollectionId = _componentCollectionDAO.SaveComponentCollection(componentCollectionId, premiumReceivableId, Convert.ToInt32(dataRow["ComponentCode"]), amount, exchange, localAmount);
                        if (componentCollectionId > 0)
                        {
                            isComponentSaved = true;
                        }
                        else
                        {
                            isComponentSaved = false;
                            ObjectCriteriaBuilder componentCriteriaBuilder = new ObjectCriteriaBuilder();
                            componentCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableId);
                            BusinessCollection componentBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                               typeof(ACCOUNTINGEN.ComponentCollection), componentCriteriaBuilder.GetPredicate()));

                            if (componentBusinessCollection.Count > 0)
                            {
                                foreach (ACCOUNTINGEN.ComponentCollection componentCollection in componentBusinessCollection.OfType<ACCOUNTINGEN.ComponentCollection>())
                                {
                                    ObjectCriteriaBuilder prefixCriteriaBuilder = new ObjectCriteriaBuilder();
                                    prefixCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixComponentCollection.Properties.ComponentCollectionCode, componentCollection.ComponentCollectionCode);
                                    BusinessCollection prefixBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                        typeof(ACCOUNTINGEN.PrefixComponentCollection), prefixCriteriaBuilder.GetPredicate()));
                                    if (prefixBusinessCollection.Count > 0)
                                    {
                                        foreach (ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollection in prefixBusinessCollection.OfType<ACCOUNTINGEN.PrefixComponentCollection>())
                                        {
                                            _prefixComponentCollectionDAO.DeletePrefixComponentCollection(prefixComponentCollection.PrefixComponentCollectionCode);
                                        }
                                    }
                                    _componentCollectionDAO.DeleteComponentCollection(componentCollection.ComponentCollectionCode);
                                }
                            }
                        }

                        if (isComponentSaved)
                        {
                            ObjectCriteriaBuilder premiumCriteriaBuilder = new ObjectCriteriaBuilder();
                            premiumCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimePrefixComponent.Properties.PolicyId, policyId);
                            premiumCriteriaBuilder.And();
                            premiumCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimePrefixComponent.Properties.EndorsementId, endorsementId);
                            premiumCriteriaBuilder.And();
                            premiumCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimePrefixComponent.Properties.ComponentCode, Convert.ToInt32(Enums.Components.Prime));

                            UIView premiumPrefixComponents = _dataFacadeManager.GetDataFacade().GetView("PrimePrefixComponentView",
                                                            premiumCriteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                            if (premiumPrefixComponents.Rows.Count > 0)
                            {
                                foreach (DataRow prefixDataRow in premiumPrefixComponents)
                                {
                                    int prefixComponentCollectionId = 0;
                                    decimal prefixComponentAmount = 0;

                                    //prefixDataRow["ComponentAmount"]) / netPremium ==> porcentaje de prima
                                    prefixComponentAmount = componentAmount * (Convert.ToDecimal(prefixDataRow["ComponentAmount"]) / netPremium);

                                    Amount prefixAmount = new Amount()
                                    {
                                        Currency = new Currency() { Id = currencyId },
                                        Value = Math.Round(prefixComponentAmount, 2)
                                    };
                                    exchange = new ExchangeRate() { SellAmount = Math.Round(exchangeRate, 2) };
                                    localAmount = new Amount() { Value = Math.Round((componentAmount * exchangeRate), 2) };

                                    prefixComponentCollectionId = _prefixComponentCollectionDAO.SavePrefixComponentCollection(prefixComponentCollectionId, componentCollectionId,
                                        Convert.ToInt32(prefixDataRow["LineBusinessCode"]), Convert.ToInt32(prefixDataRow["SubLineBusinessCode"]), prefixAmount, exchange, localAmount);
                                    if (prefixComponentCollectionId > 0)
                                    {
                                        isPrefixSaved = true;
                                    }
                                    else
                                    {
                                        isPrefixSaved = false;
                                        ObjectCriteriaBuilder collectionCriteriaBuilder = new ObjectCriteriaBuilder();

                                        collectionCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableId);
                                        BusinessCollection businessCollectionComponent = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                            typeof(ACCOUNTINGEN.ComponentCollection), collectionCriteriaBuilder.GetPredicate()));

                                        if (businessCollectionComponent.Count > 0)
                                        {
                                            foreach (ACCOUNTINGEN.ComponentCollection componentCollection in businessCollectionComponent.OfType<ACCOUNTINGEN.ComponentCollection>())
                                            {
                                                ObjectCriteriaBuilder prefixCriteriaBuilder = new ObjectCriteriaBuilder();
                                                prefixCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixComponentCollection.Properties.ComponentCollectionCode, componentCollection.ComponentCollectionCode);
                                                BusinessCollection prefixBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                    typeof(ACCOUNTINGEN.PrefixComponentCollection), prefixCriteriaBuilder.GetPredicate()));
                                                if (prefixBusinessCollection.Count > 0)
                                                {
                                                    foreach (ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollection in prefixBusinessCollection.OfType<ACCOUNTINGEN.PrefixComponentCollection>())
                                                    {
                                                        _prefixComponentCollectionDAO.DeletePrefixComponentCollection(prefixComponentCollection.PrefixComponentCollectionCode);
                                                    }
                                                }
                                                _componentCollectionDAO.DeleteComponentCollection(componentCollection.ComponentCollectionCode);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (isComponentSaved && isPrefixSaved)
                {
                    // Comprueba que los valores cuadren
                    decimal componentSum = 0;

                    // Por componente de cobro
                    ObjectCriteriaBuilder collectCriteriaBuilder = new ObjectCriteriaBuilder();
                    collectCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableId);

                    BusinessCollection businessCollectionComponent = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.ComponentCollection), collectCriteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.ComponentCollection componentCollection in businessCollectionComponent.OfType<ACCOUNTINGEN.ComponentCollection>())
                    {
                        componentSum += Convert.ToDecimal(componentCollection.IncomeAmount);
                    }

                    if (Math.Abs(componentSum) < Math.Abs(paidAmount))
                    {
                        decimal remainingAmount = Math.Abs(paidAmount) - Math.Abs(componentSum);
                        int newComponentCollectionId = 0;
                        int newPremiumReceivableId = 0;
                        int newComponentCode = 0;
                        Amount amount = new Amount();
                        ExchangeRate exchange = new ExchangeRate();
                        Amount localAmount = new Amount();

                        ObjectCriteriaBuilder componentCriteriaBuilder = new ObjectCriteriaBuilder();
                        componentCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableId);
                        componentCriteriaBuilder.And();
                        componentCriteriaBuilder.Property(ACCOUNTINGEN.ComponentCollection.Properties.IncomeAmount);
                        componentCriteriaBuilder.Distinct();
                        componentCriteriaBuilder.Constant(0);

                        BusinessCollection componentBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ComponentCollection), componentCriteriaBuilder.GetPredicate()));

                        foreach (ACCOUNTINGEN.ComponentCollection componentCollection in componentBusinessCollection.OfType<ACCOUNTINGEN.ComponentCollection>())
                        {
                            decimal newAmount = 0;
                            if (componentCollection.IncomeAmount > 0)
                            {
                                newAmount = Convert.ToDecimal(componentCollection.IncomeAmount + remainingAmount);
                            }

                            if (componentCollection.IncomeAmount < 0)
                            {
                                newAmount = Convert.ToDecimal(componentCollection.IncomeAmount - remainingAmount);
                            }

                            newComponentCollectionId = componentCollection.ComponentCollectionCode;
                            newPremiumReceivableId = Convert.ToInt32(componentCollection.PremiumReceivableTransCode);
                            newComponentCode = Convert.ToInt32(componentCollection.ComponentId);
                            amount.Currency = new Currency() { Id = currencyId };
                            amount.Value = Convert.ToDecimal(newAmount);
                            exchange.SellAmount = exchangeRate;
                            localAmount.Value = Convert.ToDecimal(newAmount) * exchangeRate;
                        }

                        isComponentSaved = _componentCollectionDAO.UpdateComponentCollection(newComponentCollectionId, newPremiumReceivableId, newComponentCode, amount, exchange, localAmount);
                    }

                    // Por componente de ramo
                    foreach (ACCOUNTINGEN.ComponentCollection componentCollection in businessCollectionComponent.OfType<ACCOUNTINGEN.ComponentCollection>())
                    {
                        decimal prefixComponentSum = 0;

                        ObjectCriteriaBuilder prefixCriteriaBuilder = new ObjectCriteriaBuilder();
                        prefixCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixComponentCollection.Properties.ComponentCollectionCode, componentCollection.ComponentCollectionCode);

                        BusinessCollection businessPrefixComponentCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                            typeof(ACCOUNTINGEN.PrefixComponentCollection), prefixCriteriaBuilder.GetPredicate()));

                        foreach (ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollection in businessPrefixComponentCollection.OfType<ACCOUNTINGEN.PrefixComponentCollection>())
                        {
                            prefixComponentSum += Convert.ToDecimal(prefixComponentCollection.IncomeAmount);
                        }

                        if (prefixComponentSum < componentCollection.IncomeAmount)
                        {
                            decimal remainingPrexiAmount = Convert.ToDecimal(componentCollection.IncomeAmount - prefixComponentSum);
                            int newComponentPrexiCollectionId = 0;
                            int newComponentCollectionId = 0;
                            int newLineBusinessId = 0;
                            int newSubLineBusinessId = 0;
                            decimal incomeAmount = 0;
                            Amount prefixAmount = new Amount();

                            ObjectCriteriaBuilder prefixComponentCriteriaBuilder = new ObjectCriteriaBuilder();

                            prefixComponentCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixComponentCollection.Properties.ComponentCollectionCode, componentCollection.ComponentCollectionCode);

                            BusinessCollection businessCollectionPrefixCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(ACCOUNTINGEN.PrefixComponentCollection), prefixComponentCriteriaBuilder.GetPredicate()));

                            foreach (ACCOUNTINGEN.PrefixComponentCollection prefixComponent in businessCollectionPrefixCollection.OfType<ACCOUNTINGEN.PrefixComponentCollection>())
                            {
                                newComponentPrexiCollectionId = prefixComponent.PrefixComponentCollectionCode;
                                newComponentCollectionId = Convert.ToInt32(prefixComponent.ComponentCollectionCode);
                                newLineBusinessId = Convert.ToInt32(prefixComponent.LineBusinessCode);
                                newSubLineBusinessId = Convert.ToInt32(prefixComponent.SubLineBusinessCode);
                                incomeAmount = Convert.ToDecimal(prefixComponent.IncomeAmount);
                            }

                            prefixAmount.Value = Convert.ToDecimal(incomeAmount + remainingPrexiAmount);
                            prefixAmount.Currency = new Currency();
                            prefixAmount.Currency.Id = currencyId;
                            ExchangeRate exchange = new ExchangeRate() { SellAmount = exchangeRate };
                            Amount localAmount = new Amount() { Value = Convert.ToDecimal(incomeAmount + remainingPrexiAmount) * exchangeRate };
                            isPrefixSaved = _prefixComponentCollectionDAO.UpdatePrefixComponentCollection(newComponentPrexiCollectionId, newComponentCollectionId, newLineBusinessId, newSubLineBusinessId, prefixAmount, exchange, localAmount);
                        }
                    }
                }
                if (isComponentSaved && isPrefixSaved)
                {
                    isSaved = true;
                }
            }
            catch (BusinessException)
            {
                isSaved = false;
            }

            return isSaved;
        }
        /// <summary>
        /// SaveDepositPremiumTransaction
        /// Graba de un item de primas x cobrar
        /// </summary>
        /// <param name="depositPremiumTransaction"></param>
        /// <param name="premiumReceivableCode"></param>
        /// <param name="payerTypeId"></param>
        /// <returns>int</returns>
        public int SaveDepositPremiumTransaction(DepositPremiumTransactionDTO depositPremiumTransaction, int premiumReceivableCode, int payerTypeId)
        {
            try
            {
                int saved = 0;

                saved = depositPremiumTransaction.Id == 0 ? _depositPremiumTransactionDAO.SaveDepositPremiumTransaction(depositPremiumTransaction.ToModel(), premiumReceivableCode, payerTypeId).ToDTO().Id :
                    _depositPremiumTransactionDAO.UpdateDepositPremiumTransaction(depositPremiumTransaction.ToModel(), premiumReceivableCode, payerTypeId).ToDTO().Id;

                return saved;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetTempApplicationBySourceCode
        /// Obtiene un temporal de aplicación por su tipo de origen
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO GetTempApplicationBySourceCode(int moduleId, int sourceCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.ModuleCode, moduleId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.SourceCode, sourceCode);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempApplication), criteriaBuilder.GetPredicate()));

                Models.Imputations.Application application = new Models.Imputations.Application();

                foreach (ACCOUNTINGEN.TempApplication imputationEntity in businessCollection.OfType<ACCOUNTINGEN.TempApplication>())
                {
                    application.Id = imputationEntity.TempAppCode;
                    application.RegisterDate = Convert.ToDateTime(imputationEntity.RegisterDate);
                    application.UserId = Convert.ToInt32(imputationEntity.UserCode);
                    application.SourceId = Convert.ToInt32(imputationEntity.SourceCode);
                }

                return application.ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        /// <summary>
        /// SaveClaimsPaymentRequest
        /// Graba pago de siniestro
        /// </summary>
        /// <param name="claimsPaymentRequestTransaction"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>bool</returns>
        private bool SaveClaimsPaymentRequest(ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestTransaction,
                                              int imputationId, decimal exchangeRate, int paymentSourceId, int paymentNumber, DateTime firstPaymentDue)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ClaimPaymentRequestTrans claimPaymentRequestTrans = EntityAssembler.CreateClaimPayment(
                                                     claimsPaymentRequestTransaction.ToModel(), imputationId, exchangeRate, paymentSourceId, paymentNumber, firstPaymentDue);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(claimPaymentRequestTrans);

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }
        /// <summary>
        /// DeleteClaimsPaymentRequestByImputationId
        /// Borra una solicitud de pago de siniestro por imputación
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        private bool DeleteClaimPaymentRequestByImputationId(int imputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.ApplicationCode, imputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ClaimPaymentRequestTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.ClaimPaymentRequestTrans claimPaymentEntity in businessCollection.OfType<ACCOUNTINGEN.ClaimPaymentRequestTrans>())
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(claimPaymentEntity);
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        public bool DeleteTempApplicationByTempAplicationIdModuleId(int tempApplicationId, int moduleId)
        {
            TempReinsuranceCheckingAccountTransactionItemDAO _tempReinsuranceCheckingAccountTransactionItemDAO = new TempReinsuranceCheckingAccountTransactionItemDAO();
            TempCoinsuranceCheckingAccountTransactionDAO _tempCoinsuranceCheckingAccountTransactionDAO = new TempCoinsuranceCheckingAccountTransactionDAO();
            TempBrokerCheckingAccountTransactionItemDAO _tempBrokerCheckingAccountTransactionItemDAO = new TempBrokerCheckingAccountTransactionItemDAO();
            TempCoinsuranceCheckingAccountTransactionItemDAO _tempCoinsuranceCheckingAccountTransactionItemDAO = new TempCoinsuranceCheckingAccountTransactionItemDAO();
            TempApplicationAccountingItemDAO tempApplicationAccountingItemDAO = new TempApplicationAccountingItemDAO();

            try
            {
                // ELIMINA LOS TEMPORALES
                // Primas por Cobrar
                //DeleteTempApplicationPremiumTransaction(tempApplicationId);

                // Cta. Cte. Agentes
                _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempApplicationId);
                _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempApplicationId);

                // Cta. Cte. Coaseguros
                _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempApplicationId);
                _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempApplicationId);

                // Cta. Cte. Reaseguros
                _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempApplicationId);
                _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempApplicationId);

                // Contabilidad
                tempApplicationAccountingItemDAO.DeleteTempApplicationAccountingsByTempApplicationId(tempApplicationId);

                // Borra temporales Reversion de Primas
                TempApplicationPremiumItemDAO.DeleteTempPremiumReversionTransactionItem(tempApplicationId);

                // Borra temporales solicitudes de pagos varios
                new TempApplicationPremiumDAO().DeleteTempApplicationPremiumByTempApplication(tempApplicationId);

                // Borra temporales solicitudes de pago de siniestros
                _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempApplicationId);

                if (moduleId != Convert.ToInt32(ApplicationTypes.PreLiquidation))
                {
                    tempApplicationDAO.DeleteTempApplication(tempApplicationId);
                }
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }



        ///<summary>
        /// DeletePremiumReceivableTransaction
        /// Borrado de primas x cobrar
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplicationPremiumTransaction(int tempApplicationId)
        {
            TempApplicationPremiumItemDAO tempApplicationPremiumItemDAO = new TempApplicationPremiumItemDAO();
            ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();

            bool isDeleted = false;

            try
            {
                // Obtengo los temp premium receivable a partir del tempImputationId
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                                             (typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempApplicationPremium tempPremiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.TempApplicationPremium>())
                    {
                        // Obtengo y elimino los pagos en exceso.
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempUsedDepositPremium.Properties.TempPremiumReceivableTransCode, tempPremiumReceivableEntity.TempAppPremiumCode);
                        BusinessCollection businessCollectionPremiums = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                            (typeof(ACCOUNTINGEN.TempUsedDepositPremium), criteriaBuilder.GetPredicate()));

                        if (businessCollectionPremiums.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.TempUsedDepositPremium tempUsedDepositPremiumEntity in businessCollectionPremiums.OfType<ACCOUNTINGEN.TempUsedDepositPremium>())
                            {
                                _tempUsedDepositPremiumDAO.DeleteTempUsedDepositPremium(tempUsedDepositPremiumEntity.TempUsedDepositPremiumCode);
                            }
                        }
                        // Elimino los temp premium component
                        tempApplicationPremiumComponentDAO.DeleteTempApplicationPremiumComponentsByTemApp(tempPremiumReceivableEntity.TempAppPremiumCode);
                        // Elimino los temp premium commiss
                        applicationPremiumCommisionDAO.DeleteTempApplicationPremiumCommisses(tempPremiumReceivableEntity.TempAppPremiumCode);
                        // Elimino los temp premium receivable
                        tempApplicationPremiumItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivableEntity.TempAppPremiumCode);
                    }
                }

                isDeleted = true;
            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }

            return isDeleted;
        }

        /// <summary>
        /// UpdatePaymentRequestToStatusPayed
        /// Actualiza el estado de la solicitud de pago a "Pagado" en PaymentRequest
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="paymentUserId"></param>
        /// <param name="executedPaymentDate"></param>
        public void UpdatePaymentRequestToStatusPayed(int imputationId, int paymentUserId, DateTime executedPaymentDate)
        {
            try
            {
                // Busca por imputación 
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.ApplicationCode, imputationId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(ACCOUNTINGEN.ClaimPaymentRequestTrans), criteriaBuilder.GetPredicate()));


                foreach (ACCOUNTINGEN.ClaimPaymentRequestTrans claimPaymentRequestTransEntity in businessCollection.OfType<ACCOUNTINGEN.ClaimPaymentRequestTrans>())
                {
                    PRMOD.PaymentRequest paymentRequest = new PRMOD.PaymentRequest()
                    {
                        Id = Convert.ToInt32(claimPaymentRequestTransEntity.PaymentRequestCode),
                        PaymentDate = executedPaymentDate,
                        UserId = paymentUserId
                    };

                    //1 Estado Pagado en PaymentRequest
                    // parametrizationServices.UpdatePaymentRequest(paymentRequest, 1);  TODO ACE-650 Definir de donde se lo va tomar talvez ACC
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public CollectApplication SaveApplicationPortalPayment(int sourceCode, int tempApplicationId, int moduleId,
                                                string comments, int statusId, int userId, int tempSourceCode, int technicalTransaction, DateTime accountingDate)
        {

            Collect collect = null;
            if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                CollectDAO collectDAO = new CollectDAO();
                collect = collectDAO.GetCollectByCollectId(sourceCode);
            }

            using (Context.Current)
            {
                //CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();
                //using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                //{
                try
                {
                    bool isConverted = false;
                    int imputationId = 0;
                    Models.Imputations.Application application = new Models.Imputations.Application();
                    DateTime registerDate = DateTime.Now;
                    Transaction transactionModel = new Transaction();

                    application.Id = imputationId;
                    application.RegisterDate = registerDate;
                    application.ModuleId = moduleId;

                    application.UserId = userId;
                    //int technicalTransaction = GetTechnicalTransaction();
                    application.AccountingDate = accountingDate;
                    // Graba la cabecera de imputación
                    if (collect != null)
                    {
                        if (collect.Payer != null)
                            application.IndividualId = collect.Payer.IndividualId;
                        if (collect.CollectControlId > 0)
                        {
                            CollectControlDAO collectControlDAO = new CollectControlDAO();
                            var collectControl = collectControlDAO.GetCollectControlByCollectControlId(collect.CollectControlId);
                            if (collectControl != null && collectControl.Branch != null)
                                application.BranchId = collectControl.Branch.Id;

                        }
                    }

                    using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                    {
                        try
                        {
                            application = new ApplicationDAO().SaveImputation(application, sourceCode, technicalTransaction);
                            transaction.Complete();
                        }
                        catch (Exception ex)
                        {
                            transaction.Dispose();
                            throw new BusinessException("", ex);
                        }
                    }

                    transactionModel.TechnicalTransaction = technicalTransaction;
                    imputationId = application.Id;

                    #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS


                    using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                    {
                        try
                        {
                            // CONVIERTE DE TEMPORALES A TABLAS REALES
                            isConverted = ConvertTempApplicationtoRealApplication(tempSourceCode, application.ModuleId, application.Id, accountingDate, userId);
                            transaction.Complete();
                        }
                        catch (Exception ex)
                        {
                            transaction.Dispose();
                            throw new BusinessException("", ex);
                        }
                    }



                    #endregion

                    if (isConverted) // ELIMINA LOS TEMPORALES
                    {
                        using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                        {
                            try
                            {
                                //Elimina Temporales
                                DeleteTempApplicationByTempAplicationIdModuleId(tempApplicationId, moduleId);
                                transaction.Complete();
                            }
                            catch (Exception ex)
                            {
                                transaction.Dispose();
                                throw new BusinessException("", ex);
                            }
                        }

                        using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                        {
                            try
                            {
                                // Actualiza estatus en Solicitud de pago a pagado
                                UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);
                                transaction.Complete();
                            }
                            catch (Exception ex)
                            {
                                transaction.Dispose();
                                throw new BusinessException("", ex);
                            }
                        }


                        

                        #region APLICACIÓN DE RECIBO

                        if (application.ModuleId == Convert.ToInt32(ApplicationTypes.Collect))
                        {
                            // ACTUALIZA ESTADO DE BILL

                            string[] accountindDateSplit = Convert.ToString(accountingDate).Split();
                            collect.Id = sourceCode;
                            collect.Date = Convert.ToDateTime(accountindDateSplit[0]);
                            collect.UserId = userId;
                            collect.Status = Convert.ToInt32(CollectStatus.Applied);
                            collect.Comments = comments == "" ? null : comments;
                            collect.Transaction = new Transaction();

                            using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                            {
                                try
                                {
                                    new CollectDAO().UpdateCollect(collect, -1);
                                    transaction.Complete();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Dispose();
                                    throw new BusinessException("", ex);
                                }
                            }
                        }

                        #endregion
                    }


                    //transaction.Complete();

                    return new CollectApplication() { Application = application, Transaction = transactionModel };
                }
                catch (BusinessException ex)
                {
                    //transaction.Dispose();
                    // LLAMAR A ELIMINAR INFORMACIÓN
                    throw new BusinessException(Resources.Resources.BusinessException);
                }
                //}
            }
        }
    }
}
