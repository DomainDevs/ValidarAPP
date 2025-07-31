using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.LiabilityChangeCoinsuranceService
{
    [ServiceContract]
    public interface ILiabilityChangeCoinsuranceServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false);

        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangeCoinsurance(CompanyEndorsement companyPolicy, bool clearPolicies = false);
    }
}
