#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
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
    internal class EntryTypeAccountingDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryTypeAccounting
        /// </summary>
        /// <param name="entryTypeItem"></param>
        /// <param name="entryTypeId"></param>
        /// <returns></returns>
        public GeneralLedgerModels.EntryTypeItem SaveEntryTypeAccounting(GeneralLedgerModels.EntryTypeItem entryTypeItem, int entryTypeId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.EntryTypeItem entryTypeAccountingEntity = EntityAssembler.CreateEntryTypeItem(entryTypeItem, entryTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entryTypeAccountingEntity);

                return ModelAssembler.CreateEntryTypeItem(entryTypeAccountingEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateEntryTypeAccounting
        /// </summary>
        /// <param name="entryTypeItem"></param>
        /// <param name="entryTypeId"></param>
        /// <returns></returns>
        public GeneralLedgerModels.EntryTypeItem UpdateEntryTypeAccounting(GeneralLedgerModels.EntryTypeItem entryTypeItem, int entryTypeId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryTypeItem.CreatePrimaryKey(Convert.ToInt32(entryTypeItem.Id));

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.EntryTypeItem entryTypeAccountingEntity = (GENERALLEDGEREN.EntryTypeItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entryTypeAccountingEntity.AccountingMovementTypeId = entryTypeItem.AccountingMovementType.AccountingMovementTypeId;
                entryTypeAccountingEntity.AccountingNature = Convert.ToInt32(entryTypeItem.AccountingNature);
                entryTypeAccountingEntity.AnalysisId = entryTypeItem.Analysis.AnalysisId;
                entryTypeAccountingEntity.CostCenterId = entryTypeItem.CostCenter.CostCenterId;
                entryTypeAccountingEntity.CurrencyCode = entryTypeItem.Currency.Id;
                entryTypeAccountingEntity.AccountingAccountId = entryTypeItem.AccountingAccount.AccountingAccountId;
                entryTypeAccountingEntity.AccountingConceptCode = entryTypeItem.AccountingConcept.Id;
                entryTypeAccountingEntity.EntryTypeId = entryTypeId;
                entryTypeAccountingEntity.Description = entryTypeItem.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entryTypeAccountingEntity);

                // Return del model
                return ModelAssembler.CreateEntryTypeItem(entryTypeAccountingEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteEntryTypeAccounting
        /// </summary>
        /// <param name="entryTypeAccountingId"></param>
        /// <returns></returns>
        public bool DeleteEntryTypeAccounting(int entryTypeAccountingId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryTypeItem.CreatePrimaryKey(entryTypeAccountingId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryTypeItem entryTypeAccountingEntity = (GENERALLEDGEREN.EntryTypeItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(entryTypeAccountingEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetEntryTypeItem
        /// </summary>
        /// <param name="entryTypeAccounting"></param>
        /// <returns></returns>
        public GeneralLedgerModels.EntryTypeItem GetEntryTypeItem(GeneralLedgerModels.EntryTypeItem entryTypeItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryTypeItem.CreatePrimaryKey(entryTypeItem.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryTypeItem entryTypeItemEntity = (GENERALLEDGEREN.EntryTypeItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateEntryTypeItem(entryTypeItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryTypeAccountings
        /// </summary>
        /// <returns></returns>
        public List<GeneralLedgerModels.EntryTypeItem> GetEntryTypeItems()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryTypeItem)));

                // Return como Lista
                return ModelAssembler.CreateEntryTypeItems(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}

