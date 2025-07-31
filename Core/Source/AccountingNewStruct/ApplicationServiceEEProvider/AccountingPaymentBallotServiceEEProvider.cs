using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using CoreTransaction = Sistran.Core.Framework.Transactions;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACC = Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;

using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;
using Sistran.Core.Application.AccountingServices.DTOs.GeneralLedger;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingPaymentBallotServiceEEProvider : IAccountingPaymentBallotService
    {
        #region Constants

        #endregion

        #region Instance Variables

        #region Interfaz

        internal static readonly IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        //readonly AccountingImputationServiceEEProvider _imputationService = new AccountingImputationServiceEEProvider();

        #endregion Interfaz

        #region DAOs

        readonly PaymentBallotDAO _paymentBallotDAO = new PaymentBallotDAO();
        readonly PaymentTicketBallotDAO _paymentTicketBallotDAO = new PaymentTicketBallotDAO();
        readonly PaymentTicketDAO _paymentTicketDAO = new PaymentTicketDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region PaymentBallot

        public List<PaymentBallotResponsesDTO> SaveAccountingPaymentBallot(PaymentBallotAccountingDTO paymentBallotAccounting)
        {
            try
            {
                int result;
                string message = "";
                // Flag para mostrar mensaje de contabilidad en EE
                bool showMessage = true;

                PaymentBallotDTO paymentBallot = new PaymentBallotDTO();

                AmountDTO ballotAmount = new AmountDTO() { Value = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotAmount };
                AmountDTO bankAmount = new AmountDTO() { Value = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotBankAmount };
                BankDTO bank = new BankDTO() { Id = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotBankId };
                SEARCH.CurrencyDTO currency = new SEARCH.CurrencyDTO() { Id = paymentBallotAccounting.PaymentBallotParameters.PaymentCurrency };

                paymentBallot.Id = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotId;
                paymentBallot.BallotNumber = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotNumber;
                paymentBallot.Bank = bank;
                paymentBallot.AccountNumber = paymentBallotAccounting.PaymentBallotParameters.PaymentAccountNumber;
                paymentBallot.Currency = currency;
                paymentBallot.BankDate = paymentBallotAccounting.PaymentBallotParameters.PaymentBankDate;
                paymentBallot.Amount = ballotAmount;
                paymentBallot.BankAmount = bankAmount;
                paymentBallot.Status = paymentBallotAccounting.PaymentBallotParameters.PaymentStatus;

                string receiptNumber = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotNumber;
                DateTime receiptDate = paymentBallotAccounting.PaymentBallotParameters.PaymentBankDate;

                List<PaymentTicketDTO> paymentTickets = new List<PaymentTicketDTO>();
                List<PaymentDTO> payments = new List<PaymentDTO>();

                if (paymentBallotAccounting.TypeId == 1)
                {
                    paymentBallotAccounting.TypeId = (int)PaymentMethods.CurrentCheck;
                }
                else
                {
                    paymentBallotAccounting.TypeId = (int)PaymentMethods.CreditableCreditCard;
                }

                if (paymentBallotAccounting.PaymentBallotParameters.PaymentTicketBallotModels != null)
                {
                    for (int i = 0; i < paymentBallotAccounting.PaymentBallotParameters.PaymentTicketBallotModels.Count; i++)
                    {
                        List<SEARCH.PaymentTicketItemDTO> paymentTicketItems;
                        paymentTicketItems = (DelegateService.accountingPaymentTicketService.GetPaymentTicketItemsByPaymentTicketCode(paymentBallotAccounting.PaymentBallotParameters.PaymentTicketBallotModels[i].PaymentTicketBallotId));

                        for (int j = 0; j < paymentTicketItems.Count; j++)
                        {
                            PaymentDTO paymentCode = new PaymentDTO() { Id = paymentTicketItems[j].PaymentCode };
                            payments.Add(paymentCode);
                        }

                        PaymentTicketDTO paymentTicket = new PaymentTicketDTO() { Id = paymentBallotAccounting.PaymentBallotParameters.PaymentTicketBallotModels[i].PaymentTicketBallotId };
                        paymentTicket.PaymentMethod = paymentBallotAccounting.TypeId;

                        paymentTicket.Payments = payments;
                        paymentTickets.Add(paymentTicket);
                    }
                    paymentBallot.PaymentTickets = paymentTickets;
                }

                paymentBallot = SavePaymentBallot(paymentBallot, paymentBallotAccounting.UserId);

                #region Accounting

                // Se dispara el método de contabilización
                if (Sistran.Core.Application.Utilities.Helper.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_ENABLED_GENERAL_LEGDER).ToString() == "true")
                {
                    int decimalPlaces = 2;
                    var accountingAccountId = DelegateService.accountingApplicationService.GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(bank.Id, currency.Id, paymentBallot.AccountNumber);

                    if (CommonBusiness.GetBooleanParameter(Enums.AccountingKeys.ACC_PAYMENT_BALLOT_AID_CHECK))
                    {
                        var automaticUserId = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_AUTOMATIC_APPLICATION_USER);
                        if (automaticUserId >= 0 && paymentBallotAccounting.UserId == automaticUserId)
                        {
                            var accountingIdforAutomaticUserId = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_PAYMENT_BALLOT_AID);
                            if (accountingAccountId > 0 && accountingAccountId != accountingIdforAutomaticUserId)
                                accountingAccountId = accountingIdforAutomaticUserId;
                        }
                    }

                    GenericJournalEntryDTO journalEntryParameters = new GenericJournalEntryDTO()
                    {
                        AccountingMovementTypeId = paymentBallotAccounting.TypeId,
                        UserId = paymentBallotAccounting.UserId,
                        RegisterDate = receiptDate,
                        AccountingDate = paymentBallotAccounting.PaymentBallotParameters.PaymentBankDate,
                        ModuleId = Convert.ToInt32(ApplicationTypes.Collect),
                        TechnicalTransaction = paymentBallot.Transaction.TechnicalTransaction,
                        JournalEntryItems = new List<GenericJournalEntryItemDTO>(),
                        BridgeAccountId = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_BRIDGE_PAYMENT_BALLOT),
                        AccountingTypeId = Convert.ToInt32(AccountingType.Ballot),
                        BankId = paymentBallotAccounting.PaymentBallotParameters.PaymentBallotBankId,
                        AccountingNumber = paymentBallotAccounting.PaymentBallotParameters.PaymentAccountNumber,
                        AccountingAccountId = accountingAccountId,
                        Description = Resources.Resources.AccountDepositBallot + " " + receiptNumber
                    };

                    PaymentTicketDAO paymentTicketDAO = new PaymentTicketDAO();
                    List<Models.Payments.PaymentTicket> tickets = paymentTicketDAO.GetPaymentsByPaymentBallotId(paymentBallot.Id);
                    decimal cashAmount = 0;
                    journalEntryParameters.JournalEntryItems = new List<GenericJournalEntryItemDTO>();
                    bool assigned = false;

                    int bankReconciliation = 0, recieptNumber = 0;
                    DateTime recieptDate = new DateTime();

                    if (tickets != null && tickets.Count > 0)
                    {
                        journalEntryParameters.BranchId = tickets[0].Branch.Id;
                        tickets.ForEach(t =>
                        {
                            cashAmount += t.CashAmount.Value;

                            if (t.Payments != null && t.Payments.Count > 0)
                            {
                                t.Payments.ForEach(p =>
                                {
                                    GenericJournalEntryItemDTO journalEntryItem = new GenericJournalEntryItemDTO();
                                    journalEntryItem.CurrencyCode = p.Amount.Currency.Id;
                                    journalEntryItem.BranchCode = t.Branch.Id;
                                    journalEntryItem.Amount = p.Amount.Value;
                                    journalEntryItem.LocalAmount = p.LocalAmount.Value;
                                    journalEntryItem.ExchangeRate = p.ExchangeRate.SellAmount;
                                    journalEntryItem.PaymentCode = p.Id;
                                    journalEntryItem.PaymentMethodTypeCode = p.PaymentMethod.Id;
                                    //journalEntryItem.PayerId =

                                    if (p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher)
                                        || p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                                    {
                                        if (!assigned)
                                        {
                                            recieptNumber = Convert.ToInt32(p.DocumentNumber);
                                            recieptDate = Convert.ToDateTime(p.DatePayment);
                                            bankReconciliation = Convert.ToInt32(BankReconciliation.Yes);
                                            assigned = true;
                                        }
                                        if (p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher))
                                        {
                                            journalEntryItem.AccountingAccountId = DelegateService.accountingApplicationService.GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(
                                                p.IssuingBankCode, p.Amount.Currency.Id, p.IssuingAccountNumber);
                                            journalEntryItem.PaymentMethodTypeCode = Convert.ToInt32(PaymentMethods.Cash);
                                            cashAmount -= p.Amount.Value;
                                        }
                                        else if (p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                                            journalEntryItem.PaymentMethodTypeCode = Convert.ToInt32(PaymentMethods.CurrentCheck);
                                    }
                                    journalEntryParameters.JournalEntryItems.Add(journalEntryItem);
                                });
                            }
                        });
                    }

                    if (cashAmount > 0)
                    {
                        var exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, tickets[0].Currency.Id).SellAmount;

                        GenericJournalEntryItemDTO accountingListParametersDTO = new GenericJournalEntryItemDTO();
                        accountingListParametersDTO.CurrencyCode = tickets[0].Currency.Id;
                        accountingListParametersDTO.BranchCode = tickets[0].Branch.Id;
                        accountingListParametersDTO.Amount = cashAmount;
                        accountingListParametersDTO.LocalAmount = Math.Round(cashAmount * exchangeRate, decimalPlaces);
                        accountingListParametersDTO.ExchangeRate = exchangeRate;
                        accountingListParametersDTO.PaymentCode = 0;
                        accountingListParametersDTO.PaymentMethodTypeCode = Convert.ToInt32(PaymentMethods.Cash);
                        journalEntryParameters.JournalEntryItems.Add(accountingListParametersDTO);
                    }


                    if (journalEntryParameters.JournalEntryItems != null && journalEntryParameters.JournalEntryItems.Count == 1 && assigned)
                    {
                        journalEntryParameters.RecieptDate = recieptDate;
                        journalEntryParameters.RecieptNumber = recieptNumber;
                        journalEntryParameters.BankReconciliationId = bankReconciliation;
                    }
                    else
                    {
                        journalEntryParameters.RecieptDate = paymentBallot.BankDate;
                        journalEntryParameters.RecieptNumber = Convert.ToInt32(paymentBallot.BallotNumber);
                        journalEntryParameters.BankReconciliationId = Convert.ToInt32(BankReconciliation.Yes);
                    }

                    string serializedAccountingParameters = Newtonsoft.Json.JsonConvert.SerializeObject(journalEntryParameters);

                    result = DelegateService.accountingGeneralLedgerApplicationService.SaveJournalEntry(serializedAccountingParameters);

                    if (result > 0)
                    {
                        message = Resources.Resources.IntegrationSuccessMessage + " " + paymentBallot.Transaction.TechnicalTransaction;

                    }
                    else if (result == 0)
                    {
                        message = Resources.Resources.AccountingIntegrationUnbalanceEntry;
                    }
                    else if (result == -1)
                    {
                        message = Resources.Resources.ErrorApplicationNotFound;
                    }
                    else if (result == -2)
                    {
                        message = Resources.Resources.EntryRecordingError;
                    }
                }
                else
                {
                    message = Convert.ToString(Resources.Resources.IntegrationServiceDisabledLabel);
                    showMessage = false;
                }

                List<PaymentBallotResponsesDTO> paymentBallotResponses = new List<PaymentBallotResponsesDTO>();

                Sistran.Core.Application.UniqueUserServices.Models.User User = DelegateService.uniqueUserService.GetUserById(paymentBallotAccounting.UserId);
                //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
                Models.Collect.Collect movementToControl = new Models.Collect.Collect()
                {
                    Id = paymentBallot.Transaction.TechnicalTransaction
                };
                Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
                integration2GBusiness.Save(movementToControl.ToModelIntegration(3));

                paymentBallotResponses.Add(new PaymentBallotResponsesDTO()
                {
                    Id = paymentBallot.Id,
                    BallotId = paymentBallot.BallotNumber,
                    Total = paymentBallot.Amount.Value,
                    Date = DateTime.Now.ToString(),
                    User = User.AccountName,
                    Message = message,
                    ShowMessage = showMessage,
                    TechnicalTransaction = paymentBallot.Transaction.TechnicalTransaction,
                });

                return paymentBallotResponses;

            }
            catch (Exception)
            {

                throw;
            }

            #endregion Accounting
        }


        /// <summary>
        /// SavePaymentBallot
        /// Graba una boleta de depósito
        /// </summary>
        /// <param name="paymentBallot"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentBallot</returns>
        public ACC.PaymentBallotDTO SavePaymentBallot(ACC.PaymentBallotDTO paymentBallotDTO, int userId)
        {
            ACC.PaymentBallotDTO newPaymentBallot;

            using (Context.Current)
            {
                CoreTransaction.Transaction transaction = new CoreTransaction.Transaction();
                Models.PaymentBallot paymentBallot = paymentBallotDTO.ToModel();
                try
                {
                    PaymentTicketDAO paymentTicketDAO = new PaymentTicketDAO();
                    Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments.PaymentTicket paymentTickets = paymentTicketDAO.GetPaymentTicket(paymentBallot.PaymentTickets[0]);

                    TechnicalTransactionParameterDTO parameter = new TechnicalTransactionParameterDTO()
                    {
                        BranchId = paymentTickets.Branch.Id,
                    };

                    TechnicalTransactionDTO technicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(parameter);

                    paymentBallot.Transaction = new ACC.TransactionDTO() { TechnicalTransaction = technicalTransaction.Id }.ToModel();

                    newPaymentBallot = _paymentBallotDAO.SavePaymentBallot(paymentBallot,
                                             new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))), userId).ToDTO();

                    const int paymentTicketItemId = 0; //Identificador para clave autonumérica.

                    foreach (var paymentTicket in paymentBallot.PaymentTickets)
                    {
                        _paymentTicketBallotDAO.SavePaymentTicketBallot(paymentTicketItemId, paymentTicket.Id, newPaymentBallot.Id);
                        newPaymentBallot.PaymentTickets.Add(paymentTicket.ToDTO());
                        // Graba PaymentLog
                        for (int i = 0; i < paymentTicket.Payments.Count; i++)
                        {
                            new AccountingPaymentServiceEEProvider().SavePaymentLog(Convert.ToInt32(ActionTypes.PaymentBallotDeposit),
                                                           newPaymentBallot.Id,
                                                           paymentTicket.Payments[i].Id,
                                                           Convert.ToInt32(PaymentStatus.Deposited),
                                                           userId);
                        }

                        Models.Payments.PaymentTicket updatePaymentTicket = new Models.Payments.PaymentTicket();
                        updatePaymentTicket.Id = paymentTicket.Id;
                        updatePaymentTicket = _paymentTicketDAO.GetPaymentTicket(updatePaymentTicket);
                        updatePaymentTicket.Status = Convert.ToInt32(PaymentStatus.Deposited);

                        _paymentTicketDAO.UpdatePaymentTicket(updatePaymentTicket, DateTime.Now, userId, 1);
                    }

                    transaction.Complete();
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();

                    throw new BusinessException(Resources.Resources.BusinessException);
                }

                return newPaymentBallot;
            }
        }

        /// <summary>
        /// GetCheckBallots
        /// Obtiene todas las boletas para ser depositadas menos las ya depositadas
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <returns>List<PaymentBallotDTO/></returns>
        public List<SEARCH.PaymentBallotDTO> GetCheckBallots(int userId, int bankCode, string accountNumber, int branchId)
        {
            try
            {

                UIView depositedPaymentBallotUiwiew = GetCheckBallotsUIview(userId, bankCode, accountNumber, branchId);

                #region LoadDTO

                List<SEARCH.PaymentBallotDTO> paymentBallotDTOs = new List<SEARCH.PaymentBallotDTO>();

                foreach (DataRow dataRow in depositedPaymentBallotUiwiew.Rows)
                {
                    paymentBallotDTOs.Add(new SEARCH.PaymentBallotDTO
                    {
                        DepositBallotAccountNumber = dataRow["AccountNumber"].ToString(),
                        DepositBallotAmount = Convert.ToDecimal(dataRow["Amount"]),
                        DepositBallotBankId = Convert.ToInt32(dataRow["BankCode"]),
                        DepositBallotCashAmount = Convert.ToDecimal(dataRow["CashAmount"]),
                        Currency = dataRow["Currency"].ToString(),
                        DepositBallotBankDescription = dataRow["Description"].ToString(),
                        DepositBallotId = Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        DepositBallotRegisterDate = Convert.ToDateTime(dataRow["RegisterDate"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        UserId = Convert.ToInt32(dataRow["UserId"]),
                        Rows = dataRow["Rows"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Rows"])
                    });
                }

                #endregion

                return paymentBallotDTOs;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }



        /// <summary>
        /// GetTechnicalTransactionForPaymentBallotByPaymentCode
        /// obtiene tehcnical transaction de la boleta a la que se va arechazar un cheque
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>int</returns>
        public int GetTechnicalTransactionForPaymentBallotByPaymentCode(int paymentCode)
        {
            try
            {
                PaymentTicketDAO paymentTicketDAO = new PaymentTicketDAO();
                return paymentTicketDAO.GetTechnicalTransactionForPaymentBallotByPaymentCode(paymentCode);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCreditCardDepositBallots
        /// Obtiene todas las boletas de tarjetas de credito para ser depositadas menos las ya depositadas y las anuladas
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param> 
        /// <param name="branchId"></param> 
        /// <returns>List<CreditCardPaymentBallotDTO/></returns>
        public List<SEARCH.CreditCardPaymentBallotDTO> GetCreditCardDepositBallots(int userId, int status, int creditCardTypeCode,
                                                                            int paymentTicketCode, int bankCode, string accountNumber,
                                                                            int branchId)
        {
            try
            {

                #region LoadUIVIEW

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.Status, status); // Por Status
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CreditCardTypeCode, creditCardTypeCode); // Por Conducto

                // Por Número de Boleta Interna
                if (paymentTicketCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, paymentTicketCode);
                }

                // Por Banco Receptor
                if (bankCode != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BankCode, bankCode);
                }

                // Por Número de Cuenta
                if (!accountNumber.Equals("-1"))
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.AccountNumber, accountNumber);
                }

                // Por Sucursal
                if (branchId != -1)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, branchId);
                }

                UIView depositedPaymentBallotUiwiew = dataFacadeManager.GetDataFacade().GetView("PaymentCreditCardTypeView",
                                                      criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (depositedPaymentBallotUiwiew.Rows.Count > 0)
                {
                    depositedPaymentBallotUiwiew.Columns.Add("Rows", typeof(int));
                    depositedPaymentBallotUiwiew.Rows[0]["Rows"] = rows;
                }

                #endregion

                #region LoadDTO

                List<SEARCH.CreditCardPaymentBallotDTO> creditCardPaymentBallotDTOs = new List<SEARCH.CreditCardPaymentBallotDTO>();

                foreach (DataRow dataRow in depositedPaymentBallotUiwiew.Rows)
                {
                    decimal taxes;
                    decimal commission;

                    if (dataRow["Taxes"] == DBNull.Value)
                    {
                        taxes = 0;
                    }
                    else
                    {
                        taxes = Convert.ToInt32(dataRow["Taxes"]);
                    }
                    if (dataRow["Commission"] == DBNull.Value)
                    {
                        commission = 0;
                    }
                    else
                    {
                        commission = Convert.ToDecimal(dataRow["Commission"]);
                    }

                    creditCardPaymentBallotDTOs.Add(new SEARCH.CreditCardPaymentBallotDTO()
                    {
                        CreditCardTypeCode = Convert.ToInt32(dataRow["CreditCardTypeCode"]),
                        CreditCardDescription = dataRow["CreditCardDescription"].ToString(),
                        BankCode = Convert.ToInt32(dataRow["BankCode"]),
                        BankDescription = dataRow["BankDescription"].ToString(),
                        AccountNumber = dataRow["AccountNumber"].ToString(),
                        PaymentTicketCode = Convert.ToInt32(dataRow["PaymentTicketCode"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        CurrencyDescription = dataRow["CurrencyDescription"].ToString(),
                        RegisterDate = Convert.ToDateTime(dataRow["RegisterDate"]),
                        Status = Convert.ToInt32(dataRow["Status"]),
                        UserId = Convert.ToInt32(dataRow["UserId"]),
                        Taxes = taxes,
                        Commission = commission,
                        BranchCode = Convert.ToInt32(dataRow["BranchCode"]),
                        BranchDescription = dataRow["BranchDescription"].ToString(),
                        Rows = dataRow["Rows"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Rows"])
                    });
                }

                #endregion

                return creditCardPaymentBallotDTOs;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region PaymentTicketBallot

        /// <summary>
        /// SavePaymentTicketBallot
        /// Graba una boleta interna 
        /// </summary>
        /// <param name="paymentTicketBallotId"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="paymentBallotId"></param>
        /// <returns>int</returns>
        public int SavePaymentTicketBallot(int paymentTicketBallotId, int paymentTicketId, int paymentBallotId)
        {
            try
            {
                return _paymentTicketBallotDAO.SavePaymentTicketBallot(paymentTicketBallotId, paymentTicketId, paymentBallotId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ValidateExternalBallotDeposited
        /// Revisa si una boleta esta depositada en boleta externa 
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <returns>bool</returns>
        public bool ValidateExternalBallotDeposited(int paymentTicketCode)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketBallot.Properties.PaymentTicketCode, paymentTicketCode);

                BusinessCollection businessCollection = new BusinessCollection(dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PaymentTicketBallot), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
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

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// GetCheckBallotsUIview
        /// Obtiene las boletas de depósito para ser depositadas menos las ya depositadas en una UIView
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <returns>UIView</returns>
        private UIView GetCheckBallotsUIview(int userId, int bankCode, string accountNumber, int branchId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.BankCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(bankCode);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.AccountNumber);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(accountNumber);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(branchId);
                criteriaBuilder.And();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.UserId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);

                UIView ballots = dataFacadeManager.GetDataFacade().GetView("BallotDepositView",
                                               criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                if (ballots.Rows.Count > 0)
                {
                    ballots.Columns.Add("Rows", typeof(int));
                    ballots.Rows[0]["rows"] = rows;
                }

                return ballots;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Private Methods

    }
}
