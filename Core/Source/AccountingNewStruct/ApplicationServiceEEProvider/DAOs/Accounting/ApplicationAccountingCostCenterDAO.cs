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
    public class ApplicationAccountingCostCenterDAO
    {
        /// <summary>
        /// SaveApplicationAccountingCostCenter
        /// </summary>
        /// <param name="appAccountingCostCenter"></param>
        /// <param name="appAccountingId"></param>
        /// <returns></returns>
        public int SaveApplicationAccountingCostCenter(ApplicationAccountingCostCenter appAccountingCostCenter, int appAccountingId)
        {
            try
            {
                ACCOUNTINGEN.ApplicationAccountingCostCenter entityApplicationAccountingCostCenter = EntityAssembler.CreateApplicationAccountingCostCenter(appAccountingCostCenter, appAccountingId);

                DataFacadeManager.Insert(entityApplicationAccountingCostCenter);

                // Return del model
                return entityApplicationAccountingCostCenter.AppAccountingCostCenterCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteApplicationAccountingCostCenter
        /// </summary>
        /// <param name="appAccountingCostCenterId"></param>
        public void DeleteApplicationAccountingCostCenter(int appAccountingCostCenterId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationAccountingCostCenter.CreatePrimaryKey(appAccountingCostCenterId);
                DataFacadeManager.Delete(primaryKey);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetApplicationAccountingCostCenter
        /// </summary>
        /// <param name="appAccountingCostCenterId"></param>
        /// <returns></returns>
        public ApplicationAccountingCostCenter GetApplicationAccountingCostCenter(int appAccountingCostCenterId)
        {
            try
            {
                //Se crea la clave primaria con el código de la entidad.
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationAccountingCostCenter.CreatePrimaryKey(appAccountingCostCenterId);

                //Se obtiene el objeto mediante la llave primaria
                ACCOUNTINGEN.ApplicationAccountingCostCenter entityApplicationAccountingCostCenter = 
                    (ACCOUNTINGEN.ApplicationAccountingCostCenter)DataFacadeManager.GetObject(primaryKey);

                //Se retorna el modelo
                return ModelAssembler.CreateApplicationAccountingCostCenter(entityApplicationAccountingCostCenter);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
