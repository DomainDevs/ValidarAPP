using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.SuretyChangeConsolidationService
{
    [ServiceContract]
    public interface ICiaSuretyChangeConsolidationService
    {
        [OperationContract]
        CompanyChangeConsolidation CreateTemporal(CompanyChangeConsolidation companyChangeConsolidation, bool isMassive = false);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangeConsolidation(CompanyChangeConsolidation companyChangeConsolidation, bool clearPolicies = false);

        [OperationContract]
        CompanyContract GetCompanyContractByTemporalId(int temporalId, bool isMasive);
    }
}
