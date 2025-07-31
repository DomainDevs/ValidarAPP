using Sistran.Company.Application.UnderwritingServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Utilities.RulesEngine;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static void CreateFacadeGeneral(Rules.Facade facade, CompanyPolicy tplPolicy)
        {
            facade.SetConcept(RuleConceptGeneral.TempId, tplPolicy.Endorsement.TemporalId);
            facade.SetConcept(RuleConceptGeneral.QuotationId, tplPolicy.Endorsement.QuotationId);
            facade.SetConcept(RuleConceptGeneral.DocumentNumber, tplPolicy.DocumentNumber);
            facade.SetConcept(RuleConceptGeneral.PrefixCode, tplPolicy.Prefix.Id);
            facade.SetConcept(RuleConceptGeneral.EndorsementTypeCode, tplPolicy.Endorsement.EndorsementType);
            facade.SetConcept(RuleConceptGeneral.CurrencyCode, tplPolicy.ExchangeRate.Currency.Id);
            facade.SetConcept(RuleConceptGeneral.UserId, tplPolicy.UserId);
            facade.SetConcept(RuleConceptGeneral.ExchangeRate, tplPolicy.ExchangeRate.BuyAmount);
            facade.SetConcept(RuleConceptGeneral.IssueDate, tplPolicy.IssueDate);
            facade.SetConcept(RuleConceptGeneral.CurrentFrom, tplPolicy.CurrentFrom);
            facade.SetConcept(RuleConceptGeneral.CurrentTo, tplPolicy.CurrentTo);
            facade.SetConcept(RuleConceptGeneral.BeginDate, DateTime.Now);
            //facade.SetConcept(RuleConceptGeneral.CommitDate, DateTime.Now);
            facade.SetConcept(RuleConceptGeneral.BillingDate, DateTime.Now);
            facade.SetConcept(RuleConceptGeneral.BillingGroupCode, tplPolicy.BillingGroup?.Id);
            facade.SetConcept(RuleConceptGeneral.ProductId, tplPolicy.Product.Id);
            facade.SetConcept(RuleConceptGeneral.PolicyId, tplPolicy.Endorsement.PolicyId == 0 ? (int?)null : tplPolicy.Endorsement.PolicyId);
            facade.SetConcept(RuleConceptGeneral.EndorsementId, tplPolicy.Endorsement.Id == 0 ? (int?)null : tplPolicy.Endorsement.Id);
            facade.SetConcept(RuleConceptGeneral.ConditionTextId, tplPolicy.Text?.Id ?? 0);
            facade.SetConcept(RuleConceptGeneral.ConditionText, tplPolicy.Text?.TextBody);
            facade.SetConcept(RuleConceptGeneral.ConditionTextObservations, tplPolicy.Text?.Observations);
            facade.SetConcept(RuleConceptGeneral.BusinessTypeCode, tplPolicy.BusinessType == null ? 0 : (int)tplPolicy.BusinessType);
            facade.SetConcept(RuleConceptGeneral.OperationId, tplPolicy.Id);
            facade.SetConcept(RuleConceptGeneral.PolicyTypeCode, tplPolicy.PolicyType == null ? 0 : tplPolicy.PolicyType.Id);
            facade.SetConcept(RuleConceptGeneral.RequestId, tplPolicy.PolicyType == null ? 0 : tplPolicy.PolicyType.Id);
            //facade.SetConcept(RuleConceptGeneral.EffectPeriod, tplPolicy.EffectPeriod);
            //facade.SetConcept(RuleConceptGeneral.IsRequest, tplPolicy.Request == null ? false : tplPolicy.Request.Id == 0 ? false : true);
            facade.SetConcept(RuleConceptGeneral.PrimaryAgentId, tplPolicy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            facade.SetConcept(RuleConceptGeneral.PrimaryAgentCode, tplPolicy.Agencies.First(x => x.IsPrincipal).Code);
            facade.SetConcept(RuleConceptGeneral.IsPrimary, tplPolicy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            facade.SetConcept(RuleConceptGeneral.ScriptId, tplPolicy.Product.ScriptId);
            //facade.SetConcept(RuleConceptGeneral.PreRuleSetId, tplPolicy.Product.PreRuleSetId);
            //facade.SetConcept(RuleConceptGeneral.PreRuleSetId, tplPolicy.Product.PreRuleSetId);
            facade.SetConcept(RuleConceptGeneral.RuleSetId, tplPolicy.Product.RuleSetId);
            facade.SetConcept(RuleConceptGeneral.StandardCommissionPercentage, tplPolicy.Product.StandardCommissionPercentage);
            //facade.SetConcept(RuleConceptGeneral.IsFlatRate, tplPolicy.Product.IsFlatRate,
            //    IsCollective = tplPolicy.Product.IsCollective,
            //    IsGreen = tplPolicy.Product.IsGreen,
            //    PaymentScheduleId = tplPolicy.PaymentPlan?.Id,
            //    CalculateMinPremium = tplPolicy.CalculateMinPremium ?? false);

            if (tplPolicy.Summary != null)
            {
                facade.SetConcept(RuleConceptGeneral.RisksQuantity, tplPolicy.Summary.RiskCount);
                facade.SetConcept(RuleConceptGeneral.AmountInsured, tplPolicy.Summary.AmountInsured);
                facade.SetConcept(RuleConceptGeneral.Premium, tplPolicy.Summary.Premium);
                facade.SetConcept(RuleConceptGeneral.Expenses, tplPolicy.Summary.Expenses);
                facade.SetConcept(RuleConceptGeneral.Taxes, tplPolicy.Summary.Taxes);
                facade.SetConcept(RuleConceptGeneral.FullPremium, tplPolicy.Summary.FullPremium);
            }
            if (tplPolicy.Holder != null)
            {
                facade.SetConcept(RuleConceptGeneral.PolicyHolderId, tplPolicy.Holder.IndividualId);

                facade.SetConcept(RuleConceptGeneral.CustomerTypeCode, (int)tplPolicy.Holder.CustomerType);
                if (tplPolicy.Holder.IdentificationDocument != null)
                {
                    facade.SetConcept(RuleConceptGeneral.HolderIdentificationDocument, tplPolicy.Holder.IdentificationDocument.Number);
                }
                if (tplPolicy.Holder.CompanyName != null && tplPolicy.Holder.CompanyName.Address != null)
                {
                    facade.SetConcept(RuleConceptGeneral.MailAddressId, tplPolicy.Holder.CompanyName.Address.Id);
                }
                if (tplPolicy.Holder.IndividualType == IndividualType.Person)
                {
                    int holderAge = 0;

                    if (tplPolicy.Holder.BirthDate.GetValueOrDefault() != DateTime.MinValue)
                    {
                        holderAge = (DateTime.Today - tplPolicy.Holder.BirthDate.Value).Days / 365;
                    }
                    facade.SetConcept(RuleConceptGeneral.HolderAge, holderAge);
                    facade.SetConcept(RuleConceptGeneral.HolderBirthDate, tplPolicy.Holder.BirthDate);
                    facade.SetConcept(RuleConceptGeneral.HolderGender, tplPolicy.Holder.Gender);
                }
            }

            if (tplPolicy.Branch != null)
            {
                facade.SetConcept(RuleConceptGeneral.BranchCode, tplPolicy.Branch.Id);
                facade.SetConcept(RuleConceptGeneral.SalePointCode, tplPolicy.Branch.SalePoints?.FirstOrDefault()?.Id);
            }

            if (tplPolicy.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in tplPolicy.DynamicProperties)
                {

                    if (dynamicConcept.Value != null && !string.IsNullOrEmpty(dynamicConcept.TypeName))
                    {
                        facade.SetConcept(RuleConceptGeneral.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                    }
                }
            }
        }

        public static void CreateFacadeRiskThirdPartyLiability(Rules.Facade facade, CompanyTplRisk tplRisk)
        {
            
            facade.SetConcept(CompanyRuleConceptRisk.VehicleMakeCode, Convert.ToInt32(tplRisk.Make?.Id));
            facade.SetConcept(CompanyRuleConceptRisk.VehicleYear, tplRisk.Year);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleTypeCode, Convert.ToInt32(tplRisk.Version?.Type?.Id));
            facade.SetConcept(CompanyRuleConceptRisk.VehicleBodyCode, Convert.ToInt32(tplRisk.Version?.Body?.Id));
            facade.SetConcept(CompanyRuleConceptRisk.LicensePlate, tplRisk.LicensePlate);
            facade.SetConcept(CompanyRuleConceptRisk.EngineSerialNumber, tplRisk.EngineSerial);
            facade.SetConcept(CompanyRuleConceptRisk.ChassisSerialNumber, tplRisk.ChassisSerial);
            facade.SetConcept(CompanyRuleConceptRisk.EngineSerialNumber, tplRisk.EngineSerial);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleColorCode, Convert.ToInt32(tplRisk.Color?.Id));
            facade.SetConcept(CompanyRuleConceptRisk.PassengerQuantity, Convert.ToInt32(tplRisk.PassengerQuantity));
            facade.SetConcept(CompanyRuleConceptRisk.FlatRatePercentage, tplRisk.Rate);
            facade.SetConcept(CompanyRuleConceptRisk.DeductId, Convert.ToInt32(tplRisk.Deductible?.Id));
            facade.SetConcept(CompanyRuleConceptRisk.RateTypeCode, Convert.ToInt32(tplRisk.Rate));
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, Convert.ToInt32(tplRisk.Risk?.GroupCoverage?.Id));
            facade.SetConcept(CompanyRuleConceptRisk.IsFacultative, tplRisk.Risk.IsFacultative);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleGallonTankCapacity, tplRisk.GallonTankCapacity);
            if (tplRisk.Risk.MainInsured != null )
            {
                facade.SetConcept(RuleConceptRisk.IsInsuredPayer, tplRisk.Risk.MainInsured.IndividualId == tplRisk.Risk.Policy.Holder.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.InsuredId, tplRisk.Risk.MainInsured.IndividualId == 0 ? (int?)null : tplRisk.Risk.MainInsured.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, (int)tplRisk.Risk.MainInsured.CustomerType);

                if (tplRisk.Risk.MainInsured.IndividualType == IndividualType.Person && tplRisk.Risk.MainInsured.BirthDate != null)
                {
                    int insuredAge = (DateTime.Today - tplRisk.Risk.MainInsured.BirthDate.Value).Days / 365;
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, tplRisk.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, tplRisk.Risk.MainInsured.BirthDate);                    
                }
            }

            //if (tplRisk.Risk.Beneficiaries != null && tplRisk.Risk.Beneficiaries.Count > 0)
            //{
            //    facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryId, tplRisk.Risk.Beneficiaries.First().IndividualId);
            //    facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryPercentage, tplRisk.Risk.Beneficiaries.First().Participation);
            //    facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryTypeCode, tplRisk.Risk.Beneficiaries.First().BeneficiaryType.Id);
            //    //HasWarrantCreditor
            //}
            if (tplRisk.Risk.Beneficiaries != null && tplRisk.Risk.Beneficiaries.Count > 0)
            {
                CompanyBeneficiary beneficiary = tplRisk.Risk.Beneficiaries.First();
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

            if (tplRisk.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in tplRisk.Risk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

        public static void CreateFacadeCoverage(Rules.Facade facade, CompanyCoverage coverage)
        {
            facade.SetConcept(RuleConceptCoverage.CoverageId, coverage.Id);
            facade.SetConcept(RuleConceptCoverage.IsDeclarative, coverage.IsDeclarative);
            facade.SetConcept(RuleConceptCoverage.IsMinimumPremiumDeposit, coverage.IsMinPremiumDeposit);
            facade.SetConcept(RuleConceptCoverage.FirstRiskTypeCode, coverage.FirstRiskType == null ? (int?)null : (int)coverage.FirstRiskType.Value);
            facade.SetConcept(RuleConceptCoverage.CalculationTypeCode, coverage.CalculationType == null ? (int?)null : (int)coverage.CalculationType.Value);
            facade.SetConcept(RuleConceptCoverage.DeclaredAmount, coverage.DeclaredAmount == 0 ? (decimal?)null : coverage.DeclaredAmount);
            facade.SetConcept(RuleConceptCoverage.PremiumAmount, coverage.PremiumAmount == 0 ? (decimal?)null : coverage.PremiumAmount);
            facade.SetConcept(RuleConceptCoverage.LimitAmount, coverage.LimitAmount == 0 ? (decimal?)null : coverage.LimitAmount);
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
        public static void CreateFacadeRiskVehicle(Rules.Facade facade, CompanyTplRisk companyTpl)
        {
            facade.SetConcept(CompanyRuleConceptRisk.TempId, companyTpl.Risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, companyTpl.Risk.RiskId);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, (int)companyTpl.Risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, companyTpl.Risk.Status == null ? (int?)null : (int)companyTpl.Risk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, companyTpl.Risk.OriginalStatus == null ? (int?)null : (int)companyTpl.Risk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyTpl.Risk.Text == null ? string.Empty : companyTpl.Risk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, companyTpl.Risk.RatingZone?.Id == 0 ? null : companyTpl.Risk.RatingZone?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupIdPreview, companyTpl.Risk.GroupCoverage?.Id == 0 ? null : companyTpl.Risk.GroupCoverage?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, companyTpl.Risk.GroupCoverage?.Id == 0 ? null : companyTpl.Risk.GroupCoverage?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, companyTpl.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCodePreview, companyTpl.Risk.LimitRc?.Id == 0 ? null : companyTpl.Risk.LimitRc?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, companyTpl.Risk.LimitRc?.Id == 0 ? null : companyTpl.Risk.LimitRc?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, companyTpl.Risk.LimitRc?.LimitSum == 0 ? null : companyTpl.Risk.LimitRc?.LimitSum);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleVersionCode, companyTpl.Version?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleModelCode, companyTpl.Model?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleMakeCode, companyTpl.Make?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleYear, companyTpl.Year);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleTypeCode, companyTpl.Version == null ? 0 : companyTpl.Version.Type?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleUseCode, companyTpl.Use?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleBodyCode, companyTpl.Version?.Body?.Id == 0 ? null : companyTpl.Version?.Body?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleFuelCode, companyTpl.Version?.Fuel?.Id == 0 ? null : companyTpl.Version?.Fuel?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.IsNew, companyTpl.IsNew);
            facade.SetConcept(CompanyRuleConceptRisk.LicensePlate, companyTpl.LicensePlate);
            facade.SetConcept(CompanyRuleConceptRisk.EngineSerialNumber, companyTpl.EngineSerial);
            facade.SetConcept(CompanyRuleConceptRisk.ChassisSerialNumber, companyTpl.ChassisSerial);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleColorCode, companyTpl.Color?.Id == 0 ? null : companyTpl.Color?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.PassengerQuantity, companyTpl.PassengerQuantity == 0 ? (int?)null : companyTpl.PassengerQuantity);
            facade.SetConcept(CompanyRuleConceptRisk.NewVehiclePrice, companyTpl.NewPrice == 0 ? (decimal?)null : companyTpl.NewPrice);
            facade.SetConcept(CompanyRuleConceptRisk.FlatRatePercentage, (decimal?)companyTpl.Rate);
            facade.SetConcept(CompanyRuleConceptRisk.ServiceTypeCode, companyTpl.ServiceType?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.RateTypeCode, companyTpl.RateType == null ? (int?)null : (int)companyTpl?.RateType.Value);
            facade.SetConcept(CompanyRuleConceptRisk.RiskNumber, companyTpl.Risk.Policy.Endorsement.Number);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyTpl.Risk.Premium);
            facade.SetConcept(CompanyRuleConceptRisk.AmountInsured, companyTpl.Risk?.AmountInsured ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionTextId, companyTpl.Risk.Text?.Id == 0 ? null : companyTpl.Risk.Text?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyTpl.Risk.Text?.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.CoveragesCount, companyTpl.Risk.Coverages?.Count);
            facade.SetConcept(CompanyRuleConceptRisk.Coverages, companyTpl.Risk.Coverages);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyTpl.Risk.Premium);

            if (companyTpl.Risk.MainInsured != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.InsuredId, companyTpl.Risk.MainInsured.IndividualId == 0 ? (int?)null : companyTpl.Risk.MainInsured.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, (int)companyTpl.Risk.MainInsured.CustomerType);
                facade.SetConcept(CompanyRuleConceptRisk.IsInsuredPayer, companyTpl.Risk.MainInsured.IndividualId == companyTpl.Risk.Policy.Holder.IndividualId);
                if (companyTpl.Risk.MainInsured.IdentificationDocument != null)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredIdentificationDocument, companyTpl.Risk.MainInsured.IdentificationDocument.Number);
                }
                if (companyTpl.Risk.MainInsured.IndividualType == IndividualType.Person)
                {
                    int insuredAge = 0;
                    if (companyTpl.Risk.MainInsured.BirthDate.GetValueOrDefault() > DateTime.MinValue)
                    {
                        insuredAge = (DateTime.Today - companyTpl.Risk.MainInsured.BirthDate.Value).Days / 365;
                    }
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);


                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, companyTpl.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, companyTpl.Risk.MainInsured.BirthDate);
                }
            }
            if (companyTpl.Risk.Beneficiaries != null && companyTpl.Risk.Beneficiaries.Count > 0)
            {
                CompanyBeneficiary beneficiary = companyTpl.Risk.Beneficiaries.First();
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
            if (companyTpl.Risk?.CompanyClaimsBills != null)
            {
                CompanyClaimsBills claimsBills = companyTpl.Risk.CompanyClaimsBills;
            }
            if (companyTpl.Risk?.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in companyTpl.Risk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

    }
}

