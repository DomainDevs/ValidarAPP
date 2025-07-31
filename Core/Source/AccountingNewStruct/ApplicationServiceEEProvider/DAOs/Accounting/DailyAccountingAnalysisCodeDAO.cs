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
    public class DailyAccountingAnalysisCodeDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="dailyAccountingAnalysisCode"></param>
        /// <param name="dailyAccountingTransactionItemId"></param>
        /// <returns></returns>
        public int SaveDailyAccountingAnalysisCode(DailyAccountingAnalysisCode dailyAccountingAnalysisCode, int dailyAccountingTransactionItemId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.DailyAccountingAnalysis dailyAccountingAnalysisEntity = EntityAssembler.CreateDailyAccountingAnalysis(dailyAccountingAnalysisCode, dailyAccountingTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(dailyAccountingAnalysisEntity);

                // Return del model
                return dailyAccountingAnalysisEntity.DailyAccountingAnalysisId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="dailyAccountingAnalysisCodeId"></param>
        public void DeleteDailyAccountingAnalysisCode(int dailyAccountingAnalysisCodeId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.DailyAccountingAnalysis.CreatePrimaryKey(dailyAccountingAnalysisCodeId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.DailyAccountingAnalysis dailyAccountingAnalysisEntity = (ACCOUNTINGEN.DailyAccountingAnalysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(dailyAccountingAnalysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAnalysisCode
        /// </summary>
        /// <param name="dailyAccountingAnalysisCodeId"></param>
        /// <returns></returns>
        public DailyAccountingAnalysisCode GetAccountingAnalysisCode(int dailyAccountingAnalysisCodeId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.DailyAccountingAnalysis.CreatePrimaryKey(dailyAccountingAnalysisCodeId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.DailyAccountingAnalysis dailyAccountingAnalysisEntity = (ACCOUNTINGEN.DailyAccountingAnalysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //Se retorna el modelo
                return ModelAssembler.CreateDailyAccountingAnalysisCode(dailyAccountingAnalysisEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
