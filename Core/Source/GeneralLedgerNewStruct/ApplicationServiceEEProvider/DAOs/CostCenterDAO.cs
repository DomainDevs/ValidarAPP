#region Using

//Sistran Core
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

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class CostCenterDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveCostCenter
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns></returns>
        public Models.CostCenter SaveCostCenter(Models.CostCenter costCenter)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.CostCenter costCenterEntity = EntityAssembler.CreateCostCenter(costCenter);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(costCenterEntity);

                // Return del model
                return ModelAssembler.CreateCostCenter(costCenterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        #endregion

        #region Update

        /// <summary>
        /// UpdateCostCenter
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns></returns>
        public Models.CostCenter UpdateCostCenter(Models.CostCenter costCenter)
        {
            try
            {
                // Crea la Primary key con el cóodigo de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.CostCenter.CreatePrimaryKey(costCenter.CostCenterId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.CostCenter costCenterEntity = (GENERALLEDGEREN.CostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                costCenterEntity.Description = costCenter.Description;
                costCenterEntity.CostCenterTypeId = costCenter.CostCenterType.CostCenterTypeId;
                costCenterEntity.IsProrated = costCenter.IsProrated;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(costCenterEntity);

                // Return del model
                return ModelAssembler.CreateCostCenter(costCenterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        #endregion

        #region Delete

        /// <summary>
        /// DeleteCostCenter
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        public bool DeleteCostCenter(int costCenterId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.CostCenter.CreatePrimaryKey(costCenterId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.CostCenter costCenterEntity = (GENERALLEDGEREN.CostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(costCenterEntity);

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
        /// GetCostCenter
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns></returns>
        public Models.CostCenter GetCostCenter(Models.CostCenter costCenter)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.CostCenter.CreatePrimaryKey(costCenter.CostCenterId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.CostCenter costCenterEntity = (GENERALLEDGEREN.CostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCostCenter(costCenterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
      
        #endregion Get

        #region List

        /// <summary>
        /// GetCostCenters
        /// </summary>
        /// <returns></returns>
        public List<Models.CostCenter> GetCostCenters()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.CostCenter)));

                // Return  como Lista
                return ModelAssembler.CreateCostCenters(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCostCenterByDescription
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Models.CostCenter> GetCostCenterByDescription(string description)
        {
            try
            {
                // Criterio de  búsqueda
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals("Description",description);

                // Realizar las operaciones con los entities utilizando DAF
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.CostCenter), criteriaBuilder.GetPredicate()));

                // Return  como Lista
                return ModelAssembler.CreateCostCenters(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion List
    }
}
