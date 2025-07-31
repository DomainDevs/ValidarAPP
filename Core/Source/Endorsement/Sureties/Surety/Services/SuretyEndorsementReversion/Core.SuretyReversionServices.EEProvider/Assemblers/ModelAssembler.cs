using AutoMapper;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Application.SuretyEndorsementReversionService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region automaper
        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<Policy, Policy>(cfg =>
            {
                cfg.CreateMap<Policy, Policy>();
            });
            return config;
        }
        #endregion
    }
}
