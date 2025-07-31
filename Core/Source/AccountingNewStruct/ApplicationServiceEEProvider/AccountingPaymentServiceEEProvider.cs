using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

//Sistran Core
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.CommonService.Models;
using PaymentModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using System.Threading;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using PAY = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
using ENUMACC = Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using AccountsPayables = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingPaymentServiceEEProvider : IAccountingPaymentService
    {
        #region Constants

        #endregion

        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        readonly PaymentDAO _paymentDAO = new PaymentDAO();
        readonly ExchangePaymentDAO _exchangePaymentDAO = new ExchangePaymentDAO();
        readonly LegalPaymentDAO _legalPaymentDAO = new LegalPaymentDAO();
        readonly RejectedPaymentDAO _rejectedPaymentDAO = new RejectedPaymentDAO();
        readonly PaymentLogDAO _paymentLogDAO = new PaymentLogDAO();
        readonly RegularizedPaymentDAO _regularizedPaymentDAO = new RegularizedPaymentDAO();
        readonly PaymentTaxDAO _paymentTaxDAO = new PaymentTaxDAO();
        readonly TempPaymentRequestDAO _tempPaymentRequestDAO = new TempPaymentRequestDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region Payment

        /// <summary>
        /// SavePayment 
        /// Graba un nuevo pago
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectId"></param>
        /// <returns>Payment</returns>
        public PAY.PaymentDTO SavePayment(PAY.PaymentDTO payment, int collectId)
        {
            try
            {
                return _paymentDAO.SavePayment(payment.ToModel(), collectId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePayment
        /// Actualiza un nuevo pago
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectCode"></param>
        /// <returns>Payment</returns>
        public PAY.PaymentDTO UpdatePayment(PAY.PaymentDTO payment, int collectCode)
        {
            try
            {
                return _paymentDAO.UpdatePayment(payment.ToModel(), collectCode).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePayment
        /// Elimina un registro de pago
        /// </summary>
        /// <param name="payment"></param>
        public void DeletePayment(PAY.PaymentDTO payment)
        {

            try
            {
                _paymentDAO.DeletePayment(payment.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPayment
        /// Obtiene un registro de pago
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>Payment</returns>
        public PAY.PaymentDTO GetPayment(PAY.PaymentDTO payment)
        {
            try
            {
                return _paymentDAO.GetPayment(payment.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPayments
        /// Obtiene todos los registros de pagos
        /// </summary>
        /// <returns>List<Payment/></returns>
        public List<PAY.PaymentDTO> GetPayments()
        {
            try
            {
                return _paymentDAO.GetPayments().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Transactions

        /// <summary>
        /// DetailValues
        /// Devuelve el detalle de un recibo 
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<DetailValuesDTO/></returns>
        public List<SEARCH.DetailValuesDTO> DetailValues(int collectId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collectId); //LFR

                UIView details = _dataFacadeManager.GetDataFacade().GetView("DetailReceiptView",
                                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (details.Rows.Count > 0)
                {
                    details.Columns.Add("rows", typeof(int));
                    details.Rows[0]["rows"] = rows;
                }

                #region LoadDTO

                List<SEARCH.DetailValuesDTO> detailValuesDTOs = new List<SEARCH.DetailValuesDTO>();

                foreach (DataRow dataRow in details)
                {
                    int currencyId = dataRow["CurrencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyId"]);
                    double amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Amount"]);
                    double localAmount = dataRow["LocalAmount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["LocalAmount"]);
                    if (currencyId != 0)
                    {
                        double aux;
                        aux = localAmount;
                        localAmount = amount;
                        amount = aux;
                    }

                    detailValuesDTOs.Add(new SEARCH.DetailValuesDTO()
                    {
                        PaymentTypeId = dataRow["PaymentTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTypeId"]),
                        PaymentTypeDescription = dataRow["PaymentTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["PaymentTypeDescription"]),
                        CurrencyId = currencyId,
                        Currency = dataRow["Currency"] == DBNull.Value ? "" : Convert.ToString(dataRow["Currency"]),
                        Amount = amount,
                        Exchange = dataRow["Exchange"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Exchange"]),
                        LocalAmount = localAmount,
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        Voucher = dataRow["Voucher"] == DBNull.Value ? "" : Convert.ToString(dataRow["Voucher"]),
                        CardType = dataRow["CardType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardType"]),
                        CardTypeName = dataRow["CardTypeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["CardTypeName"]),
                        AuthorizationNumber = dataRow["AuthorizationNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["AuthorizationNumber"]),
                        ValidThruMonth = dataRow["ValidThruMonth"] == DBNull.Value ? "" : Convert.ToString(dataRow["ValidThruMonth"]),
                        ValidThruYear = dataRow["ValidThruYear"] == DBNull.Value ? "" : Convert.ToString(dataRow["ValidThruYear"]),
                        IssuingBankId = dataRow["IssuingBankId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IssuingBankId"]),
                        IssuinBankName = dataRow["IssuinBankName"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuinBankName"]),
                        IssuingBankAccountNumber = dataRow["IssuingBankAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuingBankAccountNumber"]),
                        IssuerName = dataRow["IssuerName"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuerName"]),
                        RecievingBankId = dataRow["RecievingBankId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RecievingBankId"]),
                        RecievingBankAccountNumber = dataRow["RecievingBankAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["RecievingBankAccountNumber"]),
                        SerialVoucher = dataRow["SerialVoucher"] == DBNull.Value ? "" : Convert.ToString(dataRow["SerialVoucher"]),
                        SerialNumber = dataRow["SerialNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["SerialNumber"]),
                        Date = dataRow["Date"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", dataRow["Date"]), //Convert.ToString(dataRow["Date"]).,
                        CollectCode = dataRow["CollectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectId"]),
                        PaymentStatus = dataRow["PaymentStatus"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentStatus"]),
                        StatusDescription = dataRow["StatusDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["StatusDescription"]),
                        Tax = dataRow["Taxes"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Taxes"])
                    });
                }

                #endregion

                return detailValuesDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateVoucher
        /// Valida que no se ingrese el mismo número de voucher para un mismo número de tarjeta 
        /// </summary>
        /// <param name="creditCardNumber"></param>
        /// <param name="voucherNumber"></param>
        /// <param name="conduitType"></param>
        /// <returns>int</returns>
        public int ValidateVoucher(string creditCardNumber, string voucherNumber, int conduitType)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DocumentNumber);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(creditCardNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.Voucher);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(voucherNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.Status);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(Convert.ToInt32(PaymentStatus.Canceled));

                BusinessCollection businessCollection = new BusinessCollection
                    (_dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));

                return businessCollection.Count;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateDepositVoucher
        /// Valida que no se ingrese el mismo número de cheque, cuenta para un mismo banco 
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="numberDoc"></param>
        /// <param name="accountNumber"></param>
        /// <returns>int</returns>
        public int ValidateDepositVoucher(int bankId, string numberDoc, string accountNumber)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.ReceivingBankCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(bankId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DocumentNumber);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(numberDoc);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.ReceivingAccountNumber);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(accountNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.Status);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(Convert.ToInt32(PaymentStatus.Canceled));

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                                            (typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));

                return businessCollection.Count;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Check

        /// <summary>
        /// GetPaymentByBankIdAndDocumentNumber
        /// Obtiene la cantidad de pagos por id de banco y número de documento, si es > 0 devuelve verdadero caso contrario falso
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>bool</returns>
        public bool GetPaymentByBankIdAndDocumentNumber(int bankId, string documentNumber)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DocumentNumber, documentNumber);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, (int)PaymentStatus.Active);
                criteriaBuilder.CloseParenthesis();
                criteriaBuilder.Or();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DocumentNumber, documentNumber);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, (int)PaymentStatus.Deposited);
                criteriaBuilder.CloseParenthesis();

                UIView payments = _dataFacadeManager.GetDataFacade().GetView("PaymentBankDocumentView",
                                         criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                if (payments.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckInformation
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        public List<SEARCH.CheckInformationDTO> GetCheckInformation(int bankId, string documentNumber)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DocumentNumber, documentNumber);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, (int)PaymentStatus.Active);

                List<ACCOUNTINGEN.CheckInformationV> dataChecks = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.CheckInformationV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.CheckInformationV>().ToList();

                List<SEARCH.CheckInformationDTO> checkInformationDTOs = new List<SEARCH.CheckInformationDTO>();

                foreach (ACCOUNTINGEN.CheckInformationV checkInformation in dataChecks)
                {
                    SEARCH.CheckInformationDTO checkInformationDTO = new SEARCH.CheckInformationDTO();
                    string datePayment = Convert.ToString(checkInformation.DatePayment == null ? "" : Convert.ToString(checkInformation.DatePayment));

                    if (datePayment != "")
                    {
                        int x = datePayment.IndexOf(" ");
                        datePayment = datePayment.Substring(0, x);
                    }

                    checkInformationDTO.DatePayment = datePayment;
                    checkInformationDTO.CurrencyDescription = checkInformation.CurrencyDescription == null ? "" : (string)checkInformation.CurrencyDescription;
                    checkInformationDTO.Amount = checkInformation.Amount == 0 ? 1 : (decimal)checkInformation.Amount;
                    checkInformationDTO.Holder = checkInformation.Holder == null ? "" : (string)checkInformation.Holder;
                    checkInformationDTO.ReceivingBankCode = checkInformation.ReceivingBankCode == 0 ? 1 : (int)checkInformation.ReceivingBankCode;
                    checkInformationDTO.PayerId = checkInformation.IndividualId == 0 ? 1 : (int)checkInformation.IndividualId;
                    checkInformationDTO.IssuingBankCode = checkInformation.IssuingBankCode == 0 ? 1 : (int)checkInformation.IssuingBankCode;
                    checkInformationDTO.DocumentNumber = checkInformation.DocumentNumber == null ? "" : (string)checkInformation.DocumentNumber;
                    checkInformationDTO.CollectCode = checkInformation.CollectId == 0 ? -1 : (int)checkInformation.CollectId;
                    checkInformationDTO.PaymentCode = checkInformation.PaymentCode == 0 ? -1 : (int)checkInformation.PaymentCode;
                    checkInformationDTO.RegisterDate = checkInformation.RegisterDate == null ? "" : Convert.ToString(checkInformation.RegisterDate);
                    checkInformationDTO.PaymentTicketCode = checkInformation.PaymentTicketCode == 0 ? -1 : (int)checkInformation.PaymentTicketCode;
                    checkInformationDTO.ReceivingAccountNumber = checkInformation.ReceivingAccountNumber == null ? "" : (string)checkInformation.ReceivingAccountNumber;
                    checkInformationDTO.PaymentTicketItemCode = checkInformation.PaymentTicketCode == 0 ? -1 : (int)checkInformation.PaymentTicketCode;
                    checkInformationDTO.Status = checkInformation.Status == 0 ? -1 : (int)checkInformation.Status;
                    checkInformationDTO.PaymentStatus = checkInformation.PaymentStatus == 0 ? -1 : (int)checkInformation.PaymentStatus;
                    checkInformationDTO.PaymentTicketStatus = checkInformation.PaymentStatus == 0 ? 1 : (int)checkInformation.PaymentStatus;
                    checkInformationDTOs.Add(checkInformationDTO);
                }
                return checkInformationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckInformationGrid
        /// Obtiene la información de todos los cheques pagados de la tabla PAYMENT
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>List<CheckInformationGridDTO/></returns>
        public List<SEARCH.CheckInformationGridDTO> GetCheckInformationGrid(int paymentId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentId);

                UIView checks = _dataFacadeManager.GetDataFacade().GetView("ChecksPaidView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                List<SEARCH.CheckInformationGridDTO> checkInformationGridDTOs = new List<SEARCH.CheckInformationGridDTO>();

                foreach (DataRow dataRow in checks.Rows)
                {
                    checkInformationGridDTOs.Add(new SEARCH.CheckInformationGridDTO()
                    {
                        DatePayment = dataRow["DatePayment"] == DBNull.Value ? "" : Convert.ToString(dataRow["DatePayment"]),
                        CurrencyDescription = dataRow["DatePayment"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Amount"]),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : Convert.ToString(dataRow["Holder"]),
                        ReceivingBankCode = dataRow["ReceivingBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReceivingBankCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        IssuingBankCode = dataRow["IssuingBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IssuingBankCode"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        ReceivingAccountNumber = dataRow["ReceivingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["ReceivingAccountNumber"]),
                        Comission = dataRow["Comission"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Comission"]),
                        TaxComission = dataRow["TaxComission"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["TaxComission"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        RejectionDescription = dataRow["RejectionDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["RejectionDescription"]),
                        PayerId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        CreditCardDescription = dataRow["CreditCardDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardDescription"]),
                        Voucher = dataRow["Voucher"] == DBNull.Value ? "" : Convert.ToString(dataRow["Voucher"]),
                        CreditCardTypeCode = dataRow["CreditCardTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CreditCardTypeCode"]),
                        PaymentMethodTypeCode = dataRow["PaymentMethodTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentMethodTypeCode"])
                    });
                }

                return checkInformationGridDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRejectedPaymentByBankIdAndDocumentNumber
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>List<RejectedPaymentDTO/></returns>
        public List<SEARCH.RejectedPaymentDTO> GetRejectedPaymentByBankIdAndDocumentNumber(int bankId, string documentNumber)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DocumentNumber, documentNumber);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, Convert.ToInt32(PaymentStatus.Rejected));

                List<ACCOUNTINGEN.GetRejectedPaymentV> data = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.GetRejectedPaymentV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetRejectedPaymentV>().ToList();

                List<SEARCH.RejectedPaymentDTO> rejectedPaymentsDTOs = new List<SEARCH.RejectedPaymentDTO>();

                if (data.Count > 0)
                {
                    foreach (ACCOUNTINGEN.GetRejectedPaymentV RejectedPayment in data)
                    {
                        SEARCH.RejectedPaymentDTO rejectedPaymentDTO = new SEARCH.RejectedPaymentDTO();
                        rejectedPaymentDTO.RejectedPaymentCode = RejectedPayment.RejectedPaymentCode;
                        rejectedPaymentDTO.PaymentCode = RejectedPayment.PaymentCode;
                        rejectedPaymentDTO.IssuingBankCode = RejectedPayment.IssuingBankCode == 0 ? 1 : (int)RejectedPayment.IssuingBankCode;
                        rejectedPaymentDTO.IssuingAccountNumber = RejectedPayment.IssuingAccountNumber;
                        rejectedPaymentDTO.BankDescription = RejectedPayment.BankDescription;
                        rejectedPaymentDTO.DocumentNumber = RejectedPayment.DocumentNumber;
                        rejectedPaymentDTO.DatePayment = Convert.ToString(RejectedPayment.DatePayment);
                        rejectedPaymentDTO.CurrencyCode = Convert.ToInt32(RejectedPayment.CurrencyCode);
                        rejectedPaymentDTO.CurrencyDescription = RejectedPayment.CurrencyDescription;
                        rejectedPaymentDTO.Amount = RejectedPayment.Amount == 0 ? 1 : (decimal)RejectedPayment.Amount;
                        rejectedPaymentDTO.Holder = RejectedPayment.Holder;
                        rejectedPaymentDTO.CollectCode = RejectedPayment.CollectCode == 0 ? 1 : (int)RejectedPayment.CollectCode;
                        rejectedPaymentDTO.PayerId = RejectedPayment.IndividualId == 0 ? 1 : (int)RejectedPayment.IndividualId;
                        rejectedPaymentDTO.Name = RejectedPayment.Name;
                        rejectedPaymentDTO.RejectionDate = Convert.ToDateTime(RejectedPayment.RejectionDate).ToString("d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                        rejectedPaymentDTO.RejectionDescription = RejectedPayment.RejectionDescription;
                        rejectedPaymentDTO.Status = RejectedPayment.Status == 0 ? 1 : (int)RejectedPayment.Status;
                        rejectedPaymentDTO.PaymentMethodTypeCode = RejectedPayment.PaymentMethodTypeCode == 0 ? 1 : (int)RejectedPayment.PaymentMethodTypeCode;
                        rejectedPaymentsDTOs.Add(rejectedPaymentDTO);
                    }
                }

                return rejectedPaymentsDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentBallotInfoByPaymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>PaymentBallot</returns>
        public SEARCH.PaymentBallotDTO GetPaymentBallotInfoByPaymentId(int paymentId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode, paymentId);

                UIView paymentBallotView = _dataFacadeManager.GetDataFacade().GetView("PaymentTicketBallotView",
                                           criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                PaymentBallot paymentBallot = new PaymentBallot();

                if (paymentBallotView.Count > 0)
                {
                    paymentBallot.Id = Convert.ToInt32(paymentBallotView.Rows[0]["PaymentBallotCode"]);
                    paymentBallot.BankDate = Convert.ToDateTime(paymentBallotView.Rows[0]["RegisterDate"]);
                    paymentBallot.BallotNumber = Convert.ToString(paymentBallotView.Rows[0]["PaymentBallotNumber"]);
                    paymentBallot.Bank = new Bank();
                    paymentBallot.Bank.Id = Convert.ToInt32(paymentBallotView.Rows[0]["BankCode"]);
                    paymentBallot.Bank.Description = Convert.ToString(paymentBallotView.Rows[0]["BankDescription"]);
                    paymentBallot.AccountNumber = Convert.ToString(paymentBallotView.Rows[0]["AccountNumber"]);
                }

                return paymentBallot.ToDTOPayment();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateCheckBankOrTransfer
        /// Valida que no se ingrese el mismo número de transferencia, cuenta para un mismo banco 
        /// Modificado: Alejandro Villagrán.- aumentado campo Status en la validación
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="checkNumber"></param>
        /// <param name="accountNumber"></param>
        /// <returns>int</returns>
        public int ValidateCheckBankOrTransfer(int bankId, string checkNumber, string accountNumber)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.IssuingBankCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(bankId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DocumentNumber);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(checkNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.IssuingAccountNumber);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(accountNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.Status);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(Convert.ToInt32(PaymentStatus.Canceled));

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                    (typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));

                return businessCollection.Count;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region LegalPayment

        ///<summary>
        /// SaveLegalPayment
        /// </summary>
        /// <param name="legalPaymentId"></param>
        /// <param name="rejectedPaymentId"></param>
        /// <param name="legalDate"></param>
        /// <returns>int</returns>
        public int SaveLegalPayment(int legalPaymentId, int rejectedPaymentId, DateTime legalDate)
        {
            try
            {
                return _legalPaymentDAO.SaveLegalPayment(legalPaymentId, rejectedPaymentId, legalDate);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// SaveLegalPaymentRequest
        /// Graba LegalPayment Enviando un objeto
        /// </summary>
        /// <param name="legalPaymentDto"></param>
        /// <param name="payerId"></param>
        /// <param name="descriptionLegalize"></param>
        /// <returns>int</returns>
        public MessageSuccessDTO SaveLegalPaymentRequest(SEARCH.LegalPaymentDTO legalPaymentDto, int payerId, string descriptionLegalize, int branchId, int userId, DateTime accountingDate)
        {

            try
            {


                int saved;
                bool showMessage = true;
                saved = SaveLegalPayment(Convert.ToInt32(legalPaymentDto.LegalPaymentId), Convert.ToInt32(legalPaymentDto.RejectedPaymentId), Convert.ToDateTime(legalPaymentDto.LegalDate));

                #region Log

                SavePaymentLog(Convert.ToInt32(ActionTypes.PayLegalized), saved, Convert.ToInt32(legalPaymentDto.PaymentId), Convert.ToInt32(PaymentStatus.Legalized), Convert.ToInt32(legalPaymentDto.UserId));

                #endregion

                Models.Payments.Payment payment = new Models.Payments.Payment();
                payment.Id = legalPaymentDto.PaymentId;
                payment = _paymentDAO.GetPayment(payment);
                payment.Status = Convert.ToInt16(PaymentStatus.Exchanged);
                Collect newCollect = new AccountingCollectServiceEEProvider().ReplicateCheckinCollect(legalPaymentDto.CollectId, descriptionLegalize, payerId, 0, payment, Convert.ToInt32(PaymentStatus.Legalized), branchId);

                int technicalTransactionForLegalize = new AccountingCollectServiceEEProvider().GetTechnicalTransactionByPaymentId(payment.Id);
                string recordCollectMessage = "";
                int bridgeLegalize = (int)DelegateService.commonService.GetParameterByDescription(ENUMACC.AccountingKeys.ACC_CHECK_LEGALIZE.ToString()).NumberParameter;
                bool generalLedgerSuccess = true;
                #region accounting

                if ((string)UTILHELPER.EnumHelper.GetEnumParameterValue<ENUMACC.AccountingKeys>(ENUMACC.AccountingKeys.ACC_ENABLED_GENERAL_LEGDER) == "true")
                {
                    AccountingPaymentBusiness accountingPaymentBusiness = new AccountingPaymentBusiness();
                    var message = accountingPaymentBusiness.ProcessAccountingCheck(newCollect, accountingDate, userId, technicalTransactionForLegalize, payment.Id, Convert.ToInt32(PaymentStatus.Legalized), bridgeLegalize);
                    generalLedgerSuccess = message.GeneralLedgerSuccess;
                    recordCollectMessage = message.Info;
                    showMessage = true;
                }
                else
                {
                    recordCollectMessage = Convert.ToString(Resources.Resources.IntegrationServiceDisabledLabel);
                    showMessage = false;
                }
                #endregion
                //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
                Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
                integration2GBusiness.Save(newCollect.ToModelIntegration(1));
                MessageSuccessDTO messageSuccessDTO = new MessageSuccessDTO()
                {
                    ImputationMessage = recordCollectMessage,
                    TechnicalTransaction = newCollect.Transaction.TechnicalTransaction,
                    ShowMessage = showMessage,
                    BillId = newCollect.Id,
                    GeneralLedgerSuccess = generalLedgerSuccess
                };
                return messageSuccessDTO;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region RejectedPayment

        /// <summary>
        /// SaveRejectedPayment
        /// </summary>
        /// <param name="rejectedPayment"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <param name="collectId"></param>
        /// <param name="payerId"></param>
        /// <param name="description"></param>
        /// <returns>int</returns>
        public int SaveRejectedPayment(RejectedPaymentDTO rejectedPayment, int userId, DateTime registerDate,
                                       int collectId, int payerId, string description, int branchId)
        {

            try
            {
                Collect collect;

                rejectedPayment = _rejectedPaymentDAO.SaveRejectedPayment(rejectedPayment.ToModel(), userId, registerDate).ToDTO();

                // Grabación del Log
                SavePaymentLog(Convert.ToInt32(ActionTypes.RejectionPayment), rejectedPayment.Id, rejectedPayment.Payment.Id, Convert.ToInt32(PaymentStatus.Rejected), userId);

                Models.Payments.Payment payment = new Models.Payments.Payment { Id = rejectedPayment.Payment.Id };
                payment = _paymentDAO.GetPayment(payment);

                // Grabación ReplicateBill

                // Por Tarjeta
                if (payment.PaymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<ENUMACC.AccountingKeys>(ENUMACC.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)) ||
                    payment.PaymentMethod.Id == Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<ENUMACC.AccountingKeys>(ENUMACC.AccountingKeys.ACC_PARAM_PAYMENT_METHOD_UNCREDITABLE_CREDITCARD)))
                {
                    collect = new AccountingCollectServiceEEProvider().ReplicateCollect(collectId, description, 0, 0, payment, branchId);
                }
                else
                // Por Cheque
                {
                    collect = new AccountingCollectServiceEEProvider().ReplicateCollect(collectId, description, payerId, 0, payment, branchId);
                }

                return collect.Id;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region PaymentLog

        /// <summary>
        /// SavePaymentLog
        /// </summary>
        /// <param name="actionTypeId"></param>
        /// <param name="sourceId"></param>
        /// <param name="paymentId"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        public void SavePaymentLog(int actionTypeId, int sourceId, int paymentId, int status, int userId)
        {
            try
            {
                Dictionary<string, string> itemParam = new Dictionary<string, string>();

                itemParam.Add("Id", 0.ToString()); //Identity
                itemParam.Add("ActionTypeCode", actionTypeId.ToString());
                itemParam.Add("SourceCode", sourceId.ToString());
                itemParam.Add("PaymentCode", paymentId.ToString());
                itemParam.Add("Status", status.ToString());
                itemParam.Add("UserId", userId.ToString());
                itemParam.Add("RegisterDate", DateTime.Now.ToString());

                _paymentLogDAO.SavePaymentLog(itemParam);

                _paymentDAO.UpdatePaymentStatusById(Convert.ToInt32(itemParam["PaymentCode"]), Convert.ToInt32(itemParam["Status"]));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region BillReports

        /// <summary>
        /// GetReportPayment
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branch"></param>
        /// <param name="uiviewName"></param>
        /// <param name="collectCode"></param>
        /// <returns>List<ReportCollectDTO/></returns>
        public List<SEARCH.ReportCollectDTO> GetReportPayment(int userId, int branch, string uiviewName, int collectCode)
        {
            try
            {

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (userId > -1)
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.UserId);
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(userId);
                    criteriaBuilder.And();
                }


                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(branch);

                if (collectCode < 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.Status); // Estado de la Caja
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant((int)CollectControlStatus.Open);
                }
                else
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.Collect.Properties.CollectId); // Número del recibo
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(collectCode);
                }

                UIView reports = _dataFacadeManager.GetDataFacade().GetView(uiviewName,
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.ReportCollectDTO> reportCollectDTOs = new List<SEARCH.ReportCollectDTO>();

                if (uiviewName == "CollectView")
                {
                    #region ReportCollectDTO

                    foreach (DataRow dataRow in reports.Rows)
                    {
                        reportCollectDTOs.Add(new SEARCH.ReportCollectDTO()
                        {
                            CollectStatus = Convert.ToInt32(dataRow["CollectStatus"]),
                            CollectControlId = Convert.ToInt32(dataRow["CollectControlCode"]),
                            CollectId = Convert.ToInt32(dataRow["CollectId"]),
                            UserId = Convert.ToInt32(dataRow["UserId"]),
                            AccountName = dataRow["AccountName"].ToString(),
                            BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                            BranchDescription = dataRow["BranchDescription"].ToString(),
                            Status = Convert.ToInt32(dataRow["Status"]),
                            DateTransaction = Convert.ToDateTime(dataRow["DateTransaction"]),
                            CollectItemId = Convert.ToInt32(dataRow["CollectItemId"]),
                            Policy = dataRow["Policy"].ToString(),
                            Endorsement = dataRow["Endorsement"].ToString(),
                            Quota = Convert.ToInt32(dataRow["Quota"]),
                            Amount = Convert.ToDecimal(dataRow["Amount"]),
                            AmountReceived = Convert.ToDecimal(dataRow["AmountReceived"]),
                            Description = dataRow["Description"].ToString(),
                            CollectConceptId = Convert.ToInt32(dataRow["CollectConceptId"]),
                            CollectConceptDescription = dataRow["CollectConceptDescription"].ToString(),
                            NamePayer = dataRow["NamePayer"].ToString()
                        });
                    }

                    #endregion
                }
                if (uiviewName == "PaymentView")
                {
                    #region LoadReportPaymentDTO

                    int collectingConcept;

                    foreach (DataRow dataRow in reports.Rows)
                    {
                        if (dataRow["CollectConceptId"] == DBNull.Value)
                        {
                            collectingConcept = 0;
                        }
                        else
                        {
                            collectingConcept = Convert.ToInt32(dataRow["CollectConceptId"]);
                        }

                        reportCollectDTOs.Add(new SEARCH.ReportCollectDTO()
                        {
                            CollectStatus = Convert.ToInt32(dataRow["CollectStatus"]),
                            CollectControlId = Convert.ToInt32(dataRow["CollectControlCode"]),
                            CollectId = Convert.ToInt32(dataRow["CollectId"]),
                            UserId = Convert.ToInt32(dataRow["UserId"]),
                            AccountName = dataRow["AccountName"].ToString(),
                            BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                            BranchDescription = dataRow["BranchDescription"].ToString(),
                            Status = Convert.ToInt32(dataRow["Status"]),
                            DateTransaction = Convert.ToDateTime(dataRow["DateTransaction"]),
                            TotalCollect = Convert.ToDecimal(dataRow["TotalCollect"]),
                            PaymentMethodId = Convert.ToInt32(dataRow["PaymentMethodId"]),
                            PaymentAmount = Convert.ToDecimal(dataRow["Amount"]),
                            PaymentMethod = dataRow["PaymentMethod"].ToString(),
                            CollectDescription = dataRow["Description"].ToString(),
                            CollectConceptId = collectingConcept,
                            CollectConceptDescription = dataRow["CollectConceptDescription"].ToString(),
                            IncomeAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                            Amount = Convert.ToDecimal(dataRow["Amount"]),
                            ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                            Holder = dataRow["Holder"].ToString(),
                            DocumentNumber = dataRow["DocumentNumber"].ToString(),
                            CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                            CurrencyDescription = dataRow["CurrencyDescription"].ToString(),
                            PayerId = Convert.ToInt32(dataRow["IndividualId"]),
                            CollectNumber = Convert.ToInt32(dataRow["CollectNumber"]),
                            TechnicalTransaction = String.IsNullOrEmpty(dataRow["TechnicalTransaction"].ToString()) ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"]),
                            AccountingDate = Convert.ToDateTime(dataRow["AccountingDate"]?.ToString())
                        });
                    }

                    #endregion
                }
                if (uiviewName == "CollectingView")
                {
                    #region LoadBillingReportPaymentDTO

                    foreach (DataRow dataRow in reports.Rows)
                    {
                        reportCollectDTOs.Add(new SEARCH.ReportCollectDTO()
                        {
                            CollectId = Convert.ToInt32(dataRow["CollectId"]),
                            Description = dataRow["Description"].ToString(),
                            AccountNumber = dataRow["IssuingAccountNumber"].ToString(),
                            DocumentNumber = dataRow["DocumentNumber"].ToString(),
                            CurrencyId = Convert.ToInt32(dataRow["CurrencyId"]),
                            CurrencyDescription = dataRow["CurrencyDescription"].ToString(),
                            Amount = Convert.ToDecimal(dataRow["Amount"]),
                            IncomeAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                            ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                            PaymentDescription = dataRow["PaymentDescription"].ToString(),
                            PaymentId = Convert.ToInt32(dataRow["PaymentId"]),
                            PayerId = Convert.ToInt32(dataRow["IndividualId"]),
                            Status = Convert.ToInt32(dataRow["Status"]),
                            UserId = Convert.ToInt32(dataRow["UserId"]),
                            BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                            AccountingDate = Convert.ToDateTime(dataRow["AccountingDate"])
                        });
                    }

                    #endregion
                }

                return reportCollectDTOs;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region RegularizedPayment

        ///<summary>
        /// SaveRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="billIdFinal"></param>
        /// <returns>int</returns>
        public int SaveRegularizedPayment(int regularizedPaymentId, int paymentIdSource, int billIdFinal)
        {
            try
            {
                return _regularizedPaymentDAO.SaveRegularizedPayment(regularizedPaymentId, paymentIdSource, billIdFinal);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        /// <summary>
        /// UpdateRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="billIdFinal"></param>
        /// <returns>int</returns>
        public int UpdateRegularizedPayment(int regularizedPaymentId, int paymentIdSource, int billIdFinal)
        {
            try
            {
                return _regularizedPaymentDAO.UpdateRegularizedPayment(regularizedPaymentId, paymentIdSource, billIdFinal);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        /// <summary>
        /// DeleteRegularizedPayment
        /// </summary>
        /// <param name="regularizedPaymentId"></param>
        public void DeleteRegularizedPayment(int regularizedPaymentId)
        {
            try
            {
                _regularizedPaymentDAO.DeleteRegularizedPayment(regularizedPaymentId);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// Graba regularizacion y contabilidad
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="billControlId"></param>
        /// <param name="sourcePaymentId"></param>
        /// <param name="branchId"></param>
        /// <param name="accountingDate"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public MessageSuccessDTO SaveRegularizationCollect(CollectDTO collect, int billControlId, int sourcePaymentId, int branchId, DateTime accountingDate, int userId)
        {

            try
            {
                // Save Collect
                bool showMessage = false;
                CollectDTO newCollect = DelegateService.accountingCollectService.SaveRegularizationCollect(collect, billControlId, sourcePaymentId, branchId);

                int technicalTransactionForRegularize = DelegateService.accountingCollectService.GetTechnicalTransactionByPaymentId(sourcePaymentId);
                string recordCollectMessage = "";
                int bridgeRegularizate = (int)DelegateService.commonService.GetParameterByDescription(ENUMACC.AccountingKeys.ACC_CHECK_REGULARIZED.ToString()).NumberParameter;
                #region accounting

                bool generalLedgerSuccess = true;
                if ((string)UTILHELPER.EnumHelper.GetEnumParameterValue<ENUMACC.AccountingKeys>(ENUMACC.AccountingKeys.ACC_ENABLED_GENERAL_LEGDER) == "true")
                {
                    AccountingPaymentBusiness accountingPaymentBusiness = new AccountingPaymentBusiness();
                    var message = accountingPaymentBusiness.ProcessAccountingCheck(newCollect.ToModel(), accountingDate, userId, technicalTransactionForRegularize, sourcePaymentId, Convert.ToInt32(PaymentStatus.Regularized), bridgeRegularizate);
                    generalLedgerSuccess = message.GeneralLedgerSuccess;
                    recordCollectMessage = message.Info;
                    showMessage = true;
                }
                else
                {
                    recordCollectMessage = Convert.ToString(Resources.Resources.IntegrationServiceDisabledLabel);
                    showMessage = false;
                }
                #endregion
                //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
                Models.Collect.Collect movementToControl = new Models.Collect.Collect()
                {
                    Id = newCollect.Id
                };
                Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
                integration2GBusiness.Save(movementToControl.ToModelIntegration(1));

                MessageSuccessDTO messageSuccessDTO = new MessageSuccessDTO()
                {
                    ImputationMessage = recordCollectMessage,
                    TechnicalTransaction = newCollect.Transaction.TechnicalTransaction,
                    ShowMessage = true,
                    BillId = newCollect.Id,
                    GeneralLedgerSuccess = generalLedgerSuccess
                };
                return messageSuccessDTO;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region SearchCheck

        /// <summary>
        /// GetChecks
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="checkNumber"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        public List<SEARCH.CheckInformationDTO> GetChecks(int paymentCode, int bankCode, string accountNumber, string checkNumber)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentCode);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankCode);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingAccountNumber, accountNumber);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DocumentNumber, checkNumber);

                List<ACCOUNTINGEN.CollectV> getCollectViews = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.CollectV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.CollectV>().ToList();

                List<SEARCH.CheckInformationDTO> checkInformationDTOs = new List<SEARCH.CheckInformationDTO>();

                foreach (ACCOUNTINGEN.CollectV collectView in getCollectViews)
                {
                    SEARCH.CheckInformationDTO checkInformationDTO = new SEARCH.CheckInformationDTO();
                    checkInformationDTO.PaymentCode = collectView.PaymentCode;
                    checkInformationDTO.PaymentMethodTypeCode = collectView.PaymentMethodTypeCode == 0 ? -1 : Convert.ToInt32(collectView.PaymentMethodTypeCode);
                    checkInformationDTO.PayerId = collectView.IndividualId == 0 ? -1 : Convert.ToInt32(collectView.IndividualId);
                    checkInformationDTO.Name = collectView.Name;
                    checkInformationDTO.DatePayment = Convert.ToString(collectView.DatePayment);
                    checkInformationDTO.CurrencyCode = Convert.ToInt32(collectView.CurrencyCode);
                    checkInformationDTO.CurrencyDescription = collectView.CurrencyDescription;
                    checkInformationDTO.Amount = collectView.Amount == 0 ? 0 : Convert.ToDecimal(collectView.Amount);
                    checkInformationDTO.Holder = collectView.Holder;
                    checkInformationDTO.CollectCode = collectView.CollectId == 0 ? -1 : (int)collectView.CollectId;
                    checkInformationDTO.CollectConceptCode = collectView.CollectConceptId == 0 ? -1 : Convert.ToInt32(collectView.CollectConceptId);
                    checkInformationDTO.CollectConceptDescription = collectView.CollectConceptDescription;
                    checkInformationDTO.RegisterDate = Convert.ToString(collectView.RegisterDate);
                    checkInformationDTO.Status = collectView.Status == 0 ? -1 : Convert.ToInt32(collectView.Status);
                    checkInformationDTO.StatusDescription = collectView.StatusDescription;
                    checkInformationDTO.IssuingBankCode = collectView.IssuingBankCode == 0 ? -1 : Convert.ToInt32(collectView.IssuingBankCode);
                    checkInformationDTO.DocumentNumber = collectView.DocumentNumber;
                    checkInformationDTO.ReceivingAccountNumber = collectView.IssuingAccountNumber;
                    checkInformationDTO.BranchCode = collectView.BranchCode;
                    checkInformationDTO.BranchDescription = collectView.BranchDescription;
                    checkInformationDTOs.Add(checkInformationDTO);
                }
                return checkInformationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetInternalBallotInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<SEARCH.DepositCheckInformationDTO></returns>
        public List<SEARCH.DepositCheckInformationDTO> GetInternalBallotInformation(int paymentCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentCode);

                UIView internalBallots = _dataFacadeManager.GetDataFacade().GetView("InternalPaymentTicketView",
                    criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                List<SEARCH.DepositCheckInformationDTO> depositCheckInformationDTOs = new List<SEARCH.DepositCheckInformationDTO>();

                foreach (DataRow dataRow in internalBallots.Rows)
                {
                    depositCheckInformationDTOs.Add(new SEARCH.DepositCheckInformationDTO()
                    {
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? "" : string.Format("{0:dd/MM/yyyy}", dataRow["RegisterDate"]),
                        PaymentTicketCode = dataRow["PaymentTicketCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        BankCode = dataRow["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        ReceivingAccountNumber = dataRow["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["AccountNumber"])
                    });
                }

                return depositCheckInformationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDepositInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<DepositCheckInformationDTO/></returns>
        public List<SEARCH.DepositCheckInformationDTO> GetDepositInformation(int paymentCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentCode);

                UIView deposits = _dataFacadeManager.GetDataFacade().GetView("CollectPaymentTicketView",
                    criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                List<SEARCH.DepositCheckInformationDTO> depositCheckInformationDTOs = new List<SEARCH.DepositCheckInformationDTO>();

                foreach (DataRow dataRow in deposits.Rows)
                {
                    depositCheckInformationDTOs.Add(new SEARCH.DepositCheckInformationDTO()
                    {
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? "" : string.Format("{0:dd/MM/yyyy}", dataRow["RegisterDate"]),
                        PaymentTicketCode = dataRow["PaymentTicketCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        BankCode = dataRow["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        ReceivingAccountNumber = dataRow["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["AccountNumber"]),
                        DepositRegisterDate = dataRow["DepositRegisterDate"] == DBNull.Value ? "" : string.Format("{0:dd/MM/yyyy}", dataRow["DepositRegisterDate"]),
                        PaymentBallotNumber = dataRow["PaymentBallotNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PaymentBallotNumber"]),
                        PaymentBallotCode = dataRow["PaymentBallotCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentBallotCode"])
                    });
                }

                return depositCheckInformationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRejectedInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<RejectedPaymentDTO/></returns>
        public List<SEARCH.RejectedPaymentDTO> GetRejectedInformation(int paymentCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentCode);

                UIView rejections = _dataFacadeManager.GetDataFacade().GetView("RejectedPaymentView",
                                                 criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                List<SEARCH.RejectedPaymentDTO> rejectionPaymentDTOs = new List<SEARCH.RejectedPaymentDTO>();

                foreach (DataRow dataRow in rejections.Rows)
                {
                    rejectionPaymentDTOs.Add(new SEARCH.RejectedPaymentDTO()
                    {
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        RejectionDate = dataRow["RejectionDate"] == DBNull.Value ? "" : string.Format("{0:dd/MM/yyyy}", dataRow["RejectionDate"]),
                        RejectionDescription = dataRow["Description"] == DBNull.Value ? "" : Convert.ToString(dataRow["Description"]),
                        Comission = dataRow["Comission"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["Comission"]),
                        TaxComission = dataRow["TaxComission"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["TaxComission"]),
                        RejectedPaymentCode = dataRow["RejectedPaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RejectedPaymentCode"])
                    });
                }

                return rejectionPaymentDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRegularizedInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<RegularizedPaymentDTO/></returns>
        public List<SEARCH.RegularizedPaymentDTO> GetRegularizedInformation(int paymentCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentCode);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals("PaymentStatus", Convert.ToInt32(PaymentStatus.Regularized));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals("LogStatus", Convert.ToInt32(ActionTypes.PayRegularized));

                UIView regularizations = _dataFacadeManager.GetDataFacade().GetView("PaymentRegularizePaymentView",
                                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                List<SEARCH.RegularizedPaymentDTO> regularizedPaymentDTOs = new List<SEARCH.RegularizedPaymentDTO>();

                foreach (DataRow dataRow in regularizations.Rows)
                {
                    regularizedPaymentDTOs.Add(new SEARCH.RegularizedPaymentDTO()
                    {
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        PaymentStatus = dataRow["PaymentStatus"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentStatus"]),
                        LogStatus = dataRow["LogStatus"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LogStatus"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? "" : string.Format("{0:dd/MM/yyyy}", dataRow["RegisterDate"]),
                        RegularizePaymentCode = dataRow["RegularizePaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RegularizePaymentCode"]),
                        SourceCode = dataRow["SourceCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SourceCode"])
                    });
                }

                return regularizedPaymentDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetLegalInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<LegalPaymentDTO/></returns>
        public List<SEARCH.LegalPaymentDTO> GetLegalInformation(int paymentCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentCode);

                UIView legalizations = _dataFacadeManager.GetDataFacade().GetView("RejectedLegalPaymentView",
                         criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                List<SEARCH.LegalPaymentDTO> legalPaymentDTOs = new List<SEARCH.LegalPaymentDTO>();

                foreach (DataRow dataRow in legalizations.Rows)
                {
                    legalPaymentDTOs.Add(new SEARCH.LegalPaymentDTO()
                    {
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        Date = (dataRow["LegalDate"]).ToString(),
                        LegalPaymentCode = dataRow["LegalPaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LegalPaymentCode"])
                    });
                }

                return legalPaymentDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region GetChecksUpdated

        /// <summary>
        /// GetChecksUpdated
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="checkNumber"></param>
        /// <param name="accountNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        public List<SEARCH.CheckInformationDTO> GetChecksUpdated(int bankCode, string checkNumber, string accountNumber, int technicalTransaction, int branchCode)

        {
            try
            {
                #region LoadUIVIEW

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Filtro por banco receptor y método de pago cheques
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankCode);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode);
                criteriaBuilder.In();
                criteriaBuilder.ListValue();
                criteriaBuilder.Constant(1);
                criteriaBuilder.Constant(2);
                criteriaBuilder.Constant(25);
                criteriaBuilder.EndList();

                // Por número de cheque
                if (checkNumber != "-1")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DocumentNumber, checkNumber);
                }

                // Por banco receptor
                if (accountNumber != "-1")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingAccountNumber, accountNumber);
                }

                // Por número transacción técnica
                if (technicalTransaction != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, technicalTransaction);
                }

                // Por Sucursal
                if (branchCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchCode);
                }

                UIView checks = _dataFacadeManager.GetDataFacade().GetView("PaymentCheckDepositView",
                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (checks.Rows.Count > 0)
                {
                    checks.Columns.Add("Rows", typeof(int));
                    checks.Rows[0]["Rows"] = rows;
                }
                #endregion

                #region LoadDTO

                List<SEARCH.CheckInformationDTO> checkInformationDTOs = new List<SEARCH.CheckInformationDTO>();

                foreach (DataRow dataRow in checks.Rows)
                {
                    checkInformationDTOs.Add(new SEARCH.CheckInformationDTO()
                    {
                        CollectCode = dataRow["CollectCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectCode"]),
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        IssuingBankCode = dataRow["IssuingBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IssuingBankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        ReceivingAccountNumber = dataRow["IssuingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuingAccountNumber"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        DatePayment = dataRow["DatePayment"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", dataRow["DatePayment"]),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        Status = dataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Status"]),
                        StatusDescription = dataRow["StatusDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["StatusDescription"]),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : Convert.ToString(dataRow["Holder"]),
                        PaymentMethodTypeCode = dataRow["PaymentMethodTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        ExchangeRate = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        Rows = dataRow["Rows"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Rows"])
                    });
                }

                #endregion

                return checkInformationDTOs.OrderBy(o => o.BranchCode).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveChangeCheck
        /// Cambia el estado del cheque original a canjeado e inserta el nuevo cheque en la tabla BILL.PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="collectCode"></param>
        /// <param name="userId"></param>
        /// <param name="payerId"></param>
        /// <param name="changeDescription"></param>
        /// <returns>int</returns>
        public MessageSuccessDTO SaveChangeCheck(PAY.PaymentDTO payment, int paymentIdSource, int collectCode, int userId, int payerId, string changeDescription, DateTime accountingDate)
        {
            try
            {
                int collectId = 0;
                int paymentId = 0;
                string recordCollectMessage = "";
                bool showMessage = true;

                collectId = collectCode;
                paymentId = paymentIdSource;

                // Cambia de estado el cheque a canjeado
                _paymentDAO.UpdatePaymentStatusById(paymentId, Convert.ToInt32(PaymentStatus.Exchanged));

                // Graba PaymentLog
                SavePaymentLog(Convert.ToInt32(ActionTypes.ExchangePayment),
                               paymentId,
                               paymentId,
                               Convert.ToInt32(PaymentStatus.Exchanged),
                               userId);

                Collect newCollect = new AccountingCollectServiceEEProvider().ReplicateCheckinCollect(collectId, changeDescription, payerId, 1, payment.ToModel(), Convert.ToInt32(PaymentStatus.Active), payment.BranchId);
                int technicalTransactionForChange = new AccountingCollectServiceEEProvider().GetTechnicalTransactionByPaymentId(payment.Id);
                int bridgeChange = (int)DelegateService.commonService.GetParameterByDescription(ENUMACC.AccountingKeys.ACC_CHECK_CHANGE.ToString()).NumberParameter;

                bool generalLedgerSuccess = true;
                if ((string)UTILHELPER.EnumHelper.GetEnumParameterValue<ENUMACC.AccountingKeys>(ENUMACC.AccountingKeys.ACC_ENABLED_GENERAL_LEGDER) == "true")
                {
                    AccountingPaymentBusiness accountingPaymentBusiness = new AccountingPaymentBusiness();
                    var message = accountingPaymentBusiness.ProcessAccountingCheck(newCollect, accountingDate, userId, technicalTransactionForChange, payment.Id, Convert.ToInt32(PaymentStatus.Exchanged), bridgeChange);
                    generalLedgerSuccess = message.GeneralLedgerSuccess;
                    recordCollectMessage = message.Info;
                    showMessage = false;
                }
                else
                {
                    recordCollectMessage = Convert.ToString(Resources.Resources.IntegrationServiceDisabledLabel);
                    showMessage = false;
                }


                // Se obtiene el paymentId
                if (newCollect.Payments.FirstOrDefault() != null && newCollect.Payments.FirstOrDefault().Id > 0)
                {
                    int exchangePaymentId = _exchangePaymentDAO.SaveExchangePayment(0, paymentIdSource, newCollect.Payments.FirstOrDefault().Id);

                    // Graba el LogPayment para el nuevo cheque
                    SavePaymentLog(Convert.ToInt32(ActionTypes.CreatePayment), exchangePaymentId,
                                   newCollect.Payments.FirstOrDefault().Id, Convert.ToInt32(PaymentStatus.Active), userId);

                }
                else
                {
                    ArrayList payments = GetPaymentByBillId(newCollect.Id);

                    int newPaymentId = 0;

                    foreach (ACCOUNTINGEN.Payment paymentEntity in payments)
                    {
                        newPaymentId = paymentEntity.PaymentCode;
                    }
                    int exchangePaymentId = _exchangePaymentDAO.SaveExchangePayment(0, paymentIdSource, newPaymentId);

                    // Graba el LogPayment para el nuevo cheque
                    SavePaymentLog(Convert.ToInt32(ActionTypes.CreatePayment), exchangePaymentId,
                                   newPaymentId, Convert.ToInt32(PaymentStatus.Active), userId);
                }

                //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
                Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
                integration2GBusiness.Save(newCollect.ToModelIntegration(1));

                MessageSuccessDTO messageSuccessDTO = new MessageSuccessDTO()
                {
                    ImputationMessage = recordCollectMessage,
                    TechnicalTransaction = newCollect.Transaction.TechnicalTransaction,
                    ShowMessage = showMessage,
                    BillId = newCollect.Id,
                    GeneralLedgerSuccess = generalLedgerSuccess
                };
                return messageSuccessDTO;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CardVoucher

        /// <summary>
        /// GetCardVoucher
        /// Carga la UIView GetCardVoucher a su respectivo DTO
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        ///  <returns>List<CardVoucherDTO/></returns>
        public List<SEARCH.CardVoucherDTO> GetCardVoucher(int creditCardTypeCode, string voucher, long documentNumber, int technicalTransaction,
                                                   int branchCode, DateTime startDate, DateTime endDate, int status)
        {

            try
            {

                #region LoadDTO

                List<SEARCH.CardVoucherDTO> cardVoucherDTOs = new List<SEARCH.CardVoucherDTO>();

                UIView cardVouchers = LoadCardVoucher(creditCardTypeCode, voucher, documentNumber, technicalTransaction,
                                                         branchCode, startDate, endDate, status);

                foreach (DataRow dataRow in cardVouchers.Rows)
                {
                    decimal taxes = 0;
                    decimal commission = 0;

                    if (dataRow["Taxes"] != DBNull.Value)
                    {
                        taxes = Convert.ToDecimal(dataRow["Taxes"]);
                    }

                    if (dataRow["Commission"] != DBNull.Value)
                    {
                        commission = Convert.ToDecimal(dataRow["Commission"]);
                    }

                    cardVoucherDTOs.Add(new SEARCH.CardVoucherDTO()
                    {
                        IncomeAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        CollectCode = Convert.ToInt32(dataRow["CollectCode"]),
                        BranchCode = Convert.ToInt32(dataRow["BranchCode"]),
                        CardDate = String.Format("{0:dd/MM/yyyy}", dataRow["CardDate"]),
                        CardDescription = dataRow["CardDescription"].ToString(),
                        CreditCardTypeCode = Convert.ToInt32(dataRow["CreditCardTypeCode"]),
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"].ToString(),
                        Description = dataRow["Description"].ToString(),
                        DocumentNumber = dataRow["DocumentNumber"].ToString(),
                        PaymentCode = Convert.ToInt32(dataRow["PaymentCode"]),
                        PaymentDate = Convert.ToDateTime(dataRow["DatePayment"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        StatusDescription = dataRow["StatusDescription"].ToString(),
                        Taxes = taxes,
                        Retention = dataRow["Retention"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Retention"]),
                        Commission = commission,
                        Voucher = dataRow["Voucher"].ToString(),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"]),
                    });
                }

                #endregion

                return cardVoucherDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRejectedCardVoucherInfoByPaymentId 
        /// Autor: Alejandro Villagrán
        /// Desc: Obtiene los datos para mostrar en regularizacion de tarjetas
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>RejectedCardVoucherInfoDTO</returns>
        public SEARCH.RejectedCardVoucherInfoDTO GetRejectedCardVoucherInfoByPaymentId(int paymentId)
        {
            try
            {

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentId);

                UIView cardVouchers = _dataFacadeManager.GetDataFacade().GetView("CardVoucherView",
                                             criteriaBuilder.GetPredicate(), null, 0, 1, null, true, out int rows);

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, PaymentStatus.Rejected);


                List<ACCOUNTINGEN.GetRejectedPaymentV> dataRejections = _dataFacadeManager.GetDataFacade().List(
                    typeof(ACCOUNTINGEN.GetRejectedPaymentV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetRejectedPaymentV>().ToList();


                SEARCH.RejectedCardVoucherInfoDTO rejectedCardVoucherInfoDTO = new SEARCH.RejectedCardVoucherInfoDTO();

                rejectedCardVoucherInfoDTO.CreditCardTypeCode = Convert.ToInt32(cardVouchers.Rows[0]["CreditCardTypeCode"]);
                rejectedCardVoucherInfoDTO.CardDescription = Convert.ToString(cardVouchers.Rows[0]["CardDescription"]);
                rejectedCardVoucherInfoDTO.Voucher = Convert.ToString(cardVouchers.Rows[0]["Voucher"]);
                rejectedCardVoucherInfoDTO.CollectCode = Convert.ToInt32(cardVouchers.Rows[0]["CollectCode"]);
                rejectedCardVoucherInfoDTO.CardDate = Convert.ToDateTime(cardVouchers.Rows[0]["CardDate"]);
                rejectedCardVoucherInfoDTO.CurrencyCode = Convert.ToInt32(cardVouchers.Rows[0]["CurrencyCode"]);
                rejectedCardVoucherInfoDTO.CurrencyDescription = Convert.ToString(cardVouchers.Rows[0]["CurrencyDescription"]);
                rejectedCardVoucherInfoDTO.Amount = Convert.ToDecimal(cardVouchers.Rows[0]["Amount"]);
                rejectedCardVoucherInfoDTO.Tax = cardVouchers.Rows[0]["Taxes"] == DBNull.Value ? 0 : Convert.ToDouble(cardVouchers.Rows[0]["Taxes"]);
                rejectedCardVoucherInfoDTO.Status = Convert.ToInt32(cardVouchers.Rows[0]["Status"]);
                rejectedCardVoucherInfoDTO.StatusDescription = Convert.ToString(cardVouchers.Rows[0]["StatusDescription"]);

                if (dataRejections.Count > 0)
                {
                    rejectedCardVoucherInfoDTO.RejectionDate = String.Format("{0:dd/MM/yyyy}", dataRejections[0].RejectionDate);
                    rejectedCardVoucherInfoDTO.RejectedPaymentCode = dataRejections[0].RejectedPaymentCode;
                    rejectedCardVoucherInfoDTO.RejectionDescription = dataRejections[0].RejectionDescription;
                    rejectedCardVoucherInfoDTO.Holder = dataRejections[0].Holder;
                    rejectedCardVoucherInfoDTO.DocumentNumber = dataRejections[0].DocumentNumber;
                }

                return rejectedCardVoucherInfoDTO;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRejectedCheckInfoByPaymentId 
        /// Obtiene los datos para mostrar en regularizacion de cheques
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>RejectedPaymentDTO</returns>
        public SEARCH.RejectedPaymentDTO GetRejectedCheckInfoByPaymentId(int paymentId)
        {

            try
            {

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentId);

                UIView checks = _dataFacadeManager.GetDataFacade().GetView("PaymentCheckDepositView",
                                                        criteriaBuilder.GetPredicate(), null, 0, 1, null, true, out int rows);

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, paymentId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, PaymentStatus.Rejected);

                List<ACCOUNTINGEN.GetRejectedPaymentV> dataRejections = _dataFacadeManager.GetDataFacade().List(
                   typeof(ACCOUNTINGEN.GetRejectedPaymentV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.GetRejectedPaymentV>().ToList();


                SEARCH.RejectedPaymentDTO rejectedPaymentDTO = new SEARCH.RejectedPaymentDTO();

                if (dataRejections.Count > 0)
                {
                    rejectedPaymentDTO.RejectedPaymentCode = dataRejections[0].RejectedPaymentCode;
                    rejectedPaymentDTO.PaymentCode = Convert.ToInt32(checks.Rows[0]["PaymentCode"]);
                    rejectedPaymentDTO.IssuingBankCode = Convert.ToInt32(checks.Rows[0]["IssuingBankCode"]);
                    rejectedPaymentDTO.IssuingAccountNumber = Convert.ToString(checks.Rows[0]["IssuingAccountNumber"]);
                    rejectedPaymentDTO.BankDescription = Convert.ToString(checks.Rows[0]["BankDescription"]);
                    rejectedPaymentDTO.DocumentNumber = Convert.ToString(checks.Rows[0]["DocumentNumber"]);
                    rejectedPaymentDTO.DatePayment = String.Format("{0:dd/MM/yyyy}", checks.Rows[0]["DatePayment"]);
                    rejectedPaymentDTO.CurrencyCode = Convert.ToInt32(checks.Rows[0]["CurrencyCode"]);
                    rejectedPaymentDTO.CurrencyDescription = Convert.ToString(checks.Rows[0]["CurrencyDescription"]);
                    rejectedPaymentDTO.Amount = Convert.ToDecimal(checks.Rows[0]["Amount"]);
                    rejectedPaymentDTO.ExchangeRate = Convert.ToDecimal(checks.Rows[0]["ExchangeRate"]);
                    rejectedPaymentDTO.Holder = Convert.ToString(checks.Rows[0]["Holder"]);
                    rejectedPaymentDTO.CollectCode = Convert.ToInt32(checks.Rows[0]["CollectCode"]);
                    rejectedPaymentDTO.RejectionDate = String.Format("{0:dd/MM/yyyy}", dataRejections[0].RejectionDate);
                    rejectedPaymentDTO.RejectionDescription = dataRejections[0].RejectionDescription;
                    rejectedPaymentDTO.Status = Convert.ToInt32(checks.Rows[0]["Status"]);
                    rejectedPaymentDTO.StatusDescription = Convert.ToString(checks.Rows[0]["StatusDescription"]);
                    rejectedPaymentDTO.CollectConceptCode = Convert.ToInt32(checks.Rows[0]["CollectConceptCode"]);
                }

                return rejectedPaymentDTO;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region StatusPayment

        /// <summary>
        /// GetTaxInformationByPaimentId
        /// Obtiene los impuestos de la tabla de dado el methodTypeId
        /// </summary>
        /// <param name="methodTypeId"></param>
        /// <returns>List<StatusPaymentDTO/></returns>
        public List<SEARCH.StatusPaymentDTO> GetStatusPaymentByMethodTypeId(int methodTypeId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.StatusPayment.Properties.PaymentMethodTypeCode, methodTypeId);

                UIView statusPayments = _dataFacadeManager.GetDataFacade().GetView("StatusPaymentView",
                                                 criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (statusPayments.Rows.Count > 0)
                {
                    statusPayments.Columns.Add("rows", typeof(int));
                    statusPayments.Rows[0]["rows"] = rows;
                }

                List<SEARCH.StatusPaymentDTO> statusPaymentDTOs = new List<SEARCH.StatusPaymentDTO>();

                foreach (DataRow dataRow in statusPayments)
                {
                    statusPaymentDTOs.Add(new SEARCH.StatusPaymentDTO()
                    {
                        PaymentMethodTypeCode = Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        MethodDescription = Convert.ToString(dataRow["MethodDescription"]),
                        StatusDescription = Convert.ToString(dataRow["StatusDescription"])
                    });
                }

                return statusPaymentDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateStatusPayment
        /// Actualiza la tabla StatusPayment
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="status"></param>
        /// <param name="description"></param>
        public void UpdateStatusPayment(int methodId, int status, string description)
        {
            try
            {
                _paymentDAO.UpdateStatusPayment(methodId, status, description);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteStatusPayment
        /// Elimina un registro de la tabla StatusPayment
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="status"></param>
        public void DeleteStatusPayment(int methodId, int status)
        {
            try
            {
                _paymentDAO.DeleteStatusPayment(methodId, status);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Payment

        /// <summary>
        /// GetPaymentByBillId
        /// Recupera un registro de la tabla Payment por el BillId
        /// Autor:David Delgado
        /// </summary>
        /// <returns>ArrayList</returns>
        public ArrayList GetPaymentByBillId(int billId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.CollectCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(billId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection
                    (_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                ArrayList payment = new ArrayList();
                foreach (ACCOUNTINGEN.Payment paymentEntity in businessCollection.OfType<ACCOUNTINGEN.Payment>())
                {
                    payment.Add(paymentEntity);
                }

                return payment;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCardVoucher
        /// Carga la UIView GetInformationPayment a su respectivo DTO
        /// </summary>
        /// <param name="paymentCode"></param>
        ///  <returns>List<InformationPaymentDTO/></returns>
        public List<SEARCH.InformationPaymentDTO> GetInformationPayment(int paymentCode)
        {
            try
            {
                #region LoadDTO

                List<SEARCH.InformationPaymentDTO> informationPaymentDTOs = new List<SEARCH.InformationPaymentDTO>();
                UIView payments = LoadInformationPayment(paymentCode);

                foreach (DataRow dataRow in payments.Rows)
                {
                    informationPaymentDTOs.Add(new SEARCH.InformationPaymentDTO()
                    {
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Amount"]),
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["IncomeAmount"]),
                        CollectCode = dataRow["CollectCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CollectCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : dataRow["BranchDescription"].ToString(),
                        BankCode = dataRow["BankCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : dataRow["BankDescription"].ToString(),
                        CollectConceptCode = dataRow["CollectConceptId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CollectConceptId"]),
                        CollectConceptDescription = dataRow["CollectConceptDescription"] == DBNull.Value ? "" : dataRow["CollectConceptDescription"].ToString(),
                        CreditCardTypeCode = dataRow["CreditCardTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CreditCardTypeCode"]),
                        CardDate = dataRow["CardDate"] == DBNull.Value ? "" : dataRow["CardDate"].ToString(),
                        Description = dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(), // CardDescription
                        DatePayment = dataRow["DatePayment"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", dataRow["DatePayment"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : dataRow["DocumentNumber"].ToString(),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : dataRow["CurrencyDescription"].ToString(),
                        AuthorizationNumber = dataRow["AuthorizationNumber"] == DBNull.Value ? "" : dataRow["AuthorizationNumber"].ToString(),
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentCode"]),
                        ExchangeRate = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ExchangeRate"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["IndividualId"]),
                        IssuingAccountNumber = dataRow["IssuingAccountNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["IssuingAccountNumber"]),
                        ValidMonth = dataRow["ValidMonth"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ValidMonth"]),
                        ValidYear = dataRow["ValidYear"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ValidYear"]),
                        Name = dataRow["Name"] == DBNull.Value ? "" : dataRow["Name"].ToString(),
                        StatusPayment = dataRow["StatusPayment"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["StatusPayment"]),
                        StatusDescription = dataRow["StatusDescription"] == DBNull.Value ? "" : dataRow["StatusDescription"].ToString(),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : dataRow["Holder"].ToString(),
                        PaymentMethodTypeCode = dataRow["PaymentMethodTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        ReceivingAccountNumber = dataRow["ReceivingAccountNumber"] == DBNull.Value ? "" : dataRow["ReceivingAccountNumber"].ToString(),
                        ReceivingBankCode = dataRow["ReceivingBankCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReceivingBankCode"]),
                        SerialNumber = dataRow["SerialNumber"] == DBNull.Value ? "" : dataRow["SerialNumber"].ToString(),
                        SerialVoucher = dataRow["SerialVoucher"] == DBNull.Value ? "" : dataRow["SerialVoucher"].ToString(),
                        Taxes = dataRow["Taxes"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Taxes"]),
                        Commission = dataRow["Commission"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Commission"]),
                        Voucher = dataRow["Voucher"] == DBNull.Value ? "" : dataRow["Voucher"].ToString(),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"])
                    });
                }

                #endregion

                return informationPaymentDTOs;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }


        }

        #endregion

        #region CheckingRejection

        /// <summary>
        /// GetCheckInformationByDocumentNumber
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns>List<InformationPaymentDTO/></returns>
        public List<SEARCH.CheckInformationDTO> GetCheckInformationByDocumentNumber(string documentNumber)
        {
            try
            {

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentCode, Convert.ToInt32(documentNumber));

                List<ACCOUNTINGEN.CheckInformationV> dataChecks = _dataFacadeManager.GetDataFacade().List(
                   typeof(ACCOUNTINGEN.CheckInformationV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.CheckInformationV>().ToList();

                #region LoadDTO

                List<SEARCH.CheckInformationDTO> checkInformationDTOs = new List<SEARCH.CheckInformationDTO>();

                foreach (ACCOUNTINGEN.CheckInformationV checkInformation in dataChecks)
                {
                    string datePayment = Convert.ToString(checkInformation.DatePayment == null ? "" : Convert.ToString(checkInformation.DatePayment));
                    if (datePayment != "")
                    {
                        int date = datePayment.IndexOf(" ");
                        datePayment = datePayment.Substring(0, date);
                    }

                    SEARCH.CheckInformationDTO checkInformationDTO = new SEARCH.CheckInformationDTO();
                    checkInformationDTO.DatePayment = datePayment;
                    checkInformationDTO.CurrencyDescription = checkInformation.CurrencyDescription == null ? "" : (string)checkInformation.CurrencyDescription;
                    checkInformationDTO.Amount = checkInformation.Amount == 0 ? 1 : (decimal)checkInformation.Amount;
                    checkInformationDTO.Holder = checkInformation.Holder == null ? "" : (string)checkInformation.Holder;
                    checkInformationDTO.BankDescription = checkInformation.BankDescription;
                    checkInformationDTO.ReceivingBankCode = checkInformation.ReceivingBankCode == 0 ? 1 : (int)checkInformation.ReceivingBankCode;
                    checkInformationDTO.ReceivingBankName = checkInformation.ReceivingBankName;
                    checkInformationDTO.Name = checkInformation.Name;
                    checkInformationDTO.DocumentNumber = checkInformation.DocumentNumber;
                    checkInformationDTO.IssuingAccountNumber = checkInformation.IssuingAccountNumber;
                    checkInformationDTO.CollectCode = checkInformation.CollectId == 0 ? -1 : (int)checkInformation.CollectId;
                    checkInformationDTO.PaymentCode = checkInformation.PaymentCode == 0 ? -1 : (int)checkInformation.PaymentCode;
                    checkInformationDTO.RegisterDate = Convert.ToString(checkInformation.RegisterDate == null ? "" : Convert.ToString(checkInformation.RegisterDate));
                    checkInformationDTO.PaymentBallotNumber = checkInformation.PaymentBallotNumber == null ? "" : (string)checkInformation.PaymentBallotNumber;
                    checkInformationDTO.PaymentBallotCode = checkInformation.PaymentBallotCode == 0 ? -1 : (int)checkInformation.PaymentBallotCode;
                    checkInformationDTO.ReceivingAccountNumber = checkInformation.ReceivingAccountNumber;
                    checkInformationDTO.PayerId = checkInformation.IndividualId == 0 ? -1 : (int)checkInformation.IndividualId;
                    checkInformationDTO.TechnicalTransaction = checkInformation.TechnicalTransaction == 0 ? -1 : (int)checkInformation.TechnicalTransaction;
                    checkInformationDTOs.Add(checkInformationDTO);
                }

                #endregion

                return checkInformationDTOs;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region PaymentTax

        /// <summary>
        /// SavePaymentTax
        /// </summary>
        /// <param name="paymentTax"></param>
        /// <param name="paymentId"></param>
        /// <returns>PaymentTax</returns>
        public PAY.PaymentTaxDTO SavePaymentTax(PAY.PaymentTaxDTO paymentTax, int paymentId)
        {
            try
            {
                return _paymentTaxDAO.SavePaymentTax(paymentTax.ToModel(), paymentId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePaymentTax
        /// Elimina un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="paymentTax"></param>
        public void DeletePaymentTax(PAY.PaymentTaxDTO paymentTax)
        {
            try
            {
                _paymentTaxDAO.DeletePaymentTax(paymentTax.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTaxInformationByPaimentId
        /// Obtiene los impuestos de la tabla de dado el paymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>List<TaxInformationDTO/></returns>
        public List<SEARCH.TaxInformationDTO> GetTaxInformationByPaimentId(int paymentId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTax.Properties.PaymentCode, paymentId);

                UIView taxesbyItemView = _dataFacadeManager.GetDataFacade().GetView("PaymentTaxView",
                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SEARCH.TaxInformationDTO> taxInformationDTOs = new List<SEARCH.TaxInformationDTO>();

                foreach (DataRow dataRow in taxesbyItemView)
                {
                    taxInformationDTOs.Add(new SEARCH.TaxInformationDTO()
                    {
                        PaymentTaxCode = Convert.ToInt32(dataRow["PaymentTaxCode"]),
                        PaymentCode = Convert.ToInt32(dataRow["PaymentCode"]),
                        TaxCode = Convert.ToInt32(dataRow["TaxCode"]),
                        TaxRate = Convert.ToDouble(dataRow["TaxRate"]),
                        TaxAmount = Convert.ToDouble(dataRow["TaxAmount"]),
                        TaxBase = Convert.ToDouble(dataRow["TaxBase"]),
                        Description = Convert.ToString(dataRow["Description"])
                    });
                }

                return taxInformationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region GetChecksDepositingPending

        /// <summary>
        /// GetChecksDepositingPending
        /// Obtiene los cheques pendientes de depositar
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        public List<SEARCH.CheckInformationDTO> GetChecksDepositingPending(int bankCode, DateTime startDate, DateTime endDate, int branchCode)
        {
            try
            {
                #region LoadUIVIEW

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (bankCode != -1)
                {
                    // Filtro por banco receptor y método de pago cheques
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankCode);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, 1);
                    criteriaBuilder.CloseParenthesis();
                    criteriaBuilder.Or();
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, bankCode);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, 2);
                    criteriaBuilder.CloseParenthesis();

                    // Status: 1 - ingresado
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, 1);
                }

                // Por fecha desde
                if (startDate != new DateTime(1900, 1, 1))
                {
                    if (bankCode == -1)
                    {
                        criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(startDate);
                    }
                    else
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(startDate);
                    }
                }

                // Por fecha hasta
                if (endDate != new DateTime(1900, 1, 1))
                {
                    if (startDate == new DateTime(1900, 1, 1) && bankCode == -1)
                    {
                        criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(endDate);
                    }
                    else
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                        criteriaBuilder.LessEqual();
                        criteriaBuilder.Constant(endDate);
                    }
                }

                // Por sucursal
                if (branchCode != -1)
                {
                    if (bankCode == -1 && endDate == new DateTime(1900, 1, 1))
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchCode);
                    }
                    else
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchCode);
                    }
                }

                UIView checks = _dataFacadeManager.GetDataFacade().GetView("PaymentCheckDepositView",
                                                                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (checks.Rows.Count > 0)
                {
                    checks.Columns.Add("Rows", typeof(int));
                    checks.Rows[0]["Rows"] = rows;
                }
                #endregion

                #region LoadDTO

                List<SEARCH.CheckInformationDTO> checkInformationDTOs = new List<SEARCH.CheckInformationDTO>();

                foreach (DataRow dataRow in checks.Rows)
                {
                    checkInformationDTOs.Add(new SEARCH.CheckInformationDTO()
                    {
                        CollectCode = dataRow["CollectCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectCode"]),
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        IssuingBankCode = dataRow["IssuingBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IssuingBankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        ReceivingAccountNumber = dataRow["IssuingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuingAccountNumber"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        DatePayment = dataRow["DatePayment"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", dataRow["DatePayment"]),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        Status = dataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Status"]),
                        StatusDescription = dataRow["StatusDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["StatusDescription"]),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : Convert.ToString(dataRow["Holder"]),
                        PaymentMethodTypeCode = dataRow["PaymentMethodTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        ExchangeRate = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"]),
                        Rows = dataRow["Rows"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Rows"])
                    });
                }

                #endregion

                return checkInformationDTOs.OrderBy(o => o.PaymentMethodTypeCode).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetReportChecksDepositingPending
        /// Obtiene cheques pendientes de depositar para la impresión
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="issuingBankCode"> </param>
        /// <param name="startDate"> </param>
        /// <param name="endDate"> </param>
        /// <param name="branchCode"> </param>
        /// <returns>List<CheckInformationDTO/></returns>
        public List<SEARCH.CheckInformationDTO> GetReportChecksDepositingPending(int userId, int issuingBankCode, string startDate, string endDate, int branchCode)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (issuingBankCode != -1)
                {
                    // Filtro por banco receptor y método de pago cheques
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, issuingBankCode);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, 1);
                    criteriaBuilder.CloseParenthesis();
                    criteriaBuilder.Or();
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, issuingBankCode);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, 2);
                    criteriaBuilder.CloseParenthesis();

                    // Status: 1 - ingresado
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, 1);
                }

                // Por fecha desde
                if (startDate != "")
                {
                    if (issuingBankCode == -1)
                    {
                        criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(FormatDateTime(startDate));
                    }
                    else
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                        criteriaBuilder.GreaterEqual();
                        criteriaBuilder.Constant(FormatDateTime(startDate));
                    }
                }
                // Por fecha hasta
                if (endDate != "")
                {
                    DateTime date = FormatDateTime(endDate);
                    DateTime dateHours = date.AddHours(23);
                    DateTime dateMinutes = dateHours.AddMinutes(59);
                    DateTime dateSeconds = dateMinutes.AddSeconds(59);

                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(dateSeconds);
                }
                //Por sucursal
                if (branchCode != -1)
                {
                    if (issuingBankCode == -1)
                    {
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchCode);
                    }
                    else
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchCode);
                    }
                }


                UIView checks = _dataFacadeManager.GetDataFacade().GetView("PaymentCheckDepositView",
                                                               criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                if (checks.Rows.Count > 0)
                {
                    checks.Columns.Add("Rows", typeof(int));
                    checks.Rows[0]["Rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SEARCH.CheckInformationDTO> checkInformationDTOs = new List<SEARCH.CheckInformationDTO>();

                foreach (DataRow dataRow in checks.Rows)
                {
                    checkInformationDTOs.Add(new SEARCH.CheckInformationDTO()
                    {
                        CollectCode = dataRow["CollectCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectCode"]),
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        IssuingBankCode = dataRow["IssuingBankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IssuingBankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        ReceivingAccountNumber = dataRow["IssuingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuingAccountNumber"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        DatePayment = dataRow["DatePayment"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", dataRow["DatePayment"]),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        Status = dataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Status"]),
                        StatusDescription = dataRow["StatusDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["StatusDescription"]),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : Convert.ToString(dataRow["Holder"]),
                        PaymentMethodTypeCode = dataRow["PaymentMethodTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        ExchangeRate = dataRow["ExchangeRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ExchangeRate"]),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"])
                    });
                }

                #endregion

                return checkInformationDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region GetCardDepositingPending

        /// <summary>
        /// GetReportCardsDepositingPending
        /// Obtiene las tarjetas pendientes de depositar para la impresión
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="collectCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns>List<CardVoucherDTO/></returns>
        public List<SEARCH.CardVoucherDTO> GetReportCardsDepositingPending(string creditCardTypeCode, string voucher, string documentNumber, string collectCode,
                                         string branchCode, string startDate, string endDate, string status)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por tipo de conducto
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.CreditCardTypeCode, Convert.ToInt32(creditCardTypeCode));

                if (voucher != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.Voucher, voucher);
                }
                if (documentNumber != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.DocumentNumber, documentNumber);
                }
                if (collectCode != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.CollectCode, Convert.ToInt32(collectCode));
                }
                if (branchCode != "-1")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.BranchCode, Convert.ToInt32(branchCode));
                }
                if (startDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.CardVoucher.Properties.DatePayment);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(startDate));
                }
                if (endDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.CardVoucher.Properties.DatePayment);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(endDate));
                }

                if (status != "-1")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.Status, Convert.ToInt32(status));
                }

                UIView cards = _dataFacadeManager.GetDataFacade().GetView("CardVoucherView",
                                            criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);


                if (cards.Rows.Count > 0)
                {
                    cards.Columns.Add("Rows", typeof(int));
                    cards.Rows[0]["Rows"] = rows;
                }

                #endregion

                #region LoadDTO

                IFormatProvider culture = new CultureInfo("es-EC", true);

                List<SEARCH.CardVoucherDTO> cardVoucherDTOs = new List<SEARCH.CardVoucherDTO>();

                foreach (DataRow dataRow in cards.Rows)
                {
                    decimal taxes = 0;
                    decimal commission = 0;

                    if (dataRow["Taxes"] != DBNull.Value)
                    {
                        taxes = Convert.ToDecimal(dataRow["Taxes"]);
                    }

                    if (dataRow["Commission"] != DBNull.Value)
                    {
                        commission = Convert.ToDecimal(dataRow["Commission"]);
                    }

                    cardVoucherDTOs.Add(new SEARCH.CardVoucherDTO()
                    {
                        IncomeAmount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                        CollectCode = dataRow["CollectCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectCode"]),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        CardDate = dataRow["CardDate"].ToString(),
                        CardDescription = dataRow["CardDescription"] == DBNull.Value ? "" : dataRow["CardDescription"].ToString(),
                        CreditCardTypeCode = dataRow["CreditCardTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CreditCardTypeCode"]),
                        CurrencyCode = dataRow["CurrencyCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Description = dataRow["Description"].ToString(),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        PaymentDate = dataRow["DatePayment"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dataRow["DatePayment"], culture),
                        Status = dataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Status"]),
                        StatusDescription = dataRow["StatusDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["StatusDescription"]),
                        Taxes = taxes,
                        Retention = dataRow["Retention"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Retention"]),
                        Commission = commission,
                        Voucher = dataRow["Voucher"] == DBNull.Value ? "" : dataRow["Voucher"].ToString(),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"])
                    });
                }

                #endregion

                return cardVoucherDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region ReprintCollect

        /// <summary>
        /// ItemsBill
        /// Trae ítems del recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>bool</returns>
        public bool ItemsCollect(int collectId)
        {
            try
            {
                bool isCollected;

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);

                BusinessCollection businessCollection = new BusinessCollection
                                   (_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    isCollected = true;
                }
                else
                {
                    isCollected = false;
                }

                return isCollected;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region TempPaymentRequestClaim

        ///<summary>
        ///SaveTempPaymentRequest
        ///Graba temporal orden de pago siniestros
        ///</summary>
        ///<param name="paymentRequestInfo"></param>
        ///<returns>int</returns>
        public int SaveTempPaymentRequest(PaymentRequestDTO paymentRequestInfo)
        {
            try
            {
                return _tempPaymentRequestDAO.SaveTempPaymentRequest(paymentRequestInfo.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ConvertTempPaymentRequestToPaymentRequest
        /// Convierte temporal orden de pago siniestros a real
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        /// <returns>PaymentRequest</returns>
        public PaymentRequestDTO ConvertTempPaymentRequestToPaymentRequest(int tempPaymentRequestId)
        {
            try
            {

                int[] payments = _tempPaymentRequestDAO.ConvertTempPaymentRequestToPaymentRequest(tempPaymentRequestId);

                AccountsPayables.PaymentRequest paymentRequest = new AccountsPayables.PaymentRequest()
                {
                    Id = payments[0],
                    PaymentRequestNumber = new AccountsPayables.PaymentRequestNumber() { Number = payments[1] },
                    MovementType = new MovementType()
                    {
                        ConceptSource = new ConceptSource() { Id = payments[2] }
                    }
                };

                return paymentRequest.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Cards

        /// <summary>
        /// GetTaxCreditCard
        /// </summary>
        /// <param name="creditCardTypeId"></param>
        /// <param name="branchId"></param>
        /// <returns>Payment</returns>
        public PAY.PaymentDTO GetTaxCreditCard(int creditCardTypeId, int branchId)
        {

            try
            {

                UIView cardTaxes = LoadGetTaxCard(creditCardTypeId, branchId);

                PaymentModels.CreditCard creditCard = new PaymentModels.CreditCard();

                creditCard.Taxes = new List<PaymentModels.PaymentTax>();

                foreach (DataRow dataRow in cardTaxes.Rows)
                {
                    PaymentModels.PaymentTax paymentTax = new PaymentModels.PaymentTax();

                    // Payment
                    creditCard.Type = new PaymentModels.CreditCardType();
                    creditCard.Type.Commission = Convert.ToDecimal(dataRow["Commission"]);
                    creditCard.Type.Id = Convert.ToInt32(dataRow["CreditCardTypeCode"]);
                    creditCard.Type.Description = dataRow["Duct"].ToString();

                    // PaymentTax
                    paymentTax.Branch = new Branch();
                    paymentTax.Branch.Id = Convert.ToInt32(dataRow["BranchCode"]);
                    paymentTax.Branch.Description = dataRow["BranchDescription"].ToString();
                    paymentTax.Rate = Convert.ToDecimal(dataRow["Rate"]);

                    // Tax
                    Tax tax = new Tax();
                    tax.Id = Convert.ToInt32(dataRow["TaxCode"]);
                    tax.Description = dataRow["TaxDescription"].ToString();
                    TaxCategory taxCategory = new TaxCategory();
                    taxCategory.Id = Convert.ToInt32(dataRow["TaxCategoryCode"]);
                    taxCategory.Description = dataRow["TaxCategory"].ToString();

                    TaxCondition taxCondition = new TaxCondition();
                    taxCondition.Id = Convert.ToInt32(dataRow["TaxConditionCode"]);
                    taxCondition.Description = dataRow["TaxCondition"].ToString();

                    paymentTax.Tax = tax;

                    creditCard.Taxes.Add(paymentTax);
                }

                return creditCard.ToDTO();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region CardVoucher

        /// <summary>
        /// LoadCardVoucher
        /// Obtiene un listado de Vouchers para poder rechazarlos
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns>UIView</returns>
        private UIView LoadCardVoucher(int creditCardTypeCode, string voucher, long documentNumber, int technicalTransaction,
                                       int branchId, DateTime startDate, DateTime endDate, int status)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                UIView uiView;

                // Por tipo de conducto
                if (creditCardTypeCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.CreditCardTypeCode, creditCardTypeCode).And();
                }

                // Por número de voucher
                if (voucher != "-1")
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.Voucher, voucher.ToString()).And();
                }

                // Número de tarjeta
                if (documentNumber != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.DocumentNumber, documentNumber.ToString()).And();
                }

                // Por el número de transacción
                if (technicalTransaction != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.TechnicalTransaction, technicalTransaction).And();
                }

                // Por la sucursal
                if (branchId != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.BranchCode, branchId).And();
                }

                // Por el estado
                if (status != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardVoucher.Properties.Status, status).And();
                }

                // Por fecha desde
                if (startDate != new DateTime(1900, 1, 1))
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.CardVoucher.Properties.DatePayment);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(startDate);
                    criteriaBuilder.And();
                }

                // Por fecha hasta
                if (endDate != new DateTime(1900, 1, 1))
                {
                    criteriaBuilder.Property(ACCOUNTINGEN.CardVoucher.Properties.DatePayment);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(endDate);
                    criteriaBuilder.And();
                }

                criteriaBuilder.Property(ACCOUNTINGEN.CardVoucher.Properties.PaymentCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                if (creditCardTypeCode != -1 || voucher != "-1" || documentNumber != -1
                     || technicalTransaction != -1 || branchId != -1 || status != -1 || startDate.ToString() != "*" || endDate.ToString() != "*")
                {
                    uiView = _dataFacadeManager.GetDataFacade().GetView("CardVoucherView",
                                                            criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                    // Add New row for return total records
                    if (uiView.Rows.Count > 0)
                    {
                        uiView.Columns.Add("rows", typeof(int));
                        uiView.Rows[0]["rows"] = rows;
                    }
                }
                else
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.CardVoucher.Properties.PaymentCode);
                    criteriaBuilder.Less();
                    criteriaBuilder.Constant(0);
                    uiView = _dataFacadeManager.GetDataFacade().GetView("CardVoucherView",
                                                                  criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);
                }

                return uiView;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion CardVoucher

        #region Payment

        /// <summary>
        /// LoadInformationPayment
        /// Obtiene un listado de Vouchers para poder rechazarlos
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>UIView</returns>
        private UIView LoadInformationPayment(int paymentCode)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por tipo de conducto
                if (paymentCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.StatusPaymentCreditCardType.Properties.PaymentCode, paymentCode);
                }

                UIView payments = _dataFacadeManager.GetDataFacade().GetView("StatusPaymentCreditCardTypeView",
                                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                return payments;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Payment

        #region Cards

        /// <summary>
        /// LoadGetTaxCard
        /// Obtiene impuestos condiciones categorías
        /// </summary>
        /// <param name="cardTypeId"></param>
        /// <param name="branchId"></param>
        /// <returns>UIView</returns>
        private UIView LoadGetTaxCard(int cardTypeId, int branchId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(COMMEN.TaxCard.Properties.CreditCardTypeCode, cardTypeId).And();
                criteriaBuilder.PropertyEquals(COMMEN.TaxCard.Properties.BranchCode, branchId);

                UIView cardTaxes = _dataFacadeManager.GetDataFacade().GetView("TaxCardView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                return cardTaxes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Cards

        #region FormatDateTime
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

        #endregion

        #endregion Private Methods

        #region Rechazo de cheques
        public string AccountingCheck(Collect collect, DateTime accountingDate, int userId, int oldTechnicalTransaction, int paymentId, int statusTypeId, int bridgeAccountId)
        {
            string message = "";
            SaveBillParametersDTO saveBillParametersModel = new SaveBillParametersDTO()
            {
                Collect = new CollectDTO
                {
                    Id = collect.Id,
                    Transaction = collect.Transaction.ToDTO(),
                    Date = accountingDate,
                    Payments = collect.Payments.ToDTOs().ToList(),
                },
                TypeId = statusTypeId,
                UserId = userId,
                TechnicalTransaction = oldTechnicalTransaction,
                PaymentCode = paymentId,
                BridgeAccoutingId = bridgeAccountId,
                BridgePackageCode = CommonBusiness.GetStringParameter(ENUMACC.AccountingKeys.ACC_RULE_PACKAGE_COLLECT)
            };

            int entryNumber = DelegateService.accountingAccountService.AccountingChecks(saveBillParametersModel);

            if (entryNumber > 0)
            {
                message = Resources.Resources.IntegrationSuccessMessage + " " + entryNumber;
            }
            if (entryNumber == 0)
            {
                message = Resources.Resources.AccountingIntegrationUnbalanceEntry;
            }
            if (entryNumber == -2)
            {
                message = Resources.Resources.EntryRecordingError;
            }
            return message;
        }

        public MessageSuccessDTO SaveCheckingRejection(DTOs.RejectedPaymentDTO rejectedPayment, int billId, int payerId, int branchId, int userId, string voucherRejection, string forCheckRejection, DateTime accountingDate)
        {
            string message = "";
            Collect newcollect = new Collect();
            Models.Payments.Payment payment = new Models.Payments.Payment();

            //Flag para mostrar mensaje de contabilidad en EE
            bool showMessage = true;
            if (rejectedPayment?.Payment != null)
            {
                payment.Id = rejectedPayment.Payment.Id;
            }

            payment = _paymentDAO.GetPayment(payment);

            var registerDate = DateTime.Now;

            //Se guarda registro en la tabla ACC.Rejected_Payment
            rejectedPayment = _rejectedPaymentDAO.SaveRejectedPayment(rejectedPayment.ToModel(), userId, registerDate).ToDTO();

            // Grabación del Log
            SavePaymentLog(Convert.ToInt32(ActionTypes.RejectionPayment), rejectedPayment.Id, rejectedPayment.Payment.Id, Convert.ToInt32(PaymentStatus.Rejected), userId);

            CollectBusiness collectBusiness = new CollectBusiness();
            newcollect = new AccountingCollectServiceEEProvider().ReplicateCheckinCollect(billId, forCheckRejection, payerId, 0, payment, Convert.ToInt32(PaymentStatus.Exchanged), branchId);
            int technicalTransactionForRejection = new AccountingPaymentBallotServiceEEProvider().GetTechnicalTransactionForPaymentBallotByPaymentCode(payment.Id);
            int bridgeRejection = (int)DelegateService.commonService.GetParameterByDescription(ENUMACC.AccountingKeys.ACC_CHECK_REJECTION.ToString()).NumberParameter;
            #region Accounting

            bool generalLedgerSuccess = true;
            //disparo la contabilización del movimiento
            if ((string)UTILHELPER.EnumHelper.GetEnumParameterValue(ENUMACC.AccountingKeys.ACC_ENABLED_GENERAL_LEGDER) == "true")
            {
                AccountingPaymentBusiness accountingPaymentBusiness = new AccountingPaymentBusiness();
                var messageResponse = accountingPaymentBusiness.ProcessAccountingCheck(newcollect, accountingDate, userId, technicalTransactionForRejection, payment.Id, Convert.ToInt32(PaymentStatus.Rejected), bridgeRejection);
                generalLedgerSuccess = messageResponse.GeneralLedgerSuccess;
                message = messageResponse.Info;
                showMessage = true;
            }
            else
            {
                message = Convert.ToString(Resources.Resources.IntegrationServiceDisabledLabel);
                showMessage = false;
            }
            #endregion Accounting
            //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
            Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
            integration2GBusiness.Save(newcollect.ToModelIntegration(1));
            MessageSuccessDTO messageSuccessDTO = new MessageSuccessDTO()
            {
                ImputationMessage = message,
                TechnicalTransaction = newcollect.Transaction.TechnicalTransaction,
                ShowMessage = showMessage,
                BillId = newcollect.Id,
                GeneralLedgerSuccess = generalLedgerSuccess
            };
            return messageSuccessDTO;

        }
        #endregion

        public BillReportDTO GetBillReport(int technicalTransaction)
        {
            try
            {
                AccountingPaymentBusiness accountingPaymentBusiness = new AccountingPaymentBusiness();
                return accountingPaymentBusiness.GetBillReport(technicalTransaction).ToDTO();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGettingBillReport);
            }
        }
    }
}
