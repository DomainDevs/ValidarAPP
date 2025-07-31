using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        public static IMapper CreateMapPayerPaymentComp()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.PayerPaymentComp, PayerPaymentComp>(cfg =>
            {
                cfg.CreateMap<ISSEN.PayerPaymentComp, PayerPaymentComp>();
            });
            return config;
        }

        public static IMapper CreateMapPayerPaymentCompLbsb()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.PayerPaymentCompLbsb, PayerPaymentCompLbsb>(cfg =>
            {
                cfg.CreateMap<ISSEN.PayerPaymentCompLbsb, PayerPaymentCompLbsb>();
            });
            return config;
        }

        #region Component
        public static IMapper CreateMapComponentType()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.Component, ComponentTypeDTO>(cfg =>
            {
                cfg.CreateMap<QUOEN.Component, ComponentTypeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ComponentTypeCode))
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentCode));
            });
            return config;
        }
        public static IMapper CreateMapPaymentDistributionComponents()
        {
            IMapper config = MapperCache.GetMapper<PRODEN.CoPaymentDistributionComponent, PaymentDistributionCompDTO>(cfg =>
            {
                cfg.CreateMap<PRODEN.CoPaymentDistributionComponent, PaymentDistributionCompDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentNumber))
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentCode));
            });
            return config;
        }
        #endregion
    }
}
