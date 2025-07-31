using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static void CreateFacadeRiskContract(Rules.Facade facade, CompanyContract contract)
        {

            var policy = contract.Risk.Policy;
            facade.SetConcept(RuleConceptRisk.TempId, policy.Endorsement.TemporalId);
            facade.SetConcept(RuleConceptRisk.RiskId, contract.Risk.RiskId);
            facade.SetConcept(RuleConceptRisk.InsuredId, contract.Risk.MainInsured?.IndividualId);
            facade.SetConcept(RuleConceptRisk.CustomerTypeCode, contract.Risk.MainInsured == null ? 0 : (int)contract.Risk.MainInsured.CustomerType);
            facade.SetConcept(RuleConceptRisk.CoveredRiskTypeCode, (int)policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(RuleConceptRisk.RiskStatusCode, contract.Risk.Status == null ? (int?)null : (int)contract.Risk.Status);
            facade.SetConcept(RuleConceptRisk.RiskOriginalStatusCode, contract.Risk.OriginalStatus == null ? (int?)null : (int)contract.Risk.OriginalStatus);
            facade.SetConcept(RuleConceptRisk.ConditionText, contract.Risk.Text == null ? string.Empty : contract.Risk.Text.TextBody);
            facade.SetConcept(RuleConceptRisk.RatingZoneCode, contract.Risk.RatingZone == null ? (int?)null : contract.Risk.RatingZone.Id == 0 ? (int?)null : contract.Risk.RatingZone.Id);
            facade.SetConcept(RuleConceptRisk.CoverageGroupId, contract.Risk.GroupCoverage == null ? (int?)null : contract.Risk.GroupCoverage.Id == 0 ? (int?)null : contract.Risk.GroupCoverage.Id);
            facade.SetConcept(RuleConceptRisk.OperationId, contract.Risk.Id);
            facade.SetConcept(RuleConceptRisk.LimitsRcCode, contract.Risk.LimitRc == null ? (int?)null : contract.Risk.LimitRc.Id == 0 ? (int?)null : contract.Risk.LimitRc.Id);
            facade.SetConcept(RuleConceptRisk.LimitsRcSum, contract.Risk.LimitRc == null ? (decimal?)null : contract.Risk.LimitRc.LimitSum == 0 ? (decimal?)null : contract.Risk.LimitRc.LimitSum);
            facade.SetConcept(RuleConceptRisk.SuretyContractType, contract.ContractType.Id);
            facade.SetConcept(RuleConceptRisk.IndividualId, contract.Contractor == null ? 0 : contract.Contractor.IndividualId);
            facade.SetConcept(RuleConceptRisk.SuretyContractCategoriesCode, contract.Class == null ? (int?)null : contract.Class.Id == 0 ? (int?)null : contract.Class.Id);
            facade.SetConcept(RuleConceptRisk.ContractAmount, contract.Value == null ? 0.0M : contract.Value.Value);
            facade.SetConcept(RuleConceptRisk.OperatingPile, contract.Aggregate);
            facade.SetConcept(RuleConceptRisk.OperatingQuotaAmount, contract.OperatingQuota == null ? decimal.Zero : contract.OperatingQuota.Amount);
            facade.SetConcept(CompanyRuleConceptRisk.CoveragesCount, contract.Risk.Coverages?.Count);
            facade.SetConcept(CompanyRuleConceptRisk.Coverages, contract.Risk.Coverages);
            facade.SetConcept(CompanyRuleConceptRisk.IsFacultative, contract.Isfacultative);
            facade.SetConcept(CompanyRuleConceptRisk.ContractorId, contract.Contractor?.IndividualId);
            facade.SetConcept(CompanyRuleConceptRisk.ContractorInsuredCode, contract.Contractor?.InsuredId);
            facade.SetConcept(CompanyRuleConceptRisk.SinesterCount, contract.Contractor?.SinisterCount);
            facade.SetConcept(CompanyRuleConceptRisk.TechnicalCard, contract.Contractor?.TechnicalCard);
            facade.SetConcept(CompanyRuleConceptRisk.ContractorAssociationType, contract?.Contractor.AssociationTypeId);
            facade.SetConcept(CompanyRuleConceptRisk.IsConsortium, contract?.Contractor.IsConsortium);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, contract.Risk.Premium);
            facade.SetConcept(CompanyRuleConceptRisk.Guarantees, contract.Guarantees);
            facade.SetConcept(CompanyRuleConceptRisk.InsuredValueGuarantee, contract.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.AppraisalAmount);
            facade.SetConcept(CompanyRuleConceptRisk.OpenGuarantee, !(contract.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.IsCloseInd));
            facade.SetConcept(CompanyRuleConceptRisk.GuaranteeStatus, contract.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.Status?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.GuaranteeId, contract.Guarantees?.FirstOrDefault()?.InsuredGuarantee?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CityCode, contract.City?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CountryCode, contract.Country?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.StateCode, contract.State?.Id);


            if (contract.Risk.MainInsured != null)
            {
                facade.SetConcept(RuleConceptRisk.IsInsuredPayer, contract.Risk.MainInsured.IndividualId == policy.Holder.IndividualId);
                facade.SetConcept(RuleConceptRisk.IndividualInsured, contract.Risk.MainInsured.IndividualId);
                if (contract.Risk.MainInsured.IndividualType == IndividualType.Person)
                {
                    if (contract.Risk.MainInsured?.BirthDate != null)
                    {
                        int insuredAge = (DateTime.Today - contract.Risk.MainInsured.BirthDate.Value).Days / 365;
                        facade.SetConcept(RuleConceptRisk.InsuredAge, insuredAge);
                    }
                    facade.SetConcept(RuleConceptRisk.InsuredGender, contract.Risk.MainInsured?.Gender);
                    facade.SetConcept(RuleConceptRisk.InsuredBirthDate, contract.Risk.MainInsured?.BirthDate);
                }

                if(contract.Risk.MainInsured.AssociationType != null && contract.Risk.MainInsured.AssociationType.Id != null && contract.Risk.MainInsured.AssociationType.Id > 0)
                    facade.SetConcept(RuleConceptRisk.InsuredAssociationType, contract.Risk.MainInsured.AssociationType.Id);
                
            }

            facade.SetConcept(RuleConceptRisk.Beneficiaries, contract.Risk.Beneficiaries);
            if (contract.Risk.Beneficiaries != null && contract.Risk.Beneficiaries.Count > 0)
            {
                facade.SetConcept(RuleConceptRisk.BeneficiaryId, contract.Risk.Beneficiaries.First().IndividualId);
                facade.SetConcept(RuleConceptRisk.BeneficiaryPercentage, contract.Risk.Beneficiaries.First().Participation);
                facade.SetConcept(RuleConceptRisk.BeneficiaryTypeCode, contract.Risk.Beneficiaries.First().BeneficiaryType.Id);
                facade.SetConcept(RuleConceptRisk.IndividualBeneficiary, contract.Risk.Beneficiaries.First().IndividualId);
            }

            if (contract.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in contract.Risk.DynamicProperties)
                {
                    facade.SetConcept(RuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }

            facade.SetConcept(CompanyRuleConceptRisk.PileAmount, contract.Aggregate);

            if (contract.Risk.Coverages?.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.LimitPileAmount, contract.Risk.Coverages.Sum(x => x.SubLimitAmount) + contract.Aggregate);
                facade.SetConcept(CompanyRuleConceptRisk.LimitAmount, contract.Risk.Coverages.Sum(x => x.LimitAmount));
            }

            if (contract.RiskSuretyPost != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.ContractDate, contract.RiskSuretyPost.ContractDate);
                facade.SetConcept(CompanyRuleConceptRisk.DeliveryDate, contract.RiskSuretyPost.IssueDate);
            }
            facade.SetConcept(RuleConceptRisk.InsuredDocumentNumber, contract.Risk?.MainInsured?.IdentificationDocument?.Number);
            facade.SetConcept(RuleConceptRisk.TypeOfInsuredDocument, contract.Risk?.MainInsured?.IdentificationDocument?.DocumentType?.Id);
            facade.SetConcept(RuleConceptRisk.TypeOfTheBondDocument, contract.Contractor?.IdentificationDocument?.DocumentType?.Id);
            facade.SetConcept(RuleConceptRisk.TypeOfInsuredPerson, (int)contract.Risk?.MainInsured?.IndividualType);
            facade.SetConcept(RuleConceptRisk.AmountInsured, contract.Risk?.AmountInsured);
            facade.SetConcept(RuleConceptRisk.NameInsured, contract.Risk?.MainInsured?.Name);
            if (contract.Risk?.Beneficiaries.Count >= 1)
            {
                foreach (CompanyBeneficiary companyBeneficiary in contract.Risk?.Beneficiaries)
                {
                    facade.SetConcept(RuleConceptRisk.NumberDocumentBeneficiary, companyBeneficiary.IdentificationDocument.Number);
                    facade.SetConcept(RuleConceptRisk.NameBeneficiary, companyBeneficiary.Name);
                    facade.SetConcept(RuleConceptRisk.TypeOfBeneficiaryDocument, companyBeneficiary?.IdentificationDocument?.DocumentType?.Id);

                }
            }
            facade.SetConcept(CompanyRuleConceptRisk.IndividualOfTheBond, contract.Contractor.IndividualId);
            facade.SetConcept(CompanyRuleConceptRisk.InsuredDocumentNumberOfTheBond, contract?.Contractor?.IdentificationDocument.Number);
            facade.SetConcept(CompanyRuleConceptRisk.InsuredNameOfTheBond, contract?.Contractor?.Name);
            
            if(contract.Contractor.AssociationType != null && contract.Contractor.AssociationType.Id != null && contract.Contractor.AssociationType.Id >0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.ContractorAssociationType, contract.Contractor.AssociationTypeId);
            }
        }

        public static void CreateFacadeCoverage(Rules.Facade facade, CompanyCoverage coverage)
        {
            facade.SetConcept(RuleConceptCoverage.CoverageId, coverage.Id);
            facade.SetConcept(RuleConceptCoverage.IsPrimary, coverage.IsPrimary);
            facade.SetConcept(RuleConceptCoverage.IsDeclarative, coverage.IsDeclarative);
            facade.SetConcept(RuleConceptCoverage.IsMinimumPremiumDeposit, coverage.IsMinPremiumDeposit);
            facade.SetConcept(RuleConceptCoverage.FirstRiskTypeCode, coverage.FirstRiskType == null ? (int?)null : (int)coverage.FirstRiskType.Value);
            facade.SetConcept(RuleConceptCoverage.CalculationTypeCode, coverage.CalculationType == null ? (int?)null : (int)coverage.CalculationType.Value);
            facade.SetConcept(RuleConceptCoverage.DeclaredAmount, coverage.DeclaredAmount == 0 ? (decimal?)null : coverage.DeclaredAmount);
            facade.SetConcept(RuleConceptCoverage.PremiumAmount, coverage.PremiumAmount == 0 ? (decimal?)null : coverage.PremiumAmount);
            facade.SetConcept(RuleConceptCoverage.LimitAmount, coverage.LimitAmount == 0 ? (decimal?)null : coverage.LimitAmount);
            facade.SetConcept(RuleConceptCoverage.SubLimitAmount, coverage.SubLimitAmount == 0 ? (decimal?)null : coverage.SubLimitAmount);
            facade.SetConcept(RuleConceptCoverage.EndorsementLimitAmount, coverage.EndorsementLimitAmount );
            facade.SetConcept(RuleConceptCoverage.EndorsementSubLimitAmount, coverage.EndorsementSublimitAmount );
            facade.SetConcept(RuleConceptCoverage.LimitInExcess, coverage.ExcessLimit == 0 ? (decimal?)null : coverage.ExcessLimit);
            facade.SetConcept(RuleConceptCoverage.LimitOccurrenceAmount, coverage.LimitOccurrenceAmount);
            facade.SetConcept(RuleConceptCoverage.LimitClaimantAmount, coverage.LimitClaimantAmount);
            facade.SetConcept(RuleConceptCoverage.AccumulatedLimitAmount, coverage.AccumulatedLimitAmount == 0 ? (decimal?)null : coverage.AccumulatedLimitAmount);
            facade.SetConcept(RuleConceptCoverage.AccumulatedSubLimitAmount, coverage.AccumulatedSubLimitAmount);
            facade.SetConcept(RuleConceptCoverage.CurrentFrom, coverage.CurrentFrom);
            facade.SetConcept(RuleConceptCoverage.RateTypeCode, coverage.RateType == null ? 0 : (int)coverage.RateType.Value);
            facade.SetConcept(RuleConceptCoverage.Rate, coverage.Rate == null ? (decimal?)null : coverage.Rate.Value);
            facade.SetConcept(RuleConceptCoverage.CurrentTo, coverage.CurrentTo == null ? (DateTime?)null : coverage.CurrentTo.Value);
            facade.SetConcept(RuleConceptCoverage.MainCoverageId, coverage.MainCoverageId == null ? (int?)null : coverage.MainCoverageId);
            facade.SetConcept(RuleConceptCoverage.MainCoveragePercentage, coverage.MainCoveragePercentage == null ? (decimal?)null : coverage.MainCoveragePercentage);
            facade.SetConcept(RuleConceptCoverage.CoverageNumber, coverage.Number == 0 ? (int?)null : coverage.Number);
            facade.SetConcept(RuleConceptCoverage.RiskCoverageId, coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(RuleConceptCoverage.CoverageStatusCode, coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(RuleConceptCoverage.CoverageOriginalStatusCode, coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);
            facade.SetConcept(RuleConceptCoverage.ConditionText, coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(RuleConceptCoverage.MaxLiabilityAmount, coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(RuleConceptCoverage.MinimumPremiumCoverage, coverage.MinimumPremiumCoverage);
            facade.SetConcept(RuleConceptCoverage.DaysVigencyCoverage, (coverage.CurrentTo.Value - coverage.CurrentFrom).Days);
            facade.SetConcept(CompanyRuleConceptCoverage.PercentageContract, coverage.ContractAmountPercentage);
            facade.SetConcept(CompanyRuleConceptCoverage.IsEnabledMinimumPremium, coverage.IsEnabledMinimumPremium);

            if (coverage.Deductible == null)
            {
                facade.SetConcept(RuleConceptCoverage.DeductRateTypeCode, null);
                facade.SetConcept(RuleConceptCoverage.DeductRate, null);
                facade.SetConcept(RuleConceptCoverage.DeductPremiumAmount, null);
                facade.SetConcept(RuleConceptCoverage.DeductValue, null);
                facade.SetConcept(RuleConceptCoverage.DeductUnitCode, null);
                facade.SetConcept(RuleConceptCoverage.DeductSubjectCode, null);
                facade.SetConcept(RuleConceptCoverage.MinDeductValue, null);
                facade.SetConcept(RuleConceptCoverage.MinDeductUnitCode, null);
                facade.SetConcept(RuleConceptCoverage.MinDeductSubjectCode, null);
                facade.SetConcept(RuleConceptCoverage.MaxDeductValue, null);
                facade.SetConcept(RuleConceptCoverage.MaxDeductUnitCode, null);
                facade.SetConcept(RuleConceptCoverage.MaxDeductSubjectCode, null);
                facade.SetConcept(RuleConceptCoverage.CurrencyCode, null);
                facade.SetConcept(RuleConceptCoverage.AccDeductAmt, null);
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