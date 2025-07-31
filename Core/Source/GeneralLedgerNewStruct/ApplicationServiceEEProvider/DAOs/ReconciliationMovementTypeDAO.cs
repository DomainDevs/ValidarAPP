//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class ReconciliationMovementTypeDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;


        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public void SaveReconciliationMovementType(ReconciliationMovementType reconciliationMovementType)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.BankReconciliation bankReconciliationEntity = EntityAssembler.CreateReconciliationMovementType(reconciliationMovementType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankReconciliationEntity);

                // Return del model
                ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public void UpdateReconciliationMovementType(ReconciliationMovementType reconciliationMovementType)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.BankReconciliation.CreatePrimaryKey(reconciliationMovementType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.BankReconciliation bankReconciliationEntity = (GENERALLEDGEREN.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankReconciliationEntity.DebitBank = (reconciliationMovementType.AccountingNature == AccountingNatures.Credit);
                bankReconciliationEntity.DebitCompany = (reconciliationMovementType.AccountingNature == AccountingNatures.Debit);
                bankReconciliationEntity.Description = reconciliationMovementType.Description;
                bankReconciliationEntity.ShortDescription = reconciliationMovementType.SmallDescription;

                // Realiza las operaciones con las entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankReconciliationEntity);

                // Return del model
                ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public bool DeleteReconciliationMovementType(ReconciliationMovementType reconciliationMovementType)
        {
            bool isDelete = false;
            try
            {                
                if (ValidateReconciliationMovementTypeById(reconciliationMovementType.Id))
                {
                    // Crea el Primary key con el código de la entidad
                    PrimaryKey primaryKey = GENERALLEDGEREN.BankReconciliation.CreatePrimaryKey(reconciliationMovementType.Id);

                    // Realiza las operaciones con las entities utilizando DAF
                    GENERALLEDGEREN.BankReconciliation bankReconciliationEntity = (GENERALLEDGEREN.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    _dataFacadeManager.GetDataFacade().DeleteObject(bankReconciliationEntity);

                    isDelete = true;
                }
                
            }
            catch (BusinessException ex)
            {                
                throw new BusinessException(ex.Message);
            }

            return isDelete;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetReconciliationMovementTypes: Obtiene tipo de Movimiento de Conciliacion
        /// </summary>        
        /// <returns>List<ReconciliationMovementType></returns>
        public List<ReconciliationMovementType> GetReconciliationMovementTypes()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(GENERALLEDGEREN.BankReconciliation.Properties.BankReconciliationId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                // Se asigna BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.BankReconciliation), criteriaBuilder.GetPredicate()));

                // Return Lista
                return ModelAssembler.CreateReconciliationMovementTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// ValidateReconciliationMovementTypeById
        /// </summary>
        /// <param name="reconciliationMovementTypeId"></param>
        /// <returns></returns>       
        private bool ValidateReconciliationMovementTypeById(int reconciliationMovementTypeId)
        {
            bool isDelete = false;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.BankReconciliationId, reconciliationMovementTypeId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count == 0)
            {
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.BankReconciliationId, reconciliationMovementTypeId);

                businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntryItem), criteriaBuilder.GetPredicate()));                

                if(businessCollection.Count == 0)
                {
                    int rows = 10000;
                    var dailyTransReconciliation = _dataFacadeManager.GetDataFacade().GetView("DailyTransReconciliationView", criteriaBuilder.GetPredicate(), null, 0, rows, null, true, out rows);
                    
                    if (dailyTransReconciliation.Count > 0)
                    {                       
                        isDelete = false;
                    }
                    else
                    {
                        isDelete = true;
                    }
                }
            }
            return isDelete;            
        }

        #endregion

    }
}
