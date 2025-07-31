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
    public class CostCenterTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveCostCenterType
        /// </summary>
        /// <param name="costCenterType"></param>
        /// <returns></returns>
        public Models.CostCenterType SaveCostCenterType(CostCenterType costCenterType)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.CostCenterType costCenterTypeEntity = EntityAssembler.CreateCostCenterType(costCenterType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(costCenterTypeEntity);

                // Return del model
                return ModelAssembler.CreateCostCenterType(costCenterTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// UpdateCostCenterType
        /// </summary>
        /// <param name="costCenterType"></param>
        /// <returns></returns>
        public CostCenterType UpdateCostCenterType(CostCenterType costCenterType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.CostCenterType.CreatePrimaryKey(costCenterType.CostCenterTypeId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.CostCenterType costCenterTypeEntity = (GENERALLEDGEREN.CostCenterType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                costCenterTypeEntity.Description = costCenterType.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(costCenterTypeEntity);

                // Return del model
                return ModelAssembler.CreateCostCenterType(costCenterTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteCostCenterType
        /// </summary>
        /// <param name="costCenterTypeId"></param>
        /// <returns></returns>
        public bool DeleteCostCenterType(int costCenterTypeId)
        {
            bool isDeleted;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.CostCenterType.CreatePrimaryKey(costCenterTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.CostCenterType costCenterTypeEntity = (GENERALLEDGEREN.CostCenterType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(costCenterTypeEntity);
                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;

        }

        #endregion

        #region Get

        /// <summary>
        /// GetCostCenterType
        /// </summary>
        /// <param name="costCenterType"></param>
        /// <returns></returns>
        public Models.CostCenterType GetCostCenterType(CostCenterType costCenterType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.CostCenterType.CreatePrimaryKey(costCenterType.CostCenterTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.CostCenterType costCenterTypeEntity = (GENERALLEDGEREN.CostCenterType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCostCenterType(costCenterTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCostCenterTypeById
        /// </summary>
        /// <param name="costCenterTypeId"></param>
        /// <returns></returns>
        public CostCenterType GetCostCenterTypeById(int costCenterTypeId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.CostCenterType.CreatePrimaryKey(costCenterTypeId);

                // Realizar las operaciones con los entities utilizando DAF
                var costCenterTypeEntity = (GENERALLEDGEREN.CostCenterType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Retornar el model
                return ModelAssembler.CreateCostCenterType(costCenterTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCostCenterTypes
        /// </summary>
        /// <returns></returns>
        public List<CostCenterType> GetCostCenterTypes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.CostCenterType)));

                // Return como Lista
                return ModelAssembler.CreateCostCenterTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
