using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.AircraftEndorsementRenewalService
{
    [ServiceContract]
    public interface ICiaAircraftRenewalService
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);
    }
}
