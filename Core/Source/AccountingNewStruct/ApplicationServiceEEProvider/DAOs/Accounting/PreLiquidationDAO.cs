//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs

{
    public class PreLiquidationDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region PreLiquidation

        /// <summary>
        /// SavePreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidation SavePreLiquidation(PreLiquidation preLiquidation)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.Preliquidation preliquidation = EntityAssembler.CreatePreliquidation(preLiquidation);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(preliquidation);

                // Return del model
                return ModelAssembler.CreatePreLiquidation(preliquidation);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidation UpdatePreLiquidation(PreLiquidation preLiquidation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Preliquidation.CreatePrimaryKey(preLiquidation.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Preliquidation preliquidationEntity = (ACCOUNTINGEN.Preliquidation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                preliquidationEntity.BranchCode = preLiquidation.Branch.Id;
                preliquidationEntity.SalesPointCode = preLiquidation.SalePoint.Id;
                preliquidationEntity.CompanyCode = preLiquidation.Company.IndividualId;
                preliquidationEntity.IndividualId = preLiquidation.Payer.IndividualId;
                preliquidationEntity.Description = preLiquidation.Description;
                preliquidationEntity.Status = preLiquidation.Status;
                preliquidationEntity.RegisterDate = preLiquidation.RegisterDate.Date;
                preliquidationEntity.PersonTypeCode = preLiquidation.PersonType.Id;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(preliquidationEntity);

                // Return del model
                return ModelAssembler.CreatePreLiquidation(preliquidationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        /// <summary>
        /// GetPreLiquidation
        /// </summary>
        /// <param name="preLiquidationId"></param>
        /// <returns>PreLiquidation</returns>
        public PreLiquidation GetPreLiquidation(int preLiquidationId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Preliquidation.CreatePrimaryKey(preLiquidationId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Preliquidation preLiquidationEntity = (ACCOUNTINGEN.Preliquidation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreatePreLiquidation(preLiquidationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
