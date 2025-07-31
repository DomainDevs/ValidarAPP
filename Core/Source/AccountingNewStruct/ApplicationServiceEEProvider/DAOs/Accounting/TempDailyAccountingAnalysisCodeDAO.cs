using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempDailyAccountingAnalysisCodeDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveTempDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="tempDailyAccountingAnalysisCode"></param>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns></returns>
        public int SaveTempDailyAccountingAnalysisCode(DailyAccountingAnalysisCode tempDailyAccountingAnalysisCode, int tempDailyAccountingTransactionItemId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempDailyAccountingAnalysis tempDailyAccountingAnalysisEntity = EntityAssembler.CreateTempDailyAccountingAnalysis(tempDailyAccountingAnalysisCode, tempDailyAccountingTransactionItemId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempDailyAccountingAnalysisEntity);

                // Return del model
                return tempDailyAccountingAnalysisEntity.TempDailyAccountingAnalysisId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="tempDailyAccountingAnalysisId"></param>
        public void DeleteTempDailyAccountingAnalysisCode(int tempDailyAccountingAnalysisId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingAnalysis.CreatePrimaryKey(tempDailyAccountingAnalysisId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempDailyAccountingAnalysis tempDailyAccountingAnalysisEntity = (ACCOUNTINGEN.TempDailyAccountingAnalysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempDailyAccountingAnalysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempAccountingAnalysisCode
        /// </summary>
        /// <param name="tempDailyAccountingAnalysisId"></param>
        /// <returns></returns>
        public DailyAccountingAnalysisCode GetTempAccountingAnalysisCode(int tempDailyAccountingAnalysisId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.TempDailyAccountingAnalysis.CreatePrimaryKey(tempDailyAccountingAnalysisId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.TempDailyAccountingAnalysis tempDailyAccountingAnalysisEntity = (ACCOUNTINGEN.TempDailyAccountingAnalysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //Se retorna el modelo
                return ModelAssembler.CreateTempDailyAccountingAnalysisCode(tempDailyAccountingAnalysisEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
