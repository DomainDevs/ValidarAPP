using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Framework.DAF;
using UWMO = Sistran.Core.Application.UnderwritingServices.Models;
using UWPR = Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class FinancialPlanDAO
    {
        /// <summary>
        /// Ontiene el plan financiero por id
        /// </summary>
        /// <param name="financialPlanId"></param>
        /// <returns></returns>
        public UWMO.FinancialPlan GetFinancialPlanById(int financialPlanId)
        {
            PrimaryKey key = FinancialPlan.CreatePrimaryKey(financialPlanId);
            FinancialPlan financialPlanEntity = (FinancialPlan)Core.Application.Utilities.DataFacade.DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return UWPR.ModelAssembler.CreateFinancialPlan(financialPlanEntity);
        }
    }
}
