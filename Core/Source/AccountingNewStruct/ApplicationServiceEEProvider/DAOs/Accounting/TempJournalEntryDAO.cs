using System;
using System.Configuration;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempJournalEntryDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region TempJournalEntry

        /// <summary>
        /// SaveTempJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntry SaveTempJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempJournalEntry journalEntryEntity = EntityAssembler.CreateTempJournalEntry(journalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(journalEntryEntity);

                // Return del model
                return ModelAssembler.CreateTempJournalEntry(journalEntryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        public JournalEntry UpdateTempJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempJournalEntry.CreatePrimaryKey(journalEntry.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempJournalEntry journalEntryEntity = (ACCOUNTINGEN.TempJournalEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

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
                return ModelAssembler.CreateTempJournalEntry(journalEntryEntity);
            }
            catch (BusinessException)
            {
                throw new BusinessException(ConfigurationManager.AppSettings["UnhandledExceptionMsj"]);
            }
        }

        /// <summary>
        /// DeleteTempJournalEntry
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempJournalEntry(int journalEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempJournalEntry.CreatePrimaryKey(journalEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempJournalEntry journalEntryEntity = (ACCOUNTINGEN.TempJournalEntry)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (journalEntryEntity != null)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(journalEntryEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        public JournalEntry GetTempJournalEntry(int journalEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempJournalEntry.CreatePrimaryKey(journalEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempJournalEntry entityJournalEntry = (ACCOUNTINGEN.TempJournalEntry)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (entityJournalEntry == null)
                {
                    return null;
                }

                // Return del model
                return ModelAssembler.CreateTempJournalEntry(entityJournalEntry);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
