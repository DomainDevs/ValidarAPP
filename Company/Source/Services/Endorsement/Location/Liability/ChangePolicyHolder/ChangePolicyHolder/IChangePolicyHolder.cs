using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.LiabilityChangePolicyHolderService
{
    [ServiceContract]
    public interface ICiaLiabilityChangePolicyHolderService
    {
        [OperationContract]
        CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolder, bool isMassive = false);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder, bool clearPolicies = false);
    }
}
