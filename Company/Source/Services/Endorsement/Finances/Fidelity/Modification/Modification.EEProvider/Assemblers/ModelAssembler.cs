using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.FidelityModificationService.EEProvider.Assemblers
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

        #region mapper Cobertura
        /// <summary>
        /// Creates the map company coverage.
        /// </summary>
        public static IMapper CreateMapCompanyCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
            });
            return config.CreateMapper();

        }
        /// <summary>
        /// Creates the map coverage.
        /// </summary>
        public static IMapper CreateMapCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
            });
            return config.CreateMapper();
        }
        #endregion mapper Cobertura
        #endregion automapper
    }
}
