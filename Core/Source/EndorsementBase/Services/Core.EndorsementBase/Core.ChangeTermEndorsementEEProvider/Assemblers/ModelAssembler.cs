using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.ChangeTermEndorsement.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static List<Coverage> CreateCoverages(List<ISSEN.RiskCoverage> riskCoverages)
        {
            List<Coverage> coverages = new List<Coverage>();

            foreach (ISSEN.RiskCoverage riskCoverage in riskCoverages)
            {
                coverages.Add(CreateCoverage(riskCoverage));
            }

            return coverages;
        }

        public static Coverage CreateCoverage(ISSEN.RiskCoverage riskCoverage)
        {
            Coverage coverage = new Coverage
            {
                Id = riskCoverage.CoverageId,
                RiskCoverageId = riskCoverage.RiskCoverId,
                IsDeclarative = riskCoverage.IsDeclarative,
                IsMinPremiumDeposit = riskCoverage.IsMinPremiumDeposit,
                FirstRiskType = (FirstRiskType?)riskCoverage.FirstRiskTypeCode,
                CalculationType = (Services.UtilitiesServices.Enums.CalculationType?)riskCoverage.CalculationTypeCode,
                PremiumAmount = riskCoverage.PremiumAmount,
                LimitOccurrenceAmount = riskCoverage.LimitOccurrenceAmount,
                LimitClaimantAmount = riskCoverage.LimitClaimantAmount,
                RateType = (RateType?)riskCoverage.RateTypeCode,
                Rate = riskCoverage.Rate,
                CurrentFrom = riskCoverage.CurrentFrom.Value,
                CurrentTo = riskCoverage.CurrentTo,
                DeclaredAmount = riskCoverage.DeclaredAmount,
                LimitAmount = riskCoverage.LimitAmount,
                SubLimitAmount = riskCoverage.SublimitAmount,
                EndorsementLimitAmount = riskCoverage.EndorsementLimitAmount == null ? 0 : riskCoverage.EndorsementLimitAmount.Value,
                EndorsementSublimitAmount = riskCoverage.EndorsementSublimitAmount == null ? 0 : riskCoverage.EndorsementSublimitAmount.Value,
                CoverStatus = CoverageStatusType.Included,
                CoverageOriginalStatus = CoverageStatusType.Included,
                CoverNum = riskCoverage.CoverageId,
                DynamicProperties = new List<DynamicConcept>(),
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
    }
}
