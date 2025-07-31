using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using CoreTransaction = Sistran.Core.Framework.Transactions;
using PaymentMethod = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments.PaymentMethod;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Voucher = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.Voucher;
using VoucherConcept = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherConcept;
using VoucherConceptTax = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables.VoucherConceptTax;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountsPayableServiceEEProvider : IAccountingAccountsPayableService
    {
        #region Constants

        #endregion Constants

        #region Interfaz

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #endregion Interfaz

        #region Instance Viarables


        #region DAOs

        private readonly CheckBookControlDAO _checkBookControlDAO = new CheckBookControlDAO();
        private readonly PaymentOrderDAO _paymentOrderDAO = new PaymentOrderDAO();
        private readonly TempPaymentOrderDAO _tempPaymentOrderDAO = new TempPaymentOrderDAO();
        private readonly CheckPaymentOrderDAO _checkPaymentOrderDAO = new CheckPaymentOrderDAO();
        private readonly CheckPaymentOrderTransactionItemDAO _checkPaymentOrderTransactionItemDAO = new CheckPaymentOrderTransactionItemDAO();
        private readonly TempPremiumReceivableTransactionItemDAO _tempPremiumReceivableTransactionItemDAO = new TempPremiumReceivableTransactionItemDAO();
        private readonly TempBrokerCheckingAccountTransactionItemDAO _tempBrokerCheckingAccountTransactionItemDAO = new TempBrokerCheckingAccountTransactionItemDAO();
        private readonly TempReinsuranceCheckingAccountTransactionItemDAO _tempReinsuranceCheckingAccountTransactionItemDAO = new TempReinsuranceCheckingAccountTransactionItemDAO();
        private readonly TempDailyAccountingTransactionItemDAO _tempDailyAccountingTransactionItemDAO = new TempDailyAccountingTransactionItemDAO();
        private readonly TempCoinsuranceCheckingAccountTransactionItemDAO _tempCoinsuranceCheckingAccountTransactionItemDAO = new TempCoinsuranceCheckingAccountTransactionItemDAO();
        private readonly TempClaimPaymentRequestDAO _tempClaimPaymentRequestDAO = new TempClaimPaymentRequestDAO();
        private readonly TempPaymentRequestTransactionDAO _tempPaymentRequestTransactionDAO = new TempPaymentRequestTransactionDAO();
        private readonly OtherPaymentsRequestDAO _otherPaymentsRequestDAO = new OtherPaymentsRequestDAO();
        private readonly TransferPaymentOrderDAO _transferPaymentOrderDAO = new TransferPaymentOrderDAO();
        private readonly PaymentOrderTransferPaymentDAO _paymentOrderTransferPaymentDAO = new PaymentOrderTransferPaymentDAO();
        private readonly CompanyDAO _companyDAO = new CompanyDAO();
        private readonly TempPaymentRequestDAO _tempPaymentRequestDAO = new TempPaymentRequestDAO();
        private readonly PaymentRequestNumberDAO _paymentRequestNumberDAO = new PaymentRequestNumberDAO();
        private readonly PaymentRequestDAO _paymentRequestDAO = new PaymentRequestDAO();
        private readonly VoucherDAO _voucherDAO = new VoucherDAO();
        private readonly VoucherConceptDAO _voucherConceptDAO = new VoucherConceptDAO();
        private readonly VoucherConceptTaxDAO _voucherConceptTaxDAO = new VoucherConceptTaxDAO();
        private readonly PaymentOrderAuthorizationDAO _paymentOrderAuthorizationDAO = new PaymentOrderAuthorizationDAO();
        private readonly DAOs.Accounting.TempApplicationDAO tempApplicationDAO = new DAOs.Accounting.TempApplicationDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region CheckBookControl
        /// <summary>
        /// SaveCheckBookControl
        /// Graba el control de chequera
        /// </summary>
        /// <param name="checkBookControl"></param>
        /// <returns>CheckBookControl</returns>
        public CheckBookControlDTO SaveCheckBookControl(CheckBookControlDTO checkBookControl)
        {
            try
            {

                if (checkBookControl.Id == 0)
                {
                    // Verifica si ya existe un cierto tipo de chequera registrado manual/auto
                    List<CheckBookControl> checkBookControls = _checkBookControlDAO.GetCheckBookControlActiveByAccountBankId(
                        checkBookControl.AccountBank.Id,
                        checkBookControl.IsAutomatic ? 1 : 0);

                    if (checkBookControls.Count > 0)
                    {
                        checkBookControl.Id = 0;
                        return checkBookControl;
                    }
                    else
                    {
                        // Verifica si ya existe un cierto tipo de chequera registrado manual/auto
                        checkBookControls = _checkBookControlDAO.GetCheckBookControlActiveByAccountBankId(
                                              checkBookControl.AccountBank.Id,
                                              checkBookControl.IsAutomatic ? 0 : 1);

                        if (checkBookControls.Count > 0)
                        {
                            checkBookControl.Id = 0;
                            return checkBookControl;
                        }
                        else
                        {
                            return _checkBookControlDAO.SaveCheckBookControl(checkBookControl.ToModel()).ToDTO();
                        }
                    }
                }

                return _checkBookControlDAO.UpdateCheckBookControl(checkBookControl.ToModel()).ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckBookControlsByAccountBankId
        /// Obtiene el control de chequera por cuenta bancaria
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <returns>List<CheckBookControl></returns>
        public List<CheckBookControlDTO> GetCheckBookControlsByAccountBankId(int accountBankId)
        {
            int rows;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CheckBookControlBank.Properties.AccountBankCode, accountBankId);

                UIView bookControls = _dataFacadeManager.GetDataFacade().GetView("CheckBookControlBankView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                List<CheckBookControl> checkBookControls = new List<CheckBookControl>();

                foreach (DataRow row in bookControls)
                {
                    BankAccountCompany bankAccountCompany = new BankAccountCompany();
                    bankAccountCompany.Id = Convert.ToInt32(row["AccountBankCode"]);

                    checkBookControls.Add(new CheckBookControl()
                    {
                        Id = Convert.ToInt32(row["CheckbookControlCode"]),
                        AccountBank = bankAccountCompany,
                        CheckFrom = Convert.ToInt32(row["CheckFrom"]),
                        CheckTo = Convert.ToInt32(row["CheckTo"]),
                        DisabledDate = row["DisabledDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(row["DisabledDate"]),
                        IsAutomatic = Convert.ToBoolean(row["IsAutomatic"]),
                        LastCheck = Convert.ToInt32(row["LastCheck"]),
                        Status = Convert.ToInt32(row["Status"])
                    });
                }
                return checkBookControls.ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckBookControlsByBankIdBranchId
        /// Obtiene el control de chequera por banco y sucursal
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="branchId"></param>
        /// <returns>List<CheckBookControlDTO/></returns>
        public List<SearchDTO.CheckBookControlDTO> GetCheckBookControlsByBankIdBranchId(int bankId, int branchId)
        {
            int rows;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CheckBookControlBank.Properties.BankCode, bankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CheckBookControlBank.Properties.BranchCode, branchId);

                UIView bookControls = _dataFacadeManager.GetDataFacade().GetView("CheckBookControlBankView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                List<SearchDTO.CheckBookControlDTO> checkBookControls = new List<SearchDTO.CheckBookControlDTO>();

                foreach (DataRow row in bookControls)
                {
                    checkBookControls.Add(new SearchDTO.CheckBookControlDTO()
                    {
                        CheckbookControlCode = row["CheckbookControlCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckbookControlCode"]),
                        AccountBankCode = row["AccountBankCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["AccountBankCode"]),
                        BankCode = row["BankCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BankCode"]),
                        BranchCode = row["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchCode"]),
                        CheckFrom = row["CheckFrom"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckFrom"]),
                        CheckTo = row["CheckTo"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckTo"]),
                        DescriptionBank = row["DescriptionBank"] == DBNull.Value ? "" : Convert.ToString(row["DescriptionBank"]),
                        DisabledDate = row["DisabledDate"] == DBNull.Value ? "" : Convert.ToString(row["DisabledDate"]),
                        IsAutomatic = row["IsAutomatic"] == DBNull.Value ? 0 : Convert.ToInt32(row["IsAutomatic"]),
                        LastCheck = row["LastCheck"] == DBNull.Value ? 0 : Convert.ToInt32(row["LastCheck"]),
                        Status = row["Status"] == DBNull.Value ? 0 : Convert.ToInt32(row["Status"]),
                        Number = row["Number"] == DBNull.Value ? "" : Convert.ToString(row["Number"]),
                        SmallDescriptionBranch = row["SmallDescriptionBranch"] == DBNull.Value ? "" : Convert.ToString(row["SmallDescriptionBranch"])
                    });
                }

                return checkBookControls;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckBookControlActiveByAccountBankId
        /// Obtiene el control de chequeras activas por cuenta bancaria
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="isAutomatic"></param>
        /// <returns>List<CheckBookControl/></returns>
        public List<CheckBookControlDTO> GetCheckBookControlActiveByAccountBankId(int accountBankId, int isAutomatic)
        {
            try
            {

                return _checkBookControlDAO.GetCheckBookControlActiveByAccountBankId(accountBankId, isAutomatic).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region PaymentOrder

        /// <summary>
        /// SaveTempPaymentOrder
        /// Graba una orden de pago temporal
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrderDTO SaveTempPaymentOrder(PaymentOrderDTO paymentOrder)
        {
            try
            {

                return _tempPaymentOrderDAO.SaveTempPaymentOrder(paymentOrder.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempPaymentOrder
        /// Edita una orden de pago temporal
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrderDTO UpdateTempPaymentOrder(PaymentOrderDTO paymentOrder)
        {
            try
            {
                return _tempPaymentOrderDAO.UpdateTempPaymentOrder(paymentOrder.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempPaymentOrderById
        /// Obtiene una orden de pago temporal
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrderDTO GetTempPaymentOrderById(int paymentOrderId)
        {

            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentOrder.Properties.TempPaymentOrderCode, paymentOrderId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempPaymentOrder), criteriaBuilder.GetPredicate()));

                PaymentOrder paymentOrder = new PaymentOrder();

                foreach (ACCOUNTINGEN.TempPaymentOrder paymentOrderEntity in businessCollection.OfType<ACCOUNTINGEN.TempPaymentOrder>())
                {
                    paymentOrder.Id = paymentOrderEntity.TempPaymentOrderCode;
                    paymentOrder.BankAccountPerson = new BankAccountPerson() { Id = Convert.ToInt32(paymentOrderEntity.AccountBankCode) };
                    paymentOrder.AccountingDate = Convert.ToDateTime(paymentOrderEntity.AccountingDate);
                    paymentOrder.Amount = new Amount()
                    {
                        Currency = new Currency() { Id = Convert.ToInt32(paymentOrderEntity.CurrencyCode) },
                        Value = Convert.ToDecimal(paymentOrderEntity.IncomeAmount)
                    };
                    paymentOrder.LocalAmount = new Amount() { Value = Convert.ToDecimal(paymentOrderEntity.Amount) };
                    paymentOrder.ExchangeRate = new ExchangeRate() { BuyAmount = Convert.ToDecimal(paymentOrderEntity.ExchangeRate.Value) };
                    paymentOrder.Beneficiary = new Individual();
                    paymentOrder.Beneficiary.IndividualId = Convert.ToInt32(paymentOrderEntity.IndividualId);
                    paymentOrder.Branch = new Branch() { Id = Convert.ToInt32(paymentOrderEntity.BranchCode) };
                    paymentOrder.BranchPay = new Branch() { Id = Convert.ToInt32(paymentOrderEntity.BranchCdPay) };
                    paymentOrder.Company = new Company() { IndividualId = Convert.ToInt32(paymentOrderEntity.CompanyCode) };
                    paymentOrder.EstimatedPaymentDate = Convert.ToDateTime(paymentOrderEntity.EstimatedPaymentDate);
                    paymentOrder.PaymentMethod = new PaymentMethod() { Id = Convert.ToInt32(paymentOrderEntity.PaymentMethodCode) };                    
                    paymentOrder.PaymentSource = new ConceptSourceDTO() { Id = Convert.ToInt32(paymentOrderEntity.PaymentSourceCode) }.ToModel();
                    paymentOrder.PayTo = paymentOrderEntity.PayTo;
                    paymentOrder.PersonType = new PersonType() { Id = Convert.ToInt32(paymentOrderEntity.PersonTypeCode) };
                    paymentOrder.Status = Convert.ToInt32(paymentOrderEntity.Status);
                    paymentOrder.Observation = paymentOrderEntity.Observation;
                }

                return paymentOrder.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// SavePaymentOrderImputationRequest
        /// Graba una orden de pago 
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int SavePaymentOrderImputationRequest(int tempPaymentOrderId, int tempImputationId, int imputationTypeId, int userId)
        {

            try
            {

                bool success = false;
                int paymentOrderId = 0;

                // Se graba la cabecera 
                PaymentOrder newPaymentOrder = GetTempPaymentOrderById(tempPaymentOrderId).ToModel();
                newPaymentOrder.Id = 0;
                newPaymentOrder = _paymentOrderDAO.SavePaymentOrder(newPaymentOrder);

                if (newPaymentOrder.Id > 0)
                {
                    Models.Imputations.Application imputation = new Models.Imputations.Application();
                    imputation.Id = tempImputationId;
                    imputation.IsTemporal = true;

                    tempApplicationDAO.UpdateTempApplication(imputation, newPaymentOrder.Id);
                    success = true;
                }

                if (success)
                {
                    _tempPaymentOrderDAO.DeleteTempPaymentOrder(tempPaymentOrderId);
                    paymentOrderId = newPaymentOrder.Id;
                }

                return paymentOrderId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempPaymentOrderByTempId
        /// Obtiene una orden de pago temporal dado el su Id
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <returns>TempPaymentOrderDTO</returns>
        public SearchDTO.TempPaymentOrderDTO GetTempPaymentOrderByTempId(int tempPaymentOrderId)
        {
            int rows;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPaymentOrder.Properties.TempPaymentOrderCode, tempPaymentOrderId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.TempPaymentOrder.Properties.BranchCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                UIView paymentOrderItems = _dataFacadeManager.GetDataFacade().GetView("TempPaymentOrderItemView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                #endregion

                #region DTO

                SearchDTO.TempPaymentOrderDTO temporalPaymentOrder = new SearchDTO.TempPaymentOrderDTO();

                if (paymentOrderItems.Rows.Count > 0)
                {
                    temporalPaymentOrder.AccountBankId = paymentOrderItems.Rows[0]["AccountBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["AccountBankCode"]);
                    temporalPaymentOrder.Amount = paymentOrderItems.Rows[0]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(paymentOrderItems.Rows[0]["Amount"]);
                    temporalPaymentOrder.BranchId = paymentOrderItems.Rows[0]["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["BranchCode"]);
                    temporalPaymentOrder.BranchName = paymentOrderItems.Rows[0]["BranchName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["BranchName"]);
                    temporalPaymentOrder.BranchPayId = paymentOrderItems.Rows[0]["BranchPayCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["BranchPayCode"]);
                    temporalPaymentOrder.BranchPayName = paymentOrderItems.Rows[0]["BranchPayName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["BranchPayName"]);
                    temporalPaymentOrder.CompanyId = paymentOrderItems.Rows[0]["CompanyCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["CompanyCode"]);
                    temporalPaymentOrder.CompanyName = paymentOrderItems.Rows[0]["CompanyName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["CompanyName"]);
                    temporalPaymentOrder.CurrencyId = paymentOrderItems.Rows[0]["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["CurrencyCode"]);
                    temporalPaymentOrder.CurrencyName = paymentOrderItems.Rows[0]["CurrencyName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["CurrencyName"]);
                    temporalPaymentOrder.Exchange = paymentOrderItems.Rows[0]["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(paymentOrderItems.Rows[0]["ExchangeRate"]);
                    temporalPaymentOrder.IncomeAmount = paymentOrderItems.Rows[0]["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(paymentOrderItems.Rows[0]["IncomeAmount"]);
                    temporalPaymentOrder.IndividualId = paymentOrderItems.Rows[0]["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["IndividualId"]);
                    temporalPaymentOrder.EstimatedPaymentDate = paymentOrderItems.Rows[0]["EstimatedPaymentDate"] == DBNull.Value ? "" : ((DateTime)paymentOrderItems.Rows[0]["EstimatedPaymentDate"]).ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    temporalPaymentOrder.PaymentMethodId = paymentOrderItems.Rows[0]["PaymentMethodCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["PaymentMethodCode"]);
                    temporalPaymentOrder.PaymentMethodName = paymentOrderItems.Rows[0]["PaymentMethodName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["PaymentMethodName"]);
                    temporalPaymentOrder.PaymentSourceId = paymentOrderItems.Rows[0]["PaymentSourceCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["PaymentSourceCode"]);
                    temporalPaymentOrder.PaymentSourceName = paymentOrderItems.Rows[0]["PaymentSourceName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["PaymentSourceName"]);
                    temporalPaymentOrder.PayTo = paymentOrderItems.Rows[0]["PayTo"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["PayTo"]);
                    temporalPaymentOrder.PersonTypeId = paymentOrderItems.Rows[0]["PersonTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["PersonTypeCode"]);
                    temporalPaymentOrder.PersonTypeName = paymentOrderItems.Rows[0]["PersonTypeName"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["PersonTypeName"]);
                    temporalPaymentOrder.StatusId = paymentOrderItems.Rows[0]["Status"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["Status"]);
                    temporalPaymentOrder.TempPaymentOrderId = paymentOrderItems.Rows[0]["TempPaymentOrderCode"] == DBNull.Value ? -1 : Convert.ToInt32(paymentOrderItems.Rows[0]["TempPaymentOrderCode"]);
                    temporalPaymentOrder.Observation = paymentOrderItems.Rows[0]["Observation"] == DBNull.Value ? "" : Convert.ToString(paymentOrderItems.Rows[0]["Observation"]);

                }

                #endregion

                return temporalPaymentOrder;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankAccountByBeneficiaryId
        /// Obtiene una cuenta bancaria dado el beneficiario
        /// </summary>
        /// <param name="benficiaryId"></param>
        /// <returns>List<BeneficiaryBankAccountsDTO/></returns>
        public List<SearchDTO.BeneficiaryBankAccountsDTO> GetBankAccountByBeneficiaryId(string benficiaryId)
        {
            int rows;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AccountBankBeneficiary.Properties.IndividualId, Convert.ToInt32(benficiaryId));

                UIView bankAccounts = _dataFacadeManager.GetDataFacade().GetView("AccountBankBeneficiaryView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (bankAccounts.Rows.Count > 0)
                {
                    bankAccounts.Columns.Add("rows", typeof(int));
                    bankAccounts.Rows[0]["rows"] = bankAccounts.Rows.Count;
                }

                #region DTO

                List<SearchDTO.BeneficiaryBankAccountsDTO> beneficiaryBankAccounts = new List<SearchDTO.BeneficiaryBankAccountsDTO>();

                foreach (DataRow row in bankAccounts)
                {
                    beneficiaryBankAccounts.Add(new SearchDTO.BeneficiaryBankAccountsDTO()
                    {
                        AccountBankCode = row["AccountBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["AccountBankCode"]),
                        AccountNumber = row["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(row["AccountNumber"]),
                        AccountTypeCode = row["AccountTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["AccountTypeCode"]),
                        AccountTypeName = row["AccountTypeName"] == DBNull.Value ? "" : Convert.ToString(row["AccountTypeName"]),
                        BankCode = row["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["BankCode"]),
                        BankName = row["BankName"] == DBNull.Value ? "" : Convert.ToString(row["BankName"]),
                        CurrencyCode = row["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["CurrencyCode"]),
                        CurrencyName = row["CurrencyName"] == DBNull.Value ? "" : Convert.ToString(row["CurrencyName"]),
                        IndividualId = row["IndividualId"] == DBNull.Value ? "" : Convert.ToString(row["IndividualId"]),
                        TinyDescription = row["TinyDescription"] == DBNull.Value ? "" : Convert.ToString(row["TinyDescription"]),
                        Rows = bankAccounts.Rows.Count
                    });
                }

                #endregion

                return beneficiaryBankAccounts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentOrderByPaymentSourceIdPayDate
        ///  Obtiene órdenes de pago por origne de pago y fecha
        /// </summary>
        /// <param name="paymentSourceId"></param>
        /// <param name="payDate"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentOrderId"></param>
        /// <returns>List<TempPaymentOrderDTO/></returns>
        public List<SearchDTO.TempPaymentOrderDTO> GetPaymentOrderByPaymentSourceIdPayDate(int paymentSourceId, DateTime payDate,
                                                                                 int currencyId, int paymentOrderId)
        {
            int rows;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderPersonType.Properties.CurrencyCode, currencyId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderPersonType.Properties.Amount);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderPersonType.Properties.AccountBankCode);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(0);

                if (paymentSourceId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderPersonType.Properties.PaymentSourceCode, paymentSourceId);
                }

                if (payDate != new DateTime())
                {
                    string payDateSearch = payDate.ToString("dd/MM/yyyy");
                    payDateSearch = payDateSearch + " 23:59:59";

                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderPersonType.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(payDateSearch));
                }
                if (paymentOrderId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderPersonType.Properties.PaymentOrderCode, paymentOrderId);
                }

                UIView paymentOrders = _dataFacadeManager.GetDataFacade().GetView("PaymentOrderPersonTypeView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                if (paymentOrders.Rows.Count > 0)
                {
                    paymentOrders.Columns.Add("rows", typeof(int));
                    paymentOrders.Rows[0]["rows"] = paymentOrders.Rows.Count;
                }

                #endregion

                #region DTO

                List<SearchDTO.TempPaymentOrderDTO> tempPaymentOrderDTOs = new List<SearchDTO.TempPaymentOrderDTO>();

                if (paymentOrders.Rows.Count > 0)
                {
                    foreach (DataRow row in paymentOrders)
                    {
                        tempPaymentOrderDTOs.Add(new SearchDTO.TempPaymentOrderDTO()
                        {
                            TempPaymentOrderId = Convert.ToInt32(row["PaymentOrderCode"]),
                            Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Amount"]),
                            BeneficiaryDocumentNumber = row["BeneficiaryDocumentNumber"] == DBNull.Value ? "" : Convert.ToString(row["BeneficiaryDocumentNumber"]),
                            BeneficiaryName = row["BeneficiaryName"] == DBNull.Value ? "" : Convert.ToString(row["BeneficiaryName"]),
                            BeneficiaryType = row["BeneficiaryType"] == DBNull.Value ? "" : Convert.ToString(row["BeneficiaryType"]),
                            BranchId = row["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["BranchCode"]),
                            BranchName = row["BranchName"] == DBNull.Value ? "" : Convert.ToString(row["BranchName"]),
                            BranchPayId = row["BranchCdPay"] == DBNull.Value ? -1 : Convert.ToInt32(row["BranchCdPay"]),
                            BranchPayName = row["BranchPayName"] == DBNull.Value ? "" : Convert.ToString(row["BranchPayName"]),
                            CompanyId = row["CompanyCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["CompanyCode"]),
                            CompanyName = row["CompanyName"] == DBNull.Value ? "" : Convert.ToString(row["CompanyName"]),
                            CurrencyId = row["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["CurrencyCode"]),
                            CurrencyName = row["CurrencyName"] == DBNull.Value ? "" : Convert.ToString(row["CurrencyName"]),
                            Exchange = row["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ExchangeRate"]),
                            IndividualId = row["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(row["IndividualId"]),
                            PaymentDate = row["PaymentDate"] == DBNull.Value ? "" : Convert.ToString(row["PaymentDate"]),
                            EstimatedPaymentDate = row["EstimatedPaymentDate"] == DBNull.Value ? "" : Convert.ToString(row["EstimatedPaymentDate"]),
                            PaymentMethodId = row["PaymentMethodCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["PaymentMethodCode"]),
                            PaymentMethodName = row["PaymentMethodName"] == DBNull.Value ? "" : Convert.ToString(row["PaymentMethodName"]),
                            PaymentSourceId = row["PaymentSourceCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["PaymentSourceCode"]),
                            PaymentSourceName = row["PaymentSourceName"] == DBNull.Value ? "" : Convert.ToString(row["PaymentSourceName"]),
                            PayTo = row["PayTo"] == DBNull.Value ? "" : Convert.ToString(row["PayTo"]),
                            PersonTypeId = row["PersonTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["PersonTypeCode"]),
                            PersonTypeName = row["BeneficiaryType"] == DBNull.Value ? "" : Convert.ToString(row["BeneficiaryType"]),
                            StatusId = row["Status"] == DBNull.Value ? -1 : Convert.ToInt32(row["Status"])
                        });
                    }
                }

                #endregion

                return tempPaymentOrderDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// SearchPaymentOrders
        /// Trae órdenes de pago dados varios paràmetros
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<PaymentOrderDTO/></returns>
        public List<SearchDTO.PaymentOrderDTO> SearchPaymentOrders(SearchDTO.SearchParameterPaymentOrdersDTO searchParameter)
        {
            List<SearchDTO.PaymentOrderDTO> paymentOrderDTOs = new List<SearchDTO.PaymentOrderDTO>();

            int rows;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                if (searchParameter != null)
                {
                    if (searchParameter.Branch.Id > 0)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.BranchCode, searchParameter.Branch.Id);
                    }
                    else
                    {
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.BranchCode);
                        criteriaBuilder.Greater();
                        criteriaBuilder.Constant(0);
                    }

                    if (searchParameter.UserId != -1)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.UserId, searchParameter.UserId);
                    }

                    if (searchParameter.PaymentMethod.Id != -1)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.PaymentMethodCode, searchParameter.PaymentMethod.Id);
                    }

                    if (searchParameter.PaymentOrderNumber != "*")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.PaymentOrderCode, Convert.ToInt64(searchParameter.PaymentOrderNumber));
                    }

                    if (searchParameter.PersonType.Id != -1)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.PersonTypeCode, searchParameter.PersonType.Id);
                    }

                    if (searchParameter.BeneficiaryDocumentNumber != "*")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.BeneficiaryDocumentNumber, searchParameter.BeneficiaryDocumentNumber);
                    }

                    if (searchParameter.BeneficiaryFullName != "*")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.BeneficiaryName, searchParameter.BeneficiaryFullName);
                    }

                    if (searchParameter.StartDate != "*")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.AdmissionDate);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(FormatDateTime(searchParameter.StartDate));
                    }

                    if (searchParameter.EndDate != "*")
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.AdmissionDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(FormatDateTime(searchParameter.EndDate));
                    }
                    // Para controlar en Asignación de Cheque Manual
                    if (searchParameter.AccountBankId != 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.AccountBankCode);
                        criteriaBuilder.Equal();
                        criteriaBuilder.Constant(searchParameter.AccountBankId);
                    }
                    if (searchParameter.CheckNumber != null)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.CheckNumber);
                        criteriaBuilder.Equal();
                        criteriaBuilder.Constant(Convert.ToInt32(searchParameter.CheckNumber));
                    }

                    if (searchParameter.Branch.Id > 0)
                    {
                        //******* FILTROS DE COMBOS NUEVOS
                        //FILTRO ESTADO
                        if (searchParameter.StatusId > -1)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.Status, searchParameter.StatusId);
                            if (searchParameter.StatusId == (int)PaymentOrderStatus.Paid)
                            {
                                //para considerarse Estado pagado el cheque debe estar entregado o el medio de 
                                //pago fue por transferencia
                                //lo de transferencia queda pendiente de confirmarse ya que una transferencia 
                                //activa no necesariamente está pagada
                                criteriaBuilder.And();
                                criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.DeliveryDate);
                                criteriaBuilder.IsNotNull();
                            }
                        }

                        //FILTRO CONTABILIZAR (queda pendiente cuando venga de una Transferencia la OP)
                        if (searchParameter.IsAccounting == true)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.CheckNumber);
                            criteriaBuilder.IsNotNull();
                        }
                        else if (searchParameter.IsAccounting == false)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.CheckNumber);
                            criteriaBuilder.IsNull();
                        }

                        //FILTRO ENTREGADO (queda pendiente cuando venga de una Transferencia la OP)
                        if (searchParameter.IsDelivered == true)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.DeliveryDate);
                            criteriaBuilder.IsNotNull();
                        }
                        else if (searchParameter.IsDelivered == false)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.DeliveryDate);
                            criteriaBuilder.IsNull();
                        }

                        //FILTRO CONCILIAR
                        if (searchParameter.IsReconciled == true)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.ReconciliationDate);
                            criteriaBuilder.IsNotNull();
                        }
                        else if (searchParameter.IsReconciled == false)
                        {
                            criteriaBuilder.And();
                            criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.ReconciliationDate);
                            criteriaBuilder.IsNull();
                        }
                    }

                    UIView paymentOrders = _dataFacadeManager.GetDataFacade().GetView("PaymentOrderTransferTypeView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                    #endregion

                    if (paymentOrders.Rows.Count > 0)
                    {
                        paymentOrders.Columns.Add("rows", typeof(int));
                        paymentOrders.Rows[0]["rows"] = paymentOrders.Rows.Count;
                    }

                    #region DTO



                    foreach (DataRow row in paymentOrders)
                    {
                        paymentOrderDTOs.Add(new SearchDTO.PaymentOrderDTO()
                        {
                            PaymentOrderCode = row["PaymentOrderCode"] == DBNull.Value ? "" : row["PaymentOrderCode"].ToString(),
                            PaymentSourceCode = row["PaymentSourceCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PaymentSourceCode"]),
                            PaymentSourceName = row["PaymentSourceName"] == DBNull.Value ? "" : row["PaymentSourceName"].ToString(),
                            IncomeAmount = row["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["IncomeAmount"]),
                            UserId = row["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(row["UserId"]),
                            UserName = row["UserName"] == DBNull.Value ? "" : row["UserName"].ToString(),
                            BranchCode = row["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchCode"]),
                            BranchName = row["BranchName"] == DBNull.Value ? "" : row["BranchName"].ToString(),
                            AccountingDate = row["AccountingDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["AccountingDate"].ToString()).ToString("dd/MM/yyyy"),
                            IndividualId = row["IndividualId"] == DBNull.Value ? "" : row["IndividualId"].ToString(),
                            PayerName = row["PayerName"] == DBNull.Value ? "" : row["PayerName"].ToString(),
                            BeneficiaryName = row["BeneficiaryName"] == DBNull.Value ? "" : row["BeneficiaryName"].ToString(),
                            CurrencyCode = row["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CurrencyCode"]),
                            CurrencyName = row["CurrencyName"] == DBNull.Value ? "" : row["CurrencyName"].ToString(),
                            ExchangeRate = row["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ExchangeRate"]),
                            CheckNumber = row["CheckNumber"] == DBNull.Value ? "" : row["CheckNumber"].ToString(),
                            EstimatedPaymentDate = row["EstimatedPaymentDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["EstimatedPaymentDate"].ToString()).ToString("dd/MM/yyyy"),
                            BranchPayCode = row["BranchPayCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchPayCode"]),
                            BranchPayName = row["BranchPayName"] == DBNull.Value ? "" : row["BranchPayName"].ToString(),
                            Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Amount"]),
                            TempImputationCode = row["TempImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["TempImputationCode"]),
                            CompanyCode = row["CompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CompanyCode"]),
                            AccountBankCode = row["AccountBankCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["AccountBankCode"]),
                            BankAccountNumber = row["BankAccountNumber"] == DBNull.Value ? "" : row["BankAccountNumber"].ToString(),
                            BankName = row["BankName"] == DBNull.Value ? "" : row["BankName"].ToString(),
                            PersonTypeCode = row["PersonTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PersonTypeCode"]),
                            PersonTypeName = row["PersonTypeName"] == DBNull.Value ? "" : row["PersonTypeName"].ToString(),
                            CompanyName = row["CompanyName"] == DBNull.Value ? "" : row["CompanyName"].ToString(),
                            CancellationDate = row["CancellationDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["CancellationDate"].ToString()).ToString("dd/MM/yyyy"),
                            BeneficiaryDocumentNumber = row["BeneficiaryDocumentNumber"] == DBNull.Value ? "" : row["BeneficiaryDocumentNumber"].ToString(),
                            PaymentMethodCode = row["PaymentMethodCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PaymentMethodCode"]),
                            PaymentMethodName = row["PaymentMethodName"] == DBNull.Value ? "" : row["PaymentMethodName"].ToString(),
                            AdmissionDate = row["AdmissionDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["AdmissionDate"].ToString()).ToString("dd/MM/yyyy"),//NO 2G
                            PayTo = row["PayTo"] == DBNull.Value ? "" : row["PayTo"].ToString(),//NO 2G
                            Status = row["Status"] == DBNull.Value ? 0 : Convert.ToInt32(row["Status"]),//NO 2G
                            Observation = row["Observation"] == DBNull.Value ? "" : row["Observation"].ToString(),
                            Rows = paymentOrders.Rows.Count,
                            IndividualTypeId = row["IndividualTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["IndividualTypeCode"]),

                            BankAccountNumberPerson = row["BankAccountNumberPerson"] == DBNull.Value ? "" : Convert.ToString(row["BankAccountNumberPerson"]),
                            BankNamePerson = row["BankAccountNamePerson"] == DBNull.Value ? "" : Convert.ToString(row["BankAccountNamePerson"]),
                        });
                    }

                }
                #endregion

                return paymentOrderDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// CancellationPaymentOrder
        /// Cancela una órden de pago
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public bool CancellationPaymentOrder(int paymentOrderId, int tempImputationId, int userId)
        {
            bool success = false;

            try
            {
                // Actualiza la orden de pago
                PaymentOrder paymentOrder = new PaymentOrder();
                paymentOrder.Id = paymentOrderId;
                paymentOrder = _paymentOrderDAO.GetPaymentOrder(paymentOrder);

                PaymentOrder paymentOrderNew = new PaymentOrder();
                paymentOrderNew.Id = paymentOrder.Id;
                paymentOrderNew.BankAccountPerson = paymentOrder.BankAccountPerson;
                paymentOrderNew.AccountingDate = paymentOrder.AccountingDate;
                paymentOrderNew.Amount = paymentOrder.Amount;
                paymentOrderNew.Beneficiary = paymentOrder.Beneficiary;
                paymentOrderNew.Branch = paymentOrder.Branch;
                paymentOrderNew.BranchPay = paymentOrder.BranchPay;
                paymentOrderNew.CancellationDate = DateTime.Now;
                paymentOrderNew.Company = paymentOrder.Company;
                paymentOrderNew.EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate;
                paymentOrderNew.Imputation = paymentOrder.Imputation;
                paymentOrderNew.IsTemporal = paymentOrder.IsTemporal;
                paymentOrderNew.PaymentDate = paymentOrder.PaymentDate;
                paymentOrderNew.PaymentMethod = paymentOrder.PaymentMethod;
                paymentOrderNew.PaymentSource = paymentOrder.PaymentSource;
                paymentOrderNew.PayTo = paymentOrder.PayTo;
                paymentOrderNew.PersonType = paymentOrder.PersonType;
                paymentOrderNew.Status = Convert.ToInt32(PaymentOrderStatus.Canceled); //anulada
                paymentOrderNew.UserId = userId;
                paymentOrderNew.Observation = paymentOrder.Observation;
                paymentOrderNew.LocalAmount = paymentOrder.LocalAmount;
                paymentOrderNew.ExchangeRate = paymentOrder.ExchangeRate;

                bool isUpdated = _paymentOrderDAO.UpdatePaymentOrder(paymentOrderNew);

                //borra los movimientos relacionados.
                if (isUpdated)
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempPremiumReceivableTrans.Properties.TempApplicationCode, tempImputationId);

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

                success = true;
            }
            catch (BusinessException)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// UpdatePaymentOrder
        /// Actualiza una órden de pago
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>bool</returns>
        public bool UpdatePaymentOrder(PaymentOrderDTO paymentOrder)
        {
            try
            {
                return _paymentOrderDAO.UpdatePaymentOrder(paymentOrder.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentOrder
        /// Obtiene una órden de pago específica
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrderDTO GetPaymentOrder(int paymentOrderId)
        {
            try
            {
                PaymentOrder paymentOrder = new PaymentOrder() { Id = paymentOrderId };

                return _paymentOrderDAO.GetPaymentOrder(paymentOrder).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CheckPaymentOrder

        ///<summary>
        /// SaveCheckPaymentOrderRequest
        /// Graba la asignación de cheques a órdenes de pago
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <param name="checkBookControlId"></param>
        /// <returns>bool</returns>
        public bool SaveCheckPaymentOrderRequest(CheckPaymentOrderDTO checkPaymentOrder, int checkBookControlId)
        {
            bool isUpdated = false;
            bool succeeded = false;
            CoreTransaction.Transaction.Created += delegate (object sender, TransactionEventArgs e)
            {
            };
            using (Context.Current)
            {
                using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    try
                    {

                        bool isCheckBookControlUpdated = false;
                        bool isCheckSaved = false;

                        #region CheckPaymentOrder

                        CheckPaymentOrder checkPaymentOrderNew = _checkPaymentOrderDAO.SaveCheckPaymentOrder(checkPaymentOrder.ToModel());

                        if (checkPaymentOrderNew.Id > 0)
                        {
                            isCheckSaved = true;
                        }

                        #endregion

                        #region PaymentOrderCheckPayment

                        if (isCheckSaved && checkPaymentOrder.PaymentOrders.Count > 0)
                        {
                            foreach (PaymentOrderDTO paymentOrderItem in checkPaymentOrder.PaymentOrders)
                            {
                                bool itemSaved = _checkPaymentOrderTransactionItemDAO.SavePaymentOrderCheckPaymentTransactionItem(checkPaymentOrderNew.Id, paymentOrderItem.Id);

                                if (itemSaved)
                                {
                                    PaymentOrder paymentOrder = new PaymentOrder();
                                    paymentOrder.Id = paymentOrderItem.Id;
                                    paymentOrder = _paymentOrderDAO.GetPaymentOrder(paymentOrder);

                                    PaymentOrder paymentOrderNew = new PaymentOrder();
                                    paymentOrderNew.Id = paymentOrder.Id;
                                    paymentOrderNew.PersonType = paymentOrder.PersonType;
                                    paymentOrderNew.Beneficiary = paymentOrder.Beneficiary;
                                    paymentOrderNew.Branch = paymentOrder.Branch;
                                    paymentOrderNew.Amount = paymentOrder.Amount;
                                    paymentOrderNew.Company = paymentOrder.Company;
                                    paymentOrderNew.BranchPay = paymentOrder.BranchPay;
                                    paymentOrderNew.PaymentSource = paymentOrder.PaymentSource;
                                    paymentOrderNew.PaymentDate = DateTime.Now;
                                    paymentOrderNew.EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate;
                                    paymentOrderNew.PaymentMethod = paymentOrder.PaymentMethod;
                                    paymentOrderNew.PayTo = paymentOrder.PayTo;
                                    paymentOrderNew.BankAccountPerson = new BankAccountPerson();
                                    paymentOrderNew.BankAccountPerson.Id = checkPaymentOrder.BankAccountCompany.Id;
                                    paymentOrderNew.Status = Convert.ToInt32(PaymentOrderStatus.Applied);
                                    paymentOrderNew.Imputation = paymentOrder.Imputation;
                                    paymentOrderNew.IsTemporal = paymentOrder.IsTemporal;
                                    paymentOrderNew.CancellationDate = paymentOrder.CancellationDate;
                                    paymentOrderNew.UserId = paymentOrder.UserId;
                                    paymentOrderNew.AccountingDate = paymentOrder.AccountingDate;
                                    paymentOrderNew.Observation = paymentOrder.Observation;
                                    paymentOrderNew.LocalAmount = paymentOrder.LocalAmount;
                                    paymentOrderNew.ExchangeRate = paymentOrder.ExchangeRate;

                                    isUpdated = _paymentOrderDAO.UpdatePaymentOrder(paymentOrderNew);
                                }
                            }
                            isUpdated = true;
                        }

                        if (isUpdated && isCheckSaved)
                        {
                            CheckBookControl checkBookControl = new CheckBookControl();
                            checkBookControl.Id = checkBookControlId;
                            checkBookControl = _checkBookControlDAO.GetCheckBookControl(checkBookControl);

                            CheckBookControl checkBookControlNew = new CheckBookControl();
                            checkBookControlNew.Id = checkBookControl.Id;
                            checkBookControlNew.AccountBank = checkBookControl.AccountBank;
                            checkBookControlNew.IsAutomatic = checkBookControl.IsAutomatic;
                            checkBookControlNew.CheckFrom = checkBookControl.CheckFrom;
                            checkBookControlNew.CheckTo = checkBookControl.CheckTo;
                            checkBookControlNew.LastCheck = checkPaymentOrder.CheckNumber;

                            if (checkPaymentOrder.CheckNumber < checkBookControl.CheckTo)
                            {
                                checkBookControlNew.Status = checkBookControl.Status;
                                checkBookControlNew.DisabledDate = null;
                            }
                            if (checkPaymentOrder.CheckNumber >= checkBookControl.CheckTo)
                            {
                                checkBookControlNew.Status = 0;
                                checkBookControlNew.DisabledDate = DateTime.Now;
                            }

                            CheckBookControl checkBookControlOut = _checkBookControlDAO.UpdateCheckBookControl(checkBookControlNew);

                            if (checkBookControlOut.Id > 0)
                            {
                                isCheckBookControlUpdated = true;
                            }
                            succeeded = isCheckBookControlUpdated;
                        }

                        #endregion

                        //LLAMA A LA GRABACIÓN DE LA IMPUTACIÓN
                        DelegateService.accountingApplicationService.SavePaymentOrderApplication(checkPaymentOrder.PaymentOrders[0].Id, Convert.ToInt32(ApplicationTypes.PaymentOrder), checkPaymentOrder.CancellationUser);

                        transaction.Complete();
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Dispose();

                        throw new BusinessException(Resources.Resources.BusinessException);
                    }
                }
                return succeeded;
            }
        }

        ///<summary>
        /// UpdateCheckPaymentOrder
        /// Actualiza la orden de pago en la entrega de cheques
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>bool</returns>
        public bool UpdateCheckPaymentOrder(CheckPaymentOrderDTO checkPaymentOrder)
        {
            CoreTransaction.Transaction.Created += delegate (object sender, TransactionEventArgs e)
            {
            };
            using (Context.Current)
            {
                using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    try
                    {
                        bool success = false;

                        CheckPaymentOrder newCheckPaymentOrder;
                        if (checkPaymentOrder.DeliveryDate == new DateTime() && checkPaymentOrder.CourierName == null &&
                            checkPaymentOrder.RefundDate == new DateTime() && checkPaymentOrder.CancellationDate == new DateTime())
                        {
                            #region CheckPaymentOrder

                            newCheckPaymentOrder = _checkPaymentOrderDAO.GetCheckPaymentOrder(checkPaymentOrder.ToModel());
                            if (newCheckPaymentOrder.Id > 0)
                            {
                                CheckPaymentOrder checkPaymentOrderOut = new CheckPaymentOrder();
                                checkPaymentOrderOut.BankAccountCompany = newCheckPaymentOrder.BankAccountCompany;
                                checkPaymentOrderOut.CheckNumber = newCheckPaymentOrder.CheckNumber;
                                checkPaymentOrderOut.Delivery = newCheckPaymentOrder.Delivery;
                                if (newCheckPaymentOrder.DeliveryDate == new DateTime())
                                {
                                    newCheckPaymentOrder.DeliveryDate = null;
                                }
                                if (newCheckPaymentOrder.RefundDate == new DateTime())
                                {
                                    newCheckPaymentOrder.RefundDate = null;
                                }
                                if (newCheckPaymentOrder.CancellationDate == new DateTime())
                                {
                                    newCheckPaymentOrder.CancellationDate = null;
                                }

                                checkPaymentOrderOut.CancellationDate = newCheckPaymentOrder.CancellationDate;
                                checkPaymentOrderOut.RefundDate = newCheckPaymentOrder.RefundDate;
                                checkPaymentOrderOut.CourierName = newCheckPaymentOrder.CourierName;
                                checkPaymentOrderOut.DeliveryDate = newCheckPaymentOrder.DeliveryDate;
                                checkPaymentOrderOut.Id = newCheckPaymentOrder.Id;
                                checkPaymentOrderOut.IsCheckPrinted = checkPaymentOrder.IsCheckPrinted;
                                checkPaymentOrderOut.PaymentOrders = newCheckPaymentOrder.PaymentOrders;
                                checkPaymentOrderOut.PersonType = newCheckPaymentOrder.PersonType;
                                checkPaymentOrderOut.PrintedDate = checkPaymentOrder.PrintedDate;
                                checkPaymentOrderOut.PrintedUser = checkPaymentOrder.PrintedUser;
                                checkPaymentOrderOut.CancellationUser = newCheckPaymentOrder.CancellationUser;
                                checkPaymentOrderOut.Status = checkPaymentOrder.Status;
                                checkPaymentOrder = _checkPaymentOrderDAO.UpdateCheckPaymentOrder(checkPaymentOrderOut).ToDTO();

                                if (checkPaymentOrder.Id > 0)
                                {
                                    success = true;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region CheckPaymentOrder

                            newCheckPaymentOrder = _checkPaymentOrderDAO.GetCheckPaymentOrder(checkPaymentOrder.ToModel());
                            if (newCheckPaymentOrder.Id > 0)
                            {
                                CheckPaymentOrder checkPaymentOrderOut = new CheckPaymentOrder();
                                checkPaymentOrderOut.BankAccountCompany = newCheckPaymentOrder.BankAccountCompany;
                                checkPaymentOrderOut.CheckNumber = newCheckPaymentOrder.CheckNumber;
                                checkPaymentOrderOut.Delivery = newCheckPaymentOrder.Delivery;
                                if (checkPaymentOrder.DeliveryDate == new DateTime())
                                {
                                    checkPaymentOrder.DeliveryDate = null;
                                }
                                if (checkPaymentOrder.RefundDate == new DateTime())
                                {
                                    checkPaymentOrder.RefundDate = null;
                                }
                                if (checkPaymentOrder.CancellationDate == new DateTime())
                                {
                                    checkPaymentOrder.CancellationDate = null;
                                }
                                checkPaymentOrderOut.CancellationDate = checkPaymentOrder.CancellationDate;
                                checkPaymentOrderOut.RefundDate = checkPaymentOrder.RefundDate;
                                checkPaymentOrderOut.CourierName = checkPaymentOrder.CourierName;
                                checkPaymentOrderOut.DeliveryDate = checkPaymentOrder.DeliveryDate;
                                checkPaymentOrderOut.Id = newCheckPaymentOrder.Id;
                                checkPaymentOrderOut.IsCheckPrinted = newCheckPaymentOrder.IsCheckPrinted;
                                checkPaymentOrderOut.PaymentOrders = newCheckPaymentOrder.PaymentOrders;
                                checkPaymentOrderOut.PersonType = checkPaymentOrder.PersonType.ToModel();

                                if (newCheckPaymentOrder.PrintedDate.ToString() != "01/01/0001 0:00:00")
                                {
                                    checkPaymentOrderOut.PrintedDate = newCheckPaymentOrder.PrintedDate;
                                }
                                else
                                {
                                    checkPaymentOrderOut.PrintedDate = null;
                                }
                                checkPaymentOrderOut.PrintedUser = newCheckPaymentOrder.PrintedUser;
                                checkPaymentOrderOut.CancellationUser = newCheckPaymentOrder.CancellationUser;
                                checkPaymentOrderOut.Status = checkPaymentOrder.Status;
                                checkPaymentOrder = _checkPaymentOrderDAO.UpdateCheckPaymentOrder(checkPaymentOrderOut).ToDTO();
                                if (checkPaymentOrder.Id > 0)
                                {
                                    success = true;
                                }
                            }

                            #endregion
                        }

                        transaction.Complete();
                        return success;
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Dispose();

                        throw new BusinessException(Resources.Resources.BusinessException);
                    }
                }
            }
        }

        #endregion

        #region PrintCheck

        /// <summary>
        /// GetPrintCheck
        /// Obtiene cheques a imprimir
        /// </summary>
        /// <param name="searchParameterCheckPaymentOrder"></param>
        /// <returns>List<PrintCheckDTO/></returns>
        public List<SearchDTO.PrintCheckDTO> GetPrintCheck(SearchDTO.SearchParameterCheckPaymentOrderDTO searchParameterCheckPaymentOrder)
        {
            int rows;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.AccountBankCode, searchParameterCheckPaymentOrder.BankAccountCompany.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.IsCheckPrinted, searchParameterCheckPaymentOrder.IsPrinted);

                if (searchParameterCheckPaymentOrder.PaymentSource.Id != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.PaymentSourceCode, searchParameterCheckPaymentOrder.PaymentSource.Id);
                }

                if (searchParameterCheckPaymentOrder.EstimatedPaymentDate != new DateTime())
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.EstimatedPaymentDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(searchParameterCheckPaymentOrder.EstimatedPaymentDate);
                }
                if (searchParameterCheckPaymentOrder.NumberCheck != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.CheckNumber, searchParameterCheckPaymentOrder.NumberCheck);
                }
                if (searchParameterCheckPaymentOrder.CheckFrom != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.CheckNumber);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(searchParameterCheckPaymentOrder.CheckFrom);
                }
                if (searchParameterCheckPaymentOrder.CheckTo != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.CheckNumber);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(searchParameterCheckPaymentOrder.CheckTo);
                }
                if (searchParameterCheckPaymentOrder.DeliveryType != -1)
                {
                    if (searchParameterCheckPaymentOrder.DeliveryType == 3)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.DeliveryDate);
                        criteriaBuilder.IsNotNull();
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.CourierName);
                        criteriaBuilder.IsNotNull();
                    }
                    if (searchParameterCheckPaymentOrder.DeliveryType == 2)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.DeliveryDate);
                        criteriaBuilder.IsNotNull();
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.CourierName);
                        criteriaBuilder.IsNull();
                    }
                    if (searchParameterCheckPaymentOrder.DeliveryType == 1)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderChecks.Properties.DeliveryDate);
                        criteriaBuilder.IsNull();
                    }
                }
                if (searchParameterCheckPaymentOrder.PaymentOrderNumber != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.NumberPaymentOrder,
                                          searchParameterCheckPaymentOrder.PaymentOrderNumber);
                }
                if (searchParameterCheckPaymentOrder.Amount != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.Amount, searchParameterCheckPaymentOrder.Amount);
                }
                if (searchParameterCheckPaymentOrder.BeneficiaryFullName != null)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderChecks.Properties.PayTo, searchParameterCheckPaymentOrder.BeneficiaryFullName);
                }

                UIView printChecks = _dataFacadeManager.GetDataFacade().GetView("PaymentOrderChecksView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                #endregion

                #region DTO

                List<SearchDTO.PrintCheckDTO> printCheckDTOs = new List<SearchDTO.PrintCheckDTO>();

                if (printChecks.Rows.Count > 0)
                {
                    foreach (DataRow row in printChecks)
                    {
                        printCheckDTOs.Add(new SearchDTO.PrintCheckDTO()
                        {
                            AddressCompany = row["AddressCompany"] == DBNull.Value ? "" : Convert.ToString(row["AddressCompany"]),
                            Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Amount"]),
                            BeneficiaryName = row["BeneficiaryName"] == DBNull.Value ? "" : Convert.ToString(row["BeneficiaryName"]),
                            BranchCode = row["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchCode"]),
                            CheckNumber = row["CheckNumber"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckNumber"]),
                            CompanyName = row["CompanyName"] == DBNull.Value ? "" : Convert.ToString(row["CompanyName"]),
                            EstimatedPaymentDate = row["EstimatedPaymentDate"] == DBNull.Value ? "" : ((DateTime)row["EstimatedPaymentDate"]).ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat),
                            NumberPaymentOrder = row["NumberPaymentOrder"] == DBNull.Value ? 0 : Convert.ToInt32(row["NumberPaymentOrder"]),
                            PaymentSourceCode = row["PaymentSourceCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PaymentSourceCode"]),
                            AccountBankId = row["AccountBankCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["AccountBankCode"]),
                            CheckPaymentOrderCode = row["CheckPaymentOrderCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckPaymentOrderCode"]),
                            CourierName = row["CourierName"] == DBNull.Value ? "" : Convert.ToString(row["CourierName"]),
                            DeliveryDate = row["DeliveryDate"] == DBNull.Value ? "" : ((DateTime)row["DeliveryDate"]).ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat),
                            RefundDate = row["RefundDate"] == DBNull.Value ? "" : ((DateTime)row["RefundDate"]).ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat),
                            DescriptionCity = ""
                        });
                    }
                }

                #endregion

                return printCheckDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Transfer

        /// <summary>
        /// SearchTransferPaymentOrders
        /// Trae las órdenes de pago con tipo de pago transferencia
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<PaymentOrderDTO/></returns>
        public List<SearchDTO.PaymentOrderDTO> SearchTransferPaymentOrders(SearchDTO.SearchParameterPaymentOrdersDTO searchParameter)
        {
            int rows;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                //Controla cuando se envía fecha nula
                if (searchParameter.PaymentDate != Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.PaymentMethodCode, searchParameter.PaymentMethod.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentOrderTransferType.Properties.EstimatedPaymentDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(searchParameter.PaymentDate);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.Status, searchParameter.StatusId);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferType.Properties.PaymentOrderCode, -1);
                }

                UIView paymentOrders = _dataFacadeManager.GetDataFacade().GetView("PaymentOrderTransferTypeView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                #endregion

                if (paymentOrders.Rows.Count > 0)
                {
                    paymentOrders.Columns.Add("rows", typeof(int));
                    paymentOrders.Rows[0]["rows"] = paymentOrders.Rows.Count;
                }

                #region DTO

                List<SearchDTO.PaymentOrderDTO> paymentOrderDTOs = new List<SearchDTO.PaymentOrderDTO>();

                foreach (DataRow row in paymentOrders)
                {
                    paymentOrderDTOs.Add(new SearchDTO.PaymentOrderDTO()
                    {
                        AccountBankCode = row["AccountBankCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["AccountBankCode"]),
                        AccountingDate = row["AccountingDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["AccountingDate"].ToString()).ToString("dd/MM/yyyy"),
                        AdmissionDate = row["AdmissionDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["AdmissionDate"].ToString()).ToString("dd/MM/yyyy"), //register date
                        Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Amount"]),
                        BankAccountNumber = row["BankAccountNumber"] == DBNull.Value ? "" : row["BankAccountNumber"].ToString(),
                        BankName = row["BankName"] == DBNull.Value ? "" : row["BankName"].ToString(),
                        BeneficiaryDocumentNumber = row["BeneficiaryDocumentNumber"] == DBNull.Value ? "" : row["BeneficiaryDocumentNumber"].ToString(),
                        BeneficiaryName = row["BeneficiaryName"] == DBNull.Value ? "" : row["BeneficiaryName"].ToString(),
                        BranchCode = row["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchCode"]),
                        BranchName = row["BranchName"] == DBNull.Value ? "" : row["BranchName"].ToString(),
                        BranchPayCode = row["BranchPayCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchPayCode"]),
                        BranchPayName = row["BranchPayName"] == DBNull.Value ? "" : row["BranchPayName"].ToString(),
                        CancellationDate = row["CancellationDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["CancellationDate"].ToString()).ToString("dd/MM/yyyy"),
                        CheckNumber = row["CheckNumber"] == DBNull.Value ? "" : row["CheckNumber"].ToString(),
                        CompanyCode = row["CompanyCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CompanyCode"]),
                        CompanyName = row["CompanyName"] == DBNull.Value ? "" : row["CompanyName"].ToString(),
                        CurrencyCode = row["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CurrencyCode"]),
                        CurrencyName = row["CurrencyName"] == DBNull.Value ? "" : row["CurrencyName"].ToString(),
                        EstimatedPaymentDate = row["EstimatedPaymentDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["EstimatedPaymentDate"].ToString()).ToString("dd/MM/yyyy"),
                        ExchangeRate = row["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ExchangeRate"]),
                        PayerName = row["PayerName"] == DBNull.Value ? "" : row["PayerName"].ToString(),
                        IncomeAmount = row["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["IncomeAmount"]),
                        IndividualId = row["IndividualId"] == DBNull.Value ? "" : row["IndividualId"].ToString(),
                        PaymentMethodCode = row["PaymentMethodCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PaymentMethodCode"]),
                        PaymentMethodName = row["PaymentMethodName"] == DBNull.Value ? "" : row["PaymentMethodName"].ToString(),
                        PaymentOrderCode = row["PaymentOrderCode"] == DBNull.Value ? "" : row["PaymentOrderCode"].ToString(),
                        PaymentSourceCode = row["PaymentSourceCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PaymentSourceCode"]),
                        PaymentSourceName = row["PaymentSourceName"] == DBNull.Value ? "" : row["PaymentSourceName"].ToString(),
                        PayTo = row["PayTo"] == DBNull.Value ? "" : row["PayTo"].ToString(),
                        PersonTypeCode = row["PersonTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["PersonTypeCode"]),
                        PersonTypeName = row["PersonTypeName"] == DBNull.Value ? "" : row["PersonTypeName"].ToString(),
                        Status = row["Status"] == DBNull.Value ? 0 : Convert.ToInt32(row["Status"]),
                        StatusDescription = EnumHelper.GetEnumDescription((PaymentOrderStatus)Convert.ToInt32(row["Status"])),
                        TempImputationCode = row["TempImputationCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["TempImputationCode"]),
                        UserId = row["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(row["UserId"]),
                        UserName = row["UserName"] == DBNull.Value ? "" : row["UserName"].ToString(),
                        BankAccountNumberPerson = row["BankAccountNumberPerson"] == DBNull.Value ? "" : Convert.ToString(row["BankAccountNumberPerson"]),
                        BankNamePerson = row["BankAccountNamePerson"] == DBNull.Value ? "" : Convert.ToString(row["BankAccountNamePerson"]),
                        Rows = paymentOrders.Rows.Count
                    });
                }

                #endregion

                return paymentOrderDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region OtherPaymentRequest

        /// <summary>
        /// OtherPaymentRequestReport
        /// Reporte para solicitud de pagos varios
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns>OtherPaymentsRequestReportHeaderDTO</returns>
        public SearchDTO.OtherPaymentsRequestReportHeaderDTO OtherPaymentRequestReport(int paymentRequestId)
        {
            int rows;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                //query para la cabecera
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestHeader.Properties.PaymentRequestId, paymentRequestId);

                UIView paymentRequests = _dataFacadeManager.GetDataFacade().GetView("PaymentRequestHeaderView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                //query para los detalles
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentRequestDetail.Properties.PaymentRequestCode, paymentRequestId);

                UIView dataDetails = _dataFacadeManager.GetDataFacade().GetView("PaymentRequestDetailView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                #endregion

                #region DTO

                SearchDTO.OtherPaymentsRequestReportHeaderDTO report = new SearchDTO.OtherPaymentsRequestReportHeaderDTO();

                if (paymentRequests.Rows.Count > 0)
                {
                    foreach (DataRow row in paymentRequests.Rows)
                    {
                        report.PaymentRequestId = Convert.ToInt32(row["PaymentRequestId"]); //PaymentRequestCode
                        report.Number = Convert.ToInt32(row["Number"]);
                        report.EstimatedDate = String.Format("{0:dd/MM/yyyy}", row["EstimatedDate"]);
                        report.PersonTypeId = Convert.ToInt32(row["PersonTypeCode"]);
                        report.PersonTypeDescription = Convert.ToString(row["PersonTypeDescription"]);
                        report.IndividualId = Convert.ToInt32(row["IndividualId"]);
                        report.DocumentNumber = Convert.ToInt32(row["DocumentNumber"]).ToString();
                        report.Name = Convert.ToString(row["Name"]);
                        report.CurrencyId = Convert.ToInt32(row["CurrencyCode"]);
                        report.CurrencyDescription = Convert.ToString(row["CurrencyDescription"]);
                        report.RegistrationDate = String.Format("{0:dd/MM/yyyy}", row["RegistrationDate"]);
                        report.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                        report.UserId = Convert.ToInt32(row["UserId"]);
                        report.UserAccountName = Convert.ToString(row["AccountName"]);
                        report.PaymentMethodId = Convert.ToInt32(row["PaymentMethodCode"]);
                        report.PaymentMethodDescription = Convert.ToString(row["PaymentMethodTypeDescription"]);
                        report.PaymentRequestDescription = Convert.ToString(row["PaymentRequestDescription"]);
                        report.CollectId = Convert.ToInt32(row["CollectCode"]);
                    }

                    if (dataDetails.Rows.Count > 0)
                    {
                        report.OtherPaymentRequestReportDetails = new List<SearchDTO.OtherPaymentRequestReportDetails>();

                        foreach (DataRow detailRow in dataDetails)
                        {
                            report.OtherPaymentRequestReportDetails.Add(new SearchDTO.OtherPaymentRequestReportDetails()
                            {
                                VoucherTypeId = Convert.ToInt32(detailRow["VoucherTypeCode"]),
                                VoucherTypeDescription = Convert.ToString(detailRow["VoucherDescription"]),
                                VoucherNumber = Convert.ToString(detailRow["Number"]),
                                TotalAmount = Convert.ToDecimal(detailRow["Amount"]),
                                Taxes = Convert.ToDecimal(detailRow["Taxes"]),
                                Retentions = Convert.ToDecimal(detailRow["Retentions"])
                            });
                        }

                    }
                }
                #endregion

                return report;
            }

            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCostCenterByAccountingAccountId
        /// Obtiene los centros de gastos asociados a una cuenta contable parametrizados para un concepto
        /// de pago
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>List<CostCenterDTO/></returns>
        public List<SearchDTO.CostCenterDTO> GetCostCenterByAccountingAccountId(int accountingAccountId)
        {
            List<SearchDTO.CostCenterDTO> costCenters = new List<SearchDTO.CostCenterDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetCostCenter.Properties.AccountingAccountId, accountingAccountId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.GetCostCenter), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetCostCenter costCenterCollection in businessCollection.OfType<ACCOUNTINGEN.GetCostCenter>())
                    {
                        SearchDTO.CostCenterDTO costCenter = new SearchDTO.CostCenterDTO();
                        costCenter.CostCenterId = costCenterCollection.CostCenterId;
                        costCenter.Description = costCenterCollection.Description;
                        costCenters.Add(costCenter);
                    }
                }
            }
            catch (BusinessException)
            {
                costCenters = null;
            }

            return costCenters;
        }

        /// <summary>
        /// SavePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        public PaymentRequestDTO SavePaymentRequest(PaymentRequestDTO paymentRequest)
        {
            PaymentRequest newPaymentRequest = new PaymentRequest();

            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {
                    //Se graba la cabecera de la solicitud.
                    if (paymentRequest.Id == 0)
                    {
                        //Se obtiene el número de solicitud.
                        PaymentRequestNumber paymentRequestNumber = new PaymentRequestNumber();
                        paymentRequestNumber.Branch = new Branch();
                        paymentRequestNumber.Branch.Id = paymentRequest.Branch.Id;
                        paymentRequestNumber = _paymentRequestNumberDAO.GetPaymentRequestNumber(paymentRequestNumber);

                        if (paymentRequestNumber == null)
                        {
                            paymentRequestNumber = new PaymentRequestNumber();
                            paymentRequestNumber.Branch = new Branch();
                            paymentRequestNumber.Branch.Id = paymentRequest.Branch.Id;

                            paymentRequestNumber.Number = 1;
                            paymentRequestNumber = _paymentRequestNumberDAO.SavePaymentRequestNumber(paymentRequestNumber);
                        }


                        //Se realiza la grabación
                        paymentRequest.PaymentRequestNumber = paymentRequestNumber.ToDTO();
                        newPaymentRequest = _paymentRequestDAO.SavePaymentRequest(paymentRequest.ToModel());

                        //Se actualiza el número de solicitud
                        paymentRequestNumber.Number = paymentRequestNumber.Number + 1;
                        _paymentRequestNumberDAO.UpdatePaymentRequestNumber(paymentRequestNumber);
                    }

                    //Se actualiza la cabecera de la solicitud.
                    if (paymentRequest.Id > 0)
                    {
                        newPaymentRequest = _paymentRequestDAO.UpdatePaymentRequest(paymentRequest.ToModel());

                        //Elimina todos los movimientos asociados, para agregar los movimientos actualizados.

                        //Se obtienen los vouchers
                        List<Voucher> vouchers = GetVouchersByPaymentRequestId(paymentRequest.Id).ToModels().ToList();

                        if (vouchers.Count > 0)
                        {
                            foreach (Voucher voucher in vouchers)
                            {
                                //obtiene los conceptos asociados al voucher
                                List<VoucherConcept> voucherConcepts = GetVoucherConcepsByVoucherId(voucher.Id).ToModels().ToList();

                                if (voucherConcepts.Count > 0)
                                {
                                    foreach (VoucherConcept voucherConcept in voucherConcepts)
                                    {
                                        //obtiene los impuestos de cada conceptos
                                        List<VoucherConceptTax> voucherConceptTaxes = GetVoucherConcepTaxesByVoucherConceptId(voucherConcept.Id).ToModels().ToList();

                                        if (voucherConceptTaxes.Count > 0)
                                        {
                                            foreach (VoucherConceptTax voucherConceptTax in voucherConceptTaxes)
                                            {
                                                //se eliminan los impuestos.
                                                _voucherConceptTaxDAO.DeleteVoucherConceptTax(voucherConceptTax);
                                            }
                                        }

                                        //se eliminan los conceptos.
                                        _voucherConceptDAO.DeleteVoucherConcept(voucherConcept);
                                    }
                                }

                                //se eliminan los vouchers
                                _voucherDAO.DeleteVoucher(voucher);
                            }
                        }
                    }

                    //se graban los voucher asociados.
                    if (paymentRequest.Vouchers.Count > 0)
                    {
                        foreach (Voucher voucher in paymentRequest.Vouchers.ToModels())
                        {
                            voucher.Id = 0; //es un nuevo registro
                            VoucherDTO newVoucher = DTOAssembler.ToDTO(_voucherDAO.SaveVoucher(voucher, newPaymentRequest.Id));

                            //se graban los conceptos asociados al voucher
                            if (voucher.VoucherConcepts.Count > 0)
                            {
                                foreach (VoucherConcept voucherConcept in voucher.VoucherConcepts)
                                {
                                    voucherConcept.Id = 0; //es un nuevo registro
                                    VoucherConcept newVoucherConcept = (_voucherConceptDAO.SaveVoucherConcept(voucherConcept, newVoucher.Id));

                                    if (voucherConcept.VoucherConceptTaxes.Count > 0)
                                    {
                                        foreach (VoucherConceptTax voucherConceptTax in voucherConcept.VoucherConceptTaxes)
                                        {
                                            voucherConceptTax.Id = 0; //es un nuevo registro
                                            _voucherConceptTaxDAO.SaveVoucherConceptTax(voucherConceptTax, newVoucherConcept.Id);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    transaction.Complete();

                    return newPaymentRequest.ToDTO();
                }
                catch (BusinessException exception)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        /// <summary>
        /// UpdatePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        public PaymentRequestDTO UpdatePaymentRequest(PaymentRequestDTO paymentRequest)
        {
            try
            {
                return _paymentRequestDAO.UpdatePaymentRequest(paymentRequest.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        public PaymentRequestDTO GetPaymentRequest(PaymentRequestDTO paymentRequest)
        {
            try
            {
                //se obtiene la cabecera de la solicitud.
                PaymentRequest newPaymentRequest = _paymentRequestDAO.GetPaymentRequest(paymentRequest.ToModel());

                //se obtienes los vouchers
                newPaymentRequest.Vouchers = new List<Voucher>();
                newPaymentRequest.Vouchers = GetVouchersByPaymentRequestId(newPaymentRequest.Id).ToModels().ToList();

                //se obtienen los conceptos
                if (newPaymentRequest.Vouchers.Count > 0)
                {
                    foreach (Voucher voucher in newPaymentRequest.Vouchers)
                    {
                        voucher.VoucherConcepts = new List<VoucherConcept>();
                        voucher.VoucherConcepts = GetVoucherConcepsByVoucherId(voucher.Id).ToModels().ToList();

                        //se obtienes los impuestos.
                        if (voucher.VoucherConcepts.Count > 0)
                        {
                            foreach (VoucherConcept voucherConcept in voucher.VoucherConcepts)
                            {
                                voucherConcept.VoucherConceptTaxes = new List<VoucherConceptTax>();
                                voucherConcept.VoucherConceptTaxes = GetVoucherConcepTaxesByVoucherConceptId(voucherConcept.Id).ToModels().ToList();
                            }
                        }
                    }
                }

                return newPaymentRequest.ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetPaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns>PaymentRequestNumber</returns>
        public PaymentRequestNumberDTO GetPaymentRequestNumber(PaymentRequestNumberDTO paymentRequestNumber)
        {
            try
            {
                return _paymentRequestNumberDAO.GetPaymentRequestNumber(paymentRequestNumber.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion OtherPaymentRequest

        #region CancelCheck

        /// <summary>
        /// GetCancelCheckPaymentOrder
        /// Obtiene un cheque cancelado específico
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="checkNumber"></param>
        /// <returns>List<CancelCheckDTO/></returns>
        public List<SearchDTO.CancelCheckDTO> GetCancelCheckPaymentOrder(int accountBankId, int checkNumber)
        {
            int rows;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region UiView

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CancelCheckPaymentOrder.Properties.AccountBankCode, accountBankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CancelCheckPaymentOrder.Properties.CheckNumber, checkNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CancelCheckPaymentOrder.Properties.PaymetOrderStatus);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(Convert.ToInt16(PaymentOrderStatus.Active));

                UIView cancelChecks = _dataFacadeManager.GetDataFacade().GetView("CancelCheckPaymentOrderView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                #endregion

                #region DTO

                List<SearchDTO.CancelCheckDTO> cancelCheckDTOs = new List<SearchDTO.CancelCheckDTO>();

                if (cancelChecks.Rows.Count > 0)
                {
                    foreach (DataRow row in cancelChecks)
                    {
                        cancelCheckDTOs.Add(new SearchDTO.CancelCheckDTO()
                        {
                            AccountBankId = Convert.ToInt32(row["AccountBankCode"]),
                            Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Amount"]),
                            CheckNumber = row["CheckNumber"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckNumber"]),
                            CheckPaymentOrderCode = row["CheckPaymentOrderCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["CheckPaymentOrderCode"]),
                            CheckPaymentOrderStatus = row["CheckPaymentOrderStatus"] == DBNull.Value ? -1 : Convert.ToInt32(row["CheckPaymentOrderStatus"]),
                            ExchangeRate = row["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ExchangeRate"]),
                            IncomeAmount = row["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["IncomeAmount"]),
                            IndividualId = row["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(row["IndividualId"]),
                            IsCheckPrinted = row["IsCheckPrinted"] == DBNull.Value ? -1 : Convert.ToInt32(row["IsCheckPrinted"]),
                            PaymentDate = row["PaymentDate"] == DBNull.Value ? "" : ((DateTime)row["PaymentDate"]).ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat),
                            PaymentOrderCode = row["PaymentOrderCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["PaymentOrderCode"]),
                            PaymetOrderStatus = row["PaymetOrderStatus"] == DBNull.Value ? -1 : Convert.ToInt32(row["PaymetOrderStatus"]),
                            PayTo = row["PayTo"] == DBNull.Value ? "" : Convert.ToString(row["PayTo"]),
                            TempImputationCode = row["TempImputationCode"] == DBNull.Value ? -1 : Convert.ToInt32(row["TempImputationCode"]),
                            PaymetOrderStatusDescription = Convert.ToInt32(row["PaymetOrderStatus"]) == Convert.ToInt16(PaymentOrderStatus.Applied) ? "Contabilizado" : "No Contabilizado"
                        });
                    }
                }

                #endregion

                return cancelCheckDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// SaveCancelCheckRequest
        /// Graba una cancelación de cheque
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>bool</returns>
        public bool SaveCancelCheckRequest(CheckPaymentOrderDTO checkPaymentOrder)
        {
            CoreTransaction.Transaction.Created += delegate (object sender, TransactionEventArgs e)
            {
            };
            using (Context.Current)
            {
                using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    try
                    {
                        bool checkPaymentOrderUpdated = false;
                        bool cancelationSucceeded = false;

                        #region CheckPaymentOrder

                        CheckPaymentOrder newCheckPaymentOrder = _checkPaymentOrderDAO.GetCheckPaymentOrder(checkPaymentOrder.ToModel());

                        if (newCheckPaymentOrder.Id > 0)
                        {
                            CheckPaymentOrder checkPaymentOrderOut = new CheckPaymentOrder();
                            if (newCheckPaymentOrder.DeliveryDate == new DateTime())
                            {
                                newCheckPaymentOrder.DeliveryDate = null;
                            }
                            if (newCheckPaymentOrder.RefundDate == new DateTime())
                            {
                                newCheckPaymentOrder.RefundDate = null;
                            }
                            if (checkPaymentOrder.CancellationDate == new DateTime())
                            {
                                checkPaymentOrder.CancellationDate = null;
                            }
                            if (newCheckPaymentOrder.PrintedDate == new DateTime())
                            {
                                newCheckPaymentOrder.PrintedDate = null;
                            }
                            checkPaymentOrderOut.Id = newCheckPaymentOrder.Id;
                            checkPaymentOrderOut.BankAccountCompany = newCheckPaymentOrder.BankAccountCompany;
                            checkPaymentOrderOut.CheckNumber = newCheckPaymentOrder.CheckNumber;
                            checkPaymentOrderOut.CourierName = newCheckPaymentOrder.CourierName;
                            checkPaymentOrderOut.Delivery = newCheckPaymentOrder.Delivery;
                            checkPaymentOrderOut.DeliveryDate = newCheckPaymentOrder.DeliveryDate;
                            checkPaymentOrderOut.IsCheckPrinted = checkPaymentOrder.IsCheckPrinted;
                            checkPaymentOrderOut.PersonType = newCheckPaymentOrder.PersonType;
                            checkPaymentOrderOut.PrintedDate = newCheckPaymentOrder.PrintedDate;
                            checkPaymentOrderOut.PrintedUser = newCheckPaymentOrder.PrintedUser;
                            checkPaymentOrderOut.RefundDate = newCheckPaymentOrder.RefundDate;
                            checkPaymentOrderOut.CancellationDate = checkPaymentOrder.CancellationDate;
                            checkPaymentOrderOut.CancellationUser = checkPaymentOrder.CancellationUser;
                            checkPaymentOrderOut.Status = checkPaymentOrder.Status;
                            newCheckPaymentOrder = _checkPaymentOrderDAO.UpdateCheckPaymentOrder(checkPaymentOrderOut);
                            if (newCheckPaymentOrder.Id > 0)
                            {
                                checkPaymentOrderUpdated = true;
                            }
                        }

                        if (checkPaymentOrderUpdated)
                        {
                            if (checkPaymentOrder.PaymentOrders.Count > 0)
                            {
                                foreach (PaymentOrder paymentOrderItem in checkPaymentOrder.PaymentOrders.ToModels())
                                {
                                    PaymentOrder paymentOrder = new PaymentOrder();
                                    paymentOrder.Id = paymentOrderItem.Id;
                                    paymentOrder = _paymentOrderDAO.GetPaymentOrder(paymentOrder);

                                    PaymentOrder newPaymentOrder = new PaymentOrder();
                                    newPaymentOrder.Id = paymentOrder.Id;
                                    newPaymentOrder.PersonType = paymentOrder.PersonType;
                                    newPaymentOrder.Beneficiary = paymentOrder.Beneficiary;
                                    newPaymentOrder.Branch = paymentOrder.Branch;
                                    newPaymentOrder.Amount = paymentOrderItem.Amount;
                                    newPaymentOrder.Company = paymentOrder.Company;
                                    newPaymentOrder.BranchPay = paymentOrder.BranchPay;
                                    newPaymentOrder.PaymentSource = paymentOrder.PaymentSource;
                                    newPaymentOrder.PaymentDate = paymentOrderItem.PaymentDate;
                                    newPaymentOrder.EstimatedPaymentDate = paymentOrder.EstimatedPaymentDate;
                                    newPaymentOrder.PaymentMethod = paymentOrder.PaymentMethod;
                                    newPaymentOrder.PayTo = paymentOrder.PayTo;
                                    newPaymentOrder.BankAccountPerson = paymentOrderItem.BankAccountPerson;
                                    newPaymentOrder.Status = paymentOrder.Status;
                                    newPaymentOrder.Imputation = paymentOrder.Imputation;
                                    newPaymentOrder.IsTemporal = paymentOrder.IsTemporal;
                                    newPaymentOrder.CancellationDate = paymentOrder.AccountingDate;
                                    newPaymentOrder.UserId = paymentOrder.UserId;
                                    newPaymentOrder.AccountingDate = paymentOrder.AccountingDate;
                                    newPaymentOrder.Observation = paymentOrder.Observation;
                                    newPaymentOrder.LocalAmount = paymentOrder.LocalAmount;
                                    newPaymentOrder.ExchangeRate = paymentOrder.ExchangeRate;

                                    var resultPaymentOrder = _paymentOrderDAO.UpdatePaymentOrder(newPaymentOrder);

                                    if (resultPaymentOrder)
                                    {
                                        cancelationSucceeded = CancellationPaymentOrder(newPaymentOrder.Id, paymentOrderItem.Imputation.Id, paymentOrderItem.UserId);
                                    }
                                }
                            }
                        }

                        #endregion

                        transaction.Complete();

                        return cancelationSucceeded;
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Dispose();


                        throw new BusinessException(Resources.Resources.BusinessException);
                    }
                }
            }
        }

        #endregion

        #region Transfer

        ///<summary>
        /// HasTransferFormat
        /// Verifica si tiene formato de transferencia
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns>bool</returns>
        public bool HasTransferFormat(int bankCode)
        {
            bool hasTransferFormat = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TransferFormatBank.Properties.BankCode, bankCode);

                //Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TransferFormatBank), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    hasTransferFormat = true;
                }
            }
            catch (BusinessException)
            {
                hasTransferFormat = false;
            }

            return hasTransferFormat;
        }

        /// <summary>
        /// SaveTransferRequest
        /// Graba la transferencia de órden de pago
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        public TransferPaymentOrderDTO SaveTransferRequest(TransferPaymentOrderDTO transferPaymentOrder)
        {
            CoreTransaction.Transaction.Created += delegate (object sender, TransactionEventArgs e)
            {
            };
            using (Context.Current)
            {
                using (CoreTransaction.Transaction transaction = new CoreTransaction.Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {
                    };
                    try
                    {
                        TransferPaymentOrder newTransferPaymentOrder = new TransferPaymentOrder();

                        newTransferPaymentOrder.BankAccountCompany = new BankAccountCompany();
                        newTransferPaymentOrder.BankAccountCompany.Id = transferPaymentOrder.BankAccountCompany.Id;
                        newTransferPaymentOrder.DeliveryDate = transferPaymentOrder.DeliveryDate;
                        newTransferPaymentOrder.Status = transferPaymentOrder.Status;
                        newTransferPaymentOrder.UserId = transferPaymentOrder.UserId;

                        newTransferPaymentOrder = _transferPaymentOrderDAO.SaveTransferPaymentOrder(newTransferPaymentOrder);

                        //actualizo las ordenes de pago
                        if (newTransferPaymentOrder.Id > 0)
                        {
                            if (transferPaymentOrder.PaymentOrders != null)
                            {
                                newTransferPaymentOrder.PaymentOrders = new List<PaymentOrder>();
                                newTransferPaymentOrder.PaymentOrders = transferPaymentOrder.PaymentOrders.ToModels().ToList();

                                foreach (PaymentOrder paymentOrder in transferPaymentOrder.PaymentOrders.ToModels().ToList())
                                {
                                    bool result = false;
                                    //obtengo la orden de pago
                                    PaymentOrderDTO paymentOrderNew = DTOAssembler.ToDTO( _paymentOrderDAO.GetPaymentOrder(paymentOrder));

                                    //actualizo los valores
                                    paymentOrderNew.BankAccountPerson = new DTOs.BankAccounts.BankAccountPersonDTO();
                                    paymentOrderNew.BankAccountPerson.Id = paymentOrder.BankAccountPerson.Id;
                                    paymentOrderNew.Status = Convert.ToInt32(PaymentOrderStatus.Applied);
                                    paymentOrderNew.UserId = -1;

                                    result = _paymentOrderDAO.UpdatePaymentOrder(ModelDTOAssembler.ToModel(paymentOrderNew));

                                    if (result)
                                    {
                                        _paymentOrderTransferPaymentDAO.SavePaymentOrderTransferPayment(paymentOrderNew.Id, newTransferPaymentOrder.Id);
                                    }
                                }
                            }
                        }

                        foreach (PaymentOrder paymentOrder in newTransferPaymentOrder.PaymentOrders)
                        {
                            //LLAMA A LA GRABACIÓN DE LA IMPUTACIÓN
                            DelegateService.accountingApplicationService.SavePaymentOrderApplication(paymentOrder.Id, Convert.ToInt32(ApplicationTypes.PaymentOrder), newTransferPaymentOrder.UserId);
                        }

                        transaction.Complete();

                        return newTransferPaymentOrder.ToDTO();

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
        /// GenerateTransferFile
        /// Genera archivo de transferencia
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public string GenerateTransferFile(TransferPaymentOrderDTO transferPaymentOrder, string path)
        {
            string fileName = "";

            //se obtienen los datos de la transferencia
            TransferPaymentOrder newTransferPaymentOrder = new TransferPaymentOrder();

            newTransferPaymentOrder.Id = transferPaymentOrder.Id;
            newTransferPaymentOrder = _transferPaymentOrderDAO.GetTransferPaymentOrder(newTransferPaymentOrder);

            //Se obtiene el número de la cuenta emisora de la compañía
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId, newTransferPaymentOrder.BankAccountCompany.Id);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.BankAccountCompany), criteriaBuilder.GetPredicate()));

            foreach (ACCOUNTINGEN.BankAccountCompany companyAccountBank in businessCollection.OfType<ACCOUNTINGEN.BankAccountCompany>())
            {
                newTransferPaymentOrder.BankAccountCompany = new BankAccountCompany();
                newTransferPaymentOrder.BankAccountCompany.Number = companyAccountBank.AccountNumber;
            }

            newTransferPaymentOrder.PaymentOrders = new List<PaymentOrder>();

            //Se obtiene las órdenes de pago relacionadas con la transferencia
            criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentOrderTransferPayment.Properties.TransferPaymentOrderCode, newTransferPaymentOrder.Id);

            BusinessCollection transferPaymentOrders = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentOrderTransferPayment), criteriaBuilder.GetPredicate()));

            foreach (ACCOUNTINGEN.PaymentOrderTransferPayment item in transferPaymentOrders.OfType<ACCOUNTINGEN.PaymentOrderTransferPayment>())
            {
                //obtengo la orden de pago
                PaymentOrder paymentOrder = new PaymentOrder();
                paymentOrder.Id = Convert.ToInt32(item.PaymentOrderCode);
                paymentOrder = _paymentOrderDAO.GetPaymentOrder(paymentOrder);

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.BankAccountCompanyId, paymentOrder.BankAccountPerson.Id);

                BusinessCollection businessCollectionCompanies = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.BankAccountCompany), criteriaBuilder.GetPredicate()));

                if (businessCollectionCompanies.Count() == 0)
                {
                    paymentOrder.BankAccountPerson.Number = "";

                    paymentOrder.BankAccountPerson.Bank = new Bank();
                    paymentOrder.BankAccountPerson.Bank.Id = 0;
                    paymentOrder.BankAccountPerson.Bank.Description = "";
                }
                else
                {
                    foreach (ACCOUNTINGEN.BankAccountCompany bankAccountCompany in businessCollectionCompanies.OfType<ACCOUNTINGEN.BankAccountCompany>())
                    {
                        paymentOrder.BankAccountPerson.Number = bankAccountCompany.AccountNumber;

                        paymentOrder.BankAccountPerson.Bank = new Bank();
                        paymentOrder.BankAccountPerson.Bank.Id = Convert.ToInt32(bankAccountCompany.BankCode);
                        paymentOrder.BankAccountPerson.Bank.Description = DelegateService.commonService.GetBanksByBankId(paymentOrder.BankAccountPerson.Bank.Id).Description;
                    }
                }

                paymentOrder.Beneficiary.FullName = GetIndividualByIndividualId(paymentOrder.Beneficiary.IndividualId).Name;


                newTransferPaymentOrder.PaymentOrders.Add(paymentOrder);
            }

            fileName = "TRANSFER" + Convert.ToString(newTransferPaymentOrder.Id).PadLeft(5, '0') + DateTime.Today.ToString("d").Replace("/", "") + ".txt";

            path = Path.Combine(path, fileName);

            //Se comienza la grabación del archivo
            FileInfo file = new FileInfo(path);

            StreamWriter writer = file.CreateText();

            foreach (PaymentOrder paymentOrder in newTransferPaymentOrder.PaymentOrders)
            {
                try
                {

                    //NOTA: EL FORMATO DEL ARCHIVO DEBE SER PREVIAMENTE DEFINIDO PARA SABER QUE CAMPOS SE DEBE GRABAR
                    //ACTUALMENTE ESTA PENDIENTE SEGUN EF
                    string line = Convert.ToString(paymentOrder.Id).PadRight(10, ' ') +                     //ID DE LA ORDEN DE PAGO
                                  paymentOrder.Beneficiary.FullName.PadRight(30, ' ').Substring(0, 30) +        //NOMBRE BENEFICIARIO
                                  String.Format("{0:dd/MM/yyyy}", paymentOrder.EstimatedPaymentDate).PadRight(11, ' ') +    //FECHA ESTIMADA DE PAGO
                                  Convert.ToString(paymentOrder.BankAccountPerson.Bank.Id).PadRight(3, ' ') + //ID DE BANCO RECEPTOR  
                                  paymentOrder.BankAccountPerson.Bank.Description.PadRight(26, ' ') +       //NOMBRE BANCO RECEPTOR  
                                  Convert.ToString(paymentOrder.BankAccountPerson.Id).PadRight(3, ' ') +    //ID CUENTA BANCO RECEPTOR
                                  newTransferPaymentOrder.BankAccountCompany.Number.PadRight(12, ' ') +     //NRO. CUENTA BANCO EMISOR
                                  Convert.ToString(paymentOrder.LocalAmount.Value).PadRight(13, ' ') +      //MONTO
                                  Convert.ToString(paymentOrder.Status).PadRight(3, ' ') +                  //ESTADO
                                  Convert.ToString(newTransferPaymentOrder.BankAccountCompany.Id).PadRight(3, ' ');  //ID. CUENTA BANCO EMISOR
                    writer.WriteLine(line);
                }
                catch (BusinessException ex)
                {
                    throw new BusinessException(ex.Message);
                }
            }
            writer.Close();

            return fileName;
        }

        #endregion

        #region TransferCancellation

        /// <summary>
        /// CancelTransferBank
        /// Llama al proceso de anulación de Transferencias de pago bancarias hechas en efectivo
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="userCancelId"></param>
        /// <param name="typeOperation"></param>
        /// <returns>PaymentOrder</returns>
        public PaymentOrderDTO CancelTransferBank(int paymentOrderId, int userCancelId, int typeOperation)
        {

            try
            {

                PaymentOrder paymentOrder = new PaymentOrder();

                paymentOrder.Id = paymentOrderId;
                paymentOrder = _paymentOrderDAO.GetPaymentOrder(paymentOrder);

                if (paymentOrder != null)
                {
                    //Se verifica que la OP esté en estado Aplicada para poder anularla
                    if (paymentOrder.Status == Convert.ToInt32(PaymentOrderStatus.Applied))
                    {
                        if (typeOperation == 1)  //solo si es 1 debe realizar el update 
                        {
                            //Cambia el estado de la OP a "Anulada"
                            paymentOrder.Status = Convert.ToInt32(PaymentOrderStatus.Canceled);
                            paymentOrder.CancellationDate = DateTime.Now;
                            paymentOrder.UserId = userCancelId;

                            _paymentOrderDAO.UpdatePaymentOrder(paymentOrder);

                            paymentOrder = _paymentOrderDAO.GetPaymentOrder(paymentOrder);

                            List<ACCOUNTINGEN.PaymentOrderTransferPayment> paymentOrderTransferPayments = _paymentOrderTransferPaymentDAO.GetPaymentOrderTransferPaymentByPaymentOrderId(paymentOrderId);
                            int transferPaymentOrderId = 0;

                            if (paymentOrderTransferPayments != null)
                            {
                                foreach (ACCOUNTINGEN.PaymentOrderTransferPayment paymentOrderTransferPaymentEntity in paymentOrderTransferPayments)
                                {
                                    transferPaymentOrderId = Convert.ToInt32(paymentOrderTransferPaymentEntity.TransferPaymentOrderCode);
                                }
                            }

                            TransferPaymentOrder transferPaymentOrder = new TransferPaymentOrder();
                            transferPaymentOrder.Id = transferPaymentOrderId;
                            transferPaymentOrder = _transferPaymentOrderDAO.GetTransferPaymentOrder(transferPaymentOrder);
                            //Cambia el estado de la Transferencia a "Rechazada"
                            transferPaymentOrder.Status = Convert.ToInt32(TransferPaymentOrders.Rejected);
                            transferPaymentOrder.CancellationDate = DateTime.Now;
                            _transferPaymentOrderDAO.UpdateTransferPaymentOrder(transferPaymentOrder);
                        }

                        return paymentOrder.ToDTO();
                    }

                    paymentOrder.Id = 0; //si devuelve Id 0 es porque la OP ya estaba anulada previamente
                }
                else
                {
                    return null;  //si devuelve nulo es porque el # de OP que esta en el archivo plano no existe en la BDD
                }

                return paymentOrder.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region AccountingCompany

        ///<summary>
        /// SaveAccountingCompany
        /// Registra una companía contable
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        public CompanyDTO SaveAccountingCompany(CompanyDTO company)
        {
            try
            {
                return _companyDAO.SaveCompany(company.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateAccountingCompany
        /// Edita una companía contable
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        public CompanyDTO UpdateAccountingCompany(CompanyDTO company)
        {
            try
            {
                return _companyDAO.UpdateCompany(company.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteAccountingCompany
        /// Elimina una companía contable
        /// </summary>
        /// <param name="accountingCompanyId"></param>
        /// <returns>bool</returns>
        public bool DeleteAccountingCompany(int accountingCompanyId)
        {
            try
            {
                return _companyDAO.DeleteCompany(accountingCompanyId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingCompany
        /// Obtiene una companía contable
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        public CompanyDTO GetAccountingCompany(CompanyDTO company)
        {
            try
            {
                return _companyDAO.GetCompany(company.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// GetAccountingCompanies
        /// Obtiene companías contables
        /// </summary>
        /// <returns>List<Company/></returns>
        public List<CompanyDTO> GetAccountingCompanies()
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

        /// <summary>
        /// DeleteTempVoucherByRangeVoucherId
        /// Consumido por AccountingServicesEEProvider
        /// </summary>
        /// <param name="voucherIdBegin"></param>
        /// <param name="voucherIdLast"></param>
        public void DeleteTempVoucherByRangeVoucherId(int voucherIdBegin, int voucherIdLast)
        {
            try
            {
                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(COMMEN.TempVoucher.Properties.VoucherCode);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(voucherIdBegin);
                criteriaBuilder.And();
                criteriaBuilder.Property(COMMEN.TempVoucher.Properties.VoucherCode, voucherIdLast.ToString());
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(voucherIdLast);

                // Obtiene la lista de registros a borrar
                BusinessCollection tempVouchers = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(COMMEN.TempVoucher), criteriaBuilder.GetPredicate()));

                foreach (COMMEN.TempVoucher tempVoucherEntity in tempVouchers.OfType<COMMEN.TempVoucher>())
                {
                    new TempVoucherDAO().DeleteTempVoucher(tempVoucherEntity.VoucherCode);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// DeleteTempVoucherConceptByTempPaymentRequest
        /// Consumido por AccountingServicesEEProvider
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        public void DeleteTempVoucherConceptByTempPaymentRequest(int tempPaymentRequestId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(COMMEN.TempVoucherConcept.Properties.TempPaymentRequestCode, tempPaymentRequestId);

                //Obtiene la lista de registros a borrar
                BusinessCollection tempVoucherConcepts = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(COMMEN.TempVoucherConcept), criteriaBuilder.GetPredicate()));

                if (tempVoucherConcepts.Count > 0)
                {
                    foreach (COMMEN.TempVoucherConcept tempVoucherConceptEntity in tempVoucherConcepts.OfType<COMMEN.TempVoucherConcept>())
                    {
                        new TempVoucherConceptDAO().DeleteTempVoucherConcept(tempVoucherConceptEntity.VoucherConceptCode);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        /// <summary>
        /// GetTempVoucherByRangeVoucherId
        /// Consumido desde AccountingService
        /// </summary>
        /// <param name="voucherIdBegin"></param>
        /// <param name="voucherIdLast"></param>
        /// <returns>List<Voucher/></returns>
        public List<VoucherDTO> GetTempVoucherByRangeVoucherId(int voucherIdBegin, int voucherIdLast)
        {
            List<Voucher> temporalVouchers = new List<Voucher>();

            try
            {
                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(COMMEN.TempVoucher.Properties.VoucherCode);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(voucherIdBegin);
                criteriaBuilder.And();
                criteriaBuilder.Property(COMMEN.TempVoucher.Properties.VoucherCode, voucherIdLast.ToString());
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(voucherIdLast);

                //Asignamos BusinessCollection a una Lista
                BusinessCollection tempVoucherEntities = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(COMMEN.TempVoucher), criteriaBuilder.GetPredicate()));

                if (tempVoucherEntities.Count > 0)
                {
                    foreach (COMMEN.TempVoucher tempVoucherEntity in tempVoucherEntities.OfType<COMMEN.TempVoucher>())
                    {
                        Voucher temporalVoucher = new Voucher();
                        temporalVoucher.Id = tempVoucherEntity.VoucherCode;
                        temporalVoucher = new TempVoucherDAO().GetTempVoucher(temporalVoucher);
                        temporalVouchers.Add(temporalVoucher);
                    }
                }

                return temporalVouchers.ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetVoucherTypes
        /// Obtiene una lista de los tipos de Voucher
        /// </summary>
        /// <returns>List<VoucherType/></returns>
        public List<VoucherTypeDTO> GetVoucherTypes()
        {
            try
            {
                VoucherTypeDAO voucherTypeDAO = new VoucherTypeDAO();
                return voucherTypeDAO.GetVoucherTypes().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Public Methods


        #region PaymentOrderAuthorization

        /// <summary>
        /// GetPaymentOrderAuthorization
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="levelUser"></param>
        /// <returns>List<PaymentOrder></returns>
        public List<PaymentOrderDTO> GetPaymentOrderAuthorization(int branchId, int paymentMethodId, DateTime estimatedPaymentDate, int userId)
        {
            int moduleId = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_ACCOUNTING));
            int rows;

            List<PaymentOrder> paymentOrders = new List<PaymentOrder>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            try
            {
                int hierarchy = GetHierarchyByUserModule(userId, moduleId);

                if (hierarchy > 0)
                {
                    int authorizationLevel = Convert.ToInt32(ConfigurationManager.AppSettings["HIERARCHY_" + hierarchy.ToString()]);

                    if (authorizationLevel == 1)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                        IFormatProvider culture = new CultureInfo("es-EC", true);
                        DateTime estimationDate = Convert.ToDateTime(Convert.ToString(estimatedPaymentDate.ToString("dd/MM/yyyy")), culture);

                        #region criteriaBuilder
                        criteriaBuilder.Property(ACCOUNTINGEN.PayerOrderUnauthorizedView.Properties.EstimatedPaymentDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(estimationDate);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PayerOrderUnauthorizedView.Properties.BranchCode, branchId);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PayerOrderUnauthorizedView.Properties.PaymentMethodCode, paymentMethodId);
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PayerOrderUnauthorizedView.Properties.UserId);
                        criteriaBuilder.Distinct();
                        criteriaBuilder.Constant(userId);
                        #endregion

                        UIView paymentOrderList = _dataFacadeManager.GetDataFacade().GetView("PayerOrderUnauthorizedView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                        if (paymentOrderList.Rows.Count > 0)
                        {
                            paymentOrders = PaymentOrderAuthorizationLevel(paymentOrderList).ToModels().ToList();
                        }
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                        IFormatProvider culture = new CultureInfo("es-EC", true);
                        DateTime estimationDate = Convert.ToDateTime(Convert.ToString(estimatedPaymentDate.ToString("dd/MM/yyyy")), culture);

                        #region criteriaBuilder
                        criteriaBuilder.Property(ACCOUNTINGEN.PayerOrderAuthorizedView.Properties.EstimatedPaymentDate);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(estimationDate);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PayerOrderAuthorizedView.Properties.BranchCode, branchId);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PayerOrderAuthorizedView.Properties.PaymentMethodCode, paymentMethodId);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PayerOrderAuthorizedView.Properties.AuthorizationLevel, authorizationLevel - 1);
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.PayerOrderAuthorizedView.Properties.UserId);
                        criteriaBuilder.Distinct();
                        criteriaBuilder.Constant(userId);
                        #endregion

                        UIView paymentOrderList = _dataFacadeManager.GetDataFacade().GetView("PayerOrderAuthorizedView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                        if (paymentOrderList.Rows.Count > 0)
                        {
                            paymentOrders = PaymentOrderAuthorizationLevel(paymentOrderList).ToModels().ToList();
                        }
                    }
                }
                return paymentOrders.ToDTOs().ToList();
            }

            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// SavePaymentOrderAuthorization
        /// Registra el nivel autorización de las órdenes de pago y actualiza el Estado>
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <param name="authorizationLevel"></param>
        /// <returns></returns>
        public void SavePaymentOrderAuthorization(PaymentOrderDTO paymentOrder)
        {

            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {

                    int moduleId = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_ACCOUNTING));
                    int hierarchy = GetHierarchyByUserModule(paymentOrder.UserId, moduleId);
                    int authorizationLevel = Convert.ToInt32(ConfigurationManager.AppSettings["HIERARCHY_" + hierarchy.ToString()]);

                    _paymentOrderAuthorizationDAO.SavePaymentOrderAuthorization(paymentOrder.ToModel(), authorizationLevel).ToDTO();

                    //ACTUALIZA ESTADO EN LA OP

                    PaymentOrder paymentOrderAuthorized = new PaymentOrder();

                    paymentOrderAuthorized = _paymentOrderDAO.GetPaymentOrder(paymentOrder.ToModel());

                    paymentOrderAuthorized.Status = Convert.ToInt16(PaymentOrderStatus.Authorized);

                    _paymentOrderDAO.UpdatePaymentOrder(paymentOrderAuthorized);

                    transaction.Complete();

                }
                catch (BusinessException exception)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }
            }
        }

        /// <summary>
        /// GetIndividualByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public IndividualDTO GetIndividualByIndividualId(int individualId)
        {
            try
            {
                AccountsPayableBusiness accountsPayableBusiness = new AccountsPayableBusiness();
                return accountsPayableBusiness.GetIndividualsByIndividualId(individualId).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// GetVoucherConcepTaxesByVoucherConcept
        /// Obtiene los registro de VoucherConcepTax por Id de VoucherConcept
        /// </summary>
        /// <param name="voucherConceptId"></param>
        /// <returns>List<VoucherConceptTax></returns>
        private List<VoucherConceptTaxDTO> GetVoucherConcepTaxesByVoucherConceptId(int voucherConceptId)
        {
            List<VoucherConceptTax> voucherConceptTaxes = new List<VoucherConceptTax>();

            try
            {
                ObjectCriteriaBuilder voucherConceptTaxFilter = new ObjectCriteriaBuilder();

                voucherConceptTaxFilter.PropertyEquals(ACCOUNTINGEN.VoucherConceptTax.Properties.VoucherConceptCode, voucherConceptId);
                BusinessCollection voucherConceptTaxCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.VoucherConceptTax), voucherConceptTaxFilter.GetPredicate()));

                if (voucherConceptTaxCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.VoucherConceptTax voucherConceptTaxEntity in voucherConceptTaxCollection.OfType<ACCOUNTINGEN.VoucherConceptTax>())
                    {
                        VoucherConceptTax voucherConceptTax = new VoucherConceptTax();
                        voucherConceptTax.Id = voucherConceptTaxEntity.VoucherConceptTaxId;
                        voucherConceptTax = _voucherConceptTaxDAO.GetVoucherConceptTax(voucherConceptTax);

                        voucherConceptTaxes.Add(voucherConceptTax);
                    }
                }

                return voucherConceptTaxes.ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetVoucherConcepsByVoucherId
        /// Obtiene los registro de VoucherConcep por Id de Voucher
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns>List<VoucherConcept></returns>
        private List<VoucherConceptDTO> GetVoucherConcepsByVoucherId(int voucherId)
        {
            List<VoucherConcept> voucherConcepts = new List<VoucherConcept>();

            try
            {
                ObjectCriteriaBuilder voucherConceptFilter = new ObjectCriteriaBuilder();

                voucherConceptFilter.PropertyEquals(ACCOUNTINGEN.VoucherConcept.Properties.VoucherCode, voucherId);
                BusinessCollection voucherConceptCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.VoucherConcept), voucherConceptFilter.GetPredicate()));

                if (voucherConceptCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.VoucherConcept voucherConceptEntity in voucherConceptCollection.OfType<ACCOUNTINGEN.VoucherConcept>())
                    {
                        VoucherConcept voucherConcept = new VoucherConcept();
                        voucherConcept.Id = voucherConceptEntity.VoucherConceptId;
                        voucherConcept = _voucherConceptDAO.GetVoucherConcept(voucherConcept);

                        voucherConcepts.Add(voucherConcept);
                    }
                }

                return voucherConcepts.ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetVouchersByPaymentRequestId
        /// Obtiene los registro de Voucher por Id de PaymentRequest
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns>List<Voucher></returns>
        private List<VoucherDTO> GetVouchersByPaymentRequestId(int paymentRequestId)
        {
            List<Voucher> vouchers = new List<Voucher>();

            try
            {
                ObjectCriteriaBuilder voucherFilter = new ObjectCriteriaBuilder();

                voucherFilter.PropertyEquals(ACCOUNTINGEN.Voucher.Properties.PaymentRequestCode, paymentRequestId);
                BusinessCollection voucherCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Voucher), voucherFilter.GetPredicate()));

                if (voucherCollection.Count > 0)
                {
                    foreach (ACCOUNTINGEN.Voucher voucherEntity in voucherCollection.OfType<ACCOUNTINGEN.Voucher>())
                    {
                        Voucher voucher = new Voucher();
                        voucher.Id = voucherEntity.VoucherId;
                        voucher = _voucherDAO.GetVoucher(voucher);

                        vouchers.Add(voucher);
                    }
                }

                return vouchers.ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetPersonDocumentNumberByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List<Person></returns>
        
        

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
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetHierarchyByUserModule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <returns>int</returns>
        private int GetHierarchyByUserModule(int userId, int moduleId)
        {
            try
            {
                int levelHierarchy = 0;
                List<Object> levelHierarchies = new List<Object>();

                var parameters = new NameValue[2];

                parameters[0] = new NameValue("@USER_CD", userId);
                parameters[1] = new NameValue("@MODULE_CD", moduleId);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ACC.GET_HIERARCHY_BY_USER_MOD", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in result.Rows)
                    {
                        levelHierarchy = DBNull.ReferenceEquals(arrayItem[0], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[0]);
                    }
                }
                else
                {
                    levelHierarchy = -1;
                }

                return levelHierarchy;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// PaymentOrderAuthorizationLevel
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>List<PaymentOrder></returns>
        private List<PaymentOrderDTO> PaymentOrderAuthorizationLevel(UIView paymentOrder)
        {
            List<PaymentOrder> paymentOrders = new List<PaymentOrder>();
            foreach (DataRow row in paymentOrder.Rows)
            {
                Branch branch = new Branch()
                {
                    Id = row["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(row["BranchCode"]),
                    Description = row["BranchName"] == DBNull.Value ? "" : row["BranchName"].ToString()
                };

                Individual individual = new Individual()
                {
                    FullName = row["BeneficiaryName"] == DBNull.Value ? "" : row["BeneficiaryName"].ToString()
                };
                Currency currency = new Currency() { Description = row["CurrencyName"] == DBNull.Value ? "" : row["CurrencyName"].ToString() };
                Amount amount = new Amount() { Currency = currency };

                PersonType personType = new PersonType()
                {
                    Description = row["PersonTypeName"] == DBNull.Value ? "" : row["PersonTypeName"].ToString()
                };

                PaymentMethod paymentMethod = new PaymentMethod()
                {
                    Description = row["PaymentMethodName"] == DBNull.Value ? "" : row["PaymentMethodName"].ToString()
                };


                paymentOrders.Add(new PaymentOrder()
                {
                    Id = Convert.ToInt32(row["PaymentOrderCode"]),
                    Branch = branch,
                    Beneficiary = individual,
                    Amount = amount,
                    EstimatedPaymentDate = Convert.ToDateTime(row["EstimatedPaymentDate"]),
                    PersonType = personType,
                    PaymentMethod = paymentMethod,
                    UserId = Convert.ToInt32(row["UserId"])
                });
            }
            return paymentOrders.ToDTOs().ToList();
        }
        #endregion Private Methods
    }
}
