using Sistran.Company.Application.QuotationServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.QuotationServices.EEProvider.Business
{
    public class QuoteBusiness
    {
        private Sistran.Core.Application.QuotationServices.EEProvider.QuotationServiceEEProviderCore coreProvider;

        public QuoteBusiness()
        {
            coreProvider = new Core.Application.QuotationServices.EEProvider.QuotationServiceEEProviderCore();
        }

        public List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            List<Policy> corePolicies = coreProvider.GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
            List<CompanyPolicy> companyPolicies = corePolicies.Select(ModelAssembler.CreateCompanyPolicy).ToList();
            return companyPolicies;
        }

    }
}
