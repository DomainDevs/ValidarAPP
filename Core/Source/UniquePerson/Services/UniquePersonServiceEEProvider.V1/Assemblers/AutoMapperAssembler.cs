using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using ENPERSON = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Assemblers
{
    class AutoMapperAssembler
    {
        #region Automapper
        #region Consorcio
        public static IMapper CreateMapConsortium()
        {
            var config = MapperCache.GetMapper<ENPERSON.CoConsortium, Models.Consortium>(cfg =>
            {
                cfg.CreateMap<ENPERSON.CoConsortium, Models.Consortium>()
               .ForMember(des => des.FullName, ori => ori.ResolveUsing<ConsortiumResolver>());
            });
            return config;
        }
        public class ConsortiumResolver : IValueResolver<ENPERSON.CoConsortium, Models.Consortium, string>

        {
            public string Resolve(ENPERSON.CoConsortium source, Models.Consortium destination, string member,ResolutionContext context)
            {
                return "";
            }
          
        }
        #endregion Consorcio
        #endregion Automapper
    }
}
