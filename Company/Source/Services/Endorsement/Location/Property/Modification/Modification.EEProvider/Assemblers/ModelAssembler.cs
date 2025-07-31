using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.PropertyModificationService.EEProvider.Assemblers
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

        #region mapper Cobertura
        /// <summary>
        /// Creates the map company coverage.
        /// </summary>
        public static IMapper CreateMapCompanyCoverage()
        {
            var config = MapperCache.GetMapper<Coverage, CompanyCoverage>(cfg =>
            {
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
            });
            return config;

        }
        /// <summary>
        /// Creates the map coverage.
        /// </summary>
        public static IMapper CreateMapCoverage()
        {
            var config = MapperCache.GetMapper<CompanyCoverage, Coverage>(cfg =>
            {
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
            });

            return config;
        }
        #endregion mapper Cobertura
        #endregion automapper
    }
}
