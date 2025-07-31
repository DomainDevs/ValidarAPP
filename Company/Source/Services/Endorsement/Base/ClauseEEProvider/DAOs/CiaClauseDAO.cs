using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.CancellationEndorsement.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.ClauseEndorsement.EEProvider.DAOs
{
    public class CiaClauseDAO
    {
        public static object obj = new object();
        public CompanyPolicy CreateCiaClause(CompanyPolicy companyPolicy)
        {
            var UserId= BusinessContext.Current.UserId;
            var policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
            if (policy != null)
            {
                policy.Clauses = null;
                policy.UserId = UserId;
                List<Clause> clauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory == true).ToList();
                bool allMandatory = true;
                var result = clauses.Where(x => !companyPolicy.Clauses.Select(z => z.Id).Contains(x.Id)).ToList();
                if (result != null && result.Count() != 0)
                {
                    allMandatory = false;
                }

                if (allMandatory)
                {
                    policy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                    policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                    policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;

                    if (companyPolicy.Endorsement.BusinessTypeDescription != 0)
                    {
                        policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyPolicy.Endorsement.BusinessTypeDescription.ToString());
                    }
                    policy.IssueDate = companyPolicy.IssueDate;
                    policy.TemporalType = TemporalType.Endorsement;
                    policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                    Assemblers.ModelAssembler.CreateMapClause();
                    policy.Clauses = companyPolicy.Clauses;
                    policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    policy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    if (companyPolicy.Endorsement.TemporalId > 0)
                    {
                        policy.Id = companyPolicy.Endorsement.TemporalId;
                    }
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    policy.IssueDate = DelegateService.commonService.GetDate();
                    return policy;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception(Errors.PolicyNotFound);
            }

        }

    }
}
