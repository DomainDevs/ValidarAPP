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
    public class DailyAccountingCostCenterDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveDailyAccountingCostCenter
        /// </summary>
        /// <param name="dailyAccountingCostCenter"></param>
        /// <param name="dailyAccountingTransactionItemId"></param>
        /// <returns></returns>
        public int SaveDailyAccountingCostCenter(DailyAccountingCostCenter dailyAccountingCostCenter, int dailyAccountingTransactionItemId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.DailyAccountingCostCenter dailyAccountingCostCenterEntity = EntityAssembler.CreateDailyAccountingCostCenter(dailyAccountingCostCenter, dailyAccountingTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(dailyAccountingCostCenterEntity);

                // Return del model
                return dailyAccountingCostCenterEntity.DailyAccountingCostCenterId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteDailyAccountingCostCenter
        /// </summary>
        /// <param name="dailyAccountingCostCenterId"></param>
        public void DeleteDailyAccountingCostCenter(int dailyAccountingCostCenterId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.DailyAccountingCostCenter.CreatePrimaryKey(dailyAccountingCostCenterId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.DailyAccountingCostCenter dailyAccountingCostCenterEntity = (ACCOUNTINGEN.DailyAccountingCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(dailyAccountingCostCenterEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetDailyAccountingCostCenter
        /// </summary>
        /// <param name="dailyAccountingCostCenterId"></param>
        /// <returns></returns>
        public DailyAccountingCostCenter GetDailyAccountingCostCenter(int dailyAccountingCostCenterId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.DailyAccountingCostCenter.CreatePrimaryKey(dailyAccountingCostCenterId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.DailyAccountingCostCenter dailyAccountingCostCenterEntity = (ACCOUNTINGEN.DailyAccountingCostCenter)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //Se retorna el modelo
                return ModelAssembler.CreateDailyAccountingCostCenter(dailyAccountingCostCenterEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
