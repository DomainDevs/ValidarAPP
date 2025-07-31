#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class EntryTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryType
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns></returns>
        public EntryType SaveEntryType(EntryType entryType)
        {
            try
            {
                if (entryType.EntryTypeItems != null)
                {
                    // Convertir de model a entity
                    var entryTypeItemEntity = EntityAssembler.CreateEntryTypeItem(entryType.EntryTypeItems[0], entryType.EntryTypeId);

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(entryTypeItemEntity);

                    return entryType;
                }
                else
                {
                    // Convertir de model a entity
                    var entryTypeEntity = EntityAssembler.CreateEntryType(entryType);

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(entryTypeEntity);

                    // Return del model
                    return ModelAssembler.CreateEntryType(entryTypeEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateEntryType
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns></returns>
        public EntryType UpdateEntryType(EntryType entryType)
        {
            try
            {
                if (entryType.EntryTypeItems != null)
                {
                    var entryTypeItem = entryType.EntryTypeItems[0];

                    // Crea la Primary key con el código de la entidad
                    var primaryKey = GENERALLEDGEREN.EntryTypeItem.CreatePrimaryKey(entryTypeItem.Id);

                    // Encuentra el objeto en referencia a la llave primaria
                    var entryTypeItemEntity = (GENERALLEDGEREN.EntryTypeItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    entryTypeItemEntity.AccountingAccountId = entryTypeItem.AccountingAccount.AccountingAccountId;
                    entryTypeItemEntity.AccountingConceptCode = entryTypeItem.AccountingConcept.Id;
                    entryTypeItemEntity.AccountingMovementTypeId = entryTypeItem.AccountingMovementType.AccountingMovementTypeId;
                    entryTypeItemEntity.AccountingNature = Convert.ToInt32(entryTypeItem.AccountingNature);
                    entryTypeItemEntity.AnalysisId = entryTypeItem.Analysis.AnalysisId;
                    entryTypeItemEntity.CostCenterId = entryTypeItem.CostCenter.CostCenterId;
                    entryTypeItemEntity.CurrencyCode = entryTypeItem.Currency.Id;
                    entryTypeItemEntity.Description = entryTypeItem.Description;
                    entryTypeItemEntity.EntryTypeId = entryType.EntryTypeId;

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().UpdateObject(entryTypeItemEntity);

                    return entryType;
                }
                else
                {
                    // Crea la Primary key con el código de la entidad
                    var primaryKey = GENERALLEDGEREN.EntryType.CreatePrimaryKey(entryType.EntryTypeId);

                    // Encuentra el objeto en referencia a la llave primaria
                    var entryTypeEntity = (GENERALLEDGEREN.EntryType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    entryTypeEntity.Description = entryType.Description;
                    entryTypeEntity.SmallDescription = entryType.SmallDescription;

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().UpdateObject(entryTypeEntity);

                    return ModelAssembler.CreateEntryType(entryTypeEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteEntryType
        /// </summary>
        /// <param name="entryType"></param>
        public void DeleteEntryType(EntryType entryType)
        {
            try
            {
                if (entryType.EntryTypeItems != null)
                {
                    // Crea la Primary key con el código de la entidad
                    PrimaryKey primaryKey = GENERALLEDGEREN.EntryTypeItem.CreatePrimaryKey(entryType.EntryTypeItems[0].Id);

                    // Realizar las operaciones con los entities utilizando DAF
                    GENERALLEDGEREN.EntryTypeItem entryTypeItemEntity = (GENERALLEDGEREN.EntryTypeItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(entryTypeItemEntity);
                }
                else
                {
                    // Crea la Primary key con el código de la entidad
                    var primaryKey = GENERALLEDGEREN.EntryType.CreatePrimaryKey(entryType.EntryTypeId);

                    // Realizar las operaciones con los entities utilizando DAF
                    var entryTypeEntity = (GENERALLEDGEREN.EntryType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(entryTypeEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteEntryTypeAccounting
        /// </summary>
        /// <param name="entryTypeAccountingId"></param>
        /// <returns></returns>
        public bool DeleteEntryTypeItem(int entryTypeItemId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryTypeItem.CreatePrimaryKey(entryTypeItemId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryTypeItem entryTypeItemEntity = (GENERALLEDGEREN.EntryTypeItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(entryTypeItemEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetEntryType
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns></returns>
        public EntryType GetEntryType(EntryType entryType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.EntryType.CreatePrimaryKey(entryType.EntryTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                var entryTypeEntity = (GENERALLEDGEREN.EntryType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateEntryType(entryTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryTypes
        /// </summary>
        /// <returns></returns>
        public List<EntryType> GetEntryTypes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryType)));

                // Return como Lista
                return ModelAssembler.CreateEntryTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryTypeItem
        /// </summary>
        /// <param name="entryTypeAccounting"></param>
        /// <returns></returns>
        public EntryTypeItem GetEntryTypeItem(EntryTypeItem entryTypeItem)
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
        public List<EntryTypeItem> GetEntryTypeItems()
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

        #endregion Get
    }
}

