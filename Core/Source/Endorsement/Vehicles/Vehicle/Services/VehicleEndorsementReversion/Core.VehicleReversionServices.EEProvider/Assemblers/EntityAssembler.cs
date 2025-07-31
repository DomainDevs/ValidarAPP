using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine.Facades;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.VehicleEndorsementReversionService3GProvider.Assemblers
{
    public class EntityAssembler
    {

        public static FacadeGeneral CreateFacadeGeneral(Policy policy)
        {
            FacadeGeneral facadeGeneral = new FacadeGeneral();

            facadeGeneral.TempId = policy.Endorsement.TemporalId;
            facadeGeneral.QuotationId = policy.Endorsement.QuotationId;
            facadeGeneral.DocumentNumber = policy.DocumentNumber == 0 ? (decimal?)null : policy.DocumentNumber;
            facadeGeneral.PrefixCode = policy.Prefix.Id;
            facadeGeneral.EndorsementTypeCode = (int)policy.Endorsement.EndorsementType;
            facadeGeneral.CurrencyCode = policy.ExchangeRate.Currency.Id;
            facadeGeneral.UserId = policy.UserId;
            facadeGeneral.ExchangeRate = policy.ExchangeRate.SellAmount;
            facadeGeneral.IssueDate = policy.IssueDate;
            facadeGeneral.CurrentFrom = policy.CurrentFrom;
            facadeGeneral.CurrentTo = policy.CurrentTo;
            facadeGeneral.BeginDate = DateTime.Now;
            facadeGeneral.CommitDate = DateTime.Now;
            facadeGeneral.BillingDate = DateTime.Now;
            facadeGeneral.ProductId = policy.Product.Id;
            facadeGeneral.PolicyId = policy.Endorsement.PolicyId == 0 ? (int?)null : policy.Endorsement.PolicyId;
            facadeGeneral.EndorsementId = policy.Endorsement.Id == 0 ? (int?)null : policy.Endorsement.Id;
            facadeGeneral.TemporalTypeCode = (int)policy.TemporalType;
            facadeGeneral.ConditionText = policy.Text == null ? string.Empty : policy.Text.TextBody;
            facadeGeneral.BusinessTypeCode = policy.BusinessType == null ? 0 : (int)policy.BusinessType;
            facadeGeneral.OperationId = policy.Id;
            facadeGeneral.PolicyTypeCode = policy.PolicyType == null ? 0 : policy.PolicyType.Id;
            facadeGeneral.RequestId = policy.Request == null ? (int?)null : policy.Request.Id == 0 ? (int?)null : policy.Request.Id;
            facadeGeneral.EffectPeriod = policy.EffectPeriod;
            facadeGeneral.IsRequest = policy.Request == null ? false : policy.Request.Id == 0 ? false : true;
            facadeGeneral.PrimaryAgentId = policy.Agencies.Find(x => x.IsPrincipal).Agent.IndividualId;
            facadeGeneral.PrimaryAgentAgencyId = policy.Agencies.Find(x => x.IsPrincipal).Id;
            facadeGeneral.PrimaryAgentCode = policy.Agencies.Find(x => x.IsPrincipal).Code;
            facadeGeneral.IsPrimary = policy.Agencies.Find(x => x.IsPrincipal).IsPrincipal;
            facadeGeneral.ScriptId = policy.Product.ScriptId;
            facadeGeneral.PreRuleSetId = policy.Product.PreRuleSetId;
            facadeGeneral.RuleSetId = policy.Product.RuleSetId;
            facadeGeneral.StandardCommissionPercentage = policy.Product.StandardCommissionPercentage;
            facadeGeneral.IsFlatRate = policy.Product.IsFlatRate;
            facadeGeneral.IsCollective = policy.Product.IsCollective;
            facadeGeneral.IsGreen = policy.Product.IsGreen;
            facadeGeneral.DaysVigency = policy.Endorsement != null ? policy.Endorsement.EndorsementDays : 0;


            if (policy.Holder != null)
            {
                facadeGeneral.PolicyHolderId = policy.Holder.IndividualId;
                facadeGeneral.CustomerTypeCode = (int)policy.Holder.CustomerType;
                if (policy.Holder.CompanyName != null)
                {
                    if (policy.Holder.CompanyName.Address != null)
                    {
                        facadeGeneral.MailAddressId = policy.Holder.CompanyName.Address.Id;
                    }
                }

                if (policy.Holder.IndividualType == IndividualType.Person)
                {
                    int holderAge = 0;

                    if (policy.Holder.BirthDate.GetValueOrDefault() != DateTime.MinValue)
                    {
                        holderAge = (DateTime.Today - policy.Holder.BirthDate.Value).Days / 365;
                    }

                    facadeGeneral.HolderAge = holderAge;
                    facadeGeneral.HolderBirthDate = policy.Holder.BirthDate;
                    facadeGeneral.HolderGender = policy.Holder.Gender;
                }
            }

            if (policy.Branch != null)
            {
                facadeGeneral.BranchCode = policy.Branch.Id;

                if (policy.Branch.SalePoints != null && policy.Branch.SalePoints.Count > 0)
                {
                    facadeGeneral.SalePointCode = policy.Branch.SalePoints.First().Id;
                }
            }

            if (policy.DynamicProperties != null)
            {
                foreach (DynamicConcept dynamicConcept in policy.DynamicProperties)
                {
                    facadeGeneral.SetDynamicConcept(dynamicConcept.Id, dynamicConcept.Value);
                }
            }

            return facadeGeneral;
        }

    }
}
