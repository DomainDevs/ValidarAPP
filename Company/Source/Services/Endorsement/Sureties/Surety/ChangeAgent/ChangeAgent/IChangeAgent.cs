using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyChangeAgentService
{
    [ServiceContract]
    public interface ICiaSuretyChangeAgentService : ICiaChangeAgentEndorsement
    {
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyPolicy companyEndorsement, bool isMassive);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy, bool clearPolicies = false);
    }
}
