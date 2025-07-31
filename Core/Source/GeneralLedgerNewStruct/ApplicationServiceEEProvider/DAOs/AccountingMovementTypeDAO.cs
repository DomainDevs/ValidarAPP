#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    class AccountingMovementTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAccountingMovementType
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns></returns>
        public Models.AccountingMovementType SaveAccountingMovementType(Models.AccountingMovementType accountingMovementType)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AccountingMovementType accountingMovementTypeEntity = EntityAssembler.CreateAccountingMovementType(accountingMovementType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingMovementTypeEntity);

                return ModelAssembler.CreateAccountingMovementType(accountingMovementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAccountingMovementType
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns></returns>
        public Models.AccountingMovementType UpdateAccountingMovementType(Models.AccountingMovementType accountingMovementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingMovementType.CreatePrimaryKey(accountingMovementType.AccountingMovementTypeId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AccountingMovementType acccountingMovementTypeEntity = (GENERALLEDGEREN.AccountingMovementType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                acccountingMovementTypeEntity.Description = accountingMovementType.Description;
                acccountingMovementTypeEntity.IsAutomatic = accountingMovementType.IsAutomatic;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(acccountingMovementTypeEntity);

                return ModelAssembler.CreateAccountingMovementType(acccountingMovementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAccountingMovementType
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns></returns>
        public void DeleteAccountingMovementType(Models.AccountingMovementType accountingMovementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingMovementType.CreatePrimaryKey(accountingMovementType.AccountingMovementTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingMovementType acccountingMovementTypeEntity = (GENERALLEDGEREN.AccountingMovementType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(acccountingMovementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetAccountingMovementType
        /// </summary>
        /// <returns>AccountingMovementType</returns>
        public Models.AccountingMovementType GetAccountingMovementType(Models.AccountingMovementType accountingMovementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AccountingMovementType.CreatePrimaryKey(accountingMovementType.AccountingMovementTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AccountingMovementType accountingMovementTypeEntity = (GENERALLEDGEREN.AccountingMovementType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAccountingMovementType(accountingMovementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingMovementTypes
        /// </summary>
        /// <returns></returns>
        public List<Models.AccountingMovementType> GetAccountingMovementTypes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingMovementType)));

                // Return como Lista
                return ModelAssembler.CreateAccountingMovementTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
