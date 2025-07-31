using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    public class CoverageBusiness
    {
        UnderwritingServiceEEProviderCore underwritingServiceEEProviderCore = new UnderwritingServiceEEProviderCore();

        /// <summary>
        /// calcula el valor de la prima mínima para tasa por cobertura
        /// </summary>
        /// <param name="coverages"></param>
        /// <param name="minimumPremiumAmount"></param>
        /// <param name="prorate"></param>
        /// <returns>Coberturas actualizadas</returns>
        public List<CompanyCoverage> CalculateMinimumPremiumRatePerCoverage(List<CompanyCoverage> coverages, decimal minimumPremiumAmount, bool prorate, bool assistance)
        {
            List<CompanyCoverage> requiredCoverages = new List<CompanyCoverage>();
            if (coverages == null)
            {
                throw new ValidationException("INPUT_COVERAGES_NOT_FOUND");
            }
            requiredCoverages = coverages.Where(x => x.RateType.HasValue && x.RateType != RateType.FixedValue && x.IsAccMinPremium
                                                        /*estas condiciones se agregan para evitar errores en los cálculos*/
                                                        && x.Rate.HasValue && x.SubLimitAmount != 0).ToList();
            if (!assistance)
            {
                requiredCoverages = requiredCoverages.Where(x => !x.IsAssistance).ToList();
            }

            if (!requiredCoverages.Any())
            {
                return coverages;
            }
            var initialPremiumAmount = requiredCoverages.Sum(x =>
            {
                var rateFactor = x.RateType == RateType.Percentage ? 0.01M : 0.001M;
                return x.SubLimitAmount * x.Rate.Value * rateFactor;
            });
            if (initialPremiumAmount <= 0 || initialPremiumAmount >= minimumPremiumAmount)
            {
                return coverages;
            }
            var premiumDifferenceToDistribute = minimumPremiumAmount - initialPremiumAmount;
            foreach (var coverage in requiredCoverages)
            {
                var rateFactor = coverage.RateType == RateType.Percentage ? 100M : 1000M;
                var rate = coverage.Rate.Value / rateFactor;
                var annualizedCoveragePremium = coverage.SubLimitAmount * rate;
                if (annualizedCoveragePremium == 0)
                {
                    continue;
                }
                var premiumDifferenceAmount = premiumDifferenceToDistribute * annualizedCoveragePremium / initialPremiumAmount;
                var differenceRate = premiumDifferenceAmount / coverage.SubLimitAmount;
                var prorateFactor = 1M;
                if (prorate)
                {
                    if (!coverage.CurrentTo.HasValue)
                    {
                        throw new ValidationException("NULL_CURRENT_TO_DATE_COVERAGE");
                    }
                    var dateDif = coverage.CurrentTo.Value - coverage.CurrentFrom;
                    prorateFactor = dateDif.Days / 365M;
                }
                var newRate = differenceRate + rate;
                var newPremium = coverage.SubLimitAmount * newRate * prorateFactor;
                coverage.DiffMinPremiumAmount = newPremium - coverage.PremiumAmount;
                coverage.Rate = newRate * rateFactor;
                coverage.PremiumAmount = newPremium;
            }
            return coverages;
        }
        // TODO definir la ubicación correcta de estos métodos
        public decimal GetMinimumPremiumAmount(List<DynamicConcept> modelDynamicProperties)
        {
            if (modelDynamicProperties == null)
            {
                return 0;
            }
            var minimumPremiumConceptId = GetConceptIdByParameterId((int)ParametersTypes.MinimumPremiumConcept, true);
            var minimumPremiumAmountConcept = modelDynamicProperties.FirstOrDefault(x => x.Id == minimumPremiumConceptId.Value);
            if (minimumPremiumAmountConcept == null || minimumPremiumAmountConcept.Value == null)
            {
                return 0;
            }
            return Convert.ToDecimal(minimumPremiumAmountConcept.Value);
        }

        public bool GetProrateMinimumPremium(List<DynamicConcept> modelDynamicProperties)
        {
            var prorateConceptId = GetConceptIdByParameterId((int)ParametersTypes.ProrateConcept, true);
            var prorateConcept = modelDynamicProperties.FirstOrDefault(x => x.Id == prorateConceptId.Value);
            if (prorateConcept == null)
            {
                return false;
            }
            return prorateConcept.Value as int? == 1;

        }
        private int? GetConceptIdByParameterId(int id, bool required)
        {
            Core.Application.CommonService.Models.Parameter parameter;
            Exception exception = null;
            try
            {
                parameter = DelegateService.commonService.GetParameterByParameterId(id);
                if (parameter == null)
                {
                    //Consulta en la co_parameter
                    parameter = DelegateService.commonService.GetExtendedParameterByParameterId(id);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                parameter = null;
            }
            if (parameter == null)
            {
                if (required)
                {
                    throw new ValidationException("ERROR_GET_PARAMETER_ID_" + id, exception);
                }
                return null;
            }
            if (parameter.NumberParameter.HasValue)
            {
                return parameter.NumberParameter.Value;
            }
            if (required)
            {
                throw new ValidationException("ERROR_GET_CONCEPT_ID_FROM_PARAMETER_" + id);
            }
            return null;
        }

        public bool GetAssistanceInPremiumMin(List<DynamicConcept> modelDynamicProperties)
        {
            var assistanceInPremiumMinConceptId = GetConceptIdByParameterId((int)ParametersTypes.IncludeAssistanceInPremiumMin, true);
            var assistanceInPremiumMinConcept = modelDynamicProperties.FirstOrDefault(x => x.Id == assistanceInPremiumMinConceptId.Value);
            if (assistanceInPremiumMinConcept == null)
            {
                return false;
            }
            return assistanceInPremiumMinConcept.Value as int? == 1;
        }

        public List<CompanyCoverage> GetDeductiblesByCompanyCoverages(List<CompanyCoverage> companyCoverages)
        {
            List<Coverage> coverages = ModelAssembler.CreateCoverages(companyCoverages);
            coverages = underwritingServiceEEProviderCore.GetDeductiblesByCoverages(coverages);
            List<CompanyCoverage> listCompanyCoverage = ModelAssembler.CreateCompanyCoverages(coverages);
            foreach (CompanyCoverage companyCoverage in companyCoverages)
            {
                listCompanyCoverage.Find(x => x.Id == companyCoverage.Id).IsPostcontractual= companyCoverage.IsPostcontractual;
            }
            return listCompanyCoverage;
        }

        public void CalculateCompanyPremiumDeductible(CompanyCoverage companyCoverage)
        {
            Coverage coverage = ModelAssembler.CreateCoverage(companyCoverage);
            underwritingServiceEEProviderCore.CalculatePremiumDeductible(coverage);
            companyCoverage = ModelAssembler.CreateCompanyCoverage(coverage);
        }

        public CompanyCoverage GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            Coverage coverage = underwritingServiceEEProviderCore.GetCoverageByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            return ModelAssembler.CreateCompanyCoverage(coverage);
        }

        public List<CompanyCoverage> GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public CompanyCoverage QuotateCompanyCoverage(CompanyCoverage companyCoverage, int policyId, int riskId, int decimalQuantity, int? CoveredRiskType = 0, int? prefixId = 0)
        {
            Coverage coverage = ModelAssembler.CreateCoverage(companyCoverage);
            coverage = underwritingServiceEEProviderCore.Quotate(coverage, policyId, riskId, decimalQuantity, CoveredRiskType, prefixId);
            var mapCompanyCoverage = ModelAssembler.CreateCompanyCoverage(coverage);
            mapCompanyCoverage.IsPostcontractual = companyCoverage.IsPostcontractual;
            mapCompanyCoverage.IsEnabledMinimumPremium = companyCoverage.IsEnabledMinimumPremium;
            return mapCompanyCoverage;
        }

        public List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, groupCoverageId, productId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<CompanyCoverage> GetCompanyCoverageByCoverageIdsProductIdGroupCoverageId(List<int> coverageIds, int productId, int groupCoverageId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoverageByListCoverageIdProductIdGroupCoverageId(coverageIds, productId, groupCoverageId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public CompanyCoverage GetCompanyCoverageByRiskCoverageId(int riskCoverageId)
        {
            Coverage coverage = underwritingServiceEEProviderCore.GetCoverageByRiskCoverageId(riskCoverageId);
            return ModelAssembler.CreateCompanyCoverage(coverage);
        }

        public List<CompanyCoverage> GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetAllyCoveragesByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<CompanyCoverage> GetAddCompanyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetAddCoveragesByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<CompanyCoverage> GetCompanyCoveragesByTechnicalPlanId(int technicalPlanId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesByTechnicalPlanId(technicalPlanId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<CompanyCoverage> GetCompanyCoveragesPrincipalByInsuredObjectId(int insuredObjectId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesPrincipalByInsuredObjectId(insuredObjectId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public CompanyCoverage GetCompanyCoverageProductByCoverageId(int coverageId)
        {
            Coverage coverage = underwritingServiceEEProviderCore.GetCoverageProductByCoverageId(coverageId);
            return ModelAssembler.CreateCompanyCoverage(coverage);
        }

        public List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectId(int insuredObjectId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesByInsuredObjectId(insuredObjectId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<CompanyCoverage> GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(List<int> insuredObjectsIds, int groupCoverageId, int productId, bool filterSelected)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesByInsuredObjectIdsGroupCoverageIdProductId(insuredObjectsIds, groupCoverageId, productId, filterSelected);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }

        public List<int> GetAssistanceCoveragesIds(CompanyParameterType companyParameterType)
        {
            CompanyParameter cptParameter = DelegateService.commonService.FindCoParameter((int)companyParameterType);
            var a = new List<int>();
            if (cptParameter == null || string.IsNullOrEmpty(cptParameter.TextParameter))
            {
                return a;
            }
            List<int> assistanceCoveargesIds = cptParameter.TextParameter.Split(',').Select(x =>
            {
                int assistanceCoverageId;
                if (int.TryParse(x, out assistanceCoverageId))
                    return assistanceCoverageId;
                return 0;
            }).Where(x => x > 0).ToList();
            return assistanceCoveargesIds;
        }

        internal List<CompanyCoverage> GetCompanyCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            List<Coverage> coverages = underwritingServiceEEProviderCore.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId);
            return ModelAssembler.CreateCompanyCoverages(coverages);
        }
    }
}