using AutoMapper;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.TextEndorsement.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.TextEndorsement.EEProvider.DAOs
{
    public class CiaTextDAO
    {

        /// <summary>
        /// Creates the cia texts.
        /// </summary>
        /// <param name="endorsement">The endorsement.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public CompanyPolicy CreateCiaTexts(CompanyEndorsement endorsement)
        {
            try
            {
                if (endorsement == null || string.IsNullOrEmpty(endorsement.Text.TextBody))
                {
                    throw new ArgumentException(Errors.ErrorDataEmpty);
                }
                var policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsement.Id);
                if (policy != null)
                { 
                    policy.IssueDate = endorsement.IssueDate;
                    policy.UserId = endorsement.UserId;
                    var immaper = Sistran.Company.TextEndorsement.EEProvider.Assemblers.ModelAssembler.CreateMapCompanyClause();
                    policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionPolicyId(policy?.Endorsement?.PolicyId??0)); 
                    policy.CurrentFrom = endorsement.CurrentFrom;
                    policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                    policy.Text = null;
                    policy.Text = new CompanyText
                    {
                        TextBody = endorsement.Text.TextBody                        
                    };
                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    policy.Endorsement.Text = null;
                    policy.Endorsement.Text = new CompanyText
                    {
                        Observations = endorsement.Text.Observations
                    };
                    policy.TemporalType = TemporalType.Endorsement;
                    policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                    policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    policy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    if (endorsement.BusinessTypeDescription != 0)
                    {
                        policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = endorsement.BusinessTypeDescription.ToString());
                    }
                    if (endorsement.TemporalId > 0)
                    {
                        policy.Id = endorsement.TemporalId;
                    }
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    policy.Endorsement.TicketNumber = endorsement.TicketNumber;
                    policy.Endorsement.TicketDate = endorsement.TicketDate;
                    //
                    return policy;
                }
                else
                {
                    throw new Exception(Errors.PolicyNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
