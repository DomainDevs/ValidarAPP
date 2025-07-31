using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class LogJournalEntryItemDAO
    {
        #region Save

        /// <summary>
        /// SaveJournalEntryItem
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <param name="journalEntryId"></param>
        /// <returns>JournalEntryItem</returns>
        public void SaveJournalEntryItem(LogJournalEntryItem logJournalEntryItem)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.LogJournalEntryItem entityLogJournalEntry = EntityAssembler.CreateLogJournalEntryItem(logJournalEntryItem);

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityLogJournalEntry);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        #endregion Save
    }
}
