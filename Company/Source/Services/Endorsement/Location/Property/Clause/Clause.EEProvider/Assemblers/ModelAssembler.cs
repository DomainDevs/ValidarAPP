using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.PropertyClauseService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region automapper
        #region Clausulas
        public static IMapper CreateMapClause()
        {
            var config = MapperCache.GetMapper<CompanyClause, Clause>(cfg =>
            {
                cfg.CreateMap<CompanyClause, Clause>();
            });
            return config;
        }
        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config;
        }
        #endregion
        #endregion automapper
    }
}
