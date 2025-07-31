using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Entities;
using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities;

using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Engine;
using Sistran.Core.Framework.Rules.Integration;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using Sistran.Company.Application.Utilities.RulesEngine;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {

        public static void CreateFacadeGeneral(Rules.Facade facade, CompanyPolicy CompanyPolicy)
        {
            facade.SetConcept(CompanyRuleConceptGeneral.TempId, CompanyPolicy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptGeneral.QuotationId, CompanyPolicy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptGeneral.DocumentNumber, CompanyPolicy.DocumentNumber == 0 ? (decimal?)null : CompanyPolicy.DocumentNumber);
            facade.SetConcept(CompanyRuleConceptGeneral.PrefixCode, CompanyPolicy.Prefix.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.EndorsementTypeCode, (int)CompanyPolicy.Endorsement.EndorsementType);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrencyCode, CompanyPolicy.ExchangeRate.Currency.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.UserId, CompanyPolicy.UserId);
            facade.SetConcept(CompanyRuleConceptGeneral.ExchangeRate, CompanyPolicy.ExchangeRate.BuyAmount);
            facade.SetConcept(CompanyRuleConceptGeneral.IssueDate, CompanyPolicy.IssueDate);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrentFrom, CompanyPolicy.CurrentFrom);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrentTo, CompanyPolicy.CurrentTo);
            facade.SetConcept(CompanyRuleConceptGeneral.BeginDate, DateTime.Now);
            // facade.SetConcept(CompanyRuleConceptGeneral.CommitDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.BillingDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.BillingGroupCode, CompanyPolicy.BillingGroup?.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.ProductId, CompanyPolicy.Product.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyId, CompanyPolicy.Endorsement.PolicyId == 0 ? (int?)null : CompanyPolicy.Endorsement.PolicyId);
            facade.SetConcept(CompanyRuleConceptGeneral.EndorsementId, CompanyPolicy.Endorsement.Id == 0 ? (int?)null : CompanyPolicy.Endorsement.Id);
            //facade.SetConcept(CompanyRuleConceptGeneral.TemporalTypeCode, (int)CompanyPolicy.TemporalType);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextId, CompanyPolicy.Text?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionText, CompanyPolicy.Text?.TextBody);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextObservations, CompanyPolicy.Text?.Observations);
            facade.SetConcept(CompanyRuleConceptGeneral.BusinessTypeCode, CompanyPolicy.BusinessType == null ? 0 : (int)CompanyPolicy.BusinessType);
            facade.SetConcept(CompanyRuleConceptGeneral.OperationId, CompanyPolicy.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyTypeCode, CompanyPolicy.PolicyType == null ? 0 : CompanyPolicy.PolicyType.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.RequestId, CompanyPolicy.Request == null ? (int?)null : CompanyPolicy.Request.Id == 0 ? (int?)null : CompanyPolicy.Request.Id);
            // facade.SetConcept(CompanyRuleConceptGeneral.EffectPeriod, CompanyPolicy.EffectPeriod);
            // facade.SetConcept(CompanyRuleConceptGeneral.IsRequest, CompanyPolicy.Request == null ? false : CompanyPolicy.Request.Id == 0 ? false : true);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentId, CompanyPolicy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimnaryAgentAgencyId, CompanyPolicy.Agencies.First(x => x.IsPrincipal).Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCode, CompanyPolicy.Agencies.First(x => x.IsPrincipal).Code);
            facade.SetConcept(CompanyRuleConceptGeneral.IsPrimary, CompanyPolicy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            facade.SetConcept(CompanyRuleConceptGeneral.ScriptId, CompanyPolicy.Product.ScriptId);
            // facade.SetConcept(CompanyRuleConceptGeneral.PreRuleSetId, CompanyPolicy.Product.PreRuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.RuleSetId, CompanyPolicy.Product.RuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.StandardCommissionPercentage, CompanyPolicy.Product.StandardCommissionPercentage);
            //  facade.SetConcept(CompanyRuleConceptGeneral.IsFlatRate, CompanyPolicy.Product.IsFlatRate);
            //  facade.SetConcept(CompanyRuleConceptGeneral.IsCollective, CompanyPolicy.Product.IsCollective);
            facade.SetConcept(CompanyRuleConceptGeneral.IsGreen, CompanyPolicy.Product.IsGreen);
            facade.SetConcept(CompanyRuleConceptGeneral.PaymentScheduleId, CompanyPolicy.PaymentPlan?.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.CalculateMinPremium, CompanyPolicy.CalculateMinPremium ?? false);

            if (CompanyPolicy.Summary != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.AmountInsured, CompanyPolicy.Summary.AmountInsured);
                facade.SetConcept(CompanyRuleConceptGeneral.Premium, CompanyPolicy.Summary.Premium);
                facade.SetConcept(CompanyRuleConceptGeneral.Expenses, CompanyPolicy.Summary.Expenses);
                facade.SetConcept(CompanyRuleConceptGeneral.Taxes, CompanyPolicy.Summary.Taxes);
                facade.SetConcept(CompanyRuleConceptGeneral.FullPremium, CompanyPolicy.Summary.FullPremium);
            }
            if (CompanyPolicy.Holder != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.PolicyHolderId, CompanyPolicy.Holder.IndividualId);
                facade.SetConcept(CompanyRuleConceptGeneral.CustomerTypeCode, (int)CompanyPolicy.Holder.CustomerType);

                if (CompanyPolicy.Holder.IdentificationDocument != null)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderIdentificationDocument, CompanyPolicy.Holder.IdentificationDocument.Number);
                }
                if (CompanyPolicy.Holder.CompanyName != null && CompanyPolicy.Holder.CompanyName.Address != null)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.MailAddressId, CompanyPolicy.Holder.CompanyName.Address.Id);
                }
                if (CompanyPolicy.Holder.IndividualType == IndividualType.Person)
                {
                    int holderAge = 0;

                    if (CompanyPolicy.Holder.BirthDate.GetValueOrDefault() != DateTime.MinValue)
                    {
                        holderAge = (DateTime.Today - CompanyPolicy.Holder.BirthDate.Value).Days / 365;
                    }

                    facade.SetConcept(CompanyRuleConceptGeneral.HolderAge, holderAge);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderBirthDate, CompanyPolicy.Holder.BirthDate);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderGender, CompanyPolicy.Holder.Gender);
                }
            }

            if (CompanyPolicy.Branch != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.BranchCode, CompanyPolicy.Branch.Id);
                facade.SetConcept(CompanyRuleConceptGeneral.SalePointCode, CompanyPolicy.Branch.SalePoints?.FirstOrDefault()?.Id);
            }

            if (CompanyPolicy.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in CompanyPolicy.DynamicProperties)
                {

                    if (dynamicConcept.Value != null && !string.IsNullOrEmpty(dynamicConcept.TypeName))
                    {
                        facade.SetConcept(CompanyRuleConceptGeneral.DynamicConcept(dynamicConcept.Id,dynamicConcept.EntityId), dynamicConcept.Value);
                    }
                }
            }

        }

        public static void CreateFacadeRiskFidelity(Rules.Facade facade, CompanyFidelityRisk FidelityRisk)
        {
            facade.SetConcept(CompanyRuleConceptRisk.TempId, FidelityRisk.Risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, FidelityRisk.Risk.RiskId);
            facade.SetConcept(CompanyRuleConceptRisk.InsuredId, FidelityRisk.Risk.MainInsured == null ? (int?)null : FidelityRisk.Risk.MainInsured.IndividualId == 0 ? (int?)null : FidelityRisk.Risk.MainInsured.IndividualId);
            facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, FidelityRisk.Risk.MainInsured == null ? 0 : (int)FidelityRisk.Risk.MainInsured.CustomerType);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, (int)FidelityRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, FidelityRisk.Risk.Status == null ? (int?)null : (int)FidelityRisk.Risk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, FidelityRisk.Risk.OriginalStatus == null ? (int?)null : (int)FidelityRisk.Risk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, FidelityRisk.Risk.Text == null ? string.Empty : FidelityRisk.Risk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, FidelityRisk.Risk.RatingZone == null ? (int?)null : FidelityRisk.Risk.RatingZone.Id == 0 ? (int?)null : FidelityRisk.Risk.RatingZone.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, FidelityRisk.Risk.GroupCoverage == null ? (int?)null : FidelityRisk.Risk.GroupCoverage.Id == 0 ? (int?)null : FidelityRisk.Risk.GroupCoverage.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, FidelityRisk.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, FidelityRisk.Risk.LimitRc == null ? (int?)null : FidelityRisk.Risk.LimitRc.Id == 0 ? (int?)null : FidelityRisk.Risk.LimitRc.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, FidelityRisk.Risk.LimitRc == null ? (decimal?)null : FidelityRisk.Risk.LimitRc.LimitSum == 0 ? (decimal?)null : FidelityRisk.Risk.LimitRc.LimitSum);
            //facade.SetConcept(CompanyRuleConceptRisk.Apartment, FidelityRisk.NomenclatureAddress == null ? 0 : FidelityRisk.NomenclatureAddress.ApartmentOrOffice == null ? 0 : FidelityRisk.NomenclatureAddress.ApartmentOrOffice.Id);
            //facade.SetConcept(CompanyRuleConceptRisk.CityCode, FidelityRisk.City == null ? (int?)null : FidelityRisk.City.Id);
            //facade.SetConcept(CompanyRuleConceptRisk.RiskAge, FidelityRisk.RiskAge);
            //facade.SetConcept(CompanyRuleConceptRisk.EmlPercentage, FidelityRisk.PML);

            if (FidelityRisk.Risk.MainInsured != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.IsInsuredPayer, FidelityRisk.Risk.MainInsured.IndividualId == FidelityRisk.Risk.Policy.Holder.IndividualId);

                if (FidelityRisk.Risk.MainInsured.IndividualType == IndividualType.Person && FidelityRisk.Risk.MainInsured.BirthDate!= null)
                {
                    int insuredAge = (DateTime.Today - FidelityRisk.Risk.MainInsured.BirthDate.Value).Days / 365;
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, FidelityRisk.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, FidelityRisk.Risk.MainInsured.BirthDate);
                }
            }

            if (FidelityRisk.Risk.Beneficiaries != null && FidelityRisk.Risk.Beneficiaries.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryId, FidelityRisk.Risk.Beneficiaries.First().IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryPercentage, FidelityRisk.Risk.Beneficiaries.First().Participation);
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryTypeCode, (int)FidelityRisk.Risk.Beneficiaries.First().BeneficiaryType.Id);
            }


            if (FidelityRisk.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in FidelityRisk.Risk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }


        }

        public static void CreateFacadeCoverage(Rules.Facade facade, CompanyCoverage coverage)
        {
            facade.SetConcept(CompanyRuleConceptCoverage.RiskId, coverage.RiskCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageId, coverage.Id);
            facade.SetConcept(CompanyRuleConceptCoverage.IsDeclarative, coverage.IsDeclarative);
            facade.SetConcept(CompanyRuleConceptCoverage.IsMinimumPremiumDeposit, coverage.IsMinPremiumDeposit);
            facade.SetConcept(CompanyRuleConceptCoverage.FirstRiskTypeCode, coverage.FirstRiskType == null ? (int?)null : (int)coverage.FirstRiskType.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CalculationTypeCode, coverage.CalculationType == null ? (int?)null : (int)coverage.CalculationType.Value);
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
            //EndorsementId = coverage.Risk.Policy.Endorsement.Id == 0 ? (int?)null : coverage.Risk.Policy.Endorsement.Id, //estaba comentada en el codigo antes de la modificacion
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageNumber, coverage.Number == 0 ? (int?)null : coverage.Number);
            facade.SetConcept(CompanyRuleConceptCoverage.RiskCoverageId, coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageStatusCode, coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageOriginalStatusCode, coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.ConditionText, coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptCoverage.MaxLiabilityAmount, coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.MinimumPremiumCoverage, coverage.MinimumPremiumCoverage);






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
                 facade.SetConcept(CompanyRuleConceptCoverage.DeductId , coverage.Deductible.Id);
                 facade.SetConcept(CompanyRuleConceptCoverage.DeductRateTypeCode , (int)coverage.Deductible.RateType);
                 facade.SetConcept(CompanyRuleConceptCoverage.DeductRate , coverage.Deductible.Rate == null ? (decimal?)null : coverage.Deductible.Rate.Value);
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
