using Sistran.Company.Application.Issuance.Entities;
using Sistran.Company.Application.Temporary.Entities;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Utilities.RulesEngine;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers
{
    internal class EntityAssembler
    {
        public static void CreateFacadeGeneral(Rules.Facade facade, CompanyPolicy companyPolicy)
        {
            if (companyPolicy.Product.StandardCommissionPercentage <= 0 )
            {
                decimal? StandardCommissionPercentage = companyPolicy.Agencies?.FirstOrDefault(x => x.IsPrincipal)?.Commissions?.FirstOrDefault()?.Percentage;
                if (companyPolicy.Agencies.FirstOrDefault(x => x.IsPrincipal)?.Commissions.FirstOrDefault()?.Percentage == null)
                {
                    StandardCommissionPercentage = companyPolicy.Product.StandardCommissionPercentage;
                }
            }
           

            ProductServices.Models.CompanyProduct product = companyPolicy.Product;

            facade.SetConcept(CompanyRuleConceptGeneral.TempId, companyPolicy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptGeneral.QuotationId, companyPolicy.Endorsement.QuotationId);
            facade.SetConcept(CompanyRuleConceptGeneral.DocumentNumber, companyPolicy.DocumentNumber == 0 ? (decimal?)null : companyPolicy.DocumentNumber);
            facade.SetConcept(CompanyRuleConceptGeneral.PrefixCode, companyPolicy.Prefix.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.EndorsementTypeCode, (int)companyPolicy.Endorsement.EndorsementType);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrencyCode, companyPolicy.ExchangeRate.Currency.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.UserId, companyPolicy.UserId);
            facade.SetConcept(CompanyRuleConceptGeneral.ExchangeRate, companyPolicy.ExchangeRate.SellAmount);
            facade.SetConcept(CompanyRuleConceptGeneral.IssueDate, companyPolicy.IssueDate);
            facade.SetConcept(CompanyRuleConceptGeneral.IssueYear, companyPolicy.IssueDate.Year);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrentFrom, companyPolicy.CurrentFrom);
            facade.SetConcept(CompanyRuleConceptGeneral.YearCurrentFrom, companyPolicy.CurrentFrom.Year);
            facade.SetConcept(CompanyRuleConceptGeneral.DaysRetroactivityPosterity, (companyPolicy.CurrentFrom - companyPolicy.IssueDate).Days);
            facade.SetConcept(CompanyRuleConceptGeneral.CurrentTo, companyPolicy.CurrentTo);
            facade.SetConcept(CompanyRuleConceptGeneral.BeginDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.CommitDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.BillingDate, DateTime.Now);
            facade.SetConcept(CompanyRuleConceptGeneral.BillingGroupCode, companyPolicy.Request?.BillingGroupId);
            facade.SetConcept(CompanyRuleConceptGeneral.ProductId, companyPolicy.Product.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyId, companyPolicy.Endorsement.PolicyId == 0 ? (int?)null : companyPolicy.Endorsement.PolicyId);
            facade.SetConcept(CompanyRuleConceptGeneral.EndorsementId, companyPolicy.Endorsement.Id == 0 ? (int?)null : companyPolicy.Endorsement.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.TemporalTypeCode, (int)companyPolicy.TemporalType);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextId, companyPolicy.Text?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionText, companyPolicy.Text?.TextBody);
            facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextObservations, companyPolicy.Text?.Observations);
            facade.SetConcept(CompanyRuleConceptGeneral.BusinessTypeCode, companyPolicy.BusinessType == null ? 0 : companyPolicy.BusinessType);
            facade.SetConcept(CompanyRuleConceptGeneral.OperationId, companyPolicy.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyTypeCode, companyPolicy.PolicyType.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.RequestId, companyPolicy.Request == null ? null : companyPolicy.Request.Id == 0 ? (int?)null : companyPolicy.Request.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.EffectPeriod, companyPolicy.EffectPeriod);
            facade.SetConcept(CompanyRuleConceptGeneral.IsRequest, companyPolicy.Request == null ? false : companyPolicy.Request.Id == 0 ? false : true);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentId, companyPolicy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentAgencyId, companyPolicy.Agencies.First(x => x.IsPrincipal).Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCode, companyPolicy.Agencies.First(x => x.IsPrincipal).Code);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCodePrincipal, companyPolicy.Agencies.First(p => p.IsPrincipal).Code);
            facade.SetConcept(CompanyRuleConceptGeneral.IsPrimary, companyPolicy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            facade.SetConcept(CompanyRuleConceptGeneral.ScriptId, companyPolicy.Product.ScriptId);
            facade.SetConcept(CompanyRuleConceptGeneral.PreRuleSetId, companyPolicy.Product.PreRuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.RuleSetId, companyPolicy.Product.RuleSetId);
            if (companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions != null && companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions.Count > 0 && companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions.First() != null)
                facade.SetConcept(CompanyRuleConceptGeneral.StandardCommissionPercentage, companyPolicy.Product.StandardCommissionPercentage);
            facade.SetConcept(CompanyRuleConceptGeneral.IsFlatRate, companyPolicy.Product.IsFlatRate);
            facade.SetConcept(CompanyRuleConceptGeneral.IsCollective, companyPolicy.Product.IsCollective);
            facade.SetConcept(CompanyRuleConceptGeneral.IsGreen, companyPolicy.Product.IsGreen);
            facade.SetConcept(CompanyRuleConceptGeneral.DaysVigency, (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days);
            facade.SetConcept(CompanyRuleConceptGeneral.HasTotalLoss, companyPolicy.HasTotalLoss);
            facade.SetConcept(CompanyRuleConceptGeneral.SinisterQuantity, companyPolicy.SinisterQuantity);
            facade.SetConcept(CompanyRuleConceptGeneral.PortfolioBalance, companyPolicy.PortfolioBalance);
            facade.SetConcept(CompanyRuleConceptGeneral.AgentType, companyPolicy?.Agencies?.FirstOrDefault(x => x.IsPrincipal)?.Agent?.AgentType?.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PaymentScheduleId, companyPolicy.Product.RuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.CalculateMinPremium, companyPolicy.CalculateMinPremium ?? false);
            facade.SetConcept(CompanyRuleConceptGeneral.SinisterQuantity, companyPolicy.SinisterQuantity);
            facade.SetConcept(CompanyRuleConceptGeneral.ProcessType, companyPolicy.SubMassiveProcessType);
            facade.SetConcept(CompanyRuleConceptGeneral.TotalRisk, companyPolicy.TotalRisk);


            if (companyPolicy.JustificationSarlaft != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.JustificationSarlaft, companyPolicy.JustificationSarlaft.JustificationReasonCode);
            }

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

                    facade.SetConcept(CompanyRuleConceptGeneral.HolderCellPhone, companyPolicy.Holder.CompanyName.Phone != null ? companyPolicy.Holder.CompanyName.Phone.Description : null);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderAge, holderAge);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderEmail, companyPolicy.Holder.CompanyName.Email != null ? companyPolicy.Holder.CompanyName.Email.Description : null);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderBirthDate, companyPolicy.Holder.BirthDate);
                    facade.SetConcept(CompanyRuleConceptGeneral.HolderGender, companyPolicy.Holder.Gender);
                }
            }

            if (companyPolicy.Branch != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.BranchCode, companyPolicy.Branch.Id);

                if (companyPolicy.Branch.SalePoints != null && companyPolicy.Branch.SalePoints.Count > 0)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.SalePointCode, companyPolicy.Branch.SalePoints.First().Id);
                }
            }

            Core.Application.CommonService.Models.Parameter parameterAppSourceR2 = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.ConceptAppSourceR2);
            if (parameterAppSourceR2 != null && parameterAppSourceR2.NumberParameter.HasValue)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.DynamicConcept(parameterAppSourceR2.NumberParameter.Value, 83), companyPolicy.AppSourceR2);
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

        public static void CreateFacadeRiskVehicle(Rules.Facade facade, CompanyVehicle companyVehicle)
        {
            facade.SetConcept(CompanyRuleConceptRisk.TempId, companyVehicle.Risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, companyVehicle.Risk.RiskId);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, (int)companyVehicle.Risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, companyVehicle.Risk.Status == null ? (int?)null : (int)companyVehicle.Risk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, companyVehicle.Risk.OriginalStatus == null ? (int?)null : (int)companyVehicle.Risk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyVehicle.Risk.Text == null ? string.Empty : companyVehicle.Risk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, companyVehicle.Risk.RatingZone?.Id == 0 ? null : companyVehicle.Risk.RatingZone?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupIdPreview, companyVehicle.Risk.GroupCoverage?.Id == 0 ? null : companyVehicle.Risk.GroupCoverage?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, companyVehicle.Risk.GroupCoverage?.Id == 0 ? null : companyVehicle.Risk.GroupCoverage?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, companyVehicle.Risk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCodePreview, companyVehicle.Risk.LimitRc?.Id == 0 ? null : companyVehicle.Risk.LimitRc?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, companyVehicle.Risk.LimitRc?.Id == 0 ? null : companyVehicle.Risk.LimitRc?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, companyVehicle.Risk.LimitRc?.LimitSum == 0 ? null : companyVehicle.Risk.LimitRc?.LimitSum);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleVersionCode, companyVehicle.Version?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleModelCode, companyVehicle.Model?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleMakeCode, companyVehicle.Make?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleYear, companyVehicle.Year);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleTypeCode, companyVehicle.Version == null ? 0 : companyVehicle.Version.Type?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleUseCode, companyVehicle.Use?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleBodyCode, companyVehicle.Version?.Body?.Id == 0 ? null : companyVehicle.Version?.Body?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleFuelCode, companyVehicle.Version?.Fuel?.Id == 0 ? null : companyVehicle.Version?.Fuel?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.VehiclePrice, companyVehicle.Price);
            facade.SetConcept(CompanyRuleConceptRisk.IsNew, companyVehicle.IsNew);
            facade.SetConcept(CompanyRuleConceptRisk.LicensePlate, companyVehicle.LicensePlate);
            facade.SetConcept(CompanyRuleConceptRisk.EngineSerialNumber, companyVehicle.EngineSerial);
            facade.SetConcept(CompanyRuleConceptRisk.ChassisSerialNumber, companyVehicle.ChassisSerial);
            facade.SetConcept(CompanyRuleConceptRisk.VehicleColorCode, companyVehicle.Color?.Id == 0 ? null : companyVehicle.Color?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LoadTypeCode, companyVehicle.LoadTypeCode == 0 ? (int?)null : companyVehicle.LoadTypeCode);
            facade.SetConcept(CompanyRuleConceptRisk.TrailersQuantity, companyVehicle.TrailersQuantity == 0 ? (int?)null : companyVehicle.TrailersQuantity);
            facade.SetConcept(CompanyRuleConceptRisk.PassengerQuantity, companyVehicle.PassengerQuantity == 0 ? (int?)null : companyVehicle.PassengerQuantity);
            facade.SetConcept(CompanyRuleConceptRisk.NewVehiclePrice, companyVehicle.NewPrice == 0 ? (decimal?)null : companyVehicle.NewPrice);
            facade.SetConcept(CompanyRuleConceptRisk.StandardVehiclePrice, companyVehicle.StandardVehiclePrice);
            facade.SetConcept(CompanyRuleConceptRisk.FlatRatePercentage, (decimal?)companyVehicle.Rate);
            facade.SetConcept(CompanyRuleConceptRisk.ServiceTypeCode, companyVehicle.ServiceType?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.RateTypeCode, companyVehicle.RateType == null ? (int?)null : (int)companyVehicle?.RateType.Value);
            facade.SetConcept(CompanyRuleConceptRisk.RiskNumber, companyVehicle.Risk.Policy.Endorsement.Number);
            facade.SetConcept(CompanyRuleConceptRisk.IsTruck, companyVehicle.IsTruck);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyVehicle.Risk.Premium);
            facade.SetConcept(CompanyRuleConceptRisk.AmountInsured, companyVehicle.Risk?.AmountInsured ?? 0);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionTextId, companyVehicle.Risk.Text?.Id == 0 ? null : companyVehicle.Risk.Text?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyVehicle.Risk.Text?.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.CoveragesCount, companyVehicle.Risk.Coverages?.Count);
            facade.SetConcept(CompanyRuleConceptRisk.Coverages, companyVehicle.Risk.Coverages);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyVehicle.Risk.Premium);
            facade.SetConcept(CompanyRuleConceptRisk.FasecoldaCode, companyVehicle.Fasecolda.Description);
            facade.SetConcept(CompanyRuleConceptRisk.Accesories, companyVehicle.Accesories);
            facade.SetConcept(CompanyRuleConceptRisk.CurrentFromRisk, (companyVehicle.Risk.CurrentFrom - companyVehicle.Risk.IssueDate)?.Days);

            if (companyVehicle.Risk.MainInsured != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.InsuredId, companyVehicle.Risk.MainInsured.IndividualId == 0 ? (int?)null : companyVehicle.Risk.MainInsured.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, (int)companyVehicle.Risk.MainInsured.CustomerType);
                facade.SetConcept(CompanyRuleConceptRisk.IsInsuredPayer, companyVehicle.Risk.MainInsured.IndividualId == companyVehicle.Risk.Policy.Holder.IndividualId);
                if (companyVehicle.Risk.MainInsured.IdentificationDocument != null)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredIdentificationDocument, companyVehicle.Risk.MainInsured.IdentificationDocument.Number);
                }
                if (companyVehicle.Risk.MainInsured.IndividualType == IndividualType.Person)
                {
                    int insuredAge = 0;
                    if (companyVehicle.Risk.MainInsured.BirthDate.GetValueOrDefault() > DateTime.MinValue)
                    {
                        insuredAge = (DateTime.Today - companyVehicle.Risk.MainInsured.BirthDate.Value).Days / 365;
                    }
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);


                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, companyVehicle.Risk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, companyVehicle.Risk.MainInsured.BirthDate);
                }
            }
            if (companyVehicle.Risk.Beneficiaries != null && companyVehicle.Risk.Beneficiaries.Count > 0)
            {
                CompanyBeneficiary beneficiary = companyVehicle.Risk.Beneficiaries.First();
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
            if (companyVehicle.Accesories?.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.Accesories, companyVehicle.Accesories);
            }
            // Información de siniestralidad
            if (companyVehicle.Risk?.CompanyClaimsBills != null)
            {
                CompanyClaimsBills claimsBills = companyVehicle.Risk.CompanyClaimsBills;
            }
            if (companyVehicle.Risk?.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in companyVehicle.Risk.DynamicProperties)
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

            facade.SetConcept(CompanyRuleConceptCoverage.RateTypeCode, coverage.RateType == null ? 0 : coverage.RateType.Value);
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
            
            if (coverage.Deductible != null && coverage.Deductible.Id != 0)
            {
                Core.Application.CommonService.Models.Parameter parameterDeductibleDynamic = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.DeductibleDynamic);
                if (parameterDeductibleDynamic != null && parameterDeductibleDynamic.NumberParameter.HasValue)
                {
                    facade.SetConcept(CompanyRuleConceptCoverage.DynamicConcept(parameterDeductibleDynamic.NumberParameter.Value, 84), coverage.Deductible.Id);
                }
            }

            if (coverage.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in coverage.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptCoverage.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

        public static void CreateFacadeRisk(Rules.Facade facade, CompanyRisk companyRisk)
        {
            if (companyRisk != null)
            {
                IMapper imapper = CreateMapFacadeRisk();
                CompanyRuleConceptRisk companyRuleConceptRisk = new CompanyRuleConceptRisk();
                companyRuleConceptRisk = imapper.Map<CompanyRisk, CompanyRuleConceptRisk>(companyRisk);
                facade.SetConcept(CompanyRuleConceptRisk.CoveragesAdd, companyRisk.Coverages.Cast<object>().ToList());
                facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyRisk.Premium);
                if (companyRisk.LimitRc != null)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, companyRisk.LimitRc.LimitSum);
                }
                facade.SetConcept(CompanyRuleConceptRisk.BeneficiaryId, companyRisk.Beneficiaries?.FirstOrDefault().IndividualId ?? 0);
            }
        }

      

        #region GoodExperienceYear

        public static CoTempCptGoodexpyears CreateGoodExperienceYear(Models.GoodExperienceYear entity)
        {
            CoTempCptGoodexpyears goodExperienceYear = null;

            if (entity != null)
            {
                goodExperienceYear = HelperAssembler.CreateObjectMappingEqualProperties<Models.GoodExperienceYear, CoTempCptGoodexpyears>(entity);
            }

            return goodExperienceYear;
        }

        #endregion

        #region Automaper
        public static IMapper CreateMapFacadeRisk()
        {

            IMapper config = MapperCache.GetMapper<CompanyRisk, CompanyRuleConceptRisk>(cfg =>
            {
                cfg.CreateMap<CompanyRisk, CompanyRuleConceptRisk>();
                cfg.CreateMap<CompanyCoverage, object>()
                .ForAllMembers(opt => opt.Ignore());

            });
            return config;
        }
        #endregion Automaper
    }
}