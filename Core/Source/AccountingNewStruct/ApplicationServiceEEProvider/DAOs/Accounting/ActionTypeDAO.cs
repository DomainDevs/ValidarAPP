//Sitran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class ActionTypeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        ///<summary>
        /// SaveActionType
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns>ActionType</returns>
        public ActionType SaveActionType(ActionType actionType)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ActionType actionTypeEntity = EntityAssembler.CreateActionType(actionType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(actionTypeEntity);

                // Return del model
                return ModelAssembler.CreateActionType(actionTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateActionType
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns>ActionType</returns>
        public ActionType UpdateActionType(ActionType actionType)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ActionType.CreatePrimaryKey(actionType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.ActionType actionTypeEntity = (ACCOUNTINGEN.ActionType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionTypeEntity.Description = actionType.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionTypeEntity);

                // Return del model
                return ModelAssembler.CreateActionType(actionTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteActionType
        /// </summary>
        /// <param name="actionType"></param>
        public void DeleteActionType(ActionType actionType)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ActionType.CreatePrimaryKey(actionType.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.ActionType actionTypeEntity = (ACCOUNTINGEN.ActionType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(actionTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetActionTypes
        /// </summary>
        /// <returns>List<ActionType/></returns>
        public List<ActionType> GetActionTypes()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.ActionType.Properties.ActionTypeCode);
                criteriaBuilder.Greater();
                criteriaBuilder.Constant(0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.ActionType), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                List<ActionType> actionTypes = new List<ActionType>();

                foreach (ACCOUNTINGEN.ActionType actionTypeEntity in businessCollection.OfType<ACCOUNTINGEN.ActionType>())
                {
                    actionTypes.Add(new ActionType
                    {
                        Id = actionTypeEntity.ActionTypeCode,
                        Description = actionTypeEntity.Description
                    });
                }

                return actionTypes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
