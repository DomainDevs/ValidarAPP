using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretyChangePolicyHolderService
{
    [ServiceContract]
    public interface ICiaJudicialSuretyChangePolicyHolderService
    {
        [OperationContract]
        CompanyChangePolicyHolder CreateTemporal(CompanyChangePolicyHolder companyChangePolicyHolder, bool isMassive = false);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangePolicyHolder(CompanyChangePolicyHolder companyChangePolicyHolder, bool clearPolicies = false);
    }
}
