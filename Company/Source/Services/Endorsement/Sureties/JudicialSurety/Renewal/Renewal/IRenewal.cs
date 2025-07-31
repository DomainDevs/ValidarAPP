using Sistran.Company.Application.UnderwritingServices.Models;

using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretyRenewalService
{
    [ServiceContract]
    public interface IJudicialSuretyRenewalService
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);

    }
}
