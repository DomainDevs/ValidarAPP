//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.Utilities.DataFacade;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class TempApplicationAccountingAnalysisDAO
    {
        /// <summary>
        /// SaveTempDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="tempApplicationAccountingAnalysis"></param>
        /// <param name="tempAccountingId"></param>
        /// <returns></returns>
        public int SaveTempApplicationAccountingAnalysis(ApplicationAccountingAnalysis tempApplicationAccountingAnalysis, int tempAccountingId)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationAccountingAnalysis entityTempApplicationAccountingAnalysis = EntityAssembler.CreateTempAccountingAnalysis(tempApplicationAccountingAnalysis, tempAccountingId);

                DataFacadeManager.Insert(entityTempApplicationAccountingAnalysis);

                return entityTempApplicationAccountingAnalysis.TempAppAccountingAnalysisCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="tempAccountingAnalysisId"></param>
        public void DeleteTempApplicationAccountingAnalysis(int tempAccountingAnalysisId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccountingAnalysis.CreatePrimaryKey(tempAccountingAnalysisId);
                DataFacadeManager.Delete(primaryKey);
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
        public ApplicationAccountingAnalysis GetTempAccountingAnalysisCode(int tempDailyAccountingAnalysisId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccountingAnalysis.CreatePrimaryKey(tempDailyAccountingAnalysisId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.TempApplicationAccountingAnalysis entityTempApplicationAccountingAnalysis = 
                    (ACCOUNTINGEN.TempApplicationAccountingAnalysis)DataFacadeManager.GetObject(primaryKey);

                //Se retorna el modelo
                return ModelAssembler.CreateTempDailyAccountingAnalysisCode(entityTempApplicationAccountingAnalysis);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
