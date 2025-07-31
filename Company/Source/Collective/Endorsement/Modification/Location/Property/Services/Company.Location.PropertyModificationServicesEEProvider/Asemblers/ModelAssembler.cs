using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Automapper
        #region QuotateMapper

        public static IMapper CreateMapperQuotateCollectiveExclusionRows()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Component, CompanyComponent>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
            });
            return config.CreateMapper();
        }
        #endregion
        #region InclusionMapper
        public static IMapper CreateMapperInclusion()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyInsured, CompanyIssuanceInsured>();
            });
            return config.CreateMapper();
        }

        #endregion
        #endregion
    }
}
