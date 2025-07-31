using System.ServiceModel;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices
{
    [ServiceContract]
    public interface IAccountingGeneralLedgerApplicationService
    {
        /// <summary>
        /// Saves journal entry (ledger) for Collect
        /// </summary>
        /// <param name="collectParameter">Collect serialized</param>
        /// <returns>New identifier for general ledger</returns>
        [OperationContract]
        int SaveCollectJournalEntry(string collectParameter);

        /// <summary>
        /// Saves journal entry (ledger) for payment ballot
        /// </summary>
        /// <param name="paymentBallotAccountingParameters">Payment ballot serialized</param>
        /// <returns>New identifier for general ledger</returns>
        [OperationContract]
        string RecordPaymentBallot(string paymentBallotAccountingParameters);

        /// <summary>
        /// Saves journal entry (ledger) for Application
        /// </summary>
        /// <param name="applicationParameters">Application serialized</param>
        /// <returns>New identifier for general ledger</returns>
        [OperationContract]
        int SaveApplicationJournalEntry(string applicationParameters);

        [OperationContract]
        int JournalEntryChecks(string accountigParameters);

        /// <summary>
        /// Saves a journal entry operation
        /// </summary>
        /// <param name="journalEntryParameters">Parameters</param>
        /// <returns></returns>
        [OperationContract]
        int SaveJournalEntry(string journalEntryParameters);

        /// <summary>
        /// Reverse journal entry
        /// </summary>
        /// <param name="journalEntryParameters">Serialized parameters</param>
        /// <returns>New journal entry identifier</returns>
        [OperationContract]
        int ReverseJournalEntry(string journalEntryParameters);
    }
}
