using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider
{
    public class FinancialPlanServiceEEProvider : IFinancialPlanService
    {
        /// <summary>
        /// Crear nuevo plan Financiero
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public FinancialDTO CreateFinancialPlan(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            try
            {
                return FinancialPlanBussines.CreateFinancialPlan(filterFinancialPlanDTO);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Creates the quotas.
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public bool CreateQuotas(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            try
            {
                if (FinancialPlanBussines.CreateQuotas(filterFinancialPlanDTO))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

    }
}

