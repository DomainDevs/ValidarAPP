//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempPreLiquidationDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region TempPreLiquidation

        /// <summary>
        /// SaveTempPreLiquidation
        /// Graba las preliquidaciones en Temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidation SaveTempPreLiquidation(PreLiquidation preLiquidation)
        {
            // Convertir de model a entity
            ACCOUNTINGEN.TempPreliquidation tempPreliquidationEntity = EntityAssembler.CreateTempPreliquidation(preLiquidation);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(tempPreliquidationEntity);

            // Return del model
            return ModelAssembler.CreateTempPreLiquidation(tempPreliquidationEntity);
        }

        /// <summary>
        /// UpdateTempPreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidation UpdateTempPreLiquidation(PreLiquidation preLiquidation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPreliquidation.CreatePrimaryKey(preLiquidation.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempPreliquidation preliquidationEntity = (ACCOUNTINGEN.TempPreliquidation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                preliquidationEntity.RegisterDate = preLiquidation.RegisterDate;
                preliquidationEntity.BranchCode = preLiquidation.Branch.Id;
                preliquidationEntity.Description = preLiquidation.Description;
                preliquidationEntity.CompanyCode = preLiquidation.Company.IndividualId;
                preliquidationEntity.IndividualId = preLiquidation.Payer.IndividualId;
                preliquidationEntity.PersonTypeCode = preLiquidation.PersonType.Id;
                preliquidationEntity.SalesPointCode = preLiquidation.SalePoint.Id;
                preliquidationEntity.Status = preLiquidation.Status;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(preliquidationEntity);

                // Return del model
                return ModelAssembler.CreateTempPreLiquidation(preliquidationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPreLiquidation(PreLiquidation preLiquidation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPreliquidation.CreatePrimaryKey(preLiquidation.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempPreliquidation preliquidationEntity = (ACCOUNTINGEN.TempPreliquidation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (preliquidationEntity != null)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(preliquidationEntity);

                    return true;
                }

                return false;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempPreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidation GetTempPreLiquidation(PreLiquidation preLiquidation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempPreliquidation.CreatePrimaryKey(preLiquidation.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempPreliquidation tempPreLiquidationEntity = (ACCOUNTINGEN.TempPreliquidation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateTempPreLiquidation(tempPreLiquidationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
