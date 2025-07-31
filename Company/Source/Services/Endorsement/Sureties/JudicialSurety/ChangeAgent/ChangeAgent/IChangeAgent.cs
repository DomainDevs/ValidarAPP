using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretyChangeAgentService
{
    [ServiceContract]
    public interface IJudicialSuretyChangeAgentServiceCompany : ICiaChangeAgentEndorsement
    {
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyPolicy companyEndorsement, bool isMassive);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy, bool clearPolicies = false);
    }
}
