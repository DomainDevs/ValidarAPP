using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
    
        #region Componentes
        public static IMapper CreateMapCompanyComponentValueDTO()
        {
            IMapper config = MapperCache.GetMapper<CompanySummary, ComponentValueDTO>(cfg =>
            {
                cfg.CreateMap<CompanySummary, ComponentValueDTO>();
            });
            return config;
        }
        #endregion Componentes
     
    }
}
