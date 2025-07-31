using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Cache;
using COMMEN = Sistran.Core.Application.Common.Entities;
namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers
{
    public class AutommaperAssembler
    {
        #region punto de venta
        public static IMapper CreateMapSalePoints()
        {
            var config = MapperCache.GetMapper<COMMEN.SalePoint, SalePoint>(cfg =>
            {
                cfg.CreateMap<COMMEN.SalePoint, SalePoint>().
                ForMember(dest => dest.Id, ori => ori.MapFrom(x => x.SalePointCode)).
                ForMember(dest => dest.IsEnabled, ori => ori.MapFrom(x => x.Enabled == null ? false : x.Enabled));
            });
            return config;
        }
        #endregion punto de venta
    }
}
