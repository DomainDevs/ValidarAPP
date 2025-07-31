using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.TransportEndorsementRenewalService
{
    [ServiceContract]
    public interface ICiaTransportRenewalService
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);
    }
}
