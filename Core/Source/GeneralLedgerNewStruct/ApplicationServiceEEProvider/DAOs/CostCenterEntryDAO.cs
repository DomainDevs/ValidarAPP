//Sistran Core
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class CostCenterEntryDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveCostCenterEntry
        /// </summary>
        /// <param name="costCenter"></param>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        public void SaveCostCenterEntry(GeneralLedgerModels.CostCenter costCenter, int entryItemId, bool isJournalEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.CostCenterEntryItem costCenterEntryItemEntity = EntityAssembler.CreateCostCenterEntryItem(costCenter, entryItemId, isJournalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(costCenterEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save
    }
}
