using AutoMapper;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.ClauseEndorsement.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Automapper
        /// <summary>
        /// Creates the map endorsement.
        /// </summary>
        public static IMapper CreateMapEndorsement()
        {
            var config = MapperCache.GetMapper<Endorsement, CompanyEndorsement>(cfg =>
            {

                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                cfg.CreateMap<Text, CompanyText>();
            });
            return config;
        }

        /// <summary>
        /// Creates the map endorsement.
        /// </summary>
        public static IMapper CreateMapEntityCoverage()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskCoverage, CompanyCoverage>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskCoverage, CompanyCoverage>()
            .ForMember(m => m.Id, opt => opt.MapFrom(src => src.CoverageId))
            .ForMember(m => m.FirstRiskType, opt => opt.MapFrom(src => (FirstRiskType?)src.FirstRiskTypeCode))
            .ForMember(m => m.CalculationType, opt => opt.MapFrom(src => (Core.Services.UtilitiesServices.Enums.CalculationType?)src.CalculationTypeCode))
            .ForMember(m => m.RateType, opt => opt.MapFrom(src => (RateType?)src.RateTypeCode))
            .ForMember(m => m.Id, opt => opt.MapFrom(src => src.CoverageId));
                cfg.CreateMap<DynamicProperty, DynamicConcept>();
            });
            return config;
        }
        public static IMapper CreateMapClause()
        {
            var config = MapperCache.GetMapper<CompanyClause, Clause>(cfg =>
            {
                cfg.CreateMap<CompanyClause, Clause>();
            });
            return config;
        }
        #endregion
    }
}
