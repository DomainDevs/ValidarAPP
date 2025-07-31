using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.Application;
using Sistran.Core.Application.AccountingServices.DTOs.Filter;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting.Reversion;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.AccountingServices.Enums;
//Sistran Core
using Sistran.Core.Application.CommonService.Models;
//using Sistran.Core.Application.UnderwritingServices.Models;
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
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using AccountingModels = Sistran.Core.Application.AccountingServices.EEProvider.Models;
using application = Sistran.Core.Application.AccountingServices.DTOs.Imputations;
//using claimsModels = Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
//using PRMOD = Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using COMMEN = Sistran.Core.Application.Common.Entities;
using CoreTransaction = Sistran.Core.Framework.Transactions;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using ImputationModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PaymentMethod = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using PRMOD = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using UNDDTOs = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingApplicationServiceEEProvider : IAccountingApplicationService
    {
        #region Constants

        public const int ModuleId = 2;
        public const int StatusQuotaPartial = 1; // No se crean enum de esto ya que pertenece a Emisión
        public const int StatusQuotaTotal = 2;   // No se crean enum de esto ya que pertenece a Emisión

        //// Variable para lectura de UIViews
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
        readonly TempApplicationPremiumItemDAO tempApplicationPremiumItemDAO = new TempApplicationPremiumItemDAO();
        readonly TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();
        readonly ApplicationDAO applicationDAO = new ApplicationDAO();
        readonly TempApplicationDAO tempApplicationDAO = new TempApplicationDAO();
        //readonly PremiumReceivableTransactionItemDAO _premiumReceivableTransactionItemDAO = new PremiumReceivableTransactionItemDAO();
        readonly ApplicationPremiumItemDAO applicationPremiumItemDAO = new ApplicationPremiumItemDAO();
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
        readonly TempApplicationAccountingItemDAO tempApplicationAccountingItemDAO = new TempApplicationAccountingItemDAO();
        readonly TempApplicationAccountingDAO tempApplicationAccountingDAO = new TempApplicationAccountingDAO();
        readonly ApplicationAccountingItemDAO applicationAccountingItemDAO = new ApplicationAccountingItemDAO();
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
        readonly TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
        readonly TempApplicationAccountingAnalysisDAO tempApplicationAccountingAnalysisDAO = new TempApplicationAccountingAnalysisDAO();
        readonly TempApplicationAccountingCostCenterDAO tempApplicationAccountingCostCenterDAO = new TempApplicationAccountingCostCenterDAO();
        readonly ApplicationAccountingAnalysisDAO applicationAccountingAnalysisDAO = new ApplicationAccountingAnalysisDAO();
        readonly ApplicationAccountingCostCenterDAO applicationAccountingCostCenterDAO = new ApplicationAccountingCostCenterDAO();
        readonly PaymentDAO _paymentDAO = new PaymentDAO();
        readonly PaymentRequestClaimDAO _paymentRequestClaimDAO = new PaymentRequestClaimDAO();
        readonly ApplicationPremiumComponentDAO applicationPremiumComponentDAO = new ApplicationPremiumComponentDAO();
        readonly ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();
        readonly IncomeBusiness _incomeBusiness = new IncomeBusiness();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region ApplicationDTO

        /// <summary>
        /// SaveApplicationRequest
        /// Ejecuta el proceso de grabación de una imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        public bool SaveApplicationRequest(int sourceCode, int tempApplicationId, int moduleId,
                                           string comments, int statusId, int userId, int tempSourceCode, int technicalTransaction, DateTime accountingDate)
        {
            SaveApplication(sourceCode, tempApplicationId, moduleId, comments, statusId, userId, tempSourceCode, technicalTransaction, accountingDate);
            return true;
        }

        /// <summary>
        /// SaveApplication
        /// Graba la imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>CollectApplication</returns>
        public CollectApplicationDTO SaveApplication(int sourceCode, int tempApplicationId, int moduleId,
                                                string comments, int statusId, int userId, int tempSourceCode, int technicalTransaction, DateTime accountingDate)
        {
            /*
            if (userId == 1)
            {
                return new IncomeBusiness().SaveApplicationPortalPayment(sourceCode, tempApplicationId, moduleId,
                    comments, statusId, userId, tempSourceCode, technicalTransaction, accountingDate).ToDTO();
            }*/

            Collect collect = null;
            if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                CollectDAO collectDAO = new CollectDAO();
                collect = collectDAO.GetCollectByCollectId(sourceCode);
            }

            using (Context.Current)
            {
                //CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();
                using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                {
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

                        application = new ApplicationDAO().SaveImputation(application, sourceCode, technicalTransaction);

                        transactionModel.TechnicalTransaction = technicalTransaction;
                        imputationId = application.Id;

                        #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                        // CONVIERTE DE TEMPORALES A TABLAS REALES
                        isConverted = _incomeBusiness.ConvertTempApplicationtoRealApplication(tempSourceCode, application.ModuleId, application.Id, accountingDate, userId);


                        #endregion

                        if (isConverted) // ELIMINA LOS TEMPORALES
                        {
                            //Elimina Temporales
                            CancelAppliationReceipt(tempApplicationId);

                            // Actualiza estatus en Solicitud de pago a pagado
                            UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

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

                                new CollectDAO().UpdateCollect(collect, -1);
                            }

                            #endregion
                        }


                        #region APLICACIÓN DE PRELIQUIDATION

                        if (moduleId == (int)ApplicationTypes.PreLiquidation)
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

                        return new CollectApplication() { Application = application, Transaction = transactionModel }.ToDTO();
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Dispose();

                        throw new BusinessException(Resources.Resources.BusinessException);
                    }
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
                                              int userId, int tempSourceCode, DateTime accountingDate)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error
            try
            {
                bool isConverted = false;
                int imputationId = 0;

                Models.Imputations.Application application = new Models.Imputations.Application();

                DateTime registerDate = DateTime.Now;
                Collect collect = new Collect();

                application.Id = imputationId;
                application.RegisterDate = registerDate;
                application.ModuleId = (int)ApplicationTypes.Collect;
                application.UserId = userId;
                application.AccountingDate = accountingDate;

                int technicalTransaction = GetTechnicalTransaction();
                // Graba la cabecera de Imputación
                application = applicationDAO.SaveImputation(application, sourceCode, technicalTransaction);

                imputationId = application.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                // CONVIERTE DE TEMPORALES A TABLAS REALES
                isConverted = _incomeBusiness.ConvertTempApplicationtoRealApplication(tempSourceCode, application.ModuleId, application.Id, accountingDate, userId);



                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    //Elimina Temporales
                    CancelAppliationReceipt(tempImputationId);

                    // Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);

                    // Imputación



                    #region APLICACIÓN DE RECIBO

                    // ACTUALIZA ESTADO DE BILL


                    string[] accountingDateSplit = Convert.ToString(accountingDate).Split();

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
                                              int userId, int tempSourceCode, int technicalTransaction, DateTime accountingDate)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error
            try
            {
                bool isConverted = false;
                string accountingDatestr;
                int imputationId = 0;

                Models.Imputations.Application application = new Models.Imputations.Application();

                DateTime registerDate = DateTime.Now;
                Collect collect = new Collect();

                application.Id = imputationId;
                application.RegisterDate = registerDate;
                application.ModuleId = (int)ApplicationTypes.Collect;
                application.UserId = userId;

                // Graba la cabecera de Imputación
                application = applicationDAO.SaveImputation(application, sourceCode, technicalTransaction);

                imputationId = application.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                // CONVIERTE DE TEMPORALES A TABLAS REALES
                isConverted = _incomeBusiness.ConvertTempApplicationtoRealApplication(tempSourceCode, application.ModuleId, application.Id, accountingDate, userId);

                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    //Elimina Temporales
                    CancelAppliationReceipt(tempImputationId);

                    // Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);


                    #region APLICACIÓN DE RECIBO

                    // ACTUALIZA ESTADO DE BILL
                    accountingDatestr = Convert.ToString(accountingDate);

                    string[] accountingDateSplit = accountingDatestr.Split();

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
        public bool SaveImputationRequestPaymentOrder(int sourceCode, int tempImputationId, int userId, int tempSourceCode, DateTime accountingDate)
        {
            // Este método fue creado para separlo del SaveImputationRequest por el tema de Transaccionalidad
            // Ojo No se puede tener mas de una transaccionalidad abierta sino da error

            try
            {
                bool isConverted = false;
                int imputationId = 0;
                CollectApplication collectImputation = new CollectApplication();
                Models.Imputations.Application application = new Models.Imputations.Application();
                DateTime registerDate = DateTime.Now;

                application.Id = imputationId;
                application.RegisterDate = registerDate;
                application.UserId = userId;
                application.ModuleId = (int)ApplicationTypes.PaymentOrder;
                application.AccountingDate = accountingDate;

                int technicalTransaction = GetTechnicalTransaction();
                // Graba la cabecera de Imputación
                application = applicationDAO.SaveImputation(application, sourceCode, technicalTransaction);

                imputationId = application.Id;

                #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS

                // CONVIERTE DE TEMPORALES A TABLAS REALES
                isConverted = _incomeBusiness.ConvertTempApplicationtoRealApplication(tempSourceCode, application.ModuleId, application.Id, accountingDate, userId);

                #endregion

                if (isConverted) // ELIMINA LOS TEMPORALES
                {
                    //Elimina Temporales
                    CancelAppliationReceipt(tempImputationId);

                    //Actualiza estatus en Solicitud de pago a pagado
                    UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);
                }

                return true;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveApplicationRequestJournalEntry
        /// Ejecuta el proceso de grabación de una imputación de Asiento de Diario
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>CollectApplication</returns>
        public CollectApplicationDTO SaveApplicationRequestJournalEntry(int tempApplicationId, int userId, int tempSourceCode)
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

                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                if (applicationBusiness.ValidateTempApplication(tempApplicationId))
                {

                    Models.Imputations.Application application = new Models.Imputations.Application()
                    {
                        RegisterDate = registerDate,
                        Id = imputationId,
                        ModuleId = (int)ApplicationTypes.JournalEntry,
                        UserId = userId,
                        AccountingDate = journalEntryHeader.AccountingDate
                    };

                    // Obtiene el número de transacción
                    //int technicalTransaction = GetTechnicalTransaction();
                    TechnicalTransactionParameterDTO technicalTransactionParameterDTO = new TechnicalTransactionParameterDTO();
                    technicalTransactionParameterDTO.BranchId = journalEntryHeader.Branch.Id;
                    TechnicalTransactionDTO technicalTransactionDTO =
                        DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(technicalTransactionParameterDTO);
                    int technicalTransaction = technicalTransactionDTO.Id;

                    // Graba la cabecera de imputación
                    application = applicationDAO.SaveImputation(application, sourceCode, technicalTransaction);

                    imputationId = application.Id;

                    #region TransactionTypes - GRABACION DE LOS 7 MOVIMIENTOS


                    // CONVIERTE DE TEMPORALES A TABLAS REALES
                    isConverted = _incomeBusiness.ConvertTempApplicationtoRealApplication(tempSourceCode, application.ModuleId, application.Id, journalEntryHeader.AccountingDate, userId);

                    #endregion

                    if (isConverted) // ELIMINA LOS TEMPORALES
                    {
                        // Primas por Cobrar
                        //Elimina Temporales
                        CancelAppliationReceipt(tempApplicationId);

                        // Actualiza estatus en Solicitud de pago a pagado
                        UpdatePaymentRequestToStatusPayed(imputationId, userId, DateTime.Now);
                    }

                    _tempJournalEntryDAO.DeleteTempJournalEntry(tempSourceCode);
                    Transaction accountingTransaction = new Transaction()
                    {
                        TechnicalTransaction = technicalTransaction
                    };

                    accountingTransaction.Id = journalEntryHeader.Id;
                    return new CollectApplication() { Application = application, Transaction = accountingTransaction }.ToDTO();
                }
                return new CollectApplication() { Application = new ImputationModels.Application() }.ToDTO();
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
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ApplicationCode, imputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().
                    SelectObjects(typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.Application imputationItem in businessCollection.OfType<ACCOUNTINGEN.Application>())
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

        public bool CancelAppliationReceipt(int tempImputationId, int moduleId = 0)
        {
            try
            {
                return new IncomeBusiness().DeleteTempApplicationByTempAplicationIdModuleId(tempImputationId, moduleId);
            } 
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Resources.ErrorCancelAppliationReceipt, ex);
            } 

        }
        #endregion ApplicationDTO

        #region TempApplication

        /// <summary>
        /// SaveTempApplication
        /// Graba temporal de aplicación
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO SaveTempApplication(ApplicationDTO application, int sourceCode)
        {
            try
            {
                return tempApplicationDAO.SaveTempApplication(application.ToModel(), sourceCode).ToDTO();
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
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO GetTempApplication(ApplicationDTO tempImputation)
        {
            try
            {
                return tempApplicationDAO.GetTempApplication(tempImputation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        /// <summary>
        /// GetTempImputation
        /// Obtiene temporal de Applicacion
        /// </summary>
        /// <param name="tempImputation"></param>
        /// <returns>ApplicationDTO</returns>
        public List<TempApplicationDTO> GetTempApplicationByUserId(int userId)
        {
            try
            {
                return tempApplicationDAO.GetTempApplicationByUserId(userId).ToDTOs().ToList();
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
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplication(int tempApplicationId)
        {
            try
            {
                return tempApplicationDAO.DeleteTempApplication(tempApplicationId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }


        }

        /// <summary>
        /// UpdateTempApplication
        /// Edita un temporal de imputación
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO UpdateTempApplication(ApplicationDTO application, int sourceCode)
        {
            try
            {
                return tempApplicationDAO.UpdateTempApplication(application.ToModel(), sourceCode).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        /// <summary>
        /// UpdateTempApplication
        /// Edita un temporal de imputación
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO UpdateSourceCodeTempApplication(int applicationId, int sourceCode)
        {
            try
            {
                return tempApplicationDAO.UpdateSourceCodeTempApplication(applicationId, sourceCode).ToDTO();
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
        /// GetTempImputationItem
        /// Obtiene un item de imputación temporal
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>TransactionType</returns>
        public TransactionTypeDTO GetTempApplicationItem(int imputationTypeId, int tempImputationId)
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
                            Models.Imputations.Application tempApplicationPremium = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId, imputationTypeId);
                            ApplicationPremiumTransaction tempApplicationPremiumRev = CreatePremiumReversion(tempImputationId, imputationTypeId);
                            //Reversion Primas
                            if (tempApplicationPremium != null && tempApplicationPremium.ApplicationPremiumTransaction.PremiumReceivableItems.Any())
                            {
                                tempApplicationPremium.ApplicationPremiumTransaction.TotalCredit.Value += Math.Abs(tempApplicationPremiumRev.TotalCredit.Value);
                                tempApplicationPremium.ApplicationPremiumTransaction.TotalDebit.Value += Math.Abs(tempApplicationPremiumRev.TotalDebit.Value);
                                tempApplicationPremium.ApplicationPremiumTransaction.PremiumReceivableItems.AddRange(tempApplicationPremiumRev.PremiumReceivableItems);
                            }
                            else
                            {
                                tempApplicationPremium.ApplicationPremiumTransaction = tempApplicationPremiumRev;
                            }
                            //Fin Reversion
                            transactionType = tempApplicationPremium.ApplicationPremiumTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.CheckingAccountBrokers:
                        {
                            Models.Imputations.Application tempBrokersCheckingAccountTransaction = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId);
                            transactionType = tempBrokersCheckingAccountTransaction.BrokersCheckingAccountTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.CheckingAccountCoinsurances:
                        {
                            Models.Imputations.Application tempCoinsuranceCheckingAccountTransaction = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId);
                            transactionType = tempCoinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.CheckingAccountReinsurances:
                        {
                            Models.Imputations.Application tempReinsuranceCheckingAccountTransaction = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId);
                            transactionType = tempReinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.Accounting:
                        {
                            Models.Imputations.Application tempDailyAccountingTransaction = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId);
                            transactionType = tempDailyAccountingTransaction.DailyAccountingTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.PaymentSuppliers:
                        {
                            Models.Imputations.Application paymentRequestTransaction = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId);
                            transactionType = paymentRequestTransaction.PaymentRequestTransaction;
                        }
                        break;
                    case (int)ImputationItemTypes.PaymentClaims:
                        {
                            Models.Imputations.Application claimsPaymentRequestTransaction = _incomeBusiness.GetTempApplicationByTempApplicationId(tempImputationId, 0, 2);
                            transactionType = claimsPaymentRequestTransaction.ClaimsPaymentRequestTransaction;
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
        /// Creates the premium reversion.
        /// </summary>
        /// <param name="tempImputationId">The temporary imputation identifier.</param>
        /// <param name="imputationTypeId">The imputation type identifier.</param>
        /// <returns></returns>
        private ApplicationPremiumTransaction CreatePremiumReversion(int tempImputationId, int imputationTypeId)
        {
            //Reversion Primas
            ApplicationPremiumTransaction tempApplicationPremiumRev = TempApplicationPremiumReversionDAO.GetTempApplicationPremiumByTempApplicationId(tempImputationId);
            DebitCreditDTO debitCreditDTO = ApplicationPremiumBusiness.CalculatePremium(imputationTypeId, tempApplicationPremiumRev.PremiumReceivableItems);
            tempApplicationPremiumRev.TotalDebit = new Amount { Value = debitCreditDTO.Debit };
            tempApplicationPremiumRev.TotalCredit = new Amount { Value = debitCreditDTO.Credit };
            return tempApplicationPremiumRev;
        }

        /// <summary>
        /// GetDebitsAndCreditsMovementTypes
        /// Obtiene el total de débitos y créditos de los tipos de movimientos
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="amountValue"></param>
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO GetDebitsAndCreditsMovementTypes(ApplicationDTO application, decimal amountValue)
        {
            try
            {
                Amount amount = new Amount();
                Models.Imputations.Application applicationModel = new Models.Imputations.Application();
                applicationModel.ApplicationItems = new List<TransactionType>();
                decimal debit = 0;
                decimal credit = 0;

                for (int i = 1; i <= 9; i++)
                {
                    TransactionType transactionType = GetTempApplicationItem(i, application.Id).ToModel();

                    applicationModel.ApplicationItems.Add(transactionType);

                    if (applicationModel.ApplicationItems[i - 1] != null)
                    {
                        if ((applicationModel.ApplicationItems[i - 1].TotalDebit.Value != 0))
                        {
                            debit += Math.Abs(Convert.ToDecimal(applicationModel.ApplicationItems[i - 1].TotalDebit.Value));
                        }

                        if ((applicationModel.ApplicationItems[i - 1].TotalCredit.Value != 0))
                        {
                            credit += Math.Abs(Convert.ToDecimal(applicationModel.ApplicationItems[i - 1].TotalCredit.Value));
                        }
                    }
                }

                amount.Value = amountValue + debit - credit;

                applicationModel.Id = application.Id;
                applicationModel.UserId = application.UserId;
                applicationModel.RegisterDate = application.RegisterDate;
                applicationModel.VerificationValue = amount;

                return applicationModel.ToDTO();
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
        /// <param name="tempApplicationId"></param>
        /// <param name="sourceId"></param>
        /// <returns>int</returns>
        public int UpdateTempApplicationSourceCode(int tempApplicationId, int sourceId)
        {
            try
            {

                Models.Imputations.Application application = new Models.Imputations.Application() { Id = tempApplicationId };

                application = _incomeBusiness.GetTempApplicationByTempApplicationId(tempApplicationId, 0, 0, false); 
                application.IsTemporal = true;
                application = tempApplicationDAO.UpdateTempApplication(application, sourceId);
                return application.Id;
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
        public List<ApplicationTypeDTO> GetApplicationTypes()
        {
            List<ApplicationTypeDTO> applicationTypes = new List<ApplicationTypeDTO>();
            try
            {
                foreach (ApplicationTypes type in Enum.GetValues(typeof(ApplicationTypes)))
                {
                    applicationTypes.Add(new ApplicationTypeDTO()
                    {
                        Id = (int)type,
                        Description = Resources.Resources.ResourceManager.GetString(type.ToString())
                    });
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return applicationTypes;
        }

        /// <summary>
        /// DeleteTemporaryApplicationRequest
        /// Borra los temporales de imputación
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>bool</returns>
        public bool DeleteTemporaryApplicationRequest(int tempApplicationId, int moduleId, int sourceCode)
        {
            bool isDeleted = false;

            try
            {
                // Primas por Cobrar
                DeleteTempApplicationPremiumTransaction(tempApplicationId);

                // Cta. Cte. Agentes
                _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountItemByTempImputationId(tempApplicationId);
                _tempBrokerCheckingAccountTransactionItemDAO.DeleteTempBrokerCheckingAccountByTempImputationId(tempApplicationId);

                // Cta. Cte. Coaseguros
                _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountItemByTempImputationId(tempApplicationId);
                _tempCoinsuranceCheckingAccountTransactionItemDAO.DeleteTempCoinsuranceCheckingAccountByTempImputationId(tempApplicationId);

                // Cta. Cte. Reaseguros
                _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountItemByTempImputationId(tempApplicationId);
                _tempReinsuranceCheckingAccountTransactionItemDAO.DeleteTempReinsuranceCheckingAccountByTempImputationId(tempApplicationId);

                // Borra temporales de transacciones solicitudes de pago de siniestros
                tempApplicationPremiumDAO.DeleteTempApplicationPremiumByTempApplication(tempApplicationId);

                // Borra temporales solicitudes de pago de siniestros
                _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempApplicationId);

                // Contabilidad
                tempApplicationAccountingItemDAO.DeleteTempApplicationAccountingsByTempApplicationId(tempApplicationId);

                // Borra temporales Reversion de Primas
                TempApplicationPremiumItemDAO.DeleteTempPremiumReversionTransactionItem(tempApplicationId);

                // Imputación
                tempApplicationDAO.DeleteTempApplication(tempApplicationId);

                if (sourceCode > 0)
                {
                    // Asiento de diario
                    if (moduleId == Convert.ToInt32(ApplicationTypes.JournalEntry))
                    {
                        _tempJournalEntryDAO.DeleteTempJournalEntry(sourceCode);
                    }
                    // Preliquidación
                    else if (moduleId == Convert.ToInt32(ApplicationTypes.PreLiquidation))
                    {
                        PreLiquidation temPreliquidation = new PreLiquidation();
                        temPreliquidation.Id = sourceCode;
                        _tempPreLiquidationDAO.DeleteTempPreLiquidation(temPreliquidation);
                    }
                    // Órdenes de pago
                    else if (moduleId == Convert.ToInt32(ApplicationTypes.PaymentOrder))
                    {
                        _tempPaymentOrderDAO.DeleteTempPaymentOrder(sourceCode);
                    }
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
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="sourceId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        public bool RecalculatingForeignCurrencyAmount(int tempApplicationId, int moduleId, int sourceId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = false;

            try
            {
                // Primas por Cobrar
                RecalculatingForeignCurrencyAmountTempPremiumRecievable(tempApplicationId, foreignCurrencyExchangeRates);

                // Cta. Cte. Agentes
                RecalculatingForeignCurrencyAmountTempBrokerCheckingAccount(tempApplicationId, foreignCurrencyExchangeRates);

                // Cta. Cte. Coaseguros
                RecalculatingForeignCurrencyAmountTempCoinsuranceCheckingAccount(tempApplicationId, foreignCurrencyExchangeRates);

                // Cta. Cte. Reaseguros
                RecalculatingForeignCurrencyAmountTempReinsuranceCheckingAccount(tempApplicationId, foreignCurrencyExchangeRates);

                // Solicitudes de pago de siniestros
                RecalculatingForeignCurrencyAmountTempClaimPayment(tempApplicationId, foreignCurrencyExchangeRates);

                // Contabilidad
                RecalculatingForeignCurrencyAmountTempDailyAccounting(tempApplicationId, foreignCurrencyExchangeRates);

                // Órdenes de pago
                if (moduleId == Convert.ToInt32(ApplicationTypes.PaymentOrder))
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

        #region ApplicationTypes

        // Primas por Cobrar
        #region PremiumsReceivable

        ///<summary>
        /// SaveTempPremiumRecievableTransaction
        /// Graba una prima por cobrar
        /// </summary>
        /// <param name="premiumRecievableTransaction"></param>
        /// <param name="applicationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>int</returns>
        public int SaveTempPremiumRecievableTransaction(PremiumReceivableTransactionDTO premiumRecievableTransaction, int applicationId, decimal exchangeRate, int userId, DateTime registerDate, DateTime accountingDate)
        {
            int recorded = 0;

            try
            {
                foreach (PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem in premiumRecievableTransaction.PremiumReceivableItems)
                {
                    if (premiumReceivableTransactionItem.Id == 0)
                    {
                        tempApplicationPremiumItemDAO.SaveTempPremiumRecievableTransactionItem(premiumReceivableTransactionItem.ToModel(), applicationId, exchangeRate, userId, registerDate, accountingDate).ToDTO();
                    }
                    else
                    {
                        tempApplicationPremiumItemDAO.UpdateTempPremiumReceivableTransactionItem(premiumReceivableTransactionItem.ToModel(), applicationId, exchangeRate, userId, registerDate, accountingDate).ToDTO();
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
        /// SaveTempApplicationPremiumComponents
        /// Graba una prima por cobrar
        /// </summary>
        /// <param name="tempApplicationPremiumDTO"></param>
        /// <returns>int</returns>
        public bool SaveTempApplicationPremiumComponents(TempApplicationPremiumDTO tempApplicationPremiumDTO)
        {
            try
            {
                TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                return tempApplicationBusiness.SaveTempApplicationPremiumComponents(tempApplicationPremiumDTO.ToModel(),
                    tempApplicationPremiumDTO.Tax, tempApplicationPremiumDTO.NoExpenses);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorSaveTempApplicationPremiumComponents);
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
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(0);
                }

                if (insuredDocumentNumber != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.InsuredDocumentNumber, (insuredDocumentNumber));
                }

                if (insuredId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(Issuance.Entities.Policy.Properties.PolicyholderId, Convert.ToInt64(insuredId));

                    //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.InsuredIndividualId, Convert.ToInt64(insuredId));
                }

                if (payerId != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(Issuance.Entities.PayerPayment.Properties.PayerId, Convert.ToInt64(payerId));
                    //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PayerIndividualId, Convert.ToInt32(payerId));
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
        public List<SEARCH.PremiumReceivableItemDTO> GetTempApplicationPremiumByApplicationId(int tempImputationId)
        {
            try
            {
                return TempApplicationPremiumItemDAO.GetTempApplicationPremiumByApplicationId(tempImputationId);
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

                DateTime accountingDate = Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)))).Split()[0]);

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
                            decimal localAmount = 0;
                            decimal collectionAmount = 0;
                            decimal payableAmount = applicationBusiness.GetPayableComponentstByEndorsementIdQuoutaId(
                                tempApplicationPremium.TempAppPremiumCode, tempApplicationPremium.EndorsementCode, tempApplicationPremium.PaymentNum).Sum(x => x.Amount);

                            // Controlar si los 2 valores son negativos
                            if (Convert.ToDecimal(tempApplicationPremium.Amount) > 0 && payableAmount > 0)
                            {
                                if (Convert.ToDecimal(tempApplicationPremium.Amount) > payableAmount)
                                {
                                    excessPayment = Convert.ToDecimal(tempApplicationPremium.Amount) - payableAmount;
                                    localAmount = excessPayment * Convert.ToDecimal(tempApplicationPremium.ExchangeRate);
                                }
                                collectionAmount = excessPayment > 0 ? payableAmount : Convert.ToDecimal(tempApplicationPremium.Amount);

                                List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(tempApplicationPremium.TempAppPremiumCode);

                                UpdTempApplicationPremiumComponentDTO updateTempApplicationSourceCode = new UpdTempApplicationPremiumComponentDTO()
                                {
                                    ComponentCurrencyCode = tempApplicationPremium.CurrencyCode,
                                    ExchangeRate = tempApplicationPremium.ExchangeRate,
                                    TempApplicationPremiumCode = tempApplicationPremium.TempAppPremiumCode,
                                    ExpensesLocalAmount = tempApplicationPremiumComponents.Where(x => x.ComponentTinyDescription == "G").Sum(x => x.Amount),
                                    TaxLocalAmount = tempApplicationPremiumComponents.Where(x => x.ComponentTinyDescription == "I").Sum(x => x.Amount)
                                };
                                updateTempApplicationSourceCode.PremiumAmount = collectionAmount - updateTempApplicationSourceCode.ExpensesLocalAmount - updateTempApplicationSourceCode.TaxLocalAmount;
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

                            var tempAppPremium = tempApplicationPremium;
                            tempAppPremium.TempAppCode = imputationId;
                            int statusQuota = (int)AccountingApplicationServiceEEProvider.StatusQuotaPartial;
                            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount)
                            {
                                statusQuota = (int)AccountingApplicationServiceEEProvider.StatusQuotaPartial; // PARCIAL
                            }
                            if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
                            {
                                statusQuota = (int)AccountingApplicationServiceEEProvider.StatusQuotaTotal; // TOTAL
                            }


                            premiumReceivableTransactionItem = applicationPremiumItemDAO.SavePremiumRecievableTransactionItem(tempAppPremium, statusQuota);

                            var AccountingComponentDistributionServiceEEProvider = new AccountingComponentDistributionServiceEEProvider();
                            AccountingComponentDistributionServiceEEProvider.CreateApplicationPremiumComponent(new ParamApplicationPremiumComponent { EndorsementId = tempApplicationPremium.EndorsementCode, QuotaNumber = tempApplicationPremium.PaymentNum, PremiumId = premiumReceivableTransactionItem.Id, TempApplicationPremiumCode = tempApplicationPremium.TempAppPremiumCode, ApplicationAmount = collectionAmount });


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
                                    depositPremiumTransaction.Collect.Payer.IndividualId = Convert.ToInt32(tempApplicationPremium.PayerCode);
                                    depositPremiumTransaction.Date = Convert.ToDateTime(tempApplicationPremium.RegisterDate);
                                    depositPremiumTransaction.Amount = new Amount()
                                    {
                                        Currency = new Currency() { Id = Convert.ToInt32(tempApplicationPremium.CurrencyCode) },
                                        Value = Math.Round(excessPayment, 2)
                                    };
                                    depositPremiumTransaction.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempApplicationPremium.ExchangeRate) };
                                    depositPremiumTransaction.LocalAmount = new Amount();
                                    depositPremiumTransaction.LocalAmount.Value = Math.Round(localAmount, 2);

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
                                isConverted = true;
                                #endregion

                                #region DiscountedCommision

                                //se comenta debido a que se implementa las comisiones a la nueva tabla de APPLICATION PREMIUM
                                //if (Math.Abs(Convert.ToDecimal(tempApplicationPremium.DiscountedCommission)) > 0)
                                //{
                                //    isDiscountedCommissionSaved = SaveDiscountedCommissionRequest(premiumReceivableTransactionItem.Id, Math.Abs(Convert.ToDecimal(tempApplicationPremium.DiscountedCommission)));
                                //    if (isDiscountedCommissionSaved)
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
        /// DeletePremiumReceivableTransaction
        /// Borrado de primas x cobrar
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplicationPremiumTransaction(int tempApplicationId)
        {
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
                        // Elimino los temp premium receivable
                        tempApplicationPremiumItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivableEntity.TempAppPremiumCode);
                        // Elimino los temp premium commiss
                        applicationPremiumCommisionDAO.DeleteTempApplicationPremiumCommisses(tempPremiumReceivableEntity.TempAppPremiumCode);
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
        /// <param name="tempApplicationId"></param>
        /// <param name="tempApplicationPremiumId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPremiumRecievableTransactionItem(int tempApplicationId, int tempApplicationPremiumId, bool IsReversion = false)
        {
            try
            {
                if (IsReversion)
                {
                    if (tempApplicationPremiumId != -1)
                    {
                        return TempApplicationPremiumItemDAO.DeleteTempPremiumReversionItem(tempApplicationPremiumId);
                    }
                    else
                    {
                        return TempApplicationPremiumItemDAO.DeleteTempPremiumReversionTransactionItem(tempApplicationId);
                    }
                }

                bool isDeleted = false;

                // Borro primas en deposito que se hayan usado
                if (tempApplicationPremiumId != -1)
                {
                    DeleteTempUsedDepositPremiumRequest(tempApplicationPremiumId);
                }

                // Borro el item de primas por cobrar.
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                if (tempApplicationPremiumId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppPremiumCode, tempApplicationPremiumId);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempApplicationPremium tempPremiumReceivable in businessCollection.OfType<ACCOUNTINGEN.TempApplicationPremium>())
                {
                    // Elimino los temp premium component
                    tempApplicationPremiumComponentDAO.DeleteTempApplicationPremiumComponentsByTemApp(tempPremiumReceivable.TempAppPremiumCode);
                    tempApplicationPremiumItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivable.TempAppPremiumCode);
                    // Elimino los temp premium commiss
                    applicationPremiumCommisionDAO.DeleteTempApplicationPremiumCommisses(tempPremiumReceivable.TempAppPremiumCode);
                    isDeleted = true;
                }

                return isDeleted;

            }
            catch (BusinessException)
            {
                throw new BusinessException();
            }
        }

        public List<SEARCH.DiscountedCommissionDTO> SearhDiscountedCommission(string policyId, string endorsementId)
        {
            List<UNDDTOs.IssuanceAgencyDTO> issuanceAgencyDTOs = DelegateService.integrationUnderwritingService.GetAgentsByPolicyIdEndorsementId(Convert.ToInt32(policyId), Convert.ToInt32(endorsementId));

            List<SEARCH.DiscountedCommissionDTO> discountedCommissionDTO = ModelAssembler.CreateAgentCommissions(issuanceAgencyDTOs);//canUsedCommission);
            return discountedCommissionDTO;
        }

        public List<TempApplicationPremiumComponentDTO> GetTempApplicationPremiumComponentsByTemApp(int tempApp)
        {
            try
            {
                return tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(tempApp).ToDTOs();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempApplicationPremiumComponentsByTemApp);
            }
        }

        public List<TempApplicationPremiumComponentDTO> UpdTempApplicationPremiumComponents(UpdTempApplicationPremiumComponentDTO updTempApplicationPremiumComponentDTO)
        {
            try
            {
                TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                return tempApplicationBusiness.UpdTempApplicationPremiumComponents(updTempApplicationPremiumComponentDTO.ToModel()).ToDTOs();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorUpdTempApplicationPremiumComponents);
            }
        }

        /// <summary>
        /// GetPaymentRequestsByPaymentRequestId
        /// </summary>
        /// <param name="applicationPremiumId"></param>
        /// <returns>List<ACCOUNTINGEN.ApplicationPremium></returns>
        public List<ApplicationPremiumDTO> GetApplicationPremiumsByApplicationPremiumId(int applicationPremiumId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AppPremiumCode, applicationPremiumId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()));

                List<ACCOUNTINGEN.ApplicationPremium> entityApplicationPremiums = new List<ACCOUNTINGEN.ApplicationPremium>();
                foreach (ACCOUNTINGEN.ApplicationPremium applicationPremium in businessCollection.OfType<ACCOUNTINGEN.ApplicationPremium>())
                {
                    entityApplicationPremiums.Add(applicationPremium);
                }

                return ModelAssembler.CreateApplicationPremiums(entityApplicationPremiums).ToDTOs();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ApplicationPremiumDTO> GetApplicationPremiumsByApplicationPremiums(DateTime accountingDate)
        {
            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AccountingDate, accountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(accountingDate);


                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()));

                Individual individual = new Individual();
                List<ACCOUNTINGEN.ApplicationPremium> entityApplicationPremiums = new List<ACCOUNTINGEN.ApplicationPremium>();
                foreach (ACCOUNTINGEN.ApplicationPremium applicationPremium in businessCollection.OfType<ACCOUNTINGEN.ApplicationPremium>())
                {
                    entityApplicationPremiums.Add(applicationPremium);
                }
                List<ApplicationPremiumDTO> ApplicationPremiumDTO = ModelAssembler.CreateApplicationPremiums(entityApplicationPremiums).ToDTOs();
                foreach (ApplicationPremiumDTO applicationPremiumDTO in ApplicationPremiumDTO)
                {
                    individual = DelegateService.personService.GetIndividualByIndividualId(applicationPremiumDTO.PayerId);
                    if (individual.IndividualType == Services.UtilitiesServices.Enums.IndividualType.Person)
                    {
                        if (applicationPremiumDTO.PayerId == individual.IndividualId)
                        {
                            Person person = DelegateService.personService.GetPersonByIndividualId(individual.IndividualId);
                            applicationPremiumDTO.Name += person.Name + person.SurName;
                        }

                    }
                    if (individual.IndividualType == Services.UtilitiesServices.Enums.IndividualType.Company)
                    {
                        if (applicationPremiumDTO.PayerId == individual.IndividualId)
                        {
                            Company company = DelegateService.personService.GetCompanyByIndividualId(individual.IndividualId);
                            applicationPremiumDTO.Name = company.FullName;
                        }

                    }
                    applicationPremiumDTO.NameCurrency = DelegateService.commonService.GetCurrencies().FirstOrDefault(x => x.Id == applicationPremiumDTO.Currencyid).Description;

                }

                return ApplicationPremiumDTO;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
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
        /// <param name="tempApplicationPremiumId"></param>
        public void DeleteTempUsedDepositPremiumRequest(int tempApplicationPremiumId)
        {
            List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = _tempUsedDepositPremiumDAO.GetTempUsedDepositPremiumByTempPremiumReceivableId(tempApplicationPremiumId);

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
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO SaveTempClaimPaymentRequestItem(ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestItem, int imputationId, decimal exchangeRate)
        {
            Models.Imputations.Application application = new Models.Imputations.Application();
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
                    application.Id = Convert.ToInt32(claimPaymentRequestTrans[0].ApplicationCode);
                    application.IsTemporal = false; // Esta en un imputación real
                }
            }
            else
            {
                application.Id = Convert.ToInt32(tempClaimPayments[0].TempApplicationCode);
                application.IsTemporal = true; // Esta en un imputación temporal
            }

            return application.ToDTO();
        }

        /// <summary>
        /// SaveTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="applicationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO SaveTempApplicationPremiumItem(PaymentRequestTransactionItemDTO paymentRequestTransactionItem, int applicationId, decimal exchangeRate)
        {
            try
            {
                Models.Imputations.Application application = new Models.Imputations.Application();

                // Convertir de model a entity
                ACCOUNTINGEN.TempApplicationPremium tempApplicationPremium = EntityAssembler.CreateTempApplicationPremiumTrans(paymentRequestTransactionItem.ToModel(), applicationId, exchangeRate);

                //if (tempApplicationPremium.PaymentExpirationDate == new DateTime())
                //{
                //    tempApplicationPremium.PaymentExpirationDate = null;
                //}

                // Valida si ya consta en temporales o en reales
                List<ACCOUNTINGEN.TempApplicationPremium> tempApplicationPremiums = tempApplicationPremiumDAO.GetTempApplicationPremiums(Convert.ToInt32(tempApplicationPremium.TempAppPremiumCode), 0);

                // Si no existe en temporales entonces busca en reales 
                if (tempApplicationPremiums.Count == 0)
                {
                    // Llamado a verificar en reales
                    List<ApplicationPremiumDTO> applicationPremium = GetApplicationPremiumsByApplicationPremiumId(Convert.ToInt32(tempApplicationPremium.TempAppPremiumCode));
                    // Si no existe en reales ahi si graba en temporales
                    if (applicationPremium.Count == 0)
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().InsertObject(tempApplicationPremium);
                    }
                    else
                    {
                        application.Id = Convert.ToInt32(applicationPremium[0].ApplicationId);
                        application.IsTemporal = false; // Esta en un imputación real
                    }
                }
                else
                {
                    application.Id = Convert.ToInt32(tempApplicationPremiums[0].TempAppCode);
                    application.IsTemporal = true; // Esta en un imputación temporal
                }

                return application.ToDTO();

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
        /// <param name="applicationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>bool</returns>
        public bool DeleteTempClaimPaymentRequestItem(int claimsPaymentRequestId, int applicationId, bool isPaymentVarious)
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
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPaymentRequestItem(int paymentRequestId, int applicationId)
        {
            try
            {
                return tempApplicationPremiumDAO.DeleteTempApplicationPremium(paymentRequestId);
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
        /// <param name="applicationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>List<TempPaymentRequestClaimDTO/></returns>
        public List<SEARCH.TempPaymentRequestClaimDTO> GetTempClaimsPaymentRequest(int applicationId, bool isPaymentVarious)
        {
            #region LoadDTO

            List<SEARCH.TempPaymentRequestClaimDTO> tempPaymentRequestClaims = new List<SEARCH.TempPaymentRequestClaimDTO>();

            UIView tempPaymentRequestClaimView = LoadGetTempPaymentRequestClaim(applicationId, isPaymentVarious);

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
        /// <param name="applicationId"></param>
        /// <returns>List<TempPaymentRequestClaimDTO></returns>
        public List<SEARCH.TempPaymentRequestClaimDTO> GetTempPremiumAmountByApplicationId(int applicationId)
        {
            try
            {

                #region LoadDTO

                List<SEARCH.TempPaymentRequestClaimDTO> tempPaymentRequestClaims = new List<SEARCH.TempPaymentRequestClaimDTO>();

                UIView tempPaymentRequestClaimView = LoadGetTempPaymentRequestItems(applicationId);
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
        /// <param name="tempApplicationId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        public List<SEARCH.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemByTempApplicationId(int tempApplicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);
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
                        TempImputationId = Convert.ToInt32(tempBrokerCheckingAccountItem.TempApplicationCode),
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
        /// <param name="tempApplicationId"></param>
        /// <param name="brokersCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteBrokersCheckingAccountItem(int tempApplicationId, int brokersCheckingAccountItemId)
        {
            bool isDeleted = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);
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
        /// <param name="applicationId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>bool</returns>
        public bool SaveBrokersCheckingAccount(BrokersCheckingAccountTransactionDTO brokersCheckingAccountTransaction, int applicationId, DateTime accountingDate)
        {
            bool isSaved = false;

            try
            {
                foreach (BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItem in brokersCheckingAccountTransaction.BrokersCheckingAccountTransactionItems)
                {
                    BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItemNew;

                    brokersCheckingAccountTransactionItemNew = brokersCheckingAccountTransactionItem.Id == 0 ? _tempBrokerCheckingAccountTransactionItemDAO.SaveTempBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem.ToModel(), applicationId, 0, 0 /*billCode*/, accountingDate).ToDTO() : _tempBrokerCheckingAccountTransactionItemDAO.UpdateTempBrokerCheckingAccount(brokersCheckingAccountTransactionItem.ToModel(), applicationId, 0, 0 /*billCode*/, accountingDate).ToDTO();

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
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO ValidateDuplicateBrokerCheckingAccount(SEARCH.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter)
        {
            Models.Imputations.Application application = new Models.Imputations.Application();
            application.VerificationValue = new Amount();
            application.VerificationValue.Value = -1;

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
                        application.VerificationValue.Value = 0;
                        application.Id = (int)tempBrokerCheckingAccount.TempApplicationCode;
                    }
                }

                // Existe el registro en imputacion temporal
                if (application.Id > 0)
                {
                    // Se verifica en la tabla temporal
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.TempAppCode, application.Id);

                    BusinessCollection busCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempApplication), criteriaBuilder.GetPredicate()));

                    if (busCollection.Count > 0)
                    {

                        ACCOUNTINGEN.TempApplication entityTempApplication = busCollection.OfType<ACCOUNTINGEN.TempApplication>().First();
                        application.ModuleId = entityTempApplication.ModuleCode;

                        //application.IsTemporal = Convert.ToBoolean(!entityTempApplication.IsRealSource);

                        //if (Convert.ToInt32(entityTempApplication.IsRealSource) == 1) //--> ya esta converida la OP/PL en real
                        //{
                        //    TransactionType transactionType = new TransactionType();
                        //    application.ImputationItems = new List<TransactionType>();
                        //    transactionType.Id = Convert.ToInt32(entityTempApplication.SourceCode);
                        //}

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
                        application.VerificationValue.Value = 1;
                        application.Id = (int)brokerCheckingAccount.ApplicationCode;
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return application.ToDTO();
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
                        ImputationId = dataRow["ApplicationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationCode"]),
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
        /// <param name="tempApplicationId"></param>
        /// <param name="reinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteReinsuranceCheckingAccountItem(int tempApplicationId, int reinsuranceCheckingAccountItemId)
        {
            bool isDeleted = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);
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
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        public bool SaveReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionDTO reinsuranceCheckingAccountTransaction, int applicationId)
        {
            bool isSaved = false;

            try
            {
                foreach (ReInsuranceCheckingAccountTransactionItemDTO reinsuranceCheckingAccountTransactionItem in reinsuranceCheckingAccountTransaction.ReInsuranceCheckingAccountTransactionItems)
                {
                    ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItemNew;


                    reinsuranceCheckingAccountTransactionItemNew = reinsuranceCheckingAccountTransactionItem.Id == 0 ?
                        _tempReinsuranceCheckingAccountTransactionItemDAO.SaveTempReinsuranceCheckingAccountTransactionItem(reinsuranceCheckingAccountTransactionItem.ToModel(), applicationId, 0) :
                        _tempReinsuranceCheckingAccountTransactionItemDAO.UpdateTempReinsuranceCheckingAccount(reinsuranceCheckingAccountTransactionItem.ToModel(), applicationId, 0); // El valor 0 corresponede al campo tempReinsuranceParentCode pendiente averiguar

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
        /// <param name="tempApplicationId"></param>
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.ReinsuranceCheckingAccountItemDTO> GetTempReinsuranceCheckingAccountItemByTempApplicationId(int tempApplicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);
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
                        TempImputationId = dataRow["TempAppCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempAppCode"]),
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
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO ValidateDuplicateReinsuranceCheckingAccount(SEARCH.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter)
        {
            Models.Imputations.Application application = new Models.Imputations.Application()
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
                        application.VerificationValue.Value = 0;
                        application.Id = (int)tempReinsuranceCheckingAccount.TempApplicationCode;
                    }
                }

                if (application.Id <= 0)
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
                            application.VerificationValue.Value = 1;
                            application.Id = (int)reinsuranceCheckingAccount.ApplicationCode;
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return application.ToDTO();
        }

        #endregion

        //Contabilidad
        #region Accounting

        /// <summary>
        /// Guarda un listado de movimientos contables
        /// </summary>
        /// <param name="applicationAccountingTransaction">Listado de movimientos contables</param>
        /// <returns>Identificador del elemento creado</returns>
        public int SaveTempAccountingTransaction(ApplicationAccountingTransactionDTO applicationAccountingTransaction)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.SaveTempAccountingTransaction(applicationAccountingTransaction.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempApplicationAccounting
        /// Elimina un temporal de transacción contable
        /// </summary>
        /// <param name="tempApplicationAccountingId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplicationAccounting(int tempApplicationAccountingId)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.DeleteTempApplicationAccounting(tempApplicationAccountingId);
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
        /// <param name="tempApplicationId"></param>
        /// <returns>List<TempDailyAccountingDTO/></returns>
        public List<ApplicationAccountingDTO> GetTempAccountingTransactionItemByTempApplicationId(int tempApplicationId)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.GetTempAccountingTransactionItemByTempApplicationId(tempApplicationId).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ApplicationAccountingDTO> GetAccountingTransactionItemsByApplicationId(int applicationId)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.GetTempAccountingTransactionItemByTempApplicationId(applicationId).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetTempAccountingTransactionByTempAccountingApplicationId
        /// Obtiene un temporal de movimiento contable 
        /// </summary>
        /// <param name="tempAccountingApplicationId"></param>
        /// <returns>List<TempDailyAccountingDTO/></returns>
        public ApplicationAccountingDTO GetTempAccountingTransactionByTempAccountingApplicationId(int tempAccountingApplicationId)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.GetTempAccountingTransactionByTempAccountingApplicationId(tempAccountingApplicationId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
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
                criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(accountingAccountNumber + "%");

                if (branchId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.DefaultBranchCode, branchId);
                }

                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccount.Properties.IsOfficialNomenclature, 0);

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
                            DefaultCurrency = Convert.ToInt32(accountingAccount.DefaultCurrencyCode),
                            AnalysisId = Convert.ToInt32(accountingAccount.AnalysisId),
                            RequireAnalysis = Convert.ToBoolean(accountingAccount.RequireAnalysis)
                        });
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return accountingAccounts;
        }

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// Trae un item de prima por cobrar
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        public List<ApplicationAccountingDTO> GetApplicationAccountByApplicationId(int applicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.AppCode, applicationId);

                // Realizar las operaciones con los entities utilizando DAF
                BusinessCollection businessObjects = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ApplicationAccounting), criteriaBuilder.GetPredicate()));
                List<ApplicationAccountingDTO> applicationAccountingDTOs = new List<ApplicationAccountingDTO>();
                foreach (ACCOUNTINGEN.ApplicationAccounting applicationAccounting in businessObjects.OfType<ACCOUNTINGEN.ApplicationAccounting>())
                {
                    applicationAccountingDTOs.Add(ModelAssembler.CreateApplicationAccounting(applicationAccounting).ToDTO());
                }

                return applicationAccountingDTOs;
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
        /// GetTempPremiumReceivableItemByTempImputationId
        /// Trae un item de prima por cobrar
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        public List<ApplicationAccountingDTO> GetApplicationAccountsByApplicationId(int applicationId)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetApplicationAccountsByApplicationId(applicationId).ToDTOs().ToList();
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
        /// <param name="tempApplicationId"></param>
        /// <param name="coinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteCoinsuranceCheckingAccountItem(int tempApplicationId, int coinsuranceCheckingAccountItemId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);
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
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        public bool SaveCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionDTO coinsuranceCheckingAccountTransaction, int applicationId)
        {
            bool isSaved = false;

            try
            {
                foreach (CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItem in coinsuranceCheckingAccountTransaction.CoInsuranceCheckingAccountTransactionItems)
                {
                    CoInsuranceCheckingAccountTransactionItemDTO coinsuranceCheckingAccountTransactionItemNew;

                    coinsuranceCheckingAccountTransactionItemNew = coinsuranceCheckingAccountTransactionItem.Id == 0 ? _tempCoinsuranceCheckingAccountTransactionItemDAO.SaveTempCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem.ToModel(), applicationId, 0).ToDTO() : _tempCoinsuranceCheckingAccountTransactionItemDAO.UpdateTempCoinsuranceCheckingAccount(coinsuranceCheckingAccountTransactionItem.ToModel(), applicationId, 0).ToDTO();

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
        /// <param name="tempApplicationId"></param>
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        public List<SEARCH.CoinsuranceCheckingAccountItemDTO> GetTempCoinsuranceCheckingAccountItemByTempApplicationId(int tempApplicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);
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
                        TempImputationId = dataRow["TempApplicationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TempApplicationCode"]),
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
                        ImputationId = dataRow["ApplicationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationCode"]),
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
                        ImputationId = dataRow["ApplicationCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ApplicationCode"]),
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
        public ApplicationDTO ValidateDuplicateCoinsuranceCheckingAccount(SEARCH.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter)
        {
            Models.Imputations.Application application = new Models.Imputations.Application();
            application.VerificationValue = new Amount();
            application.VerificationValue.Value = -1;

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
                        application.VerificationValue.Value = 0;
                        application.Id = (int)tempCoinsuranceCheckingAccount.TempApplicationCode;
                    }
                }

                if (application.Id <= 0)
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
                            application.VerificationValue.Value = 1;
                            application.Id = (int)coinsuranceCheckingAccount.ApplicationCode;
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return application.ToDTO();
        }


        #endregion


        #endregion //ApplicationTypes

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

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AppPremiumCode, premiumReceivableId);
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.ApplicationPremium premiumReceivableTrans in businessCollection.OfType<ACCOUNTINGEN.ApplicationPremium>())
                    {
                        //policyId = Convert.ToInt32(premiumReceivableTrans.PolicyId);
                        endorsementId = Convert.ToInt32(premiumReceivableTrans.EndorsementCode);
                        paymentNumber = Convert.ToInt32(premiumReceivableTrans.PaymentNum);
                    }
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                /*criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyId, policyId);
                criteriaBuilder.And();*/
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PaymentNum, paymentNumber);

                UIView receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("AppReceivablePremiumStatusSearchView",
                                         criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (receivablePremiums.Count > 0)
                {
                    foreach (DataRow dataRow in receivablePremiums)
                    {
                        SEARCH.PendingCommissionDTO commission = CommisionDAO.GetPendingCommission(Convert.ToInt32(dataRow["PolicyId"]), Convert.ToInt32(dataRow["EndorsementId"]));

                        currencyId = Convert.ToInt32(dataRow["CurrencyCode"]);
                        exchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]);
                        agentIndividualId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]);
                        commissionPercentage = commission.CommissionPercentage;
                        netPremium = Convert.ToDecimal(dataRow["PolicyNetPremium"]);
                    }
                }

                agentTypeId = GetAgentTypeIdByAgentId(agentIndividualId);

                discountedCommissionDto.DiscountedCommissionId = discountedCommissionId;
                discountedCommissionDto.ApplicationPremiumId = premiumReceivableId;
                discountedCommissionDto.AgentTypeCode = agentTypeId;
                discountedCommissionDto.AgentIndividualId = agentIndividualId;
                discountedCommissionDto.CurrencyId = currencyId;
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

        public ApplicationPremiumCommision SavePremiumCommission(ApplicationPremiumCommision applicationPremiumCommision)
        {
            try
            {
                return applicationPremiumCommisionDAO.CreateApplicationPremiumCommision(applicationPremiumCommision);
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
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.UserCode, preliquidationsDto.UsertId);
                }
                // Filtros que se deben cumplir para mostrar en la consulta.
                criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.ImputationTypeCode, 3);
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.ModuleCode, 3);
                //criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.IsRealSource, 1);

                #endregion

                List<ACCOUNTINGEN.AppBranchPreliquidationV> dataPreliquidations = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.AppBranchPreliquidationV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.AppBranchPreliquidationV>().ToList();
                int rows = 0;
                if (dataPreliquidations.Count > 0)
                {
                    rows = dataPreliquidations.Count;
                }
                #region LoadDTO

                List<SEARCH.PreliquidationsDTO> preliquidationDTOs = new List<SEARCH.PreliquidationsDTO>();

                foreach (ACCOUNTINGEN.AppBranchPreliquidationV preliquidations in dataPreliquidations)
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
                    preliquidationsDTO.TempImputationId = preliquidations.TempApplicationCode;
                    preliquidationsDTO.SourceId = preliquidations.SourceCode == 0 ? -1 : (int)preliquidations.SourceCode;
                    preliquidationsDTO.ImputationTypeId = preliquidations.ModuleCode == 0 ? -1 : (int)preliquidations.ModuleCode;
                    preliquidationsDTO.ImputationTypeDescription = preliquidations.ModuleDescription;
                    //preliquidationsDTO.IsRealSource = Convert.ToInt32(preliquidations.IsRealSource);
                    preliquidationsDTO.Description = preliquidations.PreliquidationDescription.ToString();
                    preliquidationsDTO.TotalAmount = GetDebitsAndCreditsMovementTypes(new ApplicationDTO { Id = Convert.ToInt32(preliquidations.TempApplicationCode), UserId = preliquidationsDto.UsertId }, 0).VerificationValue.Value;
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
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        public bool CancelPreliquidation(int preliquidationId, int tempApplicationId)
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
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()));

                    foreach (ACCOUNTINGEN.TempApplicationPremium tempPremiumReceivable in businessCollection.OfType<ACCOUNTINGEN.TempApplicationPremium>())
                    {
                        // Elimino los temp premium component
                        tempApplicationPremiumComponentDAO.DeleteTempApplicationPremiumComponentsByTemApp(tempPremiumReceivable.TempAppPremiumCode);
                        // Primas por Cobrar
                        tempApplicationPremiumItemDAO.DeleteTempPremiumRecievableTransactionItem(tempPremiumReceivable.TempAppPremiumCode);
                        // Elimino los temp premium commiss
                        applicationPremiumCommisionDAO.DeleteTempApplicationPremiumCommisses(tempPremiumReceivable.TempAppPremiumCode);
                    }

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

                    // Borra temporales de transacciones solicitudes de pago de siniestros
                    tempApplicationPremiumDAO.DeleteTempApplicationPremiumByTempApplication(tempApplicationId);

                    // Borra temporales solicitudes de pago de siniestros
                    _tempClaimPaymentRequestDAO.DeleteTempClaimPaymentRequestByTempImputationId(tempApplicationId);
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
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <returns>int</returns>
        public int ConvertTempPreLiquidationToPreLiquidation(int tempPreLiquidationId, int tempApplicationId, int moduleId)
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
                    Models.Imputations.Application application = new Models.Imputations.Application();
                    application.Id = tempApplicationId;
                    application.IsTemporal = true;

                    tempApplicationDAO.UpdateTempApplication(application, newPreLiquidation.Id);

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
                bool addAnd = false;
                if (searchParameter.Branch.Id > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.BranchCode, searchParameter.Branch.Id);
                    addAnd = true;
                }

                if (searchParameter.UserId != -1)
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.UserCode, searchParameter.UserId);
                    addAnd = true;
                }

                if (searchParameter.ImputationType.Id != -1)
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.ModuleCode, searchParameter.ImputationType.Id);
                    addAnd = true;
                }

                if (searchParameter.TemporaryNumber != -1)
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.TempAppCode, searchParameter.TemporaryNumber);
                    addAnd = true;
                }

                if (searchParameter.StartDate != "*")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(searchParameter.StartDate));
                    addAnd = true;
                }

                if (searchParameter.EndDate != "*")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(searchParameter.EndDate));
                }

                string[] sort = new string[]
                {
                    "-" + ACCOUNTINGEN.GetAppTemporaryItemSearch.Properties.TempAppCode
                };

                UIView temporary = _dataFacadeManager.GetDataFacade().GetView("GetAppTemporaryItemSearchView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, sort, true, out int rows);

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
                        TemporaryNumber = dataRow["TempAppCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TempAppCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchName"]),
                        ImputationTypeCode = dataRow["ModuleCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModuleCode"]),
                        ImputationTypeName = dataRow["ApplicationTypeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApplicationTypeName"]),
                        UserCode = dataRow["UserCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserCode"]),
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
        /// <returns>CollectApplication</returns>
        public CollectApplicationDTO SaveJournalEntryImputation(int tempJournalEntryId, int tempImputationId, int userId)
        {
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    CollectApplication collectApplication = SaveApplicationRequestJournalEntry(tempImputationId, userId, tempJournalEntryId).ToModel();

                    transaction.Complete();

                    return collectApplication.ToDTO();
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
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PolicyId, policyId);
                criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.EndorsementId, endorsementId);
                criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PaymentNum, 1); //se indica que es la primera cuota

                // BusinessCollection businessCollectionPayment = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                /* foreach (ACCOUNTINGEN.PremiumReceivableTrans collection in businessCollectionPayment.OfType<ACCOUNTINGEN.PremiumReceivableTrans>())
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
                return CommisionDAO.GetPendingCommission(policyId, endorsementId);
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
                    if (Convert.ToBoolean(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_RELEASE_COMMISSIONS_PRORATE)) == true) //Liberacion de comisiones a prorrata
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
                            newPaymentOrder.PaymentMethod.Id = Convert.ToInt32(accountBankId == -1 ? UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK) : UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_TRANSFER));
                            newPaymentOrder.PaymentSource = new EEProvider.Models.Claims.PaymentRequest.ConceptSource()
                            {
                                Id = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_CONCEPT_SOURCE_ID))  //CÓDIGO 8 DE GL.CONCEPT_SOURCE
                            };
                            newPaymentOrder.PayTo = agentName;
                            newPaymentOrder.PersonType = new PersonType() { Id = Convert.ToInt32(1) };   // AGENTE - SE INSERTO EN PARAM.PERSON_TYPE
                            newPaymentOrder.Status = 1;
                            newPaymentOrder.Observation = "GENERADO AUTOMÁTICAMENTE AGENTES";

                            // Graba la cabecera de orden de pago
                            newPaymentOrder = _paymentOrderDAO.SavePaymentOrder(newPaymentOrder);

                            DateTime registerDate = DateTime.Now;

                            Models.Imputations.Application application = new Models.Imputations.Application();
                            application.Id = 0;
                            application.IsTemporal = true;
                            application.RegisterDate = registerDate;
                            application.ModuleId = (int)ApplicationTypes.PaymentOrder;
                            application.UserId = userId;

                            // Graba la cabecera de imputación
                            application = tempApplicationDAO.SaveTempApplication(application, newPaymentOrder.Id);
                            int tempImputationId = application.Id;

                            application.Id = tempImputationId;
                            application.IsTemporal = true;

                            tempApplicationDAO.UpdateTempApplication(application, newPaymentOrder.Id);

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
        public ApplicationDTO GetTempApplicationByEndorsementIdQuotaNumber(int tempApplicationId, int endorsementId, int quotaNumber)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetTemporalApplicationByEndorsementIdPaymentNumber(tempApplicationId, endorsementId, quotaNumber).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempApplicationByEndorsementIdQuotaNumber);
            }
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
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO CheckDailyAccounting(int branchId, int salesPointId, int beneficiaryIndividualId, int accountingConceptId, int accountingNature)
        {
            Models.Imputations.Application application = new Models.Imputations.Application();

            // Realizo un select para ver si la póliza se encuentra en temporales.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.BranchCode, branchId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.SalePointCode, salesPointId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.IndividualCode, beneficiaryIndividualId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.AccountingConceptCode, accountingConceptId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.AccountingNature, accountingNature);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(ACCOUNTINGEN.TempApplicationAccounting), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0) // Se encuentra en temporales
            {
                foreach (ACCOUNTINGEN.TempApplicationAccounting tempDailyAccounting in businessCollection.OfType<ACCOUNTINGEN.TempApplicationAccounting>())
                {
                    application.Id = Convert.ToInt32(tempDailyAccounting.TempAppCode);
                    application.IsTemporal = Convert.ToBoolean(true);
                }
            }
            else
            {
                // Realizo un select para ver si la póliza se encuentra en reales.
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.SalePointCode, salesPointId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.IndividualCode, beneficiaryIndividualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingConceptCode, accountingConceptId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingNature, accountingNature);

                businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.ApplicationAccounting), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0) // Se encuentra en reales
                {
                    foreach (ACCOUNTINGEN.ApplicationAccounting dailyAccounting in businessCollection.OfType<ACCOUNTINGEN.ApplicationAccounting>())
                    {
                        application.Id = Convert.ToInt32(dailyAccounting.AppCode);
                        application.IsTemporal = Convert.ToBoolean(false);
                    }
                }
                else
                {
                    application.Id = 0;
                    application.IsTemporal = false;
                }
            }

            return application.ToDTO();
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

                bool addAnd = false;

                if (insuredId != "")
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.InsuredIndividualId, Convert.ToInt64(insuredId));
                    addAnd = true;
                }

                if (policyDocumentNumber != "")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyDocumentNumber,
                                          Convert.ToInt32(policyDocumentNumber));
                    addAnd = true;
                }

                if (branchId != "")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.BranchCode, Convert.ToInt32(branchId));
                    addAnd = true;
                }

                if (prefixId != "")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PrefixCode, Convert.ToInt32(prefixId));
                    addAnd = true;
                }

                if (endorsementId != "")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.EndorsementDocumentNumber, Convert.ToInt32(endorsementId));
                    addAnd = true;
                }
                if (ExpirationDateFrom != "" && ExpirationDateTo != "")
                {
                    if (addAnd)
                        criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PaymentExpirationDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(Convert.ToDateTime(ExpirationDateFrom));
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PaymentExpirationDate);
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
                    receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("AppReceivablePremiumStatusSearchView",
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
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyDocumentNumber,
                                          Convert.ToInt64(policyDocumentNumber));

                    criteriaBuilder.And(); // Branch
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.BranchCode, Convert.ToInt32(branchId));

                    criteriaBuilder.And(); // Prefix
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PrefixCode, Convert.ToInt32(prefixId));

                    criteriaBuilder.And(); // Negativos
                    criteriaBuilder.Property(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.Amount);
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
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyDocumentNumber,
                                         Convert.ToInt64(policyDocumentNumber));

                    criteriaBuilder.And(); // Branch
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.BranchCode, Convert.ToInt32(branchId));

                    criteriaBuilder.And(); // Prefix
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PrefixCode, Convert.ToInt32(prefixId));

                    criteriaBuilder.And(); // Positivos
                    criteriaBuilder.Property(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.Amount);
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
                                    ApplicationDTO tempImputation = new ApplicationDTO();
                                    tempImputation.Id = 0;
                                    tempImputation.RegisterDate = DateTime.Now;
                                    tempImputation.ModuleId = (int)ApplicationTypes.JournalEntry; // la nota de crédito es un asiento diario.
                                    tempImputation.UserId = userId;

                                    tempImputation = SaveTempApplication(tempImputation, newJournalEntry.Id);

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
                                    premiumSaved = SaveTempPremiumRecievableTransaction(tempPremiumReceivableTransaction.ToDTO(), tempImputation.Id, 1, tempImputation.UserId, DateTime.Now, journalEntry.AccountingDate);

                                    // Convierto a reales
                                    if (premiumSaved == 1)
                                    {
                                        CollectApplication collectApplicationResult = SaveApplicationRequestJournalEntry(tempImputation.Id, tempImputation.UserId, newJournalEntry.Id).ToModel();
                                        journalEntryId = collectApplicationResult.Transaction.Id;
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

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyDocumentNumber, Convert.ToInt64(policyDocumentNumber));
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.BranchCode, Convert.ToInt32(branchId));
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PrefixCode, Convert.ToInt32(prefixId));

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

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyDocumentNumber, Convert.ToInt64(policyDocumentNumber));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.BranchCode, Convert.ToInt32(branchId));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PrefixCode, Convert.ToInt32(prefixId));

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.AppReceivablePremiumStatusSearch), criteriaBuilder.GetPredicate()));

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
        /// ReverseApplicationRequest
        /// Método para reversar una imputación de recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="moduleId"></param>
        /// <param name="userId"></param>
        ///<returns>bool</returns>
        public bool ReverseApplicationRequest(int collectId, int moduleId, int userId)
        {
            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(ModuleId, DateTime.Now);
            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    bool isReversed = false;
                    bool isSaved = false;
                    CollectApplication collectApplication;
                    Models.Imputations.Application application = new Models.Imputations.Application();
                    Models.Imputations.Application newImputation; //se genera una nueva imputación con los valores en negativo.

                    // Obtengo la imputación
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, collectId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, moduleId);

                    BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                            typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()));

                    if (businessCollection.Count > 0)
                    {
                        foreach (ACCOUNTINGEN.Application imputationEntity in businessCollection.OfType<ACCOUNTINGEN.Application>())
                        {
                            application.Id = imputationEntity.ApplicationCode;
                        }
                    }

                    collectApplication = applicationDAO.GetApplication(application);
                    newImputation = collectApplication.Application;
                    newImputation.Id = 0; // Seteo en 0 para nuevo registro
                    newImputation.UserId = userId;
                    newImputation.RegisterDate = DateTime.Now;
                    newImputation.ModuleId = moduleId;

                    int technicalTransaction = GetTechnicalTransaction();
                    // Grabo la nueva cabecera de imputación
                    newImputation = applicationDAO.SaveImputation(newImputation, collectId, technicalTransaction);

                    // Si la imputación tiene primas en depósito se procede a la reversión.
                    if (HasPremiumRecievable(application.Id))
                    {
                        int payerTypeId = 1; // Quemado hasta definir

                        // Obtengo las primas en depósito.
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AppCode, application.Id);

                        BusinessCollection premiumReceivableCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()));

                        foreach (ACCOUNTINGEN.ApplicationPremium premiumReceivableItem in premiumReceivableCollection.OfType<ACCOUNTINGEN.ApplicationPremium>())
                        {
                            decimal paidAmount = 0;

                            PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();
                            premiumReceivableTransactionItem.Id = premiumReceivableItem.AppPremiumCode;

                            premiumReceivableTransactionItem = applicationPremiumItemDAO.GetPremiumRecievableTransactionItem(premiumReceivableTransactionItem);

                            paidAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                            premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount = (paidAmount * -1);
                            premiumReceivableTransactionItem.Id = 0; // Seteo en cero para el nuevo registro.

                            // Grabo la reversión de la prima en la tabla de primas
                            premiumReceivableTransactionItem = applicationPremiumItemDAO.SavePremiumRecievableTransactionItem(premiumReceivableTransactionItem,
                                newImputation.Id, Convert.ToDecimal(premiumReceivableItem.ExchangeRate), DateTime.Now, accountingDate);

                            if (premiumReceivableTransactionItem.Id > 0) // Si me reversó la prima, continua.
                            {
                                // Reverso comisiones descontadas
                                ObjectCriteriaBuilder discountedCommissionCriteriaBuilder = new ObjectCriteriaBuilder();
                                discountedCommissionCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.DiscountedCommission.Properties.PremiumReceivableTransCode, premiumReceivableItem.AppPremiumCode);

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
                                        discountedCommissionDto.ApplicationPremiumId = premiumReceivableTransactionItem.Id; // Se lo relaciona con la reversión del cobro.

                                        _discountedCommissionDAO.SaveDiscountedCommission(discountedCommissionDto); // Grabo el nuevo registro
                                    }
                                }

                                // Reverso las primas en depósito usadas.
                                ObjectCriteriaBuilder depositCriteriaBuilder = new ObjectCriteriaBuilder();

                                depositCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.DepositPremiumTransaction.Properties.PremiumReceivableTransCode, premiumReceivableItem.AppPremiumCode);

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

                                componentCollectionCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.ComponentCollection.Properties.PremiumReceivableTransCode, premiumReceivableItem.AppPremiumCode);

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

                            brokerCheckingAccountCriteriaBuilder.PropertyEquals(ACCOUNTINGEN.BrokerCheckingAccountTrans.Properties.ApplicationCode, application.Id);

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
                    if (moduleId != 2)
                    {
                        if (isReversed) // Realizo la actualizacíon del estado del recibo a anulado.
                        {
                            Collect newCollect = new Collect();

                            if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
                            {
                                newCollect.Id = collectId;
                            }

                            if (moduleId == Convert.ToInt32(ApplicationTypes.JournalEntry))
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
        /// <param name="moduleId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        public List<SEARCH.PremiumReceivableItemDTO> GetPremiumRecievableAppliedByCollectId(int collectId, int moduleId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, moduleId);

                UIView premiumReceivables = _dataFacadeManager.GetDataFacade().
                                GetView("PremiumRecievableApplicationView", criteriaBuilder.GetPredicate(),
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
                        PremiumReceivableItemId = Convert.ToInt32(dataRow["AppPremiumCode"]),
                        ImputationId = Convert.ToInt32(dataRow["AppCode"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        AccountingTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"])
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

                criteriaBuilder.Property(ACCOUNTINGEN.GetAppPremiumReceivableAgent.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.GetAppPremiumReceivableAgent.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);


                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                            typeof(ACCOUNTINGEN.GetAppPremiumReceivableAgent), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.GetAppPremiumReceivableAgent premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.GetAppPremiumReceivableAgent>())
                {
                    PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();
                    premiumReceivableTransactionItem.Id = premiumReceivableEntity.TempAppPremiumCode;
                    premiumReceivableTransactionItem = applicationPremiumItemDAO.GetPremiumRecievableTransactionItem(premiumReceivableTransactionItem);
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
        /// <returns>ApplicationDTO</returns>
        public ApplicationDTO GetTempImputationByPaymentOrderId(int paymentOrderId, int imputationTypeId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.SourceCode, paymentOrderId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.ModuleCode, imputationTypeId);
                //criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.IsRealSource, 1);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempApplication), criteriaBuilder.GetPredicate()));

                Models.Imputations.Application application = new Models.Imputations.Application();

                foreach (ACCOUNTINGEN.TempApplication imputationEntity in businessCollection.OfType<ACCOUNTINGEN.TempApplication>())
                {
                    application.Id = imputationEntity.TempAppCode;
                    application.RegisterDate = Convert.ToDateTime(imputationEntity.RegisterDate);
                    application.UserId = Convert.ToInt32(imputationEntity.UserCode);
                }

                return application.ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SavePaymentOrderApplication
        /// Graba imputación de orden de pago
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int SavePaymentOrderApplication(int paymentOrderId, int imputationTypeId, int userId)
        {
            try
            {
                bool isSaved = false;
                int paymentOrderSaved = 0;

                PaymentOrder newPaymentOrder = new PaymentOrder();

                // Se obtiene el temporal de la imputación
                ApplicationDTO tempImputation = GetTempImputationByPaymentOrderId(paymentOrderId, imputationTypeId);

                // Se obtiene la cabecera 
                newPaymentOrder.Id = paymentOrderId;
                newPaymentOrder = _paymentOrderDAO.GetPaymentOrder(newPaymentOrder);

                if (newPaymentOrder.Id > 0)
                {
                    isSaved = SaveImputationRequestPaymentOrder(newPaymentOrder.Id, tempImputation.Id, userId, paymentOrderId, tempImputation.AccountingDate);

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
                // criteriaBuilder.Property(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId);
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
        /// Obtiene ApplicationDTO a partir de prima por cobrar
        /// </summary>
        ///<param name="applicationPremiumId"> </param>
        ///<returns>int</returns>
        public int GetApplicationIdByApplicationPremiumId(int applicationPremiumId)
        {
            int imputationId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.ApplicationPremium.Properties.AppPremiumCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(applicationPremiumId);

                //Asignamos BusinessCollection 
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()));


                foreach (ACCOUNTINGEN.ApplicationPremium applicationPremium in businessCollection.OfType<ACCOUNTINGEN.ApplicationPremium>())
                {
                    imputationId = Convert.ToInt32(applicationPremium.AppCode);
                }

                return imputationId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetApplicationIdByTechnicalTransaction
        /// Obtiene ApplicationId a partir de technicalTransaction
        /// </summary>
        ///<param name="technicalTransaction"> </param>
        ///<returns>int</returns>
        public int GetApplicationIdByTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                return applicationDAO.GetApplicationIdByTechnicalTransaction(technicalTransaction);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public CollectApplicationDTO GetApplicationByTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                return applicationDAO.GetApplicationByTechnicalTransaction(technicalTransaction).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetApplicationIdByTechnicalTransaction
        /// Obtiene ApplicationId a partir de technicalTransaction
        /// </summary>
        ///<param name="technicalTransaction"> </param>
        ///<returns>int</returns>
        public CollectApplicationDTO GetApplicationByApplication(ApplicationDTO application)
        {
            try
            {
                return applicationDAO.GetApplication(application.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
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

                UIView massiveProcesses = _dataFacadeManager.GetDataFacade().GetView("MassiveProcessDataBillView_",
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
                        //EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        PayerId = Convert.ToInt32(dataRow["PayerId"]),
                         CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        //PaymentIndividualId = Convert.ToInt32(dataRow["PaymentIndividualId"]),
                        //PaymentMethodCode = Convert.ToInt32(dataRow["PaymentMethodCode"]),
                        //PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        PolicyNumber = Convert.ToInt32(dataRow["PolicyNumber"]),
                        EndorsementNumber = Convert.ToInt32(dataRow["EndorsementNumber"]),
                        //Quote = Convert.ToInt32(dataRow["Quote"]),
                        BranchId = Convert.ToInt32(dataRow["BranchId"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        AgentId = Convert.ToInt32(dataRow["AgentId"]),
                        AgentName = Convert.ToString(dataRow["AgentName"]),
                        BeneficiaryDocumentNumber = Convert.ToString(dataRow["BeneficiaryDocumentNumber"]),
                        BeneficiaryName = Convert.ToString(dataRow["BeneficiaryName"]), 
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
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
        /// <param name="tempApplicationId"></param>
        /// <param name="tempInsuredLoanId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempInsuredLoanTransactionItem(int tempApplicationId, int tempInsuredLoanId)
        {
            return new bool();
        }

        /// <summary>
        /// GetTmpInsuredLoansByTempImputationId
        /// Obtiene Temporal de Préstamos de Asegurados 
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<InsuredLoanTransaction/></returns>
        public List<InsuredLoanTransactionDTO> GetTmpInsuredLoansByTempApplicationId(int tempApplicationId)
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
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PolicyId, policyId);
                //criteriaBuilder.And();
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.EndorsementId, endorsementId);

                //  BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PremiumReceivableTrans), criteriaBuilder.GetPredicate()));

                /*  if (businessCollection.Count > 0)
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
                          Models.Imputations.Application application = new Models.Imputations.Application();
                          application.Id = Convert.ToInt32(premiumReceivableEntity.ImputationCode);

                          appPaymentPolicyDto.UserId = Convert.ToInt32(applicationDAO.GetApplication(application).Application.UserId);
                          appPaymentPolicyDto.CollectCode = Convert.ToInt32(GetSourceCodeByImputationId(application.Id));

                          collections.Add(appPaymentPolicyDto);
                      }
            }*/
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
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyId, Convert.ToInt32(policyDocumentNumber));
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PrefixCode, prefixId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.BranchCode, branchId);
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyDocumentNumber);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(policyDocumentNumber + "%");
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.AppReceivablePremiumStatusSearch.Properties.PolicyCurrentTo);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(DateTime.Now);
                }

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.AppReceivablePremiumStatusSearch), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.AppReceivablePremiumStatusSearch receivablePremium in businessCollection.OfType<ACCOUNTINGEN.AppReceivablePremiumStatusSearch>())
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

                int parameterId = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_TRANSACTION_NUMBER));
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
                        Tax = Convert.ToDecimal(dataRow["Tax"]).ToString(),
                        Expenses = Convert.ToDecimal(dataRow["Expenses"]),
                        //ComponentId = Convert.ToInt32(dataRow["ComponentId"]).ToString(),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        TotalPremium = Convert.ToDecimal(dataRow["PaymentAmount"]) + Convert.ToDecimal(dataRow["Tax"]) + Convert.ToDecimal(dataRow["Expenses"])
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


                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPayment.Properties.TempApplicationCode, imputationId);

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
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentRequestItems.Properties.TempApplicationCode, imputationId);

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
        /// <param name="tempApplicationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempPremiumRecievable(int tempApplicationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempApplicationPremium premiumReceivableEntity in businessCollection.OfType<ACCOUNTINGEN.TempApplicationPremium>())
                {
                    if (premiumReceivableEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(premiumReceivableEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(premiumReceivableEntity.TempAppPremiumCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempApplicationPremium tempPremiumReceivableEntity = (ACCOUNTINGEN.TempApplicationPremium)
                                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempPremiumReceivableEntity.LocalAmount = tempPremiumReceivableEntity.Amount * exchangeRate;
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
        /// <param name="tempApplicationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempBrokerCheckingAccount(int tempApplicationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.PropertyEquals(ACCOUNTINGEN.TempBrokerCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);

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
        /// <param name="tempApplicationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempCoinsuranceCheckingAccount(int tempApplicationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempCoinsCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);

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
        /// <param name="tempApplicationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempReinsuranceCheckingAccount(int tempApplicationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempReinsCheckingAccTrans.Properties.TempApplicationCode, tempApplicationId);

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
        /// <param name="tempApplicationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempClaimPayment(int tempApplicationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.TempApplicationCode, tempApplicationId);

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
        /// <param name="tempApplicationId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        private bool RecalculatingForeignCurrencyAmountTempDailyAccounting(int tempApplicationId, List<SEARCH.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates)
        {
            bool isRecalculated = true;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.TempAppCode, tempApplicationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempApplicationAccounting), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.TempApplicationAccounting dailyAccountingEntity in businessCollection.OfType<ACCOUNTINGEN.TempApplicationAccounting>())
                {
                    if (dailyAccountingEntity.CurrencyCode != 0)
                    {
                        decimal exchangeRate = GetForeignCurrencyExchangeRate(Convert.ToInt32(dailyAccountingEntity.CurrencyCode), foreignCurrencyExchangeRates);

                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccounting.CreatePrimaryKey(dailyAccountingEntity.TempAppAccountingCode);

                        // Encuentra el objeto en referencia a la llave primaria
                        ACCOUNTINGEN.TempApplicationAccounting tempDailyAccountingEntity = (ACCOUNTINGEN.TempApplicationAccounting)
                            (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        tempDailyAccountingEntity.LocalAmount = tempDailyAccountingEntity.Amount * exchangeRate;
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
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AppCode, imputationId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()));

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
        /// <param name="tempAppAccountingId"></param>
        /// <returns>List<DailyAccountingAnalysisCode></returns>
        public List<ApplicationAccountingAnalysisDTO> GetTempApplicationAccountingAnalysisByTempAppAccountingId(int tempAppAccountingId)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.GetTempApplicationAccountingAnalysisByTempAppAccountingId(tempAppAccountingId).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempApplicationAccountingCostCentersByTempAppAccountingId
        /// </summary>
        /// <param name="tempAppAccountingId"></param>
        /// <returns>List<ApplicationAccountingCostCenterDTO></returns>
        public List<ApplicationAccountingCostCenterDTO> GetTempApplicationAccountingCostCentersByTempAppAccountingId(int tempAppAccountingId)
        {
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                return accountingBusiness.GetTempApplicationAccountingCostCentersByTempAppAccountingId(tempAppAccountingId).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(Resources.Resources.ErrorGetTempApplicationAccountingCostCentersByTempAppAccountingId);
            }
        }



        #endregion TempDailyAccountingAnalysis


        #region DailyAccountingTransactionItem

        /// <summary>
        /// SaveDailyAccountingTransactionItem
        /// </summary>
        /// <param name="applicationAccountingDTO"></param>
        /// <param name="imputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        private int SaveApplicationAccountingItem(ApplicationAccountingDTO applicationAccountingDTO, int imputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate)
        {
            int appAccountingId = 0;

            try
            {
                appAccountingId = applicationAccountingItemDAO.SaveApplicationAccounting(applicationAccountingDTO.ToModel(), imputationId, paymentConceptCode, description, bankReconciliationId, receiptNumber, receiptDate);

                //grabación de analisis.
                if (applicationAccountingDTO.AccountingAnalysisCodes != null
                    && applicationAccountingDTO.AccountingAnalysisCodes.Count > 0)
                {
                    foreach (ApplicationAccountingAnalysisDTO accountingAnalysis in applicationAccountingDTO.AccountingAnalysisCodes)
                    {
                        accountingAnalysis.Id = 0; //es clave autonumérica
                        applicationAccountingAnalysisDAO.SaveAccountingAnalysisCode(accountingAnalysis.ToModel(), appAccountingId);
                    }
                }

                if (applicationAccountingDTO.AccountingCostCenters != null
                    && applicationAccountingDTO.AccountingCostCenters.Count > 0)
                {
                    foreach (ApplicationAccountingCostCenterDTO accountingCostCenter in applicationAccountingDTO.AccountingCostCenters)
                    {
                        accountingCostCenter.Id = 0; //es clave autonumérica
                        applicationAccountingCostCenterDAO.SaveApplicationAccountingCostCenter(accountingCostCenter.ToModel(), appAccountingId);
                    }
                }

            }
            catch (BusinessException)
            {
                appAccountingId = 0;
            }

            return appAccountingId;
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

                    DateTime accountingDate = Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.
                                                                 GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)
/*                                                                 ConfigurationManager.AppSettings["ModuleDateAccounting"]*/))).Split()[0]);

                    // Recorre las cobranzas para para ejecutar el proceso de inserción.
                    List<PremiumReceivableTransactionItem> premiumReceivableTransactions = GetPremiumRecievableTransactionPartialClousure(dateTo, dateFrom).ToModels().ToList();

                    if (premiumReceivableTransactions.Count > 0)
                    {
                        foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in premiumReceivableTransactions)
                        {
                            // Obtengo los ramos y subramos
                            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppPrefixPremiumReceivable.Properties.AppPremiumCode, Convert.ToInt32(premiumReceivableTransactionItem.Id));

                            decimal exchangeRate = 0;
                            exchangeRate = premiumReceivableTransactionItem.Policy.ExchangeRate.SellAmount;

                            decimal amount = 0;
                            amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;

                            UIView receivablePremiums = _dataFacadeManager.GetDataFacade().GetView("GetAppPrefixPremiumReceivableView",
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

                                            CollectApplication collectApplication;
                                            Models.Imputations.Application application = new Models.Imputations.Application();

                                            application.Id = GetApplicationIdByApplicationPremiumId(premiumReceivableTransactionItem.Id);
                                            collectApplication = applicationDAO.GetApplication(application);
                                            application = collectApplication.Application;

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
                                                sourceId = GetSourceIdByApplication(application.ToDTO());

                                                brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);

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
                                                                sourceId = GetSourceIdByApplication(application.ToDTO());

                                                                brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);
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
                                                            sourceId = GetSourceIdByApplication(application.ToDTO());

                                                            brokersCheckingAccountTransactionItem = _brokerCheckingAccountTransactionItemDAO.SaveBrokerCheckingAccountTransactionItem(brokersCheckingAccountTransactionItem, application.Id, 0, sourceId, agentTypeId, accountingDate);
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
                                                        coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, application.Id, 0);

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
                                                        coinsuranceCheckingAccountTransactionItem = _coinsuranceCheckingAccountTransactionItemDAO.SaveCoinsuranceCheckingAccountTransactionItem(coinsuranceCheckingAccountTransactionItem, application.Id, 0);

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

                    DateTime accountingDate = Convert.ToDateTime(Convert.ToString(DelegateService.accountingParameterService.
                                                                 GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)
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
                            CollectApplication collectApplication;
                            Models.Imputations.Application application = new Models.Imputations.Application();

                            application.Id = GetApplicationIdByApplicationPremiumId(premiumReceivableTransactionItem.Id);
                            collectApplication = applicationDAO.GetApplication(application);
                            application = collectApplication.Application;

                            int sourceId = GetSourceIdByApplication(application.ToDTO());

                            isSaved = BrokersCommission(premiumReceivableTransactionItem.ToDTO(), application.ToDTO(), sourceId);

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
        #endregion
        #endregion
        #endregion

        /// <summary>
        /// Obtiene el identificador de la cuenta para un banco / moneda
        /// </summary>
        /// <param name="bankId">Identificador del banco</param>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <returns>Identificador de la cuenta</returns>
        public int GetAccountingAccountIdByBankIdCurrencyId(int bankId, int currencyId)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetAccountingAccountIdByBankIdCurrencyId(bankId, currencyId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// Obtiene el identificador de la cuenta para un banco / moneda
        /// </summary>
        /// <param name="bankId">Identificador del banco</param>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <param name="accountNumer">Identificador de la cuenta</param>
        /// <returns>Identificador de la cuenta</returns>
        public int GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(int bankId, int currencyId, string accountNumer)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(bankId, currencyId, accountNumer);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        public bool ReverseApplicationPremiumByCollectPaymentId(int collectPaymentId)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.ReverseApplicationPremiumByCollectPaymentId(collectPaymentId);
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public List<ApplicationPremiumCommisionDTO> GetTempApplicationPremiumCommissDTOs()
        {
            try
            {
                return applicationPremiumCommisionDAO.GetTempApplicationPremiumCommisses().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public List<SEARCH.DiscountedCommissionDTO> GetTempApplicationPremiumCommissByTempAppId(string policyId, string endorsementId, int tempAppPremiumId)
        {
            try
            {
                List<SEARCH.DiscountedCommissionDTO> temps = applicationPremiumCommisionDAO.GetTempApplicationPremiumCommissByTempAppId(tempAppPremiumId).ToDtTOs().ToList();
                if (policyId != string.Empty && temps.Count > 0)
                {
                    List<UNDDTOs.IssuanceAgencyDTO> issuanceAgencyDTOs = DelegateService.integrationUnderwritingService.GetAgentsByPolicyIdEndorsementId(Convert.ToInt32(policyId), Convert.ToInt32(endorsementId));

                    List<SEARCH.DiscountedCommissionDTO> discountedCommissionDTO = ModelAssembler.CreateAgentCommissions(issuanceAgencyDTOs, temps);//canUsedCommission);
                    return discountedCommissionDTO;
                }
                else
                {
                    return temps;
                }
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }

        public ApplicationPremiumCommisionDTO CreateTempApplicationPremiumCommisses(TempApplicationPremiumCommissDTO tempApplicationPremiumCommiss)
        {
            try
            {
                return applicationPremiumCommisionDAO.CreateTempApplicationPremiumCommisses(tempApplicationPremiumCommiss.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public ApplicationPremiumCommisionDTO UpdateTempApplicationPremiumCommisses(TempApplicationPremiumCommissDTO tempApplicationPremiumCommiss)
        {
            try
            {
                TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
                if (tempApplicationPremiumCommiss.Id > 0)
                {
                    tempApplicationPremiumDAO.UpdateCommissonTempApplicationPremium(tempApplicationPremiumCommiss.TempApplicationPremiumId, tempApplicationPremiumCommiss.LocalAmount);
                    return applicationPremiumCommisionDAO.UpdateTempApplicationPremiumCommisses(tempApplicationPremiumCommiss.ToModel()).ToDTO();
                }
                else
                {
                    tempApplicationPremiumDAO.UpdateCommissonTempApplicationPremium(tempApplicationPremiumCommiss.TempApplicationPremiumId, tempApplicationPremiumCommiss.LocalAmount);
                    return applicationPremiumCommisionDAO.CreateTempApplicationPremiumCommisses(tempApplicationPremiumCommiss.ToModel()).ToDTO();
                }

            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public bool DeleteTempApplicationPremiumCommisses(int id)
        {
            try
            {
                bool deleteTempApplicationPremiumCommisses = applicationPremiumCommisionDAO.DeleteTempApplicationPremiumCommisses(id);
                return deleteTempApplicationPremiumCommisses;
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public List<ApplicationPremiumCommisionDTO> GetApplicationPremiumCommisions()
        {
            try
            {
                return applicationPremiumCommisionDAO.GetApplicationPremiumCommisions().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public List<ApplicationPremiumCommisionDTO> GetApplicationPremiumCommisionsByApplicationPremiumId(int applicationPremiumId)
        {
            try
            {
                return applicationPremiumCommisionDAO.GetApplicationPremiumCommissByAppPremiumId(applicationPremiumId).ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public ApplicationPremiumCommisionDTO CreateApplicationPremiumCommision(ApplicationPremiumCommisionDTO applicationPremiumCommisionDTO)
        {
            try
            {
                return applicationPremiumCommisionDAO.CreateApplicationPremiumCommision(applicationPremiumCommisionDTO.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public ApplicationPremiumCommisionDTO UpdateApplicationPremiumCommision(ApplicationPremiumCommisionDTO applicationPremiumCommisionDTO)
        {
            try
            {
                return applicationPremiumCommisionDAO.UpdateApplicationPremiumCommision(applicationPremiumCommisionDTO.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }
        public bool DeleteApplicationPremiumCommision(int id)
        {
            try
            {
                return applicationPremiumCommisionDAO.DeleteApplicationPremiumCommision(id);
            }
            catch (BusinessException exception)
            {

                throw new BusinessException(exception.Message);
            }
        }

        public decimal GetParticipationPercentageByEndorsementId(int endorsementId)
        {
            try
            {
                IntegrationBusiness integrationBusiness = new IntegrationBusiness();
                return integrationBusiness.GetParticipationPercentageByEndorsementId(endorsementId);
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetParticipationPercentageByEndorsementId);
            }
        }


        public bool ReverseApplicationPremiumByPremiumId(int premiumId)
        {
            throw new NotImplementedException();
        }
        #region validacion temporales
        public TemporalPremiumDTO ValidatePremiumTemporal(PremiumFilterDTO premiumFilterDTO)
        {
            try
            {
                return ApplicationDataDAO.ValidatePremiumTemporal(premiumFilterDTO);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }
        #endregion validacion temporales

        #region Aplicacion Primas Portal
        /// <summary>
        /// Saves the temporary premium component.
        /// </summary>
        /// <param name="premiumRequestDTO">The premium request dto.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public int SaveTmpPremiumComponent(PremiumRequestDTO premiumRequestDTO)
        {
            int recorded = 0;
            int decimalPlaces = 2;
            if (premiumRequestDTO.PremiumReceivableTransaction != null && premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems.Any())
            {
                try
                {
                    PremiumReceivableTransactionItemDTO tempApplicationPremium = null;
                    ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                    for (int i = 0; i < premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems.Count; i++)
                    {
                        PremiumReceivableTransactionItem modelPremium = premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems[i].ToModel();
                        if (modelPremium.DeductCommission != null)
                        {
                            modelPremium.DeductCommission.Value = premiumRequestDTO.PremiumReceivableTransaction.CommissionsDiscounted?.Sum(x => x.LocalAmount) ?? decimal.Zero;
                        }
                        decimal percentageCoinsurace = DelegateService.integrationUnderwritingService.GetPercentageByEndorsementId(modelPremium.Policy.Endorsement.Id);
                        if (modelPremium.Id == 0)
                        {
                            tempApplicationPremium = tempApplicationPremiumItemDAO.SaveTempPremiumRecievableTransactionItem(modelPremium, premiumRequestDTO.ApplicationId, modelPremium.Policy.ExchangeRate.SellAmount, premiumRequestDTO.UserId, premiumRequestDTO.RegisterDate, premiumRequestDTO.AccountingDate).ToDTO();
                            SaveCommisionDiscount(tempApplicationPremium.Id, premiumRequestDTO.PremiumReceivableTransaction.CommissionsDiscounted);

                            var payerPaymentComponentDTOs = applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems[i].Policy.Endorsement.Id,
                                premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems[i].Policy.PaymentPlan.Quotas[0].Number);

                            payerPaymentComponentDTOs = payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.I.ToString()
                                            || x.TinyDescription == ComponentTypes.P.ToString() || x.TinyDescription == ComponentTypes.G.ToString()).ToList();

                            AccountingModels.Payments.PaymentComponentModel PaymentComponentModel = new AccountingModels.Payments.PaymentComponentModel { AppId = tempApplicationPremium.Id, CurrencyId = tempApplicationPremium.Policy.ExchangeRate.Currency.Id, ExchangeRate = tempApplicationPremium.Policy.ExchangeRate.SellAmount, payerPaymentComponentDTOs = payerPaymentComponentDTOs, PercentageCoinsurance = percentageCoinsurace };
                            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = ApplicationPremiumBusiness.CreatePremiumComponent(PaymentComponentModel);
                            foreach (TempApplicationPremiumComponent tempApplicationPremiumComponent in tempApplicationPremiumComponents)
                            {
                                tempApplicationPremiumComponentDAO.SaveTempApplicationPremiumComponent(tempApplicationPremiumComponent);
                            }

                            TempApplicationPremium tempApplicationUpd = new TempApplicationPremium()
                            {
                                Amount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.Amount, decimalPlaces)), decimalPlaces),
                                LocalAmount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.LocalAmount, decimalPlaces)), decimalPlaces),
                                MainLocalAmount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.MainLocalAmount, decimalPlaces)), decimalPlaces),
                                MainAmount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.MainAmount, decimalPlaces)), decimalPlaces),
                                ExchangeRate = tempApplicationPremium.Policy.ExchangeRate.SellAmount,
                                Id = tempApplicationPremium.Id
                            };
                            
                            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
                            tempApplicationPremiumDAO.UpdateTempApplicationPremiumAmounts(tempApplicationUpd);
                            recorded = 1;
                        }
                    }
                    return recorded;
                }
                catch (BusinessException ex)
                {
                    throw new BusinessException(ex.Message);
                }
            }
            return 0;
        }

        private void SaveCommisionDiscount(int appId, List<ApplicationPremiumCommisionDTO> commissionsDiscounteds)
        {
            if (commissionsDiscounteds != null)
            {
                ApplicationPremiumCommisionDTO tempApplicationPremiumCommiss = null;
                for (int i = 0; i < commissionsDiscounteds.Count; i++)
                {
                    tempApplicationPremiumCommiss = new ApplicationPremiumCommisionDTO();
                    tempApplicationPremiumCommiss = commissionsDiscounteds[i];
                    tempApplicationPremiumCommiss.ApplicationPremiumId = appId;
                    applicationPremiumCommisionDAO.CreateTempApplicationPremiumCommisses(tempApplicationPremiumCommiss.ToModel());
                }
            }
        }
        #endregion

        public MessageDTO SaveApplicationByTempApplicationIdUserId(int tempApplicationId, int userId)
        {
            try
            {
                TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                return tempApplicationBusiness.SaveApplicationGeneralLedger(tempApplicationId, userId).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        /// <summary>
        /// Get list of payment quotas
        /// </summary>
        /// <param name="searchPolicyPayment">Search Criteria</param>
        /// <returns>List of payment quotas</returns>
        public List<Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> GetPaymentQuotas(
            Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.SearchPolicyPaymentDTO searchPolicyPayment)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetPaymentQuotas(searchPolicyPayment);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentQuotas);
            }
        }


        /// <summary>
        /// Get list of payment quotas
        /// </summary>
        /// <param name="searchPolicyPayment">Search Criteria</param>
        /// <returns>List of payment quotas</returns>
        public List<Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> GetTempApplicationsPremiumByTempApplicationId(int tempApplicationId)
        {
            try
            {
                TempApplicationBusiness applicationBusiness = new TempApplicationBusiness();
                return applicationBusiness.GetTempApplicationsPremiumByTempApplicationId(tempApplicationId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentQuotas);
            }
        }

        public List<Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PayerPaymentComponentDTO> GetPayerPaymentComponentsByEndorsementIdQuotaNumber(int endorsementId, int quotaNumber)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(endorsementId, quotaNumber);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPayerPaymentComponentsByEndorsementIdQuotaNumber);
            }
        }

        /// <summary>
        /// GetTempApplicationAccountingCostCentersByTempAppAccountingId
        /// </summary>
        /// <param name="tempAppAccountingId"></param>
        /// <returns>List<ApplicationAccountingCostCenterDTO></returns>
        public int CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(int AnalysisConceptKeyId, string KeyValue)
        {
            int result = 0;
            try
            {
                ApplicationBusiness accountingBusiness = new ApplicationBusiness();
                result = accountingBusiness.CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(AnalysisConceptKeyId, KeyValue);
                if (result == -4)// validar mensaj correpo
                {
                    throw new BusinessException(Resources.Resources.ErrorCheckoutAnalysisConceptKey);//
                }
                else if (result == -3)
                {
                    throw new BusinessException(Resources.Resources.ErrorNotFoundConceptKeyId);
                }
                else if (result == -2)
                {
                    throw new BusinessException(string.Format(Resources.Resources.ErrorNotFoundTableAnalysisCode, KeyValue));
                }
                else if (result == -1)
                {
                    throw new BusinessException(string.Format(Resources.Resources.ErrorNotFoundColumnAnalysisCode, KeyValue));
                }
                else if (result == 0)
                {
                    throw new BusinessException(string.Format(Resources.Resources.ErrorAnalysisCodeValueNotExist, KeyValue));
                }


                return result;
            }
            catch (BusinessException bussinesExecp)
            {
                throw new BusinessException(bussinesExecp.Message);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorCheckoutAnalysisConceptKey);
            }
        }

        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="Exception"></exception>
        public long SavePaymentAuthorization(string ballotNumber)
        {
            try
            {
                return PaymentAuthorizationDAO.SavePaymentAuthorization(ballotNumber);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <param name="technicalTransaction">The technical transaction.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="Exception"></exception>
        public long UpdatePaymenTechnicalTransactionforAuthorization(long paymentAuthorizationId, int technicalTransaction)
        {
            try
            {
                return PaymentAuthorizationDAO.UpdatePaymentAuthorization(paymentAuthorizationId, technicalTransaction);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public MessageDTO ReverseApplication(int sourceId, int moduleId, int userId)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.ReverseApplication(sourceId, moduleId, userId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(Resources.Resources.ErrorReverseApplication);
            }
        }

        public List<ApplicationPremiumDTO> GetApplicationPremiumsByEndorsementId(int endorsementId)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.GetApplicationPremiumsByEndorsementId(endorsementId).ToDTOs();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(Resources.Resources.ErrorGetApplicationPremiumsByEndorsementId);
            }
        }

        public MessageDTO ReverseJournalEntry(JournalEntryReversionParametersDTO parameters)
        {
            try
            {
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                return applicationBusiness.ReverseJournalEntry(parameters.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(Resources.Resources.ErrorReverseJournalEntry);
            }
        }

        public PaymentOrderDTO SaveTempApplicationData(PremiumRequestDTO applicationRequest)
        {
            try
            {
                TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                return tempApplicationBusiness.SaveTempApplication(applicationRequest);
            }
            catch (BusinessException message)
            {

                throw new BusinessException(Resources.Resources.ErrorSavingApplication);
            }
        }
        
        public bool SaveLogMassiveDataPolicy(LogMassiveDataPolicyDTO logMassiveData) {
            try
            {
                TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                return tempApplicationBusiness.SaveLogMassiveDataPolicy(logMassiveData);
            }
            catch (BusinessException message)
            {

                throw new BusinessException(Resources.Resources.ErrorSaveLogMassiveDataPolicy);
            }
        }

        public bool ExistsPaymenTechnicalTransactioByBallotNumber(string ballotNumber)
        {
            try
            {
                return PaymentAuthorizationDAO.ExistsPaymentAuthorization(ballotNumber);
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.Resources.ErrorExistsPaymenTechnicalTransactioByBallotNumber);
            }
        }
    }
}