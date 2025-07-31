using System;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempDailyAccountingCostCenterDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="tempDailyAccountingCostCenter"></param>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns></returns>
        public int SaveTempDailyAccountingCostCenter(DailyAccountingCostCenter tempDailyAccountingCostCenter, int tempDailyAccountingTransactionItemId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenterEntity = EntityAssembler.CreateTempDailyAccountingCostCenter(tempDailyAccountingCostCenter, tempDailyAccountingTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempDailyAccountingCostCenterEntity);

                // Return del model
                return tempDailyAccountingCostCenterEntity.TempDailyAccountingCostCenterId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="tempDailyAccountingCostCenterId"></param>
        public void DeleteTempDailyAccountingCostCenter(int tempDailyAccountingCostCenterId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingCostCenter.CreatePrimaryKey(tempDailyAccountingCostCenterId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenterEntity = (ACCOUNTINGEN.TempDailyAccountingCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempDailyAccountingCostCenterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempAccountingAnalysisCode
        /// </summary>
        /// <param name="tempDailyAccountingCostCenterId"></param>
        /// <returns></returns>
        public DailyAccountingCostCenter GetTempDailyAccountingCostCenter(int tempDailyAccountingCostCenterId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingCostCenter.CreatePrimaryKey(tempDailyAccountingCostCenterId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.TempDailyAccountingCostCenter tempDailyAccountingCostCenterEntity = (ACCOUNTINGEN.TempDailyAccountingCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //Se retorna el modelo
                return ModelAssembler.CreateTempDailyAccountingCostCenter(tempDailyAccountingCostCenterEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
