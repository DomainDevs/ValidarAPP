using AutoMapper;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.Cache;
using UNDModel = Sistran.Core.Application.UnderwritingServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Locations.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Suffix
        public static IMapper CreateMapSuffix()
        {

            var config = MapperCache.GetMapper<StreetType, Models.Suffix>(cfg =>
            {
                cfg.CreateMap<StreetType, Models.Suffix>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StreetTypeCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription));
            });

            return config;
        }
        #endregion Suffix

        #region Suffix
        public static IMapper CreateMapApartmentsOrOffices()
        {

            var config = MapperCache.GetMapper<StreetType, Models.ApartmentOrOffice>(cfg =>
            {
                cfg.CreateMap<StreetType, Models.ApartmentOrOffice>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StreetTypeCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription));
            });

            return config;
        }
        #endregion Suffix

        #region RouteType
        public static IMapper CreateMapRouteType()
        {

            var config = MapperCache.GetMapper<StreetType, Models.RouteType>(cfg =>
            {
                cfg.CreateMap<StreetType, Models.RouteType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StreetTypeCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.SimilarStreetTypeCd, opt => opt.MapFrom(src => src.SimilarStreetTypeCode));
            });

            return config;
        }
        #endregion RouteType

        #region ConstructionType
        public static IMapper CreateMapConstructionType()
        {

            var config = MapperCache.GetMapper<ConstructionCategory, Models.ConstructionType>(cfg =>
            {
                cfg.CreateMap<ConstructionCategory, Models.ConstructionType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ConstructionCategoryCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription));
            });

            return config;
        }
        #endregion ConstructionType


        #region RiskTypes
        public static IMapper CreateMapRiskTypes()
        {

            var config = MapperCache.GetMapper<RiskTypeLocation, UNDModel.RiskType>(cfg =>
            {
                cfg.CreateMap<RiskTypeLocation, UNDModel.RiskType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RiskTypeLocationCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription));
            });

            return config;
        }
        #endregion RiskTypes

        #region RiskUse
        public static IMapper CreateMapRiskUse()
        {

            var config = MapperCache.GetMapper<RiskUseEarthquake, Models.RiskUse>(cfg =>
            {
                cfg.CreateMap<RiskUseEarthquake, Models.RiskUse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src.RiskUseCode)));
            });

            return config;
        }
        #endregion RiskUse

        #region RiskUse
        public static IMapper CreateMapPolicyRiskDTOs()
        {

            var config = MapperCache.GetMapper<ISSEN.Policy, UNDTO.PolicyRiskDTO>(cfg =>
            {
                cfg.CreateMap<ISSEN.Policy, UNDTO.PolicyRiskDTO>()
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixCode))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchCode));
            });

            return config;
        }
        #endregion RiskUse
    }
}
