using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static void CreateFacadeGeneral(CompanyPolicy companyPolicy, Rules.Facade facade)
        {
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
            facade.SetConcept(CompanyRuleConceptGeneral.BusinessTypeCode, companyPolicy.BusinessType == null ? 0 : (int)companyPolicy.BusinessType);
            facade.SetConcept(CompanyRuleConceptGeneral.OperationId, companyPolicy.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.PolicyTypeCode, companyPolicy.PolicyType.Id);
            facade.SetConcept(CompanyRuleConceptGeneral.RequestId, companyPolicy.Request == null ? (int?)null : companyPolicy.Request.Id == 0 ? (int?)null : companyPolicy.Request.Id);
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
            if (companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions != null && companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions.Count > 0 && companyPolicy.Agencies.First(p => p.IsPrincipal).Commissions.First() != null)
                facade.SetConcept(CompanyRuleConceptGeneral.StandardCommissionPercentage, companyPolicy.Agencies?.First(p => p.IsPrincipal)?.Commissions?.First()?.Percentage);
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
                facade.SetConcept(CompanyRuleConceptGeneral.GroupBranchId, companyPolicy.Branch.GroupBranchId);
            }

            if (companyPolicy.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in companyPolicy.DynamicProperties)
                {
                    facade.SetConcept(CompanyRuleConceptGeneral.DynamicConcept(dynamicConcept.Id, dynamicConcept.EntityId), dynamicConcept.Value);
                }
            }
        }


    }
}
