#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class EntryRevertionDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryRevertion
        /// </summary>
        /// <param name="entryRevertionId"></param>
        /// <param name="entrySourceId"></param>
        /// <param name="entryDestinationId"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public int SaveEntryRevertion(int entryRevertionId, int entrySourceId, int entryDestinationId, int userId, DateTime date, bool isJournalEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.EntryRevertion entryRevertionEntity = EntityAssembler.CreateEntryRevertion(entryRevertionId, entrySourceId, entryDestinationId, 
                                                                                                    userId, date, isJournalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entryRevertionEntity);

                return entryRevertionEntity.EntryRevertionId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateEntryRevertion
        /// </summary>
        /// <param name="entryRevertionItems"></param>
        public void UpdateEntryRevertion(Dictionary<string, string> entryRevertionItems)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryRevertion.CreatePrimaryKey(Convert.ToInt32(entryRevertionItems["EntryRevertionId"]));

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.EntryRevertion entryRevertionEntity = (GENERALLEDGEREN.EntryRevertion)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entryRevertionEntity.EntrySourceId = Convert.ToInt32(entryRevertionItems["EntrySourceId"]);
                entryRevertionEntity.EntryDestinationId = Convert.ToInt32(entryRevertionItems["EntryDestinationId"]);
                entryRevertionEntity.UserCode = Convert.ToInt32(entryRevertionItems["UserId"]);
                entryRevertionEntity.Date = Convert.ToDateTime(entryRevertionItems["Date"]);
                entryRevertionEntity.IsJournalEntry = Convert.ToBoolean(entryRevertionItems["IsJournalEntry"]);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entryRevertionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteEntryRevertion
        /// </summary>
        /// <param name="entryRevertionId"></param>
        public void DeleteEntryRevertion(int entryRevertionId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryRevertion.CreatePrimaryKey(entryRevertionId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryRevertion entryRevertionEntity = (GENERALLEDGEREN.EntryRevertion)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(entryRevertionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
