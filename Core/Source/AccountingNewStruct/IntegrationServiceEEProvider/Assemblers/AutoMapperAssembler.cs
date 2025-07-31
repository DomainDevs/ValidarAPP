using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;

namespace Sistran.Core.Integration.AccountingServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        public static IMapper CreateMapPaymentAppliedDTO()
        {
            IMapper config = MapperCache.GetMapper<PaymentAppliedDTO, PremiumBaseDTO>(cfg =>
            {
                cfg.CreateMap<PaymentAppliedDTO, PremiumBaseDTO>();
            });
            return config;
        }
    }
}
