using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class JournalEntryBusiness
    {
        public JournalEntry SaveJournalEntry(JournalEntry journalEntry)
        {
            JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
            return journalEntryDAO.SaveJournalEntry(journalEntry);
        }

        public bool UpdateJournalEntryStatus(int journalEntryId, int status)
        {
            JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
            return journalEntryDAO.UpdateJournalEntryStatus(journalEntryId, status);
        }
    }
}
