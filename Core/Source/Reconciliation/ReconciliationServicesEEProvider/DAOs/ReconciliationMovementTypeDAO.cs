//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReconciliationServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReconciliationServices.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReconciliationServices.EEProvider.DAOs
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
                Entities.BankReconciliation bankReconciliationEntity = EntityAssembler.CreateBankReconciliation(reconciliationMovementType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankReconciliationEntity);

                // Return del model
                ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (Exception ex)
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
                PrimaryKey primaryKey = Entities.BankReconciliation.CreatePrimaryKey(reconciliationMovementType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.BankReconciliation bankReconciliationEntity = (Entities.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankReconciliationEntity.DebitBank = reconciliationMovementType.AccountingNature == AccountingNatures.Credit ? true : false;
                bankReconciliationEntity.DebitCompany = reconciliationMovementType.AccountingNature == AccountingNatures.Debit ? true : false;
                bankReconciliationEntity.Description = reconciliationMovementType.Description;
                bankReconciliationEntity.ShortDescription = reconciliationMovementType.SmallDescription;

                // Realiza las operaciones con las entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankReconciliationEntity);

                // Return del model
                ModelAssembler.CreateReconciliationMovementType(bankReconciliationEntity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        public void DeleteReconciliationMovementType(ReconciliationMovementType reconciliationMovementType)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankReconciliation.CreatePrimaryKey(reconciliationMovementType.Id);

                // Realiza las operaciones con las entities utilizando DAF
                Entities.BankReconciliation bankReconciliationEntity = (Entities.BankReconciliation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(bankReconciliationEntity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

                criteriaBuilder.Property(Entities.BankReconciliation.Properties.BankReconciliationId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                // Se asigna BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.BankReconciliation), criteriaBuilder.GetPredicate()));

                // Return Lista
                return ModelAssembler.CreateReconciliationMovementTypes(businessCollection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        #endregion

    }
}
