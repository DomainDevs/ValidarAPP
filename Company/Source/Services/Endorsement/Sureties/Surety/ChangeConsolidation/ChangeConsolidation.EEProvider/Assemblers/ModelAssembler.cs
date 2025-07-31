using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        internal static CompanyChangeConsolidation CreateCompanyChangeConsolidation(CompanyPolicy companyPolicy)
        {
            return new CompanyChangeConsolidation
            {
                companyContract = companyPolicy.Summary.companyContract,
                Endorsement = companyPolicy.Endorsement,
                Text = companyPolicy.Text,
                CurrentFrom = companyPolicy.CurrentFrom,
                CurrentTo = companyPolicy.CurrentTo,
                Id = companyPolicy.Id,
                UserId = companyPolicy.UserId,
                InfringementPolicies = companyPolicy.InfringementPolicies
            };
        }
    }
}
