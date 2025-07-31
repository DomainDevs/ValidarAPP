using Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs;
using System;
using System.Collections.Generic;
using OQDTO = Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using OQINTDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using EGINTDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using System.Linq;

namespace Sistran.Core.Application.ReinsuranceOperatingQuotaServices.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        internal static OQDTO.FilterOperationQuotaDTO CreateFilterOperationQuotaIntegrationDTO(FilterOperationQuotaDTO filterOperationQuotaDTO)
        {
            return new OQDTO.FilterOperationQuotaDTO()
            {
                IndividualId = filterOperationQuotaDTO.IndividualId,
                DateCumulus = filterOperationQuotaDTO.DateCumulus,
                IsDatePolicy = filterOperationQuotaDTO.IsDatePolicy,
                IsFuture = filterOperationQuotaDTO.IsFuture,
                LineBusiness = filterOperationQuotaDTO.LineBusiness,
                SubLineBusiness = filterOperationQuotaDTO.SubLineBusiness,
                PrefixCd = filterOperationQuotaDTO.PrefixCd
            };
        }
        
        internal static OperatingQuotaEventDTO CreateOperatingQuotaEventIntegrationDTO(OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            return new OperatingQuotaEventDTO()
            {
                Cov_End_Date = operatingQuotaEventDTO.Cov_End_Date,
                Cov_Init_Date = operatingQuotaEventDTO.Cov_Init_Date,
                IdentificationId = operatingQuotaEventDTO.IdentificationId,
                IssueDate = operatingQuotaEventDTO.IssueDate,
                LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                OperatingQuotaEventID = operatingQuotaEventDTO.OperatingQuotaEventID,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
                payload = operatingQuotaEventDTO.payload,
                Policy_End_Date = operatingQuotaEventDTO.Policy_End_Date,
                Policy_Init_Date = operatingQuotaEventDTO.Policy_Init_Date,                
                ApplyReinsurance = CreateApplyReinsuranceIntegrationDTO(operatingQuotaEventDTO.ApplyReinsurance),
                PrefixCd = operatingQuotaEventDTO.PrefixCd
            };
        }
        
        internal static List<OperatingQuotaEventDTO> CreateOperatingQuotaEventsIntegrationDTO(List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEventIntegrationDTOs)
        {
            List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
            foreach (OQDTO.OperatingQuotaEventDTO operatingQuotaEventIntegrationDTO in operatingQuotaEventIntegrationDTOs)
            {
                operatingQuotaEventDTOs.Add(CreateOperatingQuotaEventIntegrationDTO(operatingQuotaEventIntegrationDTO));
            }
            return operatingQuotaEventDTOs;
        }
        
        internal static ApplyReinsuranceDTO CreateApplyReinsuranceIntegrationDTO(OQDTO.ApplyReinsuranceDTO applyReinsuranceDTO)
        {
            return new ApplyReinsuranceDTO()
            {                
                ContractCoverage = CreateContractsCoverageIntegrationDTO(applyReinsuranceDTO.ContractCoverage),
                CoverageId = applyReinsuranceDTO.CoverageId,
                CurrencyType = applyReinsuranceDTO.CurrencyType,
                CurrencyTypeDesc = applyReinsuranceDTO.CurrencyTypeDesc,
                DocumentNum = applyReinsuranceDTO.DocumentNum,
                EndorsementId = applyReinsuranceDTO.EndorsementId,
                EndorsementType = applyReinsuranceDTO.EndorsementType,
                IndividualId = applyReinsuranceDTO.IndividualId,
                ConsortiumId = applyReinsuranceDTO.ConsortiumId,
                EconomicGroupId = applyReinsuranceDTO.EconomicGroupId,
                ParticipationPercentage = applyReinsuranceDTO.ParticipationPercentage,
                PolicyID = applyReinsuranceDTO.PolicyID,
                PrefixId = applyReinsuranceDTO.PrefixId,
                BranchId = applyReinsuranceDTO.BranchId,
            };
        }
        
        internal static ContractCoverageDTO CreateContractCoverageIntegrationDTO(OQDTO.ContractCoverageDTO contractCoverageDTO)
        {
            return new ContractCoverageDTO()
            {
                Amount = contractCoverageDTO.Amount,
                LevelLimit = contractCoverageDTO.LevelLimit,
                ContractDescription = contractCoverageDTO.ContractDescription,
                ContractId = contractCoverageDTO.ContractId,
                ContractCurrencyId = contractCoverageDTO.ContractCurrencyId,
                Premium = contractCoverageDTO.Premium
            };
        }
        
        internal static List<ContractCoverageDTO> CreateContractsCoverageIntegrationDTO(List<OQDTO.ContractCoverageDTO> contractCoverageDTOs)
        {
            List<ContractCoverageDTO> contractCoverageIntegrationDTOs = new List<ContractCoverageDTO>();
            foreach (OQDTO.ContractCoverageDTO contractCoverageDTO in contractCoverageDTOs)
            {
                contractCoverageIntegrationDTOs.Add(CreateContractCoverageIntegrationDTO(contractCoverageDTO));
            }
            return contractCoverageIntegrationDTOs;
        }

        internal static IMapper CreateMapOQINTDTOOperatingQuotaByOperatingQuotaEventDTO()
        {
            var config = MapperCache.GetMapper<OperatingQuotaEventDTO, OQINTDTO.OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<OQINTDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>();
                cfg.ValidateInlineMaps = false;
            });
            return config;
        }

        internal static OQINTDTO.OperatingQuotaEventDTO CreateOQINTDTOOperatingQuotaByOperatingQuotaEventDTO(this OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapOQINTDTOOperatingQuotaByOperatingQuotaEventDTO();
            return config.Map<OperatingQuotaEventDTO, OQINTDTO.OperatingQuotaEventDTO>(operatingQuotaEventDTO);
        }

        internal static List<OQINTDTO.OperatingQuotaEventDTO> CreateOQINTDTOOperatingQuotaByOperatingQuotaEventDTOs(this List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            var config = CreateMapOQINTDTOOperatingQuotaByOperatingQuotaEventDTO();
            return config.Map<List<OperatingQuotaEventDTO>, List<OQINTDTO.OperatingQuotaEventDTO>>(operatingQuotaEventDTOs);
        }

        internal static IMapper CreateMapEconomicGroupEventIntegration()
        {
            var config = MapperCache.GetMapper<EGINTDTO.EconomicGroupEventDTO, EconomicGroupEventDTO>(cfg =>
            {
                cfg.CreateMap<EGINTDTO.EconomicGroupEventDTO, EconomicGroupEventDTO>();
            });
            return config;
        }

        internal static EconomicGroupEventDTO ToIntegrationDTO(this EGINTDTO.EconomicGroupEventDTO economicGroupEventDTO)
        {
            var config = CreateMapEconomicGroupEventIntegration();
            return config.Map<EGINTDTO.EconomicGroupEventDTO, EconomicGroupEventDTO>(economicGroupEventDTO);
        }

        internal static IEnumerable<EconomicGroupEventDTO> ToIntegrationDTOs(this IEnumerable<EGINTDTO.EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            return economicGroupEventDTOs.Select(ToIntegrationDTO);
        }
    }
}
