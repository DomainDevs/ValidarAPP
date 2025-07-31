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

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class ResultDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Models.AccountingRules.Result SaveResult(Models.AccountingRules.Result result)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.Result resultEntity = EntityAssembler.CreateResult(result);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(resultEntity);

                // Return del model
                return ModelAssembler.CreateResult(resultEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Models.AccountingRules.Result UpdateResult(Models.AccountingRules.Result result)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Result.CreatePrimaryKey(result.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.Result resultEntity = (GENERALLEDGEREN.Result)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                resultEntity.AccountingNatureId = Convert.ToInt32(result.AccountingNature);
                resultEntity.AccountingAccountNumber = result.AccountingAccount;
                resultEntity.ParameterId = result.Parameter.Id;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(resultEntity);

                // Return del model
                return ModelAssembler.CreateResult(resultEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteResult
        /// </summary>
        /// <param name="resultId"></param>
        public void DeleteResult(int resultId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Result.CreatePrimaryKey(resultId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Result resultEntity = (GENERALLEDGEREN.Result)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(resultEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Models.AccountingRules.Result GetResult(Models.AccountingRules.Result result)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Result.CreatePrimaryKey(result.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Result resultEntity = (GENERALLEDGEREN.Result)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateResult(resultEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetConditions
        /// </summary>
        /// <returns></returns>
        public List<Models.AccountingRules.Condition> GetConditions()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Condition)));

                // Return como Lista
                return ModelAssembler.CreateConditions(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}
