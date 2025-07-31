

using Sistran.Company.Application.UnderwritingServices.EEProvider.DAO;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    public class QuotationBusiness
    {
        public List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            List<CompanyPolicy> companyPolicies = new QuotationDAO().GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
            return companyPolicies;
        }
    }
}
