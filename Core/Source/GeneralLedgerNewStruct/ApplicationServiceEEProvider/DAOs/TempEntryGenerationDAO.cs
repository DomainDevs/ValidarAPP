//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using System;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class TempEntryGenerationDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveTempEntryGeneration
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="transactionNumber"></param>
        /// <param name="isJournalEntry"></param>
        /// <param name="userId"></param>
        public void SaveTempEntryGeneration(LedgerEntry ledgerEntry, LedgerEntryItem ledgerEntryItem, int transactionNumber, bool isJournalEntry, int userId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.TempEntryGeneration tempEntryGenerationEntity = EntityAssembler.CreateTempEntryGeneration(ledgerEntry, ledgerEntryItem, transactionNumber, isJournalEntry, userId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempEntryGenerationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Delete

        /// <summary>
        /// DeleteTempEntryGeneration
        /// </summary>
        /// <param name="tempEntryGenerationId"></param>
        public void DeleteTempEntryGeneration(int tempEntryGenerationId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.TempEntryGeneration.CreatePrimaryKey(tempEntryGenerationId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.TempEntryGeneration tempEntryGenerationEntity = (GENERALLEDGEREN.TempEntryGeneration)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempEntryGenerationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete
    }
}
