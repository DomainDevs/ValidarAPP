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

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class BankReconciliationDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveBankReconciliation
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        /// <returns></returns>
        public ReconciliationMovementType SaveBankReconciliation(ReconciliationMovementType reconciliationMovementType)
        {
            try
            {
                // Convertir de model a entity
                var bankReconciliationEntity = EntityAssembler.CreateReconciliationMovementType(reconciliationMovementType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankReconciliationEntity);

                // Return del model
                return ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateBankReconciliation
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public void UpdateBankReconciliation(ReconciliationMovementType reconciliationMovementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.BankReconciliation.CreatePrimaryKey(reconciliationMovementType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                var bankReconciliationEntity = (GENERALLEDGEREN.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankReconciliationEntity.Description = reconciliationMovementType.Description;

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        } 
        #endregion

        #region Delete

        /// <summary>
        /// DeleteBankReconciliation
        /// </summary>
        /// <param name="bankReconciliationId"></param>
        /// <returns></returns>
        public bool DeleteBankReconciliation(int bankReconciliationId)
        {
            bool isDeleted;

            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.BankReconciliation.CreatePrimaryKey(bankReconciliationId);

                // Realizar las operaciones con los entities utilizando DAF
                var bankReconciliationEntity = (GENERALLEDGEREN.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(bankReconciliationEntity);

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
        /// GetBankReconciliationById
        /// </summary>
        /// <param name="bankReconciliationId"></param>
        /// <returns></returns>
        public ReconciliationMovementType GetBankReconciliationById(int bankReconciliationId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.BankReconciliation.CreatePrimaryKey(bankReconciliationId);

                // Realizar las operaciones con los entities utilizando DAF
                var bankReconciliationEntity = (GENERALLEDGEREN.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Retornar el model
                return ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankReconciliation
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        /// <returns></returns>
        public ReconciliationMovementType GetBankReconciliation(ReconciliationMovementType reconciliationMovementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.BankReconciliation.CreatePrimaryKey(reconciliationMovementType.Id);

                // Realizar las operaciones con los entities utilizando DAF
                var bankReconciliationEntity = (GENERALLEDGEREN.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankReconciliations
        /// </summary>
        /// <returns></returns>
        public List<ReconciliationMovementType> GetBankReconciliations()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.BankReconciliation)));

                // Return como Lista
                return ModelAssembler.CreateReconciliationMovementTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        

        #endregion Get
    }
}