using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.TempCommonService.DTOs;

namespace Sistran.Core.Integration.TempCommon.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        #region Policy
        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<Policy, PolicyDTO>(cfg =>
            {
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<PrefixType, PrefixTypeDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<Risk, RiskDTO>();
                cfg.CreateMap<Coverage, CoverageDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        public static PolicyDTO ToDTO(this Policy policy)
        {
            var config = CreateMapPolicy();
            return config.Map<Policy, PolicyDTO>(policy);
        }

        public static IEnumerable<PolicyDTO> ToDTOs(this IEnumerable<Policy> policy)
        {
            return policy.Select(ToDTO);
        }
        #endregion

        #region ModuleDate
        public static IMapper CreateMapModuleDate()
        {
            var config = MapperCache.GetMapper<ModuleDate, ModuleDateDTO>(cfg =>
            {
                cfg.CreateMap<ModuleDate, ModuleDateDTO>();
            });
            return config;
        }

        public static ModuleDateDTO ToDTO(this ModuleDate moduleDate)
        {
            var config = CreateMapModuleDate();
            return config.Map<ModuleDate, ModuleDateDTO>(moduleDate);
        }

        public static IEnumerable<ModuleDateDTO> ToDTOs(this IEnumerable<ModuleDate> moduleDate)
        {
            return moduleDate.Select(ToDTO);
        }

        #endregion

        #region Endorsement
        public static IMapper CreateMapEndorsement()
        {
            var config = MapperCache.GetMapper<Application.TempCommonServices.DTOs.EndorsementDTO, EndorsementDTO>(cfg =>
            {
                cfg.CreateMap<Application.TempCommonServices.DTOs.EndorsementDTO, EndorsementDTO>();                
            });
            return config;
        }

        public static EndorsementDTO ToDTO(this Application.TempCommonServices.DTOs.EndorsementDTO endorsement)
        {
            var config = CreateMapEndorsement();
            return config.Map<Application.TempCommonServices.DTOs.EndorsementDTO, EndorsementDTO>(endorsement);
        }

        public static IEnumerable<EndorsementDTO> ToDTOs(this IEnumerable<Application.TempCommonServices.DTOs.EndorsementDTO> endorsement)
        {
            return endorsement.Select(ToDTO);
        }

        #endregion

        public static IMapper CreateMapIndividual()
        {
            var config = MapperCache.GetMapper<Application.TempCommonServices.DTOs.IndividualDTO, IndividualDTO>(cfg =>
            {
                cfg.CreateMap<Application.TempCommonServices.DTOs.IndividualDTO, IndividualDTO>();
            });
            return config;
        }

        public static IndividualDTO ToDTO(this Application.TempCommonServices.DTOs.IndividualDTO individualDTO)
        {
            var config = CreateMapIndividual();
            return config.Map<Application.TempCommonServices.DTOs.IndividualDTO, IndividualDTO>(individualDTO);
        }

        public static IEnumerable<IndividualDTO> ToDTOs(this IEnumerable<Application.TempCommonServices.DTOs.IndividualDTO> individualDTO)
        {
            return individualDTO.Select(ToDTO);
        }

        public static IMapper CreateMapAgent()
        {
            var config = MapperCache.GetMapper<Application.TempCommonServices.DTOs.AgentDTO, AgentDTO>(cfg =>
            {
                cfg.CreateMap<Application.TempCommonServices.DTOs.AgentDTO, AgentDTO>();
            });
            return config;
        }

        public static AgentDTO ToDTO(this Application.TempCommonServices.DTOs.AgentDTO agentDTO)
        {
            var config = CreateMapAgent();
            return config.Map<Application.TempCommonServices.DTOs.AgentDTO, AgentDTO>(agentDTO);
        }

        public static IEnumerable<AgentDTO> ToDTOs(this IEnumerable<Application.TempCommonServices.DTOs.AgentDTO> agentDTO)
        {
            return agentDTO.Select(ToDTO);
        }

        public static IMapper CreateMapProduct()
        {
            var config = MapperCache.GetMapper<Product, ProductDTO>(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
            });
            return config;
        }

        public static ProductDTO ToDTO(this Product product)
        {
            var config = CreateMapAgent();
            return config.Map<Product, ProductDTO>(product);
        }

        public static IEnumerable<ProductDTO> ToDTOs(this IEnumerable<Product> product)
        {
            return product.Select(ToDTO);
        }
    }
}
