using AutoMapper;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.ChangePolicyHolderEndorsement.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        /// <summary>
        /// Creates the coverages.
        /// </summary>
        /// <param name="riskCoverages">The risk coverages.</param>
        /// <returns></returns>
        public static List<CompanyCoverage> CreateCiaCoverages(List<ISSEN.RiskCoverage> riskCoverages)
        {
            var mapper = CreateMapEntityCoverage();
            var companyCoverage = mapper.Map<List<ISSEN.RiskCoverage>, List<CompanyCoverage>>(riskCoverages);
            companyCoverage.AsParallel().ForAll(x => x.DynamicProperties = new List<DynamicConcept>());
            object objlock = new object();
            companyCoverage.AsParallel().ForAll(x =>
            {
                var dinamicPropertyList = riskCoverages.FirstOrDefault(z => z.CoverageId == x.Id).DynamicProperties;
                if (dinamicPropertyList != null & dinamicPropertyList.Any())
                {
                    TP.Parallel.ForEach(dinamicPropertyList, item =>
                    {
                        DynamicProperty ItemDynamicProperty = (DynamicProperty)item.Value;
                        DynamicConcept ItemDynamicConcept = new DynamicConcept();
                        ItemDynamicConcept.Id = ItemDynamicProperty.Id;
                        ItemDynamicConcept.Value = ItemDynamicProperty.Value;
                        lock (objlock)
                        {
                            x.DynamicProperties.Add(ItemDynamicConcept);
                        }
                    });

                }
            });
            return companyCoverage;
        }
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
        #endregion
    }
}
