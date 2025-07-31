using AutoMapper;
using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Models;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System.Collections.Generic;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Assemblers
{
    public class DtoAssembler
    {
        /// <summary>
        /// Gets the financial plan.
        /// </summary>
        /// <param name="financialPlanDTOs">The financial plan dt os.</param>
        /// <returns></returns>
        public static List<QuotaPlanDTO> GetFinancialPlan(List<FinancialPlanDTO> financialPlanDTOs)
        {
            IMapper iMapper = AutoMapperAssembler.CreateMapFinancialPlan();
            return iMapper.Map<List<FinancialPlanDTO>, List<QuotaPlanDTO>>(financialPlanDTOs);
        }

        /// <summary>
        /// Distribucion Cuotas
        /// </summary>
        /// <param name="financialPlanDTOs">The financial plan dt os.</param>
        /// <returns></returns>
        public static List<FinPayerPaymentModel> CreatePayerPayment(List<QuotaPlanDTO> quotaPlanDTOs)
        {
            IMapper iMapper = AutoMapperAssembler.CreateMapPayerPayment();
            return iMapper.Map<List<QuotaPlanDTO>, List<FinPayerPaymentModel>>(quotaPlanDTOs);
        }
    }
}
