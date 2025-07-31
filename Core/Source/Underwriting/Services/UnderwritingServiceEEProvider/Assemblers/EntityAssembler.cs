using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ENUMUN = Sistran.Core.Application.UnderwritingServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using ISSModels = Sistran.Core.Application.UnderwritingServices.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TAMOD = Sistran.Core.Application.TaxServices.Models;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.Tax.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        #region TempSubscription

        public static TMPEN.TempSubscription CreateTempSubscription(Models.Policy policy)
        {
            return new TMPEN.TempSubscription(policy.Endorsement.TemporalId)
            {
                QuotationId = policy.Endorsement.QuotationId,
                DocumentNumber = policy.DocumentNumber,
                PolicyHolderId = policy.Holder.IndividualId,
                CustomerTypeCode = (int)policy.Holder.CustomerType,
                PrefixCode = policy.Prefix.Id,
                BranchCode = policy.Branch.Id,
                EndorsementTypeCode = (int)policy.Endorsement.EndorsementType,
                CurrencyCode = policy.ExchangeRate.Currency.Id,
                UserId = policy.UserId,
                ExchangeRate = policy.ExchangeRate.SellAmount,
                IsPolicyHolderBill = true,
                IssueDate = policy.IssueDate,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.CurrentTo,
                BeginDate = DateTime.Now,
                CommitDate = DateTime.Now,
                BillingDate = DateTime.Now,
                MailAddressId = policy.Holder.CompanyName.Address.Id,
                SalePointCode = policy.Branch.SalePoints == null ? (int?)null : policy.Branch.SalePoints[0].Id,
                ProductId = policy.Product.Id,
                PolicyId = policy.Endorsement.PolicyId,
                EndorsementId = policy.Endorsement.Id,
                TemporalTypeCode = (int?)policy.TemporalType,
                ConditionText = policy.Text == null ? string.Empty : policy.Text.TextBody,
                CalculateMinPremium = policy.CalculateMinPremium,
                BusinessTypeCode = (int)policy.BusinessType,
                Annotations = policy.Text == null ? string.Empty : policy.Text.Observations,
                CoissuePercentage = policy.CoInsuranceCompanies.First().ParticipationPercentageOwn,
                OperationId = policy.Id,
                PolicyTypeCode = policy.PolicyType.Id,
                RequestId = policy.Request == null ? 0 : policy.Request.Id,
                EffectPeriod = policy.EffectPeriod,
                IsRequest = policy.Request == null ? false : policy.Request.Id > 0 ? true : false
            };
        }

        #endregion

        #region TempSubscriptionAgent

        public static TMPEN.TempSubscriptionAgent CreateTempSubscriptionAgent(IssuanceAgency agency)
        {
            return new TMPEN.TempSubscriptionAgent(0, agency.Agent.IndividualId, agency.Id)
            {
                IsPrimary = agency.IsPrincipal
            };
        }

        #endregion

        #region Crear clausula

        public static TMPEN.TempClause CreateTempClause(Models.PolicyClause clause, int TempId)
        {


            return new TMPEN.TempClause(TempId, clause.Id)
            {
                ClauseId = clause.Id,
                ClauseOrigStatusCode = clause.OriginalClauseStatus,
                ClauseStatusCode = clause.ClauseStatus,
                EndorsementId = clause.EndorsementId,
                TempId = TempId
            };
        }
        public static ISSEN.PolicyClause CreatePolicyClause(Models.PolicyClause clause, int PolicidId)
        {


            return new ISSEN.PolicyClause(PolicidId, clause.EndorsementId, clause.Id)
            {
                ClauseStatusCode = clause.ClauseStatus,
                IsCurrent = clause.IsCurrent


            };
        }

        #endregion

        #region TempRisk

        public static TMPEN.TempRisk CreateTempRisk(Models.Risk risk)
        {
            return new TMPEN.TempRisk
            {
                RiskId = risk.Id,
                InsuredId = 0,
                CustomerTypeCode = 0,
                CoveredRiskTypeCode = 0,
                RiskNumber = 0,
                EndorsementId = 0,
                RiskStatusCode = risk.Status.HasValue ? (int)risk.Status : 0,
                RiskOriginalStatusCode = null,
                RiskInspectionTypeCode = 0,
                InspectionId = 0,
                ConditionText = risk.Text == null ? null : risk.Text.TextBody,
                RatingZoneCode = 0,
                CoverageGroupId = risk.GroupCoverage == null ? (int?)null : risk.GroupCoverage.Id,
                PrefixCode = 0,
                IsFacultative = true,
                RiskCommercialClassCode = 0,
                RiskCommercialTypeCode = 0
            };
        }

        #endregion TempRisk

        #region TempRiskCoverage
        public static TMPEN.TempRiskCoverage CreateTempRiskCoverage(Models.Coverage coverage)
        {
            return new TMPEN.TempRiskCoverage()
            {
                CoverageId = coverage.Id,
                AccumulatedLimitAmount = coverage.AccumulatedLimitAmount,
                AccumulatedPremiumAmount = coverage.AccumulatedPremiumAmount,
                AccumulatedSubLimitAmount = coverage.AccumulatedSubLimitAmount,
                CalculationTypeCode = (int)coverage.CalculationType.GetValueOrDefault(CalculationType.Prorate),
                ConditionText = coverage.Text != null ? coverage.Text.TextBody : null,
                ContractAmountPercentage = coverage.ContractAmountPercentage,
                CoverageNumber = coverage.Number,
                CoverageOriginalStatusCode = coverage.CoverageOriginalStatus.HasValue ? (int)coverage.CoverageOriginalStatus.Value : (int?)null,
                CoverageStatusCode = coverage.CoverStatus.HasValue ? (int)coverage.CoverStatus.Value : (int?)null,
                CurrentFrom = coverage.CurrentFrom,
                CurrentTo = coverage.CurrentTo,
                DeclaredAmount = coverage.DeclaredAmount,
                EndorsementLimitAmount = coverage.EndorsementLimitAmount,
                EndorsementSubLimitAmount = coverage.EndorsementSublimitAmount,
                FlatRatePercentage = coverage.FlatRatePorcentage,
                FirstRiskTypeCode = (int)coverage.FirstRiskType.GetValueOrDefault(Enums.FirstRiskType.None),
                IsDeclarative = coverage.IsDeclarative,
                IsMinimumPremiumDeposit = coverage.IsMinPremiumDeposit,
                LimitAmount = coverage.LimitAmount,
                LimitClaimantAmount = coverage.LimitClaimantAmount,
                LimitOccurrenceAmount = coverage.LimitOccurrenceAmount,
                MainCoverageId = coverage.MainCoverageId,
                PremiumAmount = coverage.PremiumAmount,
                Rate = coverage.Rate,
                RateTypeCode = coverage.RateType.HasValue ? (int)coverage.RateType : 0,
                ShortTermPercentage = coverage.ShortTermPercentage,
                SublimitAmount = coverage.SubLimitAmount,
                DiffMinPremiumAmount = coverage.DiffMinPremiumAmount
            };
        }
        #endregion

        #region BillingGroup
        public static ISSEN.BillingGroup CreateBillingGroup(Models.BillingGroup billingGroup)
        {
            return new ISSEN.BillingGroup(billingGroup.Id)
            {
                BillingGroupCode = billingGroup.Id,
                Description = billingGroup.Description
            };
        }
        #endregion

        #region Facades

        public static void CreateFacadeGeneral(Rules.Facade facade, Policy policy)
        {
            facade.SetConcept(RuleConceptGeneral.TempId, policy.Endorsement.TemporalId);
            facade.SetConcept(RuleConceptGeneral.QuotationId, policy.Endorsement.QuotationId);
            facade.SetConcept(RuleConceptGeneral.DocumentNumber, policy.DocumentNumber == 0 ? (decimal?)null : policy.DocumentNumber);
            facade.SetConcept(RuleConceptGeneral.PrefixCode, policy.Prefix.Id);
            facade.SetConcept(RuleConceptGeneral.EndorsementTypeCode, (int)policy.Endorsement.EndorsementType);
            facade.SetConcept(RuleConceptGeneral.CurrencyCode, policy.ExchangeRate.Currency.Id);
            facade.SetConcept(RuleConceptGeneral.UserId, policy.UserId);
            facade.SetConcept(RuleConceptGeneral.ExchangeRate, policy.ExchangeRate.SellAmount);
            facade.SetConcept(RuleConceptGeneral.IssueDate, policy.IssueDate);
            facade.SetConcept(RuleConceptGeneral.CurrentFrom, policy.CurrentFrom);
            facade.SetConcept(RuleConceptGeneral.CurrentTo, policy.CurrentTo);
            facade.SetConcept(RuleConceptGeneral.BeginDate, DateTime.Now);
            facade.SetConcept(RuleConceptGeneral.BillingDate, DateTime.Now);
            facade.SetConcept(RuleConceptGeneral.BillingGroupCode, policy.BillingGroup?.Id);
            facade.SetConcept(RuleConceptGeneral.ProductId, policy.Product.Id);
            facade.SetConcept(RuleConceptGeneral.PolicyId, policy.Endorsement.PolicyId == 0 ? (int?)null : policy.Endorsement.PolicyId);
            facade.SetConcept(RuleConceptGeneral.EndorsementId, policy.Endorsement.Id == 0 ? (int?)null : policy.Endorsement.Id);
            facade.SetConcept(RuleConceptGeneral.ConditionTextId, policy.Text?.Id ?? 0);
            facade.SetConcept(RuleConceptGeneral.ConditionText, policy.Text?.TextBody);
            facade.SetConcept(RuleConceptGeneral.ConditionTextObservations, policy.Text?.Observations);
            facade.SetConcept(RuleConceptGeneral.BusinessTypeCode, (int)policy.BusinessType);
            facade.SetConcept(RuleConceptGeneral.OperationId, policy.Id);
            facade.SetConcept(RuleConceptGeneral.PolicyTypeCode, policy.PolicyType.Id);
            facade.SetConcept(RuleConceptGeneral.RequestId, policy.Request == null ? (int?)null : policy.Request.Id == 0 ? (int?)null : policy.Request.Id);
            facade.SetConcept(RuleConceptGeneral.PrimaryAgentId, policy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            facade.SetConcept(RuleConceptGeneral.PrimaryAgentCode, policy.Agencies.First(x => x.IsPrincipal).Code);
            facade.SetConcept(RuleConceptGeneral.IsPrimary, policy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            facade.SetConcept(RuleConceptGeneral.ScriptId, policy.Product.ScriptId.GetValueOrDefault());
            facade.SetConcept(RuleConceptGeneral.RuleSetId, policy.Product.RuleSetId);
            facade.SetConcept(RuleConceptGeneral.StandardCommissionPercentage, policy.Product.StandardCommissionPercentage);
            facade.SetConcept(RuleConceptGeneral.IsGreen, policy.Product.IsGreen);
            facade.SetConcept(RuleConceptGeneral.PaymentScheduleId, policy.PaymentPlan?.Id);
            facade.SetConcept(RuleConceptGeneral.CalculateMinPremium, policy.CalculateMinPremium ?? false);
            facade.SetConcept(RuleConceptGeneral.DaysVigency, (policy.CurrentTo - policy.CurrentFrom).Days);

            if (policy.Summary != null)
            {
                facade.SetConcept(RuleConceptGeneral.RisksQuantity, policy.Summary.RiskCount);
                facade.SetConcept(RuleConceptGeneral.AmountInsured, policy.Summary.AmountInsured);
                facade.SetConcept(RuleConceptGeneral.Premium, policy.Summary.Premium);
                facade.SetConcept(RuleConceptGeneral.Expenses, policy.Summary.Expenses);
                facade.SetConcept(RuleConceptGeneral.Taxes, policy.Summary.Taxes);
                facade.SetConcept(RuleConceptGeneral.FullPremium, policy.Summary.FullPremium);
            }

            if (policy.Holder != null)
            {
                facade.SetConcept(RuleConceptGeneral.PolicyHolderId, policy.Holder.IndividualId);
                facade.SetConcept(RuleConceptGeneral.CustomerTypeCode, (int)policy.Holder.CustomerType);
                facade.SetConcept(RuleConceptGeneral.HolderIdentificationDocument, policy.Holder.IdentificationDocument);

                if (policy.Holder.CompanyName.Address != null)
                {
                    facade.SetConcept(RuleConceptGeneral.MailAddressId, policy.Holder.CompanyName.Address.Id);
                }
                if (policy.Holder.IndividualType == IndividualType.Person)
                {
                    int holderAge = 0;

                    if (policy.Holder.BirthDate.GetValueOrDefault() != DateTime.MinValue)
                    {
                        holderAge = (DateTime.Today - policy.Holder.BirthDate.Value).Days / 365;
                    }

                    facade.SetConcept(RuleConceptGeneral.HolderAge, holderAge);
                    facade.SetConcept(RuleConceptGeneral.HolderBirthDate, policy.Holder.BirthDate);
                    facade.SetConcept(RuleConceptGeneral.HolderGender, policy.Holder.Gender);
                }
                facade.SetConcept(RuleConceptGeneral.DocumentoNumberHolder, policy.Holder?.IdentificationDocument);
                facade.SetConcept(RuleConceptGeneral.NameHolder, policy.Holder?.Name);
            }

            if (policy.Branch != null)
            {
                facade.SetConcept(RuleConceptGeneral.BranchCode, policy.Branch.Id);

                if (policy.Branch.SalePoints != null && policy.Branch.SalePoints.Count > 0)
                {
                    facade.SetConcept(RuleConceptGeneral.SalePointCode, policy.Branch.SalePoints.First().Id);
                }
            }

            if (policy.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in policy.DynamicProperties)
                {
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
            if (policy.Risk?.MainInsured != null)
            {
                facade.SetConcept(RuleConceptGeneral.InsuredDocumentNumberOfTheBond, policy.Risk?.MainInsured?.IdentificationDocument);
                facade.SetConcept(RuleConceptGeneral.InsuredNameOfTheBond, policy.Risk?.MainInsured?.Name);
            }
        }

        public static void CreateFacadeRisk(Rules.Facade facade, Risk risk)
        {
            facade.SetConcept(RuleConceptRisk.TempId, risk.Policy.Endorsement.TemporalId);
            facade.SetConcept(RuleConceptRisk.RiskId, risk.RiskId);
            facade.SetConcept(RuleConceptRisk.CoveredRiskTypeCode, (int)risk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(RuleConceptRisk.RiskStatusCode, (int)risk.Status);
            facade.SetConcept(RuleConceptRisk.RiskOriginalStatusCode, risk.OriginalStatus == null ? (int?)null : (int)risk.OriginalStatus);
            facade.SetConcept(RuleConceptRisk.ConditionText, risk.Text == null ? string.Empty : risk.Text.TextBody);
            facade.SetConcept(RuleConceptRisk.RatingZoneCode, risk.RatingZone?.Id == 0 ? null : risk.RatingZone?.Id);
            facade.SetConcept(RuleConceptRisk.CoverageGroupId, risk.GroupCoverage?.Id == 0 ? null : risk.GroupCoverage?.Id);
            facade.SetConcept(RuleConceptRisk.OperationId, risk.Id);
            facade.SetConcept(RuleConceptRisk.LimitsRcCode, risk.LimitRc?.Id == 0 ? null : risk.LimitRc?.Id);
            facade.SetConcept(RuleConceptRisk.LimitsRcSum, risk.LimitRc?.LimitSum == 0 ? null : risk.LimitRc?.LimitSum);

            facade.SetConcept(RuleConceptRisk.ServiceTypeCode, 1);
            facade.SetConcept(RuleConceptRisk.PremiunRisk, risk.Premium);
            facade.SetConcept(RuleConceptRisk.AmountInsured, risk.AmountInsured);
            facade.SetConcept(RuleConceptRisk.ConditionTextId, risk.Text?.Id == 0 ? null : risk.Text?.Id);
            facade.SetConcept(RuleConceptRisk.ConditionText, risk.Text?.TextBody);

            if (risk.MainInsured != null)
            {
                facade.SetConcept(RuleConceptRisk.IsInsuredPayer, risk.MainInsured.IndividualId == risk.Policy.Holder.IndividualId);
                facade.SetConcept(RuleConceptRisk.InsuredIdentificationDocument, risk.MainInsured.IdentificationDocument?.Number);
                facade.SetConcept(RuleConceptRisk.InsuredId, risk.MainInsured.IndividualId == 0 ? (int?)null : risk.MainInsured.IndividualId);
                facade.SetConcept(RuleConceptRisk.CustomerTypeCode, (int)risk.MainInsured.CustomerType);


                if (risk.MainInsured.IndividualType == IndividualType.Person)
                {
                    int insuredAge = 0;

                    if (risk.MainInsured.BirthDate.GetValueOrDefault() > DateTime.MinValue)
                    {
                        insuredAge = (DateTime.Today - risk.MainInsured.BirthDate.Value).Days / 365;
                    }

                    facade.SetConcept(RuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(RuleConceptRisk.InsuredGender, risk.MainInsured.Gender);
                    facade.SetConcept(RuleConceptRisk.InsuredBirthDate, risk.MainInsured.BirthDate);
                }
            }

            if (risk.Beneficiaries != null && risk.Beneficiaries.Count > 0)
            {
                Beneficiary beneficiary = risk.Beneficiaries.First();
                facade.SetConcept(RuleConceptRisk.BeneficiaryId, beneficiary.IndividualId);
                facade.SetConcept(RuleConceptRisk.BeneficiaryPercentage, beneficiary.Participation);
                facade.SetConcept(RuleConceptRisk.BeneficiaryTypeCode, beneficiary.BeneficiaryType);
                facade.SetConcept(RuleConceptRisk.BeneficiaryIdentificationDocument, beneficiary.IdentificationDocument?.Number);
            }

            if (risk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in risk.DynamicProperties)
                {
                    facade.SetConcept(RuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }

        public static void CreateFacadeCoverage(Rules.Facade facade, Coverage coverage)
        {
            facade.SetConcept(RuleConceptCoverage.CoverageId, coverage.Id);
            facade.SetConcept(RuleConceptCoverage.IsDeclarative, coverage.IsDeclarative);
            facade.SetConcept(RuleConceptCoverage.IsMinimumPremiumDeposit, coverage.IsMinPremiumDeposit);
            facade.SetConcept(RuleConceptCoverage.FirstRiskTypeCode, (int)coverage.FirstRiskType.GetValueOrDefault());
            facade.SetConcept(RuleConceptCoverage.CalculationTypeCode, (int)coverage.CalculationType.GetValueOrDefault());
            facade.SetConcept(RuleConceptCoverage.DeclaredAmount, coverage.DeclaredAmount == 0 ? (decimal?)null : coverage.DeclaredAmount);
            facade.SetConcept(RuleConceptCoverage.PremiumAmount, coverage.PremiumAmount == 0 ? (decimal?)null : coverage.PremiumAmount);
            facade.SetConcept(RuleConceptCoverage.LimitAmount, coverage.LimitAmount == 0 ? (decimal?)null : coverage.LimitAmount);
            facade.SetConcept(RuleConceptCoverage.SubLimitAmount, coverage.SubLimitAmount == 0 ? (decimal?)null : coverage.SubLimitAmount);
            facade.SetConcept(RuleConceptCoverage.LimitInExcess, coverage.ExcessLimit == 0 ? (decimal?)null : coverage.ExcessLimit);
            facade.SetConcept(RuleConceptCoverage.LimitOccurrenceAmount, coverage.LimitOccurrenceAmount);
            facade.SetConcept(RuleConceptCoverage.LimitClaimantAmount, coverage.LimitClaimantAmount);
            facade.SetConcept(RuleConceptCoverage.AccumulatedLimitAmount, coverage.AccumulatedLimitAmount == 0 ? (decimal?)null : coverage.AccumulatedLimitAmount);
            facade.SetConcept(RuleConceptCoverage.AccumulatedSubLimitAmount, coverage.AccumulatedSubLimitAmount);
            facade.SetConcept(RuleConceptCoverage.CurrentFrom, coverage.CurrentFrom);
            facade.SetConcept(RuleConceptCoverage.RateTypeCode, (int)coverage.RateType.GetValueOrDefault());
            facade.SetConcept(RuleConceptCoverage.Rate, coverage.Rate);
            facade.SetConcept(RuleConceptCoverage.CurrentTo, coverage.CurrentTo);
            facade.SetConcept(RuleConceptCoverage.MainCoverageId, coverage.MainCoverageId);
            facade.SetConcept(RuleConceptCoverage.MainCoveragePercentage, coverage.MainCoveragePercentage);
            facade.SetConcept(RuleConceptCoverage.CoverageNumber, coverage.Number == 0 ? (int?)null : coverage.Number);
            facade.SetConcept(RuleConceptCoverage.RiskCoverageId, coverage.RiskCoverageId == 0 ? (int?)null : coverage.RiskCoverageId);
            facade.SetConcept(RuleConceptCoverage.CoverageStatusCode, coverage.CoverStatus == null ? (int?)null : (int)coverage.CoverStatus.Value);
            facade.SetConcept(RuleConceptCoverage.CoverageOriginalStatusCode, coverage.CoverageOriginalStatus == null ? (int?)null : (int)coverage.CoverageOriginalStatus.Value);
            facade.SetConcept(RuleConceptCoverage.ConditionText, coverage.Text == null ? string.Empty : coverage.Text.TextBody);
            facade.SetConcept(RuleConceptCoverage.ConditionTextId, coverage.Text?.Id == 0 ? null : coverage.Text?.Id);
            facade.SetConcept(RuleConceptCoverage.MaxLiabilityAmount, coverage.MaxLiabilityAmount == 0 ? (decimal?)null : coverage.MaxLiabilityAmount);
            facade.SetConcept(RuleConceptCoverage.MinimumPremiumCoverage, coverage.MinimumPremiumCoverage);

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
                facade.SetConcept(RuleConceptCoverage.DeductRateTypeCode, coverage.Deductible.RateType);
                facade.SetConcept(RuleConceptCoverage.DeductRate, coverage.Deductible.Rate);
                facade.SetConcept(RuleConceptCoverage.DeductPremiumAmount, coverage.Deductible.DeductPremiumAmount);
                facade.SetConcept(RuleConceptCoverage.DeductValue, coverage.Deductible.DeductValue);
                facade.SetConcept(RuleConceptCoverage.DeductUnitCode, coverage.Deductible.DeductibleUnit?.Id);
                facade.SetConcept(RuleConceptCoverage.DeductSubjectCode, coverage.Deductible.DeductibleSubject?.Id);
                facade.SetConcept(RuleConceptCoverage.MinDeductValue, coverage.Deductible.MinDeductValue);
                facade.SetConcept(RuleConceptCoverage.MinDeductUnitCode, coverage.Deductible.MinDeductibleUnit?.Id);
                facade.SetConcept(RuleConceptCoverage.MinDeductSubjectCode, coverage.Deductible.MinDeductibleSubject?.Id);
                facade.SetConcept(RuleConceptCoverage.MaxDeductValue, coverage.Deductible.MaxDeductValue);
                facade.SetConcept(RuleConceptCoverage.MaxDeductUnitCode, coverage.Deductible.MaxDeductibleUnit?.Id);
                facade.SetConcept(RuleConceptCoverage.MaxDeductSubjectCode, coverage.Deductible.MaxDeductibleSubject?.Id);
                facade.SetConcept(RuleConceptCoverage.CurrencyCode, coverage.Deductible.Currency?.Id);
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

        public static void CreateFacadeComponent(Rules.Facade facade, Models.PayerComponent payerComponent)
        {
            facade.SetConcept(RuleConceptComponent.ComponentCode, payerComponent.Component.Id);
            facade.SetConcept(RuleConceptComponent.RateTypeCode, (int)payerComponent.RateType);
            facade.SetConcept(RuleConceptComponent.Rate, payerComponent.Rate);
            facade.SetConcept(RuleConceptComponent.CalculationBaseAmount, payerComponent.BaseAmount);
        }
        #endregion Facades
        #region Gastos

        public static QUOEN.Component CreateComponent(Models.Expense Expenses)
        {
            return new QUOEN.Component(Expenses.id)
            {
                SmallDescription = Expenses.Description,
                TinyDescription = Expenses.Abbreviation,
                ComponentClassCode = Expenses.ComponentClass,
                ComponentTypeCode = Expenses.ComponentType
            };
        }

        public static QUOEN.ExpenseComponent CreateExpenseComponent(Models.Expense Expenses)
        {
            return new QUOEN.ExpenseComponent(Expenses.id)
            {
                RateTypeCode = Expenses.RateType,
                Rate = Expenses.Rate,
                IsMandatory = Expenses.Mandatory,
                IsInitially = Expenses.InitiallyIncluded,
                RuleSetId = Expenses.RuleSet
            };
        }

        #endregion
        #region adicional
        #region limiteRC
        public static COMMEN.CoLimitsRcRel CreateLimitRCRelation(ISSModels.LimitRCRelation model, int prefixCode, int productId)
        {
            return new COMMEN.CoLimitsRcRel(prefixCode, model.PolicyType.Id, productId, model.Id)
            {
                IsDefault = model.IsDefault,
            };
        }
        #endregion
        #region GroupCoverage
        public static PRODEN.GroupCoverage CreateGroupCoverageByCoverage(Models.GroupCoverage groupCoverage, Models.Coverage coverage)
        {
            return new PRODEN.GroupCoverage(coverage.Id, groupCoverage.Product.Id, groupCoverage.Id)
            {
                CoverGroupId = groupCoverage.Id,
                ProductId = groupCoverage.Product.Id,
                CoverageId = coverage.Id,
                IsMandatory = coverage.IsMandatory,
                IsSelected = coverage.IsSelected,
                CoverNum = coverage.Number,
                RuleSetId = coverage.RuleSetId,
                PosRuleSetId = coverage.PosRuleSetId,
                ScriptId = coverage.ScriptId,
                MainCoverageId = coverage.MainCoverageId
            };
        }
        #endregion

        #region CreateProductGroupInsuredObject
        public static PRODEN.GroupInsuredObject CreateProductGroupInsuredObject(InsuredObject insuredObject, Models.GroupCoverage groupCoverage, int productId)
        {
            return new PRODEN.GroupInsuredObject(productId, groupCoverage.Id, insuredObject.Id)
            {
                IsMandatory = insuredObject.IsMandatory,
                IsSelected = insuredObject.IsSelected
            };
        }
        #endregion
        #region CreateProductGroupCover
        public static PRODEN.ProductGroupCover CreateProductGroupCover(Models.GroupCoverage groupCoverage)
        {
            return new PRODEN.ProductGroupCover(groupCoverage.Product.Id, groupCoverage.Id)
            {
                CoverGroupId = groupCoverage.Id,
                SmallDescription = groupCoverage.Description,
                PrefixCode = groupCoverage.Product.Prefix.Id,
                CoveredRiskTypeCode = (int)groupCoverage.CoveredRiskType
            };
        }
        #endregion
        #region RiskCommercialClass
        public static PARAMEN.RiskCommercialClass CreateRiskCommercialClass(Models.RiskCommercialClass riskCommercialClass)
        {
            return new PARAMEN.RiskCommercialClass(riskCommercialClass.RiskCommercialClassCode)
            {
                Description = riskCommercialClass.Description,
                SmallDescription = riskCommercialClass.SmallDescription,
                Enabled = riskCommercialClass.Enabled
            };
        }
        #endregion
        #endregion

        #region VehicleType

        public static COMMEN.VehicleType CreateVehicleType(VehicleType vehicleType)
        {
            return new COMMEN.VehicleType(vehicleType.Id)
            {
                Description = vehicleType.Description,
                SmallDescription = vehicleType.SmallDescription,
                Enabled = vehicleType.IsActive,
                IsTruck = vehicleType.IsTruck,
                ExtendedProperties = CreateExtendedProperties(vehicleType.ExtendedProperties)
            };
        }

        #endregion

        #region VehicleBody

        /// <summary>
        /// Crea la entidad a partir del modelo 
        /// </summary>
        /// <param name="vehicleBody">Carrocería de vehículo</param>
        /// <returns>Entidad de Carrocería de vehículo</returns>
        public static COMMEN.VehicleBody CreateVehicleBody(VehicleBody vehicleBody)
        {
            return new COMMEN.VehicleBody(vehicleBody.Id)
            {
                SmallDescription = vehicleBody.SmallDescription
            };
        }

        /// <summary>
        /// Crea la entidad a partir del modelo
        /// </summary>
        /// <param name="vehicleTypeCode">Id del tipo de vehiculo</param>
        /// <param name="vehicleBodies">Listado de tipo de carrocerias/param>
        /// <returns>Listado de entidad de carrocerias/returns>
        public static List<COMMEN.VehicleTypeBody> CreateVehicleTypeBody(int vehicleTypeCode, List<VehicleBody> vehicleBodies)
        {
            List<COMMEN.VehicleTypeBody> vehicleTypeBodies = new List<COMMEN.VehicleTypeBody>();
            if (vehicleBodies != null)
            {
                foreach (VehicleBody item in vehicleBodies)
                {
                    vehicleTypeBodies.Add(new COMMEN.VehicleTypeBody(vehicleTypeCode, item.Id));
                }
            }
            return vehicleTypeBodies;
        }

        #endregion

        #region VehicleUse

        /// <summary>
        /// Crea la entidad a partir del modelo
        /// </summary>
        /// <param name="vehicleTypeCode">Id de Carrocería de vehículo</param>
        /// <param name="vehicleUses">Listado de Usos/param>
        /// <returns>Listado de Usos/returns>
        public static List<COMMEN.VehicleBodyUse> CreateVehicleBodyUse(int vehicleTypeCode, List<VehicleUse> vehicleUses)
        {
            List<COMMEN.VehicleBodyUse> vehicleBodyUses = new List<COMMEN.VehicleBodyUse>();
            if (vehicleUses != null)
            {
                foreach (VehicleUse item in vehicleUses)
                {
                    vehicleBodyUses.Add(new COMMEN.VehicleBodyUse(vehicleTypeCode, item.Id));
                }
            }
            return vehicleBodyUses;
        }

        #endregion

        #region ExtendedProperty

        private static List<Framework.DAF.ExtendedProperty> CreateExtendedProperties(List<Extensions.ExtendedProperty> extendedProperties)
        {
            List<Framework.DAF.ExtendedProperty> entityExtendedProperties = new List<Framework.DAF.ExtendedProperty>();

            if (extendedProperties != null)
            {
                foreach (Extensions.ExtendedProperty extendedProperty in extendedProperties)
                {
                    entityExtendedProperties.Add(new Framework.DAF.ExtendedProperty
                    {
                        Name = extendedProperty.Name,
                        Value = extendedProperty.Value
                    });
                }
            }

            return entityExtendedProperties;
        }

        #endregion

        #region CoCoverage
        public static QUOEN.CoCoverageValue CreateParamCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            return new QUOEN.CoCoverageValue(paramCoCoverageValue.Prefix.Id, paramCoCoverageValue.Coverage.Id)
            {
                PrefixCode = paramCoCoverageValue.Prefix.Id,
                CoverageId = paramCoCoverageValue.Coverage.Id,
                ValuePje = paramCoCoverageValue.Percentage
            };
        }
        #endregion

        #region Condition Text
        public static QUOEN.ConditionText CreateParamConditionText(ParamConditionText conditionText)
        {
            QUOEN.ConditionText EntityConditionText = new QUOEN.ConditionText()
            {
                ConditionTextId = conditionText.Id,
                ConditionLevelCode = conditionText.ConditionTextLevel.Id,
                TextTitle = conditionText.Title,
                TextBody = conditionText.Body,
            };

            return EntityConditionText;
        }

        public static QUOEN.CondTextLevel CreateParamConditionTextLevel(ParamConditionText conditionText)
        {
            QUOEN.CondTextLevel EntityConditionTextlevel = new QUOEN.CondTextLevel()
            {
                ConditionTextId = conditionText.Id,
                ConditionLevelId = conditionText.ConditionTextLevelType.Id,
                IsAutomatic = false
            };

            return EntityConditionTextlevel;
        }


        #endregion

        #region Tax

        internal static Tax.Entities.Tax CreateParamTax(ParamTax paramTax)
        {
            Tax.Entities.Tax entityTax = new Tax.Entities.Tax()
            {
                TaxCode = paramTax.Id,
                Description = paramTax.Description,
                SmallDescription = paramTax.TinyDescription,
                RateTypeCode = paramTax.RateType.Id,
                AdditionalRateTypeCode = paramTax.AdditionalRateType.Id > 0 ? paramTax.AdditionalRateType.Id : (int?)null,
                IsSurplus = paramTax.IsSurPlus,
                IsAdditionalSurplus = paramTax.IsAdditionalSurPlus,
                CurrentFrom = paramTax.CurrentFrom,
                Enabled = paramTax.Enabled,
                IsEarned = paramTax.IsEarned,
                IsRetention = paramTax.IsRetention,
                RetentionTaxCode = paramTax.RetentionTax.Id > 0 ? paramTax.RetentionTax.Id : (int?)null,
                BaseConditionTaxCode = paramTax.BaseConditionTax.Id > 0 ? paramTax.BaseConditionTax.Id : (int?)null,
                TaxRepaymentTypeCode = (int)ENUMUN.RepaimentTax.Unica,
                DevolutionTaxTypeCode = (int)ENUMUN.RepaimentTax.Unica,
            };
            return entityTax;
        }

        internal static Tax.Entities.TaxRate CreateParamTaxRate(ParamTaxRate paramTaxRate)
        {
            Tax.Entities.TaxRate entityTaxRate = new Tax.Entities.TaxRate(paramTaxRate.Id)
            {
                TaxCode = paramTaxRate.IdTax,
                TaxConditionCode = paramTaxRate.TaxCondition.Id > 0 ? paramTaxRate.TaxCondition.Id : (int?)null,
                TaxCategoryCode = paramTaxRate.TaxCategory.Id > 0 ? paramTaxRate.TaxCategory.Id : (int?)null,
                LineBusinessCode = paramTaxRate.LineBusiness.Id > 0 ? paramTaxRate.LineBusiness.Id : (int?)null,
                StateCode = paramTaxRate.TaxState.IdState > 0 ? paramTaxRate.TaxState.IdState : (int?)null,
                CountryCode = paramTaxRate.TaxState.IdCountry > 0 ? paramTaxRate.TaxState.IdCountry : (int?)null,
                EconomicActivityTaxCode = paramTaxRate.EconomicActivity.Id > 0 ? paramTaxRate.EconomicActivity.Id : (int?)null,
                BranchCode = paramTaxRate.Branch.Id > 0 ? paramTaxRate.Branch.Id : (int?)null,
                CoverageId = paramTaxRate.Coverage.Id > 0 ? paramTaxRate.Coverage.Id : (int?)null,
                CityCode = paramTaxRate.TaxState.IdCity > 0 ? paramTaxRate.TaxState.IdCity : (int?)null
            };
            return entityTaxRate;
        }

        internal static Tax.Entities.TaxPeriodRate CreateParamTaxPeriodRate(TAMOD.TaxPeriodRate paramTaxPeriodRate)
        {
            Tax.Entities.TaxPeriodRate entityTaxPeriodRate = new Tax.Entities.TaxPeriodRate(paramTaxPeriodRate.Id, paramTaxPeriodRate.CurrentFrom)
            {
                TaxRateId = paramTaxPeriodRate.Id,
                CurrentFrom = paramTaxPeriodRate.CurrentFrom,
                Rate = paramTaxPeriodRate.Rate,
                AdditionalRate = paramTaxPeriodRate.AdditionalRate,
                BaseTaxIncInAdditional = paramTaxPeriodRate.BaseTaxAdditional,
                MinBaseAmount = paramTaxPeriodRate.MinBaseAMT,
                MinAdditionalBaseAmount = paramTaxPeriodRate.MinAdditionalBaseAMT,
                MinTaxAmount = paramTaxPeriodRate.MinTaxAMT,
                MinAdditionalTaxAmount = paramTaxPeriodRate.MinAdditionalTaxAMT
            };
            return entityTaxPeriodRate;
        }

        internal static Tax.Entities.TaxCategory CreateParamTaxCategory(ParamTaxCategory paramTaxCategory)
        {
            Tax.Entities.TaxCategory entityTaxCategory = new Tax.Entities.TaxCategory(paramTaxCategory.IdTax, paramTaxCategory.Id)
            {
                TaxCategoryCode = paramTaxCategory.Id,
                TaxCode = paramTaxCategory.IdTax,
                Description = paramTaxCategory.Description
            };
            return entityTaxCategory;
        }

        internal static Tax.Entities.TaxCondition CreateParamTaxCondition(ParamTaxCondition paramTaxCondition)
        {
            Tax.Entities.TaxCondition entityTaxCondition = new Tax.Entities.TaxCondition(paramTaxCondition.IdTax, paramTaxCondition.Id)
            {
                TaxConditionCode = paramTaxCondition.Id,
                TaxCode = paramTaxCondition.IdTax,
                Description = paramTaxCondition.Description,
                HasNationalRate = paramTaxCondition.HasNationalRate,
                IsIndependent = paramTaxCondition.IsIndependent,
                IsDefault = paramTaxCondition.IsDefault
            };
            return entityTaxCondition;
        }
        #endregion
    }
}
