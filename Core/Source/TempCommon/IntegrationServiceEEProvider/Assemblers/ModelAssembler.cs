using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.TempCommonService.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Integration.TempCommon.EEProvider.Assemblers
{
    internal static class ModelAssembler
    {
        #region Policy

        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<PolicyDTO, Policy>(cfg =>
            {
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<PrefixTypeDTO, PrefixType>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<RiskDTO, Risk>();
                cfg.CreateMap<CoverageDTO, Coverage>();
                cfg.CreateMap<AmountDTO, AmountDTO>();
            });
            return config;
        }

        public static Policy ToModel(this PolicyDTO policyDTO)
        {
            var config = CreateMapPolicy();
            return config.Map<PolicyDTO, Policy>(policyDTO);
        }

        public static IEnumerable<Policy> ToModels(this IEnumerable<PolicyDTO> policyDTO)
        {
            return policyDTO.Select(ToModel);
        }
        #endregion

        #region ModuleDate
        public static IMapper CreateMapModuleDate()
        {
            var config = MapperCache.GetMapper<ModuleDateDTO, ModuleDate>(cfg =>
            {
                cfg.CreateMap<ModuleDateDTO, ModuleDate>();
            });
            return config;
        }

        public static ModuleDate ToModel(this ModuleDateDTO moduleDateDTO)
        {
            var config = CreateMapModuleDate();
            return config.Map<ModuleDateDTO, ModuleDate>(moduleDateDTO);
        }

        public static IEnumerable<ModuleDate> ToModels(this IEnumerable<ModuleDateDTO> moduleDateDTO)
        {
            return moduleDateDTO.Select(ToModel);
        }
        #endregion

        #region Endorsement

        public static IMapper CreateMapEndorsement()
        {
            var config = MapperCache.GetMapper<EndorsementDTO, Endorsement>(cfg =>
            {
                cfg.CreateMap<EndorsementDTO, Endorsement>();
            });
            return config;
        }

        public static Endorsement ToModel(this EndorsementDTO endorsementDTO)
        {
            var config = CreateMapModuleDate();
            return config.Map<EndorsementDTO, Endorsement>(endorsementDTO);
        }

        public static IEnumerable<Endorsement> ToModels(this IEnumerable<EndorsementDTO> endorsementDTO)
        {
            return endorsementDTO.Select(ToModel);
        }

        #endregion


    }
}
