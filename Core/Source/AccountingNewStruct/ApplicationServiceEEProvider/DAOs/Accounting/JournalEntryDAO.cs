//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.Utilities.DataFacade;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class JournalEntryDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region JournalEntry

        /// <summary>
        /// SaveJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public Models.Imputations.JournalEntry SaveJournalEntry(Models.Imputations.JournalEntry journalEntry)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.JournalEntry journalEntryEntity = EntityAssembler.CreateJournalEntry(journalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(journalEntryEntity);

                // Return del model
                return ModelAssembler.CreateJournalEntry(journalEntryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public Models.Imputations.JournalEntry UpdateJournalEntry(Models.Imputations.JournalEntry journalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.JournalEntry.CreatePrimaryKey(journalEntry.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.JournalEntry journalEntryEntity = (ACCOUNTINGEN.JournalEntry)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                journalEntryEntity.AccountingDate = journalEntry.AccountingDate;
                journalEntryEntity.BranchCode = journalEntry.Branch.Id;
                journalEntryEntity.Comments = journalEntry.Comments;
                journalEntryEntity.CompanyCode = journalEntry.Company.IndividualId;
                journalEntryEntity.Description = journalEntry.Description;
                journalEntryEntity.IndividualId = journalEntry.Payer.IndividualId;
                journalEntryEntity.PersonTypeCode = journalEntry.PersonType.Id;
                journalEntryEntity.SalesPointCode = journalEntry.SalePoint.Id;
                journalEntryEntity.Status = journalEntry.Status;


                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(journalEntryEntity);

                // Return del model
                return journalEntry;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public Models.Imputations.JournalEntry GetJournalEntry(Models.Imputations.JournalEntry journalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.JournalEntry.CreatePrimaryKey(journalEntry.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.JournalEntry journalEntryEntity = (ACCOUNTINGEN.JournalEntry)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateJournalEntry(journalEntryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Get Journal Entry by Identifier
        /// </summary>
        /// <param name="journalEntryId">Journal Entry identifier</param>
        /// <returns>Journal Entry</returns>
        public Models.Imputations.JournalEntry GetJournalEntryById(int journalEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.JournalEntry.CreatePrimaryKey(journalEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.JournalEntry entityJournalEntry = (ACCOUNTINGEN.JournalEntry)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateJournalEntry(entityJournalEntry);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Update journal entry status
        /// </summary>
        /// <param name="journalEntryId">Journal entry identifier</param>
        /// <param name="status">new status</param>
        /// <returns>True if update journal entry</returns>
        public bool UpdateJournalEntryStatus(int journalEntryId, int status)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.JournalEntry.CreatePrimaryKey(journalEntryId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.JournalEntry entityJournalEntry =  (ACCOUNTINGEN.JournalEntry)DataFacadeManager.GetObject(primaryKey);

                if (entityJournalEntry != null && entityJournalEntry.JournalEntryCode > 0)
                {
                    entityJournalEntry.Status = status;

                    // Realizar las operaciones con los entities utilizando DAF
                    return DataFacadeManager.Update(entityJournalEntry);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return false;
        }

        #endregion

    }
}
