using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.TransportExtensionService.EEProvider.AutoMapperAssembler
{
    public class AutoMapperAssembler
    {
        #region automapper

        public static IMapper CreateMapClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });

            return config;
        }
        #endregion
    }
}
