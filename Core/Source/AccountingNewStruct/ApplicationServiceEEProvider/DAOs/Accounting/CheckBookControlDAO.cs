//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System.Collections.Generic;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class CheckBookControlDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveCheckBookControl
        /// </summary>
        /// <param name="checkBookControl"></param>
        /// <returns>CheckBookControl</returns>
        public CheckBookControl SaveCheckBookControl(CheckBookControl checkBookControl)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CheckbookControl checkbookControlEntity = EntityAssembler.CreateCheckBookControl(checkBookControl);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(checkbookControlEntity);

                // Return del model
                return ModelAssembler.CreateCheckBookControl(checkbookControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCheckBookControl
        /// </summary>
        /// <param name="checkBookControl"></param>
        /// <returns>CheckBookControl</returns>
        public CheckBookControl UpdateCheckBookControl(CheckBookControl checkBookControl)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CheckbookControl.CreatePrimaryKey(checkBookControl.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CheckbookControl checkBookControlEntity = (ACCOUNTINGEN.CheckbookControl)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                checkBookControlEntity.Status = checkBookControl.Status;
                checkBookControlEntity.DisabledDate = checkBookControl.DisabledDate;
                checkBookControlEntity.LastCheck = checkBookControl.LastCheck;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(checkBookControlEntity);

                // Return del model
                return ModelAssembler.CreateCheckBookControl(checkBookControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckBookControl
        /// </summary>
        /// <param name="checkBookControl"></param>
        /// <returns>CheckBookControl</returns>
        public CheckBookControl GetCheckBookControl(CheckBookControl checkBookControl)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CheckbookControl.CreatePrimaryKey(checkBookControl.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CheckbookControl checkbookControlEntity = (ACCOUNTINGEN.CheckbookControl)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCheckBookControl(checkbookControlEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckBookControlActiveByAccountBankId
        /// bcardenas
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="isAutomatic"></param>
        /// <returns>List<CheckBookControl></returns>
        public List<CheckBookControl> GetCheckBookControlActiveByAccountBankId(int accountBankId, int isAutomatic)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CheckbookControl.Properties.AccountBankCode, accountBankId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CheckbookControl.Properties.Status, 1);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CheckbookControl.Properties.IsAutomatic, isAutomatic);

                //Asignamos BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CheckbookControl), criteriaBuilder.GetPredicate()));

                // Return como lista
                return ModelAssembler.CreateCheckBookControls(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
