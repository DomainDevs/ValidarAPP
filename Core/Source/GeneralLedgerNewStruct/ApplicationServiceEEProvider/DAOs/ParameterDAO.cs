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
    internal class ParameterDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveParameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Models.AccountingRules.Parameter SaveParameter(Models.AccountingRules.Parameter parameter)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.Parameter parameterEntity = EntityAssembler.CreateParameter(parameter);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(parameterEntity);

                // Return del model
                return ModelAssembler.CreateParameter(parameterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateParameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Models.AccountingRules.Parameter UpdateParameter(Models.AccountingRules.Parameter parameter)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Parameter.CreatePrimaryKey(parameter.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.Parameter parameterEntity = (GENERALLEDGEREN.Parameter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                parameterEntity.ModuleCode = parameter.ModuleDateId;
                parameterEntity.DataTypeCode = Convert.ToInt32(parameter.DataType);
                parameterEntity.Order = parameter.Order;
                parameterEntity.Description = parameter.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(parameterEntity);

                // Return del model
                return ModelAssembler.CreateParameter(parameterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteParameter
        /// </summary>
        /// <param name="parameterId"></param>
        public void DeleteParameter(int parameterId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Parameter.CreatePrimaryKey(parameterId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Parameter parameterEntity = (GENERALLEDGEREN.Parameter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(parameterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetParameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Models.AccountingRules.Parameter GetParameter(Models.AccountingRules.Parameter parameter)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Parameter.CreatePrimaryKey(parameter.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Parameter parameterEntity = (GENERALLEDGEREN.Parameter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateParameter(parameterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetParameters
        /// </summary>
        /// <returns></returns>
        public List<Models.AccountingRules.Parameter> GetParameters()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Parameter)));

                // Return como Lista
                return ModelAssembler.CreateParameters(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}
