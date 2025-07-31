using Sistran.Company.Application.UnderwritingServices.Models;
//using Sistran.Company.Application.Utilities.RulesEngine.Facades;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.RulesEngine;
using ISSMODEL = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Issuance.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {

        public static void CreateFacadeGeneral(CompanyPolicy companyPolicy, Rules.Facade facade)
        {
            facade.SetConcept(CompanyRuleConceptPolicies.UserId, companyPolicy.UserId);
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

            if (companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.ChangeAgentEndorsement ||
                companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.ChangeCoinsuranceEndorsement ||
                companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.ChangeConsolidationEndorsement ||
                companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.ChangePolicyHolderEndorsement)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.DaysRetroactivityPosterity, (companyPolicy.Endorsement.CurrentFrom.Date - companyPolicy.IssueDate.Date).Days);
            }
            else
            {
                facade.SetConcept(CompanyRuleConceptGeneral.DaysRetroactivityPosterity, (companyPolicy.CurrentFrom.Date - companyPolicy.IssueDate.Date).Days);
            }

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
            
            if ((companyPolicy.Endorsement.IsUnderIdenticalConditions==false && companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Renewal )|| companyPolicy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Emission)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextObservations, companyPolicy.Text?.Observations);
                facade.SetConcept(CompanyRuleConceptGeneral.ConditionText, companyPolicy.Text?.TextBody);
            }
            else {
                facade.SetConcept(CompanyRuleConceptGeneral.ConditionTextObservations, companyPolicy.Endorsement.Text?.Observations);
                facade.SetConcept(CompanyRuleConceptGeneral.ConditionText, companyPolicy.Endorsement.Text?.TextBody);
            }

            facade.SetConcept(CompanyRuleConceptGeneral.BusinessTypeCode, companyPolicy.BusinessType == null ? 0 : (int)companyPolicy.BusinessType);
            facade.SetConcept(CompanyRuleConceptGeneral.OperationId, companyPolicy.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyTypeCode, companyPolicy?.PolicyType?.Id ?? 0);
            facade.SetConcept(CompanyRuleConceptGeneral.RequestId, companyPolicy.Request == null ? null : companyPolicy.Request.Id == 0 ? (int?)null : companyPolicy.Request.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.EffectPeriod, companyPolicy.EffectPeriod);
            facade.SetConcept(CompanyRuleConceptGeneral.IsRequest, companyPolicy.Request == null ? false : companyPolicy.Request.Id == 0 ? false : true);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentId, companyPolicy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentAgencyId, companyPolicy.Agencies.First(x => x.IsPrincipal).Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCode, companyPolicy.Agencies.First(x => x.IsPrincipal).Code);
            facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCodePrincipal, companyPolicy.Agencies.First(p => p.IsPrincipal).Code);
            facade.SetConcept(CompanyRuleConceptGeneral.IsPrimary, companyPolicy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            facade.SetConcept(CompanyRuleConceptGeneral.ScriptId, companyPolicy.Product.ScriptId.GetValueOrDefault());
            facade.SetConcept(CompanyRuleConceptGeneral.PreRuleSetId, companyPolicy.Product.PreRuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.RuleSetId, companyPolicy.Product.RuleSetId);
            facade.SetConcept(CompanyRuleConceptGeneral.ProcessType, companyPolicy.SubMassiveProcessType);
            facade.SetConcept(CompanyRuleConceptGeneral.TotalRisk, companyPolicy.TotalRisk);
            facade.SetConcept(CompanyRuleConceptGeneral.StandardCommissionPercentage, companyPolicy?.Product?.StandardCommissionPercentage);

            if (companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions != null && companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions.Count > 0 && companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions.First() != null)
            {
                decimal? StandardCommissionPercentage = companyPolicy.Agencies?.FirstOrDefault(x => x.IsPrincipal)?.Commissions?.FirstOrDefault()?.Percentage;
                facade.SetConcept(CompanyRuleConceptGeneral.StandardCommissionPercentage, StandardCommissionPercentage);
            }

            facade.SetConcept(CompanyRuleConceptGeneral.IsFlatRate, companyPolicy.Product.IsFlatRate);
            facade.SetConcept(CompanyRuleConceptGeneral.IsCollective, companyPolicy.Product.IsCollective);
            facade.SetConcept(CompanyRuleConceptGeneral.IsGreen, companyPolicy.Product.IsGreen);
            facade.SetConcept(CompanyRuleConceptGeneral.DaysVigency, (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days);
            facade.SetConcept(CompanyRuleConceptGeneral.HasTotalLoss, companyPolicy.HasTotalLoss);
            facade.SetConcept(CompanyRuleConceptGeneral.SinisterQuantity, companyPolicy.SinisterQuantity);
            facade.SetConcept(CompanyRuleConceptGeneral.PortfolioBalance, companyPolicy.PortfolioBalance);
            facade.SetConcept(CompanyRuleConceptGeneral.AgentType, companyPolicy?.Agencies?.FirstOrDefault(x => x.IsPrincipal)?.Agent?.AgentType?.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PaymentScheduleId, companyPolicy.PaymentPlan?.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.CalculateMinPremium, companyPolicy.CalculateMinPremium ?? false);
            facade.SetConcept(CompanyRuleConceptGeneral.UserProfileId, companyPolicy.User?.UserProfileId);
            facade.SetConcept(CompanyRuleConceptGeneral.SinisterQuantity, companyPolicy.SinisterQuantity);

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
                facade.SetConcept(CompanyRuleConceptGeneral.EmailElectronicBilling, companyPolicy.Holder.Email != null ? companyPolicy.Holder.Email : null);
                facade.SetConcept(CompanyRuleConceptGeneral.RegimeType, companyPolicy.Holder.RegimeType);
                facade.SetConcept(CompanyRuleConceptGeneral.fiscalResponsibility, companyPolicy.Holder.FiscalResponsibility != null ? companyPolicy.Holder.FiscalResponsibility.Count(x => x.Id > 0) : 0);

                // if (companyPolicy.Holder.IndividualType == IndividualType.Company)
                if (companyPolicy.Holder.AssociationType != null && companyPolicy.Holder.AssociationType.Id != null && companyPolicy.Holder.AssociationType.Id> 0)
                { 
                    facade.SetConcept(CompanyRuleConceptGeneral.AssociationType, companyPolicy.Holder.AssociationType.Id);
                }

                facade.SetConcept(CompanyRuleConceptGeneral.TypeOfHolderDocument, companyPolicy.Holder?.IdentificationDocument.DocumentType.Id);
                facade.SetConcept(CompanyRuleConceptGeneral.DocumentoNumberHolder, companyPolicy?.Holder?.IdentificationDocument?.Number);
                facade.SetConcept(CompanyRuleConceptGeneral.IndividualNumberHolder, companyPolicy?.Holder?.IndividualId);
                facade.SetConcept(CompanyRuleConceptGeneral.NameHolder, companyPolicy?.Holder?.Name);

                facade.SetConcept(CompanyRuleConceptGeneral.CoinsuranceExpensesAccepted, companyPolicy?.CoInsuranceCompanies?.First().ExpensesPercentage);
                facade.SetConcept(CompanyRuleConceptGeneral.CoinsuranceExpensesCeded, companyPolicy?.CoInsuranceCompanies?.First().ExpensesPercentage);
            }

            if (companyPolicy.Branch != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.BranchCode, companyPolicy.Branch.Id);

                if (companyPolicy.Branch.SalePoints != null && companyPolicy.Branch.SalePoints.Count > 0)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.SalePointCode, companyPolicy.Branch.SalePoints.First().Id);
                }
                facade.SetConcept(CompanyRuleConceptGeneral.GroupBranchId, companyPolicy.Branch.GroupBranchId);
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
            int? paramter = DelegateService.commonService.GetParameterByParameterId(parameterTempSubscription).NumberParameter;


            if (paramter != null)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.TempId, paramter.Value);
            }
            if (companyPolicy.Agencies.Count > 0)
            {
                facade.SetConcept(CompanyRuleConceptGeneral.AgentType, companyPolicy?.Agencies[0]?.Agent?.AgentType?.Id);
            }
        }
        internal static InsuredObject CreateInsuredObject(CompanyInsuredObject companyInsuredObject)
        {
            return new InsuredObject(companyInsuredObject.Id)
            {
                Description = companyInsuredObject.Description,
                SmallDescription = companyInsuredObject.SmallDescription,
                IsDeclarative = companyInsuredObject.IsDeclarative
            };
        }

        internal static TMPEN.TempSubscription CreateTempSubscription(CompanyPolicy companyPolicy)
        {
            TMPEN.TempSubscription entityTempSubscription = new TMPEN.TempSubscription
            {
                PolicyHolderId = companyPolicy.Holder.IndividualId,
                CustomerTypeCode = (int)companyPolicy.Holder.CustomerType,
                PrefixCode = companyPolicy.Prefix.Id,
                BranchCode = companyPolicy.Branch.Id,
                EndorsementTypeCode = (int)companyPolicy.Endorsement.EndorsementType,
                CurrencyCode = companyPolicy.ExchangeRate.Currency.Id,
                UserId = companyPolicy.UserId,
                ExchangeRate = companyPolicy.ExchangeRate.SellAmount,
                IsPolicyHolderBill = true,
                IssueDate = companyPolicy.IssueDate,
                CurrentFrom = companyPolicy.CurrentFrom,
                CurrentTo = companyPolicy.CurrentTo,
                BeginDate = DateTime.Now,
                CommitDate = DateTime.Now,
                MailAddressId = companyPolicy.Holder.CompanyName.Address.Id,
                ProductId = companyPolicy.Product.Id,
                TemporalTypeCode = (int)companyPolicy.TemporalType,
                EndoGroupId = 1,
                CalculateMinPremium = companyPolicy.CalculateMinPremium == null ? false : companyPolicy.CalculateMinPremium,
                BusinessTypeCode = (int)companyPolicy.BusinessType,
                CoissuePercentage = companyPolicy.CoInsuranceCompanies[0].ParticipationPercentageOwn,
                OperationId = companyPolicy.Id
            };

            if (companyPolicy.Endorsement.TemporalId > 0)
            {
                entityTempSubscription.TempId = companyPolicy.Endorsement.TemporalId;
            }

            if (companyPolicy.Endorsement.QuotationId > 0)
            {
                entityTempSubscription.QuotationId = companyPolicy.Endorsement.QuotationId;
                entityTempSubscription.QuotationVersion = companyPolicy.Endorsement.QuotationVersion == 0 ? 1 : companyPolicy.Endorsement.QuotationVersion;
            }

            if (companyPolicy.DocumentNumber > 0)
            {
                entityTempSubscription.DocumentNumber = companyPolicy.DocumentNumber;
                entityTempSubscription.PolicyId = companyPolicy.Endorsement.PolicyId;
                entityTempSubscription.EndorsementId = companyPolicy.Endorsement.Id;
            }

            if (companyPolicy.Branch.SalePoints != null && companyPolicy.Branch.SalePoints.Count > 0 && companyPolicy.Branch.SalePoints[0].Id > 0)
            {
                entityTempSubscription.SalePointCode = companyPolicy.Branch.SalePoints[0].Id;
            }

            if (companyPolicy.Endorsement.EndorsementReasonId > 0)
            {
                entityTempSubscription.EndoReasonCode = companyPolicy.Endorsement.EndorsementReasonId;
            }

            return entityTempSubscription;
        }
        #region riesgo
        public static void CreatefacadeRisk(CompanyRisk companyRisk, Rules.Facade facade)
        {

            facade.SetConcept(CompanyRuleConceptRisk.RiskId, companyRisk.RiskId);
            facade.SetConcept(CompanyRuleConceptRisk.CoveredRiskTypeCode, (int)companyRisk.Policy.Product.CoveredRisk.CoveredRiskType);
            facade.SetConcept(CompanyRuleConceptRisk.RiskStatusCode, companyRisk.Status == null ? (int?)null : (int)companyRisk.Status);
            facade.SetConcept(CompanyRuleConceptRisk.TempId, companyRisk.Policy.Endorsement.TemporalId);
            facade.SetConcept(CompanyRuleConceptRisk.RiskOriginalStatusCode, companyRisk.OriginalStatus == null ? (int?)null : (int)companyRisk.OriginalStatus);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyRisk.Text == null ? string.Empty : companyRisk.Text.TextBody);
            facade.SetConcept(CompanyRuleConceptRisk.RatingZoneCode, companyRisk.RatingZone?.Id == 0 ? null : companyRisk.RatingZone?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.CoverageGroupId, companyRisk.GroupCoverage?.Id == 0 ? null : companyRisk.GroupCoverage?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.OperationId, companyRisk.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcCode, companyRisk.LimitRc?.Id == 0 ? null : companyRisk.LimitRc?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.LimitsRcSum, companyRisk.LimitRc?.LimitSum == 0 ? null : companyRisk.LimitRc?.LimitSum);
            facade.SetConcept(CompanyRuleConceptRisk.RiskNumber, companyRisk.Policy.Endorsement.Number);
            facade.SetConcept(CompanyRuleConceptRisk.PremiunRisk, companyRisk.Premium);
            facade.SetConcept(CompanyRuleConceptRisk.AmountInsured, companyRisk.AmountInsured);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionTextId, companyRisk.Text?.Id == 0 ? null : companyRisk.Text?.Id);
            facade.SetConcept(CompanyRuleConceptRisk.ConditionText, companyRisk.Text?.TextBody);
            //facade.SetConcept(CompanyRuleConceptRisk.OnerousBeneficiaries, companyRisk.Beneficiaries != null ? companyRisk.Beneficiaries.Count(x => x.BeneficiaryType != null && (int)x.BeneficiaryType.Id == Core.Application.Utilities.Configuration.KeySettings.OnerousBeneficiaryTypeId) : 0);
            //facade.SetConcept(CompanyRuleConceptRisk.NoOnerousBeneficiaries, companyRisk.Beneficiaries != null ? companyRisk.Beneficiaries.Count(x => x.BeneficiaryType != null && (int)x.BeneficiaryType.Id == Core.Application.Utilities.Configuration.KeySettings.NotOnerousBeneficiaryTypeId) : 0);

            if (companyRisk.MainInsured != null)
            {
                facade.SetConcept(CompanyRuleConceptRisk.InsuredId, companyRisk.MainInsured.IndividualId == 0 ? (int?)null : companyRisk.MainInsured.IndividualId);
                facade.SetConcept(CompanyRuleConceptRisk.CustomerTypeCode, (int)companyRisk.MainInsured.CustomerType);

                facade.SetConcept(CompanyRuleConceptRisk.IsInsuredPayer, companyRisk.MainInsured.IndividualId == companyRisk.Policy.Holder.IndividualId);
                if (companyRisk.MainInsured.IdentificationDocument != null)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredIdentificationDocument, companyRisk.MainInsured.IdentificationDocument.Number);
                }

                if (companyRisk.MainInsured.IndividualType == IndividualType.Person)
                {
                    int insuredAge = 0;

                    if (companyRisk.MainInsured.BirthDate.GetValueOrDefault() > DateTime.MinValue)
                    {
                        insuredAge = (DateTime.Today - companyRisk.MainInsured.BirthDate.Value).Days / 365;
                    }

                    facade.SetConcept(CompanyRuleConceptRisk.InsuredAge, insuredAge);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredGender, companyRisk.MainInsured.Gender);
                    facade.SetConcept(CompanyRuleConceptRisk.InsuredBirthDate, companyRisk.MainInsured.BirthDate);
                }
            }

            if (companyRisk.Beneficiaries != null && companyRisk.Beneficiaries.Count > 0)
            {
                CompanyBeneficiary beneficiary = companyRisk.Beneficiaries.First();
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

            if (companyRisk.CompanyClaimsBills != null)
            {
                var claimsBills = companyRisk.CompanyClaimsBills;
                //  facade.SetConcept(CompanyRuleConceptRisk.HasSinister, claimsBills.SinisterQuantity > 0);
            }

            if (companyRisk.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in companyRisk.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }

        }

        #endregion
        #region componente
        public static void CreateFacadeComponent(Rules.Facade facade, ISSMODEL.PayerComponent payerComponent)
        {
            facade.SetConcept(RuleConceptComponent.ComponentCode, payerComponent.Component.Id);
            facade.SetConcept(RuleConceptComponent.RateTypeCode, (int)payerComponent.RateType);
            facade.SetConcept(RuleConceptComponent.Rate, payerComponent.Rate);
            facade.SetConcept(RuleConceptComponent.CalculationBaseAmount, payerComponent.BaseAmount);
        }
        #endregion componente


        #region CiaRatingZoneBranch

        public static CiaRatingZoneBranch CreateCiaRatingZoneBranch(CiaRatingZoneBranch ciaRatingZoneBranch)
        {
            return null; // new CiaRatingZoneBranch(ciaRatingZoneBranch.RatingZone.Id, ciaRatingZoneBranch.Branch.Id);
        }
        #endregion
        internal static Endorsement CreateCompanyEndorsement(ISSMODEL.Endorsement endorsement)
        {
            return new Endorsement(endorsement.Id, endorsement.PolicyId)
            {
                //Annotations = endorsement.Description,
                BeginDate = endorsement.BeginDate,
                //CapacityOfCode = endorsement.CapacityOfCode,
                CommitDate = endorsement.CommitDate,
                ConditionText = endorsement.Text.TextBody,
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo,
                DocumentNum = endorsement.Number,
                //EndoReasonCode = endorsement.EndorsementReasonId,
                EndorsementId = endorsement.Id,
                EndoTypeCode = Convert.ToInt32(endorsement.EndorsementType),
                ExchangeRate = endorsement.ExchangeRate,
                IsMassive = Convert.ToBoolean(endorsement.IsMassive),
                IssueDate = endorsement.IssueDate,
                PolicyId = endorsement.PolicyId,
                //PrintedDate = Convert.ToDateTime(endorsement.PrintedDate),
                //QuotationId = endorsement.QuotationId,
                //SubscriptionReqId = endorsement.SubscriptionReqId,
                UserId = endorsement.UserId
            };
        }

        internal static Core.Application.Issuance.Entities.Risk CreateCompanyRisks(ISSMODEL.RiskChangeText risk)
        {
            return new Core.Application.Issuance.Entities.Risk(risk.RiskId)
            {
                RatingZoneCode = risk.RatingZoneCode,
                AddressId = risk.AddressId,
                ConditionText = risk.ConditionText,
                CoveredRiskTypeCode = risk.CoveredRiskTypeCode,
                CoverGroupId = risk.CoverGroupId,
                InsuredId = risk.InsuredId,
                IsFacultative = risk.IsFacultative,
                NameNum = risk.NameNum,
                PhoneId = risk.PhoneId,
                RiskCommercialClassCode = risk.RiskCommercialClassCode,
                RiskCommercialTypeCode = risk.RiskCommercialTypeCode,
                RiskId = risk.RiskId,
                SecondaryInsuredId = risk.SecondaryInsuredId
            };
        }

        internal static Core.Application.Issuance.Entities.EndoChangeText CreateEndoChangeText(Models.EndoChangeText endoChangeText)
        {
            Core.Application.Issuance.Entities.EndoChangeText entityendoChangeText = new Core.Application.Issuance.Entities.EndoChangeText
            {
                EndorsementId = endoChangeText.endorsementId,
                PolicyId = endoChangeText.policiId,
                Reason = endoChangeText.reason,
                RiskId = endoChangeText.riskId,
                TextNewPolicy = endoChangeText.textNewPolicy,
                TextNewRisk = endoChangeText.textnewRisk,
                TextOldPolicy = endoChangeText.textOldPolicy,
                TextOldRisk = endoChangeText.textOldRisk,
                UserId = endoChangeText.userId
            };
            if (endoChangeText.Id > 0)
            {
                entityendoChangeText.EndoChangeTextCode = endoChangeText.Id;
            }
            return entityendoChangeText;
        }

        public static INTEN.IssCoEndoChangeTextControl CreateCoEndoChangeTextControl(int  ContractId, int PerifericoId)
        {
            return new INTEN.IssCoEndoChangeTextControl()
            {
                ContractId = ContractId,
                PerifericoId = PerifericoId
            };
        }
    }
}