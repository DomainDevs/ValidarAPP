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
    public class TempApplicationAccountingCostCenterDAO
    {
        /// <summary>
        /// SaveTempAccountingCostCenter
        /// </summary>
        /// <param name="tempApplicationAccountingCostCenter"></param>
        /// <param name="tempAppAccountingId"></param>
        /// <returns></returns>
        public int SaveTempAccountingCostCenter(ApplicationAccountingCostCenter tempApplicationAccountingCostCenter, int tempAppAccountingId)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationAccountingCostCenter entityTempAppAccountingCostCenter = EntityAssembler.CreateTempAccountingCostCenter(tempApplicationAccountingCostCenter, tempAppAccountingId);
                DataFacadeManager.Insert(entityTempAppAccountingCostCenter);
                return entityTempAppAccountingCostCenter.TempAppAccountingCostCenterCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempDailyAccountingCostCenter
        /// </summary>
        /// <param name="tempAccountingCostCenterId"></param>
        public void DeleteTempAccountingCostCenter(int tempAccountingCostCenterId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccountingCostCenter.CreatePrimaryKey(tempAccountingCostCenterId);
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
        /// <param name="tempAccountingCostCenterId"></param>
        /// <returns></returns>
        public ApplicationAccountingCostCenter GetTempAccountingCostCenter(int tempAccountingCostCenterId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationAccountingCostCenter.CreatePrimaryKey(tempAccountingCostCenterId);
                ACCOUNTINGEN.TempApplicationAccountingCostCenter entityTempAppAccountingCostCenter = (ACCOUNTINGEN.TempApplicationAccountingCostCenter)
                    DataFacadeManager.GetObject(primaryKey);
                return ModelAssembler.CreateTempAccountingCostCenter(entityTempAppAccountingCostCenter);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
