using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;

using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Utilities.RulesEngine;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        
        public static void CreateFacadeRiskJudgement(Rules.Facade facade, CompanyJudgement judgement)
        {
            facade.SetConcept(RuleConceptRisk.TempId, judgement.Risk.Policy.Endorsement != null ? judgement.Risk.Policy.Endorsement.TemporalId : 0);
            facade.SetConcept(RuleConceptRisk.RiskId, judgement.Risk.RiskId);
            facade.SetConcept(RuleConceptRisk.InsuredId, judgement.Risk.MainInsured == null ? (int?)null : judgement.Risk.MainInsured.IndividualId == 0 ? (int?)null : judgement.Risk.MainInsured.IndividualId);
            facade.SetConcept(RuleConceptRisk.CustomerTypeCode, judgement.Risk.MainInsured == null ? 0 : (int)judgement.Risk.MainInsured.CustomerType);
            facade.SetConcept(RuleConceptRisk.CoveredRiskTypeCode, judgement.Risk.Policy.Product != null ? (int)judgement.Risk.Policy.Product.CoveredRisk.CoveredRiskType : 0);
            facade.SetConcept(RuleConceptRisk.RiskStatusCode, judgement.Risk.Status == null ? (int?)null : (int)judgement.Risk.Status);
            facade.SetConcept(RuleConceptRisk.RiskOriginalStatusCode, judgement.Risk.OriginalStatus == null ? (int?)null : (int)judgement.Risk.OriginalStatus);
            facade.SetConcept(RuleConceptRisk.ConditionText, judgement.Risk.Text == null ? string.Empty : judgement.Risk.Text.TextBody);
            facade.SetConcept(RuleConceptRisk.RatingZoneCode, judgement.Risk.RatingZone == null ? (int?)null : judgement.Risk.RatingZone.Id == 0 ? (int?)null : judgement.Risk.RatingZone.Id);
            facade.SetConcept(RuleConceptRisk.CoverageGroupId, judgement.Risk.GroupCoverage == null ? (int?)null : judgement.Risk.GroupCoverage.Id == 0 ? (int?)null : judgement.Risk.GroupCoverage.Id);
            facade.SetConcept(RuleConceptRisk.OperationId, judgement.Risk.Id);
            facade.SetConcept(RuleConceptRisk.LimitsRcCode, judgement.Risk.LimitRc == null ? (int?)null : judgement.Risk.LimitRc.Id == 0 ? (int?)null : judgement.Risk.LimitRc.Id);
            facade.SetConcept(RuleConceptRisk.LimitsRcSum, judgement.Risk.LimitRc == null ? (decimal?)null : judgement.Risk.LimitRc.LimitSum == 0 ? (decimal?)null : judgement.Risk.LimitRc.LimitSum);
            facade.SetConcept(RuleConceptRisk.JudRiskActivity, judgement.Risk?.RiskActivity?.Id);

            facade.SetConcept(CompanyRuleConceptRisk.IsFacultative, judgement.Risk?.IsFacultative);
            facade.SetConcept(RuleConceptRisk.CaucionCity, judgement.City.Id);
            facade.SetConcept(RuleConceptRisk.CityCode, judgement.City?.Id);
            facade.SetConcept(RuleConceptRisk.StateCode, judgement.City?.State?.Id);
            facade.SetConcept(RuleConceptRisk.CountryCode, judgement.City?.State?.Country.Id);
            facade.SetConcept(RuleConceptRisk.CaucionCourtType, judgement.Court.Id);
            facade.SetConcept(RuleConceptRisk.CaucionDepartment, judgement.City.State.Id);
            facade.SetConcept(RuleConceptRisk.CaucionHolderActAs, (int)judgement.HolderActAs);
            facade.SetConcept(RuleConceptRisk.CaucionInsuredActAs, (int)judgement.InsuredActAs);
            facade.SetConcept(RuleConceptRisk.CaucionInsuredValue, judgement.InsuredValue);
            facade.SetConcept(RuleConceptRisk.CaucionSettledNumber, judgement.SettledNumber);
            facade.SetConcept(RuleConceptRisk.Guarantees, judgement.Guarantees);
            facade.SetConcept(RuleConceptRisk.InsuredValueGuarantee, judgement.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.InsuranceAmount);
            facade.SetConcept(RuleConceptRisk.OpenGuarantee, judgement.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.ClosedInd);
            facade.SetConcept(RuleConceptRisk.GuaranteeStatus, judgement.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.Status?.Id);
            facade.SetConcept(RuleConceptRisk.GuaranteeId, judgement.Guarantees?.FirstOrDefault()?.Id);

            facade.SetConcept(RuleConceptRisk.InsuredDocumentNumber, judgement.Risk?.MainInsured?.IdentificationDocument?.Number);
            facade.SetConcept(RuleConceptRisk.TypeOfInsuredDocument, judgement.Risk?.MainInsured?.IdentificationDocument?.DocumentType?.Id);
            facade.SetConcept(RuleConceptRisk.TypeOfInsuredPerson, (int)judgement.Risk?.MainInsured?.IndividualType);
            facade.SetConcept(RuleConceptRisk.AmountInsured, judgement.Risk?.AmountInsured);
            facade.SetConcept(RuleConceptRisk.NameInsured, judgement.Risk?.MainInsured?.Name);
            facade.SetConcept(CompanyRuleConceptRisk.IsFacultative, judgement.Risk?.IsFacultative);
            facade.SetConcept(RuleConceptRisk.Beneficiaries, judgement.Risk.Beneficiaries);
            if (judgement.Risk?.Beneficiaries.Count >= 1)
            {
                foreach (CompanyBeneficiary companyBeneficiary in judgement.Risk?.Beneficiaries)
                {
                    facade.SetConcept(RuleConceptRisk.NumberDocumentBeneficiary, companyBeneficiary.IdentificationDocument.Number);
                    facade.SetConcept(RuleConceptRisk.NameBeneficiary, companyBeneficiary.Name);
                    facade.SetConcept(RuleConceptRisk.TypeOfBeneficiaryDocument, companyBeneficiary?.IdentificationDocument?.DocumentType?.Id);
                }
            }
            


            if (judgement.Risk.MainInsured != null)
            {
                if (judgement.Risk.Policy.Holder != null)
                {
                    facade.SetConcept(RuleConceptRisk.IsInsuredPayer, judgement.Risk.MainInsured.IndividualId == judgement.Risk.Policy.Holder.IndividualId);
                }

                if (judgement.Risk.MainInsured.IndividualType == IndividualType.Person)
                {
                    int insuredAge = 0;

                    if (judgement.Risk.MainInsured.BirthDate.GetValueOrDefault() > DateTime.MinValue)
                    {
                        insuredAge = (DateTime.Today - judgement.Risk.MainInsured.BirthDate.Value).Days / 365;
                    }

                    facade.SetConcept(RuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(RuleConceptRisk.InsuredGender, judgement.Risk.MainInsured.Gender);
                    facade.SetConcept(RuleConceptRisk.InsuredBirthDate, judgement.Risk.MainInsured.BirthDate);
                }
                facade.SetConcept(RuleConceptRisk.IndividualInsured, judgement.Risk.MainInsured.IndividualId);
                facade.SetConcept(RuleConceptRisk.InsuredDocumentNumberOfTheBond, judgement.Risk.MainInsured.IdentificationDocument.Number);

                if (judgement.Risk.MainInsured.AssociationType != null && judgement.Risk.MainInsured.AssociationType.Id != null && judgement.Risk.MainInsured.AssociationType.Id > 0)
                    facade.SetConcept(RuleConceptRisk.InsuredAssociationType, judgement.Risk.MainInsured.AssociationType.Id);

            }

            if (judgement.Risk.Beneficiaries != null && judgement.Risk.Beneficiaries.Count > 0)
            {
                facade.SetConcept(RuleConceptRisk.BeneficiaryId, judgement.Risk?.Beneficiaries?.FirstOrDefault().IndividualId);
                facade.SetConcept(RuleConceptRisk.BeneficiaryPercentage, judgement.Risk?.Beneficiaries?.FirstOrDefault().Participation);
                facade.SetConcept(RuleConceptRisk.BeneficiaryTypeCode, judgement.Risk?.Beneficiaries?.FirstOrDefault().BeneficiaryType.Id);
                facade.SetConcept(RuleConceptRisk.IndividualBeneficiary, judgement.Risk?.Beneficiaries?.FirstOrDefault().IndividualId);
                facade.SetConcept(RuleConceptRisk.NumberDocumentBeneficiary, judgement.Risk?.Beneficiaries?.FirstOrDefault().IdentificationDocument.Number);
                facade.SetConcept(RuleConceptRisk.NameBeneficiary, judgement.Risk?.Beneficiaries?.FirstOrDefault().Name);
            }

            
            if (judgement.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in judgement.Risk.DynamicProperties)
                {
                    facade.SetConcept(RuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

        public static void CreateFacadeCoverage(Rules.Facade facade, CompanyCoverage coverage)
        {

            facade.SetConcept(RuleConceptCoverage.CoverageId,coverage.Id);
            facade.SetConcept(RuleConceptCoverage.IsDeclarative,coverage.IsDeclarative);
            facade.SetConcept(RuleConceptCoverage.IsPrimary, coverage.IsPrimary);
            facade.SetConcept(RuleConceptCoverage.IsMinimumPremiumDeposit,coverage.IsMinPremiumDeposit);
            facade.SetConcept(RuleConceptCoverage.FirstRiskTypeCode,coverage.FirstRiskType == null ? (int?)null : (int)coverage.FirstRiskType.Value);
            facade.SetConcept(RuleConceptCoverage.CalculationTypeCode,coverage.CalculationType == null ? (int?)null : (int)coverage.CalculationType.Value);
            facade.SetConcept(RuleConceptCoverage.DeclaredAmount,coverage.DeclaredAmount == 0 ? (decimal?)null : coverage.DeclaredAmount);
            facade.SetConcept(RuleConceptCoverage.PremiumAmount,coverage.PremiumAmount == 0 ? (decimal?)null : coverage.PremiumAmount);
            facade.SetConcept(RuleConceptCoverage.LimitAmount,coverage.LimitAmount == 0 ? (decimal?)null : coverage.LimitAmount);
            facade.SetConcept(RuleConceptCoverage.SubLimitAmount,coverage.SubLimitAmount == 0 ? (decimal?)null : coverage.SubLimitAmount);
            facade.SetConcept(RuleConceptCoverage.EndorsementLimitAmount, coverage.EndorsementLimitAmount);
            facade.SetConcept(RuleConceptCoverage.EndorsementSubLimitAmount, coverage.EndorsementSublimitAmount);
            facade.SetConcept(RuleConceptCoverage.LimitInExcess,coverage.ExcessLimit == 0 ? (decimal?)null : coverage.ExcessLimit);
            facade.SetConcept(RuleConceptCoverage.LimitOccurrenceAmount,coverage.LimitOccurrenceAmount);
            facade.SetConcept(RuleConceptCoverage.LimitClaimantAmount,coverage.LimitClaimantAmount);
            facade.SetConcept(RuleConceptCoverage.AccumulatedLimitAmount,coverage.AccumulatedLimitAmount == 0 ? (decimal?)null : coverage.AccumulatedLimitAmount);
            facade.SetConcept(RuleConceptCoverage.AccumulatedSubLimitAmount,coverage.AccumulatedSubLimitAmount);
            facade.SetConcept(RuleConceptCoverage.CurrentFrom,coverage.CurrentFrom);
            facade.SetConcept(RuleConceptCoverage.RateTypeCode,coverage.RateType == null ? 0 : (int)coverage.RateType.Value);
            facade.SetConcept(RuleConceptCoverage.Rate,coverage.Rate == null ? (decimal?)null : coverage.Rate.Value);
            facade.SetConcept(RuleConceptCoverage.CurrentTo,coverage.CurrentTo == null ? (DateTime?)null : coverage.CurrentTo.Value);
            facade.SetConcept(RuleConceptCoverage.MainCoverageId,coverage.MainCoverageId == null ? (int?)null : coverage.MainCoverageId);
            facade.SetConcept(RuleConceptCoverage.MainCoveragePercentage,coverage.MainCoveragePercentage == null ? (decimal?)null : coverage.MainCoveragePercentage);
            facade.SetConcept(RuleConceptCoverage.CoverageNumber,coverage.Number == 0 ? (int?)null : coverage.Number);
            facade.SetConcept(RuleConceptCoverage.RiskCoverageId,coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(RuleConceptCoverage.CoverageStatusCode,coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(RuleConceptCoverage.CoverageOriginalStatusCode,coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);
            facade.SetConcept(RuleConceptCoverage.ConditionText,coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(RuleConceptCoverage.MaxLiabilityAmount,coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(RuleConceptCoverage.MinimumPremiumCoverage,coverage.MinimumPremiumCoverage);
            facade.SetConcept(RuleConceptCoverage.DaysVigencyCoverage, (coverage.CurrentTo.Value - coverage.CurrentFrom).Days);



            if (coverage.Deductible == null)
            {
                facade.SetConcept(RuleConceptCoverage.DeductRateTypeCode,null);
                facade.SetConcept(RuleConceptCoverage.DeductRate,null);
                facade.SetConcept(RuleConceptCoverage.DeductPremiumAmount,null);
                facade.SetConcept(RuleConceptCoverage.DeductValue,null);
                facade.SetConcept(RuleConceptCoverage.DeductUnitCode,null);
                facade.SetConcept(RuleConceptCoverage.DeductSubjectCode,null);
                facade.SetConcept(RuleConceptCoverage.MinDeductValue,null);
                facade.SetConcept(RuleConceptCoverage.MinDeductUnitCode,null);
                facade.SetConcept(RuleConceptCoverage.MinDeductSubjectCode,null);
                facade.SetConcept(RuleConceptCoverage.MaxDeductValue,null);
                facade.SetConcept(RuleConceptCoverage.MaxDeductUnitCode,null);
                facade.SetConcept(RuleConceptCoverage.MaxDeductSubjectCode,null);
                facade.SetConcept(RuleConceptCoverage.CurrencyCode,null);
                facade.SetConcept(RuleConceptCoverage.AccDeductAmt,null);
            }
            else
            {
                facade.SetConcept(RuleConceptCoverage.DeductId, coverage.Deductible.Id);
                facade.SetConcept(RuleConceptCoverage.DeductRateTypeCode, (int)coverage.Deductible.RateType);
                facade.SetConcept(RuleConceptCoverage.DeductRate, coverage.Deductible.Rate == null ? (decimal?)null : coverage.Deductible.Rate.Value);
                facade.SetConcept(RuleConceptCoverage.DeductPremiumAmount, coverage.Deductible.DeductPremiumAmount);
                facade.SetConcept(RuleConceptCoverage.DeductValue, coverage.Deductible.DeductValue);
                facade.SetConcept(RuleConceptCoverage.DeductUnitCode, coverage.Deductible.DeductibleUnit == null ? (int?)null : coverage.Deductible.DeductibleUnit.Id);
                facade.SetConcept(RuleConceptCoverage.DeductSubjectCode, coverage.Deductible.DeductibleSubject == null ? (int?)null : coverage.Deductible.DeductibleSubject.Id);
                facade.SetConcept(RuleConceptCoverage.MinDeductValue, coverage.Deductible.MinDeductValue);
                facade.SetConcept(RuleConceptCoverage.MinDeductUnitCode, coverage.Deductible.MinDeductibleUnit == null ? (int?)null : coverage.Deductible.MinDeductibleUnit.Id);
                facade.SetConcept(RuleConceptCoverage.MinDeductSubjectCode, coverage.Deductible.MinDeductibleSubject == null ? (int?)null : coverage.Deductible.MinDeductibleSubject.Id);
                facade.SetConcept(RuleConceptCoverage.MaxDeductValue, coverage.Deductible.MaxDeductValue);
                facade.SetConcept(RuleConceptCoverage.MaxDeductUnitCode, coverage.Deductible.MaxDeductibleUnit == null ? (int?)null : coverage.Deductible.MaxDeductibleUnit.Id);
                facade.SetConcept(RuleConceptCoverage.MaxDeductSubjectCode, coverage.Deductible.MaxDeductibleSubject == null ? (int?)null : coverage.Deductible.MaxDeductibleSubject.Id);
                facade.SetConcept(RuleConceptCoverage.CurrencyCode, coverage.Deductible.Currency == null ? (int?)null : coverage.Deductible.Currency.Id);
                facade.SetConcept(RuleConceptCoverage.AccDeductAmt, coverage.Deductible.AccDeductAmt);
            }

            if (coverage.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in coverage.DynamicProperties)
                {
                    facade.SetConcept(RuleConceptCoverage.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }
    }
}
