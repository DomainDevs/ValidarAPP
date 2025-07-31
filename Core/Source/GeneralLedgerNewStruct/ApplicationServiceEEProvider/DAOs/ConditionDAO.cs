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
    internal class ConditionDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Models.AccountingRules.Condition SaveCondition(Models.AccountingRules.Condition condition)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.Condition conditionEntity = EntityAssembler.CreateCondition(condition);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(conditionEntity);

                // Return del model
                return ModelAssembler.CreateCondition(conditionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Models.AccountingRules.Condition UpdateCondition(Models.AccountingRules.Condition condition)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Condition.CreatePrimaryKey(condition.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.Condition conditionEntity = (GENERALLEDGEREN.Condition)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                int? resultId = null;

                if (condition.IdResult > 0)
                    resultId = condition.IdResult;

                conditionEntity.AccountingRuleId = condition.AccountingRule.Id;
                conditionEntity.ParameterId = condition.Parameter.Id;
                conditionEntity.Operator = condition.Operator;
                conditionEntity.Value = condition.Value;
                conditionEntity.RightConditionId = condition.IdRightCondition;
                conditionEntity.LeftConditionId = condition.IdLeftCondition;
                conditionEntity.ResultId = resultId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(conditionEntity);

                // Return del model
                return ModelAssembler.CreateCondition(conditionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteCondition
        /// </summary>
        /// <param name="conditionId"></param>
        public void DeleteCondition(int conditionId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Condition.CreatePrimaryKey(conditionId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Condition conditionEntity = (GENERALLEDGEREN.Condition)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(conditionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetCondition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Models.AccountingRules.Condition GetCondition(Models.AccountingRules.Condition condition)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Condition.CreatePrimaryKey(condition.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Condition conditionEntity = (GENERALLEDGEREN.Condition)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCondition(conditionEntity);
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

                // Return  como Lista
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
