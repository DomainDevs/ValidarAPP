using AutoMapper;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Locations.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Suffix
        public static IMapper CreateMapSuffix()
        {
            IMapper config = MapperCache.GetMapper<StreetType, Suffix>(cfg =>
            {
                cfg.CreateMap<StreetType, Suffix>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.StreetTypeCode))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.SmallDescription));
            });
            return config;
        }
        #endregion Suffix

        #region ApartmentOrOffice
        public static IMapper CreateMapApartmentOrOffice()
        {
            IMapper config = MapperCache.GetMapper<StreetType, ApartmentOrOffice>(cfg =>
            {
                cfg.CreateMap<StreetType, ApartmentOrOffice>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.StreetTypeCode))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.SmallDescription));
            });
            return config;
        }
        #endregion ApartmentOrOffice

        #region RouteType
        public static IMapper CreateMapRouteType()
        {
            IMapper config = MapperCache.GetMapper<StreetType, RouteType>(cfg =>
            {
                cfg.CreateMap<StreetType, RouteType>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.StreetTypeCode))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.SmallDescription));
            });
            return config;
        }
        #endregion RouteType
    }
}
