using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.AircraftChangeAgentService
{
    [ServiceContract]
    public interface ICiaAircraftChangeAgentService : ICiaChangeAgentEndorsement
    {
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyPolicy companyEndorsement, bool isMassive);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy);
    }
}
