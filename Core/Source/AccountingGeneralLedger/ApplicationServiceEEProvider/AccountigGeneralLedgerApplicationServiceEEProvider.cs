using System;
using System.Collections.Generic;
using Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs;
using Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider
{
    public class AccountingGeneralLedgerApplicationServiceEEProvider : IAccountingGeneralLedgerApplicationService
    {
        public int SaveCollectJournalEntry(string collectParameter)
        {
            try
            {
                AccountingGeneralLedgerBusiness accountingGeneralLedgerBusiness = new AccountingGeneralLedgerBusiness();
                return accountingGeneralLedgerBusiness.SaveCollectJournalEntry(collectParameter);
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                return 0;
            }
        }
		
        public string RecordPaymentBallot(string paymentBallotAccountingParameters)
        {

            PaymentBallotAccountingParametersDTO paymentBallotAccountingParametersDTO = new PaymentBallotAccountingParametersDTO();

            paymentBallotAccountingParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentBallotAccountingParametersDTO>(paymentBallotAccountingParameters);

            string message = "";
            int transactionNumber = paymentBallotAccountingParametersDTO.PaymentBallot.Transaction.TechnicalTransaction;

            try
            {
                #region Parameters

                int moduleId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_COLLECTING).ToString());

                // Se obtiene los parámetros para la contabilización de la aplicación
                CheckBallotAccountingParameterDTO checkBallotAccountingParameter = paymentBallotAccountingParametersDTO.CheckBallotAccountingParameters;

                #endregion Parameters

                #region JournalEntryHeader

                // Generaración del asiento.
                JournalEntryDTO journalEntry = new JournalEntryDTO();

                journalEntry.Id = 0;
                journalEntry.AccountingMovementType = new AccountingMovementTypeDTO();
                journalEntry.ModuleDateId = moduleId;
                journalEntry.Branch = new BranchDTO() { Id = checkBallotAccountingParameter.BranchCode };
                journalEntry.SalePoint = new SalePointDTO() { Id = 0 };
                journalEntry.EntryNumber = 0;
                journalEntry.TechnicalTransaction = transactionNumber;
                journalEntry.Description = Resources.Resources.AccountDepositBallot + " " + checkBallotAccountingParameter.BallotNumber;
                //journalEntry.AccountingDate = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                journalEntry.AccountingDate = paymentBallotAccountingParametersDTO.AccountingDate;
                journalEntry.RegisterDate = DateTime.Now;
                journalEntry.Status = 1; // activo
                journalEntry.UserId = paymentBallotAccountingParametersDTO.UserId;

                journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

                #endregion JournalEntryHeader

                #region ValidateAndSave

                AccountingParameterDTO accountingJournalEntryParametersCollection = new AccountingParameterDTO();

                accountingJournalEntryParametersCollection.JournalEntry = journalEntry;
                accountingJournalEntryParametersCollection.checkBallotAccountingParameter = checkBallotAccountingParameter;
                accountingJournalEntryParametersCollection.ReceiptDate = paymentBallotAccountingParametersDTO.ReceiptDate;
                accountingJournalEntryParametersCollection.ReceiptNumber = paymentBallotAccountingParametersDTO.ReceiptNumber;
                accountingJournalEntryParametersCollection.TypeId = paymentBallotAccountingParametersDTO.TypeId;
                accountingJournalEntryParametersCollection.IndividualId = paymentBallotAccountingParametersDTO.IndividualId;
                accountingJournalEntryParametersCollection.SourceCode = 0;
                if (paymentBallotAccountingParametersDTO.PaymentBallot.PaymentTickets.Count > 0)
                {
                    if (paymentBallotAccountingParametersDTO.PaymentBallot.PaymentTickets[0].Payments.Count > 0)
                    {
                        accountingJournalEntryParametersCollection.SourceCode = paymentBallotAccountingParametersDTO.PaymentBallot.PaymentTickets[0].Payments[0].Id;
                    }
                }
                
                string journalEntryParameters = Newtonsoft.Json.JsonConvert.SerializeObject(accountingJournalEntryParametersCollection);

                int entryNumber = DelegateService.generalLedgerService.AccountingPaymentBallot(journalEntryParameters);

                if (entryNumber > 0)
                {
                    message =  String.Format(Resources.Resources.IntegrationSuccessMessageTech, journalEntry.TechnicalTransaction);
                }
                if (entryNumber == 0)
                {
                    message = Resources.Resources.AccountingIntegrationUnbalanceEntry;
                }
                if (entryNumber == -2)
                {
                    message = Resources.Resources.EntryRecordingError;
                }

                #endregion ValidateAndSave

            }
            catch (Exception)
            {
                message = Resources.Resources.EntryRecordingError;
            }

            return message;
        }

        public int SaveApplicationJournalEntry(string applicationParameters)
        {
            try
            {
                AccountingGeneralLedgerBusiness accountingGeneralLedgerBusiness = new AccountingGeneralLedgerBusiness();
                return accountingGeneralLedgerBusiness.SaveApplicationJournalEntry(applicationParameters, 0);
            }
            catch (BusinessException exception)
            {
                var message = exception.Message;
                return 0;
            }
        }

        public int JournalEntryChecks(string accountigParameters) //TODO: Cheques
        {
            try
            {
                AccountingGeneralLedgerBusiness accountingGeneralLedgerBusiness = new AccountingGeneralLedgerBusiness();
                return accountingGeneralLedgerBusiness.SaveChecksJournalEntry(accountigParameters);
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                return 0;
            }

        }

        public int SaveJournalEntry(string journalEntryParameters)
        {
            try
            {
                AccountingGeneralLedgerBusiness accountingGeneralLedgerBusiness = new AccountingGeneralLedgerBusiness();
                return accountingGeneralLedgerBusiness.SaveJournalEntry(journalEntryParameters);
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                return 0;
            }
        }

        public int ReverseJournalEntry(string journalEntryParameters)
        {
            try
            {
                AccountingGeneralLedgerBusiness accountingGeneralLedgerBusiness = new AccountingGeneralLedgerBusiness();
                return accountingGeneralLedgerBusiness.ReverseJournalEntry(journalEntryParameters);
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                return -1;
            }
        }
    }
}
