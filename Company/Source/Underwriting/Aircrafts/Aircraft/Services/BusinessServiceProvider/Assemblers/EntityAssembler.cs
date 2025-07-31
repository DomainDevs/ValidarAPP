using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;


namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        /// <summary>
        /// Crea la fachada general
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="companyPolicy"></param>
        public static void CreateFacadeGeneral(Facade facade, CompanyPolicy companyPolicy)
        {
            decimal? StandardCommissionPercentage = companyPolicy.Agencies?.FirstOrDefault(x => x.IsPrincipal)?.Commissions?.FirstOrDefault()?.Percentage;
            if (companyPolicy.Agencies.FirstOrDefault(x => x.IsPrincipal)?.Commissions.FirstOrDefault()?.Percentage == null)
            {
                StandardCommissionPercentage = companyPolicy.Product.StandardCommissionPercentage;
            }
            var product = companyPolicy.Product;
            facade.SetConcept(CompanyRuleConceptGeneral.TempId, companyPolicy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptGeneral.QuotationId, companyPolicy.Endorsement.QuotationId);
            facade.SetConcept(CompanyRuleConceptGeneral.DocumentNumber, companyPolicy.DocumentNumber == 0 ? (decimal?)null : companyPolicy.DocumentNumber);

            facade.SetConcept(CompanyRuleConceptGeneral.PrefixCode, companyPolicy.Prefix.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.EndorsementTypeCode, (int)companyPolicy.Endorsement.EndorsementType);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrencyCode, companyPolicy.ExchangeRate.Currency.Id);

            facade.SetConcept(CompanyRuleConceptGeneral.UserId, companyPolicy.UserId);
            facade.SetConcept(CompanyRuleConceptGeneral.ExchangeRate, companyPolicy.ExchangeRate.BuyAmount);
            facade.SetConcept(CompanyRuleConceptGeneral.IssueDate, companyPolicy.IssueDate);

            facade.SetConcept(CompanyRuleConceptGeneral.IssueYear, companyPolicy.IssueDate.Year);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrentFrom, companyPolicy.CurrentFrom);

            facade.SetConcept(CompanyRuleConceptGeneral.BeginDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.BillingDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.BillingGroupCode, companyPolicy.BillingGroup?.Id);

            facade.SetConcept(CompanyRuleConceptGeneral.ProductId, companyPolicy.Product.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyId, companyPolicy.Endorsement.PolicyId == 0 ? (int?)null : companyPolicy.Endorsement.PolicyId);
            facade.SetConcept(CompanyRuleConceptGeneral.EndorsementId, companyPolicy.Endorsement.Id == 0 ? (int?)null : companyPolicy.Endorsement.Id);

            facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextId, companyPolicy.Text?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionText, companyPolicy.Text?.TextBody);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextObservations, companyPolicy.Text?.Observations);

            facade.SetConcept(CompanyRuleConceptGeneral.BusinessTypeCode, companyPolicy.BusinessType == null ? 0 : (int)companyPolicy.BusinessType);
            facade.SetConcept(CompanyRuleConceptGeneral.OperationId, companyPolicy.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyTypeCode, companyPolicy.PolicyType == null ? 0 : companyPolicy.PolicyType.Id);

            facade.SetConcept(CompanyRuleConceptGeneral.RequestId, companyPolicy.Request == null ? (int?)null : companyPolicy.Request.Id == 0 ? (int?)null : companyPolicy.Request.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentId, companyPolicy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCode, companyPolicy.Agencies.First(x => x.IsPrincipal).Code);

            facade.SetConcept(CompanyRuleConceptGeneral.IsPrimary, companyPolicy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            facade.SetConcept(CompanyRuleConceptGeneral.ScriptId, companyPolicy.Product.ScriptId);
            facade.SetConcept(CompanyRuleConceptGeneral.RuleSetId, companyPolicy.Product.RuleSetId);

            facade.SetConcept(CompanyRuleConceptGeneral.StandardCommissionPercentage, companyPolicy.Product.StandardCommissionPercentage);
            facade.SetConcept(CompanyRuleConceptGeneral.IsGreen, companyPolicy.Product.IsGreen);
            facade.SetConcept(CompanyRuleConceptGeneral.DaysVigency, companyPolicy.Endorsement != null ? companyPolicy.Endorsement.EndorsementDays : 0);

            facade.SetConcept(CompanyRuleConceptGeneral.PaymentScheduleId, companyPolicy.Product.RuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.CalculateMinPremium, companyPolicy.CalculateMinPremium ?? false);

            if (companyPolicy.Summary != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.RisksQuantity, companyPolicy.Summary.RiskCount);
                facade.SetConcept(CompanyRuleConceptGeneral.AmountInsured, companyPolicy.Summary.AmountInsured);
                facade.SetConcept(CompanyRuleConceptGeneral.Premium, companyPolicy.Summary.Premium);

                facade.SetConcept(CompanyRuleConceptGeneral.Expenses, companyPolicy.Summary.Expenses);
                facade.SetConcept(CompanyRuleConceptGeneral.Taxes, companyPolicy.Summary.Taxes);
                facade.SetConcept(CompanyRuleConceptGeneral.FullPremium, companyPolicy.Summary.FullPremium);
            }
            if (companyPolicy.Holder != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.PolicyHolderId, companyPolicy.Holder.IndividualId);
                facade.SetConcept(CompanyRuleConceptGeneral.CustomerTypeCode, (int)companyPolicy.Holder.CustomerType);

                if (companyPolicy.Holder.IdentificationDocument != null)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderIdentificationDocument, companyPolicy.Holder.IdentificationDocument.Number);
                }
                if (companyPolicy.Holder.CompanyName != null && companyPolicy.Holder.CompanyName.Address != null)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.MailAddressId, companyPolicy.Holder.CompanyName.Address.Id);
                }
                if (companyPolicy.Holder.IndividualType == IndividualType.Person)
                {
                    int holderAge = 0;

                    if (companyPolicy.Holder.BirthDate.GetValueOrDefault() != DateTime.MinValue)
                    {
                        holderAge = (DateTime.Today - companyPolicy.Holder.BirthDate.Value).Days / 365;
                    }
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderAge, holderAge);            
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderBirthDate, companyPolicy.Holder.BirthDate);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderGender, companyPolicy.Holder.Gender);
                }
            }
            if (companyPolicy.Branch != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.BranchCode, companyPolicy.Branch.Id);

                facade.GetConcept<int>(CompanyRuleConceptGeneral.BranchCode);
                if (facade.GetConcept<int>(CompanyRuleConceptGeneral.SalePointCode) > 0)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.SalePointCode, facade.GetConcept<int>(CompanyRuleConceptGeneral.SalePointCode));
                }
            }
            if (companyPolicy.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in companyPolicy.DynamicProperties)
                {
                    if (dynamicConcept.Value != null && !string.IsNullOrEmpty(dynamicConcept.TypeName))
                    {
                        facade.SetConcept(CompanyRuleConceptGeneral.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                    }
                }
            }
            const int parameterTempSubscription = 73;

            int? parameterTempSubscriptionValue = DelegateService.commonService.GetParameterByParameterId(parameterTempSubscription).NumberParameter;
            if (parameterTempSubscriptionValue != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.TempId, parameterTempSubscriptionValue.Value);
            }
        }

        /// <summary>
        /// Crea la fachada de coberturas
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="coverage"></param>
        internal static void CreateFacadeCoverage(Facade facade, CompanyCoverage coverage)
        {
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

            facade.SetConcept(CompanyRuleConceptCoverage.AccumulatedLimitAmount, coverage.ExcessLimit == 0 ? (decimal?)null : coverage.ExcessLimit);
            facade.SetConcept(CompanyRuleConceptCoverage.AccumulatedSubLimitAmount, coverage.LimitOccurrenceAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.CurrentFrom, coverage.CurrentFrom);

            facade.SetConcept(CompanyRuleConceptCoverage.RateTypeCode, coverage.RateType == null ? 0 : (int)coverage.RateType.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.Rate, coverage.Rate);
            facade.SetConcept(CompanyRuleConceptCoverage.CurrentTo, coverage.CurrentTo);

            facade.SetConcept(CompanyRuleConceptCoverage.MainCoverageId, coverage.MainCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.MainCoveragePercentage, coverage.MainCoveragePercentage);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageNumber, coverage.Number == 0 ? (int?)null : coverage.Number);

            facade.SetConcept(CompanyRuleConceptCoverage.RiskCoverageId, coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageStatusCode, coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(CompanyRuleConceptCoverage.CoverageOriginalStatusCode, coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);

            facade.SetConcept(CompanyRuleConceptCoverage.ConditionText, coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptCoverage.ConditionTextId, coverage.Text?.Id == 0 ? null : coverage.Text?.Id);
            facade.SetConcept(CompanyRuleConceptCoverage.MaxLiabilityAmount, coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(CompanyRuleConceptCoverage.MinimumPremiumCoverage, coverage.MinimumPremiumCoverage);
            facade.SetConcept(CompanyRuleConceptCoverage.InsuredObjectId, coverage.InsuredObject.Id);
            facade.SetConcept(CompanyRuleConceptCoverage.InsuredObjectAmount, coverage.InsuredObject.Amount);

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

        /// <summary>
        /// Crea la fachada de los riesgos de Aircrafte
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="companyAircraft"></param>
        internal static void CreateFacadeRiskAircraft(Facade facade, CompanyAircraft companyAircraft)
        {
            facade.SetConcept(CompanyRuleConceptRisk.TempId, companyAircraft.Risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, companyAircraft.Risk.RiskId);
            facade.SetConcept(CompanyRuleConceptRisk.HolderTypeCode, 2);
            facade.SetConcept(CompanyRuleConceptRisk.EndorsementId, companyAircraft.Risk.Policy.Endorsement.Id);
            facade.SetConcept(CompanyRuleConceptRisk.PolicyTypeViewCode, companyAircraft.Risk.Policy.PolicyType.Id);
            facade.SetConcept(CompanyRuleConceptRisk.HolderTypeCode, companyAircraft.Risk.Policy.Holder.CustomerType);
            facade.SetConcept(CompanyRuleConceptRisk.CustomsAgentId, companyAircraft.Risk.Policy.Agencies[0].Agent.IndividualId);
            facade.SetConcept(CompanyRuleConceptRisk.BeneficiariesCount, companyAircraft.Risk.Beneficiaries?.Count == null?0: companyAircraft.Risk.Beneficiaries?.Count);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, (int)companyAircraft.Risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, companyAircraft.Risk.Status == null ? (int?)null : (int)companyAircraft.Risk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, companyAircraft.Risk.OriginalStatus == null ? (int?)null : (int)companyAircraft.Risk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyAircraft.Risk.Text == null ? string.Empty : companyAircraft.Risk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, companyAircraft.Risk.RatingZone?.Id == 0 ? null : companyAircraft.Risk.RatingZone?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, companyAircraft.Risk.GroupCoverage?.Id == 0 ? null : companyAircraft.Risk.GroupCoverage?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, companyAircraft.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, companyAircraft.Risk.LimitRc?.Id == 0 ? null : companyAircraft.Risk.LimitRc?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, companyAircraft.Risk.LimitRc?.LimitSum == 0 ? null : companyAircraft.Risk.LimitRc?.LimitSum);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyAircraft.Risk.Premium);
            facade.SetConcept(CompanyRuleConceptRisk.AmountInsured, companyAircraft.Risk?.AmountInsured ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionTextId, companyAircraft.Risk.Text?.Id == 0 ? null : companyAircraft.Risk.Text?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyAircraft.Risk.Text?.TextBody);

            if (companyAircraft.Risk.MainInsured != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.InsuredId, companyAircraft.Risk.MainInsured.IndividualId == 0 ? (int?)null : companyAircraft.Risk.MainInsured.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, (int)companyAircraft.Risk.MainInsured.CustomerType);
                facade.SetConcept(CompanyRuleConceptRisk.IsInsuredPayer, companyAircraft.Risk.MainInsured.IndividualId == companyAircraft.Risk.Policy.Holder.IndividualId);
                if (companyAircraft.Risk.MainInsured.IdentificationDocument != null)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredIdentificationDocument, companyAircraft.Risk.MainInsured.IdentificationDocument.Number);
                }
                if (companyAircraft.Risk.MainInsured.IndividualType == IndividualType.Person)
                {
                    int insuredAge = 0;
                    if (companyAircraft.Risk.MainInsured.BirthDate.GetValueOrDefault() > DateTime.MinValue)
                    {
                        insuredAge = (DateTime.Today - companyAircraft.Risk.MainInsured.BirthDate.Value).Days / 365;
                    }

                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, companyAircraft.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, companyAircraft.Risk.MainInsured.BirthDate);
                }
            }
            if (companyAircraft.Risk.Beneficiaries != null && companyAircraft.Risk.Beneficiaries.Count > 0)
            {
                CompanyBeneficiary beneficiary = companyAircraft.Risk.Beneficiaries.First();
                if (beneficiary != null)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryId, beneficiary.IndividualId);
                    facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryPercentage, beneficiary.Participation);
                    facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryTypeCode, beneficiary?.BeneficiaryType?.Id);

                    if (beneficiary.IdentificationDocument != null)
                    {
                        facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryIdentificationDocument, beneficiary.IdentificationDocument.Number);
                    }
                }
                else
                {
                    throw new Exception("No existe el beneficiario");
                }
            }
            // Información de siniestralidad
            if (companyAircraft.Risk?.CompanyClaimsBills != null)
            {
                var claimsBills = companyAircraft.Risk.CompanyClaimsBills;
            }
            if (companyAircraft.Risk?.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in companyAircraft.Risk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }
      }
}