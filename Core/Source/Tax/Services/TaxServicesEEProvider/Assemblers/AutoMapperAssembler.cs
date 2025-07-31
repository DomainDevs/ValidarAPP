using AutoMapper;
using Sistran.Core.Application.Tax.Entities;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Application.TaxServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Calculate Tax     
        /// <summary>
        /// Creates the map company individual tax exemption.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapCompanyIndividualTaxExemption()
        {
            var config = MapperCache.GetMapper<IndividualTaxExemption, IndividualTaxExemptionDTO>(cfg =>
            {
                cfg.CreateMap<IndividualTaxExemption, IndividualTaxExemptionDTO>();
            });
            return config;
        }
        #endregion
        #region taxcomponent    
        /// <summary>
        /// Creates the map tax component.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapTaxComponent()
        {
            var config = MapperCache.GetMapper<TaxComponent, TaxComponentDTO>(cfg =>
            {
                cfg.CreateMap<TaxComponent, TaxComponentDTO>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.TaxCode);
                })
                 .ForMember(dest => dest.ComponentId, opt =>
                 {
                     opt.MapFrom(src => src.ComponentCode);
                 });
            });
            return config;
        }
        #endregion
    }
}
