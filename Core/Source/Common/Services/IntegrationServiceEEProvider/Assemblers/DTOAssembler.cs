using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.CommonServices.DTOs;

namespace Sistran.Core.Integration.CommonServices.EEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        internal static IMapper CreateMapLineBusiness()
        {
            var config = MapperCache.GetMapper<LineBusiness, LineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
            });
            return config;
        }

        internal static LineBusinessDTO ToDTO(this LineBusiness lineBusiness)
        {
            var config = CreateMapLineBusiness();
            return config.Map<LineBusiness, LineBusinessDTO>(lineBusiness);
        }

        internal static IEnumerable<LineBusinessDTO> ToDTOs(this IEnumerable<LineBusiness> lineBusiness)
        {
            return lineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateSubLineBusinessByLineBusinessId()
        {
            var config = MapperCache.GetMapper<SubLineBusiness, SubLineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
            });
            return config;
        }

        internal static SubLineBusinessDTO ToDTO(this SubLineBusiness subLineBusiness)
        {
            var config = CreateSubLineBusinessByLineBusinessId();
            return config.Map<SubLineBusiness, SubLineBusinessDTO>(subLineBusiness);
        }

        internal static IEnumerable<SubLineBusinessDTO> ToDTOs(this IEnumerable<SubLineBusiness> subLineBusiness)
        {
            return subLineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateParameter()
        {
            var config = MapperCache.GetMapper<Parameter, ParameterDTO>(cfg =>
            {
                cfg.CreateMap<Parameter, ParameterDTO>();
            });
            return config;
        }

        internal static ParameterDTO ToDTO(this Parameter parameter)
        {
            var config = CreateParameter();
            return config.Map<Parameter, ParameterDTO>(parameter);
        }

        internal static IEnumerable<ParameterDTO> ToDTOs(this IEnumerable<Parameter> parameters)
        {
            return parameters.Select(ToDTO);
        }

        internal static IMapper CreateMapCurrencies()
        {
            var config = MapperCache.GetMapper<Currency, CurrencyDTO>(cfg =>
            {
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        internal static CurrencyDTO ToDTO(this Currency currency)
        {
            var config = CreateMapCurrencies();
            return config.Map<Currency, CurrencyDTO>(currency);
        }

        internal static IEnumerable<CurrencyDTO> ToDTOs(this IEnumerable<Currency> currencies)
        {
            return currencies.Select(ToDTO);
        }

        internal static IMapper CreateMapBranches()
        {
            var config = MapperCache.GetMapper<Branch, BranchDTO>(cfg =>
            {
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
            });
            return config;
        }

        internal static BranchDTO ToDTO(this Branch branchDTO)
        {
            var config = CreateMapBranches();
            return config.Map<Branch, BranchDTO>(branchDTO);
        }

        internal static IEnumerable<BranchDTO> ToDTOs(this IEnumerable<Branch> branches)
        {
            return branches.Select(ToDTO);
        }

        internal static IMapper CreateMapPrefixes()
        {
            var config = MapperCache.GetMapper<Prefix, PrefixDTO>(cfg =>
            {
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<PrefixType, PrefixTypeDTO>();
            });
            return config;
        }

        internal static PrefixDTO ToDTO(this Prefix prefixDTO)
        {
            var config = CreateMapPrefixes();
            return config.Map<Prefix, PrefixDTO>(prefixDTO);
        }

        internal static IEnumerable<PrefixDTO> ToDTOs(this IEnumerable<Prefix> prefixes)
        {
            return prefixes.Select(ToDTO);
        }


        internal static IMapper CreateMapExchangeRates()
        {
            var config = MapperCache.GetMapper<ExchangeRate, ExchangeRateDTO>(cfg =>
            {
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        internal static ExchangeRateDTO ToDTO(this ExchangeRate exchangeRate)
        {
            var config = CreateMapPrefixes();
            return config.Map<ExchangeRate, ExchangeRateDTO>(exchangeRate);
        }

        internal static IEnumerable<ExchangeRateDTO> ToDTOs(this IEnumerable<ExchangeRate> exchangeRates)
        {
            return exchangeRates.Select(ToDTO);
        }

    }
}
