using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.MarineExtensionService.EEProvider.AutoMapperAssembler
{
    public class AutoMapperAssembler
    {
        #region automapper

        public static IMapper CreateMapClause()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });

            return config.CreateMapper();
        }
        #endregion
    }
}
