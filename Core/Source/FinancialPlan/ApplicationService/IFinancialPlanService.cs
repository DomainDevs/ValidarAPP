using Sistran.Core.Application.FinancialPlanServices.DTOs;
using System.ServiceModel;

namespace Sistran.Core.Application.FinancialPlanServices
{
    /// <summary>
    /// Interfaz Plan Financiero
    /// </summary>
    [ServiceContract]
    public interface IFinancialPlanService
    {
        /// <summary>
        /// Creates the financial plan.
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <returns></returns>
        [OperationContract]
        FinancialDTO CreateFinancialPlan(FilterFinancialPlanDTO filterFinancialPlanDTO);

        /// <summary>
        /// Creates the quotas.
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <returns></returns>
        [OperationContract]
        bool CreateQuotas(FilterFinancialPlanDTO filterFinancialPlanDTO);
    }
}
