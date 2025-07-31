using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using CoreTransaction = Sistran.Core.Framework.Transactions;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using ACCOUNTINGEN=Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.DTOs;
using ServicesModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.Resources;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Assemblers;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingPaymentTicketServiceEEProvider : IAccountingPaymentTicketService
    {
        #region Constants

        private const int PageSize = 200;
        private const int RowsGrid = 50;

        #endregion

        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        readonly PaymentTicketDAO _paymentTicketDAO = new PaymentTicketDAO();
        readonly PaymentTicketItemDAO _paymentTicketItemDAO = new PaymentTicketItemDAO();

        #endregion DAOs

        #endregion

        #region Public Methods

        #region PaymentTicket

        /// <summary>
        /// SaveInternalBallot
        /// Graba un nueva boleta interna
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        public PaymentTicketDTO SaveInternalBallot(PaymentTicketDTO paymentTicketDTO, int userId)
        {
            ServicesModels.PaymentTicket paymentTicket = paymentTicketDTO.ToModel();
            ServicesModels.PaymentTicket newPaymentTicket = new ServicesModels.PaymentTicket();

            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();

                try
                {
                    newPaymentTicket = _paymentTicketDAO.SavePaymentTicket(paymentTicket,
                           new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))),
                            userId);

                    const int paymentTicketItemId = 0; //Identificador para clave autonumérica.

                    foreach (ServicesModels.Payment payment in paymentTicket.Payments)
                    {
                        _paymentTicketItemDAO.SavePaymentTicketItem(paymentTicketItemId, payment.Id, newPaymentTicket.Id);

                        // Graba PaymentLog
                        new AccountingPaymentServiceEEProvider().SavePaymentLog(Convert.ToInt32(ActionTypes.PaymentInternalBallot),
                                                       newPaymentTicket.Id,
                                                       payment.Id,
                                                       Convert.ToInt32(PaymentStatus.InternalBallot),
                                                       userId);
                    }

                    transaction.Complete();
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }

                return newPaymentTicket.ToDTO();
            }
        }

        /// <summary>
        /// UpdateInternalBallot
        /// Actualiza un registro de boleta interna
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        public PaymentTicketDTO UpdateInternalBallot(PaymentTicketDTO paymentTicketDTO, int userId)
        {
            ServicesModels.PaymentTicket paymentTicket = paymentTicketDTO.ToModel();
            try
            {
                return _paymentTicketDAO.UpdatePaymentTicketItems(paymentTicket, userId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(Resources.Resources.BusinessException);
            }
        }


        /// <summary>
        /// DeleteInternalBallot
        /// Borra un registro de boleta interna
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void DeleteInternalBallot(PaymentTicketDTO paymentTicketDTO, int userId)
        {
            ServicesModels.PaymentTicket paymentTicket = paymentTicketDTO.ToModel();
            try
            {
                ServicesModels.PaymentTicket deletePaymentTicket = _paymentTicketItemDAO.GetPaymentTicketsByPaymentTicketCode(paymentTicket.Id);

                bool isSaved = false;
                // Se elimina los registros seleccionados
                foreach (ServicesModels.Payment payment in paymentTicket.Payments)
                {
                    if (payment.Taxes != null)
                    {
                        foreach (ServicesModels.PaymentTax paymentTax in payment.Taxes)
                        {
                            _paymentTicketItemDAO.DeletePaymentTicketItem(paymentTax.Id);

                            new AccountingPaymentServiceEEProvider().SavePaymentLog(Convert.ToInt32(ActionTypes.ReversePaymentInternalBallot),
                                                            paymentTicket.Id, payment.Id, Convert.ToInt32(PaymentStatus.Active), userId);
                        }
                    }
                }

                // Se ingresan los nuevos payment ticket
                foreach (ServicesModels.Payment payment in paymentTicket.Payments)
                {
                    if (payment.Taxes == null)
                    {
                        foreach (ServicesModels.Payment newPayment in deletePaymentTicket.Payments)
                        {
                            if (newPayment.Id == payment.Id)
                            {
                                isSaved = false;
                                break;
                            }
                            else
                            {
                                isSaved = true;
                            }
                        }

                        if (isSaved)
                        {
                            _paymentTicketItemDAO.SavePaymentTicketItem(0, payment.Id, paymentTicket.Id);
                            new AccountingPaymentServiceEEProvider().SavePaymentLog(Convert.ToInt32(ActionTypes.PaymentInternalBallot),
                                                              paymentTicket.Id, payment.Id, Convert.ToInt32(PaymentStatus.InternalBallot), userId);
                        }
                    }
                }
            }
            catch (BusinessException ex)
            {

                throw new BusinessException(Resources.Resources.BusinessException);
            }
        }

        /// <summary>
        /// ValidateCashAmount
        /// Valida que el importe en efectivo ingresado no sea mayor total del efectivo recibido por caja menos el efectivo que ya esté asociado a 
        /// otras boletas internas del mismo usuario y fecha de caja abierta
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currencyId"></param>
        /// <param name="userId"></param>
        /// <param name="cashAmountAdmitted"></param>
        /// <param name="registerDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <returns>int</returns>
        public decimal ValidateCashAmount(int branchId, int currencyId, int userId,
                                      decimal cashAmountAdmitted, string registerDate, int paymentTicketId)
        {
            try
            {
                int cashAmountValidated = 0;
                string startDate = "";

                string endDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy");
                endDate = endDate + " 23:59:59";

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Obtener fecha apertura caja
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.Status, 1).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.UserId, userId).And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CollectControl.Properties.BranchCode, branchId);

                BusinessCollection businessCollection = new BusinessCollection(
                                        _dataFacadeManager.GetDataFacade().SelectObjects(
                                                                                typeof(ACCOUNTINGEN.CollectControl), criteriaBuilder.GetPredicate()));

                //hace si hay una sucursal abierta
                if (businessCollection.Count > 0)
                {
                    startDate = Convert.ToDateTime(businessCollection.OfType<ACCOUNTINGEN.CollectControl>().First().OpenDate).ToString("dd/MM/yyyy HH:mm:ss");

                    // Si el mes contable no esta cerrado las fechas se invierten
                    if (Convert.ToDateTime(registerDate) < Convert.ToDateTime(startDate))
                    {
                        endDate = Convert.ToDateTime(startDate).ToString("dd/MM/yyyy") + " 23:59:59";
                        startDate = Convert.ToDateTime(registerDate).ToString("dd/MM/yyyy");
                    }
                }
                else
                {
                    throw new Exception(Resources.Resources.ErroClosedBranch);
                }

                // Filtro
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.UserId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.CurrencyCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(currencyId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(branchId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.Status);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(1);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                businessCollection = new BusinessCollection(
                            _dataFacadeManager.GetDataFacade().SelectObjects(
                                        typeof(ACCOUNTINGEN.PaymentTicket), criteriaBuilder.GetPredicate()));

                // Importe en efectivo en boleta interna
                decimal cashAmount = 0;

                foreach (ACCOUNTINGEN.PaymentTicket paymentTicket in businessCollection.OfType<ACCOUNTINGEN.PaymentTicket>())
                {
                    if (paymentTicket.PaymentTicketCode != paymentTicketId)
                    {
                        cashAmount += Convert.ToDecimal(paymentTicket.CashAmount);
                    }
                }

                // Importe en efectivo recibo por caja
                #region UIView

                //Filtro
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode);
                criteriaBuilder.In();
                criteriaBuilder.ListValue();
                criteriaBuilder.Constant(Convert.ToInt32(PaymentMethods.Cash));
                criteriaBuilder.Constant(Convert.ToInt32(PaymentMethods.DepositVoucher));
                criteriaBuilder.EndList();
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.CurrencyCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(currencyId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(branchId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.CollectControl.Properties.UserId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.DatePayment);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                UIView cashCollections = _dataFacadeManager.GetDataFacade().GetView("CollectPaymentView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                decimal cashAmountReceived = 0;
                foreach (DataRow dataRow in cashCollections.Rows)
                {
                    // Excluye los recibos que tiene estado como cancelado.
                    if (Convert.ToInt32(dataRow["CollectStatus"]) != Convert.ToInt32(PaymentStatus.Canceled))
                    {
                        if (currencyId == 0) // Si es moneda local
                        {
                            cashAmountReceived += Convert.ToDecimal(dataRow["Amount"]);
                        }
                        else // Si es moneda extranjera
                        {
                            cashAmountReceived += Convert.ToDecimal(dataRow["IncomeAmount"]);
                        }
                    }
                }

                // Filtro efectivo boleta de depósito
                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.Status, 1);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.UserId, userId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentBallot.Properties.CurrencyCode, currencyId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentBallot.Properties.RegisterDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(FormatDateTime(startDate));
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentBallot.Properties.RegisterDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(FormatDateTime(endDate));

                UIView cashBallotDeposited = _dataFacadeManager.GetDataFacade().
                                      GetView("PaymentBallotView", criteriaBuilder.GetPredicate(),
                                      null, 0, -1, null, false, out rows);

                decimal cashBallotDepositedAmount = 0;

                foreach (DataRow dataRow in cashBallotDeposited.Rows)
                {
                    cashBallotDepositedAmount += Convert.ToDecimal(dataRow["CashAmount"]);
                }

                #endregion

                if (cashAmountAdmitted <= (cashAmountReceived - cashAmount - cashBallotDepositedAmount))
                    return 0;

                return cashAmountReceived - cashAmount - cashBallotDepositedAmount;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// CancelInternalBallot
        /// Permite anular la boleta interna de tarjetas, cambia estado de boleta a "anulada", actualiza la fecha de anulación y libera los vouchers 
        /// de tarjetas asociadas
        /// </summary>
        /// <param name="paymetTicketInput"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int CancelInternalBallot(PaymentTicketDTO paymetTicketInputDTO, int userId)
        {
            ServicesModels.PaymentTicket paymetTicketInput = paymetTicketInputDTO.ToModel();
            try
            {
                ServicesModels.PaymentTicket paymentTicket = new ServicesModels.PaymentTicket();
                paymentTicket.Id = paymetTicketInput.Id;
                paymentTicket = _paymentTicketDAO.GetPaymentTicket(paymentTicket);
                paymentTicket.Status = Convert.ToInt32(PaymentStatus.Canceled);
                paymentTicket.DisabledDate = DateTime.Now;
                paymentTicket.DisabledUser = userId;

                paymentTicket = _paymentTicketDAO.UpdatePaymentTicket(paymentTicket, DateTime.Now, userId, 1);
                List<SearchDTO.PaymentTicketItemDTO> paymentTicketItemDTOs = GetPaymentTicketItemsByPaymentTicketCode(paymentTicket.Id);
                if (paymentTicketItemDTOs.Count > 0)
                {
                    foreach (SearchDTO.PaymentTicketItemDTO paymentTicketItem in paymentTicketItemDTOs)
                    {
                        new AccountingPaymentServiceEEProvider().SavePaymentLog(Convert.ToInt32(ActionTypes.ReversePaymentInternalBallot),
                                                                       paymentTicket.Id,
                                                                       paymentTicketItem.PaymentCode,
                                                                       Convert.ToInt32(PaymentStatus.Active),
                                                                       userId);
                    }
                }

                return paymentTicket.Id;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion

        #region Search Internal Ballot

        /// <summary>
        /// GetChecksToDepositBallot
        /// Obtiene los cheques corrientes/postfechados a depositar en una boleta interna. Se filtra por la sucursal, el tipo 
        /// de pago y moneda (obligatorio), banco emisor y número de cuenta emisora (opcionales) 
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="currencyCode"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="checkNumber"></param>
        /// <returns>List<CheckToDepositInternalBallotDTO/></returns>
        public List<SearchDTO.CheckToDepositInternalBallotDTO> GetChecksToDepositBallot(int branchCode, int paymentMethodTypeCode,
                                                                              int currencyCode, int issuingBankCode, int checkNumber, int userId)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por sucursal
                if (branchCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ChecksDeposit.Properties.BranchCode, branchCode).And();
                }

                // Por moneda
                if (currencyCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ChecksDeposit.Properties.CurrencyCode, currencyCode).And();
                }

                // Por código de banco
                if (issuingBankCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ChecksDeposit.Properties.IssuingBankCode, issuingBankCode).And();
                }

                // Por número de cheque
                if (checkNumber != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ChecksDeposit.Properties.CheckNumber, checkNumber.ToString()).And();
                }

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ChecksDeposit.Properties.UserId, userId).And();

                criteriaBuilder.Property(ACCOUNTINGEN.ChecksDeposit.Properties.Amount);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                UIView checkDeposits;
                int rows;

                if (branchCode != -1 || paymentMethodTypeCode != -1 || currencyCode != -1 || issuingBankCode != -1 || checkNumber != -1)
                {
                    checkDeposits = _dataFacadeManager.GetDataFacade().GetView("ChecksDepositView",
                                            criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
                }
                else
                {
                    checkDeposits = _dataFacadeManager.GetDataFacade().GetView("ChecksDepositView",
                                            criteriaBuilder.GetPredicate(), null, 0, 0, null, false, out rows);
                }

                if (checkDeposits.Rows.Count > 0)
                {
                    checkDeposits.Columns.Add("rows", typeof(int));
                    checkDeposits.Rows[0]["rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SearchDTO.CheckToDepositInternalBallotDTO> checksToDepositInternalBallotDTOs = new List<SearchDTO.CheckToDepositInternalBallotDTO>();

                foreach (DataRow dataRow in checkDeposits.Rows)
                {
                    checksToDepositInternalBallotDTOs.Add(new SearchDTO.CheckToDepositInternalBallotDTO()
                    {
                        PaymentCode = Convert.ToInt32(dataRow["PaymentCode"]),
                        PaymentMethodTypeCode = Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        PaymentMethodTypeName = dataRow["PaymentMethodTypeName"].ToString(),
                        IssuingBankCode = Convert.ToInt32(dataRow["IssuingBankCode"]),
                        BankName = dataRow["BankName"].ToString(),
                        IssuingAccountNumber = dataRow["IssuingAccountNumber"] == DBNull.Value ? "" : dataRow["IssuingAccountNumber"].ToString(),
                        CheckNumber = dataRow["CheckNumber"].ToString(),   //número de cheque o número de transferencia
                        ReceiptNumber = Convert.ToInt32(dataRow["ReceiptNumber"]),
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyName"].ToString(),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        CheckDate = Convert.ToDateTime(dataRow["CheckDate"]),  //fecha de pago
                        Holder = dataRow["Holder"].ToString(),
                        PaymentTicketItemId = 0,
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"] == DBNull.Value ? "" : dataRow["BranchName"].ToString(),
                        TechnicalTransaction = dataRow["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TechnicalTransaction"]),
                        Rows = checkDeposits.Count
                    });
                }

                #endregion

                return checksToDepositInternalBallotDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetInternalBallotNotDeposited
        /// Obtiene las boletas internas no depositadas. Cuando el parámetro es 0 se obtiene los números de boleta interna y el resultado se coloca 
        /// en el combo. Si es > 0 se obtiene la cabecera de la boleta interna.
        /// </summary>
        /// <param name="internalBallotNumber"></param>
        /// <returns>List<BallotNotDepositedDTO/></returns>
        public List<SearchDTO.BallotNotDepositedDTO> GetInternalBallotNotDeposited(int internalBallotNumber)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                #region SortExp

                string sortExp = null;
                string sortOrder = string.Empty;

                sortOrder = "+";
                sortExp = sortOrder + "PaymentTicketCode";

                string[] sortExps = null;
                if ((sortExp != null))
                {
                    sortExps = new string[] { sortExp };
                }


                #endregion

                UIView ballotNotDeposited;
                int rows;

                // Obtiene todos
                if (internalBallotNumber == 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketNotDeposited.Properties.Status, 1);

                    ballotNotDeposited = _dataFacadeManager.GetDataFacade().GetView("PaymentTicketNotDepositedView",
                                            criteriaBuilder.GetPredicate(), null, 0, RowsGrid, sortExps, true, out rows);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketNotDeposited.Properties.PaymentTicketCode, internalBallotNumber);

                    ballotNotDeposited = _dataFacadeManager.GetDataFacade().GetView("PaymentTicketNotDepositedView",
                                            criteriaBuilder.GetPredicate(), null, 0, RowsGrid, null, true, out rows);
                }

                if (ballotNotDeposited.Rows.Count > 0)
                {
                    ballotNotDeposited.Columns.Add("rows", typeof(int));
                    ballotNotDeposited.Rows[0]["rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SearchDTO.BallotNotDepositedDTO> ballotNotDepositedDTOs = new List<SearchDTO.BallotNotDepositedDTO>();

                foreach (DataRow dataRow in ballotNotDeposited.Rows)
                {
                    ballotNotDepositedDTOs.Add(new SearchDTO.BallotNotDepositedDTO()
                    {
                        PaymentTicketCode = Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        BankCode = Convert.ToInt32(dataRow["BankCode"]),
                        BankName = dataRow["BankName"].ToString(),
                        BranchCode = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"].ToString(),
                        AccountNumber = dataRow["AccountNumber"].ToString(),
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyName"].ToString(),
                        CashAmount = dataRow["CashAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CashAmount"]),
                        CheckAmount = dataRow["CheckAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CheckAmount"]),
                        PaymentMethodTypeCode = Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        RegisterDate = dataRow["RegisterDate"].ToString(),
                        Rows = ballotNotDeposited.Count
                    });
                }

                #endregion

                return ballotNotDepositedDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDetailInternalBallotNotDeposited
        /// Obtiene el detalle de una boleta interna, para ello se filtra por el número de boleta,
        /// estado o número de cheque (opcional)
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="checkDate"></param>
        /// <returns>List<DetailBallotNotDepositedDTO/></returns>
        public List<SearchDTO.DetailBallotNotDepositedDTO> GetDetailInternalBallotNotDeposited(int branchCode, int paymentTicketCode,
                                                                                      int paymentStatus, DateTime checkDate)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por id sucursal
                if (branchCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchCode).And();
                }

                // Por número de boleta interna
                if (paymentTicketCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTicketCode).And();
                }

                // Por status
                if (paymentStatus != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.Status, paymentStatus).And();
                }

                // Por fecha cheque
                if (checkDate != new DateTime(1900, 1, 1))
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.DatePayment, checkDate).And();
                }

                criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.Amount);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                UIView detailBallot;
                int rows;

                if (branchCode != -1 || paymentTicketCode != -1 || paymentStatus != -1 || checkDate.ToString() != "-1")
                {
                    detailBallot = _dataFacadeManager.GetDataFacade().
                            GetView("DetailPaymentTicketView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
                }
                else
                {
                    detailBallot = _dataFacadeManager.GetDataFacade().
                            GetView("DetailPaymentTicketView", criteriaBuilder.GetPredicate(), null, 0, 0, null, false, out rows);
                }

                if (detailBallot.Rows.Count > 0)
                {
                    detailBallot.Columns.Add("rows", typeof(int));
                    detailBallot.Rows[0]["rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SearchDTO.DetailBallotNotDepositedDTO> detailBallotNotDepositedDTOs = new List<SearchDTO.DetailBallotNotDepositedDTO>();

                foreach (DataRow dataRow in detailBallot.Rows)
                {
                    detailBallotNotDepositedDTOs.Add(new SearchDTO.DetailBallotNotDepositedDTO()
                    {
                        PaymentCode = Convert.ToInt32(dataRow["PaymentCode"]),
                        BranchCode = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchName = dataRow["BranchName"].ToString(),
                        PaymentMethodTypeCode = Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        PaymentMethodTypeName = dataRow["PaymentMethodTypeName"].ToString(),
                        IssuingBankCode = Convert.ToInt32(dataRow["IssuingBankCode"]),
                        BankName = dataRow["BankName"].ToString(),
                        IssuingAccountNumber = dataRow["IssuingAccountNumber"].ToString(),
                        CheckNumber = dataRow["CheckNumber"].ToString(),
                        ReceiptNumber = Convert.ToInt32(dataRow["ReceiptNumber"]),
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyName = dataRow["CurrencyName"].ToString(),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        IncomeAmount = Convert.ToDecimal(dataRow["IncomeAmount"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        CheckDate = Convert.ToDateTime(dataRow["CheckDate"]),
                        Holder = dataRow["Holder"].ToString(),
                        PaymentTicketCode = Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        PaymentTicketItemCode = Convert.ToInt32(dataRow["PaymentTicketItemCode"]),
                        Rows = detailBallotNotDepositedDTOs.Count
                    });
                }

                #endregion

                return detailBallotNotDepositedDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentTicketItemsByPaymentCode
        /// Obtiene los registros de boletas internas registradas
        /// </summary>
        /// <returns>List<PaymentTicketItem/></returns>
        public List<SearchDTO.PaymentTicketItemDTO> GetPaymentTicketItemsByPaymentTicketCode(int paymentTicketCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTicketCode);

                UIView paymentTickets = _dataFacadeManager.GetDataFacade().GetView("PaymentTicketItemView",
                                        criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out int rows);
                List<SearchDTO.PaymentTicketItemDTO> paymentTicketItemDTOs = new List<SearchDTO.PaymentTicketItemDTO>();

                foreach (DataRow dataRow in paymentTickets.Rows)
                {
                    paymentTicketItemDTOs.Add(new SearchDTO.PaymentTicketItemDTO
                    {
                        PaymentCode = dataRow["PaymentCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentCode"]),
                        PaymentTicketCode = dataRow["PaymentTicketCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        PaymentTicketItemCode = dataRow["PaymentTicketItemCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTicketItemCode"])
                    });
                }

                return paymentTicketItemDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentTicketItemsByCollectId
        /// Obtiene boletas internas por el recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<PaymentTicketItemDTO/></returns>
        public List<SearchDTO.PaymentTicketItemDTO> GetPaymentTicketItemsByCollectId(int collectId)
        {
            try
            {
                int enumParameterPaymentMethodCash = 0;
                int enumParameterPaymentMethodCreditableCard = 0;
                int enumParameterPaymentDataPhone = 0;
                int enumParameterPaymentMethodCurrentCheck = 0;
                int enumParameterPaymentMethodPostDatedCheck = 0;

                #region UIView
                if (Convert.ToString(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CASH)) != "")
                {
                    enumParameterPaymentMethodCash = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CASH));
                }
                else
                {
                    enumParameterPaymentMethodCash = 0;
                }

                if (Convert.ToString(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)) != "")
                {
                    enumParameterPaymentMethodCreditableCard = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD));
                }
                else
                {
                    enumParameterPaymentMethodCreditableCard = 0;
                }

                if (Convert.ToString(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DATA_PHONE)) != "")
                {
                    enumParameterPaymentDataPhone = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DATA_PHONE));
                }
                else
                {
                    enumParameterPaymentDataPhone = 0;
                }

                if (Convert.ToString(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK)) != "")
                {
                    enumParameterPaymentMethodCurrentCheck = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK));
                }
                else
                {
                    enumParameterPaymentMethodCurrentCheck = 0;
                }

                if (Convert.ToString(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK)) != "")
                {
                    enumParameterPaymentMethodPostDatedCheck = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_POSTDATED_CHECK));
                }
                else
                {
                    enumParameterPaymentMethodPostDatedCheck = 0;
                }

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, enumParameterPaymentMethodCash);
                criteriaBuilder.Or();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, enumParameterPaymentMethodCreditableCard);
                criteriaBuilder.CloseParenthesis();
                criteriaBuilder.Or();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, enumParameterPaymentMethodCreditableCard);
                criteriaBuilder.CloseParenthesis();
                criteriaBuilder.Or();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, enumParameterPaymentDataPhone);
                criteriaBuilder.CloseParenthesis();
                criteriaBuilder.Or();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, enumParameterPaymentMethodCurrentCheck);
                criteriaBuilder.CloseParenthesis();
                criteriaBuilder.Or();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, enumParameterPaymentMethodPostDatedCheck);
                criteriaBuilder.CloseParenthesis();

                UIView paymentTicketItems = _dataFacadeManager.GetDataFacade().
                                    GetView("PaymentTicketItemPaymentView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);

                #endregion

                #region LoadDTO

                List<SearchDTO.PaymentTicketItemDTO> paymentTicketItemDTOs = new List<SearchDTO.PaymentTicketItemDTO>();

                foreach (DataRow dataRow in paymentTicketItems.Rows)
                {
                    paymentTicketItemDTOs.Add(new SearchDTO.PaymentTicketItemDTO()
                    {
                        PaymentTicketItemCode = Convert.ToInt32(dataRow["PaymentTicketItemCode"]),
                        PaymentCode = Convert.ToInt32(dataRow["PaymentCode"]),
                        PaymentTicketCode = Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        Rows = paymentTicketItems.Count
                    });
                }

                #endregion

                return paymentTicketItemDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DepositSlipSearch
        /// Obtiene los boletas internas tanto de cheques como de efectivo
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <returns>List<SearchInternalBallotDTO/></returns>
        public List<SearchDTO.SearchInternalBallotDTO> DepositSlipSearch(int bankId, string startDate, string endDate, int paymentTicketId)
        {
            try
            {
                #region Filter

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                if (bankId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BankCode, bankId);
                }

                if (paymentTicketId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketId);
                }

                if (startDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(startDate));
                }
                if (endDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(endDate));
                }

                #endregion

                UIView deposits = _dataFacadeManager.GetDataFacade().GetView("PaymentTicketDepositSlipView",
                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (deposits.Rows.Count > 0)
                {
                    deposits.Columns.Add("rows", typeof(int));
                    deposits.Rows[0]["rows"] = rows;
                }

                #region LoadDTO

                List<SearchDTO.SearchInternalBallotDTO> searchInternalBallotDTOs = new List<SearchDTO.SearchInternalBallotDTO>();

                foreach (DataRow dataRow in deposits)
                {
                    searchInternalBallotDTOs.Add(new SearchDTO.SearchInternalBallotDTO()
                    {
                        PaymentTicketCode = dataRow["PaymentTicketCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        BankCode = dataRow["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BankCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        AccountNumber = dataRow["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["AccountNumber"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        CashAmount = dataRow["CashAmount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["CashAmount"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Amount"]),
                        Status = dataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Status"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dataRow["RegisterDate"])),
                        UserId = dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                        User = dataRow["User"] == DBNull.Value ? "" : Convert.ToString(dataRow["User"]),
                        PaymentBallotNumber = dataRow["PaymentBallotNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PaymentBallotNumber"]),
                        DespositDate = dataRow["DepositDate"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dataRow["DepositDate"])),
                        Rows = deposits.Count
                    });
                }

                #endregion

                return searchInternalBallotDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// SearchInternalBallotCard
        /// Obtiene las boletas internas de depósito de tarjetas 
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="startDate"></param>
        /// <param name="branchId"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="creditCardTypeId"></param>
        /// <returns>List<SearchInternalBallotCardDTO/></returns>
        public List<SearchDTO.SearchInternalBallotCardDTO> SearchInternalBallotCard(int bankId, string startDate, string endDate,
                                                          int paymentTicketId, int creditCardTypeId, int branchId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (endDate != null && endDate != "")
                {
                    endDate = endDate + " 23:59:59";
                }

                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)));

                if (creditCardTypeId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CreditCardTypeCode, creditCardTypeId);
                }

                if (bankId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BankCode, bankId);
                }

                if (paymentTicketId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketId);
                }
                if (branchId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                }

                if (startDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(startDate));
                }
                if (endDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(endDate));
                }
                criteriaBuilder.CloseParenthesis();

                criteriaBuilder.Or();

                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_CREDITABLE_CREDITCARD)));

                if (creditCardTypeId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CreditCardTypeCode, creditCardTypeId);
                }

                if (bankId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BankCode, bankId);
                }
                if (paymentTicketId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketId);
                }

                if (branchId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                }

                if (startDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(startDate));
                }
                if (endDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(endDate));
                }
                criteriaBuilder.CloseParenthesis();

                criteriaBuilder.Or();

                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DATA_PHONE)));

                if (creditCardTypeId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CreditCardTypeCode, creditCardTypeId);
                }

                if (bankId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BankCode, bankId);
                }
                if (paymentTicketId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketId);
                }

                if (branchId > -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                }

                if (startDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.GreaterEqual();
                    criteriaBuilder.Constant(FormatDateTime(startDate));
                }
                if (endDate != "")
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.RegisterDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(FormatDateTime(endDate));
                }
                criteriaBuilder.CloseParenthesis();

                UIView internalBallotCards = _dataFacadeManager.GetDataFacade().GetView("PaymentTicketPaymentBallotView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out int rows);


                if (internalBallotCards.Rows.Count > 0)
                {
                    // Equivalente a un Distinct
                    for (int y = 0; y < (internalBallotCards.Rows.Count - 1); y++)
                    {
                        if (internalBallotCards.Rows[y]["PaymentTicketCode"].ToString() == internalBallotCards.Rows[y + 1]["PaymentTicketCode"].ToString())
                        {
                            DataRow dataRow = internalBallotCards.Rows[y + 1];
                            dataRow.Delete();
                            y--;
                        }
                    }
                }

                #region LoadDTO

                List<SearchDTO.SearchInternalBallotCardDTO> searchInternalBallotCardDTOs = new List<SearchDTO.SearchInternalBallotCardDTO>();

                foreach (DataRow dataRow in internalBallotCards)
                {
                    searchInternalBallotCardDTOs.Add(new SearchDTO.SearchInternalBallotCardDTO()
                    {
                        PaymentTicketCode = dataRow["PaymentTicketCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        AccountNumber = dataRow["AccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["AccountNumber"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        CreditCardDescription = dataRow["CreditCardDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardDescription"]),
                        PaymentMethodTypeCode = dataRow["PaymentMethodTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentMethodTypeCode"]),
                        PaymentMethodTypeDescription = dataRow["PaymentMethodTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["PaymentMethodTypeDescription"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Amount"]),
                        Status = dataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Status"]),
                        RegisterDate = dataRow["RegisterDate"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dataRow["RegisterDate"])),
                        PaymentBallotNumber = dataRow["PaymentBallotNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["PaymentBallotNumber"]),
                        DepositDate = dataRow["DepositDate"] == DBNull.Value ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dataRow["DepositDate"])),
                        BranchCode = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BranchDescription"]),
                        Rows = internalBallotCards.Count
                    });
                }

                #endregion

                return searchInternalBallotCardDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDetailChecks
        /// Obtiene el detalle de los cheques 
        /// </summary>
        /// <param name="paymentTicketId"></param>
        /// <returns>List<DetailCheckDTO/></returns>
        public List<SearchDTO.DetailCheckDTO> GetDetailChecks(int paymentTicketId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (paymentTicketId > -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTicketId);
                }


                UIView checkDetails = _dataFacadeManager.GetDataFacade().GetView("DetailChecksView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (checkDetails.Rows.Count > 0)
                {
                    checkDetails.Columns.Add("rows", typeof(int));
                    checkDetails.Rows[0]["rows"] = rows;
                }

                #region LoadDTO

                List<SearchDTO.DetailCheckDTO> detailCheckDTOs = new List<SearchDTO.DetailCheckDTO>();

                foreach (DataRow dataRow in checkDetails)
                {
                    detailCheckDTOs.Add(new SearchDTO.DetailCheckDTO()
                    {
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        IssuingAccountNumber = dataRow["IssuingAccountNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["IssuingAccountNumber"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        CollectCode = dataRow["CollectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectId"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["IncomeAmount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["IncomeAmount"]),
                        DatePayment = dataRow["DatePayment"] == DBNull.Value ? "" : Convert.ToDateTime(dataRow["DatePayment"]).ToString("d", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : Convert.ToString(dataRow["Holder"]),
                    });
                }

                #endregion

                return detailCheckDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDetailCards
        /// Obtiene el detalle de la boleta interna de depósito de tarjetas dado el número de boleta 
        /// </summary>
        /// <param name="paymentTicketId"></param>
        /// <returns>List<DetailCardDTO/></returns>
        public List<SearchDTO.DetailCardDTO> GetDetailCards(int paymentTicketId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (paymentTicketId > -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTicketId);
                }


                UIView cardDetails = _dataFacadeManager.GetDataFacade().GetView("DetailCardsView",
                                                       criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                // Add New row for return total records
                if (cardDetails.Rows.Count > 0)
                {
                    cardDetails.Columns.Add("rows", typeof(int));
                    cardDetails.Rows[0]["rows"] = rows;
                }

                #region LoadDTO

                List<SearchDTO.DetailCardDTO> detailCardDTOs = new List<SearchDTO.DetailCardDTO>();

                foreach (DataRow dataRow in cardDetails)
                {
                    detailCardDTOs.Add(new SearchDTO.DetailCardDTO()
                    {
                        CreditCardDescription = dataRow["CreditCardDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardDescription"]),
                        BankDescription = dataRow["BankDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BankDescription"]),
                        DocumentNumber = dataRow["DocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["DocumentNumber"]),
                        CurrencyDescription = dataRow["CurrencyDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyDescription"]),
                        Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Amount"]),
                        Tax = dataRow["Taxes"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Taxes"]),
                        Voucher = dataRow["Voucher"] == DBNull.Value ? "" : Convert.ToString(dataRow["Voucher"]),
                        Holder = dataRow["Holder"] == DBNull.Value ? "" : Convert.ToString(dataRow["Holder"]),
                        CollectCode = dataRow["CollectId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CollectId"])
                    });
                }

                #endregion

                return detailCardDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        #endregion

        #region Reports Internal Ballot

        /// <summary>
        /// GetReportInternalBallot
        /// Obtiene la cabecera y detalle de una boleta interna para la impresión
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="uiviewName"></param>
        /// <returns>List<InternalBallotReportDTO/></returns>
        public List<SearchDTO.InternalBallotReportDTO> GetReportInternalBallot(int userId, int paymentTicketCode, string uiviewName)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.UserId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(paymentTicketCode);

                UIView reportBallots = _dataFacadeManager.GetDataFacade().GetView(uiviewName,
                                                criteriaBuilder.GetPredicate(), null, 0, PageSize, null, false, out int rows);

                #endregion

                #region LoadDTO

                List<SearchDTO.InternalBallotReportDTO> internalBallotReportDTOs = new List<SearchDTO.InternalBallotReportDTO>();

                if (uiviewName == "PaymentTicketDetailView")
                {
                    foreach (DataRow row in reportBallots.Rows)
                    {
                        internalBallotReportDTOs.Add(new SearchDTO.InternalBallotReportDTO()
                        {
                            PaymentTicketCode = Convert.ToInt32(row["PaymentTicketCode"]),
                            BankCode = Convert.ToInt32(row["BankCode"]),
                            BankName = row["BankName"].ToString(),
                            AccountNumber = row["AccountNumber"].ToString(),
                            CurrencyCode = Convert.ToInt32(row["CurrencyCode"]),
                            CurrencyName = row["CurrencyName"].ToString(),
                            UserId = row["UserId"].ToString(),
                            RegisterDate = Convert.ToDateTime(row["RegisterDate"]),
                            CashAmount = Convert.ToDecimal(row["CashAmount"]),
                            Amount = Convert.ToDecimal(row["Amount"]),
                            IssuingBankCode = Convert.ToInt32(row["IssuingBankCode"]),
                            IssuingBankName = row["IssuingBankName"].ToString(),
                            IssuingAccountNumber = row["IssuingAccountNumber"].ToString(),
                            DocumentNumber = row["DocumentNumber"].ToString(),
                            Holder = row["Holder"].ToString(),
                            TinyDescription = row["TinyDescription"].ToString(),
                            CheckAmount = Convert.ToDecimal(row["IncomeAmount"]),
                            IncomeAmount = Convert.ToDecimal(row["IncomeAmount"]),
                            ExchangeRate = Convert.ToDecimal(row["ExchangeRate"]),
                            Rows = reportBallots.Count
                        });
                    }
                }
                else
                {
                    foreach (DataRow row in reportBallots.Rows)
                    {
                        internalBallotReportDTOs.Add(new SearchDTO.InternalBallotReportDTO()
                        {
                            PaymentTicketCode = Convert.ToInt32(row["PaymentTicketCode"]),
                            BankCode = Convert.ToInt32(row["BankCode"]),
                            BankName = row["BankName"].ToString(),
                            AccountNumber = row["AccountNumber"].ToString(),
                            CurrencyCode = Convert.ToInt32(row["CurrencyCode"]),
                            CurrencyName = row["CurrencyName"].ToString(),
                            UserId = row["UserId"].ToString(),
                            RegisterDate = Convert.ToDateTime(row["RegisterDate"]),
                            CashAmount = Convert.ToDecimal(row["CashAmount"]),
                            Amount = Convert.ToDecimal(row["Amount"]),
                            TinyDescription = row["TinyDescription"].ToString(),
                            Rows = reportBallots.Count
                        });
                    }
                }

                #endregion

                return internalBallotReportDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetReportInternalBallotCard
        /// Obtiene la cabecera y detalle de una boleta interna de tarjetas para la impresión
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="uiviewName"></param>
        /// <returns>List<InternalBallotCardReportDTO/></returns>
        public List<SearchDTO.InternalBallotCardReportDTO> GetReportInternalBallotCard(int userId, int paymentTicketCode, string uiviewName)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.UserId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(paymentTicketCode);

                UIView reportBallots = _dataFacadeManager.GetDataFacade().GetView(uiviewName,
                                            criteriaBuilder.GetPredicate(), null, 0, PageSize, null, false, out int rows);

                #endregion

                #region LoadDTO

                double taxAmount = 0;
                List<SearchDTO.InternalBallotCardReportDTO> internalBallotCardReportDTOs = new List<SearchDTO.InternalBallotCardReportDTO>();

                if (uiviewName == "PaymentTicketCreditCardView")
                {
                    foreach (DataRow row in reportBallots.Rows)
                    {
                        taxAmount += row["CardTaxAmount"] == DBNull.Value ? 0 : Convert.ToDouble(row["CardTaxAmount"]);
                    }

                    foreach (DataRow row in reportBallots.Rows)
                    {
                        internalBallotCardReportDTOs.Add(new SearchDTO.InternalBallotCardReportDTO()
                        {
                            PaymentTicketCode = Convert.ToInt32(row["PaymentTicketCode"]),
                            PaymentMethodTypeCode = Convert.ToInt32(row["PaymentMethodTypeCode"]),
                            PaymentMethodTypeName = row["PaymentMethodTypeName"].ToString(),
                            CreditCardTypeCode = Convert.ToInt32(row["CreditCardTypeCode"]),
                            CreditCardTypeName = row["CreditCardTypeName"].ToString(),
                            BranchCode = Convert.ToInt32(row["BranchCode"]),
                            BranchName = row["BranchName"].ToString(),
                            BankCode = Convert.ToInt32(row["BankCode"]),
                            BankName = row["BankName"].ToString(),
                            AccountNumber = row["AccountNumber"].ToString(),
                            CurrencyCode = Convert.ToInt32(row["CurrencyCode"]),
                            CurrencyName = row["CurrencyName"].ToString(),
                            UserId = userId,
                            UserName = row["userId"].ToString(),
                            RegisterDate = Convert.ToDateTime(row["RegisterDate"]),
                            Amount = Convert.ToDecimal(row["Amount"]),
                            TaxAmount = Convert.ToDecimal(taxAmount),//
                            CommissionAmount = Convert.ToDecimal(row["CommissionAmount"]),
                            IssuingBankCode = Convert.ToInt32(row["IssuingBankCode"]),
                            IssuingBankName = row["IssuingBankName"].ToString(),
                            VoucherNumber = row["VoucherNumber"].ToString(),
                            CardNumber = row["CardNumber"].ToString(),
                            CashAmount = Convert.ToDecimal(row["CashAmount"]),
                            CardDate = Convert.ToDateTime(row["CardDate"]),
                            AuthorizationNumber = row["AuthorizationNumber"].ToString(),
                            Holder = row["Holder"].ToString(),
                            TinyDescription = row["TinyDescription"].ToString(),
                            CardAmount = Convert.ToDecimal(row["IncomeAmount"]),
                            IncomeAmount = Convert.ToDecimal(row["IncomeAmount"]),//
                            ExchangeRate = Convert.ToDecimal(row["ExchangeRate"]),
                            CardTaxAmount = row["CardTaxAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["CardTaxAmount"]),//
                            CardCommissionAmount = row["CardCommissionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["CardCommissionAmount"]),
                            Rows = reportBallots.Count
                        });
                    }
                }
                else
                {
                    taxAmount = 0;
                    foreach (DataRow row in reportBallots.Rows)
                    {
                        taxAmount += row["CardTaxAmount"] == DBNull.Value ? 0 : Convert.ToDouble(row["CardTaxAmount"]);
                    }
                    foreach (DataRow row in reportBallots.Rows)
                    {
                        internalBallotCardReportDTOs.Add(new SearchDTO.InternalBallotCardReportDTO()
                        {
                            PaymentTicketCode = Convert.ToInt32(row["PaymentTicketCode"]),
                            PaymentMethodTypeCode = Convert.ToInt32(row["PaymentMethodTypeCode"]),
                            PaymentMethodTypeName = row["PaymentMethodTypeName"].ToString(),
                            CreditCardTypeCode = Convert.ToInt32(row["CreditCardTypeCode"]),
                            CreditCardTypeName = row["CreditCardTypeName"].ToString(),
                            BranchCode = Convert.ToInt32(row["BranchCode"]),
                            BranchName = row["BranchName"].ToString(),
                            BankCode = -1,
                            BankName = "",
                            AccountNumber = "",
                            CurrencyCode = -1,
                            CurrencyName = "",
                            UserId = userId,
                            UserName = row["userId"].ToString(),
                            RegisterDate = Convert.ToDateTime(row["RegisterDate"]),
                            Amount = Convert.ToDecimal(row["Amount"]),
                            TaxAmount = Convert.ToDecimal(taxAmount),
                            CommissionAmount = Convert.ToDecimal(row["CommissionAmount"]),
                            IssuingBankCode = Convert.ToInt32(row["IssuingBankCode"]),
                            IssuingBankName = row["IssuingBankName"].ToString(),
                            VoucherNumber = row["VoucherNumber"].ToString(),
                            CardNumber = row["CardNumber"].ToString(),
                            CashAmount = Convert.ToDecimal(row["CashAmount"]),
                            CardDate = Convert.ToDateTime(row["CardDate"]),
                            AuthorizationNumber = row["AuthorizationNumber"].ToString(),
                            Holder = row["Holder"].ToString(),
                            TinyDescription = row["TinyDescription"].ToString(),
                            CardAmount = Convert.ToDecimal(row["IncomeAmount"]),
                            IncomeAmount = Convert.ToDecimal(row["IncomeAmount"]),
                            ExchangeRate = Convert.ToDecimal(row["ExchangeRate"]),
                            CardTaxAmount = row["CardTaxAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["CardTaxAmount"]),
                            CardCommissionAmount = row["CardCommissionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["CardCommissionAmount"]),
                            Rows = reportBallots.Count
                        });
                    }
                }

                #endregion

                return internalBallotCardReportDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Internal Ballot Card

        /// <summary>
        /// GetCardsToDepositBallot
        /// Obtiene las tarjetas acreditables y no acreditables a depositar en una boleta interna. Se filtra por el tipo de pago, conducto, sucursal y
        /// fecha ingreso hasta (obligatorio), banco receptor, número de cuenta, moneda de la cuenta (obligatorios si el tipo de tarjeta es acreditable) 
        /// banco emisor y número de voucher (opcionales) 
        /// </summary>
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="currencyCode"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="voucherNumber"></param>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="toDate"></param>
        /// <returns>List<CardToDepositInternalBallotDTO/></returns>
        public List<SearchDTO.CardToDepositInternalBallotDTO> GetCardsToDepositBallot(int paymentMethodTypeCode, int currencyCode,
                                                                            int issuingBankCode, int voucherNumber,
                                                                            int creditCardTypeCode, int branchCode, DateTime toDate)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por tipo de pago
                if (paymentMethodTypeCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.PaymentMethodTypeCode, paymentMethodTypeCode).Or();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.PaymentMethodTypeCode,
                    Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_PARAM_PAYMENT_METHOD_DATA_PHONE))).And();
                }

                // Por tipo de tarjeta
                if (creditCardTypeCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.CreditCardTypeCode, creditCardTypeCode).And();
                }

                // Por sucursal
                if (branchCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.BranchCode, branchCode).And();
                }

                // Por fecha hasta
                if (toDate.ToString() != "*")
                {
                    string endDate = Convert.ToDateTime(toDate).ToString("yyyy/MM/dd");
                    endDate = endDate + " 23:59:59";

                    criteriaBuilder.Property(ACCOUNTINGEN.CardsDeposit.Properties.CardDate);
                    criteriaBuilder.LessEqual();
                    criteriaBuilder.Constant(Convert.ToDateTime(endDate));
                    criteriaBuilder.And();
                }

                //Por moneda
                if (currencyCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.CurrencyCode, currencyCode).And();
                }

                // Por código de banco
                if (issuingBankCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.IssuingBankCode, issuingBankCode).And();
                }

                // Por número de voucher
                if (voucherNumber != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CardsDeposit.Properties.Voucher, voucherNumber.ToString()).And();
                }

                criteriaBuilder.Property(ACCOUNTINGEN.ChecksDeposit.Properties.Amount);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                UIView cards;
                int rows;

                if (paymentMethodTypeCode != -1 || creditCardTypeCode != -1 || branchCode != -1 || toDate.ToString() != "*")
                {
                    cards = _dataFacadeManager.GetDataFacade().
                                    GetView("CardsDepositView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
                }
                else
                {
                    cards = _dataFacadeManager.GetDataFacade().
                                    GetView("CardsDepositView", criteriaBuilder.GetPredicate(), null, 0, 0, null, false, out rows);
                }

                if (cards.Rows.Count > 0)
                {
                    cards.Columns.Add("rows", typeof(int));
                    cards.Rows[0]["rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SearchDTO.CardToDepositInternalBallotDTO> cardToDepositInternalBallotDTOs = new List<SearchDTO.CardToDepositInternalBallotDTO>();

                foreach (DataRow row in cards.Rows)
                {
                    cardToDepositInternalBallotDTOs.Add(new SearchDTO.CardToDepositInternalBallotDTO()
                    {
                        PaymentCode = Convert.ToInt32(row["PaymentCode"]),
                        PaymentMethodTypeCode = Convert.ToInt32(row["PaymentMethodTypeCode"]),
                        PaymentMethodTypeName = row["PaymentMethodTypeName"].ToString(),
                        IssuingBankCode = Convert.ToInt32(row["IssuingBankCode"]),
                        BankName = row["BankName"].ToString(),
                        VoucherNumber = row["Voucher"].ToString(),
                        CardNumber = row["CardNumber"].ToString(),
                        ReceiptNumber = Convert.ToInt32(row["ReceiptNumber"]),
                        CurrencyCode = Convert.ToInt32(row["CurrencyCode"]),
                        CurrencyName = row["CurrencyName"].ToString(),
                        ExchangeRate = Convert.ToDecimal(row["ExchangeRate"]),
                        IncomeAmount = Convert.ToDecimal(row["IncomeAmount"]),
                        //si el tipo de moneda es distinto de 0 retorna el valor en moneda extranjera
                        Amount = Convert.ToInt32(row["CurrencyCode"]) == 0 ? Convert.ToDecimal(row["Amount"]) : Convert.ToDecimal(row["IncomeAmount"]),   //Convert.ToDecimal(row["Amount"]),
                        TaxAmount = row["TaxAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TaxAmount"]),
                        CommissionAmount = row["CommissionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["CommissionAmount"]),
                        CardDate = Convert.ToDateTime(row["CardDate"]),
                        Holder = row["Holder"].ToString(),
                        BranchCode = Convert.ToInt32(row["BranchCode"]),
                        Status = Convert.ToInt32(row["Status"]),
                        PaymentTicketItemId = 0,
                        TechnicalTransaction = row["TechnicalTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(row["TechnicalTransaction"]),
                        Rows = cards.Count
                    });
                }

                #endregion

                return cardToDepositInternalBallotDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCreditableHeaderCard
        /// Obtiene la cabecera de tarjeta de crédito acreditable y no acreditable
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <returns>CreditableCardHeaderDTO</returns>
        public SearchDTO.CreditableCardHeaderDTO GetCreditableHeaderCard(int paymentTicketCode, int paymentMethodTypeCode)
        {
            try
            {
                int pageIndex = 0;
                pageIndex--;

                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketCode).And();

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.Amount);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                UIView creditableCards;
                int rows;

                // Tarjeta de crédito acreditable
                if (paymentMethodTypeCode == 4)
                {
                    creditableCards = _dataFacadeManager.GetDataFacade().GetView("CreditableHeaderCardView", criteriaBuilder.GetPredicate(), null, pageIndex, PageSize, null, true, out rows);
                }
                else
                {
                    creditableCards = _dataFacadeManager.GetDataFacade().GetView("NotCreditableHeaderCardView", criteriaBuilder.GetPredicate(), null, pageIndex, PageSize, null, true, out rows);
                }

                #endregion

                #region LoadDTO

                SearchDTO.CreditableCardHeaderDTO creditableCardHeaderDTO = new SearchDTO.CreditableCardHeaderDTO();

                foreach (DataRow row in creditableCards.Rows)
                {
                    if (paymentMethodTypeCode == 4)
                    {
                        creditableCardHeaderDTO.AccountNumber = row["AccountNumber"].ToString();
                    }
                    else
                    {
                        creditableCardHeaderDTO.AccountNumber = "";
                    }

                    creditableCardHeaderDTO.Amount = Convert.ToDecimal(row["Amount"]);
                    creditableCardHeaderDTO.BankCode = Convert.ToInt32(row["BankCode"]);
                    if (paymentMethodTypeCode == 4)
                    {
                        creditableCardHeaderDTO.BankName = row["BankName"].ToString();
                    }
                    else
                    {
                        creditableCardHeaderDTO.BankName = "";
                    }

                    creditableCardHeaderDTO.BranchCode = Convert.ToInt32(row["BranchCode"]);
                    creditableCardHeaderDTO.BranchName = row["BranchName"].ToString();
                    creditableCardHeaderDTO.CommissionAmount = Convert.ToDecimal(row["CommissionAmount"]);
                    creditableCardHeaderDTO.CreditCardTypeCode = Convert.ToInt32(row["CreditCardTypeCode"]);
                    creditableCardHeaderDTO.CreditCardTypeName = row["CreditCardTypeName"].ToString();
                    creditableCardHeaderDTO.CurrencyCode = Convert.ToInt32(row["CurrencyCode"]);
                    creditableCardHeaderDTO.CurrencyName = row["CurrencyName"].ToString();
                    creditableCardHeaderDTO.PaymentMethodTypeCode = Convert.ToInt32(row["PaymentMethodTypeCode"]);
                    creditableCardHeaderDTO.PaymentTicketCode = Convert.ToInt32(row["PaymentTicketCode"]);
                }

                #endregion

                return creditableCardHeaderDTO;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCreditableDetailCard
        /// Obtiene el detalle de la boleta interna de depósito de tarjetas acreditables y no acreditables
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <returns>List<CardToDepositInternalBallotDTO/></returns>
        public List<SearchDTO.CardToDepositInternalBallotDTO> GetCreditableDetailCard(int paymentTicketCode, int paymentMethodTypeCode)
        {
            try
            {
                #region UIView

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Por número de boleta interna
                if (paymentTicketCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketCode).And();
                }

                // Por tipo de pago
                if (paymentMethodTypeCode != -1)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentMethodTypeCode, paymentMethodTypeCode).And();
                }

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.Amount);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                UIView cardDetails;
                int rows;

                if (paymentMethodTypeCode == 4)
                {
                    cardDetails = _dataFacadeManager.GetDataFacade().GetView("CreditableDetailCardView",
                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
                }
                else
                {
                    cardDetails = _dataFacadeManager.GetDataFacade().GetView("NotCreditableDetailCardView",
                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);
                }

                if (cardDetails.Rows.Count > 0)
                {
                    cardDetails.Columns.Add("rows", typeof(int));
                    cardDetails.Rows[0]["rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SearchDTO.CardToDepositInternalBallotDTO> cardToDepositInternalBallotDTOs = new List<SearchDTO.CardToDepositInternalBallotDTO>();

                foreach (DataRow row in cardDetails.Rows)
                {
                    cardToDepositInternalBallotDTOs.Add(new SearchDTO.CardToDepositInternalBallotDTO()
                    {
                        PaymentCode = Convert.ToInt32(row["PaymentCode"]),
                        PaymentMethodTypeCode = Convert.ToInt32(row["PaymentMethodTypeCode"]),
                        PaymentMethodTypeName = row["PaymentMethodTypeName"].ToString(),
                        IssuingBankCode = Convert.ToInt32(row["IssuingBankCode"]),
                        BankName = row["BankName"].ToString(),
                        VoucherNumber = row["Voucher"].ToString(),
                        CardNumber = row["CardNumber"].ToString(),
                        ReceiptNumber = Convert.ToInt32(row["ReceiptNumber"]),
                        CurrencyCode = Convert.ToInt32(row["CurrencyCode"]),
                        CurrencyName = row["CurrencyName"].ToString(),
                        ExchangeRate = Convert.ToDecimal(row["ExchangeRate"]),
                        IncomeAmount = Convert.ToDecimal(row["IncomeAmount"]),
                        Amount = Convert.ToDecimal(row["Amount"]),
                        CardDate = Convert.ToDateTime(row["CardDate"]),
                        Holder = row["Holder"].ToString(),
                        BranchCode = Convert.ToInt32(row["BranchCode"]),
                        Status = Convert.ToInt32(row["Status"]),
                        PaymentTicketItemId = Convert.ToInt32(row["PaymentTicketItemCode"]),
                        TaxAmount = row["Taxes"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Taxes"]),
                        CommissionAmount = row["Commission"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Commission"]),
                        Rows = cardDetails.Count
                    });
                }

                #endregion

                return cardToDepositInternalBallotDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

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


        #endregion Private Methods


        public List<PaymentTicketDTO> GetPaymentTicketsByCollectId(int collectId)
        {
            try
            {
                Business.PaymentTicketBusiness paymentTicketBusiness = new Business.PaymentTicketBusiness();
                return paymentTicketBusiness.GetPaymentTicketItemsByCollectId(collectId).ToDTOs().ToList();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentTicketsByCollectId);
            }
        }

        public int GetPaymentTicketSequence()
        {
            try
            {
                Business.PaymentTicketBusiness paymentTicketBusiness = new Business.PaymentTicketBusiness();
                return paymentTicketBusiness.GetPaymentTicketSequence();
            }
            catch (BusinessException)
            {
                throw new BusinessException(Resources.Resources.ErrorGetPaymentTicketSequence);
            }
        }
    }
}
