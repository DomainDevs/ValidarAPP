using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        internal static CompanyChangePolicyHolder CreateCompanyChangePolicyHolder(CompanyPolicy companyPolicy)
        {
            return new CompanyChangePolicyHolder
            {
                InfringementPolicies = companyPolicy.InfringementPolicies,
                Endorsement = companyPolicy.Endorsement,
                Text = companyPolicy.Text,
                CurrentFrom = companyPolicy.CurrentFrom,
                CurrentTo = companyPolicy.CurrentTo,
                Id = companyPolicy.Id,
                UserId = companyPolicy.UserId,
                holder = companyPolicy.Holder
            };
        }
    }
}
