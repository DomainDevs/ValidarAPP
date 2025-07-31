using Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Engine;
using Sistran.Core.Framework.Rules.Integration;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers
{
    /// <summary>
    /// Constructor de entidades
    /// </summary>
    public class EntityAssembler
    {

        public static void CreateFacadeRiskProperty(Rules.Facade facade, CompanyPropertyRisk propertyRisk /*Model.CompanyPropertyRisk propertyRisk*/)
        {
            //facade.SetConcept(CompanyRuleConceptGeneral.
            //FacadeRiskProperty facadeRiskProperty = new FacadeRiskProperty

            facade.SetConcept(CompanyRuleConceptRisk.TempId, propertyRisk.Risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, propertyRisk.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.InsuredId, propertyRisk.Risk.MainInsured == null ? (int?)null : propertyRisk.Risk.MainInsured.IndividualId == 0 ? (int?)null : propertyRisk.Risk.MainInsured.IndividualId);
            facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, propertyRisk.Risk.MainInsured == null ? 0 : (int)propertyRisk.Risk.MainInsured.CustomerType);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, propertyRisk.Risk.Policy.Product == null ? 0 : (int)propertyRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, propertyRisk.Risk.Status == null ? (int?)null : (int)propertyRisk.Risk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, propertyRisk.Risk.OriginalStatus == null ? (int?)null : (int)propertyRisk.Risk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, propertyRisk.Risk.Text == null ? string.Empty : propertyRisk.Risk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, propertyRisk.Risk.RatingZone == null ? (int?)null : propertyRisk.Risk.RatingZone.Id == 0 ? (int?)null : propertyRisk.Risk.RatingZone.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, propertyRisk.Risk.GroupCoverage == null ? (int?)null : propertyRisk.Risk.GroupCoverage.Id == 0 ? (int?)null : propertyRisk.Risk.GroupCoverage.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, propertyRisk.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, propertyRisk.Risk.LimitRc == null ? (int?)null : propertyRisk.Risk.LimitRc.Id == 0 ? (int?)null : propertyRisk.Risk.LimitRc.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, propertyRisk.Risk.LimitRc == null ? (decimal?)null : propertyRisk.Risk.LimitRc.LimitSum == 0 ? (decimal?)null : propertyRisk.Risk.LimitRc.LimitSum);
            facade.SetConcept(CompanyRuleConceptRisk.Apartment, propertyRisk.NomenclatureAddress == null ? 0 : propertyRisk.NomenclatureAddress.ApartmentOrOffice == null ? 0 : propertyRisk.NomenclatureAddress.ApartmentOrOffice.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CityCode, propertyRisk.City == null ? (int?)null : propertyRisk.City.Id);
            facade.SetConcept(CompanyRuleConceptRisk.RiskAge, propertyRisk.RiskAge);
            facade.SetConcept(CompanyRuleConceptRisk.EmlPercentage, propertyRisk.PML);
            facade.SetConcept(CompanyRuleConceptRisk.IsRetention, propertyRisk.Risk.IsRetention);
            facade.SetConcept(CompanyRuleConceptRisk.IsFacultative, propertyRisk.Risk.IsFacultative);
            facade.SetConcept(CompanyRuleConceptRisk.RiskTypeCode, propertyRisk.RiskType);


            if (propertyRisk.Risk.MainInsured != null)
            {
                // facadeRiskProperty.IsInsuredPayer = propertyRisk.MainInsured.IndividualId == propertyPolicy.Holder.IndividualId;

                if (propertyRisk.Risk.MainInsured.IndividualType == IndividualType.Person && propertyRisk.Risk.MainInsured.BirthDate != null)
                {
                    int insuredAge = (DateTime.Today - propertyRisk.Risk.MainInsured.BirthDate.Value).Days / 365;
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, propertyRisk.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, propertyRisk.Risk.MainInsured.BirthDate);
                }
            }

            if (propertyRisk.Risk.Beneficiaries != null && propertyRisk.Risk.Beneficiaries.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryId, propertyRisk.Risk.Beneficiaries.First().IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryPercentage, propertyRisk.Risk.Beneficiaries.First().Participation);
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryTypeCode, propertyRisk.Risk.Beneficiaries.First().BeneficiaryType.Id);
                //HasWarrantCreditor
            }

            if (propertyRisk.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in propertyRisk.Risk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

        public static void CreateFacadeCoverage(Rules.Facade facade, CompanyCoverage coverage)
        {
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageId, coverage.Id);
            facade.SetConcept(CompanyRuleConceptCoverage.IsDeclarative, coverage.IsDeclarative);
            facade.SetConcept(CompanyRuleConceptCoverage.IsMinimumPremiumDeposit, coverage.IsMinPremiumDeposit);
            facade.SetConcept(CompanyRuleConceptCoverage.FirstRiskTypeCode, coverage.FirstRiskType == null ? (int?)null : (int)coverage.FirstRiskType.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CalculationTypeCode, coverage.CalculationType == null ? (int?)null : (int)coverage.CalculationType.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.MultirriesgoVAseg, coverage.DeclaredAmount == 0 ? (decimal?)null : coverage.DeclaredAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.DeclaredAmount, coverage.DeclaredAmount == 0 ? (decimal?)null : coverage.DeclaredAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.PremiumAmount, coverage.PremiumAmount == 0 ? (decimal?)null : coverage.PremiumAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.LimitAmount, coverage.LimitAmount == 0 ? (decimal?)null : coverage.LimitAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.SubLimitAmount, coverage.SubLimitAmount == 0 ? (decimal?)null : coverage.SubLimitAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.LimitInExcess, coverage.ExcessLimit == 0 ? (decimal?)null : coverage.ExcessLimit);
            facade.SetConcept(CompanyRuleConceptCoverage.LimitOccurrenceAmount, coverage.LimitOccurrenceAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.LimitClaimantAmount, coverage.LimitClaimantAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.AccumulatedLimitAmount, coverage.AccumulatedLimitAmount == 0 ? (decimal?)null : coverage.AccumulatedLimitAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.AccumulatedSubLimitAmount, coverage.AccumulatedSubLimitAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.CurrentFrom, coverage.CurrentFrom);
            facade.SetConcept(CompanyRuleConceptCoverage.RateTypeCode, coverage.RateType == null ? 0 : (int)coverage.RateType.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.Rate, coverage.Rate == null ? (decimal?)null : coverage.Rate.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CurrentTo, coverage.CurrentTo == null ? (DateTime?)null : coverage.CurrentTo.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.MainCoverageId, coverage.MainCoverageId == null ? (int?)null : coverage.MainCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.MainCoveragePercentage, coverage.MainCoveragePercentage == null ? (decimal?)null : coverage.MainCoveragePercentage);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageNumber, coverage.Number == 0 ? (int?)null : coverage.Number);
            facade.SetConcept(CompanyRuleConceptCoverage.RiskCoverageId, coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageStatusCode, coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageOriginalStatusCode, coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.ConditionText, coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptCoverage.MaxLiabilityAmount, coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.InsuredObjectAmount, coverage.InsuredObject.Amount);
            facade.SetConcept(CompanyRuleConceptCoverage.MinimumPremiumCoverage, coverage.MinimumPremiumCoverage);
            facade.SetConcept(CompanyRuleConceptCoverage.SubLineBusinessCode, coverage.SubLineBusiness.LineBusiness.Id);
            facade.SetConcept(CompanyRuleConceptCoverage.LineBusinessCode, coverage.SubLineBusiness.LineBusiness.Id);

            if (coverage.Deductible == null)
            {
                facade.SetConcept(CompanyRuleConceptCoverage.DeductId, null);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductRateTypeCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductRate, null);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductPremiumAmount, null);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductValue, null);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductUnitCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductSubjectCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.MinDeductValue, null);
                facade.SetConcept(CompanyRuleConceptCoverage.MinDeductUnitCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.MinDeductSubjectCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.MaxDeductValue, null);
                facade.SetConcept(CompanyRuleConceptCoverage.MaxDeductUnitCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.MaxDeductSubjectCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.CurrencyCode, null);
                facade.SetConcept(CompanyRuleConceptCoverage.AccDeductAmt, null);
            }
            else
            {
                facade.SetConcept(CompanyRuleConceptCoverage.DeductId, coverage.Deductible.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductRateTypeCode, (int)coverage.Deductible.RateType);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductRate, coverage.Deductible.Rate == null ? (decimal?)null : coverage.Deductible.Rate.Value);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductPremiumAmount, coverage.Deductible.DeductPremiumAmount);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductValue, coverage.Deductible.DeductValue);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductUnitCode, coverage.Deductible.DeductibleUnit == null ? (int?)null : coverage.Deductible.DeductibleUnit.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.DeductSubjectCode, coverage.Deductible.DeductibleSubject == null ? (int?)null : coverage.Deductible.DeductibleSubject.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.MinDeductValue, coverage.Deductible.MinDeductValue);
                facade.SetConcept(CompanyRuleConceptCoverage.MinDeductUnitCode, coverage.Deductible.MinDeductibleUnit == null ? (int?)null : coverage.Deductible.MinDeductibleUnit.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.MinDeductSubjectCode, coverage.Deductible.MinDeductibleSubject == null ? (int?)null : coverage.Deductible.MinDeductibleSubject.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.MaxDeductValue, coverage.Deductible.MaxDeductValue);
                facade.SetConcept(CompanyRuleConceptCoverage.MaxDeductUnitCode, coverage.Deductible.MaxDeductibleUnit == null ? (int?)null : coverage.Deductible.MaxDeductibleUnit.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.MaxDeductSubjectCode, coverage.Deductible.MaxDeductibleSubject == null ? (int?)null : coverage.Deductible.MaxDeductibleSubject.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.CurrencyCode, coverage.Deductible.Currency == null ? (int?)null : coverage.Deductible.Currency.Id);
                facade.SetConcept(CompanyRuleConceptCoverage.AccDeductAmt, coverage.Deductible.AccDeductAmt);
            }

            if (coverage.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in coverage.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptCoverage.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

    }
}
