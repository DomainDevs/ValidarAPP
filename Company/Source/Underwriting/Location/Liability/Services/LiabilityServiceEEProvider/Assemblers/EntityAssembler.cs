using System;
using System.Linq;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {

        public static void CreateFacadeRiskLiability(Rules.Facade facade, CompanyLiabilityRisk LiabilityRisk)
        {
            facade.SetConcept(CompanyRuleConceptRisk.TempId, LiabilityRisk.Risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, LiabilityRisk.Risk.RiskId);
            facade.SetConcept(CompanyRuleConceptRisk.InsuredId, LiabilityRisk.Risk.MainInsured == null ? (int?)null : LiabilityRisk.Risk.MainInsured.IndividualId == 0 ? (int?)null : LiabilityRisk.Risk.MainInsured.IndividualId);
            facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, LiabilityRisk.Risk.MainInsured == null ? 0 : (int)LiabilityRisk.Risk.MainInsured.CustomerType);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, (int)LiabilityRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, LiabilityRisk.Risk.Status == null ? (int?)null : (int)LiabilityRisk.Risk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, LiabilityRisk.Risk.OriginalStatus == null ? (int?)null : (int)LiabilityRisk.Risk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, LiabilityRisk.Risk.Text == null ? string.Empty : LiabilityRisk.Risk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, LiabilityRisk.Risk.RatingZone == null ? (int?)null : LiabilityRisk.Risk.RatingZone.Id == 0 ? (int?)null : LiabilityRisk.Risk.RatingZone.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, LiabilityRisk.Risk.GroupCoverage == null ? (int?)null : LiabilityRisk.Risk.GroupCoverage.Id == 0 ? (int?)null : LiabilityRisk.Risk.GroupCoverage.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, LiabilityRisk.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, LiabilityRisk.Risk.LimitRc == null ? (int?)null : LiabilityRisk.Risk.LimitRc.Id == 0 ? (int?)null : LiabilityRisk.Risk.LimitRc.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, LiabilityRisk.Risk.LimitRc == null ? (decimal?)null : LiabilityRisk.Risk.LimitRc.LimitSum == 0 ? (decimal?)null : LiabilityRisk.Risk.LimitRc.LimitSum);
            facade.SetConcept(CompanyRuleConceptRisk.Apartment, LiabilityRisk.NomenclatureAddress == null ? 0 : LiabilityRisk.NomenclatureAddress.ApartmentOrOffice == null ? 0 : LiabilityRisk.NomenclatureAddress.ApartmentOrOffice.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CityCode, LiabilityRisk.City?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CountryCode, LiabilityRisk.City?.State?.Country?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.StateCode, LiabilityRisk.City?.State?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.RiskAge, LiabilityRisk.RiskAge);
            facade.SetConcept(CompanyRuleConceptRisk.EmlPercentage, LiabilityRisk.PML);
            facade.SetConcept(CompanyRuleConceptRisk.IsFacultative, LiabilityRisk.IsDeclarative);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, LiabilityRisk.Risk.Premium);

            facade.SetConcept(CompanyRuleConceptRisk.InsuredDocumentNumber, LiabilityRisk.Risk?.MainInsured?.IdentificationDocument?.Number);
            facade.SetConcept(CompanyRuleConceptRisk.TypeOfInsuredDocument, LiabilityRisk.Risk?.MainInsured?.IdentificationDocument?.DocumentType?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.TypeOfInsuredPerson, (int)LiabilityRisk.Risk?.MainInsured?.IndividualType);
            facade.SetConcept(CompanyRuleConceptRisk.AmountInsured, LiabilityRisk.Risk?.AmountInsured);
            facade.SetConcept(CompanyRuleConceptRisk.NameInsured, LiabilityRisk.Risk?.MainInsured?.Name);
            facade.SetConcept(CompanyRuleConceptRisk.RcRiskActivity, LiabilityRisk.Risk?.RiskActivity?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.InsuranceMode, LiabilityRisk.AssuranceMode?.Id);

            facade.SetConcept(RuleConceptRisk.Beneficiaries, LiabilityRisk.Risk.Beneficiaries);

            if (LiabilityRisk.Risk?.Beneficiaries.Count >= 1)
            {
                foreach (CompanyBeneficiary companyBeneficiary in LiabilityRisk.Risk?.Beneficiaries)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.NumberDocumentBeneficiary, companyBeneficiary.IdentificationDocument?.Number);
                    facade.SetConcept(CompanyRuleConceptRisk.NameBeneficiary, companyBeneficiary.Name);
                    facade.SetConcept(CompanyRuleConceptRisk.TypeOfBeneficiaryDocument, companyBeneficiary.IdentificationDocument?.DocumentType?.Id);
                }
            }


            if (LiabilityRisk.Risk.MainInsured != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.IsInsuredPayer, LiabilityRisk.Risk.MainInsured.IndividualId == LiabilityRisk.Risk.Policy.Holder.IndividualId);
                facade.SetConcept(RuleConceptRisk.IndividualInsured, LiabilityRisk.Risk.MainInsured.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.InsuredDocumentNumberOfTheBond, LiabilityRisk.Risk?.MainInsured.IdentificationDocument.Number);

                if (LiabilityRisk.Risk.MainInsured.IndividualType == IndividualType.Person && LiabilityRisk.Risk.MainInsured.BirthDate != null)
                {
                    int insuredAge = (DateTime.Today - LiabilityRisk.Risk.MainInsured.BirthDate.Value).Days / 365;
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, LiabilityRisk.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, LiabilityRisk.Risk.MainInsured.BirthDate);
                }

                if (LiabilityRisk.Risk.MainInsured.AssociationType != null && LiabilityRisk.Risk.MainInsured.AssociationType.Id != null && LiabilityRisk.Risk.MainInsured.AssociationType.Id > 0)
                    facade.SetConcept(RuleConceptRisk.InsuredAssociationType, LiabilityRisk.Risk.MainInsured.AssociationType.Id);
            }

            if (LiabilityRisk.Risk.Beneficiaries != null && LiabilityRisk.Risk.Beneficiaries.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryId, LiabilityRisk.Risk.Beneficiaries.First().IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryPercentage, LiabilityRisk.Risk.Beneficiaries.First().Participation);
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryTypeCode, (int)LiabilityRisk.Risk.Beneficiaries.First().BeneficiaryType.Id);
                facade.SetConcept(CompanyRuleConceptRisk.IndividualBeneficiary, LiabilityRisk.Risk.Beneficiaries.First().IndividualId);
            }


            if (LiabilityRisk.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in LiabilityRisk.Risk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }

            if (LiabilityRisk.Risk.Coverages?.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.LimitAmount, LiabilityRisk.Risk.Coverages.Sum(x => x.LimitAmount));
            }
            facade.SetConcept(RuleConceptRisk.InsuredDocumentNumber, LiabilityRisk.Risk?.MainInsured?.IdentificationDocument?.Number);
            facade.SetConcept(RuleConceptRisk.TypeOfInsuredDocument, LiabilityRisk.Risk?.MainInsured?.IdentificationDocument?.DocumentType?.Id);
            facade.SetConcept(RuleConceptRisk.TypeOfInsuredPerson, (int)LiabilityRisk.Risk?.MainInsured?.IndividualType);
            facade.SetConcept(RuleConceptRisk.AmountInsured, LiabilityRisk.Risk?.AmountInsured);
            facade.SetConcept(RuleConceptRisk.NameInsured, LiabilityRisk.Risk?.MainInsured?.Name);
            if (LiabilityRisk.Risk?.Beneficiaries.Count >= 1)
            {
                foreach (CompanyBeneficiary companyBeneficiary in LiabilityRisk.Risk?.Beneficiaries)
                {
                    facade.SetConcept(RuleConceptRisk.NumberDocumentBeneficiary, companyBeneficiary.IdentificationDocument.Number);
                    facade.SetConcept(RuleConceptRisk.NameBeneficiary, companyBeneficiary.Name);
                    facade.SetConcept(RuleConceptRisk.TypeOfBeneficiaryDocument, companyBeneficiary?.IdentificationDocument?.DocumentType?.Id);
                }
            }
        }

        public static void CreateFacadeCoverage(Rules.Facade facade, CompanyCoverage coverage)
        {
            facade.SetConcept(CompanyRuleConceptCoverage.RiskId, coverage.RiskCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageId, coverage.Id);
            facade.SetConcept(CompanyRuleConceptCoverage.IsDeclarative, coverage.IsDeclarative);
            facade.SetConcept(CompanyRuleConceptCoverage.IsPrimary, coverage.IsPrimary);
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
            facade.SetConcept(CompanyRuleConceptCoverage.Rate, coverage.Rate.GetValueOrDefault());
            facade.SetConcept(CompanyRuleConceptCoverage.CurrentTo, coverage.CurrentTo == null ? (DateTime?)null : coverage.CurrentTo.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.MainCoverageId, coverage.MainCoverageId == null ? (int?)null : coverage.MainCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.MainCoveragePercentage, coverage.MainCoveragePercentage == null ? (decimal?)null : coverage.MainCoveragePercentage);
            //EndorsementId = coverage.Risk.Policy.Endorsement.Id == 0 ? (int?)null : coverage.Risk.Policy.Endorsement.Id, //estaba comentada en el codigo antes de la modificacion
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageNumber, coverage.Number == 0 ? (int?)null : coverage.Number);
            facade.SetConcept(CompanyRuleConceptCoverage.RiskCoverageId, coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageStatusCode, coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageOriginalStatusCode, coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.ConditionText, coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptCoverage.MaxLiabilityAmount, coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.MinimumPremiumCoverage, coverage.MinimumPremiumCoverage);
            facade.SetConcept(CompanyRuleConceptCoverage.DaysVigencyCoverage, (coverage.CurrentTo.Value - coverage.CurrentFrom).Days);
            //facade.SetConcept(CompanyRuleConceptCoverage.InsuredObjectAmount, coverage.InsuredObject.Amount);

            if (coverage.Deductible == null)
            {
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
