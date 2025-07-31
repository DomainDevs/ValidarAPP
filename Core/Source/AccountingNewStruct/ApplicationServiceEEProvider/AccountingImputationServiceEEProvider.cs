using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using ACCOUNTINGCONCEPTS = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using AccountingModels = Sistran.Core.Application.AccountingServices.EEProvider.Models;
using claimsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using PRMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using CoreTransaction = Sistran.Core.Framework.Transactions;
using imputation = Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using ImputationModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using PaymentMethod = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UNDMOD = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingImputationServiceEEProvider : IAccountingImputationService
    {
        #region Constants

        public const int StatusQuotaPartial = 1; // No se crean enum de esto ya que pertenece a Emisión
        public const int StatusQuotaTotal = 2;   // No se crean enum de esto ya que pertenece a Emisión

        // Variable para lectura de UIViews
        private int Rows = 50;

        #endregion

        #region Instance Variables

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #region Interfaz

        #endregion Interfaz

        #region DAOs

        /// <summary>
        /// Declaración de DAOs
        /// </summary>
        readonly TempPremiumReceivableTransactionDAO _tempPremiumRecievableTransactionDAO = new TempPremiumReceivableTransactionDAO();
        readonly TempPremiumReceivableTransactionItemDAO _tempPremiumReceivableTransactionItemDAO = new TempPremiumReceivableTransactionItemDAO();
        readonly ImputationDAO _imputationDAO = new ImputationDAO();
        readonly TempImputationDAO _tempImputationDAO = new TempImputationDAO();
        readonly PremiumReceivableTransactionItemDAO _premiumReceivableTransactionItemDAO = new PremiumReceivableTransactionItemDAO();
        readonly DepositPremiumTransactionDAO _depositPremiumTransactionDAO = new DepositPremiumTransactionDAO();
        readonly TempBrokerCheckingAccountTransactionDAO _tempBrokerCheckingAccountTransactionDAO = new TempBrokerCheckingAccountTransactionDAO();
        readonly TempBrokerCheckingAccountTransactionItemDAO _tempBrokerCheckingAccountTransactionItemDAO = new TempBrokerCheckingAccountTransactionItemDAO();
        readonly BrokerCheckingAccountTransactionItemDAO _brokerCheckingAccountTransactionItemDAO = new BrokerCheckingAccountTransactionItemDAO();
        readonly TempClaimPaymentRequestDAO _tempClaimPaymentRequestDAO = new TempClaimPaymentRequestDAO();
        readonly TempClaimsPaymentRequestTransactionDAO _tempClaimsPaymentRequestTransactionDAO = new TempClaimsPaymentRequestTransactionDAO();
        readonly TempUsedDepositPremiumDAO _tempUsedDepositPremiumDAO = new TempUsedDepositPremiumDAO();
        readonly CollectDAO _collectDAO = new CollectDAO();
        readonly UsedAmountDAO _usedAmountDAO = new UsedAmountDAO();
        readonly ComponentCollectionDAO _componentCollectionDAO = new ComponentCollectionDAO();
        readonly PrefixComponentCollectionDAO _prefixComponentCollectionDAO = new PrefixComponentCollectionDAO();
        readonly TempReinsuranceCheckingAccountTransactionDAO _tempReinsuranceCheckingAccountTransactionDAO = new TempReinsuranceCheckingAccountTransactionDAO();
        readonly TempReinsuranceCheckingAccountTransactionItemDAO _tempReinsuranceCheckingAccountTransactionItemDAO = new TempReinsuranceCheckingAccountTransactionItemDAO();
        readonly ReinsuranceCheckingAccountTransactionItemDAO _reinsuranceCheckingAccountTransactionItemDAO = new ReinsuranceCheckingAccountTransactionItemDAO();
        readonly TempDailyAccountingTransactionItemDAO _tempDailyAccountingTransactionItemDAO = new TempDailyAccountingTransactionItemDAO();
        readonly TempDailyAccountingTransactionDAO _tempDailyAccountingTransactionDAO = new TempDailyAccountingTransactionDAO();
        readonly DailyAccountingTransactionItemDAO _dailyAccountingTransactionItemDAO = new DailyAccountingTransactionItemDAO();
        readonly JournalEntryDAO _journalEntryDAO = new JournalEntryDAO();
        readonly TempJournalEntryDAO _tempJournalEntryDAO = new TempJournalEntryDAO();
        readonly DiscountedCommissionDAO _discountedCommissionDAO = new DiscountedCommissionDAO();
        readonly TempPreLiquidationDAO _tempPreLiquidationDAO = new TempPreLiquidationDAO();
        readonly PreLiquidationDAO _preLiquidationDAO = new PreLiquidationDAO();
        readonly TempCoinsuranceCheckingAccountTransactionItemDAO _tempCoinsuranceCheckingAccountTransactionItemDAO = new TempCoinsuranceCheckingAccountTransactionItemDAO();
        readonly TempCoinsuranceCheckingAccountTransactionDAO _tempCoinsuranceCheckingAccountTransactionDAO = new TempCoinsuranceCheckingAccountTransactionDAO();
        readonly CoinsuranceCheckingAccountTransactionItemDAO _coinsuranceCheckingAccountTransactionItemDAO = new CoinsuranceCheckingAccountTransactionItemDAO();
        readonly TempPaymentOrderDAO _tempPaymentOrderDAO = new TempPaymentOrderDAO();
        readonly PaymentOrderDAO _paymentOrderDAO = new PaymentOrderDAO();
        readonly BrokerBalanceDAO _brokerBalanceDAO = new BrokerBalanceDAO();
        readonly CoinsuranceBalanceDAO _coinsuranceBalanceDAO = new CoinsuranceBalanceDAO();
        readonly AgentCoinsuranceCheckingAccountDAO _agentCoinsuranceCheckingAccountDAO = new AgentCoinsuranceCheckingAccountDAO();
        readonly AgentCommissionClosureDAO _agentCommissionClosureDAO = new AgentCommissionClosureDAO();
        readonly AgentCommissionBalanceItemDAO _agentCommissionBalanceItemDAO = new AgentCommissionBalanceItemDAO();
        readonly PaymentOrderBrokerAccountDAO _paymentOrderBrokerAccountDAO = new PaymentOrderBrokerAccountDAO();
        readonly PaymentRequestDAO _paymentRequestDAO = new PaymentRequestDAO();
        readonly TempPaymentRequestTransactionDAO _tempPaymentRequestTransactionDAO = new TempPaymentRequestTransactionDAO();
        readonly TempDailyAccountingAnalysisCodeDAO _tempDailyAccountingAnalysisCodeDAO = new TempDailyAccountingAnalysisCodeDAO();
        readonly TempDailyAccountingCostCenterDAO _tempDailyAccountingCostCenterDAO = new TempDailyAccountingCostCenterDAO();
        readonly DailyAccountingAnalysisCodeDAO _dailyAccountingAnalysisCodeDAO = new DailyAccountingAnalysisCodeDAO();
        readonly DailyAccountingCostCenterDAO _dailyAccountingCostCenterDAO = new DailyAccountingCostCenterDAO();
        readonly PaymentDAO _paymentDAO = new PaymentDAO();
        readonly PaymentRequestClaimDAO _paymentRequestClaimDAO = new PaymentRequestClaimDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region ImputationDTO

        /// <summary>
        /// SaveImputationRequest
        /// Ejecuta el proceso de grabación de una imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        public bool SaveImputationRequest(int sourceCode, int tempImputationId, int imputationTypeId,
                                           string comments, int statusId, int userId, int tempSourceCode)
        {
            SaveImputation(sourceCode, tempImputationId, imputationTypeId, comments, statusId, userId, tempSourceCode);
            return true;
        }

        /// <summary>
        /// SaveImputation
        /// Graba la imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>CollectImputation</returns>
        public CollectImputationDTO SaveImputation(int sourceCode, int tempImputationId, int imputationTypeId,
                                                string comments, int statusId, int userId, int tempSourceCode)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    bool isConverted = false;
                    string accountingDate;
                    int imputationId = 0;
                    Imputation imputation = new Imputation();
                    DateTime registerDate = DateTime.Now;
                    Collect collect = new Collect();
                    Transaction transactionModel = new Transaction();

                    imputation.Id = imputationId;
                    imputation.Date = registerDate;

                    switch (imputationTypeId)
                    {
                        case 1:
                            imputation.ImputationType = ImputationTypes.Collect;
                            break;
                        case 2:
                            imputation.ImputationType = ImputationTypes.JournalEntry;
                            break;
                        case 3:
                            imputation.ImputationType = ImputationTypes.Collect;
                            break;
                        case 4:
                            imputation.ImputationType = ImputationTypes.PaymentOrder;
                            break;
                    }

                    imputation.UserId = userId;
                    int technicalTransaction = GetTechnicalTransaction();
                    // Graba la cabecera de imputación
                    imputation = _imputationDAO.SaveImputation(imputation, sourceCode, technicalTransaction);

                    transactionModel.TechnicalTransaction = technicalTransaction;
                    imputationId = imputation.Id;

                    #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                    // GRABA PRIMAS EN REALES
                    isConverted = ConvertTemptoRealPremiumReceivableTransaction(tempSourceCode, imputationTypeId, imputation.Id);
                    if (isConverted)
                    {
                        // GRABA MOVIMIENTOS CUENTA CORRIENTE AGENTES EN REALES Y ELIMINA LAS TEMPORALES
                        isConverted = ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(tempSourceCode, imputationTypeId, imputation.Id);
                        if (isConverted)
                        {
                            // GRABA MOVIMIENTOS CUENTA CORRIENTE REASEGURADORAS EN REALES Y ELIMINA LAS TEMPORALES
                            isConverted = ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(tempSourceCode, imputationTypeId, imputation.Id);
                            if (isConverted)
                            {
                                // Solcitudes de pago siniestros 
                                isConverted = ConvertTempClaimPaymentRequestToClaimPaymentRequest(tempSourceCode, imputationTypeId, imputation.Id, userId);
                                if (isConverted)
                                {
                                    // GRABA MOVIMIENTOS CUENTA CORRIENTE COASEGUROS EN REALES
                                    isConverted = ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(tempSourceCode, imputationTypeId, imputation.Id);
                                    if (isConverted)
                                    {
                                        // GRABA MOVIMIENTOS CONTABILIDAD EN REALES
                                        isConverted = ConvertTempDailyAccountingToDailyDailyAccounting(tempSourceCode, imputationTypeId, imputation.Id);

                                        if (isConverted)
                                        {
                                            // Solcitudes de pagos varios 
                                            isConverted = ConvertTempPaymentRequestToPaymentRequest(tempSourceCode, imputationTypeId, imputation.Id, userId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    if (isConverted) // ELIMINA LOS TEMPORALES
                    {
                        // Primas por Cobrar
                        DeleteTempPremiumReceivableTransaction(tempImputationId);

                        // Cta. Cte. Agentes
                        _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                        _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                        // Cta. Cte. Coaseguros
                        _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                        _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                        // Cta. Cte. Reaseguros
                        _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                        _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                        // Contabilidad
                        _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                        // Borra temporales solicitudes de pagos varios
                        _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                        // Borra temporales solicitudes de pago de siniestros
                        _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);

                        // Actualiza estatus en Solicitud de pago a pagado
                        UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

                        // Imputación
                        if (imputationTypeId != Convert.ToInt32(ImputationTypes.PreLiquidation))
                        {
                            _tempImputationDAO.DeleteTempImputation(tempImputationId);
                        }

                        #region APLICACIÓN DE RECIBO

                        if (imputationTypeId == Convert.ToInt32(ImputationTypes.Collect))
                        {
                            // ACTUALIZA ESTADO DE BILL
                            accountingDate = Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))));

                            string[] accountingDateSplit = accountingDate.Split();

                            collect.Id = sourceCode;
                            collect.Date = Convert.ToDateTime(accountingDateSplit[0]);
                            collect.UserId = userId;
                            collect.Status = statusId;
                            collect.Comments = comments == "" ? null : comments;
                            collect.Transaction = new Transaction();

                            _collectDAO.UpdateCollect(collect, -1);
                        }

                        #endregion
                    }


                    #region APLICACIÓN DE PRELIQUIDATION

                    if (imputationTypeId == (int)ImputationTypes.PreLiquidation)
                    {
                        PreLiquidation preliquidation = _preLiquidationDAO.GetPreLiquidation(tempSourceCode);

                        // Actualización del estado de preliquidación.
                        PreLiquidation preLiquidationNew = new PreLiquidation();
                        preLiquidationNew.Id = preliquidation.Id;
                        preLiquidationNew.Company = new Company()
                        {
                            IndividualId = preliquidation.Company.IndividualId
                        };
                        preLiquidationNew.Branch = new Branch() { Id = preliquidation.Branch.Id };
                        preLiquidationNew.Description = preliquidation.Description;
                        preLiquidationNew.Imputation = preliquidation.Imputation;
                        preLiquidationNew.IsTemporal = false;
                        preLiquidationNew.Payer = new Individual()
                        {
                            IndividualId = preliquidation.Payer.IndividualId
                        };
                        preLiquidationNew.PersonType = new PersonType()
                        {
                            Id = preliquidation.PersonType.Id
                        };
                        preLiquidationNew.RegisterDate = DateTime.Now;
                        preLiquidationNew.SalePoint = new SalePoint()
                        {
                            Id = preliquidation.SalePoint.Id
                        };
                        preLiquidationNew.Status = Convert.ToInt32(CollectStatus.Applied); //aplicado

                        _preLiquidationDAO.UpdatePreLiquidation(preLiquidationNew);
                    }

                    #endregion

                    transaction.Complete();

                    return new CollectImputation() { Imputation = imputation, Transaction = transactionModel }.ToDTO();
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        /// <summary>
        /// SaveImputationRequestBill
        /// Ejecuta el proceso de grabación de una imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="comments"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        public bool SaveImputationRequestBill(int sourceCode, int tempImputationId, string comments,
                                              int userId, int tempSourceCode)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error
            try
            {
                bool isConverted = false;
                string accountingDate;
                int imputationId = 0;

                Imputation imputation = new Imputation();

                DateTime registerDate = DateTime.Now;
                Collect collect = new Collect();

                imputation.Id = imputationId;
                imputation.Date = registerDate;
                imputation.ImputationType = ImputationTypes.Collect;
                imputation.UserId = userId;

                int technicalTransaction = GetTechnicalTransaction();
                // Graba la cabecera de Imputación
                imputation = _imputationDAO.SaveImputation(imputation, sourceCode, technicalTransaction);

                imputationId = imputation.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                // GRABA PRIMAS EN REALES
                isConverted = ConvertTemptoRealPremiumReceivableTransaction(tempSourceCode,
                                                                               Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                if (isConverted)
                {
                    // GRABA MOVIMIENTOS CUENTA CORRIENTE AGENTES EN REALES Y ELIMINA LAS TEMPORALES
                    isConverted = ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(tempSourceCode,
                                                                                   Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                    if (isConverted)
                    {
                        // GRABA MOVIMIENTOS CUENTA CORRIENTE REASEGURADORAS EN REALES Y ELIMINA LAS TEMPORALES
                        isConverted = ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(tempSourceCode,
                                                                                     Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                        if (isConverted)
                        {
                            // Solcitudes de pago siniestros 
                            isConverted = ConvertTempClaimPaymentRequestToClaimPaymentRequest(tempSourceCode,
                                                                                         Convert.ToInt32(ImputationTypes.Collect), imputation.Id, userId);
                            if (isConverted)
                            {
                                // GRABA MOVIMIENTOS CUENTA CORRIENTE COASEGUROS EN REALES
                                isConverted = ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(tempSourceCode,
                                                                                           Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                                if (isConverted)
                                {
                                    // GRABA MOVIMIENTOS CONTABILIDAD EN REALES
                                    isConverted = ConvertTempDailyAccountingToDailyDailyAccounting(tempSourceCode,
                                                                                              Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                                    if (isConverted)
                                    {
                                        // Solcitudes de pagos varios 
                                        isConverted = ConvertTempPaymentRequestToPaymentRequest(tempSourceCode, Convert.ToInt32(ImputationTypes.Collect), imputation.Id, userId);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    // Primas por Cobrar
                    DeleteTempPremiumReceivableTransaction(tempImputationId);

                    // Cta. Cte. Agentes
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Coaseguros
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Reaseguros
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Contabilidad
                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pagos varios
                    _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pago de siniestros
                    _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);

                    // Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

                    // Imputación
                    _tempImputationDAO.DeleteTempImputation(tempImputationId);


                    #region APLICACIÓN DE RECIBO

                    // ACTUALIZA ESTADO DE BILL
                    accountingDate = Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(
                                                      Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))));

                    string[] accountingDateSplit = accountingDate.Split();

                    collect.Id = Convert.ToInt32(tempSourceCode);
                    collect.Date = Convert.ToDateTime(accountingDateSplit[0]);
                    collect.UserId = userId;
                    collect.Status = Convert.ToInt32(CollectStatus.Applied);
                    collect.Comments = comments == "" ? null : comments;
                    collect.Transaction = new Transaction();

                    _collectDAO.UpdateCollect(collect, -1);

                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveImputationRequestBill
        /// Ejecuta el proceso de grabación de una imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="comments"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        public bool SaveImputationRequestBillWithTransaction(int sourceCode, int tempImputationId, string comments,
                                              int userId, int tempSourceCode, int technicalTransaction)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error
            try
            {
                bool isConverted = false;
                string accountingDate;
                int imputationId = 0;

                Imputation imputation = new Imputation();

                DateTime registerDate = DateTime.Now;
                Collect collect = new Collect();

                imputation.Id = imputationId;
                imputation.Date = registerDate;
                imputation.ImputationType = ImputationTypes.Collect;
                imputation.UserId = userId;

                // Graba la cabecera de Imputación
                imputation = _imputationDAO.SaveImputation(imputation, sourceCode, technicalTransaction);

                imputationId = imputation.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                // GRABA PRIMAS EN REALES
                isConverted = ConvertTemptoRealPremiumReceivableTransaction(tempSourceCode,
                                                                               Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                if (isConverted)
                {
                    // GRABA MOVIMIENTOS CUENTA CORRIENTE AGENTES EN REALES Y ELIMINA LAS TEMPORALES
                    isConverted = ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(tempSourceCode,
                                                                                   Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                    if (isConverted)
                    {
                        // GRABA MOVIMIENTOS CUENTA CORRIENTE REASEGURADORAS EN REALES Y ELIMINA LAS TEMPORALES
                        isConverted = ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(tempSourceCode,
                                                                                     Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                        if (isConverted)
                        {
                            // Solcitudes de pago siniestros 
                            isConverted = ConvertTempClaimPaymentRequestToClaimPaymentRequest(tempSourceCode,
                                                                                         Convert.ToInt32(ImputationTypes.Collect), imputation.Id, userId);
                            if (isConverted)
                            {
                                // GRABA MOVIMIENTOS CUENTA CORRIENTE COASEGUROS EN REALES
                                isConverted = ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(tempSourceCode,
                                                                                           Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                                if (isConverted)
                                {
                                    // GRABA MOVIMIENTOS CONTABILIDAD EN REALES
                                    isConverted = ConvertTempDailyAccountingToDailyDailyAccounting(tempSourceCode,
                                                                                              Convert.ToInt32(ImputationTypes.Collect), imputation.Id);
                                    if (isConverted)
                                    {
                                        // Solcitudes de pagos varios 
                                        isConverted = ConvertTempPaymentRequestToPaymentRequest(tempSourceCode, Convert.ToInt32(ImputationTypes.Collect), imputation.Id, userId);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    // Primas por Cobrar
                    DeleteTempPremiumReceivableTransaction(tempImputationId);

                    // Cta. Cte. Agentes
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Coaseguros
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Reaseguros
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Contabilidad
                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pagos varios
                    _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pago de siniestros
                    _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);

                    // Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

                    // Imputación
                    _tempImputationDAO.DeleteTempImputation(tempImputationId);


                    #region APLICACIÓN DE RECIBO

                    // ACTUALIZA ESTADO DE BILL
                    accountingDate = Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(
                                                      Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))));

                    string[] accountingDateSplit = accountingDate.Split();

                    collect.Id = Convert.ToInt32(tempSourceCode);
                    collect.Date = Convert.ToDateTime(accountingDateSplit[0]);
                    collect.UserId = userId;
                    collect.Status = Convert.ToInt32(CollectStatus.Applied);
                    collect.Comments = comments == "" ? null : comments;
                    collect.Transaction = new Transaction();

                    _collectDAO.UpdateCollect(collect, -1);

                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// SaveImputationRequestPaymentOrder
        /// Ejecuta el proceso de grabación de una imputación de Orden de Pago
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        public bool SaveImputationRequestPaymentOrder(int sourceCode, int tempImputationId, int userId, int tempSourceCode)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error

            try
            {
                bool isConverted = false;
                int imputationId = 0;
                CollectImputation collectImputation = new CollectImputation();
                Imputation imputation = new Imputation();
                DateTime registerDate = DateTime.Now;

                imputation.Id = imputationId;
                imputation.Date = registerDate;
                imputation.UserId = userId;
                imputation.ImputationType = ImputationTypes.PaymentOrder;

                int technicalTransaction = GetTechnicalTransaction();
                // Graba la cabecera de Imputación
                imputation = _imputationDAO.SaveImputation(imputation, sourceCode, technicalTransaction);

                imputationId = imputation.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                // GRABA PRIMAS EN REALES
                isConverted = ConvertTemptoRealPremiumReceivableTransaction(tempSourceCode,
                                                                     Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id);
                if (isConverted)
                {
                    // GRABA MOVIMIENTOS CUENTA CORRIENTE AGENTES EN REALES Y ELIMINA LAS TEMPORALES
                    isConverted = ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(tempSourceCode,
                                                                       Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id);
                    if (isConverted)
                    {
                        // GRABA MOVIMIENTOS CUENTA CORRIENTE REASEGURADORAS EN REALES Y ELIMINA LAS TEMPORALES
                        isConverted = ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(tempSourceCode,
                                                                        Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id);
                        if (isConverted)
                        {
                            // Solcitudes de pago siniestros 
                            isConverted = ConvertTempClaimPaymentRequestToClaimPaymentRequest(tempSourceCode,
                                                                          Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id, userId);
                            if (isConverted)
                            {
                                // GRABA MOVIMIENTOS CUENTA CORRIENTE COASEGUROS EN REALES
                                isConverted = ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(tempSourceCode,
                                                                             Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id);
                                if (isConverted)
                                {
                                    //GRABA MOVIMIENTOS CONTABILIDAD EN REALES
                                    isConverted = ConvertTempDailyAccountingToDailyDailyAccounting(tempSourceCode,
                                                                                Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id);

                                    if (isConverted)
                                    {
                                        // Solcitudes de pagos varios 
                                        isConverted = ConvertTempPaymentRequestToPaymentRequest(tempSourceCode, Convert.ToInt32(ImputationTypes.PaymentOrder), imputation.Id, userId);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    // Primas por Cobrar
                    DeleteTempPremiumReceivableTransaction(tempImputationId);

                    // Cta. Cte. Agentes
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Coaseguros
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Reaseguros
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Contabilidad
                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pagos varios
                    _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pago de siniestros
                    _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);

                    //Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

                    _tempImputationDAO.DeleteTempImputation(tempImputationId);
                }

                return true;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveImputationRequestJournalEntry
        /// Ejecuta el proceso de grabación de una imputación de Asiento de Diario
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>CollectImputation</returns>
        public CollectImputationDTO SaveImputationRequestJournalEntry(int tempImputationId, int userId, int tempSourceCode)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error

            try
            {
                ImputationModels.JournalEntry journalEntryHeader = GetTempJournalEntryById(tempSourceCode).ToModel();
                journalEntryHeader.Id = 0;
                journalEntryHeader.Status = 1;//Estado Aplicado
                journalEntryHeader = _journalEntryDAO.SaveJournalEntry(journalEntryHeader);

                int sourceCode = journalEntryHeader.Id;

                bool isConverted = false;
                int imputationId = 0;
                DateTime registerDate = DateTime.Now;

                Imputation imputation = new Imputation()
                {
                    Date = registerDate,
                    Id = imputationId,
                    ImputationType = ImputationTypes.JournalEntry,
                    UserId = userId
                };

                // Obtiene el número de transacción
                int technicalTransaction = GetTechnicalTransaction();
                // Graba la cabecera de imputación
                imputation = _imputationDAO.SaveImputation(imputation, sourceCode, technicalTransaction);

                imputationId = imputation.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS


                // GRABA PRIMAS EN REALES
                isConverted = ConvertTemptoRealPremiumReceivableTransaction(tempSourceCode,
                                                           Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id);
                if (isConverted)
                {
                    // GRABA MOVIMIENTOS CUENTA CORRIENTE AGENTES EN REALES Y ELIMINA LAS TEMPORALES
                    isConverted = ConvertTempBrokerCheckingAccountToBrokerCheckingAccount(tempSourceCode,
                                                           Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id);
                    if (isConverted)
                    {
                        // GRABA MOVIMIENTOS CUENTA CORRIENTE REASEGURADORAS EN REALES Y ELIMINA LAS TEMPORALES
                        isConverted = ConvertTempReinsuranceCheckingAccountToReinsuranceCheckingAccount(tempSourceCode,
                                                              Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id);
                        if (isConverted)
                        {
                            // Solcitudes de pago siniestros 
                            isConverted = ConvertTempClaimPaymentRequestToClaimPaymentRequest(tempSourceCode,
                                                                 Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id, userId);
                            if (isConverted)
                            {
                                // GRABA MOVIMIENTOS CUENTA CORRIENTE COASEGUROS EN REALES
                                isConverted = ConvertTempCoinsuranceCheckingAccountToCoinsuranceCheckingAccount(tempSourceCode,
                                                                      Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id);
                                if (isConverted)
                                {
                                    // GRABA MOVIMIENTOS CONTABILIDAD EN REALES
                                    isConverted = ConvertTempDailyAccountingToDailyDailyAccounting(tempSourceCode,
                                                                       Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id);

                                    if (isConverted)
                                    {
                                        // Solcitudes de pagos varios 
                                        isConverted = ConvertTempPaymentRequestToPaymentRequest(tempSourceCode, Convert.ToInt32(ImputationTypes.JournalEntry), imputation.Id, userId);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    // Primas por Cobrar
                    DeleteTempPremiumReceivableTransaction(tempImputationId);

                    // Cta. Cte. Agentes
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Coaseguros
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Reaseguros
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Contabilidad
                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pagos varios
                    _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pago de siniestros
                    _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);

                    // Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

                    _tempImputationDAO.DeleteTempImputation(tempImputationId);
                }

                _tempJournalEntryDAO.DeleteTempJournalEntry(tempSourceCode);

                Transaction accountingTransaction = new Transaction()
                {
                    TechnicalTransaction = technicalTransaction
                };

                accountingTransaction.Id = journalEntryHeader.Id;
                return new CollectImputation() { Imputation = imputation, Transaction = accountingTransaction }.ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetSourceCodeByImputationId
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>int</returns>
        public int GetSourceCodeByImputationId(int imputationId)
        {
            int sourceCode = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.ImputationCode, imputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                    SelectObjects(typeof(ACCOUNTINGEN.Imputation), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.Imputation imputationItem in businessCollection.OfType<ACCOUNTINGEN.Imputation>())
                    {
                        sourceCode = Convert.ToInt32(imputationItem.SourceCode);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return sourceCode;
        }

        #endregion ImputationDTO

        #region TempImputation

        /// <summary>
        /// SaveTempImputation
        /// Graba temporal de imputación
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO SaveTempImputation(ImputationDTO imputation, int sourceCode)
        {
            try
            {
                return _tempImputationDAO.SaveTempImputation(imputation.ToModel(), sourceCode).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetTempImputation
        /// Obtiene temporal de imputación
        /// </summary>
        /// <param name="tempImputation"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO GetTempImputation(ImputationDTO tempImputation)
        {
            try
            {
                return _tempImputationDAO.GetTempImputation(tempImputation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        /// <summary>
        /// DeleteTempImputation
        /// Elimina un temporal de imputación
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempImputation(int tempImputationId)
        {
            try
            {
                return _tempImputationDAO.DeleteTempImputation(tempImputationId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }


        }

        /// <summary>
        /// UpdateTempImputation
        /// Edita un temporal de imputación
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO UpdateTempImputation(ImputationDTO imputation, int sourceCode)
        {
            try
            {
                return _tempImputationDAO.UpdateTempImputation(imputation.ToModel(), sourceCode).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        /// <summary>
        /// GetTempImputationBySourceCode
        /// Obtiene un temporal de imputación por su tipo de origen
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO GetTempImputationBySourceCode(int imputationTypeId, int sourceCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.ImputationTypeCode, imputationTypeId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.SourceCode, sourceCode);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempImputation), criteriaBuilder.GetPredicate()));

                Imputation imputations = new Imputation();

                foreach (ACCOUNTINGEN.TempImputation imputationEntity in businessCollection.OfType<ACCOUNTINGEN.TempImputation>())
                {
                    imputations.Id = imputationEntity.TempImputationCode;
                    imputations.Date = Convert.ToDateTime(imputationEntity.RegisterDate);
                    imputations.UserId = Convert.ToInt32(imputationEntity.UserId);
                }

                return imputations.ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempImputationItem
        /// Obtiene un item de imputación temporal
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>TransactionType</returns>
        public TransactionTypeDTO GetTempImputationItem(int imputationTypeId, int tempImputationId)
        {
            try
            {

                TransactionType transactionType = new TransactionType();

                switch (imputationTypeId)
                {
                    case (int)ImputationItemTypes.PremiumsReceivable:
                    case (int)ImputationItemTypes.DepositPremiums:
                    case (int)ImputationItemTypes.CommissionRetained:
                        {
                            PremiumReceivableTransaction tempPremiumReceivableTransaction = _tempPremiumRecievableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(tempImputationId, imputationTypeId);
                            transactionType = tempPremiumReceivableTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.CheckingAccountBrokers:
                        {
                            BrokersCheckingAccountTransaction tempBrokersCheckingAccountTransaction = _tempBrokerCheckingAccountTransactionDAO.GetTempBrokerCheckingAccountTransactionByTempImputationId(tempImputationId);
                            transactionType = tempBrokersCheckingAccountTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.CheckingAccountCoinsurances:
                        {
                            CoInsuranceCheckingAccountTransaction tempCoinsuranceCheckingAccountTransaction = _tempCoinsuranceCheckingAccountTransactionDAO.GetTempCoinsuranceCheckingAccountTransactionByTempImputationId(tempImputationId);
                            transactionType = tempCoinsuranceCheckingAccountTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.CheckingAccountReinsurances:
                        {
                            ReInsuranceCheckingAccountTransaction tempReinsuranceCheckingAccountTransaction = _tempReinsuranceCheckingAccountTransactionDAO.GetTempReinsuranceCheckingAccountTransactionByTempImputationId(tempImputationId);
                            transactionType = tempReinsuranceCheckingAccountTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.Accounting:
                        {
                            DailyAccountingTransaction tempDailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempImputationId);
                            transactionType = tempDailyAccountingTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.PaymentSuppliers:
                        {
                            PaymentRequestTransaction paymentRequestTransaction = _tempPaymentRequestTransactionDAO.GetTempPaymentRequestTransactionByTempImputationId(tempImputationId);
                            transactionType = paymentRequestTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.PaymentClaims:
                        {
                            ClaimsPaymentRequestTransaction claimsPaymentRequestTransaction = _tempClaimsPaymentRequestTransactionDAO.GetTempClaimsPaymentRequestTransactionByTempImputationId(tempImputationId, 2);
                            transactionType = claimsPaymentRequestTransaction;
                        }
                        break;
                }

                return transactionType.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDebitsAndCreditsMovementTypes
        /// Obtiene el total de débitos y créditos de los tipos de movimientos
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="amountValue"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO GetDebitsAndCreditsMovementTypes(ImputationDTO imputation, decimal amountValue)
        {
            try
            {

                Amount amount = new Amount();
                Imputation imputationModel = new Imputation();
                imputationModel.ImputationItems = new List<TransactionType>();
                decimal debit = 0;
                decimal credit = 0;

                for (int i = 1; i <= 9; i++)
                {
                    TransactionType transactionType = GetTempImputationItem(i, imputation.Id).ToModel();

                    imputationModel.ImputationItems.Add(transactionType);

                    if (imputationModel.ImputationItems[i - 1] != null)
                    {
                        if ((imputationModel.ImputationItems[i - 1].TotalDebit.Value != 0))
                        {
                            debit += Math.Abs(Convert.ToDecimal(imputationModel.ImputationItems[i - 1].TotalDebit.Value));
                        }

                        if ((imputationModel.ImputationItems[i - 1].TotalCredit.Value != 0))
                        {
                            credit += Math.Abs(Convert.ToDecimal(imputationModel.ImputationItems[i - 1].TotalCredit.Value));
                        }
                    }
                }

                amount.Value = amountValue + debit - credit;

                imputationModel.Id = imputation.Id;
                imputationModel.UserId = imputation.UserId;
                imputationModel.Date = imputation.Date;
                imputationModel.VerificationValue = amount;

                return imputationModel.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempImputationSourceCode
        /// Edita un temporal de imputación por el tipo de origen
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="sourceId"></param>
        /// <returns>int</returns>
        public int UpdateTempImputationSourceCode(int tempImputationId, int sourceId)
        {
            try
            {

                Imputation imputation = new Imputation() { Id = tempImputationId };

                imputation = _tempImputationDAO.GetTempImputation(imputation);
                imputation.IsTemporal = true;
                imputation = _tempImputationDAO.UpdateTempImputation(imputation, sourceId);

                return imputation.Id;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region SearchTemporary

        /// <summary>
        /// GetImputationTypes
        /// Obtiene los tipos de imputación
        /// </summary>
        /// <returns>List<ImputationType/></returns>
        public List<ImputationTypeDTO> GetImputationTypes()
        {
            List<ImputationType> imputationTypes = new List<ImputationType>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.ImputationType.Properties.ImputationTypeCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.ImputationType), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.ImputationType imputationType in businessCollection.OfType<ACCOUNTINGEN.ImputationType>())
                {
                    imputationTypes.Add(new ImputationType()
                    {
                        Id = Convert.ToInt32(imputationType.ImputationTypeCode),
                        Description = imputationType.Description
                    });
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return imputationTypes.ToDTOs().ToList();
        }

        /// <summary>
        /// DeleteTemporaryApplicationRequest
        /// Borra los temporales de imputación
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>bool</returns>
        public bool DeleteTemporaryApplicationRequest(int tempImputationId, int imputationTypeId, int sourceCode)
        {
            bool isDeleted = false;

            try
            {
                // Primas por Cobrar
                DeleteTempPremiumReceivableTransaction(tempImputationId);

                // Cta. Cte. Agentes
                _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                // Cta. Cte. Coaseguros
                _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                // Cta. Cte. Reaseguros
                _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                // Borra temporales de transacciones solicitudes de pago de siniestros
                _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                // Borra temporales solicitudes de pago de siniestros
                _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);

                // Contabilidad
                _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                // Imputación
                _tempImputationDAO.DeleteTempImputation(tempImputationId);

                // Asiento de diario
                if (imputationTypeId == Convert.ToInt32(ImputationTypes.JournalEntry))
                {
                    _tempJournalEntryDAO.DeleteTempJournalEntry(sourceCode);
                }

                // Preliquidación
                if (imputationTypeId == Convert.ToInt32(ImputationTypes.PreLiquidation))
                {
                    PreLiquidation temPreliquidation = new PreLiquidation();
                    temPreliquidation.Id = sourceCode;
                    _tempPreLiquidationDAO.DeleteTempPreLiquidation(temPreliquidation);
                }

                // Órdenes de pago
                if (imputationTypeId == Convert.ToInt32(ImputationTypes.PaymentOrder))
                {
                    _tempPaymentOrderDAO.DeleteTempPaymentOrder(sourceCode);
                }

                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmount
        /// Permite actualizar la tasa de cambio a la fecha actual a una imputación temporal
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        public bool RecalculatingForeignCurrencyAmount(int tempImputationId, int imputationTypeId, int sourceId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = false;

            try
            {
                // Primas por Cobrar
                RecalculatingForeignCurrencyAmountTempPremiumRecievable(tempImputationId, foreignCurrencyExchangeRates);

                // Cta. Cte. Agentes
                RecalculatingForeignCurrencyAmountTempBrokerCheckingAccount(tempImputationId, foreignCurrencyExchangeRates);

                // Cta. Cte. Coaseguros
                RecalculatingForeignCurrencyAmountTempCoinsuranceCheckingAccount(tempImputationId, foreignCurrencyExchangeRates);

                // Cta. Cte. Reaseguros
                RecalculatingForeignCurrencyAmountTempReinsuranceCheckingAccount(tempImputationId, foreignCurrencyExchangeRates);

                // Solicitudes de pago de siniestros
                RecalculatingForeignCurrencyAmountTempClaimPayment(tempImputationId, foreignCurrencyExchangeRates);

                // Contabilidad
                RecalculatingForeignCurrencyAmountTempDailyAccounting(tempImputationId, foreignCurrencyExchangeRates);

                // Órdenes de pago
                if (imputationTypeId == Convert.ToInt32(ImputationTypes.PaymentOrder))
                {
                    RecalculatingForeignCurrencyAmountTempPaymentOrder(sourceId, foreignCurrencyExchangeRates);
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        #endregion

        #region ImputationTypes

        // Primas por Cobrar
        #region PremiumsReceivable

        ///<summary>
        /// SaveTempPremiumRecievableTransaction
        /// Graba una prima por cobrar
        /// </summary>
        /// <param name="premiumRecievableTransaction"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>int</returns>
        public int SaveTempPremiumRecievableTransaction(PremiumReceivableTransactionDTO premiumRecievableTransaction, int imputationId, decimal exchangeRate, int userId, DateTime registerDate)
        {
            int recorded = 0;

            try
            {
                foreach (PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem in premiumRecievableTransaction.PremiumReceivableItems)
                {
                    if (premiumReceivableTransactionItem.Id == 0)
                    {
                        _tempPremiumReceivableTransactionItemDAO.SaveTempPremiumRecievableTransactionItem(premiumReceivableTransactionItem.ToModel(), imputationId, exchangeRate, userId, registerDate).ToDTO();
                    }
                    else
                    {
                        _tempPremiumReceivableTransactionItemDAO.UpdateTempPremiumReceivableTransactionItem(premiumReceivableTransactionItem.ToModel(), imputationId, exchangeRate, userId, registerDate).ToDTO();
                    }

                    recorded = 1;
                }

                return recorded;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// PremiumReceivableSearchPolicy
        /// Búsqueda de primas por cobrar de una póliza
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="payerId"></param>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="policyId"></param>
        /// <param name="salesTicket"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        public List<SEARCH.PremiumReceivableSearchPolicyDTO> PremiumReceivableSearchPolicy(string insuredId, string payerId, string agentId,
                                                      string groupId, string policyId, string policyDocumentNumber, string salesTicket,
                                                      string branchId, string prefixId, string endorsementId,
                                                       string dateFrom, string dateTo, string insuredDocumentNumber, int pageSize, int pageIndex)
        {
            try
            {
                pageIndex--;

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (insuredId != "" || payerId != "" || agentId != "" || groupId != "" || policyId != "" ||
                    salesTicket != "" || branchId != "" || prefixId != "" || endorsementId != "" ||
                    dateFrom != "" || dateTo != "")
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyId);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(0);
                    criteriaBuilder.And();
                    //Filtra solo las cuotas que tengan saldo
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PaymentAmount);
                    criteriaBuilder.Distinct();
                    criteriaBuilder.Constant(0);
                }
                else
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(-1);
                }

                if (insuredDocumentNumber != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.InsuredDocumentNumber, (insuredDocumentNumber));
                }

                if (insuredId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.InsuredIndividualId, Convert.ToInt64(insuredId));
                }

                if (payerId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PayerIndividualId, Convert.ToInt32(payerId));
                }

                if (agentId != "")
                {
                    // Implementar en la vista el campo para Agente.
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyAgentIndividualId, Convert.ToInt32(agentId));
                }

                if (groupId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.BillingGroupCode, Convert.ToInt32(groupId));
                }

                if (policyId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyId, Convert.ToDecimal(policyId));
                }
                if (policyDocumentNumber != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyDocumentNumber, Convert.ToDecimal(policyDocumentNumber));
                }

                if (salesTicket != "")
                {
                    //Pendiente implementar en la vista el campo para Factura.
                }

                if (branchId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.BranchCode, Convert.ToInt32(branchId));
                }

                if (prefixId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PrefixCode, Convert.ToInt32(prefixId));
                }

                if (endorsementId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.EndorsementId, Convert.ToInt32(endorsementId));
                }

                if (dateFrom != "" && dateTo != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PaymentExpirationDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(dateFrom));
                    criteriaBuilder.CloseParenthesis();
                    criteriaBuilder.And();
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PaymentExpirationDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(dateTo));
                    criteriaBuilder.CloseParenthesis();
                }

                UIView premiums = _dataFacadeManager.GetDataFacade().GetView("PolicyPremiumReceivableTransView", criteriaBuilder.GetPredicate(), null, pageIndex, pageSize, null, true, out int rows);

                List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivablePolicies = SetPremiumReceivableSearchPolicy(premiums);

                return premiumReceivablePolicies;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// Trae un item de prima por cobrar
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        public List<SEARCH.PremiumReceivableItemDTO> GetTempPremiumReceivableItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempImputationCode, tempImputationId);

                UIView tempPremiumReceivableItems = _dataFacadeManager.GetDataFacade().GetView("TempPremiumReceivableItemView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (tempPremiumReceivableItems.Rows.Count > 0)
                {
                    tempPremiumReceivableItems.Columns.Add("rows", typeof(int));
                    tempPremiumReceivableItems.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.PremiumReceivableItemDTO> premiumReceivableItems = new List<SEARCH.PremiumReceivableItemDTO>();

                foreach (DataRow dataRow in tempPremiumReceivableItems)
                {
                    decimal commissionPercentage = 0;
                    decimal agentParticipationPercentage = 0;

                    SEARCH.PendingCommissionDTO pendingCommission = new SEARCH.PendingCommissionDTO();
                    SEARCH.PendingCommissionDTO commission = GetPendingCommission(Convert.ToInt32(dataRow["PolicyId"]), Convert.ToInt32(dataRow["EndorsementId"]));

                    pendingCommission.PendingCommission = commission.PendingCommission;
                    pendingCommission.CommissionPercentage = commission.CommissionPercentage;
                    pendingCommission.AgentParticipationPercentage = commission.CommissionPercentage;

                    premiumReceivableItems.Add(new SEARCH.PremiumReceivableItemDTO()
                    {
                        // Campos propios de Item
                        PremiumReceivableItemId = Convert.ToInt32(dataRow["TempPremiumReceivableTransCode"]),
                        ImputationId = Convert.ToInt32(dataRow["TempImputationCode"]),
                        PayableAmount = (Convert.ToDecimal(dataRow["IncomeAmount"]) > Convert.ToDecimal(dataRow["PaymentAmount"])) ? Convert.ToDecimal(dataRow["PaymentAmount"]) : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        // Campos para la grilla
                        BranchPrefixPolicyEndorsement = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PolicyDocumentNumber"]) + '-' + Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        InsuredDocumentNumberName = Convert.ToString(dataRow["InsuredDocumentNumber"]) + '-' + Convert.ToString(dataRow["InsuredName"]),
                        PayerDocumentNumberName = Convert.ToString(dataRow["PayerDocumentNumber"]) + '-' + Convert.ToString(dataRow["PayerName"]),
                        PolicyAgentDocumentNumberName = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]) + '-' + Convert.ToString(dataRow["PolicyAgentName"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BussinessTypeDescription = Convert.ToString(dataRow["BussinessTypeDescription"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                        EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                        CollectGroupId = dataRow["BillingGroupCode"] != DBNull.Value ? Convert.ToInt32(dataRow["BillingGroupCode"]) : 0,
                        CollectGroupDescription = dataRow["BillingGroupDescription"] != DBNull.Value ? Convert.ToString(dataRow["BillingGroupDescription"]) : "",
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        PayerId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                        PayerDocumentNumber = Convert.ToString(dataRow["PayerDocumentNumber"]),
                        PayerName = Convert.ToString(dataRow["PayerName"]),
                        PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                        PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                        CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = Convert.ToString(dataRow["CurrencyDescription"]),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        PolicyAgentId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]),
                        PolicyAgentDocumentNumber = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]),
                        PolicyAgentName = Convert.ToString(dataRow["PolicyAgentName"]),
                        InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        InsuredDocumentNumber = Convert.ToString(dataRow["InsuredDocumentNumber"]),
                        InsuredName = Convert.ToString(dataRow["InsuredName"]),
                        ExcessPayment = (Convert.ToDecimal(dataRow["IncomeAmount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]) > 0) ? Convert.ToDecimal(dataRow["IncomeAmount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]) : 0,
                        DiscountedCommission = dataRow["DiscountedCommission"] != DBNull.Value ? Math.Abs(Convert.ToDecimal(dataRow["DiscountedCommission"])) : 0,
                        PendingCommission = (Convert.ToDecimal(dataRow["PaymentAmount"]) * (agentParticipationPercentage / 100) * (commissionPercentage / 100)) - (dataRow["DiscountedCommission"] != DBNull.Value ? Math.Abs(Convert.ToDecimal(dataRow["DiscountedCommission"])) : 0),
                        Address = dataRow["Street"].ToString() + " " + dataRow["HouseNumber"].ToString(),
                        Upd = HasDepositPrimes(Convert.ToInt32(dataRow["TempPremiumReceivableTransCode"])) ? 1 : 0,
                        PaidAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Rows = rows
                    });
                }

                return premiumReceivableItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (UnhandledException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// ConvertTemptoRealPremiumReceivableTransaction
        /// Proceso de conversión de primas x cobrar
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        private bool ConvertTemptoRealPremiumReceivableTransaction(int sourceId, int imputationTypeId, int imputationId)
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

                DateTime accountingDate = Convert.ToDateTime(Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]);

                DateTime dateFrom = Convert.ToDateTime("01" + "/" + accountingDate.Month + "/" + accountingDate.Year);
                DateTime dateTo = accountingDate;

                int payerTypeId = 1; // Quemado hasta definir

                try
                {
                    // Obtengo el temporal.
                    Imputation tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId).ToModel();

                    tempImputation.ImputationItems = new List<TransactionType>();
                    PremiumReceivableTransaction tempPremiumRecievableTransaction = _tempPremiumRecievableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(tempImputation.Id, Convert.ToInt32(ImputationItemTypes.PremiumsReceivable));

                    tempImputation.ImputationItems.Add(tempPremiumRecievableTransaction);

                    ImputationDTO imputation = new ImputationDTO();

                    imputation.UserId = tempImputation.UserId;
                    imputation.Date = DateTime.Now;
                    imputation.Id = imputationId;

                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempImputationCode, tempImputation.Id);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivable in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                        {
                            // Determina valores en exceso y valores a pagar.
                            decimal excessPayment = 0;
                            decimal localAmount = 0;
                            decimal collectionAmount = 0;

                            // Controlar si los 2 valores son negativos
                            if (Convert.ToDecimal(tempPremiumReceivable.Amount) > 0 && Convert.ToDecimal(tempPremiumReceivable.PaymentAmount) > 0)
                            {
                                if (Convert.ToDecimal(tempPremiumReceivable.Amount) > Convert.ToDecimal(tempPremiumReceivable.PaymentAmount))
                                {
                                    excessPayment = Convert.ToDecimal(tempPremiumReceivable.Amount) - Convert.ToDecimal(tempPremiumReceivable.PaymentAmount);
                                    localAmount = excessPayment * Convert.ToDecimal(tempPremiumReceivable.ExchangeRate);
                                }
                                collectionAmount = excessPayment > 0 ? Convert.ToDecimal(tempPremiumReceivable.PaymentAmount) : Convert.ToDecimal(tempPremiumReceivable.Amount);
                            }
                            else
                            {
                                collectionAmount = Convert.ToDecimal(tempPremiumReceivable.Amount);
                            }

                            PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();
                            premiumReceivableTransactionItem.Policy = new Policy();
                            premiumReceivableTransactionItem.Policy.Id = Convert.ToInt32(tempPremiumReceivable.PolicyId);
                            premiumReceivableTransactionItem.Policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(tempPremiumReceivable.EndorsementId) };
                            premiumReceivableTransactionItem.Policy.ExchangeRate = new ExchangeRate()
                            {
                                Currency = new Currency() { Id = Convert.ToInt32(tempPremiumReceivable.CurrencyCode) }
                            };
                            premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                            {
                                new Beneficiary() { IndividualId = Convert.ToInt32(tempPremiumReceivable.PayerId) }
                            };
                            premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponent>()
                            {
                                new PayerComponent()
                                {
                                    Amount = Convert.ToDecimal(tempPremiumReceivable.PaymentAmount),
                                    BaseAmount = Convert.ToDecimal(collectionAmount)
                                }
                            };
                            premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                            {
                                Quotas = new List<Quota>()
                                {
                                    new Quota() { Number = Convert.ToInt32(tempPremiumReceivable.PaymentNum) }
                                }
                            };

                            premiumReceivableTransactionItem.DeductCommission = new Amount() { Value = Convert.ToDecimal(tempPremiumReceivable.DiscountedCommission) };

                            premiumReceivableTransactionItem = _premiumReceivableTransactionItemDAO.SavePremiumRecievableTransactionItem(premiumReceivableTransactionItem, imputationId,
                            //Convert.ToDecimal(tempPremiumReceivable.ExchangeRate), DateTime.Now);
                            Convert.ToDecimal(tempPremiumReceivable.ExchangeRate), accountingDate);


                            if (premiumReceivableTransactionItem.Id != 0)
                            {
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
                                    depositPremiumTransaction.Collect.Payer.IndividualId = Convert.ToInt32(tempPremiumReceivable.PayerId);
                                    depositPremiumTransaction.Date = Convert.ToDateTime(tempPremiumReceivable.RegisterDate);
                                    depositPremiumTransaction.Amount = new Amount()
                                    {
                                        Currency = new Currency() { Id = Convert.ToInt32(tempPremiumReceivable.CurrencyCode) },
                                        Value = Math.Round(excessPayment, 2)
                                    };
                                    depositPremiumTransaction.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempPremiumReceivable.ExchangeRate) };
                                    depositPremiumTransaction.LocalAmount = new Amount();
                                    depositPremiumTransaction.LocalAmount.Value = Math.Round(localAmount, 2);

                                    depositPremiumsTransactionSaved = SaveDepositPremiumTransaction(depositPremiumTransaction.ToDTO(), premiumReceivableTransactionItem.Id, payerTypeId);
                                    // Si no graba el pago en exceso, borra el item grabado y la cobranza
                                    if (depositPremiumsTransactionSaved < 0)
                                    {
                                        _premiumReceivableTransactionItemDAO.DeletePremiumRecievableTransactionItem(premiumReceivableTransactionItem.Id);
                                        isConverted = false;
                                    }
                                }

                                #endregion

                                #region UsedAmounts

                                // GRABA AMOUNTS USADOS
                                List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = _tempUsedDepositPremiumDAO.GetTempUsedDepositPremiumByTempPremiumReceivableId(tempPremiumReceivable.TempPremiumReceivableTransCode);

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
                                        ExchangeRate exchangeRateModel = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempPremiumReceivable.ExchangeRate) };
                                        Amount incomeAmount = new Amount() { Value = Convert.ToDecimal(tempUsedDepositPremiumDto.Amount * tempPremiumReceivable.ExchangeRate) };

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

                                isComponentCollectionSaved = SaveComponentCollectionRequest(premiumReceivableTransactionItem.Id, Convert.ToDecimal(tempPremiumReceivable.ExchangeRate), Convert.ToInt32(tempPremiumReceivable.CurrencyCode));
                                if (isComponentCollectionSaved)
                                {
                                    isConverted = true;
                                }

                                #endregion

                                #region DiscountedCommision

                                if (Math.Abs(Convert.ToDecimal(tempPremiumReceivable.DiscountedCommission)) > 0)
                                {
                                    isDiscountedCommissionSaved = SaveDiscountedCommissionRequest(premiumReceivableTransactionItem.Id, Math.Abs(Convert.ToDecimal(tempPremiumReceivable.DiscountedCommission)));
                                    if (isDiscountedCommissionSaved)
                                    {
                                        isConverted = true;
                                    }
                                }

                                #endregion

                                //VALIDA SI ESTÁ ACTIVADA LA LIBERACIÓN EN LÍNEA
                                if (UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_RELEASE_COMMISSIONS_INLINE).ToString() == "1")
                                {
                                    //VALIDA EL TIPO DE LIBERACIÓN DE COMISIÓN
                                    if (UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_RELEASE_COMMISSIONS_PRORATE).ToString() == "1") //Liberacion de comisiones a prorrata
                                    {
                                        //Liberación comisión Prorrata
                                        /*La liberación de las coutas es por cada cobranza*/
                                        BrokersCommission(premiumReceivableTransactionItem.ToDTO(), imputation, sourceId);
                                    }
                                    else
                                    {
                                        //Código Original
                                        #region Liberación comisión Normal

                                        if (tempPremiumReceivable.PaymentNum == 1) // La liberación de comisión solo se lo hace el momento de pagar la primera cuota.
                                        {
                                            bool isValidated = false;

                                            isValidated = ValidateAgentCommissionRelease(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id);

                                            if (isValidated)
                                            {
                                                // Obtengo los ramos y subramos
                                                criteriaBuilder = new ObjectCriteriaBuilder();
                                                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixPremiumReceivable.Properties.PremiumReceivableTransId, premiumReceivableTransactionItem.Id);

                                                decimal exchangeRate = 0;
                                                exchangeRate = premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount;

                                                decimal amount = 0;
                                                amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;

                                                int agentTypeId = 0;

                                                UIView premiumReceivablePrefixes = _dataFacadeManager.GetDataFacade().GetView("PrefixPremiumReceivableView",
                                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                                                if (premiumReceivablePrefixes.Count > 0)
                                                {
                                                    foreach (DataRow dataRow in premiumReceivablePrefixes)
                                                    {
                                                        // Obtengo las comisiones de agente
                                                        List<SEARCH.BrokerCheckingAccountItemDTO> commissions = GetAgentCommissions(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id,
                                                            premiumReceivableTransactionItem.DeductCommission.Value, premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount, Convert.ToInt32(dataRow["LineBusinessCode"]),
                                                            Convert.ToInt32(dataRow["SubLineBusinessCode"]), imputation.UserId);

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
                                                                Convert.ToDateTime(Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]));

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
                                                                            sourceId = GetSourceIdByImputation(imputation);

                                                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);
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
                                                                        sourceId = GetSourceIdByImputation(imputation);

                                                                        brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);
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
                                                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputation.Id, 0);

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
                                                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputation.Id, 0);

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
        /// DeletePremiumReceivableTransaction
        /// Borrado de primas x cobrar
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPremiumReceivableTransaction(int tempImputationId)
        {
            bool isDeleted = false;

            try
            {
                // Obtengo los temp premium receivable a partir del tempImputationId
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                                             (typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                    {
                        // Obtengo y elimino los pagos en exceso.
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempUsedDepositPremium.Properties.TempPremiumReceivableTransCode, tempPremiumReceivableEntity.TempPremiumReceivableTransCode);
                        BusinessCollection businessCollectionPremiums = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                            (typeof(ACCOUNTINGEN.TempUsedDepositPremium), criteriaBuilder.GetPredicate()));

                        if (businessCollectionPremiums.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.TempUsedDepositPremium tempUsedDepositPremiumEntity in businessCollectionPremiums.OfType<ACCOUNTINGEN.TempUsedDepositPremium>())
                            {
                                _tempUsedDepositPremiumDAO.DeleteTempUsedDepositPremium(tempUsedDepositPremiumEntity.TempUsedDepositPremiumCode);
                            }
                        }

                        // Elimino los temp premium receivable
                        _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivableEntity.TempPremiumReceivableTransCode);
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
        /// DeleteTempPremiumRecievableTransactionItem
        /// Borrado de un item de primas x cobrar
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="tempPremiumReceivableCode"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPremiumRecievableTransactionItem(int tempImputationId, int tempPremiumReceivableCode)
        {
            try
            {

                bool isDeleted = false;

                // Borro primas en deposito que se hayan usado
                if (tempPremiumReceivableCode != -1)
                {
                    DeleteTempUsedDepositPremiumRequest(tempPremiumReceivableCode);
                }

                // Borro el item de primas por cobrar.
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempImputationCode, tempImputationId);

                if (tempPremiumReceivableCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempPremiumReceivableTransCode, tempPremiumReceivableCode);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivable in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                {
                    _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivable.TempPremiumReceivableTransCode);
                    isDeleted = true;
                }

                return isDeleted;

            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        public List<SEARCH.DiscountedCommissionDTO> SearhDiscountedCommission(string endorsementId, string policyId)
        {
            List<UNDMOD.IssuanceAgency> agencyIss =
                DelegateService.underwritingService.GetAgentsByPolicyIdEndorsementId(Convert.ToInt32(policyId), Convert.ToInt32(endorsementId));


            List<SEARCH.DiscountedCommissionDTO> discountedCommissionDTO = ModelAssembler.CreateAgentCommissions(agencyIss, DelegateService.personService.GetAgentByIndividualId(agencyIss.FirstOrDefault().Agent.IndividualId).CommissionDiscountAgreement);//canUsedCommission);
            return discountedCommissionDTO;
        }

        #endregion

        //Primas en Depósito
        #region DepositPremium

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
        /// GetDepositPremiumTransactionByPayerId
        /// Obtiene un item de primas por cobrar el pagador
        /// </summary>
        /// <param name="payerId"></param>
        /// <returns>List<DepositPremiumTransactionDTO/></returns>
        public List<SEARCH.DepositPremiumTransactionDTO> GetDepositPremiumTransactionByPayerId(int payerId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DepositPremiumTransactionCurrency.Properties.PayerId, payerId);

                UIView depositPremiums = _dataFacadeManager.GetDataFacade().GetView("DepositPremiumTransactionCurrencyView",
                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #region LoadDTO

                //Add New row for return total records
                if (depositPremiums.Rows.Count > 0)
                {
                    depositPremiums.Columns.Add("rows", typeof(int));
                    depositPremiums.Rows[0]["rows"] = rows;
                }

                List<SEARCH.DepositPremiumTransactionDTO> depositPremiumTransactions = new List<SEARCH.DepositPremiumTransactionDTO>();

                foreach (DataRow dataRow in depositPremiums)
                {
                    depositPremiumTransactions.Add(new SEARCH.DepositPremiumTransactionDTO()
                    {
                        DepositPremiumTransactionId = dataRow["DepositPremiumTransactionCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DepositPremiumTransactionCode"]),
                        CollectId = dataRow["CollectCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectCode"]),
                        PayerId = dataRow["PayerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PayerId"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dataRow["RegisterDate"]),
                        CurrencyId = dataRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["Value"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Value"]),
                        TempAmount = dataRow["TempUsedAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TempUsedAmount"]),
                        Used = dataRow["UsedAmounts"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["UsedAmounts"]),
                        TotalAmount = (dataRow["Value"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Value"])) - (dataRow["TempUsedAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TempUsedAmount"])) - (dataRow["UsedAmounts"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["UsedAmounts"])),
                        UsedAmount = 0,
                        Rows = rows
                    });
                }

                #endregion

                return depositPremiumTransactions;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempUsedDepositPremiumRequest
        /// Graba temporal primas en depósito usadas
        /// </summary>
        /// <param name="tempUsedDepositPremiums"></param>
        /// <returns>int</returns>
        public int SaveTempUsedDepositPremiumRequest(List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums)
        {
            int saved = 0;

            try
            {
                if (tempUsedDepositPremiums != null)
                {
                    foreach (SEARCH.TempUsedDepositPremiumDTO tempUsedDepositPremiumDto in tempUsedDepositPremiums)
                    {
                        _tempUsedDepositPremiumDAO.SaveTempUsedDepositPremium(tempUsedDepositPremiumDto);
                    }
                    saved = 1;
                }
                else
                {
                    saved = 0;
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return saved;
        }

        /// <summary>
        /// HasDepositPrimes
        /// Indica si se usaron primas en depósito al aplicar una prima por cobrar.
        /// </summary>
        /// <param name="tempPremiumReceivableId"></param>
        /// <returns>bool</returns>
        public bool HasDepositPrimes(int tempPremiumReceivableId)
        {
            bool isDeposit = false;

            List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = _tempUsedDepositPremiumDAO.GetTempUsedDepositPremiumByTempPremiumReceivableId(tempPremiumReceivableId);

            if (tempUsedDepositPremiums.Count > 0)
            {
                isDeposit = true;
            }

            return isDeposit;
        }

        /// <summary>
        /// DeleteTempUsedDepositPremiumRequest
        /// Borra primas en depósito que están en temporales.
        /// </summary>
        /// <param name="tempPremiumReceivableId"></param>
        public void DeleteTempUsedDepositPremiumRequest(int tempPremiumReceivableId)
        {
            List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = _tempUsedDepositPremiumDAO.GetTempUsedDepositPremiumByTempPremiumReceivableId(tempPremiumReceivableId);

            if (tempUsedDepositPremiums.Count > 0)
            {
                foreach (SEARCH.TempUsedDepositPremiumDTO tempUsedDepositPremium in tempUsedDepositPremiums)
                {
                    _tempUsedDepositPremiumDAO.DeleteTempUsedDepositPremium(tempUsedDepositPremium.Id);
                }
            }
        }

        #endregion

        // Solicitud Pago Siniestros/Varios
        #region PaymentRequest

        /// <summary>
        /// SearchClaimsPaymentRequest
        /// Obtiene las solicitudes de pago de acuerdo a varios criterios de búsqueda
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="typeSearch"></param>
        /// <returns>List<PaymentRequestVariousDTO/></returns>
        public List<SEARCH.PaymentRequestVariousDTO> SearchClaimsPaymentRequest(SEARCH.SearchParameterClaimsPaymentRequestDTO searchParameter, int typeSearch)
        {
            List<SEARCH.PaymentRequestVariousDTO> paymentRequestVarious = new List<SEARCH.PaymentRequestVariousDTO>();
            UIView claims;
            UIView various;

            int k = 1;
            string paymentRequestNumber = "";
            string index = "";

            // Busca por liquidaciones de siniestros
            if (typeSearch == 1)//Convert.ToInt32(CommonModelPayments.PaymentSources.ClaimsPaymentRequest)
            {
                #region LoadDTOClaimsPaymentRequest

                claims = LoadGetPaymentRequestClaim(searchParameter);

                SEARCH.PaymentRequestVariousDTO paymentRequestVariousDTO;

                foreach (DataRow dataRow in claims.Rows)
                {

                    DateTime? expirationDate;
                    if (dataRow["ExpirationDateQuota"] != DBNull.Value)
                    {
                        expirationDate = Convert.ToDateTime(dataRow["ExpirationDateQuota"]);
                    }
                    else
                    {
                        expirationDate = null;
                    }

                    int? quota;
                    if (dataRow["Quota"] != DBNull.Value)
                    {
                        quota = Convert.ToInt32(dataRow["Quota"]);
                    }
                    else
                    {
                        quota = null;
                    }

                    if (Convert.ToInt32(dataRow["PaymentSourceCode"]) != 2 && //Convert.ToInt32(CommonModelPayments.PaymentSources.Salvage)
                        Convert.ToInt32(dataRow["PaymentSourceCode"]) != 3) //Convert.ToInt32(CommonModelPayments.PaymentSources.Recovery)
                    {
                        index = dataRow["PaymentRequestNumber"].ToString();
                    }
                    else
                    {
                        index = dataRow["PaymentRequestNumber"].ToString();
                        paymentRequestNumber = "";
                    }

                    if (paymentRequestNumber != index)
                    {

                        paymentRequestVariousDTO = new SEARCH.PaymentRequestVariousDTO()
                        {
                            RowNumberUnique = Convert.ToInt32(Convert.ToString(k)),// + Convert.ToString(gridSetting["pageIndex"])),TODO validar
                            PaymentRequestCode = dataRow["PaymentRequestCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentRequestCode"]),
                            PaymentRequestNumber = dataRow["PaymentRequestNumber"] == DBNull.Value ? "" : dataRow["PaymentRequestNumber"].ToString(),
                            BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                            BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : dataRow["BranchDescription"].ToString(),
                            PrefixCode = dataRow["PrefixCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PrefixCode"]),
                            PrefixDescription = dataRow["PrefixDescription"] == DBNull.Value ? "" : dataRow["PrefixDescription"].ToString(),
                            PersonTypeCode = dataRow["PersonTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PersonTypeCode"]),
                            PersonTypeDescription = dataRow["PersonTypeDescription"] == DBNull.Value ? "" : dataRow["PersonTypeDescription"].ToString(),
                            PaymentSourceCode = dataRow["PaymentSourceCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentSourceCode"]),
                            PaymentSourceDescription = dataRow["PaymentSourceDescription"] == DBNull.Value ? "" : dataRow["PaymentSourceDescription"].ToString(),
                            ClaimNumber = dataRow["ClaimNumber"] == DBNull.Value ? "" : dataRow["ClaimNumber"].ToString(),
                            IdPayBeneficiary = dataRow["IdPayBeneficiary"] == DBNull.Value ? "" : dataRow["IdPayBeneficiary"].ToString(),
                            DocNumPayBeneficiary = dataRow["DocNumPayBeneficiary"] == DBNull.Value ? "" : dataRow["DocNumPayBeneficiary"].ToString(),
                            NamePayBeneficiary = dataRow["NamePayBeneficiary"] == DBNull.Value ? "" : dataRow["NamePayBeneficiary"].ToString(),
                            CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                            CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : dataRow["CurrencyDescription"].ToString(),
                            IdAgent = dataRow["IdAgent"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["IdAgent"]),
                            DocNumAgent = dataRow["DocNumAgent"] == DBNull.Value ? "" : dataRow["DocNumAgent"].ToString(),
                            NameAgent = dataRow["NameAgent"] == DBNull.Value ? "" : dataRow["NameAgent"].ToString(),
                            BusinessTypeCode = dataRow["BusinessTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BusinessTypeCode"]),
                            BusinessTypeDescription = dataRow["BusinessTypeDescription"] == DBNull.Value ? "" : dataRow["BusinessTypeDescription"].ToString(),
                            Branch_Prefix_Request = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 4) + "-" + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 4) + "-" + dataRow["PaymentRequestNumber"].ToString(),
                            DocumentNumber_Beneficiary = dataRow["DocNumPayBeneficiary"].ToString() + "-" + dataRow["NamePayBeneficiary"].ToString(),
                            FinancialPlanNumber = dataRow["FinancialPlanNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["FinancialPlanNumber"]),
                            TotalAmount = dataRow["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TotalAmount"]),
                            Quota = quota,
                            ExpirationDateQuota = expirationDate,
                            ClaimCode = dataRow["ClaimCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ClaimCode"])
                        };

                        if (DBNull.Value != dataRow["RegistrationDate"])
                        {
                            paymentRequestVariousDTO.RegistrationDate = (DateTime)dataRow["RegistrationDate"];
                        }

                        if (DBNull.Value != dataRow["EstimatedDate"])
                        {
                            paymentRequestVariousDTO.EstimatedDate = (DateTime)dataRow["EstimatedDate"];
                        }

                        if (DBNull.Value != dataRow["PaymentDate"])
                        {
                            paymentRequestVariousDTO.PaymentDate = (DateTime)dataRow["PaymentDate"];
                        }
                        paymentRequestVarious.Add(paymentRequestVariousDTO);
                    }
                    paymentRequestNumber = index;
                    k++;
                }

                #endregion

            }
            // Busca por pagos varios
            if (typeSearch == 4)//Convert.ToInt32(CommonModelPayments.PaymentSources.PaymentRequest)
            {
                #region LoadDTOPaymentRequestVarious

                various = LoadGetPaymentRequestVarious(searchParameter);
                SEARCH.PaymentRequestVariousDTO paymentRequestVariousDTO;
                foreach (DataRow dataRow in various.Rows)
                {

                    index = dataRow["PaymentRequestNumber"].ToString();

                    if (paymentRequestNumber != index)
                    {
                        paymentRequestVariousDTO = new SEARCH.PaymentRequestVariousDTO()
                        {
                            RowNumberUnique = Convert.ToInt32(Convert.ToString(k)),// + Convert.ToString(gridSetting["pageIndex"])), TODO VALIDAR
                            PaymentRequestCode = dataRow["PaymentRequestCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentRequestCode"]),
                            PaymentRequestNumber = dataRow["PaymentRequestNumber"] == DBNull.Value ? "" : dataRow["PaymentRequestNumber"].ToString(),
                            BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                            BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : dataRow["BranchDescription"].ToString(),
                            PrefixCode = dataRow["PrefixCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PrefixCode"]),
                            PrefixDescription = dataRow["PrefixDescription"] == DBNull.Value ? "" : dataRow["PrefixDescription"].ToString(),
                            PersonTypeCode = dataRow["PersonTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PersonTypeCode"]),
                            PersonTypeDescription = dataRow["PersonTypeDescription"] == DBNull.Value ? "" : dataRow["PersonTypeDescription"].ToString(),
                            PaymentSourceCode = dataRow["PaymentSourceCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentSourceCode"]),
                            PaymentSourceDescription = dataRow["PaymentSourceDescription"] == DBNull.Value ? "" : dataRow["PaymentSourceDescription"].ToString(),
                            IdPayBeneficiary = dataRow["IdPayBeneficiary"] == DBNull.Value ? "" : dataRow["IdPayBeneficiary"].ToString(),
                            DocNumPayBeneficiary = dataRow["DocNumPayBeneficiary"] == DBNull.Value ? "" : dataRow["DocNumPayBeneficiary"].ToString(),
                            NamePayBeneficiary = dataRow["NamePayBeneficiary"] == DBNull.Value ? "" : dataRow["NamePayBeneficiary"].ToString(),
                            CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                            CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : dataRow["CurrencyDescription"].ToString(),
                            StatusPayment = dataRow["StatusPayment"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["StatusPayment"]),
                            Branch_Request = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 4) + "-" + dataRow["PaymentRequestNumber"].ToString(),
                            DocumentNumber_Beneficiary = dataRow["DocNumPayBeneficiary"].ToString() + "-" + dataRow["NamePayBeneficiary"].ToString(),
                            TotalAmount = dataRow["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TotalAmount"])
                        };

                        if (DBNull.Value != dataRow["RegistrationDate"])
                        {
                            paymentRequestVariousDTO.RegistrationDate = (DateTime)dataRow["RegistrationDate"];
                        }

                        if (DBNull.Value != dataRow["EstimatedDate"])
                        {
                            paymentRequestVariousDTO.EstimatedDate = (DateTime)dataRow["EstimatedDate"];
                        }

                        if (DBNull.Value != dataRow["PaymentDate"])
                        {
                            paymentRequestVariousDTO.PaymentDate = (DateTime)dataRow["PaymentDate"];
                        }
                        paymentRequestVarious.Add(paymentRequestVariousDTO);

                    }

                    paymentRequestNumber = index;

                    k++;
                }
                #endregion
            }

            return paymentRequestVarious;
        }

        /// <summary>
        /// SaveClaimsPaymentRequestItem
        /// Graba las solicitudes pendientes de pago o cobro (salvamento/recobros)
        /// de siniestros/varios en Temporales
        /// </summary>
        /// <param name="claimsPaymentRequestItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO SaveTempClaimPaymentRequestItem(ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestItem, int imputationId, decimal exchangeRate)
        {
            Imputation imputation = new Imputation();
            int paymentSourceId = claimsPaymentRequestItem.PaymentRequest.MovementType.ConceptSource.Id;
            int paymentNumber = claimsPaymentRequestItem.PaymentRequest.TemporalId;
            DateTime firstPaymentDue = Convert.ToDateTime(claimsPaymentRequestItem.PaymentRequest.PaymentDate);

            // Convertir de model a entity
            ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPaymentEntity = EntityAssembler.CreateTempClaimPayment(claimsPaymentRequestItem.ToModel(), imputationId, exchangeRate,
                paymentSourceId, paymentNumber, firstPaymentDue);

            if (tempClaimPaymentEntity.PaymentExpirationDate == new DateTime())
            {
                tempClaimPaymentEntity.PaymentExpirationDate = null;
            }

            // Valida si ya consta en temporales o en reales
            List<ACCOUNTINGEN.TempClaimPaymentReqTrans> tempClaimPayments =
              _tempClaimPaymentRequestDAO.GetTempClaimPayment(Convert.ToInt32(tempClaimPaymentEntity.PaymentRequestCode),
                                                Convert.ToInt32(tempClaimPaymentEntity.PaymentNum), 0, 0);

            // Si no existe en temporales entonces busca en reales 
            if (tempClaimPayments.Count == 0)
            {
                // Llamado a verificar en reales
                List<ACCOUNTINGEN.ClaimPaymentRequestTrans> claimPaymentRequestTrans =
                                        GetClaimPaymentByPaymentRequestIdAndPaymentNum(Convert.ToInt32(tempClaimPaymentEntity.PaymentRequestCode),
                                                                                       Convert.ToInt32(tempClaimPaymentEntity.PaymentNum));
                // Si no existe en reales ahi si graba en temporales
                if (claimPaymentRequestTrans.Count == 0)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(tempClaimPaymentEntity);
                }
                else
                {
                    imputation.Id = Convert.ToInt32(claimPaymentRequestTrans[0].ImputationCode);
                    imputation.IsTemporal = false; // Esta en un imputación real
                }
            }
            else
            {
                imputation.Id = Convert.ToInt32(tempClaimPayments[0].TempImputationCode);
                imputation.IsTemporal = true; // Esta en un imputación temporal
            }

            return imputation.ToDTO();
        }

        /// <summary>
        /// SaveTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO SaveTempPaymentRequestItem(PaymentRequestTransactionItemDTO paymentRequestTransactionItem, int imputationId, decimal exchangeRate)
        {

            try
            {

                Imputation imputation = new Imputation();

                // Convertir de model a entity
                ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestEntity = EntityAssembler.CreateTempPaymentRequestTrans(paymentRequestTransactionItem.ToModel(), imputationId, exchangeRate);

                if (tempPaymentRequestEntity.PaymentExpirationDate == new DateTime())
                {
                    tempPaymentRequestEntity.PaymentExpirationDate = null;
                }

                // Valida si ya consta en temporales o en reales
                List<ACCOUNTINGEN.TempPaymentRequestTrans> tempPaymentRequestTrans = _tempPaymentRequestTransactionDAO.GetTempPaymentRequestTransactions(Convert.ToInt32(tempPaymentRequestEntity.PaymentRequestCode), 0);

                // Si no existe en temporales entonces busca en reales 
                if (tempPaymentRequestTrans.Count == 0)
                {
                    // Llamado a verificar en reales
                    List<ACCOUNTINGEN.PaymentRequestTrans> paymentRequestTrans = GetPaymentRequestTransByPaymentRequestId(Convert.ToInt32(tempPaymentRequestEntity.PaymentRequestCode));
                    // Si no existe en reales ahi si graba en temporales
                    if (paymentRequestTrans.Count == 0)
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().InsertObject(tempPaymentRequestEntity);
                    }
                    else
                    {
                        imputation.Id = Convert.ToInt32(paymentRequestTrans[0].ImputationCode);
                        imputation.IsTemporal = false; // Esta en un imputación real
                    }
                }
                else
                {
                    imputation.Id = Convert.ToInt32(tempPaymentRequestTrans[0].TempImputationCode);
                    imputation.IsTemporal = true; // Esta en un imputación temporal
                }

                return imputation.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// DeleteClaimsPaymentRequestItem
        /// Borra una solicitud de pago de siniestro 
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>
        /// <param name="imputationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>bool</returns>
        public bool DeleteTempClaimPaymentRequestItem(int claimsPaymentRequestId, int imputationId, bool isPaymentVarious)
        {
            try
            {
                return _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestItem(claimsPaymentRequestId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPaymentRequestItem(int paymentRequestId, int imputationId)
        {
            try
            {
                return _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestItem(paymentRequestId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempClaimsPaymentRequest
        /// Retorna los registros temporales desde pago de siniestro por el número de imputación deseada
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>List<TempPaymentRequestClaimDTO/></returns>
        public List<SEARCH.TempPaymentRequestClaimDTO> GetTempClaimsPaymentRequest(int imputationId, bool isPaymentVarious)
        {
            #region LoadDTO

            List<SEARCH.TempPaymentRequestClaimDTO> tempPaymentRequestClaims = new List<SEARCH.TempPaymentRequestClaimDTO>();

            UIView tempPaymentRequestClaimView = LoadGetTempPaymentRequestClaim(imputationId, isPaymentVarious);

            int k = 1;
            SEARCH.TempPaymentRequestClaimDTO tempPaymentRequestClaimDTO;
            foreach (DataRow dataRow in tempPaymentRequestClaimView.Rows)
            {
                DateTime? expirationDate;
                if (dataRow["PaymentExpirationDate"] != DBNull.Value)
                {
                    expirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]);
                }
                else
                {
                    expirationDate = null;
                }

                int? quota;
                if (dataRow["PaymentNum"] != DBNull.Value)
                {
                    quota = Convert.ToInt32(dataRow["PaymentNum"]);
                }
                else
                {
                    quota = null;
                }

                tempPaymentRequestClaimDTO = new SEARCH.TempPaymentRequestClaimDTO()
                {
                    TempClaimPaymentCode = dataRow["tempClaimPaymentReqTransCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["tempClaimPaymentReqTransCode"]),
                    TempImputationCode = dataRow["TempImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempImputationCode"]),
                    PaymentRequestCode = dataRow["PaymentRequestCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentRequestCode"]),
                    BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                    BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : dataRow["BranchDescription"].ToString(),
                    PrefixCode = dataRow["PrefixCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PrefixCode"]),
                    PrefixDescription = dataRow["PrefixDescription"] == DBNull.Value ? "" : dataRow["PrefixDescription"].ToString(),
                    BeneficiaryId = dataRow["BeneficiaryId"] == DBNull.Value ? "" : dataRow["BeneficiaryId"].ToString(),
                    DocNumPayBeneficiary = dataRow["DocNumPayBeneficiary"] == DBNull.Value ? "" : dataRow["DocNumPayBeneficiary"].ToString(),
                    NamePayBeneficiary = dataRow["NamePayBeneficiary"] == DBNull.Value ? "" : dataRow["NamePayBeneficiary"].ToString(),
                    CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                    CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : dataRow["CurrencyDescription"].ToString(),
                    BussinessType = dataRow["BussinessType"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BussinessType"]),
                    BusinessTypeDescription = dataRow["BusinessTypeDescription"] == DBNull.Value ? "" : dataRow["BusinessTypeDescription"].ToString(),
                    RequestType = dataRow["RequestType"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RequestType"]),
                    PaymentSourceDescription = dataRow["PaymentSourceDescription"] == DBNull.Value ? "" : dataRow["PaymentSourceDescription"].ToString(),
                    IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                    PaymentNum = quota,
                    PaymentExpirationDate = expirationDate,
                    Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                    Branch_Prefix_Request = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 4) + "-" + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 4) + "-" + dataRow["PaymentRequestNumber"].ToString(),
                    DocumentNumber_Beneficiary = dataRow["DocNumPayBeneficiary"].ToString() + "-" + dataRow["NamePayBeneficiary"].ToString(),
                    Branch_Request = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 4) + "-" + dataRow["PaymentRequestNumber"].ToString()
                };
                if (dataRow["RegistrationDate"] != DBNull.Value)
                {
                    tempPaymentRequestClaimDTO.RegistrationDate = (DateTime)dataRow["RegistrationDate"];
                }
                if (dataRow["EstimationDate"] != DBNull.Value)
                {
                    tempPaymentRequestClaimDTO.EstimationDate = (DateTime)dataRow["EstimationDate"];
                }
                tempPaymentRequestClaims.Add(tempPaymentRequestClaimDTO);

                k++;
            }

            #endregion

            return tempPaymentRequestClaims;
        }

        /// <summary>
        /// GetTempPaymentRequestByImputationId
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>List<TempPaymentRequestClaimDTO></returns>
        public List<SEARCH.TempPaymentRequestClaimDTO> GetTempPaymentRequestByImputationId(int imputationId)
        {
            try
            {

                #region LoadDTO

                List<SEARCH.TempPaymentRequestClaimDTO> tempPaymentRequestClaims = new List<SEARCH.TempPaymentRequestClaimDTO>();

                UIView tempPaymentRequestClaimView = LoadGetTempPaymentRequestItems(imputationId);
                SEARCH.TempPaymentRequestClaimDTO tempPaymentRequestClaimDTO;
                foreach (DataRow dataRow in tempPaymentRequestClaimView.Rows)
                {
                    DateTime? expirationDate;
                    if (dataRow["PaymentExpirationDate"] != DBNull.Value)
                    {
                        expirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]);
                    }
                    else
                    {
                        expirationDate = null;
                    }

                    int? quota;
                    if (dataRow["PaymentNumber"] != DBNull.Value)
                    {
                        quota = Convert.ToInt32(dataRow["PaymentNumber"]);
                    }
                    else
                    {
                        quota = null;
                    }

                    tempPaymentRequestClaimDTO = new SEARCH.TempPaymentRequestClaimDTO()
                    {
                        TempClaimPaymentCode = dataRow["TempPaymentRequestTransId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempPaymentRequestTransId"]),
                        TempImputationCode = dataRow["TempImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempImputationCode"]),
                        PaymentRequestCode = dataRow["PaymentRequestId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentRequestId"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : dataRow["BranchDescription"].ToString(),
                        PrefixCode = dataRow["PrefixCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = dataRow["PrefixDescription"] == DBNull.Value ? "" : dataRow["PrefixDescription"].ToString(),
                        BeneficiaryId = dataRow["BeneficiaryId"] == DBNull.Value ? "" : dataRow["BeneficiaryId"].ToString(),
                        DocNumPayBeneficiary = dataRow["BeneficiaryDocumentNumber"] == DBNull.Value ? "" : dataRow["BeneficiaryDocumentNumber"].ToString(),
                        NamePayBeneficiary = dataRow["BeneficiaryName"] == DBNull.Value ? "" : dataRow["BeneficiaryName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : dataRow["CurrencyDescription"].ToString(),
                        BussinessType = dataRow["BusinessTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BusinessTypeDescription = dataRow["BusinessTypeDescription"] == DBNull.Value ? "" : dataRow["BusinessTypeDescription"].ToString(),
                        RequestType = dataRow["ConceptSourceCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ConceptSourceCode"]),
                        PaymentSourceDescription = dataRow["ConceptSourceDescription"] == DBNull.Value ? "" : dataRow["ConceptSourceDescription"].ToString(),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        PaymentNum = quota,
                        PaymentExpirationDate = expirationDate,
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        Branch_Prefix_Request = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 4) + "-" + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 4) + "-" + dataRow["PaymentRequestNumber"],
                        DocumentNumber_Beneficiary = dataRow["BeneficiaryDocumentNumber"] + "-" + dataRow["BeneficiaryName"],
                        Branch_Request = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 4) + "-" + dataRow["PaymentRequestNumber"],
                        PaymentRequestNumber = Convert.ToString(dataRow["PaymentRequestNumber"])
                    };
                    if (dataRow["RegistrationDate"] != DBNull.Value)
                    {
                        tempPaymentRequestClaimDTO.RegistrationDate = (DateTime)dataRow["RegistrationDate"];
                    }
                    if (dataRow["EstimationDate"] != DBNull.Value)
                    {
                        tempPaymentRequestClaimDTO.EstimationDate = (DateTime)dataRow["EstimationDate"];
                    }

                    tempPaymentRequestClaims.Add(tempPaymentRequestClaimDTO);
                }

                #endregion

                return tempPaymentRequestClaims;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        //Cuenta Corriente Agentes
        #region CurrentAccountAgents

        #region TempBrokersCheckingAccount

        /// <summary>
        /// DeleteBrokerCheckingAccountItemChild
        /// Elimina un item de cuenta de agente
        /// </summary>
        /// <param name="tempBrokerCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteBrokerCheckingAccountItemChild(int tempBrokerCheckingAccountItemId)
        {
            try
            {
                return _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItem(tempBrokerCheckingAccountItemId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetTempBrokerCheckingAccountItemByTempImputationId
        /// Obtiene un item de cuenta de agente
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempImputationCode, tempImputationId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempBrokerParentCode, 0);

                //UIView tempBrokerCheckingAccountItems = _dataFacadeManager.GetDataFacade().GetView("TempBrokerCheckingAccTransView",
                //                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<ACCOUNTINGEN.TempBrokerCheckingAcctransV> tempBrokerCheckingAccountItems = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.TempBrokerCheckingAcctransV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.TempBrokerCheckingAcctransV>().ToList();
                int rows = 0;
                if (tempBrokerCheckingAccountItems.Count > 0)
                {
                    rows = tempBrokerCheckingAccountItems.Count;
                }

                // Load DTO
                List<SEARCH.BrokerCheckingAccountItemDTO> brokerCheckingAccountItems = new List<SEARCH.BrokerCheckingAccountItemDTO>();

                foreach (ACCOUNTINGEN.TempBrokerCheckingAcctransV tempBrokerCheckingAccountItem in tempBrokerCheckingAccountItems)
                {
                    brokerCheckingAccountItems.Add(new SEARCH.BrokerCheckingAccountItemDTO()
                    {
                        BrokerCheckingAccountItemId = Convert.ToInt32(tempBrokerCheckingAccountItem.TempBrokerCheckingAccTransCode),
                        TempImputationId = Convert.ToInt32(tempBrokerCheckingAccountItem.TempImputationCode),
                        TempBrokerParentId = Convert.ToInt32(tempBrokerCheckingAccountItem.TempBrokerParentCode),
                        BranchCode = Convert.ToInt32(tempBrokerCheckingAccountItem.BranchCode),
                        BranchName = tempBrokerCheckingAccountItem.BranchDescription.ToString(),
                        PosCode = Convert.ToInt32(tempBrokerCheckingAccountItem.SalePointCode),
                        PosName = tempBrokerCheckingAccountItem.SalePointName.ToString(),
                        CompanyCode = Convert.ToInt32(tempBrokerCheckingAccountItem.CompanyId),
                        CompanyName = tempBrokerCheckingAccountItem.AccountingCompanyName.ToString(),
                        AgentCode = Convert.ToInt32(tempBrokerCheckingAccountItem.Agentcode),
                        AgentName = tempBrokerCheckingAccountItem.DocumentNumber.ToString() + '-' + tempBrokerCheckingAccountItem.AgentName,
                        AgentDocumentNumber = tempBrokerCheckingAccountItem.DocumentNumber.ToString(),
                        CheckingAccountConceptCode = Convert.ToInt32(tempBrokerCheckingAccountItem.CheckingAccountConceptCode),
                        ConceptName = tempBrokerCheckingAccountItem.CheckingAccountConceptCode.ToString() + '-' + tempBrokerCheckingAccountItem.PaymentConceptName,
                        DebitCreditCode = Convert.ToInt32(tempBrokerCheckingAccountItem.AccountingNature),
                        DebitCreditName = Convert.ToInt32(tempBrokerCheckingAccountItem.AccountingNature) == 1 ? "Crédito" : "Débito",
                        AccountNature = Convert.ToInt32(tempBrokerCheckingAccountItem.AccountingNature),
                        CurrencyCode = Convert.ToInt32(tempBrokerCheckingAccountItem.CurrencyCode),
                        CurrencyName = tempBrokerCheckingAccountItem.TinyDescription.ToString(),
                        CurrencyChange = Convert.ToDecimal(tempBrokerCheckingAccountItem.ExchangeRate),
                        IncomeAmount = Convert.ToDecimal(tempBrokerCheckingAccountItem.IncomeAmount),
                        Amount = Convert.ToDecimal(tempBrokerCheckingAccountItem.Amount),
                        Description = tempBrokerCheckingAccountItem.Description.ToString(),
                        AgentTypeCode = Convert.ToInt32(tempBrokerCheckingAccountItem.AgentTypeCode),
                        AgentAgencyCode = Convert.ToInt32(tempBrokerCheckingAccountItem.AgentAgencyCode),
                        CollectNumber = 0,
                        Status = 0,
                        Items = Convert.ToInt32(tempBrokerCheckingAccountItem.Transactionnumber) == 0 ? 0 : Convert.ToDecimal(tempBrokerCheckingAccountItem.IncomeAmount),
                        Rows = rows
                    });
                }

                return brokerCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempBrokerCheckingAccountItemChildByTempBrokerParentId
        ///  Obtiene un item temporal de cuenta de agente
        /// </summary>
        /// <param name="tempBrokerParentId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemChildByTempBrokerParentId(int tempBrokerParentId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccountItem.Properties.TempBrokerCheckingAccTransCode, tempBrokerParentId);

                UIView tempBrokerCheckingAccountItemChilds = _dataFacadeManager.GetDataFacade().GetView("TempBrokerCheckingAccountItemsView",
                                            criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (tempBrokerCheckingAccountItemChilds.Rows.Count > 0)
                {
                    tempBrokerCheckingAccountItemChilds.Columns.Add("rows", typeof(int));
                    tempBrokerCheckingAccountItemChilds.Rows[0]["rows"] = rows;
                }

                List<SEARCH.BrokerCheckingAccountItemDTO> brokerCheckingAccountItems = new List<SEARCH.BrokerCheckingAccountItemDTO>();

                foreach (DataRow dataRow in tempBrokerCheckingAccountItemChilds)
                {
                    brokerCheckingAccountItems.Add(new SEARCH.BrokerCheckingAccountItemDTO()
                    {
                        BrokerCheckingAccountId = dataRow["TempBrokerCheckingAccountCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempBrokerCheckingAccountCode"]),
                        BrokerCheckingAccountItemId = Convert.ToInt32(dataRow["BrokerCheckingAccountTransId"]),
                        TempImputationId = Convert.ToInt32(dataRow["ImputationCode"]),
                        BrokerCheckingAccountItemChildId = Convert.ToInt32(dataRow["TempBrokerCheckingAccountItemCode"]),
                        BranchCode = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"].ToString(),
                        SalePointCode = Convert.ToInt32(dataRow["SalePointCode"]),
                        SalePointName = dataRow["SalePointName"].ToString(),
                        DebitCreditCode = Convert.ToDecimal(dataRow["IncomeAmount"]) > 0 ? Convert.ToInt32(AccountingNature.Debit) : Convert.ToInt32(AccountingNature.Credit),
                        DebitCreditName = Convert.ToDecimal(dataRow["IncomeAmount"]) > 0 ? "Débito" : "Crédito",
                        AccountNature = Convert.ToDecimal(dataRow["IncomeAmount"]) > 0 ? Convert.ToInt32(AccountingNature.Debit) : Convert.ToInt32(AccountingNature.Credit),
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyName"].ToString(),
                        CurrencyChange = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        Description = dataRow["Description"].ToString(),
                        AgentTypeCode = Convert.ToInt32(dataRow["AgentTypeCode"]),
                        AgentAgencyCode = Convert.ToInt32(dataRow["AgentAgencyCode"]),
                        CollectNumber = 0,
                        Status = 0,
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        PolicyDocumentNumber = dataRow["PolicyNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PolicyNumber"]),
                        PrefixCode = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixName = dataRow["PrefixName"].ToString(),
                        InsuredId = Convert.ToInt32(dataRow["InsuredId"]),
                        InsuredDocNumber = dataRow["InsuredDocumentNumber"].ToString(),
                        InsuredName = dataRow["InsuredDocumentNumber"] + "-" + dataRow["InsuredName"],
                        CommissionTypeCode = Convert.ToInt32(dataRow["CommissionTypeCode"]),
                        CommissionTypeName = "Normal",
                        CommissionPercentage = Convert.ToDecimal(dataRow["StCommissionPercentage"]),
                        CommissionAmount = Convert.ToDecimal(dataRow["StCommissionAmount"]),
                        CommissionDiscounted = Convert.ToDecimal(dataRow["DiscountedCommission"]),
                        CommissionBalance = Convert.ToDecimal(dataRow["CommissionBalance"]),
                        Rows = rows
                    });
                }

                return brokerCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region BrokersCheckingAccount

        /// <summary>
        /// SearchBrokersCheckingAccount
        /// Búsqueda de cuenta de agentes
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<SearchAgentsItemsDTO/></returns>
        public List<SEARCH.SearchAgentsItemsDTO> SearchBrokersCheckingAccount(SEARCH.SearchParameterBrokersCheckingAccountDTO searchParameter)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.AgentId, searchParameter.AgentId);

                if (searchParameter.Branch.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.BranchCode, searchParameter.Branch.Id);
                }

                if (searchParameter.SalePoint.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.SalePointCode, searchParameter.SalePoint.Id);
                }

                if (searchParameter.Prefix.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.PrefixCode, searchParameter.Prefix.Id);
                }

                if (searchParameter.PolicyNumber != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.PolicyDocumentNumber, searchParameter.PolicyNumber);
                }

                if (searchParameter.Currency.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.CurrencyCode, searchParameter.Currency.Id);
                }

                if (searchParameter.InsuredDocumentNumber != "-1")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.InsuredId, searchParameter.InsuredDocumentNumber);
                }

                UIView agentItems = _dataFacadeManager.GetDataFacade().GetView("SearchAgentsItems",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #endregion

                if (agentItems.Rows.Count > 0)
                {
                    agentItems.Columns.Add("rows", typeof(int));
                    agentItems.Rows[0]["rows"] = rows;
                }

                #region DTO

                List<SEARCH.SearchAgentsItemsDTO> searchAgentsItems = new List<SEARCH.SearchAgentsItemsDTO>();

                foreach (DataRow dataRow in agentItems)
                {
                    searchAgentsItems.Add(new SEARCH.SearchAgentsItemsDTO()
                    {
                        BrokerCheckingAccountItemId = dataRow["BrokerCheckingAccountTransId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BrokerCheckingAccountTransId"]),
                        BrokerParentId = dataRow["BrokerParentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BrokerParentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        SalePointCode = dataRow["SalePointCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SalePointCode"]),
                        SalePointName = dataRow["SalePointDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["SalePointDescription"]),
                        PrefixCode = dataRow["PrefixCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixName = dataRow["PrefixDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["PrefixDescription"]),
                        PolicyDocumentNumber = dataRow["PolicyDocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["PolicyId"]),
                        InsuredDocNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        InsuredName = (dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"])) + "-" + (dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"])),
                        InsuredId = dataRow["InsuredId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["InsuredId"]),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        CommissionTypeCode = dataRow["CommissionTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CommissionTypeCode"]),
                        CommissionTypeName = "Normal",
                        CommissionPercentage = dataRow["CommissionPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CommissionPercentage"]),
                        CommissionAmount = dataRow["CommissionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CommissionAmount"]),
                        CommissionDiscounted = dataRow["CommissionDiscounted"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CommissionDiscounted"]),
                        CommissionBalance = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CommissionBalance"]),
                        Rows = rows
                    });
                }

                #endregion

                return searchAgentsItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveBrokersCheckingAccountItem
        /// Registra una cuenta de agente
        /// </summary>
        /// <param name="brokerCheckingAccountItem"></param>
        /// <returns>bool</returns>
        public bool SaveBrokersCheckingAccountItem(List<BrokerCheckingAccountItemDTO> brokerCheckingAccountItem)
        {
            try
            {
                foreach (BrokerCheckingAccountItemDTO tempBrokerCheckingAccountItem in brokerCheckingAccountItem)
                {
                    _tempBrokerCheckingAccountTransactionItemDAO.SaveTempBrokerCheckingAccountItem(tempBrokerCheckingAccountItem.ToModel()).ToDTO();
                }
            }
            catch (BusinessException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// UpdateTempBrokersCheckingAccountTotal
        /// </summary>
        /// <param name="tempBrokerCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>BrokersCheckingAccountTransaction</returns>
        public BrokersCheckingAccountTransactionDTO UpdateTempBrokersCheckingAccountTotal(int tempBrokerCheckingAccountId, decimal selectedTotal)
        {
            try
            {
                return _tempBrokerCheckingAccountTransactionItemDAO.UpdateTempBrokersCheckingAccountTotal(tempBrokerCheckingAccountId, selectedTotal).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteBrokersCheckingAccountItem
        /// Elimina una cuenta de agente
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="brokersCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteBrokersCheckingAccountItem(int tempImputationId, int brokersCheckingAccountItemId)
        {
            bool isDeleted = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempImputationCode, tempImputationId);
                if (brokersCheckingAccountItemId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempBrokerCheckingAccTransCode, brokersCheckingAccountItemId);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccountItem.Properties.TempBrokerCheckingAccTransCode, tempBrokerCheckingAccount.TempBrokerCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new
                        BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                              (typeof(ACCOUNTINGEN.TempBrokerCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempBrokerCheckingAccountItem tempBrokerCheckingAccountItemEntity in businessCollectionItems.OfType<ACCOUNTINGEN.TempBrokerCheckingAccountItem>())
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempBrokerCheckingAccountItemEntity);
                    }

                    if (brokersCheckingAccountItemId == -1)
                    {
                        _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountTransactionItem(tempBrokerCheckingAccount.TempBrokerCheckingAccTransCode);
                        isDeleted = true;
                    }

                    if (brokersCheckingAccountItemId != -1)
                    {
                        _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountTransactionItem(tempBrokerCheckingAccount.TempBrokerCheckingAccTransCode);
                        isDeleted = true;
                    }
                }
            }
            catch (Exception)
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        /// <summary>
        /// SaveBrokersCheckingAccount
        /// Graba una cuenta de agente
        /// </summary>
        /// <param name="brokersCheckingAccountTransaction"></param>
        /// <param name="imputationId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>bool</returns>
        public bool SaveBrokersCheckingAccount(BrokersCheckingAccountTransactionDTO brokersCheckingAccountTransaction, int imputationId, DateTime accountingDate)
        {
            bool isSaved = false;

            try
            {
                foreach (BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItem in brokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems)
                {
                    BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItemNew;

                    brokersCheckingAccountTransactionItemNew = brokersCheckingAccountTransactionItem.Id == 0 ? _tempBrokerCheckingAccountTransactionItemDAO.SaveTempBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem.ToModel(), imputationId, 0, 0 /*billCode*/, accountingDate).ToDTO() : _tempBrokerCheckingAccountTransactionItemDAO.UpdateTempBrokerCheckingAccount(brokersCheckingAccountTransactionItem.ToModel(), imputationId, 0, 0 /*billCode*/, accountingDate).ToDTO();

                    foreach (BrokerCheckingAccountItemDTO brokerCheckingAccountItemChild in brokersCheckingAccountTransactionItem.BrokersCheckingAccountItems)
                    {
                        brokerCheckingAccountItemChild.TempBrokerCheckingAccountId = brokersCheckingAccountTransactionItemNew.Id;

                        if (brokerCheckingAccountItemChild.Id == 0)
                        {
                            _tempBrokerCheckingAccountTransactionItemDAO.SaveTempBrokerCheckingAccountItem(brokerCheckingAccountItemChild.ToModel()).ToDTO();
                        }
                        else
                        {
                            _tempBrokerCheckingAccountTransactionItemDAO.UpdateTempBrokerCheckingAccountItem(brokerCheckingAccountItemChild.ToModel()).ToDTO();
                        }
                    }

                    isSaved = true;
                }
            }
            catch (Exception)
            {
                isSaved = false;
            }

            return isSaved;
        }

        /// <summary>
        /// GetCurrencyDifferenceByCurrencyId
        /// Obtiene el porcentaje de diferencia dada la moneda
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        public decimal GetPercentageDifferenceByCurrencyId(int currencyId)
        {
            decimal percentageDifference = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CurrencyDifference.Properties.CurrencyCode, currencyId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.CurrencyDifference), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.CurrencyDifference currencyDifference in businessCollection.OfType<ACCOUNTINGEN.CurrencyDifference>())
                {
                    percentageDifference = Convert.ToDecimal(currencyDifference.PercentageDifference);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return percentageDifference;
        }

        #endregion

        #region SearchAgents

        /// <summary>
        /// SearchAgentsItems
        /// Búsqueda generales de agentes
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNum"></param>
        /// <param name="currencyId"></param>
        /// <param name="insuredId"></param>
        /// <returns>List<SearchAgentsItemsDTO/></returns>
        public List<SEARCH.SearchAgentsItemsDTO> SearchAgentsItems(int branchId, int salePointId, int prefixId, int policyNum, int currencyId,
                                            int insuredId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                if (branchId != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.BranchCode, branchId);
                }

                if (salePointId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.SalePointCode, salePointId);
                }

                if (prefixId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.PrefixCode, prefixId);
                }

                if (policyNum != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckAccountTrans.Properties.PolicyDocumentNumber, policyNum);
                }

                if (currencyId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.CurrencyCode, currencyId);
                }

                if (insuredId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(UPEN.ListPersons.Properties.IndividualId, insuredId);
                }

                UIView agentItems = _dataFacadeManager.GetDataFacade().GetView("BrokerCheckAccountTransView",
                                                          criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #endregion

                if (agentItems.Rows.Count > 0)
                {
                    agentItems.Columns.Add("rows", typeof(int));
                    agentItems.Rows[0]["rows"] = rows;
                }

                #region DTO

                List<SEARCH.SearchAgentsItemsDTO> searchAgentsItems = new List<SEARCH.SearchAgentsItemsDTO>();

                foreach (DataRow dataRow in agentItems)
                {
                    searchAgentsItems.Add(new SEARCH.SearchAgentsItemsDTO()
                    {
                        BrokerCheckingAccountItemId = dataRow["BrokerCheckingAccountTransId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BrokerCheckingAccountTransId"]),
                        BrokerParentId = dataRow["BrokerParentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BrokerParentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        SalePointCode = dataRow["SalePointCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SalePointCode"]),
                        SalePointName = dataRow["SalePointDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["SalePointDescription"]),
                        PrefixCode = dataRow["PrefixCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixName = dataRow["PrefixDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["PrefixDescription"]),
                        PolicyDocumentNumber = dataRow["PolicyDocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["PolicyId"]),
                        InsuredDocNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        InsuredName = (dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"])) + "-" + (dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"])),
                        InsuredId = dataRow["InsuredId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["InsuredId"]),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        CommissionTypeCode = dataRow["CommissionTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CommissionTypeCode"]),
                        CommissionTypeName = "Normal",
                        CommissionPercentage = dataRow["StCommissionPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["StCommissionPercentage"]),
                        CommissionAmount = dataRow["StCommissionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["StCommissionAmount"]),
                        CommissionDiscounted = dataRow["CommissionDiscounted"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CommissionDiscounted"]),
                        CommissionBalance = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CommissionBalance"]),
                        Rows = rows
                    });
                }
                #endregion

                return searchAgentsItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Validate

        /// <summary>
        /// ValidateDuplicateBrokerCheckingAccount
        /// Permite validar el ingreso de duplicados tanto en temporales como en reales de agentes
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO ValidateDuplicateBrokerCheckingAccount(SEARCH.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter)
        {
            Imputation imputation = new Imputation();
            imputation.VerificationValue = new Amount();
            imputation.VerificationValue.Value = -1;

            try
            {
                // Se verifica en la tabla temporal
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.BranchCode, validateParameter.Branch.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.SalePointCode, validateParameter.SalePoint.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.AccountingCompanyCode, validateParameter.Company.IndividualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.AccountingNature, validateParameter.AccountingNatureId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.CurrencyCode, validateParameter.Currency.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.AgentId, validateParameter.AgentId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.CheckingAccountConceptCode, validateParameter.CheckingAccountConceptId);

                BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), criteriaBuilder.GetPredicate()));

                if (businessCollectionItems.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccount in businessCollectionItems.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
                    {
                        imputation.VerificationValue.Value = 0;
                        imputation.Id = (int)tempBrokerCheckingAccount.TempImputationCode;
                    }
                }

                // Existe el registro en imputacion temporal
                if (imputation.Id > 0)
                {
                    // Se verifica en la tabla temporal
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.TempImputationCode, imputation.Id);

                    BusinessCollection busCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempImputation), criteriaBuilder.GetPredicate()));

                    if (busCollection.Count > 0)
                    {

                        ACCOUNTINGEN.TempImputation tempImputation = busCollection.OfType<ACCOUNTINGEN.TempImputation>().First();

                        switch (tempImputation.ImputationTypeCode)
                        {
                            case 1:
                                imputation.ImputationType = ImputationTypes.Collect;
                                break;
                            case 2:
                                imputation.ImputationType = ImputationTypes.JournalEntry;
                                break;
                            case 3:
                                imputation.ImputationType = ImputationTypes.PreLiquidation;
                                break;
                            case 4:
                                imputation.ImputationType = ImputationTypes.PaymentOrder;
                                break;
                            default:
                                imputation.ImputationType = ImputationTypes.Collect;
                                break;
                        }

                        imputation.IsTemporal = Convert.ToBoolean(!tempImputation.IsRealSource);

                        if (Convert.ToInt32(tempImputation.IsRealSource) == 1) //--> ya esta converida la OP/PL en real
                        {
                            TransactionType transactionType = new TransactionType();
                            imputation.ImputationItems = new List<TransactionType>();
                            transactionType.Id = Convert.ToInt32(tempImputation.SourceCode);
                        }

                    }
                }

                // Se verifica en la tabla real
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.BranchCode, validateParameter.Branch.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.SalePointCode, validateParameter.SalePoint.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AccountingCompanyCode, validateParameter.Company.IndividualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AccountingNature, validateParameter.AccountingNatureId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.CurrencyCode, validateParameter.Currency.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.AgentId, validateParameter.AgentId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.CheckingAccountConceptCode, validateParameter.CheckingAccountConceptId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.BrokerCheckingAccountTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.BrokerCheckingAccountTrans>())
                    {
                        imputation.VerificationValue.Value = 1;
                        imputation.Id = (int)brokerCheckingAccount.ImputationCode;
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return imputation.ToDTO();
        }

        #endregion

        #endregion

        //Cuenta Corriente Reaseguros
        #region CurrentAccountReinsurance

        /// <summary>
        /// SearchReinsuranceCheckingAccount
        /// Búsqueda de cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.ReinsuranceCheckingAccountItemDTO> SearchReinsuranceCheckingAccount(SEARCH.SearchParameterReinsuranceCheckingAccountDTO searchParameter)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ReinsuranceCompanyId, searchParameter.ReinsurerId);

                if (searchParameter.Branch.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.BranchCode, searchParameter.Branch.Id);
                }

                if (searchParameter.SalePoint.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.SalePointCode, searchParameter.SalePoint.Id);
                }

                if (searchParameter.AccountingCompany.IndividualId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.AccountingCompanyCode, searchParameter.AccountingCompany.IndividualId);
                }

                if (searchParameter.Prefix.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.LineBusinessCode, searchParameter.Prefix.Id);
                }

                if (searchParameter.Prefix.LineBusinessId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.SubLineBusinessCode, searchParameter.Prefix.LineBusinessId);
                }

                if (searchParameter.Currency.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.CurrencyCode, searchParameter.Currency.Id);
                }

                if (searchParameter.ContractType != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ContractTypeCode, searchParameter.ContractType);
                }

                if (searchParameter.ContractNumber != "*")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ContractCode, searchParameter.ContractNumber); // No es ContractNumber es ContractId
                }

                if (searchParameter.AgentId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.AgentId, searchParameter.AgentId);
                }

                if (searchParameter.ReinsuranceCompanyId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ReinsuranceCompanyId, searchParameter.ReinsuranceCompanyId);
                }
                if (searchParameter.SlipNumber != "*")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.SlipNumber, searchParameter.SlipNumber);
                }
                if (searchParameter.DateFrom != "*")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ApplicationDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(searchParameter.DateFrom);
                }
                if (searchParameter.DateUntil != "*")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ApplicationDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(searchParameter.DateUntil);
                }


                UIView reinsuranceItems = _dataFacadeManager.GetDataFacade().GetView("ReinsuranceCheckingAccountView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #endregion

                if (reinsuranceItems.Rows.Count > 0)
                {
                    reinsuranceItems.Columns.Add("rows", typeof(int));
                    reinsuranceItems.Rows[0]["rows"] = rows;
                }

                #region DTO

                List<SEARCH.ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItems = new List<SEARCH.ReinsuranceCheckingAccountItemDTO>();

                foreach (DataRow dataRow in reinsuranceItems)
                {
                    reinsuranceCheckingAccountItems.Add(new SEARCH.ReinsuranceCheckingAccountItemDTO()
                    {
                        ReinsuranceCheckingAccountItemId = dataRow["ReinsCheckingAccTransId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsCheckingAccTransId"]),
                        ImputationId = dataRow["ImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ImputationCode"]),
                        ReinsuranceParentId = dataRow["ReinsuranceParentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsuranceParentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        PosCode = dataRow["SalePointCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SalePointCode"]),
                        PosName = dataRow["SalePointName"] == DBNull.Value ? "" : dataRow["SalePointName"].ToString(),
                        LineBusinessCode = dataRow["LineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["LineBusinessCode"]),
                        PrefixName = dataRow["LineBusinessName"] == DBNull.Value ? "" : dataRow["LineBusinessName"].ToString(),
                        SubLineBusinessCode = dataRow["SubLineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubLineBusinessCode"]),
                        SubPrefixName = dataRow["SubLineBusinessName"] == DBNull.Value ? "" : dataRow["SubLineBusinessName"].ToString(),
                        BrokerName = dataRow["BrokerName"] == DBNull.Value ? "" : dataRow["BrokerName"].ToString(),
                        ReinsurerName = dataRow["ReinsurerName"] == DBNull.Value ? "" : dataRow["ReinsurerName"].ToString(),
                        SlipNumber = dataRow["SlipNumber"] == DBNull.Value ? "" : dataRow["SlipNumber"].ToString(),
                        ContractTypeCode = dataRow["ContractTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ContractTypeCode"]),
                        Contract = dataRow["ContractCode"] == DBNull.Value ? "" : dataRow["ContractCode"].ToString(),
                        Stretch = dataRow["Section"] == DBNull.Value ? "" : dataRow["Section"].ToString(),
                        Region = dataRow["Region"] == DBNull.Value ? "" : dataRow["Region"].ToString(),
                        Excercise = dataRow["Period"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Period"]),
                        CheckingAccountConceptCode = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptName = dataRow["CheckingAccountConceptName"] == DBNull.Value ? "" : dataRow["CheckingAccountConceptName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["TinyDescription"].ToString(),
                        CurrencyChange = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        DebitCreditName = Convert.ToInt32(dataRow["AccountingNature"]) == 1 ? "Crédito" : "Débito",
                        AccountingNature = dataRow["AccountingNature"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNature"]),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                        YearMonthApplies = dataRow["YearMonth"] == DBNull.Value ? "" : dataRow["YearMonth"].ToString(),
                        PolicyEndorsement = dataRow["PolicyNumber"] == DBNull.Value ? "" : dataRow["PolicyNumber"].ToString(),
                        CompanyCode = dataRow["AccountingCompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingCompanyCode"]),
                        CompanyName = dataRow["AccountingCompanyName"] == DBNull.Value ? "" : dataRow["AccountingCompanyName"].ToString(),
                        AgentTypeCode = 1,
                        AgentCode = dataRow["AgentId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AgentId"]),
                        AgentAgencyCode = 1,
                        CollectNumber = 0,
                        FacultativeCode = dataRow["ContractTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ContractTypeCode"]),
                        ReinsuranceCompanyCode = dataRow["ReinsuranceCompanyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsuranceCompanyId"]),
                        ApplicationMonth = dataRow["ApplicationMonth"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationMonth"]),
                        ApplicationYear = dataRow["ApplicationYear"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationYear"]),
                        ReinsurancePolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PolicyId"]),
                        ReinsuranceEndorsementId = dataRow["EndorsementId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["EndorsementId"]),
                        TransactionNumber = dataRow["TransactionNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TransactionNumber"]),
                        Status = 0,
                        DebitCreditCode = dataRow["AccountingNature"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNature"]),
                        Items = Convert.ToInt32(dataRow["TransactionNumber"]) == 0 ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Rows = rows
                    });
                }

                #endregion

                return reinsuranceCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveReinsuranceCheckingAccountItems
        /// Grabación de cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="reinsuranceCheckingAccountItems"></param>
        /// <returns>bool</returns>
        public bool SaveReinsuranceCheckingAccountItems(List<ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItems)
        {
            try
            {
                foreach (ReinsuranceCheckingAccountItemDTO tempReinsuranceCheckingAccountItem in reinsuranceCheckingAccountItems)
                {
                    _tempReinsuranceCheckingAccountTransactionItemDAO.SaveTempReinsuranceCheckingAccountItem(tempReinsuranceCheckingAccountItem.ToModel()).ToDTO();
                }
            }
            catch (BusinessException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// DeleteReinsuranceCheckingAccountItem
        /// Eliminación de cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="reinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteReinsuranceCheckingAccountItem(int tempImputationId, int reinsuranceCheckingAccountItemId)
        {
            bool isDeleted = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempImputationCode, tempImputationId);
                if (reinsuranceCheckingAccountItemId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempReinsCheckingAccTransCode, reinsuranceCheckingAccountItemId);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem.Properties.TempReinsCheckingAccTransCode, tempReinsuranceCheckingAccount.TempReinsCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new
                        BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                              (typeof(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempReinsuranceCheckingAccountItem tempReinsuranceCheckingAccountItemEntity in businessCollectionItems.OfType<ACCOUNTINGEN.TempReinsuranceCheckingAccountItem>())
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempReinsuranceCheckingAccountItemEntity);
                    }

                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountTransactionItem(tempReinsuranceCheckingAccount.TempReinsCheckingAccTransCode);
                }

                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        /// <summary>
        /// SaveReinsuranceCheckingAccount
        /// Registra cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransaction"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        public bool SaveReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionDTO reinsuranceCheckingAccountTransaction, int imputationId)
        {
            bool isSaved = false;

            try
            {
                foreach (ReInsuranceCheckingAccountTransactionItemDTO reinsuranceCheckingAccountTransactionItem in reinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems)
                {
                    ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItemNew;


                    reinsuranceCheckingAccountTransactionItemNew = reinsuranceCheckingAccountTransactionItem.Id == 0 ?
                        _tempReinsuranceCheckingAccountTransactionItemDAO.SaveTempReinsuranceCheckingAccountTransactionItem(reinsuranceCheckingAccountTransactionItem.ToModel(), imputationId, 0) :
                        _tempReinsuranceCheckingAccountTransactionItemDAO.UpdateTempReinsuranceCheckingAccount(reinsuranceCheckingAccountTransactionItem.ToModel(), imputationId, 0); // El valor 0 corresponede al campo tempReinsuranceParentCode pendiente averiguar

                    foreach (ReinsuranceCheckingAccountItemDTO reinsuranceCheckingAccountItemChild in reinsuranceCheckingAccountTransactionItem.ReinsurancesCheckingAccountItems)
                    {
                        reinsuranceCheckingAccountItemChild.TempReinsuranceCheckingAccountId = reinsuranceCheckingAccountTransactionItemNew.Id;

                        if (reinsuranceCheckingAccountItemChild.Id == 0)
                        {
                            _tempReinsuranceCheckingAccountTransactionItemDAO.SaveTempReinsuranceCheckingAccountItem(reinsuranceCheckingAccountItemChild.ToModel());
                        }
                        else
                        {
                            _tempReinsuranceCheckingAccountTransactionItemDAO.UpdateTempReinsuranceCheckingAccountItem(reinsuranceCheckingAccountItemChild.ToModel());
                        }
                    }

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
        /// DeleteReinsuranceCheckingAccountItemChild
        /// Elimina cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteReinsuranceCheckingAccountItemChild(int tempReinsuranceCheckingAccountItemId)
        {
            try
            {
                return _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItem(tempReinsuranceCheckingAccountItemId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempReinsuranceCheckingAccountTotal
        /// Actualiza temporal cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>ReInsuranceCheckingAccountTransaction</returns>
        public ReInsuranceCheckingAccountTransactionDTO UpdateTempReinsuranceCheckingAccountTotal(int tempReinsuranceCheckingAccountId, decimal selectedTotal)
        {
            try
            {
                return _tempReinsuranceCheckingAccountTransactionItemDAO.UpdateTempReinsuranceCheckingAccountTotal(tempReinsuranceCheckingAccountId, selectedTotal).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetTempReinsuranceCheckingAccountItemByTempImputationId
        /// Obtiene temporal cuenta de cheques de reaseguros por imputación
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.ReinsuranceCheckingAccountItemDTO> GetTempReinsuranceCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempImputationCode, tempImputationId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempReinsuranceParentCode, 0);

                UIView tempReinsuranceCheckingAccountItems = _dataFacadeManager.GetDataFacade().GetView("TempReinsuranceCheckAccountView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (tempReinsuranceCheckingAccountItems.Rows.Count > 0)
                {
                    tempReinsuranceCheckingAccountItems.Columns.Add("rows", typeof(int));
                    tempReinsuranceCheckingAccountItems.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItems = new List<SEARCH.ReinsuranceCheckingAccountItemDTO>();

                foreach (DataRow dataRow in tempReinsuranceCheckingAccountItems)
                {
                    reinsuranceCheckingAccountItems.Add(new SEARCH.ReinsuranceCheckingAccountItemDTO()
                    {
                        ReinsuranceCheckingAccountItemId = dataRow["TempReinsCheckingAccTransCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempReinsCheckingAccTransCode"]),
                        TempImputationId = dataRow["TempImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempImputationCode"]),
                        TempReinsuranceParentId = dataRow["TempReinsuranceParentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempReinsuranceParentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        PosCode = dataRow["SalePointCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SalePointCode"]),
                        PosName = dataRow["SalePointName"] == DBNull.Value ? "" : dataRow["SalePointName"].ToString(),
                        LineBusinessCode = dataRow["LineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["LineBusinessCode"]),
                        PrefixName = dataRow["LineBusinessName"] == DBNull.Value ? "" : dataRow["LineBusinessName"].ToString(),
                        SubLineBusinessCode = dataRow["SubLineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubLineBusinessCode"]),
                        SubPrefixName = dataRow["SubLineBusinessName"] == DBNull.Value ? "" : dataRow["SubLineBusinessName"].ToString(),
                        BrokerName = dataRow["BrokerName"] == DBNull.Value ? "" : dataRow["BrokerName"].ToString(),
                        ReinsurerName = dataRow["ReinsurerName"] == DBNull.Value ? "" : dataRow["ReinsurerName"].ToString(),
                        SlipNumber = dataRow["SlipNumber"] == DBNull.Value ? "" : dataRow["SlipNumber"].ToString(),
                        ContractTypeCode = dataRow["ContractTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ContractTypeCode"]),
                        Contract = dataRow["ContractCode"] == DBNull.Value ? "" : dataRow["ContractCode"].ToString(),
                        Stretch = dataRow["Section"] == DBNull.Value ? "" : dataRow["Section"].ToString(),
                        Region = dataRow["Region"] == DBNull.Value ? "" : dataRow["Region"].ToString(),
                        Excercise = dataRow["Period"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Period"]),
                        CheckingAccountConceptCode = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptName = dataRow["CheckingAccountConceptName"] == DBNull.Value ? "" : dataRow["CheckingAccountConceptName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["TinyDescription"].ToString(),
                        CurrencyChange = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        DebitCreditName = Convert.ToInt32(dataRow["AccountingNature"]) == 1 ? "Crédito" : "Débito",
                        AccountingNature = dataRow["AccountingNature"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNature"]),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                        YearMonthApplies = dataRow["YearMonth"] == DBNull.Value ? "" : dataRow["YearMonth"].ToString(),
                        PolicyEndorsement = dataRow["PolicyNumber"] == DBNull.Value ? "" : dataRow["PolicyNumber"].ToString(),
                        CompanyCode = dataRow["AccountingCompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingCompanyCode"]),
                        CompanyName = dataRow["AccountingCompanyName"] == DBNull.Value ? "" : dataRow["AccountingCompanyName"].ToString(),
                        AgentTypeCode = 1,
                        AgentCode = dataRow["AgentId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AgentId"]),
                        AgentAgencyCode = 1,
                        CollectNumber = 0,
                        IsFacultative = Convert.ToBoolean(dataRow["IsFacultative"]),
                        FacultativeCode = !Convert.ToBoolean(dataRow["IsFacultative"]) ? 1 : 2,
                        ReinsuranceCompanyCode = dataRow["ReinsuranceCompanyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsuranceCompanyId"]),
                        ApplicationMonth = dataRow["ApplicationMonth"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationMonth"]),
                        ApplicationYear = dataRow["ApplicationYear"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationYear"]),
                        ReinsurancePolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PolicyId"]),
                        ReinsuranceEndorsementId = dataRow["EndorsementId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["EndorsementId"]),
                        TransactionNumber = dataRow["TransactionNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TransactionNumber"]),
                        Status = 0,
                        DebitCreditCode = dataRow["AccountingNature"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNature"]),
                        Items = Convert.ToInt32(dataRow["TransactionNumber"]) == 0 ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Rows = rows
                    });
                }

                return reinsuranceCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (UnhandledException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempReinsuranceCheckingAccountItemChildByTempReinsuranceParentId
        /// Obtiene temporal cuenta de cheques de reaseguros por temporal reaseguros
        /// </summary>
        /// <param name="tempReinsuranceParentId"></param>
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.ReinsuranceCheckingAccountItemDTO> GetTempReinsuranceCheckingAccountItemChildByTempReinsuranceParentId(int tempReinsuranceParentId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsuranceCheckingAccountItem.Properties.TempReinsCheckingAccTransCode, tempReinsuranceParentId);

                UIView tempReinsuranceCheckingAccountItemChilds = _dataFacadeManager.GetDataFacade().GetView("TempReinsuranceCheckingAccountItemsView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (tempReinsuranceCheckingAccountItemChilds.Rows.Count > 0)
                {
                    tempReinsuranceCheckingAccountItemChilds.Columns.Add("rows", typeof(int));
                    tempReinsuranceCheckingAccountItemChilds.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItems = new List<SEARCH.ReinsuranceCheckingAccountItemDTO>();

                foreach (DataRow dataRow in tempReinsuranceCheckingAccountItemChilds)
                {
                    reinsuranceCheckingAccountItems.Add(new SEARCH.ReinsuranceCheckingAccountItemDTO()
                    {
                        ReinsuranceCheckingAccountId = dataRow["TempReinsuranceCheckingAccountCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempReinsuranceCheckingAccountCode"]),
                        ReinsuranceCheckingAccountItemId = dataRow["ReinsuranceCheckingAccountCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsuranceCheckingAccountCode"]),
                        ImputationId = dataRow["ImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ImputationCode"]),
                        ReinsuranceParentId = dataRow["ReinsuranceParentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsuranceParentCode"]),
                        ReinsuranceCheckingAccountItemChildId = Convert.ToInt32(dataRow["TempReinsuranceCheckingAccountItemCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        PosCode = dataRow["SalePointCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SalePointCode"]),
                        PosName = dataRow["SalePointName"] == DBNull.Value ? "" : dataRow["SalePointName"].ToString(),
                        LineBusinessCode = dataRow["LineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["LineBusinessCode"]),
                        PrefixName = dataRow["LineBusinessName"] == DBNull.Value ? "" : dataRow["LineBusinessName"].ToString(),
                        SubLineBusinessCode = dataRow["SubLineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubLineBusinessCode"]),
                        SubPrefixName = dataRow["SubLineBusinessName"] == DBNull.Value ? "" : dataRow["SubLineBusinessName"].ToString(),
                        BrokerName = dataRow["BrokerName"] == DBNull.Value ? "" : dataRow["BrokerName"].ToString(),
                        ReinsurerName = dataRow["ReinsurerName"] == DBNull.Value ? "" : dataRow["ReinsurerName"].ToString(),
                        SlipNumber = dataRow["SlipNumber"] == DBNull.Value ? "" : dataRow["SlipNumber"].ToString(),
                        ContractTypeCode = dataRow["ContractTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ContractTypeCode"]),
                        Contract = dataRow["ContractCode"] == DBNull.Value ? "" : dataRow["ContractCode"].ToString(),
                        Stretch = dataRow["Section"] == DBNull.Value ? "" : dataRow["Section"].ToString(),
                        Region = dataRow["Region"] == DBNull.Value ? "" : dataRow["Region"].ToString(),
                        Excercise = dataRow["Period"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Period"]),
                        CheckingAccountConceptCode = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptName = dataRow["CheckingAccountConceptName"] == DBNull.Value ? "" : dataRow["CheckingAccountConceptName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["TinyDescription"].ToString(),
                        CurrencyChange = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        DebitCreditName = Convert.ToInt32(dataRow["AccountingNature"]) == 1 ? "Crédito" : "Débito",
                        AccountingNature = dataRow["AccountingNature"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNature"]),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                        YearMonthApplies = dataRow["YearMonth"] == DBNull.Value ? "" : dataRow["YearMonth"].ToString(),
                        PolicyEndorsement = dataRow["PolicyNumber"] == DBNull.Value ? "" : dataRow["PolicyNumber"].ToString(),
                        CompanyCode = dataRow["AccountingCompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingCompanyCode"]),
                        CompanyName = dataRow["AccountingCompanyName"] == DBNull.Value ? "" : dataRow["AccountingCompanyName"].ToString(),
                        AgentTypeCode = 1,
                        AgentCode = dataRow["AgentId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AgentId"]),
                        AgentAgencyCode = 1,
                        CollectNumber = 0,
                        FacultativeCode = dataRow["ContractTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ContractTypeCode"]),
                        ReinsuranceCompanyCode = dataRow["ReinsuranceCompanyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReinsuranceCompanyId"]),
                        ApplicationMonth = dataRow["ApplicationMonth"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationMonth"]),
                        ApplicationYear = dataRow["ApplicationYear"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationYear"]),
                        ReinsurancePolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PolicyId"]),
                        ReinsuranceEndorsementId = dataRow["EndorsementId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["EndorsementId"]),
                        TransactionNumber = dataRow["TransactionNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TransactionNumber"]),
                        Status = 0,
                        DebitCreditCode = dataRow["AccountingNature"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNature"]),
                        Items = Convert.ToInt32(dataRow["TransactionNumber"]) == 0 ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Rows = rows
                    });
                }

                return reinsuranceCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateInsurancePolicyEndorsement
        /// Valida póliza, seguros y endoso
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <returns>int</returns>
        public int ValidateInsurancePolicyEndorsement(string policyNumber, int endorsementNumber, int branchId, int prefixId)
        {
            int validateEndorsement = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssuePolicyEndorsement.Properties.PolicyDocumentNumber, policyNumber);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssuePolicyEndorsement.Properties.PrefixCode, prefixId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssuePolicyEndorsement.Properties.BranchCode, branchId);

                UIView policyEndorsements = _dataFacadeManager.GetDataFacade().GetView("InsurancePolicyEndorsementView",
                                                                criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);
                if (policyEndorsements.Count > 0)
                {
                    var filteredPolicyEndorsements = (from DataRow dataRow in policyEndorsements.AsEnumerable() where Convert.ToInt32(dataRow["EndorsementDocumentNumber"]) == endorsementNumber select dataRow).ToList();

                    if (filteredPolicyEndorsements.Count > 0)
                    {
                        validateEndorsement = 1;
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return validateEndorsement;
        }

        /// <summary>
        /// ValidateDuplicateReinsuranceCheckingAccount
        /// Permite validar el ingreso de duplicados tanto en temporales como en reales 
        /// de cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO ValidateDuplicateReinsuranceCheckingAccount(SEARCH.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter)
        {
            Imputation imputation = new Imputation()
            {
                VerificationValue = new Amount() { Value = -1 }
            };

            try
            {
                // Se verifica en la tabla temporal
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.BranchCode, validateParameter.Branch.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.SalePointCode, validateParameter.SalePoint.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.AccountingCompanyCode, validateParameter.Company.IndividualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.AccountingNature, validateParameter.AccountingNatureId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.CurrencyCode, validateParameter.Currency.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.ReinsuranceCompanyId, validateParameter.ReinsuranceId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.CheckingAccountConceptCode, validateParameter.CheckingAccountConceptId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.AgentId, validateParameter.AgentId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.LineBusinessCode, validateParameter.Prefix.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.SubLineBusinessCode, validateParameter.SubPrefix.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.ContractTypeCode, validateParameter.ContractTypeId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.ContractCode, Convert.ToInt32(validateParameter.ContractNumber)); // No es ContractNumber es ContractId
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.Section, Convert.ToString(validateParameter.StretchId));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.ApplicationYear, validateParameter.ApplicationYear);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.ApplicationMonth, validateParameter.ApplicationMonth);

                BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                if (businessCollectionItems.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccount in businessCollectionItems.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                    {
                        imputation.VerificationValue.Value = 0;
                        imputation.Id = (int)tempReinsuranceCheckingAccount.TempImputationCode;
                    }
                }

                if (imputation.Id <= 0)
                {
                    // Se verifica en la tabla real
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.BranchCode, validateParameter.Branch.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.SalePointCode, validateParameter.SalePoint.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.AccountingCompanyCode, validateParameter.Company.IndividualId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.AccountingNature, validateParameter.AccountingNatureId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.CurrencyCode, validateParameter.Currency.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ReinsuranceCompanyId, validateParameter.ReinsuranceId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.CheckingAccountConceptCode, validateParameter.CheckingAccountConceptId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.AgentId, validateParameter.AgentId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.LineBusinessCode, validateParameter.Prefix.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.SubLineBusinessCode, validateParameter.SubPrefix.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ContractTypeCode, validateParameter.ContractTypeId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ContractCode, Convert.ToInt32(validateParameter.ContractNumber)); // No es ContractNumber es ContractId
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.Section, Convert.ToString(validateParameter.StretchId));
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ApplicationYear, validateParameter.ApplicationYear);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ReinsCheckingAccTrans.Properties.ApplicationMonth, validateParameter.ApplicationMonth);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.ReinsCheckingAccTrans reinsuranceCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.ReinsCheckingAccTrans>())
                        {
                            imputation.VerificationValue.Value = 1;
                            imputation.Id = (int)reinsuranceCheckingAccount.ImputationCode;
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return imputation.ToDTO();
        }

        #endregion

        //Contabilidad
        #region Accounting

        /// <summary>
        /// SaveTempAccountingTransaction
        /// Graba un temporal de transacción contable
        /// </summary>
        /// <param name="tempDailyAccountingTransaction"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="accountingConceptId"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        public int SaveTempAccountingTransaction(DailyAccountingTransactionDTO tempDailyAccountingTransaction, int tempImputationId, int accountingConceptId, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            int saved = 0;

            try
            {
                foreach (DailyAccountingTransactionItemDTO tempDailyAccountingTransactionItem in tempDailyAccountingTransaction.DailyAccountingTransactionItems)
                {
                    if (tempDailyAccountingTransactionItem.Id == 0)
                    {
                        saved = _tempDailyAccountingTransactionItemDAO.SaveTempDailyAccountingTransactionItem(tempDailyAccountingTransactionItem.ToModel(), tempImputationId, accountingConceptId, tempDailyAccountingTransaction.Description, bankReconciliationId, receiptNumber, receiptDate, postdatedAmount);

                        //graba códigos de analisis
                        if (tempDailyAccountingTransactionItem.DailyAccountingAnalysisCodes.Count > 0)
                        {
                            foreach (DailyAccountingAnalysisCodeDTO tempDailyAccountingAnalysisCode in tempDailyAccountingTransactionItem.DailyAccountingAnalysisCodes)
                            {
                                _tempDailyAccountingAnalysisCodeDAO.SaveTempDailyAccountingAnalysisCode(tempDailyAccountingAnalysisCode.ToModel(), saved);
                            }
                        }

                        //graba centros de costos
                        if (tempDailyAccountingTransactionItem.DailyAccountingCostCenters.Count > 0)
                        {
                            foreach (DailyAccountingCostCenterDTO tempDailyAccountingCostCenter in tempDailyAccountingTransactionItem.DailyAccountingCostCenters)
                            {
                                _tempDailyAccountingCostCenterDAO.SaveTempDailyAccountingCostCenter(tempDailyAccountingCostCenter.ToModel(), saved);
                            }
                        }
                    }
                    else
                    {
                        saved = _tempDailyAccountingTransactionItemDAO.UpdateTempDailyAccountingTransactionItem(tempDailyAccountingTransactionItem.ToModel(), tempImputationId, accountingConceptId, tempDailyAccountingTransaction.Description, bankReconciliationId, receiptNumber, receiptDate, postdatedAmount);

                        //analisis
                        //se eliminan los registros previos.
                        List<DailyAccountingAnalysisCodeDTO> tempAnalysisCodes = GetTempDailyAccountingAnalysisCodesByTempDailyAccountingTransactionItemId(saved);

                        if (tempAnalysisCodes.Count > 0)
                        {
                            foreach (DailyAccountingAnalysisCodeDTO tempDailyAccountingAnalysis in tempAnalysisCodes)
                            {
                                _tempDailyAccountingAnalysisCodeDAO.DeleteTempDailyAccountingAnalysisCode(tempDailyAccountingAnalysis.Id);
                            }
                        }

                        //se graban los nuevos registros
                        if (tempDailyAccountingTransactionItem.DailyAccountingAnalysisCodes.Count > 0)
                        {
                            foreach (DailyAccountingAnalysisCodeDTO tempDailyAccountingAnalysisCode in tempDailyAccountingTransactionItem.DailyAccountingAnalysisCodes)
                            {
                                _tempDailyAccountingAnalysisCodeDAO.SaveTempDailyAccountingAnalysisCode(tempDailyAccountingAnalysisCode.ToModel(), saved);
                            }
                        }

                        //centros de costos
                        //se eliminan los registros previos
                        List<DailyAccountingCostCenterDTO> tempCostCenters = GetTempDailyAccountingCostCentersByTempDailyAccountingTransactionItemId(saved);

                        if (tempCostCenters.Count > 0)
                        {
                            foreach (DailyAccountingCostCenterDTO tempDailyAccountingCostCenter in tempCostCenters)
                            {
                                _tempDailyAccountingCostCenterDAO.DeleteTempDailyAccountingCostCenter(tempDailyAccountingCostCenter.Id);
                            }
                        }

                        //se graban los nuevos registros.
                        if (tempDailyAccountingTransactionItem.DailyAccountingCostCenters.Count > 0)
                        {
                            foreach (DailyAccountingCostCenterDTO tempDailyAccountingCostCenter in tempDailyAccountingTransactionItem.DailyAccountingCostCenters)
                            {
                                _tempDailyAccountingCostCenterDAO.SaveTempDailyAccountingCostCenter(tempDailyAccountingCostCenter.ToModel(), saved);
                            }
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return saved;
        }

        /// <summary>
        /// DeleteTempDailyAccountingTransaction
        /// Elimina un temporal de transacción contable
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempDailyAccountingTransactionItem(int tempDailyAccountingTransactionItemId)
        {

            try
            {

                //se eliminan los analisis
                List<DailyAccountingAnalysisCode> tempAnalysisCodes = GetTempDailyAccountingAnalysisCodesByTempDailyAccountingTransactionItemId(tempDailyAccountingTransactionItemId).ToModels().ToList();

                if (tempAnalysisCodes.Count > 0)
                {
                    foreach (DailyAccountingAnalysisCode tempDailyAccountingAnalysis in tempAnalysisCodes)
                    {
                        _tempDailyAccountingAnalysisCodeDAO.DeleteTempDailyAccountingAnalysisCode(tempDailyAccountingAnalysis.Id);
                    }
                }

                //se eliminan los centros de costos
                List<DailyAccountingCostCenter> tempCostCenters = GetTempDailyAccountingCostCentersByTempDailyAccountingTransactionItemId(tempDailyAccountingTransactionItemId).ToModels().ToList();

                if (tempCostCenters.Count > 0)
                {
                    foreach (DailyAccountingCostCenter tempDailyAccountingCostCenter in tempCostCenters)
                    {
                        _tempDailyAccountingCostCenterDAO.DeleteTempDailyAccountingCostCenter(tempDailyAccountingCostCenter.Id);
                    }
                }

                return _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItem(tempDailyAccountingTransactionItemId);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetTempAccountingTransactionItemByTempImputationId
        /// Obtiene un temporal de transacción contable dada la imputación
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<TempDailyAccountingDTO/></returns>
        public List<SEARCH.TempDailyAccountingDTO> GetTempAccountingTransactionItemByTempImputationId(int tempImputationId)
        {
            try
            {
                List<SEARCH.TempDailyAccountingDTO> tempDailyAccountings = new List<SEARCH.TempDailyAccountingDTO>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.TempImputationCode, tempImputationId);

                //UIView dailyAccountings = _dataFacadeManager.GetDataFacade().GetView("TempDailyAccountingTransView",
                //                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<ACCOUNTINGEN.GetTempDailyAccoutingV> dailyAccountings = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.GetTempDailyAccoutingV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetTempDailyAccoutingV>().ToList();

                int rows = 0;
                if (dailyAccountings.Count > 0)
                {
                    rows = dailyAccountings.Count;
                }

                // Load DTO
                foreach (ACCOUNTINGEN.GetTempDailyAccoutingV dailyAccounting in dailyAccountings)
                {
                    string paymentConceptCode = "";
                    string paymentConceptDescription = "";

                    if (Convert.ToInt32(dailyAccounting.PaymentConceptCode) > 0)
                    {
                        criteriaBuilder = new ObjectCriteriaBuilder();

                        criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.AccountingConceptCode,
                                                Convert.ToInt32(dailyAccounting.PaymentConceptCode));
                        BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                            typeof(GENERALLEDGEREN.AccountingConcept), criteriaBuilder.GetPredicate()));

                        foreach (GENERALLEDGEREN.AccountingConcept paymentConcept in businessCollection.OfType<GENERALLEDGEREN.AccountingConcept>())
                        {
                            paymentConceptCode = Convert.ToString(paymentConcept.AccountingConceptCode);
                            paymentConceptDescription = paymentConcept.Description;
                        }
                    }

                    tempDailyAccountings.Add(new SEARCH.TempDailyAccountingDTO()
                    {
                        TempDailyAccountingId = Convert.ToInt32(dailyAccounting.TempDailyAccountingTransId),
                        TempImputationId = Convert.ToInt32(dailyAccounting.TempImputationCode),
                        BranchId = Convert.ToInt32(dailyAccounting.BranchCode),
                        Branch = Convert.ToString(dailyAccounting.BranchDescription),
                        SalePointId = Convert.ToInt32(dailyAccounting.SalePointCode),
                        SalePoint = Convert.ToString(dailyAccounting.SalePointDescription),
                        CompanyId = Convert.ToInt32(dailyAccounting.CompanyCode),
                        Company = Convert.ToString(dailyAccounting.AccountingCompanyDescription),
                        ConceptId = paymentConceptCode,
                        ConceptDescription = paymentConceptDescription,
                        ConceptCodeDescription = paymentConceptCode + " - " + paymentConceptDescription, //campo compuesto
                        AccountId = Convert.ToInt32(dailyAccounting.BookAccountCode),
                        AccountNumber = Convert.ToString(dailyAccounting.AccountingNumber),
                        AccountName = Convert.ToString(dailyAccounting.AccountingName),
                        AccountingNumberAccount = Convert.ToString(dailyAccounting.AccountingNumber) + " - " + Convert.ToString(dailyAccounting.AccountingName),//campo compuesto
                        NatureId = Convert.ToInt32(dailyAccounting.AccountingNatureCode),
                        Nature = Resources.Resources.ResourceManager.GetString(EnumHelper.GetEnumDescription((AccountingNature)Convert.ToInt32(dailyAccounting.AccountingNatureCode))),
                        CurrencyId = Convert.ToInt32(dailyAccounting.CurrencyCode),
                        Currency = Convert.ToString(dailyAccounting.CurrencyDescription),
                        IncomeAmount = Convert.ToDecimal(dailyAccounting.IncomeAmount),
                        Exchange = Convert.ToDecimal(dailyAccounting.ExchangeRate),
                        Amount = Convert.ToDecimal(dailyAccounting.Amount),
                        Description = Convert.ToString(dailyAccounting.Description),
                        BankReconciliationId = Convert.ToInt32(dailyAccounting.BankReconciliationId),
                        BankReconciliation = "",
                        ReceiptNumber = Convert.ToString(dailyAccounting.ReceiptNumber),
                        ReceiptDate = Convert.ToBoolean(dailyAccounting.ReceiptDate) ? " " : String.Format("{0:dd/MM/yyyy}", dailyAccounting.ReceiptDate),
                        PostdatedAmount = Convert.ToDecimal(dailyAccounting.PostdatedAmount),
                        BeneficiaryId = Convert.ToInt32(dailyAccounting.BeneficiaryId),
                        BeneficiaryDocumentNumber = Convert.ToString(dailyAccounting.Beneficiarydocumentnumber),
                        BeneficiaryName = Convert.ToString(dailyAccounting.Beneficiaryname),
                        BeneficiaryNameDocumentNumber = Convert.ToString(dailyAccounting.Beneficiarydocumentnumber) + " - " + Convert.ToString(dailyAccounting.Beneficiaryname), //campo compuesto
                        Rows = rows
                    });
                }

                return tempDailyAccountings;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// ConvertTempDailyAccountingToDailyDailyAccounting
        /// Convierte un temporal de transacción contable dada la imputación
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        public bool ConvertTempDailyAccountingToDailyDailyAccounting(int sourceId, int imputationTypeId, int imputationId)
        {
            bool isConverted = false;

            try
            {
                // Obtengo el temporal.
                Imputation tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ImputationItems = new List<TransactionType>();
                DailyAccountingTransaction tempDailyAccountingTransaction = _tempDailyAccountingTransactionDAO.GetTempDailyAccountingTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ImputationItems.Add(tempDailyAccountingTransaction);

                Imputation imputation = new Imputation()
                {
                    Date = DateTime.Now,
                    UserId = tempImputation.UserId
                };

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.TempImputationCode, tempImputation.Id);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempDailyAccountingTrans), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccounting in businessCollection.OfType<ACCOUNTINGEN.TempDailyAccountingTrans>())
                    {
                        int dailyAccountingTransactionItemId = 0;
                        DateTime? receiptDate = null;

                        // Controla cuando la fecha del recibo es nula
                        if (Convert.ToDateTime(tempDailyAccounting.ReceiptDate) == Convert.ToDateTime("01/01/0001 0:00:00"))
                        {
                            receiptDate = null;
                        }
                        else
                        {
                            receiptDate = Convert.ToDateTime(tempDailyAccounting.ReceiptDate);
                        }

                        DailyAccountingTransactionItem dailyAccountingTransactionItem = new DailyAccountingTransactionItem();

                        dailyAccountingTransactionItem.Id = dailyAccountingTransactionItemId;
                        dailyAccountingTransactionItem.Branch = new Branch();
                        dailyAccountingTransactionItem.Branch.Id = Convert.ToInt32(tempDailyAccounting.BranchCode);
                        dailyAccountingTransactionItem.SalePoint = new SalePoint();
                        dailyAccountingTransactionItem.SalePoint.Id = Convert.ToInt32(tempDailyAccounting.SalePointCode);
                        dailyAccountingTransactionItem.Company = new Company();
                        dailyAccountingTransactionItem.Company.IndividualId = Convert.ToInt32(tempDailyAccounting.CompanyCode);
                        dailyAccountingTransactionItem.Beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempDailyAccounting.BeneficiaryId) };
                        dailyAccountingTransactionItem.AccountingNature = new AccountingNature();
                        dailyAccountingTransactionItem.AccountingNature = (AccountingNature)tempDailyAccounting.AccountingNature;
                        dailyAccountingTransactionItem.Amount = new Amount();
                        dailyAccountingTransactionItem.Amount.Currency = new Currency();
                        dailyAccountingTransactionItem.Amount.Currency.Id = Convert.ToInt32(tempDailyAccounting.CurrencyCode);
                        dailyAccountingTransactionItem.Amount.Value = Convert.ToDecimal(tempDailyAccounting.IncomeAmount);
                        dailyAccountingTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempDailyAccounting.ExchangeRate) };
                        dailyAccountingTransactionItem.LocalAmount = new Amount() { Value = Convert.ToDecimal(tempDailyAccounting.Amount) };
                        dailyAccountingTransactionItem.BookAccount = new BookAccount();
                        dailyAccountingTransactionItem.BookAccount.Id = Convert.ToInt32(tempDailyAccounting.BookAccountCode);
                        dailyAccountingTransactionItem.DailyAccountingAnalysisCodes = GetTempDailyAccountingAnalysisCodesByTempDailyAccountingTransactionItemId(tempDailyAccounting.TempDailyAccountingTransId).ToModels().ToList();
                        dailyAccountingTransactionItem.DailyAccountingCostCenters = GetTempDailyAccountingCostCentersByTempDailyAccountingTransactionItemId(tempDailyAccounting.TempDailyAccountingTransId).ToModels().ToList();

                        dailyAccountingTransactionItemId = SaveDailyAccountingTransactionItem(dailyAccountingTransactionItem.ToDTO(), imputationId, Convert.ToInt32(tempDailyAccounting.PaymentConceptCode), tempDailyAccounting.Description, Convert.ToInt32(tempDailyAccounting.BankReconciliationId), Convert.ToInt32(tempDailyAccounting.ReceiptNumber), receiptDate, Convert.ToDecimal(tempDailyAccounting.PostdatedAmount));

                        if (dailyAccountingTransactionItemId > 0)
                        {
                            isConverted = true;
                        }

                        // Si graba la transacción, borra el temporal
                        if (isConverted)
                        {
                            _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputation.Id);
                        }
                        else
                        {
                            ObjectCriteriaBuilder filterDaily = new ObjectCriteriaBuilder();
                            filter.PropertyEquals(ACCOUNTINGEN.DailyAccountingTrans.Properties.ImputationCode, imputationId);
                            BusinessCollection businessCollectionDaily = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(ACCOUNTINGEN.DailyAccountingTrans), filterDaily.GetPredicate()));

                            foreach (ACCOUNTINGEN.DailyAccountingTrans dailyAccounting in businessCollectionDaily.OfType<ACCOUNTINGEN.DailyAccountingTrans>())
                            {
                                _dailyAccountingTransactionItemDAO.DeleteDailyAccountingTransactionItem(dailyAccounting.DailyAccountingTransId);
                            }
                        }
                    }
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// GetAccountingAccountByDescription
        /// Obtiene cuentas contables por descripción
        /// </summary>
        /// <param name="accountingAccountDescription"></param>
        /// <param name="branchId"></param>
        /// <returns>List<AccountingAccountDTO/></returns>
        public List<SEARCH.AccountingAccountDTO> GetAccountingAccountByDescription(string accountingAccountDescription, int branchId)
        {
            List<SEARCH.AccountingAccountDTO> accountingAccounts = new List<SEARCH.AccountingAccountDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (branchId != -1)
                {
                    // Si existe el branch especificado.
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.DefaultBranchCode, branchId);
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountName);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccountDescription + "%");

                    // Se debe justificar el por qué de éste filtro ...
                    // activo para contabilidad diaria
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.IsOfficialNomenclature, 0);
                    // No es nomeclatura oficial

                    BusinessCollection businessCollection =
                        new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                        {
                            accountingAccounts.Add(new SEARCH.AccountingAccountDTO()
                            {
                                AccountingAccountId = accountingAccount.AccountingAccountId,
                                AccountingNumber = accountingAccount.AccountNumber,
                                AccountingName = accountingAccount.AccountName,
                                IsMulticurrency = Convert.ToInt32(accountingAccount.IsMulticurrency),
                                DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode)
                            });
                        }
                    }
                    else
                    {
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountName);
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(accountingAccountDescription + "%");
                        // Activo para contabilidad diaria
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.IsOfficialNomenclature, 0);
                        // No es nomeclatura oficial

                        BusinessCollection businessCollectionAccounting =
                            new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                        if (businessCollectionAccounting.Count > 0)
                        {
                            foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollectionAccounting.OfType<GENERALLEDGEREN.AccountingAccount>())
                            {
                                accountingAccounts.Add(new SEARCH.AccountingAccountDTO()
                                {
                                    AccountingAccountId = accountingAccount.AccountingAccountId,
                                    AccountingNumber = accountingAccount.AccountNumber,
                                    AccountingName = accountingAccount.AccountName,
                                    IsMulticurrency = Convert.ToInt32(accountingAccount.IsMulticurrency),
                                    DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode)
                                });
                            }
                        }
                    }
                }
                else
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountName);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccountDescription + "%");

                    BusinessCollection businessCollectionAccountingAccount =
                        new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                            typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                    if (businessCollectionAccountingAccount.Count > 0)
                    {
                        foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollectionAccountingAccount.OfType<GENERALLEDGEREN.AccountingAccount>())
                        {
                            accountingAccounts.Add(new SEARCH.AccountingAccountDTO()
                            {
                                AccountingAccountId = accountingAccount.AccountingAccountId,
                                AccountingNumber = accountingAccount.AccountNumber,
                                AccountingName = accountingAccount.AccountName,
                                IsMulticurrency = Convert.ToInt32(accountingAccount.IsMulticurrency),
                                DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode)
                            });
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingAccounts;
        }

        /// <summary>
        /// GetAccountingAccountByNumber
        /// Obtiene cuentas contables por número 
        /// </summary>
        /// <param name="accountingAccountNumber"></param>
        /// <param name="branchId"></param>
        /// <returns>List<AccountingAccountDTO/></returns>
        public List<SEARCH.AccountingAccountDTO> GetAccountingAccountByNumber(string accountingAccountNumber, int branchId)
        {
            List<SEARCH.AccountingAccountDTO> accountingAccounts = new List<SEARCH.AccountingAccountDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (branchId != -1)
                {
                    // Si existe el branch especificado.
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.DefaultBranchCode, branchId);

                    if (accountingAccountNumber != "*")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber);
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(accountingAccountNumber + "%");
                        // Activo para contabilidad diaria
                    }

                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.IsOfficialNomenclature, 0);

                    BusinessCollection businessCollection =
                        new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                        {
                            accountingAccounts.Add(new SEARCH.AccountingAccountDTO()
                            {
                                AccountingAccountId = accountingAccount.AccountingAccountId,
                                AccountingNumber = accountingAccount.AccountNumber,
                                AccountingName = accountingAccount.AccountName,
                                IsMulticurrency = Convert.ToInt32(accountingAccount.IsMulticurrency),
                                DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode)
                            });
                        }
                    }
                    else
                    {
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber);
                        criteriaBuilder.Like();
                        criteriaBuilder.Constant(accountingAccountNumber + "%");
                        // Activo para contabilidad diaria
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.IsOfficialNomenclature, 0);
                        // No es nomeclatura oficial

                        businessCollection =
                            new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                        if (businessCollection.Count > 0)
                        {
                            foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                            {
                                accountingAccounts.Add(new SEARCH.AccountingAccountDTO()
                                {
                                    AccountingAccountId = accountingAccount.AccountingAccountId,
                                    AccountingNumber = accountingAccount.AccountNumber,
                                    AccountingName = accountingAccount.AccountName,
                                    IsMulticurrency = Convert.ToInt32(accountingAccount.IsMulticurrency),
                                    DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode)
                                });
                            }
                        }
                    }
                }
                else
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccountNumber + "%");

                    BusinessCollection businessCollection =
                        new BusinessCollection(
                            _dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                        {
                            accountingAccounts.Add(new SEARCH.AccountingAccountDTO()
                            {
                                AccountingAccountId = accountingAccount.AccountingAccountId,
                                AccountingNumber = accountingAccount.AccountNumber,
                                AccountingName = accountingAccount.AccountName,
                                IsMulticurrency = Convert.ToInt32(accountingAccount.IsMulticurrency),
                                DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode)
                            });
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingAccounts;
        }


        #endregion

        //Cuenta Corriente Coaseguros
        #region CurrentAccountCoinsurance

        /// <summary>
        /// SaveCoinsuranceCheckingAccountItems
        /// Graba cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="coInsuranceCheckingAccountItems"></param>
        /// <returns>bool</returns>
        public bool SaveCoinsuranceCheckingAccountItems(List<CoInsuranceCheckingAccountItemDTO> coInsuranceCheckingAccountItems)
        {
            try
            {
                foreach (CoInsuranceCheckingAccountItemDTO tempCoinsuranceCheckingAccountItem in coInsuranceCheckingAccountItems)
                {
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.SaveTempCoinsuranceCheckingAccountItem(tempCoinsuranceCheckingAccountItem.ToModel());
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// DeleteCoinsuranceCheckingAccountItem
        /// Actualiza cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="coinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteCoinsuranceCheckingAccountItem(int tempImputationId, int coinsuranceCheckingAccountItemId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempImputationCode, tempImputationId);
                if (coinsuranceCheckingAccountItemId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempCoinsCheckingAccTransCode, coinsuranceCheckingAccountItemId);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem.Properties.TempCoinsCheckingAccTransCode, tempCoinsuranceCheckingAccount.TempCoinsCheckingAccTransCode);

                    BusinessCollection businessCollectionItems = new
                        BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                              (typeof(ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem tempCoinsuranceCheckingAccountItemEntity in businessCollectionItems.OfType<ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem>())
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempCoinsuranceCheckingAccountItemEntity);
                    }

                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountTransactionItem(tempCoinsuranceCheckingAccount.TempCoinsCheckingAccTransCode);
                }

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// SaveCoinsuranceCheckingAccount
        /// Graba cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransaction"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        public bool SaveCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionDTO coinsuranceCheckingAccountTransaction, int imputationId)
        {
            bool isSaved = false;

            try
            {
                foreach (CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItem in coinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems)
                {
                    CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItemNew;

                    coinsuranceCheckingAccountTransactionItemNew = coinsuranceCheckingAccountTransactionItem.Id == 0 ? _tempCoinsuranceCheckingAccountTransactionItemDAO.SaveTempCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem.ToModel(), imputationId, 0).ToDTO() : _tempCoinsuranceCheckingAccountTransactionItemDAO.UpdateTempCoinsuranceCheckingAccount(coinsuranceCheckingAccountTransactionItem.ToModel(), imputationId, 0).ToDTO();

                    foreach (CoInsuranceCheckingAccountItemDTO coinsuranceCheckingAccountItemChild in coinsuranceCheckingAccountTransactionItem.CoinsurancesCheckingAccountItems)
                    {
                        coinsuranceCheckingAccountItemChild.TempCoinsuranceCheckingAccountId = coinsuranceCheckingAccountTransactionItemNew.Id;

                        if (coinsuranceCheckingAccountItemChild.Id == 0)
                        {

                            _tempCoinsuranceCheckingAccountTransactionItemDAO.SaveTempCoinsuranceCheckingAccountItem(coinsuranceCheckingAccountItemChild.ToModel()).ToDTO();
                        }
                        else
                        {
                            _tempCoinsuranceCheckingAccountTransactionItemDAO.UpdateTempCoinsuranceCheckingAccountItem(coinsuranceCheckingAccountItemChild.ToModel()).ToDTO();
                        }
                    }
                }

                isSaved = true;
            }
            catch (BusinessException)
            {
                isSaved = false;
            }

            return isSaved;
        }

        /// <summary>
        /// DeleteCoinsuranceCheckingAccountItemChild
        /// Elimina cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteCoinsuranceCheckingAccountItemChild(int tempCoinsuranceCheckingAccountItemId)
        {
            try
            {
                return _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItem(tempCoinsuranceCheckingAccountItemId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempCoinsuranceCheckingAccountTotal
        /// Actualiza cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>CoInsuranceCheckingAccountTransaction</returns>
        public CoInsuranceCheckingAccountTransactionDTO UpdateTempCoinsuranceCheckingAccountTotal(int tempCoinsuranceCheckingAccountId, decimal selectedTotal)
        {
            try
            {
                return _tempCoinsuranceCheckingAccountTransactionItemDAO.UpdateTempCoinsuranceCheckingAccountTotal(tempCoinsuranceCheckingAccountId, selectedTotal).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetTempCoinsuranceCheckingAccountItemByTempImputationId
        /// Obtiene temporal de cuenta de cheques de coaseguros dada la imputación
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.CoinsuranceCheckingAccountItemDTO> GetTempCoinsuranceCheckingAccountItemByTempImputationId(int tempImputationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempImputationCode, tempImputationId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempCoinsuranceParentCode, 0);

                UIView tempCoinsurancerCheckingAccountItems = _dataFacadeManager.GetDataFacade().GetView("TempCoinsuranceCheckingAccountingView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (tempCoinsurancerCheckingAccountItems.Rows.Count > 0)
                {
                    tempCoinsurancerCheckingAccountItems.Columns.Add("rows", typeof(int));
                    tempCoinsurancerCheckingAccountItems.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.CoinsuranceCheckingAccountItemDTO> coinsuranceCheckingAccountItems = new List<SEARCH.CoinsuranceCheckingAccountItemDTO>();

                foreach (DataRow dataRow in tempCoinsurancerCheckingAccountItems)
                {
                    coinsuranceCheckingAccountItems.Add(new SEARCH.CoinsuranceCheckingAccountItemDTO()
                    {
                        CoinsuranceCheckingAccountItemId = dataRow["TempCoinsCheckingAccTransCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempCoinsCheckingAccTransCode"]),
                        TempImputationId = dataRow["TempImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempImputationCode"]),
                        TempCoinsuranceParentId = dataRow["TempCoinsuranceParentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempCoinsuranceParentCode"]),
                        CoinsuranceTypeName = Convert.ToInt32(dataRow["CoinsuranceType"]) == 1 ? "Aceptado" : "Cedido",
                        CoinsuranceType = dataRow["CoinsuranceType"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuranceType"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        PosCode = dataRow["SalePointCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SalePointCode"]),
                        PosName = dataRow["SalePointName"] == DBNull.Value ? "" : dataRow["SalePointName"].ToString(),
                        CoinsurerName = dataRow["CoinsurerName"] == DBNull.Value ? "" : dataRow["CoinsurerName"].ToString(),
                        CheckingAccountConceptCode = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptName = dataRow["CheckingAccountConceptName"] == DBNull.Value ? "" : dataRow["CheckingAccountConceptName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["TinyDescription"].ToString(),
                        CurrencyChange = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        DebitCreditName = Convert.ToInt32(dataRow["AccountingNatureCode"]) == 1 ? "Crédito" : "Débito",
                        AccountingNature = dataRow["AccountingNatureCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNatureCode"]),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                        CompanyCode = dataRow["AccountingCompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingCompanyCode"]),
                        CompanyName = dataRow["AccountingCompanyName"] == DBNull.Value ? "" : dataRow["AccountingCompanyName"].ToString(),
                        AgentAgencyCode = 1,
                        CollectNumber = 0,
                        CoinsuranceCompanyCode = dataRow["CoinsuredCompanyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuredCompanyId"]),
                        Status = 0,
                        DebitCreditCode = dataRow["AccountingNatureCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNatureCode"]),
                        ItemsEnabled = dataRow["ItemEnabled"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ItemEnabled"]),
                        Rows = rows
                    });
                }

                return coinsuranceCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempCoinsuranceCheckingAccountItemChildByTempCoinsuranceParentId
        /// Obtiene temporal de cuenta de cheques de coaseguros dado el coaseguro
        /// </summary>
        /// <param name="tempCoinsuranceParentId"></param>
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.CoinsuranceCheckingAccountItemDTO> GetTempCoinsuranceCheckingAccountItemChildByTempCoinsuranceParentId(int tempCoinsuranceParentId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsuranceCheckingAccountItem.Properties.TempCoinsCheckingAccTransCode, tempCoinsuranceParentId);

                UIView tempCoinsurancerCheckingAccountItemChilds = _dataFacadeManager.GetDataFacade().GetView("TempCoinsuranceCheckingAccTransView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (tempCoinsurancerCheckingAccountItemChilds.Rows.Count > 0)
                {
                    tempCoinsurancerCheckingAccountItemChilds.Columns.Add("rows", typeof(int));
                    tempCoinsurancerCheckingAccountItemChilds.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.CoinsuranceCheckingAccountItemDTO> coinsuranceCheckingAccountItems = new List<SEARCH.CoinsuranceCheckingAccountItemDTO>();

                foreach (DataRow dataRow in tempCoinsurancerCheckingAccountItemChilds)
                {
                    coinsuranceCheckingAccountItems.Add(new SEARCH.CoinsuranceCheckingAccountItemDTO()
                    {
                        CoinsuranceCheckingAccountId = dataRow["TempCoinsCheckingAccTransCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempCoinsCheckingAccTransCode"]),
                        CoinsuranceCheckingAccountItemId = dataRow["CoinsCheckingAccTransId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsCheckingAccTransId"]),
                        ImputationId = dataRow["ImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ImputationCode"]),
                        CoinsuranceParentId = dataRow["CoinsuranceParentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuranceParentCode"]),
                        CoinsuranceCheckingAccountItemChildId = Convert.ToInt32(dataRow["TempCoinsuranceCheckingAccountItemCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        PosCode = dataRow["SalePointCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SalePointCode"]),
                        PosName = dataRow["SalePointName"] == DBNull.Value ? "" : dataRow["SalePointName"].ToString(),
                        LineBusinessCode = dataRow["PrefixCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixName = dataRow["PrefixName"] == DBNull.Value ? "" : dataRow["PrefixName"].ToString(),
                        CoinsurerName = dataRow["CoinsurerName"] == DBNull.Value ? "" : dataRow["CoinsurerName"].ToString(),
                        CheckingAccountConceptCode = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptName = dataRow["CheckingAccountConceptName"] == DBNull.Value ? "" : dataRow["CheckingAccountConceptName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyName"].ToString(),
                        CurrencyChange = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        DebitCreditName = Convert.ToInt32(dataRow["AccountingNatureCode"]) == 1 ? "Crédito" : "Débito",
                        AccountingNature = dataRow["AccountingNatureCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNatureCode"]),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                        PolicyNumber = dataRow["PolycyNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PolycyNumber"]),
                        CompanyCode = dataRow["AccountingCompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingCompanyCode"]),
                        CompanyName = dataRow["AccountingCompanyName"] == DBNull.Value ? "" : dataRow["AccountingCompanyName"].ToString(),
                        AgentAgencyCode = 1,
                        CollectNumber = 0,
                        CoinsuranceCompanyCode = dataRow["CoinsuredCompanyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuredCompanyId"]),
                        CoinsurancePolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PolicyId"]),
                        Status = 0,
                        DebitCreditCode = dataRow["AccountingNatureCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNatureCode"]),
                        CoinsuranceTypeName = Convert.ToInt32(dataRow["CoinsuranceType"]) == 1 ? "Aceptado" : "Cedido",
                        CoinsuranceType = dataRow["CoinsuranceType"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuranceType"]),
                        ClaimCode = dataRow["ClaimCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ClaimCode"]),
                        ClaimNumber = dataRow["ClaimNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ClaimNumber"]),
                        ComplaintNumber = dataRow["ComplaintNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ComplaintNumber"]),
                        Rows = rows
                    });
                }

                return coinsuranceCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SearchCoinsuranceCheckingAccount
        /// Búsqueda temporal de cuenta de cheques de coaseguros 
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.CoinsuranceCheckingAccountItemDTO> SearchCoinsuranceCheckingAccount(SEARCH.SearchParameterCoinsuranceCheckingAccountDTO searchParameter)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CoinsuranceType, searchParameter.CoinsuranceType);

                if (searchParameter.Branch.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.BranchCode, searchParameter.Branch.Id);
                }

                if (searchParameter.SalePoint.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.SalePointCode, searchParameter.SalePoint.Id);
                }

                if (searchParameter.CoinsurerId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CoinsuredCompanyId, searchParameter.CoinsurerId);
                }

                if (searchParameter.PolicyNumber != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.PolicyId, searchParameter.PolicyNumber);
                }

                if (searchParameter.CoinsurerId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CoinsuredCompanyId, searchParameter.CoinsurerId);
                }

                UIView coinsuranceItems = _dataFacadeManager.GetDataFacade().GetView("CoinsuranceCheckingAccountView",
                                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #endregion

                if (coinsuranceItems.Rows.Count > 0)
                {
                    coinsuranceItems.Columns.Add("rows", typeof(int));
                    coinsuranceItems.Rows[0]["rows"] = rows;
                }

                #region DTO

                List<SEARCH.CoinsuranceCheckingAccountItemDTO> coinsuranceCheckingAccountItems = new List<SEARCH.CoinsuranceCheckingAccountItemDTO>();

                foreach (DataRow dataRow in coinsuranceItems)
                {
                    coinsuranceCheckingAccountItems.Add(new SEARCH.CoinsuranceCheckingAccountItemDTO()
                    {
                        CoinsuranceCheckingAccountItemId = dataRow["CoinsCheckingAccTransId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsCheckingAccTransId"]),
                        ImputationId = dataRow["ImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ImputationCode"]),
                        CoinsuranceParentId = dataRow["CoinsuranceParentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuranceParentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        PosCode = dataRow["SalePointCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SalePointCode"]),
                        PosName = dataRow["SalePointName"] == DBNull.Value ? "" : dataRow["SalePointName"].ToString(),
                        LineBusinessCode = dataRow["PrefixCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixName = dataRow["PrefixName"] == DBNull.Value ? "" : dataRow["PrefixName"].ToString(),
                        CoinsurerName = dataRow["CoinsurerName"] == DBNull.Value ? "" : dataRow["CoinsurerName"].ToString(),
                        CheckingAccountConceptCode = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptName = dataRow["CheckingAccountConceptName"] == DBNull.Value ? "" : dataRow["CheckingAccountConceptName"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyName"].ToString(),
                        CurrencyChange = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        DebitCreditName = Convert.ToInt32(dataRow["AccountingNatureCode"]) == 1 ? "Crédito" : "Débito",
                        AccountingNature = dataRow["AccountingNatureCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNatureCode"]),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                        PolicyNumber = dataRow["PolycyNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PolycyNumber"]),
                        CompanyCode = dataRow["AccountingCompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingCompanyCode"]),
                        CompanyName = dataRow["AccountingCompanyName"] == DBNull.Value ? "" : dataRow["AccountingCompanyName"].ToString(),
                        AgentAgencyCode = 1,
                        CollectNumber = 0,
                        CoinsuranceCompanyCode = dataRow["CoinsuredCompanyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuredCompanyId"]),
                        CoinsurancePolicyId = dataRow["PolicyId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PolicyId"]),
                        Status = 0,
                        DebitCreditCode = dataRow["AccountingNatureCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingNatureCode"]),
                        CoinsuranceTypeName = Convert.ToInt32(dataRow["CoinsuranceType"]) == 1 ? "Aceptado" : "Cedido",
                        CoinsuranceType = dataRow["CoinsuranceType"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CoinsuranceType"]),
                        ClaimCode = dataRow["ClaimCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ClaimCode"]),
                        ClaimNumber = dataRow["ClaimNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ClaimNumber"]),
                        ComplaintNumber = dataRow["ComplaintNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ComplaintNumber"]),
                        Rows = rows
                    });
                }

                #endregion

                return coinsuranceCheckingAccountItems;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateDuplicateCoinsuranceCheckingAccount
        /// Permite validar el ingreso de duplicados tanto en temporales como en reales en 
        /// de cuenta de cheques de coaseguros 
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>Imputation</returns>
        public ImputationDTO ValidateDuplicateCoinsuranceCheckingAccount(SEARCH.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter)
        {
            Imputation imputation = new Imputation();
            imputation.VerificationValue = new Amount();
            imputation.VerificationValue.Value = -1;

            try
            {
                // Se verifica en la tabla temporal
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.BranchCode, validateParameter.Branch.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.SalePointCode, validateParameter.SalePoint.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.AccountingCompanyCode, validateParameter.Company.IndividualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.AccountingNatureCode, validateParameter.AccountingNatureId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.CurrencyCode, validateParameter.Currency.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.CoinsuredCompanyId, validateParameter.CoinsuranceId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.CheckingAccountConceptCode, validateParameter.CheckingAccountConceptId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.CoinsuranceType, validateParameter.CoinsuranceTypeId);

                BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                if (businessCollectionItems.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccount in businessCollectionItems.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
                    {
                        imputation.VerificationValue.Value = 0;
                        imputation.Id = (int)tempCoinsuranceCheckingAccount.TempImputationCode;
                    }
                }

                if (imputation.Id <= 0)
                {
                    // Se verifica en la tabla real
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.BranchCode, validateParameter.Branch.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.SalePointCode, validateParameter.SalePoint.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.AccountingCompanyCode, validateParameter.Company.IndividualId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.AccountingNatureCode, validateParameter.AccountingNatureId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CurrencyCode, validateParameter.Currency.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CoinsuredCompanyId, validateParameter.CoinsuranceId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CheckingAccountConceptCode, validateParameter.CheckingAccountConceptId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CoinsuranceType, validateParameter.CoinsuranceTypeId);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.CoinsCheckingAccTrans coinsuranceCheckingAccount in businessCollection.OfType<ACCOUNTINGEN.CoinsCheckingAccTrans>())
                        {
                            imputation.VerificationValue.Value = 1;
                            imputation.Id = (int)coinsuranceCheckingAccount.ImputationCode;
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return imputation.ToDTO();
        }


        #endregion


        #endregion //ImputationTypes

        #region Various

        #region CurrencyDiference

        /// <summary>
        /// GetCurrencyDiferenceByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>List<CurrencyDiferenceDTO/></returns>
        public List<SEARCH.CurrencyDiferenceDTO> GetCurrencyDiferenceByCurrencyId(int currencyId)
        {

            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CurrencyDifference.Properties.CurrencyCode, currencyId);
                BusinessCollection businessCollection =
                    new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.CurrencyDifference), criteriaBuilder.GetPredicate()));

                List<SEARCH.CurrencyDiferenceDTO> currencies = new List<SEARCH.CurrencyDiferenceDTO>();

                foreach (ACCOUNTINGEN.CurrencyDifference currencyDifference in businessCollection.OfType<ACCOUNTINGEN.CurrencyDifference>())
                {
                    currencies.Add(new SEARCH.CurrencyDiferenceDTO()
                    {
                        CurrencyId = Convert.ToInt32(currencyDifference.CurrencyCode),
                        PercentageDiference = Convert.ToDouble(currencyDifference.PercentageDifference)
                    });
                }

                return currencies;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Company

        /// <summary>
        /// GetParameterMulticompany
        /// Trae parámetros de multicompanía
        /// </summary>
        /// <returns>bool</returns>
        public bool GetParameterMulticompany()
        {
            try
            {

                int parameter;
                bool isParameter = false;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                UIView multiCompanyParameters = _dataFacadeManager.GetDataFacade().GetView("ParameterMulticompanyView",
                              criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                if (multiCompanyParameters.Count > 0)
                {
                    parameter = multiCompanyParameters.Rows[0]["BoolParameter"] == DBNull.Value ? -1 : Convert.ToInt32(multiCompanyParameters.Rows[0]["BoolParameter"]);

                    isParameter = (parameter == 1);
                }

                return isParameter;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCompaniesByUserId
        /// Trae companías por usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<CompanyDTO/></returns>
        public List<CompanyDTO> GetCompaniesByUserId(int userId)
        {
            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.UserAccountingCompany.Properties.UserId, userId);

                UIView userCompanies = _dataFacadeManager.GetDataFacade().GetView("CompaniesUserView",
                                criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                List<CompanyDTO> companies = new List<CompanyDTO>();

                foreach (DataRow dateRow in userCompanies)
                {
                    companies.Add(new CompanyDTO()
                    {
                        IndividualId = dateRow["AccountingCompanyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dateRow["AccountingCompanyCode"]),
                        Name = dateRow["CompanyDescription"] == DBNull.Value ? "" : Convert.ToString(dateRow["CompanyDescription"])
                    });
                }

                #endregion

                return companies;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CurrentAccount

        /// <summary>
        /// GetConceptCurrentAccountDescription
        /// Trae descripción de concepto de pago
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="conceptId"></param>
        /// <param name="sourceId"></param>
        /// <returns>List<ConceptCurrentAccountDTO></returns>
        public List<SEARCH.ConceptCurrentAccountDTO> GetConceptCurrentAccountDescription(int branchId, int userId, int conceptId, int sourceId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.UserCode, userId);
                criteriaBuilder.And();
                if (sourceId == 1) // Agente
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.AgentEnabled, 1);
                }
                if (sourceId == 2) // Coaseguro
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.CoinsuranceEnabled, 1);
                }
                if (sourceId == 3) // Reaseguro
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.ReinsuranceEnabled, 1);
                }
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.AccountingConcept.Properties.AccountingConceptCode);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(conceptId + "%");

                UIView conceptAccounts = _dataFacadeManager.GetDataFacade().GetView("AccountingConceptCurrentView",
                                                            criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                List<SEARCH.ConceptCurrentAccountDTO> conceptCurrentAccounts = new List<SEARCH.ConceptCurrentAccountDTO>();

                foreach (DataRow dataRow in conceptAccounts)
                {
                    conceptCurrentAccounts.Add(new SEARCH.ConceptCurrentAccountDTO()
                    {
                        ConceptId = dataRow["AccountingConceptCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AccountingConceptCode"]),
                        ConceptDescription = dataRow["Description"] == DBNull.Value ? "" : Convert.ToString(dataRow["Description"]),
                        AgentEnabled = dataRow["AgentEnabled"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentEnabled"]),
                        CurrencyId = GetCurrencyDefaultByAccountingConceptId(Convert.ToInt32(dataRow["PaymentConceptCode"])),
                        ItemsEnabled = dataRow["ItemEnabled"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ItemEnabled"]),
                        AccountingConceptId = dataRow["PaymentConceptCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentConceptCode"])
                    });
                }

                #endregion

                return conceptCurrentAccounts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetConceptCurrentAccountCode
        /// Trae código de concepto de pago
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="conceptDescription"></param>
        /// <param name="sourceId"></param>
        /// <returns>List<ConceptCurrentAccountDTO/></returns>
        public List<SEARCH.ConceptCurrentAccountDTO> GetConceptCurrentAccountCode(int branchId, int userId, string conceptDescription, int sourceId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.UserCode, userId);
                criteriaBuilder.And();
                if (sourceId == 1) // Agente
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.AgentEnabled, 1);
                }
                if (sourceId == 2) // Coaseguro
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.CoinsuranceEnabled, 1);
                }
                if (sourceId == 3) // Reaseguro
                {
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.ReinsuranceEnabled, 1);
                }
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.AccountingConcept.Properties.Description);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(conceptDescription + "%");

                UIView conceptAccounts = _dataFacadeManager.GetDataFacade().GetView("AccountingConceptCurrentView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                List<SEARCH.ConceptCurrentAccountDTO> conceptCurrentAccounts = new List<SEARCH.ConceptCurrentAccountDTO>();

                foreach (DataRow dataRow in conceptAccounts)
                {
                    conceptCurrentAccounts.Add(new SEARCH.ConceptCurrentAccountDTO()
                    {
                        ConceptId = dataRow["CheckingAccountConceptCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckingAccountConceptCode"]),
                        ConceptDescription = dataRow["Description"] == DBNull.Value ? "" : Convert.ToString(dataRow["Description"]),
                        AgentEnabled = dataRow["AgentEnabled"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentEnabled"]),
                        CurrencyId = GetCurrencyDefaultByAccountingConceptId(Convert.ToInt32(dataRow["PaymentConceptCode"])),
                        ItemsEnabled = dataRow["ItemsEnabled"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ItemsEnabled"]),
                        AccountingConceptId = dataRow["PaymentConceptCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentConceptCode"])
                    });
                }

                #endregion

                return conceptCurrentAccounts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidatePolicyComponents: Valida que la póliza tenga componentes.
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>bool</returns>
        public bool ValidatePolicyComponents(int policyId, int endorsementId)
        {
            bool hasComponents = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ISSEN.PayerComp.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ISSEN.PayerComp.Properties.EndorsementId, endorsementId);

                BusinessCollection componentsCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ISSEN.PayerComp), criteriaBuilder.GetPredicate()));

                if (componentsCollection.Count > 0)
                {
                    hasComponents = true;
                }
            }
            catch (BusinessException)
            {
                hasComponents = false;
            }

            return hasComponents;
        }

        #endregion

        #region ComponentCollection

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
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId, premiumReceivableId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                    {
                        policyId = Convert.ToInt32(premiumReceivableEntity.PolicyId);
                        endorsementId = Convert.ToInt32(premiumReceivableEntity.EndorsementId);
                        paidAmount = Convert.ToDecimal(premiumReceivableEntity.Amount);
                    }
                }

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
                            premiumCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrimePrefixComponent.Properties.ComponentCode, Convert.ToInt32(Components.Prime));

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

        #endregion        

        #region DiscountedCommision

        /// <summary>
        /// SaveDiscountedCommissionRequest
        //  Graba las comisiones descontadas
        /// </summary>
        /// <param name="premiumReceivableId"></param>
        /// <param name="discountedCommission"></param>
        /// <returns>bool</returns>
        public bool SaveDiscountedCommissionRequest(int premiumReceivableId, decimal discountedCommission)
        {
            try
            {
                bool isSaved = false;
                int discountedCommissionId = 0;
                int policyId = 0;
                int endorsementId = 0;
                int paymentNumber = 0;
                int agentIndividualId = 0;
                int agentTypeId = 0;
                int currencyId = 0;
                decimal exchangeRate = 0;
                decimal netPremium = 0;
                decimal commissionPercentage = 0;

                SEARCH.DiscountedCommissionDTO discountedCommissionDto = new SEARCH.DiscountedCommissionDTO();

                // Obtengo PolicyId, EndorsementId y PaymentNum a través del appPaymentId
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId, premiumReceivableId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableTrans in businessCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                    {
                        policyId = Convert.ToInt32(premiumReceivableTrans.PolicyId);
                        endorsementId = Convert.ToInt32(premiumReceivableTrans.EndorsementId);
                        paymentNumber = Convert.ToInt32(premiumReceivableTrans.PaymentNum);
                    }
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PaymentNum, paymentNumber);

                UIView receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("PolicyPremiumReceivableView",
                                         criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (receivablePremiums.Count > 0)
                {
                    foreach (DataRow dataRow in receivablePremiums)
                    {
                        SEARCH.PendingCommissionDTO commission = GetPendingCommission(Convert.ToInt32(dataRow["PolicyId"]), Convert.ToInt32(dataRow["EndorsementId"]));

                        currencyId = Convert.ToInt32(dataRow["CurrencyCode"]);
                        exchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]);
                        agentIndividualId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]);
                        commissionPercentage = commission.CommissionPercentage;
                        netPremium = Convert.ToDecimal(dataRow["PolicyNetPremium"]);
                    }
                }

                agentTypeId = GetAgentTypeIdByAgentId(agentIndividualId);

                discountedCommissionDto.DiscountedCommissionId = discountedCommissionId;
                discountedCommissionDto.PremiumReceivableId = premiumReceivableId;
                discountedCommissionDto.AgentTypeCode = agentTypeId;
                discountedCommissionDto.AgentIndividualId = agentIndividualId;
                discountedCommissionDto.CurrencyCode = currencyId;
                discountedCommissionDto.ExchangeRate = exchangeRate;
                discountedCommissionDto.BaseIncomeAmount = netPremium;
                discountedCommissionDto.BaseAmount = netPremium * exchangeRate;
                discountedCommissionDto.CommissionPercentage = commissionPercentage;
                discountedCommissionDto.CommissionType = 0; // Pendiente de crear tabla para obtener datos
                discountedCommissionDto.CommissionDiscountIncomeAmount = Math.Abs(discountedCommission);
                discountedCommissionDto.CommissionDiscountAmount = Math.Abs(discountedCommission * exchangeRate);

                isSaved = _discountedCommissionDAO.SaveDiscountedCommission(discountedCommissionDto);

                return isSaved;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #endregion

        #region Preliquidation

        /// <summary>
        /// UpdatePreLiquidation
        /// Actualiza una preliquidación
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidationDTO UpdatePreLiquidation(PreLiquidationDTO preLiquidation)
        {
            try
            {
                return _preLiquidationDAO.UpdatePreLiquidation(preLiquidation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetPreliquidations
        /// Obtiene preliquidaciones
        /// </summary>
        /// <param name="preliquidationsDto"></param>
        /// <returns>List<PreliquidationsDTO/></returns>
        public List<SEARCH.PreliquidationsDTO> GetPreliquidations(SEARCH.PreliquidationsDTO preliquidationsDto)
        {

            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region Filter

                // Por sucursal (campo obligatorio)
                if (preliquidationsDto.BranchId == 0)
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.Preliquidation.Properties.BranchCode);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(0);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.BranchCode, preliquidationsDto.BranchId);
                }

                // Por compañía
                if (preliquidationsDto.AccountingCompanyId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.CompanyCode,
                                          preliquidationsDto.AccountingCompanyId);
                }
                // Por punto de venta
                if (preliquidationsDto.SalesPointId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.SalesPointCode, preliquidationsDto.SalesPointId);
                }
                // Por fecha de inicio
                if (!string.IsNullOrEmpty(preliquidationsDto.StartDate))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.Preliquidation.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(preliquidationsDto.StartDate));
                }
                // Por fecha de fin
                if (!string.IsNullOrEmpty(preliquidationsDto.EndDate))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.Preliquidation.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(preliquidationsDto.EndDate));
                }
                // Por número de preliquidación
                if (preliquidationsDto.PreliquidationId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.PreliquidationCode,
                                          preliquidationsDto.PreliquidationId);
                }
                // Por tipo de abonador
                if (preliquidationsDto.PersonTypeId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.PersonTypeCode, preliquidationsDto.PersonTypeId);
                }
                // Por individualId
                if (preliquidationsDto.BeneficiaryIndividualId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Preliquidation.Properties.IndividualId,
                                          preliquidationsDto.BeneficiaryIndividualId);
                }
                // Por usuario
                if (preliquidationsDto.UsertId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.UserId, preliquidationsDto.UsertId);
                }
                // Filtros que se deben cumplir para mostrar en la consulta.
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.ImputationTypeCode, 3);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.IsRealSource, 1);

                #endregion

                List<ACCOUNTINGEN.BranchPreliquidationV> dataPreliquidations = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.BranchPreliquidationV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.BranchPreliquidationV>().ToList();
                int rows = 0;
                if (dataPreliquidations.Count > 0)
                {
                    rows = dataPreliquidations.Count;
                }
                #region LoadDTO

                List<SEARCH.PreliquidationsDTO> preliquidationDTOs = new List<SEARCH.PreliquidationsDTO>();

                foreach (ACCOUNTINGEN.BranchPreliquidationV preliquidations in dataPreliquidations)
                {
                    string statusDescription = "";
                    if (Convert.ToInt32(preliquidations.PreliquidationStatus) == 1)
                    {
                        statusDescription = "PAR. APLICADO";
                    }
                    if (Convert.ToInt32(preliquidations.PreliquidationStatus) == 2)
                    {
                        statusDescription = "ANULADO";
                    }
                    if (Convert.ToInt32(preliquidations.PreliquidationStatus) == 3)
                    {
                        statusDescription = "APLICADO";
                    }
                    SEARCH.PreliquidationsDTO preliquidationsDTO = new SEARCH.PreliquidationsDTO();


                    preliquidationsDTO.PreliquidationId = preliquidations.PreliquidationCode;
                    preliquidationsDTO.Status = preliquidations.PreliquidationStatus == 0 ? -1 : (int)preliquidations.PreliquidationStatus;
                    preliquidationsDTO.StatusDescription = statusDescription;
                    preliquidationsDTO.BranchId = preliquidations.BranchCode == 0 ? -1 : (int)preliquidations.BranchCode;
                    preliquidationsDTO.BranchDescription = preliquidations.BranchDescription;
                    preliquidationsDTO.UsertId = preliquidations.UserId == 0 ? -1 : (int)preliquidations.UserId;
                    preliquidationsDTO.UserName = preliquidations.AccountName;
                    preliquidationsDTO.SalesPointId = preliquidations.SalesPointCode == 0 ? -1 : (int)preliquidations.SalesPointCode;
                    preliquidationsDTO.SalesPointDescription = preliquidations.SalesPointDescription;
                    preliquidationsDTO.RegisterDate = String.Format("{0:dd/MM/yyyy}", preliquidations.RegisterDate);
                    preliquidationsDTO.AccountingCompanyId = preliquidations.CompanyCode == 0 ? -1 : (int)preliquidations.CompanyCode;
                    preliquidationsDTO.AccountingCompanyDescription = preliquidations.AccountingCompanyDescription;
                    preliquidationsDTO.PersonTypeId = preliquidations.PersonTypeCode == 0 ? -1 : (int)preliquidations.PersonTypeCode;
                    preliquidationsDTO.PersonTypeDescription = preliquidations.PersonTypeDescription;
                    preliquidationsDTO.BeneficiaryIndividualId = preliquidations.IndividualId == 0 ? -1 : (int)preliquidations.IndividualId;
                    preliquidationsDTO.BeneficiaryDocumentNumber = preliquidations.DocumentNumber;
                    preliquidationsDTO.BeneficiaryName = preliquidations.Name;
                    preliquidationsDTO.TempImputationId = preliquidations.TempImputationCode;
                    preliquidationsDTO.SourceId = preliquidations.SourceCode == 0 ? -1 : (int)preliquidations.SourceCode;
                    preliquidationsDTO.ImputationTypeId = preliquidations.ImputationTypeCode == 0 ? -1 : (int)preliquidations.ImputationTypeCode;
                    preliquidationsDTO.ImputationTypeDescription = preliquidations.ImputationTypeDescription;
                    preliquidationsDTO.IsRealSource = Convert.ToInt32(preliquidations.IsRealSource);
                    preliquidationsDTO.Description = preliquidations.PreliquidationDescription.ToString();
                    preliquidationsDTO.TotalAmount = GetDebitsAndCreditsMovementTypes(new ImputationDTO { Id = Convert.ToInt32(preliquidations.TempImputationCode), UserId = preliquidationsDto.UsertId }, 0).VerificationValue.Value;
                    Rows = rows;
                    preliquidationDTOs.Add(preliquidationsDTO);
                }
                #endregion

                return preliquidationDTOs;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// CancelPreliquidation
        /// Cancela preliquidaciones
        /// </summary>
        /// <param name="preliquidationId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        public bool CancelPreliquidation(int preliquidationId, int tempImputationId)
        {
            bool isCanceled = false;
            bool isUpdated = false;
            try
            {
                // Actualiza la preliquidación
                PreLiquidation preLiquidation = _preLiquidationDAO.GetPreLiquidation(preliquidationId);

                PreLiquidation newPreLiquidation = new PreLiquidation();
                newPreLiquidation.Id = preLiquidation.Id;
                newPreLiquidation.Company = preLiquidation.Company;
                newPreLiquidation.Branch = preLiquidation.Branch;
                newPreLiquidation.Description = preLiquidation.Description;
                newPreLiquidation.Imputation = preLiquidation.Imputation;
                newPreLiquidation.IsTemporal = preLiquidation.IsTemporal;
                newPreLiquidation.Payer = preLiquidation.Payer;
                newPreLiquidation.PersonType = preLiquidation.PersonType;
                newPreLiquidation.RegisterDate = DateTime.Now;
                newPreLiquidation.SalePoint = preLiquidation.SalePoint;
                newPreLiquidation.Status = 2; //inactivo

                newPreLiquidation = _preLiquidationDAO.UpdatePreLiquidation(newPreLiquidation);

                if (newPreLiquidation.Id > 0)
                {
                    isUpdated = true;
                }

                // Borra los movimientos relacionados.
                if (isUpdated)
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempImputationCode, tempImputationId);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivable in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                    {
                        // Primas por Cobrar
                        _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivable.TempPremiumReceivableTransCode);
                    }

                    // Cta. Cte. Agentes
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Coaseguros
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Cta. Cte. Reaseguros
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempImputationId);
                    _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempImputationId);

                    // Contabilidad
                    _tempDailyAccountingTransactionItemDAO.DeleteTempDailyAccountingTransactionItemByTempImputationId(tempImputationId);

                    // Borra temporales de transacciones solicitudes de pago de siniestros
                    _tempPaymentRequestTransactionDAO.DeleteTempPaymentRequestByTempImputationId(tempImputationId);

                    // Borra temporales solicitudes de pago de siniestros
                    _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempImputationId);
                }

                isCanceled = true;
            }
            catch (BusinessException)
            {
                isCanceled = false;
            }

            return isCanceled;
        }

        #region TempPreliquidation

        /// <summary>
        /// SaveTempPreLiquidation
        /// Graba las preliquidaciones en Temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidationDTO SaveTempPreLiquidation(PreLiquidationDTO preLiquidation)
        {
            try
            {
                return _tempPreLiquidationDAO.SaveTempPreLiquidation(preLiquidation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateTempPreLiquidation
        /// Actualiza las preliquidaciones en Temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidationDTO UpdateTempPreLiquidation(PreLiquidationDTO preLiquidation)
        {

            try
            {
                return _tempPreLiquidationDAO.UpdateTempPreLiquidation(preLiquidation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// ConvertTempPreLiquidationToPreLiquidation
        /// Convierte preliquidaciones Temporales en reales
        /// </summary>
        /// <param name="tempPreLiquidationId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>int</returns>
        public int ConvertTempPreLiquidationToPreLiquidation(int tempPreLiquidationId, int tempImputationId, int imputationTypeId)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    // Recupera preliquidación temporal
                    PreLiquidation tempPreLiquidation = new PreLiquidation();
                    tempPreLiquidation.Id = tempPreLiquidationId;
                    tempPreLiquidation = _tempPreLiquidationDAO.GetTempPreLiquidation(tempPreLiquidation);

                    // Graba preliquidación de temporal a real
                    tempPreLiquidation.Id = 0;
                    tempPreLiquidation.Status = 1; //Estado Ingresado segun EF validar si viene de algún enum
                    PreLiquidation newPreLiquidation = _preLiquidationDAO.SavePreLiquidation(tempPreLiquidation);

                    // Proceso de actualización de tabla de imputación temporal
                    Imputation imputation = new Imputation();
                    imputation.Id = tempImputationId;
                    imputation.IsTemporal = true;

                    _tempImputationDAO.UpdateTempImputation(imputation, newPreLiquidation.Id);

                    // Asigna el valor del temporal de preliquidación para poder borrarlo
                    tempPreLiquidation.Id = tempPreLiquidationId;

                    // Borra preliquidación temporal
                    _tempPreLiquidationDAO.DeleteTempPreLiquidation(tempPreLiquidation);

                    transaction.Complete();

                    // Devuelve el Id de preliquidación 
                    return newPreLiquidation.Id;
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }


        /// <summary>
        /// GetTempPreLiquidation
        /// Trae preliquidaciones temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidationDTO GetTempPreLiquidation(PreLiquidationDTO preLiquidation)
        {
            try
            {
                return _tempPreLiquidationDAO.GetTempPreLiquidation(preLiquidation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #endregion

        #region JournalEntry

        /// <summary>
        /// TemporarySearch
        /// Búsqueda de temporales
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<TemporaryItemSearchDTO/></returns>
        public List<SEARCH.TemporaryItemSearchDTO> TemporarySearch(SEARCH.SearchParameterTemporaryDTO searchParameter)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                if (searchParameter.Branch.Id > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputationType.Properties.BranchCode, searchParameter.Branch.Id);
                }
                else
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.TempImputationType.Properties.BranchCode);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(0);
                }

                if (searchParameter.UserId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputationType.Properties.UserId, searchParameter.UserId);
                }

                if (searchParameter.ImputationType.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputationType.Properties.ImputationTypeCode, searchParameter.ImputationType.Id);
                }

                if (searchParameter.TemporaryNumber != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputationType.Properties.TempImputationCode, searchParameter.TemporaryNumber);
                }

                if (searchParameter.StartDate != "*")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.TempImputationType.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(searchParameter.StartDate));
                }
                if (searchParameter.EndDate != "*")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.TempImputationType.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(searchParameter.EndDate));
                }

                UIView temporary = _dataFacadeManager.GetDataFacade().GetView("TempImputationTypeView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #endregion

                if (temporary.Rows.Count > 0)
                {
                    temporary.Columns.Add("rows", typeof(int));
                    temporary.Rows[0]["rows"] = rows;
                }

                #region DTO

                List<SEARCH.TemporaryItemSearchDTO> temporaryDTOs = new List<SEARCH.TemporaryItemSearchDTO>();

                foreach (DataRow dataRow in temporary)
                {
                    temporaryDTOs.Add(new SEARCH.TemporaryItemSearchDTO()
                    {
                        TemporaryNumber = dataRow["TempImputationCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TempImputationCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchName"]),
                        ImputationTypeCode = dataRow["ImputationTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ImputationTypeCode"]),
                        ImputationTypeName = dataRow["ImputationTypeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["ImputationTypeName"]),
                        UserCode = dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                        UserName = dataRow["UserName"] == DBNull.Value ? "" : Convert.ToString(dataRow["UserName"]),
                        SourceCode = dataRow["SourceCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SourceCode"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dataRow["RegisterDate"]),
                        Rows = rows
                    });
                }

                #endregion

                return temporaryDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempJournalEntry
        /// Graba asientos de diario temporal
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntryDTO SaveTempJournalEntry(JournalEntryDTO journalEntry)
        {
            try
            {
                return _tempJournalEntryDAO.SaveTempJournalEntry(journalEntry.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateTempJournalEntry
        /// Actualiza asientos de diario temporal
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntryDTO UpdateTempJournalEntry(JournalEntryDTO journalEntry)
        {
            try
            {
                return _tempJournalEntryDAO.UpdateTempJournalEntry(journalEntry.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntryDTO UpdateJournalEntry(JournalEntryDTO journalEntry)
        {
            try
            {

                JournalEntry newJournalEntry = _journalEntryDAO.GetJournalEntry(journalEntry.ToModel());
                newJournalEntry.Status = journalEntry.Status;

                return _journalEntryDAO.UpdateJournalEntry(journalEntry.ToModel()).ToDTO();

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetTempJournalEntryById
        /// Trae asientos de diario temporal dado su identificativo
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntryDTO GetTempJournalEntryById(int journalEntryId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempJournalEntry.Properties.TempJournalEntryCode, journalEntryId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempJournalEntry), criteriaBuilder.GetPredicate()));

                AccountingModels.Imputations.JournalEntry journalEntry = new AccountingModels.Imputations.JournalEntry();

                foreach (ACCOUNTINGEN.TempJournalEntry journalEntryEntity in businessCollection.OfType<ACCOUNTINGEN.TempJournalEntry>())
                {
                    journalEntry.Id = journalEntryEntity.TempJournalEntryCode;
                    journalEntry.AccountingDate = Convert.ToDateTime(journalEntryEntity.AccountingDate);
                    journalEntry.Branch = new Branch();
                    journalEntry.Branch.Id = Convert.ToInt32(journalEntryEntity.BranchCode);
                    journalEntry.Comments = journalEntryEntity.Comments;
                    journalEntry.Company = new Company();
                    journalEntry.Company.IndividualId = Convert.ToInt32(journalEntryEntity.CompanyCode);
                    journalEntry.Description = journalEntryEntity.Description;
                    journalEntry.Payer = new Individual();
                    journalEntry.Payer.IndividualId = Convert.ToInt32(journalEntryEntity.IndividualId);
                    journalEntry.PersonType = new PersonType();
                    journalEntry.PersonType.Id = Convert.ToInt32(journalEntryEntity.PersonTypeCode);
                    journalEntry.SalePoint = new SalePoint();
                    journalEntry.SalePoint.Id = Convert.ToInt32(journalEntryEntity.SalesPointCode);
                    journalEntry.Status = Convert.ToInt32(journalEntryEntity.Status);
                }

                return journalEntry.ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveJournalEntryImputation
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="userId"></param>
        /// <returns>CollectImputation</returns>
        public CollectImputationDTO SaveJournalEntryImputation(int tempJournalEntryId, int tempImputationId, int userId)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    CollectImputation collectImputation = SaveImputationRequestJournalEntry(tempImputationId, userId, tempJournalEntryId).ToModel();

                    transaction.Complete();

                    return collectImputation.ToDTO();
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();


                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        /// <summary>
        /// GetTempJournalEntryByTempId
        /// Obtiene asientos de diario temporales por su temporal
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <returns>TempJournalEntryDTO</returns>
        public SEARCH.TempJournalEntryDTO GetTempJournalEntryByTempId(int tempJournalEntryId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempJournalEntry.Properties.TempJournalEntryCode, tempJournalEntryId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.TempJournalEntry.Properties.BranchCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                //UIView tempJournalEntries = _dataFacadeManager.GetDataFacade().GetView("TempJournalEntryItemView",
                //                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);
                List<ACCOUNTINGEN.GetTempJournalV> data = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.GetTempJournalV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetTempJournalV>().ToList();

                #endregion

                #region DTO

                SEARCH.TempJournalEntryDTO journalEntry = new SEARCH.TempJournalEntryDTO();
                if (data.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetTempJournalV tempJournalEntries in data)
                    {
                        journalEntry.AccountingDate = tempJournalEntries.AccountingDate == null ? "" : Convert.ToString(tempJournalEntries.AccountingDate);
                        journalEntry.BranchId = tempJournalEntries.BranchCode == 0 ? -1 : Convert.ToInt32(tempJournalEntries.BranchCode);
                        journalEntry.BranchName = tempJournalEntries.BranchName == null ? "" : Convert.ToString(tempJournalEntries.BranchName);
                        journalEntry.Comments = tempJournalEntries.Comments == null ? "" : Convert.ToString(tempJournalEntries.Comments);
                        journalEntry.CompanyId = tempJournalEntries.CompanyCode == 0 ? -1 : Convert.ToInt32(tempJournalEntries.CompanyCode);
                        journalEntry.CompanyName = tempJournalEntries.CompanyName == null ? "" : Convert.ToString(tempJournalEntries.CompanyName);
                        journalEntry.Description = tempJournalEntries.Description == null ? "" : Convert.ToString(tempJournalEntries.Description);
                        journalEntry.DocumentNumber = tempJournalEntries.PersonDocumentnumber == null ? "" : Convert.ToString(tempJournalEntries.PersonDocumentnumber);
                        journalEntry.IndividualId = Convert.ToInt32(tempJournalEntries.IndividualId);
                        journalEntry.PayerName = tempJournalEntries.PayerName == null ? "" : Convert.ToString(tempJournalEntries.PayerName);
                        journalEntry.PersonTypeId = Convert.ToInt32(tempJournalEntries.PersonTypeCode);
                        journalEntry.PersonTypeName = tempJournalEntries.PersonTypeName == null ? "" : Convert.ToString(tempJournalEntries.PersonTypeName);
                        journalEntry.SalesPointId = Convert.ToInt32(tempJournalEntries.SalesPointCode);
                        journalEntry.SalesPointName = tempJournalEntries.SalesPointName == null ? "" : Convert.ToString(tempJournalEntries.SalesPointName);
                        journalEntry.StatusId = tempJournalEntries.Status == null ? -1 : Convert.ToInt32(tempJournalEntries.Status);
                        journalEntry.TempJournalEntryId = tempJournalEntries.TempJournalEntryCode == 0 ? -1 : Convert.ToInt32(tempJournalEntries.TempJournalEntryCode);
                    }
                }

                #endregion

                return journalEntry;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region AgentCommissions

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
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PaymentNum, 1); //se indica que es la primera cuota

                BusinessCollection businessCollectionPayment = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.PremiumReceivableTrans collection in businessCollectionPayment.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                {
                    paidAmount = paidAmount + Convert.ToDecimal(collection.Amount);
                }

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
        /// GetPendingCommission
        /// Obtiene la comision pendiente, a partir de la poliza y el endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>PendingCommissionDTO</returns>
        public SEARCH.PendingCommissionDTO GetPendingCommission(int policyId, int endorsementId)
        {

            try
            {

                decimal pendingCommission = 0;
                decimal commissionPercentage = 0;
                decimal agentParticipationPercentage = 0;

                SEARCH.PendingCommissionDTO pendingCommissionDTO = new SEARCH.PendingCommissionDTO();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AgentCommission.Properties.EndorsementId, endorsementId);

                UIView agentCommissions = _dataFacadeManager.GetDataFacade().GetView("AgentCommissionView",
                             criteriaBuilder.GetPredicate(), null, 0, 10, null, true, out int rows);

                foreach (DataRow dataRow in agentCommissions)
                {
                    pendingCommission = pendingCommission + Convert.ToDecimal(Convert.ToDecimal(dataRow["AgentCommissionAmount"]));
                    commissionPercentage = Convert.ToDecimal(Convert.ToDecimal(dataRow["CommissionPercentage"]));
                    agentParticipationPercentage = Convert.ToDecimal(Convert.ToDecimal(dataRow["AgentParticipationPercentage"]));
                }

                pendingCommissionDTO.PendingCommission = pendingCommission;
                pendingCommissionDTO.CommissionPercentage = commissionPercentage;
                pendingCommissionDTO.AgentParticipationPercentage = agentParticipationPercentage;

                return pendingCommissionDTO;

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

        #endregion

        #region AgentCommissionsProrate

        /// <summary>
        /// GetAgentCommissionProrates
        /// Obtiene las comisiones prorrateadas del agente, a partir de la poliza y el endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="discountedCommission"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="lineBusinessCode"></param>
        /// <param name="subLineBusinessCode"></param>
        /// <param name="userId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.BrokerCheckingAccountItemDTO> GetAgentCommissionProrates(int policyId, int endorsementId, decimal discountedCommission,
                                                                      decimal exchangeRate, int lineBusinessCode, int subLineBusinessCode, int userId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAgentCommissionProrate.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAgentCommissionProrate.Properties.EndorsementId, endorsementId);
                if (lineBusinessCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAgentCommissionProrate.Properties.LineBusinessCode, lineBusinessCode);
                }
                if (subLineBusinessCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAgentCommissionProrate.Properties.SubLineBusinessCode, subLineBusinessCode);
                }

                UIView agentCommissions = _dataFacadeManager.GetDataFacade().GetView("AgentCommissionProrateView",
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
                        brokerCheckingAccountItemDTO.CommissionAmount = Convert.ToDecimal(dataRow["AgentNormalCommissionAmount"]);

                        brokerCheckingAccountItemDTO.CommissionDiscounted = Convert.ToDecimal(discountedCommission * (brokerCheckingAccountItemDTO.CommissionPercentage / 100)); // Revisar este cálculo    

                        brokerCheckingAccountItemDTO.CommissionBalance = brokerCheckingAccountItemDTO.CommissionAmount - brokerCheckingAccountItemDTO.CommissionDiscounted;

                        brokerCheckingAccountItemDTO.Payed = (brokerCheckingAccountItemDTO.CommissionAmount - brokerCheckingAccountItemDTO.CommissionDiscounted) > 0 ? 0 : 1;

                        brokerCheckingAccountItemDTO.LineBusiness = Convert.ToInt32(dataRow["LineBusinessCode"]);
                        brokerCheckingAccountItemDTO.SubLineBusiness = Convert.ToInt32(dataRow["SubLineBusinessCode"]);
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

        #endregion

        #region PartialClousureAgents

        /// <summary>
        /// SavePartialClousureAgentsRequest
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public bool SavePartialClousureAgentsRequest(DateTime dateTo, DateTime dateFrom, int userId, int typeProcess)
        {
            try
            {

                bool partialClousure;

                // agentes 
                if (typeProcess == 0)
                {
                    if (Convert.ToBoolean(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_RELEASE_COMMISSIONS_PRORATE)) == true) //Liberacion de comisiones a prorrata
                    {
                        partialClousure = ReleaseCommissionsProrate(dateTo, dateFrom, userId);
                    }
                    else
                    {
                        partialClousure = PartialClosingCommissions(dateTo, dateFrom, userId);
                    }
                }
                else
                {
                    partialClousure = PartialClosingCommissions(dateTo, dateFrom, userId);

                }

                return partialClousure;
            }
            catch (BusinessException ex)
            {

                throw new BusinessException(Resources.Resources.BusinessException);
            }
        }


        #endregion

        #region CommissionPaymentOrderAgents

        /// <summary>
        /// SaveCommissionPaymentOrder
        /// Permite generar una orden de pago comisiones
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="companyId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="agentId"></param>
        /// <param name="agentName"></param>
        /// <param name="accountingDate"></param>
        /// <param name="userId"></param>
        /// <param name="salePointId"></param>
        /// <param name="processNumber"></param>
        /// <returns>List<PaymentOrder/></returns>
        public List<PaymentOrderDTO> SaveCommissionPaymentOrder(int branchId, int companyId,
                                                            DateTime estimatedPaymentDate, int agentId, string agentName,
                                                            DateTime accountingDate, int userId, int salePointId, int processNumber)
        {
            List<PaymentOrder> paymentOrders = new List<PaymentOrder>();

            try
            {
                // Se busca los agentes que tienen registros en la cuenta corriente que no hayan sido pagadas o no estén bloqueadas 
                // por otro proceso de aplicación
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.AccountingCompanyCode, companyId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(estimatedPaymentDate);

                // Un solo agente
                if (agentId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.AgentId, agentId);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    // Agrupo por agente.
                    var query = from ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid row in businessCollection group row by new { row.AgentId, row.CurrencyCode } into g select new { g.Key.AgentId, g.Key.CurrencyCode, Amount = g.Sum(row => row.StCommissionAmountTotal) };

                    //armo la orden de pago por agente.
                    if (query.Any())
                    {
                        foreach (var item in query)
                        {
                            int accountBankId = GetBankAccountByAgentId(Convert.ToInt32(item.AgentId));

                            PaymentOrder newPaymentOrder = new PaymentOrder();
                            newPaymentOrder.Id = 0;
                            newPaymentOrder.BankAccountPerson = new BankAccountPerson() { Id = Convert.ToInt32(accountBankId) };
                            newPaymentOrder.AccountingDate = Convert.ToDateTime(accountingDate);
                            newPaymentOrder.Amount = new Amount()
                            {
                                Currency = new Currency() { Id = Convert.ToInt32(item.CurrencyCode) },
                                Value = Convert.ToDecimal(item.Amount)
                            };

                            //Amount = paymentOrderTransactionItem.LocalAmount.Value,
                            newPaymentOrder.ExchangeRate = new ExchangeRate()
                            {
                                BuyAmount = Convert.ToDecimal(DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, Convert.ToInt32(item.CurrencyCode)).SellAmount)
                            };

                            newPaymentOrder.LocalAmount = new Amount()
                            { Value = Convert.ToDecimal(item.Amount) * newPaymentOrder.ExchangeRate.BuyAmount };
                            newPaymentOrder.Beneficiary = new Individual() { IndividualId = Convert.ToInt32(item.AgentId) };
                            newPaymentOrder.Branch = new Branch() { Id = Convert.ToInt32(branchId) };
                            newPaymentOrder.BranchPay = new Branch() { Id = Convert.ToInt32(branchId) };
                            newPaymentOrder.Company = new Company() { IndividualId = Convert.ToInt32(companyId) };
                            newPaymentOrder.EstimatedPaymentDate = Convert.ToDateTime(estimatedPaymentDate);
                            newPaymentOrder.PaymentMethod = new PaymentMethod.PaymentMethod();
                            newPaymentOrder.PaymentMethod.Id = Convert.ToInt32(accountBankId == -1 ? UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK) : UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_TRANSFER));
                            newPaymentOrder.PaymentSource = new ACCOUNTINGCONCEPTS.ConceptSource()
                            {
                                Id = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_CONCEPT_SOURCE_ID))  //CÓDIGO 8 DE GL.CONCEPT_SOURCE
                            };
                            newPaymentOrder.PayTo = agentName;
                            newPaymentOrder.PersonType = new PersonType() { Id = Convert.ToInt32(1) };   // AGENTE - SE INSERTO EN PARAM.PERSON_TYPE
                            newPaymentOrder.Status = 1;
                            newPaymentOrder.Observation = "GENERADO AUTOMÁTICAMENTE AGENTES";

                            // Graba la cabecera de orden de pago
                            newPaymentOrder = _paymentOrderDAO.SavePaymentOrder(newPaymentOrder);

                            DateTime registerDate = DateTime.Now;

                            Imputation imputation = new Imputation();
                            imputation.Id = 0;
                            imputation.IsTemporal = true;
                            imputation.Date = registerDate;
                            imputation.ImputationType = ImputationTypes.PaymentOrder;
                            imputation.UserId = userId;

                            // Graba la cabecera de imputación
                            imputation = _tempImputationDAO.SaveTempImputation(imputation, newPaymentOrder.Id);
                            int tempImputationId = imputation.Id;

                            imputation.Id = tempImputationId;
                            imputation.IsTemporal = true;

                            _tempImputationDAO.UpdateTempImputation(imputation, newPaymentOrder.Id);

                            // Grabo la relación 
                            criteriaBuilder = new ObjectCriteriaBuilder();
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.BranchCode, branchId);
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.AccountingDate);
                            criteriaBuilder.LessEqual();
                            criteriaBuilder.Constant(estimatedPaymentDate);
                            criteriaBuilder.And();
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.AccountingCompanyCode, companyId);
                            criteriaBuilder.And();
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid.Properties.AgentId, item.AgentId);

                            BusinessCollection businessCollectionBroker = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid), criteriaBuilder.GetPredicate()));

                            if (businessCollectionBroker.Count > 0)
                            {
                                foreach (ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid itemNotPaid in businessCollectionBroker.OfType<ACCOUNTINGEN.GetBrokerCheckingAccountNotPaid>())
                                {
                                    // Grabar el registro
                                    _paymentOrderBrokerAccountDAO.SavePaymentOrderBrokerAccount(newPaymentOrder.Id, itemNotPaid.BrokerCheckingAccountTransId);
                                }
                            }

                            paymentOrders.Add(newPaymentOrder);
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return paymentOrders.ToDTOs().ToList();
        }

        /// <summary>
        /// GetPaymentOrdersCommission
        /// Permite obtener las órdenes de pago comisiones para el reporte
        /// </summary>
        /// <param name="paymentOrders"></param>
        /// <returns>List<PaymentOrdersCommissionDTO/></returns>
        public List<SEARCH.PaymentOrdersCommissionDTO> GetPaymentOrdersCommission(List<int> paymentOrders)
        {
            List<SEARCH.PaymentOrdersCommissionDTO> paymentOrderCommissionDTOs = new List<SEARCH.PaymentOrdersCommissionDTO>();

            try
            {
                if (paymentOrders.Count > 0)
                {
                    ArrayList values = new ArrayList();

                    // Si existen registros
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrder.Properties.PaymentOrderCode);
                    criteriaBuilder.In();
                    criteriaBuilder.ListValue();
                    foreach (int paymentOrderId in paymentOrders)
                    {
                        if (!values.Contains(paymentOrderId))
                        {
                            criteriaBuilder.Constant(paymentOrderId);
                            values.Add(paymentOrderId);
                        }
                    }
                    criteriaBuilder.EndList();

                    List<ACCOUNTINGEN.IndividualPaymentOrderV> dataOIndividualPaymentOrder = _dataFacadeManager.GetDataFacade().List(
                        typeof(ACCOUNTINGEN.IndividualPaymentOrderV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.IndividualPaymentOrderV>().ToList();

                    foreach (ACCOUNTINGEN.IndividualPaymentOrderV individualPaymentOrder in dataOIndividualPaymentOrder)
                    {
                        SEARCH.PaymentOrdersCommissionDTO paymentOrdersCommissionDTO = new SEARCH.PaymentOrdersCommissionDTO();

                        paymentOrdersCommissionDTO.AgentDocNumberName = individualPaymentOrder.DocumentNumber + "-" + individualPaymentOrder.Name;
                        paymentOrdersCommissionDTO.AgentDocumentNumber = individualPaymentOrder.DocumentNumber;
                        paymentOrdersCommissionDTO.AgentName = individualPaymentOrder.Name;
                        paymentOrdersCommissionDTO.Amount = individualPaymentOrder.Amount == 0 ? 0 : (decimal)individualPaymentOrder.Amount;
                        paymentOrdersCommissionDTO.BranchId = individualPaymentOrder.BranchCode == 0 ? -1 : (int)individualPaymentOrder.BranchCode;
                        paymentOrdersCommissionDTO.CompanyId = individualPaymentOrder.CompanyCode == 0 ? -1 : (int)individualPaymentOrder.CompanyCode;
                        paymentOrdersCommissionDTO.CurrencyId = Convert.ToInt32(individualPaymentOrder.CurrencyCode);
                        paymentOrdersCommissionDTO.CurrencyName = individualPaymentOrder.CurrencyName;
                        paymentOrdersCommissionDTO.EstimatedDatePayment = Convert.ToDateTime(individualPaymentOrder.EstimatedPaymentDate);
                        paymentOrdersCommissionDTO.ExchangeRate = individualPaymentOrder.ExchangeRate == 0 ? 0 : (decimal)individualPaymentOrder.ExchangeRate;
                        paymentOrdersCommissionDTO.IncomeAmount = individualPaymentOrder.IncomeAmount == 0 ? 0 : (decimal)individualPaymentOrder.IncomeAmount;
                        paymentOrdersCommissionDTO.PaymentOrderNumber = Convert.ToString(individualPaymentOrder.PaymentOrderCode);
                        paymentOrderCommissionDTOs.Add(paymentOrdersCommissionDTO);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return paymentOrderCommissionDTOs;
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
        /// GetCurrencyDefaultByPaymentConceptId
        /// Obtiene la moneda por default de la cuenta contable asociada a una cuenta corriente
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <returns>int</returns>
        public int GetCurrencyDefaultByAccountingConceptId(int accountingConceptId)
        {
            int defaultCurrencyId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.AccountingConceptCode, accountingConceptId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingConcept), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingConcept accountingConcept in businessCollection.OfType<GENERALLEDGEREN.AccountingConcept>())
                    {
                        criteriaBuilder = new ObjectCriteriaBuilder();

                        criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, accountingConcept.AccountingAccountId);

                        BusinessCollection businessCollectionLedger = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate()));

                        if (businessCollectionLedger.Count > 0)
                        {
                            foreach (GENERALLEDGEREN.AccountingAccount accountingAccount in businessCollectionLedger.OfType<GENERALLEDGEREN.AccountingAccount>())
                            {
                                defaultCurrencyId = Convert.ToBoolean(accountingAccount.IsMulticurrency) ? -2 : Convert.ToInt32(accountingAccount.DefaultCurrencyCode);
                            }
                        }
                        else
                        {
                            defaultCurrencyId = -2;
                        }
                    }
                }
                else
                {
                    defaultCurrencyId = -2;
                }
            }
            catch (BusinessException)
            {
                defaultCurrencyId = -2;
            }

            return defaultCurrencyId;
        }

        #endregion

        #region CommissionBalance

        ///<summary>
        /// SaveAgentCommissionBalanceItem
        /// Graba un item el registro identificativo para iniciar el proceso de generación de reporte 
        /// de Balance de Comisiones
        /// </summary>
        /// <param name="agentCommissionBalanceItem"></param>
        /// <returns>int</returns>
        public int SaveAgentCommissionBalanceItem(SEARCH.AgentCommissionBalanceItemDTO agentCommissionBalanceItem)
        {
            return _agentCommissionBalanceItemDAO.SaveAgentCommissionBalanceItem(agentCommissionBalanceItem);
        }

        /// <summary>
        /// GetCommissionBalance
        /// Trae balance de comisiones
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        ///  <returns>List<AgentCommissionBalanceItemDTO/></returns>
        public List<SEARCH.AgentCommissionBalanceItemDTO> GetCommissionBalance(int branchId, int accountingCompanyId, int agentId, DateTime dateFrom, DateTime dateTo)
        {
            #region LoadDTO

            try
            {
                List<SEARCH.AgentCommissionBalanceItemDTO> commissionBalance = new List<SEARCH.AgentCommissionBalanceItemDTO>();

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por Sucursal
                if (branchId != 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetCommissionBalance.Properties.BranchCode, branchId).And();
                }

                // Por Compañía
                if (accountingCompanyId != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetCommissionBalance.Properties.AccountingCompanyCode, accountingCompanyId).And();
                }

                // Por Agente
                if (agentId != 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetCommissionBalance.Properties.AgentId, agentId).And();
                }

                // Por fecha desde
                if (dateFrom.ToString() != "*")
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.GetCommissionBalance.Properties.AccountingDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(dateFrom);
                    criteriaBuilder.And();
                }

                // Por fecha hasta
                if (dateTo.ToString() != "*")
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.GetCommissionBalance.Properties.AccountingDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(dateTo);
                    criteriaBuilder.And();
                }

                criteriaBuilder.Property(ACCOUNTINGEN.GetCommissionBalance.Properties.BrokerCheckingAccountTransId);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                    SelectObjects(typeof(ACCOUNTINGEN.GetCommissionBalance), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetCommissionBalance commissionBalanceItem in businessCollection.OfType<ACCOUNTINGEN.GetCommissionBalance>())
                    {
                        SEARCH.AgentCommissionBalanceItemDTO agentCommissionBalanceItemDto = new SEARCH.AgentCommissionBalanceItemDTO();

                        agentCommissionBalanceItemDto.BrokerCheckingAccountId = Convert.ToInt32(commissionBalanceItem.BrokerCheckingAccountTransId);
                        agentCommissionBalanceItemDto.BrokerCheckingAccountDescription = Convert.ToString(commissionBalanceItem.Description);
                        agentCommissionBalanceItemDto.PolicyId = Convert.ToInt32(commissionBalanceItem.PolicyId);
                        agentCommissionBalanceItemDto.DocumentNumPolicy = commissionBalanceItem.DocumentNumPolicy == null ? "" : Convert.ToString(commissionBalanceItem.DocumentNumPolicy);
                        agentCommissionBalanceItemDto.EndorsementId = Convert.ToInt32(commissionBalanceItem.EndorsementId);
                        agentCommissionBalanceItemDto.DocumentNumEndorsement = commissionBalanceItem.DocumentNumEndorsement == 0 ? "" : Convert.ToString(commissionBalanceItem.DocumentNumEndorsement);
                        agentCommissionBalanceItemDto.AgentId = Convert.ToInt32(commissionBalanceItem.AgentId);
                        agentCommissionBalanceItemDto.DocumentNumAgent = Convert.ToString(commissionBalanceItem.DocumentNumAgent);
                        agentCommissionBalanceItemDto.AgentName = Convert.ToString(commissionBalanceItem.AgentName);
                        agentCommissionBalanceItemDto.AgentTypeCode = Convert.ToInt32(commissionBalanceItem.AgentTypeCode);
                        agentCommissionBalanceItemDto.AgentTypeDescription = Convert.ToString(commissionBalanceItem.AgentType);
                        agentCommissionBalanceItemDto.InsuredId = Convert.ToInt32(commissionBalanceItem.InsuredId);
                        agentCommissionBalanceItemDto.InsuredName = Convert.ToString(commissionBalanceItem.InsuredName);
                        agentCommissionBalanceItemDto.DocumentNumInsured = Convert.ToString(commissionBalanceItem.DocumentNumInsured);
                        agentCommissionBalanceItemDto.PrefixId = Convert.ToInt32(commissionBalanceItem.PrefixId);
                        agentCommissionBalanceItemDto.PrefixDescription = Convert.ToString(commissionBalanceItem.PrefixDescription);
                        agentCommissionBalanceItemDto.LineBusinessCode = Convert.ToInt32(commissionBalanceItem.LineBusinessCode);
                        agentCommissionBalanceItemDto.LineBusinessDescription = Convert.ToString(commissionBalanceItem.LineBusinessDescription);
                        agentCommissionBalanceItemDto.CommissionTypeCode = Convert.ToInt32(commissionBalanceItem.CommissionTypeCode);
                        agentCommissionBalanceItemDto.CommissionPercentage = Convert.ToDecimal(commissionBalanceItem.CommissionPercentage);
                        agentCommissionBalanceItemDto.AccountingNature = Convert.ToInt32(commissionBalanceItem.AccountingNature);
                        agentCommissionBalanceItemDto.IncomeAmount = Convert.ToDecimal(commissionBalanceItem.IncomeAmount);
                        agentCommissionBalanceItemDto.CurrencyId = Convert.ToInt32(commissionBalanceItem.CurrencyCode);
                        agentCommissionBalanceItemDto.CurrencyDescription = Convert.ToString(commissionBalanceItem.CurrencyDescription);
                        agentCommissionBalanceItemDto.Amount = Convert.ToDecimal(commissionBalanceItem.Amount);
                        agentCommissionBalanceItemDto.BranchCode = Convert.ToInt32(commissionBalanceItem.BranchCode);
                        agentCommissionBalanceItemDto.Branch = Convert.ToString(commissionBalanceItem.Branch);
                        agentCommissionBalanceItemDto.CompanyCode = Convert.ToInt32(commissionBalanceItem.AccountingCompanyCode);
                        agentCommissionBalanceItemDto.AccountingDate = Convert.ToDateTime(commissionBalanceItem.AccountingDate);
                        agentCommissionBalanceItemDto.ParticipationPercentage = Convert.ToDecimal(commissionBalanceItem.ParticipationPercentage);
                        agentCommissionBalanceItemDto.CommissionPercentage = Convert.ToDecimal(commissionBalanceItem.CommissionPercentage);
                        agentCommissionBalanceItemDto.AdditCommissionPct = Convert.ToDecimal(commissionBalanceItem.AdditCommissPercentage);
                        agentCommissionBalanceItemDto.CommissionAmount = Convert.ToDecimal(commissionBalanceItem.CommissionAmountTotal);
                        agentCommissionBalanceItemDto.CommissionDiscounted = Convert.ToDecimal(commissionBalanceItem.DiscountedCommissionTotal);

                        commissionBalance.Add(agentCommissionBalanceItemDto);
                    }
                }

                #endregion

                return commissionBalance;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region PremiumReceivableValidation

        ///<summary>
        /// CheckPremiumReceivable
        /// Permite validar si la cuota a aplicar ya está siendo utilizada
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="paymentNum"></param>
        /// <param name="payerIndividualId"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO CheckPremiumReceivable(int policyId, int endorsementId, int paymentNum, int payerIndividualId)
        {
            Imputation imputation = new Imputation();

            // Realizo un select para ver si la póliza se encuentra en temporales.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.PolicyId, policyId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.EndorsementId, endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.PaymentNum, paymentNum);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.PayerId, payerIndividualId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                    typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0) // Se encuentra en temporales
            {
                foreach (ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivable in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                {
                    imputation.Id = Convert.ToInt32(tempPremiumReceivable.TempImputationCode);
                    imputation.IsTemporal = !Convert.ToBoolean(new TempImputationDAO().GetTempImputation(imputation).IsTemporal);
                    TransactionType transactionType = new TransactionType();
                    transactionType.Id = new TempImputationDAO().GetTempImputation(imputation).ImputationItems[0].Id;
                    imputation.ImputationItems = new List<TransactionType>();
                    imputation.ImputationItems.Add(transactionType);
                    imputation.ImputationType = new TempImputationDAO().GetTempImputation(imputation).ImputationType;
                }
            }
            else
            {
                // Realizo un select para ver si la póliza se encuentra en reales.
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PaymentNum, paymentNum);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PayerId, payerIndividualId);

                BusinessCollection businessCollectionPremiumReceivable = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                if (businessCollectionPremiumReceivable.Count > 0) // Se encuentra en reales
                {
                    foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableTrans in businessCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                    {
                        imputation.Id = Convert.ToInt32(premiumReceivableTrans.ImputationCode);
                        imputation.IsTemporal = Convert.ToBoolean(false);
                    }
                }
                else
                {
                    imputation.Id = 0;
                    imputation.IsTemporal = false;
                }
            }

            return imputation.ToDTO();
        }

        /// <summary>
        /// ItemHasCancelationEndorsement
        /// Verifica si la cuota pertenece a una póliza cancelada
        /// </summary>
        ///<param name="policyId"> </param>
        ///<returns>bool</returns>
        public bool ItemHasCancelationEndorsement(int policyId)
        {
            bool isEndorsementCancelated = false;
            int endorsementId = 0;
            int endorsementTypeId = 0;

            try
            {
                // Obtengo el máximo id de endoso de esa póliza
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetMaxEndorsement.Properties.PolicyId, policyId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                                                               (typeof(ACCOUNTINGEN.GetMaxEndorsement), criteriaBuilder.GetPredicate()));
                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetMaxEndorsement item in businessCollection.OfType<ACCOUNTINGEN.GetMaxEndorsement>())
                    {
                        endorsementId = Convert.ToInt32(item.MaxEndorsementId);
                    }
                }

                if (endorsementId > 0)
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.IssueEndorsementType.Properties.EndorsementId, endorsementId);

                    businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                                                    (typeof(ACCOUNTINGEN.IssueEndorsementType), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.IssueEndorsementType endorsement in businessCollection.OfType<ACCOUNTINGEN.IssueEndorsementType>())
                        {
                            endorsementTypeId = endorsement.EndorsementTypeCode;
                        }
                    }
                }

                if (endorsementTypeId == 3) // Es endoso de cancelación.
                {
                    isEndorsementCancelated = true;
                }

                return isEndorsementCancelated;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region DailyAccountingValidation

        ///<summary>
        /// CheckDailyAccounting
        /// Permite validar si el momvimiento contable a aplicar ya está siendo utilizada
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salesPointId"></param>
        /// <param name="beneficiaryIndividualId"></param>
        /// <param name="accountingConceptId"></param>
        /// <param name="accountingNature"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO CheckDailyAccounting(int branchId, int salesPointId, int beneficiaryIndividualId, int accountingConceptId, int accountingNature)
        {
            Imputation imputation = new Imputation();

            // Realizo un select para ver si la póliza se encuentra en temporales.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.BranchCode, branchId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.SalePointCode, salesPointId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.BeneficiaryId, beneficiaryIndividualId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.PaymentConceptCode, accountingConceptId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.AccountingNature, accountingNature);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(ACCOUNTINGEN.TempDailyAccountingTrans), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0) // Se encuentra en temporales
            {
                foreach (ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccounting in businessCollection.OfType<ACCOUNTINGEN.TempDailyAccountingTrans>())
                {
                    imputation.Id = Convert.ToInt32(tempDailyAccounting.TempImputationCode);
                    imputation.IsTemporal = Convert.ToBoolean(true);
                }
            }
            else
            {
                // Realizo un select para ver si la póliza se encuentra en reales.
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DailyAccountingTrans.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DailyAccountingTrans.Properties.SalesPointCode, salesPointId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DailyAccountingTrans.Properties.BeneficiaryId, beneficiaryIndividualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DailyAccountingTrans.Properties.PaymentConceptCode, accountingConceptId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DailyAccountingTrans.Properties.AccountingNature, accountingNature);

                businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.DailyAccountingTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0) // Se encuentra en reales
                {
                    foreach (ACCOUNTINGEN.DailyAccountingTrans dailyAccounting in businessCollection.OfType<ACCOUNTINGEN.DailyAccountingTrans>())
                    {
                        imputation.Id = Convert.ToInt32(dailyAccounting.ImputationCode);
                        imputation.IsTemporal = Convert.ToBoolean(false);
                    }
                }
                else
                {
                    imputation.Id = 0;
                    imputation.IsTemporal = false;
                }
            }

            return imputation.ToDTO();
        }

        #endregion

        #region RecievablePremiumPaymentStatus

        ///<summary>
        /// PremiumReceivableStatus
        /// Consulta de saldos en cuotas de póliza
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="policiesWithPortfolio"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        public List<SEARCH.PremiumReceivableSearchPolicyDTO> PremiumReceivableStatus(string insuredId, string policyDocumentNumber,
                                                                              string branchId, string prefixId, string endorsementId,
                                                                              string policiesWithPortfolio, int pageSize, int pageIndex, string ExpirationDateFrom, string ExpirationDateTo)
        {
            try
            {
                pageIndex--;

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (insuredId != "" || policyDocumentNumber != "" || branchId != "" || prefixId != "" || endorsementId != "")
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyId);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(0);
                }
                else
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(-1);
                }

                if (insuredId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.InsuredIndividualId, Convert.ToInt64(insuredId));
                }

                if (policyDocumentNumber != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyDocumentNumber,
                                          Convert.ToInt32(policyDocumentNumber));
                }

                if (branchId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.BranchCode, Convert.ToInt32(branchId));
                }

                if (prefixId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PrefixCode, Convert.ToInt32(prefixId));
                }

                if (endorsementId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.EndorsementDocumentNumber, Convert.ToInt32(endorsementId));
                }
                if (ExpirationDateFrom != "" && ExpirationDateTo != "")
                {

                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PaymentExpirationDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(Convert.ToDateTime(ExpirationDateFrom));
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PaymentExpirationDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(Convert.ToDateTime(ExpirationDateTo));
                }

                UIView receivablePremiums;
                int rows;
                if (policiesWithPortfolio.Equals("N"))
                {
                    receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("PolicyPremiumReceivableView",
                         criteriaBuilder.GetPredicate(), null, pageIndex, pageSize, null, true, out rows);
                }
                else
                {
                    receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("ReceivablePremiumStatusSearchView",
                         criteriaBuilder.GetPredicate(), null, pageIndex, pageSize, null, true, out rows);
                }

                if (receivablePremiums.Rows.Count > 0)
                {
                    receivablePremiums.Columns.Add("rows", typeof(int));
                    receivablePremiums.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs = new List<SEARCH.PremiumReceivableSearchPolicyDTO>();

                foreach (DataRow dataRow in receivablePremiums)
                {
                    premiumReceivableSearchPolicyDTOs.Add(new SEARCH.PremiumReceivableSearchPolicyDTO()
                    {
                        BranchPrefixPolicyEndorsement = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PolicyDocumentNumber"]) + '-' + Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        InsuredDocumentNumberName = Convert.ToString(dataRow["InsuredDocumentNumber"]) + '-' + Convert.ToString(dataRow["InsuredName"]),
                        PayerDocumentNumberName = Convert.ToString(dataRow["PayerDocumentNumber"]) + '-' + Convert.ToString(dataRow["PayerName"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BussinessTypeDescription = Convert.ToString(dataRow["BussinessTypeDescription"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                        EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        PayerId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                        PayerDocumentNumber = Convert.ToString(dataRow["PayerDocumentNumber"]),
                        PayerName = Convert.ToString(dataRow["PayerName"]),
                        PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        PaidAmount = Convert.ToDecimal(dataRow["Amount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]),
                        PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                        CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = Convert.ToString(dataRow["CurrencyDescription"]),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        PolicyAgentId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]),
                        PolicyAgentDocumentNumber = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]),
                        PolicyAgentName = Convert.ToString(dataRow["PolicyAgentName"]),
                        InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        InsuredDocumentNumber = Convert.ToString(dataRow["InsuredDocumentNumber"]),
                        InsuredName = Convert.ToString(dataRow["InsuredName"]),
                        NetPremium = Convert.ToDecimal(dataRow["PolicyNetPremium"]),
                        Expenses = Convert.ToDecimal(dataRow["PolicyExpenses"]),
                        Surcharges = Convert.ToDecimal(dataRow["PolicySurcharges"]),
                        Taxes = Convert.ToDecimal(dataRow["PolicyTaxes"]),
                        Discounts = Convert.ToDecimal(dataRow["PolicyDiscounts"]),
                        PolicyIssuanceDate = Convert.ToDateTime(dataRow["PolicyIssuanceDate"]),
                        PolicyCurrentFrom = Convert.ToDateTime(dataRow["PolicyCurrentFrom"]),
                        PolicyCurrentTo = Convert.ToDateTime(dataRow["PolicyCurrentTo"]),
                        PaymentMethodId = Convert.ToInt32(dataRow["PaymentMethodId"]),
                        PaymentMethodDescription = Convert.ToString(dataRow["PaymentMethodDescription"]),
                        PaymentScheduleId = Convert.ToInt32(dataRow["PaymentScheduleId"]),
                        PaymentScheduleDescription = Convert.ToString(dataRow["PaymentScheduleDescription"]),
                        ItemId = dataRow["EndorsementId"] + dataRow["PaymentNum"].ToString(),  // Se agrego este campo para que al añadir items a pagar respete el orden de cuota
                        Rows = rows,
                        Observations = Convert.ToString(dataRow["Observations"])
                    });
                }

                return premiumReceivableSearchPolicyDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion

        #region CreditNotes

        ///<summary>
        /// GetNegativeEndorsements
        /// Obtiene solo endosos con saldo negativo
        /// </summary>
        ///<param name="policyDocumentNumber"> </param>
        ///<param name="branchId"> </param>
        ///<param name="prefixId"> </param>
        ///<returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        public List<SEARCH.PremiumReceivableSearchPolicyDTO> GetNegativeEndorsements(string policyDocumentNumber, string branchId, string prefixId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (policyDocumentNumber != "" && branchId != "" && prefixId != "")
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyDocumentNumber,
                                          Convert.ToInt64(policyDocumentNumber));

                    criteriaBuilder.And(); // Branch
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.BranchCode, Convert.ToInt32(branchId));

                    criteriaBuilder.And(); // Prefix
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PrefixCode, Convert.ToInt32(prefixId));

                    criteriaBuilder.And(); // Negativos
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PaymentAmount);
                    criteriaBuilder.Less();
                    criteriaBuilder.Constant(0);
                }

                // Ordenamiento por fecha de antigua a más reciente
                #region SortExp

                string sortExp = null;
                string sortOrder = string.Empty;

                sortOrder = "+";
                sortExp = sortOrder + "PaymentNum" + " " + sortOrder + "EndorsementId" + " " + sortOrder + "PolicyId";

                string[] sortExps = null;
                if ((sortExp != null))
                {
                    sortExps = sortExp.Split(' ');
                }

                #endregion

                UIView receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("PolicyPremiumReceivableView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, sortExps, false, out int rows);

                if (receivablePremiums.Rows.Count > 0)
                {
                    receivablePremiums.Columns.Add("rows", typeof(int));
                    receivablePremiums.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs = new List<SEARCH.PremiumReceivableSearchPolicyDTO>();

                foreach (DataRow dataRow in receivablePremiums)
                {
                    premiumReceivableSearchPolicyDTOs.Add(new SEARCH.PremiumReceivableSearchPolicyDTO()
                    {
                        BranchPrefixPolicyEndorsement = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PolicyDocumentNumber"]) + '-' + Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        InsuredDocumentNumberName = Convert.ToString(dataRow["InsuredDocumentNumber"]) + '-' + Convert.ToString(dataRow["InsuredName"]),
                        PayerDocumentNumberName = Convert.ToString(dataRow["PayerDocumentNumber"]) + '-' + Convert.ToString(dataRow["PayerName"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BussinessTypeDescription = Convert.ToString(dataRow["BussinessTypeDescription"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                        EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        PayerId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                        PayerDocumentNumber = Convert.ToString(dataRow["PayerDocumentNumber"]),
                        PayerName = Convert.ToString(dataRow["PayerName"]),
                        PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        PaidAmount = Convert.ToDecimal(dataRow["Amount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]),
                        PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                        CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = Convert.ToString(dataRow["CurrencyDescription"]),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        PolicyAgentId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]),
                        PolicyAgentDocumentNumber = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]),
                        PolicyAgentName = Convert.ToString(dataRow["PolicyAgentName"]),
                        InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        InsuredDocumentNumber = Convert.ToString(dataRow["InsuredDocumentNumber"]),
                        InsuredName = Convert.ToString(dataRow["InsuredName"]),
                        NetPremium = Convert.ToDecimal(dataRow["PolicyNetPremium"]),
                        ItemId = dataRow["EndorsementId"] + dataRow["PaymentNum"].ToString(),  // Se agrego este campo para que al añadir items a pagar respete el orden de cuota
                        Rows = rows
                    });
                }

                return premiumReceivableSearchPolicyDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetPositiveEndorsements
        /// aurresta
        /// Obtiene solo endosos con saldo positivo.
        /// </summary>
        ///<param name="policyDocumentNumber"> </param>
        ///<param name="branchId"> </param>
        ///<param name="prefixId"> </param>
        ///<returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        public List<SEARCH.PremiumReceivableSearchPolicyDTO> GetPositiveEndorsements(string policyDocumentNumber, string branchId, string prefixId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (policyDocumentNumber != "" && branchId != "" && prefixId != "")
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyDocumentNumber,
                                         Convert.ToInt64(policyDocumentNumber));

                    criteriaBuilder.And(); // Branch
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.BranchCode, Convert.ToInt32(branchId));

                    criteriaBuilder.And(); // Prefix
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PrefixCode, Convert.ToInt32(prefixId));

                    criteriaBuilder.And(); // Positivos
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PaymentAmount);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(0);
                }

                // Ordenamiento por fecha de antigua a más reciente
                #region SortExp

                string sortExp = null;
                string sortOrder = string.Empty;

                sortOrder = "+";
                sortExp = sortOrder + "PaymentNum" + " " + sortOrder + "EndorsementId" + " " + sortOrder + "PolicyId";//"PaymentExpirationDate";

                string[] sortExps = null;
                if ((sortExp != null))
                {
                    sortExps = sortExp.Split(' ');
                }

                #endregion

                UIView receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("PolicyPremiumReceivableView",
                                                       criteriaBuilder.GetPredicate(), null, 0, -1, sortExps, false, out int rows);

                if (receivablePremiums.Rows.Count > 0)
                {
                    receivablePremiums.Columns.Add("rows", typeof(int));
                    receivablePremiums.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicyDTOs = new List<SEARCH.PremiumReceivableSearchPolicyDTO>();

                foreach (DataRow dataRow in receivablePremiums)
                {
                    premiumReceivableSearchPolicyDTOs.Add(new SEARCH.PremiumReceivableSearchPolicyDTO()
                    {
                        BranchPrefixPolicyEndorsement = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PolicyDocumentNumber"]) + '-' + Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        InsuredDocumentNumberName = Convert.ToString(dataRow["InsuredDocumentNumber"]) + '-' + Convert.ToString(dataRow["InsuredName"]),
                        PayerDocumentNumberName = Convert.ToString(dataRow["PayerDocumentNumber"]) + '-' + Convert.ToString(dataRow["PayerName"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BussinessTypeDescription = Convert.ToString(dataRow["BussinessTypeDescription"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                        EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        PayerId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                        PayerDocumentNumber = Convert.ToString(dataRow["PayerDocumentNumber"]),
                        PayerName = Convert.ToString(dataRow["PayerName"]),
                        PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        PaidAmount = Convert.ToDecimal(dataRow["Amount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]),
                        PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                        CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = Convert.ToString(dataRow["CurrencyDescription"]),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        PolicyAgentId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]),
                        PolicyAgentDocumentNumber = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]),
                        PolicyAgentName = Convert.ToString(dataRow["PolicyAgentName"]),
                        InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        InsuredDocumentNumber = Convert.ToString(dataRow["InsuredDocumentNumber"]),
                        InsuredName = Convert.ToString(dataRow["InsuredName"]),
                        NetPremium = Convert.ToDecimal(dataRow["PolicyNetPremium"]),
                        ItemId = dataRow["EndorsementId"] + dataRow["PaymentNum"].ToString(),  // Se agrego este campo para que al añadir items a pagar respete el orden de cuota
                        Rows = rows
                    });
                }

                return premiumReceivableSearchPolicyDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GenerateCreditNoteRequest
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="userId"></param>
        ///<returns>List<JournalEntry/></returns>
        public List<JournalEntryDTO> GenerateCreditNoteRequest(JournalEntryDTO journalEntry, string policyDocumentNumber,
                                                                            string branchId, string prefixId, int userId)
        {
            List<ImputationModels.JournalEntry> journalEntries = new List<ImputationModels.JournalEntry>();

            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    int imputationTypeId = (int)ImputationTypes.JournalEntry; //2 // la nota de crédito es un asiento diario.

                    // Obtengo endosos negativos
                    List<SEARCH.PremiumReceivableSearchPolicyDTO> negativeEndorsements = GetNegativeEndorsements(policyDocumentNumber, branchId, prefixId);

                    if (negativeEndorsements.Count > 0)
                    {
                        foreach (SEARCH.PremiumReceivableSearchPolicyDTO negativeEndorsement in negativeEndorsements)
                        {
                            decimal endorsementControl = 0;
                            endorsementControl = negativeEndorsement.PaymentAmount;

                            // Obtengo endosos positivos
                            List<SEARCH.PremiumReceivableSearchPolicyDTO> positiveEndorsements = GetPositiveEndorsements(policyDocumentNumber, branchId, prefixId);

                            foreach (SEARCH.PremiumReceivableSearchPolicyDTO positiveEndorsement in positiveEndorsements)
                            {
                                // Creo los temporales de primas por cobrar
                                PremiumReceivableTransaction tempPremiumReceivableTransaction = new PremiumReceivableTransaction();
                                tempPremiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItem>();

                                if (endorsementControl < 0)
                                {
                                    // Por cada endoso negativo, se creará una transacción (imputacion)
                                    ImputationModels.JournalEntry newJournalEntry = SaveTempJournalEntry(journalEntry).ToModel(); // Genero un temporal de asiento diario

                                    // Genero un temporal de imputación
                                    ImputationDTO tempImputation = new ImputationDTO();
                                    tempImputation.Id = 0;
                                    tempImputation.Date = DateTime.Now;
                                    tempImputation.ImputationType = (int)ImputationTypes.JournalEntry; // la nota de crédito es un asiento diario.
                                    tempImputation.UserId = userId;

                                    tempImputation = SaveTempImputation(tempImputation, newJournalEntry.Id);

                                    if (Math.Abs(positiveEndorsement.PaymentAmount) <= Math.Abs(endorsementControl))
                                    {
                                        // Creo la cobranza para el endoso negativo
                                        PremiumReceivableTransactionItem tempPremiumReceivableTransactionItemDebit = new PremiumReceivableTransactionItem();
                                        tempPremiumReceivableTransactionItemDebit.Id = 0;
                                        tempPremiumReceivableTransactionItemDebit.Policy = new Policy();
                                        tempPremiumReceivableTransactionItemDebit.Policy.Id = negativeEndorsement.PolicyId;
                                        tempPremiumReceivableTransactionItemDebit.Policy.Endorsement = new Endorsement() { Id = negativeEndorsement.EndorsementId };
                                        tempPremiumReceivableTransactionItemDebit.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                        {
                                            new Beneficiary()
                                            {
                                                IndividualId = Convert.ToInt32(negativeEndorsement.PayerId),

                                            }
                                        };
                                        tempPremiumReceivableTransactionItemDebit.Policy.ExchangeRate = new ExchangeRate()
                                        {
                                            Currency = new Currency() { Id = Convert.ToInt32(negativeEndorsement.CurrencyId) }
                                        };
                                        tempPremiumReceivableTransactionItemDebit.Policy.PayerComponents = new List<PayerComponent>()
                                        {
                                            new PayerComponent()
                                            {
                                                Amount = Convert.ToDecimal(negativeEndorsement.Amount),
                                                BaseAmount = Convert.ToDecimal(positiveEndorsement.PaymentAmount * -1)
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemDebit.Policy.PaymentPlan = new PaymentPlan()
                                        {
                                            Quotas = new List<Quota>()
                                            {
                                                new Quota() { Number = Convert.ToInt32(negativeEndorsement.PaymentNumber) }
                                            }
                                        };

                                        tempPremiumReceivableTransactionItemDebit.DeductCommission = new Amount() { Value = Convert.ToInt32(negativeEndorsement.DiscountedCommission) };

                                        tempPremiumReceivableTransaction.PremiumReceivableItems.Add(tempPremiumReceivableTransactionItemDebit);

                                        // Creo la cobranza para el endoso positivo
                                        PremiumReceivableTransactionItem tempPremiumReceivableTransactionItemCredit = new PremiumReceivableTransactionItem();
                                        tempPremiumReceivableTransactionItemCredit.Id = 0;
                                        tempPremiumReceivableTransactionItemCredit.Policy = new Policy();
                                        tempPremiumReceivableTransactionItemCredit.Policy.Id = positiveEndorsement.PolicyId;
                                        tempPremiumReceivableTransactionItemCredit.Policy.Endorsement = new Endorsement() { Id = positiveEndorsement.EndorsementId };
                                        tempPremiumReceivableTransactionItemCredit.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                        {
                                            new Beneficiary() { IndividualId = Convert.ToInt32(positiveEndorsement.PayerId) }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.Policy.ExchangeRate = new ExchangeRate()
                                        {
                                            Currency = new Currency() { Id = Convert.ToInt32(positiveEndorsement.CurrencyId) }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.Policy.PayerComponents = new List<PayerComponent>()
                                        {
                                            new PayerComponent()
                                            {
                                                Amount = Convert.ToDecimal(positiveEndorsement.Amount),
                                                BaseAmount = Convert.ToDecimal(positiveEndorsement.PaymentAmount)
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.Policy.PaymentPlan = new PaymentPlan()
                                        {
                                            Quotas = new List<Quota>()
                                            {
                                                new Quota() { Number = Convert.ToInt32(positiveEndorsement.PaymentNumber) }
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.DeductCommission = new Amount() { Value = Convert.ToInt32(positiveEndorsement.DiscountedCommission) };

                                        tempPremiumReceivableTransaction.PremiumReceivableItems.Add(tempPremiumReceivableTransactionItemCredit);

                                        // Obtengo el saldo pendiente del endoso negativo
                                        endorsementControl = endorsementControl + positiveEndorsement.PaymentAmount;
                                    }
                                    else
                                    {
                                        // Creo la cobranza positiva
                                        PremiumReceivableTransactionItem tempPremiumReceivableTransactionItemDebit = new PremiumReceivableTransactionItem();
                                        tempPremiumReceivableTransactionItemDebit.Id = 0;
                                        tempPremiumReceivableTransactionItemDebit.Policy = new Policy();
                                        tempPremiumReceivableTransactionItemDebit.Policy.Id = negativeEndorsement.PolicyId;
                                        tempPremiumReceivableTransactionItemDebit.Policy.Endorsement = new Endorsement() { Id = negativeEndorsement.EndorsementId };
                                        tempPremiumReceivableTransactionItemDebit.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                        {
                                            new Beneficiary() { IndividualId = Convert.ToInt32(negativeEndorsement.PayerId) }
                                        };
                                        tempPremiumReceivableTransactionItemDebit.Policy.ExchangeRate = new ExchangeRate()
                                        {
                                            Currency = new Currency() { Id = Convert.ToInt32(negativeEndorsement.CurrencyId) }
                                        };
                                        tempPremiumReceivableTransactionItemDebit.Policy.PayerComponents = new List<PayerComponent>()
                                        {
                                            new PayerComponent()
                                            {
                                                Amount = Convert.ToDecimal(negativeEndorsement.Amount),
                                                BaseAmount = Convert.ToDecimal(endorsementControl)
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemDebit.Policy.PaymentPlan = new PaymentPlan()
                                        {
                                            Quotas = new List<Quota>()
                                             {
                                                 new Quota() { Number = Convert.ToInt32(negativeEndorsement.PaymentNumber) }
                                             }
                                        };

                                        tempPremiumReceivableTransactionItemDebit.DeductCommission = new Amount() { Value = Convert.ToInt32(positiveEndorsement.DiscountedCommission) };

                                        tempPremiumReceivableTransaction.PremiumReceivableItems.Add(tempPremiumReceivableTransactionItemDebit);

                                        //creo la cobranza negativa para el endoso positivo
                                        PremiumReceivableTransactionItem tempPremiumReceivableTransactionItemCredit = new PremiumReceivableTransactionItem();
                                        tempPremiumReceivableTransactionItemCredit.Id = 0;
                                        tempPremiumReceivableTransactionItemCredit.Policy = new Policy();
                                        tempPremiumReceivableTransactionItemCredit.Policy.Id = positiveEndorsement.PolicyId;
                                        tempPremiumReceivableTransactionItemCredit.Policy.Endorsement = new Endorsement() { Id = positiveEndorsement.EndorsementId };
                                        tempPremiumReceivableTransactionItemCredit.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                        {
                                            new Beneficiary()
                                            {
                                                IndividualId = Convert.ToInt32(positiveEndorsement.PayerId)
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.Policy.ExchangeRate = new ExchangeRate()
                                        {
                                            Currency = new Currency() { Id = Convert.ToInt32(positiveEndorsement.CurrencyId) },
                                        };
                                        tempPremiumReceivableTransactionItemCredit.Policy.PayerComponents = new List<PayerComponent>()
                                        {
                                            new PayerComponent()
                                            {
                                                 Amount = Convert.ToDecimal(positiveEndorsement.Amount),
                                                 BaseAmount = Convert.ToDecimal(endorsementControl * -1)
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.Policy.PaymentPlan = new PaymentPlan()
                                        {
                                            Quotas = new List<Quota>()
                                            {
                                                new Quota() { Number = Convert.ToInt32(positiveEndorsement.PaymentNumber) }
                                            }
                                        };
                                        tempPremiumReceivableTransactionItemCredit.DeductCommission = new Amount() { Value = Convert.ToInt32(positiveEndorsement.DiscountedCommission) };

                                        tempPremiumReceivableTransaction.PremiumReceivableItems.Add(tempPremiumReceivableTransactionItemCredit);

                                        // Obtengo el saldo pendiente del endoso negativo
                                        endorsementControl = endorsementControl + positiveEndorsement.PaymentAmount;
                                    }

                                    // Grabo las primas por cobrar en temporales.
                                    int premiumSaved = 0;
                                    int journalEntryId = 0;
                                    premiumSaved = SaveTempPremiumRecievableTransaction(tempPremiumReceivableTransaction.ToDTO(), tempImputation.Id, 1, tempImputation.UserId, DateTime.Now);

                                    // Convierto a reales
                                    if (premiumSaved == 1)
                                    {
                                        CollectImputation collectImputationResult = SaveImputationRequestJournalEntry(tempImputation.Id, tempImputation.UserId, newJournalEntry.Id).ToModel();
                                        journalEntryId = collectImputationResult.Transaction.Id;
                                    }

                                    if (journalEntryId > 0)
                                    {
                                        journalEntries.Add(new ImputationModels.JournalEntry()
                                        {
                                            Id = journalEntryId
                                        });
                                    }
                                }
                            }
                        }
                    }

                    transaction.Complete();
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();


                    throw new BusinessException(Resources.Resources.BusinessException);
                }

                return journalEntries.ToDTOs().ToList();
            }
        }

        ///<summary>
        /// GenerateCreditNoteReport
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        ///<returns>List<EndorsementPaymentDTO/></returns>
        public List<SEARCH.EndorsementPaymentDTO> GenerateCreditNoteReport(string policyDocumentNumber, string branchId, string prefixId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyDocumentNumber, Convert.ToInt64(policyDocumentNumber));
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.BranchCode, Convert.ToInt32(branchId));
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PrefixCode, Convert.ToInt32(prefixId));

            // Ordenamiento por EndorsementId
            #region SortExp

            string sortExp = null;
            string sortOrder = string.Empty;

            sortOrder = "+";
            sortExp = sortOrder + "ImputationCode";

            string[] sortExps = null;
            if ((sortExp != null))
            {
                sortExps = sortExp.Split(' ');
            }

            #endregion

            UIView endorsementPayments = _dataFacadeManager.GetDataFacade().GetView("EndorsementsPaymentsView",
                                            criteriaBuilder.GetPredicate(), null, 0, -1, sortExps, false, out int rows);

            List<SEARCH.EndorsementPaymentDTO> endorsementPaymentDTOs = new List<SEARCH.EndorsementPaymentDTO>();

            foreach (DataRow dataRow in endorsementPayments)
            {
                endorsementPaymentDTOs.Add(new SEARCH.EndorsementPaymentDTO()
                {
                    PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                    EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                    PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                    PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                    EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                    BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                    PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                    ImputationId = Convert.ToInt32(dataRow["ImputationCode"]),
                    Amount = Convert.ToDecimal(dataRow["Amount"]),
                    BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                    PrefixDescription = Convert.ToString(dataRow["PrefixDescription"])
                });
            }

            return endorsementPaymentDTOs;
        }

        ///<summary>
        /// ValidateCreditNoteGeneration
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        ///<returns>bool</returns>
        public bool ValidateCreditNoteGeneration(string policyDocumentNumber, string branchId, string prefixId)
        {
            try
            {

                bool isValidated = false;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyDocumentNumber, Convert.ToInt64(policyDocumentNumber));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.BranchCode, Convert.ToInt32(branchId));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PrefixCode, Convert.ToInt32(prefixId));

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PolicyPremiumReceivable), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    isValidated = true;
                }

                return isValidated;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        #endregion

        #region ReverseImputation

        /// <summary>
        /// ReverseImputationRequest
        /// Método para reversar una imputación de recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        ///<returns>bool</returns>
        public bool ReverseImputationRequest(int collectId, int imputationTypeId, int userId)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    bool isReversed = false;
                    bool isSaved = false;
                    CollectImputation collectImputation;
                    Imputation imputation = new Imputation();
                    Imputation newImputation; //se genera una nueva imputación con los valores en negativo.

                    // Obtengo la imputación
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.SourceCode, collectId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.ImputationTypeCode, imputationTypeId);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                            typeof(ACCOUNTINGEN.Imputation), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.Imputation imputationEntity in businessCollection.OfType<ACCOUNTINGEN.Imputation>())
                        {
                            imputation.Id = imputationEntity.ImputationCode;
                        }
                    }

                    collectImputation = _imputationDAO.GetImputation(imputation);
                    newImputation = collectImputation.Imputation;
                    newImputation.Id = 0; // Seteo en 0 para nuevo registro
                    newImputation.UserId = userId;
                    newImputation.Date = DateTime.Now;

                    switch (imputationTypeId)
                    {
                        case 1:
                            newImputation.ImputationType = ImputationTypes.Collect;
                            break;
                        case 2:
                            newImputation.ImputationType = ImputationTypes.JournalEntry;
                            break;
                        case 3:
                            newImputation.ImputationType = ImputationTypes.PreLiquidation;
                            break;
                        case 4:
                            newImputation.ImputationType = ImputationTypes.PaymentOrder;
                            break;
                    }

                    int technicalTransaction = GetTechnicalTransaction();
                    // Grabo la nueva cabecera de imputación
                    newImputation = _imputationDAO.SaveImputation(newImputation, collectId, technicalTransaction);

                    // Si la imputación tiene primas en depósito se procede a la reversión.
                    if (HasPremiumRecievable(imputation.Id))
                    {
                        int payerTypeId = 1; // Quemado hasta definir

                        // Obtengo las primas en depósito.
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.ImputationCode, imputation.Id);

                        BusinessCollection premiumReceivableCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                        foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableItem in premiumReceivableCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                        {
                            decimal paidAmount = 0;

                            PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();
                            premiumReceivableTransactionItem.Id = premiumReceivableItem.PremiumReceivableTransId;

                            premiumReceivableTransactionItem = _premiumReceivableTransactionItemDAO.GetPremiumRecievableTransactionItem(premiumReceivableTransactionItem);

                            paidAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                            premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount = (paidAmount * -1);
                            premiumReceivableTransactionItem.Id = 0; // Seteo en cero para el nuevo registro.

                            // Grabo la reversión de la prima en la tabla de primas
                            premiumReceivableTransactionItem = _premiumReceivableTransactionItemDAO.SavePremiumRecievableTransactionItem(premiumReceivableTransactionItem,
                                newImputation.Id, Convert.ToDecimal(premiumReceivableItem.ExchangeRate), DateTime.Now);

                            if (premiumReceivableTransactionItem.Id > 0) // Si me reversó la prima, continua.
                            {
                                // Reverso comisiones descontadas
                                ObjectCriteriaBuilder discountedCommissionCriteriaBuilder = new ObjectCriteriaBuilder();
                                discountedCommissionCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.DiscountedCommission.Properties.PremiumReceivableTransCode, premiumReceivableItem.PremiumReceivableTransId);

                                BusinessCollection discountedCommissionCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                    typeof(ACCOUNTINGEN.DiscountedCommission), discountedCommissionCriteriaBuilder.GetPredicate()));

                                if (discountedCommissionCollection.Count > 0)
                                {
                                    foreach (ACCOUNTINGEN.DiscountedCommission discountedCommission in discountedCommissionCollection.OfType<ACCOUNTINGEN.DiscountedCommission>())
                                    {
                                        SEARCH.DiscountedCommissionDTO discountedCommissionDto = _discountedCommissionDAO.GetDiscountedCommission(discountedCommission.DiscountedCommissionCode);

                                        discountedCommissionDto.DiscountedCommissionId = 0; // Seteo en 0 para nuevo registro.
                                        discountedCommissionDto.CommissionDiscountIncomeAmount = discountedCommissionDto.CommissionDiscountIncomeAmount * -1; // Registro negativo.
                                        discountedCommissionDto.CommissionDiscountAmount = discountedCommissionDto.CommissionDiscountAmount * -1; // Registro negativo.
                                        discountedCommissionDto.PremiumReceivableId = premiumReceivableTransactionItem.Id; // Se lo relaciona con la reversión del cobro.

                                        _discountedCommissionDAO.SaveDiscountedCommission(discountedCommissionDto); // Grabo el nuevo registro
                                    }
                                }

                                // Reverso las primas en depósito usadas.
                                ObjectCriteriaBuilder depositCriteriaBuilder = new ObjectCriteriaBuilder();

                                depositCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.DepositPremiumTransaction.Properties.PremiumReceivableTransCode, premiumReceivableItem.PremiumReceivableTransId);

                                BusinessCollection depositBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                    typeof(ACCOUNTINGEN.DepositPremiumTransaction), depositCriteriaBuilder.GetPredicate()));

                                if (depositBusinessCollection.Count > 0)
                                {
                                    foreach (ACCOUNTINGEN.DepositPremiumTransaction depositPremiumTransactionItem in depositBusinessCollection.OfType<ACCOUNTINGEN.DepositPremiumTransaction>())
                                    {
                                        int depositPremiumTransactionResult = 0;

                                        DepositPremiumTransaction depositPremiumTransaction = new DepositPremiumTransaction();

                                        depositPremiumTransaction.Id = depositPremiumTransactionItem.DepositPremiumTransactionCode;
                                        depositPremiumTransaction = _depositPremiumTransactionDAO.GetDepositPremiumTransaction(depositPremiumTransaction);
                                        depositPremiumTransaction.Id = 0; // seteo para los nuevos registros

                                        // Actualizo los valores en negativo.
                                        depositPremiumTransaction.Amount = new Amount()
                                        {
                                            Currency = new Currency() { Id = depositPremiumTransaction.Amount.Currency.Id },
                                            Value = depositPremiumTransaction.Amount.Value * -1
                                        };
                                        ;
                                        depositPremiumTransaction.ExchangeRate = new ExchangeRate() { SellAmount = depositPremiumTransaction.ExchangeRate.SellAmount };
                                        depositPremiumTransaction.LocalAmount = new Amount() { Value = depositPremiumTransaction.LocalAmount.Value * -1 };

                                        depositPremiumTransactionResult = _depositPremiumTransactionDAO.SaveDepositPremiumTransaction(depositPremiumTransaction, Convert.ToInt32(depositPremiumTransactionItem.PremiumReceivableTransCode), payerTypeId).Id;

                                        // Obtengo los montos usados.
                                        ObjectCriteriaBuilder amountCriteriaBuilder = new ObjectCriteriaBuilder();

                                        amountCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.Amount.Properties.DepositPremiumTransactionCode, depositPremiumTransactionItem.DepositPremiumTransactionCode);
                                        BusinessCollection amountBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                            typeof(ACCOUNTINGEN.Amount), amountCriteriaBuilder.GetPredicate()));

                                        if (amountBusinessCollection.Count > 0)
                                        {
                                            foreach (ACCOUNTINGEN.Amount usedAmount in amountBusinessCollection.OfType<ACCOUNTINGEN.Amount>())
                                            {
                                                int usedAmountSaved = 0;

                                                Amount amount = new Amount()
                                                {
                                                    Currency = new Currency(),
                                                    Value = Convert.ToDecimal(usedAmount.IncomeAmount) * -1 // Revierto el valor
                                                };
                                                ExchangeRate exchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(usedAmount.ExchangeRate) };
                                                Amount localAmount = new Amount() { Value = Convert.ToDecimal(amount.Value) * -1 }; // Revierto el valor

                                                usedAmountSaved = _usedAmountDAO.SaveUsedAmount(amount, depositPremiumTransactionResult, exchangeRate, localAmount);
                                            }
                                        }
                                    }
                                }

                                // Reversión de la tabla colección de componentes
                                ObjectCriteriaBuilder componentCollectionCriteriaBuilder = new ObjectCriteriaBuilder();

                                componentCollectionCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableItem.PremiumReceivableTransId);

                                BusinessCollection componentBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                        typeof(ACCOUNTINGEN.ComponentCollection), componentCollectionCriteriaBuilder.GetPredicate()));

                                if (componentBusinessCollection.Count > 0)
                                {
                                    foreach (ACCOUNTINGEN.ComponentCollection componentCollection in componentBusinessCollection.OfType<ACCOUNTINGEN.ComponentCollection>())
                                    {
                                        int componentCollectionId = 0;

                                        Amount amount = new Amount()
                                        {
                                            Currency = new Currency { Id = Convert.ToInt32(componentCollection.CurrencyCode) },
                                            Value = Math.Round((Convert.ToDecimal(componentCollection.IncomeAmount) * -1), 2)  // Registro negativo
                                        };
                                        ExchangeRate exchangeRate = new ExchangeRate() { SellAmount = Math.Round(Convert.ToDecimal(componentCollection.ExchangeRate), 2) };
                                        Amount localAmount = new Amount() { Value = Math.Round((Convert.ToDecimal(componentCollection.Amount) * -1), 2) }; // Registro negativo

                                        componentCollectionId = _componentCollectionDAO.SaveComponentCollection(componentCollectionId, Convert.ToInt32(premiumReceivableTransactionItem.Id), Convert.ToInt32(componentCollection.ComponentId), amount, exchangeRate, localAmount);

                                        // Reversión de la tabla colección de componentes
                                        ObjectCriteriaBuilder prefixComponentCriteriaBuilder = new ObjectCriteriaBuilder();

                                        prefixComponentCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixComponentCollection.Properties.ComponentCollectionCode, componentCollection.ComponentCollectionCode);

                                        BusinessCollection prefixComponentBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                            typeof(ACCOUNTINGEN.PrefixComponentCollection), prefixComponentCriteriaBuilder.GetPredicate()));

                                        if (prefixComponentBusinessCollection.Count > 0)
                                        {
                                            foreach (ACCOUNTINGEN.PrefixComponentCollection prefixComponentCollection in prefixComponentBusinessCollection.OfType<ACCOUNTINGEN.PrefixComponentCollection>())
                                            {
                                                int prefixComponentCollectionId = 0;

                                                Amount prefixAmount = new Amount()
                                                {
                                                    Currency = new Currency(),
                                                    Value = Convert.ToDecimal(prefixComponentCollection.IncomeAmount) * -1 // Registro negativo
                                                };
                                                exchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(prefixComponentCollection.ExchangeRate) };
                                                localAmount = new Amount() { Value = Convert.ToDecimal(prefixComponentCollection.Amount) * -1 }; // Registro negativo

                                                prefixComponentCollectionId = _prefixComponentCollectionDAO.SavePrefixComponentCollection(prefixComponentCollectionId, Convert.ToInt32(componentCollectionId),
                                                    Convert.ToInt32(prefixComponentCollection.LineBusinessCode), Convert.ToInt32(prefixComponentCollection.SubLineBusinessCode),
                                                    prefixAmount, exchangeRate, localAmount);
                                            }
                                        }
                                    }
                                }
                                isReversed = true;
                            }
                            else
                            {
                                isReversed = false;
                            }
                        }

                        if (isReversed) // Si se revirtieron las pólizas, continua.
                        {
                            // Reversión de la tabla de cuenta corriente de agentes. 
                            ObjectCriteriaBuilder brokerCheckingAccountCriteriaBuilder = new ObjectCriteriaBuilder();

                            brokerCheckingAccountCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.ImputationCode, imputation.Id);

                            BusinessCollection brokerAccountBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                    typeof(ACCOUNTINGEN.BrokerCheckingAccountTrans), brokerCheckingAccountCriteriaBuilder.GetPredicate()));

                            if (brokerAccountBusinessCollection.Count > 0)
                            {
                                foreach (ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccount in brokerAccountBusinessCollection.OfType<ACCOUNTINGEN.BrokerCheckingAccountTrans>())
                                {
                                    BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                    brokersCheckingAccountTransactionItem.Id = brokerCheckingAccount.BrokerCheckingAccountTransId;

                                    brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.GetBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem);

                                    brokersCheckingAccountTransactionItem.Id = 0; // Seteo para el nuevo registro.
                                    brokersCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = brokersCheckingAccountTransactionItem.CommissionAmount.Value * -1 };
                                    brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = brokersCheckingAccountTransactionItem.CommissionBalance.Value * -1 };

                                    brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem,
                                        newImputation.Id, Convert.ToInt32(brokerCheckingAccount.BrokerParentCode),
                                        Convert.ToInt32(brokerCheckingAccount.TransactionNumber),
                                        Convert.ToInt32(brokerCheckingAccount.AgentTypeCode),
                                        Convert.ToDateTime(brokerCheckingAccount.AccountingDate));

                                    isReversed = (brokersCheckingAccountTransactionItem.Id > 0);
                                }
                            }
                        }
                    }

                    // Todas la imputaciones menos Asientos de Diario
                    if (imputationTypeId != 2)
                    {
                        if (isReversed) // Realizo la actualizacíon del estado del recibo a anulado.
                        {
                            Collect newCollect = new Collect();

                            if (imputationTypeId == Convert.ToInt32(ImputationTypes.Collect))
                            {
                                newCollect.Id = collectId;
                            }

                            if (imputationTypeId == Convert.ToInt32(ImputationTypes.JournalEntry))
                            {
                                newCollect.Id = _collectDAO.GetCollectByBookEntry(collectId);
                            }

                            newCollect = _collectDAO.GetCollect(newCollect);
                            newCollect.Status = Convert.ToInt32(CollectStatus.Active);
                            newCollect.Transaction = new Transaction();

                            newCollect = _collectDAO.UpdateCollect(newCollect, -1);

                            if (newCollect.Id > 0)
                            {
                                isSaved = true;
                            }
                        }
                    }
                    else
                    {
                        isSaved = true;
                    }

                    transaction.Complete();

                    return isSaved;
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        /// <summary>
        /// GetPremiumRecievableAppliedByCollectId
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        public List<SEARCH.PremiumReceivableItemDTO> GetPremiumRecievableAppliedByCollectId(int collectId, int imputationTypeId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.SourceCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.ImputationTypeCode, imputationTypeId);

                UIView premiumReceivables = _dataFacadeManager.GetDataFacade().
                                GetView("PremiumRecievableImputationView", criteriaBuilder.GetPredicate(),
                                null, 0, -1, null, true, out int rows);

                if (premiumReceivables.Rows.Count > 0)
                {
                    premiumReceivables.Columns.Add("rows", typeof(int));
                    premiumReceivables.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.PremiumReceivableItemDTO> premiumReceivableItemDTOs = new List<SEARCH.PremiumReceivableItemDTO>();

                foreach (DataRow dataRow in premiumReceivables)
                {
                    premiumReceivableItemDTOs.Add(new SEARCH.PremiumReceivableItemDTO()
                    {
                        //PremiumReceivableItemId = Convert.ToInt32(dataRow["PremiumReceivableCode"]),                        
                        PremiumReceivableItemId = Convert.ToInt32(dataRow["PremiumReceivableTransId"]),
                        ImputationId = Convert.ToInt32(dataRow["ImputationCode"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        AccountingTransaction = dataRow["AccountingTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["AccountingTransaction"])
                    });
                }

                return premiumReceivableItemDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region BrokerCheckingAccount

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

        #endregion

        #region BrokerBalance

        /// <summary>
        /// GetBrokerBalanceByAgentAndDate
        /// Obtiene balance de agentes
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>BrokerBalanceDTO</returns>
        public SEARCH.BrokerBalanceDTO GetBrokerBalanceByAgentAndDate(int agentId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                SEARCH.BrokerBalanceDTO brokerBalance = new SEARCH.BrokerBalanceDTO();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.BrokerBalance.Properties.BalanceDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.BrokerBalance.Properties.BalanceDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerBalance.Properties.AgentId, agentId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.BrokerBalance), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.BrokerBalance brokerBalanceEntity in businessCollection.OfType<ACCOUNTINGEN.BrokerBalance>())
                {
                    brokerBalance.BrokerBalanceId = brokerBalanceEntity.BrokerBalanceCode;
                    brokerBalance = _brokerBalanceDAO.GetBrokerBalance(brokerBalance);
                }

                return brokerBalance;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBrokerBalancePreviousPeriod
        /// Trae Balance Periodo Anterior de agente
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <returns>BrokerBalanceDTO</returns>
        public SEARCH.BrokerBalanceDTO GetBrokerBalancePreviousPeriod(int agentId, DateTime dateFrom)
        {
            try
            {
                int month = dateFrom.Month - 1;
                int day = dateFrom.Day;

                string date = dateFrom.ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);


                SEARCH.BrokerBalanceDTO brokerBalance = new SEARCH.BrokerBalanceDTO();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.BrokerBalance.Properties.LastBalanceDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(date);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerBalance.Properties.AgentId, agentId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                            typeof(ACCOUNTINGEN.BrokerBalance), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.BrokerBalance brokerBalanceEntity in businessCollection.OfType<ACCOUNTINGEN.BrokerBalance>())
                {
                    brokerBalance.BrokerBalanceId = brokerBalanceEntity.BrokerBalanceCode;
                    brokerBalance = _brokerBalanceDAO.GetBrokerBalance(brokerBalance);
                }

                return brokerBalance;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CoinsuranceCheckingAccount

        /// <summary>
        /// GetCoinsuranceChekingAccounts
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<CoInsuranceCheckingAccountTransactionItem/></returns>
        public List<CoInsuranceCheckingAccountTransactionItemDTO> GetCoinsuranceChekingAccounts(int agentId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<CoInsuranceCheckingAccountTransactionItem> coinsuranceCheckingAccountTransactionItems = new List<CoInsuranceCheckingAccountTransactionItem>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsCheckingAccTrans.Properties.CoinsuredCompanyId, agentId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                            typeof(ACCOUNTINGEN.CoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.CoinsCheckingAccTrans coinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.CoinsCheckingAccTrans>())
                    {
                        CoInsuranceCheckingAccountTransactionItem coInsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();
                        coInsuranceCheckingAccountTransactionItem.Id = coinsuranceCheckingAccountEntity.CoinsCheckingAccTransId;
                        coInsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.GetCoinsuranceCheckingAccountTransactionItem(coInsuranceCheckingAccountTransactionItem);

                        coinsuranceCheckingAccountTransactionItems.Add(coInsuranceCheckingAccountTransactionItem);
                    }
                }

                return coinsuranceCheckingAccountTransactionItems.ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CoinsuranceBalance

        /// <summary>
        /// GetCoinsuraceBalanceByAgentAndDate
        /// Trae balance de coaseguros
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>CoinsuranceBalanceDTO</returns>
        public SEARCH.CoinsuranceBalanceDTO GetCoinsuraceBalanceByAgentAndDate(int agentId, DateTime dateFrom,
                                                                        DateTime dateTo)
        {
            try
            {
                SEARCH.CoinsuranceBalanceDTO coinsuranceBalance = new SEARCH.CoinsuranceBalanceDTO();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.CoinsuranceBalance.Properties.BalanceDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CoinsuranceBalance.Properties.BalanceDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsuranceBalance.Properties.CoinsuredCompanyId, agentId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                            typeof(ACCOUNTINGEN.CoinsuranceBalance), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                foreach (ACCOUNTINGEN.CoinsuranceBalance coinsuranceBalanceEntity in businessCollection.OfType<ACCOUNTINGEN.CoinsuranceBalance>())
                {
                    coinsuranceBalance.CoinsuranceBalanceId = coinsuranceBalanceEntity.CoinsuranceBalanceCode;
                    coinsuranceBalance = _coinsuranceBalanceDAO.GetCoinsuranceBalance(coinsuranceBalance);
                }

                return coinsuranceBalance;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCoinsuraceBalancePreviousPeriod
        /// Trae balance previo de coaseguros
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <returns>CoinsuranceBalanceDTO</returns>
        public SEARCH.CoinsuranceBalanceDTO GetCoinsuraceBalancePreviousPeriod(int agentId, DateTime dateFrom)
        {
            try
            {
                int year = dateFrom.Year;
                int month = dateFrom.Month - 1;
                int day = dateFrom.Day;
                string date = day + "/" + month + "/" + year;

                SEARCH.CoinsuranceBalanceDTO coinsuranceBalance = new SEARCH.CoinsuranceBalanceDTO();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.CoinsuranceBalance.Properties.LastBalanceDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(date);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CoinsuranceBalance.Properties.CoinsuredCompanyId, agentId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                            typeof(ACCOUNTINGEN.CoinsuranceBalance), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                foreach (ACCOUNTINGEN.CoinsuranceBalance coinsuranceBalanceEntity in businessCollection.OfType<ACCOUNTINGEN.CoinsuranceBalance>())
                {
                    coinsuranceBalance.CoinsuranceBalanceId = coinsuranceBalanceEntity.CoinsuranceBalanceCode;
                    coinsuranceBalance = _coinsuranceBalanceDAO.GetCoinsuranceBalance(coinsuranceBalance);
                }

                return coinsuranceBalance;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region PremiumReceivableTransactionItem

        /// <summary>
        /// GetPremiumRecievableTransactionPartialClousure
        /// Cierre parcial de primas por cobrar
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <returns>List<PremiumReceivableTransactionItem/></returns>
        public List<PremiumReceivableTransactionItemDTO> GetPremiumRecievableTransactionPartialClousure(DateTime dateTo, DateTime dateFrom)
        {
            try
            {
                List<PremiumReceivableTransactionItem> premiumReceivableTransactionItems = new List<PremiumReceivableTransactionItem>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.GetPremiumReceivableAgent.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.GetPremiumReceivableAgent.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);


                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                            typeof(ACCOUNTINGEN.GetPremiumReceivableAgent), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.GetPremiumReceivableAgent premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.GetPremiumReceivableAgent>())
                {
                    PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();
                    premiumReceivableTransactionItem.Id = premiumReceivableEntity.PremiumReceivableTransId;
                    premiumReceivableTransactionItem = _premiumReceivableTransactionItemDAO.GetPremiumRecievableTransactionItem(premiumReceivableTransactionItem);
                    premiumReceivableTransactionItems.Add(premiumReceivableTransactionItem);
                }

                return premiumReceivableTransactionItems.ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }
        #endregion

        #region PaymentOrder

        /// <summary>
        /// GetTempImputationByPaymentOrderId
        /// Trae imputación temporal por orden de pago
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>ImputationDTO</returns>
        public ImputationDTO GetTempImputationByPaymentOrderId(int paymentOrderId, int imputationTypeId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.SourceCode, paymentOrderId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.ImputationTypeCode, imputationTypeId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempImputation.Properties.IsRealSource, 1);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempImputation), criteriaBuilder.GetPredicate()));

                Imputation imputation = new Imputation();

                foreach (ACCOUNTINGEN.TempImputation imputationEntity in businessCollection.OfType<ACCOUNTINGEN.TempImputation>())
                {
                    imputation.Id = imputationEntity.TempImputationCode;
                    imputation.Date = Convert.ToDateTime(imputationEntity.RegisterDate);
                    imputation.UserId = Convert.ToInt32(imputationEntity.UserId);
                }

                return imputation.ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SavePaymentOrderImputation
        /// Graba imputación de orden de pago
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int SavePaymentOrderImputation(int paymentOrderId, int imputationTypeId, int userId)
        {
            try
            {
                bool isSaved = false;
                int paymentOrderSaved = 0;

                PaymentOrder newPaymentOrder = new PaymentOrder();

                // Se obtiene el temporal de la imputación
                ImputationDTO tempImputation = GetTempImputationByPaymentOrderId(paymentOrderId, imputationTypeId);

                // Se obtiene la cabecera 
                newPaymentOrder.Id = paymentOrderId;
                newPaymentOrder = _paymentOrderDAO.GetPaymentOrder(newPaymentOrder);

                if (newPaymentOrder.Id > 0)
                {
                    isSaved = SaveImputationRequestPaymentOrder(newPaymentOrder.Id, tempImputation.Id, userId, paymentOrderId);

                    if (isSaved)
                    {
                        paymentOrderSaved = 1;

                        // Cambia el estado de los registros de cuenta corriente de agentes a pagados.
                        ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderBrokerAccount.Properties.PaymentOrderId, paymentOrderId);

                        BusinessCollection collection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.PaymentOrderBrokerAccount), criteriaBuilder.GetPredicate()));

                        if (collection.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.PaymentOrderBrokerAccount paymentOrderBrokerAccount in collection.OfType<ACCOUNTINGEN.PaymentOrderBrokerAccount>())
                            {
                                PrimaryKey primaryKey = ACCOUNTINGEN.BrokerCheckingAccountTrans.CreatePrimaryKey(Convert.ToInt32(paymentOrderBrokerAccount.BrokerCheckingAccountId));
                                ACCOUNTINGEN.BrokerCheckingAccountTrans brokerCheckingAccountEntity = (ACCOUNTINGEN.BrokerCheckingAccountTrans)
                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                                brokerCheckingAccountEntity.Payed = true;
                                _dataFacadeManager.GetDataFacade().UpdateObject(brokerCheckingAccountEntity);
                            }
                        }
                    }
                }

                return paymentOrderSaved;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region GetIds

        /// <summary>
        /// GetAgentTypeIdByAgentId
        /// Obtiene Codigo de tipo de agente a partir del agente
        /// </summary>
        ///<param name="individualId"> </param>
        ///<returns>int</returns>
        public int GetAgentTypeIdByAgentId(int individualId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.GetAgentImputation.CreatePrimaryKey(individualId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.GetAgentImputation agentImputationEntity = (ACCOUNTINGEN.GetAgentImputation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return agentImputationEntity.AgentTypeCode;
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
                criteriaBuilder.Property(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId);
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
        /// GetImputationIdByPremiumReceivableId
        /// Obtiene ImputationDTO a partir de prima por cobrar
        /// </summary>
        ///<param name="premiumReceivableId"> </param>
        ///<returns>int</returns>
        public int GetImputationIdByPremiumReceivableId(int premiumReceivableId)
        {
            int imputationId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(premiumReceivableId);

                //Asignamos BusinessCollection 
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));


                foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableItem in businessCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                {
                    imputationId = Convert.ToInt32(premiumReceivableItem.ImputationCode);
                }

                return imputationId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetSourceIdByImputation
        /// Obtiene el origen de imputación por la imputación
        /// </summary>
        ///<param name="imputation"> </param>
        ///<returns>int</returns>
        public int GetSourceIdByImputation(ImputationDTO imputation)
        {
            int sourceId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.ImputationCode, imputation.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.ImputationTypeCode, imputation.ImputationType);

                //Asignamos BusinessCollection 
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.Imputation), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.Imputation imputationEntity in businessCollection.OfType<ACCOUNTINGEN.Imputation>())
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

        #endregion

        #region MassiveProcessData

        /// <summary>
        /// MassiveDataForGenerate
        /// Genera proceso masivo
        /// </summary>
        /// <param name="issueDate"></param>
        /// <returns>List<MassiveDataGenerateDTO/></returns>
        public List<SEARCH.MassiveDataGenerateDTO> MassiveDataForGenerate(DateTime issueDate)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (issueDate.ToString() != "")
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.MassiveProcessDataBill.Properties.IssueDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(issueDate.ToString("dd/MM/yyyy HH:mm:ss")));
                }

                UIView massiveProcesses = _dataFacadeManager.GetDataFacade().GetView("MassiveProcessDataBillView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (massiveProcesses.Rows.Count > 0)
                {
                    massiveProcesses.Columns.Add("rows", typeof(int));
                    massiveProcesses.Rows[0]["rows"] = rows;
                }

                // Load DTO
                List<SEARCH.MassiveDataGenerateDTO> massiveDataGenerateDTOs = new List<SEARCH.MassiveDataGenerateDTO>();

                foreach (DataRow dataRow in massiveProcesses)
                {
                    massiveDataGenerateDTOs.Add(new SEARCH.MassiveDataGenerateDTO()
                    {
                        Amount = Convert.ToDecimal(dataRow["ComponentAmount"]),
                        IssueDate = Convert.ToDateTime(dataRow["IssueDate"]).ToString("dd/MM/yyyy"),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        PayerId = Convert.ToInt32(dataRow["PayerId"]),
                        PaymentIndividualId = Convert.ToInt32(dataRow["PaymentIndividualId"]),
                        PaymentMethodCode = Convert.ToInt32(dataRow["PaymentMethodCode"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        PolicyNumber = Convert.ToInt32(dataRow["PolicyNumber"]),
                        EndorsementNumber = Convert.ToInt32(dataRow["EndorsementNumber"]),
                        Quote = Convert.ToInt32(dataRow["Quote"]),
                        BranchId = Convert.ToInt32(dataRow["BranchId"]),
                        Rows = rows
                    });
                }

                return massiveDataGenerateDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region  InsuredLoand

        //TODO: ESTOS MÉTODOS DE PRESTAMOS SE MANTIENE YA QUE SE IMPLEMENTARÁ A FUTURO SEGUN FUNCIONAL

        /// <summary>
        /// SaveTempInsuredLoanTransaction
        /// Graba Transacción Temporal de Préstamo de Asegurado
        /// </summary>
        /// <param name="tempInsuredLoanTransaction"></param>
        /// <returns>int</returns>
        public int SaveTempInsuredLoanTransaction(InsuredLoanTransactionDTO tempInsuredLoanTransaction)
        {
            return new int();
        }

        /// <summary>
        /// DeleteTempInsuredLoanTransactionItem
        /// Elminia Transacción Temporal de Préstamo de Asegurado
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="tempInsuredLoanId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempInsuredLoanTransactionItem(int tempImputationId, int tempInsuredLoanId)
        {
            return new bool();
        }

        /// <summary>
        /// GetTmpInsuredLoansByTempImputationId
        /// Obtiene Temporal de Préstamos de Asegurados 
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<InsuredLoanTransaction/></returns>
        public List<InsuredLoanTransactionDTO> GetTmpInsuredLoansByTempImputationId(int tempImputationId)
        {
            return new List<InsuredLoanTransaction>().ToDTOs().ToList();
        }

        /// <summary>
        /// GetInsuredLoanTransaction
        /// Ontiene Transacción de Préstamo de Asegurado
        /// </summary>
        /// <param name="insuredLoanTransaction"></param>
        /// <returns>InsuredLoanTransaction</returns>
        public InsuredLoanTransactionDTO GetInsuredLoanTransaction(InsuredLoanTransactionDTO insuredLoanTransaction)
        {
            return new InsuredLoanTransaction().ToDTO();
        }

        /// <summary>
        /// SaveInsuredLoanTransaction
        /// Graba Transacción de Préstamo de Asegurado
        /// </summary>
        /// <param name="insuredLoanTransaction"></param>
        /// <returns>bool</returns>

        public bool SaveInsuredLoanTransaction(InsuredLoanTransactionDTO insuredLoanTransaction)
        {
            return new bool();
        }

        #endregion

        #region Refinancing

        /// <summary>
        /// Obtiene los cobros a partir de póliza y endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>List<AppPaymentPolicyDTO/></returns>
        public List<SEARCH.AppPaymentPolicyDTO> GetCollections(int policyId, int endorsementId)
        {
            List<SEARCH.AppPaymentPolicyDTO> collections = new List<SEARCH.AppPaymentPolicyDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.EndorsementId, endorsementId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.PremiumReceivableTrans premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
                    {
                        SEARCH.AppPaymentPolicyDTO appPaymentPolicyDto = new SEARCH.AppPaymentPolicyDTO();
                        appPaymentPolicyDto.Id = premiumReceivableEntity.PremiumReceivableTransId;
                        appPaymentPolicyDto.PolicyId = Convert.ToInt32(premiumReceivableEntity.PolicyId);
                        appPaymentPolicyDto.EndorsementId = Convert.ToInt32(premiumReceivableEntity.EndorsementId);
                        appPaymentPolicyDto.PaymentNum = Convert.ToInt32(premiumReceivableEntity.PaymentNum);
                        appPaymentPolicyDto.PayerId = Convert.ToInt32(premiumReceivableEntity.PayerId);
                        appPaymentPolicyDto.Amount = Convert.ToDecimal(premiumReceivableEntity.Amount);
                        appPaymentPolicyDto.RegisterDate = String.Format("{0:dd/MM/yyyy}", premiumReceivableEntity.RegisterDate);
                        Imputation imputation = new Imputation();
                        imputation.Id = Convert.ToInt32(premiumReceivableEntity.ImputationCode);

                        appPaymentPolicyDto.UserId = Convert.ToInt32(_imputationDAO.GetImputation(imputation).Imputation.UserId);
                        appPaymentPolicyDto.CollectCode = Convert.ToInt32(GetSourceCodeByImputationId(imputation.Id));

                        collections.Add(appPaymentPolicyDto);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return collections;
        }

        /// <summary>
        /// GetPaymentsByCollectId
        /// Obtiene los detalles del Recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<Payment></returns>
        public List<PaymentDTO> GetPaymentsByCollectId(int collectId)
        {
            try
            {
                List<PaymentsModels.Payment> payments = _paymentDAO.GetPaymentsByCollectId(collectId);

                return payments.ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// TODO: Alejandro Villagrán - Método temporal para recuotificaciones, se lo usa para mejorar el rendimiento en la búsqueda de póliza por autocomplete.
        /// </summary>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <returns>List<Policy/></returns>
        public List<PolicyDTO> GetPoliciesByBranchAndPrefix(string policyDocumentNumber, int prefixId, int branchId)
        {
            List<Policy> policies = new List<Policy>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (prefixId == 0 && branchId == 0) //para cuando venga de reins
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyId, Convert.ToInt32(policyDocumentNumber));
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PrefixCode, prefixId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.BranchCode, branchId);
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyDocumentNumber);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(policyDocumentNumber + "%");
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PolicyPremiumReceivable.Properties.PolicyCurrentTo);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(DateTime.Now);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PolicyPremiumReceivable), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.PolicyPremiumReceivable receivablePremium in businessCollection.OfType<ACCOUNTINGEN.PolicyPremiumReceivable>())
                    {
                        if (!ItemHasCancelationEndorsement(receivablePremium.PolicyId))
                        {
                            Policy policy = new Policy();
                            policy.Id = receivablePremium.PolicyId;
                            policy.DocumentNumber = Convert.ToInt32(receivablePremium.PolicyDocumentNumber);
                            policy.Branch = new Branch();
                            policy.Branch.Id = receivablePremium.BranchCode;
                            policy.Prefix = new Prefix();
                            policy.Prefix.Id = receivablePremium.PrefixCode;

                            policies.Add(policy);
                        }
                    }
                }
            }
            catch (BusinessException)
            {
                policies = new List<Policy>();
            }

            return policies.ToDTOs().ToList();
        }

        /// <summary>
        /// GetTechnicalTransaction
        /// Consula el número de transacción técnica
        /// </summary>
        /// <returns>int</returns>
        public int GetTechnicalTransaction()
        {
            try
            {

                int parameterId = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_TRANSACTION_NUMBER));
                int transactionNumber = (int)DelegateService.commonService.GetParameterByParameterId(parameterId).NumberParameter;

                // Update transaction number.
                Parameter parameter = new Parameter()
                {
                    Id = parameterId,
                    NumberParameter = transactionNumber + 1,
                    Description = "NÚMERO DE TRANSACCIÓN"
                };

                DelegateService.commonService.UpdateParameter(parameter);

                return transactionNumber;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        #endregion Refinancing

        #endregion Public Methods

        #region Private Methods

        #region PremiumsReceivable

        ///<summary>
        /// SetPremiumReceivableSearchPolicy
        /// </summary>
        /// <param name="premiumReceivable"></param>
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        private List<SEARCH.PremiumReceivableSearchPolicyDTO> SetPremiumReceivableSearchPolicy(UIView premiumReceivable)
        {

            try
            {



                List<SEARCH.PremiumReceivableSearchPolicyDTO> premiumReceivableSearchPolicies = new List<SEARCH.PremiumReceivableSearchPolicyDTO>();

                foreach (DataRow dataRow in premiumReceivable)
                {
                    decimal pendingCommission = 0;

                    premiumReceivableSearchPolicies.Add(new SEARCH.PremiumReceivableSearchPolicyDTO()
                    {
                        BranchPrefixPolicyEndorsement = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PrefixDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PolicyDocumentNumber"]) + '-' + Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        InsuredDocumentNumberName = Convert.ToString(dataRow["InsuredDocumentNumber"]) + '-' + Convert.ToString(dataRow["InsuredName"]),
                        PayerDocumentNumberName = Convert.ToString(dataRow["PayerDocumentNumber"]) + '-' + Convert.ToString(dataRow["PayerName"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BussinessTypeDescription = dataRow["BussinessTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BussinessTypeDescription"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = dataRow["EndorsementDocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                        EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                        CollectGroupId = dataRow["BillingGroupCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BillingGroupCode"]),
                        CollectGroupDescription = dataRow["BillingGroupDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BillingGroupDescription"]),
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        PayerId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                        PayerIndividualId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                        PayerDocumentNumber = Convert.ToString(dataRow["PayerDocumentNumber"]),
                        PayerName = dataRow["PayerName"] == DBNull.Value ? "" : Convert.ToString(dataRow["PayerName"]),
                        PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                        PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                        CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = Convert.ToString(dataRow["CurrencyDescription"]),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        PolicyAgentId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]),
                        PolicyAgentDocumentNumber = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]),
                        PolicyAgentName = Convert.ToString(dataRow["PolicyAgentName"]),
                        InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        InsuredIndividualId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        InsuredDocumentNumber = Convert.ToString(dataRow["InsuredDocumentNumber"]),
                        InsuredName = Convert.ToString(dataRow["InsuredName"]),
                        NetPremium = dataRow["NetPremium"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["NetPremium"]),
                        PendingCommission = pendingCommission, // No necesito la comision pendiente en la búsqueda
                        ItemId = dataRow["EndorsementId"] + dataRow["PaymentNum"].ToString(),  // Se agrego este campo para que al añadir items a pagar respete el orden de cuota
                        Tax = Convert.ToInt32(dataRow["Tax"]).ToString(),
                        Expenses = Convert.ToDecimal(dataRow["Expenses"]),
                        //ComponentId = Convert.ToInt32(dataRow["ComponentId"]).ToString(),
                        Amount = Convert.ToDecimal(dataRow["Amount"])
                    });
                }

                return premiumReceivableSearchPolicies;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion PremiumsReceivable

        #region PaymentRequest

        /// <summary>
        /// LoadGetPaymentRequestClaim
        /// Carga la data para Solicitudes de pago siniestros/recobros/salvamentos
        /// </summary>
        /// <param name="searchParameterClaimsPaymentRequest"></param>
        /// <returns>UIView</returns>
        private UIView LoadGetPaymentRequestClaim(SEARCH.SearchParameterClaimsPaymentRequestDTO searchParameterClaimsPaymentRequest)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (searchParameterClaimsPaymentRequest.SearchBy > 0)
                {
                    // Agente
                    if (searchParameterClaimsPaymentRequest.SearchBy == 10) //Convert.ToInt32(PersonTypes.Agent)
                    {
                        if (searchParameterClaimsPaymentRequest.IndividualId != 0)
                        {
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.IdAgent,
                              searchParameterClaimsPaymentRequest.IndividualId).And();
                        }
                    }
                    else
                    {
                        // Asegurado
                        if (searchParameterClaimsPaymentRequest.SearchBy == 2) //Convert.ToInt32(PersonTypes.Insured)
                        {
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PersonTypeCode, 2).And(); //Convert.ToInt32(PersonTypes.Insured)
                        }
                        // Proveedor
                        if (searchParameterClaimsPaymentRequest.SearchBy == 1) //Convert.ToInt32(PersonTypes.Supplier)
                        {
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PersonTypeCode, 1).And(); //Convert.ToInt32(PersonTypes.Supplier)
                        }

                        if (searchParameterClaimsPaymentRequest.IndividualId > 0)
                        {
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.IdPayBeneficiary,
                              searchParameterClaimsPaymentRequest.IndividualId).And();
                        }
                    }

                    if (searchParameterClaimsPaymentRequest.Branch.Id != -1)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.BranchCode, searchParameterClaimsPaymentRequest.Branch.Id).And();
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.RequestNumber))
                    {
                        // Ajuste Jira SMT-1627 Inicio
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PaymentRequestNumber, Int32.Parse(searchParameterClaimsPaymentRequest.RequestNumber)).And();
                        // Ajuste Jira SMT-1627 Fin
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.ClaimNumber))
                    {
                        // Ajuste Jira SMT-1627 Inicio
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.ClaimNumber, Int32.Parse(searchParameterClaimsPaymentRequest.ClaimNumber)).And();
                        // Ajuste Jira SMT-1627 Fin
                    }

                    if (searchParameterClaimsPaymentRequest.Prefix.Id != -1)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PrefixCode, searchParameterClaimsPaymentRequest.Prefix.Id).And();
                    }

                    if (searchParameterClaimsPaymentRequest.PaymentSource.Id != -1)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PaymentSourceCode, searchParameterClaimsPaymentRequest.PaymentSource.Id).And();
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.DateFrom))
                    {
                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.RegistrationDate);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(Convert.ToDateTime(searchParameterClaimsPaymentRequest.DateFrom));
                        criteriaBuilder.CloseParenthesis();
                        criteriaBuilder.And();
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.DateTo))
                    {

                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.RegistrationDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(Convert.ToDateTime(searchParameterClaimsPaymentRequest.DateTo));
                        criteriaBuilder.CloseParenthesis();
                        criteriaBuilder.And();
                    }

                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PaymentRequestCode);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(0);

                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PaymentSourceCode);
                    criteriaBuilder.Distinct();
                    criteriaBuilder.Constant(4);
                }
                else
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestClaimPendient.Properties.PaymentRequestCode);
                    criteriaBuilder.Less();
                    criteriaBuilder.Constant(0);
                }

                UIView paymentResquestClaims = _dataFacadeManager.GetDataFacade().GetView("PaymentRequestClaimPendientView",
                                     criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (paymentResquestClaims.Rows.Count > 0)
                {
                    paymentResquestClaims.Columns.Add("rows", typeof(int));
                    paymentResquestClaims.Rows[0]["rows"] = rows;
                }

                return paymentResquestClaims;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// LoadGetPaymentRequestVarious
        /// Carga la data para solicitudes de pago varios 
        /// </summary>
        /// <param name="searchParameterClaimsPaymentRequest"></param>
        /// <returns>UIView</returns>
        private UIView LoadGetPaymentRequestVarious(SEARCH.SearchParameterClaimsPaymentRequestDTO searchParameterClaimsPaymentRequest)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (searchParameterClaimsPaymentRequest.SearchBy != -1)
                {
                    if (searchParameterClaimsPaymentRequest.SearchBy == 10) //Productor = Agente //Convert.ToInt32(PersonTypes.Agent)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.PersonTypeCode, 10).And(); //Convert.ToInt32(PersonTypes.Agent)
                    }
                    if (searchParameterClaimsPaymentRequest.SearchBy == 1) //Proveedor //Convert.ToInt32(PersonTypes.Supplier)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.PersonTypeCode, 1).And(); //Convert.ToInt32(PersonTypes.Supplier)
                    }
                    if (searchParameterClaimsPaymentRequest.SearchBy == 11)  //Empleado //Convert.ToInt32(PersonTypes.Employee)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.PersonTypeCode, 11).And(); //Convert.ToInt32(PersonTypes.Employee)
                    }

                    if (searchParameterClaimsPaymentRequest.IndividualId != 0)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.IdPayBeneficiary,
                          searchParameterClaimsPaymentRequest.IndividualId).And();
                    }

                    if (searchParameterClaimsPaymentRequest.Branch.Id != -1)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.BranchCode, searchParameterClaimsPaymentRequest.Branch.Id).And();
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.RequestNumber))
                    {
                        // Ajuste Jira SMT-1559 Inicio
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.PaymentRequestNumber, Convert.ToInt32(searchParameterClaimsPaymentRequest.RequestNumber)).And();
                        // Ajuste Jira SMT-1559 Fin 
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.VoucherNumber))
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestPersonType.Properties.VoucherNumber, searchParameterClaimsPaymentRequest.VoucherNumber).And();
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.DateFrom))
                    {
                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestPersonType.Properties.RegistrationDate);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(Convert.ToDateTime(searchParameterClaimsPaymentRequest.DateFrom));
                        criteriaBuilder.CloseParenthesis();
                        criteriaBuilder.And();
                    }

                    if (!String.IsNullOrEmpty(searchParameterClaimsPaymentRequest.DateTo))
                    {
                        TimeSpan time = new TimeSpan(23, 59, 59);
                        DateTime searchParameterClaimsPaymentReques = new DateTime();
                        searchParameterClaimsPaymentReques = Convert.ToDateTime(searchParameterClaimsPaymentRequest.DateTo);
                        DateTime DateTo = searchParameterClaimsPaymentReques + time;
                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestPersonType.Properties.RegistrationDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(DateTo);
                        criteriaBuilder.CloseParenthesis();
                        criteriaBuilder.And();
                    }

                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestPersonType.Properties.PaymentRequestCode);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(0);
                }
                else
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentRequestPersonType.Properties.PaymentRequestCode);
                    criteriaBuilder.Less();
                    criteriaBuilder.Constant(0);
                }

                UIView paymentRequestVarious = _dataFacadeManager.GetDataFacade().GetView("PaymentRequestPersonTypeView",
                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (paymentRequestVarious.Rows.Count > 0)
                {
                    paymentRequestVarious.Columns.Add("rows", typeof(int));
                    paymentRequestVarious.Rows[0]["rows"] = rows;
                }

                return paymentRequestVarious;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetClaimPaymentByPaymentRequestIdAndPaymentNum
        /// Obtienen las solicitudes de pago de reales buscadas por el número de solicitud de pago y el número de cuota
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="paymentNumber"></param>
        /// <returns>List<BillingClaimPaymentRequestTrans/></returns>
        private List<ACCOUNTINGEN.ClaimPaymentRequestTrans> GetClaimPaymentByPaymentRequestIdAndPaymentNum(int paymentRequestId, int? paymentNumber)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.PaymentRequestCode, paymentRequestId);

                if (paymentNumber != 0) //en caso de que sea solo liquidación de siniestro
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.PaymentNum, paymentNumber);
                }

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.ClaimPaymentRequestTrans), criteriaBuilder.GetPredicate()));

                List<ACCOUNTINGEN.ClaimPaymentRequestTrans> claimPaymentRequestTrans = new List<ACCOUNTINGEN.ClaimPaymentRequestTrans>();

                foreach (ACCOUNTINGEN.ClaimPaymentRequestTrans claimPaymentRequestTransEntity in businessCollection.OfType<ACCOUNTINGEN.ClaimPaymentRequestTrans>())
                {
                    claimPaymentRequestTrans.Add(claimPaymentRequestTransEntity);
                }

                return claimPaymentRequestTrans;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentRequestsByPaymentRequestId
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns>List<ACCOUNTINGEN.PaymentRequestTrans></returns>
        private List<ACCOUNTINGEN.PaymentRequestTrans> GetPaymentRequestTransByPaymentRequestId(int paymentRequestId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestTrans.Properties.PaymentRequestCode, paymentRequestId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.PaymentRequestTrans), criteriaBuilder.GetPredicate()));

                List<ACCOUNTINGEN.PaymentRequestTrans> paymentRequestTransEntities = new List<ACCOUNTINGEN.PaymentRequestTrans>();

                foreach (ACCOUNTINGEN.PaymentRequestTrans paymentRequestTransEntity in businessCollection.OfType<ACCOUNTINGEN.PaymentRequestTrans>())
                {
                    paymentRequestTransEntities.Add(paymentRequestTransEntity);
                }

                return paymentRequestTransEntities;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
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
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.ImputationCode, imputationId);

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
        /// LoadGetTempPaymentRequestClaim
        /// Obtiene data de consulta las temporales pago siniestro
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>UIView</returns>
        private UIView LoadGetTempPaymentRequestClaim(int imputationId, bool isPaymentVarious)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Siniestros
                if (!isPaymentVarious)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPayment.Properties.RequestType, Convert.ToInt32(PaymentRequestTypes.Payment)).And();
                }
                else
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.TempClaimPayment.Properties.RequestType);
                    criteriaBuilder.Distinct();
                    criteriaBuilder.Constant(Convert.ToInt32(PaymentRequestTypes.Payment));
                    criteriaBuilder.And();
                }


                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPayment.Properties.TempImputationCode, imputationId);

                UIView claimPayments = _dataFacadeManager.GetDataFacade().GetView("TempClaimPaymentView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (claimPayments.Rows.Count > 0)
                {
                    claimPayments.Columns.Add("rows", typeof(int));
                    claimPayments.Rows[0]["rows"] = rows;
                }

                return claimPayments;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// LoadGetTempPaymentRequestItems
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>UIView</returns>
        private UIView LoadGetTempPaymentRequestItems(int imputationId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentRequestItems.Properties.TempImputationCode, imputationId);

                UIView tempPaymentRequestTrans = _dataFacadeManager.GetDataFacade().GetView("TempPaymentRequestItemsView",
                              criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (tempPaymentRequestTrans.Rows.Count > 0)
                {
                    tempPaymentRequestTrans.Columns.Add("rows", typeof(int));
                    tempPaymentRequestTrans.Rows[0]["rows"] = rows;
                }

                return tempPaymentRequestTrans;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
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
                ImputationDTO tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId);

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
                        Source = new PRMOD.ConceptSource() { Id = Convert.ToInt32(tempClaimPayment.RequestType) }
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
                ImputationDTO tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId);

                List<ACCOUNTINGEN.TempPaymentRequestTrans> tempPaymentRequest = _tempPaymentRequestTransactionDAO.GetTempPaymentRequestTrans(0, tempImputation.Id);

                foreach (ACCOUNTINGEN.TempPaymentRequestTrans tempPaymentRequestEntity in tempPaymentRequest)
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
        /// UpdatePaymentRequestToStatusPayed
        /// Actualiza el estado de la solicitud de pago a "Pagado" en PaymentRequest
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="paymentUserId"></param>
        /// <param name="executedPaymentDate"></param>
        private void UpdatePaymentRequestToStatusPayed(int imputationId, int paymentUserId, DateTime executedPaymentDate)
        {
            try
            {
                // Busca por imputación 
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ClaimPaymentRequestTrans.Properties.ImputationCode, imputationId);

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

        #endregion PaymentRequest

        #region TempBrokersCheckingAccount

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
                Imputation tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ImputationItems = new List<TransactionType>();
                BrokersCheckingAccountTransaction tempBrokersCheckingAccountTransaction = _tempBrokerCheckingAccountTransactionDAO.GetTempBrokerCheckingAccountTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ImputationItems.Add(tempBrokersCheckingAccountTransaction);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempImputationCode, tempImputation.Id);

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

        #endregion TempBrokersCheckingAccount

        #region CurrentAccountReinsurance

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
                Imputation tempImputation;

                tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ImputationItems = new List<TransactionType>();
                ReInsuranceCheckingAccountTransaction tempReinsuranceCheckingAccountTransaction;
                tempReinsuranceCheckingAccountTransaction = _tempReinsuranceCheckingAccountTransactionDAO.GetTempReinsuranceCheckingAccountTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ImputationItems.Add(tempReinsuranceCheckingAccountTransaction);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempImputationCode, tempImputation.Id);
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

        #endregion CurrentAccountReinsurance

        #region CurrentAccountCoinsurance

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
                Imputation tempImputation = GetTempImputationBySourceCode(imputationTypeId, sourceId).ToModel();

                tempImputation.ImputationItems = new List<TransactionType>();
                CoInsuranceCheckingAccountTransaction tempCoinsuranceCheckingAccountTransaction = _tempCoinsuranceCheckingAccountTransactionDAO.GetTempCoinsuranceCheckingAccountTransactionByTempImputationId(tempImputation.Id);

                tempImputation.ImputationItems.Add(tempCoinsuranceCheckingAccountTransaction);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempImputationCode, tempImputation.Id);
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

        #endregion CurrentAccountCoinsurance

        #region RecalculatingForeignCurrencyAmount

        /// <summary>
        /// GetForeignCurrencyExchangeRate
        /// Obtiene la tasa de cambio del día de la moneda extranjera 
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>decimal</returns>
        private decimal GetForeignCurrencyExchangeRate(int currencyId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            try
            {

                decimal exchangeRate = 0;

                foreach (SEARCH.ForeignCurrencyExchangeRate foreignCurrencyExchangeRate in foreignCurrencyExchangeRates)
                {
                    if (foreignCurrencyExchangeRate.CurrencyId == currencyId)
                    {
                        exchangeRate = foreignCurrencyExchangeRate.ExchangeRate;
                        break;
                    }
                }

                return exchangeRate;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempPremiumRecievable
        /// Recalcula la tasa de cambio del día de la moneda extranjera 
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempPremiumRecievable(int tempImputationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempPremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempPremiumReceivableTrans premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.TempPremiumReceivableTrans>())
                {
                    if (premiumReceivableEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(premiumReceivableEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempPremiumReceivableTrans.CreatePrimaryKey(premiumReceivableEntity.TempPremiumReceivableTransCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempPremiumReceivableTrans tempPremiumReceivableEntity = (ACCOUNTINGEN.TempPremiumReceivableTrans)
                                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempPremiumReceivableEntity.Amount = tempPremiumReceivableEntity.IncomeAmount * exchangeRate;
                        tempPremiumReceivableEntity.ExchangeRate = exchangeRate;

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(tempPremiumReceivableEntity);
                    }
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }


        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempBrokerCheckingAccount
        /// Recalcula la tasa de cambio agente
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempBrokerCheckingAccount(int tempImputationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempBrokerCheckingAccTrans), filter.GetPredicate()));

                foreach (ACCOUNTINGEN.TempBrokerCheckingAccTrans brokerCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempBrokerCheckingAccTrans>())
                {
                    if (brokerCheckingAccountEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(brokerCheckingAccountEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempBrokerCheckingAccTrans.CreatePrimaryKey(brokerCheckingAccountEntity.TempBrokerCheckingAccTransCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempBrokerCheckingAccTrans tempBrokerCheckingAccountEntity = (ACCOUNTINGEN.TempBrokerCheckingAccTrans)
                            (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempBrokerCheckingAccountEntity.Amount = tempBrokerCheckingAccountEntity.IncomeAmount * exchangeRate;
                        tempBrokerCheckingAccountEntity.ExchangeRate = exchangeRate;

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(tempBrokerCheckingAccountEntity);
                    }
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempCoinsuranceCheckingAccount
        /// Recalcula la tasa de cambio coaseguro
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempCoinsuranceCheckingAccount(int tempImputationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempCoinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempCoinsCheckingAccTrans coinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempCoinsCheckingAccTrans>())
                {
                    if (coinsuranceCheckingAccountEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(coinsuranceCheckingAccountEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempCoinsCheckingAccTrans.CreatePrimaryKey(coinsuranceCheckingAccountEntity.TempCoinsCheckingAccTransCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempCoinsCheckingAccTrans tempCoinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempCoinsCheckingAccTrans)
                            (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempCoinsuranceCheckingAccountEntity.Amount = tempCoinsuranceCheckingAccountEntity.IncomeAmount * exchangeRate;
                        tempCoinsuranceCheckingAccountEntity.ExchangeRate = exchangeRate;

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(tempCoinsuranceCheckingAccountEntity);
                    }
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempReinsuranceCheckingAccount
        /// Recalcula la tasa de cambio reaseguros
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempReinsuranceCheckingAccount(int tempImputationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempReinsCheckingAccTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempReinsCheckingAccTrans reinsuranceCheckingAccountEntity in businessCollection.OfType<ACCOUNTINGEN.TempReinsCheckingAccTrans>())
                {
                    if (reinsuranceCheckingAccountEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(reinsuranceCheckingAccountEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempReinsCheckingAccTrans.CreatePrimaryKey(reinsuranceCheckingAccountEntity.TempReinsCheckingAccTransCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempReinsCheckingAccTrans tempReinsuranceCheckingAccountEntity = (ACCOUNTINGEN.TempReinsCheckingAccTrans)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempReinsuranceCheckingAccountEntity.Amount = tempReinsuranceCheckingAccountEntity.IncomeAmount * exchangeRate;
                        tempReinsuranceCheckingAccountEntity.ExchangeRate = exchangeRate;

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(tempReinsuranceCheckingAccountEntity);
                    }
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempClaimPayment
        /// Recalcula la tasa de cambio siniestros
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempClaimPayment(int tempImputationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempClaimPaymentReqTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempClaimPaymentReqTrans claimPaymentEntity in businessCollection.OfType<ACCOUNTINGEN.TempClaimPaymentReqTrans>())
                {
                    if (claimPaymentEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(claimPaymentEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempClaimPaymentReqTrans.CreatePrimaryKey(claimPaymentEntity.TempClaimPaymentReqTransCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPaymentEntity = (ACCOUNTINGEN.TempClaimPaymentReqTrans)
                            (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempClaimPaymentEntity.Amount = tempClaimPaymentEntity.IncomeAmount * exchangeRate;
                        tempClaimPaymentEntity.ExchangeRate = exchangeRate;

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(tempClaimPaymentEntity);
                    }
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempDailyAccounting
        /// Recalcula la tasa de cambio contabilidad
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempDailyAccounting(int tempImputationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingTrans.Properties.TempImputationCode, tempImputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempDailyAccountingTrans), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempDailyAccountingTrans dailyAccountingEntity in businessCollection.OfType<ACCOUNTINGEN.TempDailyAccountingTrans>())
                {
                    if (dailyAccountingEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(dailyAccountingEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingTrans.CreatePrimaryKey(dailyAccountingEntity.TempDailyAccountingTransId);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempDailyAccountingTrans tempDailyAccountingEntity = (ACCOUNTINGEN.TempDailyAccountingTrans)
                            (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempDailyAccountingEntity.Amount = tempDailyAccountingEntity.IncomeAmount * exchangeRate;
                        tempDailyAccountingEntity.ExchangeRate = exchangeRate;

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(tempDailyAccountingEntity);
                    }
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        /// <summary>
        /// RecalculatingForeignCurrencyAmountTempPaymentOrder
        /// Recalcula la tasa de cambio orden de pago
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempPaymentOrder(int tempPaymentOrderId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPaymentOrder.CreatePrimaryKey(tempPaymentOrderId);

                //Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempPaymentOrder paymentOrderEntity = (ACCOUNTINGEN.TempPaymentOrder)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (paymentOrderEntity.CurrencyCode != 0)
                {
                    decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(paymentOrderEntity.CurrencyCode), foreignCurrencyExchangeRates);

                    paymentOrderEntity.Amount = paymentOrderEntity.IncomeAmount * exchangeRate;
                    paymentOrderEntity.ExchangeRate = exchangeRate;

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().UpdateObject(paymentOrderEntity);
                }

                isRecalculated = true;
            }
            catch (BusinessException)
            {
                isRecalculated = false;
            }

            return isRecalculated;
        }

        #endregion

        #region CommissionPaymentOrderAgents

        /// <summary>
        /// GetBankAccountByAgentId
        /// Obtiene una cuenta activa para transferencia del agente 
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>int</returns>
        private int GetBankAccountByAgentId(int agentId)
        {
            try
            {
                int accountBankId = 0;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AccountBankBeneficiary.Properties.IndividualId, agentId);

                UIView beneficiaryBankAccounts = _dataFacadeManager.GetDataFacade().GetView("AccountBankBeneficiaryView",
                                                     criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                #endregion

                if (beneficiaryBankAccounts.Rows.Count > 0)
                {
                    beneficiaryBankAccounts.Columns.Add("rows", typeof(int));
                    beneficiaryBankAccounts.Rows[0]["rows"] = rows;
                }

                #region DTO

                List<SEARCH.BeneficiaryBankAccountsDTO> beneficiaryBankAccountsDTOs = new List<SEARCH.BeneficiaryBankAccountsDTO>();
                accountBankId = -1;

                foreach (DataRow dateRow in beneficiaryBankAccounts)
                {
                    if (Convert.ToInt32(dateRow["Enabled"]) == 1)
                    {
                        accountBankId = dateRow["AccountBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dateRow["AccountBankCode"]);
                    }

                    beneficiaryBankAccountsDTOs.Add(new SEARCH.BeneficiaryBankAccountsDTO()
                    {
                        AccountBankCode = dateRow["AccountBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dateRow["AccountBankCode"]),
                        AccountNumber = dateRow["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(dateRow["AccountNumber"]),
                        AccountTypeCode = dateRow["AccountTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dateRow["AccountTypeCode"]),
                        AccountTypeName = dateRow["AccountTypeName"] == DBNull.Value ? "" : Convert.ToString(dateRow["AccountTypeName"]),
                        BankCode = dateRow["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dateRow["BankCode"]),
                        BankName = dateRow["BankName"] == DBNull.Value ? "" : Convert.ToString(dateRow["BankName"]),
                        CurrencyCode = dateRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dateRow["CurrencyCode"]),
                        CurrencyName = dateRow["CurrencyName"] == DBNull.Value ? "" : Convert.ToString(dateRow["CurrencyName"]),
                        IndividualId = dateRow["IndividualId"] == DBNull.Value ? "" : Convert.ToString(dateRow["IndividualId"]),
                        TinyDescription = dateRow["TinyDescription"] == DBNull.Value ? "" : Convert.ToString(dateRow["TinyDescription"]),
                        Rows = rows
                    });
                }

                #endregion

                return accountBankId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion CommissionPaymentOrderAgents

        #region ReverseImputation

        /// <summary>
        /// HasPremiumRecievable
        /// Indica si la imputación tiene primas en depósito
        /// </summary>
        /// <param name="imputationId"></param>
        ///<returns>bool</returns>
        private bool HasPremiumRecievable(int imputationId)
        {
            try
            {

                bool isPremiumReceivable = false;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.ImputationCode, imputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    isPremiumReceivable = true;
                }

                return isPremiumReceivable;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion ReverseImputation

        #region TempDailyAccountingAnalysis

        /// <summary>
        /// GetTempDailyAccountingAnalysisCodesByTempDailyAccountingTransactionItemId
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns>List<DailyAccountingAnalysisCode></returns>
        private List<DailyAccountingAnalysisCodeDTO> GetTempDailyAccountingAnalysisCodesByTempDailyAccountingTransactionItemId(int tempDailyAccountingTransactionItemId)
        {

            try
            {

                List<DailyAccountingAnalysisCode> tempAnalysisCodes = new List<DailyAccountingAnalysisCode>();

                ObjectCriteriaBuilder analisysFilter = new ObjectCriteriaBuilder();
                analisysFilter.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingAnalysis.Properties.TempDailyAccountingTransCode, tempDailyAccountingTransactionItemId);

                BusinessCollection analysisCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempDailyAccountingAnalysis), analisysFilter.GetPredicate()));

                if (analysisCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempDailyAccountingAnalysis tempDailyAccountingAnalysisEntity in analysisCollection)
                    {
                        DailyAccountingAnalysisCode tempAnalysisCode = _tempDailyAccountingAnalysisCodeDAO.GetTempAccountingAnalysisCode(tempDailyAccountingAnalysisEntity.TempDailyAccountingAnalysisId);
                        tempAnalysisCodes.Add(tempAnalysisCode);
                    }
                }

                return tempAnalysisCodes.ToDTOs().ToList();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion TempDailyAccountingAnalysis

        #region TempDailyAccountingCostCenter

        /// <summary>
        /// GetTempDailyAccountingCostCentersByTempDailyAccountingTransactionItemId
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns>List<DailyAccountingCostCenter></returns>
        private List<DailyAccountingCostCenterDTO> GetTempDailyAccountingCostCentersByTempDailyAccountingTransactionItemId(int tempDailyAccountingTransactionItemId)
        {

            try
            {

                List<DailyAccountingCostCenter> tempCostCenters = new List<DailyAccountingCostCenter>();

                ObjectCriteriaBuilder costCenterFilter = new ObjectCriteriaBuilder();
                costCenterFilter.PropertyEquals(ACCOUNTINGEN.TempDailyAccountingCostCenter.Properties.TempDailyAccountingTransCode, tempDailyAccountingTransactionItemId);

                BusinessCollection costCenterCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempDailyAccountingCostCenter), costCenterFilter.GetPredicate()));

                if (costCenterCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenterEntity in costCenterCollection)
                    {
                        DailyAccountingCostCenter tempCostCenter = _tempDailyAccountingCostCenterDAO.GetTempDailyAccountingCostCenter(tempDailyAccountingCostCenterEntity.TempDailyAccountingCostCenterId);
                        tempCostCenters.Add(tempCostCenter);
                    }
                }

                return tempCostCenters.ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion TempDailyAccountingCostCenter

        #region DailyAccountingTransactionItem

        /// <summary>
        /// SaveDailyAccountingTransactionItem
        /// </summary>
        /// <param name="dailyAccountingTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        private int SaveDailyAccountingTransactionItem(DailyAccountingTransactionItemDTO dailyAccountingTransactionItem, int imputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount)
        {
            int dailyAccountingTransactionItemId = 0;

            try
            {
                dailyAccountingTransactionItemId = _dailyAccountingTransactionItemDAO.SaveDailyAccountingTransactionItem(dailyAccountingTransactionItem.ToModel(), imputationId, paymentConceptCode, description, bankReconciliationId, receiptNumber, receiptDate, postdatedAmount);

                //grabación de analisis.
                if (dailyAccountingTransactionItem.DailyAccountingAnalysisCodes.Count > 0)
                {
                    foreach (DailyAccountingAnalysisCodeDTO dailyAccountingAnalysisCode in dailyAccountingTransactionItem.DailyAccountingAnalysisCodes)
                    {
                        dailyAccountingAnalysisCode.Id = 0; //es clave autonumérica
                        _dailyAccountingAnalysisCodeDAO.SaveDailyAccountingAnalysisCode(dailyAccountingAnalysisCode.ToModel(), dailyAccountingTransactionItemId);
                    }
                }

                if (dailyAccountingTransactionItem.DailyAccountingCostCenters.Count > 0)
                {
                    foreach (DailyAccountingCostCenterDTO dailyAccountingCostCenter in dailyAccountingTransactionItem.DailyAccountingCostCenters)
                    {
                        dailyAccountingCostCenter.Id = 0; //es clave autonumérica
                        _dailyAccountingCostCenterDAO.SaveDailyAccountingCostCenter(dailyAccountingCostCenter.ToModel(), dailyAccountingTransactionItemId);
                    }
                }

            }
            catch (BusinessException)
            {
                dailyAccountingTransactionItemId = 0;
            }

            return dailyAccountingTransactionItemId;
        }

        /// <summary>
        /// FormatDateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>DateTime</returns>
        private DateTime FormatDateTime(string dateTime)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                IFormatProvider culture = new CultureInfo("es-EC", true);

                return Convert.ToDateTime(dateTime, culture);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion DailyAccountingTransactionItem

        #region PartialClosing

        #region PartialClosingCommissions
        /// <summary>
        /// PartialClosingCommissions
        /// Cierre parcial de comisiones
        /// </summary>
        /// <returns>bool</returns>

        private bool PartialClosingCommissions(DateTime dateTo, DateTime dateFrom, int userId)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    bool isSaved = false;
                    bool isDiscountedCommissionSaved = false;
                    bool isBrokerSaved = false;

                    DateTime accountingDate = Convert.ToDateTime(Convert.ToString(new AccountingParameterServiceEEProvider().
                                                                 GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)
/*                                                                 ConfigurationManager.AppSettings["ModuleDateAccounting"]*/))).Split()[0]);

                    // Recorre las cobranzas para para ejecutar el proceso de inserción.
                    List<PremiumReceivableTransactionItem> premiumReceivableTransactions = GetPremiumRecievableTransactionPartialClousure(dateTo, dateFrom).ToModels().ToList();

                    if (premiumReceivableTransactions.Count > 0)
                    {
                        foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in premiumReceivableTransactions)
                        {
                            // Obtengo los ramos y subramos
                            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PrefixPremiumReceivable.Properties.PremiumReceivableTransId, Convert.ToInt32(premiumReceivableTransactionItem.Id));

                            decimal exchangeRate = 0;
                            exchangeRate = premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount;

                            decimal amount = 0;
                            amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;

                            UIView receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("PrefixPremiumReceivableView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                            if (receivablePremiums.Count > 0)
                            {
                                foreach (DataRow dataRow in receivablePremiums)
                                {
                                    #region BrokersCommission

                                    if (premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number == 1) // La liberación de comisión solo se lo hace el momento de pagar la primera cuota.
                                    {
                                        bool isReleaseCommissionValidated = false;

                                        // Valida condiciones para liberación de agentes.
                                        isReleaseCommissionValidated = ValidateAgentCommissionRelease(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id);

                                        if (isReleaseCommissionValidated)
                                        {
                                            #region DiscountedCommision

                                            if (Math.Abs(Convert.ToDecimal(premiumReceivableTransactionItem.DeductCommission.Value)) > 0)
                                            {
                                                isDiscountedCommissionSaved = SaveDiscountedCommissionRequest(premiumReceivableTransactionItem.Id, Math.Abs(premiumReceivableTransactionItem.DeductCommission.Value));
                                                if (isDiscountedCommissionSaved)
                                                {
                                                    isSaved = true;
                                                }
                                            }

                                            #endregion

                                            // Obtengo las comisiones de agente
                                            List<SEARCH.BrokerCheckingAccountItemDTO> commissions = GetAgentCommissions(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id,
                                                premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, Convert.ToInt32(dataRow["LineBusinessCode"]), Convert.ToInt32(dataRow["SubLineBusinessCode"]), userId);

                                            CollectImputation collectImputation;
                                            Imputation imputation = new Imputation();

                                            imputation.Id = GetImputationIdByPremiumReceivableId(premiumReceivableTransactionItem.Id);
                                            collectImputation = _imputationDAO.GetImputation(imputation);
                                            imputation = collectImputation.Imputation;

                                            int agentTypeId = 0;
                                            int sourceId = 0;

                                            foreach (SEARCH.BrokerCheckingAccountItemDTO commission in commissions)
                                            {
                                                // Grabación en la tabla de cuentas corrientes de agentes.
                                                BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                                brokersCheckingAccountTransactionItem.Id = 0;
                                                brokersCheckingAccountTransactionItem.InsuredId = commission.InsuredId;
                                                brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                                brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = commission.CommissionDiscounted };
                                                brokersCheckingAccountTransactionItem.Policy = new Policy();
                                                brokersCheckingAccountTransactionItem.Policy.Id = commission.PolicyId;
                                                brokersCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = commission.EndorsementId };
                                                brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                                {
                                                    new Beneficiary()
                                                    {
                                                        IndividualId = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId,
                                                    }
                                                };
                                                brokersCheckingAccountTransactionItem.Policy.ExchangeRate = premiumReceivableTransactionItem.Policy.ExchangeRate;

                                                brokersCheckingAccountTransactionItem.Policy.PaymentPlan = new PaymentPlan();
                                                brokersCheckingAccountTransactionItem.Policy.PaymentPlan.Quotas = new List<Quota>();
                                                Quota quota = new Quota();
                                                quota.Number = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number;
                                                brokersCheckingAccountTransactionItem.Policy.PaymentPlan.Quotas.Add(quota);
                                                brokersCheckingAccountTransactionItem.PrefixId = commission.PrefixCode;
                                                brokersCheckingAccountTransactionItem.AccountingNature = (AccountingNature)commission.AccountNature;
                                                brokersCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = commission.CommissionAmount };
                                                brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = commission.CommissionBalance };
                                                brokersCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = commission.CommissionPercentage };
                                                brokersCheckingAccountTransactionItem.CommissionType = Convert.ToInt32(commission.CommissionTypeCode);
                                                brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = commission.CommissionDiscounted };
                                                brokersCheckingAccountTransactionItem.Holder = new Agent();
                                                brokersCheckingAccountTransactionItem.Holder.FullName = commission.AgentName;
                                                brokersCheckingAccountTransactionItem.Holder.IndividualId = commission.AgentCode;
                                                brokersCheckingAccountTransactionItem.Agencies = new List<Agency>();
                                                brokersCheckingAccountTransactionItem.Agencies.Add(new Agency()
                                                {
                                                    Id = commission.AgentAgencyCode
                                                });
                                                brokersCheckingAccountTransactionItem.Branch = new Branch() { Id = commission.BranchCode };
                                                brokersCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = commission.SalePointCode };
                                                brokersCheckingAccountTransactionItem.Company = new Company() { IndividualId = commission.CompanyCode };
                                                brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = commission.CheckingAccountConceptCode };
                                                brokersCheckingAccountTransactionItem.Amount = new Amount()
                                                {
                                                    Currency = new Currency() { Id = Convert.ToInt32(commission.CurrencyCode) },
                                                    Value = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount
                                                };
                                                brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = commission.ExchangeRate };
                                                brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                                brokersCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = commission.LineBusiness };
                                                brokersCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = commission.SubLineBusiness };
                                                brokersCheckingAccountTransactionItem.IsPayed = Convert.ToBoolean(commission.Payed);
                                                agentTypeId = commission.AgentTypeCode;
                                                sourceId = GetSourceIdByImputation(imputation.ToDTO());

                                                brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);

                                                if (brokersCheckingAccountTransactionItem.Id > 0)
                                                {
                                                    isBrokerSaved = true;
                                                }
                                            }
                                            if (isBrokerSaved)
                                            {
                                                #region CoinsuranceAccepted

                                                if (GetBussinessTypeIdByPremiumReceivableId(premiumReceivableTransactionItem.Id) == 2) // Coaseguro Aceptado
                                                {
                                                    isBrokerSaved = false;

                                                    List<SEARCH.CoinsuranceCheckingAccountItemDTO> acceptedCoinsurances = GetCoinsuredAccepted(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                                    if (acceptedCoinsurances.Count > 0)
                                                    {
                                                        foreach (SEARCH.CoinsuranceCheckingAccountItemDTO acceptedCoinsurance in acceptedCoinsurances)
                                                        {
                                                            List<BrokersCheckingAccountTransactionItem> brokerCheckingAccountTransactions = GetBrokerChekingAccounts(-1, dateFrom, dateTo, acceptedCoinsurance.CoinsurancePolicyId);

                                                            if (brokerCheckingAccountTransactions.Count > 0)
                                                            {
                                                                decimal collectionAmount = 0;
                                                                decimal collectionIncomeAmount = 0;
                                                                foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccount in brokerCheckingAccountTransactions)
                                                                {
                                                                    collectionAmount = collectionAmount + Convert.ToDecimal(brokerCheckingAccount.LocalAmount.Value);
                                                                    collectionIncomeAmount = collectionIncomeAmount + Convert.ToDecimal(brokerCheckingAccount.Amount.Value);
                                                                }

                                                                // Grabación en la tabla de cuentas corrientes de agentes.
                                                                BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                                                brokersCheckingAccountTransactionItem.Id = 0;
                                                                brokersCheckingAccountTransactionItem.InsuredId = Convert.ToInt32(acceptedCoinsurance.InsuredId);
                                                                brokersCheckingAccountTransactionItem.IsAutomatic = true;
                                                                brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTransactions[0].DiscountedCommission.Value) };
                                                                brokersCheckingAccountTransactionItem.Policy = new Policy();
                                                                brokersCheckingAccountTransactionItem.Policy.Id = Convert.ToInt32(acceptedCoinsurance.CoinsurancePolicyId);
                                                                brokersCheckingAccountTransactionItem.Policy.Endorsement = new Endorsement() { Id = Convert.ToInt32(brokerCheckingAccountTransactions[0].Policy.Endorsement.Id) };
                                                                brokersCheckingAccountTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                                                                {
                                                                    new Beneficiary()
                                                                    {
                                                                        IndividualId = brokerCheckingAccountTransactions[0].Policy.DefaultBeneficiaries[0].IndividualId,
                                                                    }
                                                                };
                                                                brokersCheckingAccountTransactionItem.PrefixId = brokerCheckingAccountTransactions[0].PrefixId;
                                                                brokersCheckingAccountTransactionItem.AccountingNature = brokerCheckingAccountTransactions[0].AccountingNature;
                                                                brokersCheckingAccountTransactionItem.CommissionAmount = new Amount() { Value = collectionIncomeAmount };
                                                                brokersCheckingAccountTransactionItem.CommissionBalance = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTransactions[0].CommissionBalance.Value) };
                                                                brokersCheckingAccountTransactionItem.CommissionPercentage = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTransactions[0].CommissionPercentage.Value) };
                                                                brokersCheckingAccountTransactionItem.CommissionType = Convert.ToInt32(brokerCheckingAccountTransactions[0].CommissionType);
                                                                brokersCheckingAccountTransactionItem.DiscountedCommission = new Amount() { Value = Convert.ToDecimal(brokerCheckingAccountTransactions[0].DiscountedCommission.Value) };
                                                                brokersCheckingAccountTransactionItem.Holder = new Agent();
                                                                brokersCheckingAccountTransactionItem.Holder.IndividualId = Convert.ToInt32(brokerCheckingAccountTransactions[0].Holder.IndividualId);
                                                                brokersCheckingAccountTransactionItem.Agencies = new List<Agency>();
                                                                brokersCheckingAccountTransactionItem.Agencies.Add(new Agency() { Id = brokerCheckingAccountTransactions[0].Agencies[0].Id });
                                                                brokersCheckingAccountTransactionItem.Branch = new Branch() { Id = brokerCheckingAccountTransactions[0].Branch.Id };
                                                                brokersCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = brokerCheckingAccountTransactions[0].SalePoint.Id };
                                                                brokersCheckingAccountTransactionItem.Company = new Company() { IndividualId = (int)brokerCheckingAccountTransactions[0].Company.IndividualId };
                                                                brokersCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = brokerCheckingAccountTransactions[0].CheckingAccountConcept.Id };
                                                                brokersCheckingAccountTransactionItem.Amount = new Amount()
                                                                {
                                                                    Currency = new Currency() { Id = Convert.ToInt32(brokerCheckingAccountTransactions[0].Amount.Currency.Id) },
                                                                    Value = collectionIncomeAmount
                                                                };
                                                                brokersCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = brokerCheckingAccountTransactions[0].ExchangeRate.SellAmount };
                                                                brokersCheckingAccountTransactionItem.Prefix = new LineBusiness() { Id = brokerCheckingAccountTransactions[0].PrefixId };
                                                                brokersCheckingAccountTransactionItem.SubPrefix = new SubLineBusiness() { Id = brokerCheckingAccountTransactions[0].SubPrefix.Id };
                                                                brokersCheckingAccountTransactionItem.IsPayed = true;
                                                                sourceId = GetSourceIdByImputation(imputation.ToDTO());

                                                                brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);
                                                                if (brokersCheckingAccountTransactionItem.Id > 0)
                                                                {
                                                                    isBrokerSaved = true;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    List<BrokersCheckingAccountTransactionItem> brokerCheckingAccounts = new List<BrokersCheckingAccountTransactionItem>();
                                                    brokerCheckingAccounts = GetBrokerChekingAccountCollections(Convert.ToInt32(premiumReceivableTransactionItem.Policy.Id), dateFrom, dateTo);

                                                    if (brokerCheckingAccounts.Count > 0)
                                                    {
                                                        foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccountCollection in brokerCheckingAccounts)
                                                        {
                                                            // Grabación en la tabla de cuentas corrientes de agentes.
                                                            BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem = new BrokersCheckingAccountTransactionItem();

                                                            brokersCheckingAccountTransactionItem = brokerCheckingAccountCollection;
                                                            sourceId = GetSourceIdByImputation(imputation.ToDTO());

                                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);
                                                            if (brokersCheckingAccountTransactionItem.Id > 0)
                                                            {
                                                                isBrokerSaved = true;
                                                            }
                                                        }
                                                    }
                                                }

                                                #endregion

                                                #region CoinsuranceGiven

                                                if (GetBussinessTypeIdByPremiumReceivableId(premiumReceivableTransactionItem.Id) == 3) // Coaseguro Cedido 
                                                {
                                                    isBrokerSaved = false;
                                                    List<SEARCH.CoinsuranceCheckingAccountItemDTO> asignedCoinsurances = GetCoinsuredAsigned(premiumReceivableTransactionItem.Policy.Id, premiumReceivableTransactionItem.Policy.Endorsement.Id, premiumReceivableTransactionItem.DeductCommission.Value, exchangeRate, amount);

                                                    foreach (SEARCH.CoinsuranceCheckingAccountItemDTO asignedCoinsurance in asignedCoinsurances)
                                                    {
                                                        CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccountTransactionItem = new CoInsuranceCheckingAccountTransactionItem();

                                                        coinsuranceCheckingAccountTransactionItem.AccountingNature = AccountingNature.Debit;
                                                        coinsuranceCheckingAccountTransactionItem.CoInsuranceType = CoInsuranceTypes.Given;
                                                        coinsuranceCheckingAccountTransactionItem.Amount = new Amount()
                                                        {
                                                            Currency = new Currency() { Id = Convert.ToInt32(asignedCoinsurance.CurrencyCode) },
                                                            Value = Convert.ToDecimal(asignedCoinsurance.Amount)
                                                        };
                                                        coinsuranceCheckingAccountTransactionItem.Branch = new Branch() { Id = Convert.ToInt32(asignedCoinsurance.BranchCode) };
                                                        coinsuranceCheckingAccountTransactionItem.CheckingAccountConcept = new CheckingAccountConcept() { Id = Convert.ToInt32(asignedCoinsurance.CheckingAccountConceptCode) };
                                                        coinsuranceCheckingAccountTransactionItem.Comments = "";
                                                        coinsuranceCheckingAccountTransactionItem.Company = new Company() { IndividualId = Convert.ToInt32(asignedCoinsurance.CompanyCode) };
                                                        coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint() { Id = Convert.ToInt32(asignedCoinsurance.SalePointCode) };
                                                        coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(asignedCoinsurance.ExchangeRate) };
                                                        coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(asignedCoinsurance.CoinsuranceCompanyCode) };
                                                        coinsuranceCheckingAccountTransactionItem.Policy = new Policy();
                                                        coinsuranceCheckingAccountTransactionItem.Policy.Id = asignedCoinsurance.CoinsurancePolicyId;
                                                        coinsuranceCheckingAccountTransactionItem.AccountingDate = accountingDate;
                                                        coinsuranceCheckingAccountTransactionItem.LineBusiness = new LineBusiness() { Id = Convert.ToInt32(asignedCoinsurance.LineBusinessCode) };
                                                        coinsuranceCheckingAccountTransactionItem.SubLineBusiness = new SubLineBusiness() { Id = Convert.ToInt32(asignedCoinsurance.SubLineBusinessCode) };
                                                        coinsuranceCheckingAccountTransactionItem.AdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(asignedCoinsurance.AdministrativeExpenses) };
                                                        coinsuranceCheckingAccountTransactionItem.TaxAdministrativeExpenses = new Amount() { Value = Convert.ToDecimal(asignedCoinsurance.TaxAdministrativeExpenses) };
                                                        coinsuranceCheckingAccountTransactionItem.ExtraCommission = new Amount() { Value = 0 };
                                                        coinsuranceCheckingAccountTransactionItem.OverCommission = new Amount() { Value = 0 };

                                                        // GRABA A REALES
                                                        coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputation.Id, 0);

                                                        if (coinsuranceCheckingAccountTransactionItem.Id > 0)
                                                        {
                                                            isBrokerSaved = true;
                                                            // punto 5.10

                                                            SEARCH.AgentCoinsuranceCheckingAccountDTO agentCoinsuranceCheckingAccountDto = new SEARCH.AgentCoinsuranceCheckingAccountDTO();
                                                            agentCoinsuranceCheckingAccountDto.AgentCode = asignedCoinsurance.AgentId;
                                                            agentCoinsuranceCheckingAccountDto.AgentTypeCode = agentTypeId;
                                                            agentCoinsuranceCheckingAccountDto.CommissionAmount = asignedCoinsurance.AgentCommissionAmount;
                                                            agentCoinsuranceCheckingAccountDto.CurrencyCode = asignedCoinsurance.CurrencyCode;
                                                            agentCoinsuranceCheckingAccountDto.IncomeCommissionAmount = asignedCoinsurance.AgentCommissionIncomeAmount;

                                                            isBrokerSaved = _agentCoinsuranceCheckingAccountDAO.SaveAgentCoinsuranceCheckingAccount(agentCoinsuranceCheckingAccountDto);
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
                                                        coinsuranceCheckingAccountTransactionItem.SalePoint = new SalePoint();
                                                        coinsuranceCheckingAccountTransactionItem.SalePoint.Id = Convert.ToInt32(acceptedCoinsurance.SalePointCode);
                                                        coinsuranceCheckingAccountTransactionItem.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(acceptedCoinsurance.ExchangeRate) };
                                                        coinsuranceCheckingAccountTransactionItem.Holder = new Company() { IndividualId = Convert.ToInt32(0) };
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
                                                        coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputation.Id, 0);

                                                        if (coinsuranceCheckingAccountTransactionItem.Id > 0)
                                                        {
                                                            isBrokerSaved = true;
                                                        }
                                                    }
                                                }

                                                #endregion
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                        }

                        if (isBrokerSaved)
                        {
                            UIView brokerCheckingAccountAgents = _dataFacadeManager.GetDataFacade().GetView("BrokerCheckingAccountView", null, null, 0, -1, null, true, out int rows);

                            foreach (DataRow dataRow in brokerCheckingAccountAgents)
                            {
                                decimal commissionPeriod = 0;

                                List<BrokersCheckingAccountTransactionItem> brokerCheckingAccounts;

                                List<CoInsuranceCheckingAccountTransactionItem> coinsuranceCheckingAccounts;

                                SEARCH.BrokerBalanceDTO brokerBalance = new SEARCH.BrokerBalanceDTO();
                                SEARCH.CoinsuranceBalanceDTO coinsuranceBalanceDto = new SEARCH.CoinsuranceBalanceDTO();

                                SEARCH.BrokerBalanceDTO brokerBalancePreviousPeriod;

                                SEARCH.BrokerBalanceDTO brokerBalancePartial;

                                SEARCH.CoinsuranceBalanceDTO coinsuranceBalancePreviosPeriod;
                                SEARCH.CoinsuranceBalanceDTO coinsuranceBalancePartial;

                                #region BrokerBalance

                                brokerCheckingAccounts = GetBrokerChekingAccounts(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo, -1);

                                if (brokerCheckingAccounts.Count > 0)
                                {
                                    brokerBalance.AgentTypeCode = Convert.ToInt32(brokerCheckingAccounts[0].Holder.FullName);
                                    brokerBalance.AgentCode = brokerCheckingAccounts[0].Holder.IndividualId;
                                    brokerBalance.CurrencyId = brokerCheckingAccounts[0].Amount.Currency.Id;
                                    brokerBalance.LastBalanceDate = accountingDate;

                                    foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccount in brokerCheckingAccounts)
                                    {
                                        commissionPeriod = commissionPeriod + (brokerCheckingAccount.CommissionAmount.Value - brokerCheckingAccount.DiscountedCommission.Value);
                                    }

                                    brokerBalancePreviousPeriod = GetBrokerBalancePreviousPeriod(Convert.ToInt32(dataRow["AgentId"]), dateFrom);
                                    brokerBalancePartial = GetBrokerBalanceByAgentAndDate(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo);

                                    if (brokerBalancePreviousPeriod.BrokerBalanceId > 0)
                                    {
                                        if (brokerBalancePartial.BrokerBalanceId > 0)
                                        {
                                            brokerBalance.BrokerBalanceId = brokerBalancePartial.BrokerBalanceId;
                                            brokerBalance.PartialBalanceAmount = commissionPeriod + brokerBalancePreviousPeriod.PartialBalanceAmount;
                                            brokerBalance.PartialBalanceIncomeAmount = commissionPeriod + brokerBalancePreviousPeriod.PartialBalanceIncomeAmount;
                                            brokerBalance.TaxPartialSum = brokerBalancePreviousPeriod.TaxPartialSum;
                                            brokerBalance.TaxPartialSubtraction = brokerBalancePreviousPeriod.TaxPartialSubtraction;
                                            brokerBalance.BalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate) > dateTo ? dateTo : dateFrom;
                                            isSaved = _brokerBalanceDAO.UpdateBrokerBalance(brokerBalance);
                                        }
                                    }
                                    else
                                    {
                                        if (brokerBalancePartial.BrokerBalanceId <= 0)
                                        {
                                            brokerBalance.BrokerBalanceId = 0;
                                            brokerBalance.PartialBalanceAmount = commissionPeriod;
                                            brokerBalance.PartialBalanceIncomeAmount = commissionPeriod;
                                            brokerBalance.TaxPartialSum = 0;
                                            brokerBalance.TaxPartialSubtraction = 0;
                                            brokerBalance.BalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate) > dateTo ? dateTo : dateFrom;
                                            isSaved = _brokerBalanceDAO.SaveBrokerBalance(brokerBalance);
                                        }
                                    }
                                }

                                #endregion

                                #region CoinsuranceBalance

                                coinsuranceCheckingAccounts = GetCoinsuranceChekingAccounts(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo).ToModels().ToList();

                                if (coinsuranceCheckingAccounts.Count > 0)
                                {
                                    coinsuranceBalanceDto.CoinsuredCompanyId = coinsuranceCheckingAccounts[0].Holder.IndividualId;
                                    coinsuranceBalanceDto.CurrencyId = coinsuranceCheckingAccounts[0].Amount.Currency.Id;
                                    coinsuranceBalanceDto.LastBalanceDate = accountingDate;

                                    foreach (CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccount in coinsuranceCheckingAccounts)
                                    {
                                        commissionPeriod = commissionPeriod + (coinsuranceCheckingAccount.Amount.Value - coinsuranceCheckingAccount.AdministrativeExpenses.Value - coinsuranceCheckingAccount.TaxAdministrativeExpenses.Value);
                                    }

                                    coinsuranceBalancePreviosPeriod = GetCoinsuraceBalancePreviousPeriod(Convert.ToInt32(dataRow["AgentId"]), dateFrom);
                                    coinsuranceBalancePartial = GetCoinsuraceBalanceByAgentAndDate(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo);

                                    if (coinsuranceBalancePreviosPeriod.CoinsuranceBalanceId > 0)
                                    {
                                        if (coinsuranceBalancePartial.CoinsuranceBalanceId > 0)
                                        {
                                            coinsuranceBalanceDto.CoinsuranceBalanceId = coinsuranceBalancePartial.CoinsuranceBalanceId;
                                            coinsuranceBalanceDto.BalanceAmount = commissionPeriod + coinsuranceBalancePreviosPeriod.BalanceAmount;
                                            coinsuranceBalanceDto.BalanceIncomeAmount = commissionPeriod + coinsuranceBalancePreviosPeriod.BalanceIncomeAmount;
                                            coinsuranceBalanceDto.BalanceDate = Convert.ToDateTime(coinsuranceBalanceDto.LastBalanceDate) > dateTo ? dateTo : dateFrom;

                                            isSaved = _coinsuranceBalanceDAO.UpdateCoinsuranceBalance(coinsuranceBalanceDto);
                                        }
                                    }
                                    else
                                    {
                                        if (coinsuranceBalancePartial.CoinsuranceBalanceId <= 0)
                                        {
                                            coinsuranceBalanceDto.CoinsuranceBalanceId = 0;
                                            coinsuranceBalanceDto.BalanceAmount = commissionPeriod;
                                            coinsuranceBalanceDto.BalanceIncomeAmount = commissionPeriod;
                                            coinsuranceBalanceDto.BalanceDate = Convert.ToDateTime(coinsuranceBalanceDto.LastBalanceDate) > dateTo ? dateTo : dateFrom;

                                            isSaved = _coinsuranceBalanceDAO.SaveCoinsuranceBalance(coinsuranceBalanceDto);
                                        }
                                    }
                                }

                                #endregion
                            }
                        }

                        #region AgentCommissionClousure

                        SEARCH.AgentCommissionClosureDTO agentCommissionClousureDTO = new SEARCH.AgentCommissionClosureDTO();
                        if (isSaved)
                        {
                            agentCommissionClousureDTO.UserId = userId;
                            agentCommissionClousureDTO.StartDate = dateFrom;
                            agentCommissionClousureDTO.EndDate = dateTo;
                            agentCommissionClousureDTO.RegisterDate = DateTime.Now;
                            agentCommissionClousureDTO.Status = 1; // En 1 si se ejecuto correctamente
                        }
                        else
                        {
                            agentCommissionClousureDTO.UserId = userId;
                            agentCommissionClousureDTO.StartDate = dateFrom;
                            agentCommissionClousureDTO.EndDate = dateTo;
                            agentCommissionClousureDTO.RegisterDate = DateTime.Now;
                            agentCommissionClousureDTO.Status = 0; // En 0 si hay error
                        }

                        _agentCommissionClosureDAO.SaveAgentCommissionClosure(agentCommissionClousureDTO);
                    }

                    #endregion

                    transaction.Complete();

                    return isSaved;
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        #endregion

        #region ReleaseCommissionsProrate
        /// <summary>
        /// ReleaseCommissionsProrate
        /// Liberacion de comisiones a prorrata
        /// </summary>
        /// <returns>bool</returns>

        private bool ReleaseCommissionsProrate(DateTime dateTo, DateTime dateFrom, int userId)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    bool isSaved = false;
                    bool isDiscountedCommissionSaved = false;

                    DateTime accountingDate = Convert.ToDateTime(Convert.ToString(new AccountingParameterServiceEEProvider().
                                                                 GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)
/*                                                                 ConfigurationManager.AppSettings["ModuleDateAccounting"]*/))).Split()[0]);

                    // Recorre las cobranzas para para ejecutar el proceso de inserción.
                    List<PremiumReceivableTransactionItem> premiumReceivableTransactions = GetPremiumRecievableTransactionPartialClousure(dateTo, dateFrom).ToModels().ToList();

                    if (premiumReceivableTransactions.Count > 0)
                    {
                        foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in premiumReceivableTransactions)
                        {
                            #region DiscountedCommision

                            if (Math.Abs(Convert.ToDecimal(premiumReceivableTransactionItem.DeductCommission.Value)) > 0)
                            {
                                isDiscountedCommissionSaved = SaveDiscountedCommissionRequest(premiumReceivableTransactionItem.Id, Math.Abs(premiumReceivableTransactionItem.DeductCommission.Value));
                                if (isDiscountedCommissionSaved)
                                {
                                    isSaved = true;
                                }
                            }

                            #endregion

                            #region BrokersCommission
                            CollectImputation collectImputation;
                            Imputation imputation = new Imputation();

                            imputation.Id = GetImputationIdByPremiumReceivableId(premiumReceivableTransactionItem.Id);
                            collectImputation = _imputationDAO.GetImputation(imputation);
                            imputation = collectImputation.Imputation;

                            int sourceId = GetSourceIdByImputation(imputation.ToDTO());

                            isSaved = BrokersCommission(premiumReceivableTransactionItem.ToDTO(), imputation.ToDTO(), sourceId);

                            #endregion
                        }

                        if (isSaved)
                        {
                            UIView brokerCheckingAccountAgents = _dataFacadeManager.GetDataFacade().GetView("BrokerCheckingAccountView", null, null, 0, -1, null, true, out int rows);

                            foreach (DataRow dataRow in brokerCheckingAccountAgents)
                            {
                                decimal commissionPeriod = 0;

                                List<BrokersCheckingAccountTransactionItem> brokerCheckingAccounts;

                                List<CoInsuranceCheckingAccountTransactionItem> coinsuranceCheckingAccounts;

                                SEARCH.BrokerBalanceDTO brokerBalance = new SEARCH.BrokerBalanceDTO();
                                SEARCH.CoinsuranceBalanceDTO coinsuranceBalanceDto = new SEARCH.CoinsuranceBalanceDTO();

                                SEARCH.BrokerBalanceDTO brokerBalancePreviousPeriod;

                                SEARCH.BrokerBalanceDTO brokerBalancePartial;

                                SEARCH.CoinsuranceBalanceDTO coinsuranceBalancePreviosPeriod;
                                SEARCH.CoinsuranceBalanceDTO coinsuranceBalancePartial;

                                #region BrokerBalance

                                brokerCheckingAccounts = GetBrokerChekingAccounts(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo, -1);

                                if (brokerCheckingAccounts.Count > 0)
                                {
                                    brokerBalance.AgentTypeCode = Convert.ToInt32(brokerCheckingAccounts[0].Holder.FullName);
                                    brokerBalance.AgentCode = brokerCheckingAccounts[0].Holder.IndividualId;
                                    brokerBalance.CurrencyId = brokerCheckingAccounts[0].Amount.Currency.Id;
                                    brokerBalance.LastBalanceDate = accountingDate;

                                    foreach (BrokersCheckingAccountTransactionItem brokerCheckingAccount in brokerCheckingAccounts)
                                    {
                                        commissionPeriod = commissionPeriod + (brokerCheckingAccount.CommissionAmount.Value - brokerCheckingAccount.DiscountedCommission.Value);
                                    }

                                    brokerBalancePreviousPeriod = GetBrokerBalancePreviousPeriod(Convert.ToInt32(dataRow["AgentId"]), dateFrom);
                                    brokerBalancePartial = GetBrokerBalanceByAgentAndDate(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo);

                                    if (brokerBalancePreviousPeriod.BrokerBalanceId > 0)
                                    {
                                        if (brokerBalancePartial.BrokerBalanceId > 0)
                                        {
                                            brokerBalance.BrokerBalanceId = brokerBalancePartial.BrokerBalanceId;
                                            brokerBalance.PartialBalanceAmount = commissionPeriod + brokerBalancePreviousPeriod.PartialBalanceAmount;
                                            brokerBalance.PartialBalanceIncomeAmount = commissionPeriod + brokerBalancePreviousPeriod.PartialBalanceIncomeAmount;
                                            brokerBalance.TaxPartialSum = brokerBalancePreviousPeriod.TaxPartialSum;
                                            brokerBalance.TaxPartialSubtraction = brokerBalancePreviousPeriod.TaxPartialSubtraction;
                                            brokerBalance.BalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate) > dateTo ? dateTo : dateFrom;
                                            isSaved = _brokerBalanceDAO.UpdateBrokerBalance(brokerBalance);
                                        }
                                    }
                                    else
                                    {
                                        if (brokerBalancePartial.BrokerBalanceId <= 0)
                                        {
                                            brokerBalance.BrokerBalanceId = 0;
                                            brokerBalance.PartialBalanceAmount = commissionPeriod;
                                            brokerBalance.PartialBalanceIncomeAmount = commissionPeriod;
                                            brokerBalance.TaxPartialSum = 0;
                                            brokerBalance.TaxPartialSubtraction = 0;
                                            brokerBalance.BalanceDate = Convert.ToDateTime(brokerBalance.LastBalanceDate) > dateTo ? dateTo : dateFrom;
                                            isSaved = _brokerBalanceDAO.SaveBrokerBalance(brokerBalance);
                                        }
                                    }
                                }

                                #endregion

                                #region CoinsuranceBalance

                                coinsuranceCheckingAccounts = GetCoinsuranceChekingAccounts(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo).ToModels().ToList();

                                if (coinsuranceCheckingAccounts.Count > 0)
                                {
                                    coinsuranceBalanceDto.CoinsuredCompanyId = coinsuranceCheckingAccounts[0].Holder.IndividualId;
                                    coinsuranceBalanceDto.CurrencyId = coinsuranceCheckingAccounts[0].Amount.Currency.Id;
                                    coinsuranceBalanceDto.LastBalanceDate = accountingDate;

                                    foreach (CoInsuranceCheckingAccountTransactionItem coinsuranceCheckingAccount in coinsuranceCheckingAccounts)
                                    {
                                        commissionPeriod = commissionPeriod + (coinsuranceCheckingAccount.Amount.Value - coinsuranceCheckingAccount.AdministrativeExpenses.Value - coinsuranceCheckingAccount.TaxAdministrativeExpenses.Value);
                                    }

                                    coinsuranceBalancePreviosPeriod = GetCoinsuraceBalancePreviousPeriod(Convert.ToInt32(dataRow["AgentId"]), dateFrom);
                                    coinsuranceBalancePartial = GetCoinsuraceBalanceByAgentAndDate(Convert.ToInt32(dataRow["AgentId"]), dateFrom, dateTo);

                                    if (coinsuranceBalancePreviosPeriod.CoinsuranceBalanceId > 0)
                                    {
                                        if (coinsuranceBalancePartial.CoinsuranceBalanceId > 0)
                                        {
                                            coinsuranceBalanceDto.CoinsuranceBalanceId = coinsuranceBalancePartial.CoinsuranceBalanceId;
                                            coinsuranceBalanceDto.BalanceAmount = commissionPeriod + coinsuranceBalancePreviosPeriod.BalanceAmount;
                                            coinsuranceBalanceDto.BalanceIncomeAmount = commissionPeriod + coinsuranceBalancePreviosPeriod.BalanceIncomeAmount;
                                            coinsuranceBalanceDto.BalanceDate = Convert.ToDateTime(coinsuranceBalanceDto.LastBalanceDate) > dateTo ? dateTo : dateFrom;

                                            isSaved = _coinsuranceBalanceDAO.UpdateCoinsuranceBalance(coinsuranceBalanceDto);
                                        }
                                    }
                                    else
                                    {
                                        if (coinsuranceBalancePartial.CoinsuranceBalanceId <= 0)
                                        {
                                            coinsuranceBalanceDto.CoinsuranceBalanceId = 0;
                                            coinsuranceBalanceDto.BalanceAmount = commissionPeriod;
                                            coinsuranceBalanceDto.BalanceIncomeAmount = commissionPeriod;
                                            coinsuranceBalanceDto.BalanceDate = Convert.ToDateTime(coinsuranceBalanceDto.LastBalanceDate) > dateTo ? dateTo : dateFrom;

                                            isSaved = _coinsuranceBalanceDAO.SaveCoinsuranceBalance(coinsuranceBalanceDto);
                                        }
                                    }
                                }

                                #endregion
                            }
                        }

                        #region AgentCommissionClousure

                        SEARCH.AgentCommissionClosureDTO agentCommissionClousureDTO = new SEARCH.AgentCommissionClosureDTO();
                        if (isSaved)
                        {
                            agentCommissionClousureDTO.UserId = userId;
                            agentCommissionClousureDTO.StartDate = dateFrom;
                            agentCommissionClousureDTO.EndDate = dateTo;
                            agentCommissionClousureDTO.RegisterDate = DateTime.Now;
                            agentCommissionClousureDTO.Status = 1; // En 1 si se ejecuto correctamente
                        }
                        else
                        {
                            agentCommissionClousureDTO.UserId = userId;
                            agentCommissionClousureDTO.StartDate = dateFrom;
                            agentCommissionClousureDTO.EndDate = dateTo;
                            agentCommissionClousureDTO.RegisterDate = DateTime.Now;
                            agentCommissionClousureDTO.Status = 0; // En 0 si hay error
                        }
                        #endregion

                        _agentCommissionClosureDAO.SaveAgentCommissionClosure(agentCommissionClousureDTO);
                    }


                    transaction.Complete();

                    return isSaved;
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        /// <summary>
        /// BrokersCommission
        /// La liberación de las coutas es por cada cobranza
        /// </summary>
        /// <param name="premiumReceivableTransactionItem"></param>
        /// <param name="imputation"></param>
        /// <param name="sourceId"></param>
        /// <returns>bool</returns>
        private bool BrokersCommission(PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem, ImputationDTO imputation, int sourceId)
        {
            bool isSaved = new bool();
            try
            {
                DateTime accountingDate = Convert.ToDateTime(Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]);

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
                            Convert.ToInt32(dataRow["SubLineBusinessCode"]), imputation.UserId);

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
                                Id = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_CHECKING_ACCOUNT_CONCEPTID))
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
                                imputation.Id, 0, sourceId, commision.AgentTypeCode,
                                Convert.ToDateTime(Convert.ToString(new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]));


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
                                            sourceId = GetSourceIdByImputation(imputation);

                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);
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
                                        sourceId = GetSourceIdByImputation(imputation);

                                        brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, imputation.Id, 0, sourceId, agentTypeId, accountingDate);
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
                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputation.Id, 0);

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
                                    coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, imputation.Id, 0);

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
    }
}
#endregion

#endregion

#endregion Private Methods