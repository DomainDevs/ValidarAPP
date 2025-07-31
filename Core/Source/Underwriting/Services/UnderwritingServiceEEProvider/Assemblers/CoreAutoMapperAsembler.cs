using AutoMapper;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using PRODEN = Sistran.Core.Application.Product.Entities;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers
{
    public class CoreAutoMapperAsembler
    {
        #region Automapper
        #region ContractType
        public static IMapper CreateMapContratType()
        {
            var config = MapperCache.GetMapper<SuretyContractType, ComboDTO>(cfg =>
            {
                cfg.CreateMap<SuretyContractType, ComboDTO>();

            });
            return config;
        }
        #endregion ContractType      
        #region ContractType
        public static IMapper CreateMapContractCategories()
        {
            var config = MapperCache.GetMapper<SuretyContractCategories, ComboDTO>(cfg =>
            {
                cfg.CreateMap<SuretyContractCategories, ComboDTO>();

            });
            return config;
        }
        #endregion ContractType
        #region ContractType
        public static IMapper CreateMapGroupCoverages()
        {
            var config = MapperCache.GetMapper<GroupCoverage, ComboDTO>(cfg =>
            {
                cfg.CreateMap<GroupCoverage, ComboDTO>();

            });
            return config;
        }
        #endregion ContractType

        #region CreateMapProductGroupCoverage
        public static IMapper CreateMapProductGroupCoverage()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductGroupCover, GroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductGroupCover, GroupCoverage>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverGroupId))
               .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => src.CoveredRiskTypeCode));
            });
            return config;
        }
        #endregion CreateMapProductGroupCoverage

        #endregion Automaper
    }
}
