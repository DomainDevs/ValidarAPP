using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.AircraftClauseService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region automapper
        #region Clausulas
        public static IMapper CreateMapClause()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyClause, Clause>();
            });
            return config.CreateMapper();
        }
        public static IMapper CreateMapCompanyClause()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config.CreateMapper();
        }
        #endregion
        #endregion automapper
    }
}
