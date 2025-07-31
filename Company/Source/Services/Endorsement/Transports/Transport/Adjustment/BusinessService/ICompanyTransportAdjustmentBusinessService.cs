using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices
{
    [ServiceContract]
    public interface ICompanyTransportAdjustmentBusinessService 
    {
        [OperationContract]
        CompanyPolicy CreateEndorsementAdjustment(CompanyPolicy companyPolicy, Dictionary<string, object> formValues);

        [OperationContract]
        List<CompanyCoverage> GetCoverageByEndorsementIdPolicyIdriskId(int policyId, int riskId);
    }
}