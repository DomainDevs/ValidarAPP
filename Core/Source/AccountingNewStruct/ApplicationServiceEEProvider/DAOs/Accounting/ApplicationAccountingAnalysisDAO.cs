//Sistran Core
using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.Utilities.DataFacade;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Data;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class ApplicationAccountingAnalysisDAO
    {
        /// <summary>
        /// SaveDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="applicationAccountingAnalysis"></param>
        /// <param name="appAccountingId"></param>
        /// <returns></returns>
        public int SaveAccountingAnalysisCode(ApplicationAccountingAnalysis applicationAccountingAnalysis, int appAccountingId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationAccountingAnalysis entityApplicationAccountingAnalysis = EntityAssembler.CreateApplicationAccountingAnalysis(applicationAccountingAnalysis, appAccountingId);

                DataFacadeManager.Insert(entityApplicationAccountingAnalysis);

                // Return del model
                return entityApplicationAccountingAnalysis.AppAccountingAnalysisCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteDailyAccountingAnalysisCode
        /// </summary>
        /// <param name="appAccountingId"></param>
        public void DeleteAccountingAnalysisCode(int appAccountingId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationAccountingAnalysis.CreatePrimaryKey(appAccountingId);
                DataFacadeManager.Delete(primaryKey);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingAnalysisCode
        /// </summary>
        /// <param name="appAccountingId"></param>
        /// <returns></returns>
        public ApplicationAccountingAnalysis GetAccountingAnalysisCode(int appAccountingId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationAccountingAnalysis.CreatePrimaryKey(appAccountingId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.ApplicationAccountingAnalysis entityApplicationAccountingAnalysis = (ACCOUNTINGEN.ApplicationAccountingAnalysis)
                    DataFacadeManager.GetObject(primaryKey);

                //Se retorna el modelo
                return ModelAssembler.CreateApplicationAccountingAnalysis(entityApplicationAccountingAnalysis);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        public int CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(int AnalysisConceptKeyId, string KeyValue)
        {
            var parameters = new NameValue[2];
            parameters[0] = new NameValue("ANALYSIS_CONCEPT_KEY_ID", AnalysisConceptKeyId);
            parameters[1] = new NameValue("COLUMN_VALUE", KeyValue);
            int result = -4;
            using (var dataAccess = new DynamicDataAccess())
            {
                result = Convert.ToInt32(dataAccess.ExecuteSPScalar("ACC.CHECKOUT_ANALYSIS_CODE", parameters));
            }
            return  result;

        }
    }
}
