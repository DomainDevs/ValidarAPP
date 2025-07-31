using Sistran.Core.Integration.CommonServices.DTOs;
using Sistran.Core.Integration.CommonServices.EEProvider.Assemblers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Integration.CommonServices.EEProvider
{
    public class CommonIntegrationServiceEEProvider : ICommonIntegrationServiceCore
    {
        public List<LineBusinessDTO> GetLinesBusiness()
        {
            return DelegateService.commonService.GetLinesBusiness().ToDTOs().ToList();
        }

        public List<SubLineBusinessDTO> GetSubLineBusinessByLineBusinessId()
        {
            return DelegateService.commonService.GetSubLineBusinessByLineBusinessId().ToDTOs().ToList();
        }
        public ParameterDTO GetParameterByParameterId(int parameterId)
        {
            return DelegateService.commonService.GetParameterByParameterId(parameterId).ToDTO();
        }

        public DateTime GetModuleDateIssue(int moduleCode, DateTime issueDate)
        {
            return DelegateService.commonService.GetModuleDateIssue(moduleCode, issueDate);
        }

        public ParameterDTO UpdateParameter(ParameterDTO parameter)
        {
            return DelegateService.commonService.UpdateParameter(parameter.ToModel()).ToDTO();
        }

        public List<CurrencyDTO> GetCurrencies()
        {
            return DelegateService.commonService.GetCurrencies().ToDTOs().ToList();
        }

        public List<BranchDTO> GetBranches()
        {
            return DelegateService.commonService.GetBranches().ToDTOs().ToList();
        }

        public List<PrefixDTO> GetPrefixes()
        { 
            return DelegateService.commonService.GetPrefixes().ToDTOs().ToList();
        }

        /// <summary>
        /// Obtener sucursal por id
        /// </summary>
        /// <returns></returns>

        public BranchDTO GetBranchById(int id) { 
            return DelegateService.commonService.GetBranchById(id).ToDTO();
        }

        /// <summary>
        /// Obtener un ramo por id
        /// </summary>
        /// <returns></returns>
        
        public PrefixDTO GetPrefixById(int id)
        {
            return DelegateService.commonService.GetPrefixById(id).ToDTO();
        }

        public ExchangeRateDTO GetExchangeRateByCurrencyId(int currencyId)
        {
            return DelegateService.commonService.GetExchangeRateByCurrencyId(currencyId).ToDTO();
        }

        public List<ExchangeRateDTO> GetExchangeRates(DateTime? dateCumulus = null, int? CurrecyCode = null)
        {
            return DelegateService.commonService.GetExchangeRates(dateCumulus,CurrecyCode).ToDTOs().ToList();
        }
        public List<LineBusinessDTO> GetLinesBusinessByPrefixId(int PrefixId)
        {
            return DelegateService.commonService.GetLinesBusinessByPrefixId(PrefixId).ToDTOs().ToList();
        }

    }
}
