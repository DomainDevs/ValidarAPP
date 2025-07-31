using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Views;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using CoreTransaction = Sistran.Core.Framework.Transactions;
using SCRMOD = Sistran.Core.Application.AccountingServices.DTOs.Search;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingCollectServiceEEProvider : IAccountingCollectService
    {

        #region Instance Viarables
        private const int ModuleId = 2;
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #region DAOs

        readonly PaymentDAO _paymentDAO = new PaymentDAO();
        readonly CollectControlDAO _collectControlDAO = new CollectControlDAO();
        readonly RegularizedPaymentDAO _regularizedPaymentDAO = new RegularizedPaymentDAO();
        readonly PaymentTaxDAO _paymentTaxDAO = new PaymentTaxDAO();
        readonly TempPremiumReceivableTransactionItemDAO _tempPremiumReceivableTransactionItemDAO = new TempPremiumReceivableTransactionItemDAO();
        //readonly ImputationDAO _imputationDAO = new ImputationDAO();
        readonly ApplicationDAO applicationDAO = new ApplicationDAO();
        //readonly TempImputationDAO _tempImputationDAO = new TempImputationDAO();
        readonly DAOs.Accounting.TempApplicationDAO tempApplicationDAO = new DAOs.Accounting.TempApplicationDAO();
        readonly CollectDAO _collectDAO = new CollectDAO();
        readonly CollectMassiveProcessDAO _collectMassiveProcessDAO = new CollectMassiveProcessDAO();
        readonly JournalEntryDAO _journalEntryDAO = new JournalEntryDAO();
        readonly AccountingPaymentServiceEEProvider _paymentService = new AccountingPaymentServiceEEProvider();
        readonly CompanyDAO _companyDAO = new CompanyDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region CollectApplication

        /// <summary>
        /// SaveCollectImputation
        /// Graba 3 movimientos, 1) la aplicacion del recibo; 2) el recibo e imputatción; 3)asiento de diario,
        /// </summary>
        /// <param name="collectImputation">Aplicación</param>
        /// <param name="collectControlId">Identificador de la caja</param>
        /// <param name="mustToApply">Indica si debe convertir la aplicación temporal a permanente</param>
        /// <returns>Aplicación</returns>
        public CollectApplicationDTO SaveCollectImputation(CollectApplicationDTO collectImputation, int collectControlId, bool mustToApply = false)
        {
            CollectControlBusiness collectControlBusiness = new CollectControlBusiness();
            Collect collect;
            if (collectImputation.Transaction.TechnicalTransaction > 0)
            {
                collect = GetCollectByTransactionId(collectImputation.Transaction.TechnicalTransaction);
                collectImputation.Collect = collect.ToDTO();
            }
            else
            {
                collect = collectImputation.Collect.ToModel();
            }
            Collect updatedCollect = new Collect();
            Models.Imputations.Application imputation = new Models.Imputations.Application();
            CollectApplication collectImputationResult = new CollectApplication();
            collectImputationResult.Collect = collect;



            int temporalNumber = 0;
            DateTime accountingDate = collectControlBusiness.GetAccountingDateByCollectControlId(collectControlId);

            CollectApplicationDTO collectApplicationDTO = SaveCollect(collectImputation, collectControlId, mustToApply);
            collectApplicationDTO.Collect.PaymentsTotal = new AmountDTO()
            {
                Value = collect.PaymentsTotal.Value
            };
            if (collect.Id > 0 && collectApplicationDTO.Collect.Id == 0)
            {
                collectApplicationDTO.Collect.Id = collect.Id;
            }

            #region Recibo e imputacion


            if (collectImputation.Application != null)
            {
                temporalNumber = -1;
                imputation = collectApplicationDTO.Application.ToModel();
                imputation.ModuleId = (int)ApplicationTypes.Collect;
            }

            collectImputationResult = SaveApplication(collectApplicationDTO, temporalNumber, accountingDate, mustToApply);

            #endregion Aplicación del Recibo

            #endregion Recibo e imputacion

            #region Asiento de Dario

            // TransactionId = Nro.Temporal del Asiento
            if (temporalNumber > 0 && imputation.ModuleId == (int)Application.AccountingServices.Enums.ApplicationTypes.JournalEntry)
            {
                collectImputationResult = DelegateService.accountingApplicationService.SaveJournalEntryImputation(temporalNumber, imputation.Id, collect.UserId).ToModel();
            }

            #endregion Asiento de Dario

            return collectImputationResult.ToDTO();
        }
        public CollectApplicationDTO SaveCollect(CollectApplicationDTO collectImputation, int collectControlId, bool mustToApply = false)
        {
            Collect collect = collectImputation.Collect.ToModel();
            Collect updatedCollect = new Collect();
            Models.Imputations.Application imputation = new Models.Imputations.Application();
            CollectApplication collectImputationResult = new CollectApplication();
            collectImputationResult.Collect = collect;


            if (collectImputation.Collect != null && collect.Transaction == null)
            {
                TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
                {
                    BranchId = collectImputation.Collect.Branch.Id
                };

                TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);

                // Se actualiza el número de transacción 
                collectImputation.Collect.Transaction = new TransactionDTO()
                {
                    TechnicalTransaction = technicalTransaction.Id
                };

                collect.Transaction = new Models.Collect.Transaction()
                {
                    TechnicalTransaction = collectImputation.Collect.Transaction.TechnicalTransaction,
                    Id = 0
                };
            }

            if (collect.Id == 0)
            {
                CoreTransaction.Transaction.Created += delegate (object sender, TransactionEventArgs e)
                { };
                using (Context.Current)
                {
                    using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                    {
                        transaction.Completed += delegate (object sender, TransactionEventArgs e)
                        { };
                        transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                        { };
                        try
                        {
                            #region Collect

                            Collect newCollect = _collectDAO.SaveCollect(collect, collectControlId);

                            //Update Preliquidation
                            //Recibo ya aplicado
                            if (collectImputation.Application == null)
                            {
                                PrimaryKey primaryKey = ACCOUNTINGEN.Preliquidation.CreatePrimaryKey(collectImputation.Id);
                                ACCOUNTINGEN.Preliquidation preliquidationEntity = (ACCOUNTINGEN.Preliquidation)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                                if (preliquidationEntity != null)
                                {
                                    preliquidationEntity.TechnicalTransaction = collect.Transaction.TechnicalTransaction;
                                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(preliquidationEntity);
                                }
                            }

                            #endregion Collect

                            //graba pagos
                            if (collect.Payments != null && collect.Payments.Count > 0)
                            {
                                #region Payments

                                newCollect.Payments = new List<Models.Payments.Payment>();

                                foreach (Models.Payments.Payment payment in collect.Payments)
                                {
                                    Models.Payments.Payment newPayment = _paymentDAO.SavePayment(payment, newCollect.Id);

                                    if (payment.Taxes != null)
                                    {
                                        foreach (PaymentTax paymentTax in payment.Taxes)
                                        {
                                            _paymentTaxDAO.SavePaymentTax(paymentTax, newPayment.Id);
                                        }
                                    }

                                    newCollect.Payments.Add(newPayment);

                                    //Graba el Log de Payment
                                    _paymentService.SavePaymentLog(Convert.ToInt32(ActionTypes.CreatePayment), newCollect.Id, newPayment.Id, Convert.ToInt32(PaymentStatus.Active), newCollect.UserId);

                                    //Graba log de retenciones
                                    if (payment.PaymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_RETENTION_RECEIPT)))
                                    {
                                        _paymentDAO.SaveRetentionHistory(payment, newCollect.Id);
                                    }
                                }

                                #endregion Payments


                            }

                            updatedCollect.Id = newCollect.Id;
                            updatedCollect = _collectDAO.GetCollect(updatedCollect); //me devuelve el bill con la actualización de estado
                            updatedCollect.Payments = newCollect.Payments;

                            collectImputationResult.Collect = updatedCollect;
                            collectImputationResult.Application = imputation;
                            collect = updatedCollect;
                            collectImputation.Collect = updatedCollect.ToDTO();
                            if (collectImputation.Application != null)
                            {
                                DelegateService.accountingApplicationService.UpdateTempApplicationSourceCode(collectImputation.Application.Id, collect.Id);
                            }

                            transaction.Complete();
                        }
                        catch (BusinessException ex)
                        {
                            transaction.Dispose();

                            throw new BusinessException(Resources.Resources.BusinessException);
                        }
                    }
                }
            }
            else
            {
                List<Models.Payments.Payment> payments = _paymentDAO.GetPaymentsByCollectId(collect.Id);
                collectImputation.Collect.Payments = new List<DTOs.Payments.PaymentDTO>();
                collectImputation.Collect.Payments.AddRange(payments.ToDTOs());
            }
            return collectImputation;
        }

        public CollectApplication SaveApplication(CollectApplicationDTO collectImputation, int temporalNumber, DateTime acountingDate, bool mustToApply = false)
        {
            Collect collect = collectImputation.Collect.ToModel();
            Models.Imputations.Application imputation = new Models.Imputations.Application();
            CollectApplication collectImputationResult = new CollectApplication();
            collectImputationResult.Collect = collect;
            imputation = collectImputation.Application.ToModel();

            if (temporalNumber == -1 && mustToApply)
            {

                // Validar que la aplicación esté balanceada
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                // Controla que sólo se genera la aplicación permanente si el ingreso está balanceado
                if (applicationBusiness.ValidateTempApplicationByTotal(imputation.Id, collect.PaymentsTotal.Value))
                {
                    var imputationResult = DelegateService.accountingApplicationService.SaveApplication(collect.Id, imputation.Id, imputation.ModuleId, collect.Comments, collect.Status, collect.UserId, collect.Id, collect.Transaction.TechnicalTransaction, acountingDate).ToModel();
                    collectImputationResult.Application = imputationResult.Application;
                    collectImputationResult.Transaction = new Models.Collect.Transaction { TechnicalTransaction = collect.Transaction.TechnicalTransaction };
                }
                else
                {
                    var automaticUserId = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_AUTOMATIC_APPLICATION_USER);
                    if (automaticUserId >= 0 && collect.UserId == automaticUserId)
                    {
                        var tolerance = Convert.ToDouble(CommonBusiness.GetFloatParameter(Enums.AccountingKeys.ACC_APPLICATION_AMOUNT_TOLERAN));
                        if (tolerance > 0)
                        {
                            int decimalPlaces = 2;
                            var debitsandCredits = applicationBusiness.GetDebitsAndCreditsByTempApplicationId(imputation.Id);
                            var difference = Math.Round(Convert.ToDouble(collect.PaymentsTotal.Value - (debitsandCredits.Credits - debitsandCredits.Debits)), decimalPlaces);
                            var absDifference = Math.Abs(difference);

                            if (difference != 0 && absDifference <= tolerance)
                            {
                                collect.Branch = new Branch()
                                {
                                    Id = DelegateService.accountingCollectService.GetCollectByCollectId(collect.Id).Branch.Id
                                };

                                var salePointId = DelegateService.commonService.GetSalePointsByBranchId(collect.Branch.Id).First().Id;

                                ApplicationAccountingDTO applicationAccountingDTO = new ApplicationAccountingDTO()
                                {
                                    AccountingAnalysisCodes = new List<ApplicationAccountingAnalysisDTO>(),
                                    AccountingCostCenters = new List<ApplicationAccountingCostCenterDTO>(),
                                    ExchangeRate = new ExchangeRateDTO()
                                    {
                                        SellAmount = 1,
                                        BuyAmount = 1,
                                        Currency = new CurrencyDTO()
                                        {
                                            Id = 0
                                        }
                                    },
                                    Amount = new AmountDTO()
                                    {
                                        Value = Convert.ToDecimal(Math.Abs(difference)),
                                        Currency = new CurrencyDTO()
                                        {
                                            Id = 0
                                        }
                                    },
                                    LocalAmount = new AmountDTO()
                                    {
                                        Value = Convert.ToDecimal(Math.Abs(difference))
                                    },
                                    Beneficiary = new IndividualDTO()
                                    {
                                        IndividualId = collect.Payer.IndividualId
                                    },
                                    CurrencyId = 0,
                                    ApplicationAccountingId = imputation.Id,
                                    Description = Resources.Resources.AutomaticAdjustment,
                                    Branch = new BranchDTO() { Id = collect.Branch.Id },
                                    SalePoint = new SalePointDTO() { Id = salePointId },
                                    AccountingConcept = new ApplicationAccountingConceptDTO()
                                    {
                                        Id = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_AT_ACCOUNTING_CONCEPT_ID)
                                    },
                                    BookAccount = new BookAccountDTO()
                                    {
                                        Id = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_AT_ACCOUNTING_ID)
                                    }
                                };

                                if (difference > 0)
                                    applicationAccountingDTO.AccountingNature = Convert.ToInt32(AccountingNature.Credit);
                                else
                                    applicationAccountingDTO.AccountingNature = Convert.ToInt32(AccountingNature.Debit);

                                ApplicationAccountingTransactionDTO applicationAccountingTransaction = new ApplicationAccountingTransactionDTO();
                                applicationAccountingTransaction.ApplicationAccountingItems = new List<ApplicationAccountingDTO>();
                                applicationAccountingTransaction.ApplicationAccountingItems.Add(applicationAccountingDTO);

                                DelegateService.accountingApplicationService.SaveTempAccountingTransaction(applicationAccountingTransaction);
                                return SaveApplication(collectImputation, temporalNumber, acountingDate, mustToApply);
                            }
                        }
                    }
                }
            }
            return collectImputationResult;
        }

        public CollectApplicationDTO SaveCollectApplication(ApplicationDTO application)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// UpdateCollectImputation
        /// </summary>
        /// <param name="collectImputation"></param>
        /// <returns>CollectApplication</returns>
        public CollectApplicationDTO UpdateCollectImputation(CollectApplicationDTO collectImputation)
        {
            try
            {
                return applicationDAO.UpdateImputation(collectImputation.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetCollectImputations
        /// </summary>
        /// <param name="collectImputation"></param>
        /// <returns>List<AccountingModels.CollectApplication/></returns>
        public List<CollectApplicationDTO> GetCollectImputations(CollectApplicationDTO collectImputation)
        {
            List<CollectApplication> collectImputations = new List<CollectApplication>();

            try
            {
                ObjectCriteriaBuilder imputationFilter = new ObjectCriteriaBuilder();
                imputationFilter.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, collectImputation.Collect.Id);
                imputationFilter.And();
                imputationFilter.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, Convert.ToInt32(collectImputation.Application.ModuleId));

                BusinessCollection imputations = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Application), imputationFilter.GetPredicate()));

                if (imputations.Count > 0)
                {
                    foreach (ACCOUNTINGEN.Application imputationItem in imputations.OfType<ACCOUNTINGEN.Application>())
                    {
                        CollectApplication collectApplication = Assemblers.ModelAssembler.CreateCollectApplication(imputationItem);
                        collectImputations.Add(collectApplication);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return collectImputations.ToDTOs().ToList();
        }

        #endregion collectImputations

        #region Collect

        /// <summary>
        /// SaveRegularizationCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <param name="sourcePaymentId"></param>
        /// <returns>Collect</returns>
        public CollectDTO SaveRegularizationCollect(CollectDTO collect, int collectControlId, int sourcePaymentId, int brachId)
        {
            TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
            {
                BranchId = brachId
            };

            TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);

            collect.Transaction = new TransactionDTO()
            {
                TechnicalTransaction = technicalTransaction.Id
            };
            //graba collect
            Collect newCollect = _collectDAO.SaveCollect(collect.ToModel(), collectControlId);

            //graba pagos
            foreach (Models.Payments.Payment payment in collect.Payments.ToModels())
            {
                Models.Payments.Payment newPayment = _paymentDAO.SavePayment(payment, newCollect.Id);

                if (payment.Taxes != null)
                {
                    foreach (PaymentTax paymentTax in payment.Taxes)
                    {
                        _paymentTaxDAO.SavePaymentTax(paymentTax, newPayment.Id);
                    }
                }
                newCollect.Payments.Add(newPayment);
            }

            //graba pago regularizado
            int regularizedPaymentId = 0; //autonumerico

            regularizedPaymentId = _regularizedPaymentDAO.SaveRegularizedPayment(regularizedPaymentId, sourcePaymentId, newCollect.Id);

            //graba el log del pago regularizado
            AccountingPaymentServiceEEProvider paymentService = new AccountingPaymentServiceEEProvider();
            paymentService.SavePaymentLog(Convert.ToInt32(ActionTypes.PayRegularized), regularizedPaymentId, sourcePaymentId, Convert.ToInt32(PaymentStatus.Exchanged), newCollect.UserId);


            return newCollect.ToDTO();
        }

        /// <summary>
        /// UpdateCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <returns>Collect</returns>
        public CollectDTO UpdateCollect(CollectDTO collect, int collectControlId)
        {
            return _collectDAO.UpdateCollect(collect.ToModel(), collectControlId).ToDTO();
        }

        /// <summary>
        /// CancelCollect
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="collectControlId"></param>
        /// <param name="authorizationUserId"></param>
        /// <returns>int</returns>
        public int CancelCollect(int collectId, int collectControlId, int authorizationUserId)
        {
            int canceledCollectionId = 0;

            try
            {
                CollectControl collectControl = new CollectControl();
                collectControl.Id = collectControlId;

                collectControl = _collectControlDAO.GetCollectControl(collectControl);

                if (collectControl.Status == Convert.ToInt32(CollectControlStatus.Open))
                {
                    int status = Convert.ToInt32(CollectControlStatus.Close);

                    Collect collect = new Collect();
                    collect.Id = collectId;
                    collect = _collectDAO.GetCollect(collect);
                    collect.Status = status;
                    Collect canceledCollect;
                    canceledCollect = collect;
                    canceledCollect.Status = status;
                    canceledCollect.Transaction = new Models.Collect.Transaction();

                    canceledCollect = _collectDAO.UpdateCollect(canceledCollect, collectControlId);
                    List<Models.Payments.Payment> payments = _paymentDAO.GetPaymentsByCollectId(collectId);
                    AccountingPaymentServiceEEProvider paymentService = new AccountingPaymentServiceEEProvider();

                    TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
                    {
                        BranchId = collectControl.Branch.Id
                    };


                    TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);
                    collect.Transaction = new Models.Collect.Transaction { Id = 0, TechnicalTransaction = technicalTransaction.Id };
                    collect.Id = 0;
                    collect.PaymentsTotal.Value = collect.PaymentsTotal.Value * -1;
                    foreach (Models.Payments.Payment payment in payments)
                    {
                        payment.Amount.Value = payment.Amount.Value * -1;
                        payment.LocalAmount.Value = payment.LocalAmount.Value * -1;
                        payment.Status = status;
                        collect.Payments.Add(payment);
                        paymentService.SavePaymentLog(Convert.ToInt32(ActionTypes.ExchangePayment), collectId, payment.Id, Convert.ToInt32(PaymentStatus.Canceled), authorizationUserId);
                    }
                    Collect newCollect = SaveCollectRequestFromReplicate(collect, collectControl.Id);


                    canceledCollectionId = newCollect.Transaction.TechnicalTransaction;
                }

                if (collectControl.Status == Convert.ToInt32(CollectControlStatus.Close))
                {
                    canceledCollectionId = -1;
                }

                return canceledCollectionId;
            }
            catch (BusinessException)
            {
                throw new BusinessException(ConfigurationManager.AppSettings["UnhandledExceptionMsj"]);
            }
        }

        /// <summary>
        /// GetCollect
        /// </summary>
        /// <param name="collect"></param>
        /// <returns>Collect</returns>
        public CollectDTO GetCollect(CollectDTO collectDTO)
        {
            Collect collect = new Collect();
            try
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                collect = collectBusiness.GetCollect(collectDTO.ToModel());
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return collect.ToDTO();
        }

        public CollectDTO GetCollectByTechnicalTransaction(int technicalTransaction)
        {
            Collect collect = new Collect();
            try
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                collect = collectBusiness.GetCollectByTechnicalTransaction(technicalTransaction);


            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return collect.ToDTO();
        }
        public int GetTechnicalTransactionByPaymentId(int paymentId)
        {
            Collect collect = new Collect();
            try
            {
                return _collectDAO.GetTechnicalTransactionByPaymentId(paymentId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }


        }

        #endregion Collect

        #region PolicySearch

        /// <summary>
        /// GetPoliciesByCollectId
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns> List<CollectItemPolicyDTO/></returns>
        public List<SCRMOD.CollectItemPolicyDTO> GetPoliciesByCollectId(int collectId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.CollectId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(collectId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.Status);
                criteriaBuilder.In();
                criteriaBuilder.ListValue();
                criteriaBuilder.Constant((int)CollectStatus.Applied);
                criteriaBuilder.Constant((int)CollectStatus.Active);
                criteriaBuilder.Constant((int)CollectStatus.PartiallyApplied);
                criteriaBuilder.EndList();

                UIView policiesbyItemView = DataFacadeManager.Instance.GetDataFacade().GetView("PremiumReceivableTransCollectView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rowsGrid);

                //Add New row for return total records
                if (policiesbyItemView.Rows.Count > 0)
                {
                    policiesbyItemView.Columns.Add("rows", typeof(int));
                    policiesbyItemView.Rows[0]["rows"] = rowsGrid;
                }

                List<SCRMOD.CollectItemPolicyDTO> collectItemPolicyDTO = new List<SCRMOD.CollectItemPolicyDTO>();

                foreach (DataRow dataRow in policiesbyItemView)
                {
                    collectItemPolicyDTO.Add(new SCRMOD.CollectItemPolicyDTO()
                    {
                        BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                        PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        Endorsement = Convert.ToInt32(dataRow["Endorsement"]),
                        QuoteNum = Convert.ToInt32(dataRow["QuoteNum"]),
                        PayerId = Convert.ToInt32(dataRow["PayerCode"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        CollectCode = Convert.ToInt32(dataRow["CollectId"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        CollectItemCode = Convert.ToInt32(dataRow["AppPremiumCode"]),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"]),
                        Rows = rowsGrid
                    });
                }

                return collectItemPolicyDTO;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion PolicySearch

        #region SearchCollects

        /// <summary>
        /// SearchCollects
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="collectConceptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="collectId"></param>
        /// <param name="imputationType"></param>
        /// <param name="journalEntryStatusId"></param>
        /// <returns>List<SearchCollectDTO/></returns>
        public List<SCRMOD.SearchCollectDTO> SearchCollects(int branchId, int collectConceptId, string startDate, string endDate, int userId, int collectId, int imputationType, int journalEntryStatusId)
        {
            try
            {
                List<SCRMOD.SearchCollectDTO> searchCollects = new List<SCRMOD.SearchCollectDTO>();

                UIView data;

                #region Collect

                if (imputationType == Convert.ToInt32(Application.AccountingServices.Enums.ApplicationTypes.Collect))
                {

                    #region Filter

                    //Filtro
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.BranchCode, branchId);

                    if (collectConceptId >= 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectConceptCode, collectConceptId);
                    }

                    if (userId >= 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, userId);
                    }

                    if (collectId >= 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, collectId);
                    }

                    if (startDate != null && startDate != "")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.RegisterDate);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(FormatDateTime(startDate));
                    }
                    if (endDate != null && endDate != "")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.RegisterDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(FormatDateTime(endDate));
                    }

                    List<ACCOUNTINGEN.CollectControlV> dataCollectControl = DataFacadeManager.Instance.GetDataFacade().List(
                        typeof(ACCOUNTINGEN.CollectControlV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.CollectControlV>().ToList();
                    int rowsGrid = 0;
                    //Add New row for return total records
                    if (dataCollectControl.Count > 0)
                    {
                        rowsGrid = dataCollectControl.Count;
                    }

                    #endregion Filter

                    #region LoadDTO

                    foreach (ACCOUNTINGEN.CollectControlV collectControl in dataCollectControl)
                    {
                        SCRMOD.SearchCollectDTO searchCollectDTO = new SCRMOD.SearchCollectDTO();

                        searchCollectDTO.CollectCode = collectControl.CollectId;
                        searchCollectDTO.CollectStatus = collectControl.CollectStatus == 0 ? 0 : Convert.ToInt32(collectControl.CollectStatus);
                        searchCollectDTO.PaymentsTotal = collectControl.PaymentsTotal == 0 ? 0 : Convert.ToDouble(collectControl.PaymentsTotal);
                        searchCollectDTO.AccountName = collectControl.AccountName;
                        searchCollectDTO.RegisterDate = Convert.ToDateTime(collectControl.RegisterDate);
                        searchCollectDTO.AccountingDate = Convert.ToDateTime(collectControl.AccountingDate);
                        searchCollectDTO.PayerId = collectControl.IndividualId == 0 ? -1 : Convert.ToInt32(collectControl.IndividualId);
                        searchCollectDTO.Payer = collectControl.Payer;
                        searchCollectDTO.BranchCode = collectControl.BranchCode == 0 ? -1 : Convert.ToInt32(collectControl.BranchCode);
                        searchCollectDTO.BranchDescription = collectControl.BranchDescription;
                        searchCollectDTO.CollectControlCode = collectControl.CollectControlId;
                        searchCollectDTO.DocumentNumber = collectControl.DocumentNumber;
                        searchCollectDTO.TechnicalTransaction = collectControl.TechnicalTransaction == 0 ? 0 : Convert.ToInt32(collectControl.TechnicalTransaction);
                        searchCollectDTO.Rows = rowsGrid;
                        searchCollects.Add(searchCollectDTO);
                    }

                    #endregion LoadDTO
                }

                #endregion Collect

                #region JournalEntry

                if (imputationType == Convert.ToInt32(Application.AccountingServices.Enums.ApplicationTypes.JournalEntry))
                {

                    #region Filter

                    //Filtro
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.JournalEntry.Properties.BranchCode, branchId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, Convert.ToInt32(Application.AccountingServices.Enums.ApplicationTypes.JournalEntry));

                    if (userId >= 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AppJournalEntryV.Properties.UserId, userId);
                    }

                    if (collectId >= 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.TechnicalTransaction, collectId);
                    }

                    if (startDate != null && startDate != "")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Application.Properties.RegisterDate);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(FormatDateTime(startDate));
                    }
                    if (endDate != null && endDate != "")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Application.Properties.RegisterDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(FormatDateTime(endDate));
                    }

                    //Por estado del Asiento

                    if (journalEntryStatusId != -1)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.JournalEntry.Properties.Status, journalEntryStatusId);
                    }
                    List<ACCOUNTINGEN.AppJournalEntryV> dataJournalEntry = DataFacadeManager.Instance.GetDataFacade().List(
                        typeof(ACCOUNTINGEN.AppJournalEntryV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.AppJournalEntryV>().ToList();

                    int rowsGrid = 0;
                    //Add New row for return total records
                    if (dataJournalEntry.Count > 0)
                    {
                        rowsGrid = dataJournalEntry.Count;
                        //data.Columns.Add("rows", typeof(int));
                        //data.Rows[0]["rows"] = rowsGrid;
                    }

                    #endregion Filter

                    #region LoadDTO

                    foreach (ACCOUNTINGEN.AppJournalEntryV journalEntry in dataJournalEntry)
                    {
                        SCRMOD.SearchCollectDTO searchCollectDTO = new SCRMOD.SearchCollectDTO();
                        searchCollectDTO.ImputationId = journalEntry.ApplicationCode == 0 ? -1 : journalEntry.ApplicationCode;
                        searchCollectDTO.JournalEntryStatus = Convert.ToInt32(journalEntry.Status);  //  0=ESTADO REVERSADO 1=ESTADO APLICADO
                        searchCollectDTO.UserId = Convert.ToString(journalEntry.UserId == 0 ? -1 : (int)journalEntry.UserId);
                        searchCollectDTO.RegisterDate = journalEntry.RegisterDate == null ? DateTime.Now : journalEntry.RegisterDate;
                        searchCollectDTO.AccountingDate = journalEntry.AccountingDate == null ? DateTime.Now : journalEntry.AccountingDate;
                        searchCollectDTO.PayerId = journalEntry.IndividualId == 0 ? -1 : (int)journalEntry.IndividualId;
                        searchCollectDTO.Payer = journalEntry.Name;
                        searchCollectDTO.DocumentNumber = journalEntry.DocumentNumber;
                        searchCollectDTO.BranchCode = journalEntry.BranchCode == 0 ? -1 : (int)journalEntry.BranchCode;
                        searchCollectDTO.BranchDescription = journalEntry.Description;
                        searchCollectDTO.JournalEntryId = journalEntry.JournalEntryCode == 0 ? -1 : (int)journalEntry.JournalEntryCode;
                        searchCollectDTO.Comments = journalEntry.JurnalDescription;
                        searchCollectDTO.TechnicalTransaction = journalEntry.TechnicalTransaction == null ? 0 : (int)journalEntry.TechnicalTransaction;
                        //searchCollectDTO.AccountingTransaction = journalEntry.AccountingTransaction == null ? 0 : (int)journalEntry.AccountingTransaction;
                        searchCollectDTO.AccountName = journalEntry.AccountName;
                        searchCollectDTO.Rows = rowsGrid;
                        searchCollects.Add(searchCollectDTO);
                    }

                    #endregion LoadDTO
                }

                #endregion JournalEntry

                return searchCollects;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetReceiptApplicationInformation
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<SearchCollectDTO/></returns>
        public List<SCRMOD.SearchCollectDTO> GetReceiptApplicationInformation(int collectId)
        {
            try
            {
                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collectId);

                List<ACCOUNTINGEN.CollectConceptV> collectConceptVs = DataFacadeManager.Instance.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.CollectConceptV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.CollectConceptV>().ToList();


                int rowsGrid = 0;
                //Add New row for return total records
                if (collectConceptVs.Count > 0)
                {
                    rowsGrid = collectConceptVs.Count;
                }

                #region LoadDTO

                List<SCRMOD.SearchCollectDTO> searchCollects = new List<SCRMOD.SearchCollectDTO>();
                double postedAmount = 0;

                foreach (ACCOUNTINGEN.CollectConceptV collectConcept in collectConceptVs)
                {
                    if (collectConcept.PaymentMethodTypeCode != 0 && Convert.ToInt32(collectConcept.PaymentMethodTypeCode) == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)))
                    {
                        postedAmount += Convert.ToDouble(collectConcept.Amount);
                    }
                }

                foreach (ACCOUNTINGEN.CollectConceptV collectConceptV in collectConceptVs)
                {
                    SCRMOD.SearchCollectDTO searchCollectDTO = new SCRMOD.SearchCollectDTO();
                    searchCollectDTO.CollectCode = collectConceptV.CollectId == 0 ? -1 : (int)collectConceptV.CollectId;
                    searchCollectDTO.CollectConceptDescription = collectConceptV.CollectConceptDescription;
                    searchCollectDTO.CollectDescription = collectConceptV.CollectDescription;
                    searchCollectDTO.PaymentMethodTypeCode = collectConceptV.PaymentMethodTypeCode == 0 ? -1 : (int)collectConceptV.PaymentMethodTypeCode;
                    searchCollectDTO.PaymentsTotal = Convert.ToDouble(collectConceptV.Amount == 0 ? -1 : (decimal)collectConceptV.Amount);
                    searchCollectDTO.Comments = collectConceptV.Comments;
                    searchCollectDTO.CollectControlCode = collectConceptV.CollectControlCode == 0 ? -1 : (int)collectConceptV.CollectControlCode;
                    searchCollectDTO.PostdatedValue = postedAmount;
                    searchCollectDTO.PayerId = collectConceptV.IndividualId == 0 ? -1 : (int)collectConceptV.IndividualId;
                    searchCollectDTO.Payer = collectConceptV.Payer;
                    searchCollectDTO.PayerDocumentNumber = collectConceptV.PayerDocumentNumber;
                    searchCollectDTO.AccountingDateImputation = Convert.ToString(collectConceptV.AccountingDate);
                    searchCollectDTO.PaymentsTotalImputation = Convert.ToDouble(collectConceptV.PaymentsTotal == 0 ? -1 : (decimal)collectConceptV.PaymentsTotal);
                    searchCollectDTO.BranchCode = collectConceptV.BranchCode == 0 ? -1 : (int)collectConceptV.BranchCode;
                    searchCollectDTO.Rows = rowsGrid;
                    searchCollects.Add(searchCollectDTO);
                }

                #endregion LoadDTO

                return searchCollects;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<SCRMOD.SearchCollectDTO> GetReceiptApplicationTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, technicalTransaction);

                List<ACCOUNTINGEN.CollectConceptV> collectConceptVs = DataFacadeManager.Instance.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.CollectConceptV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.CollectConceptV>().ToList();


                int rowsGrid = 0;
                //Add New row for return total records
                if (collectConceptVs.Count > 0)
                {
                    rowsGrid = collectConceptVs.Count;
                }

                #region LoadDTO

                List<SCRMOD.SearchCollectDTO> searchCollects = new List<SCRMOD.SearchCollectDTO>();
                double postedAmount = 0;

                foreach (ACCOUNTINGEN.CollectConceptV collectConcept in collectConceptVs)
                {
                    if (collectConcept.PaymentMethodTypeCode != 0 && Convert.ToInt32(collectConcept.PaymentMethodTypeCode) == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)))
                    {
                        postedAmount += Convert.ToDouble(collectConcept.Amount);
                    }
                }

                foreach (ACCOUNTINGEN.CollectConceptV collectConceptV in collectConceptVs)
                {
                    SCRMOD.SearchCollectDTO searchCollectDTO = new SCRMOD.SearchCollectDTO();
                    searchCollectDTO.CollectCode = collectConceptV.CollectId == 0 ? -1 : (int)collectConceptV.CollectId;
                    searchCollectDTO.CollectConceptDescription = collectConceptV.CollectConceptDescription;
                    searchCollectDTO.CollectDescription = collectConceptV.CollectDescription;
                    searchCollectDTO.PaymentMethodTypeCode = collectConceptV.PaymentMethodTypeCode == 0 ? -1 : (int)collectConceptV.PaymentMethodTypeCode;
                    searchCollectDTO.PaymentsTotal = Convert.ToDouble(collectConceptV.Amount == 0 ? -1 : (decimal)collectConceptV.Amount);
                    searchCollectDTO.Comments = collectConceptV.Comments;
                    searchCollectDTO.CollectControlCode = collectConceptV.CollectControlCode == 0 ? -1 : (int)collectConceptV.CollectControlCode;
                    searchCollectDTO.PostdatedValue = postedAmount;
                    searchCollectDTO.PayerId = collectConceptV.IndividualId == 0 ? -1 : (int)collectConceptV.IndividualId;
                    searchCollectDTO.Payer = collectConceptV.Payer;
                    searchCollectDTO.PayerDocumentNumber = collectConceptV.PayerDocumentNumber;
                    searchCollectDTO.AccountingDateImputation = Convert.ToString(collectConceptV.AccountingDate);
                    searchCollectDTO.PaymentsTotalImputation = Convert.ToDouble(collectConceptV.PaymentsTotal == 0 ? -1 : (decimal)collectConceptV.PaymentsTotal);
                    searchCollectDTO.BranchCode = collectConceptV.BranchCode == 0 ? -1 : (int)collectConceptV.BranchCode;
                    searchCollectDTO.TechnicalTransaction = collectConceptV.TechnicalTransaction == 0 ? -1 : (int)collectConceptV.TechnicalTransaction;
                    searchCollectDTO.BranchDescription = collectConceptV.BranchDescription;
                    searchCollectDTO.Rows = rowsGrid;
                    searchCollects.Add(searchCollectDTO);
                }

                #endregion LoadDTO

                return searchCollects;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectIdByJournalEntryId
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>int</returns>
        public int GetCollectIdByJournalEntryId(int journalEntryId)
        {
            try
            {
                return _collectDAO.GetCollectByBookEntry(journalEntryId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion SearchCollects

        #region CollectItem

        /// <summary>
        /// GetPoliciesByCollectItemId
        /// </summary>
        /// <param name="collectItemId"></param>
        /// <returns>List<CollectItemPolicyDTO/></returns>
        public List<SCRMOD.CollectItemPolicyDTO> GetPoliciesByCollectItemId(int collectItemId)
        {
            List<SCRMOD.CollectItemPolicyDTO> collectItemPolicyDTO = new List<SCRMOD.CollectItemPolicyDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                /*  criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PremiumReceivableTrans.Properties.PremiumReceivableTransId, collectItemId);

                  UIView policiesByCollectItemView = DataFacadeManager.Instance.GetDataFacade().GetView("PremiumReceivableTransCollectView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out int rowsGrid);

                  foreach (DataRow dataRow in policiesByCollectItemView)
                  {
                      collectItemPolicyDTO.Add(new SCRMOD.CollectItemPolicyDTO()
                      {
                          BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                          PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                          PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                          Endorsement = Convert.ToInt32(dataRow["Endorsement"]),
                          QuoteNum = Convert.ToInt32(dataRow["QuoteNum"]),
                          PayerId = Convert.ToInt32(dataRow["PayerId"]),
                          Amount = Convert.ToDecimal(dataRow["Amount"]),
                          CollectItemCode = Convert.ToInt32(dataRow["PremiumReceivableCode"])
                      });
                  }*/

                return collectItemPolicyDTO;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion CollectItem

        #region CollectItemsWithoutPaymentTicket

        /// <summary>
        /// GetCollectItemsWithoutPaymentTicket
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>List<CollectItemWithoutPaymentTicketDTO/></returns>
        public List<SCRMOD.CollectItemWithoutPaymentTicketDTO> GetCollectItemsWithoutPaymentTicket(int collectControlId)
        {
            List<SCRMOD.CollectItemWithoutPaymentTicketDTO> collectItemWithoutPaymentTicketDTOs = new List<SCRMOD.CollectItemWithoutPaymentTicketDTO>();
            SCRMOD.CollectItemWithoutPaymentTicketDTO collectItemWithoutPaymentTicketDTO;
            string startDate = "";
            int iCurrency = -1;
            decimal cashJustinCollects = 0;

            try
            {
                int branchId = 0;
                int userId = 0;

                ObjectCriteriaBuilder collectControlFilter = new ObjectCriteriaBuilder();
                collectControlFilter.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.CollectControlId, collectControlId);

                BusinessCollection collectControlCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), collectControlFilter.GetPredicate()));
                if (collectControlCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.CollectControl collectControlEntity in collectControlCollection.OfType<ACCOUNTINGEN.CollectControl>())
                    {
                        branchId = Convert.ToInt32(collectControlEntity.BranchCode);
                        userId = Convert.ToInt32(collectControlEntity.UserId);
                    }
                }

                #region AccountingDate

                DateTime registerDate = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));

                string endDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy");
                endDate = endDate + " 23:59:59";

                // Obtener fecha apertura caja
                ObjectCriteriaBuilder filterCash = new ObjectCriteriaBuilder();
                filterCash.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.Status, 1).And();
                filterCash.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, userId).And();
                filterCash.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.BranchCode, branchId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(
                                                        typeof(ACCOUNTINGEN.CollectControl), filterCash.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    startDate = businessCollection.OfType<ACCOUNTINGEN.CollectControl>().First().OpenDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }

                #endregion

                #region Monto recibido en caja y no cerrado

                filterCash = new ObjectCriteriaBuilder();

                filterCash.PropertyEquals(ACCOUNTINGEN.CashPaymentCollect.Properties.UserId, userId).And();
                filterCash.PropertyEquals(ACCOUNTINGEN.CashPaymentCollect.Properties.BranchCode, branchId).And();

                filterCash.Property(ACCOUNTINGEN.CashPaymentCollect.Properties.RegisterDate);
                filterCash.GreaterEqual();
                filterCash.Constant(FormatDateTime(startDate));
                filterCash.And();
                filterCash.Property(ACCOUNTINGEN.CashPaymentCollect.Properties.RegisterDate);
                filterCash.LessEqual();
                filterCash.Constant(FormatDateTime(endDate));

                UIView cashDeposited = DataFacadeManager.Instance.GetDataFacade().GetView("CashPaymentCollectView",
                                       filterCash.GetPredicate(), null, 0, -1, null, false, out int rows);

                Dictionary<int, decimal> cashAmountInCollect = new Dictionary<int, decimal>();

                foreach (DataRow row in cashDeposited.Rows)
                {
                    if (cashAmountInCollect.ContainsKey(Convert.ToInt32(row["CurrencyCode"])))
                    {
                        cashAmountInCollect[Convert.ToInt32(row["CurrencyCode"])] += Convert.ToDecimal(row["IncomeAmount"]);
                    }
                    else
                    {
                        cashAmountInCollect.Add(Convert.ToInt32(row["CurrencyCode"]), Convert.ToDecimal(row["IncomeAmount"]));
                    }
                }

                #endregion

                #region Monto en boleta interna
                //Monto en boleta interna
                filterCash = new ObjectCriteriaBuilder();

                filterCash.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.Status, 1).And();
                filterCash.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.UserId, userId).And();
                filterCash.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId).And();
                filterCash.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                filterCash.GreaterEqual();
                filterCash.Constant(FormatDateTime(startDate));
                filterCash.And();
                filterCash.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                filterCash.LessEqual();
                filterCash.Constant(FormatDateTime(endDate));

                BusinessCollection cashInternalBallot = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().
                    SelectObjects(typeof(ACCOUNTINGEN.PaymentTicket), filterCash.GetPredicate()));


                Dictionary<int, decimal> cashAmountTicket = new Dictionary<int, decimal>();

                foreach (ACCOUNTINGEN.PaymentTicket paymentTicket in cashInternalBallot.OfType<ACCOUNTINGEN.PaymentTicket>())
                {
                    if (cashAmountTicket.ContainsKey(Convert.ToInt32(paymentTicket.CurrencyCode)))
                    {
                        cashAmountTicket[Convert.ToInt32(paymentTicket.CurrencyCode)] += Convert.ToDecimal(paymentTicket.CashAmount);
                    }
                    else
                    {
                        cashAmountTicket.Add(Convert.ToInt32(paymentTicket.CurrencyCode), Convert.ToDecimal(paymentTicket.CashAmount));
                    }
                }
                #endregion

                #region Efectivo boleta de depósito

                filterCash = new ObjectCriteriaBuilder();

                filterCash.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.Status, 1);
                filterCash.And();
                filterCash.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.UserId, userId);
                filterCash.And();
                filterCash.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                filterCash.And();
                filterCash.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                filterCash.GreaterEqual();
                filterCash.Constant(FormatDateTime(startDate));
                filterCash.And();
                filterCash.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                filterCash.LessEqual();
                filterCash.Constant(FormatDateTime(endDate));

                UIView cashBallotDeposited = DataFacadeManager.Instance.GetDataFacade().GetView("PaymentBallotView",
                                             filterCash.GetPredicate(), null, 0, -1, null, false, out rows);

                Dictionary<int, decimal> cashAmountBallot = new Dictionary<int, decimal>();

                foreach (DataRow row in cashBallotDeposited.Rows)
                {
                    if (cashAmountBallot.ContainsKey(Convert.ToInt32(row["CurrencyCode"])))
                    {
                        cashAmountBallot[Convert.ToInt32(row["CurrencyCode"])] += Convert.ToDecimal(row["CashAmount"]);
                    }
                    else
                    {
                        cashAmountBallot.Add(Convert.ToInt32(row["CurrencyCode"]), Convert.ToDecimal(row["CashAmount"]));
                    }
                }
                #endregion

                List<int> cahsCurrencies = new List<int>();

                List<SCRMOD.BalanceInquieriesDTO> balanceInquieriesDTOs = GetBalanceInquiriesData(branchId, userId, startDate);
                foreach (SCRMOD.BalanceInquieriesDTO rowli in balanceInquieriesDTOs)
                {
                    if (rowli.PaymentDescription == "EFECTIVO")
                    {
                        if (cahsCurrencies.Contains(rowli.CurrencyCode))
                            continue;

                        iCurrency = rowli.CurrencyCode;
                        cahsCurrencies.Add(iCurrency);

                        decimal cashReleated = (cashAmountInCollect.ContainsKey(iCurrency) ? Convert.ToDecimal(cashAmountInCollect[iCurrency]) : 0)
                                         - (cashAmountBallot.ContainsKey(iCurrency) ? Convert.ToDecimal(cashAmountBallot[iCurrency]) : 0);

                        if (cashReleated != 0)
                        {
                            decimal cashNotDeposited = (cashAmountTicket.ContainsKey(iCurrency) ? Convert.ToDecimal(cashAmountTicket[iCurrency]) : 0);
                            cashJustinCollects = cashReleated - cashNotDeposited;

                            if (cashJustinCollects > 0)
                            {
                                collectItemWithoutPaymentTicketDTO = new SCRMOD.CollectItemWithoutPaymentTicketDTO();
                                collectItemWithoutPaymentTicketDTO.User = rowli.UserDescription;
                                collectItemWithoutPaymentTicketDTO.AccountingDate = Convert.ToDateTime(rowli.AccountingDate);
                                collectItemWithoutPaymentTicketDTO.OpenDate = Convert.ToDateTime(rowli.OpenDate);
                                collectItemWithoutPaymentTicketDTO.BranchId = rowli.BranchCode;
                                collectItemWithoutPaymentTicketDTO.BranchDescription = rowli.BranchDescription;
                                collectItemWithoutPaymentTicketDTO.TechnicalTransaction = 0;
                                collectItemWithoutPaymentTicketDTO.PaymentMethod = rowli.PaymentDescription;
                                collectItemWithoutPaymentTicketDTO.CurrencyDescription = rowli.CurrencyDescription;
                                collectItemWithoutPaymentTicketDTO.Status = rowli.StatusDescription;
                                collectItemWithoutPaymentTicketDTO.Amount = cashJustinCollects;
                                collectItemWithoutPaymentTicketDTOs.Add(collectItemWithoutPaymentTicketDTO);
                            }

                            // Cash associated in tickets
                            if (cashNotDeposited > 0)
                            {
                                collectItemWithoutPaymentTicketDTO = new SCRMOD.CollectItemWithoutPaymentTicketDTO();
                                collectItemWithoutPaymentTicketDTO.User = rowli.UserDescription;
                                collectItemWithoutPaymentTicketDTO.AccountingDate = Convert.ToDateTime(rowli.AccountingDate);
                                collectItemWithoutPaymentTicketDTO.OpenDate = Convert.ToDateTime(rowli.OpenDate);
                                collectItemWithoutPaymentTicketDTO.BranchId = rowli.BranchCode;
                                collectItemWithoutPaymentTicketDTO.BranchDescription = rowli.BranchDescription;
                                collectItemWithoutPaymentTicketDTO.TechnicalTransaction = 0;
                                collectItemWithoutPaymentTicketDTO.PaymentMethod = rowli.PaymentDescription;
                                collectItemWithoutPaymentTicketDTO.CurrencyDescription = rowli.CurrencyDescription;
                                collectItemWithoutPaymentTicketDTO.Status = Resources.Resources.PaymentTicket.ToUpperInvariant();
                                collectItemWithoutPaymentTicketDTO.Amount = cashNotDeposited;
                                collectItemWithoutPaymentTicketDTOs.Add(collectItemWithoutPaymentTicketDTO);
                            }
                        }
                    }
                    else
                    {
                        collectItemWithoutPaymentTicketDTO = new SCRMOD.CollectItemWithoutPaymentTicketDTO();
                        collectItemWithoutPaymentTicketDTO.User = rowli.UserDescription;
                        collectItemWithoutPaymentTicketDTO.AccountingDate = Convert.ToDateTime(rowli.AccountingDate);
                        collectItemWithoutPaymentTicketDTO.OpenDate = Convert.ToDateTime(rowli.OpenDate);
                        collectItemWithoutPaymentTicketDTO.BranchId = rowli.BranchCode;
                        collectItemWithoutPaymentTicketDTO.BranchDescription = rowli.BranchDescription;
                        collectItemWithoutPaymentTicketDTO.TechnicalTransaction = rowli.TechnicalTransaction;
                        collectItemWithoutPaymentTicketDTO.PaymentMethod = rowli.PaymentDescription;
                        collectItemWithoutPaymentTicketDTO.CurrencyDescription = rowli.CurrencyDescription;
                        collectItemWithoutPaymentTicketDTO.Status = rowli.StatusDescription;
                        collectItemWithoutPaymentTicketDTO.Amount = rowli.IncomeAmount;
                        collectItemWithoutPaymentTicketDTOs.Add(collectItemWithoutPaymentTicketDTO);
                    }
                }
                return collectItemWithoutPaymentTicketDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion CollectItemsWithoutPaymentTicket

        #region BalanceInquiries

        /// <summary>
        /// GetBalanceInquiriesUIview
        /// Obtiene la data para la consulta de saldos 
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns>UIView</returns>
        private List<SCRMOD.BalanceInquieriesDTO> GetBalanceInquiriesData(int branch, int userId, string date)
        {
            try
            {
                int pageIndex = 0;

                DateTime registerDate = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));
                string strEndDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy");
                strEndDate = strEndDate + " 23:59:59";

                #region Filtro
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                IFormatProvider culture = new CultureInfo("es-EC", true);
                DateTime startDate = Convert.ToDateTime(date, culture);
                DateTime endDate = Convert.ToDateTime(strEndDate, culture);

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                //Filtro
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.UserId, "bc");
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode, "bc");
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(branch);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.OpenDate, "bc");
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(startDate);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.Status, "bc");
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(Convert.ToInt32(CollectControlStatus.Open));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.Status, "b");
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(Convert.ToInt32(CollectStatus.Canceled));
                //criteriaBuilder.And();
                //criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.RegisterDate, "b");
                //criteriaBuilder.LessEqual();
                //criteriaBuilder.Constant(endDate);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.Status, "pay");
                criteriaBuilder.In();
                criteriaBuilder.ListValue();
                criteriaBuilder.Constant(1);
                criteriaBuilder.Constant(2);
                criteriaBuilder.EndList();

                #endregion end Filtro
                #region selectQuery
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectControl.Properties.UserId, "bc"), "USER_ID"));

                select.AddSelectValue(new SelectValue(new Column(UUEN.UniqueUsers.Properties.AccountName, "uu"), "USER_DESCRIPTION"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectControl.Properties.AccountingDate, "bc"), "ACCOUNTING_DATE"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Collect.Properties.RegisterDate, "b"), "REGISTER_DATE"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectControl.Properties.OpenDate, "bc"), "OPEN_DATE"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectControl.Properties.BranchCode, "bc"), "BRANCH_ID"));
                select.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, "br"), "BRANCH_DESCRIPTION"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, "b"), "TECHNICAL_TRANSACTION"));

                select.AddSelectValue(new SelectValue(new Column(COMMEN.CreditCardType.Properties.Description, "cc"), "CREDIT_CARD_TYPE_DESCRIPTION"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentType.Properties.Description, "pmt"), "PAYMENT_METHOD_TYPE_DESCRIPTION"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, "pay"), "PAYMENT_METHOD_TYPE_CD"));

                select.AddSelectValue(new SelectValue(new Column(COMMEN.Currency.Properties.CurrencyCode, "c"), "CURRENCY_CD"));
                select.AddSelectValue(new SelectValue(new Column(COMMEN.Currency.Properties.Description, "c"), "CURRENCY_DESC"));

                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.StatusPayment.Properties.Description, "s"), "STATUS_DESCRIPTION"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Status, "pay"), "STATUS"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.IncomeAmount, "pay"), "INCOME_AMOUNT"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Amount, "pay"), "AMOUNT"));
                select.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectControl.Properties.Status, "bc"), "COLLECT_CONTROL_STATUS"));
                #endregion finish selectQuery
                #region join

                Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.Payment), "pay"), new ClassNameTable(typeof(ACCOUNTINGEN.Collect), "b"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCOUNTINGEN.Payment.Properties.CollectCode, "pay")
                    .Equal()
                    .Property(ACCOUNTINGEN.Collect.Properties.CollectId, "b")
                    .GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.CollectControl), "bc"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCOUNTINGEN.CollectControl.Properties.CollectControlId, "bc")
                    .Equal()
                    .Property(ACCOUNTINGEN.Collect.Properties.CollectControlCode, "b")
                    .GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.PaymentType), "pmt"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCOUNTINGEN.PaymentType.Properties.PaymentTypeCode, "pmt")
                    .Equal()
                    .Property(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, "pay")
                    .GetPredicate());
                join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), "br"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMEN.Branch.Properties.BranchCode, "br")
                    .Equal()
                    .Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode, "bc")
                    .GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(COMMEN.Currency), "c"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMEN.Currency.Properties.CurrencyCode, "c")
                    .Equal()
                    .Property(ACCOUNTINGEN.Payment.Properties.CurrencyCode, "pay")
                    .GetPredicate());
                join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.StatusPayment), "s"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCOUNTINGEN.StatusPayment.Properties.PaymentMethodTypeCode, "s")
                    .Equal()
                    .Property(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, "pay")
                    .And()
                    .Property(ACCOUNTINGEN.StatusPayment.Properties.Status, "s")
                    .Equal()
                    .Property(ACCOUNTINGEN.Payment.Properties.Status, "pay")
                    .GetPredicate());
                join = new Join(join, new ClassNameTable(typeof(UUEN.UniqueUsers), "uu"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(UUEN.UniqueUsers.Properties.UserId, "uu")
                    .Equal()
                    .Property(ACCOUNTINGEN.CollectControl.Properties.UserId, "bc")
                    .GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(COMMEN.CreditCardType), "cc"), JoinType.Left);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMEN.CreditCardType.Properties.CreditCardTypeCode, "cc")
                    .Equal()
                    .Property(ACCOUNTINGEN.Payment.Properties.CreditCardTypeCode, "pay")
                    .GetPredicate());
                #endregion finish join

                select.Table = join;
                select.Where = criteriaBuilder.GetPredicate();

                string desc = "";
                decimal incomeAmount = 0;
                List<SCRMOD.BalanceInquieriesDTO> balanceInquiries = new List<SCRMOD.BalanceInquieriesDTO>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        if (reader["CREDIT_CARD_TYPE_DESCRIPTION"] != null)
                            desc = reader["PAYMENT_METHOD_TYPE_DESCRIPTION"] + " - " + reader["CREDIT_CARD_TYPE_DESCRIPTION"].ToString();
                        else
                            desc = reader["PAYMENT_METHOD_TYPE_DESCRIPTION"].ToString();

                        if (reader["INCOME_AMOUNT"] != null)
                            incomeAmount = Convert.ToDecimal(reader["INCOME_AMOUNT"]);


                        balanceInquiries.Add(new SCRMOD.BalanceInquieriesDTO()
                        {
                            PaymentMethodTypeCode = Convert.ToInt32(reader["PAYMENT_METHOD_TYPE_CD"]),
                            PaymentDescription = desc,
                            CurrencyCode = Convert.ToInt32(reader["CURRENCY_CD"]),
                            CurrencyDescription = reader["CURRENCY_DESC"].ToString(),
                            IncomeAmount = incomeAmount,
                            Amount = Convert.ToDecimal(reader["AMOUNT"]),
                            BranchCode = Convert.ToInt32(reader["BRANCH_ID"]),
                            BranchDescription = reader["BRANCH_DESCRIPTION"].ToString(),
                            UserId = Convert.ToInt32(reader["USER_ID"]),
                            TechnicalTransaction = Convert.ToInt32(reader["TECHNICAL_TRANSACTION"]),
                            UserDescription = reader["USER_DESCRIPTION"].ToString(),
                            AccountingDate = String.Format("{0:dd/MM/yyyy}", reader["ACCOUNTING_DATE"]),
                            OpenDate = String.Format("{0:dd/MM/yyyy}", reader["OPEN_DATE"]),
                            StatusDescription = reader["STATUS_DESCRIPTION"].ToString(),

                            RegisterDate = String.Format("{0:MM/dd/yyyy}", reader["REGISTER_DATE"]),
                        });

                    }
                }

                return balanceInquiries;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBalanceInquiries
        /// Obtiene todas las boletas para ser depositadas menos las ya depositadas
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns>List<BalanceInquieriesDTO/></returns>
        public List<SCRMOD.BalanceInquieriesDTO> GetBalanceInquiries(int branch, int userId, string date)
        {

            return GetBalanceInquiriesData(branch, userId, date);
        }

        /// <summary>
        /// GetUserInquiries
        /// Obtiene los usuarios en relación a la sucursal para la consulta de saldos
        /// </summary>
        /// <param name="branch"></param>
        /// <returns>List<User/></returns>
        public List<int> GetUserInquiries(int branch)
        {
            List<int> users = new List<int>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(branch);

            UIView data = DataFacadeManager.Instance.GetDataFacade().GetView("CollectControlUniqueUsersView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rowsGrid);

            foreach (DataRow row in data.Rows)
            {
                users.Add(Convert.ToInt32(row["UserId"]));
            }

            return users;
        }

        #endregion BalanceInquiries

        #region IncomeChangeConcept

        /// <summary>
        /// GetReceiptForExchangeConcept
        /// Obtiene los recibos no anulados y que el mes de la fecha contable sea igual al mes de la fecha contable del módulo
        /// para cambiar el concepto de ingreso 
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>IncomeChangeConceptDTO</returns>
        public SCRMOD.IncomeChangeConceptDTO GetReceiptForExchangeConcept(int collectId, DateTime accountingDate)
        {
            try
            {
                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collectId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.Status, 1);

                UIView receiptNumber = DataFacadeManager.Instance.GetDataFacade().GetView("CollectConceptControlView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rowsGrid);

                SCRMOD.IncomeChangeConceptDTO incomeChangeConcept = new SCRMOD.IncomeChangeConceptDTO();

                int monthModule = accountingDate.Month;
                int monthReceipt = 0;

                foreach (DataRow row in receiptNumber.Rows)
                {
                    incomeChangeConcept.AccountingDate = Convert.ToDateTime(row["AccountingDate"]);
                    incomeChangeConcept.CollectCode = Convert.ToInt32(row["CollectId"]);
                    incomeChangeConcept.CollectControlCode = Convert.ToInt32(row["CollectControlCode"]);
                    incomeChangeConcept.CollectControlStatus = Convert.ToInt32(row["CollectControlStatus"]);
                    incomeChangeConcept.CollectConceptCode = Convert.ToInt32(row["CollectConceptCode"]);
                    incomeChangeConcept.CollectConceptName = row["CollectConceptName"].ToString();
                    incomeChangeConcept.CollectStatus = Convert.ToInt32(row["Status"]);
                    incomeChangeConcept.RegisterDate = row["RegisterDate"].ToString();
                    monthReceipt = incomeChangeConcept.AccountingDate.Month;
                }

                if (monthModule != monthReceipt)
                {
                    incomeChangeConcept.CollectControlCode = -1;
                }

                return incomeChangeConcept;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateIncomeChangeConcept
        /// Actualiza el concepto de ingreso de un recibo
        /// Autor: LFR
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="collectControlId"></param>
        /// <param name="collectConceptId"></param>
        /// <returns>Collect</returns>
        public CollectDTO UpdateIncomeChangeConcept(int collectId, int collectControlId, int collectConceptId)
        {
            try
            {
                Collect collect = new Collect();
                collect.Id = collectId;
                collect = _collectDAO.GetCollect(collect);
                collect.Concept = new CollectConcept();
                collect.Concept.Id = collectConceptId;
                collect.Transaction = new Models.Collect.Transaction();

                return _collectDAO.UpdateCollect(collect, collectControlId).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(ConfigurationManager.AppSettings["UnhandledExceptionMsj"]);
            }
        }

        #endregion IncomeChangeConcept

        #region DailyClosingCash

        /// <summary>
        /// GetBranchesOpenStatus
        /// Obtiene las sucursales que mantienen un registro de control de caja en estado “Abierta” 
        /// </summary>
        /// <returns>List<Branch/></returns>
        public List<BranchDTO> GetBranchesOpenStatus()
        {
            try
            {
                List<Branch> branches = new List<Branch>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.Status);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(1);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), criteriaBuilder.GetPredicate()));

                int? branchId = 0;

                foreach (ACCOUNTINGEN.CollectControl collectControl in businessCollection.OfType<ACCOUNTINGEN.CollectControl>())
                {
                    Branch branch = new Branch();

                    if (branchId != collectControl.BranchCode)
                    {
                        branch.Id = Convert.ToInt32(collectControl.BranchCode);
                        branches.Add(branch);
                    }

                    branchId = collectControl.BranchCode;
                }

                return branches.ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateCheckCardDeposited
        /// Valida que todos los cheques y tarjetas ingresadas por caja (de la sucursal, usuario
        /// y hasta la fecha de proceso) estén con estado 
        /// “asignado a boleta” (es decir depositadas)
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <returns>decimal</returns>
        public decimal ValidateCheckCardDeposited(int branchId, int userId)
        {
            decimal depositedValue = 0;
            try
            {
                string startDate = "";

                DateTime registerDate = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));
                string endDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy") + " 23:59:59";

                // Obtener fecha apertura de caja
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.Status, 1).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.BranchCode, branchId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    startDate = businessCollection.OfType<ACCOUNTINGEN.CollectControl>().First().OpenDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
                }

                // Si el mes contable no esta cerrado las fechas se invierten
                if (Convert.ToDateTime(registerDate) < Convert.ToDateTime(startDate))
                {
                    endDate = Convert.ToDateTime(startDate).ToString("dd/MM/yyyy") + " 23:59:59";
                    startDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy");
                }

                #region UIView

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentCollectControl.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentCollectControl.Properties.BranchCode, branchId).And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentCollectControl.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentCollectControl.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                UIView checkCollection = DataFacadeManager.Instance.GetDataFacade().GetView("PaymentCollectControlView", criteriaBuilder.GetPredicate(), null, 0, 1000, null, true, out int rowsGrid);

                int checkCounter = checkCollection.Rows.Count;
                decimal cardAmountDeposited = 0;

                foreach (DataRow row in checkCollection.Rows)
                {
                    cardAmountDeposited += Convert.ToDecimal(row["Amount"]);
                }

                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentCollectControl.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentCollectControl.Properties.BranchCode, branchId).And();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentCollectControl.Properties.PaymentStatus);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(2);
                criteriaBuilder.Or();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentCollectControl.Properties.PaymentStatus);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(3);
                criteriaBuilder.CloseParenthesis();
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentCollectControl.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentCollectControl.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                UIView cardCollection = DataFacadeManager.Instance.GetDataFacade().GetView("PaymentCollectControlView", criteriaBuilder.GetPredicate(), null, 0, 100, null, true, out rowsGrid);

                int cardCounter = cardCollection.Rows.Count;
                decimal cardBallotDepositedAmount = 0;

                foreach (DataRow row in cardCollection.Rows)
                {
                    cardBallotDepositedAmount += Convert.ToDecimal(row["Amount"]);
                }

                #endregion UIView

                if (cardCounter == checkCounter)
                {
                    depositedValue = 0;
                }
                else
                {
                    depositedValue = (cardAmountDeposited - cardBallotDepositedAmount);
                    if (depositedValue < 0)
                    {
                        depositedValue = depositedValue * (-1);
                    }
                }
            }
            catch (BusinessException)
            {
                depositedValue = -1;
            }

            return depositedValue;
        }

        /// <summary>
        /// ValidateCashDeposited
        /// Permite validar que el total del monto efectivo registrado en el sistema se encuentre asignado a una boleta interna, para esto restar el 
        /// total del efectivo registrado menos el efectivo asignado en boletas internas no anuladas que sean del mismo usuario, sucursal y fecha menor
        /// o igual a la del proceso
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        public decimal ValidateCashDeposited(int branchId, int userId, int currencyId)
        {
            try
            {
                decimal depositedCash = 0;
                string startDate = "";
                DateTime registerDate = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));
                string endDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy") + " 23:59:59";

                // Obtener fecha apertura de caja
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.Status, 1).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.BranchCode, branchId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CollectControl), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    startDate = businessCollection.OfType<ACCOUNTINGEN.CollectControl>().First().OpenDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
                }

                // Si el mes contable no esta cerrado las fechas se invierten
                if (Convert.ToDateTime(registerDate) < Convert.ToDateTime(startDate))
                {
                    endDate = Convert.ToDateTime(startDate).ToString("dd/MM/yyyy") + " 23:59:59";
                    startDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy");
                }

                #region UIView

                // Monto recibido en caja y no cerrado
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CashPaymentCollect.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CashPaymentCollect.Properties.BranchCode, branchId).And();
                criteriaBuilder.Property(ACCOUNTINGEN.CashPaymentCollect.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CashPaymentCollect.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                UIView cashDeposited = DataFacadeManager.Instance.GetDataFacade().GetView("CashPaymentCollectView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rowsGrid);

                decimal cashAmount = 0;

                foreach (DataRow row in cashDeposited.Rows)
                {
                    cashAmount += Convert.ToDecimal(row["IncomeAmount"]) *
                        Convert.ToDecimal(DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(FormatDateTime(startDate), Convert.ToInt32(row["CurrencyCode"])).SellAmount);
                }

                // Monto en boleta interna
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.Status, 1).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId).And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate, "PaymentTicket");
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate, "PaymentTicket");
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                BusinessCollection cashInternalBallot = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentTicket), criteriaBuilder.GetPredicate()));

                decimal cashAmountTickets = 0;

                foreach (ACCOUNTINGEN.PaymentTicket paymentTicket in cashInternalBallot.OfType<ACCOUNTINGEN.PaymentTicket>())
                {
                    cashAmountTickets += Convert.ToDecimal(paymentTicket.CashAmount) * Convert.ToDecimal(DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(FormatDateTime(startDate), Convert.ToInt32(paymentTicket.CurrencyCode)).SellAmount);
                }


                // Filtro efectivo boleta de depósito
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.Status, 1);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.UserId, userId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate, "PaymentTicket");
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate, "PaymentTicket");
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                UIView cashBallotDeposited = DataFacadeManager.Instance.GetDataFacade().GetView("PaymentBallotView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out rowsGrid);

                decimal cashAmountBallots = 0;

                foreach (DataRow row in cashBallotDeposited.Rows)
                {
                    cashAmountBallots += Convert.ToDecimal(row["CashAmount"]) * Convert.ToDecimal(DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(FormatDateTime(startDate), Convert.ToInt32(row["CurrencyCode"])).SellAmount);
                }

                #endregion UIView
                return cashAmount - cashAmountTickets - cashAmountBallots;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRegisterDateCollectControl
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>string</returns>
        public string GetRegisterDateCollectControl(int collectControlId)
        {
            try
            {
                string registerDate = "";

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.CollectControlId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(collectControlId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.Status);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(1);

                UIView collectCollection = DataFacadeManager.Instance.GetDataFacade().GetView("CollectControlView", criteriaBuilder.GetPredicate(), null, 0, 10, null, true, out int rowsGrid);

                foreach (DataRow row in collectCollection.Rows)
                {
                    registerDate = row["RegisterDate"].ToString();
                }

                return registerDate;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCashierByBranchId
        /// Obtiene los usuarios en relación a la sucursal para el cierre de caja diario
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>List<User/></returns>
        public List<int> GetCashierByBranchId(int branchId)
        {
            List<int> users = new List<int>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(branchId);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.Status);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(1);

            UIView closeBoxUsers = DataFacadeManager.Instance.GetDataFacade().GetView("UniqueUsersCloseBoxView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rowsGrid);

            foreach (DataRow row in closeBoxUsers.Rows)
            {
                users.Add(Convert.ToInt32(row["UserId"]));
            }

            return users;
        }

        #endregion DailyClosingCash

        #region Collect

        /// <summary>
        /// ReplicateCheckinCollect
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="collectDescription"></param>
        /// <param name="individualId"></param>
        /// <param name="isChangeCheck"></param>
        /// <param name="payment"></param>
        /// <returns>Collect</returns>
        public Collect ReplicateCheckinCollect(int collectId, string collectDescription, int individualId, int isChangeCheck, Models.Payments.Payment payment, int paymentStatus, int branchId)
        {
            try
            {
                int paymentType = 0;
                Collect collect = new Collect();
                Collect newCollect = new Collect();
                CollectControl collectControl = new CollectControl();

                AccountingPaymentServiceEEProvider paymentService = new AccountingPaymentServiceEEProvider();
                ArrayList collectPayment = paymentService.GetPaymentByBillId(collectId);

                Payment oldpayment = Assemblers.ModelAssembler.CreateCheckPayment(_paymentDAO.GetPayment(payment));
                oldpayment.Status = (int)PaymentStatus.Canceled;
                decimal val = oldpayment.Amount.Value;
                oldpayment.Amount.Value = val * -1;
                oldpayment.Id = 0;

                if (collectPayment.Count > 0)
                {
                    ACCOUNTINGEN.Payment paymentEntity = collectPayment.OfType<ACCOUNTINGEN.Payment>().First();

                    // Tarjetas
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.Voucher;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD));
                    }
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.Voucher;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD));
                    }
                    // Checks
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.DocumentNumber;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK));

                    }
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.DocumentNumber;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK));
                    }
                }

                #region Collect

                CollectBusiness collectBusiness = new CollectBusiness();
                // Recupera y graba Collect
                var collectChecking = collectBusiness.GetCollectByCollectId(collectId);
                // Recupera y graba Collect
                //Array collectRow = GetBillByBillId(collectId);

                if (collectChecking != null && collectChecking.Id > 0)
                {

                    CollectConcept collectConcept = new CollectConcept() { Id = 0 };

                    Amount paymentsTotal = new Amount() { Value = Convert.ToDecimal(payment.LocalAmount.Value) };

                    Person payer = new Person();

                    if (paymentType == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)))
                    {
                        payer.IndividualId = individualId;
                    }
                    else if (paymentType == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)))
                    {
                        payer.IndividualId = individualId;
                    }
                    else
                    {
                        payer.IndividualId = Convert.ToInt32(collectChecking.Payer.IndividualId);
                    }

                    payer.IdentificationDocument = new IdentificationDocument()
                    {
                        DocumentType = collectChecking?.Payer?.IdentificationDocument?.DocumentType
                    };
                    payer.PersonType = collectChecking?.Payer?.PersonType;

                    Company accountingCompany = new Company() { IndividualId = Convert.ToInt32(collectChecking.AccountingCompany.IndividualId) };

                    collect.Description = collectDescription;
                    collect.Date = DateTime.Now;
                    collect.Concept = collectConcept;
                    collect.PaymentsTotal = paymentsTotal;
                    collect.Payer = new Person();
                    collect.Payer = payer;
                    collect.Status = Convert.ToInt32(collectChecking.Status);
                    collect.Number = 0;  // En la replica va 0
                    collect.UserId = Convert.ToInt32(collectChecking.UserId);
                    collect.AccountingCompany = accountingCompany;
                    collect.CollectType = collectChecking.CollectType;

                    TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
                    {
                        BranchId = branchId
                    };

                    TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);

                    Models.Collect.Transaction transaction = new Models.Collect.Transaction()
                    {
                        TechnicalTransaction = technicalTransaction.Id
                    };

                    collect.Transaction = transaction;
                    collect.Comments = collectChecking.Comments;
                    collect.IsTemporal = Convert.ToBoolean(collectChecking.IsTemporal);

                    newCollect = collect;

                    collectControl.Id = Convert.ToInt32(collectChecking.CollectControlId);
                }

                #endregion Collect

                #region CollectItem

                // Para recuperar el UserId y CollectControlId
                collectControl = _collectControlDAO.GetCollectControl(collectControl);

                #endregion CollectItem

                #region Payment

                if (isChangeCheck == 0)
                {
                    // Recupera Payments

                    newCollect.Payments = new List<Models.Payments.Payment>();
                    Amount amount = payment.Amount;
                    Amount localAmount = payment.LocalAmount;
                    if (paymentStatus == Convert.ToInt32(PaymentStatus.Rejected))
                    {
                        amount.Value = amount.Value * -1;
                        localAmount.Value = localAmount.Value * -1;
                    }


                    ExchangeRate exchangeRate = payment.ExchangeRate;

                    Bank issuingBank = new Bank();
                    CreditCardType creditCardType = new CreditCardType();
                    BankAccountCompany recievingAccount = new BankAccountCompany();

                    Models.Payments.PaymentMethod paymentMethod = payment.PaymentMethod;

                    #region PaymentMethodType
                    #region Cash

                    // Efectivo
                    if (paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CASH)))
                    {
                        Cash cash = new Cash()
                        {
                            Amount = amount,
                            ExchangeRate = exchangeRate,
                            LocalAmount = localAmount,
                            Id = 0,
                            PaymentMethod = paymentMethod,
                            Retention = Convert.ToDecimal(payment.Retentions),
                            Status = Convert.ToInt32(PaymentStatus.Active),
                            Tax = Convert.ToDecimal(payment.Taxes)
                        };
                        newCollect.Payments.Add(cash);
                    }

                    #endregion Cash
                    #region Check

                    // Cheque y débito
                    if (paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)) ||
                        paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK))
                      )
                    {
                        issuingBank.Id = Convert.ToInt32(payment.IssuingBankCode);

                        Check check = new Check()
                        {
                            Amount = amount,
                            Date = Convert.ToDateTime(payment.DatePayment),
                            DocumentNumber = payment.DocumentNumber,
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            IssuerName = payment.Holder,
                            IssuingAccountNumber = payment.IssuingAccountNumber,
                            IssuingBank = issuingBank,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            Retention = Convert.ToDecimal(payment.Retentions),
                            Status = Convert.ToInt32(paymentStatus),
                            Tax = Convert.ToDecimal(payment.Taxes)
                        };

                        newCollect.Payments.Add(check);
                    }

                    if (paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PAYMENT_TYPE_CONSIGNMENT_CHECK)))
                    {
                        issuingBank.Id = Convert.ToInt32(payment.IssuingBankCode);
                        ConsignmentCheck check = new ConsignmentCheck()
                        {
                            Amount = amount,
                            Date = Convert.ToDateTime(payment.DatePayment),
                            DocumentNumber = payment.DocumentNumber,
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            IssuerName = payment.Holder,
                            IssuingAccountNumber = payment.IssuingAccountNumber,
                            IssuingBank = issuingBank,
                            IssuingBankCode = issuingBank.Id,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            Retention = Convert.ToDecimal(payment.Retentions),
                            Status = Convert.ToInt32(paymentStatus),
                            Tax = Convert.ToDecimal(payment.Taxes)
                        };

                        newCollect.Payments.Add(check);
                    }

                    #endregion Check
                    #endregion PaymentMethodType



                }
                else
                {
                    newCollect.Payments = new List<Models.Payments.Payment>();
                    payment.Id = 0;
                    newCollect.Payments.Add(payment);
                }

                #endregion Payment

                // Envia a grabar Collect
                newCollect = SaveCollectRequestFromReplicate(newCollect, collectControl.Id);

                return newCollect;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// ReplicateCollect
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="collectDescription"></param>
        /// <param name="individualId"></param>
        /// <param name="isChangeCheck"></param>
        /// <param name="payment"></param>
        /// <returns>Collect</returns>
        public Collect ReplicateCollect(int collectId, string collectDescription, int individualId, int isChangeCheck, Models.Payments.Payment payment, int branchId)
        {
            try
            {
                int paymentType = 0;

                Collect collect = new Collect();
                Collect newCollect = new Collect();
                CollectControl collectControl = new CollectControl();

                AccountingPaymentServiceEEProvider paymentService = new AccountingPaymentServiceEEProvider();
                ArrayList collectPayment = paymentService.GetPaymentByBillId(collectId);

                Payment oldpayment = Assemblers.ModelAssembler.CreateCheckPayment(_paymentDAO.GetPayment(payment));
                oldpayment.Status = 0;
                decimal val = oldpayment.Amount.Value;
                oldpayment.Amount.Value = val * -1;
                oldpayment.Id = 0;

                if (collectPayment.Count > 0)
                {
                    ACCOUNTINGEN.Payment paymentEntity = collectPayment.OfType<ACCOUNTINGEN.Payment>().First();

                    // Tarjetas
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.Voucher;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD));
                    }
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.Voucher;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD));
                    }
                    // Checks
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.DocumentNumber;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK));

                    }
                    if (paymentEntity.PaymentMethodTypeCode == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)))
                    {
                        collectDescription = collectDescription + ": " + paymentEntity.DocumentNumber;
                        paymentType = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK));
                    }
                }

                #region Collect

                // Recupera y graba Collect
                Array collectRow = GetBillByBillId(collectId);

                if (collectRow.Length > 0)
                {
                    ACCOUNTINGEN.Collect collectEntity = collectRow.OfType<ACCOUNTINGEN.Collect>().First();
                    CollectConcept collectConcept = new CollectConcept() { Id = 0 };

                    Amount paymentsTotal = new Amount() { Value = Convert.ToDecimal(payment.LocalAmount.Value) };

                    Person payer = new Person();

                    // Tarjetas
                    if (paymentType == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)))
                    {
                        payer.IndividualId = Convert.ToInt32(collectEntity.IndividualId);
                    }
                    else if (paymentType == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD)))
                    {
                        payer.IndividualId = Convert.ToInt32(collectEntity.IndividualId);

                    }// Checkes
                    else if (paymentType == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)))
                    {
                        payer.IndividualId = individualId;
                    }
                    else if (paymentType == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)))
                    {
                        payer.IndividualId = individualId;
                    }
                    else
                    {
                        payer.IndividualId = Convert.ToInt32(collectEntity.IndividualId);
                    }

                    payer.IdentificationDocument = new IdentificationDocument()
                    {
                        DocumentType = new DocumentType() { Id = collectEntity.DocumentTypeId == null ? 0 : Convert.ToInt32(collectEntity.DocumentTypeId) }
                    };
                    payer.PersonType = new PersonType() { Id = collectEntity.PersonTypeId == null ? 0 : Convert.ToInt32(collectEntity.PersonTypeId) };

                    Company accountingCompany = new Company() { IndividualId = Convert.ToInt32(collectEntity.AccountingCompanyCode) };

                    collect.Description = collectDescription;
                    collect.Date = DateTime.Now;
                    collect.Concept = collectConcept;
                    collect.PaymentsTotal = paymentsTotal;
                    collect.Payer = new Person();
                    collect.Payer = payer;
                    collect.Status = Convert.ToInt32(collectEntity.Status);
                    collect.Number = 0;  // En la replica va 0
                    collect.UserId = Convert.ToInt32(collectEntity.UserId);
                    collect.AccountingCompany = accountingCompany;
                    collect.CollectType = new CollectTypes();

                    if (collectEntity.CollectType == 1)
                    {
                        collect.CollectType = CollectTypes.Incoming;
                    }
                    if (collectEntity.CollectType == 2)
                    {
                        collect.CollectType = CollectTypes.Outgoing;
                    }
                    if (collectEntity.CollectType == 3)
                    {
                        collect.CollectType = CollectTypes.DailyAccount;
                    }

                    TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
                    {
                        BranchId = branchId
                    };

                    TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);

                    Models.Collect.Transaction transaction = new Models.Collect.Transaction()
                    {
                        TechnicalTransaction = technicalTransaction.Id
                    };

                    collect.Transaction = transaction;
                    collect.Comments = collectEntity.Comments;
                    collect.IsTemporal = Convert.ToBoolean(collectEntity.IsTemporal);

                    newCollect = collect;

                    collectControl.Id = Convert.ToInt32(collectEntity.CollectControlCode);
                }

                #endregion Collect

                #region CollectItem

                // Para recuperar el UserId y CollectControlId
                collectControl = _collectControlDAO.GetCollectControl(collectControl);

                #endregion CollectItem

                #region Payment

                if (isChangeCheck == 0)
                {
                    // Recupera Payments
                    if (collectPayment.Count > 0)
                    {
                        newCollect.Payments = new List<Models.Payments.Payment>();

                        foreach (ACCOUNTINGEN.Payment paymentEntity in collectPayment)
                        {
                            if (paymentEntity.PaymentCode == payment.Id)
                            {
                                Amount amount = new Amount()
                                {
                                    Currency = new Currency() { Id = Convert.ToInt32(paymentEntity.CurrencyCode) },
                                    Value = Convert.ToDecimal(paymentEntity.IncomeAmount)
                                };
                                ExchangeRate exchangeRate = new ExchangeRate() { BuyAmount = Convert.ToDecimal(paymentEntity.ExchangeRate) };
                                Amount localAmount = new Amount() { Value = Convert.ToDecimal(paymentEntity.Amount) };
                                Bank issuingBank = new Bank();
                                CreditCardType creditCardType = new CreditCardType();
                                BankAccountCompany recievingAccount = new BankAccountCompany();

                                Models.Payments.PaymentMethod paymentMethod = new Models.Payments.PaymentMethod() { Id = Convert.ToInt32(paymentEntity.PaymentMethodTypeCode) };

                                #region PaymentMethodType


                                #region Cash

                                // Efectivo
                                if (paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CASH)))
                                {
                                    Cash cash = new Cash()
                                    {
                                        Amount = amount,
                                        ExchangeRate = exchangeRate,
                                        LocalAmount = localAmount,
                                        Id = 0,
                                        PaymentMethod = paymentMethod,
                                        Retention = Convert.ToDecimal(paymentEntity.Retentions),
                                        Status = Convert.ToInt32(paymentEntity.Status),
                                        Tax = Convert.ToDecimal(paymentEntity.Taxes)
                                    };
                                    newCollect.Payments.Add(cash);
                                }

                                #endregion Cash

                                #region Check

                                // Cheque y débito
                                if (paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)) ||
                                    paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK))
                                  )
                                {
                                    issuingBank.Id = Convert.ToInt32(paymentEntity.IssuingBankCode);

                                    Check check = new Check()
                                    {
                                        Amount = amount,
                                        Date = Convert.ToDateTime(paymentEntity.DatePayment),
                                        DocumentNumber = paymentEntity.DocumentNumber,
                                        ExchangeRate = exchangeRate,
                                        Id = 0,
                                        IssuerName = paymentEntity.Holder,
                                        IssuingAccountNumber = paymentEntity.IssuingAccountNumber,
                                        IssuingBank = issuingBank,
                                        LocalAmount = localAmount,
                                        PaymentMethod = paymentMethod,
                                        Retention = Convert.ToDecimal(paymentEntity.Retentions),
                                        Status = Convert.ToInt32(paymentEntity.Status),
                                        Tax = Convert.ToDecimal(paymentEntity.Taxes)
                                    };

                                    newCollect.Payments.Add(check);
                                }

                                #endregion Check

                                #region CreditCard

                                // Tarjeta de crédito
                                if (paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)) ||
                                  paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD)) ||
                                  paymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DATA_PHONE)))
                                {
                                    issuingBank.Id = Convert.ToInt32(paymentEntity.IssuingBankCode);
                                    creditCardType.Id = Convert.ToInt32(paymentEntity.CreditCardTypeCode);
                                    CreditCardValidThru creditCardValidThru = new CreditCardValidThru()
                                    {
                                        Month = Convert.ToInt32(paymentEntity.ValidMonth),
                                        Year = Convert.ToInt32(paymentEntity.ValidYear)
                                    };

                                    CreditCard creditCard = new CreditCard()
                                    {
                                        Amount = amount,
                                        AuthorizationNumber = paymentEntity.AuthorizationNumber,
                                        CardNumber = paymentEntity.DocumentNumber,
                                        ExchangeRate = exchangeRate,
                                        Holder = paymentEntity.Holder,
                                        Id = 0,
                                        IssuingBank = issuingBank,
                                        LocalAmount = localAmount,
                                        PaymentMethod = paymentMethod,
                                        Retention = Convert.ToDecimal(paymentEntity.Retentions),
                                        Status = Convert.ToInt32(paymentEntity.Status),
                                        Tax = Convert.ToDecimal(paymentEntity.Taxes),
                                        Type = creditCardType,
                                        ValidThru = creditCardValidThru,
                                        Voucher = paymentEntity.Voucher
                                    };

                                    newCollect.Payments.Add(creditCard);
                                }

                                #endregion CreditCard

                                #region Transfer

                                string enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DIRECT_CONECTION);
                                string enumResponse2 = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_TRANSFER);
                                // Transferencia
                                if (enumResponse != "" && paymentMethod.Id == Convert.ToInt32(enumResponse) || enumResponse2 != "" && paymentMethod.Id == Convert.ToInt32(enumResponse2))
                                {
                                    issuingBank.Id = Convert.ToInt32(paymentEntity.IssuingBankCode);

                                    BankAccountPerson recievingAccountPerson = new BankAccountPerson()
                                    {
                                        Bank = new Bank() { Id = Convert.ToInt32(paymentEntity.ReceivingBankCode) },
                                        Number = paymentEntity.ReceivingAccountNumber
                                    };

                                    Transfer transfer = new Transfer()
                                    {
                                        Amount = amount,
                                        Date = Convert.ToDateTime(paymentEntity.DatePayment),
                                        DocumentNumber = paymentEntity.DocumentNumber,
                                        ExchangeRate = exchangeRate,
                                        Id = 0,
                                        IssuerName = paymentEntity.Holder,
                                        IssuingAccountNumber = paymentEntity.IssuingAccountNumber,
                                        IssuingBank = issuingBank,
                                        LocalAmount = localAmount,
                                        PaymentMethod = paymentMethod,
                                        ReceivingAccount = recievingAccountPerson,
                                        Retention = Convert.ToDecimal(paymentEntity.Retentions),
                                        Status = Convert.ToInt32(paymentEntity.Status),
                                        Tax = Convert.ToDecimal(paymentEntity.Taxes)
                                    };

                                    newCollect.Payments.Add(transfer);
                                }

                                #endregion Transfer

                                #region Deposit



                                enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DEPOSIT_VOUCHER);

                                // Boleta de depósito
                                if (enumResponse != "" && paymentMethod.Id == Convert.ToInt32(enumResponse))
                                {
                                    recievingAccount.Bank = new Bank();
                                    recievingAccount.Bank.Id = Convert.ToInt32(paymentEntity.ReceivingBankCode);
                                    recievingAccount.Number = paymentEntity.ReceivingAccountNumber;

                                    DepositVoucher depositVoucher = new DepositVoucher()
                                    {
                                        Amount = amount,
                                        Date = Convert.ToDateTime(paymentEntity.DatePayment),
                                        DepositorName = paymentEntity.Holder,
                                        ExchangeRate = exchangeRate,
                                        Id = 0,
                                        LocalAmount = localAmount,
                                        PaymentMethod = paymentMethod,
                                        ReceivingAccount = recievingAccount,
                                        Retention = Convert.ToDecimal(paymentEntity.Retentions),
                                        Status = Convert.ToInt32(paymentEntity.Status),
                                        Tax = Convert.ToDecimal(paymentEntity.Taxes),
                                        VoucherNumber = paymentEntity.DocumentNumber
                                    };

                                    newCollect.Payments.Add(depositVoucher);
                                }

                                #endregion Deposit

                                #region Retention

                                enumResponse = (string)UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_RETENTION_RECEIPT);
                                // Recibo de retención
                                if (enumResponse != "" && paymentMethod.Id == Convert.ToInt32(enumResponse))
                                {
                                    RetentionReceipt retentionReceipt = new RetentionReceipt()
                                    {
                                        Amount = amount,
                                        AuthorizationNumber = paymentEntity.AuthorizationNumber,
                                        BillNumber = paymentEntity.CollectCode.ToString(),
                                        Date = Convert.ToDateTime(paymentEntity.DatePayment),
                                        ExchangeRate = exchangeRate,
                                        Id = 0,
                                        LocalAmount = localAmount,
                                        PaymentMethod = paymentMethod,
                                        Retention = Convert.ToDecimal(paymentEntity.Retentions),
                                        Status = Convert.ToInt32(paymentEntity.Status),
                                        SerialNumber = paymentEntity.SerialNumber,
                                        SerialVoucherNumber = paymentEntity.SerialVoucher,
                                        Tax = Convert.ToDecimal(paymentEntity.Taxes),
                                        VoucherNumber = paymentEntity.Voucher
                                    };

                                    newCollect.Payments.Add(retentionReceipt);
                                }

                                #endregion Retention

                                #endregion PaymentMethodType
                            }
                        }
                    }
                }
                else
                {
                    newCollect.Payments = new List<Models.Payments.Payment>();
                    payment.Id = 0;
                    newCollect.Payments.Add(oldpayment);
                    newCollect.Payments.Add(payment);
                }

                #endregion Payment

                // Envia a grabar Collect
                newCollect = SaveCollectRequestFromReplicate(newCollect, collectControl.Id);

                return newCollect;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }



        /// <summary>
        /// SaveCollectRequestFromReplicate
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <returns>Collect</returns>
        private Collect SaveCollectRequestFromReplicate(Collect collect, int collectControlId)
        {
            //ESTE MÉTODO SE CREA YA QUE EL MÉTODO ORIGINAL (SaveBillRequest) TIENE TRANSACCIONALIDAD Y HACE CAER EL PROCESO 
            //CUANDO SE LO LLAMA DESDE OTRO CONTEXTO
            try
            {
                #region Collect

                // Graba collect
                Collect newCollect = _collectDAO.SaveCollect(collect, collectControlId);

                #endregion Collect

                // Graba pagos
                if (collect.Payments != null && collect.Payments.Count > 0)
                {
                    #region Payments

                    newCollect.Payments = new List<Models.Payments.Payment>();

                    foreach (Models.Payments.Payment payment in collect.Payments)
                    {
                        Models.Payments.Payment newPayment = _paymentDAO.SavePayment(payment, newCollect.Id);

                        if (payment.Taxes != null)
                        {
                            foreach (PaymentTax paymentTax in payment.Taxes)
                            {
                                _paymentTaxDAO.SavePaymentTax(paymentTax, newPayment.Id);
                            }
                        }

                        newCollect.Payments.Add(newPayment);

                        // Graba el Log de Payment
                        AccountingPaymentServiceEEProvider paymentService = new AccountingPaymentServiceEEProvider();
                        paymentService.SavePaymentLog(Convert.ToInt32(ActionTypes.CreatePayment), newCollect.Id, newPayment.Id, newPayment.Status, newCollect.UserId);
                    }

                    #endregion Payments
                }

                Collect updatedCollect = new Collect() { Id = newCollect.Id };
                CollectBusiness collectBusiness = new CollectBusiness();
                updatedCollect = collectBusiness.GetCollectByCollectId(newCollect.Id); // Devuelve el bill con la actualización de estado
                updatedCollect.Payments = newCollect.Payments;

                Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
                integration2GBusiness.Save(newCollect.ToModelIntegration(1, true));
                return updatedCollect;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion Collect

        #region AccountingCompany

        /// <summary>
        /// GetAccountingCompany
        /// Recupera todos las companías
        /// </summary>
        /// <returns>List<Company/></returns>
        public List<DTOs.CompanyDTO> GetAccountingCompany()
        {
            try
            {
                return _companyDAO.GetCompanies().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingCompany

        #region MassiveProcess

        /// <summary>
        /// GetMassiveProcess
        /// Obtiene los procesos masivos pendientes de culminar
        /// </summary>
        /// <returns>List<MassiveProcessDTO/></returns>
        public List<MassiveProcessDTO> GetMassiveProcess(int userId)
        {
            try
            {
                #region LoadUIVIEW

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectMassiveProcess.Properties.UserId, userId);

                UIView processes = DataFacadeManager.Instance.GetDataFacade().GetView("CollectMassiveProcessView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rowsGrid);

                if (processes.Rows.Count > 0)
                {
                    processes.Columns.Add("Rows", typeof(int));
                    processes.Rows[0]["Rows"] = rowsGrid;
                }

                #endregion LoadUIVIEW

                #region LoadDTO

                List<MassiveProcessDTO> massiveProcess = new List<MassiveProcessDTO>();

                foreach (DataRow row in processes.Rows)
                {
                    int rowsUiview;

                    rowsUiview = row["Rows"] == DBNull.Value ? 0 : Convert.ToInt32(row["Rows"]);
                    decimal totalRecords = Convert.ToDecimal(row["SuccessfulRecords"]) + Convert.ToDecimal(row["FailedRecords"]);
                    decimal porcentageAdvance = 0;

                    if (Convert.ToInt32(row["TotalRecords"]) > 0)
                    {
                        porcentageAdvance = Convert.ToDecimal((totalRecords / Convert.ToDecimal(row["TotalRecords"])) * 100);
                    }

                    massiveProcess.Add(new MassiveProcessDTO()
                    {
                        BeginDate = row["BeginDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(row["BeginDate"]),
                        Description = "",
                        EndDate = row["EndDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["EndDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                        FailedRecords = row["FailedRecords"] == DBNull.Value ? -1 : Convert.ToInt32(row["FailedRecords"]),
                        PorcentageAdvance = porcentageAdvance,
                        ProcessId = row["CollectMassiveProcessId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CollectMassiveProcessId"]),
                        StateId = row["Status"] == DBNull.Value ? -1 : Convert.ToInt32(row["Status"]),
                        StateDescription = Convert.ToInt32(row["Status"]) == 0 ? "Pendiente" : "Finalizado",
                        SuccessfulRecords = row["SuccessfulRecords"] == DBNull.Value ? -1 : Convert.ToInt32(row["SuccessfulRecords"]),
                        TotalRecords = row["TotalRecords"] == DBNull.Value ? -1 : Convert.ToInt32(row["TotalRecords"]),
                        UserId = row["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(row["UserId"]),
                        UserName = row["UserName"] == DBNull.Value ? "" : Convert.ToString(row["UserName"]),
                        Rows = rowsUiview
                    });
                }

                #endregion LoadDTO

                return massiveProcess;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// SaveCollectMassiveProcess
        /// Graba los procesos masivos
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        public CollectMassiveProcessDTO SaveCollectMassiveProcess(CollectMassiveProcessDTO collectMassiveProcess)
        {
            return _collectMassiveProcessDAO.SaveCollectMassiveProcess(collectMassiveProcess);
        }

        /// <summary>
        /// UpdateCollectMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        public CollectMassiveProcessDTO UpdateCollectMassiveProcess(CollectMassiveProcessDTO collectMassiveProcess)
        {
            return _collectMassiveProcessDAO.UpdateCollectMassiveProcess(collectMassiveProcess);
        }

        #endregion MassiveProcess




        #region Private Methods

        #region Bill

        /// <summary>
        /// GetBillByBillId
        /// Recupera un recibo específico
        /// </summary>
        /// <returns>Array</returns>
        private Array GetBillByBillId(int collectId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.CollectId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(collectId);
                // Se asigna BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuilder.GetPredicate()));

                Array collect = businessCollection.ToArray();

                return collect;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBillByBillId
        /// Recupera un recibo específico
        /// </summary>
        /// <returns>Array</returns>
        public Collect GetCollectByTransactionId(int technicalTransaction)
        {
            try
            {
                ObjectCriteriaBuilder collectFilter = new ObjectCriteriaBuilder();
                collectFilter.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, technicalTransaction);
                var entityCollect = (ACCOUNTINGEN.Collect)DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.Collect), collectFilter.GetPredicate()).First();
                return Assemblers.ModelAssembler.CreateCollect(entityCollect);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// FormatDateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DateTime FormatDateTime(string dateTime)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            IFormatProvider culture = new CultureInfo("es-EC", true);

            return Convert.ToDateTime(dateTime, culture);
        }
        #endregion Bill

        #endregion Private Methods


        /// <summary>
        /// GetCollectByCollectId
        /// </summary>
        /// <returns>Collect</returns>
        public CollectDTO GetCollectByCollectId(int collectId)
        {
            try
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                return collectBusiness.GetCollectByCollectId(collectId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<CurrencyDTO> GetAvaibleCurrencies()
        {
            try
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                return collectBusiness.GetAvaibleCurrencies().ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAvaibleCurrencies);
            }
        }

        public List<BankDTO> GetAvaibleBanksByCurrencyId(int currencyId)
        {
            try
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                return collectBusiness.GetAvaibleBanksByCurrencyId(currencyId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAvaibleBanksByCurrencyId);
            }
        }


        public List<DTOs.BankAccounts.BankAccountCompanyDTO> GetAvaibleAccountsByCurrencyIdBankId(int currencyId, int bankId)
        {
            try
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                return collectBusiness.GetAvaibleAccountsByCurrencyIdBankId(currencyId, bankId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetAvaibleAccountsByCurrencyIdBankId);
            }
        }

        
    }
}
