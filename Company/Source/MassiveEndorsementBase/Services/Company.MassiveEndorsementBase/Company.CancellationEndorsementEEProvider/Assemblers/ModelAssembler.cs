using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using USENUM = Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using AutoMapper;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static List<CompanyCoverage> CreateCoverages(List<ISSEN.RiskCoverage> riskCoverages)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();

            foreach (ISSEN.RiskCoverage riskCoverage in riskCoverages)
            {
                coverages.Add(CreateCoverage(riskCoverage));
            }

            return coverages;
        }

        public static CompanyCoverage CreateCoverage(ISSEN.RiskCoverage riskCoverage)
        {
            CompanyCoverage coverage = new CompanyCoverage
            {
                Id = riskCoverage.CoverageId,
                RiskCoverageId = riskCoverage.RiskCoverId,
                IsDeclarative = riskCoverage.IsDeclarative,
                IsMinPremiumDeposit = riskCoverage.IsMinPremiumDeposit,
                FirstRiskType = (FirstRiskType?)riskCoverage.FirstRiskTypeCode,
                CalculationType = (USENUM.CalculationType?)riskCoverage.CalculationTypeCode,
                PremiumAmount = riskCoverage.PremiumAmount,
                LimitOccurrenceAmount = riskCoverage.LimitOccurrenceAmount,
                LimitClaimantAmount = riskCoverage.LimitClaimantAmount,
                RateType = (RateType?)riskCoverage.RateTypeCode,
                Rate = riskCoverage.Rate,
                CurrentFrom = riskCoverage.CurrentFrom.HasValue ? riskCoverage.CurrentFrom.Value : DateTime.MinValue,
                CurrentTo = riskCoverage.CurrentTo,
                DynamicProperties = new List<DynamicConcept>(),
                EndorsementLimitAmount = riskCoverage.LimitAmount,
                EndorsementSublimitAmount = riskCoverage.SublimitAmount
            };

            foreach (DynamicProperty item in riskCoverage.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                coverage.DynamicProperties.Add(dynamicConcept);
            }

            return coverage;
        }

        #region Automapper
        /// <summary>
        /// Creates the map endorsement.
        /// </summary>
        public static IMapper CreateMapEndorsement()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                cfg.CreateMap<Text, CompanyText>();
            });
            return config.CreateMapper();
        }
        #endregion
        /// <summary>
        /// Creates the map CompanyPolicy.
        /// </summary>
        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<CompanyPolicy, Policy>(cfg =>
            {
                cfg.CreateMap<CompanyPolicy, Policy>();
            });
            return config;
        }

    }
}
