using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.ReversionEndorsement.EEProvider.Assemblers
{
    public class EntityAssembler
    {
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
                facade.SetConcept(RuleConceptGeneral.HolderIdentificationDocument, policy.Holder.IdentificationDocument.Number);

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
                    facade.SetConcept(RuleConceptGeneral.DynamicConcept(dynamicConcept.Id), dynamicConcept.Value);
                }
            }
        }
    }
}